
using System.ComponentModel;

namespace ExperimentDesign.General
{
    public enum VariogramType
    {
        [Description("球状模型")]
        Spherical,
        [Description("指数模型")]
        Exponential,
        [Description("高斯模型")]
        Gaussian,
    }
}
