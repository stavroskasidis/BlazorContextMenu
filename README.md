# Blazor Context Menu

[![Build status](https://stavros-kasidis.visualstudio.com/Blazor%20Context%20Menu/_apis/build/status/BlazorContextMenu?branchName=master)](https://stavros-kasidis.visualstudio.com/Blazor%20Context%20Menu/_build/latest?definitionId=12) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Blazor.ContextMenu.svg?logo=nuget)](https://www.nuget.org/packages/Blazor.ContextMenu) [![Nuget](https://img.shields.io/nuget/dt/Blazor.ContextMenu.svg?logo=nuget)](https://www.nuget.org/packages/Blazor.ContextMenu) [![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=7CRGWPYB5AKJQ&currency_code=EUR&source=url)

A context menu component for [Blazor](https://blazor.net)!

![demo-img](ReadmeResources/blazor-context-menu-demo-2.gif)

## Samples / Demo
You can find a live demo [here](https://blazor-context-menu-demo.azurewebsites.net/).

## Installation
**1. Add the nuget package in your Blazor project**
```
> dotnet add package Blazor.ContextMenu

OR

PM> Install-Package Blazor.ContextMenu
```
*Nuget package page can be found [here](https://www.nuget.org/packages/Blazor.ContextMenu).*

**2. Add the following line in your Blazor project's startup class**

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBlazorContextMenu();
    }
}
```
**3. Add the following line in your `_Imports.razor`**
```csharp
@using BlazorContextMenu
```
**4. Reference the static files**

Add the following static file references in your `_Host.cshtml` (server-side blazor) or in your `index.html` (client-side blazor). 
Make sure that there is a call to `app.UseStaticFiles();` in your server project's `Startup.cs`.

```html
<link href="_content/Blazor.ContextMenu/blazorContextMenu.min.css" rel="stylesheet" />
```
```html
<script src="_content/Blazor.ContextMenu/blazorContextMenu.min.js"></script>
```

## Basic usage

```xml

<ContextMenu Id="myMenu">
    <Item OnClick="@OnClick">Item 1</Item>
    <Item OnClick="@OnClick">Item 2</Item>
    <Item OnClick="@OnClick" Enabled="false">Item 3 (disabled)</Item>
    <Seperator />
    <Item>Submenu
        <SubMenu>
            <Item OnClick="@OnClick">Submenu Item 1</Item>
            <Item OnClick="@OnClick">Submenu Item 2</Item>
        </SubMenu>
    </Item>
</ContextMenu>

<ContextMenuTrigger MenuId="myMenu">
    <p>Right-click on me to show the context menu !!</p>
</ContextMenuTrigger>

@code{
    void OnClick(ItemClickEventArgs e)
    {
        Console.WriteLine($"Item Clicked => Menu: {e.ContextMenuId}, MenuTarget: {e.ContextMenuTargetId}, IsCanceled: {e.IsCanceled}, MenuItem: {e.MenuItemElement}, MouseEvent: {e.MouseEvent}");
    }
}

```

## Customization

### Templates

You can create templates in the configuration that you can then apply to context menus. 

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBlazorContextMenu(options =>
        {
            options.ConfigureTemplate("myTemplate", template =>
            {
                template.MenuCssClass = "my-menu";
                template.MenuItemCssClass = "my-menu-item";
                //...
            });
        });
    }
}
```
```xml
<style>
    .my-menu { color: darkblue; }
    
    /* using css specificity to override default background-color */
    .my-menu .my-menu-item { background-color: #ffb3b3;}
    .my-menu .my-menu-item:hover { background-color: #c11515;} 
</style>

<ContextMenu Id="myMenu" Template="myTemplate">
    <Item>Item 1</Item>
    <Item>Item 2</Item>
</ContextMenu>
```

You can also change the default template that will apply to all context menus (unless specified otherwise). 

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBlazorContextMenu(options =>
        {
            //Configures the default template
            options.ConfigureTemplate(defaultTemplate =>
            {
                defaultTemplate.MenuCssClass = "my-default-menu";
                defaultTemplate.MenuItemCssClass = "my-default-menu-item";
                //...
            });

            options.ConfigureTemplate("myTemplate", template =>
            {
                template.MenuCssClass = "my-menu";
                template.MenuItemCssClass = "my-menu-item";
                //...
            });
        });
    }
}
```
### Explicit customization
All components expose `CssClass` parameters that you can use to add css classes. These take precedence over any template configuration.

```xml
<ContextMenu Id="myMenu" CssClass="my-menu">
    <Item CssClass="red-menuitem">Red looking Item</Item>
    <Item>Default looking item</Item>
</ContextMenu>
```

## Overriding default css

You can override the default css classes completely in the following ways (not recommended unless  you want to achieve advanced customization).

### Override default css using templates

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBlazorContextMenu(options =>
        {
            //This will override the default css classes for the default template
            options.ConfigureTemplate(defaultTemplate =>
            {
                defaultTemplate.DefaultCssOverrides.MenuCssClass  = "custom-menu";
                defaultTemplate.DefaultCssOverrides.MenuItemCssClass= "custom-menu-item";
                defaultTemplate.DefaultCssOverrides.MenuItemDisabledCssClass = "custom-menu-item--disabled";
                //...
            });
        });
    }
}
```

### Using the `OverrideDefaultXXX` parameters on components. These take precedence over the template overrides.

```xml
<ContextMenu Id="myMenu" OverrideDefaultCssClass="custom-menu">
    <Item OverrideDefaultCssClass="custom-menu-item" OverrideDefaultDisabledCssClass="custom-menu-item--disabled">Item 1</Item>
    <Item OverrideDefaultCssClass="custom-menu-item" OverrideDefaultDisabledCssClass="custom-menu-item--disabled">Item 2</Item>
</ContextMenu>
```


## ⚠️ Breaking changes ⚠️

<details open="open"><summary>Upgrading from 1.9.0 to 1.10.0</summary>

>- The default auto-hide event is now on "mousedown". If you want the old behaviour, you can use the new `AutoHideEvent` parameter on the `ContextMenu` component to change it back to "mouseup".
</details>

<details><summary>Upgrading from 0.19 to 0.20</summary>

>- Replaced the `ContextMenuTriggerId` in events with the reference to the actual `ContextMenuTrigger`
</details>

<details><summary>Upgrading from 0.16 to 0.17</summary>

>- Removed the deprecated automatic embed of resources in blazor client-side. You must reference the static files as described in the "Installation" section.
>- The static resources path has changed in preview 7 from `_content/blazorcontextmenu/` to `_content/Blazor.ContextMenu/`
</details>

<details><summary>Upgrading from 0.15 to 0.16</summary>

>- Only for Blazor Server-Side projects: You must reference the static files as described in the "Installation" section.
</details>

<details><summary>Upgrading from 0.12 to 0.13</summary>

>- Remove the `@addTagHelper *, BlazorContextMenu` as it is no longer needed.
</details>

<details><summary>Upgrading from 0.11 to 0.12</summary>

>- The following handlers are removed as they are no longer needed: `ClickAsync`, `EnabledHandlerAsync`, `VisibleHandlerAsync`.
>- The `Click` handler has been renamed to `OnClick` to keep consistency with the framework/suggested event names.
>- The `MenuItemClickEventArgs` class has been renamed to the more appropriate `ItemClickEventArgs`.
>- The `EnabledHandler` and `VisibleHandler` parameters have been removed and replaced with the new `OnAppearing` event handler.
>- The `MenuItemEnabledHandlerArgs` and `MenuItemVisibleHandlerArgs` classes have been removed and replaced with the new `ItemAppearingEventArgs`.
</details>

<details><summary>Upgrading from 0.10 to 0.11</summary>
    
>- The `CssOverrides` API is removed and override configuration is moved into templates. The `DefaultCssOverrides` of the `ConfigureTemplate` API must be used.
</details>

<details><summary>Upgrading from 0.5 to 0.6</summary>
    
>- You must add in `Startup.ConfigureServices` of your Blazor client side project the following line `services.AddBlazorContextMenu();`
>- The `BlazorContextMenu.BlazorContextMenuDefaults` API is removed. Use the API provided in the service configuration.
</details>

<details><summary>Upgrading from 0.1 to 0.2</summary>
    
>- Rename "MenuItem" to "Item".
>- Rename "MenuSeperator" to "Seperator".
>- Replace "MenuItemWithSubmenu" with a regular "Item" component.
</details>

## Release Notes
<details open="open"><summary>1.10</summary>

>- Changed default auto hide event to "mousedown". Old behaviour ("mouseup") is available by using the `AutoHideEvent` parameter on the `ContextMenu` component. Contributed by [KristofferStrube](https://github.com/KristofferStrube).
</details>

<details><summary>1.9</summary>

>- Added `ZIndex` support in `ContextMenu` component (default `1000`). Contributed by [grishat](https://github.com/grishat).
>- Added autohide support in `ContextMenu` when window is resizing. Contributed by [grishat](https://github.com/grishat).
</details>

<details><summary>1.8</summary>

>- Added `StopPropagation` parameter on `ContextMenuTrigger` (default `true`).
</details>

<details><summary>1.7</summary>

>- Fix for [#81](https://github.com/stavroskasidis/BlazorContextMenu/issues/81).
</details>


<details><summary>1.6</summary>

>- Added contextual render fragment for `ContextMenu`, exposing a `@context` variable that simplifies advanced scenarios.
</details>


<details><summary>1.5</summary>

>- Fixed a bug when opening multiple menus with different ids.
</details>

<details><summary>1.4</summary>

>- Updated to 3.1 release.
>- Fix for [#72](https://github.com/stavroskasidis/BlazorContextMenu/issues/72).
</details>

<details><summary>1.3</summary>

>- Added menu `OnHiding` event [#68](https://github.com/stavroskasidis/BlazorContextMenu/issues/68).
</details>

<details><summary>1.2</summary>

>- Fix for [#65](https://github.com/stavroskasidis/BlazorContextMenu/issues/65).
</details>

<details><summary>1.1</summary>

>- Added the ability to show/hide a menu from code. (#63)
</details>


<details><summary>1.0</summary>

>- Updated to 3.0 release.
</details>

<details><summary>0.21</summary>

>- Updated to 3.0 preview 9.
</details>

<details><summary>0.20</summary>

>- Added `ContextMenuTrigger` data, that can be accessed from event args.
>- Replaced the `ContextMenuTriggerId` in event args with the reference to the actual `ContextMenuTrigger`
</details>

<details><summary>0.19</summary>

>- Fix for Blazor server-side prerendering: [#53](https://github.com/stavroskasidis/BlazorContextMenu/issues/53).
</details>

<details><summary>0.18</summary>

>- Updated to 3.0 preview 8.
>- Added attribute splatting to components.
</details>

<details><summary>0.17</summary>

>- Updated to 3.0 preview 7.
>- Added double click mouse trigger.
>- Removed the deprecated automatic embed of resources in blazor client-side. You now have to reference the static files just like the server-side blazor projects.
</details>

<details><summary>0.16</summary>
    
>- Updated to 3.0 preview 6.
</details>

<details><summary>0.15</summary>
    
>- Added new `OnAppearing` event to `ContextMenu` conponent, that can be used to prevent the menu from showing.
>- Added the `WrapperTag` parameter to the `ContextMenuTrigger` component, that can be used to change the `ContextMenuTrigger` component's element tag (default: div).
>- Added the `Id` parameter to the `ContextMenuTrigger` component.
</details>

<details><summary>0.14</summary>
    
>- Updated to 3.0 preview 5.
</details>

<details><summary>0.13</summary>
    
>- Updated to 3.0 preview 4.
</details>

<details><summary>0.12</summary>
    
>- Updated to Blazor 0.9.0.
>- Changed event handlers to the new `EventCallback<>`. As a consequence the following handlers are no longer needed and they are removed: `ClickAsync`, `EnabledHandlerAsync`, `VisibleHandlerAsync`.
>- Fixed menu display position when it doesn't fit on screen.
>- The `Click` handler has been renamed to `OnClick` to keep consistency with the framework/suggested event names.
>- The `MenuItemClickEventArgs` class has been renamed to the more appropriate `ItemClickEventArgs`.
>- The `EnabledHandler` and `VisibleHandler` parameters have been removed and replaced with the new `OnAppearing` event handler.
>- The `MenuItemEnabledHandlerArgs` and `MenuItemVisibleHandlerArgs` classes have been removed and replaced with the new `ItemAppearingEventArgs`.
</details>

<details><summary>0.11</summary>

>- Updated to Blazor 0.8.0.
>- Added animations.
>- Default css overrides are now part of the `Templates` API so that you can easily have multiple custom overriden menus.
>- Razor Components are not loading the static files included in the library => [#6349](https://github.com/aspnet/AspNetCore/issues/6349). As a workaround you can download and reference directly the **.css** and **.js** from the `/BlazorContextMenu/content` folder until the issue is resolved.
</details>

<details><summary>0.10</summary>
    
>- Added proper support for Razor Components (aka server-side Blazor).
</details>

<details><summary>0.9</summary>
    
>- Updated to Blazor 0.7.0.
>- Removed some js interop in favor of the new Cascading Values feature.
</details>

<details><summary>0.8</summary>
    
>- Updated to Blazor 0.6.0.
</details>

<details><summary>0.7</summary>

>- Added left-click trigger support.
</details>

<details><summary>0.6</summary>
    
>- Updated to Blazor 0.5.1.
>- Changed configuration setup.
>- Added templates.
</details>

<details><summary>0.5</summary>
    
>- Updated to Blazor 0.5.0.
</details>

<details><summary>0.4</summary>
    
>- Added minification for included css/js.
>- Updated to Blazor 0.4.0.
</details>

<details><summary>0.3</summary>
    
>- Added dynamic EnabledHandlers for menu items.
>- Added Active and dynamic ActiveHandlers for menu items.
</details>

<details><summary>0.2</summary>
    
>- Updated to Blazor 0.3.0.
>- Renamed "MenuItem" to "Item" to avoid conflicts with the html element "menuitem".
>- Renamed "MenuSeperator" to "Seperator" for consistency.
>- Removed "MenuItemWithSubmenu" (just use a regular "Item").
</details>

<details><summary>0.1</summary>
    
>- Initial release.
</details>

## Special Thanks

This project was inspired by https://github.com/fkhadra/react-contexify and https://github.com/vkbansal/react-contextmenu
