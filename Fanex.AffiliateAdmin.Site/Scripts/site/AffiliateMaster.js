/*global AFFILIATE*/
var AFFMASTER = (function ($) {
    "use strict";
    return {
        WIDTH_AFFINFO: 256,
        init: function () {
        },
        colBox: function () {
            $("#c-panel").animate({
                left: "20px"
            }, 300, function () {
                $("#l-panel").addClass("l-panel-hide");
            });
        },
        expBox: function () {
            $("#c-panel").animate({
                left: AFFMASTER.WIDTH_AFFINFO + "px"
            }, 300, function () {
                $("#l-panel").removeClass("l-panel-hide");
            });
        },
        colContent: function () {
            $("#ifStatistic").attr("class", "statistic-col");
        },
        expContent: function () {
            $("#ifStatistic").attr("class", "statistic-exp");
        },
        changePassword: function (affId, affName) {
            AFFILIATE.openDialogFrame("AffiliateInfo/ChangePassword?affid=" + affId + "&affname=" + affName, affName + ' - ' + window.Lang.ChangePassword, { width: 500, height: 280 });
        },
        changeAffInfo: function (param) {
            this.updateAffInfo(param, true);
        },
        updateAffInfo: function (param, isReload) {
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("AffiliateInfo/ChangeStatus"),
                type: 'post',
                data: jQuery.param(param),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    if (data.Success) {
                        alert(data.Message);
                        if (isReload) {
                            window.location.reload();
                        }
                        AFFINFO.setStatus();
                    } else {
                        alert(data.Message);
                    }
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        openNewWindow: function (url) {
            window.open(url);
        }
    };
})(jQuery);

$(function () {
    "use strict";
    AFFMASTER.init();
});