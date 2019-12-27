using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorContextMenu.Services
{
    public interface IContextMenuStorage
    {
        ContextMenuBase GetMenu(string id);
        void Register(ContextMenuBase menu);
        void Unregister(ContextMenuBase menu);
    }

    public class ContextMenuStorage : IContextMenuStorage
    {
        private Dictionary<string, ContextMenuBase> _initializedMenus = new Dictionary<string, ContextMenuBase>();

        public void Register(ContextMenuBase menu)
        {
            _initializedMenus[menu.Id] = menu;
        }
        public void Unregister(ContextMenuBase menu)
        {
            _initializedMenus.Remove(menu.Id);
        }

        public ContextMenuBase GetMenu(string id)
        {
            if (_initializedMenus.ContainsKey(id))
            {
                return _initializedMenus[id];
            }

            return null;
        }

    }
}
