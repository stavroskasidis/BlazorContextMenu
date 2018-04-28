//// This file is to show how a library package may provide JavaScript interop features
//// wrapped in a .NET API

//Blazor.registerFunction('BlazorContextMenu.ExampleJsInterop.Prompt', function (message) {
//    return prompt(message, 'Type anything here');
//});

//closest polyfill
if (window.Element && !Element.prototype.closest) {
    Element.prototype.closest =
        function (s) {
            var matches = (this.document || this.ownerDocument).querySelectorAll(s),
                i,
                el = this;
            do {
                i = matches.length;
                while (--i >= 0 && matches.item(i) !== el) { };
            } while ((i < 0) && (el = el.parentElement));
            return el;
        };
}


var blazorContextMenu = function (blazorContextMenu) {

    var openMenuId = null;

    blazorContextMenu.OnContextMenu = function (e, menuId) {
        blazorContextMenu.Show(menuId, e.x, e.y, e.target);
        openMenuId = menuId;

        e.preventDefault();
        return false;
    };

    blazorContextMenu.Init = function () {
        document.addEventListener("mouseup", function (e) {
            if (openMenuId) {
                var menuElement = document.getElementById(openMenuId);
                var clickedInsideMenu = menuElement.contains(e.target);
                if (!clickedInsideMenu) {
                    blazorContextMenu.Hide(openMenuId);
                    openMenuId = null;
                }
            }
        });
    };

    blazorContextMenu.Show = function (menuId, x, y, target) {
        var showMenuMethod = Blazor.platform.findMethod("BlazorContextMenu", "BlazorContextMenu", "BlazorContextMenuHandler", "ShowMenu");
        Blazor.platform.callMethod(showMenuMethod, null, [Blazor.platform.toDotNetString(menuId), Blazor.platform.toDotNetString(x.toString()), Blazor.platform.toDotNetString(y.toString()), target]);
    }

    blazorContextMenu.Hide = function(menuId) {
        var hideMenuMethod = Blazor.platform.findMethod("BlazorContextMenu", "BlazorContextMenu", "BlazorContextMenuHandler", "HideMenu");
        Blazor.platform.callMethod(hideMenuMethod, null, [Blazor.platform.toDotNetString(menuId)]);
    }

    return blazorContextMenu;
}({});

blazorContextMenu.Init();

Blazor.registerFunction('BlazorContextMenu.MenuItem.GetMenuId', function (menuItem) {
    var menu = menuItem.closest(".blazor-context-menu");
    return menu.id;
});

//Blazor.registerFunction('BlazorContextMenu.MenuItem.HideMenu', function (menuItem) {
//    var menu = menuItem.closest(".blazor-context-menu");
//    blazorContextMenu.Hide(menu.id);
//});