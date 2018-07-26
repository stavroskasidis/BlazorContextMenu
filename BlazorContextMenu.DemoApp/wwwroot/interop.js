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



DemoApp_GetItemIndex = function (targetItemId) {
    var targetItem = document.getElementById(targetItemId);
    var tr = targetItem.closest("tr");
    if (tr.parentNode.tagName == "THEAD") {
        return -1;
    }

    var children = tr.parentNode.childNodes;
    var num = 0;
    for (var i = 0; i < children.length; i++) {
        if (children[i] == tr) return num;
        if (children[i].nodeType == 1) num++;
    }
    return -1;
};