using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public static class BlazorContextMenuHandler
    {
        //TODO: Find a better way to manage menus
        private static Dictionary<string, ContextMenu> InitializedMenus = new Dictionary<string, ContextMenu>();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void Register(ContextMenu menu)
        {
            InitializedMenus[menu.Id] = menu;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static ContextMenu GetMenu(string id)
        {
            return InitializedMenus[id];
        }

        public static void ShowMenu(string id, string x, string y, string target)
        {
            if (InitializedMenus.ContainsKey(id))
            {
                InitializedMenus[id].Show(x,y, target);
            }
        }

        public static void HideMenu(string id)
        {
            if (InitializedMenus.ContainsKey(id))
            {
                InitializedMenus[id].Hide();
            }
        }
    }
}
