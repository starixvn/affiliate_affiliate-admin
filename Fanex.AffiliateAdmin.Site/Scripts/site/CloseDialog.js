/* File Created: April 25, 2014 */
var CloseDialog = (function ($) {
    "use strict";
    return {
        init: function () {
            $(".popup #btnCancel").click(function () {
                if (parent.AFFILIATE.currentOpenDialog) {
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
                }
            });
        }
    };
})(jQuery);

$(function () {
    "use strict";
    CloseDialog.init();
});