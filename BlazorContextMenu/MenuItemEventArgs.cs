using Microsoft.AspNetCore.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class MenuItemEventArgs
    {
        public MenuItemEventArgs(UIMouseEventArgs mouseEvent,string contextMenuId, ElementRef contextMenuTarget, ElementRef menuItem)
        {
            MouseEvent = mouseEvent;
            ContextMenuId = contextMenuId;
            ContextMenuTarget = contextMenuTarget;
            MenuItem = menuItem;
        }

        public UIMouseEventArgs MouseEvent { get; protected set; }
        public bool IsCanceled { get; set; }
        public string ContextMenuId { get; protected set; }
        public ElementRef ContextMenuTarget { get; protected set; }
        public ElementRef MenuItem { get; protected set; }
    }
}
