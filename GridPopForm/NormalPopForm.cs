using DevExpress.XtraEditors;
using ExperimentDesign.Uncertainty;
using System;
using System.Collections.Generic;

namespace ExperimentDesign.GridPopForm
{
    public partial class NormalPopForm : XtraForm
    {
        public NormalPopForm()
        {
            InitializeComponent();
        }

        public IArgument Argument
        {
            get
            {
                return new NormalArgument(this.spinEdit1.Value, this.spinEdit2.Value);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
    
    public class NormalArgument : IArgument
    {
        decimal mean;
        decimal std;

        public NormalArgument(decimal mean, decimal std)
        {
            this.mean = mean;
            this.std = std;
        }

        private NormalArgument() { }

        public object GetBase()
        {
            return mean;
        }

        public object GetLevel(int level)
        {
            return double.NaN;
        }

        public IReadOnlyList<object> EqualSpaceSample(int sampletimes)
        {
            decimal max = mean + (decimal)1.64485 * std;
            decimal min = mean - (decimal)1.6448536 * std;
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
            decimal max = mean + (decimal)1.64485 * std;
            decimal min = mean - (decimal)1.6448536 * std;
            Random R = new Random(seed);
            List<object> vs = new List<object>();
            for (int i = 0; i < sampletimes; i++)
            {
                vs.Add(min + (decimal)R.NextDouble() * (max - min));
            }
            return vs;
        }

        //90%置信区间 zp=1.644853
        public object GetMax()
        {
            return mean + (decimal)1.64485 * std;
        }

        public object GetMin()
        {
            return mean - (decimal)1.6448536 * std;
        }

        public void Open(string json)
        {
            string[] strs = json.Split(',');
            mean = Convert.ToDecimal(strs[0]);
            std = Convert.ToDecimal(strs[1]);
        }

        public string Save()
        {
            return $"{mean},{std}";
        }

        public override string ToString()
        {
            return $"Mean:{mean};Std:{std}";
        }
    }
}
