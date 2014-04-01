var RECHECKIN = function ($) {
    return {
        init: function () {
            setInterval("RECHECKIN.ReCheckIn()", 60000);
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
                        if (result.ErrorMessage == "Expired") {
                            alert(result.Message);
                            top.location.href = AFFILIATE.getAbsoluteUrl("Account/Login");
                        }

                        if (result.ErrorMessage == "UM") {
                            top.location.href = AFFILIATE.getAbsoluteUrl("Notify");
                        }
                    }
                },
                error: function () {
                },
                complete: function () {
                }
            });
        }
    }
} (jQuery);
$(function () {
    RECHECKIN.init();
});