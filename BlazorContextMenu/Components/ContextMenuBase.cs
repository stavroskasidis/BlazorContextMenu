using BlazorContextMenu.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public abstract class ContextMenuBase : MenuTreeComponent
    {
        [Inject] private BlazorContextMenuSettings settings { get; set; }
        [Inject] private IContextMenuStorage contextMenuStorage { get; set; }
        [Inject] private IInternalContextMenuHandler contextMenuHandler { get; set; }
        [Inject] private IJSRuntime jsRuntime { get; set; }
        [Inject] private IMenuTreeTraverser menuTreeTraverser { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        protected virtual string BaseClass => "blazor-context-menu blazor-context-menu__wrapper";

        /// <summary>
        /// The id that the <see cref="ContextMenuTrigger" /> will use to bind to. This parameter is required
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// The name of the template to use for this <see cref="ContextMenu" /> and all its <see cref="SubMenu" />.
        /// </summary>
        [Parameter]
        public string Template { get; set; }

        [CascadingParameter(Name = "CascadingTemplate")]
        protected string CascadingTemplate { get; set; }

        /// <summary>
        /// Allows you to override the default css class of the <see cref="ContextMenu"/>'s div element, for full customization.
        /// </summary>
        [Parameter]
        public string OverrideDefaultCssClass { get; set; }

        /// <summary>
        /// Allows you to override the default css class of the <see cref="ContextMenu"/>'s div element while it's shown, for full customization.
        /// </summary>
        [Parameter]
        public string OverrideDefaultShownCssClass { get; set; }

        /// <summary>
        /// Allows you to override the default css class of the <see cref="ContextMenu"/>'s div element while it's hidden, for full customization.
        /// </summary>
        [Parameter]
        public string OverrideDefaultHiddenCssClass { get; set; }

        /// <summary>
        /// Allows you to override the default css class of the <see cref="ContextMenu"/>'s ul element, for full customization.
        /// </summary>
        [Parameter]
        public string OverrideDefaultListCssClass { get; set; }

        /// <summary>
        /// Additional css class that is applied to the <see cref="ContextMenu"/>'s div element. Use this to extend the default css.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Additional css class that is applied to the <see cref="ContextMenu"/>'s div element while is shown. Use this to extend the default css.
        /// </summary>
        [Parameter]
        public string ShownCssClass { get; set; }

        /// <summary>
        /// Additional css class that is applied to the <see cref="ContextMenu"/>'s div element while is hidden. Use this to extend the default css.
        /// </summary>
        [Parameter]
        public string HiddenCssClass { get; set; }

        /// <summary>
        /// Additional css class that is applied to the <see cref="ContextMenu"/>'s ul element. Use this to extend the default css.
        /// </summary>
        [Parameter]
        public string ListCssClass { get; set; }

        /// <summary>
        /// Allows you to set the <see cref="BlazorContextMenu.Animation" /> used by this <see cref="ContextMenu" /> and all its <see cref="SubMenu" />
        /// </summary>
        [Parameter]
        public Animation? Animation { get; set; }

        /// <summary>
        /// A handler that is triggered before the menu appears. Can be used to prevent the menu from showing.
        /// </summary>
        [Parameter]
        public EventCallback<MenuAppearingEventArgs> OnAppearing { get; set; }

        /// <summary>
        /// A handler that is triggered before the menu is hidden. Can be used to prevent the menu from hiding.
        /// </summary>
        [Parameter]
        public EventCallback<MenuHidingEventArgs> OnHiding { get; set; }

        /// <summary>
        /// Set to false if you want to close the menu programmatically. Default: true
        /// </summary>
        [Parameter]
        public bool AutoHide { get; set; } = true;

        /// <summary>
        /// Set to AutoHideEvent.MouseUp if you want it to close the menu on the MouseUp event. Default: AutoHideEvent.MouseDown
        /// </summary>
        [Parameter]
        public AutoHideEvent AutoHideEvent { get; set; } = AutoHideEvent.MouseDown;

        /// <summary>
        /// Set CSS z-index for overlapping other html elements. Default: 1000
        /// </summary>
        [Parameter]
        public int ZIndex { get; set; } = 1000;

        [CascadingParameter(Name = "CascadingAnimation")]
        protected Animation? CascadingAnimation { get; set; }

        protected bool IsShowing;
        protected string X { get; set; }
        protected string Y { get; set; }
        protected string TargetId { get; set; }
        protected ContextMenuTrigger Trigger { get; set; }
        internal object Data { get; set; }

        protected string ClassCalc
        {
            get
            {
                var template = settings.GetTemplate(GetActiveTemplate());
                return Helpers.AppendCssClasses((OverrideDefaultCssClass ?? template.DefaultCssOverrides.MenuCssClass),
                                                (CssClass ?? template.MenuCssClass));
            }
        }

        protected Animation GetActiveAnimation()
        {
            var animation = CascadingAnimation;
            if (this.Animation != null)
            {
                animation = this.Animation;
            }
            if (animation == null)
            {
                animation = settings.GetTemplate(GetActiveTemplate()).Animation;
            }

            return animation.Value;
        }

        internal string GetActiveTemplate()
        {
            var templateName = CascadingTemplate;
            if (Template != null)
            {
                templateName = Template;
            }
            if (templateName == null)
            {
                templateName = BlazorContextMenuSettings.DefaultTemplateName;
            }

            return templateName;
        }

        protected string DisplayClassCalc
        {
            get
            {
                var template = settings.GetTemplate(GetActiveTemplate());
                var (showingAnimationClass, hiddenAnimationClass) = GetAnimationClasses(GetActiveAnimation());
                return IsShowing ?
                    Helpers.AppendCssClasses(OverrideDefaultShownCssClass ?? template.DefaultCssOverrides.MenuShownCssClass,
                                             showingAnimationClass,
                                             ShownCssClass ?? settings.GetTemplate(GetActiveTemplate()).MenuShownCssClass) :
                    Helpers.AppendCssClasses(OverrideDefaultHiddenCssClass ?? template.DefaultCssOverrides.MenuHiddenCssClass,
                                             hiddenAnimationClass,
                                             HiddenCssClass ?? settings.GetTemplate(GetActiveTemplate()).MenuHiddenCssClass);
            }
        }
        protected string ListClassCalc
        {
            get
            {
                var template = settings.GetTemplate(GetActiveTemplate());
                return Helpers.AppendCssClasses((OverrideDefaultListCssClass ?? template.DefaultCssOverrides.MenuListCssClass),
                                                (ListCssClass ?? settings.GetTemplate(GetActiveTemplate()).MenuListCssClass));
            }
        }

        protected (string showingClass, string hiddenClass) GetAnimationClasses(Animation animation)
        {
            switch (animation)
            {
                case BlazorContextMenu.Animation.None:
                    return ("", "");
                case BlazorContextMenu.Animation.FadeIn:
                    return ("blazor-context-menu__animations--fadeIn-shown", "blazor-context-menu__animations--fadeIn");
                case BlazorContextMenu.Animation.Grow:
                    return ("blazor-context-menu__animations--grow-shown", "blazor-context-menu__animations--grow");
                case BlazorContextMenu.Animation.Slide:
                    return ("blazor-context-menu__animations--slide-shown", "blazor-context-menu__animations--slide");
                case BlazorContextMenu.Animation.Zoom:
                    return ("blazor-context-menu__animations--zoom-shown", "blazor-context-menu__animations--zoom");
                default:
                    throw new Exception("Animation not supported");
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (string.IsNullOrEmpty(Id))
            {
                throw new ArgumentNullException(nameof(Id));
            }
            contextMenuStorage.Register(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!contextMenuHandler.ReferencePassedToJs)
            {
                await jsRuntime.InvokeAsync<object>("blazorContextMenu.SetMenuHandlerReference", DotNetObjectReference.Create(contextMenuHandler));
                contextMenuHandler.ReferencePassedToJs = true;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            contextMenuStorage.Unregister(this);
        }

        internal async Task Show(string x, string y, string targetId = null, ContextMenuTrigger trigger = null)
        {

            if (trigger is null)
            {
                var rootMenu = menuTreeTraverser.GetRootContextMenu(this);
                trigger = rootMenu?.GetTrigger();
            }

            if (trigger != null)
            {
                Data = trigger.Data;
            }

            if (OnAppearing.HasDelegate)
            {
                var eventArgs = new MenuAppearingEventArgs(Id, targetId, x, y, trigger, Data);
                await OnAppearing.InvokeAsync(eventArgs);
                x = eventArgs.X;
                y = eventArgs.Y;
                if (eventArgs.PreventShow)
                {
                    return;
                }
            }

            IsShowing = true;
            X = x;
            Y = y;
            TargetId = targetId;
            Trigger = trigger;
            await InvokeAsync(() => StateHasChanged());
        }

        internal async Task<bool> Hide()
        {
            if (OnHiding.HasDelegate)
            {
                var eventArgs = new MenuHidingEventArgs(Id, TargetId, X, Y, Trigger, Data);
                await OnHiding.InvokeAsync(eventArgs);
                if (eventArgs.PreventHide)
                {
                    return false;
                }
            }

            IsShowing = false;
            await InvokeAsync(() => StateHasChanged());
            return true;
        }

        internal string GetTarget()
        {
            return TargetId;
        }

        internal ContextMenuTrigger GetTrigger()
        {
            return Trigger;
        }
    }
}
