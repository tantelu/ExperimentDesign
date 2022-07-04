using DevExpress.XtraEditors;
using ExperimentDesign.General;
using ExperimentDesign.Uncertainty;
using ExperimentDesign.WorkList.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Volume
{
    public class ConnectVolumnWorkControl : WorkControl
    {
        public ConnectVolumnPar ConnectPar { get; set; }

        protected override string WorkName => "连通体积";

        protected override Bitmap Picture => Properties.Resources.Volume;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            float[] volumns;
            if (ConnectPar == null)
            {
                return;
            }

            string gridfile = Path.Combine(Main.GetWorkPath(), $"{index}", $"{nameof(Grid3D)}.json");
            if (!File.Exists(gridfile))
            {
                XtraMessageBox.Show("未找到工区网格定义文件");
                return;
            }
            Grid3D grid = new Grid3D();
            grid.Open(gridfile);
            //计算方式暂时不实现。
        }

        public override bool GetRunState(int index)
        {
            return true;
        }

        public override string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(ConnectPar));
            writer.WriteValue(ConnectPar?.Save());
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public override void Open(string str)
        {
            JObject jobj = JObject.Parse(str);
            var parstr = jobj[nameof(ConnectPar)]?.ToString();
            if (!string.IsNullOrEmpty(parstr))
            {
                ConnectPar = new ConnectVolumnPar();
                ConnectPar.Open(parstr);
            }
            var newparam = VariableData.ObjectToVariables(ConnectPar);
            UpdateText(newparam);
        }

        public override IReadOnlyList<VariableData> GetUncentainParam()
        {
            return VariableData.ObjectToVariables(ConnectPar);
        }

        protected override void ShowParamForm()
        {
            using (ConnectVolumnForm form = new ConnectVolumnForm())
            {
                form.SetVolumnCalPar(ConnectPar);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    ConnectPar = form.GetConnectVolumnPar();
                    var newparam = VariableData.ObjectToVariables(ConnectPar);
                    UpdateText(newparam);
                }
            }
        }
    }

    public class ConnectVolumnPar
    {
        /// <summary>
        /// 0-按相连通 1-与指定网格连通
        /// </summary>
        public int VolType { get; set; }

        /// <summary>
        /// 连通的相类型
        /// </summary>
        public List<int> Facies { get; set; }

        /// <summary>
        /// 连通的体元所在的索引(井粗化后的网格)
        /// </summary>
        public List<int> Indexs { get; set; }

        public void SetFacies(string input)
        {
            if (Facies?.Count > 0)
            {
                Facies.Clear();
            }
            else
            {
                Facies = new List<int>();
            }
            var strs = input.Split(new string[] { ";", ",", " ", "，", "；" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in strs)
            {
                Facies.Add(Convert.ToInt32(str));
            }
        }

        public string GetFacies()
        {
            if (Facies?.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Facies)
                {
                    sb.Append(item.ToString());
                    sb.Append(";");
                }
                return sb.ToString(0, sb.Length - 1);
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetIndexs()
        {
            if (Indexs?.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Indexs)
                {
                    sb.Append(item.ToString());
                    sb.Append(";");
                }
                return sb.ToString(0, sb.Length - 1);
            }
            else
            {
                return string.Empty;
            }
        }

        public void SetIndex(string input)
        {
            if (Indexs?.Count > 0)
            {
                Indexs.Clear();
            }
            else
            {
                Indexs = new List<int>();
            }
            var strs = input.Split(new string[] { ";", ",", " ", "，", "；" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in strs)
            {
                Indexs.Add(Convert.ToInt32(str));
            }
        }

        public string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(VolType));
            writer.WriteValue(VolType);
            writer.WritePropertyName(nameof(Facies));
            if (Facies?.Count > 0)
            {
                writer.WriteValue(JsonConvert.SerializeObject(Facies));
            }
            else
            {
                writer.WriteValue(string.Empty);
            }
            writer.WritePropertyName(nameof(Facies));
            if (Indexs?.Count > 0)
            {
                writer.WriteValue(JsonConvert.SerializeObject(Indexs));
            }
            else
            {
                writer.WriteValue(string.Empty);
            }
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public void Open(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JObject jo = JObject.Parse(json);
                VolType = (int)jo[nameof(VolType)];
                var fs = jo[nameof(Facies)]?.ToString();
                if (!string.IsNullOrEmpty(fs))
                {
                    Facies = JsonConvert.DeserializeObject<List<int>>(fs);
                }
                var ins = jo[nameof(Indexs)]?.ToString();
                if (!string.IsNullOrEmpty(ins))
                {
                    Indexs = JsonConvert.DeserializeObject<List<int>>(ins);
                }
            }
        }
    }
}
