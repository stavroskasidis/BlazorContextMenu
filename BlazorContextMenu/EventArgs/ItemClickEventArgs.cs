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

        /// <summary>
        /// The args of the mouse event.
        /// </summary>
        public UIMouseEventArgs MouseEvent { get; protected set; }
        
        /// <summary>
        /// If set to true, then the ContextMenu will not close after clicking on the item.
        /// </summary>
        public bool IsCanceled { get; set; }
        
        /// <summary>
        /// The id of the <see cref="ContextMenu"/> that contains the triggering item.
        /// </summary>
        public string ContextMenuId { get; protected set; }

        /// <summary>
        /// The id of the target element that the <see cref="ContextMenu"/> was triggered from.
        /// </summary>
        public string ContextMenuTargetId { get; protected set; }

        /// <summary>
        /// The ElementRef of the item's li. To be passed to javascript interop.
        /// </summary>
        public ElementRef MenuItemElement { get; protected set; }
        public Item MenuItem { get; protected set; }
    }
}
