using DevExpress.XtraEditors;
using ExperimentDesign.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExperimentDesign.WorkList.Sis
{
    public partial class SigRunForm : XtraForm
    {
        private Dictionary<object, Variogram> dic = new Dictionary<object, Variogram>();

        public SigRunForm()
        {
            InitializeComponent();
        }

        public SisPar GetSisPar()
        {
            SisPar sis = new SisPar();
            if (this.allFacies.Items?.Count > 0)
            {
                sis.Unselected = new List<int>();
                foreach (var item in this.allFacies.Items)
                {
                    sis.Unselected.Add(Convert.ToInt32(item));
                }
            }
            var pros = this.probalities.Text.Split(new string[] { " ", ";", "," }, StringSplitOptions.RemoveEmptyEntries);
            sis.Vars = new List<CategoryIndicatorParam>(dic.Count);
            foreach (var item in dic)
            {
                CategoryIndicatorParam ipar = new CategoryIndicatorParam();
                ipar.Variogram = item.Value;
                ipar.Facie = Convert.ToInt32(item.Key);
                ipar.Probability = Convert.ToDouble(pros[this.selectedFacies.Items.IndexOf(item.Key)]);
                sis.Vars.Add(ipar);
            }
            if (SearchMaxRadius.Text.Contains("$"))
            {
                sis.SearchMaxRadius = new Design<double>(Convert.ToDouble(SearchMaxRadius.Tag), SearchMaxRadius.Text);
            }
            else
            {
                sis.SearchMaxRadius = new Design<double>(Convert.ToDouble(SearchMaxRadius.Text));
            }
            if (SearchMedRadius.Text.Contains("$"))
            {
                sis.SearchMedRadius = new Design<double>(Convert.ToDouble(SearchMedRadius.Tag), SearchMedRadius.Text);
            }
            else
            {
                sis.SearchMedRadius = new Design<double>(Convert.ToDouble(SearchMedRadius.Text));
            }
            if (SearchMinRadius.Text.Contains("$"))
            {
                sis.SearchMinRadius = new Design<double>(Convert.ToDouble(SearchMinRadius.Tag), SearchMinRadius.Text);
            }
            else
            {
                sis.SearchMinRadius = new Design<double>(Convert.ToDouble(SearchMinRadius.Text));
            }

            if (Azimuth.Text.Contains("$"))
            {
                sis.Azimuth = new Design<double>(Convert.ToDouble(Azimuth.Tag), Azimuth.Text);
            }
            else
            {
                sis.Azimuth = new Design<double>(Convert.ToDouble(Azimuth.Text));
            }
            if (Dip.Text.Contains("$"))
            {
                sis.Dip = new Design<double>(Convert.ToDouble(Dip.Tag), Dip.Text);
            }
            else
            {
                sis.Dip = new Design<double>(Convert.ToDouble(Dip.Text));
            }
            if (Rake.Text.Contains("$"))
            {
                sis.Rake = new Design<double>(Convert.ToDouble(Rake.Tag), Rake.Text);
            }
            else
            {
                sis.Rake = new Design<double>(Convert.ToDouble(Rake.Text));
            }

            if (MaxData.Text.Contains("$"))
            {
                sis.MaxData = new Design<int>(Convert.ToInt32(MaxData.Tag), MaxData.Text);
            }
            else
            {
                sis.MaxData = new Design<int>(Convert.ToInt32(MaxData.Text));
            }
            sis.MedianIK = MedianIK.Checked;
            sis.KrigType = (KrigType)krigType.SelectedIndex;
            return sis;
        }

        public void InitForm(SisPar sis)
        {
            if (sis != null)
            {
                MedianIK.Checked = sis.MedianIK;
                krigType.SelectedIndex = (int)sis.KrigType;
                if (sis.MaxData.IsDesign)
                {
                    MaxData.Text = sis.MaxData.DesignName;
                    MaxData.Tag = sis.MaxData.Value;
                }
                else
                {
                    MaxData.Text = sis.MaxData.Value.ToString();
                    MaxData.Tag = sis.MaxData.Value;
                }

                if (sis.Rake.IsDesign)
                {
                    Rake.Text = sis.Rake.DesignName;
                    Rake.Tag = sis.Rake.Value;
                }
                else
                {
                    Rake.Text = sis.Rake.Value.ToString();
                    Rake.Tag = sis.Rake.Value;
                }

                if (sis.Dip.IsDesign)
                {
                    Dip.Text = sis.Dip.DesignName;
                    Dip.Tag = sis.Dip.Value;
                }
                else
                {
                    Dip.Text = sis.Dip.Value.ToString();
                    Dip.Tag = sis.Dip.Value;
                }

                if (sis.Azimuth.IsDesign)
                {
                    Azimuth.Text = sis.Azimuth.DesignName;
                    Azimuth.Tag = sis.Azimuth.Value;
                }
                else
                {
                    Azimuth.Text = sis.Azimuth.Value.ToString();
                    Azimuth.Tag = sis.Azimuth.Value;
                }

                if (sis.SearchMinRadius.IsDesign)
                {
                    SearchMinRadius.Text = sis.SearchMinRadius.DesignName;
                    SearchMinRadius.Tag = sis.SearchMinRadius.Value;
                }
                else
                {
                    SearchMinRadius.Text = sis.SearchMinRadius.Value.ToString();
                    SearchMinRadius.Tag = sis.SearchMinRadius.Value;
                }

                if (sis.SearchMedRadius.IsDesign)
                {
                    SearchMedRadius.Text = sis.SearchMedRadius.DesignName;
                    SearchMedRadius.Tag = sis.SearchMedRadius.Value;
                }
                else
                {
                    SearchMedRadius.Text = sis.SearchMedRadius.Value.ToString();
                    SearchMedRadius.Tag = sis.SearchMedRadius.Value;
                }

                if (sis.SearchMaxRadius.IsDesign)
                {
                    SearchMaxRadius.Text = sis.SearchMaxRadius.DesignName;
                    SearchMaxRadius.Tag = sis.SearchMaxRadius.Value;
                }
                else
                {
                    SearchMaxRadius.Text = sis.SearchMaxRadius.Value.ToString();
                    SearchMaxRadius.Tag = sis.SearchMaxRadius.Value;
                }
                this.allFacies.Items.Clear();
                dic.Clear();
                string pro = string.Empty;
                if (sis.Vars?.Count > 0)
                {
                    foreach (var item in sis.Vars)
                    {
                        dic.Add(item.Facie, item.Variogram);
                        this.selectedFacies.Items.Add(item.Facie);
                        pro += $"{item.Probability} ";
                    }
                }
                if (sis.Unselected?.Count > 0)
                {
                    foreach (var item in sis.Unselected)
                    {
                        this.allFacies.Items.Add(item);
                    }
                }

                this.probalities.Text = pro;
            }
        }

        public void UpdateCurrentVariogram()
        {
            var vari = this.variogramControl1.GetVariogram();
            if (selectedFacies.SelectedItem != null && dic.ContainsKey(selectedFacies.SelectedItem))
            {
                dic[selectedFacies.SelectedItem] = vari;
            }
        }

        private void simpleButton_right_Click(object sender, System.EventArgs e)
        {
            if (allFacies.SelectedItem != null)
            {
                if (!selectedFacies.Items.Contains(allFacies.SelectedItem))
                {
                    dic.Add(allFacies.SelectedItem, Variogram.Default);
                    selectedFacies.Items.Add(allFacies.SelectedItem);
                    allFacies.Items.Remove(allFacies.SelectedItem);
                    var pros = string.Empty;
                    var v = 1.0 / this.selectedFacies.Items.Count;
                    for (int i = 0; i < this.selectedFacies.Items.Count; i++)
                    {
                        pros += $"{v.ToString("f2")}  ";
                    }
                    this.probalities.Text = pros;
                }
            }
        }

        private void simpleButton_left_Click(object sender, System.EventArgs e)
        {
            if (selectedFacies.SelectedItem != null)
            {
                if (!allFacies.Items.Contains(selectedFacies.SelectedItem))
                {
                    dic.Remove(selectedFacies.SelectedItem);
                    allFacies.Items.Add(selectedFacies.SelectedItem);
                    selectedFacies.Items.Remove(selectedFacies.SelectedItem);
                    var pros = string.Empty;
                    if (this.selectedFacies.Items.Count > 0)
                    {
                        var v = 1.0 / this.selectedFacies.Items.Count;
                        for (int i = 0; i < this.selectedFacies.Items.Count; i++)
                        {
                            pros += $"{v.ToString("f2")}  ";
                        }
                    }
                    this.probalities.Text = pros;
                }
            }
        }

        private void variogramControl1_Leave(object sender, System.EventArgs e)
        {
            UpdateCurrentVariogram();
        }

        private void selectedFacies_SelectedValueChanged(object sender, System.EventArgs e)
        {
            if (this.selectedFacies.SelectedItem != null)
            {
                this.variogramControl1.SetVariogram(dic[this.selectedFacies.SelectedItem]);
            }
            else
            {
                //this.variogramControl1.Hide();
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

        private void allsame_Click(object sender, EventArgs e)
        {
            var vari = this.variogramControl1.GetVariogram();
            var keys = dic.Keys.ToList();
            foreach (var key in keys)
            {
                dic[key] = vari;
            }
        }
    }
}
