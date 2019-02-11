using BlazorContextMenu.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class MenuItemHandlerArgs
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
}
