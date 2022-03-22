using DevExpress.XtraEditors;
using System;

namespace ExperimentDesign.WorkList.Sgs
{
    public partial class SgsRunForm : XtraForm
    {
        public SgsRunForm()
        {
            InitializeComponent();
        }

        public SgsPar GetSgsPar()
        {
            return this.sgsUserControl1.GetSgsPar();
        }

        public void InitForm(SgsPar sgs)
        {
            this.sgsUserControl1.SetSgsPar(sgs);
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
