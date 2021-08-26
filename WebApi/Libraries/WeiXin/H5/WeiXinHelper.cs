﻿using Common.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using WebApi.Libraries.Http;
using WebApi.Libraries.WeiXin.H5.Models;

namespace WebApi.Libraries.WeiXin.H5
{

    public class WeiXinHelper
    {


        private static string appid;

        private static string appsecret;



        /// <summary>
        /// 微信H5网页开发帮助类
        /// </summary>
        /// <param name="in_appid">微信公众号APPID</param>
        /// <param name="in_appsecret">微信公众号密钥</param>
        public WeiXinHelper(string in_appid, string in_appsecret)
        {
            appid = in_appid;
            appsecret = in_appsecret;
        }



        /// <summary>
        /// 获取 AccessToken
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {

            string key = appid + appsecret + "accesstoken";

            var token = Common.CacheHelper.GetString(key);

            if (string.IsNullOrEmpty(token))
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + appsecret;

                var returnJson = Common.HttpHelper.Post(url, "", "form");

                token = JsonHelper.GetValueByKey(returnJson, "access_token");

                Common.CacheHelper.SetString(key, token, TimeSpan.FromSeconds(6000));
            }

            return token;
        }



        /// <summary>
        /// 获取 TicketID
        /// </summary>
        /// <returns></returns>
        private string GetTicketID()
        {

            string key = appid + appsecret + "ticketid";

            var ticketid = Common.CacheHelper.GetString(key);

            if (string.IsNullOrEmpty(ticketid))
            {

                string getUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + GetAccessToken() + "&type=jsapi";

                string returnJson = Common.HttpHelper.Post(getUrl, "", "form");

                ticketid = JsonHelper.GetValueByKey(returnJson, "ticket");

                Common.CacheHelper.SetString(key, ticketid, TimeSpan.FromSeconds(6000));
            }

            return ticketid;
        }



        /// <summary>
        /// 获取 JsSDK 签名信息
        /// </summary>
        /// <returns></returns>
        public WeiXinJsSdkSign GetJsSDKSign()
        {
            var sdkSign = new WeiXinJsSdkSign();

            string url = HttpContext.GetUrl();

            sdkSign.appid = appid;
            sdkSign.timestamp = Common.DateTimeHelper.TimeToUnix(DateTime.Now);
            sdkSign.noncestr = Guid.NewGuid().ToString().Replace("-", "");

            string jsapi_ticket = GetTicketID();
            string strYW = "jsapi_ticket=" + jsapi_ticket + "&noncestr=" + sdkSign.noncestr + "&timestamp=" + sdkSign.timestamp + "&url=" + url;

            using (var sha1 = SHA1.Create())
            {
                byte[] bytes_sha1_in = Encoding.Default.GetBytes(strYW);
                byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
                string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
                str_sha1_out = str_sha1_out.Replace("-", "").ToLower();

                sdkSign.signature = str_sha1_out;
            }

            return sdkSign;
        }

    }

}
