
namespace ExperimentDesign.WorkList.Base
{
    public interface IWork
    {
        void Delete(WorkControl control);

        void Up(WorkControl control);

        void Down(WorkControl control);

        string GetWorkPath();
    }
}
