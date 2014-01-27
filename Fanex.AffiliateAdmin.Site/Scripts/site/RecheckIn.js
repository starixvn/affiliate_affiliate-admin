var RECHECKIN = function ($) {
    return {
        init: function () {
            setInterval("RECHECKIN.ReCheckIn()", 60000);
            //RECHECKIN.ReCheckIn();
        },
        ReCheckIn: function () {
            $.ajax({
                type: "POST",
                url: AFFILIATE.getAbsoluteUrl("Account/ReCheckIn"),
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    if (result.Success) {//Use Success = IsKickedOut
                        if (result.ErrorMessage == "Expired") {
                            alert(result.Message);
                            top.location.href = AFFILIATE.getAbsoluteUrl("Account/Login");
                        }
                        else if (result.ErrorMessage == "UM") {
                            parent.location.href = AFFILIATE.getAbsoluteUrl("/UnderMaintenance");
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