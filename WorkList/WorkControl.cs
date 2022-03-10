using ExperimentDesign.WorkList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

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
        }

        public virtual void Run(string workpath) { }

        public virtual bool GetRunState() { return true; }

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
