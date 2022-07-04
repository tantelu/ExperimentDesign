using DevExpress.XtraEditors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ExperimentDesign.WorkList.Base;
using System.ComponentModel;

namespace ExperimentDesign
{
    public partial class SparseValidForm : XtraForm, IWork
    {
        private List<WorkFlow> exitworks = new List<WorkFlow>();

        private WorkFlow Current { get; set; }

        public SparseValidForm()
        {
            InitializeComponent();
            var folders = Directory.GetDirectories(GlobalWorkCongfig.WorkBasePath);
            var exits = folders?.Select(_ => new WorkFlow(Path.GetFileNameWithoutExtension(_))).ToList();
            if (exits?.Count > 0)
            {
                comboBoxEdit_exit.Properties.Items.AddRange(exits);
            }
            this.gridControl1.DataSource = SparseMethod(0);
        }

        //运行工作流
        public void Run()
        {
            if (Current != null)
            {
                if (textBox_name.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    XtraMessageBox.Show($"{textBox_name.Text} 不能作为文件名!");
                    return;
                }
                if (File.Exists(SparseName()))
                {
                    var dialog = XtraMessageBox.Show("当前工作流存在存在抽稀验证结果,是否继续？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialog == DialogResult.Yes)
                    {
                        File.Delete(SparseName());
                    }
                    else
                    {
                        return;
                    }
                }
                var workControls = this.workPanel.Controls;
                var datas = this.gridControl1.DataSource as List<SparseWellData>;
                if (datas.Count > 0)
                {
                    for (int i = 0; i < datas.Count; i++)
                    {
                        //生成一个
                        var workpath = Path.Combine(GetWorkPath(), $"{i + 1}");
                        if (!Directory.Exists(workpath))
                        {
                            Directory.CreateDirectory(workpath);
                        }
                        //把条件数据文件生成出来 然后构造一个变量字典（适配工作流）
                        //条件数据就是一个Gslib文件，这里做个测试
                        IReadOnlyDictionary<string, string> harddata = new Dictionary<string, string>();
                        //foreach (var item in workControls)
                        //{
                        //    if (item is WorkControl ctrl)
                        //    {
                        //        ctrl.Run(i + 1, designTable[i]);
                        //        int curwait = 0;
                        //        while (!ctrl.GetRunState(i + 1))
                        //        {
                        //            Thread.Sleep(1000);
                        //            curwait += 1000;
                        //            if (curwait > maxwait)
                        //            {
                        //                var dia = XtraMessageBox.Show($"单一工作流运行时间超出{maxwait / 1000.0}S,是否继续等待？", "提示", MessageBoxButtons.YesNo);
                        //                if (dia == DialogResult.Yes)
                        //                {
                        //                    maxwait *= 2;
                        //                }
                        //                else
                        //                {
                        //                    return;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
                XtraMessageBox.Show($"工作流执行完成");
            }
            else
            {
                XtraMessageBox.Show("请先设置当前工作流");
            }
        }
        //删除某一工作流
        void IWork.Delete(WorkControl control)
        {
            int index = this.workPanel.Controls.IndexOf(control);
            this.SuspendLayout();
            this.workPanel.SuspendLayout();
            for (int i = 0; i < this.workPanel.Controls.Count; i++)
            {
                if (i > index)
                {
                    this.workPanel.Controls[i].Location = this.workPanel.Controls[i].Location - new Size(0, 23);
                    (this.workPanel.Controls[i] as WorkControl)?.SetIndex(i);
                }
            }
            this.workPanel.Controls.Remove(control);
            this.workPanel.ResumeLayout();
            this.ResumeLayout(false);
        }

        void IWork.Up(WorkControl control)
        {
            int index = this.workPanel.Controls.IndexOf(control);
            if (index > 0)
            {
                this.SuspendLayout();
                this.workPanel.SuspendLayout();
                var upctrl = this.workPanel.Controls[index - 1];
                var cur = this.workPanel.Controls[index];
                var tempLoc = upctrl.Location;
                upctrl.Location = cur.Location;
                cur.Location = tempLoc;
                this.workPanel.Controls.SetChildIndex(control, index - 1);

                for (int i = 0; i < this.workPanel.Controls.Count; i++)
                {
                    (this.workPanel.Controls[i] as WorkControl)?.SetIndex(i + 1);
                }
                this.workPanel.ResumeLayout();
                this.ResumeLayout(false);
            }
        }

        void IWork.Down(WorkControl control)
        {
            int index = this.workPanel.Controls.IndexOf(control);
            if (index >= 0 && index < this.workPanel.Controls.Count - 1)
            {
                this.SuspendLayout();
                this.workPanel.SuspendLayout();
                var downctrl = this.workPanel.Controls[index + 1];
                var cur = this.workPanel.Controls[index];
                var tempLoc = downctrl.Location;
                downctrl.Location = cur.Location;
                cur.Location = tempLoc;
                this.workPanel.Controls.SetChildIndex(control, index + 1);
                for (int i = 0; i < this.workPanel.Controls.Count; i++)
                {
                    (this.workPanel.Controls[i] as WorkControl)?.SetIndex(i + 1);
                }
                this.workPanel.ResumeLayout();
                this.ResumeLayout(false);
            }
        }
        //保存工作流
        public void Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("Name");
            writer.WriteValue(Current.Name);
            writer.WritePropertyName("Controls");
            writer.WriteStartArray();
            foreach (var item in this.workPanel.Controls)
            {
                if (item is WorkControl ctrl)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("Type");
                    writer.WriteValue(ctrl.GetType().FullName);
                    writer.WritePropertyName("Params");
                    writer.WriteValue(ctrl.Save());
                    writer.WriteEndObject();
                }
            }
            writer.WriteEndArray();
            writer.WritePropertyName("UncertainParam");
            writer.WriteStartArray();
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = sw.GetStringBuilder().ToString();
            File.WriteAllText(Current.GetWorkConfigFile(), jsonText, Encoding.UTF8);
        }
        //打开工作流
        public void Open(string file)
        {
            if (File.Exists(file))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                var jsonText = File.ReadAllText(file, Encoding.UTF8);
                JObject jo = JObject.Parse(jsonText);
                JArray ctrls = jo["Controls"] as JArray;
                if (ctrls?.Count > 0)
                {
                    this.SuspendLayout();
                    this.workPanel.SuspendLayout();
                    this.workPanel.Controls.Clear();
                    for (int i = 0; i < ctrls.Count; i++)
                    {
                        JObject ctrl = ctrls[i] as JObject;
                        WorkControl workflow = assembly.CreateInstance(ctrl["Type"].ToString(), false, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null, null, null) as WorkControl;
                        var point = new Point(5, 5 + workPanel.Controls.Count * 20);
                        workflow.Location = point;
                        workflow.Name = "editworkflow";
                        workflow.SetIndex(workPanel.Controls.Count + 1);
                        workflow.Size = new Size(400, 20);
                        workflow.Main = this;
                        workflow.Open(ctrl["Params"].ToString());
                        this.workPanel.Controls.Add(workflow);
                    }
                    this.workPanel.ResumeLayout();
                    this.ResumeLayout(false);
                    this.Refresh();
                }
            }
            this.tabPane1.SelectedPageIndex = 0;
        }

        //更新设计（页面切换 设计方法切换时）

        //获取当前工作流的工作路径
        public string GetWorkPath()
        {
            return Current.GetWorkPath();
        }

        public string SparseName()
        {
            return Path.Combine(GetWorkPath(), $"{textBox_name.Text}.sparse");
        }

        public List<SparseWellData> SparseMethod(int index)
        {
            var data = new List<SparseWellData>();
            if (index == 0)
            {
                List<int> ids = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                ids = ids.OrderBy(_ => Guid.NewGuid()).ToList();
                for (int i = 0; i < 10; i++)
                {
                    data.Add(new SparseWellData() { Serial = i + 1, Id = WellIds.Build(ids[i]) });
                }
            }
            else
            {
                //其他方案待实现
            }
            return data;
        }

        //鼠标交互事件---------------------------------------------------------------------------------------------------------
        private void editworkflow_Click(object sender, System.EventArgs e)
        {
            using (WorkSelectForm select = new WorkSelectForm())
            {
                if (select.ShowDialog() == DialogResult.OK)
                {
                    this.SuspendLayout();
                    this.workPanel.SuspendLayout();
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    foreach (var item in select.GetSelects())
                    {
                        WorkControl workflow = assembly.CreateInstance(item.ControlType, false, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null, null, null) as WorkControl;
                        var point = new Point(5, 5 + workPanel.Controls.Count * 20);
                        workflow.Location = point;
                        workflow.Name = "editworkflow";
                        workflow.SetIndex(workPanel.Controls.Count + 1);
                        workflow.Size = new Size(400, 20);
                        workflow.Main = this;
                        this.workPanel.Controls.Add(workflow);
                    }
                    this.workPanel.ResumeLayout();
                    this.ResumeLayout(false);
                    this.Refresh();
                }
            }
        }

        private void checkEdit2_CheckedChanged(object sender, System.EventArgs e)
        {
            checkEdit1.Checked = !checkEdit2.Checked;
            comboBoxEdit_exit.Enabled = checkEdit2.Checked;
            textEdit_new.Enabled = checkEdit1.Checked;
            Current = this.comboBoxEdit_exit.SelectedItem as WorkFlow;
        }

        private void checkEdit1_CheckedChanged(object sender, System.EventArgs e)
        {
            checkEdit2.Checked = !checkEdit1.Checked;
            comboBoxEdit_exit.Enabled = checkEdit2.Checked;
            textEdit_new.Enabled = checkEdit1.Checked;
            Current = null;
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            Run();
        }

        private void simpleButton_savework_Click(object sender, System.EventArgs e)
        {
            //保存当前工作信息
            if (checkEdit1.Checked)
            {
                if (string.IsNullOrEmpty(this.textEdit_new.Text))
                {
                    XtraMessageBox.Show("工作流名称不能为空");
                    return;
                }
                var newwork = new WorkFlow(this.textEdit_new.Text);
                Current = newwork;
                var find = exitworks.Find(_ => string.Equals(Current.Name, _.Name));
                if (find == null)
                {
                    exitworks.Add(Current);
                    WorkFlow.UpdateWorkList(exitworks);
                    var index = this.comboBoxEdit_exit.Properties.Items.Add(Current);
                    checkEdit2.Checked = true;
                    this.comboBoxEdit_exit.SelectedIndex = index;
                }
                else
                {
                    XtraMessageBox.Show("工作流已存在");
                }
            }
            Save();
        }

        private void comboBoxEdit_exit_SelectedIndexChanged(object sender, EventArgs e)
        {
            Current = this.comboBoxEdit_exit.SelectedItem as WorkFlow;
            Open(Current?.GetWorkConfigFile());
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SparseMethod(comboBoxEdit1.SelectedIndex);
        }
    }

    public class SparseWellData
    {
        [DisplayName("序号")]
        public int Serial { get; set; }

        [DisplayName("抽稀井Id")]
        public WellIds Id { get; set; }
    }

    public class WellIds
    {
        public static WellIds Build(params int[] ids)
        {
            WellIds wellid = new WellIds();
            if (ids.Length > 0)
            {
                wellid.ids = ids.ToList();
            }
            return wellid;
        }

        public List<int> ids { get; set; }

        public override string ToString()
        {
            if (ids?.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < ids.Count; i++)
                {
                    sb.Append(ids[i]);
                    sb.Append(',');
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
