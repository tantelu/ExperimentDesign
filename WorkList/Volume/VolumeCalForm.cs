using DevExpress.XtraEditors;
using ExperimentDesign.General;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Volume
{
    public partial class VolumeCalForm : XtraForm
    {
        public VolumeCalForm()
        {
            InitializeComponent();
        }

        public VolumnCalPar GetVolumnCalPar()
        {
            VolumnCalPar par = new VolumnCalPar();
            par.VolumnOutFileName = volumnout.Text;
            par.PorosityFileName = porosityfile.Text;
            par.PenetrationFileName = penetrationfile.Text;
            par.Porosity = Design<double>.GeneralDesign(porositylow.Tag, porositylow.Text);
            par.Penetration = Design<double>.GeneralDesign(penetrationlow.Tag, penetrationlow.Text);
            return par;
        }

        public void SetVolumnCalPar(VolumnCalPar par)
        {
            if (par != null)
            {
                volumnout.Text = par.VolumnOutFileName;
                porosityfile.Text = par.PorosityFileName;
                penetrationfile.Text = par.PenetrationFileName;

                porositylow.Tag = par.Porosity;
                porositylow.Text = par.Porosity.IsDesign? par.Porosity.DesignName : par.Porosity.Value.ToString();

                penetrationlow.Tag = par.Penetration;
                penetrationlow.Text = par.Penetration.IsDesign ? par.Penetration.DesignName : par.Penetration.Value.ToString();
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
