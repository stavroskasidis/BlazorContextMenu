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
        /// <param name="id">The id of the <see cref="ContextMenu"/></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="autoClose">Set to false if you want to close the menu programmatically. Default: true</param>
        /// <returns></returns>
        Task ShowMenu(string id, int x, int y, bool autoClose);

        /// <summary>
        /// Shows a <see cref="ContextMenu" /> programmatically.
        /// </summary>
        /// <param name="id">The id of the <see cref="ContextMenu"/></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        Task ShowMenu(string id, int x, int y);
    }

    public class BlazorContextMenuService : IBlazorContextMenuService
    {
        private readonly IInternalContextMenuHandler _internalContextMenuHandler;
        private readonly IJSRuntime _jSRuntime;

        public BlazorContextMenuService(IInternalContextMenuHandler internalContextMenuHandler, IJSRuntime jSRuntime)
        {
            _internalContextMenuHandler = internalContextMenuHandler;
            _jSRuntime = jSRuntime;
        }

        public async Task HideMenu(string id)
        {
            await _internalContextMenuHandler.HideMenu(id);
        }
        
        public async Task ShowMenu(string id, int x, int y, bool autoClose)
        {
            await _jSRuntime.InvokeVoidAsync("blazorContextMenu.ManualShow", id, x, y, autoClose);
        }

        public Task ShowMenu(string id, int x, int y)
        {
            return ShowMenu(id, x, y, true);
        }
    }
}
