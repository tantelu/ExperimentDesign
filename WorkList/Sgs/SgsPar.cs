using ExperimentDesign.General;
using ExperimentDesign.WorkList.Base;
using ExperimentDesign.WorkList.FacieCtrl;
using ExperimentDesign.WorkList.Grid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Sgs
{
    public class SgsPar : IFacieCtrlPar
    {
        [Description("条件数据绝对路径")]
        public Design<string> DataFileName { get; set; }

        [Description("数据最小值")]
        public Design<double> MinValue { get; set; }

        [Description("数据最大值")]
        public Design<double> MaxValue { get; set; }

        [Group]
        public Variogram Variogram { get; set; }

        [Description("椭球体长半轴")]
        public Design<double> SearchMaxRadius { get; set; }

        [Description("椭球体中半轴")]
        public Design<double> SearchMedRadius { get; set; }

        [Description("椭球体短半轴")]
        public Design<double> SearchMinRadius { get; set; }

        [Description("椭球体主方向")]
        public Design<double> Azimuth { get; set; }

        [Description("椭球体倾角")]
        public Design<double> Dip { get; set; }

        [Description("椭球体倾覆角")]
        public Design<double> Rake { get; set; }

        [Description("克里金类型")]
        public KrigType KrigType { get; set; }

        [Description("最大条件点数")]
        public Design<int> MaxData { get; set; }

        [Description("是否使用多级网格搜索")]
        public bool UseMulti { get; set; } = false;

        [Description("多级网格")]
        public Design<int> MultiGrid { get; set; }

        public string ControlTypeName => typeof(SgsUserControl).FullName;

        public void Save(string file, Grid3D Grid3D, IReadOnlyDictionary<string, object> designVaribles)
        {
            Save(file, Grid3D, designVaribles, "sgs.out");
        }

        public void Save(string file, Grid3D Grid3D, IReadOnlyDictionary<string, object> designVaribles, string outfilename)
        {
            int dataexit = File.Exists(this.DataFileName) ? 1 : 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("              Parameters for SGSIM                                         ");
            sb.AppendLine("              ********************                                         ");
            sb.AppendLine("                                                                           ");
            sb.AppendLine("START OF PARAMETER:                                                        ");
            sb.AppendLine($"{(dataexit == 1 ? Path.GetFileName(DataFileName) : "none")}                          -file with data                              ");
            sb.AppendLine("1  2  0  4  0  0              -  columns for X,Y,Z,vr,wt,sec.var.          ");
            sb.AppendLine("-1.0e21    1.0e21             -  trimming limits                           ");
            sb.AppendLine($"{dataexit}                             -transform the data (0=no, 1=yes)            ");
            sb.AppendLine("trans.trn                      -  file for output trans table               ");
            sb.AppendLine("0                             -  consider ref. dist (0=no, 1=yes)          ");
            sb.AppendLine("none.dat                      -  file with ref. dist distribution          ");
            sb.AppendLine("1  2                          -  columns for vr and wt                     ");
            sb.AppendLine($"{MinValue}    {MaxValue}     -  zmin,zmax(tail extrapolation)             ");
            sb.AppendLine("1      1.0                   -  lower tail option, parameter              ");
            sb.AppendLine("1      1.0                   -  upper tail option, parameter              ");
            sb.AppendLine("2                             -debugging level: 0,1,2,3                    ");
            sb.AppendLine("sgs.dbg                      -file for debugging output                   ");
            sb.AppendLine($"{outfilename}                  -file for simulation output                  ");
            sb.AppendLine($"{1}" + "                 -number of realizations to generate          ");
            sb.AppendLine($"{(int)((Grid3D.Xmax - Grid3D.Xmin) / Grid3D.Xsize)} {Grid3D.Xmin} {Grid3D.Xsize}  -nx,xmn,xsiz");
            sb.AppendLine($"{(int)((Grid3D.Ymax - Grid3D.Ymin) / Grid3D.Ysize)} {Grid3D.Ymin} {Grid3D.Ysize} -ny,ymn,ysiz");
            sb.AppendLine($"{(int)((Grid3D.Zmax - Grid3D.Zmin) / Grid3D.Zsize)} {Grid3D.Zmin} {Grid3D.Zsize} -nz,zmn,zsiz");
            sb.AppendLine($"{GlobalWorkCongfig.Seed}  -random number seed");
            sb.AppendLine($"0  {MaxData}  -min and max original data for sim");
            sb.AppendLine("12                            -number of simulated nodes to use            ");
            sb.AppendLine($"{dataexit}                             -assign data to nodes (0=no, 1=yes)          ");
            sb.AppendLine($"{(UseMulti ? 1 : 0)}  {(MultiGrid == null ? 3 : MultiGrid)}   -multiple grid search (0=no, 1=yes),num");
            sb.AppendLine("0   -maximum data per octant (0=not used)");
            sb.AppendLine($"{SearchMaxRadius}  {SearchMedRadius}  {SearchMinRadius}  -maximum search  (hmax,hmin,vert)");
            sb.AppendLine($"{Azimuth}  {Dip}  {Rake}    -angles for search ellipsoid                 ");
            sb.AppendLine("101 101 1 -size of covariance lookup table");
            sb.AppendLine($"{(int)KrigType}   0.60   1.0     -ktype: 0=SK,1=OK,2=LVM,3=EXDR,4=COLC");
            sb.AppendLine("none.dat                      -  file with LVM, EXDR, or COLC variable");
            sb.AppendLine("4                             -  column for secondary variable");
            sb.AppendLine($"{1.0}  {Variogram.Nug}   -nst, nugget effect");
            sb.AppendLine($"{(int)Variogram.VarType + 1} {Variogram.Sill} {Variogram.MajorAzi}  {Variogram.MajorDip} {0.0}  -it,cc,ang1,ang2,ang3");
            sb.AppendLine($"{Variogram.MajorRange} { Variogram.MinorRange} {Variogram.VerRange}  - a_hmax, a_hmin, a_vert");
            foreach (var keyvalue in designVaribles)
            {
                sb.Replace(keyvalue.Key, keyvalue.Value.ToString());
            }
            File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
        }

        public string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(KrigType));
            writer.WriteValue((int)KrigType);
            writer.WritePropertyName(nameof(MaxData));
            writer.WriteValue(MaxData.Save());
            writer.WritePropertyName(nameof(Rake));
            writer.WriteValue(Rake.Save());
            writer.WritePropertyName(nameof(Dip));
            writer.WriteValue(Dip.Save());
            writer.WritePropertyName(nameof(Azimuth));
            writer.WriteValue(Azimuth.Save());
            writer.WritePropertyName(nameof(SearchMinRadius));
            writer.WriteValue(SearchMinRadius.Save());
            writer.WritePropertyName(nameof(SearchMedRadius));
            writer.WriteValue(SearchMedRadius.Save());
            writer.WritePropertyName(nameof(SearchMaxRadius));
            writer.WriteValue(SearchMaxRadius.Save());
            writer.WritePropertyName(nameof(MaxValue));
            writer.WriteValue(MaxValue.Save());
            writer.WritePropertyName(nameof(MinValue));
            writer.WriteValue(MinValue.Save());
            writer.WritePropertyName(nameof(DataFileName));
            writer.WriteValue(DataFileName.Save());
            writer.WritePropertyName(nameof(Variogram));
            writer.WriteValue(Variogram.Save());
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public void Open(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JObject jo = JObject.Parse(json);
                KrigType = (KrigType)(int)jo[nameof(KrigType)];
                MaxData = new Design<int>();
                MaxData.Open(jo[nameof(MaxData)]?.ToString());

                MinValue = new Design<double>();
                MinValue.Open(jo[nameof(MinValue)]?.ToString());
                MaxValue = new Design<double>();
                MaxValue.Open(jo[nameof(MaxValue)]?.ToString());
                DataFileName = new Design<string>();
                DataFileName.Open(jo[nameof(DataFileName)]?.ToString());

                Rake = new Design<double>();
                Rake.Open(jo[nameof(Rake)]?.ToString());

                Dip = new Design<double>();
                Dip.Open(jo[nameof(Dip)]?.ToString());

                Azimuth = new Design<double>();
                Azimuth.Open(jo[nameof(Azimuth)]?.ToString());

                SearchMinRadius = new Design<double>();
                SearchMinRadius.Open(jo[nameof(SearchMinRadius)]?.ToString());

                SearchMedRadius = new Design<double>();
                SearchMedRadius.Open(jo[nameof(SearchMedRadius)]?.ToString());

                SearchMaxRadius = new Design<double>();
                SearchMaxRadius.Open(jo[nameof(SearchMaxRadius)]?.ToString());

                var varjson = jo[nameof(Variogram)]?.ToString();
                if (!string.IsNullOrEmpty(varjson))
                {
                    Variogram = new Variogram();
                    Variogram.Open(varjson);
                }
            }
        }

        public float[] FacieCtrlRun(Grid3D grid, string workpath, IReadOnlyDictionary<string, object> designVaribles)
        {
            string file = Path.Combine(workpath, $"sgsim.par");
            this.Save(file, grid, designVaribles);
            string exe = Path.Combine(workpath, @"sgsim.exe");
            string _out = Path.Combine(workpath, @"sgs.out");
            if (File.Exists(this.DataFileName))
            {
                File.Copy(this.DataFileName, Path.Combine(workpath, Path.GetFileName(this.DataFileName)), true);
            }
            File.Copy(Path.Combine(Application.StartupPath, "geostatspy", "sgsim.exe"), exe, true);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = exe;
            info.WorkingDirectory = workpath;
            info.UseShellExecute = false;
            info.Arguments = "sgsim.par";
            var process = Process.Start(info);
            process.WaitForExit();
            int xcount = 0;
            int ycount = 0;
            int zcount = 0;
            var sgs = Gslib.ReadGislib(_out, out xcount, out ycount, out zcount);
            return sgs;
        }
    }
}
