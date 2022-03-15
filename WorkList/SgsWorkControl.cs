using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList
{
    public class SgsWorkControl : WorkControl
    {
        public SgsWorkControl()
        {

        }


        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        protected override string WorkName => "序贯高斯模拟";

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
            SgsPar sgsPar = new SgsPar(grid);
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
            string file = Path.Combine(Main.GetWorkPath(), $"{index}", $"sgsim.par");
            sgsPar.Save(file);
            string exe = Path.Combine(Main.GetWorkPath(), $"{index}", @"sgsim.exe");
            string _out = Path.Combine(Main.GetWorkPath(), $"{index}", @"sgs.out");
            File.Copy(Path.Combine(Application.StartupPath, "geostatspy", "sgsim.exe"), exe, true);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = exe;
            info.WorkingDirectory = Path.Combine(Main.GetWorkPath(), $"{index}");
            info.UseShellExecute = false;
            info.Arguments = "sgsim.par";
            var process = Process.Start(info);
        }

        public override bool GetRunState(int index)
        {
            string _out = Path.Combine(Main.GetWorkPath(), $"{index}", @"sgs.out");
            return File.Exists(_out);
        }

        protected override void ShowParamForm()
        {
            using (SgsEditorForm form = new SgsEditorForm())
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

    public class SgsPar
    {
        public SgsPar(Grid3D grid)
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

        [Description("基台值")]
        public int MultiGrid { get; set; }

        public void Save(string file)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("              Parameters for SGSIM                                         ");
            sb.AppendLine("              ********************                                         ");
            sb.AppendLine("                                                                           ");
            sb.AppendLine("START OF PARAMETER:                                                        ");
            sb.AppendLine("none                          -file with data                              ");
            sb.AppendLine("1  2  0  3  5  0              -  columns for X,Y,Z,vr,wt,sec.var.          ");
            sb.AppendLine("-1.0e21 1.0e21                -  trimming limits                           ");
            sb.AppendLine("0                             -transform the data (0=no, 1=yes)            ");
            sb.AppendLine("none.trn                      -  file for output trans table               ");
            sb.AppendLine("1                             -  consider ref. dist (0=no, 1=yes)          ");
            sb.AppendLine("none.dat                      -  file with ref. dist distribution          ");
            sb.AppendLine("1  0                          -  columns for vr and wt                     ");
            sb.AppendLine("-4.0    4.0                   -  zmin,zmax(tail extrapolation)             ");
            sb.AppendLine("1      -4.0                   -  lower tail option, parameter              ");
            sb.AppendLine("1       4.0                   -  upper tail option, parameter              ");
            sb.AppendLine("0                             -debugging level: 0,1,2,3                    ");
            sb.AppendLine("nonw.dbg                      -file for debugging output                   ");
            sb.AppendLine("sgs.out" + "                  -file for simulation output                  ");
            sb.AppendLine($"{1}" + "                 -number of realizations to generate          ");
            sb.AppendLine($"{(int)((Grid3D.Xmax - Grid3D.Xmin) / Grid3D.Xsize)}" + " " + $"{Grid3D.Xmin}" + " " + $"{Grid3D.Xsize}" + "                              ");
            sb.AppendLine($"{(int)((Grid3D.Ymax - Grid3D.Ymin) / Grid3D.Ysize)}" + " " + $"{Grid3D.Ymin}" + " " + $"{Grid3D.Ysize}" + "                              ");
            sb.AppendLine($"{(int)((Grid3D.Zmax - Grid3D.Zmin) / Grid3D.Zsize)}" + " " + $"{Grid3D.Zmin}" + " " + $"{Grid3D.Zsize}" + "                              ");
            sb.AppendLine("1111" + "                  -random number seed                    ");
            sb.AppendLine("0     8                       -min and max original data for sim           ");
            sb.AppendLine("12                            -number of simulated nodes to use            ");
            sb.AppendLine("0                             -assign data to nodes (0=no, 1=yes)          ");
            sb.AppendLine("1     3                       -multiple grid search (0=no, 1=yes),num      ");
            sb.AppendLine("0                             -maximum data per octant (0=not used)        ");
            sb.AppendLine($"{MajorRange}" + " " + $"{MinorRange}" + " 1.0 -maximum search  (hmax,hmin,vert) ");
            sb.AppendLine("0.0   0.0   0.0       -angles for search ellipsoid                 ");
            sb.AppendLine("101" + " " + "101" + " 1 -size of covariance lookup table        ");
            sb.AppendLine($"{ChangeKrigType}     0.60   1.0              -ktype: 0=SK,1=OK,2=LVM,3=EXDR,4=COLC        ");
            sb.AppendLine("none.dat                      -  file with LVM, EXDR, or COLC variable     ");
            sb.AppendLine("4                             -  column for secondary variable             ");
            sb.AppendLine($"{1.0}" + " " + $"{Nug}" + "  -nst, nugget effect                          ");
            sb.AppendLine($"{1}" + " " + $"{Sill}" + " " + $"{MajorAzi}" + " " + $"{MajorDip}" + " " + $"{0.0}" + " - it,cc,ang1,ang2,ang3");
            sb.AppendLine(" " + $"{MajorRange}" + " " + $"{MinorRange}" + " " + $"{VerRange}" + " 1.0 - a_hmax, a_hmin, a_vert        ");
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
