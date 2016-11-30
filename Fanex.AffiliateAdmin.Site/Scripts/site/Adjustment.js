/*global AFFILIATE*/
var ADJUST = (function ($) {
    "use strict";
    function showResultMessage() {
        var $result = $("#ResultCode");
        if($result.length === 0)
        {
            return;
        }

        var code = Number($result.val());
        if (code === 0)
        {
            AFFILIATE.showUpdateStatus(code, $result.data("message"), parent.window.location.reload);
            ADJUST.hideMsg();
        }
        else
        {
            AFFILIATE.showUpdateStatus(code, $result.data("message"), ADJUST.hideMsg);
        }
    }

    return {
        init: function () {
            showResultMessage();
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