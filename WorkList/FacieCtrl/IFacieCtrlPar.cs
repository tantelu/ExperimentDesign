
using ExperimentDesign.General;
using ExperimentDesign.WorkList.Grid;
using System.Collections.Generic;

namespace ExperimentDesign.WorkList.FacieCtrl
{
    public interface IFacieCtrlPar
    {
        string ControlTypeName { get; }

        void Open(string str);

        string Save();

        float[] FacieCtrlRun(Grid3D Grid3D,string workpath, IReadOnlyDictionary<string, object> designVaribles);
    }
}
