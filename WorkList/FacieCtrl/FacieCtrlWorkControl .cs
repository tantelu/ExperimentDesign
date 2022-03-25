using DevExpress.XtraEditors;
using ExperimentDesign.General;
using ExperimentDesign.WorkList.Base;
using ExperimentDesign.WorkList.Grid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.FacieCtrl
{
    public class FacieCtrlWorkControl : WorkControl
    {
        private Dictionary<string, IFacieCtrlPar> pars = new Dictionary<string, IFacieCtrlPar>();

        private string OutFileName { get; set; } = "porosity.out";

        public FacieCtrlWorkControl()
        {

        }

        protected override string WorkName => "相控模拟";

        protected override Bitmap Picture => Properties.Resources.FacieCtrl;

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
                    Grid3D grid = new Grid3D();
                    grid.Open(gridfile);
                    var par = item.Value;
                    var workpath = Path.Combine(Main.GetWorkPath(), $"{index}", item.Key);
                    if (!Directory.Exists(workpath))
                    {
                        Directory.CreateDirectory(workpath);
                    }
                    var sgs = par.FacieCtrlRun(grid, workpath, designVaribles);
                    sgses.Add(item.Key, sgs);
                    if (Directory.Exists(workpath))
                    {
                        Directory.Delete(workpath,true);
                    }
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
                    Gslib.WriteGislib(Path.Combine(Main.GetWorkPath(), $"{index}", OutFileName), res, xcount, ycount, zcount);
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
            return File.Exists(Path.Combine(Main.GetWorkPath(), $"{index}", OutFileName));
        }

        protected override void ShowParamForm()
        {
            using (FacieCtrlRunForm form = new FacieCtrlRunForm())
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
            writer.WriteStartArray();
            if (pars.Count > 0)
            {
                foreach (var item in pars)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("Facie");
                    writer.WriteValue(item.Key);
                    writer.WritePropertyName("Type");
                    writer.WriteValue(item.Value.GetType().FullName);
                    writer.WritePropertyName("Data");
                    writer.WriteValue(item.Value.Save());
                    writer.WriteEndObject();
                }
            }
            writer.WriteEndArray();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public override void Open(string str)
        {
            JArray ja = JArray.Parse(str);
            if (ja?.Count > 0)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                for (int i = 0; i < ja.Count; i++)
                {
                    JObject jo = ja[i] as JObject;
                    if (jo != null)
                    {
                        var key = jo["Facie"]?.ToString();
                        var vauleType = jo["Type"]?.ToString();
                        var data = jo["Data"]?.ToString();
                        IFacieCtrlPar obj = assembly.CreateInstance(vauleType, false, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null, null, null) as IFacieCtrlPar;
                        obj?.Open(data);
                        pars.Add(key, obj);
                    }
                }
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
