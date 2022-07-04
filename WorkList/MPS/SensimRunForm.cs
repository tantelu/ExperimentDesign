using DevExpress.XtraEditors;
using System;

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
            return null;
        }

        public void InitForm(SnesimPar sgs)
        {
            
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void simpleButton2_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

    }
}
