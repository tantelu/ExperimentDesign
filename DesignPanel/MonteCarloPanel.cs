using ExperimentDesign.General;
using ExperimentDesign.Uncertainty;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExperimentDesign.DesignPanel
{
    public partial class MonteCarloPanel : UserControl
    {
        private MonteCarloTable designTable;

        public MonteCarloPanel()
        {
            InitializeComponent();
        }

        public MonteCarloTable DesignTable
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
    }
}
