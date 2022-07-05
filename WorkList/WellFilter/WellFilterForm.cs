using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.WellFilter
{
    public partial class WellFilterForm : Form
    {
        public WellFilterForm()
        {
            InitializeComponent();
        }

        public List<SparseWellData> SparseMethod(int index)
        {
            var data = new List<SparseWellData>();
            if (index == 0)
            {
                List<int> ids = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                ids = ids.OrderBy(_ => Guid.NewGuid()).ToList();
                for (int i = 0; i < 10; i++)
                {
                    data.Add(new SparseWellData() { Serial = i + 1, Id = WellIds.Build(ids[i]) });
                }
            }
            else
            {
                //其他方案待实现
            }
            return data;
        }

        private void buttonEdit_welldata_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = false;
            if (of.ShowDialog() == DialogResult.OK)
            {
                buttonEdit_welldata.Text = of.FileName;
                this.gridControl1.DataSource = SparseMethod(0);
            }
            else
            {
                buttonEdit_welldata.Text = string.Empty;
                this.gridControl1.DataSource = null;
            }
            this.gridControl1.RefreshDataSource();
        }
    }
}
