using BlazorContextMenu.Components;
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
        //TODO: Find a better way to keep references
        private static Dictionary<string, ContextMenu> InitializedMenus = new Dictionary<string, ContextMenu>();
        private static Dictionary<string, Item> InitializedMenuItems = new Dictionary<string, Item>();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void Register(ContextMenu menu)
        {
            InitializedMenus[menu.GetId()] = menu;
        }

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public static void RegisterMenuItem(Item menuItem)
        //{
        //    InitializedMenuItems[menuItem.GetId()] = menuItem;
        //}

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

        //public static void CalculateMenuItemEnabled(string id)
        //{
        //    //if (InitializedMenuItems.ContainsKey(id))
        //    //{
        //    //    InitializedMenuItems[id].CalculateEnabled();
        //    //}
        //}
    }
}
