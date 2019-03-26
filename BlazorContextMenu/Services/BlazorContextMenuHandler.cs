using BlazorContextMenu.Components;
using BlazorContextMenu.Services;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class BlazorContextMenuHandler
    {
        private Dictionary<string, ContextMenu> _initializedMenus = new Dictionary<string, ContextMenu>();

        internal void Register(ContextMenu menu)
        {
            _initializedMenus[menu.Id] = menu;
        }

        internal bool ReferencePassedToJs { get; set; } = false;

        internal void Unregister(ContextMenu menu)
        {
            _initializedMenus.Remove(menu.Id);
        }

        internal ContextMenu GetMenu(string id)
        {
            return _initializedMenus[id];
        }

        [JSInvokable]
        public void ShowMenu(string id, string x, string y, string target)
        {
            if (_initializedMenus.ContainsKey(id))
            {
                _initializedMenus[id].Show(x, y, target);
            }
        }

        [JSInvokable]
        public void HideMenu(string id)
        {
            if (_initializedMenus.ContainsKey(id))
            {
                var subMenus = GetSubMenus(_initializedMenus[id]);
                foreach(var submenu in subMenus)
                {
                    submenu.Hide();
                }
                _initializedMenus[id].Hide();
            }
        }

        private IEnumerable<SubMenu> GetSubMenus(MenuTreeComponent treeComponent)
        {
            var result = new List<SubMenu>();
            var childComponents = treeComponent.GetChildComponents();
            var subMenus = childComponents.Where(x => x is SubMenu).Cast<SubMenu>();
            result.AddRange(subMenus);
            foreach (var child in childComponents)
            {
                result.AddRange(GetSubMenus(child));
            }

            return result;
        }
    }
}
