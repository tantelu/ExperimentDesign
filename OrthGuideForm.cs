using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign
{
    public partial class OrthGuideForm : Form
    {
        public OrthGuideForm()
        {
            InitializeComponent();
        }

        public DataTable GetOrthTable()
        {
            var fatorAndLevelTable = this.gridControl1.DataSource as DataTable;
            DataTable table = new DataTable();
            table.Columns.Add($"实验\\因数", Type.GetType("System.String"));
            for (int col = 0; col < FactorNum; col++)
            {
                var name = fatorAndLevelTable.Rows[0][col + 1].ToString();
                table.Columns.Add(name, Type.GetType("System.String"));
            }
            for (int i = 0; i < Times; i++)
            {
                DataRow row = table.NewRow();
                row["实验\\因数"] = $"实验{i + 1}";
                table.Rows.Add(row);
            }
            var file = Path.Combine(Application.StartupPath, "OrthTable.txt");
            StreamReader read = new StreamReader(file);
            while (!read.EndOfStream)
            {
                var test = read.ReadLine();
                if (test.Contains("n="))
                {
                    var splits = test.Split(new string[] { "n=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (splits.Length > 1)
                    {
                        int n = 0;
                        if (int.TryParse(splits[1], out n) && n == Times)
                        {
                            if (splits[0].Contains("^"))
                            {
                                var splits2 = splits[0].Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
                                if (splits2.Length > 1)
                                {
                                    int infactor = 0;
                                    int inlevel = 0;
                                    if (int.TryParse(splits2[1].Trim(), out infactor) && FactorNum == infactor &&
                                        int.TryParse(splits2[0].Trim(), out inlevel) && inlevel == LevelNum)
                                    {
                                        for (int i = 0; i < Times; i++)
                                        {
                                            var info = read.ReadLine().Trim();
                                            for (int col = 0; col < info.Length; col++)
                                            {
                                                int level = Convert.ToInt32(info[col].ToString());
                                                var s = fatorAndLevelTable.Rows[level + 1][col + 1].ToString();
                                                table.Rows[i][col + 1] = s;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return table;
        }

        public int FactorNum { get { return Convert.ToInt32(spinEdit_factor.Value); } }

        public int LevelNum { get { return Convert.ToInt32(spinEdit_level.Value); } }

        public int Times { get { return LevelNum * FactorNum - FactorNum + 1; } }

        private void RefreshOrthGrid()
        {
            var table = CreateFatorAndLevelTable();
            this.gridControl1.DataSource = table;
            this.gridControl1.MainView.PopulateColumns();
            this.gridControl1.RefreshDataSource();
            this.gridView1.Columns[0].OptionsColumn.AllowEdit = false;
        }

        private void RefreshLText(object sender, EventArgs e)
        {
            var times = Times;
            textBox_L.Text = $"正交表：L{times}_{LevelNum}_{FactorNum} " +
                $"表示{LevelNum}水平{FactorNum}因数表，需做{times}次实验";
            spinEdit_level.Properties.MaxValue = FactorNum - 1;
        }

        private DataTable CreateFatorAndLevelTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add($"序号", Type.GetType("System.String"));
            for (int col = 0; col < FactorNum; col++)
            {
                table.Columns.Add((col + 1).ToString(), Type.GetType("System.String"));
            }
            DataRow factorRow = table.NewRow();
            factorRow["序号"] = $"因数名称";
            for (int col = 0; col < FactorNum; col++)
            {
                factorRow[(col + 1).ToString()] = $"因数{col + 1}";
            }
            table.Rows.Add(factorRow);
            for (int i = 0; i < LevelNum; i++)
            {
                DataRow row = table.NewRow();
                row["序号"] = $"水平{i + 1}";
                //for (int col = 0; col < FactorNum; col++)
                //{
                //    row[factors[col]] = 0.0;
                //}
                table.Rows.Add(row);
            }
            return table;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (string.Equals(this.btn_ok.Text, "确定"))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.tabPane1.SelectedPageIndex++;
            }
        }

        private void btn_cacel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void tabPane1_SelectedPageChanged(object sender, DevExpress.XtraBars.Navigation.SelectedPageChangedEventArgs e)
        {
            if (this.tabPane1.SelectedPageIndex < 2)
            {
                this.gridControl1.DataSource = null;
                this.gridControl1.RefreshDataSource();
                this.gridControl1.Refresh();
                this.btn_ok.Text = "下一步";
            }
            else
            {
                this.btn_ok.Text = "确定";
                RefreshOrthGrid();
            }
        }
    }
}
