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

        /// <summary>
        /// The id of the <see cref="ContextMenu"/> that contains the triggering item.
        /// </summary>
        public string ContextMenuId { get; protected set; }

        /// <summary>
        /// The id of the target element that the <see cref="ContextMenu"/> was triggered from.
        /// </summary>
        public string ContextMenuTargetId { get; protected set; }

        /// <summary>
        /// The menu item that triggered the event.
        /// </summary>
        public Item MenuItem { get; protected set; }

        /// <summary>
        /// Can be used to set the item visible or not dynamically
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        ///  Can be used to set the item enabled or not dynamically
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
