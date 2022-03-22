using DevExpress.XtraEditors;
using ExperimentDesign.General;
using ExperimentDesign.WorkList.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.ShowResult
{
    public partial class Show2DWorkControl : WorkControl
    {
        public Show2DWorkControl()
        {
            InitializeComponent();
        }

        protected override string WorkName => "显示二维结果";

        private bool canjump = false;

        public string ShowFile { get; set; }

        protected override Bitmap Picture => Properties.Resources.Show2D;

        public override void Run(int index, IReadOnlyDictionary<string, object> designVaribles)
        {
            ColorBar colorbar = ColorBar.Default;
            string file = Path.Combine(Main.GetWorkPath(), $"{index}", ShowFile);
            if (File.Exists(file))
            {
                int xcount = 0;
                int ycount = 0;
                int zcount = 0;
                var gslib = Gslib.ReadGislib(file, out xcount, out ycount, out zcount);
                Bitmap map = new Bitmap(xcount, ycount);
                var max = gslib.Max();
                var min = gslib.Min();
                colorbar.SetMinMax(min, max);
                for (int i = 0; i < xcount; i++)
                {
                    for (int j = 0; j < ycount; j++)
                    {
                        int index2 = i * xcount + j;
                        if (!float.IsNaN(gslib[index2]))
                        {
                            map.SetPixel(i, j, colorbar.GetColor(gslib[index2]));
                        }
                        else
                        {
                            map.SetPixel(i, j, Color.Black);
                        }
                    }

                }
                map.Save(Path.ChangeExtension(file, "png"));
            }
            else
            {
                var dia = XtraMessageBox.Show($"在路径：{file} 下未找到文件,无法执行‘{WorkName}’,是否跳过此步骤？", "提示", MessageBoxButtons.YesNo);
                canjump = dia == DialogResult.Yes;
                return;
            }
        }

        public override bool GetRunState(int index)
        {
            return true;
        }

        public override string Save()
        {
            return ShowFile;
        }

        public override void Open(string str)
        {
            ShowFile = str;
        }

        protected override void ShowParamForm()
        {
            using (Show2DResultForm form = new Show2DResultForm())
            {
                form.FileName = ShowFile;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    ShowFile = form.FileName;
                }
            }
        }
    }
}
