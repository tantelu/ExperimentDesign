using ExperimentDesign.WorkList.Base;
using System.Collections.Generic;
using System.Drawing;

namespace ExperimentDesign.WorkList.WellFilter
{
    public class WellFilterWorkControl : WorkControl
    {
        public WellFilterWorkControl()
        {
            UpdateText("$*");
        }

        protected override string WorkName => "井数据筛选";

        protected override Bitmap Picture => Properties.Resources.Delete;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {

        }

        public override bool GetRunState(int index)
        {
            return true;
        }

        protected override void ShowParamForm()
        {
            using (WellFilterForm form = new WellFilterForm())
            {
                form.ShowDialog();
            }
        }

        public override string Save()
        {
            return string.Empty;
        }

        public override void Open(string str)
        {

        }
    }
}
