using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using ExperimentDesign.General;
using System.Collections.Generic;

namespace ExperimentDesign.InfoForm
{
    public partial class ParetoShowForm : XtraForm
    {
        public ParetoShowForm()
        {
            InitializeComponent();
        }

        public void ShowPareto(List<BarData> datas)
        {
            Series Series1 = new Series();
            Series1.ArgumentScaleType = ScaleType.Qualitative;
            Series1.ArgumentDataMember = nameof(BarData.FactorName);
            Series1.DataSource = datas;
            Series1.ValueDataMembers.AddRange(new string[] { nameof(BarData.FactorLevel) });
            chartControl1.Series.Add(Series1);
            ((XYDiagram)chartControl1.Diagram).Rotated = true;
            ((XYDiagram)chartControl1.Diagram).AxisY.Title.Text = "标准化回归系数";
            ((XYDiagram)chartControl1.Diagram).AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            chartControl1.Titles[0].Text = "标准化效应的Pareto图";
            this.Show();
        }
    }
}
