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

        public void InitForm(List<UncertainParam> param)
        {
            map = new Dictionary<int, UncertainControl>()
            {
                       {1,new UncertainControl(this.sill,"基台值",1)},
                       {2,new UncertainControl(this.nug,"块金值",0.0001)},
                       {3,new UncertainControl(this.comboBoxEdit1,"变差函数类型","高斯模型")},
                       {4,new UncertainControl(this.majordir,"主变程",500) },
                       { 5,new UncertainControl(this.minordir,"次变程",500)},
                       { 6,new UncertainControl(this.verticaldir,"垂直变程",100)},
                       {7,new UncertainControl(this.majorAzimuth,"变差函数主方向",0)},
                       {8,new UncertainControl(this.majorDip,"变差函数倾角",0)},
                       {9,new UncertainControl(this.comboBoxEdit2,"克里金类型","简单克里金")},
                       {10,new UncertainControl(this.maxdata,"最大条件点数",8)},
                       { 11,new UncertainControl(this.multigrid,"多级网格",0)},
                   };
            foreach (var par in param)
            {
                UncertainControl ctrl;
                if (map.TryGetValue(par.Index, out ctrl))
                {
                    ctrl.Control.EditValue = par.EditorValue;
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
    }
}
