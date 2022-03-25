using DevExpress.XtraEditors;
using ExperimentDesign.General;
using ExperimentDesign.WorkList.Base;
using ExperimentDesign.WorkList.Grid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Volume
{
    public class VolumeCalWorkControl : WorkControl
    {
        public VolumnCalPar VolumnCalPar { get; set; }

        protected override string WorkName => "储量计算";

        protected override Bitmap Picture => Properties.Resources.Volume;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            float[] volumns;
            if (VolumnCalPar == null)
            {
                return;
            }

            string gridfile = Path.Combine(Main.GetWorkPath(), $"{index}", $"{nameof(Grid3D)}.json");
            if (!File.Exists(gridfile))
            {
                XtraMessageBox.Show("未找到工区网格定义文件");
                return;
            }
            Grid3D grid = new Grid3D();
            grid.Open(gridfile);

            var porosityFileName = Path.Combine(Main.GetWorkPath(), $"{index}", VolumnCalPar.PorosityFileName);
            var porosity = VolumnCalPar.Porosity.Value;
            var penetration = VolumnCalPar.Penetration.Value;
            if (VolumnCalPar.Porosity.IsDesign)
            {
                if (designVaribles != null && designVaribles.ContainsKey(VolumnCalPar.Porosity.ToString()))
                {
                    porosity = Convert.ToDouble(designVaribles[VolumnCalPar.Porosity.ToString()]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
            if (VolumnCalPar.Penetration.IsDesign)
            {
                if (designVaribles != null && designVaribles.ContainsKey(VolumnCalPar.Penetration.ToString()))
                {
                    penetration = Convert.ToDouble(designVaribles[VolumnCalPar.Penetration.ToString()]);
                }
                else
                {
                    return;
                }
            }
            if (File.Exists(porosityFileName))
            {
                int xcount;
                int ycount;
                int zcount;
                var porosities = Gslib.ReadGislib(porosityFileName, out xcount, out ycount, out zcount);
                volumns = new float[xcount * ycount * zcount];
                for (int i = 0; i < porosities.Length; i++)
                {
                    volumns[i] = porosities[i] > porosity ? porosities[i] : 0;
                }
            }
            else
            {
                return;
            }
            var penetrationFilename = Path.Combine(Main.GetWorkPath(), $"{index}", VolumnCalPar.PorosityFileName);
            if (File.Exists(penetrationFilename))
            {
                int xcount;
                int ycount;
                int zcount;
                var penetrations = Gslib.ReadGislib(penetrationFilename, out xcount, out ycount, out zcount);
                for (int i = 0; i < penetrations.Length; i++)
                {
                    volumns[i] = penetrations[i] > penetration ? volumns[i] : 0;
                }
            }
            if (volumns != null)
            {
                double all = 0;
                foreach (var vol in volumns)
                {
                    all += vol;
                }
                double percentage = (all / volumns.Length);
                all = all * (grid.Xmax - grid.Xmin) * (grid.Ymax - grid.Ymin) * (grid.Zmax - grid.Zmin);
                File.WriteAllText(Path.Combine(Main.GetWorkPath(), $"{index}", VolumnCalPar.VolumnOutFileName), $"总储量:{all.ToString()}  百分比:{percentage.ToString()}");
            }
        }

        public override bool GetRunState(int index)
        {
            return true;
        }

        public override string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(VolumnCalPar));
            writer.WriteValue(VolumnCalPar?.Save());
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public override void Open(string str)
        {
            JObject jobj = JObject.Parse(str);
            var parstr = jobj[nameof(VolumnCalPar)]?.ToString();
            if (!string.IsNullOrEmpty(parstr))
            {
                VolumnCalPar = new VolumnCalPar();
                VolumnCalPar.Open(parstr);
            }
            var newparam = VariableData.ObjectToVariables(VolumnCalPar);
            UpdateText(newparam);
        }

        public override IReadOnlyList<VariableData> GetUncentainParam()
        {
            return VariableData.ObjectToVariables(VolumnCalPar);
        }

        protected override void ShowParamForm()
        {
            using (VolumeCalForm form = new VolumeCalForm())
            {
                form.SetVolumnCalPar(VolumnCalPar);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    VolumnCalPar = form.GetVolumnCalPar();
                    var newparam = VariableData.ObjectToVariables(VolumnCalPar);
                    UpdateText(newparam);
                }
            }
        }
    }

    public class VolumnCalPar
    {
        [Description("储量输出文件名")]
        public string VolumnOutFileName { get; set; }

        [Description("孔隙度文件名")]
        public string PorosityFileName { get; set; }

        [Description("渗透率文件名")]
        public string PenetrationFileName { get; set; }

        [Description("孔隙度下限")]
        public Design<double> Porosity { get; set; }

        [Description("渗透率下限")]
        public Design<double> Penetration { get; set; }

        public string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(VolumnOutFileName));
            writer.WriteValue(VolumnOutFileName);
            writer.WritePropertyName(nameof(PorosityFileName));
            writer.WriteValue(PorosityFileName);
            writer.WritePropertyName(nameof(PenetrationFileName));
            writer.WriteValue(PenetrationFileName);
            writer.WritePropertyName(nameof(Porosity));
            writer.WriteValue(Porosity.Save());
            writer.WritePropertyName(nameof(Penetration));
            writer.WriteValue(Penetration.Save());
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public void Open(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JObject jo = JObject.Parse(json);
                VolumnOutFileName = jo[nameof(VolumnOutFileName)]?.ToString();
                PorosityFileName = jo[nameof(PorosityFileName)]?.ToString();
                PenetrationFileName = jo[nameof(PenetrationFileName)]?.ToString();

                Porosity = new Design<double>();
                Porosity.Open(jo[nameof(Porosity)]?.ToString());

                Penetration = new Design<double>();
                Penetration.Open(jo[nameof(Penetration)]?.ToString());
            }
        }
    }
}
