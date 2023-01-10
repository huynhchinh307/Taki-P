
namespace Taki_Ref
{
    public class Model
    {
        static string Mission = "//*[@id=\"header\"]/div/div[2]/div[1]";
        static string Explore = "//*[@id=\"__next\"]/div/div/div[2]/div[2]";
        static string Post = "//*[@id=\"__next\"]/div/div/div[2]/div[3]";
        public string Wallet = "//*[@id=\"__next\"]/div/div/div[2]/div[4]";
        static string Post_Textarea = "//*[@id=\"modal-root\"]/div[2]/div/div[2]/div/div/div/div[1]/form/div/div[2]/div/div/textarea";
        static string Post_Post = "//*[@id=\"modal-root\"]/div[2]/div/div[2]/div/div/div/div[3]/button";
        public static string Top = "//*[@id=\"__next\"]/div/div/div[1]/div/div/div[2]/div/div/div[1]";
        static string MyFeed = "//*[@id=\"__next\"]/div/div/div[1]/div/div/div[2]/div/div/div[2]";
        static string ByCoin_Link = "https://taki.app/u/{0}/";
        static string By_Coin_Click = "//*[@id=\"__next\"]/div[2]/div/div/div[2]/div/div/div/div[1]/div[3]/button[2]";
        static string BuyCoin_Max = "//*[@id=\"modal-root\"]/div[2]/div/div[2]/form/div[1]/div[1]/div[1]/div/a";
        static string BuyCoin_Amount = "//*[@id=\"modal-root\"]/div[2]/div/div[2]/form/div[1]/div[1]/div[1]/div/div";
        static string BuyCoin_Confirm = "//*[@id=\"modal-root\"]/div[2]/div/div[2]/form/button";
        static string BuyCoin_Complete = "//*[@id=\"modal-root\"]/div[2]/div/div[2]/div/div[4]/button[1]";
        public string Wallet_Amount_Taki = "div.css-15pd7m4.ecxfbbq8 > table > tbody > tr:nth-child(1) > td:nth-child(4) > div > strong";

        // Tạo tài khoản Model 
        public static class Tạo_Tài_Khoản
        {
            public static string Accept_Invite = "div.css-12hmi8c.ej2twmx20 > button";
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
        // Lấy số coin
        public class Lấy_số_coin {
            string Số_Taki = "#__next > div.css-aweyws.ej2twmx20 > div > div > div > div.css-15pd7m4.ecxfbbq8 > table > tbody > tr:nth-child(1) > td:nth-child(4) > div > strong";
            string Số_VNĐ = "#__next > div.css-aweyws.ej2twmx20 > div > div > div > div.css-15pd7m4.ecxfbbq8 > table > tbody > tr:nth-child(1) > td:nth-child(4) > div > p";
        }

        public static class Nhiệm_Vụ
        {
            public static string Button_Nhiệm_Vụ = "div.css-1e0t1qd.ej2twmx2 > div";
            public static string Complete = "div.css-1t8e56f.ei3t0b43";
            //public static string Nv_1 = "div.css-1i4qrvu.e7gb5iu3";
            public static string Nv_Link = "div.css-1tqvuvj.e7gb5iu2 > div > a";
            //public static string NV_3_Done = "div.css-1i4qrvu.e7gb5iu3";
            public static string My_Feed = "div.css-g4jln6.eno6ygk2 > div > div > div:nth-child(2)";
            public static string Follow = "button.css-aexmy.e15ggqxc9";
            public static string is_Follow = "div.css-cgbnge.e56mmy043 > button:nth-child(1) > span";
            public static string Un_Follow = "div.css-akdqam.e5k0zht0 > button";
        }

        public static class Create_Post
        {
            public static string Button_Create = "div.css-pmbjuz.eb024ab1 > div:nth-child(3)";
            public static string Input_Post = "div.css-1n8jmyv.e1a4ahls14 > div > div > textarea";
            public static string Submit_Post = "div.css-1bik64t.e1a4ahls12 > button";
        }

        public static class Get_Invite
        {
            public static string Copy_Button = "div.css-1mg08ub.e1x758jn4";
        }

        public static class Profile
        {
            public static string Point = "div.css-7fxp0n.e56mmy04 > span";
        }

        public static class Home
        {
            public static string Captcha = "div.css-1ll9bqd.e1l1al3h31 > div > div";
            public static string Home_Taki = "#header";
        }

        public static class Give_Taki
        {
            public static string Content = "div.css-uk8pth.eno6ygk0";
            public static string GiveTaki = "div.css-14vtj9z.e1l1al3h12 > div > svg > circle";
        }

        public static class Withdraw {
            public static string Url = "https://taki.app/transfer/";
            public static string Check = "div.css-2fjj5a.er5voor17 > div.card-body > div:nth-child(3) > div";
            public static string Choose_Taki = "div.css-8wmq9d.er5voor16";
            public static string Input_Taki = "div.css-xfg7pj.emn2ld34 > div > input";
            public static string Add_Wallet = "div.css-2fjj5a.er5voor17 > div.card-footer > div > button";
            public static string Get_Msg = "div.css-2fjj5a.er5voor17 > div.card-body > div:nth-child(3) > div";
        }

        public static class Captcha {
            public static string Url = "chrome-extension://ifibfemgeogfhoebkmokieepdoobkbpo/options/options.html";
            public static string Input_API = "body > div > div.content > table > tbody > tr:nth-child(1) > td:nth-child(2) > input[type=text]";
            public static string Input = "#connect";
            public static string autoSubmitForms = "#autoSubmitForms";
            public static string autoSolveRecaptchaV2 = "#autoSolveRecaptchaV2";
            public static string autoSolveRecaptchaV3 = "#autoSolveRecaptchaV3";
            public static string captcha_solver = "div.captcha-solver";
            public static string msg = "div.css-1l75fk7.emn2ld31";
        }

        public static class Tạo_tài_khoản
        {
        }
    }
}
