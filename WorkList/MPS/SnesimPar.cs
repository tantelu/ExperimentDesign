using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ExperimentDesign.WorkList.MPS
{
    public class SnesimPar
    {
        public string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            //writer.WritePropertyName(nameof(KrigType));
            //writer.WriteValue((int)KrigType);
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public void Open(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JObject jo = JObject.Parse(json);
                
            }
        }
    }
}
