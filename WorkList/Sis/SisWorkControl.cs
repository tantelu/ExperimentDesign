﻿using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Sis
{
    public class SisWorkControl : WorkControl
    {
        public SisWorkControl()
        {

        }

        protected override string WorkName => "序贯指示模拟";

        protected override Bitmap Picture => global::ExperimentDesign.Properties.Resources.Sgs;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            string gridfile = Path.Combine(Main.GetWorkPath(), $"{index}", $"{nameof(Grid3D)}.json");
            if (!File.Exists(gridfile))
            {
                XtraMessageBox.Show($"未找到工区网格定义文件,无法执行{WorkName}");
                return;
            }
            Grid3D grid = new Grid3D();
            grid.Open(gridfile);
            SisPar sgsPar = new SisPar();
            var propeties = sgsPar.GetType().GetProperties().Where(_ => _.GetCustomAttribute<DescriptionAttribute>() != null).ToList();
            foreach (var property in propeties)
            {
                var par = this.param.FirstOrDefault(_ => string.Equals(property.GetCustomAttribute<DescriptionAttribute>().Description, _.ParDescription));
                if (par != null)
                {
                    object _value = null;
                    if (par.Name.Contains("$"))
                    {
                        if (!designVaribles.TryGetValue(par.Name, out _value))
                        {
                            XtraMessageBox.Show($"在设计表中不存在'{par.Name}',请检查错误。");
                            return;
                        }
                    }
                    else
                    {
                        _value = par.BaseValue;
                    }
                    property.SetValue(sgsPar, Convert.ChangeType(_value, property.PropertyType));
                }
                else
                {
                    XtraMessageBox.Show($"在‘{WorkName}’工作流中未找到参数‘{property.GetCustomAttribute<DescriptionAttribute>().Description}’,请检查错误");
                    return;
                }
            }
            string file = Path.Combine(Main.GetWorkPath(), $"{index}", $"sisim.par");
            sgsPar.Save(file);
            string exe = Path.Combine(Main.GetWorkPath(), $"{index}", @"sisim.exe");
            string _out = Path.Combine(Main.GetWorkPath(), $"{index}", @"sis.out");
            File.Copy(Path.Combine(Application.StartupPath, "geostatspy", "sisim.exe"), exe, true);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = exe;
            info.WorkingDirectory = Path.Combine(Main.GetWorkPath(), $"{index}");
            info.UseShellExecute = false;
            info.Arguments = "sisim.par";
            var process = Process.Start(info);
        }

        public override bool GetRunState(int index)
        {
            string _out = Path.Combine(Main.GetWorkPath(), $"{index}", @"sis.out");
            return File.Exists(_out);
        }

        protected override void ShowParamForm()
        {
            using (SigRunForm form = new SigRunForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var par = form.GetSisPar();
                    var newparam = VariableData.ToVariables(this.param, par);
                    this.param.Clear();
                    this.param.AddRange(newparam);
                }
            }
        }
    }
}