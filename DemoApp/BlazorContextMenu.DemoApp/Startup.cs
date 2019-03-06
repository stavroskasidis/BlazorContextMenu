using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu.DemoApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazorContextMenu(options=>
            {
                options.ConfigureTemplate("dark", template =>
                {
                    template.MenuCssClass = "dark-menu";
                    template.MenuItemCssClass = "dark-menu-item";
                    template.MenuItemDisabledCssClass = "dark-menu-item--disabled";
                    template.SeperatorHrCssClass = "dark-menu-seperator-hr";
                    template.MenuItemWithSubMenuCssClass = "dark-menu-item--with-submenu";
                    template.Animation = Animation.FadeIn;
                });
            });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
