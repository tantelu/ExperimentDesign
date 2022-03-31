using System;
using System.IO;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Base
{
    public static class GlobalWorkCongfig
    {
        private static Random r = new Random();

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

        public static int Seed
        {
            get
            {
                return r.Next(0, 10000000);
            }
        }
    }
}
