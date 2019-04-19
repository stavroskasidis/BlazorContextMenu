using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class ItemAppearingEventArgs
    {
        public ItemAppearingEventArgs(string contextMenuId, string contextMenuTargetId, Item menuItem, bool isVisible, bool isEnabled)
        {
            ContextMenuId = contextMenuId;
            ContextMenuTargetId = contextMenuTargetId;
            MenuItem = menuItem;
            IsEnabled = isEnabled;
            IsVisible = isVisible;
        }

        public string ContextMenuId { get; protected set; }
        public string ContextMenuTargetId { get; protected set; }
        public Item MenuItem { get; protected set; }
        public bool IsVisible { get; set; }
        public bool IsEnabled { get; set; }
    }
}
