using System;
using System.Data;
using System.Windows.Forms;

namespace ExperimentDesign
{
    public partial class PBForm : Form
    {
        private int[,] table12 = {
            { 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 0 },
            { 0 ,1, 1, 0, 1, 1, 1, 0, 0, 0, 1},
            { 1, 0,1, 1, 0, 1, 1, 1, 0, 0, 0  },
            { 0, 1, 0,1, 1, 0, 1, 1, 1, 0, 0  },
            { 0, 0, 1, 0,1, 1, 0, 1, 1, 1, 0  },
            { 0, 0, 0, 1, 0 ,1, 1, 0, 1, 1, 1 },
            { 1, 0, 0, 0, 1, 0,1, 1, 0, 1, 1 },
            { 1, 1, 0, 0, 0, 1, 0,1, 1, 0, 1  },
            { 1, 1, 1, 0, 0, 0, 1, 0,1, 1, 0  },
            { 0,1, 1, 1, 0, 0, 0, 1, 0,1, 1, },
            { 1,0,1, 1, 1, 0, 0, 0, 1, 0,1  },
            { -1, -1,-1,-1,-1, -1, -1, -1, -1, -1, -1},
        };

        private int[,] table20 = {
            { 1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0,1,0,0,0,0,1,1,0 },
            { 0,1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0,1,0,0,0,0,1,1},
            { 1,0,1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0,1,0,0,0,0,1 },
            {  1,1,0,1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0,1,0,0,0,0   },
            { 0,1,1,0,1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0,1,0,0,0  },
            { 0,0,1,1,0,1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0,1,0,0 },
            {0,0,0,1,1,0,1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0,1,0 },
            {0, 0,0,0,1,1,0,1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0,1  },
            {1,0, 0,0,0,1,1,0,1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0  },
            { 0,1,0, 0,0,0,1,1,0,1, 1, 0, 0, 1, 1, 0, 1, 0, 1 },
            {  1,0,1,0, 0,0,0,1,1,0,1, 1, 0, 0, 1, 1, 0, 1, 0 },
            {  0 ,1,0,1,0, 0,0,0,1,1,0,1, 1, 0, 0, 1, 1, 0, 1},
            {  1,0 ,1,0,1,0, 0,0,0,1,1,0,1, 1, 0, 0, 1, 1, 0},
            {  0,1,0 ,1,0,1,0, 0,0,0,1,1,0,1, 1, 0, 0, 1, 1},
            {  1,0,1,0 ,1,0,1,0, 0,0,0,1,1,0,1, 1, 0, 0, 1},
            {  1,1,0,1,0 ,1,0,1,0, 0,0,0,1,1,0,1, 1, 0, 0},
            { 0, 1,1,0,1,0 ,1,0,1,0, 0,0,0,1,1,0,1, 1, 0},
            { 0,0, 1,1,0,1,0 ,1,0,1,0, 0,0,0,1,1,0,1, 1},
            {1,0,0, 1,1,0,1,0 ,1,0,1,0, 0,0,0,1,1,0,1},
            {-1,-1,-1,-1, -1,-1,-1,-1,-1 ,-1,-1,-1,-1, -1,-1,-1,-1,-1,-1},
        };

        public PBForm()
        {
            InitializeComponent();
        }

        public DataTable GetPBTable()
        {
            var pbGrid = this.gridControl1.DataSource as DataTable;
            DataTable table = new DataTable();
            table.Columns.Add($"实验\\因数", Type.GetType("System.String"));
            for (int col = 0; col < FactorNum; col++)
            {
                var name = pbGrid.Rows[0][col + 1].ToString();
                table.Columns.Add(name, Type.GetType("System.String"));
            }
            int[,] used = Times == 12 ? table12 : table20;
            for (int i = 0; i < Times; i++)
            {
                DataRow row = table.NewRow();
                row["实验\\因数"] = $"实验{i + 1}";
                for (int col = 0; col < FactorNum; col++)
                {
                    int index = used[i, col] == 1 ? 1 : 2;
                    row[col + 1] = pbGrid.Rows[index][col + 1];
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public int FactorNum { get { return Convert.ToInt32(comboBoxEdit_factor.Text); } }

        public int Times { get { return Convert.ToInt32(textEdit_times.Text); } }

        private void RefreshPBGrid()
        {
            var table = CreatePBTable();
            this.gridControl1.DataSource = table;
            this.gridControl1.MainView.PopulateColumns();
            this.gridControl1.RefreshDataSource();
            this.gridView1.Columns[0].OptionsColumn.AllowEdit = false;
        }

        private void RefreshLText(object sender, EventArgs e)
        {
            textEdit_times.Text = (FactorNum >= 12 ? 20 : 12).ToString();
        }

        private DataTable CreatePBTable()
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
                RefreshPBGrid();
            }
        }
    }
}
