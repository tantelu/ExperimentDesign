using DevExpress.XtraEditors;

namespace ExperimentDesign.WorkList.ShowResult
{
    public partial class Show2DResultForm : XtraForm
    {
        public string FileName
        {
            get
            {
                return this.textEdit1.Text;
            }
            set
            {
                this.textEdit1.Text = value;
            }
        }

        public Show2DResultForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
