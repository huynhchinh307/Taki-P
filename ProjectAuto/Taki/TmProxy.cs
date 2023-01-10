using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAuto.Taki
{
    class TmProxy
    {
        public int code { get; set; }
        public string message { get; set; }
        public TmProxyData data { get; set; }
    }

    public class TmProxyData
    {
        public string ip_allow { get; set; }
        public string location_name { get; set; }
        public string socks5 { get; set; }
        public string https { get; set; }
        public int timeout { get; set; }
        public int next_request { get; set; }
        public string expired_at { get; set; }
    }

    class TmStats
    {
        public int code { get; set; }
        public string message { get; set; }
    }
}
