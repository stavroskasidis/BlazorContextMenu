using BlazorContextMenu.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu.Services
{
    internal class MenuTreeTraverser
    {
        public ContextMenu GetClosestContextMenuParent(MenuTreeComponent menuTreeComponent)
        {
            if (menuTreeComponent.ParentComponent == null) return null;
            if (menuTreeComponent.ParentComponent.GetType() == typeof(ContextMenu)) return menuTreeComponent.ParentComponent as ContextMenu;
            return GetClosestContextMenuParent(menuTreeComponent.ParentComponent);
        }

        public bool HasSubMenu(MenuTreeComponent menuTreeComponent)
        {
            var children = menuTreeComponent.GetChildComponents();
            if (children.Any(x => x.GetType() == typeof(SubMenu))) return true;
            foreach(var child in children)
            {
                if (HasSubMenu(child)) return true;
            }

            return false;
        }

        //public void Debug_PrintChildInfo(MenuTreeComponent menuTreeComponent, int level)
        //{
        //    foreach(var child in menuTreeComponent.GetChildComponents())
        //    {
        //        if(level - 1 >= 0)
        //        {
        //            var levelChars = new string('─', (level - 1) * 2);
        //            Console.Write("├─" + levelChars);
        //        }
        //        Console.WriteLine(child.GetType().Name);

        //        Debug_PrintChildInfo(child, level + 1);
        //    }
        //}
    }
}
