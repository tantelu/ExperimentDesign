using ExperimentDesign.WorkList;
using System.Drawing;
using System.Windows.Forms;

namespace WorkList.ExperimentDesign
{
    public class SgsWorkControl : WorkControl
    {
        public SgsWorkControl()
        {

        }

        protected override string WorkName => "序贯高斯模拟";

        protected override Bitmap Picture => global::ExperimentDesign.Properties.Resources.Sgs;

        public override void Run()
        {

        }

        protected override void ShowParamForm()
        {
            using (SgsEditorForm form = new SgsEditorForm())
            {
                form.InitForm(param);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.param.Clear();
                    this.param.AddRange(form.GetUncentainParam());
                }
            }
        }
    }
}
