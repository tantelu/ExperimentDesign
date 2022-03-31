using ExperimentDesign.General;
using ExperimentDesign.InfoForm;
using ExperimentDesign.Uncertainty;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExperimentDesign.DesignPanel
{
    public partial class PBDesignPanel : UserControl
    {
        private PlackettBurmanTable designTable;

        public IUpdateDesignTimes DesignTimes { get; set; }

        public PBDesignPanel()
        {
            InitializeComponent();
        }

        public PlackettBurmanTable DesignTable
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
            DesignTable?.SetcenterPoint(checkEdit1.Checked);
            DesignTimes?.UpdateDesignTimes(DesignTable.GetTestCount());
        }
    }
}
