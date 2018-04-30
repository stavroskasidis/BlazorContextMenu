# Blazor Context Menu ![Build Status](https://stavros-kasidis.visualstudio.com/_apis/public/build/definitions/9942c317-bff6-4b9f-9c78-0e97ce00de51/12/badge) ![NuGet Badge](https://buildstats.info/nuget/Blazor.ContextMenu?includePreReleases=true)

A context menu component for [Blazor](https://github.com/aspnet/Blazor) !

![demo-img](ReadmeResources/blazor-context-menu-demo-1.gif)

> ⚠️ Warning: This project is build on top of an experimental framework. There is a high propability that there will be breaking changes from version to version.

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
    <MenuItem Click="@OnClick">Item 1</MenuItem>
    <MenuItem Click="@OnClick">Item 2</MenuItem>
    <MenuItem Click="@OnClick" IsEnabled="false">Item 3 (disabled)</MenuItem>
    <MenuSeperator />
    <MenuItemWithSubMenu>Submenu
        <SubMenu>
            <MenuItem Click="@OnClick">Submenu Item 1</MenuItem>
            <MenuItem Click="@OnClick">Submenu Item 2</MenuItem>
        </SubMenu>
    </MenuItemWithSubMenu>
</ContextMenu>

<ContextMenuTrigger MenuId="myMenu">
    <p>Right-click on me to show the context menu !!</p>
</ContextMenuTrigger>

@functions{
    void OnClick(MenuItemEventArgs e)
    {
        Console.WriteLine($"Item Clicked => Menu: {e.ContextMenuId}, MenuTarget: {e.ContextMenuTargetId}, IsCanceled: {e.IsCanceled}, MenuItem: {e.MenuItemElement}, MouseEvent: {e.MouseEvent}");
    }
}

```

## Special Thanks

This project is inspired by https://github.com/fkhadra/react-contexify and https://github.com/vkbansal/react-contextmenu
