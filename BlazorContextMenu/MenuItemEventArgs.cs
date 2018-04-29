using BlazorContextMenu.Components;
using Microsoft.AspNetCore.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class MenuItemEventArgs
    {
        public MenuItemEventArgs(UIMouseEventArgs mouseEvent,string contextMenuId, string contextMenuTargetId, ElementRef menuItemElement, MenuItem menuItem)
        {
            MouseEvent = mouseEvent;
            ContextMenuId = contextMenuId;
            ContextMenuTargetId = contextMenuTargetId;
            MenuItemElement = menuItemElement;
            MenuItem = menuItem;
        }

        public UIMouseEventArgs MouseEvent { get; protected set; }
        public bool IsCanceled { get; set; }
        public string ContextMenuId { get; protected set; }
        public string ContextMenuTargetId { get; protected set; }
        public ElementRef MenuItemElement { get; protected set; }
        public MenuItem MenuItem { get; protected set; }
    }
}
