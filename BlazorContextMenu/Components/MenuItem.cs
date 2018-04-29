using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu.Components
{
    public class MenuItem : BlazorComponent
    {
        public string OverrideDefaultCssClass { get; set; }
        public string OverrideDefaultDisabledCssClass { get; set; }
        public string CssClass { get; set; }
        public string DisabledCssClass { get; set; }
        public RenderFragment ChildContent { get; set; }
        public Action<MenuItemEventArgs> Click { get; set; }
        public Func<MenuItemEventArgs, Task> ClickAsync { get; set; }
        protected ElementRef MenuItemElement { get; set; }
        public bool IsEnabled { get; set; } = true;
        public string Id { get; set; }
        protected string ClassCalc
        {
            get
            {
                if (IsEnabled)
                {
                    return (OverrideDefaultCssClass == null ? BlazorContextMenuDefaults.DefaultMenuItemCssClass : OverrideDefaultCssClass) + (CssClass == null ? "" : $" {CssClass}");
                }
                else
                {
                    return (OverrideDefaultDisabledCssClass == null ? BlazorContextMenuDefaults.DefaultMenuItemDisabledCssClass : OverrideDefaultDisabledCssClass) + (DisabledCssClass == null ? "" : $" {DisabledCssClass}");
                }
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            int seq = -1;
            builder.OpenElement(seq++, "li");
            builder.AddAttribute(seq++, "id", Id);
            builder.AddAttribute(seq++, "onclick", BindMethods.GetEventHandlerValue<UIMouseEventArgs>((e) => OnClickInternal(e)));
            builder.AddAttribute(seq++, "class", ClassCalc);
            builder.AddElementReferenceCapture(seq++, (reference) => MenuItemElement = reference );
            builder.AddContent(seq++, ChildContent);
            builder.CloseElement();
        }

        protected void OnClickInternal(UIMouseEventArgs e)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            var menuId = RegisteredFunction.Invoke<string>("BlazorContextMenu.MenuItem.GetMenuId", MenuItemElement);
            var menu = BlazorContextMenuHandler.GetMenu(menuId);
            var contextMenuTarget = menu.GetTarget();
            var args = new MenuItemEventArgs(e, menuId, contextMenuTarget, MenuItemElement, this);
            if (Click != null)
            {
                Click(args);
                if (!args.IsCanceled)
                {
                    BlazorContextMenuHandler.HideMenu(menuId);
                }
            }

            if (ClickAsync != null)
            {
                ClickAsync(args).ContinueWith((t) =>
                {
                    if (!args.IsCanceled)
                    {
                        BlazorContextMenuHandler.HideMenu(menuId);
                    }
                });
            }
        }
    }
}
