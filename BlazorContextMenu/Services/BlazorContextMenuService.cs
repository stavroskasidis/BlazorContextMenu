using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public interface IBlazorContextMenuService
    {
        Task HideMenu(string id);
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

        public async Task ShowMenu(string id, int x, int y)
        {
            await _jSRuntime.InvokeVoidAsync("blazorContextMenu.ManualShow", id, x, y);
        }
    }
}
