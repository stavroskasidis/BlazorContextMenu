using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class BlazorContextMenuSettings
    {
        public BlazorContextMenuDefaultCssSettings DefaultCssSettings { get; set; } = new BlazorContextMenuDefaultCssSettings();
        public BlazorContextMenuAdditionalCssSettings AdditionalCssSettings { get; set; } = new BlazorContextMenuAdditionalCssSettings();
        public int SubMenuXPositionPixelsOffset { get; set; } = 4;
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

    public class BlazorContextMenuAdditionalCssSettings
    {
        public string MenuCssClass { get; set; }
        public string MenuListCssClass { get; set; }
        public string MenuItemCssClass { get; set; }
        public string MenuItemDisabledCssClass { get; set; }
        public string SeperatorCssClass { get; set; }
        public string SeperatorHrCssClass { get; set; }
    }
}
