using ExperimentDesign.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Base
{
    public partial class VariogramControl : UserControl
    {
        public VariogramControl()
        {
            InitializeComponent();
        }

        public Variogram GetVariogram()
        {
            try
            {
                Variogram var = new Variogram();
                if (sill.Text.Contains("$"))
                {
                    var.Sill = new Design<double>(Convert.ToDouble(sill.Tag), sill.Text);
                }
                else
                {
                    var.Sill = new Design<double>(Convert.ToDouble(sill.Text));
                }
                if (nug.Text.Contains("$"))
                {
                    var.Nug = new Design<double>(Convert.ToDouble(nug.Tag), nug.Text);
                }
                else
                {
                    var.Nug = new Design<double>(Convert.ToDouble(nug.Text));
                }
                var.VarType = (VariogramType)comboBoxEdit1.SelectedIndex;
                if (majordir.Text.Contains("$"))
                {
                    var.MajorRange = new Design<double>(Convert.ToDouble(majordir.Tag), majordir.Text);
                }
                else
                {
                    var.MajorRange = new Design<double>(Convert.ToDouble(majordir.Text));
                }
                if (minordir.Text.Contains("$"))
                {
                    var.MinorRange = new Design<double>(Convert.ToDouble(minordir.Tag), minordir.Text);
                }
                else
                {
                    var.MinorRange = new Design<double>(Convert.ToDouble(minordir.Text));
                }
                if (verticaldir.Text.Contains("$"))
                {
                    var.VerRange = new Design<double>(Convert.ToDouble(verticaldir.Tag), verticaldir.Text);
                }
                else
                {
                    var.VerRange = new Design<double>(Convert.ToDouble(verticaldir.Text));
                }
                if (majorAzimuth.Text.Contains("$"))
                {
                    var.MajorAzi = new Design<double>(Convert.ToDouble(majorAzimuth.Tag), majorAzimuth.Text);
                }
                else
                {
                    var.MajorAzi = new Design<double>(Convert.ToDouble(majorAzimuth.Text));
                }
                if (majorDip.Text.Contains("$"))
                {
                    var.MajorDip = new Design<double>(Convert.ToDouble(majorDip.Tag), majorDip.Text);
                }
                else
                {
                    var.MajorDip = new Design<double>(Convert.ToDouble(majorDip.Text));
                }
                return var;
            }
            catch
            {
                return null;
            }
        }

        public void SetVariogram(Variogram var)
        {
            if (var != null)
            {
                if (var.Sill.IsDesign)
                {
                    sill.Text = var.Sill.DesignName;
                    sill.Tag = var.Sill.Value;
                }
                else
                {
                    sill.Text = var.Sill.Value.ToString();
                    sill.Tag = var.Sill.Value;
                }
                if (var.Nug.IsDesign)
                {
                    nug.Text = var.Nug.DesignName;
                    nug.Tag = var.Nug.Value;
                }
                else
                {
                    nug.Text = var.Nug.Value.ToString();
                    nug.Tag = var.Nug.Value;
                }
                comboBoxEdit1.SelectedIndex = (int)var.VarType;
                if (var.MajorRange.IsDesign)
                {
                    majordir.Text = var.MajorRange.DesignName;
                    majordir.Tag = var.MajorRange.Value;
                }
                else
                {
                    majordir.Text = var.MajorRange.Value.ToString();
                    majordir.Tag = var.MajorRange.Value;
                }

                if (var.MinorRange.IsDesign)
                {
                    minordir.Text = var.MinorRange.DesignName;
                    minordir.Tag = var.MinorRange.Value;
                }
                else
                {
                    minordir.Text = var.MinorRange.Value.ToString();
                    minordir.Tag = var.MinorRange.Value;
                }

                if (var.VerRange.IsDesign)
                {
                    verticaldir.Text = var.VerRange.DesignName;
                    verticaldir.Tag = var.VerRange.Value;
                }
                else
                {
                    verticaldir.Text = var.VerRange.Value.ToString();
                    verticaldir.Tag = var.VerRange.Value;
                }

                if (var.MajorAzi.IsDesign)
                {
                    majorAzimuth.Text = var.MajorAzi.DesignName;
                    majorAzimuth.Tag = var.MajorAzi.Value;
                }
                else
                {
                    majorAzimuth.Text = var.MajorAzi.Value.ToString();
                    majorAzimuth.Tag = var.MajorAzi.Value;
                }

                if (var.MajorDip.IsDesign)
                {
                    majorDip.Text = var.MajorDip.DesignName;
                    majorDip.Tag = var.MajorDip.Value;
                }
                else
                {
                    majorDip.Text = var.MajorDip.Value.ToString();
                    majorDip.Tag = var.MajorDip.Value;
                }
            }
        }
    }

    public class Variogram
    {
        public static Variogram Default
        {
            get
            {
                Variogram var = new Variogram();
                var.Sill = 1.0;
                var.Nug = 0.0001;
                var.VarType = VariogramType.Spherical;
                var.MajorRange = 500;
                var.MinorRange = 500;
                var.VerRange = 100;
                var.MajorAzi = 0;
                var.MajorDip = 0;
                return var;
            }
        }

        [Description("基台值")]
        public Design<double> Sill { get; set; }

        [Description("块金值")]
        public Design<double> Nug { get; set; }

        [Description("变差函数类型")]
        public VariogramType VarType { get; set; }

        [Description("主变程")]
        public Design<double> MajorRange { get; set; }

        [Description("次变程")]
        public Design<double> MinorRange { get; set; }

        [Description("垂直变程")]
        public Design<double> VerRange { get; set; }

        [Description("变差函数主方向")]
        public Design<double> MajorAzi { get; set; }

        [Description("变差函数倾角")]
        public Design<double> MajorDip { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int vartype = string.Equals(VarType, "指数模型") ? 1 : string.Equals(VarType, "球状模型") ? 2 : 0;
            sb.AppendLine($"{1.0}   {Nug}   -nst, nugget effect");//变差函数结果套合数量、块金值
            sb.AppendLine($"{vartype}  {Sill}   {MajorAzi}  {MajorDip}   {0.0}   - it,cc,ang1,ang2,ang3");//变差类型、基台值、主方向、倾角、倾覆角（Petrel里面没有此参数默认为0）
            sb.AppendLine($"{MajorRange}  " + $"{MinorRange}  " + $"{VerRange} - a_hmax, a_hmin, a_vert");//主方向变程、次方向变程、垂直方向变程
            return sb.ToString();
        }

        public string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(Sill));
            writer.WriteValue(Sill.Save());
            writer.WritePropertyName(nameof(Nug));
            writer.WriteValue(Nug.Save());
            writer.WritePropertyName(nameof(VarType));
            writer.WriteValue((int)VarType);
            writer.WritePropertyName(nameof(MajorRange));
            writer.WriteValue(MajorRange.Save());
            writer.WritePropertyName(nameof(MinorRange));
            writer.WriteValue(MinorRange.Save());
            writer.WritePropertyName(nameof(VerRange));
            writer.WriteValue(VerRange.Save());
            writer.WritePropertyName(nameof(MajorAzi));
            writer.WriteValue(MajorAzi.Save());
            writer.WritePropertyName(nameof(MajorDip));
            writer.WriteValue(MajorDip.Save());
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public void Open(string json)
        {
            JObject jo = JObject.Parse(json);
            Sill = new Design<double>();
            Sill.Open(jo[nameof(Sill)]?.ToString());
            Nug = new Design<double>();
            Nug.Open(jo[nameof(Nug)]?.ToString());
            VarType = (VariogramType)(int)jo[nameof(VarType)];
            MajorRange = new Design<double>();
            MajorRange.Open(jo[nameof(MajorRange)]?.ToString());
            MinorRange = new Design<double>();
            MinorRange.Open(jo[nameof(MinorRange)]?.ToString());
            VerRange = new Design<double>();
            VerRange.Open(jo[nameof(VerRange)]?.ToString());
            MajorAzi = new Design<double>();
            MajorAzi.Open(jo[nameof(MajorAzi)]?.ToString());
            MajorDip = new Design<double>();
            MajorDip.Open(jo[nameof(MajorDip)]?.ToString());
        }
    }
}
