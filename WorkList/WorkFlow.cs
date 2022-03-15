using System.Collections.Generic;
using System.IO;

namespace ExperimentDesign.WorkList
{
    public class WorkFlow
    {
        public WorkFlow(string name)
        {
            Name = name;
        }

        public string Name { get;private set; }

        public string GetWorkPath()
        {
            var path= Path.Combine(GlobalWorkCongfig.WorkBasePath, Name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// 读取工作流配置文件（用于保存工作流）
        /// </summary>
        /// <returns></returns>
        public string GetWorkConfigFile()
        {
            return Path.Combine(GetWorkPath(), $"{Name}.json");
        }

        public override string ToString()
        {
            return Name;
        }

        public static void UpdateWorkList(IReadOnlyList<WorkFlow> works) { }
    }
}
