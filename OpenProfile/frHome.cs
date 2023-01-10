using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace OpenProfile
{
    public partial class frHome : Telerik.WinControls.UI.RadForm
    {
        public frHome()
        {
            InitializeComponent();
        }

        string path_account = "Data";

        private void btn_Lấy_path_chrome_Click(object sender, EventArgs e)
        {
            var path = Microsoft.Win32.Registry.GetValue(@"HKEY_CLASSES_ROOT\ChromeHTML\shell\open\command", null, null) as string;
            if (path != null)
            {
                var split = path.Split('\"');
                path = split.Length >= 2 ? split[1] : null;
            }
            txt_path.Text = path;
            Properties.Settings.Default.path_chrome = txt_path.Text;
            Properties.Settings.Default.Save();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_path.Text) == true)
            {
                MessageBox.Show("Vui lòng thực hiện lấy đường dẫn Chrome trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var url = "https://taki.app";
            foreach (var row in lst_account.SelectedRows)
            {
                string name = row.Cells["name"].Value.ToString();
                using (var process = new Process())
                {
                    process.StartInfo.FileName = txt_path.Text;
                    process.StartInfo.Arguments = url + "  --profile-directory=\""+ name + "\"";
                    process.Start();
                }
            }
                
        }

        private void Lấy_Profile()
        {
            lst_account.Rows.Clear();
            string[] files = Directory.GetFiles(path_account);
            for (int i = 0; i < files.Count(); i++)
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.lst_account.MasterView);
                rowInfo.Cells["stt"].Value = i + 1;
                rowInfo.Cells["name"].Value = Path.GetFileName(files[i]).Remove(Path.GetFileName(files[i]).Length - 5, 5);
                lst_account.Rows.Add(rowInfo);
            }
        }

        private void frHome_Load(object sender, EventArgs e)
        {
            txt_path.Text = Properties.Settings.Default.path_chrome;
            Lấy_Profile();
        }

        private void btn_thêm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_name.Text) == false)
            {
                string fileName = "Data\\" + txt_name.Text + ".data";
                if (File.Exists(fileName))
                {
                    MessageBox.Show("Đã tồn tại profile này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    File.Create(fileName);
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(lst_account.MasterView);
                    rowInfo.Cells["stt"].Value = lst_account.RowCount + 1;
                    rowInfo.Cells["name"].Value = txt_name.Text;
                    lst_account.Rows.Add(rowInfo);
                    txt_name.Text = "";
                }

            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {

            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (DialogResult.OK == rs)
            {
                List<GridViewRowInfo> lst = new List<GridViewRowInfo>();
                foreach (var row in lst_account.SelectedRows)
                {
                    string name = row.Cells["name"].Value.ToString();
                    var path_profile = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                    path_profile += @"\Local\Google\Chrome\User Data\"+ name;
                    Directory.Delete(path_profile, true);
                    File.Delete(path_account + "\\" + name + ".data");
                    lst.Add(row);
                }
                foreach (var row in lst)
                {
                    lst_account.Rows.Remove(row);
                }
            }
            
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            Lấy_Profile();
        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {
            var path_profile = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            path_profile += @"\Local\Google\Chrome\User Data\";
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] dirs = Directory.GetDirectories(fbd.SelectedPath);
                    foreach(var dir in dirs)
                    {
                        string account = dir.Split('\\')[dir.Split('\\').Length - 1];
                        string path_check = path_profile + "\\" + account;
                        if (Directory.Exists(path_check))
                        {
                            DialogResult rs = MessageBox.Show("Đã tồn tại profile: "+account+"\nXác nhận ghi đè?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (DialogResult.OK == rs)
                            {
                                CopyFilesRecursively(dir + "\\Default", path_profile + "\\" + account);
                            }
                        }
                        else
                        {
                            CopyFilesRecursively(dir + "\\Default", path_profile + "\\" + account);
                        }

                        string fileName = "Data\\" + account + ".data";
                        if (!File.Exists(fileName))
                        {
                            File.Create(fileName);
                            GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(lst_account.MasterView);
                            rowInfo.Cells["stt"].Value = lst_account.RowCount + 1;
                            rowInfo.Cells["name"].Value = account;
                            lst_account.Rows.Add(rowInfo);
                        }

                        Directory.Delete(dir, true);
                    }
                }
            }
        }

        public static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        private void txt_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_path_TextChanged(object sender, EventArgs e)
        {

        }

        private void lst_account_Click(object sender, EventArgs e)
        {

        }

        private void radGroupBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
