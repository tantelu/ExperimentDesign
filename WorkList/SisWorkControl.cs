using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList
{
    public class SisWorkControl : WorkControl
    {
        public SisWorkControl()
        {

        }

        protected override string WorkName => "序贯指示模拟";

        protected override Bitmap Picture => global::ExperimentDesign.Properties.Resources.Sgs;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            string gridfile = Path.Combine(Main.GetWorkPath(), $"{index}", $"{nameof(Grid3D)}.json");
            if (!File.Exists(gridfile))
            {
                XtraMessageBox.Show($"未找到工区网格定义文件,无法执行{WorkName}");
                return;
            }
            Grid3D grid = new Grid3D();
            grid.Open(gridfile);
            SisPar sgsPar = new SisPar(grid);
            var propeties = sgsPar.GetType().GetProperties().Where(_ => _.GetCustomAttribute<DescriptionAttribute>() != null).ToList();
            foreach (var property in propeties)
            {
                var par = this.param.FirstOrDefault(_ => string.Equals(property.GetCustomAttribute<DescriptionAttribute>().Description, _.ParDescription));
                if (par != null)
                {
                    object _value = null;
                    if (par.Name.Contains("$"))
                    {
                        if (!designVaribles.TryGetValue(par.Name, out _value))
                        {
                            XtraMessageBox.Show($"在设计表中不存在'{par.Name}',请检查错误。");
                            return;
                        }
                    }
                    else
                    {
                        _value = par.BaseValue;
                    }
                    property.SetValue(sgsPar, Convert.ChangeType(_value, property.PropertyType));
                }
                else
                {
                    XtraMessageBox.Show($"在‘{WorkName}’工作流中未找到参数‘{property.GetCustomAttribute<DescriptionAttribute>().Description}’,请检查错误");
                    return;
                }
            }
            string file = Path.Combine(Main.GetWorkPath(), $"{index}", $"sisim.par");
            sgsPar.Save(file);
            string exe = Path.Combine(Main.GetWorkPath(), $"{index}", @"sisim.exe");
            string _out = Path.Combine(Main.GetWorkPath(), $"{index}", @"sis.out");
            File.Copy(Path.Combine(Application.StartupPath, "geostatspy", "sisim.exe"), exe, true);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = exe;
            info.WorkingDirectory = Path.Combine(Main.GetWorkPath(), $"{index}");
            info.UseShellExecute = false;
            info.Arguments = "sisim.par";
            var process = Process.Start(info);
        }

        public override bool GetRunState(int index)
        {
            string _out = Path.Combine(Main.GetWorkPath(), $"{index}", @"sis.out");
            return File.Exists(_out);
        }

        protected override void ShowParamForm()
        {
            using (SisEditorForm form = new SisEditorForm())
            {
                form.InitForm(param);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.param.Clear();
                    this.param.AddRange(form.GetUncentainParam());
                }
            }
        }
    }

    public class SisPar
    {
        public SisPar(Grid3D grid)
        {
            this.Grid3D = grid;
        }

        public Grid3D Grid3D { get; set; }

        [Description("基台值")]
        public double Sill { get; set; }

        [Description("块金值")]
        public double Nug { get; set; }

        [Description("变差函数类型")]
        public string VarType { get; set; }

        [Description("主变程")]
        public double MajorRange { get; set; }

        [Description("次变程")]
        public double MinorRange { get; set; }

        [Description("垂直变程")]
        public double VerRange { get; set; }

        [Description("变差函数主方向")]
        public double MajorAzi { get; set; }

        [Description("变差函数倾角")]
        public double MajorDip { get; set; }

        [Description("克里金类型")]
        public string KrigType { get; set; }

        [Description("最大条件点数")]
        public double MaxData { get; set; }

        [Description("是否使用多级网格搜索")]
        public bool UseMulti { get; set; }

        [Description("多级网格")]
        public int MultiGrid { get; set; }

        public void Save(string file)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("              Parameters for SISIM                                         ");
            sb.AppendLine("              ********************                                         ");
            sb.AppendLine("                                                                           ");
            sb.AppendLine("START OF PARAMETERS:                                                        ");
            sb.AppendLine("1                             -1=continuous(cdf), 0=categorical(pdf)");
            sb.AppendLine("5                             -number thresholds/categories");
            sb.AppendLine("0.5   1.0   2.5   5.0   10.0  -thresholds / categories");
            sb.AppendLine("0.12  0.29  0.50  0.74  0.88  -   global cdf / pdf");
            sb.AppendLine("../data/cluster.dat           -file with data");
            sb.AppendLine("1   2   0   3                 -   columns for X,Y,Z, and variable");
            sb.AppendLine("direct.ik                     -file with soft indicator input");
            sb.AppendLine("1   2   0   3 4 5 6 7         -   columns for X,Y,Z, and indicators");
            sb.AppendLine("0                             -   Markov-Bayes simulation (0=no,1=yes)");
            sb.AppendLine("0.61  0.54  0.56  0.53  0.29  -      calibration B(z) values");
            sb.AppendLine("-1.0e21    1.0e21             -trimming limits");
            sb.AppendLine($"0.0   30              -minimum and maximum data value");
            sb.AppendLine("1      0.0                    -   lower tail option and parameter");
            sb.AppendLine("1      1.0                    -   middle     option and parameter");
            sb.AppendLine("1     30.0                    -   upper tail option and parameter");
            sb.AppendLine("cluster.dat                   -   file with tabulated values");
            sb.AppendLine("3   0                         -      columns for variable, weight");
            sb.AppendLine("3                             -debugging level: 0,1,2,3");
            sb.AppendLine("sis.dbg                     -file for debugging output");
            sb.AppendLine("sis.out                     -file for simulation output");
            sb.AppendLine($"{1}" + "                 -number of realizations");
            sb.AppendLine($"{(int)((Grid3D.Xmax - Grid3D.Xmin) / Grid3D.Xsize)}" + " " + $"{Grid3D.Xmin}" + " " + $"{Grid3D.Xsize}" + "                              ");
            sb.AppendLine($"{(int)((Grid3D.Ymax - Grid3D.Ymin) / Grid3D.Ysize)}" + " " + $"{Grid3D.Ymin}" + " " + $"{Grid3D.Ysize}" + "                              ");
            sb.AppendLine($"{(int)((Grid3D.Zmax - Grid3D.Zmin) / Grid3D.Zsize)}" + " " + $"{Grid3D.Zmin}" + " " + $"{Grid3D.Zsize}" + "                              ");
            sb.AppendLine($"{GlobalWorkCongfig.Seed}  -random number seed");
            sb.AppendLine($"{MaxData}       -maximum original data  for each kriging");
            sb.AppendLine("12       -maximum previous nodes for each kriging");
            sb.AppendLine("1       -maximum soft indicator nodes for kriging");
            sb.AppendLine("1                             -assign data to nodes (0=no, 1=yes)          ");
            sb.AppendLine($"{(UseMulti ? 1 : 0)}     {MultiGrid}          -multiple grid search (0=no, 1=yes),num      ");
            sb.AppendLine("0                             -maximum data per octant (0=not used)        ");
            sb.AppendLine($"{MajorRange} {MinorRange} {VerRange} -maximum search radii");
            sb.AppendLine("0.0   0.0   0.0       -angles for search ellipsoid                 ");
            sb.AppendLine("51    51    11                -size of covariance lookup table");
            sb.AppendLine("0    2.5                      -0=full IK, 1=median approx. (cutoff)");
            sb.AppendLine($"{ChangeKrigType}             -0=SK, 1=OK");
            int vartype = string.Equals(VarType, "指数模型") ? 1 : string.Equals(VarType, "球状模型") ? 2 : 0;
            sb.AppendLine($"{1.0}" + " " + $"{Nug}" + "  -One   nst, nugget effect");
            sb.AppendLine($"{vartype} {Sill} {MajorAzi} {MajorDip} 0.0 - it,cc,ang1,ang2,ang3");
            sb.AppendLine($"{MajorRange} {MinorRange} {VerRange} -a_hmax, a_hmin, a_vert ");
            sb.AppendLine($"{1.0}" + " " + $"{Nug}" + "  -Two   nst, nugget effect");
            sb.AppendLine($"{vartype} {Sill} {MajorAzi} {MajorDip} 0.0  - it,cc,ang1,ang2,ang3");
            sb.AppendLine($"{MajorRange} {MinorRange} {VerRange}  - a_hmax, a_hmin, a_vert ");
            sb.AppendLine($"{1.0}" + " " + $"{Nug}" + "  -Three   nst, nugget effect");
            sb.AppendLine($"{vartype} {Sill} {MajorAzi} {MajorDip} 0.0 - it,cc,ang1,ang2,ang3");
            sb.AppendLine($"{MajorRange} {MinorRange} {VerRange}   - a_hmax, a_hmin, a_vert ");
            sb.AppendLine($"{1.0}" + " " + $"{Nug}" + "  -Four   nst, nugget effect");
            sb.AppendLine($"{vartype} {Sill} {MajorAzi} {MajorDip} 0.0 - it,cc,ang1,ang2,ang3");
            sb.AppendLine($"{MajorRange} {MinorRange} {VerRange} - a_hmax, a_hmin, a_vert ");
            sb.AppendLine($"{1.0}" + " " + $"{Nug}" + "  -Five   nst, nugget effect");
            sb.AppendLine($"{vartype} {Sill} {MajorAzi} {MajorDip} 0.0 - it,cc,ang1,ang2,ang3");
            sb.AppendLine($"{MajorRange} {MinorRange} {VerRange} - a_hmax, a_hmin, a_vert ");
            File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
        }

        private int ChangeKrigType
        {
            get
            {
                if (string.Equals(KrigType, "简单克里金"))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}
