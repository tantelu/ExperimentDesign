using DevExpress.XtraEditors;
using ExperimentDesign.General;
using ExperimentDesign.Uncertainty;
using ExperimentDesign.WorkList.Base;
using MPS.Condition;
using MPS.Grid;
using MPS.Mathf;
using MPS.Snesim;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.MPS
{
    public class SensimWorkControl : WorkControl
    {
        private SnesimPar par;

        public SensimWorkControl()
        {

        }

        protected override string WorkName => "Snesim模拟";

        protected override Bitmap Picture => global::ExperimentDesign.Properties.Resources.Sgs;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            string gridfile = Path.Combine(Main.GetWorkPath(), $"{index}", $"{nameof(Grid3D)}.json");
            if (!File.Exists(gridfile))
            {
                XtraMessageBox.Show("未找到工区网格定义文件,无法执行序贯高斯模拟");
                return;
            }
            Grid3D grid = new Grid3D();
            grid.Open(gridfile);

            Random R = new Random();
            int MinReplicate = 3;
            TI ti = new TI(par.TIFile);
            var cpdf = ti.Cpdf;

            DRealization RE = new DRealization(grid.Xsize, grid.Ysize, grid.Zsize, grid.Xmin, grid.Ymin, grid.Zmin,
                grid.Xcount, grid.Ycount, grid.Zcount);

            WellFile<byte> wellfile = new WellFile<byte>(par.ConditionFile);
            RE.InitCondition(wellfile, 1);
            RotMat mat = new RotMat(0, 0, 0, 1, 1, 1);

            SearchNeighbor ellipse = new SearchNeighbor(3, 3, 3, mat, 1);
            StreeNode Root = ti.CreatTree(ellipse);
            RE.SetPathLevelWithTree(1, ellipse);
            for (int pathi = 0; pathi < RE.R_Path.Length; pathi++)
            {
                int z = RE.R_Path[pathi] / (RE.Icount * RE.Jcount);
                int y = (RE.R_Path[pathi] - z * (RE.Icount * RE.Jcount)) / RE.Icount;
                int x = RE.R_Path[pathi] - z * (RE.Icount * RE.Jcount) - y * RE.Icount;
                if (RE[x, y, z] != null)
                    continue;
                List<byte?> dataArray = AddTreeCondition(x, y, z, ellipse, RE);
                int[] replicate;
                do//循环记录重复数，直到重复满足最小重复数要求结束循环 
                {
                    replicate = StreeNode.ComputerReplicate(Root, dataArray);
                    int sum = replicate.Sum();
                    if (sum >= MinReplicate)
                        break;
                    else
                    {
                        dataArray.RemoveAt(dataArray.Count - 1);
                    }
                }
                while (dataArray.Count != 0);
                if (dataArray.Count == 0)
                {
                    byte value = (byte)Pdf.DrawValue(cpdf, R);
                    RE[x, y, z] = value;
                }
                else
                {
                    byte value = DrawValue(replicate, R);
                    //此处可引入伺服系统
                    RE[x, y, z] = value;
                }
            }
            var str = RE.ToGslibFile();
            string outfile = Path.Combine(Main.GetWorkPath(), $"{index}", $"sensim.out");
            File.WriteAllText(outfile, str);
        }

        private List<byte?> AddTreeCondition(int x, int y, int z, SearchNeighbor treeneighbor, DRealization RE)
        {
            List<byte?> dataArray = new List<byte?>();
            for (int inode = 1; inode < treeneighbor.nodes.Count; inode++)//待模拟点不考虑,依据搜索模板将条件数据添加入数组中
            {
                int xnode = x + treeneighbor.nodes[inode].X;
                int ynode = y + treeneighbor.nodes[inode].Y;
                int znode = z + treeneighbor.nodes[inode].Z;
                dataArray.Add(RE[xnode, ynode, znode]);
            }
            return dataArray;
        }

        private byte DrawValue(int[] replicate, Random R)
        {
            return (byte)Pdf.DrawValue(replicate, R);
        }

        public override bool GetRunState(int index)
        {
            return true;
        }

        protected override void ShowParamForm()
        {
            using (SensimRunForm form = new SensimRunForm())
            {
                form.InitForm(par);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    par = form.GetSgsPar();
                    var newparam = VariableData.ObjectToVariables(par);
                    UpdateText(newparam);
                }
            }
        }

        public override string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(par));
            writer.WriteValue(par?.Save());
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public override void Open(string str)
        {
            JObject jobj = JObject.Parse(str);
            var parstr = jobj[nameof(par)]?.ToString();
            if (!string.IsNullOrEmpty(parstr))
            {
                par = new SnesimPar();
                par.Open(parstr);
            }
            if (par != null)
            {
                var newparam = VariableData.ObjectToVariables(par);
                UpdateText(newparam);
            }
        }

        public override IReadOnlyList<VariableData> GetUncentainParam()
        {
            return VariableData.ObjectToVariables(par);
        }
    }
}
