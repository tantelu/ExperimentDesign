using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using ExperimentDesign.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    SetParam(param.Where(_ => !string.IsNullOrEmpty(_.Name)).ToList());
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
                this.gridView1.Columns[i].DisplayFormat.FormatString = "{0:0.#######}";
            }
            this.gridView1.RefreshData();
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

        public void SetParam(List<RegressionParam> param)
        {
            this.gridControl2.DataSource = param;
            this.gridControl2.RefreshDataSource();
            for (int i = 0; i < this.gridView2.Columns.Count; i++)
            {
                this.gridView2.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                this.gridView2.Columns[i].DisplayFormat.FormatString = "{0:0.#######}";
            }
            this.gridView2.RefreshData();
        }
    }
}
