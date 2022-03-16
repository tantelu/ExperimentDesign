using DevExpress.XtraEditors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public SisPar GetSisPar(Grid3D grid, IReadOnlyDictionary<string, object> designVaribles)
        {
            return new SisPar(grid);
        }

        public string SaveForm()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(this.allFacies.Name);
            writer.WriteStartArray();
            if (dic.Count > 0)
            {
                foreach (var item in this.allFacies.Items)
                {
                    writer.WriteValue(item);
                }
            }
            writer.WriteEndArray();
            writer.WritePropertyName(this.selectedFacies.Name);
            writer.WriteStartArray();
            if (dic.Count > 0)
            {
                foreach (var item in this.selectedFacies.Items)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("Key");
                    writer.WriteValue(item);
                    writer.WritePropertyName("Value");
                    writer.WriteValue(dic[item].Save());
                    writer.WriteEndObject();
                }
            }
            writer.WriteEndArray();
            writer.WritePropertyName(this.allsameCheck.Name);
            writer.WriteValue(this.allsameCheck.Checked);
            writer.WritePropertyName(this.krigType.Name);
            writer.WriteValue(this.krigType.SelectedIndex);
            writer.WritePropertyName(this.maxData.Name);
            writer.WriteValue(this.maxData.Text);
            writer.WritePropertyName(this.mulgridnum.Name);
            writer.WriteValue(this.mulgridnum.Text);
            writer.WritePropertyName(this.usemultigrid.Name);
            writer.WriteValue(this.usemultigrid.Checked);
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public void InitForm(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JObject jo = JObject.Parse(json);
                var job = jo.GetValue(this.allFacies.Name);
                if (job is JArray ja1)
                {
                    this.allFacies.Items.Clear();
                    for (int i = 0; i < ja1.Count; i++)
                    {
                        this.allFacies.Items.Add(ja1[i]);
                    }
                }
                job = jo.GetValue(this.selectedFacies.Name);
                if (job is JArray ja2)
                {
                    this.selectedFacies.Items.Clear();
                    for (int i = 0; i < ja2.Count; i++)
                    {
                        JObject jobj = ja2[i] as JObject;
                        Variogram var = new Variogram();
                        var.Open(jobj["Value"].ToString());
                        dic.Add(jobj["Key"], var);
                        this.selectedFacies.Items.Add(jobj["Key"]);
                    }
                }
                this.allsameCheck.Checked = (bool)jo.GetValue(this.allsameCheck.Name);
                this.krigType.SelectedIndex = (int)jo[this.krigType.Name];
                this.maxData.Text = jo[this.maxData.Name].ToString();
                this.mulgridnum.Text = jo[this.mulgridnum.Name].ToString();
                this.usemultigrid.Checked = (bool)jo[this.usemultigrid.Name];
                SetSameCheck();
            }
        }

        public void UpdateCurrentVariogram()
        {
            var vari = this.variogramControl1.GetVariogram();
            if (allsameCheck.Checked)
            {
                var keys = dic.Keys.ToList();
                foreach (var key in keys)
                {
                    dic[key] = vari;
                }
            }
            else
            {
                if (selectedFacies.SelectedItem != null && dic.ContainsKey(selectedFacies.SelectedItem))
                {
                    dic[selectedFacies.SelectedItem] = vari;
                }
            }
        }

        public void SetSameCheck()
        {
            if (selectedFacies.Items.Count > 2)
            {
                allsameCheck.Enabled = true;
            }
            else
            {
                allsameCheck.Enabled = false;
                allsameCheck.Checked = true;
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
                    SetSameCheck();
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
                    SetSameCheck();
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

        private void allsameCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            if (allsameCheck.Checked)
            {
                UpdateCurrentVariogram();
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
