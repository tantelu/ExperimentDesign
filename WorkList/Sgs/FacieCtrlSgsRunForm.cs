using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Sgs
{
    public partial class FacieCtrlSgsRunForm : Form
    {
        public FacieCtrlSgsRunForm()
        {
            InitializeComponent();
        }

        private List<string> facies = new List<string>() { };

        public Dictionary<string, SgsPar> GetAllPars()
        {
            if (facies.Distinct().Count() != facies.Count)
            {
                XtraMessageBox.Show("相类型重复");
                return null;
            }
            else
            {
                Dictionary<string, SgsPar> res = new Dictionary<string, SgsPar>();
                for (int i = 0; i < facies.Count; i++)
                {
                    foreach (Control item in this.tabPane1.Pages[i].Controls)
                    {
                        if (item is SgsUserControl sgsctrl)
                        {
                            res.Add(facies[i], sgsctrl.GetSgsPar());
                            break;
                        }
                    }
                }
                return res;
            }
        }

        public void SetAllPars(Dictionary<string, SgsPar> pars)
        {
            foreach (var item in pars)
            {
                AddNew(item.Key, item.Value);
            }
            Refresh();
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                using (FacieSelectForm form = new FacieSelectForm())
                {
                    form.NewFacie = (this.tabPane1.Pages.Count).ToString();
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        AddNew(form.NewFacie, null);
                    }
                }
            }
            else if (e.Button.Index == 1)
            {
                if (this.tabPane1.Pages.Count > 0)
                {
                    this.tabPane1.Pages.Remove(this.tabPane1.Pages.Last());
                    facies.RemoveAt(facies.Count - 1);
                }
            }
            Refresh();
        }

        private void AddNew(string facie, SgsPar par)
        {
            TabNavigationPage newpage = new TabNavigationPage();
            newpage.Caption = $"相 ‘{facie}’ 参数设置";
            SgsUserControl newsgsctrl = new SgsUserControl();
            newsgsctrl.Dock = DockStyle.Fill;
            newsgsctrl.Location = new Point(0, 0);
            newsgsctrl.Margin = new Padding(3, 2, 3, 2);
            newsgsctrl.Name = $"sgsUserControl{this.tabPane1.Pages.Count}";
            newsgsctrl.Size = new Size(1111, 307);
            if (par != null)
            {
                newsgsctrl.SetSgsPar(par);
            }
            newpage.Controls.Add(newsgsctrl);
            newpage.Name = $"tabNavigationPage{this.tabPane1.Pages.Count}";
            newpage.Size = new Size(1111, 376);
            this.tabPane1.Pages.Add(newpage);
            facies.Add(facie);
            Refresh();
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        public override void Refresh()
        {
            if (facies.Count > 0)
            {
                string str = string.Empty;
                foreach (var item in facies)
                {
                    str += $"{item} ";
                }
                this.buttonEdit1.Text = str;
            }
            else
            {
                this.buttonEdit1.Text = string.Empty;
            }
            base.Refresh();
        }
    }
}
