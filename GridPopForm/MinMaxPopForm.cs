using DevExpress.XtraEditors;

namespace ExperimentDesign.GridPopForm
{
    public partial class MinMaxPopForm : XtraForm
    {
        public MinMaxPopForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
