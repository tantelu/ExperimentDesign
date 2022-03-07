using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExperimentDesign.InfoForm
{
    public partial class DesignShowForm : Form
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
