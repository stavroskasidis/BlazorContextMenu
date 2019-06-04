var dynamicItemsInterop = (function () {
    return {
        getListItemIndex: function (targetItemId) {
            var targetItem = document.getElementById(targetItemId);
            var li = targetItem.closest("li");
            var children = li.parentNode.childNodes;
            var num = 0;
            for (var i = 0; i < children.length; i++) {
                if (children[i] == li) return num;
                if (children[i].nodeType == 1) num++;
            }
            return -1;
        }
    };
})();