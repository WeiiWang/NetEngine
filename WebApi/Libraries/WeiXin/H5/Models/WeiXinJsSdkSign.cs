﻿namespace WebApi.Libraries.WeiXin.H5.Models
{
    public class WeiXinJsSdkSign
    {
        public string appid { get; set; }

        public long timestamp { get; set; }

        public string noncestr { get; set; }

        public string signature { get; set; }

    }
}
