/* File Created: April 25, 2014 */
var CloseDialog = (function ($) {

    return {
        init: function () {
            "use strict";

            $(".popup #btnCancel").click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
        }
    };
})(jQuery);

$(function () {
    CloseDialog.init();
});