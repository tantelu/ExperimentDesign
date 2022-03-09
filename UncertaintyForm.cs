using DevExpress.XtraEditors;
using ExperimentDesign.DesignPanel;
using ExperimentDesign.GridPopForm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WorkList.ExperimentDesign;

namespace ExperimentDesign
{
    public partial class UncertaintyForm : XtraForm, IUpdateDesignTimes, IDeleteWorkControl
    {
        public UncertaintyForm()
        {
            InitializeComponent();
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
                        workflow.Layout = this;
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
            List<VariableData> datas = new List<VariableData>();
            foreach (var item in workControls)
            {
                if (item is WorkControl ctrl)
                {
                    var @params = ctrl.GetUncentainParam();
                    if (@params?.Count > 0)
                    {
                        foreach (var par in @params)
                        {
                            if (par.EditorValue != null && par.EditorValue.ToString().Contains("$"))
                            {
                                VariableData data = new VariableData();
                                data.Name = par.EditorValue.ToString();
                                data.ParName = par.Name;
                                data.BaseValue = par.DefaultValue.ToString();
                                datas.Add(data);
                            }
                        }
                    }
                }
            }
            this.gridControl1.DataSource = datas;
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

        private void UpdateDesign()
        {
            var datas = this.gridControl1.DataSource as List<VariableData>;
            if (datas?.Count > 0)
            {
                var tabel = GetDesignTable(designMethod.SelectedIndex, datas.Count);
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

        private Table GetDesignTable(int methodIndex, int factornum)
        {
            this.panelControl_design.Controls.Clear();
            if (methodIndex == 0)
            {
                var res = DesignAlgorithm.GenerateBoxBehnken(factornum);
                BBDesignPanel panel = new BBDesignPanel();
                panel.DesignTable = res;
                panel.Dock = System.Windows.Forms.DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else if (methodIndex == 1)
            {
                var res = DesignAlgorithm.GenerateComposite(factornum, CenteralCompositeType.FaceCentered);
                CCDesignPanel panel = new CCDesignPanel();
                panel.DesignTable = res;
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
                panel.Dock = System.Windows.Forms.DockStyle.Fill;
                this.panelControl_design.Controls.Add(panel);
                return res;
            }
            else if (methodIndex == 3)
            {
                var res = DesignAlgorithm.GenerateOrthGuide(factornum, factornum - 1);
                OrthDesignPanel panel = new OrthDesignPanel();
                panel.DesignTable = res;
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
        }

        private void checkEdit1_CheckedChanged(object sender, System.EventArgs e)
        {
            checkEdit2.Checked = !checkEdit1.Checked;
            comboBoxEdit_exit.Enabled = checkEdit2.Checked;
            textEdit_new.Enabled = checkEdit1.Checked;
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
                    form.ShowDialog();
                }
            }
            else if (string.Equals(distribution, "集合"))
            {
                using (GridListPopForm form = new GridListPopForm())
                {
                    form.ShowDialog();
                }
            }
        }
    }

    public class VariableData
    {
        [DisplayName("变量名")]
        public string Name { get; set; }

        [DisplayName("参数名")]
        public string ParName { get; set; }

        [DisplayName("默认值")]
        public string BaseValue { get; set; }

        [DisplayName("分布")]
        public string Distribution { get; set; }

        [DisplayName("参数")]
        public string Arguments
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (Values?.Count > 0)
                {
                    foreach (var item in Values)
                    {
                        sb.Append(item);
                        sb.Append(";");
                    }
                }
                return sb.ToString();
            }
        }

        [Browsable(false)]
        public List<string> Values { get; set; }
    }
}
