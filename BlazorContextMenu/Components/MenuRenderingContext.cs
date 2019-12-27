using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorContextMenu
{
    public class MenuRenderingContext
    {
        public MenuRenderingContext(string contextMenuId, string contextMenuTargetId, string x, string y, ContextMenuTrigger trigger, object data)
        {
            ContextMenuId = contextMenuId;
            ContextMenuTargetId = contextMenuTargetId;
            ContextMenuTrigger = trigger;
            Data = data;
            X = x;
            Y = y;
        }


        /// <summary>
        /// The <see cref="ContextMenuTrigger"/> that triggered this menu.
        /// </summary>
        public ContextMenuTrigger ContextMenuTrigger { get; protected set; }

        /// <summary>
        /// The X position of the <see cref="ContextMenu"/>.
        /// </summary>
        public string X { get; protected set; }

        /// <summary>
        /// The Y position of the <see cref="ContextMenu"/>.
        /// </summary>
        public string Y { get; protected set; }

        /// <summary>
        /// The id of the <see cref="ContextMenu"/> that triggered this event.
        /// </summary>
        public string ContextMenuId { get; protected set; }

        /// <summary>
        /// The id of the target element that the <see cref="ContextMenu"/> was triggered from.
        /// </summary>
        public string ContextMenuTargetId { get; protected set; }

        /// <summary>
        /// Extra data that were passed to the <see cref="ContextMenu"/>.
        /// </summary>
        public object Data { get; protected set; }
    }
}
