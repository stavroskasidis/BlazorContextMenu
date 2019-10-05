using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorContextMenu.Services
{
    public interface IContextMenuStorage
    {
        ContextMenu GetMenu(string id);
        void Register(ContextMenu menu);
        void Unregister(ContextMenu menu);
    }

    public class ContextMenuStorage : IContextMenuStorage
    {
        private Dictionary<string, ContextMenu> _initializedMenus = new Dictionary<string, ContextMenu>();

        public void Register(ContextMenu menu)
        {
            _initializedMenus[menu.Id] = menu;
        }
        public void Unregister(ContextMenu menu)
        {
            _initializedMenus.Remove(menu.Id);
        }

        public ContextMenu GetMenu(string id)
        {
            if (_initializedMenus.ContainsKey(id))
            {
                return _initializedMenus[id];
            }

            return null;
        }

    }
}
