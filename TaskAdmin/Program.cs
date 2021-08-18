using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace TaskAdmin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Common.EnvironmentHelper.ChangeDirectory(args);
            Common.EnvironmentHelper.InitTestServer();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    //���� Kestrel Https ����֤��
                    //webBuilder.UseKestrel(options =>
                    //{
                    //    options.ConfigureHttpsDefaults(options =>
                    //    {
                    //        options.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(Path.Combine(AppContext.BaseDirectory, "xxxx.pfx"), "123456");
                    //    });
                    //});
                    //webBuilder.UseUrls("https://*");

                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseStartup<Startup>();
                });



        public static IServiceProvider ServiceProvider { get; set; }
    }
}
