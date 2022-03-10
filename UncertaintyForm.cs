using DevExpress.XtraEditors;
using ExperimentDesign.DesignPanel;
using ExperimentDesign.GridPopForm;
using ExperimentDesign.WorkList;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WorkList.ExperimentDesign;

namespace ExperimentDesign
{
    public partial class UncertaintyForm : XtraForm, IUpdateDesignTimes, IWork
    {
        private List<WorkFlow> exitworks = new List<WorkFlow>();
        private List<VariableData> olddatas = new List<VariableData>();

        private WorkFlow Current { get; set; }

        public UncertaintyForm()
        {
            InitializeComponent();
            var path = Path.Combine(WorkPath.WorkBasePath, "WorkFlowList.json");
            if (File.Exists(path))
            {
                var josn = File.ReadAllText(path);
                if (!string.IsNullOrEmpty(josn))
                {
                    exitworks = JsonConvert.DeserializeObject<List<WorkFlow>>(josn);
                    if (exitworks.Count > 0)
                    {
                        comboBoxEdit_exit.Properties.Items.AddRange(exitworks);
                    }
                }
            }
        }

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
                        WorkControl workflow = assembly.CreateInstance(item.ControlType) as WorkControl;
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
            List<VariableData> newdatas = new List<VariableData>();
            foreach (var item in workControls)
            {
                if (item is WorkControl ctrl)
                {
                    var @params = ctrl.GetUncentainParam();
                    if (@params?.Count > 0)
                    {
                        foreach (var par in @params)
                        {
                            var data = olddatas.Find(_ => string.Equals(_.Name, par.EditorValue));
                            if (data != null)
                            {
                                data.Name = par.EditorValue.ToString();
                                data.ParDescription = par.ParDescription;
                                data.BaseValue = par.DefaultValue.ToString();
                                olddatas.Add(data);
                                newdatas.Add(data);
                            }
                            else
                            {
                                if (par.EditorValue != null && par.EditorValue.ToString().Contains("$"))
                                {
                                    if (!newdatas.Exists(_ => string.Equals(_.Name, par.EditorValue.ToString())))
                                    {
                                        VariableData newdata = new VariableData();
                                        newdata.Name = par.EditorValue.ToString();
                                        newdata.ParDescription = par.ParDescription;
                                        newdata.BaseValue = par.DefaultValue.ToString();
                                        newdatas.Add(newdata);
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("存在重名参数,请修改");
                                        newdatas.Clear();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            this.gridControl1.DataSource = newdatas;
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
            olddatas = newdatas;
        }

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
                    XtraMessageBox.Show($"参数错误,无法进行{designMethod.SelectedText}实验设计");
                }
            }
        }

        private Table GetDesignTable(int methodIndex, List<VariableData> data)
        {
            int factornum = data.Count;
            this.panelControl_design.Controls.Clear();
            if (methodIndex == 0)
            {
                var res = DesignAlgorithm.GenerateBoxBehnken(factornum);
                BBDesignPanel panel = new BBDesignPanel();
                panel.DesignTable = res;
                panel.Data = data;
                panel.Dock = System.Windows.Forms.DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else if (methodIndex == 1)
            {
                var res = DesignAlgorithm.GenerateComposite(factornum, CenteralCompositeType.FaceCentered);
                CCDesignPanel panel = new CCDesignPanel();
                panel.DesignTable = res;
                panel.Data = data;
                panel.Dock = System.Windows.Forms.DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else if (methodIndex == 2)
            {
                var res = DesignAlgorithm.GeneratePlackettBurman(factornum);
                PBDesignPanel panel = new PBDesignPanel();
                panel.DesignTimes = this;
                panel.DesignTable = res;
                panel.Data = data;
                panel.Dock = System.Windows.Forms.DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else if (methodIndex == 3)
            {
                var res = DesignAlgorithm.GenerateOrthGuide(factornum, factornum - 1);
                OrthDesignPanel panel = new OrthDesignPanel();
                panel.DesignTable = res;
                panel.Data = data;
                panel.Dock = System.Windows.Forms.DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else
            {
                return null;
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

        public void UpdateDesignTimes(int times)
        {
            this.designTimes.Enabled = false;
            this.designTimes.Value = times;
        }

        public void Delete(WorkControl control)
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

        private void gridView1_ShownEditor(object sender, System.EventArgs e)
        {

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
            else
            {
                XtraMessageBox.Show("请先选择参数分布");
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            Run();
        }

        public void Run()
        {
            if (Current != null)
            {
                int maxwait = 60000;
                var workControls = this.workPanel.Controls;
                foreach (var item in workControls)
                {
                    if (item is WorkControl ctrl)
                    {
                        ctrl.Run(Current.GetWorkPath());
                        int curwait = 0;
                        while (!ctrl.GetRunState())
                        {
                            Thread.Sleep(5000);
                            curwait += 5000;
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
            else
            {
                XtraMessageBox.Show("请先设置当前工作流");
            }
        }

        private void simpleButton_savework_Click(object sender, System.EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                if (string.IsNullOrEmpty(this.textEdit_new.Text))
                {
                    XtraMessageBox.Show("工作流名称不能为空");
                    return;
                }
                var newwork = new WorkFlow();
                newwork.Name = this.textEdit_new.Text;
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
            Current.Save();
        }

        private void comboBoxEdit_exit_SelectedIndexChanged(object sender, EventArgs e)
        {
            Current = this.comboBoxEdit_exit.SelectedItem as WorkFlow;
        }
    }

    public class VariableData
    {
        [DisplayName("设计参数名")]
        ///对于参数输入界面的 EditorValue 如果其包含$  就是需要设计的参数
        public string Name { get; set; }

        [DisplayName("参数描述")]
        public string ParDescription { get; set; }

        [DisplayName("默认值")]
        public string BaseValue { get; set; }

        [DisplayName("分布")]
        public string Distribution { get; set; }

        [DisplayName("参数")]
        public IArgument Arguments { get; set; }
    }
}
