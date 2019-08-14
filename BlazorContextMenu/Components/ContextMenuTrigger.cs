using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using BlazorContextMenu.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Runtime.CompilerServices;

namespace BlazorContextMenu
{
    public class ContextMenuTrigger : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            /*
                <div @attributes="Attributes"
                     onclick="@(MouseButtonTrigger == MouseButtonTrigger.Left || MouseButtonTrigger == MouseButtonTrigger.Both ? $"blazorContextMenu.OnContextMenu(event, '{MenuId.Replace("'","\\'")}'); " : "")"
                     ondblclick="@(MouseButtonTrigger == MouseButtonTrigger.DoubleClick ? $"blazorContextMenu.OnContextMenu(event, '{MenuId.Replace("'","\\'")}'); " : "")"
                     oncontextmenu="@(MouseButtonTrigger == MouseButtonTrigger.Right || MouseButtonTrigger == MouseButtonTrigger.Both ? $"blazorContextMenu.OnContextMenu(event, '{MenuId.Replace("'","\\'")}');": "")"
                     class="@CssClass">
                    @ChildContent
                </div>
             */

            builder.OpenElement(0, WrapperTag);

            builder.AddMultipleAttributes(1, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<string, object>>>(Attributes));

            if (MouseButtonTrigger == MouseButtonTrigger.Left || MouseButtonTrigger == MouseButtonTrigger.Both)
            {
                builder.AddAttribute(2, "onclick", $"blazorContextMenu.OnContextMenu(event, '{MenuId.Replace("'", "\\'")}');");
            }

            if (MouseButtonTrigger == MouseButtonTrigger.Right || MouseButtonTrigger == MouseButtonTrigger.Both)
            {
                builder.AddAttribute(3, "oncontextmenu", $"blazorContextMenu.OnContextMenu(event, '{MenuId.Replace("'", "\\'")}');");
            }

            if(MouseButtonTrigger == MouseButtonTrigger.DoubleClick)
            {
                builder.AddAttribute(4, "ondblclick",$"blazorContextMenu.OnContextMenu(event, '{MenuId.Replace("'", "\\'")}');");
            }

            if (!string.IsNullOrWhiteSpace(CssClass))
            {
                builder.AddAttribute(5, "class", CssClass);
            }
            builder.AddAttribute(6, "id", Id);
            builder.AddContent(7, ChildContent);
            builder.CloseElement();

        }

        [Inject] private IJSRuntime jsRuntime { get; set; }
        [Inject] private BlazorContextMenuHandler blazorContextMenuHandler { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// The id of the <see cref="ContextMenuTrigger" /> wrapper element.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// The Id of the <see cref="ContextMenu" /> to open. This parameter is required.
        /// </summary>
        [Parameter]
        public string MenuId { get; set; }

        /// <summary>
        /// Additional css class for the trigger's wrapper element.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// The mouse button that triggers the menu.
        ///
        /// </summary>
        [Parameter]
        public MouseButtonTrigger MouseButtonTrigger { get; set; }

        /// <summary>
        /// The trigger's wrapper element tag (default: "div").
        /// </summary>
        [Parameter]
        public string WrapperTag { get; set; } = "div";

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(MenuId))
            {
                throw new ArgumentNullException(nameof(MenuId));
            }
        }

        #region Hack to fix https://github.com/aspnet/AspNetCore/issues/11159

        public static object CreateDotNetObjectRefSyncObj = new object();

        protected DotNetObjectRef<T> CreateDotNetObjectRef<T>(T value) where T : class
        {
            lock (CreateDotNetObjectRefSyncObj)
            {
                JSRuntime.SetCurrentJSRuntime(jsRuntime);
                return DotNetObjectRef.Create(value);
            }
        }
        #endregion

        protected override async Task OnAfterRenderAsync()
        {
            if (!blazorContextMenuHandler.ReferencePassedToJs)
            {
                await jsRuntime.InvokeAsync<object>("blazorContextMenu.SetMenuHandlerReference", CreateDotNetObjectRef(blazorContextMenuHandler));
                blazorContextMenuHandler.ReferencePassedToJs = true;
            }
        }

    }
}
