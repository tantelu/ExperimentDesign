using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WorkList.ExperimentDesign
{
    public class GridEditWorkControl : WorkControl
    {
        public GridEditWorkControl()
        {

        }

        protected override string WorkName => "创建简单三维网格";

        protected override Bitmap Picture => global::ExperimentDesign.Properties.Resources.Grid;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            string file = Path.Combine(Main.GetWorkPath(), $"{nameof(GridEditWorkControl)}.json");
            var str = Save();
            File.WriteAllText(file, str, System.Text.Encoding.UTF8);
        }

        public override bool GetRunState()
        {
            string file = Path.Combine(Main.GetWorkPath(), $"{nameof(GridEditWorkControl)}.json");
            return File.Exists(file);
        }

        protected override void ShowParamForm()
        {
            using (GridEditForm form = new GridEditForm())
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

    public class Grid3D
    {

    }
}
