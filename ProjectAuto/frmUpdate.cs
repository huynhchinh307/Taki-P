using AutoUpdaterDotNET;
using Newtonsoft.Json;
using ProjectAuto.Common;
using ProjectAuto.Taki;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ProjectAuto
{
    public partial class frmUpdate : Telerik.WinControls.UI.RadForm
    {
        Setting setting = null;
        string pathConfig = "Taki\\config.json";
        public frmUpdate()
        {
            InitializeComponent();
            if (File.Exists(pathConfig) == false)
            {
                File.Create(pathConfig);
            }
            setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText(pathConfig));
            if (setting == null)
            {
                setting = new Setting();
            }
            if (setting.basic == null)
            {
                setting.basic = new Basic();
            }
            if (setting.advanced == null)
            {
                setting.advanced = new Advanced();

                setting.advanced.max_apr_user = 5000;
                setting.advanced.max_follow_user = 1000;
                setting.advanced.max_give_post_user = 3;
                setting.advanced.max_give_user = 800;
                setting.advanced.min_follow_user = 10;
                setting.advanced.give_withdraw = true;
            }


            string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
            FileIO.Create_File_From_Json(json, pathConfig);
            txt_Key.Text = setting?.key;

            string path_file = "Taki\\follow.txt";
            if (File.Exists(path_file) == false)
            {
                File.Create(path_file);
            }
        }

        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 65432;
        bool click_button = false;

        static ASCIIEncoding encoding = new ASCIIEncoding();

        private void btn_run_Click(object sender, EventArgs e)
        {
            int coupon = 0;
            if (txt_Key.Text.Equals("MYJAA-QWYLU-KSYSH-XNYHU"))
            {
                coupon = 0;
            }
            else if (txt_Key.Text.Equals("FCWSB-DOLWF-LOJCL-SKWXF"))
            {
                coupon = 10;
            }
            else if (txt_Key.Text.Equals("EYXVL-MADPK-TYUND-RTLCZ"))
            {
                coupon = 15;
            }
            else
            {
                MessageBox.Show("Vui lòng nhập Key kích hoạt !!!\nNếu bạn không có xin vui lòng liên hệ nhà phát hành", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            setting.key = txt_Key.Text;
            string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
            FileIO.Create_File_From_Json(json, pathConfig);
            this.Hide();
            var frmTaki = new frmTaki(coupon);
            frmTaki.Closed += (s, args) => this.Close();
            frmTaki.Show();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            click_button = true;
            AutoUpdater.Start("https://raw.githubusercontent.com/huynhchinh307/Taki/main/Update/update.xml");
        }

        private void frmUpdate_Load(object sender, EventArgs e)
        {


            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            string version = fvi.FileVersion;
            txt_version.Text = "Phiên bản: " + version;
            AutoUpdater.DownloadPath = "update";
            System.Timers.Timer timer = new System.Timers.Timer
            {
                Interval = 60 * 60 * 1000,
                SynchronizingObject = this
            };
            timer.Elapsed += delegate
            {
                AutoUpdater.Start("https://raw.githubusercontent.com/huynhchinh307/Taki/main/Update/update.xml");
            };
            timer.Start();

        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.IsUpdateAvailable)
            {
                DialogResult dialogResult;
                dialogResult =
                        MessageBox.Show(
                            $@"Bạn ơi, phần mềm của bạn có phiên bản mới {args.CurrentVersion}. Phiên bản bạn đang sử dụng hiện tại  {args.InstalledVersion}. Bạn có muốn cập nhật phần mềm không?", @"Cập nhật phần mềm",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                if (dialogResult.Equals(DialogResult.Yes) || dialogResult.Equals(DialogResult.OK))
                {
                    try
                    {
                        //Process cmd = new Process();
                        //cmd.StartInfo.FileName = @"C:\Program Files (x86)\AnyDesk\AnyDesk.exe";
                        //cmd.StartInfo.Arguments = "--get-id";
                        //cmd.StartInfo.RedirectStandardInput = true;
                        //cmd.StartInfo.RedirectStandardOutput = true;
                        //cmd.StartInfo.CreateNoWindow = true;
                        //cmd.StartInfo.UseShellExecute = false;
                        //cmd.Start();
                        //cmd.WaitForExit();
                        //string id = cmd.StandardOutput.ReadToEnd();

                        //UTF8Encoding encoding = new UTF8Encoding();
                        //TcpClient client = new TcpClient();
                        //// 1. connect
                        //client.Connect("103.82.25.22", PORT_NUMBER);
                        //Stream stream = client.GetStream();
                        //string str = id + "|Date:" + DateTime.Now + "|Taki:" + Properties.Taki.Default.donate_taki + "|Follow:" + Properties.Taki.Default.donate_follow + "\n";
                        //byte[] data = encoding.GetBytes(str);
                        //stream.Write(data, 0, data.Length);
                        //str = "x";
                        //data = encoding.GetBytes(str);
                        //stream.Write(data, 0, data.Length);
                        //MessageBox.Show("Cảm ơn bạn đã sử dụng phần mềm\nSố Taki ủng hộ: " + Properties.Taki.Default.donate_taki + "\nSố Follow đã ủng hộ: " + Properties.Taki.Default.donate_follow + "\nChúc ngày làm việc thuận lợi !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Properties.Taki.Default.donate_taki = 0;
                        Properties.Taki.Default.donate_follow = 0;
                        Properties.Taki.Default.Save();
                    }
                    catch
                    {
                        //MessageBox.Show("Không thể gửi báo cáo hằng ngày !!!");
                    }
                    try
                    {
                        if (AutoUpdater.DownloadUpdate(args))
                        {
                            Application.Exit();
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                click_button = false;
            }
            else
            {
                if (click_button == true)
                {
                    MessageBox.Show(@"Phiên bản bạn đang sử dụng đã được cập nhật mới nhất.", @"Cập nhật phần mềm",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    click_button = false;
                }
                click_button = false;
            }

        }

        private void btn_convert_Click(object sender, EventArgs e)
        {
            btn_convert.Enabled = false;
            string[] files = Directory.GetFiles("Taki\\Data");
            foreach (string file in files)
            {
                string name = file.Substring(file.LastIndexOf("\\") + 1);
                string name_account = name.Remove(name.Length - 5, 5);
                string dir_name = "Taki\\Profile\\" + name_account;
                if (!Directory.Exists(dir_name))
                {
                    Directory.CreateDirectory(dir_name);
                }
                string path = Directory.GetCurrentDirectory();
                string source = setting.basic.path_profile + "\\" + name_account + "\\Default\\IndexedDB\\https_taki.app_0.indexeddb.leveldb\\";
                string dir = path + "\\" + dir_name + "\\https_taki.app_0.indexeddb.leveldb\\";
                Move(source, dir);
                Console.WriteLine("Move : " + name_account);
            }

            MessageBox.Show("Đã convert sang 3.0 thành công !!!");
        }

        public new void Move(string source, string target)
        {
            try
            {
                if (!Directory.Exists(source))
                {
                    throw new System.IO.DirectoryNotFoundException("Source directory couldn't be found.");
                }

                if (Directory.Exists(target))
                {
                    throw new System.IO.IOException("Target directory already exists.");
                }

                DirectoryInfo sourceInfo = Directory.CreateDirectory(source);
                DirectoryInfo targetInfo = Directory.CreateDirectory(target);

                if (sourceInfo.FullName == targetInfo.FullName)
                {
                    throw new System.IO.IOException("Source and target directories are the same.");
                }

                Stack<DirectoryInfo> sourceDirectories = new Stack<DirectoryInfo>();
                sourceDirectories.Push(sourceInfo);

                Stack<DirectoryInfo> targetDirectories = new Stack<DirectoryInfo>();
                targetDirectories.Push(targetInfo);

                while (sourceDirectories.Count > 0)
                {
                    DirectoryInfo sourceDirectory = sourceDirectories.Pop();
                    DirectoryInfo targetDirectory = targetDirectories.Pop();

                    foreach (FileInfo file in sourceDirectory.GetFiles())
                    {
                        file.CopyTo(Path.Combine(targetDirectory.FullName, file.Name), overwrite: true);
                    }

                    foreach (DirectoryInfo subDirectory in sourceDirectory.GetDirectories())
                    {
                        sourceDirectories.Push(subDirectory);
                        targetDirectories.Push(targetDirectory.CreateSubdirectory(subDirectory.Name));
                    }
                }
            }
            catch
            {

            }

            //sourceInfo.Delete(true);
        }
    }
}
