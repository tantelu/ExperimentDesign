using DevExpress.XtraEditors;
using System.Data;

namespace ExperimentDesign.InfoForm
{
    public partial class DesignValueShowForm : XtraForm
    {
        public DesignValueShowForm()
        {
            InitializeComponent();
        }

        public void SetGrid(DataTable table)
        {
            this.gridControl1.DataSource = table;
            this.gridControl1.RefreshDataSource();
            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                this.gridView1.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                this.gridView1.Columns[i].DisplayFormat.FormatString = "n3";
            }
            this.gridView1.RefreshData();
            this.Refresh();
        }
    }
}
