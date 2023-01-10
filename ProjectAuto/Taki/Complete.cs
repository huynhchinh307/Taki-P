using System.Collections.Generic;

namespace ProjectAuto
{
    public class Complete
    {
        public bool isComplete { get; set; }
        public bool receive { get; set; }
    }

    public class Wallet
    {
        public decimal taki { get; set; }
        public int? stake { get; set; }
    }

    public class Tweet
    {
        public string username { get; set; }
        public string text { get; set; }
        public List<string> media_url { get; set; }
        public int timestamp { get; set; }
    }

    public class RootProfile
    {
        public List<Tweet_Profile> results { get; set; }
    }

    public class Tweet_Profile
    {
        public string username { get; set; }
        public string name { get; set; }
        public string profile_pic_url { get; set; }
        public string description { get; set; }
        public int number_of_tweets { get; set; }
    }

    public class Response
    {
        public List<Tweet> results { get; set; }
    }
}
