using DevExpress.XtraEditors;
using ExperimentDesign.WorkList.Base;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.ShowResult
{
    public partial class PictureGroupForm : XtraForm
    {
        public PictureGroupForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            Bitmap map = new Bitmap(pictureEdit1.DisplayRectangle.Width, pictureEdit1.DisplayRectangle.Height);
            var files = Directory.GetDirectories(this.buttonEdit1.Text).Select(_ => Path.Combine(_, this.textEdit1.Text)).Where(_=>File.Exists(_)).ToList();
            if (files?.Count > 0)
            {
                for (int index = 0; index < files.Count; index++)
                {
                    int dety = index / (int)this.spinEdit1.Value;
                    int detx = index % (int)this.spinEdit1.Value;
                    Bitmap img = new Bitmap(files[index]);
                    for (int i = 0; i < img.Width; i++)
                    {
                        for (int j = 0; j < img.Height; j++)
                        {
                            int mapi = i + detx * (img.Width + 5);
                            int mapj = j + dety * (img.Height + 5);
                            if (mapi < pictureEdit1.DisplayRectangle.Width && mapj < pictureEdit1.DisplayRectangle.Height)
                            {
                                map.SetPixel(mapi, mapj, img.GetPixel(i, j));
                            }
                        }
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("未找到文件");
            }
            pictureEdit1.Image = map;
            this.Refresh();
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog of = new FolderBrowserDialog();
            of.SelectedPath = GlobalWorkCongfig.WorkBasePath;
            if (of.ShowDialog() == DialogResult.OK)
            {
                this.buttonEdit1.Text = of.SelectedPath;
            }
        }
    }
}
