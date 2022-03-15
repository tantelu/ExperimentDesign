using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList
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
            string file = Path.Combine(Main.GetWorkPath(), $"{index}", ShowFile);
            if (File.Exists(file))
            {
                int xcount = 0;
                int ycount = 0;
                int zcount = 0;
                var gslib = ReadGislib(file, out xcount, out ycount, out zcount);
                Bitmap map = new Bitmap(xcount, ycount);
                var max = gslib.Max();
                var min = gslib.Min();
                var det = max - min;
                for (int i = 0; i < xcount; i++)
                {
                    for (int j = 0; j < ycount; j++)
                    {
                        int index2 = i * xcount + j;
                        if (!float.IsNaN(gslib[index2]))
                        {
                            var gray = (int)Math.Round((gslib[index2] - min) / det * 255);
                            Color color = Color.FromArgb(gray, gray, gray);
                            map.SetPixel(i, j, color);
                        }
                        else
                        {
                            map.SetPixel(i, j, Color.Black);
                        }
                    }

                }
                map.Save(Path.Combine(Main.GetWorkPath(), $"{index}", $"{WorkName}.png"));
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
            return canjump || File.Exists(Path.Combine(Main.GetWorkPath(), $"{index}", $"{WorkName}.png"));
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

        private float[] ReadGislib(string gislibfile, out int xcount, out int ycount, out int zcount)
        {
            using (FileStream fileStream = new FileStream(gislibfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                StreamReader reader = new StreamReader(fileStream);
                reader.ReadLine();
                var first = reader.ReadLine();
                var gridcount = first.Split(new string[] { "x", "(", ")", " " }, StringSplitOptions.RemoveEmptyEntries);
                xcount = Convert.ToInt32(gridcount[1]);
                ycount = Convert.ToInt32(gridcount[2]);
                zcount = Convert.ToInt32(gridcount[3]);
                float[] fs = new float[xcount * ycount * zcount];
                reader.ReadLine();
                int index = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var strs = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length > 0 && strs[0].Trim().Length > 0)
                    {
                        float facie = Convert.ToSingle(strs[0].Trim());
                        fs[index++] = facie;
                    }
                }
                reader.Close();
                reader.Dispose();
                return fs;
            }
        }
    }
}
