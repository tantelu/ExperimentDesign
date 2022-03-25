using System.IO;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Base
{
    public static class GlobalWorkCongfig
    {
        /// <summary>
        /// 工作流的基础路径
        /// </summary>
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

        public static int Seed { get; set; } = 32323;
    }
}
