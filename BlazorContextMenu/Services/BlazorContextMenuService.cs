using BlazorContextMenu.Services;
using Microsoft.JSInterop;
using System;
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

        /// <summary>Determines if a <see cref="ContextMenu" /> is already being shown.</summary>
        /// <param name="id">The id of the <see cref="ContextMenu"/>.</param>
        /// <returns>True if the <see cref="ContextMenu" /> is being shown, otherwise False.</returns>
        Task<bool> IsMenuShown(string id);
    }

    public class BlazorContextMenuService : IBlazorContextMenuService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IContextMenuStorage _contextMenuStorage;

        public BlazorContextMenuService(IJSRuntime jSRuntime, IContextMenuStorage contextMenuStorage)
        {
            _jSRuntime = jSRuntime;
            _contextMenuStorage = contextMenuStorage;
        }

        public async Task HideMenu(string id)
        {
            var menu = _contextMenuStorage.GetMenu(id);
            if (menu == null)
            {
                throw new Exception($"No context menu with id '{id}' was found");
            }
            await _jSRuntime.InvokeVoidAsync("blazorContextMenu.Hide", id);
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

        public async Task<bool> IsMenuShown(string id) => await _jSRuntime.InvokeAsync<bool>("blazorContextMenu.IsMenuShown", id);
    }
}
