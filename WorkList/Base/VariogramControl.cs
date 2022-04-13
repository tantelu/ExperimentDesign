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
                var.VarType = (VariogramType)comboBoxEdit1.SelectedIndex;
                var.Sill = Design<double>.GeneralDesign(sill.Tag, sill.Text);
                var.Nug = Design<double>.GeneralDesign(nug.Tag, nug.Text);
                var.MajorRange = Design<double>.GeneralDesign(majordir.Tag, majordir.Text);
                var.MinorRange = Design<double>.GeneralDesign(minordir.Tag, minordir.Text);
                var.VerRange = Design<double>.GeneralDesign(verticaldir.Tag, verticaldir.Text);
                var.MajorAzi = Design<double>.GeneralDesign(majorAzimuth.Tag, majorAzimuth.Text);
                var.MajorDip = Design<double>.GeneralDesign(majorDip.Tag, majorDip.Text);
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
                sill.Tag = var.Sill;
                sill.Text = var.Sill.IsDesign ? var.Sill.DesignName : var.Sill.Value.ToString();
                nug.Tag = var.Nug;
                nug.Text = var.Nug.IsDesign ? var.Nug.DesignName : var.Nug.Value.ToString();

                comboBoxEdit1.SelectedIndex = (int)var.VarType;

                majordir.Tag = var.MajorRange;
                majordir.Text = var.MajorRange.IsDesign? var.MajorRange.DesignName: var.MajorRange.Value.ToString();

                minordir.Tag = var.MinorRange;
                minordir.Text = var.MinorRange.IsDesign? var.MinorRange.DesignName: var.MinorRange.Value.ToString();

                verticaldir.Tag = var.VerRange;
                verticaldir.Text = var.VerRange.IsDesign? var.VerRange.DesignName:var.VerRange.Value.ToString();

                majorAzimuth.Tag = var.MajorAzi;
                majorAzimuth.Text = var.MajorAzi.IsDesign ? var.MajorAzi.DesignName:var.MajorAzi.Value.ToString();

                majorDip.Tag = var.MajorDip;
                majorDip.Text = var.MajorDip.IsDesign ? var.MajorDip.DesignName:var.MajorDip.Value.ToString();
            }
        }
    }

    public class Variogram
    {
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

        //克隆值 但不克隆ID
        public Variogram Clone()
        {
            Variogram newvar = new Variogram();
            newvar.MajorAzi = this.MajorAzi.Clone();
            newvar.MajorDip = this.MajorDip.Clone();
            newvar.MajorRange = this.MajorRange.Clone();
            newvar.MinorRange = this.MinorRange.Clone();
            newvar.VerRange = this.VerRange.Clone();
            newvar.Nug = this.Nug.Clone();
            newvar.Sill = this.Sill.Clone();
            newvar.VarType = this.VarType;
            return newvar;
        }
    }
}
