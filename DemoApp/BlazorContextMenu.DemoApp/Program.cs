using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlazorContextMenu.DemoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
             BlazorWebAssemblyHost.CreateDefaultBuilder()
                 .UseBlazorStartup<Startup>();
    }
}
