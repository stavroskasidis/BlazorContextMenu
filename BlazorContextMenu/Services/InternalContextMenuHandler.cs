using BlazorContextMenu.Services;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public interface IInternalContextMenuHandler
    {
        bool ReferencePassedToJs { get; set; }
        Task HideMenu(string id);
        Task ShowMenu(string id, string x, string y, string targetId = null, DotNetObjectReference<ContextMenuTrigger> trigger = null);
    }

    public class InternalContextMenuHandler : IInternalContextMenuHandler
    {
        private readonly IContextMenuStorage _contextMenuStorage;

        public InternalContextMenuHandler(IContextMenuStorage contextMenuStorage)
        {
            _contextMenuStorage = contextMenuStorage;
        }

        public bool ReferencePassedToJs { get; set; } = false;

        /// <summary>
        /// Shows the context menu at the specified coordinates.
        /// </summary>
        /// <param name="id">The id of the menu.</param>
        /// <param name="x">The x coordinate on the screen.</param>
        /// <param name="y">The y coordinate on the screen.</param>
        /// <param name="targetId">Optional: The id of the element that triggered the menu show event.</param>
        /// <param name="trigger">Optional: The <see cref="ContextMenuTrigger"/> that opened the menu.</param>
        /// <returns></returns>
        [JSInvokable]
        public async Task ShowMenu(string id, string x, string y, string targetId = null, DotNetObjectReference<ContextMenuTrigger> trigger = null)
        {
            var menu = _contextMenuStorage.GetMenu(id);
            if (menu != null)
            {
                await menu.Show(x, y, targetId, trigger?.Value);
            }
        }

        /// <summary>
        /// Hides a context menu
        /// </summary>
        /// <param name="id">The id of the menu.</param>
        [JSInvokable]
        public async Task HideMenu(string id)
        {
            var menu = _contextMenuStorage.GetMenu(id);
            if (menu != null)
            {
                await menu.Hide();
            }
        }
    }
}
