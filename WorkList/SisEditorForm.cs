using System.Collections.Generic;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList
{
    public partial class SisEditorForm : EditorForm
    {
        public SisEditorForm()
        {
            InitializeComponent();
        }

        public void InitForm(List<VariableData> param)
        {
            map = new Dictionary<int, UncertainControl>()
            {
                       {1,new UncertainControl(this.sill,"基台值")},
                       {2,new UncertainControl(this.nug,"块金值")},
                       {3,new UncertainControl(this.comboBoxEdit1,"变差函数类型")},
                       {4,new UncertainControl(this.majordir,"主变程") },
                       { 5,new UncertainControl(this.minordir,"次变程")},
                       { 6,new UncertainControl(this.verticaldir,"垂直变程")},
                       {7,new UncertainControl(this.majorAzimuth,"变差函数主方向")},
                       {8,new UncertainControl(this.majorDip,"变差函数倾角")},
                       {9,new UncertainControl(this.comboBoxEdit2,"克里金类型")},
                       {10,new UncertainControl(this.maxdata,"最大条件点数")},
                       {11,new UncertainControl(this.checkEdit1,"是否使用多级网格搜索")},
                       {12,new UncertainControl(this.multigrid,"多级网格")},
                   };
            foreach (var par in param)
            {
                UncertainControl ctrl;
                if (map.TryGetValue(par.CtrlIndex, out ctrl))
                {
                    if (par.Name.Contains("$"))
                    {
                        ctrl.Control.EditValue = par.Name;
                    }
                    else
                    {
                        ctrl.Control.EditValue = par.BaseValue;
                    }
                    ctrl.Data = par;
                }
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
