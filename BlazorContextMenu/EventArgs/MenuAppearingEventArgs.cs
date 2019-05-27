using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorContextMenu
{
    public class MenuAppearingEventArgs
    {
        public MenuAppearingEventArgs(string contextMenuId, string contextMenuTargetId)
        {
            ContextMenuId = contextMenuId;
            ContextMenuTargetId = contextMenuTargetId;
        }

        /// <summary>
        /// The id of the <see cref="ContextMenu"/> that triggered this event.
        /// </summary>
        public string ContextMenuId { get; protected set; }

        /// <summary>
        /// The id of the target element that the <see cref="ContextMenu"/> was triggered from.
        /// </summary>
        public string ContextMenuTargetId { get; protected set; }

        /// <summary>
        /// If set to true, then the menu will not appear.
        /// </summary>
        public bool PreventShow { get; set; }
    }
}
