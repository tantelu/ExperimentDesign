using DevExpress.XtraEditors;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace ExperimentDesign
{
    public partial class UncertaintyForm : XtraForm
    {
        public UncertaintyForm()
        {
            InitializeComponent();
        }

        private void editworkflow_Click(object sender, System.EventArgs e)
        {
            var workflow = new GridEditWorkControl();
            var point = new Point(5, 5 + workPanel.Controls.Count * 20);
            workflow.Location = point;
            workflow.Name = "editworkflow";
            workflow.SetIndex(workPanel.Controls.Count + 1);
            workflow.Size = new Size(400, 20);
            this.SuspendLayout();
            this.workPanel.SuspendLayout();
            this.workPanel.Controls.Add(workflow);
            //var layoutitem = this.layoutControlGroup2.Items.AddItem();
            //((System.ComponentModel.ISupportInitialize)(layoutitem)).BeginInit();
            this.workPanel.ResumeLayout();
            this.ResumeLayout(false);
        }

        private void tabPane1_SelectedPageChanged(object sender, DevExpress.XtraBars.Navigation.SelectedPageChangedEventArgs e)
        {
            if (e.OldPage == this.tabNavigationPage1 && e.Page == this.tabNavigationPage2)
            {
                var workControls = this.workPanel.Controls;
                List<VariableData> datas = new List<VariableData>();
                foreach (var item in workControls)
                {
                    if (item is WorkControl ctrl)
                    {
                        var strs = ctrl.GetUncentainParam();
                        if (strs?.Count > 0)
                        {
                            foreach (var str in strs)
                            {
                                VariableData data = new VariableData();
                                data.Name = str;
                                datas.Add(data);
                            }
                        }
                    }
                }
                this.gridControl1.DataSource = datas;
                this.gridControl1.RefreshDataSource();
            }
            else if (e.Page == this.tabNavigationPage3)
            {
                UpdateDesign();
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
            if (methodIndex == 0)
            {
                return DesignAlgorithm.GenerateBoxBehnken(factornum);
            }
            else if (methodIndex == 1)
            {
                return DesignAlgorithm.GenerateComposite(factornum, CenteralCompositeType.FaceCentered);
            }
            else if (methodIndex == 2)
            {
                return DesignAlgorithm.GeneratePlackettBurman(factornum);
            }
            else if (methodIndex == 3)
            {
                return DesignAlgorithm.GenerateOrthGuide(factornum, factornum - 1);
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
    }

    public class VariableData
    {
        [DisplayName("变量名")]
        public string Name { get; set; }

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
