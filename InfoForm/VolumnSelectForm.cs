using DevExpress.XtraEditors;
using ExperimentDesign.WorkList.Base;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Data;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using NPOI.XSSF.UserModel;

namespace ExperimentDesign.InfoForm
{
    public partial class VolumnSelectForm : XtraForm
    {
        public VolumnSelectForm()
        {
            InitializeComponent();
        }

        public UncertaintyForm UncertaintyForm { get; set; }

        public string WorkPath
        {
            get
            {
                return UncertaintyForm.GetWorkPath();
            }
        }

        public List<double> volumns = new List<double>();

        public void ToExcel(DataTable dt)
        {
            if (dt != null)
            {
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Sheet0");
                IRow row = sheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = row.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }
                row.CreateCell(dt.Columns.Count).SetCellValue(this.comboBoxEdit1.Text);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row1 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                    row1.CreateCell(dt.Columns.Count).SetCellValue(volumns[i]);
                }
                MemoryStream stream = new MemoryStream();
                workbook.Write(stream);
                var buf = stream.ToArray();
                using (FileStream fs = new FileStream(this.buttonEdit1.Text, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "实验结果表|*.xlsx";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                this.buttonEdit1.Text = sf.FileName;
            }
            else
            {
                this.buttonEdit1.Text = string.Empty;
            }
        }

        private void simpleButton1_Click(object sender, System.EventArgs e)
        {
            if (volumns?.Count > 0 && !string.IsNullOrEmpty(this.buttonEdit1.Text))
            {
                ToExcel(UncertaintyForm.GetSaveDesignTable());
                XtraMessageBox.Show("完成");
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            volumns.Clear();
            var path = GlobalWorkCongfig.WorkBasePath;
            if (Directory.Exists(WorkPath))
            {
                DirectoryInfo info = new DirectoryInfo(WorkPath);
                if (info.GetDirectories().Length > 0)
                {
                    foreach (var item in info.GetDirectories())
                    {
                        var itemfile = Path.Combine(item.FullName, this.volumnfilename.Text);
                        JObject obj = JObject.Parse(File.ReadAllText(itemfile, Encoding.UTF8));
                        volumns.Add((double)obj[this.comboBoxEdit1.Text]);
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in volumns)
            {
                sb.AppendLine(item.ToString());
            }
            if (sb.Length > 0)
            {
                this.textBox1.Text = sb.ToString();
            }
        }

        private void volumnfilename_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RefreshProperty();
        }

        private void RefreshProperty()
        {
            var path = GlobalWorkCongfig.WorkBasePath;
            if (Directory.Exists(WorkPath))
            {
                DirectoryInfo info = new DirectoryInfo(WorkPath);
                if (info.GetDirectories().Length > 0)
                {
                    var first = info.GetDirectories()[0];
                    var itemfile = Path.Combine(first.FullName, this.volumnfilename.Text);
                    if (!File.Exists(itemfile))
                    {
                        XtraMessageBox.Show($"文件: {itemfile} 不存在！");
                        return;
                    }
                    JObject obj = JObject.Parse(File.ReadAllText(itemfile, Encoding.UTF8));
                    this.comboBoxEdit1.Properties.Items.Clear();
                    this.comboBoxEdit1.Properties.Items.AddRange(obj.Properties().Select(_ => _.Name).ToList());
                }
            }
        }
    }
}
