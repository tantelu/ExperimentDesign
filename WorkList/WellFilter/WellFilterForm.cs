using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ExperimentDesign.WorkList.WellFilter
{
    public partial class WellFilterForm : Form
    {
        public WellFilterForm()
        {
            InitializeComponent();
        }

        private void buttonEdit_welldata_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = false;
            if (of.ShowDialog() == DialogResult.OK)
            {
                buttonEdit_welldata.Text = of.FileName;
            }
            else
            {
                buttonEdit_welldata.Text = string.Empty;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
