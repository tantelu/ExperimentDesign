﻿using System.Collections.Generic;
using System.IO;

namespace ExperimentDesign.WorkList
{
    public class WorkFlow
    {
        public string Name { get; set; }

        public string GetWorkPath()
        {
            var path= Path.Combine(WorkPath.WorkBasePath, Name);
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

        public void Save() { }

        public void Open() { }
        public static void UpdateWorkList(IReadOnlyList<WorkFlow> works) { }
    }
}
