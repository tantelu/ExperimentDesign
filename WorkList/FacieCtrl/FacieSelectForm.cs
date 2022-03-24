using System.Windows.Forms;

namespace ExperimentDesign.WorkList.FacieCtrl
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

        public PropertyModelMethod Method
        {
            get
            {
                return (PropertyModelMethod)this.comboBoxEdit1.SelectedIndex;
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }

    public enum PropertyModelMethod
    {
        Sgs,
        DirectAssign,
    }
}
