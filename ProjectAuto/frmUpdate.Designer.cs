namespace ProjectAuto
{
    partial class frmUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdate));
            this.materialBlueGreyTheme1 = new Telerik.WinControls.Themes.MaterialBlueGreyTheme();
            this.btn_run = new Telerik.WinControls.UI.RadButton();
            this.btn_update = new Telerik.WinControls.UI.RadButton();
            this.txt_version = new Telerik.WinControls.UI.RadLabel();
            this.txt_Key = new Telerik.WinControls.UI.RadTextBox();
            this.btn_convert = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btn_run)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_update)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_version)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Key)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_convert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_run
            // 
            this.btn_run.BackColor = System.Drawing.Color.DarkOrange;
            this.btn_run.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold);
            this.btn_run.ForeColor = System.Drawing.Color.White;
            this.btn_run.Location = new System.Drawing.Point(648, 31);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(120, 36);
            this.btn_run.TabIndex = 0;
            this.btn_run.Text = "Khởi động";
            this.btn_run.ThemeName = "MaterialBlueGrey";
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // btn_update
            // 
            this.btn_update.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btn_update.ForeColor = System.Drawing.Color.White;
            this.btn_update.Location = new System.Drawing.Point(413, 31);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(229, 36);
            this.btn_update.TabIndex = 0;
            this.btn_update.Text = "Kiểm tra cập nhật";
            this.btn_update.ThemeName = "MaterialBlueGrey";
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // txt_version
            // 
            this.txt_version.Location = new System.Drawing.Point(12, 343);
            this.txt_version.Name = "txt_version";
            this.txt_version.Size = new System.Drawing.Size(162, 21);
            this.txt_version.TabIndex = 1;
            this.txt_version.Text = "Phiên bản hiện tai: 1.0.1";
            this.txt_version.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.txt_version.ThemeName = "MaterialBlueGrey";
            // 
            // txt_Key
            // 
            this.txt_Key.Location = new System.Drawing.Point(165, 31);
            this.txt_Key.Name = "txt_Key";
            this.txt_Key.NullText = "Mã kích hoạt";
            this.txt_Key.PasswordChar = '●';
            this.txt_Key.Size = new System.Drawing.Size(226, 36);
            this.txt_Key.TabIndex = 2;
            this.txt_Key.ThemeName = "MaterialBlueGrey";
            this.txt_Key.UseSystemPasswordChar = true;
            // 
            // btn_convert
            // 
            this.btn_convert.BackColor = System.Drawing.Color.DarkTurquoise;
            this.btn_convert.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Bold);
            this.btn_convert.ForeColor = System.Drawing.Color.White;
            this.btn_convert.Location = new System.Drawing.Point(650, 318);
            this.btn_convert.Name = "btn_convert";
            this.btn_convert.Size = new System.Drawing.Size(120, 36);
            this.btn_convert.TabIndex = 0;
            this.btn_convert.Text = "Convert 3.0";
            this.btn_convert.ThemeName = "MaterialBlueGrey";
            this.btn_convert.Click += new System.EventHandler(this.btn_convert_Click);
            // 
            // frmUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ProjectAuto.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(782, 366);
            this.Controls.Add(this.txt_Key);
            this.Controls.Add(this.txt_version);
            this.Controls.Add(this.btn_update);
            this.Controls.Add(this.btn_convert);
            this.Controls.Add(this.btn_run);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUpdate";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TAKI - Taki mãi đỉnh";
            this.ThemeName = "MaterialBlueGrey";
            this.Load += new System.EventHandler(this.frmUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btn_run)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_update)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_version)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Key)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_convert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.Themes.MaterialBlueGreyTheme materialBlueGreyTheme1;
        private Telerik.WinControls.UI.RadButton btn_run;
        private Telerik.WinControls.UI.RadButton btn_update;
        private Telerik.WinControls.UI.RadLabel txt_version;
        private Telerik.WinControls.UI.RadTextBox txt_Key;
        private Telerik.WinControls.UI.RadButton btn_convert;
    }
}
