using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAuto.Taki
{
    class ViOTP
    {
        public class Phone
        {
            public string phone_number { get; set; }
            public string re_phone_number { get; set; }
            public string countryISO { get; set; }
            public string countryCode { get; set; }
            public int request_id { get; set; }
            public int balance { get; set; }
        }

        public class SMS
        {
            public int Status { get; set; }
            public string Code { get; set; }
        }

        public class Lấy_SĐT
        {
            public int status_code { get; set; }
            public string message { get; set; }
            public bool success { get; set; }
            public Phone data { get; set; }
        }

        public class Lấy_SMS
        {
            public int status_code { get; set; }
            public string message { get; set; }
        }


        public static Lấy_SĐT Tạo_Yêu_Cầu()
        {
            RestClient restClient = new RestClient("https://api.viotp.com/request/getv2?token=" + frmTaki.setting.basic.api_sms + "serviceId=673");
            RestRequest request = new RestRequest(Method.GET);
            var response = restClient.Execute(request);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Lấy_SĐT>(response.Content);
            }
            else
            {
                return null;
            }
        }


        public static Lấy_SĐT Lấy_Code(string prefix = null)
        {
            string url = "https://api.viotp.com/request/getv2?token=" + frmTaki.setting.basic.api_sms + "serviceId=673";
            if (prefix != null)
            {
                url += "&prefix=" + prefix;
            }
            RestClient restClient = new RestClient("https://api.viotp.com/request/getv2?token=" + frmTaki.setting.basic.api_sms + "serviceId=673");
            RestRequest request = new RestRequest(Method.GET);
            var response = restClient.Execute(request);
            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Lấy_SĐT>(response.Content);
            }
            else
            {
                return null;
            }
        }
    }
}
