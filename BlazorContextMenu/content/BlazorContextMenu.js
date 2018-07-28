//"closest" polyfill
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
    //Helper functions
    //========================================
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

    //===========================================

    blazorContextMenu.OnContextMenu = function (e, menuId) {
        var menu = document.getElementById(menuId);
        openMenuId = menuId;
        openMenuTarget = e.target;

        //show context menu
        var originalDisplay = menu.style.display;
        menu.style.display = ""; //this is required to get the menu's width
        var x = e.x;
        var y = e.y;
        if (x + menu.offsetWidth > window.innerWidth) {
            x -= x + menu.offsetWidth - window.innerWidth;
        }

        if (y + menu.offsetHeight > window.innerHeight) {
            y -= y + menu.offsetHeight - window.innerHeight;
        }
        menu.style.display = originalDisplay;

        blazorContextMenu.Show(menuId, x, y, e.target);


        //Hide all other open submenus
        var childSubMenus = findAllChildsByClass(menu, "blazor-context-submenu");
        var i = childSubMenus.length;
        while (i--) {
            var subMenu = childSubMenus[i];
            blazorContextMenu.Hide(subMenu.id);
        }

        var menuItems = findAllChildsByClass(menu, "blazor-context-menu__item");

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
            //TODO: Rewrite this once this Blazor limitation is lifted
            target.id = guid();
        }
        DotNet.invokeMethodAsync('BlazorContextMenu', 'ShowMenu', menuId, x.toString(), y.toString(), target.id);
    }

    blazorContextMenu.Hide = function (menuId) {
        DotNet.invokeMethodAsync('BlazorContextMenu', 'HideMenu', menuId);
    }

    var subMenuTimeout = null;
    blazorContextMenu.OnMenuItemMouseOver = function (e, xOffset, boundItem) {
        if (e.target != boundItem) {
            //skip child mouseovers
            return;
        }
        var currentItem = e.target;
        if (currentItem.getAttribute("itemEnabled") != "true") return;

        var subMenu = findFirstChildByClass(currentItem, "blazor-context-submenu");
        if (!subMenu) return; //item does not contain a submenu

        subMenuTimeout = setTimeout(function () {
            subMenuTimeout = null;
            var originalDisplay = subMenu.style.display;
            subMenu.style.display = ""; //this is required to get the menu's width

            var currentMenu = currentItem.closest(".blazor-context-menu__wrapper");
            var currentMenuList = currentMenu.children[0];
            var targetRect = currentItem.getBoundingClientRect();
            var x = targetRect.left + currentMenu.clientWidth - xOffset;
            var y = targetRect.top;
            if (x + subMenu.offsetWidth > window.innerWidth) {
                x -= x + subMenu.offsetWidth + subMenu.clientWidth - window.innerWidth;
            }

            if (y + subMenu.offsetHeight > window.innerHeight) {
                y -= y + subMenu.offsetHeight - window.innerHeight;
            }

            subMenu.style.display = originalDisplay;
            blazorContextMenu.Show(subMenu.id, x, y, openMenuTarget);

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

    blazorContextMenu.OnMenuItemMouseOut = function (e) {
        if (subMenuTimeout) {
            clearTimeout(subMenuTimeout);
        }
    }

    blazorContextMenu.GetMenuId = function (menuItem) {
        var menu = menuItem.closest(".blazor-context-menu");
        return menu.id;
    };

    blazorContextMenu.MenuItemHasSubmenu = function (menuItem) {
        return !!findFirstChildByClass(menuItem, "blazor-context-submenu");
    }

    return blazorContextMenu;
}({});

blazorContextMenu.Init();

