using DevExpress.XtraEditors;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WorkList.ExperimentDesign
{
    public partial class GridEditForm : XtraForm
    {
        public GridEditForm()
        {
            InitializeComponent();
        }

        public List<string> GetUncentainParam()
        {
            List<string> res = new List<string>();
            foreach (var item in this.layoutControl1.Controls)
            {
                if (item is TextEdit t)
                {
                    if (t.Text.Contains("$"))
                    {
                        res.Add(t.Text);
                    }
                }
            }
            return res;
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void simpleButton2_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
