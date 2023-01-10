namespace OpenProfile
{
    partial class frHome
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.telerikMetroBlueTheme1 = new Telerik.WinControls.Themes.TelerikMetroBlueTheme();
            this.radGroupBox3 = new Telerik.WinControls.UI.RadGroupBox();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.btn_Lấy_path_chrome = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.btn_Upload = new Telerik.WinControls.UI.RadButton();
            this.btn = new Telerik.WinControls.UI.RadButton();
            this.txt_path = new Telerik.WinControls.UI.RadTextBox();
            this.lst_account = new Telerik.WinControls.UI.RadGridView();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.txt_name = new Telerik.WinControls.UI.RadTextBox();
            this.btn_thêm = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox3)).BeginInit();
            this.radGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Lấy_path_chrome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Upload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_path)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lst_account)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lst_account.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_thêm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox3
            // 
            this.radGroupBox3.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox3.Controls.Add(this.lst_account);
            this.radGroupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radGroupBox3.HeaderText = "Danh sách tài khoản";
            this.radGroupBox3.Location = new System.Drawing.Point(0, 154);
            this.radGroupBox3.Name = "radGroupBox3";
            this.radGroupBox3.Size = new System.Drawing.Size(578, 317);
            this.radGroupBox3.TabIndex = 12;
            this.radGroupBox3.Text = "Danh sách tài khoản";
            this.radGroupBox3.ThemeName = "TelerikMetroBlue";
            this.radGroupBox3.Click += new System.EventHandler(this.radGroupBox3_Click);
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.txt_name);
            this.radGroupBox1.Controls.Add(this.txt_path);
            this.radGroupBox1.Controls.Add(this.btn_Upload);
            this.radGroupBox1.Controls.Add(this.btn_thêm);
            this.radGroupBox1.Controls.Add(this.radButton1);
            this.radGroupBox1.Controls.Add(this.radButton2);
            this.radGroupBox1.Controls.Add(this.btn);
            this.radGroupBox1.Controls.Add(this.btn_Lấy_path_chrome);
            this.radGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radGroupBox1.HeaderText = "Cài đặt";
            this.radGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(578, 148);
            this.radGroupBox1.TabIndex = 13;
            this.radGroupBox1.Text = "Cài đặt";
            this.radGroupBox1.ThemeName = "TelerikMetro";
            // 
            // btn_Lấy_path_chrome
            // 
            this.btn_Lấy_path_chrome.Location = new System.Drawing.Point(12, 21);
            this.btn_Lấy_path_chrome.Name = "btn_Lấy_path_chrome";
            this.btn_Lấy_path_chrome.Size = new System.Drawing.Size(151, 24);
            this.btn_Lấy_path_chrome.TabIndex = 0;
            this.btn_Lấy_path_chrome.Text = "Lấy đường dẫn chrome";
            this.btn_Lấy_path_chrome.Click += new System.EventHandler(this.btn_Lấy_path_chrome_Click);
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(422, 106);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(151, 34);
            this.radButton2.TabIndex = 0;
            this.radButton2.Text = "Khởi tạo lại danh sách";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // btn_Upload
            // 
            this.btn_Upload.Location = new System.Drawing.Point(422, 21);
            this.btn_Upload.Name = "btn_Upload";
            this.btn_Upload.Size = new System.Drawing.Size(151, 24);
            this.btn_Upload.TabIndex = 0;
            this.btn_Upload.Text = "Tải lên profile";
            this.btn_Upload.Click += new System.EventHandler(this.btn_Upload_Click);
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(12, 109);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(151, 34);
            this.btn.TabIndex = 0;
            this.btn.Text = "Mở tài khoản";
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // txt_path
            // 
            this.txt_path.Location = new System.Drawing.Point(169, 21);
            this.txt_path.Name = "txt_path";
            this.txt_path.Size = new System.Drawing.Size(247, 24);
            this.txt_path.TabIndex = 1;
            this.txt_path.ThemeName = "TelerikMetroBlue";
            this.txt_path.TextChanged += new System.EventHandler(this.txt_path_TextChanged);
            // 
            // lst_account
            // 
            this.lst_account.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lst_account.Location = new System.Drawing.Point(2, 37);
            // 
            // 
            // 
            this.lst_account.MasterTemplate.AddNewRowPosition = Telerik.WinControls.UI.SystemRowPosition.Bottom;
            this.lst_account.MasterTemplate.AllowAddNewRow = false;
            this.lst_account.MasterTemplate.AllowColumnReorder = false;
            this.lst_account.MasterTemplate.AllowDragToGroup = false;
            this.lst_account.MasterTemplate.AutoGenerateColumns = false;
            gridViewTextBoxColumn1.HeaderText = "STT";
            gridViewTextBoxColumn1.Name = "stt";
            gridViewTextBoxColumn2.HeaderText = "Tên Profile";
            gridViewTextBoxColumn2.Name = "name";
            gridViewTextBoxColumn2.Width = 500;
            this.lst_account.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2});
            this.lst_account.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.lst_account.Name = "lst_account";
            this.lst_account.ReadOnly = true;
            this.lst_account.Size = new System.Drawing.Size(574, 278);
            this.lst_account.TabIndex = 0;
            this.lst_account.Click += new System.EventHandler(this.lst_account_Click);
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(169, 109);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(91, 34);
            this.radButton1.TabIndex = 0;
            this.radButton1.Text = "Xóa profile";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(12, 63);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(247, 24);
            this.txt_name.TabIndex = 1;
            this.txt_name.ThemeName = "TelerikMetroBlue";
            this.txt_name.TextChanged += new System.EventHandler(this.txt_name_TextChanged);
            // 
            // btn_thêm
            // 
            this.btn_thêm.Location = new System.Drawing.Point(286, 57);
            this.btn_thêm.Name = "btn_thêm";
            this.btn_thêm.Size = new System.Drawing.Size(91, 34);
            this.btn_thêm.TabIndex = 0;
            this.btn_thêm.Text = "Thêm Profile";
            this.btn_thêm.Click += new System.EventHandler(this.btn_thêm_Click);
            // 
            // frHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 471);
            this.Controls.Add(this.radGroupBox1);
            this.Controls.Add(this.radGroupBox3);
            this.Name = "frHome";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Open Profile";
            this.ThemeName = "TelerikMetroBlue";
            this.Load += new System.EventHandler(this.frHome_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox3)).EndInit();
            this.radGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Lấy_path_chrome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Upload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_path)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lst_account.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lst_account)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_thêm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.TelerikMetroBlueTheme telerikMetroBlueTheme1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox3;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadTextBox txt_path;
        private Telerik.WinControls.UI.RadButton btn_Upload;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton btn;
        private Telerik.WinControls.UI.RadButton btn_Lấy_path_chrome;
        private Telerik.WinControls.UI.RadGridView lst_account;
        private Telerik.WinControls.UI.RadTextBox txt_name;
        private Telerik.WinControls.UI.RadButton btn_thêm;
        private Telerik.WinControls.UI.RadButton radButton1;
    }
}