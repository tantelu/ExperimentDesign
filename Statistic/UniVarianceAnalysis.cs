using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExperimentDesign.Statistic
{
    /// <summary>
    /// 单变量方差分析 参考 数理统计与分析(陈希孺)335页 6.5.2单因数完全随机实验方差分析
    /// </summary>
    public class UniVarianceAnalysis
    {
        /// <summary>
        /// table的最后一列为响应指标（储量）
        /// </summary>
        /// <param name="table"></param>
        /// <param name="factorNames"></param>
        /// <returns></returns>
        public static List<UniVarianceAnalysis> FromTable(DataTable table)
        {
            List<UniVarianceAnalysis> vars = new List<UniVarianceAnalysis>();
            for (int i = 1; i < table.Columns.Count - 1; i++)
            {
                List<List<double>> values = new List<List<double>>();
                List<double> levels = new List<double>();
                for (int index = 0; index < table.Rows.Count; index++)
                {
                    var v = Convert.ToDouble(table.Rows[index][table.Columns[i]]);
                    var pos = levels.FindIndex(_ => Math.Abs(_ - v) < 0.0001);
                    if (pos < 0)
                    {
                        levels.Add(v);
                        values.Add(new List<double>());
                        pos = values.Count - 1;
                    }
                    var vol = Convert.ToDouble(table.Rows[index][table.Columns.Count - 1]);
                    values[pos].Add(vol);
                }
                vars.Add(new UniVarianceAnalysis(table.Columns[i].ColumnName, values));
            }
            return vars;
        }

        public static DataTable VarianceAnalysisTable(List<UniVarianceAnalysis> vars)
        {
            var se = vars[0].SS - vars.Sum(_ => _.SSA);
            var degrees = vars[0].DegreesOfSS - vars.Sum(_ => _.DegreesOfSSA);
            DataTable table = new DataTable();
            var col = table.Columns.Add($"因数", Type.GetType("System.String"));
            table.Columns.Add($"因数平方和(SSA)", Type.GetType("System.Double"));
            table.Columns.Add($"自由度(SSA)", Type.GetType("System.Double"));
            table.Columns.Add($"均方", Type.GetType("System.Double"));
            table.Columns.Add($"F比", Type.GetType("System.Double"));
            for (int i = 0; i < vars.Count; i++)
            {
                DataRow row = table.NewRow();
                row["因数"] = vars[i].FactorName;
                row["因数平方和(SSA)"] = vars[i].SSA;
                row["自由度(SSA)"] = vars[i].DegreesOfSSA;
                row["均方"] = vars[i].MSA;
                row["F比"] = vars[i].MSA / (se / degrees);
                table.Rows.Add(row);
            }
            {
                DataRow row = table.NewRow();
                row["因数"] = "误差";
                row["因数平方和(SSA)"] = se;
                row["自由度(SSA)"] = degrees;
                row["均方"] = se / degrees;
                table.Rows.Add(row);
            }
            return table;
        }

        public string FactorName { get; private set; }

        private List<List<double>> temp;

        public UniVarianceAnalysis(string factorName, List<List<double>> data)
        {
            FactorName = factorName;
            temp = new List<List<double>>(data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                temp.Add(data[i].ToList());
            }
        }

        private UniVarianceAnalysis()
        {

        }

        public double Average(int index)
        {
            return temp[index].Average();
        }

        public double AllAverage()
        {
            List<double> avgs = new List<double>();
            for (int i = 0; i < temp.Count; i++)
            {
                avgs.Add(temp[i].Average());
            }
            return avgs.Average();
        }

        public int DegreesOfSS
        {
            get
            {
                return temp.Sum(_ => _.Count) - 1;
            }
        }

        public int DegreesOfSSA
        {
            get
            {
                return temp.Count - 1;

            }
        }

        public int DegreesOfSSE
        {
            get
            {
                return DegreesOfSS - DegreesOfSSA;
            }
        }

        /// <summary>
        /// 总平方和
        /// </summary>
        public double SS
        {
            get
            {
                double sum = 0;
                double allavg = AllAverage();
                for (int i = 0; i < temp.Count; i++)
                {
                    for (int j = 0; j < temp[i].Count; j++)
                    {
                        sum += Math.Pow(temp[i][j] - allavg, 2);
                    }
                }
                return sum;
            }
        }

        /// <summary>
        /// 误差平方和
        /// </summary>
        public double SSE
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < temp.Count; i++)
                {
                    double avg = Average(i);
                    for (int j = 0; j < temp[i].Count; j++)
                    {
                        sum += Math.Pow(temp[i][j] - avg, 2);
                    }
                }
                return sum;
            }
        }

        /// <summary>
        /// 因数平方和
        /// </summary>
        public double SSA
        {
            get
            {
                return SS - SSE;
            }
        }

        public double MSA
        {
            get
            {
                return SSA / DegreesOfSSA;
            }
        }

        public double MSE
        {
            get
            {
                return SSE / DegreesOfSSE;
            }
        }
    }
}
