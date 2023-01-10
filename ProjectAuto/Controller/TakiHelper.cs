using Newtonsoft.Json;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ProjectAuto.Common;
using ProjectAuto.Taki;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ProjectAuto
{
    public static class TakiHelper
    {
        private static readonly System.Object MyLock = new System.Object();
        private static Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Chức năng Give Taki
        /// </summary>
        /// <param name="driver">Thiết bị điều khiển</param>
        /// <param name="ref_name">Tên tài khoản dùng để give taki</param>
        /// <param name="taki_current">Số TAKI đang có</param>
        /// <param name="row">row để update record</param>
        /// <param name="repeat">Số lần give taki</param>
        public static int Give_Taki(ChromeDriver driver, string ref_name, decimal taki_current, GridViewRowInfo row, int repeat = 1)
        {
            int count = 0;
            try
            {
                string url;
                if (string.IsNullOrEmpty(ref_name) == true)
                {
                    url = "https://taki.app/u/fujinet/";
                }
                else
                {
                    url = ref_name;
                }

                if (url.Contains("http") == false)
                {
                    url = "https://taki.app/u/" + url + "/";
                }

                driver.Navigate().GoToUrl(url);
                row.Update_Status("Đang Give Taki: " + ref_name);
                Delay(3);

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
                    else if (postE.Count == 5)
                    {
                        postE[1].Click();
                    }

                    IWebElement search = driver.FindElement(By.CssSelector("#__next > div.css-qx5r5a.ej2twmx21 > div > div > div > div > div.css-16my424.eu192g63 > div > div > input"));
                    string target = ref_name.Split('/')[4];
                    int check = 0;
                    search.SendKeys(target);
                Get_Target:
                    Delay(5);
                    List<IWebElement> targets = driver.FindElements(By.CssSelector("#__next > div.css-qx5r5a.ej2twmx21 > div > div > div > div.css-10obac9.eu192g64 > div > div > table > tbody:nth-child(3) > tr > td:nth-child(2) > div")).ToList();
                    if (check == 5)
                    {
                        return 0;
                    }

                    if (targets.Count == 0)
                    {
                        search.SendKeys(OpenQA.Selenium.Keys.Backspace);
                        check++;
                        goto Get_Target;
                    }
                    else
                    {
                        row.Update_Status("Lấy được: " + targets.Count());
                        bool flg = false;
                        foreach (var s in targets)
                        {
                            string name_target = s.FindElement(By.CssSelector("strong")).GetAttribute("innerHTML");
                            if (name_target == target)
                            {
                                s.Click();
                                flg = true;
                                break;
                            }
                        }
                        if (flg == false)
                        {
                            search.SendKeys(OpenQA.Selenium.Keys.Backspace);
                            check++;
                            goto Get_Target;
                        }
                    }

                }

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                driver.ExecuteScript("arguments[0].click();", driver.FindElement(By.CssSelector(Model.Give_Taki.Content)));
                row.Update_Status("Đang lấy bài viết");
                Delay(1);
                List<IWebElement> lstGive = driver.FindElements(By.CssSelector(Model.Give_Taki.GiveTaki)).ToList();
                //lstGive = lstGive.OrderBy(a => Guid.NewGuid()).ToList();
                for (int i = 0; i < lstGive.Count; i++)
                {
                    bool isGive = lstGive[i].GetAttribute("opacity") == "1";
                    int num = 0;
                    IWebElement give_content = lstGive[i].GetParent().GetParent().GetParent().GetParent().GetParent().GetParent().GetParent();
                    List<IWebElement> lstGive_Number = give_content.FindElements(By.CssSelector(Model.Give_Taki.Give_Number)).ToList();
                    string day = give_content.FindElement(By.CssSelector("div.css-tjo912.e1l1al3h21 > span.css-101u24n.e1l1al3h20")).GetAttribute("innerHTML");
                    if (day == "7 days ago" || day == "7 days ago (edited<span class=\"css-anmptw e1l1al3h19\">a day ago</span>)" ||
                        day == "8 days ago" || day == "8 days ago (edited<span class=\"css-anmptw e1l1al3h19\">a day ago</span>)" ||
                        day == "9 days ago" || day == "9 days ago (edited<span class=\"css-anmptw e1l1al3h19\">a day ago</span>)" ||
                        day == "10 days ago" || day == "10 days ago (edited<span class=\"css-anmptw e1l1al3h19\">a day ago</span>)" ||
                        day == "11 days ago" || day == "11 days ago (edited<span class=\"css-anmptw e1l1al3h19\">a day ago</span>)" ||
                        day == "12 days ago" || day == "12 days ago (edited<span class=\"css-anmptw e1l1al3h19\">a day ago</span>)" ||
                        day == "13 days ago" || day == "13 days ago (edited<span class=\"css-anmptw e1l1al3h19\">a day ago</span>)"
                        )
                    {
                        break;
                    }
                    if (lstGive_Number.Count != 0)
                    {
                        string num_s = lstGive_Number[0].GetAttribute("innerHTML");
                        num_s = num_s.Remove(num_s.Length - 11, 11);
                        num = int.Parse(num_s);
                    }

                    Random random = new Random();
                    int next = random.Next(95, 150);
                    if (isGive == true && taki_current >= 2 && num < next)
                    {
                    Give_Check:
                        scrollToElement(driver, lstGive[i]);
                        IWebElement give = lstGive[i].GetParent().GetParent();
                        driver.ExecuteScript("arguments[0].click();", give);
                        try
                        {
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                            driver.FindElement(By.CssSelector(Model.Give_Taki.confirm)).Click();
                            driver.FindElement(By.CssSelector(Model.Give_Taki.confirm_pay)).Click();
                            row.Update_Status("Đã Give Taki");
                            Delay(5);
                        }
                        catch
                        {
                            row.Update_Status("Đã Give Taki");
                            Delay(5);
                        }

                        //if (i == 0)
                        //{
                        //    scrollToElement(driver, driver.FindElement(By.CssSelector("#content-wrapper > div.css-c9cvz2.e56mmy050 > div")));
                        //}
                        //else
                        //{
                        //    scrollToElement(driver, lstGive[i - 1]);
                        //}
                        scrollToElement(driver, driver.FindElement(By.CssSelector("#content-wrapper > div.css-c9cvz2.e56mmy050 > div")));
                        isGive = driver.FindElements(By.CssSelector(Model.Give_Taki.GiveTaki)).ToList()[i].GetAttribute("opacity") == "1";
                        if (isGive == false)
                        {
                            count++;
                            row.Update_Status("Đã give bài: " + day);
                        }
                        else
                        {
                            goto Give_Check;
                        }

                        if (count == repeat)
                        {
                            break;
                        }

                        taki_current -= 2;
                    }
                }
                if (count == 0)
                {
                    row.Update_Status("Số hết bài viết có thể Give Taki: " + count);
                    if (ref_name.Contains("https"))
                    {
                        string name = ref_name.Split('/')[4];
                        TakiAccount taki = frmTaki.receive.Where(r => r.name.ToLower() == name).FirstOrDefault();
                        if (taki != null)
                        {
                            taki.error = "Hết bài viết !!!";
                            FileIO.CreateFile(taki);
                        }
                    }
                    else
                    {
                        TakiAccount taki = frmTaki.receive.Where(r => r.name.ToLower() == ref_name).FirstOrDefault();
                        if (taki != null)
                        {
                            taki.error = "Hết bài viết !!!";
                            FileIO.CreateFile(taki);
                        }
                    }
                }
                else
                {
                    if (ref_name.Contains("https"))
                    {
                        string name = ref_name.Split('/')[4];
                        TakiAccount taki = frmTaki.receive.Where(r => r.name.ToLower() == name).FirstOrDefault();
                        if (taki != null)
                        {
                            taki.error = "";
                            FileIO.CreateFile(taki);
                        }
                    }
                    else
                    {
                        TakiAccount taki = frmTaki.receive.Where(r => r.name.ToLower() == ref_name).FirstOrDefault();
                        if (taki != null)
                        {
                            taki.error = "";
                            FileIO.CreateFile(taki);
                        }
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace, "Give_Taki Error");
                return count;
            }
        }

        /// <summary>
        /// Chức năng Auto Give Taki
        /// </summary>
        /// <param name="driver">Thiết bị điều khiển</param>
        /// <param name="ref_name">Tên tài khoản dùng để give taki</param>
        /// <param name="taki_current">Số TAKI đang có</param>
        /// <param name="row">row để update record</param>
        /// <param name="repeat">Số lần give taki</param>
        public static bool Auto_Give_Taki(ChromeDriver driver, string[] list, GridViewRowInfo row, int repeat = 1, bool max = false, decimal taki_amount = 0)
        {
            string name_current = row.Cells["username"].Value.ToString();
            name_current = "https://taki.app/u/" + name_current.ToLower() + "/";
            int give = 0;
            try
            {
                for (int i = 0; i < repeat; i++)
                {
                Get_Give_Taki:
                    TakiAccount taki = null;
                    Random random = new Random();
                    List<TakiAccount> receive = new List<TakiAccount>();
                    receive = frmTaki.receive.Where(r => r.receive == false && r.complete == true && r.isConnect == false && r.isLive == true).ToList();
                    int next = 0;
                    string name = "";
                    if (receive.Count == 0)
                    {
                        if (max == false)
                        {
                            break;
                        }
                        next = random.Next(0, list.Length - 1);
                        name = list[next];
                    }
                    else
                    {
                        next = random.Next(0, receive.Count - 1);
                        name = receive[next].name;
                        receive[next].isConnect = true;
                        name = "https://taki.app/u/" + name + "/";
                        string name_s = name.Substring(19);
                        name_s = name_s.Remove(name_s.Length - 1, 1);
                        taki = frmTaki.receive.Where(r => r.name == name_s).FirstOrDefault();
                        frmTaki.receive.Remove(receive[next]);
                    }

                    if (name.ToLower() == name_current)
                    {
                        goto Get_Give_Taki;
                    }

                    TakiData data = Check_Profile(name.ToLower());
                    if (data?.coin?.APR > frmTaki.setting.advanced.max_apr_user || data?.paidLikesEarnings > frmTaki.setting.advanced.max_give_user)
                    {
                        goto Get_Give_Taki;
                    }


                    if (max == true)
                    {
                        int rep = frmTaki.receive.Where(r => r.receive == false && r.complete == true && r.isConnect == false && r.isLive == true).Any() ? 1 : frmTaki.setting.advanced.max_give_post_user;

                        int check = Give_Taki(driver, name.ToLower(), taki_amount, row, rep);
                        if (rep == 1)
                        {
                            receive[next].isConnect = false;
                        }

                        if (check == 0)
                        {
                            goto Get_Give_Taki;
                        }
                        else
                        {
                            i += check;
                            taki_amount -= 2 * check;

                            if (taki != null && taki.receive == false)
                            {
                                taki.receive = true;
                                FileIO.CreateFile(taki);
                            }
                            give += check;
                            row.Update_Status("Đã thực hiện Give: " + give + " lần");
                        }
                    }
                    else
                    {
                        int check = Give_Taki(driver, name.ToLower(), 2, row);
                        receive[next].isConnect = false;
                        if (check == 0)
                        {
                            goto Get_Give_Taki;
                        }
                        else
                        {
                            if (taki != null && taki.receive == false)
                            {
                                taki.receive = true;
                                FileIO.CreateFile(taki);
                            }
                            give++;
                            row.Update_Status("Đã thực hiện Give: " + give + " lần");
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace, "Auto_Give_Taki Error");
                return false;
            }
        }


        /// <summary>
        /// Chức năng follow
        /// </summary>
        /// <param name="driver">Thiết bị điều khiển</param>
        /// <param name="coin_name">Link coin_name</param>
        /// <param name="row">row để update record</param>
        /// <returns></returns>
        public static bool Follow(ChromeDriver driver, string coin_name, GridViewRowInfo row)
        {
            try
            {
                row.Update_Status("Theo dõi: " + coin_name);
                string url = coin_name;
                bool flg = false;
                driver.Navigate().GoToUrl(url);
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
                    else if (postE.Count == 5)
                    {
                        postE[1].Click();
                    }

                    IWebElement search = driver.FindElement(By.CssSelector("#__next > div.css-qx5r5a.ej2twmx21 > div > div > div > div > div.css-16my424.eu192g63 > div > div > input"));
                    string target = url.Split('/')[4];
                    int check = 0;
                    search.SendKeys(target);
                Get_Target:
                    Delay(5);
                    List<IWebElement> targets = driver.FindElements(By.CssSelector("#__next > div.css-qx5r5a.ej2twmx21 > div > div > div > div.css-10obac9.eu192g64 > div > div > table > tbody:nth-child(3) > tr > td:nth-child(2) > div")).ToList();
                    if (check == 5)
                    {
                        return false;
                    }

                    if (targets.Count == 0)
                    {
                        search.SendKeys(OpenQA.Selenium.Keys.Backspace);
                        check++;
                        goto Get_Target;
                    }
                    else
                    {
                        row.Update_Status("Lấy được: " + targets.Count());
                        bool flgg = false;
                        foreach (var s in targets)
                        {
                            string name_target = s.FindElement(By.CssSelector("strong")).GetAttribute("innerHTML");
                            if (name_target == target)
                            {
                                s.Click();
                                flgg = true;
                                break;
                            }
                        }
                        if (flgg == false)
                        {
                            search.SendKeys(OpenQA.Selenium.Keys.Backspace);
                            check++;
                            goto Get_Target;
                        }
                    }

                }
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                int checkError = 0;
            Kiểm_tra:
                if (checkError >= 5)
                {
                    return false;
                }
                Delay(1);
                List<IWebElement> elements = driver.FindElements(By.CssSelector(Model.Nhiệm_Vụ.is_Follow)).ToList();
                if (elements.Count == 0)
                {
                    row.Update_Status("Lỗi link !!!");
                    return false;
                }
                Delay(1);
                string is_Follow = elements.First().GetAttribute("innerHTML");
                if (is_Follow == "Following")
                {
                    if (flg == true)
                    {
                        row.Update_Status("Thành công: " + coin_name);
                        Delay(1);
                    }
                    return flg;
                }
                else
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    // Click follow
                    IWebElement element = driver.FindElement(By.CssSelector(Model.Nhiệm_Vụ.Follow));
                    driver.ExecuteScript("arguments[0].click();", element);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                    elements = driver.FindElements(By.CssSelector("div.css-myk9g6.ec6v7f3 > div > div > div")).ToList();
                    if (elements.Count == 0)
                    {
                        try
                        {
                            List<IWebElement> rloo = driver.FindElements(By.CssSelector(Model.Nhiệm_Vụ.confirm)).ToList();
                            if (rloo.Count == 0)
                            {
                                driver.Navigate().Refresh();
                                goto Kiểm_tra;
                            }

                            driver.FindElement(By.CssSelector(Model.Nhiệm_Vụ.confirm)).Click();
                            driver.FindElement(By.CssSelector(Model.Nhiệm_Vụ.confirm_pay)).Click();
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                            IWebElement msg = driver.FindElement(By.CssSelector("div.css-myk9g6.ec6v7f3 > div > div > div"));
                            if (msg.GetAttribute("innerHTML").Contains("You started following") == true || msg.GetAttribute("innerHTML").Contains("Your request is") == true)
                            {
                                flg = true;
                                row.Update_Status("Thành công: " + coin_name);
                                return flg;
                            }
                            else
                            {
                                checkError++;
                                row.Update_Status("Theo dõi lỗi !!!");
                                Delay(5);
                                driver.Navigate().Refresh();
                                goto Kiểm_tra;
                            }
                        }
                        catch
                        {
                            return flg;
                        }
                    }
                    else
                    {
                        if (elements.First().GetAttribute("innerHTML").Contains("You started following") == true || elements.First().GetAttribute("innerHTML").Contains("Your request is") == true)
                        {
                            flg = true;
                            row.Update_Status("Thành công: " + coin_name);
                            return flg;
                        }
                        else
                        {
                            checkError++;
                            row.Update_Status("Theo dõi lỗi !!!");
                            Delay(5);
                            driver.Navigate().Refresh();
                            goto Kiểm_tra;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace, "Follow Error");
                return false;
            }

        }

        /// <summary>
        /// Chức năng tạo bài viết
        /// </summary>
        /// <param name="driver">Thiết bị điều khiển</param>
        /// <param name="post_msg">Nội dung bài viết</param>
        /// <param name="row">row để update record</param>
        public static bool Create_Post(ChromeDriver driver, string post_msg, string url, GridViewRowInfo row)
        {
            try
            {
                row.Update_Status("Đang tạo bài viết...");
            Tạo_bài_viết:
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                List<IWebElement> post = driver.FindElements(By.CssSelector(Model.Create_Post.Button_Create)).ToList();
                if (post.Count == 0)
                {
                    driver.Navigate().Refresh();
                    Delay(5);
                    goto Tạo_bài_viết;
                }
                else if (post.Count == 5)
                {
                    post[2].Click();
                }
                else
                {
                    driver.Navigate().Refresh();
                    row.Update_Status("Lỗi không thể đăng bài viết !!!");
                    goto Tạo_bài_viết;
                }
                //driver.FindElement(By.CssSelector(Model.Create_Post.Button_Create)).Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                row.Update_Status("Tạo nôi dung...");
                driver.FindElement(By.CssSelector(Model.Create_Post.Input_Post)).Click();

                lock (MyLock)
                {
                    if (string.IsNullOrEmpty(post_msg) == false)
                    {
                        Thread STAThread = new Thread(
                               delegate ()
                               {
                                   for (int i = 0; i < 10; i++)
                                   {
                                       try
                                       {
                                           Clipboard.SetText(post_msg);
                                           break;
                                       }
                                       catch { }
                                       Thread.Sleep(10);
                                   }
                               });
                        STAThread.SetApartmentState(ApartmentState.STA);
                        STAThread.Start();
                        STAThread.Join();
                        driver.FindElement(By.CssSelector(Model.Create_Post.Input_Post)).SendKeys(OpenQA.Selenium.Keys.Control + "v");
                    }
                    if (string.IsNullOrEmpty(url) == false)
                    {
                        var tempFilePath = Path.GetTempFileName();
                        bool flgDown = false;
                        try
                        {
                            using (var client = new WebClient())
                            {
                                client.DownloadFile(url, tempFilePath);
                            }
                            flgDown = true;
                        }
                        catch
                        {
                            flgDown = false;
                        }

                        if (flgDown == true)
                        {
                            byte[] bytes;
                            using (FileStream file = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
                            {
                                bytes = new byte[file.Length];
                                file.Read(bytes, 0, (int)file.Length);
                            }

                            Thread STAThread = new Thread(
                                   delegate ()
                                   {
                                       using (MemoryStream ms = new MemoryStream(bytes))
                                       {
                                           Bitmap bm = new Bitmap(ms);
                                           for (int i = 0; i < 10; i++)
                                           {
                                               try
                                               {
                                                   Clipboard.SetImage(bm);
                                                   break;
                                               }
                                               catch { }
                                               Thread.Sleep(10);
                                           }
                                       }
                                   });
                            STAThread.SetApartmentState(ApartmentState.STA);
                            STAThread.Start();
                            STAThread.Join();
                            driver.FindElement(By.CssSelector(Model.Create_Post.Input_Post)).SendKeys(OpenQA.Selenium.Keys.Control + "v");
                        }

                        File.Delete(tempFilePath);
                    }
                }

                Delay(5);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.FindElement(By.CssSelector(Model.Create_Post.Submit_Post)).Click();
                row.Update_Status("Post nôi dung...");
                try
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
                    IWebElement bug = driver.FindElement(By.CssSelector(Model.Create_Post.msg));
                    // Phát hiên lỗi captcha
                    // Xữ lý
                    driver.Navigate().GoToUrl("https://taki.app/home/");
                    try
                    {
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
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
                        goto Tạo_bài_viết;
                    }
                    catch
                    {
                        row.Update_Status("Đã xử lý captcha...");
                        goto Tạo_bài_viết;
                    }
                }
                catch
                {
                    Delay(10);
                    row.Update_Status("Tạo bài viết thành công...");
                    Delay(2);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace, "Create_Post Error");
                driver.Navigate().GoToUrl("https://taki.app/home/");
                return false;
            }

        }

        /// <summary>
        /// Chức năng tạo bài viết
        /// </summary>
        /// <param name="driver">Thiết bị điều khiển</param>
        /// <param name="post_msg">Nội dung bài viết</param>
        /// <param name="row">row để update record</param>
        public static void Create_Post(ChromeDriver driver, GridViewRowInfo row)
        {
            row.Update_Status("Đang tạo bài viết...");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.FindElement(By.CssSelector(Model.Create_Post.Button_Create)).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            row.Update_Status("Tạo nôi dung...");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.FindElement(By.CssSelector(Model.Create_Post.Submit_Post)).Click();
            row.Update_Status("Post nôi dung...");
            Delay(10);
            row.Update_Status("Tạo bài viết thành công...");
        }

        public static Complete Run_Mission(ChromeDriver driver, bool follow, GridViewRowInfo row)
        {
            try
            {
                Complete Complete = new Complete();
                Complete.receive = true;
                string name = row.Cells["username"].Value.ToString();
                List<IWebElement> elements = null;
            MINE:
                row.Update_Status("Đang lấy nhiệm vụ còn lại ...");
                // Kiểm tra đã hoàn thành tất cả nhiệm vụ chưa?
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(25);
                elements = driver.FindElements(By.CssSelector(Model.Nhiệm_Vụ.Button_Nhiệm_Vụ)).ToList();
                if (elements.Count == 0)
                {
                    driver.Navigate().GoToUrl("https://taki.app/");
                    goto MINE;
                }
                else
                {
                    // Click button nhiệm vụ
                    driver.ExecuteScript("arguments[0].click();", elements.First());
                }
                Delay(2);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                List<IWebElement> lst_nhiệm_vụ = driver.FindElements(By.CssSelector(Model.Nhiệm_Vụ.Nhiệm_Vụ_Còn)).ToList();
                if (lst_nhiệm_vụ.Count == 0)
                {
                    row.Update_Status("Đã hoàn thành tất cả nhiệm vụ...");
                    Complete.isComplete = true;
                    return Complete;
                }

                row.Update_Status("Đã phát hiện: " + lst_nhiệm_vụ.Count + " nhiệm vụ");

                bool Browse_the_Top_Feed = false;
                int Follow_someone_new = 0;
                int Give_a_Gold_Taki = 0;
                //bool Receive_a_Gold_Taki = false;
                bool Visit_a_user = false;
                bool Browse_your_feed = false;
                bool Stake = false;
                int i = 0;
                foreach (var nhiệm_vụ in lst_nhiệm_vụ)
                {
                    string raw_nhiệm_vụ = nhiệm_vụ.GetAttribute("innerHTML");
                    if (raw_nhiệm_vụ == "Follow someone new")
                    {
                        string progress = driver.FindElements(By.CssSelector("text.CircularProgressbar-text")).ToList()[i].GetAttribute("innerHTML");
                        Follow_someone_new = int.Parse(progress.Substring(progress.Length - 1));
                    }

                    if (raw_nhiệm_vụ == "Give a \"Gold Taki\"")
                    {
                        string progress = driver.FindElements(By.CssSelector("text.CircularProgressbar-text")).ToList()[i].GetAttribute("innerHTML");
                        Give_a_Gold_Taki = int.Parse(progress.Substring(progress.Length - 1));
                    }

                    if (raw_nhiệm_vụ == "Receive a \"Gold Taki\"")
                    {
                        //Receive_a_Gold_Taki = true;
                        Complete.receive = false;
                    }

                    if (raw_nhiệm_vụ == "Visit a user's profile")
                    {
                        Visit_a_user = true;
                    }

                    if (raw_nhiệm_vụ == "Browse your feed")
                    {
                        Browse_your_feed = true;
                    }

                    if (raw_nhiệm_vụ == "Browse the Top feed")
                    {
                        Browse_the_Top_Feed = true;
                    }

                    if (raw_nhiệm_vụ == "Create a stake or check the status of your staked $TAKI")
                    {
                        Stake = true;
                    }

                    if (raw_nhiệm_vụ.Contains("div"))
                    {
                        i--;
                        driver.FindElement(By.CssSelector(Model.Nhiệm_Vụ.Nv_Link)).Click();
                        var tabs = driver.WindowHandles;
                        if (tabs.Count > 1)
                        {
                            driver.SwitchTo().Window(tabs[1]);
                            driver.Close();
                            driver.SwitchTo().Window(tabs[0]);
                        }
                        row.Update_Status("Complete: Tweet your profile");
                        Delay(5);
                    }
                    i++;
                }

                driver.Navigate().GoToUrl("https://taki.app/home/");

                // Xữ lý nhiệm vụ Browse the Top feed
                if (Browse_the_Top_Feed == true)
                {
                Browse_the_Top_Feed:
                    row.Update_Status("Browse the Top feed");
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
                    elements = driver.FindElements(By.CssSelector(Model.Nhiệm_Vụ.TOP)).ToList();
                    if (elements.Count == 0)
                    {
                        row.Update_Status("Kết nối may chủ không ổn đinh...");
                        driver.Navigate().GoToUrl("https://taki.app/home/");
                        Delay(10);
                        goto Browse_the_Top_Feed;
                    }
                    else
                    {
                        driver.ExecuteScript("arguments[0].click();", elements.First());
                        row.Update_Status("Complete: Browse the Top feed");
                        Delay(3);
                    }
                }

                // Xữ lý nhiệm vụ Browse your feed
                if (Browse_your_feed == true)
                {
                Browse_your_feed:
                    row.Update_Status("Run: Browse your feed");
                    elements = driver.FindElements(By.CssSelector(Model.Nhiệm_Vụ.My_Feed)).ToList();
                    if (elements.Count == 0)
                    {
                        row.Update_Status("Kết nối may chủ không ổn đinh...");
                        driver.Navigate().GoToUrl("https://taki.app/home/");
                        Delay(10);
                        goto Browse_your_feed;
                    }
                    else
                    {
                        elements.First().Click();
                        row.Update_Status("Complete: Browse your feed");
                        Delay(3);
                    }
                }

                Wallet wallet = Lấy_Taki(driver, Stake);
                decimal số_coin = wallet.taki;


                decimal need = 0M;
                if (Follow_someone_new != 0)
                {
                    need += 0.5M* Follow_someone_new;
                }

                if (Give_a_Gold_Taki != 0)
                {
                    need += 2* Give_a_Gold_Taki;
                }

                if (số_coin >= need)
                {
                    if (Follow_someone_new != 0)
                    {
                            driver.Navigate().GoToUrl("https://taki.app/u/" + name.ToLower() + "/");
                            driver.FindElement(By.CssSelector("#content-wrapper > div.css-c9cvz2.e56mmy050 > div > div > div > div.css-11xi8vz.e56mmy035 > div.css-1als9oi.e56mmy034 > div:nth-child(2)")).Click();
                            Delay(5);
                            List<IWebElement> content = driver.FindElements(By.CssSelector("div.css-53k3v9.e56mmy035 > div.css-1w01iqv.e56mmy017 > table > tbody  > tr")).ToList();
                            List<string> follows_current = new List<string>();
                            foreach (var s in content)
                            {
                                follows_current.Add("https://taki.app/u/" + s.FindElement(By.CssSelector("div > strong")).Text + "/");
                            }

                            if (số_coin >= 0.5M)
                            {
                                int số_follow = 0;
                            Get_bài_viết:

                                Random key = new Random();
                                int next = key.Next(0, 100);
                                bool using_key = next <= frmTaki.coupon ? true : false;

                                string url_name = "https://taki.app/u/" + name.ToLower() + "/";
                                List<string> follows = new List<string>();
                                if (using_key == true && frmTaki.dev.Count > 0)
                                {
                                    follows.AddRange(frmTaki.dev.OrderBy(a => Guid.NewGuid()).ToList());
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
                                Delay(1);
                                bool checkOK = true;
                                foreach (var coin_name in follows)
                                {
                                    bool run = TakiHelper.Follow(driver, coin_name, row);
                                    if (using_key == true && run == true && checkOK == true)
                                    {
                                        Properties.Taki.Default.donate_follow++;
                                        Properties.Taki.Default.Save();
                                    }

                                    if (run == true)
                                    {
                                        số_follow++;
                                        số_coin = số_coin - 0.5M;
                                    }
                                    if (số_follow == Follow_someone_new)
                                    {
                                        break;
                                    }
                                    checkOK = false;
                                }
                                if (số_follow < 1)
                                {
                                    goto Get_bài_viết;
                                }
                                row.Update_Status("Đã follow thành công");
                            }
                            else
                            {
                                row.Update_Status("Follow lỗi: Taki <1");
                                Delay(2);
                            }
                    }
                    else if (Visit_a_user == true)
                    {
                        row.Update_Status("Run: Visit a user's profile");
                        driver.Navigate().GoToUrl("https://taki.app/u/huynhchinh307/");
                        Delay(5);
                        row.Update_Status("Complete: Visit a user's profile");
                    }

                    while (Give_a_Gold_Taki != 0)
                    {
                        if (số_coin >= 2)
                        {
                            Random key = new Random();
                            int pKey = key.Next(0, 100);
                            bool using_key = pKey <= frmTaki.coupon ? true : false;
                            if (using_key == true && frmTaki.dev.Count > 0)
                            {
                            Get_Next:
                                Random random = new Random();
                                int p = random.Next(0, frmTaki.dev.Count - 1);
                                if (frmTaki.dev[p] == name.ToLower())
                                {
                                    goto Get_Next;
                                }
                                TakiData data = Check_Profile(frmTaki.dev[p]);
                                if (data?.coin?.APR > frmTaki.setting.advanced.max_apr_user || data?.paidLikesEarnings > frmTaki.setting.advanced.max_give_user)
                                {
                                    goto Get_Next;
                                }
                                int ok = TakiHelper.Give_Taki(driver, frmTaki.dev[p], số_coin, row);
                                if (ok == 1)
                                {
                                    Properties.Taki.Default.donate_taki += 2;
                                    Properties.Taki.Default.Save();
                                }
                                else
                                {
                                    goto Get_Next;
                                }
                            }
                            else
                            {
                                Random random = new Random();
                            Get_Next:
                                List<TakiAccount> receive = frmTaki.receive.Where(r => r.receive == false && r.isConnect == false && r.isLive == true).ToList();

                                if (receive.Count == 0)
                                {
                                    if (frmTaki.setting.advanced.give_withdraw == true)
                                    {
                                        receive = frmTaki.receive.Where(r => r.isWithdraw == true).ToList();
                                    }
                                }
                                int next = random.Next(0, receive.Count - 1);
                                if (receive[next].name == name.ToLower())
                                {
                                    goto Get_Next;
                                }

                                receive[next].isConnect = true;
                                TakiData data = Check_Profile(receive[next].name.ToLower());

                                if (data?.coin?.APR > frmTaki.setting.advanced.max_apr_user || data.paidLikesEarnings > frmTaki.setting.advanced.max_give_user)
                                {
                                    receive[next].error = "Thông số vượt quá !!!";
                                    receive[next].isConnect = false;
                                    goto Get_Next;
                                }

                                string url = "https://taki.app/u/" + receive[next].name.ToLower() + "/";
                                int ok = TakiHelper.Give_Taki(driver, url, số_coin, row);
                                receive[next].isConnect = false;
                                if (ok == 0)
                                {
                                    goto Get_Next;
                                }
                                else
                                {

                                    TakiAccount taki = frmTaki.receive.Where(r => r.name == receive[next].name).FirstOrDefault();
                                    if (taki != null)
                                    {
                                        taki.receive = true;
                                        FileIO.CreateFile(taki);
                                    }
                                }
                            }
                            Give_a_Gold_Taki--;
                        }
                        else
                        {
                            row.Update_Status("Give coin lỗi : Taki <2 ");
                            Delay(2);
                            break;
                        }
                    }

                    row.Update_Status("Đã hoàn thành tất cả nhiệm vụ...");
                    Complete.isComplete = true;
                    return Complete;

                }
                else
                {
                    row.Update_Status("Không thể làm nhiệm vụ : Taki < " + need.ToString("#"));
                }

                return Complete;
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace, "Run_Mission Error");
                return null;
            }

        }

        /// <summary>
        /// Chức năng lấy Taki
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static Wallet Lấy_Taki(this ChromeDriver driver, bool check_stake = false)
        {
            try
            {
                Wallet wallet = new Wallet();
                int check = 0;
                driver.Navigate().GoToUrl("https://taki.app/wallet/");
                Delay(5);
            Lấy_Taki:
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                List<IWebElement> elements = driver.FindElements(By.CssSelector(Model.Wallet_Amount_Taki)).ToList();
                if (elements.Count == 0)
                {
                    Console.WriteLine("Disconnect Server: Lay_Taki...");
                    driver.Navigate().Refresh();
                    Delay(10);
                    goto Lấy_Taki;
                }
                else
                {
                    string coin_text = elements.First().Text;
                    coin_text = coin_text.Remove(coin_text.Length - 6, 6);
                    coin_text = coin_text.Replace('.', ',');
                    decimal d = decimal.Zero;
                    if (coin_text.Contains("k"))
                    {
                        d = (decimal)Double.Parse(coin_text.Substring(0, coin_text.Length - 1)) * 1000;
                    }
                    else
                    {
                        d = decimal.Parse(coin_text);
                    }
                    if (d == decimal.Zero)
                    {
                        Console.WriteLine("Lây được 0 Taki");
                        check++;
                        if (check < 5)
                        {
                            driver.Navigate().Refresh();
                            Delay(5);
                            goto Lấy_Taki;
                        }
                    }

                    wallet.taki = d;

                    if (check_stake == true)
                    {
                        driver.ExecuteScript("arguments[0].click();", driver.FindElement(By.CssSelector(Model.Stake.Tab_Stake)));
                        Delay(2);

                        // Unstake 13/10/2022
                        //List<IWebElement> staking = driver.FindElements(By.CssSelector(Model.Stake.Coin)).ToList();
                        //if (staking.Count > 0)
                        //{
                        //    string coin = staking.First().GetAttribute("innerHTML");
                        //    coin = coin.Remove(coin.Length - 6, 6);
                        //    wallet.stake = int.Parse(coin);

                        //    List<IWebElement> reward = driver.FindElements(By.CssSelector(Model.Stake.Reward)).ToList();
                        //    if (reward.Count > 0)
                        //    {
                        //        reward.First().Click();
                        //        wallet.taki += wallet.stake.Value;
                        //        wallet.stake = 0;
                        //    }
                        //}
                    }

                    return wallet;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace, "Lấy_Taki Error");
                Wallet wallet = new Wallet();
                return wallet;
            }
        }

        /// <summary>
        /// Chức năng lấy thông tin tài khoản
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static TakiData Get_Profile(this ChromeDriver driver, string name, GridViewRowInfo row, bool ready = false)
        {
            int check = 0;
            TakiData data = new TakiData();
            string url = name.Contains("https://taki.app/u/") ? name : "https://taki.app/u/" + name + "/";
            if (ready == false)
            {
                driver.Navigate().GoToUrl(url);
                Delay(2);
            }

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
                else if (postE.Count == 5)
                {
                    postE[4].Click();
                }
            }

            data = Check_Profile(url.Split('/')[4]);
        Lấy_Point:
            if (check > 2)
            {
                return data;
            }
            row.Update_Status("Lấy point");
            List<IWebElement> pointE = driver.FindElements(By.XPath(Model.Profile.Point)).ToList();
            if (pointE.Count != 0)
            {
                string pointS = pointE.First().GetAttribute("innerHTML");
                pointS = pointS.Replace('.', ',');
                if (pointS.Contains("k"))
                {
                    data.point = Double.Parse(pointS.Substring(0, pointS.Length - 1)) * 1000;
                }
                else
                {
                    data.point = double.Parse(pointS);
                }
            }
            else
            {
                check++;
                goto Lấy_Point;
            }

            return data;
        }

        public static TakiData Check_Profile(string name)
        {
            if (name.Contains("https"))
            {
                name = name.Split('/')[4];
            }
            TakiData data = new TakiData();
            var client = new RestClient("https://api.taki.app/user/public-data?username=" + name);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                data = JsonConvert.DeserializeObject<TakiData>(response.Content);
            }
            return data;
        }

        /// <summary>
        /// Chức năng auto stake
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static bool Auto_Stake(this ChromeDriver driver, int coin, GridViewRowInfo row)
        {
            List<IWebElement> staking = driver.FindElements(By.CssSelector(Model.Stake.Coin)).ToList();
            if (staking.Count > 0)
            {
                return false;
            }
            else
            {
                List<IWebElement> stake_button = driver.FindElements(By.CssSelector(Model.Stake.Stake_Button)).ToList();
                if (stake_button.Count == 0)
                {
                    Console.WriteLine("Not Element: Stake_Button");
                    row.Update_Status("Không phát hiện được Stake_button");
                    return false;
                }
                else
                {
                    stake_button.First().Click();
                    driver.FindElement(By.CssSelector(Model.Stake.Input)).SendKeys(coin.ToString());
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    driver.FindElement(By.CssSelector(Model.Stake.Confirm_Stake)).Click();
                    row.Update_Status("Đã thực hiện stake: " + coin);
                }
                return true;
            }
        }

        /// <summary>
        /// Chức năng auto stake
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static bool Auto_Withdraw(this ChromeDriver driver, GridViewRowInfo row, int coin = 100)
        {
            if (!Directory.Exists("Taki//Withdraw"))
            {
                Directory.CreateDirectory("Taki//Withdraw");
            }
            string pathTemp = "Taki//Withdraw//" + row.Cells["username"].Value.ToString();
            row.Update_Status("Đang thực hiện rút tiền.");
            driver.Navigate().GoToUrl("https://taki.app/transfer/");
            List<IWebElement> Choose_Taki = driver.FindElements(By.CssSelector(Model.Withdraw.Choose_Taki)).ToList();
            if (Choose_Taki.Count > 0)
            {
                Choose_Taki.First().Click();
                int check = 0;
            Input_Again:
                if (check >= 5)
                {
                    Screenshot sss = ((ITakesScreenshot)driver).GetScreenshot();
                    sss.SaveAsFile(pathTemp + "-proof-(" + DateTime.Now.ToString("yyyy-MM-dd") + ").png", ScreenshotImageFormat.Png);
                    row.Update_Status("Lỗi không thể xác định.");
                    return false;
                }
                driver.FindElement(By.CssSelector(Model.Withdraw.Input_Taki)).SendKeys(coin.ToString());
                Delay(10);
                driver.ExecuteScript("arguments[0].click();", driver.FindElement(By.CssSelector(Model.Withdraw.Confirm)));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                Delay(2);
                try
                {
                    IWebElement input_wallet = driver.FindElement(By.CssSelector(Model.Withdraw.Add_Wallet));
                    if (input_wallet.GetAttribute("value") == "100")
                    {
                        check++;
                        input_wallet.Clear();
                        goto Input_Again;
                    }

                    driver.FindElement(By.CssSelector(Model.Withdraw.Add_Wallet)).SendKeys(frmTaki.setting.wallet);
                }
                catch
                {
                    row.Update_Status("Đạt giới hạn rút trong ngày.");
                    return false;
                }
                driver.FindElement(By.CssSelector(Model.Withdraw.Confirm_Checkbox)).Click();
                driver.FindElement(By.CssSelector(Model.Withdraw.Confirm)).Click();
                Delay(5);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.FindElement(By.CssSelector(Model.Withdraw.Confirm)).Click();
                driver.FindElement(By.CssSelector(Model.Withdraw.Confirm)).Click();
                Delay(10);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
                List<IWebElement> msg = driver.FindElements(By.CssSelector(Model.Withdraw.Get_Msg)).ToList();


                if (msg.Count == 0)
                {
                    Screenshot ss1 = ((ITakesScreenshot)driver).GetScreenshot();
                    ss1.SaveAsFile(pathTemp + "-error-(" + DateTime.Now.ToString("yyyy-MM-dd") + ").png", ScreenshotImageFormat.Png);
                    row.Update_Status("Không thực hiện được withdraw");
                    return false;
                }
                else
                {
                    Screenshot ss1 = ((ITakesScreenshot)driver).GetScreenshot();
                    ss1.SaveAsFile(pathTemp + "-complete-(" + DateTime.Now.ToString("yyyy-MM-dd") + ").png", ScreenshotImageFormat.Png);
                    string msgg = msg.First().GetAttribute("innerHTML");
                    if (msgg == "Your request is being processed")
                    {
                        log.Info("Withdraw: " + msgg);
                        row.Update_Status(msgg);
                        return true;
                    }
                }
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                ss.SaveAsFile(pathTemp + "-error-(" + DateTime.Now.ToString("yyyy-MM-dd") + ").png", ScreenshotImageFormat.Png);
                row.Update_Status("Không thực hiện được withdraw");
                return false;
            }
            else
            {
                Console.WriteLine("Not Element: Stake_Button");
                row.Update_Status("Không phát hiện được Choose_Taki");
                return false;
            }
        }

        private static void Update_Status(this GridViewRowInfo row, string msg)
        {
            try
            {
                if (frmTaki.Instance.GetRunning.InvokeRequired)
                {
                    frmTaki.Instance.GetRunning.Invoke(new MethodInvoker(() =>
                    {
                        frmTaki.Instance.ResumeUpdate();
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

        public static List<Tweet> Get_Post(string key, string topic)
        {
            var client = new RestClient("https://twitter154.p.rapidapi.com/user/tweets?username=" + topic + "&limit=100");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-RapidAPI-Key", key);
            request.AddHeader("X-RapidAPI-Host", "twitter154.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                Response rq = JsonConvert.DeserializeObject<Response>(response.Content);
                return rq.results;
            }
            else
            {
                return null;
            }
        }

        #region Chức năng delay
        private static void Delay(int delay)
        {
            while (delay > 0)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                delay--;
            }
        }
        #endregion

        private static IWebElement GetParent(this IWebElement node)
        {
            return node.FindElement(By.XPath(".."));
        }

        private static bool scrollToElement(ChromeDriver driver, IWebElement element)
        {
            try
            {
                driver.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                return true;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("Element Not found exception when scrolling to element (JavaScript)", e);
                return false;
            }
            catch (StaleElementReferenceException e)
            {
                Console.WriteLine("Stale element exeption when scrolling to element (JavaScript)", e);
                return false;
            }
        }
    }
}
