
namespace ExperimentDesign.WorkList.Base
{
    public interface IWork
    {
        void Delete(WorkControl control);

        string GetWorkPath();
    }
}
