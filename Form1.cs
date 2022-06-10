using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using ExperimentDesign.General;
using ExperimentDesign.InfoForm;
using ExperimentDesign.Statistic;
using ExperimentDesign.WorkList.ShowResult;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            //for (int i = 0; i < gridView1.Columns.Count - 1; i++)
            //{
            //    gridView1.Columns[i].OptionsColumn.AllowEdit = false;
            //}
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
            if (this.panelControl1.Controls.Count > 0 && (this.panelControl1.Controls[0] as GridControl)?.DataSource != null)
            {
                var table = (this.panelControl1.Controls[0] as GridControl)?.DataSource as DataTable;
                string path = Path.Combine(Application.StartupPath, @"Model\SGS");
                Directory.Delete(path, true);
                Thread.Sleep(1000);
                int seed = 1231234;
                List<string> outs = new List<string>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var max_range = Convert.ToDouble(table.Rows[i]["max_range"]);
                    var angel = Convert.ToDouble(table.Rows[i]["angel"]);
                    var nst = Convert.ToDouble(table.Rows[i]["ktype"]);
                    var nug = Convert.ToDouble(table.Rows[i]["nug"]);
                    var _out = CreateSgsFolder(path, i + 1, seed, max_range, angel, nst, nug);
                    seed += 1000;
                    outs.Add(_out);
                }
                Thread.Sleep(5000);
                this.panelControl1.Controls.Clear();
                var pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
                pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
                pictureEdit1.Location = new System.Drawing.Point(0, 0);
                pictureEdit1.Name = "pictureEdit1";
                pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
                pictureEdit1.Size = new System.Drawing.Size(800, 450);
                pictureEdit1.TabIndex = 0;
                Bitmap map = new Bitmap(pictureEdit1.DisplayRectangle.Width, pictureEdit1.DisplayRectangle.Height);
                for (int index = 0; index < outs.Count; index++)
                {
                    int xcount = 0;
                    int ycount = 0;
                    int zcount = 0;
                    if (File.Exists(outs[index]))
                    {
                        var gslib = ReadGislib(outs[index], out xcount, out ycount, out zcount);
                        var max = gslib.Max();
                        var min = gslib.Min();
                        var det = max - min;
                        int maxdetx = (pictureEdit1.DisplayRectangle.Width / (xcount + 5));
                        int dety = index / maxdetx;
                        int detx = index % maxdetx;
                        for (int i = 0; i < xcount; i++)
                        {
                            for (int j = 0; j < ycount; j++)
                            {
                                int index2 = i * xcount + j;
                                int mapi = i + detx * (xcount + 5);
                                int mapj = j + dety * (ycount + 5);
                                if (mapi < pictureEdit1.DisplayRectangle.Width && mapj < pictureEdit1.DisplayRectangle.Height)
                                {
                                    if (!float.IsNaN(gslib[index2]))
                                    {
                                        var gray = (int)Math.Round((gslib[index2] - min) / det * 255);
                                        Color color = Color.FromArgb(gray, gray, gray);
                                        map.SetPixel(mapi, mapj, color);
                                    }
                                    else
                                    {
                                        map.SetPixel(mapi, mapj, Color.Black);
                                    }
                                }
                            }
                        }
                    }
                }
                pictureEdit1.Image = map;
                this.panelControl1.Controls.Add(pictureEdit1);
                this.Refresh();
            }
            else
            {
                XtraMessageBox.Show("无数据");
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

        private string CreateSgsFolder(string folder, int index, int seed, double max_range, double angel, double ktype, double nug)
        {
            string path = Path.Combine(folder, index.ToString());
            string par = Path.Combine(path, "sgsim.par");
            string exe = Path.Combine(path, @"sgsim.exe");
            string _out = Path.Combine(path, @"sgs.out");
            Directory.CreateDirectory(path);
            File.Copy(Path.Combine(Application.StartupPath, "sgsim.exe"), Path.Combine(path, "sgsim.exe"));
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("              Parameters for SGSIM                                         ");
            sb.AppendLine("              ********************                                         ");
            sb.AppendLine("                                                                           ");
            sb.AppendLine("START OF PARAMETER:                                                        ");
            sb.AppendLine("none                          -file with data                              ");
            sb.AppendLine("1  2  0  3  5  0              -  columns for X,Y,Z,vr,wt,sec.var.          ");
            sb.AppendLine("-1.0e21 1.0e21                -  trimming limits                           ");
            sb.AppendLine("0                             -transform the data (0=no, 1=yes)            ");
            sb.AppendLine("none.trn                      -  file for output trans table               ");
            sb.AppendLine("1                             -  consider ref. dist (0=no, 1=yes)          ");
            sb.AppendLine("none.dat                      -  file with ref. dist distribution          ");
            sb.AppendLine("1  0                          -  columns for vr and wt                     ");
            sb.AppendLine("-4.0    4.0                   -  zmin,zmax(tail extrapolation)             ");
            sb.AppendLine("1      -4.0                   -  lower tail option, parameter              ");
            sb.AppendLine("1       4.0                   -  upper tail option, parameter              ");
            sb.AppendLine("0                             -debugging level: 0,1,2,3                    ");
            sb.AppendLine("nonw.dbg                      -file for debugging output                   ");
            sb.AppendLine("sgs.out" + "                  -file for simulation output                  ");
            sb.AppendLine($"{1}" + "                 -number of realizations to generate          ");
            sb.AppendLine($"{100}" + " " + $"{5.0}" + " " + $"{10}" + "                              ");
            sb.AppendLine($"{100}" + " " + $"{5.0}" + " " + $"{10}" + "                              ");
            sb.AppendLine("1 0.0 1.0                     - nz zmn zsiz                                ");
            sb.AppendLine(seed.ToString() + "                  -random number seed                    ");
            sb.AppendLine("0     8                       -min and max original data for sim           ");
            sb.AppendLine("12                            -number of simulated nodes to use            ");
            sb.AppendLine("0                             -assign data to nodes (0=no, 1=yes)          ");
            sb.AppendLine("1     3                       -multiple grid search (0=no, 1=yes),num      ");
            sb.AppendLine("0                             -maximum data per octant (0=not used)        ");
            sb.AppendLine($"{max_range}" + " " + $"{max_range}" + " 1.0 -maximum search  (hmax,hmin,vert) ");
            sb.AppendLine("0.0   0.0   0.0       -angles for search ellipsoid                 ");
            sb.AppendLine("101" + " " + "101" + " 1 -size of covariance lookup table        ");
            sb.AppendLine($"{ktype}     0.60   1.0              -ktype: 0=SK,1=OK,2=LVM,3=EXDR,4=COLC        ");
            sb.AppendLine("none.dat                      -  file with LVM, EXDR, or COLC variable     ");
            sb.AppendLine("4                             -  column for secondary variable             ");
            sb.AppendLine($"{1.0}" + " " + $"{nug}" + "  -nst, nugget effect                          ");
            sb.AppendLine($"{1}" + " " + $"{1.0}" + " " + $"{0}" + " 0.0 0.0 -it,cc,ang1,ang2,ang3");
            sb.AppendLine(" " + $"{500}" + " " + $"{500}" + " 1.0 - a_hmax, a_hmin, a_vert        ");
            sb.AppendLine($"{1}" + " " + $"{0}" + " " + $"{angel}" + " 0.0 0.0 -it,cc,ang1,ang2,ang3");
            sb.AppendLine(" " + $"{0}" + " " + $"{1.0}" + " 1.0 - a_hmax, a_hmin, a_vert        ");
            File.WriteAllText(par, sb.ToString(), Encoding.UTF8);
            if (File.Exists(_out))
            {
                File.Delete(_out);
            }
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = exe;
            info.WorkingDirectory = path;
            info.UseShellExecute = false;
            info.Arguments = "sgsim.par";
            var process = Process.Start(info);
            return _out;
        }

        private void 不确定分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncertaintyForm form = new UncertaintyForm();
            form.Show();
        }

        private void 模型显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureGroupForm form = new PictureGroupForm();
            form.Show();
        }

        private void 方差分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = false;
            of.Filter = "实验分析表|*.xlsx";
            if (of.ShowDialog() == DialogResult.OK)
            {
                DataTable table = ExcelEx.ExcelToTable(of.FileName);
                var vars = UniVarianceAnalysis.FromTable(table);
                var newtable = UniVarianceAnalysis.VarianceAnalysisTable(vars);
                using (DesignValueShowForm form = new DesignValueShowForm())
                {
                    form.Text = "方差分析";
                    form.SetGrid(newtable);
                    form.ShowDialog();
                }
            }
        }

        private void 多元回归分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MultiRegressionForm form = new MultiRegressionForm();
            form.Show();
        }

        private void 抽稀验证ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SparseValidForm form = new SparseValidForm();
            form.Show();
        }
    }
}
