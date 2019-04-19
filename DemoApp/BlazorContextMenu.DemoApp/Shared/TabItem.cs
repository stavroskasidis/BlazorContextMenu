using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu.DemoApp.Shared
{
    public class TabItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string HeaderText { get; set; }
        public RenderFragment Contents { get; set; }
    }
}
