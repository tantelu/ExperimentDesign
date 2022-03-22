using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Sgs
{
    public partial class FacieSelectForm : Form
    {
        public string NewFacie
        {
            get
            {
                return this.textEdit1.Text;
            }
            set
            {
                this.textEdit1.Text = value;
            }
        }

        public FacieSelectForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
