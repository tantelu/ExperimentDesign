using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ExperimentDesign.General
{
    public class Design<T>
    {
        public static Design<T> GeneralDesign(object tag, string text)
        {
            if (tag is Design<T> oldt)
            {
                if (text.Contains("$"))
                {
                    return new Design<T>(oldt.Value, true, text, oldt.Id);
                }
                else
                {
                    return new Design<T>((T)Convert.ChangeType(text, typeof(T)), false, string.Empty, oldt.Id);
                }
            }
            else
            {
                if (tag == null)
                {
                    return GeneralDesign(text);
                }
                else
                {
                    if (text.Contains("$"))
                    {
                        return new Design<T>((T)Convert.ChangeType(tag, typeof(T)), true, text, Guid.NewGuid().ToString());
                    }
                    else
                    {
                        return new Design<T>((T)Convert.ChangeType(text, typeof(T)), false, string.Empty, Guid.NewGuid().ToString());
                    }
                }
            }
        }

        public static Design<T> GeneralDesign(string text)
        {
            if (text.Contains("$"))
            {
                return new Design<T>(default(T), true, text, Guid.NewGuid().ToString());
            }
            else
            {
                return new Design<T>((T)Convert.ChangeType(text, typeof(T)), false, string.Empty, Guid.NewGuid().ToString());
            }
        }

        public string Id { get; private set; }

        public string DesignName { get; private set; }

        public bool IsDesign { get; private set; }

        public T Value { get; private set; }

        private Design(T t, bool isdesign, string designName, string id)
        {
            Value = t;
            IsDesign = isdesign;
            DesignName = designName;
            Id = id;
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

        //public static implicit operator Design<T>(T value)
        //{
        //    return new Design<T>(value,Guid.NewGuid().ToString());
        //}

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

        public Design<T> Clone()
        {
            Design<T> newdesign = new Design<T>();
            newdesign.Id = Guid.NewGuid().ToString();
            newdesign.IsDesign = this.IsDesign;
            newdesign.DesignName = this.DesignName;
            newdesign.Value = this.Value.Copy();
            return newdesign;
        }
    }
}
