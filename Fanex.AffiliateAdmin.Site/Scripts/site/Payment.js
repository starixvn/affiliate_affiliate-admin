/*global AFFILIATE,HeaderText*/
var Payment = (function ($) {
    "use strict";

    var gridOption = {};
    var submitOptions = {};

    var getToggleStatus = function (numCurrencies) {
        var collapseStatus = 0, expandStatus = 1;
        return numCurrencies > 1 ? collapseStatus : expandStatus;
    };

    return {
        init: function () {

            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });

            $("#GetAll").click(function () {
                var isChecked = $(this).is(':checked');
                Payment.toggleGetAllChecked(isChecked);
            });

            $('#btnSubmit').click(function () {
                Payment.submitRequest();
                return false;
            });

            $('#DateOption').change(function () {
                if ($('#DateOption').val() === "0") {
                    $("#FromDate").attr("disabled", "disabled");
                    $("#ToDate").attr("disabled", "disabled");
                } else {
                    $("#FromDate").removeAttr("disabled");
                    $("#ToDate").removeAttr("disabled");
                }
            }).change();

            if ($('#gridId').length > 0) {
                Payment.submitRequest();
            }
        },
        toggleGetAllChecked: function (isChecked) {
            if (isChecked) {
                $("#Status").attr("disabled", "disabled");
                $("#FromDate").attr("disabled", "disabled");
                $("#ToDate").attr("disabled", "disabled");
            } else {
                $("#Status").removeAttr("disabled", "disabled");
                $("#FromDate").removeAttr("disabled", "disabled");
                $("#ToDate").removeAttr("disabled", "disabled");
            }
        },

        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();

            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
            Payment.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = $.extend(true, {}, window.sampleGridOption);
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            AFFILIATE.trimBeforeSubmit();
            var url = AFFILIATE.getAbsoluteUrl("Payment/DataList");

            $.ajax({
                url: url,
                type: 'post',
                data: $.param(submitOptions),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    $('#gridId').show();
                    //To define header has prefix "$" or not
                    var headerColumns = window.columnsBase.slice(0);
                    var colAmt = window.columnAmount;
                    colAmt.HeaderText = $("#BaseCurrency").val() !== "0" ? HeaderText + " (" + window.System.Prefix + ")" : HeaderText;
                    colAmt.Name = $("#BaseCurrency").val() !== "0" ? "ConvertedAmt" : "Amt";

                    headerColumns.push(colAmt);
                    gridOption.columns = headerColumns;

                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.List.length
                    };

                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    if (data.List.length > 0) {
                        gridOption.footerRows = Payment.genFooterRow(gridOption, data.SubTotal, 8, data.Total, 8);
                    }

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.List.length === 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', window.Lang.NoDataInfo);
                        return;
                    } else {
                        ToggleManager.InitForOverview(getToggleStatus(data.SubTotal.length));
                        $('.status').show();
                    }
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },

        /*jshint ignore:start */
        genOneRow: function (columns, record, dataRow, startColIdx) {
            startColIdx = startColIdx === undefined ? 0 : startColIdx;
            dataRow = dataRow === undefined ? [] : dataRow;

            for (var i = startColIdx; i < columns.length; i++) {
                (function () {
                    var colName = columns[i].Name;
                    var col = columns[i];
                    dataRow.push({
                        Name: colName,
                        Value: record[colName],
                        CustomCellRender: function (cell, cellFormated) {
                            if (col.Formatter) {
                                return col.Formatter(record, cell, cellFormated);
                            } else {
                                return cellFormated;
                            }
                        }
                    });
                } ());
            }

            return dataRow;
        },
        /*jshint ignore:end */

        genFooterRow: function (gridOption, listSubTotalRows, colSpanSub, totalRow, colSpanTotal) {
            var columns = gridOption.columns;
            var footerRows = [];
            var subRowLength = listSubTotalRows.length;
            var subRowCurrency = listSubTotalRows[0];

            if (subRowLength > 1 || (subRowLength === 1 && subRowCurrency["Currency"] !== window.System.Currency)) {
                var realSubTotalclass = subRowLength > 1 ? 'subtotal-cell real-cell' : '';
                var pseudoSubTotalclass = subRowLength > 1 ? 'subtotal-cell pseudo-cell' : '';
                window.subTotalStatus = subRowLength > 1 ? window.subTotalStatus : window.EXPAND_SUBTOTAL_STATUS;

                $.each(listSubTotalRows, function (idx, item) {
                    var colSubTotal = [];

                    if (idx === 0) {
                        colSubTotal.push({ Value: window.Lang.SubTotal, Name: "RowID",
                            MergeColNumber: colSpanSub, MergeRowNumber: subRowLength, TdClass: realSubTotalclass
                        });
                    }

                    colSubTotal = Payment.genOneRow(columns, item, colSubTotal, colSpanSub);
                    footerRows.push({ Columns: colSubTotal, CssClass: "real-row" });
                });

                // pseudo sub total row
                var colSubTotal = [];
                colSubTotal.push({ Value: window.Lang.SubTotal, Name: "RowID", MergeColNumber: colSpanSub, MergeRowNumber: 1, TdClass: pseudoSubTotalclass });
                colSubTotal.push({ Value: "", Name: "Currency" });
                colSubTotal.push({ Value: "", Name: $("#BaseCurrency").val() !== "0" ? "ConvertedAmt" : "Amt" });
                footerRows.push({ Columns: colSubTotal, CssClass: "pseudo-row" });
            }

            //Total Row
            var colTotal = [];
            colTotal.push({ Value: window.Lang.Total, Name: "RowID", MergeColNumber: colSpanTotal });
            colTotal = Payment.genOneRow(columns, totalRow[0], colTotal, colSpanSub);
            footerRows.push({ Columns: colTotal, CssClass: "total" });

            return footerRows;
        },

        updatePayment: function (transId) {
            AFFILIATE.openDialogFrame("Payment/UpdatePayment?transId=" + transId, "Trxn.ID " + transId, { width: 500, height: 572 });
        }
    };
})(jQuery);

$(function () {
    "use strict";
    Payment.init();
});