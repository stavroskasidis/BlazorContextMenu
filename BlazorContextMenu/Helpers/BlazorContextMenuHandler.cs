using BlazorContextMenu.Components;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public static class BlazorContextMenuHandler
    {
        //TODO: Find a better way to keep references
        private static Dictionary<string, ContextMenu> InitializedMenus = new Dictionary<string, ContextMenu>();

        public static void Register(ContextMenu menu)
        {
            InitializedMenus[menu.Id] = menu;
        }

        public static void Unregister(ContextMenu menu)
        {
            InitializedMenus.Remove(menu.Id);
        }

        public static ContextMenu GetMenu(string id)
        {
            return InitializedMenus[id];
        }

        [JSInvokable]
        public static void ShowMenu(string id, string x, string y, string target)
        {
            if (InitializedMenus.ContainsKey(id))
            {
                InitializedMenus[id].Show(x, y, target);
            }
        }

        [JSInvokable]
        public static void HideMenu(string id)
        {
            if (InitializedMenus.ContainsKey(id))
            {
                InitializedMenus[id].Hide();
            }
        }
    }
}
