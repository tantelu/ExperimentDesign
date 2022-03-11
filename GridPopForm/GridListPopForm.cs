using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExperimentDesign.GridPopForm
{
    public partial class GridListPopForm : XtraForm
    {
        public GridListPopForm()
        {
            InitializeComponent();
        }

        public IArgument Argument
        {
            get
            {
                return new ListArgument(this.textBox1.Text);
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }

    public class ListArgument : IArgument
    {
        List<string> vs;

        public ListArgument(string text)
        {
            var temp = text.Split(new string[] { ",", ";", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            vs = temp.Select(_ => _.Trim()).ToList();
        }

        private ListArgument() { }

        public object GetLevel(int level)
        {
            return vs[level];
        }

        public object GetMax()
        {
            return vs[vs.Count - 1];
        }

        public object GetMin()
        {
            return vs[0];
        }

        public void Open(string json)
        {
            vs = JsonConvert.DeserializeObject<List<string>>(json);
        }

        public string Save()
        {
            return JsonConvert.SerializeObject(vs);
        }

        public override string ToString()
        {
            if (vs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("List:");
                foreach (var item in vs)
                {
                    sb.Append(item);
                    sb.Append(";");
                }
                return sb.ToString(0, sb.Length - 1);
            }
            else
            {
                return string.Empty;
            }
        }
    }

}
