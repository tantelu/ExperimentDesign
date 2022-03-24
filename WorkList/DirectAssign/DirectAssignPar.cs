using ExperimentDesign.General;
using ExperimentDesign.WorkList.FacieCtrl;
using ExperimentDesign.WorkList.Grid;
using System;
using System.Collections.Generic;

namespace ExperimentDesign.WorkList.DirectAssign
{
    public class DirectAssignPar : IFacieCtrlPar
    {
        public Design<double> PropertyValue { get; set; }

        public string ControlTypeName => typeof(DirectAssginUserControl).FullName;

        public void Open(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                PropertyValue = new Design<double>();
                PropertyValue.Open(str);
            }
        }

        public float[] FacieCtrlRun(Grid3D Grid3D, string workpath, IReadOnlyDictionary<string, object> designVaribles)
        {
            var xcount = (int)((Grid3D.Xmax - Grid3D.Xmin) / Grid3D.Xsize);
            var ycount = (int)((Grid3D.Xmax - Grid3D.Xmin) / Grid3D.Xsize);
            var zcount = (int)((Grid3D.Xmax - Grid3D.Xmin) / Grid3D.Xsize);
            float[] res = new float[xcount * ycount * zcount];
            float value = float.NaN;
            if (PropertyValue.IsDesign)
            {
                object obj = null;
                if (designVaribles.TryGetValue(PropertyValue.DesignName, out obj))
                {
                    value = Convert.ToSingle(obj);
                }
            }
            else
            {
                value = (float)PropertyValue.Value;
            }
            if (value != float.NaN)
            {
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = value;
                }
            }
            return res;
        }

        public string Save()
        {
            if (PropertyValue != null)
            {
                return PropertyValue.Save();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
