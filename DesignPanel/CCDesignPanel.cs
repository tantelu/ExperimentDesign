using ExperimentDesign.InfoForm;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExperimentDesign.DesignPanel
{
    public partial class CCDesignPanel : UserControl
    {
        private CenteralCompositeTable designTable;

        public CCDesignPanel()
        {
            InitializeComponent();
        }

        public CenteralCompositeTable DesignTable
        {
            get { return designTable; }
            set
            {
                if (value != null)
                {
                    this.simpleButton_desgin.Enabled = true;
                    this.simpleButton_sample.Enabled = true;
                    designTable = value;
                };
            }
        }

        public IReadOnlyList<VariableData> Data { get; set; }

        private void simpleButton_desgin_Click(object sender, EventArgs e)
        {
            using (DesignShowForm form = new DesignShowForm())
            {
                form.Infomation = DesignTable?.ToString();
                form.ShowDialog();
            }
        }

        private void simpleButton_sample_Click(object sender, EventArgs e)
        {
            using (DesignValueShowForm form = new DesignValueShowForm())
            {
                form.SetGrid(DesignTable.ToDataTable(Data));
                form.ShowDialog();
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            checkEdit2.Checked = !checkEdit1.Checked;
            DesignTable?.Init(checkEdit1.Checked?CenteralCompositeType.Inscribed: CenteralCompositeType.FaceCentered);
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            checkEdit1.Checked = !checkEdit2.Checked;
            DesignTable?.Init(checkEdit1.Checked ? CenteralCompositeType.Inscribed : CenteralCompositeType.FaceCentered);
        }
    }
}
