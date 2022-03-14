using DevExpress.XtraEditors;
using ExperimentDesign;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WorkList.ExperimentDesign
{
    public partial class GridEditForm : XtraForm
    {
        private Dictionary<int, UncertainControl> map;

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

        private void simpleButton2_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }

    public class UncertainControl
    {
        private string paramDescription;

        public UncertainControl(BaseEdit ctrl, string paramDescription)
        {
            this.Control = ctrl;
            this.paramDescription = paramDescription;
        }

        public BaseEdit Control { get; set; }

        public VariableData Data { get; set; }

        public string ParamDescription
        {
            get
            {
                if (Data != null)
                {
                    return Data.ParDescription;
                }
                else
                {
                    return paramDescription;
                }
            }
        }
    }
}
