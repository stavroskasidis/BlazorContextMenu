using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorContextMenu
{
    public class MenuAppearingEventArgs
    {
        public MenuAppearingEventArgs(string contextMenuId, string contextMenuTargetId, string x, string y, string contextMenuTriggerId)
        {
            ContextMenuId = contextMenuId;
            ContextMenuTargetId = contextMenuTargetId;
            X = x;
            Y = y;
            ContextMenuTriggerId = contextMenuTriggerId;
        }

        /// <summary>
        /// The Id of the <see cref="ContextMenuTrigger"/> that is showing this menu.
        /// </summary>
        public string ContextMenuTriggerId { get; protected set; }

        /// <summary>
        /// The X position of the <see cref="ContextMenu"/>.
        /// </summary>
        public string X { get; set; }

        /// <summary>
        /// The Y position of the <see cref="ContextMenu"/>.
        /// </summary>
        public string Y { get; set; }

        /// <summary>
        /// The id of the <see cref="ContextMenu"/> that triggered this event.
        /// </summary>
        public string ContextMenuId { get; protected set; }

        /// <summary>
        /// The id of the target element that the <see cref="ContextMenu"/> was triggered from.
        /// </summary>
        public string ContextMenuTargetId { get; protected set; }

        /// <summary>
        /// If set to true, then the <see cref="ContextMenu"/> will not appear.
        /// </summary>
        public bool PreventShow { get; set; }
    }
}
