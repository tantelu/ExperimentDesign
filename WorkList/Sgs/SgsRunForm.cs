﻿using DevExpress.XtraEditors;
using ExperimentDesign.General;
using System;

namespace ExperimentDesign.WorkList.Sgs
{
    public partial class SgsRunForm : XtraForm
    {
        public SgsRunForm()
        {
            InitializeComponent();
        }

        public SgsPar GetSgsPar()
        {
            SgsPar sgs = new SgsPar();
            sgs.Variogram = this.variogramControl1.GetVariogram();
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

        public void InitForm(SgsPar sgs)
        {
            if (sgs != null)
            {
                krigType.SelectedIndex = (int)sgs.KrigType;
                MaxData.Tag = sgs.MaxData;
                MaxData.Text = sgs.MaxData.IsDesign ? sgs.MaxData.DesignName : sgs.MaxData.Value.ToString();
                Rake.Tag = sgs.Rake;
                Rake.Text = sgs.Rake.IsDesign? sgs.Rake.DesignName : sgs.Rake.Value.ToString();

                Dip.Tag = sgs.Dip;
                Dip.Text = sgs.Dip.IsDesign? sgs.Dip.DesignName: sgs.Dip.Value.ToString();

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

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void simpleButton2_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

    }
}
