using System.IO;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using SudokuApp.Api;

namespace SudokuApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                  .UseContentRoot(Directory.GetCurrentDirectory())
                  .UseNLog()
                  .UseKestrel()
                  .UseIISIntegration()
                  .UseStartup<Startup>();
        }
            
    }
}
