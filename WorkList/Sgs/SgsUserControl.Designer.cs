using ExperimentDesign.WorkList.Base;

namespace ExperimentDesign.WorkList.Sgs
{
    partial class SgsUserControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.tabPane1 = new DevExpress.XtraBars.Navigation.TabPane();
            this.tabNavigationPage1 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.variogramControl1 = new ExperimentDesign.WorkList.Base.VariogramControl();
            this.tabNavigationPage2 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.SearchMinRadius = new DevExpress.XtraEditors.TextEdit();
            this.SearchMedRadius = new DevExpress.XtraEditors.TextEdit();
            this.SearchMaxRadius = new DevExpress.XtraEditors.TextEdit();
            this.Rake = new DevExpress.XtraEditors.TextEdit();
            this.Dip = new DevExpress.XtraEditors.TextEdit();
            this.Azimuth = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.MaxData = new DevExpress.XtraEditors.TextEdit();
            this.krigType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem16 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem17 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem18 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem19 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).BeginInit();
            this.tabPane1.SuspendLayout();
            this.tabNavigationPage1.SuspendLayout();
            this.tabNavigationPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SearchMinRadius.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchMedRadius.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchMaxRadius.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rake.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Azimuth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.krigType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.tabPane1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(927, 310);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // tabPane1
            // 
            this.tabPane1.Controls.Add(this.tabNavigationPage1);
            this.tabPane1.Controls.Add(this.tabNavigationPage2);
            this.tabPane1.Location = new System.Drawing.Point(3, 3);
            this.tabPane1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPane1.Name = "tabPane1";
            this.tabPane1.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPage1,
            this.tabNavigationPage2});
            this.tabPane1.RegularSize = new System.Drawing.Size(921, 304);
            this.tabPane1.SelectedPage = this.tabNavigationPage1;
            this.tabPane1.Size = new System.Drawing.Size(921, 304);
            this.tabPane1.TabIndex = 12;
            this.tabPane1.Text = "参数";
            // 
            // tabNavigationPage1
            // 
            this.tabNavigationPage1.BackgroundPadding = new System.Windows.Forms.Padding(2);
            this.tabNavigationPage1.Caption = "变差函数";
            this.tabNavigationPage1.Controls.Add(this.variogramControl1);
            this.tabNavigationPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabNavigationPage1.Name = "tabNavigationPage1";
            this.tabNavigationPage1.Size = new System.Drawing.Size(915, 253);
            // 
            // variogramControl1
            // 
            this.variogramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variogramControl1.Location = new System.Drawing.Point(0, 0);
            this.variogramControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.variogramControl1.Name = "variogramControl1";
            this.variogramControl1.Size = new System.Drawing.Size(915, 253);
            this.variogramControl1.TabIndex = 0;
            // 
            // tabNavigationPage2
            // 
            this.tabNavigationPage2.BackgroundPadding = new System.Windows.Forms.Padding(2);
            this.tabNavigationPage2.Caption = "设置";
            this.tabNavigationPage2.Controls.Add(this.layoutControl2);
            this.tabNavigationPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabNavigationPage2.Name = "tabNavigationPage2";
            this.tabNavigationPage2.Size = new System.Drawing.Size(915, 253);
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.SearchMinRadius);
            this.layoutControl2.Controls.Add(this.SearchMedRadius);
            this.layoutControl2.Controls.Add(this.SearchMaxRadius);
            this.layoutControl2.Controls.Add(this.Rake);
            this.layoutControl2.Controls.Add(this.Dip);
            this.layoutControl2.Controls.Add(this.Azimuth);
            this.layoutControl2.Controls.Add(this.labelControl1);
            this.layoutControl2.Controls.Add(this.MaxData);
            this.layoutControl2.Controls.Add(this.krigType);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(0, 0);
            this.layoutControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.Root = this.layoutControlGroup2;
            this.layoutControl2.Size = new System.Drawing.Size(915, 253);
            this.layoutControl2.TabIndex = 0;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // SearchMinRadius
            // 
            this.SearchMinRadius.EditValue = "300";
            this.SearchMinRadius.Location = new System.Drawing.Point(753, 37);
            this.SearchMinRadius.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SearchMinRadius.Name = "SearchMinRadius";
            this.SearchMinRadius.Properties.Appearance.Options.UseTextOptions = true;
            this.SearchMinRadius.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.SearchMinRadius.Size = new System.Drawing.Size(157, 28);
            this.SearchMinRadius.StyleController = this.layoutControl2;
            this.SearchMinRadius.TabIndex = 14;
            this.SearchMinRadius.Tag = "300";
            // 
            // SearchMedRadius
            // 
            this.SearchMedRadius.EditValue = "300";
            this.SearchMedRadius.Location = new System.Drawing.Point(418, 37);
            this.SearchMedRadius.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SearchMedRadius.Name = "SearchMedRadius";
            this.SearchMedRadius.Properties.Appearance.Options.UseTextOptions = true;
            this.SearchMedRadius.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.SearchMedRadius.Size = new System.Drawing.Size(221, 28);
            this.SearchMedRadius.StyleController = this.layoutControl2;
            this.SearchMedRadius.TabIndex = 13;
            this.SearchMedRadius.Tag = "300";
            // 
            // SearchMaxRadius
            // 
            this.SearchMaxRadius.EditValue = "500";
            this.SearchMaxRadius.Location = new System.Drawing.Point(117, 37);
            this.SearchMaxRadius.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SearchMaxRadius.Name = "SearchMaxRadius";
            this.SearchMaxRadius.Properties.Appearance.Options.UseTextOptions = true;
            this.SearchMaxRadius.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.SearchMaxRadius.Size = new System.Drawing.Size(182, 28);
            this.SearchMaxRadius.StyleController = this.layoutControl2;
            this.SearchMaxRadius.TabIndex = 12;
            this.SearchMaxRadius.Tag = "500";
            // 
            // Rake
            // 
            this.Rake.EditValue = "0";
            this.Rake.Location = new System.Drawing.Point(728, 71);
            this.Rake.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Rake.Name = "Rake";
            this.Rake.Properties.Appearance.Options.UseTextOptions = true;
            this.Rake.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Rake.Size = new System.Drawing.Size(182, 28);
            this.Rake.StyleController = this.layoutControl2;
            this.Rake.TabIndex = 11;
            this.Rake.Tag = "0";
            // 
            // Dip
            // 
            this.Dip.EditValue = "0";
            this.Dip.Location = new System.Drawing.Point(388, 71);
            this.Dip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Dip.Name = "Dip";
            this.Dip.Properties.Appearance.Options.UseTextOptions = true;
            this.Dip.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Dip.Size = new System.Drawing.Size(251, 28);
            this.Dip.StyleController = this.layoutControl2;
            this.Dip.TabIndex = 10;
            this.Dip.Tag = "0";
            // 
            // Azimuth
            // 
            this.Azimuth.EditValue = "0";
            this.Azimuth.Location = new System.Drawing.Point(70, 71);
            this.Azimuth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Azimuth.Name = "Azimuth";
            this.Azimuth.Properties.Appearance.Options.UseTextOptions = true;
            this.Azimuth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Azimuth.Size = new System.Drawing.Size(229, 28);
            this.Azimuth.StyleController = this.layoutControl2;
            this.Azimuth.TabIndex = 9;
            this.Azimuth.Tag = "0";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(5, 7);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(108, 22);
            this.labelControl1.StyleController = this.layoutControl2;
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "搜索椭球体：";
            // 
            // MaxData
            // 
            this.MaxData.EditValue = "8";
            this.MaxData.Location = new System.Drawing.Point(154, 139);
            this.MaxData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaxData.Name = "MaxData";
            this.MaxData.Properties.Appearance.Options.UseTextOptions = true;
            this.MaxData.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.MaxData.Size = new System.Drawing.Size(756, 28);
            this.MaxData.StyleController = this.layoutControl2;
            this.MaxData.TabIndex = 5;
            this.MaxData.Tag = "8";
            // 
            // krigType
            // 
            this.krigType.EditValue = "简单克里金";
            this.krigType.Location = new System.Drawing.Point(154, 105);
            this.krigType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.krigType.Name = "krigType";
            this.krigType.Properties.Appearance.Options.UseTextOptions = true;
            this.krigType.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.krigType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.krigType.Properties.Items.AddRange(new object[] {
            "简单克里金",
            "普通克里金"});
            this.krigType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.krigType.Size = new System.Drawing.Size(756, 28);
            this.krigType.StyleController = this.layoutControl2;
            this.krigType.TabIndex = 4;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem7,
            this.emptySpaceItem1,
            this.layoutControlItem8,
            this.layoutControlItem13,
            this.layoutControlItem14,
            this.layoutControlItem15,
            this.layoutControlItem16,
            this.layoutControlItem17,
            this.layoutControlItem18,
            this.layoutControlItem19});
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutControlGroup2.Size = new System.Drawing.Size(915, 253);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.krigType;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 100);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(911, 34);
            this.layoutControlItem7.Text = "克里金类型：";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(144, 22);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 168);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(885, 36);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.MaxData;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 134);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(911, 34);
            this.layoutControlItem8.Text = "最大条件点数量：";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(144, 22);
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.labelControl1;
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 5, 5);
            this.layoutControlItem13.Size = new System.Drawing.Size(911, 32);
            this.layoutControlItem13.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem13.TextVisible = false;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.Azimuth;
            this.layoutControlItem14.Location = new System.Drawing.Point(0, 66);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(300, 34);
            this.layoutControlItem14.Text = "主方向:";
            this.layoutControlItem14.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem14.TextSize = new System.Drawing.Size(60, 22);
            this.layoutControlItem14.TextToControlDistance = 5;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.Dip;
            this.layoutControlItem15.Location = new System.Drawing.Point(300, 66);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(340, 34);
            this.layoutControlItem15.Text = "倾角方向:";
            this.layoutControlItem15.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem15.TextSize = new System.Drawing.Size(78, 22);
            this.layoutControlItem15.TextToControlDistance = 5;
            // 
            // layoutControlItem16
            // 
            this.layoutControlItem16.Control = this.Rake;
            this.layoutControlItem16.Location = new System.Drawing.Point(640, 66);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Size = new System.Drawing.Size(271, 34);
            this.layoutControlItem16.Text = "倾覆方向:";
            this.layoutControlItem16.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem16.TextSize = new System.Drawing.Size(78, 22);
            this.layoutControlItem16.TextToControlDistance = 5;
            // 
            // layoutControlItem17
            // 
            this.layoutControlItem17.Control = this.SearchMaxRadius;
            this.layoutControlItem17.Location = new System.Drawing.Point(0, 32);
            this.layoutControlItem17.Name = "layoutControlItem17";
            this.layoutControlItem17.Size = new System.Drawing.Size(300, 34);
            this.layoutControlItem17.Text = "长半轴(max):";
            this.layoutControlItem17.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem17.TextSize = new System.Drawing.Size(107, 22);
            this.layoutControlItem17.TextToControlDistance = 5;
            // 
            // layoutControlItem18
            // 
            this.layoutControlItem18.Control = this.SearchMedRadius;
            this.layoutControlItem18.Location = new System.Drawing.Point(300, 32);
            this.layoutControlItem18.Name = "layoutControlItem18";
            this.layoutControlItem18.Size = new System.Drawing.Size(340, 34);
            this.layoutControlItem18.Text = "中半轴(med):";
            this.layoutControlItem18.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem18.TextSize = new System.Drawing.Size(108, 22);
            this.layoutControlItem18.TextToControlDistance = 5;
            // 
            // layoutControlItem19
            // 
            this.layoutControlItem19.Control = this.SearchMinRadius;
            this.layoutControlItem19.Location = new System.Drawing.Point(640, 32);
            this.layoutControlItem19.Name = "layoutControlItem19";
            this.layoutControlItem19.Size = new System.Drawing.Size(271, 34);
            this.layoutControlItem19.Text = "短半轴(min):";
            this.layoutControlItem19.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem19.TextSize = new System.Drawing.Size(103, 22);
            this.layoutControlItem19.TextToControlDistance = 5;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 5;
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(927, 310);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.tabPane1;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(927, 310);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // SgsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SgsUserControl";
            this.Size = new System.Drawing.Size(927, 310);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).EndInit();
            this.tabPane1.ResumeLayout(false);
            this.tabNavigationPage1.ResumeLayout(false);
            this.tabNavigationPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SearchMinRadius.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchMedRadius.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchMaxRadius.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rake.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Azimuth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.krigType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraBars.Navigation.TabPane tabPane1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private VariogramControl variogramControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraEditors.ComboBoxEdit krigType;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.TextEdit MaxData;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.TextEdit Rake;
        private DevExpress.XtraEditors.TextEdit Dip;
        private DevExpress.XtraEditors.TextEdit Azimuth;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem15;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem16;
        private DevExpress.XtraEditors.TextEdit SearchMinRadius;
        private DevExpress.XtraEditors.TextEdit SearchMedRadius;
        private DevExpress.XtraEditors.TextEdit SearchMaxRadius;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem17;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem18;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem19;
    }
}