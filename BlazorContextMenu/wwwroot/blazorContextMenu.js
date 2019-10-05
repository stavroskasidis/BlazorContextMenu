var blazorContextMenu = function (blazorContextMenu) {

    var closest = null;
    if (window.Element && !Element.prototype.closest) {
        closest = function (el, s) {
            var matches = (el.document || el.ownerDocument).querySelectorAll(s), i;
            do {
                i = matches.length;
                while (--i >= 0 && matches.item(i) !== el) { };
            } while ((i < 0) && (el = el.parentElement));
            return el;
        };
    }
    else {
        closest = function (el, s) {
            return el.closest(s);
        };
    }


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
        var foundElement = null;
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

    var menuHandlerReference = null;

    blazorContextMenu.SetMenuHandlerReference = function (dotnetRef) {
        if (!menuHandlerReference) {
            menuHandlerReference = dotnetRef;
        }
    }

    blazorContextMenu.ManualShow = function (menuId, x ,y) {
        var menu = document.getElementById(menuId);
        if (!menu) throw new Error("No context menu with id '" + menuId + "' was found");
        openMenuId = menuId;
        openMenuTarget = null;
        showMenuCommon(menu, menuId, x, y, null, null);
    }

    blazorContextMenu.OnContextMenu = function (e, menuId) {
        var menu = document.getElementById(menuId);
        if (!menu) throw new Error("No context menu with id '" + menuId + "' was found");
        openMenuId = menuId;
        openMenuTarget = e.target;
        var triggerDotnetRef = JSON.parse(e.currentTarget.dataset["dotnetref"]);
        showMenuCommon(menu, menuId, e.x, e.y, e.target, triggerDotnetRef);
        e.preventDefault();
        return false;
    };

    var showMenuCommon = function (menu, menuId, x, y, target, triggerDotnetRef) {
        return blazorContextMenu.Show(menuId, x, y, target, triggerDotnetRef).then(function () {
            //check for overflow
            var leftOverflownPixels = menu.offsetLeft + menu.clientWidth - window.innerWidth;
            if (leftOverflownPixels > 0) {
                menu.style.left = (menu.offsetLeft - menu.clientWidth) + "px";
            }

            var topOverflownPixels = menu.offsetTop + menu.clientHeight - window.innerHeight;
            if (topOverflownPixels > 0) {
                menu.style.top = (menu.offsetTop - menu.clientHeight) + "px";
            }
        });
    }

    blazorContextMenu.Init = function () {
        document.addEventListener("mouseup", function (e) {
            if (openMenuId) {
                var menuElement = document.getElementById(openMenuId);
                if (menuElement && menuElement.dataset["autohide"] == "true") {
                    var clickedInsideMenu = menuElement.contains(e.target);
                    if (!clickedInsideMenu) {
                        blazorContextMenu.Hide(openMenuId);
                        openMenuId = null;
                        openMenuTarget = null;
                    }
                }
            }
        });
    };

    blazorContextMenu.Show = function (menuId, x, y, target, triggerDotnetRef) {
        var targetId = null;
        if (target && !target.id) {
            //add an id to the target dynamically so that it can be referenced later 
            //TODO: Rewrite this once this Blazor limitation is lifted
            target.id = guid();
            targetId = target.id;
        }
        return menuHandlerReference.invokeMethodAsync('ShowMenu', menuId, x.toString(), y.toString(), targetId, triggerDotnetRef);
    }

    blazorContextMenu.Hide = function (menuId) {
        menuHandlerReference.invokeMethodAsync('HideMenu', menuId);
    }

    var subMenuTimeout = null;
    blazorContextMenu.OnMenuItemMouseOver = function (e, xOffset, currentItemElement) {
        if (closest(e.target, ".blazor-context-menu__wrapper") != closest(currentItemElement, ".blazor-context-menu__wrapper")) {
            //skip child menu mouseovers
            return;
        }
        if (currentItemElement.getAttribute("itemEnabled") != "true") return;

        var subMenu = findFirstChildByClass(currentItemElement, "blazor-context-submenu");
        if (!subMenu) return; //item does not contain a submenu

        subMenuTimeout = setTimeout(function () {
            subMenuTimeout = null;

            var currentMenu = closest(currentItemElement, ".blazor-context-menu__wrapper");
            var currentMenuList = currentMenu.children[0];
            var targetRect = currentItemElement.getBoundingClientRect();
            var x = targetRect.left + currentMenu.clientWidth - xOffset;
            var y = targetRect.top;

            blazorContextMenu.Show(subMenu.id, x, y, openMenuTarget).then(function () {
                var leftOverflownPixels = subMenu.offsetLeft + subMenu.clientWidth - window.innerWidth;
                if (leftOverflownPixels > 0) {
                    subMenu.style.left = (subMenu.offsetLeft - subMenu.clientWidth - currentMenu.clientWidth - xOffset) + "px"
                }

                var topOverflownPixels = subMenu.offsetTop + subMenu.clientHeight - window.innerHeight;
                if (topOverflownPixels > 0) {
                    subMenu.style.top = (subMenu.offsetTop - topOverflownPixels) + "px";
                }

                var closeSubMenus = function () {
                    var childSubMenus = findAllChildsByClass(currentItemElement, "blazor-context-submenu");
                    var i = childSubMenus.length;
                    while (i--) {
                        var subMenu = childSubMenus[i];
                        blazorContextMenu.Hide(subMenu.id);
                    }

                    i = currentMenuList.childNodes.length;
                    while (i--) {
                        var childNode = currentMenuList.childNodes[i];
                        if (childNode == currentItemElement) continue;
                        childNode.removeEventListener("mouseover", closeSubMenus);
                    }
                };

                var i = currentMenuList.childNodes.length;
                while (i--) {
                    var childNode = currentMenuList.childNodes[i];
                    if (childNode == currentItemElement) continue;

                    childNode.addEventListener("mouseover", closeSubMenus);
                }
            });
        }, 200);
    }

    blazorContextMenu.OnMenuItemMouseOut = function (e) {
        if (subMenuTimeout) {
            clearTimeout(subMenuTimeout);
        }
    }


    blazorContextMenu.RegisterTriggerReference = function (triggerElement, triggerDotNetRef) {
        triggerElement.dataset["dotnetref"] = JSON.stringify(triggerDotNetRef.serializeAsArg());
    }

    return blazorContextMenu;
}({});

blazorContextMenu.Init();