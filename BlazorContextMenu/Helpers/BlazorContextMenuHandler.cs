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
    public class BlazorContextMenuHandler
    {
        private Dictionary<string, ContextMenu> InitializedMenus = new Dictionary<string, ContextMenu>();

        internal void Register(ContextMenu menu)
        {
            InitializedMenus[menu.Id] = menu;
        }

        internal bool ReferencePassedToJs { get; set; } = false;

        internal void Unregister(ContextMenu menu)
        {
            InitializedMenus.Remove(menu.Id);
        }

        internal ContextMenu GetMenu(string id)
        {
            return InitializedMenus[id];
        }

        [JSInvokable]
        public void ShowMenu(string id, string x, string y, string target)
        {
            if (InitializedMenus.ContainsKey(id))
            {
                InitializedMenus[id].Show(x, y, target);
            }
        }

        [JSInvokable]
        public void HideMenu(string id)
        {
            if (InitializedMenus.ContainsKey(id))
            {
                InitializedMenus[id].Hide();
            }
        }
    }
}
