using DevExpress.XtraEditors;
using ExperimentDesign.Uncertainty;
using System;
using System.Collections.Generic;

namespace ExperimentDesign.GridPopForm
{
    public partial class TrianglePopForm : XtraForm
    {
        public TrianglePopForm()
        {
            InitializeComponent();
        }

        public IArgument Argument
        {
            get
            {
                return new TriangleArgument(this.spinEdit1.Value, this.spinEdit2.Value, this.spinEdit3.Value);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }

    public class TriangleArgument : IArgument
    {
        decimal min;
        decimal med;
        decimal max;

        public TriangleArgument(decimal min, decimal med, decimal max)
        {
            this.min = min;
            this.med = med;
            this.max = max;
        }

        private TriangleArgument() { }

        public object GetBase()
        {
            return med;
        }

        public object GetLevel(int level)
        {
            if (level == 0)
            {
                return min;
            }
            else if (level == 1)
            {
                return max;
            }
            else
            {
                return double.NaN;
            }
        }

        public object GetMax()
        {
            return max;
        }

        public object GetMin()
        {
            return min;
        }

        public IReadOnlyList<object> EqualSpaceSample(int sampletimes)
        {
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
            Random R = new Random(seed);
            List<object> vs = new List<object>();
            for (int i = 0; i < sampletimes; i++)
            {
                vs.Add(min + (decimal)R.NextDouble() * (max - min));
            }
            return vs;
        }

        public void Open(string json)
        {
            string[] strs = json.Split(',');
            min = Convert.ToDecimal(strs[0]);
            med = Convert.ToDecimal(strs[1]);
            max = Convert.ToDecimal(strs[2]);
        }

        public string Save()
        {
            return $"{min},{med},{max}";
        }

        public override string ToString()
        {
            return $"Min:{min};Med:{med};Max:{max}";
        }
    }
}
