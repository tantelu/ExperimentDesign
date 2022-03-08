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

        protected override void Run()
        {

        }

        protected override void ShowParamForm()
        {
            
        }
    }
}
