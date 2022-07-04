using DevExpress.XtraEditors;
using System;
using System.Text;

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
            if (!string.IsNullOrEmpty(this.textEdit1.Text))
            {
                WellIds ids = new WellIds();
                try
                {
                    var strs = this.textEdit1.Text.Split(new string[] { ",", "，", ";", "；", " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length > 0)
                    {
                        foreach (var item in strs)
                        {
                            ids.ids.Add(Convert.ToInt32(item));
                        }
                    }
                }
                catch
                {
                    ids.ids.Clear();
                }
                par.IgnoreIds = ids;
            }
            return par;
        }

        public void InitForm(SnesimPar sgs)
        {
            if (sgs != null)
            {
                this.buttonEdit1.Text = sgs.TIFile;
                this.buttonEdit2.Text = sgs.ConditionFile;
                if (sgs.IgnoreIds?.ids?.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in sgs.IgnoreIds.ids)
                    {
                        sb.Append(item.ToString());
                        sb.Append(",");
                    }
                    this.textEdit1.Text = sb.ToString(0, sb.Length - 1);
                }
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
