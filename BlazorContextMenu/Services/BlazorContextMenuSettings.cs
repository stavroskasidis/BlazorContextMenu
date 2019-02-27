using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class BlazorContextMenuSettings
    {
        public const string DefaultTemplateName = "default_{89930AFB-8CC8-4672-80D1-EA8BBE65B52A}";
        public BlazorContextMenuDefaultCssSettings DefaultCssSettings { get; set; } = new BlazorContextMenuDefaultCssSettings();
        public Dictionary<string, BlazorContextMenuTemplate> Templates = new Dictionary<string, BlazorContextMenuTemplate>()
        {
            { DefaultTemplateName, new BlazorContextMenuTemplate() }
        };

        public BlazorContextMenuTemplate GetTemplate(string templateName)
        {
            if (!Templates.ContainsKey(templateName)) throw new Exception($"Template '{templateName}' not found");
            return Templates[templateName];
        }
    }

    public class BlazorContextMenuDefaultCssSettings
    {
        public string MenuCssClass { get; set; } = "blazor-context-menu--default";
        public string MenuListCssClass { get; set; } = "blazor-context-menu__list";
        public string MenuItemCssClass { get; set; } = "blazor-context-menu__item--default";
        public string MenuItemDisabledCssClass { get; set; } = "blazor-context-menu__item--default-disabled";
        public string SeperatorCssClass { get; set; } = "blazor-context-menu__seperator";
        public string SeperatorHrCssClass { get; set; } = "blazor-context-menu__seperator__hr";
    }

    public class BlazorContextMenuTemplate
    {
        public string MenuCssClass { get; set; }
        public string MenuListCssClass { get; set; }
        public string MenuItemCssClass { get; set; }
        public string MenuItemDisabledCssClass { get; set; }
        public string SeperatorCssClass { get; set; }
        public string SeperatorHrCssClass { get; set; }
        public int SubMenuXPositionPixelsOffset { get; set; } = 4;
        public Animation Animation {get; set;}
    }

    public enum Animation
    {
        None,
        FadeIn
    }
}
