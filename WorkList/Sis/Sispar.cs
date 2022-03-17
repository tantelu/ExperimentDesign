using ExperimentDesign.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExperimentDesign.WorkList.Sis
{
    public class SisPar
    {
        public static SisPar Default
        {
            get
            {
                SisPar par = new SisPar();
                par.KrigType = KrigType.SK;
                par.Vars = new List<CategoryIndicatorParam>();
                for (int i = 0; i < 5; i++)
                {
                    CategoryIndicatorParam ipar = new CategoryIndicatorParam();
                    ipar.Facie = i;
                    ipar.Probability = 0.2;
                    ipar.Variogram = Variogram.Default;
                    par.Vars.Add(ipar);
                }
                par.MaxData = 12;
                return par;
            }
        }

        public static SisPar VariablesToSis(List<VariableData> datas, IReadOnlyDictionary<string, object> designVaribles)
        {
            SisPar sisPar = Default;
            var allproperties = sisPar.GetType().GetProperties();
            var propeties = allproperties.Where(_ => _.GetCustomAttribute<DescriptionAttribute>() != null).ToList();
            foreach (var property in propeties)
            {
                var par = datas.FirstOrDefault(_ => string.Equals(property.GetCustomAttribute<DescriptionAttribute>().Description, _.ParDescription));
                if (par != null)
                {
                    object _value = null;
                    if (designVaribles?.Count > 0 && par.Name.Contains("$"))
                    {
                        if (!designVaribles.TryGetValue(par.Name, out _value))
                        {
                            return null;
                        }
                    }
                    else
                    {
                        _value = par.BaseValue;
                    }
                    property.SetValue(sisPar, Convert.ChangeType(_value, property.PropertyType));
                }
            }
            var groups = allproperties.Where(_ => _.GetCustomAttribute<GroupAttribute>() != null).ToList();
            return null;
        }

        public SisPar()
        {

        }

        public Grid3D Grid3D { get; set; }

        [Description("指示参数")]
        [GroupList]
        public List<CategoryIndicatorParam> Vars { get; set; }

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

        [Description("是否使用中值克里金")]
        public bool MedianIK { get; set; }

        public void Save(string file)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("              Parameters for SISIM                                         ");
            sb.AppendLine("              ********************                                         ");
            sb.AppendLine("                                                                           ");
            sb.AppendLine("START OF PARAMETERS:                                                        ");
            sb.AppendLine("1                             -1=continuous(cdf), 0=categorical(pdf)");
            sb.AppendLine("5                             -number thresholds/categories");
            foreach (var item in Vars)
            {
                sb.Append($"{item.Facie}  ");
            }
            sb.AppendLine("-thresholds / categories");
            foreach (var item in Vars)
            {
                sb.Append($"{item.Probability}  ");
            }
            sb.AppendLine("-global cdf / pdf");
            sb.AppendLine("../data/cluster.dat           -file with data");
            sb.AppendLine("1   2   3   4                 -   columns for X,Y,Z, and variable");
            sb.AppendLine("direct.ik                     -file with soft indicator input");
            sb.AppendLine("1   2   3  4 5 6 7  8      -   columns for X,Y,Z, and indicators");
            sb.AppendLine("0                             -   Markov-Bayes simulation (0=no,1=yes)");
            sb.AppendLine("0.61  0.54  0.56  0.53  0.29  -      calibration B(z) values");
            sb.AppendLine("-1.0e21    1.0e21             -trimming limits");
            sb.AppendLine($"0.0   {MaxData}              -minimum and maximum data value");
            sb.AppendLine("1      0.0                    -   lower tail option and parameter");
            sb.AppendLine("1      1.0                    -   middle     option and parameter");
            sb.AppendLine("1     30.0                    -   upper tail option and parameter");
            sb.AppendLine("cluster.dat                   -   file with tabulated values");
            sb.AppendLine("3   0                         -      columns for variable, weight");
            sb.AppendLine("3                             -debugging level: 0,1,2,3");
            sb.AppendLine("sis.dbg                     -file for debugging output");
            sb.AppendLine("sis.out                     -file for simulation output");
            sb.AppendLine($"1                 -number of realizations");
            sb.AppendLine($"{(int)((Grid3D.Xmax - Grid3D.Xmin) / Grid3D.Xsize)}" + " " + $"{Grid3D.Xmin}" + " " + $"{Grid3D.Xsize}" + "                              ");
            sb.AppendLine($"{(int)((Grid3D.Ymax - Grid3D.Ymin) / Grid3D.Ysize)}" + " " + $"{Grid3D.Ymin}" + " " + $"{Grid3D.Ysize}" + "                              ");
            sb.AppendLine($"{(int)((Grid3D.Zmax - Grid3D.Zmin) / Grid3D.Zsize)}" + " " + $"{Grid3D.Zmin}" + " " + $"{Grid3D.Zsize}" + "                              ");
            sb.AppendLine($"{GlobalWorkCongfig.Seed}  -random number seed");
            sb.AppendLine($"12      -maximum original data  for each kriging");
            sb.AppendLine("12       -maximum previous nodes for each kriging");
            sb.AppendLine("1        -maximum soft indicator nodes for kriging");
            sb.AppendLine("1                             -assign data to nodes (0=no, 1=yes)          ");
            sb.AppendLine($"0     3          -multiple grid search (0=no, 1=yes),num      ");
            sb.AppendLine("0                             -maximum data per octant (0=not used)        ");
            sb.AppendLine($"{SearchMaxRadius} {SearchMedRadius} {SearchMinRadius} -maximum search radii");
            sb.AppendLine($"{Azimuth} {Dip} {Rake} -angles for search ellipsoid                 ");
            sb.AppendLine("51    51    11                -size of covariance lookup table");
            sb.AppendLine("0    2.5                      -0=full IK, 1=median approx. (cutoff)");
            sb.AppendLine($"{(int)KrigType}             -0=SK, 1=OK");
            foreach (var item in Vars)
            {
                sb.AppendLine($"{1.0}   {item.Variogram.Nug}   - nst, nugget effect");
                sb.AppendLine($"{(int)item.Variogram.VarType} {item.Variogram.Sill} {item.Variogram.MajorAzi} {item.Variogram.MajorDip} 0.0 - it,cc,ang1,ang2,ang3");
                sb.AppendLine($"{item.Variogram.MajorRange} {item.Variogram.MinorRange} {item.Variogram.VerRange} -a_hmax, a_hmin, a_vert ");
            }
            File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
        }
    }

    public class CategoryIndicatorParam
    {
        [Description("变差函数")]
        [Group]
        public Variogram Variogram { get; set; }

        /// <summary>
        /// 此处默认离散型
        /// </summary>
        [Description("相代码")]
        public int Facie { get; set; }

        [Description("全局概率")]
        public double Probability { get; set; }
    }

    public class SisVariableData : VariableData
    {
        /// <summary>
        /// 
        /// </summary>
        public int VariogramIndex { get; set; }
    }
}
