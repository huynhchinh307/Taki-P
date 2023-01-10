namespace ProjectAuto
{
    partial class frmViewTaki
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewTaki));
            this.tab = new Telerik.WinControls.UI.RadTabbedFormControl();
            this.home = new Telerik.WinControls.UI.RadTabbedFormControlTab();
            ((System.ComponentModel.ISupportInitialize)(this.tab)).BeginInit();
            this.tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tab
            // 
            this.tab.BackgroundImage = global::ProjectAuto.Properties.Resources.background_vip;
            this.tab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tab.Controls.Add(this.home);
            this.tab.Location = new System.Drawing.Point(0, 0);
            this.tab.Name = "tab";
            this.tab.SelectedTab = this.home;
            this.tab.ShowNewTabButton = false;
            this.tab.ShowTabCloseButton = false;
            this.tab.Size = new System.Drawing.Size(416, 923);
            this.tab.TabIndex = 0;
            this.tab.TabSpacing = -1;
            this.tab.TabWidth = 150;
            this.tab.Text = "Taki Chrome";
            this.tab.ThemeName = "TelerikMetro";
            // 
            // home
            // 
            this.home.BackgroundImage = global::ProjectAuto.Properties.Resources.project_t_2;
            this.home.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.home.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.home.Location = new System.Drawing.Point(1, 31);
            this.home.Name = "home";
            this.home.Size = new System.Drawing.Size(414, 891);
            this.home.Text = "Home";
            // 
            // frmViewTaki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 923);
            this.Controls.Add(this.tab);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmViewTaki";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Taki Chrome";
            this.ThemeName = "TelerikMetro";
            ((System.ComponentModel.ISupportInitialize)(this.tab)).EndInit();
            this.tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadTabbedFormControl tab;
        private Telerik.WinControls.UI.RadTabbedFormControlTab home;
    }
}
