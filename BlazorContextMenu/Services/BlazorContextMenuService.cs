using BlazorContextMenu.Services;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public interface IBlazorContextMenuService
    {
        /// <summary>
        /// Hides a <see cref="ContextMenu" /> programmatically.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task HideMenu(string id);

        /// <summary>
        /// Shows a <see cref="ContextMenu" /> programmatically.
        /// </summary>
        /// <param name="id">The id of the <see cref="ContextMenu"/>.</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        Task ShowMenu(string id, int x, int y);

        /// <summary>
        /// Shows a <see cref="ContextMenu" /> programmatically.
        /// </summary>
        /// <param name="id">The id of the <see cref="ContextMenu"/>.</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="data">Extra data that will be passed to menu events.</param>
        /// <returns></returns>
        Task ShowMenu(string id, int x, int y, object data);
    }

    public class BlazorContextMenuService : IBlazorContextMenuService
    {
        private readonly IInternalContextMenuHandler _internalContextMenuHandler;
        private readonly IJSRuntime _jSRuntime;
        private readonly IContextMenuStorage _contextMenuStorage;

        public BlazorContextMenuService(IInternalContextMenuHandler internalContextMenuHandler, IJSRuntime jSRuntime, IContextMenuStorage contextMenuStorage)
        {
            _internalContextMenuHandler = internalContextMenuHandler;
            _jSRuntime = jSRuntime;
            _contextMenuStorage = contextMenuStorage;
        }

        public async Task HideMenu(string id)
        {
            await _internalContextMenuHandler.HideMenu(id);
        }
        
        public async Task ShowMenu(string id, int x, int y, object data)
        {
            var menu = _contextMenuStorage.GetMenu(id);
            if(menu == null)
            {
                throw new Exception($"No context menu with id '{id}' was found");
            }
            menu.Data = data;
            await _jSRuntime.InvokeVoidAsync("blazorContextMenu.ManualShow", id, x, y);
        }

        public Task ShowMenu(string id, int x, int y)
        {
            return ShowMenu(id, x, y, null);
        }
    }
}
