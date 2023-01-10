
namespace ProjectAuto
{
    public class Model
    {
        public string Wallet = "//*[@id=\"__next\"]/div/div/div[2]/div[4]";
        public static string Top = "//*[@id=\"__next\"]/div/div/div[1]/div/div/div[2]/div/div/div[1]";
        public static string Wallet_Amount_Taki = "div.css-15pd7m4.ecxfbbq8 > table > tbody > tr:nth-child(1) > td:nth-child(4) > div > strong";

        // Tạo tài khoản Model 
        public static class Tạo_Tài_Khoản
        {
            public static string Accept_Invite = "div.css-3kgsnu.ej2twmx21 > button";
            public static string Continue_with_Phone = "div.css-bfvdch.e1gwtos23 > button:nth-child(1)";
            public static string Choose_Country = "//*[@id=\"__next\"]/div[2]/div[1]/div[2]/div[2]/div[1]/div[2]/div[1]/div/div/button";
            public static string Choose_VN = "//*[@id=\"modal-root\"]/div[2]/div/div[2]/div/div/div/div[24]";
            public static string Input_SDT = "//*[@id=\"__next\"]/div[2]/div[1]/div[2]/div[2]/div[1]/div[2]/div[2]/div/div/input";
            public static string Continue_Input_Sdt = "//*[@id=\"__next\"]/div[2]/div[1]/div[2]/div[2]/div[1]/div[3]";
            // Giai đoạn nhập code 
            public static string Input_Code_SMS = "//*[@id=\"__next\"]/div[2]/div[1]/div[2]/div[2]/div[1]/form/div[1]/div/div/input";
            public static string Continue_Code_SMS = "//*[@id=\"__next\"]/div[2]/div[1]/div[2]/div[2]/div[1]/form/div[2]";
            // Nhập UserName 
            public static string Input_Username = "//*[@id=\"username\"]";
            public static string Continue_Username = "//*[@id=\"__next\"]/div[2]/div[1]/div[2]/div[2]/div/form/button";
            public static string val_account = "div.css-1l75fk7.emn2ld31";
            public static string send_code = "div.css-1mk9j8t.e1nzji2n3 > h2";

            public static string Solona = "#modal-root > div:nth-child(4) > div > div > div > div:nth-child(2) > div.css-11blc1a.epiie924";
            public static string Solona_msg = "Solana is still working on your previous transaction";
        }


        public string Catpcha = "//*[@id=\"__next\"]/div[2]/div[1]/div[2]/div[2]/div/div[2]/div/div/div/div/iframe";
        public static string Captcha_Invisable = "div:nth-child(2) > iframe";

        public string Copy_Button = "//*[@id=\"__next\"]/div[2]/div/div/div/div[2]/div[2]/div[2]/div/div[2]/div[1]/div/div";

        // Back get code
        public string Back_Get_Code = "//*[@id=\"__next\"]/div[2]/div[1]/div[2]/div[2]/button";
        // Bước 2
        public string Create_My_Coin = "//*[@id=\"modal-root\"]/div[2]/div/div[2]/div/button";
        public string Goto_Wallet = "//*[@id=\"modal-root\"]/div[2]/div/div[2]/div/button[2]";
        // Buy coin
        public string Buy_Coin_Button_C = "#__next > div.css-aweyws.ej2twmx20 > div > div > div.css-c9cvz2.e56mmy048 > div > div > div > div.css-11xi8vz.e56mmy033 > div.css-cgbnge.e56mmy043 > button.css-1e691c3.e15ggqxc9";
        public string Get_Coin_C = "#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-1lju76g.epiie927 > form > div.css-1anqzxy.e16m6pu920 > div.css-7h9h7c.e19vp1032 > div.css-1k6v9io.ej2twmx20 > div > div";
        public string Buy_Amount_C = "#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-1lju76g.epiie927 > form > div.css-1anqzxy.e16m6pu920 > div.css-7h9h7c.e19vp1032 > div.css-xfg7pj.emn2ld34 > div > input";
        public string Buy_C = "#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-1lju76g.epiie927 > form > button";
        public string Go_Wallet_After_Buy_Coin = "#modal-root > div.css-1ll9bqd.e1l1al3h31 > div > div.css-1lju76g.epiie927 > div > div.css-ltuzx7.e16m6pu99 > button.css-yi2qqh.e15ggqxc9";

        public string Setting_Taki_Gold = "confirmGoldTaki";
        public string Setting_Follow = "confirmFollowUser";

        public static class Nhiệm_Vụ
        {
            public static string Nhiệm_Vụ_Còn = "div.css-1tqvuvj.e7gb5iu2";
            public static string Button_Nhiệm_Vụ = "#header > div > div.css-14e08vj.ej2twmx21 > div.css-1e0t1qd.ej2twmx3 > div";
            public static string Nv_Link = "div.css-1tqvuvj.e7gb5iu2 > div > a";
            public static string TOP = "#content-wrapper > div.css-tsltxw.eno6ygk2 > div > div > div > div:nth-child(1)";
            public static string My_Feed = "#content-wrapper > div.css-tsltxw.eno6ygk2 > div > div > div > div:nth-child(2)";
            public static string Follow = "button.css-kl1r3v.e15ggqxc9";
            public static string is_Follow = "div.css-1dfe3fa.e56mmy045 > button:nth-child(1) > span";

            public static string ALL = "#content-wrapper > div.css-tsltxw.eno6ygk2 > div > div > div > div:nth-child(3)";
            public static string New_Post = "div.css-1mbqsaa.ej2twmx21 > a";

            public static string confirm = "#checkbox";
            public static string confirm_pay = "div.css-akdqam.e5k0zht0 > button";

            public static string mine = "div.css-1t9jot6.ei3t0b48 > div";
        }

        public static class Create_Post
        {
            public static string Button_Create = "div.css-166in.eb024ab1 > div";
            public static string Input_Post = "div.css-1n8jmyv.e1a4ahls14 > div > div > textarea";
            public static string Submit_Post = "div.css-iypy54.e1a4ahls12 > button";
            public static string msg = "div.css-myk9g6.ec6v7f3 > div > div > div > span";
        }

        public static class Get_Invite
        {
            public static string Copy_Button = "div.css-1mg08ub.e1x758jn4";
        }

        public static class Profile
        {
            public static string Point = "//*[@id=\"content-wrapper\"]/div[3]/div/div/div/div[1]/div[2]/div[2]/span";
            public static string Att = "div.css-aix1p3.e56mmy025 > div";
            public static string APR = "h4";
            public static string Price = "div.css-103kw1w.e56mmy026 > small";
            public static string Circulation = "h4";
            public static string Avatar = "div.css-11xi8vz.e56mmy033 > div:nth-child(1) > div > div > div > img";
            public static string Default_Avatar = "/static/icons/user.svg";
            public static string Edit_Profile = "button.css-kl1r3v.e15ggqxc9";
            public static string Change_Photo = "a > div > div.css-1scoo8e.e2kuma41";

        }

        public static class Home
        {
            public static string Captcha = "div.css-1ll9bqd.e1l1al3h31 > div > div";
            public static string Home_Taki = "#header";
        }

        public static class Give_Taki
        {
            public static string Content = "div.css-182ucoe.eno6ygk0";
            public static string GiveTaki = "div.css-17cz3h9.e1l1al3h12 > div > svg > circle";
            public static string Give_Number = "div.css-fxp7t8.e1l1al3h32 > div:nth-child(4) > div > span";
            public static string confirm = "#checkbox";
            public static string confirm_pay = "div.css-1ll9bqd.e1l1al3h31 > div > div > div > div > button";
        }

        public static class Withdraw
        {
            public static string Url = "https://taki.app/transfer/";
            public static string Choose_Taki = "div.css-8wmq9d.er5voor16";
            public static string Input_Taki = "div.css-xfg7pj.emn2ld34 > div > input";
            public static string Add_Wallet = "div.css-xfg7pj.emn2ld34 > div > input";
            public static string Get_Msg = "div.css-12p9fkb.er5voor17 > div.card-body > div:nth-child(2) > h2 > div";
            public static string Confirm = "div.card-footer > div > button";
            public static string Confirm_Checkbox = "#verification-check";

        }

        public static class Walletq
        {
            public static string Coin = "div.css-15pd7m4.ecxfbbq8 > table > tbody > tr";
            public static string Coin_Name = "td:nth-child(2) > div > strong";
            public static string Menu = "td:nth-child(5) > div > div";
            public static string Buy = "#layers > div > div > div:nth-child(1)";
            public static string Buy_Amount = "div.css-xfg7pj.emn2ld34 > div > input";
            public static string OK = "//*[@id=\"modal-root\"]/div[2]/div/div[2]/form/button";
            public static string OKK = "button.css-jbkzcb.e15ggqxc9";
        }

        public static class Stake
        {
            public static string Coin = "b.css-1ozgtb1.e18g488g5";
            public static string Tab_Stake = "div.css-182ucoe.eno6ygk0";
            public static string Alive = "div.css-hbdr9t.ej2twmx20 > button";
            public static string Input = "div.css-xfg7pj.emn2ld34 > div > input";
            public static string Confirm_Stake = "div.epiie927 > div > div:nth-child(2) > button";
            public static string Reward = "div.css-h2vy18.e18g488g0 > button";
            public static string Stake_Button = "div.css-b0nrou.ecxfbbq21 > div > button";
        }

        public static class Captcha
        {
            public static string Url = "chrome-extension://ifibfemgeogfhoebkmokieepdoobkbpo/options/options.html";
            public static string Input_API = "body > div > div.content > table > tbody > tr:nth-child(1) > td:nth-child(2) > input[type=text]";
            public static string Input = "#connect";
            public static string autoSubmitForms = "#autoSubmitForms";
            public static string autoSolveRecaptchaV2 = "#autoSolveRecaptchaV2";
            public static string autoSolveRecaptchaV3 = "#autoSolveRecaptchaV3";
            public static string captcha_solver = "div.captcha-solver";
            public static string msg = "div.css-1l75fk7.emn2ld31";
            public static string captcha_callback = "head > captcha-widgets > captcha-widget";
        }
    }
}
