using System.Windows.Forms;

namespace WorkList.ExperimentDesign
{
    public class GridEditWorkControl : WorkControl
    {
        public GridEditWorkControl()
        {
            //this.layoutControlItem3.Text = WorkName;
        }

        protected override string WorkName => "创建简单三维网格";
        

        protected override void Run()
        {

        }

        protected override void ShowParamForm()
        {
            using (GridEditForm form = new GridEditForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SetUncentainParam(form.GetUncentainParam()); 
                }
            }
        }
    }
}
