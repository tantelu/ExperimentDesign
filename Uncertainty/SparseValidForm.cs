﻿using DevExpress.XtraEditors;
using ExperimentDesign.GridPopForm;
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
using ExperimentDesign.Uncertainty;
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
            var data = new List<SparseWellData>();
            List<int> ids = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            ids = ids.OrderBy(_ => Guid.NewGuid()).ToList();
            for (int i = 0; i < 10; i++)
            {
                data.Add(new SparseWellData() { Serial = i + 1, Id = ids[i] });
            }
            this.gridControl1.DataSource = data;
        }

        //运行工作流
        public void Run()
        {
            //if (Current != null)
            //{
            //    DirectoryInfo info = new DirectoryInfo(((IWork)this).GetWorkPath());
            //    if (info.GetDirectories().Length > 0)
            //    {
            //        var dialog = XtraMessageBox.Show("当前工作流已经运行过,运行会覆盖原有结果,是否继续？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //        if (dialog == DialogResult.Yes)
            //        {
            //            foreach (var item in info.GetDirectories())
            //            {
            //                item.Delete(true);
            //            }
            //        }
            //        else
            //        {
            //            return;
            //        }
            //    }
            //    int maxwait = 60000;
            //    var workControls = this.workPanel.Controls;
            //    var designTable = GetDesignDataTable();
            //    if (designTable == null)
            //    {
            //        return;
            //    }
            //    for (int i = 0; i < designTable.Count; i++)
            //    {
            //        foreach (var item in workControls)
            //        {
            //            if (item is WorkControl ctrl)
            //            {
            //                ctrl.Run(i + 1, designTable[i]);
            //                int curwait = 0;
            //                while (!ctrl.GetRunState(i + 1))
            //                {
            //                    Thread.Sleep(1000);
            //                    curwait += 1000;
            //                    if (curwait > maxwait)
            //                    {
            //                        var dia = XtraMessageBox.Show($"单一工作流运行时间超出{maxwait / 1000.0}S,是否继续等待？", "提示", MessageBoxButtons.YesNo);
            //                        if (dia == DialogResult.Yes)
            //                        {
            //                            maxwait *= 2;
            //                        }
            //                        else
            //                        {
            //                            return;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    XtraMessageBox.Show($"工作流执行完成");
            //}
            //else
            //{
            //    XtraMessageBox.Show("请先设置当前工作流");
            //}
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

        private void tabPane1_SelectedPageChanged(object sender, DevExpress.XtraBars.Navigation.SelectedPageChangedEventArgs e)
        {

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

        private void gridView1_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.Equals(this.gridView1.FocusedColumn.FieldName, nameof(VariableData.Name)) ||
                string.Equals(this.gridView1.FocusedColumn.FieldName, nameof(VariableData.ParDescription)))
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (string.Equals(e.Column.FieldName, nameof(VariableData.Distribution)))
            {
                e.RepositoryItem = paramDistributed;
            }
            if (string.Equals(e.Column.FieldName, nameof(VariableData.Arguments)))
            {
                e.RepositoryItem = this.argumentRepositoryItem;
            }
        }

        private void argumentRepositoryItem_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var distribution = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, nameof(VariableData.Distribution));
            if (string.Equals(distribution, "均匀分布"))
            {
                using (MinMaxPopForm form = new MinMaxPopForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        this.gridView1.SetRowCellValue(this.gridView1.FocusedRowHandle, nameof(VariableData.Arguments), form.Argument);
                    }
                }
            }
            else if (string.Equals(distribution, "集合"))
            {
                using (GridListPopForm form = new GridListPopForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        this.gridView1.SetRowCellValue(this.gridView1.FocusedRowHandle, nameof(VariableData.Arguments), form.Argument);
                    }
                }
            }
            else if (string.Equals(distribution, "正态分布"))
            {
                using (NormalPopForm form = new NormalPopForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        this.gridView1.SetRowCellValue(this.gridView1.FocusedRowHandle, nameof(VariableData.Arguments), form.Argument);
                    }
                }
            }
            else if (string.Equals(distribution, "三角分布"))
            {
                using (TrianglePopForm form = new TrianglePopForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        this.gridView1.SetRowCellValue(this.gridView1.FocusedRowHandle, nameof(VariableData.Arguments), form.Argument);
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("请先选择参数分布");
            }
        }

        private void paramDistributed_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.gridView1.SetRowCellValue(this.gridView1.FocusedRowHandle, nameof(VariableData.Arguments), null);
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
    }

    class SparseWellData
    {
        [DisplayName("序号")]
        public int Serial { get; set; }

        [DisplayName("抽稀井Id")]
        public int Id { get; set; }
    }
}
