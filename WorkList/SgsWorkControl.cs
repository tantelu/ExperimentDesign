using ExperimentDesign.WorkList;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace WorkList.ExperimentDesign
{
    public class SgsWorkControl : WorkControl
    {
        public SgsWorkControl()
        {

        }

        protected override string WorkName => "序贯高斯模拟";

        protected override Bitmap Picture => global::ExperimentDesign.Properties.Resources.Sgs;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            var param = this.param;
            string exe = Path.Combine(Main.GetWorkPath(), @"sgsim.exe");
            string _out = Path.Combine(Main.GetWorkPath(), @"sgs.out");
            File.Copy(Path.Combine(Application.StartupPath, "sgsim.exe"), Path.Combine(exe));
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = exe;
            info.WorkingDirectory = Main.GetWorkPath();
            info.UseShellExecute = false;
            info.Arguments = "sgsim.par";
            var process = Process.Start(info);
        }

        private bool WriteSgsPar(Grid3D grid)
        {
            //var file = Path.Combine(Main.GetWorkPath(), "sgsim.par");
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("              Parameters for SGSIM                                         ");
            //sb.AppendLine("              ********************                                         ");
            //sb.AppendLine("                                                                           ");
            //sb.AppendLine("START OF PARAMETER:                                                        ");
            //sb.AppendLine("none                          -file with data                              ");
            //sb.AppendLine("1  2  0  3  5  0              -  columns for X,Y,Z,vr,wt,sec.var.          ");
            //sb.AppendLine("-1.0e21 1.0e21                -  trimming limits                           ");
            //sb.AppendLine("0                             -transform the data (0=no, 1=yes)            ");
            //sb.AppendLine("none.trn                      -  file for output trans table               ");
            //sb.AppendLine("1                             -  consider ref. dist (0=no, 1=yes)          ");
            //sb.AppendLine("none.dat                      -  file with ref. dist distribution          ");
            //sb.AppendLine("1  0                          -  columns for vr and wt                     ");
            //sb.AppendLine("-4.0    4.0                   -  zmin,zmax(tail extrapolation)             ");
            //sb.AppendLine("1      -4.0                   -  lower tail option, parameter              ");
            //sb.AppendLine("1       4.0                   -  upper tail option, parameter              ");
            //sb.AppendLine("0                             -debugging level: 0,1,2,3                    ");
            //sb.AppendLine("nonw.dbg                      -file for debugging output                   ");
            //sb.AppendLine("sgs.out" + "                  -file for simulation output                  ");
            //sb.AppendLine($"{1}" + "                 -number of realizations to generate          ");
            //sb.AppendLine($"{100}" + " " + $"{5.0}" + " " + $"{10}" + "                              ");
            //sb.AppendLine($"{100}" + " " + $"{5.0}" + " " + $"{10}" + "                              ");
            //sb.AppendLine("1 0.0 1.0                     - nz zmn zsiz                                ");
            //sb.AppendLine(seed.ToString() + "                  -random number seed                    ");
            //sb.AppendLine("0     8                       -min and max original data for sim           ");
            //sb.AppendLine("12                            -number of simulated nodes to use            ");
            //sb.AppendLine("0                             -assign data to nodes (0=no, 1=yes)          ");
            //sb.AppendLine("1     3                       -multiple grid search (0=no, 1=yes),num      ");
            //sb.AppendLine("0                             -maximum data per octant (0=not used)        ");
            //sb.AppendLine($"{max_range}" + " " + $"{max_range}" + " 1.0 -maximum search  (hmax,hmin,vert) ");
            //sb.AppendLine("0.0   0.0   0.0       -angles for search ellipsoid                 ");
            //sb.AppendLine("101" + " " + "101" + " 1 -size of covariance lookup table        ");
            //sb.AppendLine($"{ktype}     0.60   1.0              -ktype: 0=SK,1=OK,2=LVM,3=EXDR,4=COLC        ");
            //sb.AppendLine("none.dat                      -  file with LVM, EXDR, or COLC variable     ");
            //sb.AppendLine("4                             -  column for secondary variable             ");
            //sb.AppendLine($"{1.0}" + " " + $"{nug}" + "  -nst, nugget effect                          ");
            //sb.AppendLine($"{1}" + " " + $"{1.0}" + " " + $"{0}" + " 0.0 0.0 -it,cc,ang1,ang2,ang3");
            //sb.AppendLine(" " + $"{500}" + " " + $"{500}" + " 1.0 - a_hmax, a_hmin, a_vert        ");
            //sb.AppendLine($"{1}" + " " + $"{0}" + " " + $"{angel}" + " 0.0 0.0 -it,cc,ang1,ang2,ang3");
            //sb.AppendLine(" " + $"{0}" + " " + $"{1.0}" + " 1.0 - a_hmax, a_hmin, a_vert        ");
            //File.WriteAllText(par, sb.ToString(), Encoding.UTF8);

            Thread.Sleep(1000);
            return true;
        }

        private string CreateSgsFolder(string folder, int index, int seed, double max_range, double angel, double ktype, double nug)
        {
            string path = Path.Combine(folder, index.ToString());
            string par = Path.Combine(path, "sgsim.par");



            return string.Empty;
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
}
