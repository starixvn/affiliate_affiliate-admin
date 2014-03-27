var STATISTIC = function ($) {
    return {
        init: function () {
            AFFILIATE.RefreshGridData = STATISTIC.ReRenderAfterPaged;
            STATISTIC.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);

            $('#btnSubmit').click(function () {
                STATISTIC.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            var siteUrl = '';
            if ($("#CustomerPage").length > 0) {
                siteUrl = AFFILIATE.getAbsoluteUrl("TrackingStatistic/LandingPageList");
            }
            else {
                siteUrl = AFFILIATE.getAbsoluteUrl("TrackingStatistic/ReferrerList");
            }
            $.ajax({ url: siteUrl,
                type: 'post',
                data: "SportCode=" + $("#CustomerPage").val() + "&LanguageCode=" + $("#BaseLanguage").val() +
                        "&FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val(),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    
                    gridOption.titleInfo.text = MSG.Title + ' [{0}]'.format(data.List.length);
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, STATISTIC.ReRenderAfterPaged);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        showReferrerDetail: function (domainReferrer) {
            var param = {
                domain: domainReferrer,
                fromDate: $("#FromDate").val(),
                toDate: $("#ToDate").val()
            };
            AFFILIATE.openDialogFrame("TrackingStatistic/StatisticDetail?" + jQuery.param(param), MSG.ReferrersStatisticDetails, { width: 820, height: 400 });
        },
        showLandingPageDetail: function (sportCode, languageCode) {
            var param = {
                sportCode: sportCode,
                fromDate: $("#FromDate").val(),
                toDate: $("#ToDate").val(),
                languageCode: languageCode
            };
            AFFILIATE.openDialogFrame("TrackingStatistic/StatisticDetail?" + jQuery.param(param), MSG.LandingPagesStatisticDetails, { width: 800, height: 400 });
        }
    }
} (jQuery);

$(function () {
    STATISTIC.init();
});