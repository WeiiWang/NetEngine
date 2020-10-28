﻿using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using TaskAdmin.Filters;
using TaskAdmin.Libraries;

namespace TaskAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            //为各数据库注入连接字符串
            Repository.Database.dbContext.ConnectionString = Configuration.GetConnectionString("dbContext");
            Common.RedisHelper.ConnectionString = Configuration.GetConnectionString("redisContext");


            //注册 HangFire(SqlServer)
            services.AddHangfire(configuration => configuration
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    SchemaName = "dbo"
                }));

            //注册 HangFire(Redis)
            //services.AddHangfire(configuration => configuration.UseRedisStorage("127.0.0.1,Password=123456,DefaultDatabase=13"));

            // 注册 HangFire 服务
            services.AddHangfireServer(config => config.SchedulePollingInterval = TimeSpan.FromSeconds(3));



            services.AddControllersWithViews();


            //注册HttpContext
            Libraries.Http.HttpContext.Add(services);


            //注册全局过滤器
            services.AddMvc(config => config.Filters.Add(new GlobalFiler()));



            //注册配置文件信息
            Libraries.Start.StartConfiguration.Add(Configuration);


            //托管Session到Redis中
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost,Password=123456,DefaultDatabase=0";
            });


            //注册Session
            services.AddSession(options =>
            {
                //设置Session过期时间
                options.IdleTimeout = TimeSpan.FromHours(3);
            });


            //解决中文被编码
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));


            //注册统一模型验证
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {

                    //获取验证失败的模型字段 
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0).Select(e => e.Value.Errors.First().ErrorMessage).ToList();

                    var dataStr = string.Join(" | ", errors);

                    //设置返回内容
                    var result = new
                    {
                        errMsg = dataStr
                    };

                    return new BadRequestObjectResult(result);
                };
            });


            //注册雪花ID算法示例
            services.AddSingleton(new Common.SnowflakeHelper(0, 0));

        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //开启倒带模式运行多次读取HttpContext.Body中的内容
            app.Use(next => context =>
            {
                context.Request.EnableBuffering();
                return next(context);
            });


            //注册全局异常处理机制
            app.UseExceptionHandler(builder => builder.Run(async context => await GlobalError.ErrorEvent(context)));

            app.UseHsts();


            //强制重定向到Https
            app.UseHttpsRedirection();


            app.UseStaticFiles();


            //注册HttpContext
            Libraries.Http.HttpContext.Initialize(app, env);


            //注册Session
            app.UseSession();


            //注册HostingEnvironment
            Libraries.Start.StartHostingEnvironment.Add(env);


            app.UseRouting();


            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new DashboardAuthorizationFilter() }
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
