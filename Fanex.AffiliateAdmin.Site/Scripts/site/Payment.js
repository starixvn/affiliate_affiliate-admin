var Payment = function ($) {
    return {
        init: function () {
            $("#GetAll").click(function () {
                var isChecked = $(this).is(':checked');
                Payment.toggleGetAllChecked(isChecked);
            });

            $('#btnSubmit').click(function () {
                Payment.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });
            
            $('#DateOption').change(function () {
                if ($('#DateOption').val() == "0") {
                    $("#FromDate").attr("disabled", "disabled");
                    $("#ToDate").attr("disabled", "disabled");
                }
                else {
                    $("#FromDate").removeAttr("disabled");
                    $("#ToDate").removeAttr("disabled");
                }
            }).change();

            $("#btnCancel").click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
            
            if ($('#gridId').length > 0) {
                Payment.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

        },
        toggleGetAllChecked: function (isChecked) {
            if (isChecked) {
                $("#Status").attr("disabled", "disabled");
                $("#FromDate").attr("disabled", "disabled");
                $("#ToDate").attr("disabled", "disabled");
            }
            else {
                $("#Status").removeAttr("disabled", "disabled");
                $("#FromDate").removeAttr("disabled", "disabled");
                $("#ToDate").removeAttr("disabled", "disabled");
            }
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            AFFILIATE.trimBeforeSubmit();

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Payment/DataList"),
                type: 'post',
                data: "Username=" + $("#Username").val() + "&Status=" + $("#Status").val() + "&AffType=" + $("#AffType").val()
                     + "&PreferredLanguage=" + $("#PreferredLanguage").val() + "&Currency=" + $("#Currency").val() + "&DateOption=" + $("#DateOption").val()
                      + "&FromDate=" + $("#FromDate").val() + "&ToDate=" + $("#ToDate").val() + "&BaseCurrency=" + $("#BaseCurrency").val()
                      + "&SortBy=" + $("#SortBy").val() + "&ExcludeTest=" + $("#ExcludeTest").is(":checked"),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    $('#gridId').show();
                    //To define header has prefix "$" or not
                    var headerColumns = columnsBase.slice(0);
                    var colAmt = columnAmount;
                    colAmt.HeaderText = $("#BaseCurrency").val() != "0" ? HeaderText + "(" + System.Prefix + ")" : HeaderText;

                    headerColumns.push(colAmt);
                    gridOption.columns = headerColumns;

                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.List.length
                    };

                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);
                    gridOption.footerRows = AFFILIATE.genFooterRow(data.List.length > 1 ? data.SubTotal : [], 8, data.Total, 8);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.List.length == 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    } else {
                        $('.status').show();
                    }
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        updatePayment: function (transId) {
            AFFILIATE.openDialogFrame("Payment/UpdatePayment/" + transId, "Trxn.ID " + transId, { width: 500, height: 572 });
        }
    }
} (jQuery);

$(function () {
    Payment.init();
});