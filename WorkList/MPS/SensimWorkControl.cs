using ExperimentDesign.Uncertainty;
using ExperimentDesign.WorkList.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.MPS
{
    public class SensimWorkControl : WorkControl
    {
        private SnesimPar par;

        public SensimWorkControl()
        {

        }

        protected override string WorkName => "Snesim模拟";

        protected override Bitmap Picture => global::ExperimentDesign.Properties.Resources.Sgs;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            
        }

        public override bool GetRunState(int index)
        {
            return true;
        }

        protected override void ShowParamForm()
        {
            using (SensimRunForm form = new SensimRunForm())
            {
                form.InitForm(par);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    par = form.GetSgsPar();
                    var newparam = VariableData.ObjectToVariables(par);
                    UpdateText(newparam);
                }
            }
        }

        public override string Save()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(par));
            writer.WriteValue(par?.Save());
            writer.WriteEndObject();
            writer.Flush();
            return sw.GetStringBuilder().ToString();
        }

        public override void Open(string str)
        {
            JObject jobj = JObject.Parse(str);
            var parstr = jobj[nameof(par)]?.ToString();
            if (!string.IsNullOrEmpty(parstr))
            {
                par = new SnesimPar();
                par.Open(parstr);
            }
            if (par != null)
            {
                var newparam = VariableData.ObjectToVariables(par);
                UpdateText(newparam);
            }
        }

        public override IReadOnlyList<VariableData> GetUncentainParam()
        {
            return VariableData.ObjectToVariables(par);
        }
    }
}
