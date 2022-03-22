using DevExpress.XtraEditors;
using ExperimentDesign.General;
using ExperimentDesign.WorkList.Base;
using ExperimentDesign.WorkList.Grid;
using ExperimentDesign.WorkList.ShowResult;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Sgs
{
    public class FacieCtrlSgsWorkControl : WorkControl
    {
        private Dictionary<string, SgsPar> pars = new Dictionary<string, SgsPar>();

        public FacieCtrlSgsWorkControl()
        {

        }

        protected override string WorkName => "相控序贯高斯模拟";

        protected override Bitmap Picture => global::ExperimentDesign.Properties.Resources.Sgs;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            string gridfile = Path.Combine(Main.GetWorkPath(), $"{index}", $"{nameof(Grid3D)}.json");
            if (!File.Exists(gridfile))
            {
                XtraMessageBox.Show("未找到工区网格定义文件,无法执行序贯高斯模拟");
                return;
            }
            if (pars?.Count > 0)
            {
                var faciemodel = Path.Combine(Main.GetWorkPath(), $"{index}", "sis.out");
                if (!File.Exists(faciemodel))
                {
                    XtraMessageBox.Show($"不存在相模型");
                    return;
                }
                Dictionary<string, float[]> sgses = new Dictionary<string, float[]>();
                foreach (var item in pars)
                {
                    var par = item.Value;
                    var workpath = Path.Combine(Main.GetWorkPath(), $"{index}", item.Key);
                    if (!Directory.Exists(workpath))
                    {
                        Directory.CreateDirectory(workpath);
                    }
                    Grid3D grid = new Grid3D();
                    grid.Open(gridfile);
                    string file = Path.Combine(workpath, $"sgsim.par");
                    par.Save(file, grid, designVaribles);
                    string exe = Path.Combine(workpath, @"sgsim.exe");
                    string _out = Path.Combine(workpath, @"sgs.out");
                    File.Copy(Path.Combine(Application.StartupPath, "geostatspy", "sgsim.exe"), exe, true);
                    ProcessStartInfo info = new ProcessStartInfo();
                    info.FileName = exe;
                    info.WorkingDirectory = workpath;
                    info.UseShellExecute = false;
                    info.Arguments = "sgsim.par";
                    var process = Process.Start(info);
                    process.WaitForExit();
                    int xcount = 0;
                    int ycount = 0;
                    int zcount = 0;
                    var sgs = Gslib.ReadGislib(_out, out xcount, out ycount, out zcount);
                    //{
                    //    ColorBar colorbar = ColorBar.Default;
                    //    Bitmap map = new Bitmap(xcount, ycount);
                    //    var max = sgs.Max();
                    //    var min = sgs.Min();
                    //    colorbar.SetMinMax(min, max);
                    //    for (int i = 0; i < xcount; i++)
                    //    {
                    //        for (int j = 0; j < ycount; j++)
                    //        {
                    //            int index2 = i * xcount + j;
                    //            if (!float.IsNaN(sgs[index2]))
                    //            {
                    //                map.SetPixel(i, j, colorbar.GetColor(sgs[index2]));
                    //            }
                    //            else
                    //            {
                    //                map.SetPixel(i, j, Color.Black);
                    //            }
                    //        }
                    //    }
                    //    map.Save(Path.ChangeExtension(file, "png"));
                    //}
                    sgses.Add(item.Key, sgs);
                }
                //检查所有文件是否都已经存在  然后进行相替换操作
                {
                    int xcount = 0;
                    int ycount = 0;
                    int zcount = 0;
                    var gslib = Gslib.ReadGislib(faciemodel, out xcount, out ycount, out zcount);
                    var res = new float[gslib.Length];
                    foreach (var item in sgses)
                    {
                        var facie = Convert.ToInt32(item.Key);
                        for (int i = 0; i < gslib.Length; i++)
                        {
                            if (Math.Abs(gslib[i] - facie) < 0.0001)
                            {
                                res[i] = item.Value[i];
                            }
                        }
                    }
                    Gslib.WriteGislib(Path.Combine(Main.GetWorkPath(), $"{index}", "faciesgs.out"), res, xcount, ycount, zcount);
                }
            }
            else
            {
                XtraMessageBox.Show($"未设置序贯高斯模拟参数");
                return;
            }
        }

        public override bool GetRunState(int index)
        {
            return File.Exists(Path.Combine(Main.GetWorkPath(), $"{index}", "faciesgs.out"));
        }

        protected override void ShowParamForm()
        {
            using (FacieCtrlSgsRunForm form = new FacieCtrlSgsRunForm())
            {
                form.SetAllPars(pars);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    pars = form.GetAllPars();
                    UpdateText(GetUncentainParam());
                }
            }
        }

        public override string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            if (pars.Count > 0)
            {
                foreach (var item in pars)
                {
                    writer.WritePropertyName(item.Key);
                    writer.WriteValue(item.Value.Save());
                }
            }
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public override void Open(string str)
        {
            JObject jobj = JObject.Parse(str);
            var propertys = jobj.Properties();
            foreach (var property in propertys)
            {
                var par = new SgsPar();
                par.Open(jobj[property.Name]?.ToString());
                pars.Add(property.Name, par);
            }
            UpdateText(GetUncentainParam());
        }

        public override IReadOnlyList<VariableData> GetUncentainParam()
        {
            List<VariableData> datas = new List<VariableData>();
            foreach (var item in pars)
            {
                var newparam = VariableData.ObjectToVariables(item.Value);
                if (newparam?.Count > 0)
                {
                    datas.AddRange(newparam);
                }
            }
            return datas;
        }
    }
}
