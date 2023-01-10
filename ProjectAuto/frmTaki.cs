using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ProjectAuto.Common;
using ProjectAuto.Taki;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Diagnostics;
using TMProxyHelper;
using RestSharp;
using ViOTP;
using System.Globalization;
using OpenQA.Selenium.Support.Events;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;
using AnyCaptchaHelper;
using NLog;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Chrome.ChromeDriverExtensions;


namespace ProjectAuto
{
    public partial class frmTaki : Telerik.WinControls.UI.RadForm
    {
        public string postData = "post.data";
        // Cấu hình phần mềm
        public static Setting setting = new Setting();
        // Danh sách tài khoản
        List<TakiAccount> takiAccounts = new List<TakiAccount>();
        // Danh sách chạy
        public List<TakiAccount> takiRunings = new List<TakiAccount>();
        // Danh sách bài post
        List<Tweet> bài_viết = new List<Tweet>();
        // Danh sách tweet
        public static List<Tweet_Profile> tweet_Profiles = new List<Tweet_Profile>();
        // Danh sách proxy
        List<string> proxy = new List<string>();
        // Danh sách Give Taki
        List<string> give = new List<string>();
        // Danh sách driver running
        List<ChromeDriver> drivers = new List<ChromeDriver>();
        List<Item> items = new List<Item>();

        public static List<string> dev = new List<string>();

        public static List<string> dev_give = new List<string>();

        public static List<string> user_agent = new List<string>();

        // Receive_a_Gold_Taki
        public static List<TakiAccount> receive = new List<TakiAccount>();

        public static int coupon = 0;

        private static readonly Object obj = new Object();

        public static object locks = new object();
        // View 
        frmViewTaki view = new frmViewTaki();

        Logger log = LogManager.GetCurrentClassLogger();

        // Reduce CPU;
        bool flgCPU = false;

        // Chrome Screen Point
        public static List<ChromeSetting> points = new List<ChromeSetting>();

        public static List<CProxy> cProxies = new List<CProxy>();

        public frmTaki(int coupon)
        {
            InitializeComponent();
            RadTextBox.CheckForIllegalCrossThreadCalls = false;
            RadLabel.CheckForIllegalCrossThreadCalls = false;
            RadGridView.CheckForIllegalCrossThreadCalls = false;
            RadButton.CheckForIllegalCrossThreadCalls = false;
            frmTaki.coupon = coupon;
        }
        #region Xữ lý Render Grid
        static frmTaki _obj;
        public static frmTaki Instance
        {
            get
            {
                if (_obj == null)
                {
                    _obj = new frmTaki(coupon);
                }
                return _obj;
            }
        }
        public RadGridView GetRunning
        {
            get { return this.lst_running; }
            set { lst_running = value; }
        }
        #endregion
        private void btnTạo_tài_khoản_ClickAsync(object sender, EventArgs e)
        {
            Model model = new Model();
            btnTạo_tài_khoản.Enabled = false;
            Thread Chrome = new Thread(() =>
           {
               string profile_name;
               string username = "";
           Tạo:
               string profile = setting.basic.path_profile + @"\" + txtUserName.Text;
               if (Directory.Exists(profile) == true)
               {
                   DialogResult rs = MessageBox.Show("Đã tồn tại tài khoản: " + txtUserName.Text + "\nBạn có muốn tạo tên tự động không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                   if (DialogResult.OK == rs)
                   {
                   Lấy_username:
                       Random random = new Random();
                       username = Support.getUsername() + random.Next(1, 999);
                       int more = username.Length - 15;
                       if (username.Length > 15)
                       {
                           username = username.Remove(username.Length - more, more);
                       }
                       profile_name = username;
                       if (Directory.Exists(profile) == true)
                       {
                           goto Lấy_username;
                       }
                       txtUserName.Text = username;
                   }
                   else
                   {
                       btnTạo_tài_khoản.Enabled = true;
                       return;
                   }
               }
               else
               {
                   profile_name = txtUserName.Text;
               }
               username = txtUserName.Text;
               lbl_test.Text = "Tạo profile: " + profile_name + "...";
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
               option.AddExtension("Taki/captcha.crx");
               if (string.IsNullOrEmpty(txtProxy.Text) == false)
               {
                   option.AddArgument("--proxy-server=" + txtProxy.Text);
               }
               ChromeDriver driver = new ChromeDriver(driverService, option);
               lbl_test.Text = "Cấu hình captcha";

               IWebElement api_key = driver.FindElement(By.CssSelector(Model.Captcha.Input_API));
               if (api_key.Text == setting.basic.api_captcha)
               {
               }
               else
               {
                   string js = "let config_captcha = {}; config_captcha['autoSubmitForms'] = true; config_captcha['autoSolveRecaptchaV2'] = true; config_captcha['autoSolveInvisibleRecaptchaV2'] = false; config_captcha['autoSolveRecaptchaV3'] = false; Config.set(config_captcha);";
                   string config = (string)driver.ExecuteScript(js);
                   api_key.Clear();
                   api_key.SendKeys(setting.basic.api_captcha);
                   driver.FindElement(By.CssSelector(Model.Captcha.Input)).Click();
                   Delay(3);
                   var alert = driver.SwitchTo().Alert();
                   alert.Accept();
               }
               lbl_test.Text = "Cấu hình captcha hoàn thành ";
               driver.Navigate().GoToUrl(txtLink.Text);
               Delay(1);
               string url = driver.Url;
               if (url == "https://taki.app/")
               {
                   lbl_test.Text = "Đã hết lượt giới thiệu !!!";
                   driver.Close();
                   driver.Quit();
                   btnTạo_tài_khoản.Enabled = true;
                   // Xoá Profile
                   if (Directory.Exists(setting.basic.path_profile + "/" + username) == true)
                   {
                       Directory.Delete(setting.basic.path_profile + "/" + username, true);
                   }
                   return;
               }
               driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(25);
               driver.FindElement(By.CssSelector(Model.Tạo_Tài_Khoản.Accept_Invite)).Click();
               driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
           Get_Code:
               Delay(2);
               driver.FindElement(By.CssSelector(Model.Tạo_Tài_Khoản.Continue_with_Phone)).Click();
               Delay(2);
               driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
               driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Choose_Country)).Click();
               driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
               driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Choose_VN)).Click();
               lbl_test.Text = "Đang lấy số điện thoại...";
               ResponsePhone phone = ViOTPHelper.Get_New_Phone(setting.basic.api_sms);

               if (phone?.data.phone_number != null)
               {
                   CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
                   string balanceVnd = phone.data.balance.ToString("#,###", cul.NumberFormat);
                   txt_balance.Text = balanceVnd + "đ";

                   string sdt = phone?.data.phone_number;
                   driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Input_SDT)).SendKeys(sdt);
                   lbl_test.Text = "Lấy được SDT: " + phone?.data.phone_number;
                   driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Continue_Input_Sdt)).Click();
                   Delay(7);
                   IWebElement send_ok = driver.FindElement(By.CssSelector(Model.Tạo_Tài_Khoản.send_code));
                   string msg = send_ok.GetAttribute("innerHTML");
                   if (msg != "We sent you a code")
                   {
                       try
                       {
                           driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                           IWebElement captcha_invisable = driver.FindElement(By.CssSelector(Model.Captcha.captcha_solver));
                           driver.ExecuteScript("arguments[0].click();", captcha_invisable);
                           Delay(1);
                           string solver = captcha_invisable.GetAttribute("data-state");
                           while (solver != "solved" && solver != "error" && solver != "ready")
                           {
                               lbl_test.Text = "Đang giải captcha ẩn";
                               Delay(2);
                               solver = captcha_invisable.GetAttribute("data-state");
                           }
                       }
                       catch
                       {
                       }
                       try
                       {
                           Delay(7);
                           driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                           IWebElement captcha_msg = driver.FindElement(By.CssSelector(Model.Captcha.msg));
                           string msg_val = "Firebase: The phone verification request contains an invalid application verifier. The reCAPTCHA token response is either invalid or expired. (auth/invalid-app-credential).";
                           string msg_val2 = "Firebase: Recaptcha verification failed - EXPIRED (auth/captcha-check-failed).";
                           string get_msg = captcha_msg.GetAttribute("innerHTML");
                           if (msg_val == get_msg || msg_val2 == get_msg)
                           {
                               lbl_test.Text = "Xữ lý captcha ẩn thất bại...";
                               driver.Close();
                               driver.Quit();
                               // Xoá Profile
                               if (Directory.Exists(setting.basic.path_profile + "/" + username) == true)
                               {
                                   Directory.Delete(setting.basic.path_profile + "/" + username, true);
                               }

                               goto Tạo;
                           }
                       }
                       catch (Exception)
                       {
                           lbl_test.Text = "Đã giải captcha ẩn";
                       }
                   }

                   int check_sms = 0;
               Get_SMS:
                   Delay(5);
                   ResponseCode code_SMS = ViOTPHelper.Get_Code_Phone(setting.basic.api_sms, phone.data.request_id);
                   if (code_SMS?.data.Code == null)
                   {
                       check_sms++;
                       if (check_sms == 25)
                       {
                           lbl_test.Text = "Code: Không lấy được code";
                           driver.FindElement(By.XPath(model.Back_Get_Code)).Click();
                           goto Get_Code;
                       }
                       goto Get_SMS;
                   }
                   else
                   {
                       lbl_test.Text = "Lấy code thành công...";
                       driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Input_Code_SMS)).SendKeys(code_SMS?.data.Code);
                       driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Continue_Code_SMS)).Click();
                       driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                   Nhập_Tên:
                       // Nhập username
                       driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Input_Username)).SendKeys(txtUserName.Text);
                       driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                       driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Continue_Username)).Click();
                       driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                       try
                       {
                           IWebElement validate = driver.FindElement(By.CssSelector(Model.Tạo_Tài_Khoản.val_account));
                           if (validate == null)
                           {
                               lbl_test.Text = "Tên không hợp lệ...";
                               Random random = new Random();
                               username = Support.getUsername() + random.Next(1, 9999);
                               int more = username.Length - 15;
                               if (username.Length > 15)
                               {
                                   username = username.Remove(username.Length - more, more);
                               }
                               goto Nhập_Tên;
                           }
                       }
                       catch
                       {

                       }
                       Stopwatch stopwatch = new Stopwatch();
                       stopwatch.Start();
                       try
                       {
                           driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
                           IWebElement captcha_invisable = driver.FindElement(By.CssSelector(Model.Captcha.captcha_solver));

                           string solver = captcha_invisable.GetAttribute("data-state");
                           while (solver != "solved")
                           {
                               lbl_test.Text = "Đang giải captcha";
                               Delay(1);
                               solver = captcha_invisable.GetAttribute("data-state");
                           }
                           lbl_test.Text = "Đã giải captcha";
                       }
                       catch
                       {
                           lbl_test.Text = "Đã giải captcha";
                       }
                       stopwatch.Stop();
                       TimeSpan ts = stopwatch.Elapsed;
                       lbl_test.Text = "Giải captcha thành công: " + ts.Seconds + " s";
                   Create_Wallet:
                       driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
                       lbl_test.Text = "Tạo wallet...";
                       try
                       {
                           driver.FindElement(By.XPath(model.Create_My_Coin)).Click();
                       }
                       catch
                       {
                           lbl_test.Text = "Đang đợi tạo wallet...";
                           driver.Navigate().Refresh();
                           goto Create_Wallet;
                       }
                       Delay(30);
                       driver.FindElement(By.XPath(model.Goto_Wallet)).Click();
                       Delay(10);
                       lbl_test.Text = "Tạo wallet thành công...";
                       TakiAccount taki = new TakiAccount();
                       taki.name = txtUserName.Text;
                       taki.isWithdraw = check_chuyển_tiền.Checked;
                       taki.isRef = check_ref.Checked;
                       taki.topic = txt_topic.Text;
                       taki.proxy = txtProxy.Text;
                       lbl_test.Text = "Tạo link invite...";
                       taki.invite_link = Support.Lấy_Invite(driver);
                       lbl_test.Text = "Tạo số TAKI...";
                       taki.taki = 3;
                       taki.SaveDate = DateTime.Now;
                       FileIO.CreateFile(taki);
                       lbl_test.Text = "Tạo file hoàn thành...";
                       Delay(1);
                       lbl_test.Text = "Hoàn thành";
                       btnTạo_tài_khoản.Enabled = true;
                   }
               }
               else
               {
                   lbl_test.Text = "Code: " + phone.message;
                   Delay(50);
                   goto Get_Code;
               }
           });
            Chrome.SetApartmentState(ApartmentState.STA);
            Chrome.Start();



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

        private void frmTaki_LoadAsync(object sender, EventArgs e)
        {
            LoadSetting();
            LoadAccount(ckb_show_live.Checked);

            // Load danh sách Recive
            receive = takiAccounts;


            lbl_withdraw_amoun.Text = "WITHDRAW: " + takiAccounts.Where(r => r.isWithdraw).Count();
            lbl_ref.Text = "INVITE: " + takiAccounts.Where(r => r.isRef).Count();
            lbl_amount.Text = "EARNING: " + takiAccounts.Where(r => r.isRef == false && r.isWithdraw == false).Count();

            txt_hold.Text = takiAccounts.Sum(r => r.taki).ToString();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string earning = setting.earn.ToString("#,###", cul.NumberFormat);
            lbl_earning.Text = earning + "TAKI";

            if (File.Exists(postData) == false)
            {
                File.Create(postData);
            }
            else
            {
                string file_content = File.ReadAllText(postData);
                bài_viết = JsonConvert.DeserializeObject<List<Tweet>>(file_content);
            }

            string token = setting.basic?.api_sms;
            try
            {
                if (string.IsNullOrEmpty(token) == false)
                {
                    ResponseBalance response = ViOTPHelper.Balance(token);
                    long balance = response.data.balance;
                    string balanceVnd = balance.ToString("#,###", cul.NumberFormat);
                    txt_balance.Text = balanceVnd + "đ";
                }
            }
            catch
            {
                MessageBox.Show("API VIOTP không chính xác !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Get List
            var tempFilePath = Path.GetTempFileName();
            using (var client = new WebClient())
            {
                client.DownloadFile("https://raw.githubusercontent.com/huynhchinh307/Taki/main/dev.text", tempFilePath);
            }
            string[] ss = File.ReadAllLines(tempFilePath);
            dev.Clear();
            foreach (var s in ss)
            {
                dev.Add("https://taki.app/u/" + s + "/");
            }
            File.Delete(tempFilePath);


            // Get List Dev Give Taki
            var tempDevTaki = Path.GetTempFileName();
            using (var client = new WebClient())
            {
                client.DownloadFile("https://raw.githubusercontent.com/huynhchinh307/Taki/main/dev_taki.text", tempDevTaki);
            }
            string[] dev_Taki = File.ReadAllLines(tempDevTaki);
            dev_give.Clear();
            foreach (var s in dev_Taki)
            {
                dev_give.Add("https://taki.app/u/" + s + "/");
            }
            File.Delete(tempFilePath);

            var tempAgent = Path.GetTempFileName();
            using (var client = new WebClient())
            {
                client.DownloadFile("https://raw.githubusercontent.com/huynhchinh307/Taki/main/UserAgent.txt", tempAgent);
            }
            string[] user_Agent = File.ReadAllLines(tempAgent);
            user_agent.Clear();
            foreach (var s in user_Agent)
            {
                user_agent.Add(s);
            }
            File.Delete(tempAgent);

            if (string.IsNullOrEmpty(txt_API_captcha.Text) == false)
            {
                var getBalanceRequest = new AnyCaptcha().GetBalance(txt_API_captcha.Text);

                if (getBalanceRequest.IsSuccess)
                {
                    txt_balance_captcha.Text = getBalanceRequest.Balance.ToString("#.##") + "$";
                }
                else
                {
                    MessageBox.Show("API Captcha đang không đúng !!!");
                }
            }

            bool count_complete = frmTaki.receive.Where(r => r.receive == false && r.complete == true || r.complete == false).Any();
            if (count_complete == false)
            {
                btn_complete.BackColor = Color.LimeGreen;
                panel_bottom.BackColor = Color.LimeGreen;
            }
            else
            {
                btn_complete.BackColor = Color.Red;
                panel_bottom.BackColor = Color.Red;
            }
        }

        private void btn_Đường_dẫn_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    if (setting.basic == null)
                    {
                        setting.basic = new Basic();
                    }
                    setting.basic.path_profile = @fbd.SelectedPath;
                    string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
                    FileIO.Create_File_From_Json(json, "Taki\\config.json");
                    label_path.Text = @fbd.SelectedPath;
                }
            }
        }

        private void LoadAccount(bool only_live = true, int sort = 1)
        {
            if (lst_account.InvokeRequired)
            {
                lst_account.Invoke(new MethodInvoker(() => { this.lst_account.Rows.Clear(); }));
            }
            else
            {
                this.lst_account.Rows.Clear();
            }
            takiAccounts = new List<TakiAccount>();
            // Xữ lý load thread account
            string[] files = Directory.GetFiles("Taki\\Data");
            for (int i = 0; i < files.Count(); i++)
            {
                TakiAccount tài_khoản = JsonConvert.DeserializeObject<TakiAccount>(FileIO.ReadFile(files[i]));
                if (tài_khoản == null)
                {
                    MessageBox.Show("Không thể đọc dữ liệu: " + files[i]);
                }
                else
                {
                    if (only_live == true)
                    {
                        if (tài_khoản.isLive)
                        {
                            takiAccounts.Add(tài_khoản);
                        }
                    }
                    else
                    {
                        if (tài_khoản.isLive == false)
                        {
                            takiAccounts.Add(tài_khoản);
                        }
                    }
                }

            }

            foreach (var s in takiAccounts)
            {
                if (s.isConnect == true)
                {
                    s.isConnect = false;
                    FileIO.CreateFile(s);
                }
            }

            if (sort == 1)
            {
                takiAccounts = takiAccounts.OrderByDescending(r => r.point).ToList();
            }
            else if (sort == 2)
            {
                takiAccounts = takiAccounts.OrderByDescending(r => r.taki).ToList();
            }
            else if (sort == 3)
            {
                takiAccounts = takiAccounts.OrderByDescending(r => r.point).ToList();
            }
            int stt = 1;
            foreach (var tài_khoản in takiAccounts)
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.lst_account.MasterView);
                rowInfo.Cells["stt"].Value = stt;
                rowInfo.Cells["username"].Value = tài_khoản.name;
                if (tài_khoản.isWithdraw == true)
                {
                    rowInfo.Cells["rank"].Value = "WITHDRAW";
                }
                else if (tài_khoản.isRef == true)
                {
                    rowInfo.Cells["rank"].Value = "INVITE";
                }
                else
                {
                    rowInfo.Cells["rank"].Value = "EARNING";
                }

                rowInfo.Cells["taki"].Value = tài_khoản.taki;
                rowInfo.Cells["point"].Value = "⚡ " + tài_khoản.point.ToString("0.##");
                rowInfo.Cells["complete"].Value = tài_khoản.complete;
                rowInfo.Cells["apr"].Value = tài_khoản.apr?.ToString("#") + "%";
                if (tài_khoản.isLive == false)
                {
                    rowInfo.Cells["check"].Value = "Lock";
                }
                else
                {
                    rowInfo.Cells["check"].Value = "ALive";
                }
                rowInfo.Cells["rank_name"].Value = tài_khoản.cosmeticTierName;
                rowInfo.Cells["earn"].Value = tài_khoản.paidLikesEarnings;
                rowInfo.Cells["receive"].Value = tài_khoản.receive;
                rowInfo.Cells["upload"].Value = tài_khoản.upload;
                rowInfo.Cells["error"].Value = tài_khoản.error;

                if (tài_khoản.isLive == false)
                {
                    foreach (GridViewCellInfo cell in rowInfo.Cells)
                    {
                        cell.Style.CustomizeFill = true;
                        cell.Style.DrawFill = true;
                        cell.Style.BackColor = Color.OrangeRed;
                    }
                }
                else if (tài_khoản.cosmeticTierName == "Silver")
                {
                    foreach (GridViewCellInfo cell in rowInfo.Cells)
                    {
                        cell.Style.CustomizeFill = true;
                        cell.Style.DrawFill = true;
                        if (tài_khoản.withdraw == true && tài_khoản.withdraw_day == false)
                        {
                            cell.Style.BackColor = ColorTranslator.FromHtml("#28a745");
                        }
                        else
                        {
                            cell.Style.BackColor = Color.Silver;
                        }
                    }
                }
                else if (tài_khoản.cosmeticTierName == "Super Silver")
                {
                    foreach (GridViewCellInfo cell in rowInfo.Cells)
                    {
                        cell.Style.CustomizeFill = true;
                        cell.Style.DrawFill = true;
                        if (tài_khoản.withdraw == true && tài_khoản.withdraw_day == false)
                        {
                            cell.Style.BackColor = ColorTranslator.FromHtml("#28a745");
                        }
                        else
                        {
                            cell.Style.BackColor = ColorTranslator.FromHtml("#ffffb3");
                        }
                    }
                }
                else if (tài_khoản.cosmeticTierName == "Gold")
                {
                    foreach (GridViewCellInfo cell in rowInfo.Cells)
                    {
                        cell.Style.CustomizeFill = true;
                        cell.Style.DrawFill = true;
                        if (tài_khoản.withdraw == true && tài_khoản.withdraw_day == false)
                        {
                            cell.Style.BackColor = ColorTranslator.FromHtml("#28a745");
                        }
                        else
                        {
                            cell.Style.BackColor = ColorTranslator.FromHtml("#ffff00");
                        }
                    }
                }

                rowInfo.Cells["stake"].Value = tài_khoản.stake;
                rowInfo.Cells["price"].Value = tài_khoản.price?.ToString("#.##");

                if (tài_khoản.withdraw == true)
                {
                    if (tài_khoản.withdraw_day == false)
                    {
                        foreach (GridViewCellInfo cell in rowInfo.Cells)
                        {
                            cell.Style.CustomizeFill = true;
                            cell.Style.DrawFill = true;
                            cell.Style.BackColor = ColorTranslator.FromHtml("#28a745");
                        }

                    }
                    //else
                    //{
                    //    rowInfo.Cells["username"].Style.CustomizeFill = true;
                    //    rowInfo.Cells["username"].Style.DrawFill = true;
                    //    rowInfo.Cells["username"].Style.BackColor = ColorTranslator.FromHtml("#28a745");
                    //}
                }

                if (string.IsNullOrEmpty(tài_khoản.invite_link) == true)
                {
                    foreach (GridViewCellInfo cell in rowInfo.Cells)
                    {
                        cell.Style.CustomizeFill = true;
                        cell.Style.DrawFill = true;
                        cell.Style.BackColor = Color.Red;
                    }
                }
                stt++;

                if (tài_khoản.isBrowser == false)
                {
                    rowInfo.Cells["stt"].Style.CustomizeFill = true;
                    rowInfo.Cells["stt"].Style.DrawFill = true;
                    rowInfo.Cells["stt"].Style.BackColor = Color.PaleVioletRed;
                }

                if (lst_account.InvokeRequired)
                {
                    lst_account.Invoke(new MethodInvoker(() => { lst_account.Rows.Add(rowInfo); }));
                }
                else
                {
                    lst_account.Rows.Add(rowInfo);
                }
            }

            lbl_withdraw_amoun.Text = "WITHDRAW: " + takiAccounts.Where(r => r.isWithdraw).Count();
            lbl_ref.Text = "INVITE: " + takiAccounts.Where(r => r.isRef).Count();
            lbl_amount.Text = "EARNING: " + takiAccounts.Where(r => r.isRef == false && r.isWithdraw == false).Count();

            txt_hold.Text = takiAccounts.Sum(r => r.taki).ToString();
        }

        private void btn_get_random_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            string name = Support.getUsername() + random.Next(1, 999);
            if (name.Length > 15)
            {
                name = name.Remove(0, name.Length - 15);
            }
            txtUserName.Text = name;
        }

        private void lst_account_invite_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            //if (lst_account_invite.SelectedIndex > -1 && lst_account_invite.SelectedItem != null)
            //{
            //    if (lst_account_invite.SelectedItem.Text == "ProjectAuto.Taki.Item")
            //    {
            //        txtLink.Text = lst_account_invite.SelectedItem.Value != null ? ((Item)lst_account_invite.SelectedValue).Id : "";
            //        txt_topic.Text = takiAccounts.Where(r => r.invite_link == txtLink.Text).First().topic;
            //    }
            //    else
            //    {
            //        txtLink.Text = lst_account_invite.SelectedItem.Value != null ? lst_account_invite.SelectedItem.Value.ToString() : "";
            //        txt_topic.Text = takiAccounts.Where(r => r.invite_link == txtLink.Text).First().topic;
            //    }
            //}
        }

        ChromeDriver test;
        private void btnRun_Profile_ClickAsync(object sender, EventArgs e)
        {
            frmViewTaki view = new frmViewTaki();
            view.Show();
            if (test != null)
            {
                test.Quit();
            }
            lbl_test.Text = "Đang khởi động...";
            // Lấy thông tin profile đang chọn
            GridViewRowInfo row = lst_account.SelectedRows.First();
            string name = row.Cells[1].Value.ToString();
            TakiAccount taki = takiAccounts.Where(r => r.name == name).First();
            Thread Chrome = new Thread(() =>
            {
                string dir = "Taki\\VMS\\VmsRuning\\Default\\IndexedDB\\https_taki.app_0.indexeddb.leveldb";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                else
                {
                    Directory.Delete(dir, true);
                    Directory.CreateDirectory(dir);
                }
                string source = "Taki\\Profile\\" + name + "\\https_taki.app_0.indexeddb.leveldb";

                new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(source, dir);

                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                ChromeOptions option = new ChromeOptions();
                option.AddArgument("window-size=414,896");
                option.AddExcludedArgument("enable-automation");
                option.AddAdditionalOption("useAutomationExtension", false);
                option.AddArgument("--autoplay-policy=no-user-gesture-required");
                option.AddArgument("--mute-audio");
                option.AddArgument("no-sandbox");
                option.AddArgument("--disable-infobars");
                option.AddArgument("--disable-default-apps");
                option.AddExtension("Taki/captcha.crx");
                option.AddArgument("--window-position=-2000,0");
                option.AddArgument("--disable-gpu");
                option.AddArgument("--FontRenderHinting[none]");
                option.AddArgument("--disable-blink-features=AutomationControlled");
                option.AddArgument("--app=https://taki.app/u/" + taki.name.ToLower() + "/");
                if (string.IsNullOrEmpty(taki.userAgent) == true)
                    taki.userAgent = Support.GetRandomUserAgent();
                FileIO.CreateFile(taki);
                option.AddArgument("--user-agent=" + taki.userAgent);
                string path = Directory.GetCurrentDirectory();
                option.AddArgument("user-data-dir=" + path + "\\Taki\\VMS\\VmsRuning");
                //if (string.IsNullOrEmpty(taki.proxy) == false)
                //{
                //    option.AddArgument("--proxy-server=" + taki.proxy);
                //    option.AddArgument("--proxy-server=" + taki.proxy);
                //}
                option.AddArgument(setting.basic.agent);
                test = new ChromeDriver(driverService, option);
                int ProcessId = driverService.ProcessId;
                Support.Setting_Captcha(test);
                view.Add_View(taki.name, ProcessId);
                ChromeView chrome_view = new ChromeView();
                chrome_view.driver = test;
                chrome_view.name = taki.name;
                view.views.Add(chrome_view);
                test.Manage().Window.Size = new Size(414, 896);
                test.Manage().Window.Position = new Point(0, 0);
                Delay(2);

                string body = test.FindElement(By.CssSelector("body")).GetAttribute("innerHTML");
                if (body == "Internal error")
                {
                Fix:
                    lbl_test.Text = "Fix lỗi Internal error";
                    test.Navigate().GoToUrl("https://taki.app/");
                    List<IWebElement> postE = test.FindElements(By.CssSelector(Model.Create_Post.Button_Create)).ToList();
                    if (postE.Count == 0)
                    {
                        test.Navigate().Refresh();
                        Delay(5);
                        goto Fix;
                    }

                Home_Screen:
                    try
                    {
                        test.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                        IWebElement captcha_invisable = test.FindElement(By.CssSelector(Model.Captcha.captcha_solver));

                        string solver = captcha_invisable.GetAttribute("data-state");
                        while (solver != "solved" && solver != "error")
                        {
                            lbl_test.Text = "Đang giải captcha";
                            Delay(5);
                            solver = captcha_invisable.GetAttribute("data-state");
                        }
                        lbl_test.Text = "Đã giải captcha";
                        test.Navigate().Refresh();
                        Delay(5);
                        goto Home_Screen;
                    }
                    catch
                    {
                        lbl_test.Text = "Đã xử lý captcha...";
                    }

                Fix_Max:
                    List<IWebElement> menu = test.FindElements(By.CssSelector(Model.Create_Post.Button_Create)).ToList();
                    if (menu.Count == 0)
                    {
                        test.Navigate().Refresh();
                        Delay(5);
                        goto Fix_Max;
                    }
                    else if (menu.Count == 5)
                    {
                        menu[4].Click();
                    }
                }
                else
                {
                Home_Screen:
                    try
                    {
                        test.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                        IWebElement captcha_invisable = test.FindElement(By.CssSelector(Model.Captcha.captcha_solver));

                        string solver = captcha_invisable.GetAttribute("data-state");
                        while (solver != "solved" && solver != "error")
                        {
                            lbl_test.Text = "Đang giải captcha";
                            Delay(5);
                            solver = captcha_invisable.GetAttribute("data-state");
                        }
                        lbl_test.Text = "Đã giải captcha";
                        test.Navigate().Refresh();
                        Delay(5);
                        goto Home_Screen;
                    }
                    catch
                    {
                        lbl_test.Text = "Đã xử lý captcha...";
                    }
                }


                #region Xữ lý lấy point
                TakiData profile = TakiHelper.Get_Profile(test, taki.name.ToLower(), row, true);
                if (profile.point == null)
                {
                    row.Update_Color(Color.Blue);
                }
                else
                {
                    taki.point = profile.point.Value;
                }
                #endregion
                if (taki.cosmeticTierName == "Silver")
                {
                    foreach (GridViewCellInfo cell in row.Cells)
                    {
                        cell.Style.CustomizeFill = true;
                        cell.Style.DrawFill = true;
                        if (taki.withdraw == true && taki.withdraw_day == false)
                        {
                            cell.Style.BackColor = ColorTranslator.FromHtml("#28a745");
                        }
                        else
                        {
                            cell.Style.BackColor = Color.Silver;
                        }
                    }
                }
                else if (taki.cosmeticTierName == "Super Silver")
                {
                    foreach (GridViewCellInfo cell in row.Cells)
                    {
                        cell.Style.CustomizeFill = true;
                        cell.Style.DrawFill = true;
                        if (taki.withdraw == true && taki.withdraw_day == false)
                        {
                            cell.Style.BackColor = ColorTranslator.FromHtml("#28a745");
                        }
                        else
                        {
                            cell.Style.BackColor = ColorTranslator.FromHtml("#ffffb3");
                        }
                    }
                }
                else if (taki.cosmeticTierName == "Gold")
                {
                    foreach (GridViewCellInfo cell in row.Cells)
                    {
                        cell.Style.CustomizeFill = true;
                        cell.Style.DrawFill = true;
                        if (taki.withdraw == true && taki.withdraw_day == false)
                        {
                            cell.Style.BackColor = ColorTranslator.FromHtml("#28a745");
                        }
                        else
                        {
                            cell.Style.BackColor = ColorTranslator.FromHtml("#ffff00");
                        }
                    }
                }

                row.Update("point", "⚡ " + taki.point);
                row.Update("price", profile.coin.value.ToString("#.##"));
                row.Update("apr", profile.coin.APR.ToString("#") + "%");

                taki.price = profile.coin.value;
                taki.apr = profile.coin.APR;
                FileIO.CreateFile(taki);

                lbl_test.Text = "Khởi động hoàn tất..";
            });
            Chrome.Name = "Thread Test: " + name;
            Chrome.SetApartmentState(ApartmentState.STA);
            Chrome.Start();

        }

        private void EventFiringWebDriver_Error(object sender, WebDriverExceptionEventArgs e)
        {
            MessageBox.Show(e.ThrownException.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnTest_function_ClickAsync(object sender, EventArgs e)
        {
            GridViewRowInfo row = lst_account.SelectedRows.First();
            string name = row.Cells[1].Value.ToString();
            TakiAccount taki = takiAccounts.Where(r => r.name == name).First();
            Thread Chrome = new Thread(() =>
            {
                string dir = "Taki\\VMS\\VmsRuning\\Default\\IndexedDB\\https_taki.app_0.indexeddb.leveldb";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                else
                {
                    Directory.Delete(dir, true);
                    Directory.CreateDirectory(dir);
                }
                string source = "Taki\\Profile\\" + name + "\\https_taki.app_0.indexeddb.leveldb";

                new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(source, dir);


                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                ChromeOptions option = new ChromeOptions();
                option.AddArgument("window-size=414,896");
                option.AddExcludedArgument("enable-automation");
                option.AddAdditionalOption("useAutomationExtension", false);
                option.AddArgument("--autoplay-policy=no-user-gesture-required");
                option.AddArgument("--mute-audio");
                option.AddArgument("no-sandbox");
                option.AddArgument("--disable-infobars");
                option.AddArgument("--disable-default-apps");
                option.AddArgument("--disable-gpu");
                option.AddArgument("--FontRenderHinting[none]");
                option.AddArgument("--disable-blink-features=AutomationControlled");
                //option.AddExtension("Taki/captcha.crx");
                //option.AddExtension("Taki/callback.crx");
                option.AddArgument("--app=https://taki.app/");
                string path = Directory.GetCurrentDirectory();
                option.AddArgument("user-data-dir=" + path + "\\Taki\\VMS\\VmsRuning");
                if (string.IsNullOrEmpty(taki.userAgent) == true)
                    taki.userAgent = Support.GetRandomUserAgent();
                FileIO.CreateFile(taki);
                option.AddArgument("--user-agent=" + taki.userAgent);
                if (string.IsNullOrEmpty(taki.proxy) == false)
                {
                    string[] proxy = taki.proxy.Split(':');
                    //option.AddHttpProxy(proxy[0], int.Parse(proxy[1]), proxy[2], proxy[3]);
                    //option.AddArgument("--proxy-server=http://" + taki.proxy);
                }
                test = new ChromeDriver(driverService, option);
            });
            Chrome.Name = "Thread Test: " + name;
            Chrome.SetApartmentState(ApartmentState.STA);
            Chrome.Start();

            //Profile profile = TakiHelper.Get_Profile(test, "6tgff", null, true);
            //Console.Write("");
        }

        private void lbl_test_Click(object sender, EventArgs e)
        {

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

        private void btnLưu_cấu_hình_Click(object sender, EventArgs e)
        {
            setting.basic.api_proxy = txt_API_Proxy.Text;
            setting.basic.api_captcha = txt_API_captcha.Text;
            setting.basic.api_sms = txt_API_SMS.Text;
            setting.basic.api_post = txt_RapidAPI.Text;

            string json_Config = JsonConvert.SerializeObject(setting, Formatting.Indented);
            FileIO.Create_File_From_Json(json_Config, "Taki\\config.json");

            proxy = new List<string>();
            foreach (var s in txt_API_Proxy.Lines)
            {
                if (string.IsNullOrEmpty(s) == false)
                {
                    string json = TMAPIHelper.Stats(s);
                    TmStats stats = JsonConvert.DeserializeObject<TmStats>(json);
                    if (stats.message == "")
                    {
                        proxy.Add(s);
                    }
                }
            }
            MessageBox.Show("Lưu thành công");
        }

        private void btn_Xoá_Click(object sender, EventArgs e)
        {
            GridViewRowInfo row = lst_account.SelectedRows.First();
            string name = row.Cells[1].Value.ToString();
            TakiAccount taki = takiAccounts.Where(r => r.name == name).First();
            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn xoá không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (DialogResult.OK == rs)
            {
                // Xoá danh sách đang chạy
                takiAccounts.Remove(taki);
                // Xoá danh sách hiển thị
                lst_account.Rows.Remove(row);

                // Xoá Profile
                if (Directory.Exists(setting.basic.path_profile + "/" + taki.name) == true)
                {
                    Directory.Delete(setting.basic.path_profile + "/" + taki.name, true);
                }
                // Xoá file data
                File.Delete("Taki/Data/" + taki.name + ".data");

                lbl_withdraw_amoun.Text = "WITHDRAW: " + takiAccounts.Where(r => r.isWithdraw).Count();
                lbl_ref.Text = "INVITE: " + takiAccounts.Where(r => r.isRef).Count();
                lbl_amount.Text = "EARNING: " + takiAccounts.Where(r => r.isRef == false && r.isWithdraw == false).Count();

                txt_hold.Text = takiAccounts.Sum(r => r.taki).ToString();
            }
        }

        private void lst_account_CellClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (lst_account.SelectedRows.Count > 0)
                {
                    GridViewRowInfo row = lst_account.SelectedRows.First();
                    string name = row.Cells["username"].Value.ToString();
                    TakiAccount taki = takiAccounts.Where(r => r.name == name).First();
                    txtUserName.Text = taki.name;
                    txtLink.Text = taki.invite_link;
                    txtProxy.Text = taki.proxy;
                    check_ref.Checked = taki.isRef;
                    check_chuyển_tiền.Checked = taki.isWithdraw;
                    txt_topic.Text = taki.topic;
                    check_live.Checked = taki.isLive;
                    txt_agent.Text = taki.userAgent;
                    ckb_isBrowser.Checked = taki.isBrowser;
                }
                else
                {
                    txtUserName.Text = "";
                    txtLink.Text = "";
                    txtProxy.Text = "";
                    check_ref.Checked = false;
                    check_chuyển_tiền.Checked = false;
                    txt_topic.Text = "";
                    check_live.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã có lỗi xảy ra\nXin vui lòng chụp lại thông báo này để chúng tôi có thể giải quyết nhanh chóng !!\nMã lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_Chỉnh_Sửa_Click(object sender, EventArgs e)
        {
            GridViewRowInfo row = lst_account.SelectedRows.First();
            string name = row.Cells[1].Value.ToString();
            if (txtUserName.Text != name)
            {
                DialogResult rs = MessageBox.Show("Bạn đang thay đổi username !!!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (DialogResult.OK == rs)
                {
                    if (File.Exists("Taki\\Data\\" + name + ".data"))
                    {
                        File.Delete("Taki\\Data\\" + name + ".data");
                    }

                    TakiAccount taki = takiAccounts.Where(r => r.name == name).First();
                    taki.name = txtUserName.Text;
                    taki.isRef = check_ref.Checked;
                    taki.isWithdraw = check_chuyển_tiền.Checked;
                    taki.invite_link = txtLink.Text;
                    taki.proxy = txtProxy.Text;
                    taki.topic = txt_topic.Text;
                    taki.isLive = check_live.Checked;
                    taki.userAgent = txt_agent.Text;
                    taki.isBrowser = ckb_isBrowser.Checked;
                    if (taki.isWithdraw == true)
                    {
                        row.Cells["rank"].Value = "WITHDRAW";
                    }
                    else if (taki.isRef == true)
                    {
                        row.Cells["rank"].Value = "INVITE";
                    }
                    else
                    {
                        row.Cells["rank"].Value = "EARNING";
                    }

                    FileIO.CreateFile(taki);
                    Directory.Move(setting.basic.path_profile + @"\" + name, setting.basic.path_profile + @"\" + taki.name);
                    lbl_withdraw_amoun.Text = "WITHDRAW: " + takiAccounts.Where(r => r.isWithdraw).Count();
                    lbl_ref.Text = "INVITE: " + takiAccounts.Where(r => r.isRef).Count();
                    lbl_amount.Text = "EARNING: " + takiAccounts.Where(r => r.isRef == false && r.isWithdraw == false).Count();

                    txt_hold.Text = takiAccounts.Sum(r => r.taki).ToString();

                    MessageBox.Show("Lưu thành công");
                }
            }
            else
            {
                TakiAccount taki = takiAccounts.Where(r => r.name == name).First();
                taki.isRef = check_ref.Checked;
                taki.isWithdraw = check_chuyển_tiền.Checked;
                taki.invite_link = txtLink.Text;
                taki.proxy = txtProxy.Text;
                taki.topic = txt_topic.Text;
                taki.isLive = check_live.Checked;
                taki.userAgent = txt_agent.Text;
                taki.isBrowser = ckb_isBrowser.Checked;
                if (taki.isWithdraw == true)
                {
                    row.Cells["rank"].Value = "WITHDRAW";
                }
                else if (taki.isRef == true)
                {
                    row.Cells["rank"].Value = "INVITE";
                }
                else
                {
                    row.Cells["rank"].Value = "EARNING";
                }

                FileIO.CreateFile(taki);

                lbl_withdraw_amoun.Text = "WITHDRAW: " + takiAccounts.Where(r => r.isWithdraw).Count();
                lbl_ref.Text = "INVITE: " + takiAccounts.Where(r => r.isRef).Count();
                lbl_amount.Text = "EARNING: " + takiAccounts.Where(r => r.isRef == false && r.isWithdraw == false).Count();

                txt_hold.Text = takiAccounts.Sum(r => r.taki).ToString();

                MessageBox.Show("Lưu thành công");
            }

        }

        private void group_Tạo_mới_Click(object sender, EventArgs e)
        {

        }

        private void btnTest_Click(object sender, EventArgs e)
        {

        }

        private void btn_Chạy_Click(object sender, EventArgs e)
        {
            // Calulator Point Screen
            int width = Screen.PrimaryScreen.Bounds.Width;
            int Height = Screen.PrimaryScreen.Bounds.Height;
            Control control = (Control)sender;
            if (check_thực_hiện_mời.Checked)
            {
                this.Size = new Size(495, 603);
                ChangeSize();
                this.Location = new Point(width - 495, 0);
            }

            const int fix_W = 414;
            //const int fix_H = 896;
            int split = (width - 495) / fix_W;

            int X = 0;
            int Y = 0;
            for (int i = 0; i < split; i++)
            {
                ChromeSetting s = new ChromeSetting();
                s.X = X;
                s.Y = Y;
                X = X + fix_W;
                points.Add(s);
            }


            flgCPU = true;

            takiRunings.Clear();
            if (lst_loại_taki.SelectedIndex == -1)
            {
                lbl_test.Text = "Vui lòng chọn loại taki để có thể thực hiện auto";
                return;
            }
            if (lst_loại_taki.SelectedItem.Text == "ALL")
            {
                takiRunings = takiAccounts;
                takiRunings = takiRunings.OrderBy(a => Guid.NewGuid()).ToList();
            }
            else if (lst_loại_taki.SelectedItem.Text == "WITHDRAW")
            {
                takiRunings = takiAccounts.Where(r => r.isWithdraw == true).ToList();
                takiRunings = takiRunings.OrderBy(a => Guid.NewGuid()).ToList();
            }
            else if (lst_loại_taki.SelectedItem.Text == "INVITE")
            {
                takiRunings = takiAccounts.Where(r => r.isRef == true).ToList();
                takiRunings = takiRunings.OrderBy(a => Guid.NewGuid()).ToList();
            }
            else if (lst_loại_taki.SelectedItem.Text == "EARNING")
            {
                takiRunings = takiAccounts.Where(r => r.isRef == false && r.isWithdraw == false).ToList();
                takiRunings = takiRunings.OrderBy(a => Guid.NewGuid()).ToList();
            }
            else if (lst_loại_taki.SelectedItem.Text == "SELECT")
            {
                foreach (var row in lst_account.SelectedRows)
                {
                    string name = row.Cells["username"].Value.ToString();
                    TakiAccount taki = takiAccounts.Where(r => r.name == name).FirstOrDefault();
                    if (taki != null)
                    {
                        takiRunings.Add(taki);
                    }
                }

                takiRunings = takiRunings.OrderBy(a => Guid.NewGuid()).ToList();
            }
            else if (lst_loại_taki.SelectedItem.Text == "TOP 200 HOLD")
            {
                takiRunings = takiAccounts.Where(r => r.isWithdraw == false).OrderByDescending(r => r.taki).Take(200).ToList();
                takiRunings = takiRunings.OrderBy(a => Guid.NewGuid()).ToList();
            }
            else if (lst_loại_taki.SelectedItem.Text == "TOP 100 HOLD")
            {
                takiRunings = takiAccounts.Where(r => r.isWithdraw == false).OrderByDescending(r => r.taki).Take(100).ToList();
                takiRunings = takiRunings.OrderBy(a => Guid.NewGuid()).ToList();
            }
            else if (lst_loại_taki.SelectedItem.Text == "TOP 50 HOLD")
            {
                takiRunings = takiAccounts.Where(r => r.isWithdraw == false).OrderByDescending(r => r.taki).Take(50).ToList();
                takiRunings = takiRunings.OrderBy(a => Guid.NewGuid()).ToList();
            }
            if (check_đăng_post.Checked)
            {
                takiRunings = takiRunings.Where(r => r.upload == false).ToList();
            }

            if (check_làm_nhiệm_vụ.Checked)
            {
                takiRunings = takiRunings.Where(r => r.complete == false).ToList();
            }

            if (check_mua_coin.Checked)
            {
                int buy = int.Parse(txt_buy_coin.Text);
                takiRunings = takiRunings.Where(r => r.price < buy).ToList();
            }

            if (check_Give_Taki.Checked)
            {
                takiRunings = takiRunings.Where(r => r.taki > (int.Parse(txt_số_give_taki.Text) + 2)).ToList();
            }

            if (ckb_auto_withdraw.Checked)
            {
                takiRunings = takiRunings.Where(r => r.withdraw == true && r.withdraw_day == false).ToList();
            }
            MessageBox.Show("Đang chạy: " + takiRunings.Count, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            view = new frmViewTaki();
            view.Show();
            cProxies.Clear();
            foreach(var s in txt_API_Proxy.Text.Split('\n'))
            {

                string[] ps = s.Split(':');
                if (ps.Count() >= 2)
                {
                    CProxy cProxy = new CProxy();
                    if (ps.Count() == 4)
                    {
                        cProxy.ip = ps[0];
                        cProxy.port = int.Parse(ps[1]);
                        cProxy.username = ps[2];
                        cProxy.password = ps[3];
                    }
                    else
                    {
                        cProxy.ip = ps[0];
                        cProxy.port = int.Parse(ps[1]);
                    }

                    cProxy.active = 0;
                    cProxies.Add(cProxy);
                }

            }
            lbl_test.Text = "Đang kiểm tra kết nối proxy...";
            

            lst_running.Rows.Clear();
            foreach (var s in takiRunings)
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.lst_running.MasterView);
                rowInfo.Cells["username"].Value = s.name;
                rowInfo.Cells["complete"].Value = s.complete;
                rowInfo.Cells["status"].Value = "Đang đợi lệnh";
                lst_running.Rows.Add(rowInfo);
            }
            tab.SelectedTab = tabPage1;
            lbl_test.Text = "Phát hiện " + proxy.Count + " đang hoạt động";
            for (int i = 0; i < int.Parse(txt_Số_luồng.Text); i++)
            {
                Thread Chrome = new Thread(() =>
                {
                    if (check_thực_hiện_mời.Checked)
                    {
                        CProxy proxy = cProxies.Where(r => r.active == 0).OrderBy(r => r.active).FirstOrDefault();
                        proxy.active = 1;
                        Thực_hiện_autoAsync(proxy);
                    }
                    else
                    {
                        CProxy proxy = cProxies.Where(r => r.active < setting.advanced.max_user_proxy).OrderBy(r => r.active).FirstOrDefault();
                        proxy.active++;
                        Thực_hiện_autoAsync(proxy);
                    }
                })
                { IsBackground = true };
                Chrome.Name = "VMS_" + i;
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
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
                Thread.Sleep(500);
                Application.DoEvents();
            }
            timer1.Start();
            //reduceCPU.RunWorkerAsync();
        }

        private void Thực_hiện_autoAsync(CProxy proxy)
        {
            ChromeDriver driver = null;
            try
            {
                bool đăng_bài = check_đăng_post.Checked;
                bool nhiệm_vụ = check_làm_nhiệm_vụ.Checked;
                bool buy_coin = check_mua_coin.Checked;
                bool thực_hiện_mời = check_thực_hiện_mời.Checked;
                bool xoá_tài_khoản = check_xoá_tài_khoản.Checked;
                bool stake = ckb_stake.Checked;
                bool give_taki = check_Give_Taki.Checked;
                bool follow = ckb_follow.Checked;
                bool withdraw = ckb_auto_withdraw.Checked;
            Next_Taki:
                TakiAccount Taki = null;
                lock (takiRunings)
                {
                    if (takiRunings.Count > 0)
                    {
                        Taki = takiRunings.First();
                        takiRunings.Remove(Taki);
                    }
                }
                if (Taki != null)
                {
                    if (give_taki == true && frmTaki.receive.Any(r => r.receive == false && r.complete == true) == false && ckb_max.Checked == false)
                    {

                        if (lst_running.InvokeRequired)
                        {
                            lst_running.Invoke(new MethodInvoker(() =>
                            {
                                lst_running.Rows.Clear();
                            }));
                        }
                        else
                        {
                            lst_running.Rows.Clear();
                        }
                        return;
                    }
                    bool check = đăng_bài == true || nhiệm_vụ == true || buy_coin == true || give_taki == true || stake == true || withdraw == true || ckbCheckWithdraw.Checked || ckb_auto_follow.Checked || ckb_check_live.Checked;
                    if (check == false)
                    {
                        view.Hide();
                    }
                    Update_Label(lbl_test, "Đang chạy còn lại: " + takiRunings.Count());
                    GridViewRowInfo row = null;
                    foreach (var info in lst_running.Rows)
                    {
                        if (info.Cells["username"].Value.ToString() == Taki.name)
                        {
                            row = info;
                            break;
                        }
                    }
                    if (thực_hiện_mời == true)
                    {
                        Support.Tạo_RefAsync(Taki.invite_link, !xoá_tài_khoản, proxy, row, txt_balance);
                        Taki.isRef = false;
                        FileIO.CreateFile(Taki);
                    }

                    if (check == false)
                    {
                        goto Next_Taki;
                    }

                    lock (cProxies)
                    {

                    }
                    string proxyU = "";

                    //if (cProxies.Count != 0)
                    //{
                    //Get_Proxy:
                    //    CProxy proxy = cProxies.Where(r => r.active < setting.advanced.max_user_proxy).OrderBy(r => r.active).FirstOrDefault();
                    //    if (proxy != null )
                    //    {
                    //        if (DateTime.Parse(proxy.expired_at).AddMinutes(7) < DateTime.Now && proxy.active == 0)
                    //        {
                    //            string request = TMAPIHelper.GetNewProxy(proxy.api_key, null, null);
                    //            TmProxy tm = JsonConvert.DeserializeObject<TmProxy>(request);
                    //            if (string.IsNullOrEmpty(tm.data.https) == false)
                    //            {
                    //                proxy.last_ip = proxy.ip;
                    //                proxy.ip = tm.data.https;
                    //                proxy.expired_at = tm.data.expired_at;
                    //                proxyU = proxy.ip;
                    //                proxy.active++;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            proxyU = proxy.ip;
                    //            proxy.active++;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        Delay(50);
                    //        goto Get_Proxy;
                    //    }
                    //}

                    Thread thread = Thread.CurrentThread;

                    string dir = "Taki\\VMS\\" + thread.Name + "\\Default\\IndexedDB\\https_taki.app_0.indexeddb.leveldb";
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    else
                    {
                    Move_Test:
                        try
                        {

                            Directory.Delete(dir, true);
                            Directory.CreateDirectory(dir);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Delay(10);
                            goto Move_Test;
                        }
                    }
                    string source = "Taki\\Profile\\" + Taki.name + "\\https_taki.app_0.indexeddb.leveldb";

                    new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(source, dir);
                    var driverService = ChromeDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    ChromeOptions option = new ChromeOptions();
                    option.AddArgument("window-size=414,896");
                    option.AddExcludedArgument("enable-automation");
                    option.AddAdditionalOption("useAutomationExtension", false);
                    option.AddArgument("--autoplay-policy=no-user-gesture-required");
                    option.AddArgument("--mute-audio");
                    option.AddArgument("no-sandbox");
                    option.AddArgument("--disable-infobars");
                    option.AddArgument("--disable-default-apps");
                    option.AddArgument("--window-position=-2000,0");
                    option.AddArgument("--disable-gpu");
                    option.AddArgument("--FontRenderHinting[none]");
                    option.AddArgument("--disable-blink-features=AutomationControlled");
                    option.AddExtension("Taki/captcha.crx");
                    option.AddArgument("--app=https://taki.app/u/" + Taki.name.ToLower() + "/");
                    string path = Directory.GetCurrentDirectory();
                    option.AddArgument("user-data-dir=" + path + "\\Taki\\VMS\\" + thread.Name);
                    //option.AddArgument("user-data-dir=" + setting.path_profile + @"\" + Taki.name);
                    if (string.IsNullOrEmpty(proxyU) == false)
                    {
                        string[] proxys = Taki.proxy.Split(':');
                        //option.AddHttpProxy(proxys[0], int.Parse(proxys[1]), proxys[2], proxys[3]);
                    }
                    else
                    {
                        //option.AddHttpProxy(proxy.ip, proxy.port, proxy.username, proxy.password);
                    }
                    if (string.IsNullOrEmpty(Taki.userAgent) == true)
                        Taki.userAgent = Support.GetRandomUserAgent();
                    FileIO.CreateFile(Taki);
                    option.AddArgument("--user-agent=" + Taki.userAgent);
                    driver = new ChromeDriver(driverService, option, TimeSpan.FromSeconds(360));
                    Support.Setting_Captcha(driver);
                    int ProcessId = driverService.ProcessId;
                    view.Add_View(Taki.name, ProcessId);
                    ChromeView chrome_view = new ChromeView();
                    chrome_view.driver = driver;
                    chrome_view.name = Taki.name;
                    view.views.Add(chrome_view);
                    driver.Manage().Window.Size = new Size(414, 896);
                    driver.Manage().Window.Position = new Point(0, 0);
                    driver.ExecuteScript("document.title = 'Taki: " + Taki.name + "'");
                    Delay(1);


                    string body = driver.FindElement(By.CssSelector("body")).GetAttribute("innerHTML");
                    if (body == "Internal error")
                    {
                    Fix:
                        row.Update_Status("Lỗi Internal error !!!");
                        driver.Navigate().GoToUrl("https://taki.app/");
                        List<IWebElement> postE = driver.FindElements(By.CssSelector(Model.Create_Post.Button_Create)).ToList();
                        if (postE.Count == 0)
                        {
                            driver.Navigate().Refresh();
                            Delay(5);
                            goto Fix;
                        }

                    Home_Screen:
                        try
                        {
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                            IWebElement captcha_invisable = driver.FindElement(By.CssSelector(Model.Captcha.captcha_solver));

                            string solver = captcha_invisable.GetAttribute("data-state");
                            while (solver != "solved" && solver != "error")
                            {
                                row.Update_Status("Đang giải captcha");
                                Delay(5);
                                solver = captcha_invisable.GetAttribute("data-state");
                            }
                            row.Update_Status("Đã giải captcha");
                            driver.Navigate().Refresh();
                            Delay(5);
                            goto Home_Screen;
                        }
                        catch
                        {
                            row.Update_Status("Đã xử lý captcha...");
                        }

                    Fix_Max:
                        List<IWebElement> menu = driver.FindElements(By.CssSelector(Model.Create_Post.Button_Create)).ToList();
                        if (menu.Count == 0)
                        {
                            driver.Navigate().Refresh();
                            Delay(5);
                            goto Fix_Max;
                        }
                        else if (menu.Count == 5)
                        {
                            menu[4].Click();
                        }
                    }
                    else
                    {
                    Home_Screen:
                        try
                        {
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                            IWebElement captcha_invisable = driver.FindElement(By.CssSelector(Model.Captcha.captcha_solver));

                            string solver = captcha_invisable.GetAttribute("data-state");
                            while (solver != "solved" && solver != "error")
                            {
                                row.Update_Status("Đang giải captcha");
                                Delay(5);
                                solver = captcha_invisable.GetAttribute("data-state");
                            }
                            row.Update_Status("Đã giải captcha");
                            driver.Navigate().Refresh();
                            Delay(5);
                            goto Home_Screen;
                        }
                        catch
                        {
                            row.Update_Status("Đã xử lý captcha...");
                        }
                    }

                    string urlCurent = driver.Url;
                    driver.Navigate().Refresh();
                    if (urlCurent != "https://taki.app/u/" + Taki.name.ToLower() + "/")
                    {
                        try
                        {
                            IWebElement h1 = driver.FindElement(By.XPath("//*[@id=\"__next\"]/div[2]/div[2]/h1"));
                            string text = h1.GetAttribute("innerHTML");
                            if (text == "Own your token-powered social network")
                            {
                                Taki.isLive = false;
                                FileIO.CreateFile(Taki);
                                row.Update_Status("Lock !!!");
                            }
                        }
                        catch
                        {
                            row.Update_Status("Lỗi cần xác minh lại !!!");
                        }

                        row.Update_Color(Color.OrangeRed);
                        driver.Quit();
                        try
                        {
                            if (view.Tab.InvokeRequired)
                            {
                                view.Tab.Invoke(new MethodInvoker(() =>
                                {
                                    foreach (var tab in view.Tab.Tabs)
                                    {
                                        if (tab.Text == Taki.name)
                                        {
                                            view.Tab.Tabs.Remove(tab);
                                            view.views.Remove(chrome_view);
                                            break;
                                        }
                                    }
                                }));
                            }
                            else
                            {
                                foreach (var tab in view.Tab.Tabs)
                                {
                                    if (tab.Text == Taki.name)
                                    {
                                        view.Tab.Tabs.Remove(tab);
                                        view.views.Remove(chrome_view);
                                        break;
                                    }
                                }
                            }
                        }
                        finally
                        {

                        }
                        goto Next_Taki;

                    }

                    Taki.isLive = true;

                    if (ckb_check_live.Checked)
                    {
                        FileIO.CreateFile(Taki);
                        row.Update_Status("Alive !!!");
                        row.Update_Color(Color.LightGreen);
                        driver.Quit();
                        try
                        {
                            if (view.Tab.InvokeRequired)
                            {
                                view.Tab.Invoke(new MethodInvoker(() =>
                                {
                                    foreach (var tab in view.Tab.Tabs)
                                    {
                                        if (tab.Text == Taki.name)
                                        {
                                            view.Tab.Tabs.Remove(tab);
                                            view.views.Remove(chrome_view);
                                            break;
                                        }
                                    }
                                }));
                            }
                            else
                            {
                                foreach (var tab in view.Tab.Tabs)
                                {
                                    if (tab.Text == Taki.name)
                                    {
                                        view.Tab.Tabs.Remove(tab);
                                        view.views.Remove(chrome_view);
                                        break;
                                    }
                                }
                            }
                        }
                        finally { }

                        goto Next_Taki;
                    }

                    // Xữ lý install app
                    List<IWebElement> install = driver.FindElements(By.CssSelector("#__next > div.css-n1swk4.e1t7gsfq2 > div.css-1cywz1h.e1t7gsfq0")).ToList();
                    if (install.Count != 0)
                    {
                        install.First().Click();
                    }

                    //TakiData profile = new TakiData();

                    #region Xữ lý lấy point
                    TakiData profile = TakiHelper.Get_Profile(driver, Taki.name.ToLower(), row, true);
                    if (profile.point == null)
                    {
                        row.Update_Color(Color.OrangeRed);
                        row.Update_Status("Lỗi cần xác minh lại !!!");
                        goto Next_Taki;
                    }
                    else
                    {
                        Taki.point = profile.point.Value;
                    }
                    #endregion

                    driver.Navigate().GoToUrl("https://taki.app/home/");

                #region Đăng bài
                Đăng_bài_again:
                    if (đăng_bài == true && string.IsNullOrEmpty(Taki.topic) == false)
                    {
                        string text = "";
                        string url = "";
                        lock (bài_viết)
                        {
                            if (bài_viết.Where(r => r.username == Taki.topic).Any() == false)
                            {
                                List<Tweet> bài_mới = TakiHelper.Get_Post(setting.basic.api_post, Taki.topic);
                                if (bài_viết == null)
                                {
                                    bài_viết = new List<Tweet>();
                                }

                                if (bài_mới != null)
                                {
                                    bài_mới.ForEach(r => r.username = Taki.topic);
                                    bài_viết.AddRange(bài_mới);
                                }
                            }

                            if (bài_viết.Count > 1)
                            {
                                Tweet tweet = bài_viết.Where(r => r.username == Taki.topic).OrderBy(r => r.timestamp).FirstOrDefault();
                                if (tweet == null)
                                {
                                    tweet = bài_viết.OrderBy(r => r.timestamp).FirstOrDefault();
                                }
                                text = tweet.text;
                                url = tweet.media_url?.FirstOrDefault();
                                bài_viết.Remove(tweet);
                            }
                        }
                        if (text != "" || url != "")
                        {
                            string post_msg = text.TrimEnd();
                            post_msg = Regex.Replace(post_msg, @"http[^\s]+", "");
                            if (post_msg.Length < 5)
                            {
                                goto Đăng_bài_again;
                            }
                            TakiHelper.Create_Post(driver, post_msg, url, row);
                            Taki.upload = true;
                        }
                        else
                        {
                            row.Update_Status("API: Đã hết bài viết");
                            Delay(1);
                        }
                    }
                    #endregion


                    #region Nhiệm vụ
                    if (nhiệm_vụ == true && Taki.complete == false)
                    {
                        Complete run_complete = TakiHelper.Run_Mission(driver, follow, row);
                        if (run_complete == null)
                        {
                            log.Info("Không thể thực hiện nhiệm vụ: " + Taki.name);
                        }
                        else
                        {
                            Taki.complete = run_complete.isComplete;
                            if (run_complete.receive == true)
                            {
                                Taki.receive = true;
                            }
                            row.Update("complete", Taki.complete.ToString());
                        }

                    }
                    #endregion

                    #region Buy Coin
                    if (buy_coin == true)
                    {
                        Wallet walletb = TakiHelper.Lấy_Taki(driver);
                        if (Convert.ToInt32(walletb.taki) > 5)
                        {
                            if (profile.coin.value < int.Parse(txt_buy_coin.Text))
                            {
                                Taki.taki = Support.Buy_Taki(driver, Taki.name.ToLower(), walletb.taki, row);
                            }
                            else
                            {
                                row.Update_Status("Buff: Đã đạt đủ điều kiện Price");
                            }
                        }
                        else
                        {
                            row.Update_Status("Error: Buff Taki: <5 !!!");
                            Taki.taki = walletb.taki;
                        }
                    }
                    #endregion

                    if (give_taki == true)
                    {
                        if (Taki.taki >= int.Parse(txt_số_give_taki.Text) + 2)
                        {
                            string[] givetaki = File.ReadAllLines("Taki\\givetaki.txt");
                            if (givetaki.Length == 0)
                            {
                                MessageBox.Show("Không thể lấy danh sách Give Taki");
                                return;
                            }
                            List<string> give = givetaki.ToList();
                            give = give.OrderBy(a => Guid.NewGuid()).ToList();
                            List<string> urls = new List<string>();
                            foreach (var s in give)
                            {
                                urls.Add("https://taki.app/u/" + s + "/");
                            }
                            int repeat = ((int)Taki.taki - int.Parse(txt_số_give_taki.Text)) / 2;
                            if (ckb_max.Checked)
                            {
                                TakiHelper.Auto_Give_Taki(driver, urls.ToArray(), row, repeat, ckb_max.Checked, Taki.taki);
                            }
                            else
                            {
                                TakiHelper.Auto_Give_Taki(driver, urls.ToArray(), row, repeat, ckb_max.Checked);
                            }
                        }
                        else
                        {
                            row.Update_Status("Give Taki: <5 !!!");
                        }
                    }
                    Wallet wallet = TakiHelper.Lấy_Taki(driver, false);
                    Taki.taki = wallet.taki;
                    Taki.stake = wallet.stake != null ? wallet.stake.Value : 0;
                    Taki.price = profile?.coin?.value;
                    lock (obj)
                    {
                        string json_dev = JsonConvert.SerializeObject(setting, Formatting.Indented);
                        FileIO.Create_File_From_Json(json_dev, "Taki\\config.json");
                    }

                    #region Withdraw
                    if (withdraw == true)
                    {
                        bool ok = TakiHelper.Auto_Withdraw(driver, row);
                        if (ok == true)
                        {
                            Taki.withdraw = false;
                            wallet = TakiHelper.Lấy_Taki(driver, true);
                            Taki.taki = wallet.taki;
                            Taki.stake = wallet.stake != null ? wallet.stake.Value : 0;
                            Taki.withdraw_day = true;
                            lock (obj)
                            {
                                setting.earn += 95;
                                string json_dev = JsonConvert.SerializeObject(setting, Formatting.Indented);
                                FileIO.Create_File_From_Json(json_dev, "Taki\\config.json");
                                CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
                                string earning = setting.earn.ToString("#,###", cul.NumberFormat);
                                if (lbl_earning.InvokeRequired)
                                {
                                    lbl_earning.Invoke(new MethodInvoker(() =>
                                    {
                                        lbl_earning.Text = earning + "TAKI";
                                    }));
                                }
                                else
                                {
                                    lbl_earning.Text = earning + "TAKI";
                                }
                            }
                        }
                        else
                        {
                            Taki.withdraw_day = true;
                        }
                    }
                    else
                    {
                        profile = TakiHelper.Get_Profile(driver, Taki.name.ToLower(), row, false);
                        if (profile.point == null)
                        {
                            row.Update_Color(Color.Blue);
                        }
                        else
                        {
                            Taki.point = profile.point.Value;
                        }

                        if (Taki.isWithdraw == true)
                        {

                            if (profile.coin.value >= 2 && Taki.point > 550 && Taki.taki > 105)
                            {
                                Taki.withdraw = true;
                            }
                            else
                            {
                                Taki.withdraw = false;
                            }
                        }
                    }
                    #endregion

                    if (ckb_auto_follow.Checked == true)
                    {
                        if (Taki.taki > 10)
                        {
                            int count = 0;
                            int repeat = (int)((Taki.taki - 10) / 0.5M);
                            driver.Navigate().GoToUrl("https://taki.app/u/" + Taki.name.ToLower() + "/");
                            driver.FindElement(By.CssSelector("#content-wrapper > div.css-c9cvz2.e56mmy050 > div > div > div > div.css-11xi8vz.e56mmy035 > div.css-1als9oi.e56mmy034 > div:nth-child(2)")).Click();
                            Delay(5);
                            List<IWebElement> content = driver.FindElements(By.CssSelector("div.css-53k3v9.e56mmy037 > div.css-1w01iqv.e56mmy017 > table > tbody > tr")).ToList();
                            List<string> follows_current = new List<string>();
                            foreach (var s in content)
                            {
                                follows_current.Add("https://taki.app/u/" + s.FindElement(By.CssSelector("td:nth-child(2) > div > strong")).Text + "/");
                            }
                        Get_bài_viết:

                            Random key = new Random();
                            int next = key.Next(0, 100);
                            bool using_key = next <= frmTaki.coupon ? true : false;

                            string url_name = "https://taki.app/u/" + Taki.name.ToLower() + "/";
                            List<string> follows = new List<string>();
                            if (using_key == true && frmTaki.dev.Count > 0)
                            {
                            Add_Dev:
                                follows.Add(frmTaki.dev.OrderBy(a => Guid.NewGuid()).First());
                                foreach (var s in follows.ToList())
                                {
                                    if (follows_current.Contains(s))
                                    {
                                        follows.Remove(s);
                                    }
                                }

                                if (follows.Count < 1)
                                {
                                    goto Add_Dev;
                                }
                            }

                            if (follow == true)
                            {
                                List<string> fl = File.ReadAllLines("Taki\\follow.txt").ToList();
                                var shuffledFollow = fl.OrderBy(a => Guid.NewGuid()).ToList();
                                foreach (var s in shuffledFollow)
                                {
                                    string url = "https://taki.app/u/" + s + "/";
                                    if (url != url_name)
                                    {
                                        follows.Add(url);
                                    }
                                }
                            }
                            else
                            {
                                driver.Navigate().GoToUrl("https://taki.app/home/");
                                List<IWebElement> all_tab = driver.FindElements(By.CssSelector(Model.Nhiệm_Vụ.ALL)).ToList();
                                if (all_tab.Count == 0)
                                {
                                    goto Get_bài_viết;
                                }
                                row.Update_Status("Đang lấy user follow");
                                Delay(5);
                                List<IWebElement> new_follows = driver.FindElements(By.CssSelector(Model.Nhiệm_Vụ.New_Post)).ToList();
                                foreach (var fl in new_follows)
                                {
                                    string coin_name = fl.GetAttribute("href");
                                    if (follows.Contains(coin_name) == false && coin_name != url_name)
                                    {
                                        follows.Add(coin_name);
                                    }
                                }
                            }
                            foreach (var s in follows.ToList())
                            {
                                if (follows_current.Contains(s))
                                {
                                    follows.Remove(s);
                                }
                            }

                            if (follows.Count < 1)
                            {
                                follows.AddRange(frmTaki.dev_give.OrderBy(a => Guid.NewGuid()).ToList());
                                foreach (var s in follows)
                                {
                                    if (follows_current.Contains(s))
                                    {
                                        follows.Remove(s);
                                    }
                                }
                            }

                            row.Update_Status("Đã lấy được: " + follows.Count + " kết quả...");
                            bool flgdev = false;
                            for (int i = 0; i < repeat && i < 5; i++)
                            {
                                foreach (var coin_name in follows.ToList())
                                {
                                    bool run = TakiHelper.Follow(driver, coin_name, row);
                                    if (using_key == true && run == true)
                                    {
                                        if (flgdev == false)
                                        {
                                            Properties.Taki.Default.donate_follow++;
                                            Properties.Taki.Default.Save();
                                        }

                                        flgdev = true;
                                    }

                                    if (run == true)
                                    {
                                        Taki.taki -= 0.5M;
                                        follows.Remove(coin_name);
                                        break;
                                    }
                                }
                                count++;
                            }
                            row.Update_Status("Đã follow thành công: " + count);
                        }
                    }

                    //if (stake == true && Taki.withdraw == true && Taki.withdraw_day == true && Taki.taki > 110)
                    //{
                    //    wallet = TakiHelper.Lấy_Taki(driver, true);
                    //    Taki.taki = wallet.taki;
                    //    Taki.stake = wallet.stake != null ? wallet.stake.Value : 0;
                    //    int stake_amount = Convert.ToInt32(Taki.taki - 110);
                    //    if (stake_amount > 20 && Taki.stake == 0)
                    //    {
                    //        bool check_stake = TakiHelper.Auto_Stake(driver, stake_amount, row);
                    //        if (check_stake)
                    //        {
                    //            Taki.taki -= stake_amount;
                    //            Taki.stake = stake_amount;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        row.Update_Status("Không đạt điều kiện để stake !!!");
                    //    }
                    //}

                    Taki.apr = profile?.coin?.APR;

                    FileIO.CreateFile(Taki);

                    if (lst_running.InvokeRequired)
                    {
                        lst_running.Invoke(new MethodInvoker(() =>
                        {
                            lst_running.Rows.Remove(row);
                        }));
                    }
                    else
                    {
                        lst_running.Rows.Remove(row);
                    }

                    if (view.Tab.InvokeRequired)
                    {
                        view.Tab.Invoke(new MethodInvoker(() =>
                        {
                            foreach (var tab in view.Tab.Tabs)
                            {
                                if (tab.Text == Taki.name)
                                {
                                    view.Tab.Tabs.Remove(tab);
                                    view.views.Remove(chrome_view);
                                    break;
                                }
                            }
                        }));
                    }
                    else
                    {
                        foreach (var tab in view.Tab.Tabs)
                        {
                            if (tab.Text == Taki.name)
                            {
                                view.Tab.Tabs.Remove(tab);
                                view.views.Remove(chrome_view);
                                break;
                            }
                        }
                    }
                    if (takiRunings.Count == 0 && view.Tab.Tabs.Count == 1)
                    {
                        view.Close();
                    }

                    txt_hold.Text = takiAccounts.Sum(r => r.taki).ToString();

                    driver.Quit();
                    goto Next_Taki;
                }
                else
                {
                    Delay(10);
                    string json = JsonConvert.SerializeObject(bài_viết, Formatting.Indented);
                    FileIO.Create_File_From_Json(json, postData);

                    string jsonT = JsonConvert.SerializeObject(tweet_Profiles, Formatting.Indented);
                    FileIO.Create_File_From_Json(jsonT, "Taki/username.json");

                    flgCPU = false;
                    if (takiRunings.Count == 0 && view.Tab.Tabs.Count == 1)
                    {
                        if (lbl_test.InvokeRequired)
                        {
                            lbl_test.Invoke(new MethodInvoker(() =>
                            {
                                lbl_test.Text = "Chức năng Auto đã hoàn thành !!";
                            }));
                        }
                        else
                        {
                            lbl_test.Text = "Chức năng Auto đã hoàn thành !!";
                        }
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                if (driver != null)
                {
                    driver.Quit();
                }

                log.Info("===Thực_hiện_autoAsync Error-START");
                log.Error(ex.Message, "Thực_hiện_autoAsync Error");
                log.Error(ex.StackTrace, "Thực_hiện_autoAsync Error");
                log.Error(ex.InnerException, "Thực_hiện_autoAsync Error");
                log.Info("===Thực_hiện_autoAsync Error-END");
                Thực_hiện_autoAsync(proxy);
            }

        }

        private void LoadSetting()
        {
            string file_content = File.ReadAllText("Taki\\config.json");
            setting = JsonConvert.DeserializeObject<Setting>(file_content);

            if (setting == null)
            {
                setting = new Setting();
            }

            if (File.Exists("Taki\\username.json") == false)
            {
                FileIO.Create_File_From_Json("", "Taki\\username.json");
            }
            string user_content = File.ReadAllText("Taki\\username.json");
            tweet_Profiles = JsonConvert.DeserializeObject<List<Tweet_Profile>>(user_content);

            if (tweet_Profiles == null)
            {
                tweet_Profiles = new List<Tweet_Profile>();
            }

            label_path.Text = setting.basic?.path_profile;
            txt_API_Proxy.Text = setting.basic?.api_proxy;
            txt_API_SMS.Text = setting.basic?.api_sms;
            txt_API_captcha.Text = setting.basic?.api_captcha;
            txt_RapidAPI.Text = setting.basic?.api_post;

            txt_Số_luồng.Text = setting.thread == null ? "5" : setting.thread.ToString();
            check_đăng_post.Checked = Properties.Taki.Default.check_tạo_post;
            check_làm_nhiệm_vụ.Checked = Properties.Taki.Default.check_làm_nhiệm_vụ;
            ckb_stake.Checked = Properties.Taki.Default.check_stake;
            check_Give_Taki.Checked = Properties.Taki.Default.check_Give_Taki;
            check_mua_coin.Checked = Properties.Taki.Default.check_mua_coin;
            check_xoá_tài_khoản.Checked = Properties.Taki.Default.check_xoá_tài_khoản;
            check_thực_hiện_mời.Checked = Properties.Taki.Default.check_thực_hiện_mời;
            txt_Wallet.Text = setting.wallet;

            // Setting nâng cao
            s_max_give_user.Text = setting.advanced?.max_give_user.ToString();
            s_max_follow_user.Text = setting.advanced?.max_follow_user.ToString();
            s_max_apr_user.Text = setting.advanced?.max_apr_user.ToString();
            s_max_give_post_user.Text = setting.advanced?.max_give_post_user.ToString();
            s_min_follow_user.Text = setting.advanced?.min_follow_user.ToString();
            s_max_user_proxy.Text = setting.advanced?.max_user_proxy.ToString();
            switch_give.Value = setting.advanced.give_withdraw;
        }

        private void btn_Lưu_cấu_hình_Click(object sender, EventArgs e)
        {
            Properties.Taki.Default.check_tạo_post = check_đăng_post.Checked;
            Properties.Taki.Default.check_làm_nhiệm_vụ = check_làm_nhiệm_vụ.Checked;
            Properties.Taki.Default.check_mua_coin = check_mua_coin.Checked;
            Properties.Taki.Default.check_stake = ckb_stake.Checked;
            Properties.Taki.Default.check_Give_Taki = check_Give_Taki.Checked;
            Properties.Taki.Default.txt_số_give_taki = int.Parse(txt_số_give_taki.Text);
            Properties.Taki.Default.check_thực_hiện_mời = check_thực_hiện_mời.Checked;
            Properties.Taki.Default.check_xoá_tài_khoản = check_xoá_tài_khoản.Checked;
            setting.thread = int.Parse(txt_Số_luồng.Text);
            setting.wallet = txt_Wallet.Text;
            string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
            string pathConfig = "Taki\\config.json";
            FileIO.Create_File_From_Json(json, pathConfig);
            Properties.Taki.Default.Save();
            MessageBox.Show("Đã lưu cấu hình !!!");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TMProxy.LinkVisited = true;
            System.Diagnostics.Process.Start("https://tmproxy.com/proxy");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Captcha69.LinkVisited = true;
            Process.Start("https://anycaptcha.com?referral=14863");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ViOTP.LinkVisited = true;
            Process.Start("https://viotp.com/Account/Register?referCode=10151");
        }

        private void ran_proxy_Click(object sender, EventArgs e)
        {
            proxy = new List<string>();
            foreach (var s in txt_API_Proxy.Lines)
            {
                if (string.IsNullOrEmpty(s) == false)
                {
                    string json = TMAPIHelper.Stats(s);
                    TmStats stats = JsonConvert.DeserializeObject<TmStats>(json);
                    if (stats.message == "")
                    {
                        proxy.Add(s);
                    }
                }
            }

            if (proxy.Count == 0)
            {
                MessageBox.Show("Vui lòng cấu hình API Proxy chính xác !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string request = TMAPIHelper.GetNewProxy(proxy[0], null, null);
            TmProxy tm = JsonConvert.DeserializeObject<TmProxy>(request);
            if (string.IsNullOrEmpty(tm.data.https) == false)
            {
                txtProxy.Text = tm.data.https;
            }
            else
            {
                request = TMAPIHelper.GetCurrentProxy(proxy[0]);
                tm = JsonConvert.DeserializeObject<TmProxy>(request);
                txtProxy.Text = tm.data.https;
            }

        }

        private void btn_lấy_bài_viết_Click(object sender, EventArgs e)
        {
            TakiHelper.Lấy_Taki(test);
        }

        private void RapidAPI_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ViOTP.LinkVisited = true;
            Process.Start("https://newsdata.io/dashboard");
        }

        private void btn_test_proxy_Click(object sender, EventArgs e)
        {
            proxy = new List<string>();
            foreach (var s in txt_API_Proxy.Lines)
            {
                if (string.IsNullOrEmpty(s) == false)
                {
                    string json = TMAPIHelper.Stats(s);
                    TmStats stats = JsonConvert.DeserializeObject<TmStats>(json);
                    if (stats.message == "")
                    {
                        proxy.Add(s);
                    }
                    else
                    {
                        MessageBox.Show("Token: " + s + " - " + stats.message);
                    }
                }
            }
            MessageBox.Show("Đã kiểm tra xong proxy !!!\nHoạt động: " + proxy.Count);
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frmUpdate = new frmUpdate();
            frmUpdate.Closed += (s, args) => this.Close();
            frmUpdate.Show();
        }

        private void btn_captcha_Click(object sender, EventArgs e)
        {
            Thread Chrome = new Thread(() =>
            {
                foreach (var takin in takiAccounts)
                {
                    //string root = setting.path_profile + @"\Astral67851\Default\Extension State";
                    //Support.CopyFilesRecursively(root, setting.path_profile + @"\" + takin.name + @"\Default\Extension State");
                    //root = setting.path_profile + @"\Astral67851\Default\Extension Scripts";
                    //Support.CopyFilesRecursively(root, setting.path_profile + @"\" + takin.name + @"\Default\Extension Scripts");
                    //root = setting.path_profile + @"\Astral67851\Default\Extension Rules";
                    //Support.CopyFilesRecursively(root, setting.path_profile + @"\" + takin.name + @"\Default\Extension Rules");
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
                    option.AddExtension("Taki/captcha.crx");
                    option.AddArgument("user-data-dir=" + setting.basic.path_profile + @"\" + takin.name);
                    ChromeDriver driver = new ChromeDriver(driverService, option);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    IWebElement api_key = driver.FindElement(By.CssSelector(Model.Captcha.Input_API));
                    string vvv = api_key.GetAttribute("value");
                    if (vvv != setting.basic.api_captcha)
                    {
                        string js = "let config_captcha = {}; config_captcha['autoSubmitForms'] = true; config_captcha['autoSolveRecaptchaV2'] = true; config_captcha['autoSolveInvisibleRecaptchaV2'] = false; config_captcha['autoSolveRecaptchaV3'] = false; Config.set(config_captcha);";
                        string config = (string)driver.ExecuteScript(js);
                        api_key.Clear();
                        Delay(1);
                        api_key.SendKeys(setting.basic.api_captcha);
                        driver.FindElement(By.CssSelector(Model.Captcha.Input)).Click();
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                        Delay(3);
                        var alert = driver.SwitchTo().Alert();
                        alert.Accept();
                        lbl_test.Text = "Đã update captcha: " + takin.name;
                        driver.Close();
                        driver.Quit();
                    }
                    else
                    {
                        lbl_test.Text = "Đã bỏ qua captcha: " + takin.name;
                        driver.Close();
                        driver.Quit();
                    }

                }
            });
            Chrome.Start();
        }

        private void btn_give_taki_Click(object sender, EventArgs e)
        {
            string path_file = "Taki\\givetaki.txt";
            if (File.Exists(path_file) == false)
            {
                File.Create(path_file);
            }
            Process.Start(path_file);
        }

        private void lst_account_Click(object sender, EventArgs e)
        {

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = @"C:\Program Files (x86)\AnyDesk\AnyDesk.exe";
            cmd.StartInfo.Arguments = "--get-id";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.WaitForExit();
            string id = cmd.StandardOutput.ReadToEnd();
            string file_content = File.ReadAllText("Taki\\config.json");
            setting = JsonConvert.DeserializeObject<Setting>(file_content);

            if (setting == null)
            {
                setting = new Setting();
            }
            //const int PORT_NUMBER = 65432;
            //try
            //{
            //    UTF8Encoding encoding = new UTF8Encoding();
            //    TcpClient client = new TcpClient();
            //    // 1. connect
            //    client.Connect("103.82.25.22", PORT_NUMBER);
            //    Stream stream = client.GetStream();
            //    string str = id + "|Date:" + DateTime.Now + "|Taki:" + Properties.Taki.Default.donate_taki + "|Follow:" + Properties.Taki.Default.donate_follow + "\n";
            //    byte[] data = encoding.GetBytes(str);
            //    stream.Write(data, 0, data.Length);
            //    str = "x";
            //    data = encoding.GetBytes(str);
            //    stream.Write(data, 0, data.Length);
            //    MessageBox.Show("Cảm ơn bạn đã sử dụng phần mềm\nSố Taki ủng hộ: " + Properties.Taki.Default.donate_taki + "\nSố Follow đã ủng hộ: " + Properties.Taki.Default.donate_follow + "\nChúc ngày làm việc thuận lợi !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    Properties.Taki.Default.donate_taki = 0;
            //    Properties.Taki.Default.donate_follow = 0;
            //    Properties.Taki.Default.Save();
            //}
            //catch
            //{
            //    MessageBox.Show("Không thể gửi báo cáo hằng ngày !!!");
            //}


            foreach (var Taki in takiAccounts)
            {
                Taki.complete = false;
                Taki.receive = false;
                Taki.upload = false;
                Taki.withdraw_day = false;
                FileIO.CreateFile(Taki);
            }
            MessageBox.Show("Đã cập nhật thành công !!");
        }

        private void frmTaki_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (test != null)
            {
                test.Quit();
            }
        }

        private void lst_running_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            //string name = (string)e.RowElement.RowInfo.Cells["username"].Value;
            //TakiAccount taki = takiAccounts.Where(r => r.name == name).First();
            //if (taki.point >=350)
            //{
            //    e.RowElement.DrawFill = true;
            //    e.RowElement.GradientStyle = GradientStyles.Solid;
            //    e.RowElement.BackColor = ColorTranslator.FromHtml("167,187,222"); ;
            //}
            //else
            //{
            //    e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
            //    e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
            //    e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
            //}
            //e.RowElement.BackColor = Color.White;
        }

        private void btn_follow_Click(object sender, EventArgs e)
        {
            string path_file = "Taki\\follow.txt";
            if (File.Exists(path_file) == false)
            {
                File.Create(path_file);
            }
            Process.Start(path_file);
        }

        private void btn_raw_Click(object sender, EventArgs e)
        {
            string raw = "";
            foreach (var s in takiAccounts)
            {
                raw += s.name.ToLower() + "\n";
            }
            MessageBox.Show("Đang có: " + takiAccounts.Count + " tài khoản\n" + raw, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_sort_hold_Click(object sender, EventArgs e)
        {
            LoadAccount(ckb_show_live.Checked, 2);
        }

        private void btn_sort_point_Click(object sender, EventArgs e)
        {
            LoadAccount(ckb_show_live.Checked, 3);
        }

        private void btn_reset_mine_Click(object sender, EventArgs e)
        {
            setting.earn = 0;
            string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
            string pathConfig = "Taki\\config.json";
            FileIO.Create_File_From_Json(json, pathConfig);
            MessageBox.Show("Đã cập nhật thành công !!");
        }

        public void Update_Label(RadLabel label, string msg)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new MethodInvoker(() =>
                {
                    label.Text = msg;
                }));
            }
            else
            {
                label.Text = msg;
            }
        }

        private void btn_clear_cache_Click(object sender, EventArgs e)
        {
            string[] dirs = Directory.GetDirectories("Taki\\VMS");
            Thread Chrome = new Thread(() =>
            {

                foreach (string s in dirs)
                {
                    DirectoryInfo directory = new DirectoryInfo(s + "\\Default\\Cache\\Cache_Data");
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        file.Delete();
                    }

                    DirectoryInfo directory2 = new DirectoryInfo(s + "\\Default\\Code Cache");
                    if (directory2.Exists)
                    {
                        directory2.Delete(true);
                    }
                    Update_Label(lbl_test, "Clear: " + s);
                }
                MessageBox.Show("Đã clear Cache !!!");
            });
            Chrome.Start();
        }

        private void btn_profile_upgrade_Click(object sender, EventArgs e)
        {
            Thread Chrome = new Thread(() =>
            {
                foreach (var run in takiAccounts.Where(r => r.topic == null || r.topic == ""))
                {
                Get_Profile:
                    try
                    {
                        if (tweet_Profiles.Count == 0)
                        {
                            Update_Label(lbl_test, "Đã hoàn thành còn lại: " + takiAccounts.Where(r => r.topic == null || r.topic == "").Count() + " chưa xử lý");
                            break;
                        }
                        Tweet_Profile profile = tweet_Profiles.First();
                        tweet_Profiles.Remove(profile);

                        if (profile.username == run.topic)
                        {
                            goto Get_Profile;
                        }

                        string dir = "Taki\\VMS\\VmsRuning\\Default\\IndexedDB\\https_taki.app_0.indexeddb.leveldb";
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        else
                        {
                            Directory.Delete(dir, true);
                            Directory.CreateDirectory(dir);
                        }
                        string source = "Taki\\Profile\\" + run.name + "\\https_taki.app_0.indexeddb.leveldb";

                        new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(source, dir);

                        var driverService = ChromeDriverService.CreateDefaultService();
                        driverService.HideCommandPromptWindow = true;
                        ChromeOptions option = new ChromeOptions();
                        option.AddArgument("window-size=414,896");
                        option.AddExcludedArgument("enable-automation");
                        option.AddAdditionalOption("useAutomationExtension", false);
                        option.AddArgument("--autoplay-policy=no-user-gesture-required");
                        option.AddArgument("--mute-audio");
                        option.AddArgument("no-sandbox");
                        option.AddArgument("--disable-infobars");
                        option.AddArgument("--disable-default-apps");
                        option.AddExtension("Taki/captcha.crx");
                        option.AddArgument("--app=https://taki.app/u/" + run.name.ToLower() + "/");
                        string path = Directory.GetCurrentDirectory();
                        option.AddArgument("user-data-dir=" + path + "\\Taki\\VMS\\VmsRuning");
                        //option.AddArgument("user-data-dir=" + setting.path_profile + @"\" + run.name);
                        option.AddArgument("--user-agent=Mozilla/5.0 (Linux; Android 10; Lenovo TB-X505F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5125.189 Mobile Safari/537.36");
                        test = new ChromeDriver(driverService, option);
                        Support.Setting_Captcha(test, false);
                        test.ExecuteScript("arguments[0].click();", test.FindElement(By.CssSelector(Model.Profile.Edit_Profile)));
                        IWebElement input_name = test.FindElement(By.CssSelector("#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-c2g4g1.epiie927 > div > div > form > div:nth-child(3) > div > input"));
                        input_name.Clear();
                        if (string.IsNullOrEmpty(profile.name) == false)
                        {
                            Thread STAThread = new Thread(
                                   delegate ()
                                   {
                                       for (int i = 0; i < 10; i++)
                                       {
                                           try
                                           {
                                               Clipboard.SetText(profile.name);
                                               break;
                                           }
                                           catch { }
                                           Thread.Sleep(10);
                                       }
                                   });
                            STAThread.SetApartmentState(ApartmentState.STA);
                            STAThread.Start();
                            STAThread.Join();
                            input_name.SendKeys(OpenQA.Selenium.Keys.Control + "v");
                        }
                        IWebElement input_bio = test.FindElement(By.CssSelector("#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-c2g4g1.epiie927 > div > div > form > div:nth-child(5) > div > textarea"));
                        input_bio.Clear();
                        if (string.IsNullOrEmpty(profile.description) == false)
                        {
                            Thread STAThread = new Thread(
                                   delegate ()
                                   {
                                       for (int i = 0; i < 10; i++)
                                       {
                                           try
                                           {
                                               Clipboard.SetText(profile.description);
                                               break;
                                           }
                                           catch { }
                                           Thread.Sleep(10);
                                       }
                                   });
                            STAThread.SetApartmentState(ApartmentState.STA);
                            STAThread.Start();
                            STAThread.Join();
                            input_bio.SendKeys(OpenQA.Selenium.Keys.Control + "v");
                        }
                        if (string.IsNullOrEmpty(profile.profile_pic_url) == false && profile.profile_pic_url.Contains("http"))
                        {
                            Monitor.Enter(locks);
                            try
                            {
                                test.FindElement(By.CssSelector(Model.Profile.Change_Photo)).Click();
                                var tempFilePath = Path.GetTempFileName();
                                using (var client = new WebClient())
                                {
                                    client.DownloadFile(profile.profile_pic_url, tempFilePath);
                                }
                                Support.InsertIntoFileDialog(tempFilePath);
                                Thread.Sleep(1000);
                                SendKeys.SendWait(@"{Enter}");
                                test.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                                test.FindElement(By.CssSelector("#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-c2g4g1.epiie927 > div > div > div > div > div.css-pz8hbm.eyrfdut1 > button")).Click();
                                Delay(5);
                                File.Delete(tempFilePath);
                            }
                            finally
                            {
                                Monitor.Exit(locks);
                            }
                        }

                        test.FindElement(By.CssSelector("#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-c2g4g1.epiie927 > div > div > form > div.css-gjfrgk.e2kuma43 > button")).Click();
                        Delay(5);
                        Update_Label(lbl_test, "Còn lại: " + takiAccounts.Where(r => r.topic == null || r.topic == "").Count() + " chưa xử lý");
                        run.topic = profile.username;
                        FileIO.CreateFile(run);
                        test.Close();
                        test.Quit();
                        string json = JsonConvert.SerializeObject(tweet_Profiles, Formatting.Indented);
                        FileIO.Create_File_From_Json(json, "Taki/username.json");
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.StackTrace, "btn_profile_upgrade_Click Error");
                        test.Quit();
                        goto Get_Profile;
                    }
                }
            });
            Chrome.Name = "Thread Upgrade...";
            Chrome.SetApartmentState(ApartmentState.STA);
            Chrome.Start();
        }

        private void btn_get_follow_Click(object sender, EventArgs e)
        {
        Get_Again:
            Update_Label(lbl_test, "Lấy thông tin tài khoản ...");
            var client = new RestClient("https://twitter154.p.rapidapi.com/user/details?username=" + txt_twitter.Text);
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-RapidAPI-Key", "3b7b1d0cadmsh3840d5495663fb0p1c9477jsn6e545ce8c03a");
            request.AddHeader("X-RapidAPI-Host", "twitter154.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            JObject jObject = JObject.Parse(response.Content);
            string user_id = (string)jObject["user_id"];
            var client2 = new RestClient("https://twitter154.p.rapidapi.com/user/following?user_id=" + user_id + "&limit=20");
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("X-RapidAPI-Key", "3b7b1d0cadmsh3840d5495663fb0p1c9477jsn6e545ce8c03a");
            request2.AddHeader("X-RapidAPI-Host", "twitter154.p.rapidapi.com");
            IRestResponse response2 = client2.Execute(request2);
            if (response2.IsSuccessful)
            {
                List<Tweet_Profile> profiles = JsonConvert.DeserializeObject<RootProfile>(response2.Content)?.results;
                if (profiles.Count == 0)
                {
                    MessageBox.Show("Get thất bại !!!");
                    return;
                }
                foreach (var s in profiles)
                {
                    string username_ok = s.username;
                    if (takiAccounts.Where(r => r.topic == username_ok).Any() == false && s.number_of_tweets > 100 && tweet_Profiles.Where(r => r.username == username_ok).Any() == false)
                    {
                        var client3 = new RestClient("https://api.taki.app/user/public-data?username=" + username_ok.ToLower().Replace("_", ""));
                        var request3 = new RestRequest(Method.GET);
                        IRestResponse response3 = client3.Execute(request3);

                        if (response3.StatusCode == HttpStatusCode.BadRequest)
                        {
                            s.profile_pic_url = s.profile_pic_url.Replace("_normal", "");
                            tweet_Profiles.Add(s);
                        }
                    }
                }
            }
            else
            {
                Delay(5);
                goto Get_Again;
            }

            string json = JsonConvert.SerializeObject(tweet_Profiles, Formatting.Indented);
            FileIO.Create_File_From_Json(json, "Taki/username.json");
            MessageBox.Show("Đang có: " + tweet_Profiles.Count + " đang trống !!!");
        }

        private void btn_raw_withdraw_Click(object sender, EventArgs e)
        {
            string raw = "";
            foreach (var s in takiAccounts.Where(r => r.isWithdraw == true))
            {
                raw += s.name.ToLower() + "\n";
            }
            MessageBox.Show("Đang có: " + takiAccounts.Where(r => r.isWithdraw == true).Count() + "\nDanh sách:\n" + raw, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_get_profile_Click(object sender, EventArgs e)
        {
            Thread Chrome = new Thread(() =>
            {
                foreach (var row in lst_account.SelectedRows)
                {
                    string name = row.Cells["username"].Value.ToString();
                    TakiAccount taki = takiAccounts.Where(r => r.name == name).FirstOrDefault();
                    Update_Label(lbl_test, "Đang get thông tin: " + taki.name.ToLower());
                    TakiData data = TakiHelper.Check_Profile(taki.name.ToLower());
                    if (data != null)
                    {
                        taki.apr = data.coin?.APR;
                        taki.price = data.coin?.value;
                        taki.cosmeticTierName = data.userData.cosmeticTierName;
                        taki.paidLikesEarnings = data.paidLikesEarnings;
                        FileIO.CreateFile(taki);
                        if (taki.cosmeticTierName == "Silver")
                        {
                            foreach (GridViewCellInfo cell in row.Cells)
                            {
                                cell.Style.CustomizeFill = true;
                                cell.Style.DrawFill = true;
                                if (taki.withdraw == true && taki.withdraw_day == false)
                                {
                                    cell.Style.BackColor = ColorTranslator.FromHtml("#28a745");
                                }
                                else
                                {
                                    cell.Style.BackColor = Color.Silver;
                                }
                            }
                        }
                        else if (taki.cosmeticTierName == "Super Silver")
                        {
                            foreach (GridViewCellInfo cell in row.Cells)
                            {
                                cell.Style.CustomizeFill = true;
                                cell.Style.DrawFill = true;
                                if (taki.withdraw == true && taki.withdraw_day == false)
                                {
                                    cell.Style.BackColor = ColorTranslator.FromHtml("#28a745");
                                }
                                else
                                {
                                    cell.Style.BackColor = ColorTranslator.FromHtml("#ffffb3");
                                }
                            }
                        }
                        else if (taki.cosmeticTierName == "Gold")
                        {
                            foreach (GridViewCellInfo cell in row.Cells)
                            {
                                cell.Style.CustomizeFill = true;
                                cell.Style.DrawFill = true;
                                if (taki.withdraw == true && taki.withdraw_day == false)
                                {
                                    cell.Style.BackColor = ColorTranslator.FromHtml("#28a745");
                                }
                                else
                                {
                                    cell.Style.BackColor = ColorTranslator.FromHtml("#ffff00");
                                }
                            }
                        }
                    }

                    row.Update("apr", taki.apr.Value.ToString("#.##") + "%");
                    row.Update("price", taki.price.Value.ToString("#.##"));
                    row.Update("earn", taki.paidLikesEarnings.ToString());
                    row.Update("rank_name", taki.cosmeticTierName);
                }
                Update_Label(lbl_test, "Hoàn thành !!!");
                //DialogResult result = MessageBox.Show("Đã hoàn tất lấy thông tin\nBạn có muốn load lại không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (DialogResult.Yes == result)
                //{
                //    if (lst_account.InvokeRequired)
                //    {
                //        lst_account.Invoke(new MethodInvoker(() => { this.lst_account.Rows.Clear(); }));
                //    }
                //    else
                //    {
                //        this.lst_account.Rows.Clear();
                //    }

                //    LoadAccount();
                //}
            });
            Chrome.Start();
        }

        private void panel_top_Paint(object sender, PaintEventArgs e)
        {

        }

        private void reduceCPU_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var chromeDriverProcesses = Process.GetProcesses().Where(pr => pr.ProcessName == "software_reporter_tool");

            foreach (var process in chromeDriverProcesses)
            {
                try
                {
                    process.Kill();
                }
                catch { }
            }
        }

        private void reduceCPU_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Complete CPU...");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (flgCPU == true)
            {
                reduceCPU.RunWorkerAsync();
            }
        }

        private void ChangeSize()
        {
            // Xữ lý khung Form
            panel_top.Visible = false;
            panel_top.Location = new Point(0, 0);
            group_Tạo_mới.Visible = false;
            group_Tạo_mới.Location = new Point(0, 0);
            radGroupBox1.Dock = DockStyle.Fill;
            btn_Đường_dẫn.Visible = false;
            label_path.Visible = false;
            lbl_amount.Visible = false;
            lbl_ref.Visible = false;
            lbl_withdraw_amoun.Visible = false;
            btn_test_proxy.Visible = false;
            radLabel8.Visible = false;
            txt_hold.Visible = false;
            radLabel13.Visible = false;
            lbl_earning.Visible = false;
            // Xữ lý Tab
            tab.Dock = DockStyle.Fill;
            btn_get_profile.Visible = false;
            btn_sort_hold.Visible = false;
            btn_sort_point.Visible = false;
            btn_reset_mine.Visible = false;
            radButton1.Visible = false;
            tab.Padding = new Point(3, 10);
            lst_running.MasterTemplate.Columns[2].Width = 250;
            // Xữ lý thanh ngang
            panel_config.Visible = false;
            panel_config.Location = new Point(0, 0);
        }

        public void ResetSize()
        {
            // Xữ lý khung Form
            panel_top.Visible = true;
            panel_top.Location = new Point(0, 0);
            group_Tạo_mới.Visible = true;
            group_Tạo_mới.Location = new Point(0, 63);
            radGroupBox1.Dock = DockStyle.None;
            btn_Đường_dẫn.Visible = true;
            label_path.Visible = true;
            lbl_amount.Visible = true;
            lbl_ref.Visible = true;
            lbl_withdraw_amoun.Visible = true;
            btn_test_proxy.Visible = true;
            radLabel8.Visible = false;
            txt_hold.Visible = false;
            radLabel13.Visible = false;
            lbl_earning.Visible = false;
            // Xữ lý Tab
            tab.Dock = DockStyle.None;
            btn_get_profile.Visible = true;
            btn_sort_hold.Visible = true;
            btn_sort_point.Visible = true;
            btn_reset_mine.Visible = true;
            radButton1.Visible = true;
            tab.Padding = new Point(3, 3);
            lst_running.MasterTemplate.Columns[2].Width = 650;
            // Xữ lý thanh ngang
            panel_config.Visible = true;
            panel_config.Location = new Point(2, 18);
        }

        private void frmTaki_Resize(object sender, EventArgs e)
        {
            //Control control = (Control)sender;
            //if (control.Size.Width < 495)
            //{
            //    ChangeSize();
            //}
            //else
            //{
            //    ResetSize();
            //}
        }

        private void btn_merge_Click(object sender, EventArgs e)
        {
            Thread Chrome = new Thread(() =>
            {
                int i = 0;
                foreach (var s in frmTaki.receive)
                {
                    i++;
                    TakiData data = TakiHelper.Check_Profile(s.name.ToLower());
                    s.beforePaidLikesEarnings = s.paidLikesEarnings;
                    s.paidLikesEarnings = data.paidLikesEarnings;
                    if (s.paidLikesEarnings > s.beforePaidLikesEarnings)
                    {
                        s.receive = true;
                    }
                    FileIO.CreateFile(s);
                    Update_Label(lbl_test, "Đang xữ lý: " + i + "/" + frmTaki.receive.Count);
                }
                Update_Label(lbl_test, "Đang xữ lý hoàn thành !!!");
                MessageBox.Show("Đã xữ lý hoàn tất !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
            Chrome.Start();

        }

        private void btn_save_advanced_Click(object sender, EventArgs e)
        {
            try
            {
                if (setting.advanced == null)
                {
                    setting.advanced = new Advanced();
                }

                setting.advanced.max_apr_user = int.Parse(s_max_apr_user.Text);
                setting.advanced.max_follow_user = int.Parse(s_max_follow_user.Text);
                setting.advanced.max_give_post_user = int.Parse(s_max_give_post_user.Text);
                setting.advanced.max_give_user = int.Parse(s_max_give_user.Text);
                setting.advanced.min_follow_user = int.Parse(s_min_follow_user.Text);
                setting.advanced.give_withdraw = switch_give.Value;

                string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
                string pathConfig = "Taki\\config.json";
                FileIO.Create_File_From_Json(json, pathConfig);
                MessageBox.Show("Đã cập nhật các thông số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_default_Click(object sender, EventArgs e)
        {
            s_max_give_user.Text = "800";
            s_max_follow_user.Text = "1000";
            s_max_apr_user.Text = "5000";
            s_max_give_post_user.Text = "3";
            s_min_follow_user.Text = "10";
            s_max_user_proxy.Text = "5";
            switch_give.Value = true;

            setting.advanced.max_apr_user = int.Parse(s_max_apr_user.Text);
            setting.advanced.max_follow_user = int.Parse(s_max_follow_user.Text);
            setting.advanced.max_give_post_user = int.Parse(s_max_give_post_user.Text);
            setting.advanced.max_give_user = int.Parse(s_max_give_user.Text);
            setting.advanced.min_follow_user = int.Parse(s_min_follow_user.Text);
            setting.advanced.max_user_proxy = int.Parse(s_max_user_proxy.Text);
            setting.advanced.give_withdraw = true;

            string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
            string pathConfig = "Taki\\config.json";
            FileIO.Create_File_From_Json(json, pathConfig);
            MessageBox.Show("Đã cập nhật các thông số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ckb_show_live_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            RadCheckBox check = (RadCheckBox)sender;
            if (check.Checked)
            {
                LoadAccount(true);
            }
            else
            {
                LoadAccount(false);
            }
        }

        private void btn_reset_mission_Click(object sender, EventArgs e)
        {
            foreach (var Taki in takiAccounts)
            {
                Taki.complete = false;
                FileIO.CreateFile(Taki);
            }
            MessageBox.Show("Đã cập nhật thành công !!");
        }

        private void ran_agent_Click(object sender, EventArgs e)
        {
            txt_agent.Text = Support.GetRandomUserAgent();
        }

        private void btnOpen_Check_Click(object sender, EventArgs e)
        {
            GridViewRowInfo row = lst_account.SelectedRows.First();
            string name = row.Cells[1].Value.ToString();
            TakiAccount taki = takiAccounts.Where(r => r.name == name).First();
            Thread Chrome = new Thread(() =>
            {
                string dir = "Taki\\VMS\\VmsRuning\\Default\\IndexedDB\\https_taki.app_0.indexeddb.leveldb";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                else
                {
                    Directory.Delete(dir, true);
                    Directory.CreateDirectory(dir);
                }
                string source = "Taki\\Profile\\" + name + "\\https_taki.app_0.indexeddb.leveldb";

                new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(source, dir);


                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = false;
                ChromeOptions option = new ChromeOptions();
                option.AddArgument("window-size=414,896");
                option.AddExcludedArgument("enable-automation");
                option.AddAdditionalOption("useAutomationExtension", false);
                option.AddArgument("--autoplay-policy=no-user-gesture-required");
                option.AddArgument("--mute-audio");
                option.AddArgument("no-sandbox");
                option.AddArgument("--disable-infobars");
                option.AddArgument("--disable-default-apps");
                option.AddArgument("--disable-gpu");
                option.AddArgument("--FontRenderHinting[none]");
                option.AddArgument("--disable-blink-features=AutomationControlled");
                option.AddArgument("--app=https://iphey.com/");
                string path = Directory.GetCurrentDirectory();
                option.AddArgument("user-data-dir=" + path + "\\Taki\\VMS\\VmsRuning");
                if (string.IsNullOrEmpty(taki.userAgent) == true)
                    taki.userAgent = Support.GetRandomUserAgent();
                FileIO.CreateFile(taki);
                taki.userAgent = "Mozilla/5.0 (Linux; Android 12; M2007J3SG) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Mobile Safari/537.36";
                option.AddArgument("--user-agent=" + taki.userAgent);
                if (string.IsNullOrEmpty(taki.proxy) == false)
                {
                    option.AddArgument("--proxy-server=" + taki.proxy);
                }
                test = new ChromeDriver(driverService, option);
            });
            Chrome.Name = "Thread Test: " + name;
            Chrome.SetApartmentState(ApartmentState.STA);
            Chrome.Start();
        }
    }
}
