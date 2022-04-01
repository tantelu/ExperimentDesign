using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.LinearRegression;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ComponentModel;

namespace ExperimentDesign.Statistic
{
    //参考博客 https://blog.csdn.net/qq_43460172/article/details/98788494
    public class UniMultipleLineRegression
    {
        private Matrix<double> x;

        private Vector<double> y;

        private List<string> factors;

        //系数
        public double[] GetBs()
        {
            return MultipleRegression.QR(x.Transpose().ToColumnArrays(), y.ToArray(), true).Skip(1).ToArray();
        }

        public List<RegressionParam> GetParam()
        {
            List<RegressionParam> pars = new List<RegressionParam>();
            var coeff = MultipleRegression.QR(x.Transpose().ToColumnArrays(), y.ToArray(), true).ToList();
            var factorArray = x.ToColumnArrays();
            for (int i = 0; i < coeff.Count; i++)
            {
                RegressionParam param = new RegressionParam();
                param.Coefficient = coeff[i];
                if (i > 0)
                {
                    param.Name = factors[i - 1];
                    param.Min = factorArray[i - 1].Min();
                    param.Max = factorArray[i - 1].Max();
                }
                else
                {
                    param.Name = "常数项";
                    param.Min = coeff[i];
                    param.Max = coeff[i];
                }
                pars.Add(param);
            }
            return pars;
        }

        public IReadOnlyList<string> GetParamNames()
        {
            return factors;
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

        public DataTable GetTable()
        {
            DataTable res = new DataTable();
            var col = res.Columns.Add($"因数", Type.GetType("System.String"));
            res.Columns.Add($"回归系数", Type.GetType("System.Double"));
            res.Columns.Add($"回归系数标准差", Type.GetType("System.Double"));
            res.Columns.Add($"标准回归系数(T值)", Type.GetType("System.Double"));
            res.Columns.Add($"显著性排序", Type.GetType("System.Double"));
            var bs = this.GetBs();
            var normalbs = this.GetNormalBs();
            List<Tuple<double, int>> list = new List<Tuple<double, int>>();
            for (int i = 0; i < normalbs.Length; i++)
            {
                list.Add(new Tuple<double, int>(Math.Abs(normalbs[i]), i));
            }
            var sortlist = list.OrderByDescending(_ => _.Item1).ToList();
            for (int i = 0; i < bs.Length; i++)
            {
                DataRow row = res.NewRow();
                var sx = this.GetSx(i);
                row["因数"] = factors[i];
                row["回归系数"] = bs[i];
                row["回归系数标准差"] = sx;
                row["标准回归系数(T值)"] = normalbs[i];
                row["显著性排序"] = sortlist.FindIndex(_ => _.Item2 == list[i].Item2) + 1;
                res.Rows.Add(row);
            }
            return res;
        }

        public static UniMultipleLineRegression Regression(DataTable table)
        {
            if (table.Columns.Count > 2)
            {
                UniMultipleLineRegression data = new UniMultipleLineRegression();
                data.x = CreateMatrix.Dense(table.Rows.Count, table.Columns.Count - 2, (int i, int j) => { return Convert.ToDouble(table.Rows[i][j + 1]); });
                data.y = CreateVector.Dense(table.Rows.Count, (int i) => { return Convert.ToDouble(table.Rows[i][table.Columns.Count - 1]); });
                data.factors = new List<string>();
                for (int i = 1; i < table.Columns.Count - 1; i++)
                {
                    data.factors.Add(table.Columns[i].ColumnName);
                }
                return data;
            }
            else
            {
                return null;
            }
        }

        public static DataTable VarianceAnalysisTable(DataTable table)
        {
            return Regression(table)?.GetTable();
        }

        /// <summary>
        /// 因子标准系数
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<BarData> GetBars()
        {
            List<BarData> bars = new List<BarData>();
            var bs = this.GetBs();
            var normalbs = this.GetNormalBs();
            List<Tuple<double, int>> list = new List<Tuple<double, int>>();
            for (int i = 0; i < normalbs.Length; i++)
            {
                list.Add(new Tuple<double, int>(Math.Abs(normalbs[i]), i));
            }
            var sortlist = list.OrderByDescending(_ => _.Item1).ToList();
            for (int i = 0; i < bs.Length; i++)
            {
                BarData bar = new BarData(factors[i], Math.Abs(normalbs[i]));
                bars.Add(bar);
            }
            return bars.OrderBy(_ => _.FactorLevel).ToList();

        }
    }

    public class BarData
    {
        double level;

        string name;

        public BarData(string name, double level)
        {
            this.name = name;
            this.level = level;
        }
        public string FactorName
        {
            get { return name; }
            set { name = value; }
        }

        public double FactorLevel
        {
            get { return level; }
            set { level = value; }
        }
    }

    public class RegressionParam
    {
        [DisplayName("参数名")]
        public string Name { get; set; }

        [DisplayName("回归系数")]
        public double Coefficient { get; set; }

        [DisplayName("最小值")]
        public double Min { get; set; }

        [DisplayName("最大值")]
        public double Max { get; set; }
    }
}
