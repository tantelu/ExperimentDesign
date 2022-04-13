using DevExpress.XtraEditors;
using ExperimentDesign.Uncertainty;
using System;
using System.Collections.Generic;

namespace ExperimentDesign.GridPopForm
{
    public partial class MinMaxPopForm : XtraForm
    {
        public MinMaxPopForm()
        {
            InitializeComponent();
        }

        public IArgument Argument
        {
            get
            {
                return new MinMaxArgument(this.spinEdit1.Value, this.spinEdit2.Value);
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }

    public class MinMaxArgument : IArgument
    {
        decimal min;
        decimal max;

        public MinMaxArgument(decimal min, decimal max)
        {
            this.min = min;
            this.max = max;
        }

        private MinMaxArgument() { }

        public object GetBase()
        {
            return (max + min) / (decimal)2.0;
        }

        public IReadOnlyList<object> EqualSpaceSample(int sampletimes)
        {
            decimal inte = (max - min) / (sampletimes - 1);
            List<object> vs = new List<object>();
            vs.Add(min);
            for (int i = 1; i < sampletimes - 1; i++)
            {
                vs.Add(min + inte * i);
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

        public void Open(string json)
        {
            string[] strs = json.Split(',');
            min = Convert.ToDecimal(strs[0]);
            max = Convert.ToDecimal(strs[1]);
        }

        public string Save()
        {
            return $"{min},{max}";
        }

        public override string ToString()
        {
            return $"Min:{min};Max:{max}";
        }
    }
}
