using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList
{
    public partial class WorkControl : UserControl
    {
        protected List<VariableData> param = new List<VariableData>();

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
            UpdateText();
        }

        protected virtual void ShowParamForm() { }

        /// <summary>
        /// 界面上显示的设计参数
        /// </summary>
        protected void UpdateText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in param)
            {
                if (item.Name.ToString().Contains("$"))
                {
                    sb.Append(item.Name);
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
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartArray();
            foreach (var item in param)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Data");
                writer.WriteValue(item.Save());
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public virtual void Open(string str)
        {
            param.Clear();
            JArray ja = JArray.Parse(str);
            for (int i = 0; i < ja.Count; i++)
            {
                JObject jo = ja[i] as JObject;
                VariableData data = new VariableData();
                data.Open(jo["Data"]?.ToString());
                param.Add(data);
            }
            UpdateText();
        }

        public virtual IReadOnlyList<VariableData> GetUncentainParam()
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
