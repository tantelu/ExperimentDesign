
namespace ExperimentDesign.DesignPanel
{
    partial class BBDesignPanel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.simpleButton_desgin = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton_sample = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // simpleButton_desgin
            // 
            this.simpleButton_desgin.Enabled = false;
            this.simpleButton_desgin.Location = new System.Drawing.Point(44, 41);
            this.simpleButton_desgin.Name = "simpleButton_desgin";
            this.simpleButton_desgin.Size = new System.Drawing.Size(128, 34);
            this.simpleButton_desgin.TabIndex = 0;
            this.simpleButton_desgin.Text = "显示设计";
            this.simpleButton_desgin.Click += new System.EventHandler(this.simpleButton_desgin_Click);
            // 
            // simpleButton_sample
            // 
            this.simpleButton_sample.Enabled = false;
            this.simpleButton_sample.Location = new System.Drawing.Point(240, 41);
            this.simpleButton_sample.Name = "simpleButton_sample";
            this.simpleButton_sample.Size = new System.Drawing.Size(140, 34);
            this.simpleButton_sample.TabIndex = 1;
            this.simpleButton_sample.Text = "显示样本";
            this.simpleButton_sample.Click += new System.EventHandler(this.simpleButton_sample_Click);
            // 
            // BBDesignPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.simpleButton_sample);
            this.Controls.Add(this.simpleButton_desgin);
            this.Name = "BBDesignPanel";
            this.Size = new System.Drawing.Size(962, 393);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton_desgin;
        private DevExpress.XtraEditors.SimpleButton simpleButton_sample;
    }
}
