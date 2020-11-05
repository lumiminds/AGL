using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AGL.Services;

namespace AGL.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((ctx, builder) =>
            {
                builder.ConfigureEnvironmnt(ctx.HostingEnvironment.ContentRootPath, ctx.HostingEnvironment.EnvironmentName);
            })
            .UseStartup<Startup>();
    }
}
