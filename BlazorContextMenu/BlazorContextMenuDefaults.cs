using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public static class BlazorContextMenuDefaults
    {
        public static string DefaultMenuCssClass { get; set; } = "blazor-context-menu--default";
        public static string DefaultMenuListCssClass { get; set; } = "blazor-context-menu__list";
        public static string DefaultMenuItemCssClass { get; set; } = "blazor-context-menu__item";
        public static string DefaultMenuItemDisabledCssClass { get; set; } = "blazor-context-menu__item--disabled";
        public static string DefaultSeperatorCssClass { get; set; } = "blazor-context-menu__seperator";
        public static string DefaultSeperatorHrCssClass { get; set; } = "blazor-context-menu__seperator__hr";
        public static int SubMenuXPositionPixelsOffset { get; set; } = 4;
    }
}
