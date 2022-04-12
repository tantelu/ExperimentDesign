using System.Collections.Generic;

namespace ExperimentDesign.Uncertainty
{
    public interface IArgument
    {
        object GetMin();

        object GetMax();

        object GetLevel(int level);

        IReadOnlyList<object> MonteCarloSample(int sampletimes,int seed);

        IReadOnlyList<object> EqualSpaceSample(int sampletimes);

        object GetBase();

        string ToString();

        string Save();

        void Open(string json);
    }
}
