using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExperimentDesign.General
{
    public static class Gslib
    {
        public static float[] ReadOut(string gslibfile, out int xcount, out int ycount, out int zcount)
        {
            using (FileStream fileStream = new FileStream(gslibfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                StreamReader reader = new StreamReader(fileStream);
                reader.ReadLine();
                var first = reader.ReadLine();
                var gridcount = first.Split(new string[] { "x", "(", ")", " " }, StringSplitOptions.RemoveEmptyEntries);
                xcount = Convert.ToInt32(gridcount[1]);
                ycount = Convert.ToInt32(gridcount[2]);
                zcount = Convert.ToInt32(gridcount[3]);
                float[] fs = new float[xcount * ycount * zcount];
                reader.ReadLine();
                int index = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var strs = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length > 0 && strs[0].Trim().Length > 0)
                    {
                        float facie = Convert.ToSingle(strs[0].Trim());
                        fs[index++] = facie;
                    }
                }
                reader.Close();
                reader.Dispose();
                return fs;
            }
        }

        public static float[] ReadSgems(string gslibfile, out int xcount, out int ycount, out int zcount)
        {
            using (FileStream fileStream = new FileStream(gslibfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                StreamReader reader = new StreamReader(fileStream);
                var first = reader.ReadLine();
                var gridcount = first.Split(new string[] { "x", "(", ")", " " }, StringSplitOptions.RemoveEmptyEntries);
                xcount = Convert.ToInt32(gridcount[1]);
                ycount = Convert.ToInt32(gridcount[2]);
                zcount = Convert.ToInt32(gridcount[3]);
                reader.ReadLine();
                float[] fs = new float[xcount * ycount * zcount];
                reader.ReadLine();
                int index = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var strs = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length > 0 && strs[0].Trim().Length > 0)
                    {
                        float facie = Convert.ToSingle(strs[0].Trim());
                        fs[index++] = facie;
                    }
                }
                reader.Close();
                reader.Dispose();
                return fs;
            }
        }

        public static void WriteOut(string gslibfile, float[] values, int xcount, int ycount, int zcount)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FacieCtrol SGSIM Realizations");
            sb.AppendLine($"1  {xcount}   {ycount}  {zcount}");
            sb.AppendLine($"value");
            for (int i = 0; i < values.Length; i++)
            {
                sb.AppendLine(values[i].ToString("f8"));
            }
            File.WriteAllText(gslibfile, sb.ToString(), Encoding.UTF8);
        }

        public static void WriteSgems(string gslibfile, float[] values, int xcount, int ycount, int zcount)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Sgems ({xcount}x{ycount}x{zcount})");
            sb.AppendLine($"1");
            sb.AppendLine($"value");
            for (int i = 0; i < values.Length; i++)
            {
                sb.AppendLine(values[i].ToString("f8"));
            }
            File.WriteAllText(gslibfile, sb.ToString(), Encoding.UTF8);
        }

        public static void WriteWellPoint(string file, IList<PointSet> sets)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FacieCtrol SGSIM Realizations");
            sb.AppendLine($"5");
            sb.AppendLine($"x");
            sb.AppendLine($"y");
            sb.AppendLine($"z");
            sb.AppendLine($"v");
            sb.AppendLine($"id");
            foreach (var item in sets)
            {
                sb.AppendLine($"{item.X} {item.Y} {item.Z} {item.V} {item.Id}");
            }
            File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
        }

        public static IList<PointSet> ReadWellPoint(string file)
        {
            List<PointSet> sets = new List<PointSet>();
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                StreamReader reader = new StreamReader(fileStream);
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                var first = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var strs = line.Split(new string[] { " ",",", "，","；", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length == 5)
                    {
                        PointSet set = new PointSet(Convert.ToInt32(strs[0]), Convert.ToInt32(strs[1]),
                            Convert.ToInt32(strs[2]), Convert.ToInt32(strs[3]));
                        set.Id = Convert.ToInt32(strs[4]);
                        sets.Add(set);
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return sets;
        }
    }
}
