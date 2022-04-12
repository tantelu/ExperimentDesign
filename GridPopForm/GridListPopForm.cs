using DevExpress.XtraEditors;
using ExperimentDesign.Uncertainty;
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

        decimal basevalue;

        public ListArgument(string text)
        {
            var temp = text.Split(new string[] { ",", ";", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            vs = temp.Select(_ => _.Trim()).ToList();
        }

        private ListArgument() { }

        public object GetBase()
        {
            return (Convert.ToDouble(vs[0]) + Convert.ToDouble(vs[vs.Count - 1])) / 2.0;
        }

        public IReadOnlyList<object> EqualSpaceSample(int sampletimes)
        {
            decimal max = Convert.ToDecimal(GetMax());
            decimal min = Convert.ToDecimal(GetMin());
            decimal inte = (max - min) / (sampletimes - 1);
            List<object> vs = new List<object>();
            vs.Add(min);
            for (int i = 1; i < sampletimes - 1; i++)
            {
                vs.Add(min + inte);
            }
            vs.Add(max);
            return vs;
        }

        public IReadOnlyList<object> MonteCarloSample(int sampletimes, int seed)
        {
            decimal max = Convert.ToDecimal(GetMax());
            decimal min = Convert.ToDecimal(GetMin());
            Random R = new Random(seed);
            List<object> vs = new List<object>();
            for (int i = 0; i < sampletimes; i++)
            {
                vs.Add(min + (decimal)R.NextDouble() * (max - min));
            }
            return vs;
        }

        public object GetLevel(int level)
        {
            if (level < 0 || level >= vs.Count)
            {
                return double.NaN;
            }
            else
            {
                return vs[level];
            }
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
