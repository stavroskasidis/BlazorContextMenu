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
    var openMenuTarget = null;
    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    function findFirstChildByClass(element, className) {
        var foundElement = null, found;
        function recurse(element, className, found) {
            for (var i = 0; i < element.childNodes.length && !found; i++) {
                var el = element.childNodes[i];
                var classes = el.className != undefined ? el.className.split(" ") : [];
                for (var j = 0, jl = classes.length; j < jl; j++) {
                    if (classes[j] == className) {
                        found = true;
                        foundElement = element.childNodes[i];
                        break;
                    }
                }
                if (found)
                    break;
                recurse(element.childNodes[i], className, found);
            }
        }
        recurse(element, className, false);
        return foundElement;
    }

    function findAllChildsByClass(element, className) {
        var foundElements = new Array();
        function recurse(element, className) {
            for (var i = 0; i < element.childNodes.length; i++) {
                var el = element.childNodes[i];
                var classes = el.className != undefined ? el.className.split(" ") : [];
                for (var j = 0, jl = classes.length; j < jl; j++) {
                    if (classes[j] == className) {
                        foundElements.push(element.childNodes[i]);
                    }
                }
                recurse(element.childNodes[i], className);
            }
        }
        recurse(element, className);
        return foundElements;
    }

    blazorContextMenu.OnContextMenu = function (e, menuId) {
        blazorContextMenu.Show(menuId, e.x, e.y, e.target);
        openMenuId = menuId;
        openMenuTarget = e.target;
        var currentMenu = document.getElementById(menuId);

        var childSubMenus = findAllChildsByClass(currentMenu, "blazor-context-submenu");
        var i = childSubMenus.length;
        while (i--) {
            var subMenu = childSubMenus[i];
            blazorContextMenu.Hide(subMenu.id);
        }

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
                    openMenuTarget = null;
                }
            }
        });
    };

    blazorContextMenu.Show = function (menuId, x, y, target) {
        if (!target.id) {
            //add an id to the target dynamically so that it can be referenced later 
            //TODO: Rewrite once this Blazor limitation is lifted
            target.id = guid();
        }
        var showMenuMethod = Blazor.platform.findMethod("BlazorContextMenu", "BlazorContextMenu", "BlazorContextMenuHandler", "ShowMenu");
        Blazor.platform.callMethod(showMenuMethod, null, [Blazor.platform.toDotNetString(menuId), Blazor.platform.toDotNetString(x.toString()), Blazor.platform.toDotNetString(y.toString()), Blazor.platform.toDotNetString(target.id)]);
    }

    blazorContextMenu.Hide = function (menuId) {
        var hideMenuMethod = Blazor.platform.findMethod("BlazorContextMenu", "BlazorContextMenu", "BlazorContextMenuHandler", "HideMenu");
        Blazor.platform.callMethod(hideMenuMethod, null, [Blazor.platform.toDotNetString(menuId)]);
    }

    var subMenuTimeout = null;
    blazorContextMenu.OnSubMenuItemMouseOver = function (e, xOffset, boundItem) {
        if (e.target != boundItem) {
            //skip child mouse overs
            return;
        }

        subMenuTimeout = setTimeout(function () {
            subMenuTimeout = null;
            var currentItem = e.target;
            var subMenu = findFirstChildByClass(currentItem, "blazor-context-submenu");
            var currentMenu = currentItem.closest(".blazor-context-menu__wrapper");
            var currentMenuList = currentMenu.childNodes[0];
            var targetRect = currentItem.getBoundingClientRect();
            blazorContextMenu.Show(subMenu.id, targetRect.left + currentMenu.clientWidth - xOffset, targetRect.top, openMenuTarget);

            var closeSubMenus = function () {
                var childSubMenus = findAllChildsByClass(currentItem, "blazor-context-submenu");
                var i = childSubMenus.length;
                while (i--) {
                    var subMenu = childSubMenus[i];
                    blazorContextMenu.Hide(subMenu.id);
                }

                i = currentMenuList.childNodes.length;
                while (i--) {
                    var childNode = currentMenuList.childNodes[i];
                    if (childNode == currentItem) continue;
                    childNode.removeEventListener("mouseover", closeSubMenus);
                }
            };

            var i = currentMenuList.childNodes.length;
            while (i--) {
                var childNode = currentMenuList.childNodes[i];
                if (childNode == currentItem) continue;

                childNode.addEventListener("mouseover", closeSubMenus);
            }
        }, 200);
    }
    blazorContextMenu.OnSubMenuItemMouseOut = function (e) {
        if (subMenuTimeout) {
            clearTimeout(subMenuTimeout);
        }
    }


    return blazorContextMenu;
}({});

blazorContextMenu.Init();

Blazor.registerFunction('BlazorContextMenu.MenuItem.GetMenuId', function (menuItem) {
    var menu = menuItem.closest(".blazor-context-menu");
    return menu.id;
});