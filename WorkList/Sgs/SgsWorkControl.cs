using DevExpress.XtraEditors;
using ExperimentDesign.WorkList.Base;
using ExperimentDesign.WorkList.Grid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Sgs
{
    public class SgsWorkControl : WorkControl
    {
        private Process process;

        private SgsPar par;

        public SgsWorkControl()
        {

        }

        protected override string WorkName => "序贯高斯模拟";

        protected override Bitmap Picture => global::ExperimentDesign.Properties.Resources.Sgs;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            string gridfile = Path.Combine(Main.GetWorkPath(), $"{index}", $"{nameof(Grid3D)}.json");
            if (!File.Exists(gridfile))
            {
                XtraMessageBox.Show("未找到工区网格定义文件,无法执行序贯高斯模拟");
                return;
            }
            if (par == null)
            {
                XtraMessageBox.Show($"未设置序贯高斯模拟参数");
                return;
            }
            Grid3D grid = new Grid3D();
            grid.Open(gridfile);
            string file = Path.Combine(Main.GetWorkPath(), $"{index}", $"sgsim.par");
            par.Save(file, grid, designVaribles);
            string exe = Path.Combine(Main.GetWorkPath(), $"{index}", @"sgsim.exe");
            string _out = Path.Combine(Main.GetWorkPath(), $"{index}", @"sgs.out");
            if (File.Exists(par.DataFileName))
            {
                File.Copy(par.DataFileName, Path.Combine(Main.GetWorkPath(), $"{index}", Path.GetFileName(par.DataFileName)), true);
            }
            File.Copy(Path.Combine(Application.StartupPath, "geostatspy", "sgsim.exe"), exe, true);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = exe;
            info.WorkingDirectory = Path.Combine(Main.GetWorkPath(), $"{index}");
            info.UseShellExecute = false;
            info.Arguments = "sgsim.par";
            process = Process.Start(info);
        }

        public override bool GetRunState(int index)
        {
            if (process != null && process.HasExited)
            {
                process.Dispose();
                process.Close();
                process = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void ShowParamForm()
        {
            using (SgsRunForm form = new SgsRunForm())
            {
                form.InitForm(par);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    par = form.GetSgsPar();
                    var newparam = VariableData.ObjectToVariables(par);
                    UpdateText(newparam);
                }
            }
        }

        public override string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(par));
            writer.WriteValue(par?.Save());
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public override void Open(string str)
        {
            JObject jobj = JObject.Parse(str);
            var parstr = jobj[nameof(par)]?.ToString();
            if (!string.IsNullOrEmpty(parstr))
            {
                par = new SgsPar();
                par.Open(parstr);
            }
            var newparam = VariableData.ObjectToVariables(par);
            UpdateText(newparam);
        }

        public override IReadOnlyList<VariableData> GetUncentainParam()
        {
            return VariableData.ObjectToVariables(par);
        }
    }
}
