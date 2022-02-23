using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ExperimentDesign
{
    public partial class Form1 : XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void 正交实验ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (OrthGuideForm form = new OrthGuideForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    AddTable(form.GetOrthTable());
                }
            }
        }

        private void plackettBurman实验ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (PBForm form = new PBForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    AddTable(form.GetPBTable());
                }
            }
        }

        private void AddTable(DataTable table)
        {
            this.panelControl1.Controls.Clear();

            var gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            var gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridControl1.Location = new System.Drawing.Point(0, 0);
            gridControl1.MainView = gridView1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new System.Drawing.Size(891, 317);
            gridControl1.TabIndex = 0;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            gridView1});

            gridView1.GridControl = gridControl1;
            gridView1.Name = "gridView1";
            gridView1.OptionsCustomization.AllowSort = false;
            gridView1.OptionsView.ShowDetailButtons = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.ShowIndicator = false;
            //gridView1.OptionsBehavior.Editable = false;
            gridView1.Appearance.FocusedRow.Options.UseTextOptions = true;
            gridView1.Appearance.FocusedRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.Row.Options.UseTextOptions = true;
            gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            gridControl1.DataSource = table;
            gridControl1.MainView.PopulateColumns();
            gridControl1.RefreshDataSource();

            this.panelControl1.Controls.Add(gridControl1);
            this.Refresh();
            for (int i = 0; i < gridView1.Columns.Count - 1; i++)
            {
                gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            }
        }

        private void 响应曲面ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (BBForm form = new BBForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    AddTable(form.GetBBTable());
                }
            }
        }

        private void 模拟ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string exe = Path.Combine(Application.StartupPath, @"sgsim.exe");
            string _out = Path.Combine(Application.StartupPath, @"sgs.out");
            if (File.Exists(exe))
            {
                File.Delete(_out);
                var process = Process.Start(exe, "sgsim.par");
                while (!File.Exists(_out))
                {
                    Thread.Sleep(1000);
                }
                int xcount = 0;
                int ycount = 0;
                int zcount = 0;

                var gslib = ReadGislib(_out, out xcount, out ycount, out zcount);
                var max = gslib.Max();
                var min = gslib.Min();
                var det = max - min;
                Bitmap map = new Bitmap(xcount, ycount);
                for (int i = 0; i < xcount; i++)
                {
                    for (int j = 0; j < ycount; j++)
                    {
                        int index = i * xcount + j;
                        if (!float.IsNaN(gslib[index]))
                        {
                            var gray = (int)Math.Round((gslib[index] - min) / det * 255);
                            Color color = Color.FromArgb(gray, gray, gray);
                            map.SetPixel(i, j, color);
                        }
                        else
                        {
                            map.SetPixel(i, j, Color.Black);
                        }
                    }
                }

                this.panelControl1.Controls.Clear();
                var pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
                pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
                pictureEdit1.Location = new System.Drawing.Point(0, 0);
                pictureEdit1.Name = "pictureEdit1";
                pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
                pictureEdit1.Size = new System.Drawing.Size(800, 450);
                pictureEdit1.TabIndex = 0;
                pictureEdit1.Properties.SizeMode = PictureSizeMode.Zoom;
                pictureEdit1.Image = map;
                this.panelControl1.Controls.Add(pictureEdit1);
                this.Refresh();
            }
        }

        private float[] ReadGislib(string gislibfile, out int xcount, out int ycount, out int zcount)
        {
            StreamReader reader = new StreamReader(gislibfile);
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
