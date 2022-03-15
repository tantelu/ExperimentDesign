using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList
{
    public partial class WorkSelectForm : Form
    {
        private List<WorkControlItem> selects = new List<WorkControlItem>();

        public WorkSelectForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void workList_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                selects.Add((WorkControlItem)this.workList.SelectedItem);
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in selects)
            {
                sb.AppendLine(item.ToString());
            }
            this.textBox1.Text = sb.ToString();
            this.Refresh();
        }

        private void WorkSelectForm_Load(object sender, EventArgs e)
        {
            this.workList.Properties.Items.Clear();
            string path = Path.Combine(Application.StartupPath, @"WorkList.json");
            var list = JsonConvert.DeserializeObject<List<WorkControlItem>>(File.ReadAllText(path));
            if (list.Count > 0)
            {
                this.workList.Properties.Items.AddRange(list);
                this.workList.SelectedIndex = 0;
            }
        }

        public List<WorkControlItem> GetSelects()
        {
            return selects;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            selects.Clear();
            this.textBox1.Text = string.Empty;
        }
    }

    public class WorkControlItem
    {
        public string ControlType { get; set; }

        public string Name { get; set; }

        public virtual void ReadJObject(JObject json)
        {
            if (json.ContainsKey(nameof(Name)))
            {
                Name = json[nameof(Name)].ToString();
            }
            if (json.ContainsKey(nameof(ControlType)))
            {
                ControlType = json[nameof(ControlType)].ToString();
            }
        }

        public virtual JObject ToJObject()
        {
            JObject obj = new JObject();
            obj.Add(nameof(Name), Name);
            obj.Add(nameof(ControlType), ControlType);
            return obj;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
