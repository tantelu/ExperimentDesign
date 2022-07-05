using DevExpress.Utils;
using DevExpress.XtraCharts;
using ExperimentDesign.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExperimentDesign.Statistic
{
    public partial class SparseValidAnalyzeForm : Form
    {
        public SparseValidAnalyzeForm()
        {
            InitializeComponent();
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog OF = new FolderBrowserDialog();
            if (OF.ShowDialog() == DialogResult.OK)
            {
                this.buttonEdit1.Text = OF.SelectedPath;
                var all = Directory.GetFiles(this.buttonEdit1.Text).Select(_ => Path.GetFileName(_)).ToList();
                comboBoxEdit_model.Properties.Items.Clear();
                comboBoxEdit_model.Properties.Items.AddRange(all);
                comboBoxEdit_well.Properties.Items.Clear();
                comboBoxEdit_well.Properties.Items.AddRange(all);
                if (all.Count > 1)
                {
                    comboBoxEdit_model.SelectedIndex = 1;
                    comboBoxEdit_well.SelectedIndex = 0;
                }
            }
            else
            {
                this.buttonEdit1.Text = string.Empty;
                comboBoxEdit_model.SelectedIndex = -1;
                comboBoxEdit_well.SelectedIndex = -1;
                comboBoxEdit_model.Properties.Items.Clear();
                comboBoxEdit_well.Properties.Items.Clear();
            }
        }

        private void ShowGridCompare()
        {
            var points = Gslib.ReadWellPoint(Path.Combine(this.buttonEdit1.Text, this.comboBoxEdit_well.Text));
            var res = SparseValidCompare.GridByGridComparison(points, Path.Combine(this.buttonEdit1.Text, this.comboBoxEdit_model.Text));

            this.chartControl_grid.Series.Clear();
            Series Series1 = new Series();
            Series1.ToolTipEnabled = DefaultBoolean.False;
            Series1.View = new LineSeriesView();
            for (int index = 0; index < res.Count; index++)
            {
                SeriesPoint seriesPoint = new SeriesPoint(index, res[index]);
                SeriesPoint seriesPoint2 = new SeriesPoint(index + 1, res[index]);
                Series1.Points.Add(seriesPoint);
                Series1.Points.Add(seriesPoint2);
            }
            chartControl_grid.Series.Add(Series1);
            ((XYDiagram)chartControl_grid.Diagram).AxisX.Title.Text = "单井网格索引";
            ((XYDiagram)chartControl_grid.Diagram).AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            ((XYDiagram)chartControl_grid.Diagram).AxisY.Title.Text = "1:相同";
            ((XYDiagram)chartControl_grid.Diagram).AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            chartControl_grid.Titles[0].Text = "逐网格对比";
        }

        private void FacieProportionCompare()
        {
            List<BarData> datas1 = new List<BarData>();
            List<BarData> datas2 = new List<BarData>();

            var points = Gslib.ReadWellPoint(Path.Combine(this.buttonEdit1.Text, this.comboBoxEdit_well.Text));
            var res = SparseValidCompare.FacieProportionComparison(points, Path.Combine(this.buttonEdit1.Text, this.comboBoxEdit_model.Text));
            for (int i = 0; i < res.Item1.Count; i++)
            {
                foreach (var item in res.Item1)
                {
                    datas1.Add(new BarData($"{item.Key}", item.Value));
                }
            }
            for (int i = 0; i < res.Item2.Count; i++)
            {
                foreach (var item in res.Item2)
                {
                    datas2.Add(new BarData($"{item.Key}", item.Value));
                }
            }
            this.chartControl_facieratio.Series.Clear();
            Series Series1 = new Series("井", ViewType.Bar);
            Series1.LegendText = "井";
            Series1.ArgumentScaleType = ScaleType.Qualitative;
            Series1.ArgumentDataMember = nameof(BarData.FactorName);
            Series1.DataSource = datas1;
            Series1.ValueDataMembers.AddRange(new string[] { nameof(BarData.FactorLevel) });

            Series Series2 = new Series("模型", ViewType.Bar);
            Series2.LegendText = "模型";
            Series2.ArgumentScaleType = ScaleType.Qualitative;
            Series2.ArgumentDataMember = nameof(BarData.FactorName);
            Series2.DataSource = datas2;
            Series2.ValueDataMembers.AddRange(new string[] { nameof(BarData.FactorLevel) });

            chartControl_facieratio.Series.AddRange(Series1, Series2);

            chartControl_facieratio.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.RightOutside; //获取或设置图表控件中的图例的水平对齐方式。
            chartControl_facieratio.Legend.AlignmentVertical = LegendAlignmentVertical.Top;//获取或设置图表控件中的图例的竖直对齐方式。
            chartControl_facieratio.Legend.Direction = LegendDirection.BottomToTop;//获取或设置在该图例中显示系列名称的方向。
            chartControl_facieratio.Legend.Visibility = DefaultBoolean.True;//是否在图表上显示图例

            ((XYDiagram)chartControl_facieratio.Diagram).AxisX.Title.Text = "相代码";
            ((XYDiagram)chartControl_facieratio.Diagram).AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            chartControl_facieratio.Titles[0].Text = "相比例";
        }

        private void ThickAndLayerCompare()
        {
            List<BarData> datas1 = new List<BarData>();
            List<BarData> datas2 = new List<BarData>();
            var points = Gslib.ReadWellPoint(Path.Combine(this.buttonEdit1.Text, this.comboBoxEdit_well.Text));
            var res = SparseValidCompare.ThickAndLayerComparison(points, Path.Combine(this.buttonEdit1.Text, this.comboBoxEdit_model.Text), 4, 1);
            for (int i = 0; i < res.Count; i++)
            {
                datas1.Add(new BarData((i + 1).ToString(), res[i].Item1));
                datas2.Add(new BarData((i + 1).ToString(), res[i].Item2));
            }
            this.chartControl_layerthick.Series.Clear();
            Series Series1 = new Series("井", ViewType.Bar);
            Series1.LegendText = "井";
            Series1.ArgumentScaleType = ScaleType.Qualitative;
            Series1.ArgumentDataMember = nameof(BarData.FactorName);
            Series1.DataSource = datas1;
            Series1.ValueDataMembers.AddRange(new string[] { nameof(BarData.FactorLevel) });

            Series Series2 = new Series("模型", ViewType.Bar);
            Series2.LegendText = "模型";
            Series2.ArgumentScaleType = ScaleType.Qualitative;
            Series2.ArgumentDataMember = nameof(BarData.FactorName);
            Series2.DataSource = datas2;
            Series2.ValueDataMembers.AddRange(new string[] { nameof(BarData.FactorLevel) });

            chartControl_layerthick.Series.AddRange(Series1, Series2);

            chartControl_layerthick.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.RightOutside; //获取或设置图表控件中的图例的水平对齐方式。
            chartControl_layerthick.Legend.AlignmentVertical = LegendAlignmentVertical.Top;//获取或设置图表控件中的图例的竖直对齐方式。
            chartControl_layerthick.Legend.Direction = LegendDirection.BottomToTop;//获取或设置在该图例中显示系列名称的方向。
            chartControl_layerthick.Legend.Visibility = DefaultBoolean.True;//是否在图表上显示图例

            ((XYDiagram)chartControl_layerthick.Diagram).AxisX.Title.Text = "层序号";
            ((XYDiagram)chartControl_layerthick.Diagram).AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            ((XYDiagram)chartControl_layerthick.Diagram).AxisY.Title.Text = "砂体厚度(1)";
            ((XYDiagram)chartControl_layerthick.Diagram).AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            chartControl_layerthick.Titles[0].Text = "分层厚度对比";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (comboBoxEdit1.SelectedIndex == 0)
            {
                ShowGridCompare();
            }
            else if (comboBoxEdit1.SelectedIndex == 1)
            {
                FacieProportionCompare();
            }
            else if (comboBoxEdit1.SelectedIndex == 2)
            {
                ThickAndLayerCompare();
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.xtraTabControl1.SelectedTabPageIndex = this.comboBoxEdit1.SelectedIndex;
        }
    }
}
