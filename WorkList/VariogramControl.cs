using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList
{
    public partial class VariogramControl : UserControl
    {
        public VariogramControl()
        {
            InitializeComponent();
        }

        public Variogram GetVariogram()
        {
            Variogram var = new Variogram();
            var.Sill = sill.Text;
            var.Nug = nug.Text;
            var.VarType = comboBoxEdit1.SelectedText;
            var.MajorRange = majordir.Text;
            var.MinorRange = minordir.Text;
            var.VerRange = verticaldir.Text;
            var.MajorAzi = majorAzimuth.Text;
            var.MajorDip = majorDip.Text;
            return var;
        }

        public void SetVariogram(Variogram variogram)
        {
            if (variogram != null)
            {
                sill.Text = variogram.Sill.ToString();
                nug.Text = variogram.Nug.ToString();
                comboBoxEdit1.SelectedText = variogram.VarType.ToString();
                majordir.Text = variogram.MajorRange.ToString();
                minordir.Text = variogram.MinorRange.ToString();
                verticaldir.Text = variogram.VerRange.ToString();
                majorAzimuth.Text = variogram.MajorAzi.ToString();
                majorDip.Text = variogram.MajorDip.ToString();
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
                var.VarType = "球状模型";
                var.MajorRange = 500;
                var.MinorRange = 500;
                var.VerRange = 100;
                var.MajorAzi = 0;
                var.MajorDip = 0;
                return var;
            }
        }

        [Description("基台值")]
        public object Sill { get; set; }

        [Description("块金值")]
        public object Nug { get; set; }

        [Description("变差函数类型")]
        public object VarType { get; set; }

        [Description("主变程")]
        public object MajorRange { get; set; }

        [Description("次变程")]
        public object MinorRange { get; set; }

        [Description("垂直变程")]
        public object VerRange { get; set; }

        [Description("变差函数主方向")]
        public object MajorAzi { get; set; }

        [Description("变差函数倾角")]
        public object MajorDip { get; set; }

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
            writer.WriteValue(Sill);
            writer.WritePropertyName(nameof(Nug));
            writer.WriteValue(Nug);
            writer.WritePropertyName(nameof(VarType));
            writer.WriteValue(VarType);
            writer.WritePropertyName(nameof(MajorRange));
            writer.WriteValue(MajorRange);
            writer.WritePropertyName(nameof(MinorRange));
            writer.WriteValue(MinorRange);
            writer.WritePropertyName(nameof(VerRange));
            writer.WriteValue(VerRange);
            writer.WritePropertyName(nameof(MajorAzi));
            writer.WriteValue(MajorAzi);
            writer.WritePropertyName(nameof(MajorDip));
            writer.WriteValue(MajorDip);
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public void Open(string json)
        {
            JObject jo = JObject.Parse(json);
            Sill = jo[nameof(Sill)];
            Nug = jo[nameof(Nug)];
            VarType = jo[nameof(VarType)];
            MajorRange = jo[nameof(MajorRange)];
            MinorRange = jo[nameof(MinorRange)];
            VerRange = jo[nameof(VerRange)];
            MajorAzi = jo[nameof(MajorAzi)];
            MajorDip = jo[nameof(MajorDip)];
        }
    }
}
