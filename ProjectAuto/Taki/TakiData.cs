namespace ProjectAuto.Taki
{
    public class TakiData
    {
        public UserData userData { get; set; }
        public Coin coin { get; set; }
        public int paidLikesEarnings { get; set; }
        public double? point { get; set; }
    }

    public class UserData
    {
        public string id { get; set; }
        public int followers { get; set; }
        public string cosmeticTier { get; set; }
        public string cosmeticTierName { get; set; }
        public string username { get; set; }
        public string bio { get; set; }
        public string fullName { get; set; }
        public int friends { get; set; }
    }

    public class Coin
    {
        public double APR { get; set; }
        public double value { get; set; }
        public string coinsInCirculation { get; set; }
        public string cosmeticTier { get; set; }
        public int countSupporters { get; set; }
        public string superlayerCoinId { get; set; }
    }
}
