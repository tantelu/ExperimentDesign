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
            this.Refresh();
        }
    }
}
