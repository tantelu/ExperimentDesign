using System.IO;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList
{
    public static class WorkPath
    {
        public static string WorkBasePath
        {
            get
            {
                var path = Path.Combine(Application.StartupPath, "WorkFlowList");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
    }
}
