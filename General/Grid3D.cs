using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace ExperimentDesign.General
{
    public class Grid3D
    {
        [Description("网格X最小值")]
        public double Xmin { get; set; }

        [Description("网格X最大值")]
        public double Xmax { get; set; }

        [Description("网格Y最小值")]
        public double Ymin { get; set; }

        [Description("网格Y最大值")]
        public double Ymax { get; set; }

        [Description("网格Z最小值")]
        public double Zmin { get; set; }

        [Description("网格Z最大值")]
        public double Zmax { get; set; }

        [Description("X方向网格大小")]
        public double Xsize { get; set; }

        [Description("Y方向网格大小")]
        public double Ysize { get; set; }

        [Description("Z方向网格大小")]
        public double Zsize { get; set; }

        public int Xcount
        {
            get
            {
                return (int)((Xmax - Xmin) / Xsize);
            }
        }

        public int Ycount
        {
            get
            {
                return (int)((Ymax - Ymin) / Ysize);
            }
        }

        public int Zcount
        {
            get
            {
                return (int)((Zmax - Zmin) / Zsize);
            }
        }

        public void Open(string file)
        {
            var jsonText = File.ReadAllText(file, Encoding.UTF8);
            JObject jo = JObject.Parse(jsonText);
            Xmin = (double)jo[nameof(Xmin)];
            Xmax = (double)jo[nameof(Xmax)];
            Ymin = (double)jo[nameof(Ymin)];
            Ymax = (double)jo[nameof(Ymax)];
            Zmin = (double)jo[nameof(Zmin)];
            Zmax = (double)jo[nameof(Zmax)];
            Xsize = (double)jo[nameof(Xsize)];
            Ysize = (double)jo[nameof(Ysize)];
            Zsize = (double)jo[nameof(Zsize)];
        }

        //覆盖
        public void Save(string file)
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(Xmin));
            writer.WriteValue(Xmin);
            writer.WritePropertyName(nameof(Xmax));
            writer.WriteValue(Xmax);
            writer.WritePropertyName(nameof(Ymin));
            writer.WriteValue(Ymin);
            writer.WritePropertyName(nameof(Ymax));
            writer.WriteValue(Ymax);
            writer.WritePropertyName(nameof(Zmin));
            writer.WriteValue(Zmin);
            writer.WritePropertyName(nameof(Zmax));
            writer.WriteValue(Zmax);
            writer.WritePropertyName(nameof(Xsize));
            writer.WriteValue(Xsize);
            writer.WritePropertyName(nameof(Ysize));
            writer.WriteValue(Ysize);
            writer.WritePropertyName(nameof(Zsize));
            writer.WriteValue(Zsize);
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = sw.GetStringBuilder().ToString();
            File.WriteAllText(file, jsonText, Encoding.UTF8);
        }
    }
}
