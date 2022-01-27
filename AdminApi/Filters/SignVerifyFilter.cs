﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace AdminApi.Filters
{

    /// <summary>
    /// 签名过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SignVerifyFilter : Attribute, IActionFilter
    {


        /// <summary>
        /// 是否跳过签名验证，可用于控制器下单个接口指定跳过签名验证
        /// </summary>
        public bool IsSkip { get; set; }


        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {

            SignVerifyFilter filter = (SignVerifyFilter)context.Filters.Where(t => t.ToString() == (typeof(SignVerifyFilter).Assembly.GetName().Name + ".Filters.SignVerifyFilter")).ToList().LastOrDefault()!;


            if (!filter.IsSkip)
            {
                var token = context.HttpContext.Request.Headers["Token"].ToString().ToLower();

                string rip = context.HttpContext.Connection.RemoteIpAddress!.ToString();

                if (!rip.Contains("127.0.0.1"))
                {
                    var timeStr = context.HttpContext.Request.Headers["Time"].ToString();
                    var time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(timeStr));

                    if (time.AddMinutes(10) > DateTime.UtcNow)
                    {
                        string privatekey = "gPmgRr9Dp3wzubTaGIgmMSpfNiKqkIAA0C8gkaBSN0ca3GWxk3W6682KuXRpxnDq";

                        string strdata = privatekey + timeStr;


                        if (context.HttpContext.Request.Method == "POST")
                        {
                            string body = Libraries.Http.HttpContext.GetRequestBody();

                            if (!string.IsNullOrEmpty(body))
                            {
                                strdata += body;
                            }
                            else if (context.HttpContext.Request.HasFormContentType)
                            {
                                var fromlist = context.HttpContext.Request.Form.OrderBy(t => t.Key).ToList();

                                foreach (var fm in fromlist)
                                {
                                    string fmv = fm.Value.ToString();
                                    strdata = strdata + fm.Key + fmv;
                                }
                            }
                        }
                        else if (context.HttpContext.Request.Method == "GET")
                        {
                            var queryList = context.HttpContext.Request.Query.OrderBy(t => t.Key).ToList();

                            foreach (var query in queryList)
                            {
                                string qv = query.Value;
                                strdata = strdata + query.Key + qv;
                            }
                        }


                        string tk = Common.CryptoHelper.GetMd5(strdata).ToLower();

                        if (token != tk)
                        {
                            context.HttpContext.Response.StatusCode = 401;

                            context.Result = new JsonResult(new { errMsg = "非法 Token" });
                        }
                    }
                    else
                    {
                        context.HttpContext.Response.StatusCode = 401;
                        context.Result = new JsonResult(new { errMsg = "Token 已过期" });
                    }
                }
            }

        }


        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
