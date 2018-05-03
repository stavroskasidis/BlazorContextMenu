using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Browser.Interop;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu.Components
{
    public class Item : BlazorComponent
    {
        /// <summary>
        /// Allows you to override the default css class of the menu's li element for full customization.
        /// If you want to override this for all menus, then you could use <see cref="BlazorContextMenuDefaults.DefaultMenuItemCssClass"/>
        /// </summary>
        [Parameter]
        protected string OverrideDefaultCssClass { get; set; }

        /// <summary>
        /// Allows you to override the default "disabled" css class of the menu's li element for full customization.
        /// If you want to override this for all menus, then you could use <see cref="BlazorContextMenuDefaults.DefaultMenuItemDisabledCssClass"/>
        /// </summary>
        [Parameter]
        protected string OverrideDefaultDisabledCssClass { get; set; }

        /// <summary>
        /// Additional css class for the menu's li element. Use this to extend the default css
        /// </summary>
        [Parameter]
        protected string CssClass { get; set; }

        /// <summary>
        /// Additional css class for the menu's li element when disabled. Use this to extend the default css
        /// </summary>
        [Parameter]
        protected string DisabledCssClass { get; set; }

        /// <summary>
        /// The menu item's onclick handler. A <see cref="MenuItemClickEventArgs"/> is passed to the action.
        /// If you want to cancel the click (i.e. stop the menu from closing), then set the "IsCanceled" event args property to "true".
        /// Note: For an async handler use <seealso cref="ClickAsync"/>
        /// </summary>
        [Parameter]
        protected Action<MenuItemClickEventArgs> Click { get; set; }

        /// <summary>
        /// The menu item's onclick async handler. A <see cref="MenuItemClickEventArgs"/> is passed to the action.
        /// If you want to cancel the click (i.e. stop the menu from closing), then set the "IsCanceled" event args property to "true".
        /// Note: For a synchronous handler use <seealso cref="Click"/>
        /// </summary>
        [Parameter]
        protected Func<MenuItemClickEventArgs, Task> ClickAsync { get; set; }

        /// <summary>
        /// Sets the item's enabled state. Default <see cref="true" />
        /// </summary>
        [Parameter]
        protected bool Enabled { get; set; } = true;


        /*=========================== TODO: Find an implementation that works
         
        ///// <summary>
        ///// A handler that can be used to set the item's <see cref="Enabled"/> status dynamically
        ///// Note: For an async handler use <seealso cref="EnabledHandlerAsync"/>
        ///// </summary>
        //[Parameter]
        //protected Func<MenuItemEnabledHandlerArgs, bool> EnabledHandler { get; set; }

        ///// <summary>
        ///// A handler that can be used to set the item's <see cref="Enabled"/> status dynamically
        ///// Note: For a synchronous handler use <seealso cref="EnabledHandler"/>
        ///// </summary>
        //[Parameter]
        //protected Func<MenuItemEnabledHandlerArgs, Task<bool>> EnabledHandlerAsync { get; set; }


        ========================  */

        /// <summary>
        /// The id of the li element. This is optional
        /// </summary>
        [Parameter]
        protected string Id { get; set; }

        /// <summary>
        /// Allows you to override the default x position offset of the submenu (i.e. the distance of the submenu from it's parent menu).
        /// If you want to override this for all menus, then you could use <see cref="BlazorContextMenuDefaults.SubMenuXPositionPixelsOffset"/>
        /// </summary>
        [Parameter]
        protected int SubmenuXOffset { get; set; } = BlazorContextMenuDefaults.SubMenuXPositionPixelsOffset;


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

            BlazorContextMenuHandler.RegisterMenuItem(this);
            base.OnInit();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = -1;
            //var hasEnabledHandler = EnabledHandler != null || EnabledHandlerAsync != null;
            builder.OpenElement(seq++, "li");
            builder.AddAttribute(seq++, "id", Id);
            //if (hasEnabledHandler)
            //{
                //builder.AddAttribute(seq++, "dynamically-enabled", "true");
            //}
            builder.AddAttribute(seq++, "onclick", BindMethods.GetEventHandlerValue<UIMouseEventArgs>((e) => OnClickInternal(e)));
            builder.AddAttribute(seq++, "class", "blazor-context-menu__item " + ClassCalc);
            if (Enabled)
            {
                builder.AddAttribute(seq++, "onmouseover", $"blazorContextMenu.OnMenuItemMouseOver(event, {SubmenuXOffset}, this);");
                builder.AddAttribute(seq++, "onmouseout", "blazorContextMenu.OnMenuItemMouseOut(event);");
            }
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetId()
        {
            return Id;
        }

        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public void CalculateEnabled()
        //{
        //    if (EnabledHandler == null && EnabledHandlerAsync == null) return;
        //    Console.WriteLine($"CalculateEnabled");
            
        //    //TODO: Find a better place for this code (enabled handlers)

        //    var menuId = RegisteredFunction.Invoke<string>("BlazorContextMenu.MenuItem.GetMenuId", MenuItemElement);
        //    var menu = BlazorContextMenuHandler.GetMenu(menuId);
        //    var contextMenuTarget = menu.GetTarget();


        //    if (contextMenuTarget == null) {
        //        Console.WriteLine($"CalculateEnabled: Contextmenu taget is null");
        //        return;
        //    }
        //    //menu is not showing. TODO: find a better way to figure this out 
        //    var args = new MenuItemEnabledHandlerArgs(menuId, contextMenuTarget, MenuItemElement, this);
        //    if (EnabledHandler != null)
        //    {
        //        Enabled = EnabledHandler(args);
        //    }
        //    else if (EnabledHandlerAsync != null)
        //    {
        //        Enabled = EnabledHandlerAsync(args).Result; //todo: change this...
        //    }

        //    StateHasChanged();

        //    //Console.WriteLine($"OldEnabled: {oldEnabledValue}");
        //    //Console.WriteLine($"Enabled: {Enabled}");

        //    //if (oldEnabledValue != Enabled)
        //    //{
        //    //    Console.WriteLine($"Enabled state has changed");
        //    //    StateHasChanged();
        //    //}
        //}
    }
}

