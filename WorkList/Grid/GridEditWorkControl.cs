using DevExpress.XtraEditors;
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

        protected override Bitmap Picture => global::ExperimentDesign.Properties.Resources.Grid;

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

    public class Grid3D
    {
        [Description("网格X最小值")]
        public double Xmin { get; set; }

        [Description("网格X最大值")]
        public double Xmax { get; set; }

        [Description("网格Y最小值")]
        public double Ymin { get; set; }

        [Description("网格Y最大值")]
        public double Ymax { get; set; }

        [Description("网格Z最小值")]
        public double Zmin { get; set; }

        [Description("网格Z最大值")]
        public double Zmax { get; set; }

        [Description("X方向网格大小")]
        public double Xsize { get; set; }

        [Description("Y方向网格大小")]
        public double Ysize { get; set; }

        [Description("Z方向网格大小")]
        public double Zsize { get; set; }

        public void Open(string file)
        {
            var jsonText = File.ReadAllText(file, Encoding.UTF8);
            JObject jo = JObject.Parse(jsonText);
            Xmin = (double)jo[nameof(Xmin)];
            Xmax = (double)jo[nameof(Xmax)];
            Ymin = (double)jo[nameof(Ymin)];
            Ymax = (double)jo[nameof(Ymax)];
            Zmin = (double)jo[nameof(Zmin)];
            Zmax = (double)jo[nameof(Zmax)];
            Xsize = (double)jo[nameof(Xsize)];
            Ysize = (double)jo[nameof(Ysize)];
            Zsize = (double)jo[nameof(Zsize)];
        }

        //覆盖
        public void Save(string file)
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(Xmin));
            writer.WriteValue(Xmin);
            writer.WritePropertyName(nameof(Xmax));
            writer.WriteValue(Xmax);
            writer.WritePropertyName(nameof(Ymin));
            writer.WriteValue(Ymin);
            writer.WritePropertyName(nameof(Ymax));
            writer.WriteValue(Ymax);
            writer.WritePropertyName(nameof(Zmin));
            writer.WriteValue(Zmin);
            writer.WritePropertyName(nameof(Zmax));
            writer.WriteValue(Zmax);
            writer.WritePropertyName(nameof(Xsize));
            writer.WriteValue(Xsize);
            writer.WritePropertyName(nameof(Ysize));
            writer.WriteValue(Ysize);
            writer.WritePropertyName(nameof(Zsize));
            writer.WriteValue(Zsize);
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = sw.GetStringBuilder().ToString();
            File.WriteAllText(file, jsonText, Encoding.UTF8);
        }
    }
}
