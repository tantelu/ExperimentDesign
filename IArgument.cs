namespace ExperimentDesign
{
    public interface IArgument
    {
        object GetMin();

        object GetMax();

        object GetLevel(int level);

        string ToString();

        string Save();

        void Open(string json);
    }
}
