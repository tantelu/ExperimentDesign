using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Data;

namespace ExperimentDesign.Statistic
{
    public class Probability
    {
        public static void CPDF(DataTable table)
        {
            //Random R = new Random();
            //List<double> volumns = new List<double>();
            //var param = UniMultipleLineRegression.Regression(table).GetParam();
            //int i = 0;
            //while (i < 1000)
            //{
            //    var x1 = R.NextDouble() - 1;
            //    var x2 = R.NextDouble() - 1;
            //    var x3 = R.NextDouble() - 1;
            //    var y = param[0] + x1 * param[1] + x2 * param[2] + x3 * param[3];
            //    volumns.Add(y);
            //    i++;
            //}
            //var p10 = Statistics.EmpiricalInvCDF(volumns, 0.9);
            //var p50 = Statistics.EmpiricalInvCDF(volumns, 0.5);
            //var p90 = Statistics.EmpiricalInvCDF(volumns, 0.1);
            //var a = p90;
        }
    }
}
