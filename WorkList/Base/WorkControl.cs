using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Base
{
    public partial class WorkControl : UserControl
    {
        public IWork Main { get; set; }

        public WorkControl()
        {
            InitializeComponent();
            this.pictureEdit1.EditValue = Picture;
            this.textEdit3.Text = WorkName;
        }

        protected virtual string WorkName { get; }

        protected virtual string GetWorkPath(int index)
        {
            string path = Path.Combine(Main.GetWorkPath(), $"{index}");
            return path;
        }

        protected virtual Bitmap Picture { get; }

        public void SetIndex(int index)
        {
            textEdit1.Text = index.ToString();
        }

        private void pictureEdit1_DoubleClick(object sender, EventArgs e)
        {
            ShowParamForm();
        }

        protected virtual void ShowParamForm() { }

        /// <summary>
        /// 界面上显示的设计参数
        /// </summary>
        protected void UpdateText(List<VariableData> param)
        {
            StringBuilder sb = new StringBuilder();
            if (param?.Count > 0)
            {
                
                foreach (var item in param)
                {
                    if (item.Name.ToString().Contains("$"))
                    {
                        sb.Append(item.Name);
                        sb.Append(",");
                    }
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
            return string.Empty;
        }

        public virtual void Open(string str)
        {
            
        }

        public virtual IReadOnlyList<VariableData> GetUncentainParam()
        {
            return null;
        }

        public override bool Focused => this.textEdit3.Focused;

        private void pictureEdit2_DoubleClick(object sender, EventArgs e)
        {
            Main?.Delete(this);
        }
    }
}
