/*global AFFILIATE*/
var ADJUST = (function ($) {
    "use strict";
    return {
        init: function () {
            $("#btnSubmit").click(function () {
                if (isNaN(Math.abs(parseFloat($("#Amount").val()))) || Math.abs(parseFloat($("#Amount").val())) > parseFloat($("#MaxAmount").val())) {
                    ADJUST.showErrMsg($("#InvalidAmount").val());
                    return false;
                }
            });
        },
        showErrMsg: function (msg) {
            AFFILIATE.showErrorMsg(msg);
            setTimeout(AFFILIATE.hideMsg, 3000);
        },
        hideMsg: function () {
            setTimeout(AFFILIATE.hideMsg, 3000);
        }
    };
})(jQuery);

$(function () {
    "use strict";
    ADJUST.init();
});