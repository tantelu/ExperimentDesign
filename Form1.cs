using DevExpress.XtraEditors;
using System.Data;
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
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Appearance.FocusedRow.Options.UseTextOptions = true;
            gridView1.Appearance.FocusedRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.Row.Options.UseTextOptions = true;
            gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            gridControl1.DataSource = table;
            this.panelControl1.Controls.Add(gridControl1);
            gridControl1.RefreshDataSource();
            this.Refresh();
        }
    }
}
