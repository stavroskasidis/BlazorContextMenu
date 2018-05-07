using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable BL9993,CS4014 // Component parameter is marked public

namespace BlazorContextMenu.Components
{
    public class Item : BlazorComponent
    {
                              /// <summary>
                              /// Allows you to override the default css class of the menu's li element for full customization.
                              /// If you want to override this for all menus, then you could use <see cref="BlazorContextMenuDefaults.DefaultMenuItemCssClass"/>
                              /// </summary>
        [Parameter]
        public string OverrideDefaultCssClass { get; protected set; }

        /// <summary>
        /// Allows you to override the default "disabled" css class of the menu's li element for full customization.
        /// If you want to override this for all menus, then you could use <see cref="BlazorContextMenuDefaults.DefaultMenuItemDisabledCssClass"/>
        /// </summary>
        [Parameter]
        public string OverrideDefaultDisabledCssClass { get; protected set; }

        /// <summary>
        /// Additional css class for the menu's li element. Use this to extend the default css
        /// </summary>
        [Parameter]
        public string CssClass { get; protected set; }

        /// <summary>
        /// Additional css class for the menu's li element when disabled. Use this to extend the default css
        /// </summary>
        [Parameter]
        public string DisabledCssClass { get; protected set; }

        /// <summary>
        /// The menu item's onclick handler. A <see cref="MenuItemClickEventArgs"/> is passed to the action.
        /// If you want to cancel the click (i.e. stop the menu from closing), then set the "IsCanceled" event args property to "true".
        /// Note: For an async handler use <seealso cref="ClickAsync"/>
        /// </summary>
        [Parameter]
        public Action<MenuItemClickEventArgs> Click { get; protected set; }

        /// <summary>
        /// The menu item's onclick async handler. A <see cref="MenuItemClickEventArgs"/> is passed to the action.
        /// If you want to cancel the click (i.e. stop the menu from closing), then set the "IsCanceled" event args property to "true".
        /// Note: For a synchronous handler use <seealso cref="Click"/>
        /// </summary>
        [Parameter]
        public Func<MenuItemClickEventArgs, Task> ClickAsync { get; protected set; }

        /// <summary>
        /// Sets the item's enabled state. Default <see cref="true" />
        /// </summary>
        [Parameter]
        public bool Enabled { get; protected set; } = true;

        /// <summary>
        /// A handler that can be used to set the item's <see cref="Enabled"/> status dynamically
        /// Note: For an async handler use <seealso cref="EnabledHandlerAsync"/>
        /// </summary>
        [Parameter]
        public Func<MenuItemHandlerArgs, bool> EnabledHandler { get; protected set; }

        /// <summary>
        /// A handler that can be used to set the item's <see cref="Enabled"/> status dynamically
        /// Note: For a synchronous handler use <seealso cref="EnabledHandler"/>
        /// </summary>
        [Parameter]
        public Func<MenuItemHandlerArgs, Task<bool>> EnabledHandlerAsync { get; protected set; }

        /// <summary>
        /// Sets the item's visible state. Default <see cref="true" />
        /// </summary>
        [Parameter]
        public bool Visible { get; protected set; } = true;

        /// <summary>
        /// A handler that can be used to set the item's <see cref="Visible"/> status dynamically
        /// Note: For an async handler use <seealso cref="VisibleHandlerAsync"/>
        /// </summary>
        [Parameter]
        public Func<MenuItemHandlerArgs, bool> VisibleHandler { get; protected set; }

        /// <summary>
        /// A handler that can be used to set the item's <see cref="Visible"/> status dynamically
        /// Note: For a synchronous handler use <seealso cref="VisibleHandler"/>
        /// </summary>
        [Parameter]
        public Func<MenuItemHandlerArgs, Task<bool>> VisibleHandlerAsync { get; protected set; }

        /// <summary>
        /// The id of the li element. This is optional
        /// </summary>
        [Parameter]
        public string Id { get; protected set; }

        /// <summary>
        /// Allows you to override the default x position offset of the submenu (i.e. the distance of the submenu from it's parent menu).
        /// If you want to override this for all menus, then you could use <see cref="BlazorContextMenuDefaults.SubMenuXPositionPixelsOffset"/>
        /// </summary>
        [Parameter]
        public int? SubmenuXOffset { get; protected set; }


        protected ElementRef MenuItemElement { get; set; }

        [Parameter]
        protected RenderFragment ChildContent { get; set; }

        protected string ClassCalc
        {
            get
            {
                if (Enabled)
                {
                    return (OverrideDefaultCssClass == null ? BlazorContextMenuDefaults.DefaultMenuItemCssClass : OverrideDefaultCssClass) + (CssClass == null ? "" : $" {CssClass}");
                }
                else
                {
                    return (OverrideDefaultDisabledCssClass == null ? BlazorContextMenuDefaults.DefaultMenuItemDisabledCssClass : OverrideDefaultDisabledCssClass) + (DisabledCssClass == null ? "" : $" {DisabledCssClass}");
                }
            }
        }

        protected override void OnInit()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }

            base.OnInit();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = -1;
            builder.OpenElement(seq++, "li");
            builder.AddAttribute(seq++, "id", Id);
            builder.AddAttribute(seq++, "onclick", BindMethods.GetEventHandlerValue<UIMouseEventArgs>((e) => OnClickInternal(e)));
            builder.AddAttribute(seq++, "class", "blazor-context-menu__item " + ClassCalc);
            builder.AddAttribute(seq++, "style", Visible ? "display:block;" : "display:none;");
            builder.AddAttribute(seq++, "item-enabled", Enabled.ToString().ToLower());
            builder.AddAttribute(seq++, "onmouseover", Enabled ? $"blazorContextMenu.OnMenuItemMouseOver(event, {SubmenuXOffset ?? BlazorContextMenuDefaults.SubMenuXPositionPixelsOffset}, this);" : "");
            builder.AddAttribute(seq++, "onmouseout", Enabled ? "blazorContextMenu.OnMenuItemMouseOut(event);" : "");

            builder.AddElementReferenceCapture(seq++, (reference) => MenuItemElement = reference);
            builder.AddContent(seq++, ChildContent);
            builder.CloseElement();

            base.BuildRenderTree(builder);
        }

        protected void OnClickInternal(UIMouseEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            var menuId = RegisteredFunction.Invoke<string>("BlazorContextMenu.MenuItem.GetMenuId", MenuItemElement);
            var menu = BlazorContextMenuHandler.GetMenu(menuId);
            var contextMenuTarget = menu.GetTarget();
            var args = new MenuItemClickEventArgs(e, menuId, contextMenuTarget, MenuItemElement, this);
            if (Click != null)
            {
                Click(args);
                if (!args.IsCanceled)
                {
                    BlazorContextMenuHandler.HideMenu(menuId);
                }
            }
            else if (ClickAsync != null)
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

        protected override void OnAfterRender()
        {
            if (EnabledHandler == null && EnabledHandlerAsync == null && VisibleHandler == null && VisibleHandlerAsync == null) return;

            var menuId = RegisteredFunction.Invoke<string>("BlazorContextMenu.MenuItem.GetMenuId", MenuItemElement);
            var menu = BlazorContextMenuHandler.GetMenu(menuId);
            var contextMenuTarget = menu.GetTarget();

            //menu is not showing. TODO: find a better way to figure this out 
            if (contextMenuTarget == null) return;

            //Hacky but works. TODO: Improve this code when this stuff is supported in Blazor
            Task.Run(async () =>
            {
                var oldEnabledValue = Enabled;
                var oldVisibleValue = Visible;

                var args = new MenuItemHandlerArgs(menuId, contextMenuTarget, MenuItemElement, this);
                if (EnabledHandler != null)
                {
                    Enabled = EnabledHandler(args);
                }
                else if (EnabledHandlerAsync != null)
                {
                    Enabled = await EnabledHandlerAsync(args);
                }

                if (VisibleHandler != null)
                {
                    Visible = VisibleHandler(args);
                }
                else if (VisibleHandlerAsync != null)
                {
                    Visible = await VisibleHandlerAsync(args);
                }

                if (oldEnabledValue != Enabled || oldVisibleValue != Visible)
                {
                    StateHasChanged();
                }
            });
        }
    }
}


#pragma warning restore BL9993, CS4014 // Component parameter is marked public