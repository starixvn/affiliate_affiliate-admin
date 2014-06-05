/*global AFFILIATE,domain*/
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
        loadPages: function (status) {
            if (status === 'approved') {
                $("#ifStatistic").attr("src", domain + "AffiliateInfo/Statistic");
                $("#ifReport").attr("src", domain + "Statement");
                $("#ifStatistic").attr("class", "statistic-exp");
            } else {
                $("#ifReport").attr("src", domain + "AffiliateInfo/Edit");
                $("#ifStatistic").attr("class", "statistic-hide");
            }
        },
        searchPages: function () {
            $("#ifStatistic").attr("src", "");
            $("#ifReport").attr("src", "");
        },
        colContent: function () {
            $("#ifStatistic").attr("class", "statistic-col");
        },
        expContent: function () {
            $("#ifStatistic").attr("class", "statistic-exp");
        },
        changePassword: function (userName) {
            AFFILIATE.openDialogFrame("AffiliateInfo/ChangePassword", userName + ' - Change Password', { width: 500, height: 280 });
        },
        changeAffInfo: function (groupId, status) {
            var param = {
                status: status,
                groupid: groupId
            };
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
                            parent.location.href = parent.location.href;
                        }
                        window[0].AFFINFO.setStatus();
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
        },
        reloadAffInfo: function () {
            if ($("#ifAffInfo").attr("src").indexOf("IsLoadOtherPage") >= 0) {
                $("#ifAffInfo").attr("src", $("#ifAffInfo").attr("src"));
            } else {
                var url = $("#ifAffInfo").attr("src") + ($("#ifAffInfo").attr("src").indexOf("?") >= 0 ? "&IsLoadOtherPage=1" : "?IsLoadOtherPage=1");
                $("#ifAffInfo").attr("src", url);
            }
        }
    };
})(jQuery);

$(function () {
    "use strict";
    AFFMASTER.init();
});