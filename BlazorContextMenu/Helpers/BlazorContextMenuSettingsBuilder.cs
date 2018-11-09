using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class BlazorContextMenuSettingsBuilder
    {
        private readonly BlazorContextMenuSettings _settings;

        public BlazorContextMenuSettingsBuilder(BlazorContextMenuSettings settings)
        {
            _settings = settings;
        }
        
        public BlazorContextMenuSettingsBuilder CssOverrides(Action<BlazorContextMenuDefaultCssSettings> overrides)
        {
            overrides(_settings.DefaultCssSettings); 
            return this;
        }

        /// <summary>
        /// Configures the default template.
        /// </summary>
        /// <param name="templateOptions"></param>
        /// <returns></returns>
        public BlazorContextMenuSettingsBuilder ConfigureTemplate(Action<BlazorContextMenuTemplate> templateOptions)
        {
            var template = _settings.GetTemplate(BlazorContextMenuSettings.DefaultTemplateName);
            templateOptions(template);
            return this;
        }

        /// <summary>
        /// Configures a named template.
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="templateOptions"></param>
        /// <returns></returns>
        public BlazorContextMenuSettingsBuilder ConfigureTemplate(string templateName,Action<BlazorContextMenuTemplate> templateOptions)
        {
            var template = new BlazorContextMenuTemplate();
            templateOptions(template);
            _settings.Templates.Add(templateName, template);
            return this;
        }

    }
}
