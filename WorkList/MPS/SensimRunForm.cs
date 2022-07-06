using DevExpress.XtraEditors;
using System;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.MPS
{
    public partial class SensimRunForm : XtraForm
    {
        public SensimRunForm()
        {
            InitializeComponent();
        }

        public SnesimPar GetSgsPar()
        {
            SnesimPar par = new SnesimPar();
            par.TIFile = this.buttonEdit1.Text;
            par.ConditionFile = this.buttonEdit2.Text;
            return par;
        }

        public void InitForm(SnesimPar sgs)
        {
            if (sgs != null)
            {
                this.buttonEdit1.Text = sgs.TIFile;
                this.buttonEdit2.Text = sgs.ConditionFile;
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog OF = new OpenFileDialog();
            OF.Multiselect = false;
            if (OF.ShowDialog() == DialogResult.OK)
            {
                this.buttonEdit1.Text = OF.FileName;
            }
            else
            {
                this.buttonEdit1.Text = string.Empty;
            }
        }

        private void buttonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog OF = new OpenFileDialog();
            OF.Multiselect = false;
            if (OF.ShowDialog() == DialogResult.OK)
            {
                this.buttonEdit2.Text = OF.FileName;
            }
            else
            {
                this.buttonEdit2.Text = string.Empty;
            }
        }
    }
}
