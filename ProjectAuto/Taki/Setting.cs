namespace ProjectAuto.Taki
{
    public class Setting
    {
        public string name_device { get; set; }
        public string key { get; set; }
        public int earn { get; set; }
        public string wallet { get; set; }
        public int? thread { get; set; }
        public Basic basic { get; set; }
        public Advanced advanced { get; set; }
    }

    public class Basic
    {
        public string api_sms { get; set; }
        public string api_proxy { get; set; }
        public string api_captcha { get; set; }
        public string api_post { get; set; }
        public string agent { get; set; }
        public string path_profile { get; set; }
    }

    public class Advanced
    {
        public int max_give_user { get; set; }
        public int max_follow_user { get; set; }
        public int min_follow_user { get; set; }
        public int max_apr_user { get; set; }
        public int max_give_post_user { get; set; }
        public int max_user_proxy { get; set; }
        public bool give_withdraw { get; set; }
    }
}
