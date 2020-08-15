using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore;
using System.IO;


namespace GiftSmrBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
            .UseUrls("http://localhost:8444/")
            .Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>();
    }
}
