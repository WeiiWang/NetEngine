﻿using Methods.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cms.Libraries
{
    public class GlobalError
    {
        public static Task ErrorEvent(HttpContext context)
        {
            var feature = context.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;

            var ret = new
            {
                errMsg = error.Message
            };


            string path = Methods.Http.HttpContext.GetUrl();

            var parameter = Methods.Http.HttpContext.GetParameter();

            var content = new
            {
                path = path,
                parameter = parameter,
                error = error
            };

            string strContent = JsonHelper.ObjectToJSON(content);

            Methods.UseDB.Log.Set("Cms", "errorlog", strContent);

            return context.Response.WriteAsync(JsonHelper.ObjectToJSON(ret));
        }
    }
}
