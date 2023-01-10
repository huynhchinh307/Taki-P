using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Telerik.WinControls.UI;
using System.Threading;
using OpenQA.Selenium;
using TMProxyHelper;
using ViOTP;
using System.Globalization;

namespace Taki_Ref
{
    public partial class Taki_REF : RadForm
    {
        static Model model = new Model();
        string path = @"D:\TakiRef";
        List<Taki> takiAccounts = new List<Taki>();
        List<Taki> takiRunning = new List<Taki>();
        public Taki_REF()
        {
            InitializeComponent();
            RadTextBox.CheckForIllegalCrossThreadCalls = false;
            RadLabel.CheckForIllegalCrossThreadCalls = false;
            RadGridView.CheckForIllegalCrossThreadCalls = false;
            RadButton.CheckForIllegalCrossThreadCalls = false;
        }

        private void Taki_REF_Load(object sender, EventArgs e)
        {
            txtSMS.Text = Properties.TakiRef.Default.sms.ToString();
            LoadAccount();
            string token = Properties.TakiRef.Default.api_sms;
            if (string.IsNullOrEmpty(token) == false)
            {
                ResponseBalance response = ViOTPHelper.Balance(token);
                long balance = response.data.balance;
                CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
                string balanceVnd = balance.ToString("#,###", cul.NumberFormat);
                txt_balance.Text = balanceVnd + "đ";
            }

            txt_captcha.Text = Properties.TakiRef.Default.api_captcha;
            txt_SMS.Text = Properties.TakiRef.Default.api_sms;
            txt_proxy.Text = Properties.TakiRef.Default.api_proxy;

        }
        private void LoadAccount()
        {
            lst_account.Rows.Clear();
            takiAccounts = new List<Taki>();
            // Xữ lý load thread account
            string[] files = Directory.GetFiles("Data");
            for (int i = 0; i < files.Count(); i++)
            {
                Taki taki = JsonConvert.DeserializeObject<Taki>(FileIO.ReadFile(files[i]));
                takiAccounts.Add(taki);

            }

            takiAccounts = takiAccounts.OrderBy(r => r.SaveDate).ToList();
            int stt = 1;
            foreach (var taki in takiAccounts)
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.lst_account.MasterView);
                rowInfo.Cells["stt"].Value = stt;
                rowInfo.Cells["name"].Value = taki.name;
                rowInfo.Cells["num_ref"].Value = taki.num_ref;
                rowInfo.Cells["label"].Value = taki.Label;
                rowInfo.Cells["status"].Value = "Đang đợi lệnh";
                stt++;
                lst_account.Rows.Add(rowInfo);
            }
        }

        private void open_browser_Click(object sender, EventArgs e)
        {
            var url = txt_link.Text;
            Process.Start(url);
        }

        private void btnTạo_tài_khoản_Click(object sender, EventArgs e)
        {
            Taki taki = new Taki();
            taki.name = txt_name.Text;
            taki.link = txt_link.Text;
            taki.Label = txt_label.Text;
            taki.SaveDate = DateTime.Now;
            takiAccounts.Add(taki);
            FileIO.CreateFile(taki);
            GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(lst_account.MasterView);
            rowInfo.Cells["stt"].Value = takiAccounts.Count();
            rowInfo.Cells["name"].Value = taki.name;
            rowInfo.Cells["num_ref"].Value = 0;
            rowInfo.Cells["label"].Value = taki.Label;
            rowInfo.Cells["status"].Value = "Đang đợi lệnh";
            Add_Taki_Profile(rowInfo);
            txt_name.Text = "";
            txt_link.Text = "";
        }

        private void Add_Taki_Profile(GridViewDataRowInfo row)
        {
            if (lst_account.InvokeRequired)
            {
                lst_account.Invoke(new MethodInvoker(() => { this.lst_account.Rows.Add(row); }));
            }
            else
            {
                lst_account.Rows.Add(row);
            }
        }

        private void lst_account_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (lst_account.SelectedRows.Count > 0)
            {
                GridViewRowInfo row = lst_account.SelectedRows.First();
                string name = row.Cells["name"].Value.ToString();
                Taki taki = takiAccounts.Where(r => r.name == name).First();
                txt_name.Text = taki.name;
                txt_link.Text = taki.link;
                txt_label.Text = taki.Label;
            }
        }

        private void btn_Xoá_Click(object sender, EventArgs e)
        {
            GridViewRowInfo row = lst_account.SelectedRows.First();
            string name = row.Cells[1].Value.ToString();
            Taki taki = takiAccounts.Where(r => r.name == name).First();
            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (DialogResult.OK == rs)
            {
                // Xoá danh sách đang chạy
                takiAccounts.Remove(taki);
                // Xoá danh sách hiển thị
                lst_account.Rows.Remove(row);

                // Xoá Profile
                if (Directory.Exists(path + "/" + taki.name) == true)
                {
                    Directory.Delete(path + "/" + taki.name, true);
                }
                // Xoá file data
                File.Delete("TakiRef/Data/" + taki.name + ".data");
            }
        }

        private void btn_Chạy_Click(object sender, EventArgs e)
        {
            List<string> proxy = new List<string>();
            proxy.Add(txt_proxy.Text);
            takiRunning = takiAccounts.Where(r => r.num_ref < 3 && r.Label == txt_profile.Text).ToList();
            for (int i = 0; i < int.Parse(txt_Số_luồng.Text); i++)
            {
                Thread Chrome = new Thread(() =>
                {
                    Thực_hiện_autoAsync(proxy[i]);
                })
                { IsBackground = true };
                Chrome.Name = "Taki : " + (i + 1);
                Chrome.Start();
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
            }
        }


        #region Chức năng delay
        public void Delay(int delay)
        {
            while (delay > 0)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                delay--;
            }
        }
        #endregion

        private void Thực_hiện_autoAsync(string api_proxy)
        {
        Next_Taki:
            Taki Taki = null;
            lock (takiRunning)
            {
                if (takiRunning.Count > 0)
                {
                    Taki = takiRunning.First();
                    takiRunning.Remove(Taki);
                }
            }
            if (Taki != null)
            {

                lbl_test.Text = "Đang chạy còn lại: " + takiRunning.Count();
                GridViewRowInfo row = null;
                foreach (var info in lst_account.Rows)
                {
                    if (info.Cells["name"].Value.ToString() == Taki.name)
                    {
                        row = info;
                        break;
                    }
                }

                Taki.num_ref += Tạo_Ref(Taki.link, api_proxy, row);
                FileIO.CreateFile(Taki);
                goto Next_Taki;
            }
            else
            {
                lbl_test.Text = "Auto đã chạy cực mượt";
                return;
            }
        }

        private int Tạo_Ref(string link, string proxy, GridViewRowInfo row)
        {
            int số_ref = 0;
            bool keep = ckb_keep.Checked;
            string profile_name;
            int value = int.Parse(row.Cells["num_ref"].Value.ToString());
        Tạo:
            Random random = new Random();

            string username = Support.getUsername(keep) + random.Next(1, 9999);
            if (username.Length > 15)
            {
                username = username.Remove(0, username.Length - 15);
            }
            profile_name = username;

            string profile = path + @"\" + username;
            if (Directory.Exists(profile) == true)
            {
                goto Tạo;
            }

            if (số_ref == 3)
            {

                row.Cells["num_ref"].Value = value + số_ref;
                row.Update_Status("Đã tạo ref thành công !!!");
                return 3;
            }
            string request = TMAPIHelper.GetNewProxy(proxy, null, null);
            TmProxy tm = JsonConvert.DeserializeObject<TmProxy>(request);
            if (string.IsNullOrEmpty(tm.data.https) == false)
            {
                proxy = tm.data.https;
            }
            else
            {
                request = TMAPIHelper.GetCurrentProxy(proxy);
                tm = JsonConvert.DeserializeObject<TmProxy>(request);
                proxy = tm.data.https;
            }

            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeOptions option = new ChromeOptions();
            option.AddArgument("window-size=414,896");
            option.AddExcludedArgument("enable-automation");
            option.AddAdditionalOption("useAutomationExtension", false);
            option.AddArgument("--autoplay-policy=no-user-gesture-required");
            option.AddArgument("--mute-audio");
            option.AddArgument("--disable-infobars");
            option.AddArgument("--disable-default-apps");
            option.AddArgument("user-data-dir=" + profile);
            option.AddArgument("--user-agent=Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25");
            option.AddArgument("--app=" + link);
            option.AddExtension("captcha.crx");
            if (string.IsNullOrEmpty(proxy) == false)
            {
                option.AddArgument("--proxy-server=" + proxy);
            }
            ChromeDriver driver = new ChromeDriver(driverService, option);
            Support.Setting_Captcha(driver);
            Delay(1);
            string url = driver.Url;
            if (url == "https://taki.app/")
            {
                row.Update_Status("Đã hết lượt giới thiệu !!!");
                row.Cells["num_ref"].Value = 3;
                driver.Close();
                driver.Quit();
                return 3;
            }
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(25);
                driver.FindElement(By.CssSelector(Model.Tạo_Tài_Khoản.Accept_Invite)).Click();
                row.Update_Status("Đang tạo ref " + (số_ref + 1) + "/3");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Get_Code:
                Delay(2);
                driver.FindElement(By.CssSelector(Model.Tạo_Tài_Khoản.Continue_with_Phone)).Click();
                Delay(2);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Choose_Country)).Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Choose_VN)).Click();
                row.Update_Status("Đang lấy số điện thoại...");
                ResponsePhone phone = ViOTPHelper.Get_New_Phone(Properties.TakiRef.Default.api_sms);

                if (phone?.data.phone_number != null)
                {
                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
                    string balanceVnd = phone.data.balance.ToString("#,###", cul.NumberFormat);
                    txt_balance.Text = balanceVnd + "đ";

                    string sdt = phone?.data.phone_number;
                    driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Input_SDT)).SendKeys(sdt);
                    row.Update_Status("Lấy được SDT: " + phone?.data.phone_number);
                    driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Continue_Input_Sdt)).Click();
                    Delay(5);
                    IWebElement send_ok = driver.FindElement(By.CssSelector(Model.Tạo_Tài_Khoản.send_code));
                    string msg = send_ok.GetAttribute("innerHTML");
                    if (msg != "We sent you a code")
                    {
                        try
                        {
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                            IWebElement show = driver.FindElement(By.CssSelector(Model.Captcha_Invisable));
                            string ss = "c-xfj014palzwd";
                            string nameVal = "c-dalo9wmyt4cj";
                            string name = show.GetAttribute("name");
                            if (name != nameVal || name != "c-38qkro1o8fbd" || name != "c-f559p6xrrsdx")
                            {
                                IWebElement captcha_invisable = driver.FindElement(By.CssSelector(Model.Captcha.captcha_solver));
                                driver.ExecuteScript("arguments[0].click();", captcha_invisable);
                                Delay(1);
                                string solver = captcha_invisable.GetAttribute("data-state");
                                while (solver != "solved" && solver != "error" && solver != "ready")
                                {
                                    row.Update_Status("Đang giải captcha ẩn");
                                    Delay(2);
                                    solver = captcha_invisable.GetAttribute("data-state");
                                }
                            }
                        }
                        catch
                        {
                        }
                        Delay(2);
                        try
                        {
                            string error_1 = "Firebase: The phone verification request contains an invalid application verifier. The reCAPTCHA token response is either invalid or expired. (auth/invalid-app-credential)."; ;
                            string error_2 = "Firebase: Recaptcha verification failed - EXPIRED (auth/captcha-check-failed).";
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                            IWebElement msggg = driver.FindElement(By.CssSelector(Model.Captcha.msg));
                            string get_msggg = msggg.GetAttribute("innerHTML");
                            if (error_1 == get_msggg || error_2 == get_msggg)
                            {
                                row.Update_Status("Xữ lý captcha ẩn thất bại...");
                                driver.Close();
                                driver.Quit();
                                // Xoá Profile
                                if (Directory.Exists(path + "/" + username) == true)
                                {
                                    Directory.Delete(path + "/" + username, true);
                                }
                                goto Tạo;
                            }
                        }
                        catch
                        {
                            row.Update_Status("Xữ lý captcha ẩn thành công...");
                        }
                    }
                   
                    int check_sms = 0;
                    row.Update_Status("Đang lấy codesim...");
                Get_SMS:
                    Delay(5);
                    ResponseCode code_SMS = ViOTPHelper.Get_Code_Phone(Properties.TakiRef.Default.api_sms, phone.data.request_id);
                    if (code_SMS?.data.Code == null)
                    {
                        check_sms++;
                        if (check_sms == 20)
                        {
                            row.Update_Status("Code: Không lấy được code");
                            driver.FindElement(By.XPath(model.Back_Get_Code)).Click();
                            goto Get_Code;
                        }
                        goto Get_SMS;
                    }
                    else
                    {
                        Properties.TakiRef.Default.sms++;
                        Properties.TakiRef.Default.Save();
                        txtSMS.Text = Properties.TakiRef.Default.sms.ToString();
                        row.Update_Status("Lấy code thành công...");
                        driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Input_Code_SMS)).SendKeys(code_SMS?.data.Code);
                        driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Continue_Code_SMS)).Click();
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    Nhập_Tên:
                        driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Input_Username)).SendKeys(username);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                        driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Continue_Username)).Click();
                        try
                        {
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                            IWebElement validate = driver.FindElement(By.CssSelector(Model.Tạo_Tài_Khoản.val_account));
                            if (validate == null)
                            {
                                row.Update_Status("Tên không hợp lệ...");
                                username = Support.getUsername(keep) + random.Next(1, 9999);
                                if (username.Length > 15)
                                {
                                    username = username.Remove(0, username.Length - 15);
                                }
                                goto Nhập_Tên;
                            }
                        }
                        catch
                        {
                            row.Update_Status("Tên OK...");
                        }
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        try
                        {
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                            IWebElement captcha_invisable = driver.FindElement(By.CssSelector(Model.Captcha.captcha_solver));
                        Try_Resovler:
                            string solver = captcha_invisable.GetAttribute("data-state");
                            while (solver != "solved")
                            {
                                row.Update_Status("Đang giải captcha");
                                Delay(1);
                                solver = captcha_invisable.GetAttribute("data-state");
                            }
                            if (solver == "error")
                            {
                                captcha_invisable.Click();
                                Delay(1);
                                goto Try_Resovler;
                            }
                            row.Update_Status("Đã giải captcha");
                        }
                        catch
                        {
                            row.Update_Status("Đã giải captcha");
                        }
                        stopwatch.Stop();
                        TimeSpan ts = stopwatch.Elapsed;
                        row.Update_Status("Giải captcha thành công: " + ts.Seconds + " s");
                        Delay(2);
                        try
                        {
                            row.Update_Status("Tạo wallet...");
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
                            driver.FindElement(By.XPath(model.Create_My_Coin)).Click();
                            if (keep == true)
                            {
                                Delay(50);
                                row.Update_Status("Đang chờ tạo...");
                                driver.FindElement(By.XPath(model.Goto_Wallet)).Click();
                                Delay(5);
                            }
                        }
                        catch
                        {
                            row.Update_Status("Đang đợi tạo wallet...");
                            driver.Navigate().Refresh();
                            username = Support.getUsername() + random.Next(1, 9999);
                            if (username.Length > 15)
                            {
                                username = username.Remove(0, username.Length - 15);
                            }
                            goto Nhập_Tên;
                        }
                        Delay(10);
                        row.Update_Status("Tạo thành công ref " + (số_ref + 1) + "/3");
                        driver.Close();
                        driver.Quit();
                        if (keep == true)
                        {
                            if (profile_name != username)
                            {
                                string profile_new = path + @"\" + username;
                                Directory.Move(profile, profile_new);
                                row.Update_Status("Thực hiện đổi tên...");
                            }
                        }
                        else
                        {
                            // Xoá Profile
                            if (Directory.Exists(path + "/" + username) == true)
                            {
                                Directory.Delete(path + "/" + username, true);
                            }
                            row.Update_Status("Đã xoá account vừa tạo");
                        }

                        số_ref++;
                        row.Cells["num_ref"].Value = value + số_ref;
                        goto Tạo;
                    }
                }
                else
                {
                    row.Update_Status("Code: " + phone.message);
                    Delay(60);
                    goto Get_Code;
                }
            }
            catch (Exception ex)
            {
                row.Cells["num_ref"].Value = value + số_ref;
                row.Update_Status("Đã hết lượt giới thiệu !!!");
                Console.WriteLine(ex.Message);
                driver.Close();
                driver.Quit();
                // Xoá Profile
                if (Directory.Exists(path + "/" + username) == true)
                {
                    Directory.Delete(path + "/" + username, true);
                }

                return số_ref;
            }
        }

        private void btn_Chỉnh_Sửa_Click(object sender, EventArgs e)
        {
            GridViewRowInfo row = lst_account.SelectedRows.First();
            string name = row.Cells[1].Value.ToString();
            Taki taki = takiAccounts.Where(r => r.name == name).First();
            taki.name = txt_name.Text;
            taki.link = txt_link.Text;
            taki.Label = txt_label.Text;
            row.Cells["label"].Value = taki.Label;
            // Xữ lý lấy invite_link
            FileIO.CreateFile(taki);
            MessageBox.Show("Lưu thành công");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (var taki in takiAccounts)
            {
                taki.num_ref = 0;
                FileIO.CreateFile(taki);
            }

            LoadAccount();
        }


        private void btn_open_broswer_Click(object sender, EventArgs e)
        {
            Thread Chrome = new Thread(() =>
            {
                string[] dirs = Directory.GetDirectories(path);
                int check = 0;
                foreach (var dir in dirs)
                {
                    var driverService = ChromeDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    ChromeOptions option = new ChromeOptions();
                    option.AddArgument("window-size=414,896");
                    option.AddExcludedArgument("enable-automation");
                    option.AddAdditionalOption("useAutomationExtension", false);
                    option.AddArgument("--autoplay-policy=no-user-gesture-required");
                    option.AddArgument("--mute-audio");
                    option.AddArgument("--disable-infobars");
                    option.AddArgument("--disable-default-apps");
                    option.AddArgument("user-data-dir=" + dir);
                    option.AddArgument("--user-agent=Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25");
                    option.AddArgument("headless");
                    ChromeDriver driver = new ChromeDriver(driverService, option);
                    driver.Navigate().GoToUrl("https://taki.app/wallet/");
                    Delay(5);
                    string url = driver.Url;
                    if (url == "https://taki.app/wallet/")
                    {
                        check++;
                        lbl_test.Text = "Kiểm tra thành công : " + check + "/" + dirs.Count();
                        driver.Close();
                        driver.Quit();
                    }
                    else
                    {
                        driver.Close();
                        driver.Quit();
                        Directory.Delete(dir, true);
                        lbl_test.Text = "Đã xoá tài khoản lỗi";
                    }
                }
                lbl_test.Text = "Kiểm tra thành công : " + check + "/" + dirs.Count();
            });
            Chrome.Start();


        }

        private void btnLưu_cấu_hình_Click(object sender, EventArgs e)
        {
            Properties.TakiRef.Default.api_captcha = txt_captcha.Text;
            Properties.TakiRef.Default.api_sms = txt_SMS.Text;
            Properties.TakiRef.Default.api_proxy = txt_proxy.Text;
            Properties.TakiRef.Default.Save();
        }
    }
}
