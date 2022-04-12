using ExperimentDesign.General;
using ExperimentDesign.InfoForm;
using ExperimentDesign.Uncertainty;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExperimentDesign.DesignPanel
{
    public partial class EqualSpacePanel : UserControl
    {
        private EqualSpaceTable designTable;

        public EqualSpacePanel()
        {
            InitializeComponent();
        }

        public EqualSpaceTable DesignTable
        {
            get { return designTable; }
            set
            {
                if (value != null)
                {
                    designTable = value;
                };
            }
        }

        public IReadOnlyList<VariableData> Data { get; set; }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            using (DesignValueShowForm form = new DesignValueShowForm())
            {
                form.SetGrid(DesignTable.ToDataTable(Data));
                form.ShowDialog();
            }
        }
    }
}
