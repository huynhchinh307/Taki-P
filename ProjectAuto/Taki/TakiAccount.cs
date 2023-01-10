using System;

namespace ProjectAuto
{
    public class TakiAccount
    {
        public string name { get; set; }
        public string invite_link { get; set; }
        public decimal taki { get; set; }
        public string proxy { get; set; }
        public string topic { get; set; }
        public bool isWithdraw { get; set; }
        public bool isRef { get; set; }
        public int stake { get; set; }
        public double point { get; set; }
        public bool complete { get; set; }
        public bool withdraw { get; set; }
        public double? price { get; set; }
        public double? apr { get; set; }
        public string cosmeticTierName { get; set; }
        public int beforePaidLikesEarnings { get; set; }
        public int paidLikesEarnings { get; set; }
        public int countSupporters { get; set; }
        public DateTime SaveDate { get; set; }
        public bool withdraw_day { get; set; }
        public string error { get; set; }
        public bool receive { get; set; }
        public bool upload { get; set; }
        public bool isProcess { get; set; }
        public bool isLive { get; set; }
        public bool isConnect { get; set; }
        public string userAgent { get; set; }
        public bool isBrowser { get; set; }
    }
}
