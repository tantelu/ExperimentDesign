using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace ExperimentDesign
{
    public class VariableData
    {
        [DisplayName("设计参数名")]
        ///对于参数输入界面的 EditorValue 如果其包含$  就是需要设计的参数
        public string Name { get; set; }

        [DisplayName("参数描述")]
        public string ParDescription { get; set; }

        [DisplayName("默认值")]
        public string BaseValue { get; set; }

        [DisplayName("分布")]
        public string Distribution { get; set; }

        [DisplayName("参数")]
        public IArgument Arguments { get; set; }

        public void Open(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JObject jo = JObject.Parse(json);
                this.Name = jo[nameof(Name)].ToString();
                this.ParDescription = jo[nameof(ParDescription)]?.ToString();
                this.BaseValue = jo[nameof(BaseValue)]?.ToString();
                this.Distribution = jo[nameof(Distribution)]?.ToString();
                var strargu = jo[nameof(Arguments)]?.ToString();
                if (!string.IsNullOrEmpty(strargu))
                {
                    JObject arguJ = JObject.Parse(strargu);
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    this.Arguments = assembly.CreateInstance(arguJ["Type"].ToString(), false, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, null, null, null) as IArgument;
                    Arguments.Open(arguJ["ArgumentData"].ToString());
                }
            }
        }

        public string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(Name));
            writer.WriteValue(Name);
            writer.WritePropertyName(nameof(ParDescription));
            writer.WriteValue(ParDescription);
            writer.WritePropertyName(nameof(BaseValue));
            writer.WriteValue(BaseValue);
            writer.WritePropertyName(nameof(Distribution));
            writer.WriteValue(Distribution);
            writer.WritePropertyName(nameof(Arguments));
            if (Arguments != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Type");
                writer.WriteValue(Arguments.GetType().FullName);
                writer.WritePropertyName("ArgumentData");
                writer.WriteValue(Arguments.Save());
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteValue(string.Empty);
            }
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = sw.GetStringBuilder().ToString();
            return jsonText;
        }
    }
}
