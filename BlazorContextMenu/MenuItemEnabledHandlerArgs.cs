using BlazorContextMenu.Components;
using Microsoft.AspNetCore.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class MenuItemEnabledHandlerArgs
    {
        public MenuItemEnabledHandlerArgs(string contextMenuId, string contextMenuTargetId, ElementRef menuItemElement, Item menuItem)
        {
            ContextMenuId = contextMenuId;
            ContextMenuTargetId = contextMenuTargetId;
            MenuItemElement = menuItemElement;
            MenuItem = menuItem;
        }

        public string ContextMenuId { get; protected set; }
        public string ContextMenuTargetId { get; protected set; }
        public ElementRef MenuItemElement { get; protected set; }
        public Item MenuItem { get; protected set; }
    }
}
