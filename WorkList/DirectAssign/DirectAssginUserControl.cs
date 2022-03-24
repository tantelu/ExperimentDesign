using ExperimentDesign.General;
using ExperimentDesign.WorkList.FacieCtrl;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.DirectAssign
{
    public partial class DirectAssginUserControl : UserControl, IFacieCtrlUserControl
    {
        public DirectAssginUserControl()
        {
            InitializeComponent();
        }

        public UserControl Control => this;

        public DirectAssignPar GetAssignPar()
        {
            DirectAssignPar par = new DirectAssignPar();
            par.PropertyValue = Design<double>.GeneralDesign(this.textEdit1.Tag, this.textEdit1.Text);
            return par;
        }

        public void SetAssignPar(DirectAssignPar par)
        {
            if (par != null)
            {
                this.textEdit1.Tag = par.PropertyValue;
                this.textEdit1.Text = par.PropertyValue.IsDesign ? par.PropertyValue.DesignName : par.PropertyValue.Value.ToString();
            }
        }

        public void SetPar(IFacieCtrlPar par)
        {
            if (par is DirectAssignPar apar)
            {
                SetAssignPar(apar);
            }
        }
    }
}
