using BlazorContextMenu;
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
            var settings = new BlazorContextMenuSettings();
            services.AddSingleton(settings);

            return services;
        }

        public static IServiceCollection AddBlazorContextMenu(this IServiceCollection services, Action<BlazorContextMenuSettingsBuilder> settings)
        {
            var settingsObj = new BlazorContextMenuSettings();
            var settingsBuilder = new BlazorContextMenuSettingsBuilder(settingsObj);
            settings(settingsBuilder);
            services.AddSingleton(settingsObj);
            return services;
        }

    }
}
