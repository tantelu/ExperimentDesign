using DevExpress.XtraEditors;
using ExperimentDesign;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList
{
    public partial class GridEditForm : EditorForm
    {
        public GridEditForm()
        {
            InitializeComponent();
        }

        public void InitForm(List<VariableData> param)
        {
            map = new Dictionary<int, UncertainControl>()
            {
                       {1,new UncertainControl(this.spinEdit1,"网格X最小值")  },
                       {2,new UncertainControl(this.spinEdit2,"网格Y最小值") },
                       {3,new UncertainControl(this.spinEdit3,"网格Z最小值") },
                       {4,new UncertainControl(this.spinEdit4,"网格X最大值") },
                       {5,new UncertainControl(this.spinEdit5,"网格Y最大值")},
                       {6,new UncertainControl(this.spinEdit6,"网格Z最大值")},
                       {7,new UncertainControl(this.spinEdit7,"X方向网格大小") },
                       {8,new UncertainControl(this.spinEdit8,"Y方向网格大小")},
                       {9,new UncertainControl(this.spinEdit9,"Z方向网格大小") },
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

        private void simpleButton2_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
