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
        /// <summary>
        /// Allows you to override the default css class of the menu's li element for full customization.
        /// If you want to override this for all menus, then you could use <see cref="BlazorContextMenuDefaults.DefaultMenuItemCssClass"/>
        /// </summary>
        public string OverrideDefaultCssClass { get; set; }

        /// <summary>
        /// Allows you to override the default "disabled" css class of the menu's li element for full customization.
        /// If you want to override this for all menus, then you could use <see cref="BlazorContextMenuDefaults.DefaultMenuItemDisabledCssClass"/>
        /// </summary>
        public string OverrideDefaultDisabledCssClass { get; set; }

        /// <summary>
        /// Additional css class for the menu's li element. Use this to extend the default css
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Additional css class for the menu's li element when disabled. Use this to extend the default css
        /// </summary>
        public string DisabledCssClass { get; set; }

        /// <summary>
        /// The menu item's onclick handler. A <see cref="MenuItemEventArgs"/> is passed to the action.
        /// If you want to cancel the click (i.e. stop the menu from closing), then set the "IsCanceled" event args property to "true".
        /// Note: For an async handler use <seealso cref="ClickAsync"/>
        /// </summary>
        public Action<MenuItemEventArgs> Click { get; set; }

        /// <summary>
        /// The menu item's onclick async handler. A <see cref="MenuItemEventArgs"/> is passed to the action.
        /// If you want to cancel the click (i.e. stop the menu from closing), then set the "IsCanceled" event args property to "true".
        /// Note: For a synchronous handler use <seealso cref="Click"/>
        /// </summary>
        public Func<MenuItemEventArgs, Task> ClickAsync { get; set; }

        /// <summary>
        /// Sets the item's enabled state. Default <see cref="true" />
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// The id of the li element. This is optional
        /// </summary>
        public string Id { get; set; }
        

        protected ElementRef MenuItemElement { get; set; }

        public RenderFragment ChildContent { get; set; }

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
