using System;
using System.IO;
using System.Text;

namespace ExperimentDesign.General
{
    public static class Gslib
    {
        public static float[] ReadGislib(string gslibfile, out int xcount, out int ycount, out int zcount)
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

        public static void WriteGislib(string gslibfile, float[] values, int xcount, int ycount, int zcount)
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
    }
}
