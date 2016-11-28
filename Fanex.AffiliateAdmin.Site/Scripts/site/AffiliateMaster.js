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
        loadPages: function (param) {
            var queryString = $.param(param);
            if (param.Status === 'approved') {
                $("#ifStatistic").attr("src", AFFILIATE.getAbsoluteUrl("AffiliateInfo/Statistic") + "?" + queryString);
                $("#ifReport").attr("src", AFFILIATE.getAbsoluteUrl("Statement") + "?" + queryString);
                $("#ifStatistic").attr("class", "statistic-exp");
            } else {
                $("#ifReport").attr("src", AFFILIATE.getAbsoluteUrl("AffiliateInfo/Edit") + "?" + queryString);
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
            var url = '';
            var date = new Date();
            if ($("#ifAffInfo").attr("src").indexOf("IsLoadOtherPage") >= 0) {
                url = $("#ifAffInfo").attr("src");
                url = url.substring(0, url.indexOf("&timestamp")) + "&timestamp=" + date.getTime();
                $("#ifAffInfo").attr("src", url);
            } else {
                url = $("#ifAffInfo").attr("src") + ($("#ifAffInfo").attr("src").indexOf("?") >= 0 ? "&IsLoadOtherPage=1" : "?IsLoadOtherPage=1");
                url += "&timestamp=" + date.getTime();
                $("#ifAffInfo").attr("src", url);
            }
        }
    };
})(jQuery);

$(function () {
    "use strict";
    AFFMASTER.init();
});