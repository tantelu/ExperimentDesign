using DevExpress.XtraEditors;
using ExperimentDesign.DesignPanel;
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
using System.Threading;
using System.Windows.Forms;
using ExperimentDesign.WorkList.Base;
using ExperimentDesign.InfoForm;
using ExperimentDesign.Uncertainty;
using ExperimentDesign.General;

namespace ExperimentDesign
{
    public partial class UncertaintyForm : XtraForm, IUpdateDesignTimes, IWork
    {
        private List<WorkFlow> exitworks = new List<WorkFlow>();

        private WorkFlow Current { get; set; }

        public UncertaintyForm()
        {
            InitializeComponent();
            var folders = Directory.GetDirectories(GlobalWorkCongfig.WorkBasePath);
            var exits = folders?.Select(_ => new WorkFlow(Path.GetFileNameWithoutExtension(_))).ToList();
            if (exits?.Count > 0)
            {
                comboBoxEdit_exit.Properties.Items.AddRange(exits);
            }
        }

        //运行工作流
        public void Run()
        {
            if (Current != null)
            {
                DirectoryInfo info = new DirectoryInfo(((IWork)this).GetWorkPath());
                if (info.GetDirectories().Length > 0)
                {
                    var dialog = XtraMessageBox.Show("当前工作流已经运行过,运行会覆盖原有结果,是否继续？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialog == DialogResult.Yes)
                    {
                        foreach (var item in info.GetDirectories())
                        {
                            item.Delete(true);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                int maxwait = 60000;
                var workControls = this.workPanel.Controls;
                var designTable = GetDesignDataTable();
                if (designTable == null)
                {
                    return;
                }
                for (int i = 0; i < designTable.Count; i++)
                {
                    foreach (var item in workControls)
                    {
                        if (item is WorkControl ctrl)
                        {
                            ctrl.Run(i + 1, designTable[i]);
                            int curwait = 0;
                            while (!ctrl.GetRunState(i + 1))
                            {
                                Thread.Sleep(1000);
                                curwait += 1000;
                                if (curwait > maxwait)
                                {
                                    var dia = XtraMessageBox.Show($"单一工作流运行时间超出{maxwait / 1000.0}S,是否继续等待？", "提示", MessageBoxButtons.YesNo);
                                    if (dia == DialogResult.Yes)
                                    {
                                        maxwait *= 2;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            }
                        }
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
            writer.WritePropertyName("DesignMethod");
            writer.WriteValue(this.designMethod.SelectedIndex);
            writer.WritePropertyName("DesignTimes");
            writer.WriteValue((int)this.designTimes.Value);
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
            var datas = this.gridControl1.DataSource as List<VariableData>;
            if (datas?.Count > 0)
            {
                foreach (var item in datas)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("Type");
                    writer.WriteValue(item.GetType().FullName);
                    writer.WritePropertyName("Data");
                    writer.WriteValue(item.Save());
                    writer.WriteEndObject();
                }
            }
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
                var times = jo.GetValue("DesignTimes");
                if (times != null)
                {
                    this.designTimes.Value = (int)times;
                }
                var temp = jo.GetValue("DesignMethod");
                if (temp != null)
                {
                    int index = (int)temp;
                    if (index >= 0 || index < this.designMethod.Properties.Items.Count)
                    {
                        this.designMethod.SelectedIndex = index;
                    }
                }
                JArray ctrls = jo["Controls"] as JArray;
                JToken t = jo["UncertainParam"];
                if (t is JArray uncertains)
                {
                    if (uncertains?.Count > 0)
                    {
                        List<VariableData> datas = new List<VariableData>();
                        for (int i = 0; i < uncertains.Count; i++)
                        {
                            JObject uncertain = uncertains[i] as JObject;
                            var obj = assembly.CreateInstance(uncertain["Type"].ToString(), false, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null, null, null) as VariableData;
                            var data = uncertain["Data"].ToString();
                            obj.Open(data);
                            datas.Add(obj);
                        }
                        this.gridControl1.DataSource = datas;
                    }
                }
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
        private void UpdateDesign()
        {
            var datas = this.gridControl1.DataSource as List<VariableData>;
            if (datas?.Count > 0)
            {
                var tabel = GetDesignTable(designMethod.SelectedIndex, datas);
                if (tabel != null)
                {
                    this.designTimes.Value = tabel.GetTestCount();
                }
                else
                {
                    this.designTimes.Value = 0;
                    XtraMessageBox.Show($"参数过少或者设置错误,无法进行{designMethod.SelectedText}设计");
                }
            }
        }
        //获取设计表
        private Table GetDesignTable(int methodIndex, List<VariableData> data)
        {
            int factornum = data.Count;
            this.panelControl_design.Controls.Clear();
            if (methodIndex == 0)
            {
                this.designTimes.ReadOnly = false;
                MonteCarloTable res = new MonteCarloTable(Convert.ToInt32(this.designTimes.Value));
                MonteCarloPanel panel = new MonteCarloPanel();
                panel.DesignTable = res;
                panel.Data = data;
                panel.Dock = DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else if (methodIndex == 1)
            {
                this.designTimes.ReadOnly = false;
                EqualSpaceTable res = new EqualSpaceTable(Convert.ToInt32(this.designTimes.Value));
                EqualSpacePanel panel = new EqualSpacePanel();
                panel.DesignTable = res;
                panel.Data = data;
                panel.Dock = DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;

            }
            if (methodIndex == 2)
            {
                this.designTimes.ReadOnly = true;
                var res = DesignAlgorithm.GenerateBoxBehnken(factornum);
                BBDesignPanel panel = new BBDesignPanel();
                panel.DesignTable = res;
                panel.Data = data;
                panel.Dock = DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else if (methodIndex == 3)
            {
                this.designTimes.ReadOnly = true;
                var res = DesignAlgorithm.GenerateComposite(factornum, CenteralCompositeType.FaceCentered);
                CCDesignPanel panel = new CCDesignPanel();
                panel.DesignTable = res;
                panel.Data = data;
                panel.Dock = DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else if (methodIndex == 4)
            {
                this.designTimes.ReadOnly = true;
                var res = DesignAlgorithm.GeneratePlackettBurman(factornum);
                PBDesignPanel panel = new PBDesignPanel();
                panel.DesignTimes = this;
                panel.DesignTable = res;
                panel.Data = data;
                panel.Dock = DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else if (methodIndex == 5)
            {
                this.designTimes.ReadOnly = true;
                var res = DesignAlgorithm.GenerateOrthGuide(factornum, factornum - 1);
                OrthDesignPanel panel = new OrthDesignPanel();
                panel.DesignTable = res;
                panel.Data = data;
                panel.Dock = DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else
            {
                return null;
            }
        }

        public void UpdateDesignTimes(int times)
        {
            this.designTimes.Enabled = false;
            this.designTimes.Value = times;
        }

        public DataTable GetSaveDesignTable()
        {
            var datas = this.gridControl1.DataSource as List<VariableData>;
            if (this.panelControl_design.Controls?.Count > 0 && datas?.Count > 0)
            {
                var property = this.panelControl_design.Controls[0].GetType().GetProperties().Where(_ => _.GetMethod.ReturnType.IsSubclassOf(typeof(Table))).FirstOrDefault();
                if (property != null)
                {
                    var table = property.GetValue(this.panelControl_design.Controls[0]) as Table;
                    return table.ToSaveDataTable(datas);
                }
            }
            return null;
        }

        //获取当前工作流的工作路径
        public string GetWorkPath()
        {
            return Current.GetWorkPath();
        }

        //获取设计表
        public IReadOnlyList<IReadOnlyDictionary<string, object>> GetDesignDataTable()
        {
            if (this.panelControl_design.Controls.Count > 0)
            {
                var property = this.panelControl_design.Controls[0].GetType().GetProperty("DesignTable");
                var designtable = property.GetValue(this.panelControl_design.Controls[0]) as Table;
                bool exitNaN;
                var res = designtable.ToDesignList(this.gridControl1.DataSource as List<VariableData>, out exitNaN);
                if (exitNaN)
                {
                    XtraMessageBox.Show("设计表存在NaN值");
                    return null;
                }
                else
                {
                    return res;
                }
            }
            else
            {
                XtraMessageBox.Show("不存在设计表");
                return null;
            }
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
            var workControls = this.workPanel.Controls;
            var olds = this.gridControl1.DataSource as List<VariableData>;
            List<VariableData> designDatas = new List<VariableData>();
            HashSet<string> distinct = new HashSet<string>();
            foreach (var item in workControls)
            {
                if (item is WorkControl ctrl)
                {
                    var @params = ctrl.GetUncentainParam();
                    if (@params?.Count > 0)
                    {
                        foreach (var par in @params)
                        {
                            if (par.Name.Contains("$"))
                            {
                                var find = olds?.Find(_ => string.Equals(_.Id, par.Id));
                                if (find != null)
                                {
                                    par.CopyFromOther(find);
                                }
                                designDatas.Add(par);
                                if (!distinct.Add(par.Name))
                                {
                                    XtraMessageBox.Show("不确定参数不能重名,请修改");
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            this.gridControl1.DataSource = designDatas;
            this.gridControl1.RefreshDataSource();
            UpdateDesign();
            if (e.OldPage == this.tabNavigationPage1 && e.Page == this.tabNavigationPage2)
            {

            }
            else if (e.Page == this.tabNavigationPage3)
            {

            }
            else
            {

            }
        }

        private void designMethod_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateDesign();
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

        private void simpleButton_exporttable_Click(object sender, EventArgs e)
        {
            using (VolumnSelectForm form = new VolumnSelectForm())
            {
                form.UncertaintyForm = this;
                form.ShowDialog();
            }
        }

        private void designTimes_ValueChanged(object sender, EventArgs e)
        {
            if (!designTimes.ReadOnly)
            {
                UpdateDesign();
            }
        }
    }
}
