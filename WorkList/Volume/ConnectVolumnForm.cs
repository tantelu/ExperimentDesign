using DevExpress.XtraEditors;
using ExperimentDesign.General;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Volume
{
    public partial class ConnectVolumnForm : XtraForm
    {
        public ConnectVolumnForm()
        {
            InitializeComponent();
        }

        public ConnectVolumnPar GetConnectVolumnPar()
        {
            ConnectVolumnPar par = new ConnectVolumnPar();
            par.VolType = combo_conType.SelectedIndex;
            par.SetIndex(wellids.Text);
            par.SetFacies(facies.Text);
            return par;
        }

        public void SetVolumnCalPar(ConnectVolumnPar par)
        {
            if (par != null)
            {
                combo_conType.SelectedIndex = par.VolType;
                facies.Text = par.GetFacies();
                wellids.Text = par.GetIndexs();
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
