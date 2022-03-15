
namespace ExperimentDesign.WorkList
{
    public interface IWork
    {
        void Delete(WorkControl control);

        string GetWorkPath();
    }
}
