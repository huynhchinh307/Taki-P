
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Telerik.WinControls.UI;
using ProjectAuto.Common;
using System.Diagnostics;
using TMProxyHelper;
using ViOTP;
using System.Windows;
using System.Windows.Forms;
using OpenQA.Selenium.Support.UI;
using System.Management;
using System.Runtime.InteropServices;
using AutoIt;
using System.Net;
using NLog;
using System.Net.Sockets;
using OpenQA.Selenium.Chrome.ChromeDriverExtensions;


namespace ProjectAuto.Taki
{
    public static class Support
    {
        static Model model = new Model();
        public static object locks = new object();
        private static Logger log = LogManager.GetCurrentClassLogger();


        public static string getUsername()
        {
            string[] lines = File.ReadAllLines(@"Taki\username.txt");
            Random rd = new Random();
            int id = rd.Next(lines.Count());
            return lines[id];
        }

        public static Tao_Giao_Dich GetNumber()
        {
            RestClient restClient = new RestClient("http://api.codesim.net/api/CodeSim/DangKy_GiaoDich?apikey=" + frmTaki.setting.basic.api_sms + "&dichvu_id=119&so_sms_nhan=1");
            RestRequest request = new RestRequest(Method.GET);
            var response = restClient.Execute(request);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Tao_Giao_Dich>(response.Content);
            }
            else
            {
                return null;
            }
        }

        public static Tao_Giao_Dich GetNumber(string api)
        {
            RestClient restClient = new RestClient("http://api.codesim.net/api/CodeSim/DangKy_GiaoDich?apikey=" + api + "&dichvu_id=119&so_sms_nhan=1");
            RestRequest request = new RestRequest(Method.GET);
            var response = restClient.Execute(request);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Tao_Giao_Dich>(response.Content);
            }
            else
            {
                return null;
            }
        }

        public static void Huỷ_Giao_Dịch(int giaodich_id, string api)
        {
            RestClient restClient = new RestClient("http://api.codesim.net/api/CodeSim/HuyGiaoDich?apikey=" + api + "&giaodich_id=" + giaodich_id);
            RestRequest request = new RestRequest(Method.GET);
            var response = restClient.Execute(request);
            if (response.IsSuccessful)
            {
                return;
            }
            else
            {
                return;
            }
        }

        public static string GetCode(int giaodich_id)
        {
            RestClient restClient = new RestClient("http://api.codesim.net/api/CodeSim/KiemTraGiaoDich?apikey=" + frmTaki.setting.basic.api_sms + "&giaodich_id=" + giaodich_id);
            RestRequest request = new RestRequest(Method.GET);
            var response = restClient.Execute(request);
            if (response.IsSuccessful)
            {
                Check_Giao_Dich check = JsonConvert.DeserializeObject<Check_Giao_Dich>(response.Content);
                return check.data.listSms.FirstOrDefault()?.number;
            }
            else
            {
                return null;
            }
        }

        public static string GetCode(int giaodich_id, string api)
        {
            RestClient restClient = new RestClient("http://api.codesim.net/api/CodeSim/KiemTraGiaoDich?apikey=" + api + "&giaodich_id=" + giaodich_id);
            RestRequest request = new RestRequest(Method.GET);
            var response = restClient.Execute(request);
            if (response.IsSuccessful)
            {
                Check_Giao_Dich check = JsonConvert.DeserializeObject<Check_Giao_Dich>(response.Content);
                return check.data.listSms.FirstOrDefault()?.number;
            }
            else
            {
                return null;
            }
        }

        public static void Setting_Captcha(ChromeDriver driver, bool auto = true)
        {
            try
            {
                string urlCurrent = driver.Url;
                if (urlCurrent.Contains("chrome-extension"))
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                    IWebElement api_key = driver.FindElement(By.CssSelector(Model.Captcha.Input_API));
                    if (auto == true)
                    {
                        string js = "let config_captcha = {}; config_captcha['autoSubmitForms'] = true; config_captcha['autoSolveRecaptchaV2'] = true; config_captcha['autoSolveInvisibleRecaptchaV2'] = false; config_captcha['autoSolveRecaptchaV3'] = false; Config.set(config_captcha);";
                        string config = (string)driver.ExecuteScript(js);
                        api_key.Clear();
                        api_key.SendKeys(frmTaki.setting.basic.api_captcha);

                        driver.FindElement(By.CssSelector(Model.Captcha.Input)).Click();
                        Delay(5);
                        var alert = driver.SwitchTo().Alert();
                        alert.Accept();
                    }
                    else
                    {
                        string js = "let config_captcha = {}; config_captcha['autoSubmitForms'] = true; config_captcha['autoSolveRecaptchaV2'] = false; config_captcha['autoSolveInvisibleRecaptchaV2'] = false; config_captcha['autoSolveRecaptchaV3'] = false; Config.set(config_captcha);";
                        string config = (string)driver.ExecuteScript(js);
                        api_key.Clear();
                        Monitor.Enter(locks);
                        try
                        {
                            api_key.SendKeys(frmTaki.setting.basic.api_captcha);
                        }
                        finally
                        {
                            Monitor.Exit(locks);
                        }

                        driver.FindElement(By.CssSelector(Model.Captcha.Input)).Click();
                        Delay(5);
                        var alert = driver.SwitchTo().Alert();
                        alert.Accept();
                    }

                    driver.Close();
                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                    //log.Info("Đã cấu hình captcha: " + frmTaki.setting.api_captcha);
                }
                else
                {
                    string current = driver.CurrentWindowHandle;
                    List<string> pages = driver.WindowHandles.ToList();
                    pages.Remove(current);
                    driver.SwitchTo().Window(pages[0]);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                    IWebElement api_key = driver.FindElement(By.CssSelector(Model.Captcha.Input_API));
                    string js = "let config_captcha = {}; config_captcha['autoSubmitForms'] = true; config_captcha['autoSolveRecaptchaV2'] = true; config_captcha['autoSolveInvisibleRecaptchaV2'] = false; config_captcha['autoSolveRecaptchaV3'] = false; Config.set(config_captcha);";
                    string config = (string)driver.ExecuteScript(js);
                    api_key.Clear();
                    Monitor.Enter(locks);
                    try
                    {
                        api_key.SendKeys(frmTaki.setting.basic.api_captcha);
                    }
                    finally
                    {
                        Monitor.Exit(locks);
                    }

                    driver.FindElement(By.CssSelector(Model.Captcha.Input)).Click();
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
                    wait.Until(drv => IsAlertShown(drv));
                    var alert = driver.SwitchTo().Alert();
                    alert.Accept();
                    driver.Close();
                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                    //log.Info("Đã cấu hình captcha: " + frmTaki.setting.api_captcha);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace, "Setting_Captcha Error");
            }
        }

        public static bool IsAlertShown(IWebDriver driver)
        {
            try
            {
                driver.SwitchTo().Alert();
            }
            catch
            {
                Console.WriteLine("Wait Alert");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Chức năng lấy link invite
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static string Lấy_Invite(this ChromeDriver driver)
        {
        Goo:
            try
            {
                driver.Navigate().GoToUrl("https://taki.app/invites/");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.Manage().Window.Size = new Size(614, 896);
                driver.FindElement(By.CssSelector(Model.Get_Invite.Copy_Button)).Click();
                string link = null;
                Thread STAThread = new Thread(
                delegate ()
                {
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            link = System.Windows.Clipboard.GetText(System.Windows.TextDataFormat.Text);
                            break;
                        }
                        catch { }
                        Thread.Sleep(10);
                    }

                });
                STAThread.SetApartmentState(ApartmentState.STA);
                STAThread.Start();
                STAThread.Join();
                driver.Manage().Window.Size = new Size(414, 896);
                driver.Navigate().GoToUrl("https://taki.app/");
                return link;
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace, "Lấy_Invite Error");
                goto Goo;
            }
        }


        /// <summary>
        /// Chức năng mua coin
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="taki_username"></param>
        /// <param name="row"></param>
        public static decimal Buy_Taki(this ChromeDriver driver, string coin_name, decimal coin, GridViewRowInfo row)
        {
            try
            {
                int check = 0;
            Buy:
                if (check >= 10)
                {
                    return coin;
                }

                List<IWebElement> coins = driver.FindElements(By.CssSelector(Model.Walletq.Coin)).ToList();
                List<IWebElement> menus = driver.FindElements(By.CssSelector(Model.Walletq.Menu)).ToList();
                for (int i = 0; i < coins.Count; i++)
                {
                    string name = coins[i].FindElement(By.CssSelector(Model.Walletq.Coin_Name)).GetAttribute("innerHTML");
                    if (name == coin_name.ToLower())
                    {
                        driver.ExecuteScript("arguments[0].click();", menus[i]);
                        break;
                    }
                }

                int intCoin = Convert.ToInt32(coin);
                int final = intCoin - 5;
                if (final == 0)
                {
                    row.Update_Status("Mua đã hoàn thành!!!");
                    return coin;
                }
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.FindElement(By.CssSelector(Model.Walletq.Buy)).Click();
                driver.FindElement(By.CssSelector(Model.Walletq.Buy_Amount)).Clear();
                driver.FindElement(By.CssSelector(Model.Walletq.Buy_Amount)).SendKeys(final.ToString());
                Delay(3);
                driver.FindElement(By.XPath(Model.Walletq.OK)).Click();
                try
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    driver.FindElement(By.CssSelector(Model.Walletq.OKK)).Click();
                }
                catch
                {
                    check++;
                    row.Update_Status("Mua đã bị lỗi !!!");
                    Delay(2);
                    row.Update_Status("Đang đợi thử lại !!!");
                    Delay(10);
                    goto Buy;
                }
                return (coin - 5);
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace, "Buy_Taki Error");
                return coin;
            }

        }



        public static List<TakiAccount> Tạo_RefAsync(string link, bool isRef, CProxy proxy, GridViewRowInfo row, RadTextBox txt_balance)
        {
            ChromeSetting s = new ChromeSetting();
            try
            {

                string user_content = File.ReadAllText("Taki\\username.json");
                List<TakiAccount> lst = new List<TakiAccount>();
                string profile_name;
                int số_ref = 0;
                bool keep = isRef;
                string username;
            Tạo:
                Tweet_Profile profile_t = new Tweet_Profile();

                lock (frmTaki.tweet_Profiles)
                {
                Get_Name:
                    if (frmTaki.tweet_Profiles.Count == 0)
                    {
                        row.Update_Status("Đã hêt user_id !!!");
                        return lst;
                    }
                    profile_t = frmTaki.tweet_Profiles.First();
                    frmTaki.tweet_Profiles.Remove(profile_t);
                    bool check = frmTaki.Instance.takiRunings.Where(r => r.topic == profile_t.username).Any();
                    if (check == true)
                    {
                        goto Get_Name;
                    }

                    RestClient restClient = new RestClient("https://api.taki.app/user/public-data?username=" + profile_t.username.ToLower().Replace("_", ""));
                    RestRequest request_name = new RestRequest(Method.GET);
                    var response = restClient.Execute(request_name);
                    if (response.StatusCode != HttpStatusCode.BadRequest)
                    {
                        goto Get_Name;
                    }

                    if(profile_t.username.ToLower().Replace("_", "").Length < 5)
                    {
                        goto Get_Name;
                    }
                }
                profile_name = profile_t.username.ToLower().Replace("_", "");
                username = profile_name;

                string profile = frmTaki.setting.basic.path_profile + @"\" + profile_name;

                if (Directory.Exists(profile) == true)
                {
                    goto Tạo;
                }



                if (số_ref == 3)
                {
                    row.Update_Status("Đã tạo ref thành công !!!");
                    return lst;
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
                string agent = GetRandomUserAgent();
                option.AddArgument("--user-agent=" + agent);
                option.AddArgument("--disable-blink-features=AutomationControlled");
                option.AddArgument("--app=" + link);
                //option.AddExtension("Taki\\callback.crx");
                option.AddExtension("Taki\\captcha.crx");
                option.AddHttpProxy(proxy.ip, proxy.port, proxy.username, proxy.password);
                //option.AddArgument("--window-position=-2000,0");

                ChromeDriver driver = new ChromeDriver(driverService, option, TimeSpan.FromSeconds(180));
                Support.Setting_Captcha(driver);
                lock (frmTaki.points)
                {
                    s = frmTaki.points.Where(r => r.Active == false).First();
                    driver.Manage().Window.Position = new Point(s.X, s.Y);
                    s.Active = true;
                }
                Delay(1);
                string url = driver.Url;
                if (url == "https://taki.app/")
                {
                    s.Active = false;
                    row.Update_Status("Đã hết lượt giới thiệu !!!");
                    driver.Close();
                    driver.Quit();
                    // Xoá Profile
                    if (Directory.Exists(profile) == true)
                    {
                        Directory.Delete(profile, true);
                    }

                    return lst;
                }
                int checkPhone = 0;
                ResponsePhone phone = null;
            Renew_Tạo:
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(25);
                driver.FindElement(By.CssSelector(Model.Tạo_Tài_Khoản.Accept_Invite)).Click();
                List<IWebElement> install_app = driver.FindElements(By.CssSelector("#__next > div.css-137m7xd.elfvif112 > div.css-1iz5yzz.elfvif111 > div.css-segaof.elfvif15 > div.noise.css-1ykx51n.e1ibz0dg5 > button")).ToList();
                if (install_app.Count != 0)
                {
                    install_app.First().Click();
                }
                row.Update_Status("Đang tạo ref " + (số_ref + 1) + "/3");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            Get_Code:
                install_app = driver.FindElements(By.CssSelector("#__next > div.css-137m7xd.elfvif112 > div.css-1iz5yzz.elfvif111 > div.css-segaof.elfvif15 > div.noise.css-1ykx51n.e1ibz0dg5 > button")).ToList();
                if (install_app.Count != 0)
                {
                    install_app.First().Click();
                }
                List<IWebElement> accept = driver.FindElements(By.CssSelector(Model.Tạo_Tài_Khoản.Continue_with_Phone)).ToList();
                if (accept.Count == 0)
                {
                    goto Renew_Tạo;
                }
                else
                {
                    driver.ExecuteScript("arguments[0].click();", accept.First());
                }
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Choose_Country)).Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Choose_VN)).Click();
                row.Update_Status("Đang lấy số điện thoại...");

                // Thêm xử lý sử dung lại số điện thoại cũ, tối đa 3 lân
                if (phone?.data.phone_number == null)
                {
                    phone = ViOTPHelper.Get_New_Phone(frmTaki.setting.basic.api_sms);
                }

                if (phone.data?.phone_number != null)
                {
                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
                    string balanceVnd = phone.data.balance.ToString("#,###", cul.NumberFormat);
                    txt_balance.Text = balanceVnd + "đ";

                    string sdt = phone?.data.phone_number;
                    driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Input_SDT)).SendKeys(sdt);
                    row.Update_Status("Lấy được SDT: " + phone?.data.phone_number);
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
                            string solver = captcha_invisable.GetAttribute("data-state");
                            int time = 0;
                            while (solver == "ready" && time < 120)
                            {
                                row.Update_Status("Đang đợi giải captcha: " + (time - 120) * -1 + "s");
                                Delay(2);
                                time += 2;
                                solver = captcha_invisable.GetAttribute("data-state");
                            }
                            driver.Navigate().GoToUrl("chrome://settings/clearBrowserData");
                            try
                            {
                                Delay(2);
                                string urlCurrent = driver.Url;
                                driver.Manage().Cookies.DeleteAllCookies();
                                if (urlCurrent.Contains("chrome://settings/clearBrowserData"))
                                {
                                    IJavaScriptExecutor executer = (IJavaScriptExecutor)driver;
                                    string buttonCssScript = "return document.querySelector('settings-ui').shadowRoot.querySelector('settings-main').shadowRoot.querySelector('settings-basic-page').shadowRoot.querySelector('settings-section > settings-privacy-page').shadowRoot.querySelector('settings-clear-browsing-data-dialog').shadowRoot.querySelector('#clearBrowsingDataDialog').querySelector('#clearBrowsingDataConfirm')";
                                    IWebElement clearButton = (IWebElement)executer.ExecuteScript(buttonCssScript);
                                    clearButton.Click();
                                }
                                else
                                {
                                    string current = driver.CurrentWindowHandle;
                                    List<string> pages = driver.WindowHandles.ToList();
                                    pages.Remove(current);
                                    driver.SwitchTo().Window(pages[0]);
                                    IJavaScriptExecutor executer = (IJavaScriptExecutor)driver;
                                    string buttonCssScript = "return document.querySelector('settings-ui').shadowRoot.querySelector('settings-main').shadowRoot.querySelector('settings-basic-page').shadowRoot.querySelector('settings-section > settings-privacy-page').shadowRoot.querySelector('settings-clear-browsing-data-dialog').shadowRoot.querySelector('#clearBrowsingDataDialog').querySelector('#clearBrowsingDataConfirm')";
                                    IWebElement clearButton = (IWebElement)executer.ExecuteScript(buttonCssScript);
                                    clearButton.Click();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                                Console.WriteLine("Error Setting...");
                            }
                            driver.Navigate().GoToUrl(link);
                            checkPhone++;
                            if (checkPhone >= 3)
                            {
                                s.Active = false;
                                phone = null;
                                checkPhone = 0;
                                driver.Close();
                                driver.Quit();
                                // Xoá Profile
                                if (Directory.Exists(profile) == true)
                                {
                                    Directory.Delete(profile, true);
                                }
                                goto Renew_Tạo;
                            }
                            goto Renew_Tạo;

                        }
                        catch
                        {
                        }


                    }


                    int check_sms = 0;
                    row.Update_Status("Đang lấy codesim...");
                Get_SMS:
                    Delay(5);
                    ResponseCode code_SMS = ViOTPHelper.Get_Code_Phone(frmTaki.setting.basic.api_sms, phone.data.request_id);
                    if (code_SMS?.data.Code == null)
                    {
                        check_sms++;
                        if (check_sms == 25)
                        {
                            row.Update_Status("Code: Không lấy được code");
                            driver.FindElement(By.XPath(model.Back_Get_Code)).Click();
                            phone = null;
                            driver.Navigate().Refresh();
                            goto Get_Code;
                        }
                        goto Get_SMS;
                    }
                    else
                    {
                        row.Update_Status("Lấy code thành công...");
                        driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Input_Code_SMS)).SendKeys(code_SMS?.data.Code);
                        driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Continue_Code_SMS)).Click();
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    Nhập_Tên:
                        // Check Install app
                        List<IWebElement> install_app1 = driver.FindElements(By.CssSelector("#__next > div.css-137m7xd.elfvif112 > div.css-1iz5yzz.elfvif111 > div.css-segaof.elfvif15 > div.noise.css-1ykx51n.e1ibz0dg5 > button")).ToList();
                        if (install_app1.Count != 0)
                        {
                            install_app1.First().Click();
                        }
                        IWebElement input = driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Input_Username));
                        input.Clear();
                        input.SendKeys(username);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                        driver.FindElement(By.XPath(Model.Tạo_Tài_Khoản.Continue_Username)).Click();

                        // Giải Captcha
                        #region Giải captcha
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                        List<IWebElement> captcha2 = driver.FindElements(By.CssSelector(Model.Captcha.captcha_solver)).ToList();
                        if (captcha2.Count > 0)
                        {
                        Try_Resovler:
                            string solver = captcha2.First().GetAttribute("data-state");
                            while (solver != "solved" && solver != "error")
                            {
                                row.Update_Status("Đang giải captcha");
                                Delay(1);
                                try
                                {
                                    solver = captcha2.First().GetAttribute("data-state");

                                }
                                catch (StaleElementReferenceException)
                                {
                                    break;
                                }
                            }
                            if (solver == "error")
                            {
                                captcha2.First().Click();
                                Delay(1);
                                goto Try_Resovler;
                            }
                            row.Update_Status("Đã giải captcha");
                        }
                        else
                        {
                            row.Update_Status("Đã giải captcha");

                        }
                        stopwatch.Stop();
                        TimeSpan ts = stopwatch.Elapsed;
                        row.Update_Status("Giải captcha thành công: " + ts.Seconds + " s");
                        Delay(1);
                        #endregion
                        try
                        {
                            Delay(5);
                            IWebElement validate = driver.FindElement(By.CssSelector("div.css-1l75fk7.emn2ld31"));
                            if (validate.GetAttribute("innerHTML") == "That username is already taken 😔. Try another" || validate.GetAttribute("innerHTML") == "Username does not follow the rules")
                            {
                                row.Update_Status("Tên không hợp lệ...");
                                lock (frmTaki.tweet_Profiles)
                                {
                                Get_Name:
                                    if (frmTaki.tweet_Profiles.Count == 0)
                                    {
                                        s.Active = false;
                                        row.Update_Status("Đã hêt user_id !!!");
                                        driver.Close();
                                        driver.Quit();
                                        // Xoá Profile
                                        if (Directory.Exists(profile) == true)
                                        {
                                            Directory.Delete(profile, true);
                                        }

                                        return lst;
                                    }
                                    profile_t = frmTaki.tweet_Profiles.First();
                                    frmTaki.tweet_Profiles.Remove(profile_t);
                                    bool check = frmTaki.Instance.takiRunings.Where(r => r.topic == profile_t.username).Any();
                                    if (check == true)
                                    {
                                        goto Get_Name;
                                    }

                                    // Check rule
                                    RestClient restClient = new RestClient("https://api.taki.app/user/public-data?username=" + profile_t.username.ToLower().Replace("_", ""));
                                    RestRequest request_name = new RestRequest(Method.GET);
                                    var response = restClient.Execute(request_name);
                                    if (response.StatusCode != HttpStatusCode.BadRequest)
                                    {
                                        goto Get_Name;
                                    }
                                }
                                username = profile_t.username.ToLower().Replace("_", "");
                                goto Nhập_Tên;
                            }
                        }
                        catch
                        {
                            row.Update_Status("Tên OK...");
                        }
                    Solona:
                        row.Update_Status("Tạo wallet...");
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
                        List<IWebElement> create_my_coin = driver.FindElements(By.XPath(model.Create_My_Coin)).ToList();
                        if (create_my_coin.Count == 0)
                        {
                            row.Update_Status("Đang đợi tạo wallet...");
                            driver.Navigate().Refresh();
                            // Check Install app
                            List<IWebElement> install_app2 = driver.FindElements(By.CssSelector("#__next > div.css-137m7xd.elfvif112 > div.css-1iz5yzz.elfvif111 > div.css-segaof.elfvif15 > div.noise.css-1ykx51n.e1ibz0dg5 > button")).ToList();
                            if (install_app2.Count != 0)
                            {
                                install_app2.First().Click();
                            }
                            lock (frmTaki.tweet_Profiles)
                            {
                            Get_Name:
                                if (frmTaki.tweet_Profiles.Count == 0)
                                {
                                    s.Active = false;
                                    row.Update_Status("Đã hêt user_id !!!");
                                    driver.Close();
                                    driver.Quit();
                                    // Xoá Profile
                                    if (Directory.Exists(profile) == true)
                                    {
                                        Directory.Delete(profile, true);
                                    }
                                    return lst;
                                }
                                profile_t = frmTaki.tweet_Profiles.First();
                                frmTaki.tweet_Profiles.Remove(profile_t);
                                bool check = frmTaki.Instance.takiRunings.Where(r => r.topic == profile_t.username).Any();
                                if (check == true)
                                {
                                    goto Get_Name;
                                }

                                // Check rule
                                RestClient restClient = new RestClient("https://api.taki.app/user/public-data?username=" + profile_t.username.ToLower().Replace("_", ""));
                                RestRequest request_name = new RestRequest(Method.GET);
                                var response = restClient.Execute(request_name);
                                if (response.StatusCode != HttpStatusCode.BadRequest)
                                {
                                    goto Get_Name;
                                }
                            }
                            username = profile_t.username.ToLower().Replace("_", "");
                            goto Nhập_Tên;
                        }
                        else
                        {
                            create_my_coin.First().Click();
                        }

                        Delay(10);
                        row.Update_Status("Đang chờ tạo...");
                        List<IWebElement> Goto_Wallet = driver.FindElements(By.XPath(model.Goto_Wallet)).ToList();
                        if (Goto_Wallet.Count == 0)
                        {
                            row.Update_Status("Lỗi mạng solona...");
                            driver.Navigate().Refresh();
                            goto Solona;
                        }
                        else
                        {
                            Goto_Wallet.First().Click();
                        }
                        Delay(5);
                        row.Update_Status("Tạo thành công ref " + (số_ref + 1) + "/3");
                        if (keep == true)
                        {
                            //taki.proxy = proxy;
                            row.Update_Status("Tạo số TAKI...");
                            TakiAccount taki = new TakiAccount();
                            taki.name = username;
                            taki.isWithdraw = false;
                            taki.isRef = true;
                            taki.topic = profile_t.username;
                            taki.userAgent = agent;
                            taki.isLive = true;
                            taki.taki = 3;
                            row.Update_Status("Tạo link invite...");
                            taki.invite_link = Lấy_Invite(driver);
                            driver.Navigate().GoToUrl("https://taki.app/u/" + username + "/");
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                            driver.ExecuteScript("arguments[0].click();", driver.FindElement(By.CssSelector(Model.Profile.Edit_Profile)));
                            IWebElement input_name = driver.FindElement(By.CssSelector("#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-c2g4g1.epiie927 > div > div > form > div:nth-child(3) > div > input"));
                            input_name.Clear();
                            if (string.IsNullOrEmpty(profile_t.name) == false)
                            {
                                Thread STAThread = new Thread(
                                       delegate ()
                                       {
                                           for (int i = 0; i < 10; i++)
                                           {
                                               try
                                               {
                                                   System.Windows.Clipboard.SetText(profile_t.name);
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
                            IWebElement input_bio = driver.FindElement(By.CssSelector("#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-c2g4g1.epiie927 > div > div > form > div:nth-child(5) > div > textarea"));
                            input_bio.Clear();

                            if (string.IsNullOrEmpty(profile_t.description) == false)
                            {
                                Thread STAThread = new Thread(
                                       delegate ()
                                       {
                                           for (int i = 0; i < 10; i++)
                                           {
                                               try
                                               {
                                                   System.Windows.Clipboard.SetText(profile_t.description);
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

                            if (string.IsNullOrEmpty(profile_t.profile_pic_url) == false && profile_t.profile_pic_url.Contains("http"))
                            {
                                lock (locks)
                                {
                                    driver.FindElement(By.CssSelector(Model.Profile.Change_Photo)).Click();
                                    var tempFilePath = Path.GetTempFileName();

                                    try
                                    {
                                        using (var client = new WebClient())
                                        {
                                            client.DownloadFile(profile_t.profile_pic_url, tempFilePath);
                                        }
                                        Support.InsertIntoFileDialog(tempFilePath);
                                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                                        driver.FindElement(By.CssSelector("#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-c2g4g1.epiie927 > div > div > div > div > div.css-pz8hbm.eyrfdut1 > button")).Click();
                                        Delay(2);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex.StackTrace, "Downloading Error");
                                    }
                                    File.Delete(tempFilePath);
                                }
                            }
                            driver.FindElement(By.CssSelector("#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-c2g4g1.epiie927 > div > div > form > div.css-gjfrgk.e2kuma43 > button")).Click();
                            Delay(5);
                            taki.SaveDate = DateTime.Now;
                            row.Update_Status("Tạo file hoàn thành...");
                            Delay(1);
                            lst.Add(taki);
                            Delay(2);
                            row.Update_Status("Tạo thành công ref " + (số_ref + 1) + "/3");
                            s.Active = false;
                            driver.Close();
                            driver.Quit();
                            FileIO.CreateFile(taki);

                            string dir_name = "Taki\\Profile\\" + taki.name;

                            if (!Directory.Exists(dir_name))
                            {
                                Directory.CreateDirectory(dir_name);
                            }

                            string path = Directory.GetCurrentDirectory();
                            string source = profile + "\\Default\\IndexedDB\\https_taki.app_0.indexeddb.leveldb\\";
                            string dir = path + "\\" + dir_name + "\\https_taki.app_0.indexeddb.leveldb\\";

                            Move(source, dir);

                            Directory.Delete(profile, true);

                            số_ref++;
                            goto Tạo;
                        }
                        else
                        {
                            s.Active = false;
                            driver.Close();
                            driver.Quit();
                            // Xoá Profile
                            if (Directory.Exists(profile) == true)
                            {
                                Directory.Delete(profile, true);
                            }
                            row.Update_Status("Đã xoá account vừa tạo");
                        }

                        số_ref++;
                        goto Tạo;
                    }
                }
                else
                {
                    s.Active = false;
                    if (phone.message == "Số dư quý khách không đủ !")
                    {
                        row.Update_Status("Số dư quý khách không đủ !");
                        driver.Close();
                        driver.Quit();
                        // Xoá Profile
                        if (Directory.Exists(profile) == true)
                        {
                            Directory.Delete(profile, true);
                        }
                        return lst;
                    }
                    //row.Update_Status("Code: " + phone.message);
                    Delay(60);
                    goto Renew_Tạo;
                }
            }
            catch (Exception ex)
            {
                s.Active = false;
                log.Error(ex.StackTrace, "Tạo_RefAsync Error");
                row.Update_Color(Color.Red);
                return null;
            }

        }

        public static void Withdraw(ChromeDriver driver, string wallet, GridViewRowInfo row)
        {
            driver.Navigate().GoToUrl(Model.Withdraw.Url);
        }

        public static bool PingHost(string strIP, int intPort)
        {
            bool blProxy = false;
            try
            {
                TcpClient client = new TcpClient(strIP, intPort);

                blProxy = true;
            }
            catch
            {
                MessageBox.Show("Error pinging host:'" + strIP + ":" + intPort.ToString() + "'");
                return false;
            }
            return blProxy;
        }

        #region Chức năng delay
        public static void Delay(int delay)
        {
            while (delay > 0)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                delay--;
            }
        }
        #endregion

        public static void Update_Status(this GridViewRowInfo row, string msg)
        {
            try
            {
                if (frmTaki.Instance.GetRunning.InvokeRequired)
                {
                    frmTaki.Instance.GetRunning.Invoke(new MethodInvoker(() =>
                    {
                        frmTaki.Instance.ResumeLayout();
                        if (row.Cells["status"] != null)
                        {
                            row.Cells["status"].Value = msg;
                        }
                        frmTaki.Instance.Update();
                    }));
                }
                else
                {
                    frmTaki.Instance.ResumeLayout();
                    if (row.Cells["status"] != null)
                    {
                        row.Cells["status"].Value = msg;
                    }
                    frmTaki.Instance.Update();
                }
            }
            catch { }
        }

        public static void Update_Color(this GridViewRowInfo row, Color color)
        {
            if (frmTaki.Instance.GetRunning.InvokeRequired)
            {
                frmTaki.Instance.GetRunning.Invoke(new MethodInvoker(() =>
                {
                    frmTaki.Instance.lst_running.BeginUpdate();
                    frmTaki.Instance.lst_account.BeginUpdate();
                    foreach (GridViewCellInfo cell in row.Cells)
                    {
                        cell.Style.CustomizeFill = true;
                        cell.Style.DrawFill = true;
                        cell.Style.BackColor = color;
                    }
                    frmTaki.Instance.lst_running.EndUpdate();
                    frmTaki.Instance.lst_account.BeginUpdate();
                }));
            }
            else
            {
                frmTaki.Instance.lst_running.BeginUpdate();
                frmTaki.Instance.lst_account.BeginUpdate();
                foreach (GridViewCellInfo cell in row.Cells)
                {
                    cell.Style.CustomizeFill = true;
                    cell.Style.DrawFill = true;
                    cell.Style.BackColor = color;
                }
                frmTaki.Instance.lst_running.EndUpdate();
                frmTaki.Instance.lst_account.BeginUpdate();
            }
        }

        public static void Update(this GridViewRowInfo row, string name, string msg)
        {
            if (frmTaki.Instance.GetRunning.InvokeRequired)
            {
                frmTaki.Instance.GetRunning.Invoke(new MethodInvoker(() =>
                {
                    row.Cells[name].Value = msg;
                }));
            }
            else
            {
                row.Cells[name].Value = msg;
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

        public static void Click_First(this ChromeDriver driver, string cssSelector)
        {

        }

        public static void Add_View(this frmViewTaki view, string name, int processId)
        {
            try
            {
                if (view.InvokeRequired)
                {
                    view.Invoke(new MethodInvoker(() =>
                    {
                        Process p = GetWindowHandleByDriverId(processId);
                        IntPtr windowHandle = p.MainWindowHandle;
                        RadTabbedFormControlTab chrome = new RadTabbedFormControlTab();
                        chrome.Size = new Size(414, 923);
                        chrome.Text = name;
                        Panel panel1 = new Panel();
                        panel1.Padding = Padding.Empty;
                        panel1.Dock = DockStyle.Fill;
                        panel1.Size = new Size(414, 923);
                        SetParent(p.MainWindowHandle, panel1.Handle);
                        MoveWindow(p.MainWindowHandle, 0, 0, panel1.Width, panel1.Height, true);
                        chrome.Controls.Add(panel1);
                        view.Tab.Tabs.Add(chrome);
                        view.Tab.SelectedTab = chrome;
                    }));
                }
                else
                {
                    Process p = GetWindowHandleByDriverId(processId);
                    IntPtr windowHandle = p.MainWindowHandle;
                    RadTabbedFormControlTab chrome = new RadTabbedFormControlTab();
                    chrome.Size = new Size(414, 923);
                    chrome.Text = name;
                    Panel panel1 = new Panel();
                    panel1.Padding = Padding.Empty;
                    panel1.Dock = DockStyle.Fill;
                    panel1.Size = new Size(414, 923);
                    SetParent(p.MainWindowHandle, panel1.Handle);
                    MoveWindow(p.MainWindowHandle, 0, 0, panel1.Width, panel1.Height, true);
                    chrome.Controls.Add(panel1);
                    view.Tab.Tabs.Add(chrome);
                    view.Tab.SelectedTab = chrome;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace, "AddView Error");
            }
        }

        public static Process GetWindowHandleByDriverId(int driverId)
        {
            var processes = Process.GetProcessesByName("chrome")
                .Where(_ => !_.MainWindowHandle.Equals(IntPtr.Zero));
            foreach (var process in processes)
            {
                var parentId = GetParentProcess(process.Id);
                if (parentId == driverId)
                {
                    return process;
                }

            }
            return null;
        }

        private static int GetParentProcess(int Id)
        {
            int parentPid = 0;
            using (ManagementObject mo = new ManagementObject($"win32_process.handle='{Id}'"))
            {
                mo.Get();
                parentPid = Convert.ToInt32(mo["ParentProcessId"]);
            }
            return parentPid;
        }

        public static void InsertIntoFileDialog(string file, int timeout = 10)
        {
            int aiDialogHandle = AutoItX.WinWaitActive("Open", "", timeout); // adjust string as you need
            if (aiDialogHandle <= 0)
            {
                Console.WriteLine("Can't find file dialog.");
            }
            AutoItX.Send(file);
            Thread.Sleep(500);
            AutoItX.Send("{ENTER}");
            Thread.Sleep(500);
        }


        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);


        public static void Move(string source, string target)
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

            sourceInfo.Delete(true);
        }

        public static string GetRandomUserAgent()
        {
            Random rand = new Random();
            return frmTaki.user_agent[rand.Next(frmTaki.user_agent.Count)];
        }
    }

    public class Data_Giao_Dich
    {
        public string phoneNumber { get; set; }
        public int id_giaodich { get; set; }
        public int dichvu_gia { get; set; }
        public string dichvu_ten { get; set; }
    }

    public class Tao_Giao_Dich
    {
        public Data_Giao_Dich data { get; set; }
        public int stt { get; set; }
        public string msg { get; set; }
    }

    public class Data
    {
        [JsonProperty("$id")]
        public string Id { get; set; }
        public List<ListSm> listSms { get; set; }
        public int giaodich_id { get; set; }
        public int dichvu_dongia { get; set; }
        public string dichvu_ten { get; set; }
        public string phoneNum { get; set; }
        public int totalPrice { get; set; }
        public int number_sms { get; set; }
        public string createDate { get; set; }
        public int status { get; set; }
    }

    public class ListSm
    {
        public string sender { get; set; }
        public string number { get; set; }
        public int id { get; set; }
        public string smsContent { get; set; }
        public string fileRecord { get; set; }
        public string phoneNumber { get; set; }
        public string receiveTimeText { get; set; }
    }

    public class Check_Giao_Dich
    {
        public int stt { get; set; }
        public object msg { get; set; }
        public Data data { get; set; }
    }


    public class Item
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public Item(string id, string description)
        {
            this.Id = id;
            this.Description = description;
        }
    }


}
