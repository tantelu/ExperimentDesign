using System.Windows.Forms;

namespace ExperimentDesign.WorkList.FacieCtrl
{
    public interface IFacieCtrlUserControl
    {
        void SetPar(IFacieCtrlPar par);

        UserControl Control { get; }
    }
}
