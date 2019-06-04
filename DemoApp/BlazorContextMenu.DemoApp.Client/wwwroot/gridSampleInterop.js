var gridSampleInterop = (function () {
    return {
        getRowIndex: function (targetItemId) {
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
        }
    };
})();