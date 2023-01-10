using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace Taki_Ref
{
    public static class Support
    {
        public static object locks = new object();
        public static string getUsername(bool keep = false)
        {
            if(keep == true)
            {
                string[] lines = File.ReadAllLines(@"username.txt");
                Random rd = new Random();
                int id = rd.Next(lines.Count());
                return lines[id];
            }
            else
            {
                Random rd = new Random();
                int id = rd.Next(0, 999999999);
                return id.ToString();
            }
        }

        public static void Setting_Captcha(ChromeDriver driver)
        {
            driver.SwitchTo().Window(driver.WindowHandles[0]);
            Delay(1);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            IWebElement api_key = driver.FindElement(By.CssSelector(Model.Captcha.Input_API));
            string js = "let config_captcha = {}; config_captcha['autoSubmitForms'] = true; config_captcha['autoSolveRecaptchaV2'] = true; config_captcha['autoSolveInvisibleRecaptchaV2'] = false; config_captcha['autoSolveRecaptchaV3'] = false; Config.set(config_captcha);";
            string config = (string)driver.ExecuteScript(js);
            api_key.Clear();
            Monitor.Enter(locks);
            try
            {
                api_key.SendKeys(Properties.TakiRef.Default.api_captcha);
            }
            finally
            {
                Monitor.Exit(locks);
            }

            driver.FindElement(By.CssSelector(Model.Captcha.Input)).Click();
            Delay(5);
            var alert = driver.SwitchTo().Alert();
            alert.Accept();
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles[0]);
            driver.Navigate().Refresh();
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
            row.Cells["status"].Value = msg;
        }
    }
}
