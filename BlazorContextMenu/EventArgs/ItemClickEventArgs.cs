using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class ItemClickEventArgs
    {
        public ItemClickEventArgs(UIMouseEventArgs mouseEvent,string contextMenuId, string contextMenuTargetId, ElementRef menuItemElement, Item menuItem)
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
        public Item MenuItem { get; protected set; }
    }
}
