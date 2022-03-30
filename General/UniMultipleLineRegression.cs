using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExperimentDesign.General
{
    //参考博客 https://blog.csdn.net/qq_43460172/article/details/98788494
    public class UniMultipleLineRegression
    {
        private Matrix<double> x;

        private Vector<double> y;

        //系数
        public double[] GetBs()
        {
            return MultipleRegression.QR(x.Transpose().ToColumnArrays(), y.ToArray(), true).Skip(1).ToArray();
        }

        //标准系数(t值)
        public double[] GetNormalBs()
        {
            var bs = this.GetBs();
            double[] normalbs = new double[bs.Length];
            for (int i = 0; i < bs.Length; i++)
            {
                normalbs[i] = bs[i] * GetSx(i) / GetSy();
            }
            return normalbs;
        }

        //变量标准误差
        private double GetSx(int xindex)
        {
            var col = x.Column(xindex);
            var s = Statistics.StandardDeviation(col);
            return s;
        }

        private double GetSy()
        {
            return Statistics.StandardDeviation(y);
        }

        public static DataTable VarianceAnalysisTable(DataTable table)
        {
            if (table.Columns.Count > 2)
            {
                UniMultipleLineRegression data = new UniMultipleLineRegression();
                data.x = CreateMatrix.Dense(table.Rows.Count, table.Columns.Count - 2, (int i, int j) => { return Convert.ToDouble(table.Rows[i][j + 1]); });
                data.y = CreateVector.Dense(table.Rows.Count, (int i) => { return Convert.ToDouble(table.Rows[i][table.Columns.Count - 1]); });

                DataTable res = new DataTable();
                var col = res.Columns.Add($"因数", Type.GetType("System.String"));
                res.Columns.Add($"回归系数", Type.GetType("System.Double"));
                res.Columns.Add($"回归系数标准差", Type.GetType("System.Double"));
                res.Columns.Add($"标准回归系数(T值)", Type.GetType("System.Double"));
                res.Columns.Add($"显著性排序", Type.GetType("System.Double"));
                var bs = data.GetBs();
                var normalbs = data.GetNormalBs();
                List<Tuple<double, int>> list = new List<Tuple<double, int>>();
                for (int i = 0; i < normalbs.Length; i++)
                {
                    list.Add(new Tuple<double, int>(Math.Abs(normalbs[i]), i));
                }
                var sortlist = list.OrderByDescending(_ => _.Item1).ToList();
                for (int i = 0; i < bs.Length; i++)
                {
                    DataRow row = res.NewRow();
                    var sx = data.GetSx(i);
                    row["因数"] = table.Columns[i + 1].ColumnName;
                    row["回归系数"] = bs[i];
                    row["回归系数标准差"] = sx;
                    row["标准回归系数(T值)"] = normalbs[i];
                    row["显著性排序"] = sortlist.FindIndex(_ => _.Item2 == list[i].Item2) + 1;
                    res.Rows.Add(row);
                }
                return res;
            }
            else
            {
                return null;
            }
        }
    }
}
