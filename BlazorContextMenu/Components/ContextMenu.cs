using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{

#pragma warning disable BL9993 // Component parameter is marked public

    public class ContextMenu : BlazorComponent, IDisposable
    {
        protected virtual string BaseClass => "blazor-context-menu blazor-context-menu__wrapper";

        /// <summary>
        /// The id that the <see cref="Components.ContextMenuTrigger"/> will use to bind to. This parameter is required
        /// </summary>
        [Parameter]
        public string Id { get; protected set; }

        /// <summary>
        /// Allows you to override the default css class of the menu's div element for full customization. 
        /// If you want to override this for all menus, then you could use <see cref="BlazorContextMenuDefaults.DefaultMenuCssClass"/>
        /// </summary>
        [Parameter]
        public string OverrideDefaultCssClass { get; protected set; }

        /// <summary>
        /// Allows you to override the default css class of the menu's ul element for full customization.
        /// If you want to override this for all menus, then you could use <see cref="BlazorContextMenuDefaults.DefaultMenuListCssClass"/>
        /// </summary>
        [Parameter]
        public string OverrideDefaultListCssClass { get; protected set; }

        /// <summary>
        /// Additional css class for the menu's div element. Use this to extend the default css
        /// </summary>
        [Parameter]
        public string CssClass { get; protected set; }

        /// <summary>
        /// Additional css class for the menu's ul element. Use this to extend the default css
        /// </summary>
        [Parameter]
        public string ListCssClass { get; protected set; }

        [Parameter]
        protected RenderFragment ChildContent { get; set; }


        protected bool IsShowing;
        protected string X { get; set; }
        protected string Y { get; set; }
        protected string TargetId { get; set; }
        protected string ClassCalc => (OverrideDefaultCssClass == null ? BlazorContextMenuDefaults.DefaultMenuCssClass : OverrideDefaultCssClass) + (CssClass == null ? "" : $" {CssClass}");
        protected string StyleDisplayCalc => !IsShowing ? "display: none;" : "";
        protected string ListClassCalc => (OverrideDefaultListCssClass == null ? BlazorContextMenuDefaults.DefaultMenuListCssClass : OverrideDefaultListCssClass) + (ListCssClass == null ? "" : $" {ListCssClass}");

        protected override void OnInit()
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new ArgumentNullException(nameof(Id));
            }
            BlazorContextMenuHandler.Register(this);
        }

        public void Dispose()
        {
            BlazorContextMenuHandler.Unregister(this);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            int seq = -1;
            builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", $"{BaseClass} {ClassCalc}");
                builder.AddAttribute(seq++, "id", Id);
                builder.AddAttribute(seq++, "style", $"left:{X}px;top:{Y}px;{StyleDisplayCalc}");

                builder.OpenElement(seq++, "ul");
                    builder.AddAttribute(seq++,"class", ListClassCalc);
                    builder.AddContent(seq++, ChildContent);
                builder.CloseElement();
            builder.CloseElement();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Show(string x, string y, string targetId)
        {
            IsShowing = true;
            X = x;
            Y = y;
            TargetId = targetId;
            StateHasChanged();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Hide()
        {
            IsShowing = false;
            StateHasChanged();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string GetTarget()
        {
            return TargetId;
        }
    }
}


#pragma warning restore BL9993 // Component parameter is marked public