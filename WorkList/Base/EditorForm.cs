using DevExpress.XtraEditors;
using ExperimentDesign.Uncertainty;
using ExperimentDesign.WorkList.Grid;
using System.Collections.Generic;

namespace ExperimentDesign.WorkList.Base
{
    public class EditorForm : XtraForm
    {
        protected Dictionary<int, UncertainControl> map;

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
                        if (item.Value.Control is CheckEdit c)
                        {
                            item.Value.Data.BaseValue = c.Checked;
                        }
                        else
                        {
                            item.Value.Data.BaseValue = item.Value.Control.EditValue;
                        }

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
                        BaseValue = item.Value.Control.EditValue.ToString().Contains("$") ? item.Value.Control.Tag : item.Value.Control.EditValue,
                    });
                }
            }
            return res;
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
