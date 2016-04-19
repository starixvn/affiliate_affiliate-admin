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
                error: function (response) {
                    if (RECHECKIN.isUndermaintenance(response)) {
                        top.location.href = AFFILIATE.getAbsoluteUrl("Notify");
                    }
                },
                complete: function () {
                }
            });
        },
        isUndermaintenance: function (response) {
            var undermaintenanceCode = 999999;

            if (response) {
                var result = $.parseJSON(response.responseText);
                return result.code === undermaintenanceCode;
            }

            return false;
        }
    };
})(jQuery);

$(function () {
    "use strict";
    RECHECKIN.init();
});