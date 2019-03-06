using BlazorContextMenu.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public abstract class MenuItemHandlerArgs
    {
        public MenuItemHandlerArgs(string contextMenuId, string contextMenuTargetId, Item menuItem)
        {
            ContextMenuId = contextMenuId;
            ContextMenuTargetId = contextMenuTargetId;
            MenuItem = menuItem;
        }

        public string ContextMenuId { get; protected set; }
        public string ContextMenuTargetId { get; protected set; }
        public Item MenuItem { get; protected set; }
    }

    public class MenuItemEnabledHandlerArgs : MenuItemHandlerArgs
    {
        public MenuItemEnabledHandlerArgs(string contextMenuId, string contextMenuTargetId, Item menuItem, bool isEnabled) : base(contextMenuId, contextMenuTargetId, menuItem)
        {
            IsEnabled = isEnabled;
        }
        public bool IsEnabled { get; set; }
    }

    public class MenuItemVisibleHandlerArgs : MenuItemHandlerArgs
    {
        public MenuItemVisibleHandlerArgs(string contextMenuId, string contextMenuTargetId, Item menuItem, bool isVisible) : base(contextMenuId, contextMenuTargetId, menuItem)
        {
            IsVisible = isVisible;
        }

        public bool IsVisible { get; set; }
    }
}
