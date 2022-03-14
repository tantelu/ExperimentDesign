using DevExpress.XtraEditors;
using ExperimentDesign.WorkList;
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

        public void InitForm(List<UncertainParam> param)
        {
            map = new Dictionary<int, UncertainControl>()
            {
                       {1,new UncertainControl(this.spinEdit1,"网格X最小值",0)  },
                       {2,new UncertainControl(this.spinEdit2,"网格Y最小值",0) },
                       {3,new UncertainControl(this.spinEdit3,"网格Z最小值",0) },
                       {4,new UncertainControl(this.spinEdit4,"网格X最大值",1000) },
                       {5,new UncertainControl(this.spinEdit5,"网格Y最大值",1000)},
                       {6,new UncertainControl(this.spinEdit6,"网格Z最大值",100)},
                       {7,new UncertainControl(this.spinEdit7,"X方向网格大小",10) },
                       {8,new UncertainControl(this.spinEdit8,"Y方向网格大小",10)},
                       {9,new UncertainControl(this.spinEdit9,"Z方向网格大小",10) },
                   };
            foreach (var par in param)
            {
                UncertainControl ctrl;
                if (map.TryGetValue(par.Index, out ctrl))
                {
                    ctrl.Control.EditValue = par.EditorValue;
                    ctrl.DefaultValue = par.DefaultValue;
                    ctrl.ParamDescription = par.ParDescription;
                }
            }
        }

        public List<UncertainParam> GetUncentainParam()
        {
            List<UncertainParam> res = new List<UncertainParam>();
            foreach (KeyValuePair<int, UncertainControl> item in map)
            {
                res.Add(new UncertainParam() { Index = item.Key, DefaultValue = item.Value.DefaultValue, ParDescription = item.Value.ParamDescription, EditorValue = item.Value.Control.EditValue });
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
        public UncertainControl(BaseEdit ctrl, string paramName, object defaultValue)
        {
            this.Control = ctrl;
            this.ParamDescription = paramName;
            this.DefaultValue = defaultValue;
        }

        public BaseEdit Control { get; set; }

        public string ParamDescription { get; set; }

        public object DefaultValue { get; set; }
    }
}
