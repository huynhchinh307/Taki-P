using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAuto.Taki
{
    class Request
    {
        public string requestId { get; set; }
        public string loaderId { get; set; }
        public string documentURL { get; set; }
        public RequestData request { get; set; }
    }

    public class RequestData
    {
        public string url { get; set; }
        public string method { get; set; }
        public Headers headers { get; set; }
    }

    public class Headers
    {
        public string Authorization { get; set; }
    }
}
