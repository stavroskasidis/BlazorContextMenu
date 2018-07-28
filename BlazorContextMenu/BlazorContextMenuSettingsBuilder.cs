using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public class BlazorContextMenuSettingsBuilder
    {
        private readonly BlazorContextMenuSettings _settings;

        public BlazorContextMenuSettingsBuilder(BlazorContextMenuSettings settings)
        {
            _settings = settings;
        }
        
        public BlazorContextMenuSettingsBuilder CssOverrides(Action<BlazorContextMenuDefaultCssSettings> overrides)
        {
            overrides(_settings.DefaultCssSettings); 
            return this;
        }

        public BlazorContextMenuSettingsBuilder CssExtensions(Action<BlazorContextMenuAdditionalCssSettings> additional)
        {
            //var builder = new BlazorContextMenuSettingsAdditionalBuilder(_settings);
            additional(_settings.AdditionalCssSettings);
            return this;
        }

        public BlazorContextMenuSettingsBuilder SubMenuXPositionPixelsOffset(int offset)
        {
            _settings.SubMenuXPositionPixelsOffset = offset;
            return this;
        }
    }

    //public class BlazorContextMenuSettingsAdditionalBuilder
    //{
    //    private readonly BlazorContextMenuSettings _settings;

    //    public BlazorContextMenuSettingsAdditionalBuilder(BlazorContextMenuSettings settings)
    //    {
    //        _settings = settings;
    //    }

    //    public BlazorContextMenuSettingsAdditionalBuilder Menu(string cssClass)
    //    {
    //        _settings.AdditionalMenuCssClass = cssClass;
    //        return this;
    //    }
    //    public BlazorContextMenuSettingsAdditionalBuilder MenuList(string cssClass)
    //    {
    //        _settings.AdditionalMenuListCssClass = cssClass;
    //        return this;
    //    }
    //    public BlazorContextMenuSettingsAdditionalBuilder MenuItem(string cssClass)
    //    {
    //        _settings.AdditionalMenuItemCssClass = cssClass;
    //        return this;
    //    }
    //    public BlazorContextMenuSettingsAdditionalBuilder MenuItemDisabled(string cssClass)
    //    {
    //        _settings.AdditionalMenuItemDisabledCssClass = cssClass;
    //        return this;
    //    }
    //    public BlazorContextMenuSettingsAdditionalBuilder Seperator(string cssClass)
    //    {
    //        _settings.AdditionalSeperatorCssClass = cssClass;
    //        return this;
    //    }
    //    public BlazorContextMenuSettingsAdditionalBuilder SeperatorHr(string cssClass)
    //    {
    //        _settings.AdditionalSeperatorHrCssClass = cssClass;
    //        return this;
    //    }
    //}

    //public class BlazorContextMenuSettingsOverrideBuilder
    //{
    //    private readonly BlazorContextMenuSettings _settings;

    //    public BlazorContextMenuSettingsOverrideBuilder(BlazorContextMenuSettings settings)
    //    {
    //        _settings = settings;
    //    }

    //    public BlazorContextMenuSettingsOverrideBuilder Menu(string cssClass)
    //    {
    //        _settings.DefaultMenuCssClass = cssClass;
    //        return this;
    //    }
    //    public BlazorContextMenuSettingsOverrideBuilder MenuList(string cssClass)
    //    {
    //        _settings.DefaultMenuListCssClass = cssClass;
    //        return this;
    //    }
    //    public BlazorContextMenuSettingsOverrideBuilder MenuItem(string cssClass)
    //    {
    //        _settings.DefaultMenuItemCssClass = cssClass;
    //        return this;
    //    }
    //    public BlazorContextMenuSettingsOverrideBuilder MenuItemDisabled(string cssClass)
    //    {
    //        _settings.DefaultMenuItemDisabledCssClass = cssClass;
    //        return this;
    //    }
    //    public BlazorContextMenuSettingsOverrideBuilder Seperator(string cssClass)
    //    {
    //        _settings.DefaultSeperatorCssClass = cssClass;
    //        return this;
    //    }
    //    public BlazorContextMenuSettingsOverrideBuilder SeperatorHr(string cssClass)
    //    {
    //        _settings.DefaultSeperatorHrCssClass = cssClass;
    //        return this;
    //    }
    //}
}
