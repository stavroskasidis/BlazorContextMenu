using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class ItemClickEventArgs
    {
        public ItemClickEventArgs(MouseEventArgs mouseEvent,string contextMenuId, string contextMenuTargetId,
            ContextMenuTrigger trigger, ElementReference menuItemElement, Item menuItem, object data)
        {
            MouseEvent = mouseEvent;
            ContextMenuId = contextMenuId;
            ContextMenuTargetId = contextMenuTargetId;
            ContextMenuTrigger = trigger;
            MenuItemElement = menuItemElement;
            MenuItem = menuItem;
            Data = data;
        }

        /// <summary>
        /// The args of the mouse event.
        /// </summary>
        public MouseEventArgs MouseEvent { get; protected set; }
        
        /// <summary>
        /// If set to true, then the ContextMenu will not close after clicking on the item.
        /// </summary>
        public bool IsCanceled { get; set; }
        
        /// <summary>
        /// The id of the <see cref="ContextMenu"/> that contains the triggering item.
        /// </summary>
        public string ContextMenuId { get; protected set; }

        /// <summary>
        /// The <see cref="ContextMenuTrigger"/> that triggered this menu.
        /// </summary>
        public ContextMenuTrigger ContextMenuTrigger { get; protected set; }

        /// <summary>
        /// The id of the target element that the <see cref="ContextMenu"/> was triggered from.
        /// </summary>
        public string ContextMenuTargetId { get; protected set; }

        /// <summary>
        /// The ElementRef of the item's li that can be passed to javascript interop.
        /// </summary>
        public ElementReference MenuItemElement { get; protected set; }

        /// <summary>
        /// The menu item that triggered the event.
        /// </summary>
        public Item MenuItem { get; protected set; }

        /// <summary>
        /// Extra data that were passed to the <see cref="ContextMenu"/>.
        /// </summary>
        public object Data { get; protected set; }
    }
}
