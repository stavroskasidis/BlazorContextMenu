using BlazorContextMenu;
using BlazorContextMenu.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorContextMenu(this IServiceCollection services)
        {
            services.AddSingleton<BlazorContextMenuSettings>();

            CommonRegistrations(services);
            return services;
        }

        public static IServiceCollection AddBlazorContextMenu(this IServiceCollection services, Action<BlazorContextMenuSettingsBuilder> settings)
        {
            var settingsObj = new BlazorContextMenuSettings();
            var settingsBuilder = new BlazorContextMenuSettingsBuilder(settingsObj);
            settings(settingsBuilder);
            services.AddSingleton(settingsObj);

            CommonRegistrations(services);
            return services;
        }

        private static void CommonRegistrations(IServiceCollection services)
        {
            var traverser = new MenuTreeTraverser();
            services.AddSingleton(traverser);
            services.AddScoped<BlazorContextMenuHandler>();
        }

    }
}
