using DevExpress.XtraEditors;

namespace ExperimentDesign.InfoForm
{
    public partial class DesignShowForm : XtraForm
    {
        public DesignShowForm()
        {
            InitializeComponent();
        }

        public string Infomation
        {
            get
            {
                return this.textBox1.Text;
            }
            set
            {
                this.textBox1.Text = value;
            }
        }
    }
}
