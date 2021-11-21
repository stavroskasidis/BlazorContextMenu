using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorContextMenu.DemoApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddBlazorContextMenu(options =>
            {
                options.ConfigureTemplate("dark", template =>
                {
                    template.MenuCssClass = "dark-menu";
                    template.MenuItemCssClass = "dark-menu-item";
                    template.MenuItemDisabledCssClass = "dark-menu-item--disabled";
                    template.MenuItemWithSubMenuCssClass = "dark-menu-item--with-submenu";
                    template.Animation = Animation.FadeIn;
                });

                options.ConfigureTemplate("pink", template =>
                {
                    template.MenuCssClass = "pink-menu";
                    template.MenuItemCssClass = "pink-menu-item";
                    template.MenuItemDisabledCssClass = "pink-menu-item--disabled";
                    template.SeperatorHrCssClass = "pink-menu-seperator-hr";
                    template.MenuItemWithSubMenuCssClass = "pink-menu-item--with-submenu";
                    template.Animation = Animation.Slide;
                });
            });

            await builder.Build().RunAsync();

        }
    }
}
