using ExperimentDesign.General;
using System.IO;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.Sgs
{
    public partial class SgsUserControl : UserControl
    {
        public SgsUserControl()
        {
            InitializeComponent();
        }

        public SgsPar GetSgsPar()
        {
            SgsPar sgs = new SgsPar();
            sgs.DataFileName = DataFile.Text;
            sgs.Variogram = this.variogramControl1.GetVariogram();
            sgs.MaxValue = Design<double>.GeneralDesign(MaxValue.Tag, MaxValue.Text);
            sgs.MinValue = Design<double>.GeneralDesign(MinValue.Tag, MinValue.Text);
            sgs.SearchMaxRadius = Design<double>.GeneralDesign(SearchMaxRadius.Tag, SearchMaxRadius.Text);
            sgs.SearchMedRadius = Design<double>.GeneralDesign(SearchMedRadius.Tag, SearchMedRadius.Text);
            sgs.SearchMinRadius = Design<double>.GeneralDesign(SearchMinRadius.Tag, SearchMinRadius.Text);
            sgs.Azimuth = Design<double>.GeneralDesign(Azimuth.Tag, Azimuth.Text);
            sgs.Dip = Design<double>.GeneralDesign(Dip.Tag, Dip.Text);
            sgs.Rake = Design<double>.GeneralDesign(Rake.Tag, Rake.Text);
            sgs.MaxData = Design<int>.GeneralDesign(MaxData.Tag, MaxData.Text);
            sgs.KrigType = (KrigType)krigType.SelectedIndex;
            return sgs;
        }

        public void SetSgsPar(SgsPar sgs)
        {
            if (sgs != null)
            {
                krigType.SelectedIndex = (int)sgs.KrigType;
                DataFile.Text = sgs.DataFileName;
                MaxData.Tag = sgs.MaxData;
                MaxData.Text = sgs.MaxData.IsDesign ? sgs.MaxData.DesignName : sgs.MaxData.Value.ToString();
                Rake.Tag = sgs.Rake;
                Rake.Text = sgs.Rake.IsDesign ? sgs.Rake.DesignName : sgs.Rake.Value.ToString();
                MaxValue.Tag = sgs.MaxValue;
                MaxValue.Text= sgs.MaxValue.IsDesign ? sgs.MaxValue.DesignName : sgs.MaxValue.Value.ToString();
                MinValue.Tag = sgs.MinValue;
                MinValue.Text = sgs.MinValue.IsDesign ? sgs.MinValue.DesignName : sgs.MinValue.Value.ToString();

                Dip.Tag = sgs.Dip;
                Dip.Text = sgs.Dip.IsDesign ? sgs.Dip.DesignName : sgs.Dip.Value.ToString();

                Azimuth.Tag = sgs.Azimuth;
                Azimuth.Text = sgs.Azimuth.IsDesign ? sgs.Azimuth.DesignName : sgs.Azimuth.Value.ToString();

                SearchMinRadius.Tag = sgs.SearchMinRadius;
                SearchMinRadius.Text = sgs.SearchMinRadius.IsDesign ? sgs.SearchMinRadius.DesignName : sgs.SearchMinRadius.Value.ToString();

                SearchMedRadius.Tag = sgs.SearchMedRadius;
                SearchMedRadius.Text = sgs.SearchMedRadius.IsDesign ? sgs.SearchMedRadius.DesignName : sgs.SearchMedRadius.Value.ToString();

                SearchMaxRadius.Tag = sgs.SearchMaxRadius;
                SearchMaxRadius.Text = sgs.SearchMaxRadius.IsDesign ? sgs.SearchMaxRadius.DesignName : sgs.SearchMaxRadius.Value.ToString();

                this.variogramControl1.SetVariogram(sgs.Variogram);
            }
        }

        private void DataFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog OF = new OpenFileDialog();
            OF.Multiselect = false;
            if (OF.ShowDialog() == DialogResult.OK)
            {
                DataFile.Text = OF.FileName;
            }
            else
            {
                DataFile.Text = string.Empty;
            }
        }
    }
}
