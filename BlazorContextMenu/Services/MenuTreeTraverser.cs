using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu.Services
{
    public interface IMenuTreeTraverser
    {
        ContextMenuBase GetClosestContextMenu(MenuTreeComponent menuTreeComponent);
        ContextMenu GetRootContextMenu(MenuTreeComponent menuTreeComponent);
        bool HasSubMenu(MenuTreeComponent menuTreeComponent);
    }

    public class MenuTreeTraverser : IMenuTreeTraverser
    {
        public ContextMenu GetRootContextMenu(MenuTreeComponent menuTreeComponent)
        {
            if (menuTreeComponent.ParentComponent == null) return null;
            if (menuTreeComponent.ParentComponent.GetType() == typeof(ContextMenu)) return menuTreeComponent.ParentComponent as ContextMenu;
            return GetRootContextMenu(menuTreeComponent.ParentComponent);
        }

        public ContextMenuBase GetClosestContextMenu(MenuTreeComponent menuTreeComponent)
        {
            if (menuTreeComponent.ParentComponent == null) return null;
            if (typeof(ContextMenuBase).IsAssignableFrom(menuTreeComponent.ParentComponent.GetType())) return menuTreeComponent.ParentComponent as ContextMenuBase;
            return GetClosestContextMenu(menuTreeComponent.ParentComponent);
        }

        public bool HasSubMenu(MenuTreeComponent menuTreeComponent)
        {
            var children = menuTreeComponent.GetChildComponents();
            if (children.Any(x => x is SubMenu)) return true;
            foreach (var child in children)
            {
                if (HasSubMenu(child)) return true;
            }

            return false;
        }

    }
}
