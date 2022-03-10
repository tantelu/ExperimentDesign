using System.IO;

namespace ExperimentDesign.WorkList
{
    public class WorkFlow
    {
        public string Name { get; set; }

        public string WorkPath { get; set; } 

        /// <summary>
        /// 读取工作流配置文件（用于保存工作流）
        /// </summary>
        /// <returns></returns>
        public string GetWorkConfigFile()
        {
            return Path.Combine(WorkPath, $"{Name}.json");
        }
    }
}
