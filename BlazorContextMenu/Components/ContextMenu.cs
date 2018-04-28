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
        protected const string DefaultCssClass = "blazor-context-menu--default";
        protected const string DefaultListCssClass = "blazor-context-menu__list";

        public string Id { get; set; }
        public RenderFragment ChildContent { get; set; }
        public string CssClass { get; set; }
        public string ListCssClass { get; set; }
        protected bool IsShowing;
        protected string X { get; set; }
        protected string Y { get; set; }
        protected ElementRef Target { get; set; }
        protected string ClassCalc => CssClass == null ? DefaultCssClass : CssClass;
        protected string ShowingClassCalc => !IsShowing ? "blazor-context-menu--hidden" : "";
        protected string ListClassCalc => ListCssClass == null ? DefaultListCssClass : ListCssClass;

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
            int seq = -1;
            builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", $"blazor-context-menu {ClassCalc} {ShowingClassCalc}");
                builder.AddAttribute(seq++, "id", Id ?? "");
                builder.AddAttribute(seq++, "style", $"left:{X}px;top:{Y}px");

                builder.OpenElement(seq++, "ul");
                    builder.AddAttribute(seq++,"class", ListClassCalc);
                    builder.AddContent(seq++, ChildContent);
                builder.CloseElement();
            builder.CloseElement();

            base.BuildRenderTree(builder);
        }

        public void Show(string x, string y, ElementRef target)
        {
            IsShowing = true;
            X = x;
            Y = y;
            Target = target;
            StateHasChanged();
        }

        public void Hide()
        {
            IsShowing = false;
            StateHasChanged();
        }

        public ElementRef GetTarget()
        {
            return Target;
        }
    }
}
