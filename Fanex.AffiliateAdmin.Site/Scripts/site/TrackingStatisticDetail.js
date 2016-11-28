/*global AFFILIATE,gridOption,MSG*/
var STATISTICDETAIL = (function ($) {
    "use strict";
    return {
        init: function() {
            STATISTICDETAIL.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        ReRenderAfterPaged: function(currentPage, itemsPerPage) {
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("TrackingStatistic/StatisticDetailList"),
                type: 'post',
                data: "SportCode=" + $("#SportCode").val() + "&LanguageCode=" + $("#LanguageCode").val() +
                    "&FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val() +
                    "&Domain=" + $("#Domain").val() + "&PageIndex=" + (currentPage + 1) + "&PageSize=" + itemsPerPage,
                beforeSend: function() {
                    $.blockUI();
                },
                success: function(data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    $('#gridId').removeClass('noinfo');

                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: STATISTICDETAIL.ReRenderAfterPaged
                    };
                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    if (data.List.length === 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', MSG.NoInfo);
                    } else {
                        $('#gridId').show().fanexGrid(gridOption);
                    }
                },
                complete: function() {
                    $.unblockUI();
                }
            });
        }
    };
}) (jQuery);

$(function () {
    "use strict";
    STATISTICDETAIL.init();
});