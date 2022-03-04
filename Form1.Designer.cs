
namespace ExperimentDesign
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.实验ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.正交实验ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plackettBurman实验ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.响应曲面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.模拟ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.实验ToolStripMenuItem,
            this.模拟ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1170, 32);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(62, 28);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 实验ToolStripMenuItem
            // 
            this.实验ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.正交实验ToolStripMenuItem,
            this.plackettBurman实验ToolStripMenuItem,
            this.响应曲面ToolStripMenuItem});
            this.实验ToolStripMenuItem.Name = "实验ToolStripMenuItem";
            this.实验ToolStripMenuItem.Size = new System.Drawing.Size(62, 28);
            this.实验ToolStripMenuItem.Text = "实验";
            // 
            // 正交实验ToolStripMenuItem
            // 
            this.正交实验ToolStripMenuItem.Name = "正交实验ToolStripMenuItem";
            this.正交实验ToolStripMenuItem.Size = new System.Drawing.Size(307, 34);
            this.正交实验ToolStripMenuItem.Text = "正交实验";
            this.正交实验ToolStripMenuItem.Click += new System.EventHandler(this.正交实验ToolStripMenuItem_Click);
            // 
            // plackettBurman实验ToolStripMenuItem
            // 
            this.plackettBurman实验ToolStripMenuItem.Name = "plackettBurman实验ToolStripMenuItem";
            this.plackettBurman实验ToolStripMenuItem.Size = new System.Drawing.Size(307, 34);
            this.plackettBurman实验ToolStripMenuItem.Text = "Plackett-Burman实验";
            this.plackettBurman实验ToolStripMenuItem.Click += new System.EventHandler(this.plackettBurman实验ToolStripMenuItem_Click);
            // 
            // 响应曲面ToolStripMenuItem
            // 
            this.响应曲面ToolStripMenuItem.Name = "响应曲面ToolStripMenuItem";
            this.响应曲面ToolStripMenuItem.Size = new System.Drawing.Size(307, 34);
            this.响应曲面ToolStripMenuItem.Text = "响应曲面(Box-Behnken)";
            this.响应曲面ToolStripMenuItem.Click += new System.EventHandler(this.响应曲面ToolStripMenuItem_Click);
            // 
            // 模拟ToolStripMenuItem
            // 
            this.模拟ToolStripMenuItem.Name = "模拟ToolStripMenuItem";
            this.模拟ToolStripMenuItem.Size = new System.Drawing.Size(62, 28);
            this.模拟ToolStripMenuItem.Text = "模拟";
            this.模拟ToolStripMenuItem.Click += new System.EventHandler(this.模拟ToolStripMenuItem_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 32);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1170, 620);
            this.panelControl1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 652);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "不确定分析";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 实验ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 正交实验ToolStripMenuItem;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ToolStripMenuItem plackettBurman实验ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 响应曲面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 模拟ToolStripMenuItem;
    }
}

