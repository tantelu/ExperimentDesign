using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ExperimentDesign.General
{
    public class Design<T>
    {
        public string Id { get; private set; }

        public string DesignName { get; private set; }

        public bool IsDesign { get; private set; }

        public T Value { get; private set; }

        public Design(T t, string designName)
        {
            Value = t;
            IsDesign = true;
            DesignName = designName;
            Id = Guid.NewGuid().ToString();
        }

        public Design(string designName)
        {
            Value = default(T);
            IsDesign = true;
            DesignName = designName;
            Id = Guid.NewGuid().ToString();
        }

        public Design(T t)
        {
            DesignName = string.Empty;
            IsDesign = false;
            Value = t;
            Id = Guid.NewGuid().ToString();
        }

        internal Design()
        {

        }

        public string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(Id));
            writer.WriteValue(Id);
            writer.WritePropertyName(nameof(DesignName));
            writer.WriteValue(DesignName);
            writer.WritePropertyName(nameof(IsDesign));
            writer.WriteValue(IsDesign);
            writer.WritePropertyName(nameof(Value));
            writer.WriteValue(Value);
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public void Open(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JObject jo = JObject.Parse(json);
                Id = (jo[nameof(Id)]?.ToString());
                DesignName = (jo[nameof(DesignName)]?.ToString());
                IsDesign = (bool)(jo[nameof(IsDesign)]);
                Value = jo[nameof(Value)] != null ? (T)Convert.ChangeType(jo[nameof(Value)], typeof(T)) : default(T);
            }
        }

        public static implicit operator Design<T>(T value)
        {
            return new Design<T>(value);
        }

        public static implicit operator T(Design<T> value)
        {
            return value.Value;
        }

        public override string ToString()
        {
            if (IsDesign)
            {
                return DesignName;
            }
            else
            {
                return Value.ToString();
            }
        }
    }
}
