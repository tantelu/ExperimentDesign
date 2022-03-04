using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign
{
    public class DesignAlgorithm
    {
        /// <summary>
        /// 中心复核设计
        /// 该方法借鉴Petrel软件
        /// Minitab帮助文档https://support.minitab.com/zh-cn/minitab/21/help-and-how-to/statistical-modeling/doe/how-to/response-surface/create-response-surface-design/create-central-composite-design/examine-the-design/all-statistics/#fnsrc_1
        /// </summary>
        /// <param name="factors"></param>
        /// <param name="designType"></param>
        /// <returns></returns>
        public static CenteralCompositeTable GenerateComposite(uint factors, CenteralCompositeType designType)
        {
            CenteralCompositeTable table = new CenteralCompositeTable(factors, designType);
            return table;
        }

        /// <summary>
        /// Box-Behnken设计
        /// </summary>
        /// <param name="factors"></param>
        /// <returns></returns>
        public static BoxBehnkenTable GenerateBoxBehnken(uint factors)
        {
            BoxBehnkenTable table = new BoxBehnkenTable(factors);
            return table;
        }

        /// <summary>
        /// Plackett-Burman原理来源于实验设计第六版 实现参考Pertrel软件
        /// </summary>
        /// <param name="factors"></param>
        /// <returns></returns>
        public static PlackettBurmanTable GeneratePlackettBurman(uint factors)
        {
            PlackettBurmanTable table = new PlackettBurmanTable(factors);
            return table;
        }

        /// <summary>
        /// 正交设计,配置文件定义所有可行的正交表
        /// </summary>
        /// <param name="factors"></param>
        /// <param name="levels"></param>
        /// <returns></returns>
        public static OrthGuideTable GenerateOrthGuide(uint factors, uint levels)
        {
            OrthGuideTable table = new OrthGuideTable(factors, levels);
            return table;
        }
    }

    public class CenteralCompositeTable : Table
    {
        char[,] data;

        Dictionary<char, double> maps;

        public CenteralCompositeTable(uint factors, CenteralCompositeType designType)
        {
            uint samples = 1 + (uint)Math.Pow(2, factors) + 2 * factors;
            data = new char[samples, factors];
            for (int i = 0; i < samples; i++)
            {
                for (int j = 0; j < factors; j++)
                {
                    data[i, j] = '0';
                }
            }
            //轴点中心点
            for (uint j = 0; j < factors; j++)
            {
                SetValue(0, j, '0');
            }
            //立方点
            FullArray(factors, 1, factors - 1, this);
            uint start = 1 + (uint)Math.Pow(2, factors);
            if (designType == CenteralCompositeType.Inscribed)
            {
                //轴点
                for (uint j = 0; j < factors; j++)
                {
                    SetValue(start++, j, 'L');
                    SetValue(start++, j, 'H');
                }
                double level = 1 / Math.Pow(Math.Pow(2, factors), 0.25);//试验设计与分析 第六版369页
                maps = new Dictionary<char, double>() { { '0', 0 }, { '-', -level }, { '+', level }, { 'L', -1 }, { 'H', 1 } };
            }
            else
            {
                //轴点
                for (uint j = 0; j < factors; j++)
                {
                    SetValue(start++, j, '-');
                    SetValue(start++, j, '+');
                }
                maps = new Dictionary<char, double>() { { '0', 0 }, { '-', -1 }, { '+', 1 } };
            }
        }

        public override int GetTestCount()
        {
            return data.GetLength(0);
        }

        public override object GetValue(int rowIndex, int colIndex)
        {
            return data[rowIndex, colIndex];
        }

        private void SetValue(uint rowIndex, uint colIndex, char _value)
        {
            data[rowIndex, colIndex] = _value;
        }

        public double[] GetRow(int rowIndex)
        {
            double[] row = new double[data.GetLength(0)];
            for (int i = 0; i < data.GetLength(1); i++)
            {
                row[i] = maps[data[rowIndex, i]];
            }
            return row;
        }

        private void FullArray(uint factors, uint start, uint curCol, CenteralCompositeTable table)
        {
            uint rows1 = start + (uint)Math.Pow(2, curCol);
            uint rows2 = start + (uint)Math.Pow(2, curCol + 1);
            for (uint i = start; i < rows1; i++)
            {
                table.SetValue(i, curCol, '-');
            }
            for (uint i = rows1; i < rows2; i++)
            {
                table.SetValue(i, curCol, '+');
            }
            if (curCol > 0)
            {
                FullArray(factors, start, curCol - 1, table);
                FullArray(factors, rows1, curCol - 1, table);
            }
            else
            {
                return;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    sb.Append(data[i, j]);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            if (maps.ContainsKey('L'))
            {
                sb.AppendLine();
                sb.AppendLine("Level values:");
                sb.AppendLine("=============");
                sb.AppendLine($"L:  {maps['L']}");
                sb.AppendLine($"-:  {maps['-']}");
                sb.AppendLine($"+:  {maps['+']}");
                sb.AppendLine($"H:  {maps['H']}");
            }
            return sb.ToString();
        }
    }

    public class BoxBehnkenTable : Table
    {
        char[,] data;

        Dictionary<char, double> maps;

        public BoxBehnkenTable(uint factors)
        {
            var file = Path.Combine(Application.StartupPath, "BBTable.txt");
            StreamReader read = new StreamReader(file);
            while (!read.EndOfStream)
            {
                var test = read.ReadLine();
                if (test.Contains("n="))
                {
                    var splits = test.Split(new string[] { "n=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (splits.Length > 1)
                    {
                        int num = 0;
                        if (int.TryParse(splits[0].Trim(), out num) && num == factors)
                        {
                            int n = 0;
                            if (int.TryParse(splits[1].Trim(), out n) && n > 0)
                            {
                                data = new char[n + 1, factors];
                                for (int j = 0; j < factors; j++)
                                {
                                    data[0, j] = '0';
                                }
                                for (int i = 1; i < n + 1; i++)
                                {
                                    var oneRow = read.ReadLine();
                                    var indexs = oneRow.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                    for (int j = 0; j < indexs.Length; j++)
                                    {
                                        data[i, j] = indexs[j] == "0" ? '0' : indexs[j] == "1" ? '+' : '-';
                                    }
                                }
                            }
                        }
                    }
                }
            }
            maps = new Dictionary<char, double>() { { '0', 0 }, { '-', -1 }, { '+', 1 } };
        }

        public override int GetTestCount()
        {
            return data.GetLength(0);
        }

        public override object GetValue(int rowIndex, int colIndex)
        {
            return data[rowIndex, colIndex];
        }

        public double[] GetRow(int rowIndex)
        {
            double[] row = new double[data.GetLength(0)];
            for (int i = 0; i < data.GetLength(1); i++)
            {
                row[i] = maps[data[rowIndex, i]];
            }
            return row;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    sb.Append(data[i, j]);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            if (maps.ContainsKey('L'))
            {
                sb.AppendLine();
                sb.AppendLine("Level values:");
                sb.AppendLine("=============");
                sb.AppendLine($"L:  {maps['L']}");
                sb.AppendLine($"-:  {maps['-']}");
                sb.AppendLine($"+:  {maps['+']}");
                sb.AppendLine($"H:  {maps['H']}");
            }
            return sb.ToString();
        }
    }

    public class PlackettBurmanTable : Table
    {
        char[,] data;

        Dictionary<char, double> maps;

        public PlackettBurmanTable(uint factors)
        {
            if (factors < 4)
            {
                var temp = new char[4, 3] { { '-', '-', '-' }, { '+', '-', '+' }, { '-', '+', '+' }, { '+', '+', '-' } };
                data = new char[4, factors];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < factors; j++)
                    {
                        data[i, j] = temp[i, j];
                    }
                }
            }
            else if (factors < 8)
            {
                var temp = new char[8, 7] { {'-', '-' ,'-', '-' ,'-', '-', '-' },{'+', '-', '+', '-', '+', '-', '+' },
                                             { '-' ,'+', '+' ,'-', '-', '+','+'},{ '+', '+', '-', '-', '+', '+', '-'},
                                             { '-', '-', '-', '+', '+', '+', '+'},{ '+', '-' ,'+', '+' ,'-', '+' ,'-'},
                                             {'-', '+' ,'+' ,'+' ,'+', '-', '-' },{ '+', '+', '-' ,'+' ,'-', '-', '+'} };
                data = new char[8, factors];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < factors; j++)
                    {
                        data[i, j] = temp[i, j];
                    }
                }
            }
            else if (factors < 12)
            {
                var temp = new char[12, 11] { { '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-',},
                                                { '+', '-', '+', '-', '-', '-', '+', '+', '+', '-', '+',},
                                                { '+', '+', '-', '+', '-', '-', '-', '+', '+', '+', '-',},
                                                {'-', '+', '+', '-', '+', '-', '-', '-', '+', '+','+', },
                                                { '+', '-', '+', '+', '-', '+', '-', '-', '-', '+','+',},
                                                {'+', '+', '-', '+', '+', '-', '+', '-', '-', '-','+', },
                                                { '+', '+', '+', '-', '+', '+', '-', '+', '-', '-','-',},
                                                { '-', '+', '+', '+', '-', '+', '+', '-', '+', '-','-',},
                                                {'-', '-', '+', '+', '+', '-', '+', '+', '-', '+','-', },
                                                {'-', '-', '-', '+', '+', '+', '-', '+', '+', '-','+', },
                                                { '+', '-', '-', '-', '+', '+', '+', '-', '+', '+','-',},
                                                { '-', '+', '-', '-', '-', '+', '+', '+', '-', '+','+',},};
                data = new char[12, factors];
                for (int i = 0; i < factors; i++)
                {
                    for (int j = 0; j < factors; j++)
                    {
                        data[i, j] = temp[i, j];
                    }
                }
            }
            else if (factors < 16)
            {
                char[] temp = new char[15] { '+', '+', '+', '+', '-', '+', '-', '+', '+', '-', '-', '+', '-', '-', '-' };
                data = new char[16, factors];
                for (int j = 0; j < factors; j++)
                {
                    data[0, j] = '-';
                }
                for (int i = 1; i < factors; i++)
                {
                    for (int j = 0; j < factors; j++)
                    {
                        int jshift = j - i + 1;
                        if (jshift < 0)
                        {
                            jshift += 15;
                        }
                        data[i, j] = temp[jshift];
                    }
                }
            }
            else if (factors < 20)
            {
                char[] temp = new char[19] { '+', '+', '-', '-', '+', '+', '-', '+', '-', '+', '-', '+', '-', '-', '-', '-', '+', '+', '-' };
                data = new char[20, factors];
                for (int j = 0; j < factors; j++)
                {
                    data[0, j] = '-';
                }
                for (int i = 1; i < factors; i++)
                {
                    for (int j = 0; j < factors; j++)
                    {
                        int jshift = j - i + 1;
                        if (jshift < 0)
                        {
                            jshift += 19;
                        }
                        data[i, j] = temp[jshift];
                    }
                }
            }
            else
            {

            }
            maps = new Dictionary<char, double>() { { '-', -1 }, { '+', 1 } };
        }

        public override int GetTestCount()
        {
            return data.GetLength(0);
        }

        public override object GetValue(int rowIndex, int colIndex)
        {
            return data[rowIndex, colIndex];
        }

        public double[] GetRow(int rowIndex)
        {
            double[] row = new double[data.GetLength(0)];
            for (int i = 0; i < data.GetLength(1); i++)
            {
                row[i] = maps[data[rowIndex, i]];
            }
            return row;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    sb.Append(data[i, j]);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            if (maps.ContainsKey('L'))
            {
                sb.AppendLine();
                sb.AppendLine("Level values:");
                sb.AppendLine("=============");
                sb.AppendLine($"L:  {maps['L']}");
                sb.AppendLine($"-:  {maps['-']}");
                sb.AppendLine($"+:  {maps['+']}");
                sb.AppendLine($"H:  {maps['H']}");
            }
            return sb.ToString();
        }
    }

    public class OrthGuideTable : Table
    {
        uint[,] data;

        public OrthGuideTable(uint factors, uint levels)
        {
            bool find = false;
            var file = Path.Combine(Application.StartupPath, "OrthTable.txt");
            StreamReader read = new StreamReader(file);
            while (!read.EndOfStream)
            {
                var test = read.ReadLine();
                if (test.Contains("n="))
                {
                    var splits = test.Split(new string[] { "n=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (splits.Length > 1)
                    {
                        int n = 0;
                        if (int.TryParse(splits[1], out n))
                        {
                            data = new uint[n, factors];
                            if (splits[0].Count(_ => _ == '^') == 1)
                            {
                                var splits2 = splits[0].Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
                                if (splits2.Length > 1)
                                {
                                    int infactor = 0;
                                    int inlevel = 0;
                                    if (int.TryParse(splits2[1].Trim(), out infactor) && factors == infactor &&
                                        int.TryParse(splits2[0].Trim(), out inlevel) && inlevel == levels)
                                    {
                                        for (int i = 0; i < n; i++)
                                        {
                                            var info = read.ReadLine().Trim();
                                            for (int col = 0; col < info.Length; col++)
                                            {
                                                find = true;
                                                data[i, col] = Convert.ToUInt32(info[col].ToString());
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            read.Close();
            read.Dispose();
        }

        public override int GetTestCount()
        {
            return data.GetLength(0);
        }

        public override object GetValue(int rowIndex, int colIndex)
        {
            return data[rowIndex, colIndex];
        }

        public double[] GetRow(int rowIndex)
        {
            double[] row = new double[data.GetLength(0)];
            for (int i = 0; i < data.GetLength(1); i++)
            {
                row[i] = data[rowIndex, i];
            }
            return row;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    sb.Append(data[i, j]);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    public abstract class Table
    {
        public abstract int GetTestCount();

        public abstract object GetValue(int rowIndex, int colIndex);

        public virtual DataTable ToDataTable(FactorInfo info)
        {
            var factorNames = info.GetFactorNames();
            var cols = factorNames.Count;
            DataTable table = new DataTable();
            table.Columns.Add($"实验\\因数", Type.GetType("System.String"));
            for (int col = 0; col < cols; col++)
            {
                var name = factorNames[col];
                table.Columns.Add(name, Type.GetType("System.String"));
            }
            for (int i = 0; i < GetTestCount(); i++)
            {
                DataRow row = table.NewRow();
                row["实验\\因数"] = $"实验{i + 1}";
                for (int col = 0; col < cols; col++)
                {
                    row[factorNames[col]] = info.GetValue(factorNames[col], GetValue(i, col));
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }

    public class FactorInfo
    {
        private Dictionary<string, Dictionary<object, object>> factormaps;

        public FactorInfo(Dictionary<string, Dictionary<object, object>> factormaps)
        {
            this.factormaps = factormaps;
        }

        public object GetValue(string factorName, object level)
        {
            var temp = factormaps[factorName];
            return temp[level];
        }

        public List<string> GetFactorNames()
        {
            return factormaps.Keys.ToList();
        }
    }

    public enum CenteralCompositeType
    {
        FaceCentered = 0,
        Inscribed = 1,
    }
}
