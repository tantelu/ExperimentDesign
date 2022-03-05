using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ExperimentDesign
{
    public partial class WorkControl : UserControl
    {
        public WorkControl()
        {
            InitializeComponent();
            this.textEdit3.Text = WorkName;
        }

        protected virtual string WorkName { get; }

        public void SetIndex(int index)
        {
            textEdit1.Text = index.ToString();
        }

        private void pictureEdit1_DoubleClick(object sender, EventArgs e)
        {
            ShowParamForm();
        }

        protected virtual void ShowParamForm() { }

        protected virtual void Run() { }

        protected virtual void SetUncentainParam(List<string> pars)
        {
            string res = string.Empty;
            foreach (var item in pars)
            {
                res += $"{item},";
            }
            this.textEdit2.Text = res;
        }

        public virtual List<String> GetUncentainParam()
        {
            var strs = this.textEdit2.Text.Split(',');
            return strs.Select(_ => _.Trim()).Where(_ => _.Length > 0).ToList();
        }

        public override bool Focused => this.textEdit3.Focused;

        //private void textEdit3_MouseDown(object sender, MouseEventArgs e)
        //{
        //    ((TextEdit)sender).BackColor = Color.BlueViolet;
        //    ((TextEdit)sender).ForeColor = Color.White;
        //}
    }
}
