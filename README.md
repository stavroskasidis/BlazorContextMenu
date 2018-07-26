# Blazor Context Menu ![Build Status](https://stavros-kasidis.visualstudio.com/_apis/public/build/definitions/9942c317-bff6-4b9f-9c78-0e97ce00de51/12/badge) ![NuGet Badge](https://buildstats.info/nuget/Blazor.ContextMenu?includePreReleases=true)

A context menu component for [Blazor](https://github.com/aspnet/Blazor) !

![demo-img](ReadmeResources/blazor-context-menu-demo-1.gif)

> ⚠️ Warning: This project is build on top of an experimental framework. There are many limitations and there is a high propability that there will be breaking changes from version to version.

## Demo
You can find a live demo [here](https://blazor-context-menu-demo.azurewebsites.net/).

## Installation

```
> dotnet add package Blazor.ContextMenu

OR

PM> Install-Package Blazor.ContextMenu
```
Nuget package page can be found [here](https://www.nuget.org/packages/Blazor.ContextMenu).

## Usage

### Basic usage

```xml
@addTagHelper *, BlazorContextMenu

<ContextMenu Id="myMenu">
    <Item Click="@OnClick">Item 1</Item>
    <Item Click="@OnClick">Item 2</Item>
    <Item Click="@OnClick" Enabled="false">Item 3 (disabled)</Item>
    <Seperator />
    <Item>Submenu
        <SubMenu>
            <Item Click="@OnClick">Submenu Item 1</Item>
            <Item Click="@OnClick">Submenu Item 2</Item>
        </SubMenu>
    </Item>
</ContextMenu>

<ContextMenuTrigger MenuId="myMenu">
    <p>Right-click on me to show the context menu !!</p>
</ContextMenuTrigger>

@functions{
    void OnClick(MenuItemClickEventArgs e)
    {
        Console.WriteLine($"Item Clicked => Menu: {e.ContextMenuId}, MenuTarget: {e.ContextMenuTargetId}, IsCanceled: {e.IsCanceled}, Item: {e.ItemElement}, MouseEvent: {e.MouseEvent}");
    }
}

```

### Customization

#### Adding css

All components expose a `CssClass` parameter that you can use to add css on top of the default classes.

```html
<style>
    .my-menu { color: darkblue; }
    
    /* using css specificity to override default background-color */
    .my-menu .red-menuitem { background-color: #ffb3b3;}
    .my-menu .red-menuitem:hover { background-color: #c11515;} 
</style>

<ContextMenu Id="myMenu" CssClass="my-menu">
    <Item CssClass="red-menuitem">Red looking Item</Item>
    <Item>Default looking item</Item>
</ContextMenu>
```

#### Overriding default css

You can override the default css classes completely in the following ways (Recommended only if you want to achieve advanced customization).

##### 1. Globally for all ContextMenus using the `BlazorContextMenu.BlazorContextMenuDefaults` API.

```csharp
    using BlazorContextMenu;

    public class Program
    {
        static void Main(string[] args)
        {
            // Code ommited ...

            BlazorContextMenuDefaults.DefaultMenuCssClass = "my-menu";
            BlazorContextMenuDefaults.DefaultMenuItemCssClass = "my-menu-item";
            BlazorContextMenuDefaults.DefaultMenuItemDisabledCssClass = "my-menu-item--disabled";
            
            // Code ommited ...
            
            new BrowserRenderer(serviceProvider).AddComponent<App>("app");
        }
    }
```

##### 2. Using the `OverrideDefaultXXX` parameters on components. This will override the defaults from the `BlazorContextMenu.BlazorContextMenuDefaults` API.

```xml
<ContextMenu Id="myMenu" OverrideDefaultCssClass="my-menu">
    <Item OverrideDefaultCssClass="my-menu-item" OverrideDefaultDisabledCssClass="my-menu-item--disabled">Item 1</Item>
    <Item OverrideDefaultCssClass="my-menu-item" OverrideDefaultDisabledCssClass="my-menu-item--disabled">Item 2</Item>
</ContextMenu>
```

## ⚠️ Breaking changes ⚠️
Upgrating from 0.1 to 0.2
>- Rename "MenuItem" to "Item"
>- Rename "MenuSeperator" to "Seperator"
>- Replace "MenuItemWithSubmenu" with a regular "Item" component


## Release Notes

### 0.5
>- Updated to Blazor 0.5.0

### 0.4
>- Added minification for included css/js
>- Updated to Blazor 0.4.0

### 0.3
>- Added dynamic EnabledHandlers for menu items
>- Added Active and dynamic ActiveHandlers for menu items

### 0.2
>- Updated to Blazor 0.3.0
>- Renamed "MenuItem" to "Item" to avoid conflicts with the html element "menuitem"
>- Renamed "MenuSeperator" to "Seperator" for consistency
>- Removed "MenuItemWithSubmenu" (just use a regular "Item")

### 0.1
>- Initial release

## Special Thanks

This project is inspired by https://github.com/fkhadra/react-contexify and https://github.com/vkbansal/react-contextmenu
