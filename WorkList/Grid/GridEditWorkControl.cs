using DevExpress.XtraEditors;
using ExperimentDesign.General;
using ExperimentDesign.Uncertainty;
using ExperimentDesign.WorkList.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Grid
{
    public class GridEditWorkControl : WorkControl
    {
        List<VariableData> param = new List<VariableData>();

        public GridEditWorkControl()
        {

        }

        protected override string WorkName => "创建简单三维网格";

        protected override Bitmap Picture => Properties.Resources.Grid;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            string path = GetWorkPath(index);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Grid3D grid = new Grid3D();
            var propeties = grid.GetType().GetProperties().Where(_ => _.GetCustomAttribute<DescriptionAttribute>() != null).ToList();
            foreach (var property in propeties)
            {
                var par = this.param.FirstOrDefault(_ => string.Equals(property.GetCustomAttribute<DescriptionAttribute>().Description, _.ParDescription));
                if (par != null)
                {
                    object _value = null;
                    if (par.Name.ToString().Contains("$"))
                    {
                        if (!designVaribles.TryGetValue(par.Name.ToString(), out _value))
                        {
                            XtraMessageBox.Show($"在设计表中不存在'{par.Name.ToString()}',请检查错误。");
                            return;
                        }
                    }
                    else
                    {
                        _value = par.Name;
                    }
                    property.SetValue(grid,Convert.ChangeType(_value, property.PropertyType));
                }
                else
                {
                    XtraMessageBox.Show($"在‘{WorkName}’工作流中未找到参数‘{property.GetCustomAttribute<DescriptionAttribute>().Description}’,请检查错误");
                    return;
                }
            }
            string file = Path.Combine(path, $"{nameof(Grid3D)}.json");
            grid.Save(file);
        }

        public override bool GetRunState(int index)
        {
            string file = Path.Combine(GetWorkPath(index), $"{nameof(Grid3D)}.json");
            return File.Exists(file);
        }

        public override string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartArray();
            foreach (var item in param)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Data");
                writer.WriteValue(item.Save());
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public override void Open(string str)
        {
            param.Clear();
            if (!string.IsNullOrEmpty(str))
            {
                JArray ja = JArray.Parse(str);
                for (int i = 0; i < ja.Count; i++)
                {
                    JObject jo = ja[i] as JObject;
                    VariableData data = new VariableData();
                    data.Open(jo["Data"]?.ToString());
                    param.Add(data);
                }
            }
            UpdateText(param);
        }

        protected override void ShowParamForm()
        {
            using (GridEditForm form = new GridEditForm())
            {
                form.InitForm(param);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.param.Clear();
                    this.param.AddRange(form.GetUncentainParam());
                }
            }
        }

        public override IReadOnlyList<VariableData> GetUncentainParam()
        {
            return this.param;
        }
    }
}
