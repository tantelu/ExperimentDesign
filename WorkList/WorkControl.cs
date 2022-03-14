using ExperimentDesign.WorkList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WorkList.ExperimentDesign
{
    public partial class WorkControl : UserControl
    {
        protected List<UncertainParam> param = new List<UncertainParam>();

        public IWork Main { get; set; }

        public WorkControl()
        {
            InitializeComponent();
            this.pictureEdit1.EditValue = Picture;
            this.textEdit3.Text = WorkName;
        }

        protected virtual string WorkName { get; }

        protected virtual Bitmap Picture { get; }

        public void SetIndex(int index)
        {
            textEdit1.Text = index.ToString();
        }

        private void pictureEdit1_DoubleClick(object sender, EventArgs e)
        {
            ShowParamForm();
            UpdateText();
        }

        protected virtual void ShowParamForm() { }

        /// <summary>
        /// 界面上显示的设计参数
        /// </summary>
        private void UpdateText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in param)
            {
                if (item.EditorValue.ToString().Contains("$"))
                {
                    sb.Append(item.EditorValue);
                    sb.Append(",");
                }
            }
            if (sb.Length > 0)
            {
                this.textEdit2.Text = sb.ToString(0, sb.Length - 1);
            }
            else
            {
                this.textEdit2.Text = string.Empty;
            }
        }

        public virtual void Run(int index, IReadOnlyDictionary<string, object> designVaribles) { }

        public virtual bool GetRunState(int index) { return true; }

        public virtual string Save()
        {
            var str = JsonConvert.SerializeObject(param);
            return str;
        }

        public virtual void Open(string str)
        {
            param = JsonConvert.DeserializeObject<List<UncertainParam>>(str);
            UpdateText();
        }

        public virtual IReadOnlyList<UncertainParam> GetUncentainParam()
        {
            return param;
        }

        public override bool Focused => this.textEdit3.Focused;

        private void pictureEdit2_DoubleClick(object sender, EventArgs e)
        {
            Main?.Delete(this);
        }
    }
}
