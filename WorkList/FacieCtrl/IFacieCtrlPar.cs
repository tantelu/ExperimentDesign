
using ExperimentDesign.WorkList.Grid;
using System.Collections.Generic;

namespace ExperimentDesign.WorkList.FacieCtrl
{
    public interface IFacieCtrlPar
    {
        string TypeName { get; }

        void Open(string str);

        string Save();

        float[] Run(Grid3D Grid3D,string workpath, IReadOnlyDictionary<string, object> designVaribles);
    }
}
