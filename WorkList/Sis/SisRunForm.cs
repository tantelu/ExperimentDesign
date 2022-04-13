using DevExpress.XtraEditors;
using ExperimentDesign.General;
using ExperimentDesign.WorkList.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExperimentDesign.WorkList.Sis
{
    public partial class SisRunForm : XtraForm
    {
        private Dictionary<object, Variogram> dic = new Dictionary<object, Variogram>();

        public SisRunForm()
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
            sis.SearchMaxRadius = Design<double>.GeneralDesign(SearchMaxRadius.Tag, SearchMaxRadius.Text);
            sis.SearchMedRadius = Design<double>.GeneralDesign(SearchMedRadius.Tag, SearchMedRadius.Text);
            sis.SearchMinRadius = Design<double>.GeneralDesign(SearchMinRadius.Tag, SearchMinRadius.Text);
            sis.Azimuth = Design<double>.GeneralDesign(Azimuth.Tag, Azimuth.Text);
            sis.Dip = Design<double>.GeneralDesign(Dip.Tag, Dip.Text);
            sis.Rake = Design<double>.GeneralDesign(Rake.Tag, Rake.Text);
            sis.MaxData = Design<int>.GeneralDesign(MaxData.Tag, MaxData.Text);
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
                MaxData.Tag = sis.MaxData;
                MaxData.Text = sis.MaxData.IsDesign ? sis.MaxData.DesignName : sis.MaxData.Value.ToString();

                Rake.Tag = sis.Rake;
                Rake.Text = sis.Rake.IsDesign ? sis.Rake.DesignName : sis.Rake.Value.ToString();

                Dip.Tag = sis.Dip;
                Dip.Text = sis.Dip.IsDesign ? sis.Dip.DesignName : sis.Dip.Value.ToString();

                Azimuth.Tag = sis.Azimuth;
                Azimuth.Text = sis.Azimuth.IsDesign? sis.Azimuth.DesignName: sis.Azimuth.Value.ToString();

                SearchMinRadius.Tag = sis.SearchMinRadius;
                SearchMinRadius.Text = sis.SearchMinRadius.IsDesign?sis.SearchMinRadius.DesignName : sis.SearchMinRadius.Value.ToString();
                
                SearchMedRadius.Tag = sis.SearchMedRadius;
                SearchMedRadius.Text = sis.SearchMedRadius.IsDesign? sis.SearchMedRadius.DesignName : sis.SearchMedRadius.Value.ToString();

                SearchMaxRadius.Tag = sis.SearchMaxRadius;
                SearchMaxRadius.Text = sis.SearchMaxRadius.IsDesign?sis.SearchMaxRadius.DesignName: sis.SearchMaxRadius.Value.ToString();
                
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
                    dic.Add(allFacies.SelectedItem,this.variogramControl1.GetVariogram().Clone());//新建一个实例 ，否则会导致ID重复使用
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
                dic[key] = vari.Clone();
            }
        }
    }
}
