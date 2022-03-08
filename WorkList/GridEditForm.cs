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
                       {1,new UncertainControl(this.spinEdit1,"Grid_Xmin",0)  },
                       {2,new UncertainControl(this.spinEdit2,"Grid_Ymin",0) },
                       {3,new UncertainControl(this.spinEdit3,"Grid_Zmin",0) },
                       {4,new UncertainControl(this.spinEdit4,"Grid_Xmax",1000) },
                       { 5,new UncertainControl(this.spinEdit5,"Grid_Ymax",1000)},
                       { 6,new UncertainControl(this.spinEdit6,"Grid_Zmax",100)},
                       {7,new UncertainControl(this.spinEdit7,"Grid_Width",10) },
                       {8,new UncertainControl(this.spinEdit8,"Grid_Height",10)},
                       {9,new UncertainControl(this.spinEdit9,"Grid_Thick",10) },
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
                res.Add(new UncertainParam() { Index = item.Key, DefaultValue = item.Value.DefaultValue, Name = item.Value.ParamName, EditorValue = item.Value.Control.EditValue });
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
            this.ParamName = paramName;
            this.DefaultValue = defaultValue;
        }

        public BaseEdit Control { get; set; }

        public string ParamName { get; set; }

        public object DefaultValue { get; set; }
    }
}
