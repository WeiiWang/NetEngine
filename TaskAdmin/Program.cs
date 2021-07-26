using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TaskAdmin
{
    public class Program
    {
        public static void Main(string[] args)
        {
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

                    webBuilder.UseStartup<Startup>();
                });
    }
}
