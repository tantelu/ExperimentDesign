
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
            this.不确定分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.模型显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.模型分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.方差分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.多元回归分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.抽稀验证ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.不确定分析ToolStripMenuItem,
            this.抽稀验证ToolStripMenuItem,
            this.模型显示ToolStripMenuItem,
            this.模型分析ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1170, 32);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 不确定分析ToolStripMenuItem
            // 
            this.不确定分析ToolStripMenuItem.Name = "不确定分析ToolStripMenuItem";
            this.不确定分析ToolStripMenuItem.Size = new System.Drawing.Size(116, 28);
            this.不确定分析ToolStripMenuItem.Text = "不确定分析";
            this.不确定分析ToolStripMenuItem.Click += new System.EventHandler(this.不确定分析ToolStripMenuItem_Click);
            // 
            // 模型显示ToolStripMenuItem
            // 
            this.模型显示ToolStripMenuItem.Name = "模型显示ToolStripMenuItem";
            this.模型显示ToolStripMenuItem.Size = new System.Drawing.Size(98, 28);
            this.模型显示ToolStripMenuItem.Text = "模型显示";
            this.模型显示ToolStripMenuItem.Click += new System.EventHandler(this.模型显示ToolStripMenuItem_Click);
            // 
            // 模型分析ToolStripMenuItem
            // 
            this.模型分析ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.方差分析ToolStripMenuItem,
            this.多元回归分析ToolStripMenuItem});
            this.模型分析ToolStripMenuItem.Name = "模型分析ToolStripMenuItem";
            this.模型分析ToolStripMenuItem.Size = new System.Drawing.Size(98, 28);
            this.模型分析ToolStripMenuItem.Text = "模型分析";
            // 
            // 方差分析ToolStripMenuItem
            // 
            this.方差分析ToolStripMenuItem.Name = "方差分析ToolStripMenuItem";
            this.方差分析ToolStripMenuItem.Size = new System.Drawing.Size(218, 34);
            this.方差分析ToolStripMenuItem.Text = "方差分析";
            this.方差分析ToolStripMenuItem.Click += new System.EventHandler(this.方差分析ToolStripMenuItem_Click);
            // 
            // 多元回归分析ToolStripMenuItem
            // 
            this.多元回归分析ToolStripMenuItem.Name = "多元回归分析ToolStripMenuItem";
            this.多元回归分析ToolStripMenuItem.Size = new System.Drawing.Size(218, 34);
            this.多元回归分析ToolStripMenuItem.Text = "多元回归分析";
            this.多元回归分析ToolStripMenuItem.Click += new System.EventHandler(this.多元回归分析ToolStripMenuItem_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 32);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1170, 620);
            this.panelControl1.TabIndex = 1;
            // 
            // 抽稀验证ToolStripMenuItem
            // 
            this.抽稀验证ToolStripMenuItem.Name = "抽稀验证ToolStripMenuItem";
            this.抽稀验证ToolStripMenuItem.Size = new System.Drawing.Size(98, 28);
            this.抽稀验证ToolStripMenuItem.Text = "抽稀验证";
            this.抽稀验证ToolStripMenuItem.Click += new System.EventHandler(this.抽稀验证ToolStripMenuItem_Click);
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
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ToolStripMenuItem 不确定分析ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 模型显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 模型分析ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 方差分析ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 多元回归分析ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 抽稀验证ToolStripMenuItem;
    }
}

