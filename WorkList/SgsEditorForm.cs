using DevExpress.XtraEditors;
using System.Collections.Generic;
using WorkList.ExperimentDesign;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList
{
    public partial class SgsEditorForm : XtraForm
    {
        private Dictionary<int, UncertainControl> map;

        public SgsEditorForm()
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
                       { 11,new UncertainControl(this.multigrid,"多级网格")},
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

        public List<VariableData> GetUncentainParam()
        {
            List<VariableData> res = new List<VariableData>();
            foreach (KeyValuePair<int, UncertainControl> item in map)
            {
                if (item.Value.Data != null)
                {
                    var name = item.Value.Control.EditValue.ToString();
                    item.Value.Data.Name = name;
                    if (!name.Contains("$"))
                    {
                        item.Value.Data.BaseValue = item.Value.Control.EditValue;
                    }
                    res.Add(item.Value.Data);
                }
                else
                {
                    res.Add(new VariableData()
                    {
                        CtrlIndex = item.Key,
                        WorkControlTypeName = nameof(GridEditWorkControl),
                        Name = item.Value.Control.EditValue.ToString(),
                        ParDescription = item.Value.ParamDescription,
                        BaseValue = Name.Contains("$") ? item.Value.Control.Tag : item.Value.Control.EditValue,
                    });
                }
            }
            return res;
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
