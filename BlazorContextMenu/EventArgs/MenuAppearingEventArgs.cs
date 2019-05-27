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

        public string ContextMenuId { get; protected set; }

        public string ContextMenuTargetId { get; protected set; }

        /// <summary>
        /// If set to true, then the menu will not appear.
        /// </summary>
        public bool IsCanceled { get; set; }
    }
}
