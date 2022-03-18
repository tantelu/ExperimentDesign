using DevExpress.XtraEditors;
using ExperimentDesign.General;
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
            SgsPar sgs = new SgsPar();
            sgs.Variogram = this.variogramControl1.GetVariogram();
            if (SearchMaxRadius.Text.Contains("$"))
            {
                sgs.SearchMaxRadius = new Design<double>(Convert.ToDouble(SearchMaxRadius.Tag), SearchMaxRadius.Text);
            }
            else
            {
                sgs.SearchMaxRadius = new Design<double>(Convert.ToDouble(SearchMaxRadius.Text));
            }
            if (SearchMedRadius.Text.Contains("$"))
            {
                sgs.SearchMedRadius = new Design<double>(Convert.ToDouble(SearchMedRadius.Tag), SearchMedRadius.Text);
            }
            else
            {
                sgs.SearchMedRadius = new Design<double>(Convert.ToDouble(SearchMedRadius.Text));
            }
            if (SearchMinRadius.Text.Contains("$"))
            {
                sgs.SearchMinRadius = new Design<double>(Convert.ToDouble(SearchMinRadius.Tag), SearchMinRadius.Text);
            }
            else
            {
                sgs.SearchMinRadius = new Design<double>(Convert.ToDouble(SearchMinRadius.Text));
            }

            if (Azimuth.Text.Contains("$"))
            {
                sgs.Azimuth = new Design<double>(Convert.ToDouble(Azimuth.Tag), Azimuth.Text);
            }
            else
            {
                sgs.Azimuth = new Design<double>(Convert.ToDouble(Azimuth.Text));
            }
            if (Dip.Text.Contains("$"))
            {
                sgs.Dip = new Design<double>(Convert.ToDouble(Dip.Tag), Dip.Text);
            }
            else
            {
                sgs.Dip = new Design<double>(Convert.ToDouble(Dip.Text));
            }
            if (Rake.Text.Contains("$"))
            {
                sgs.Rake = new Design<double>(Convert.ToDouble(Rake.Tag), Rake.Text);
            }
            else
            {
                sgs.Rake = new Design<double>(Convert.ToDouble(Rake.Text));
            }

            if (MaxData.Text.Contains("$"))
            {
                sgs.MaxData = new Design<int>(Convert.ToInt32(MaxData.Tag), MaxData.Text);
            }
            else
            {
                sgs.MaxData = new Design<int>(Convert.ToInt32(MaxData.Text));
            }
            sgs.KrigType = (KrigType)krigType.SelectedIndex;
            return sgs;
        }

        public void InitForm(SgsPar sgs)
        {
            if (sgs != null)
            {
                krigType.SelectedIndex = (int)sgs.KrigType;
                if (sgs.MaxData.IsDesign)
                {
                    MaxData.Text = sgs.MaxData.DesignName;
                    MaxData.Tag = sgs.MaxData.Value;
                }
                else
                {
                    MaxData.Text = sgs.MaxData.Value.ToString();
                    MaxData.Tag = sgs.MaxData.Value;
                }

                if (sgs.Rake.IsDesign)
                {
                    Rake.Text = sgs.Rake.DesignName;
                    Rake.Tag = sgs.Rake.Value;
                }
                else
                {
                    Rake.Text = sgs.Rake.Value.ToString();
                    Rake.Tag = sgs.Rake.Value;
                }

                if (sgs.Dip.IsDesign)
                {
                    Dip.Text = sgs.Dip.DesignName;
                    Dip.Tag = sgs.Dip.Value;
                }
                else
                {
                    Dip.Text = sgs.Dip.Value.ToString();
                    Dip.Tag = sgs.Dip.Value;
                }

                if (sgs.Azimuth.IsDesign)
                {
                    Azimuth.Text = sgs.Azimuth.DesignName;
                    Azimuth.Tag = sgs.Azimuth.Value;
                }
                else
                {
                    Azimuth.Text = sgs.Azimuth.Value.ToString();
                    Azimuth.Tag = sgs.Azimuth.Value;
                }

                if (sgs.SearchMinRadius.IsDesign)
                {
                    SearchMinRadius.Text = sgs.SearchMinRadius.DesignName;
                    SearchMinRadius.Tag = sgs.SearchMinRadius.Value;
                }
                else
                {
                    SearchMinRadius.Text = sgs.SearchMinRadius.Value.ToString();
                    SearchMinRadius.Tag = sgs.SearchMinRadius.Value;
                }

                if (sgs.SearchMedRadius.IsDesign)
                {
                    SearchMedRadius.Text = sgs.SearchMedRadius.DesignName;
                    SearchMedRadius.Tag = sgs.SearchMedRadius.Value;
                }
                else
                {
                    SearchMedRadius.Text = sgs.SearchMedRadius.Value.ToString();
                    SearchMedRadius.Tag = sgs.SearchMedRadius.Value;
                }

                if (sgs.SearchMaxRadius.IsDesign)
                {
                    SearchMaxRadius.Text = sgs.SearchMaxRadius.DesignName;
                    SearchMaxRadius.Tag = sgs.SearchMaxRadius.Value;
                }
                else
                {
                    SearchMaxRadius.Text = sgs.SearchMaxRadius.Value.ToString();
                    SearchMaxRadius.Tag = sgs.SearchMaxRadius.Value;
                }
                this.variogramControl1.SetVariogram(sgs.Variogram);
            }
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
