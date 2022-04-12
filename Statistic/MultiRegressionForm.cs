using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using ExperimentDesign.General;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.Statistic
{
    public partial class MultiRegressionForm : XtraForm
    {
        public MultiRegressionForm()
        {
            InitializeComponent();
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = false;
            of.Filter = "实验分析表|*.xlsx";
            if (of.ShowDialog() == DialogResult.OK)
            {
                this.buttonEdit1.Text = of.FileName;
            }
            else
            {
                this.buttonEdit1.Text = string.Empty;
            }
        }

        private void buttonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (File.Exists(this.buttonEdit1.Text))
            {
                DataTable table = ExcelEx.ExcelToTable(this.buttonEdit1.Text);
                var regression = UniMultipleLineRegression.Regression(table);
                if (regression != null)
                {
                    var param = regression.GetParam();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("y = ");
                    sb.Append(param[0].Coefficient.ToString("f2"));
                    for (int i = 1; i < param.Count; i++)
                    {
                        if (param[i].Coefficient < 0)
                        {
                            sb.Append($" - {param[i].Coefficient.ToString("f2").Substring(1)} * x{i}");
                        }
                        else
                        {
                            sb.Append($" + {param[i].Coefficient.ToString("f2")} * x{i}");
                        }
                    }
                    this.buttonEdit2.Text = sb.ToString();
                    SetGrid(regression.GetTable());
                    SetParam(param);
                    ShowPareto(regression.GetBars());
                    this.gridControl1.Refresh();
                }
                else
                {
                    this.buttonEdit2.Text = string.Empty;
                    XtraMessageBox.Show(@"回归分析失败,数据表格式错误");
                }
            }
            else
            {
                XtraMessageBox.Show(@"所选‘实验方案模拟结果:’文件不存在,请选择正确的文件");
            }
        }

        public void SetGrid(DataTable table)
        {
            this.gridControl1.DataSource = table;
            this.gridControl1.RefreshDataSource();
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                this.gridView1.Columns[i].DisplayFormat.FormatString = "{0:0.######}";
            }
            this.gridView1.RefreshData();
        }

        public void ShowPareto(List<BarData> datas)
        {
            this.chartControl1.Series.Clear();
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
        }

        public void SetParam(List<RegressionParam> param)
        {
            this.gridControl2.DataSource = param;
            this.gridControl2.RefreshDataSource();
            for (int i = 0; i < this.gridView2.Columns.Count; i++)
            {
                this.gridView2.Columns[i].DisplayFormat.FormatType = FormatType.Numeric;
                this.gridView2.Columns[i].DisplayFormat.FormatString = "{0:0.######}";
            }
            this.gridView2.RefreshData();
        }

        private void buttonEdit3_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var pars = this.gridControl2.DataSource as List<RegressionParam>;
            if (pars?.Count > 0)
            {
                Random R = new Random();
                List<double> volumns = new List<double>();
                int i = 0;
                var max = Convert.ToDouble(this.buttonEdit3.Text);
                while (i < max)
                {
                    double y = 0;
                    foreach (var par in pars)
                    {
                        if (!string.IsNullOrEmpty(par.Name) && par.Max > par.Min)
                        {
                            var x = par.Min + (par.Max - par.Min) * R.NextDouble();
                            y += par.Coefficient * x;
                        }
                        else
                        {
                            y += par.Coefficient;
                        }

                    }
                    volumns.Add(y);
                    i++;
                }

                this.chartControl2.Series.Clear();
                Series Series1 = new Series();
                Series1.ToolTipEnabled = DefaultBoolean.False;
                Series1.View = new SplineSeriesView();
                for (int index = 1; index < 100; index++)
                {
                    var vol = Statistics.EmpiricalInvCDF(volumns, (100 - index) / 100.0);
                    SeriesPoint seriesPoint = new SeriesPoint(vol, index);
                    Series1.Points.Add(seriesPoint);
                }
                Series Series2 = new Series();
                {
                    Series2.LabelsVisibility = DefaultBoolean.True;
                    Series2.ToolTipEnabled = DefaultBoolean.False;
                    Series2.View = new PointSeriesView();
                    var vol10 = Statistics.EmpiricalInvCDF(volumns, 0.9);
                    SeriesPoint point10 = new SeriesPoint(vol10, 10);
                    var vol50 = Statistics.EmpiricalInvCDF(volumns, 0.5);
                    SeriesPoint point50 = new SeriesPoint(vol50, 50);
                    var vol90 = Statistics.EmpiricalInvCDF(volumns, 0.1);
                    SeriesPoint point90 = new SeriesPoint(vol90, 90);
                    Series2.Points.Add(point10);
                    Series2.Points.Add(point50);
                    Series2.Points.Add(point90);

                    PointSeriesLabel myLable1 = (PointSeriesLabel)Series2.Label;
                    myLable1.Angle = 0;//获取或设置控制数据点标签位置的角度
                    myLable1.TextPattern = "P{V}-储量:{A:F2}";//获取或设置一个字符串，该字符串表示指定要在系列标注标签中显示的文本的模式。
                    myLable1.Position = PointLabelPosition.Outside;//获取或设置点标记所在的位置。
                    myLable1.ResolveOverlappingMode = ResolveOverlappingMode.Default;//启用系列标签的自动冲突检测和解决
                }
                this.chartControl2.SeriesSerializable = new Series[] {
        Series1,Series2};
                ((XYDiagram)chartControl2.Diagram).AxisY.Title.Text = "储量概率(%)";
                ((XYDiagram)chartControl2.Diagram).AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                ((XYDiagram)chartControl2.Diagram).AxisX.Title.Text = "地质储量";
                ((XYDiagram)chartControl2.Diagram).AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            }
        }
    }
}
