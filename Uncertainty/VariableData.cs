using ExperimentDesign.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ExperimentDesign.Uncertainty
{
    public class VariableData
    {
        [DisplayName("设计参数名")]
        ///对于参数输入界面的 EditorValue 如果其包含$就是需要设计的参数
        public string Name { get; set; }

        [DisplayName("参数描述")]
        public string ParDescription { get; set; }

        [DisplayName("默认值")]
        public object BaseValue { get; set; }

        [DisplayName("分布")]
        public string Distribution { get; set; }

        [DisplayName("参数")]
        public IArgument Arguments { get; set; }

        /// <summary>
        /// 对应WorkControl的名字
        /// </summary>
        [Browsable(false)]
        public string WorkControlTypeName { get; set; }

        /// <summary>
        /// 在WorkControl中参数的顺序
        /// </summary>
        [Browsable(false)]
        public int CtrlIndex { get; set; }

        [Browsable(false)]
        public string Id { get; set; }

        public void CopyFromOther(VariableData other)
        {
            this.Arguments = other.Arguments;
            this.BaseValue = other.BaseValue;
            this.Distribution = other.Distribution;
            this.WorkControlTypeName = other.WorkControlTypeName;
            this.CtrlIndex = other.CtrlIndex;
        }

        public virtual void Open(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                JObject jo = JObject.Parse(json);
                this.Name = jo[nameof(Name)].ToString();
                this.ParDescription = jo[nameof(ParDescription)]?.ToString();
                this.BaseValue = jo[nameof(BaseValue)]?.ToString();
                this.Distribution = jo[nameof(Distribution)]?.ToString();
                this.CtrlIndex = (int)jo[nameof(CtrlIndex)];
                this.WorkControlTypeName = jo[nameof(WorkControlTypeName)]?.ToString();
                this.Id = jo[nameof(Id)]?.ToString();
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

        public virtual string Save()
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
            writer.WritePropertyName(nameof(CtrlIndex));
            writer.WriteValue(CtrlIndex);
            writer.WritePropertyName(nameof(WorkControlTypeName));
            writer.WriteValue(WorkControlTypeName);
            writer.WritePropertyName(nameof(Id));
            writer.WriteValue(Id);
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

        public static List<VariableData> ObjectToVariables(object obj)
        {
            List<VariableData> datas = new List<VariableData>();
            var allproperties = obj.GetType().GetProperties();
            var ty = typeof(Design<>);
            var propeties = allproperties.Where(_ => _.GetCustomAttribute<DescriptionAttribute>() != null).ToList();
            foreach (var property in propeties)
            {
                var propertyObj = property.GetValue(obj);
                if (propertyObj != null)
                {
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == ty)
                    {
                        var designProperty = property.PropertyType.GetProperty("IsDesign", typeof(bool));
                        if (designProperty != null && (bool)designProperty.GetValue(propertyObj))
                        {
                            VariableData data = new VariableData();
                            data.ParDescription = property.GetCustomAttribute<DescriptionAttribute>().Description;
                            var nameProperty = property.PropertyType.GetProperty("DesignName", typeof(string));
                            data.Name = ((string)nameProperty.GetValue(propertyObj)).Trim();
                            var idProperty = property.PropertyType.GetProperty("Id", typeof(string));
                            data.Id = ((string)idProperty.GetValue(propertyObj));
                            var valueProperty = property.PropertyType.GetProperty("Value", property.PropertyType.GetGenericArguments()[0]);
                            data.BaseValue = valueProperty.GetValue(propertyObj);
                            datas.Add(data);
                        }
                    }
                }
            }
            var grouplists = allproperties.Where(_ => _.GetCustomAttribute<GroupListAttribute>() != null).ToList();
            foreach (var item in grouplists)
            {
                var propertyObj = item.GetValue(obj);
                if (propertyObj != null)
                {
                    var countProperty = item.PropertyType.GetProperty("Count", typeof(int));
                    var getProperty = item.PropertyType.GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
                    if (countProperty != null && getProperty != null)
                    {
                        int count = (int)countProperty.GetValue(propertyObj);
                        for (int i = 0; i < count; i++)
                        {
                            var listobj = getProperty.Invoke(propertyObj, new object[] { i });
                            var nextdatas = ObjectToVariables(listobj);
                            datas.AddRange(nextdatas);
                        }
                    }
                }
            }
            var groups = allproperties.Where(_ => _.GetCustomAttribute<GroupAttribute>() != null).ToList();
            foreach (var item in groups)
            {
                var propertyObj = item.GetValue(obj);
                if (propertyObj != null)
                {
                    var nextdatas = ObjectToVariables(propertyObj);
                    datas.AddRange(nextdatas);
                }
            }
            return datas;
        }
    }
}
