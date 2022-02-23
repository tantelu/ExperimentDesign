using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ExperimentDesign
{
    public partial class BBForm : Form
    {
        public BBForm()
        {
            InitializeComponent();
        }

        public DataTable GetBBTable()
        {
            var fatorAndLevelTable = this.gridControl1.DataSource as DataTable;
            DataTable table = new DataTable();
            table.Columns.Add($"实验\\因数", Type.GetType("System.String"));
            for (int col = 0; col < FactorNum; col++)
            {
                var name = fatorAndLevelTable.Rows[0][col + 1].ToString();
                table.Columns.Add(name, Type.GetType("System.String"));
            }
            table.Columns.Add("储量", Type.GetType("System.String"));
            var file = Path.Combine(Application.StartupPath, "BBTable.txt");
            StreamReader read = new StreamReader(file);
            while (!read.EndOfStream)
            {
                var test = read.ReadLine();
                if (test.Contains("n="))
                {
                    var splits = test.Split(new string[] { "n=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (splits.Length > 1)
                    {
                        int num = 0;
                        if (int.TryParse(splits[0].Trim(), out num) && num == FactorNum)
                        {
                            int n = 0;
                            if (int.TryParse(splits[1].Trim(), out n) && n > 0)
                            {
                                for (int i = 0; i < n + CenterNum; i++)
                                {
                                    DataRow row = table.NewRow();
                                    row["实验\\因数"] = $"实验{i + 1}";
                                    table.Rows.Add(row);
                                }
                                List<int[]> res = new List<int[]>();
                                for (int i = 0; i < n; i++)
                                {
                                    var oneRow = read.ReadLine();
                                    var indexs = oneRow.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                    int[] s = new int[indexs.Length];
                                    for (int index = 0; index < indexs.Length; index++)
                                    {
                                        s[index] = Convert.ToInt32(indexs[index]);
                                    }
                                    res.Add(s);
                                }
                                for (int i = 0; i < CenterNum; i++)
                                {
                                    res.Add(new int[res[0].Length]);
                                }
                                Random R = new Random();
                                for (int i = 0; i < res.Count; i++)
                                {
                                    int x = R.Next(0, res.Count);
                                    int y = 0;
                                    do
                                    {
                                        y = R.Next(0, res.Count);
                                    } while (y == x);
                                    var t = res[x];
                                    res[x] = res[y];
                                    res[y] = t;
                                }

                                for (int i = 0; i < n + CenterNum; i++)
                                {
                                    for (int col = 0; col < res[i].Length; col++)
                                    {
                                        int level = res[i][col] == 1 ? 1 : res[i][col] == -1 ? 3 : 2;
                                        var s = fatorAndLevelTable.Rows[level][col + 1].ToString();
                                        table.Rows[i][col + 1] = s;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return table;
        }

        public int FactorNum { get { return Convert.ToInt32(comboBoxEdit_factor.Text); } }

        public int CenterNum { get { return Convert.ToInt32(spinEdit_centers.Value); } }

        private void RefreshBBGrid()
        {
            var table = CreateBBTable();
            this.gridControl1.DataSource = table;
            this.gridControl1.MainView.PopulateColumns();
            this.gridControl1.RefreshDataSource();
            this.gridView1.Columns[0].OptionsColumn.AllowEdit = false;
        }

        private DataTable CreateBBTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add($"序号", Type.GetType("System.String"));
            for (int col = 0; col < FactorNum; col++)
            {
                table.Columns.Add((col + 1).ToString(), Type.GetType("System.String"));
            }
            DataRow nameRow = table.NewRow();
            nameRow["序号"] = $"因数名称";
            for (int col = 0; col < FactorNum; col++)
            {
                nameRow[(col + 1).ToString()] = $"因数{col + 1}";
            }
            table.Rows.Add(nameRow);
            DataRow highRow = table.NewRow();
            highRow["序号"] = $"高水平";
            for (int col = 0; col < FactorNum; col++)
            {
                highRow[(col + 1).ToString()] = 1;
            }
            table.Rows.Add(highRow);

            DataRow centerRow = table.NewRow();
            centerRow["序号"] = $"中心点";
            for (int col = 0; col < FactorNum; col++)
            {
                centerRow[(col + 1).ToString()] = 0;
            }
            table.Rows.Add(centerRow);

            DataRow lowRow = table.NewRow();
            lowRow["序号"] = $"低水平";
            for (int col = 0; col < FactorNum; col++)
            {
                lowRow[(col + 1).ToString()] = -1;
            }
            table.Rows.Add(lowRow);
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
                RefreshBBGrid();
            }
        }

        private void comboBoxEdit_factor_SelectedValueChanged(object sender, EventArgs e)
        {
            this.spinEdit_centers.Properties.MaxValue = FactorNum - 1;
        }
    }
}
