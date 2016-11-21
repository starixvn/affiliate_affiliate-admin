/*global AFFILIATE,gridOption,MSG,Lang,System*/
var STATEMENT = (function ($) {
    "use strict";
    
    var submitOptions = {};
    return {
        init: function () {
            if ($('#gridId').length > 0) {
                STATEMENT.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

            $('#DateOption').change(function () {
                if ($('#DateOption').val() === "0") {
                    $("#FromDate").attr("disabled", "disabled");
                    $("#ToDate").attr("disabled", "disabled");
                } else {
                    $("#FromDate").removeAttr("disabled");
                    $("#ToDate").removeAttr("disabled");
                }
            });

            $('.search-panel form').submit(function () {
                STATEMENT.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption); // jshint ignore:line

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('.search-panel form').serializeObject2();

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Statement/ListStatement"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage + "&affid=" + QueryString.getParam(window.location.href, "affid"),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    
                    var gridObject = $('#gridId');

                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        maxItemsPerPage: 100,
                        totalItem: data.List.length
                    };

                    var symbolCurrency = $("#BaseCurrency").val() === "1" ? " (" + System.Prefix + ")" : '';

                    gridOption.columns[3].HeaderText = MSG.Amount + symbolCurrency;

                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    gridObject.removeClass("noinfo").fanexGrid(gridOption);

                    if (data.List.length === 0) {
                        gridObject.addClass("noinfo").html(Lang.NoDataInfo);
                    }

                    AFFILIATE.updateSearchPanel(gridObject);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        showDetail: function (transId) {
            AFFILIATE.openDialogFrame("Payment/UpdatePayment?transId=" + transId + "&fromStatement=true", "Trxn.ID " + transId, { width: 500, height: 572 });
        }
    };
}) (jQuery);

$(function () {
    "use strict";
    STATEMENT.init();
});