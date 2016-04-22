/*global AFFILIATE*/
var RECHECKIN = (function ($) {
    "use strict";
    return {
        init: function () {
            setInterval(function () { RECHECKIN.ReCheckIn(); }, 60000);
        },
        ReCheckIn: function () {
            $.ajax({
                type: "POST",
                url: AFFILIATE.getAbsoluteUrl("Account/ReCheckIn"),
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    AFFILIATE.PreHandleAjaxResult(result);

                    if (result.Success) {
                        if (result.ErrorMessage === "Expired") {
                            alert(result.Message);
                            top.location.href = AFFILIATE.getAbsoluteUrl("Account/Login");
                        }
                    }
                },
                error: function () {
                },
                complete: function () {
                }
            });
        }
    };
})(jQuery);

$(function () {
    "use strict";
    RECHECKIN.init();
});