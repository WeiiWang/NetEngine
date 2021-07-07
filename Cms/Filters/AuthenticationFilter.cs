﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Cms.Filters
{


    public class AuthenticationFilter : Attribute, IActionFilter
    {

        /// <summary>
        /// 是否跳过验证，可用于控制器下单个Action指定跳过验证
        /// </summary>
        public bool IsSkip { get; set; }



        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {

        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            var filter = (AuthenticationFilter)context.Filters.Where(t => t.ToString() == (typeof(AuthenticationFilter).Assembly.GetName().Name + ".Filters.AuthenticationFilter")).ToList().LastOrDefault();

            if (!filter.IsSkip)
            {


                if (!string.IsNullOrEmpty(context.HttpContext.Session.GetString("userId")))
                {
                    //成功登录

                }
                else
                {
                    //阻断跳转原先的请求信息到登录页
                    var result = new ViewResult
                    {
                        ViewName = "~/Views/User/Login.cshtml"
                    };

                    context.Result = result;


                    //302跳转到登录页面 
                    context.HttpContext.Response.Redirect("/User/Login/");
                }
            }
        }
    }
}
