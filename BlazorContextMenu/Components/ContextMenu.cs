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
    public class ContextMenu : BlazorComponent
    {
        protected virtual string BaseClass => "blazor-context-menu blazor-context-menu__wrapper";

        public string Id { get; set; }
        public RenderFragment ChildContent { get; set; }
        public string OverrideDefaultCssClass { get; set; }
        public string OverrideDefaultListCssClass { get; set; }
        public string CssClass { get; set; }
        public string ListCssClass { get; set; }
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

        public void Show(string x, string y, string targetId)
        {
            IsShowing = true;
            X = x;
            Y = y;
            TargetId = targetId;
            StateHasChanged();
        }

        public void Hide()
        {
            IsShowing = false;
            StateHasChanged();
        }

        public string GetTarget()
        {
            return TargetId;
        }
    }
}
