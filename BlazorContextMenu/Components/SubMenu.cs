using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class SubMenu : ContextMenu
    {
        protected override string BaseClass => "blazor-context-submenu blazor-context-menu__wrapper";

        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
            base.OnInitialized();
        }
    }
}
