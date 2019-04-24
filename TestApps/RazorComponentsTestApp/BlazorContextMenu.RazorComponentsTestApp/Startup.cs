using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorContextMenu.RazorComponentsTestApp.Services;
using BlazorContextMenu.TestAppsCommon;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorContextMenu.RazorComponentsTestApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<ISampleDataService, SampleDataService>();

            services.AddBlazorContextMenu(options =>
            {
                options.ConfigureTemplate(defaultTemplate =>
                {
                    defaultTemplate.MenuCssClass = "my-menu";
                    defaultTemplate.MenuItemCssClass = "my-menu-item";
                    defaultTemplate.SeperatorCssClass = "my-menu-seperator";
                });

                options.ConfigureTemplate("red", template =>
                {
                    template.MenuCssClass = "red-menu";
                    template.MenuItemCssClass = "red-menu-item";
                    template.MenuItemDisabledCssClass = "red-menu-item--disabled";
                    template.SeperatorHrCssClass = "red-menu-seperator-hr";
                    template.MenuItemWithSubMenuCssClass = "red-menu-item--with-submenu";
                    template.Animation = Animation.FadeIn;
                });

                options.ConfigureTemplate("dark", template =>
                {
                    template.MenuCssClass = "dark-menu";
                    template.MenuItemCssClass = "dark-menu-item";
                    template.MenuItemDisabledCssClass = "dark-menu-item--disabled";
                    template.MenuItemWithSubMenuCssClass = "dark-menu-item--with-submenu";
                    template.Animation = Animation.FadeIn;
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub<App>("app");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
