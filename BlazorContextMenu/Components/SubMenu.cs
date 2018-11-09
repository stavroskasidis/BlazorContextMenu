
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu.Components
{
    public class SubMenu : ContextMenu
    {
        protected override string BaseClass => "blazor-context-submenu blazor-context-menu__wrapper";

        protected override void OnInit()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
            base.OnInit();
        }
    }
}
