using Hangfire;
using Medallion.Threading;
using Medallion.Threading.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using TaskAdmin.Filters;
using TaskAdmin.Libraries;
using TaskAdmin.Subscribes;

namespace TaskAdmin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Common.EnvironmentHelper.ChangeDirectory(args);
            Common.EnvironmentHelper.InitTestServer();

            var builder = WebApplication.CreateBuilder(args);

            //���� Kestrel Https ����֤��
            //builder.WebHost.UseKestrel(options =>
            //{
            //    options.ConfigureHttpsDefaults(options =>
            //    {
            //        options.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(Path.Combine(AppContext.BaseDirectory, "xxxx.pfx"), "123456");
            //    });
            //});
            //builder.WebHost.UseUrls("https://*");

            // Add services to the container.

            //Ϊ�����ݿ�ע�������ַ���
            Repository.Database.dbContext.ConnectionString = builder.Configuration.GetConnectionString("dbConnection");
            builder.Services.AddDbContextPool<Repository.Database.dbContext>(options => { }, 100);


            builder.Services.AddSingleton<IDistributedLockProvider>(new SqlDistributedSynchronizationProvider(builder.Configuration.GetConnectionString("dbConnection")));
            builder.Services.AddSingleton<IDistributedSemaphoreProvider>(new SqlDistributedSynchronizationProvider(builder.Configuration.GetConnectionString("dbConnection")));
            builder.Services.AddSingleton<IDistributedUpgradeableReaderWriterLockProvider>(new SqlDistributedSynchronizationProvider(builder.Configuration.GetConnectionString("dbConnection")));


            builder.Services.AddResponseCompression();

            builder.Services.AddSingleton<DemoSubscribe>();
            builder.Services.AddCap(options =>
            {

                //ʹ�� Redis ������Ϣ
                options.UseRedis(builder.Configuration.GetConnectionString("redisConnection"));

                //var rabbitMQSetting = builder.Configuration.GetSection("RabbitMQSetting").Get<RabbitMQSetting>();

                ////ʹ�� RabbitMQ ������Ϣ
                //options.UseRabbitMQ(options =>
                //{
                //    options.HostName = rabbitMQSetting.HostName;
                //    options.UserName = rabbitMQSetting.UserName;
                //    options.Password = rabbitMQSetting.PassWord;
                //    options.VirtualHost = rabbitMQSetting.VirtualHost;
                //    options.Port = rabbitMQSetting.Port;
                //    options.ConnectionFactoryOptions = options =>
                //    {
                //        options.Ssl = new RabbitMQ.Client.SslOption { Enabled = rabbitMQSetting.Ssl.Enabled, ServerName = rabbitMQSetting.Ssl.ServerName };
                //    };
                //});


                //ʹ�� ef ���� db �洢ִ�����
                options.UseEntityFramework<Repository.Database.dbContext>();

                options.UseDashboard();
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);

                options.DefaultGroupName = "default";   //Ĭ��������
                options.GroupNamePrefix = null; //ȫ��������ǰ׺
                options.TopicNamePrefix = null; //Topic ͳһǰ׺
                options.Version = "v1";
                options.FailedRetryInterval = 60;   //ʧ��ʱ���Լ��
                options.ConsumerThreadCount = 1;    //�������̲߳��д�����Ϣ���߳����������ֵ����1ʱ�������ܱ�֤��Ϣִ�е�˳��
                options.FailedRetryCount = 10;  //ʧ��ʱ���Ե�������
                options.FailedThresholdCallback = null; //������ֵ��ʧ�ܻص�
                options.SucceedMessageExpiredAfter = 24 * 3600; //�ɹ���Ϣ�Ĺ���ʱ�䣨�룩
            }).AddSubscribeFilter<CapSubscribeFilter>();


            builder.Services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(365);
            });


            //ע�� HangFire(Memory)
            builder.Services.AddHangfire(configuration => configuration.UseInMemoryStorage());


            //ע�� HangFire(Redis)
            //builder.Services.AddHangfire(options => options.UseRedisStorage(builder.Configuration.GetConnectionString("hangfireConnection")));


            //ע�� HangFire(SqlServer)
            //builder.Services.AddHangfire(options => options
            //    .UseSqlServerStorage(builder.Configuration.GetConnectionString("hangfireConnection"), new SqlServerStorageOptions
            //    {
            //        SchemaName = "hangfire"
            //    }));


            //ע�� HangFire(PostgreSQL)
            //builder.Services.AddHangfire(options => options
            //    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("hangfireConnection"), new PostgreSqlStorageOptions
            //    {
            //        SchemaName = "hangfire"
            //    }));


            //ע�� HangFire(MySql)
            //builder.Services.AddHangfire(options => options
            //    .UseStorage(new MySqlStorage(builder.Configuration.GetConnectionString("hangfireConnection") + "Allow User Variables=True", new MySqlStorageOptions
            //    {
            //        TablesPrefix = "hangfire_"
            //    })));



            // ע�� HangFire ����
            builder.Services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(3));



            builder.Services.AddControllersWithViews();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString("/User/Login/");
                options.AccessDeniedPath = new PathString("/User/Login/");
                options.ExpireTimeSpan = TimeSpan.FromHours(20);
            });


            //builder.Services.AddAuthorization(options =>
            //{
            //    options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireAssertion(context => IdentityVerification.Authorization(context)).Build();
            //});


            //ע��HttpContext
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            //ע��ȫ�ֹ�����
            builder.Services.AddMvc(config => config.Filters.Add(new GlobalFilter()));



            //�й�Session��Redis��
            if (Convert.ToBoolean(builder.Configuration["SessionToRedis"]))
            {
                builder.Services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = builder.Configuration.GetConnectionString("redisConnection");
                });
            }


            //ע��Session
            builder.Services.AddSession(options =>
            {
                //����Session����ʱ��
                options.IdleTimeout = TimeSpan.FromHours(3);
            });


            //������ı�����
            builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));


            //ע��ͳһģ����֤
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {

                    //��ȡ��֤ʧ�ܵ�ģ���ֶ� 
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0).Select(e => e.Value.Errors.First().ErrorMessage).ToList();

                    var dataStr = string.Join(" | ", errors);

                    //���÷�������
                    var result = new
                    {
                        errMsg = dataStr
                    };

                    return new BadRequestObjectResult(result);
                };
            });


            //ע��ѩ��ID�㷨ʾ��
            builder.Services.AddSingleton(new Common.SnowflakeHelper(0, 0));


            //ע�Ỻ����� �ڴ�ģʽ
            builder.Services.AddDistributedMemoryCache();


            //ע�Ỻ����� SqlServerģʽ
            //builder.Services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = builder.Configuration.GetConnectionString("dbConnection");
            //    options.SchemaName = "dbo";
            //    options.TableName = "t_cache";
            //});


            //ע�Ỻ����� Redisģʽ
            //builder.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = builder.Configuration.GetConnectionString("redisConnection");
            //    options.InstanceName = "cache";
            //});

            builder.Services.AddHttpClient("", options =>
            {
                options.DefaultRequestVersion = new Version("2.0");
                options.DefaultRequestHeaders.Add("Accept", "*/*");
                options.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36");
                options.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false
            });


            builder.Services.AddHttpClient("SkipSsl", options =>
            {
                options.DefaultRequestVersion = new Version("2.0");
                options.DefaultRequestHeaders.Add("Accept", "*/*");
                options.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36");
                options.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });

            var app = builder.Build();

            ServiceProvider = app.Services;

            app.UseResponseCompression();

            //���ñ��ػ���Ϣ����ʵ�� �̶� Hangfire �������Ϊ������ʾ
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh-CN"),
                SupportedCultures = new[]
                {
                    new CultureInfo("zh-CN")
                },
                SupportedUICultures = new[]
                {
                    new CultureInfo("zh-CN")
                }
            });



            //��������ģʽ���ж�ζ�ȡHttpContext.Body�е�����
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next.Invoke();
            });


            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //ע��ȫ���쳣�������
                app.UseExceptionHandler(builder => builder.Run(async context => await GlobalError.ErrorEvent(context)));
            }


            app.UseHsts();


            //ǿ���ض���Https
            app.UseHttpsRedirection();


            app.UseStaticFiles();


            //ע��Session
            app.UseSession();


            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();


            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new DashboardAuthorizationFilter() },
                DisplayStorageConnectionString = false
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            Tasks.Main.Run();


            app.Run();

        }




        public static IServiceProvider ServiceProvider { get; set; }
    }
}
