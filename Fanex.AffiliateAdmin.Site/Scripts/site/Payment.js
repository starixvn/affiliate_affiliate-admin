var Payment = function ($) {
    var gridOption = {};

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

            $('#gridId').on('click', '.toggle-indicator', function () {
                subTotalStatus = subTotalStatus == COLLAPSE_SUBTOTAL_STATUS ? EXPAND_SUBTOTAL_STATUS : COLLAPSE_SUBTOTAL_STATUS;
                AFFILIATE.resetToggleIndicator(gridOption);
            });

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
            gridOption = $.extend(true, {}, sampleGridOption);
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
                    colAmt.HeaderText = $("#BaseCurrency").val() != "0" ? HeaderText + " (" + System.Prefix + ")" : HeaderText;
                    colAmt.Name = $("#BaseCurrency").val() != "0" ? "ConvertedAmt" : "Amt";

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

                    if (data.List.length == 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    } else {
                        AFFILIATE.resetToggleIndicator(gridOption);
                        $('.status').show();
                    }
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },

        genOneRow: function (columns, record, dataRow, startColIdx) {
            startColIdx = startColIdx == undefined ? 0 : startColIdx;
            dataRow = dataRow == undefined ? [] : dataRow;

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
                            } else
                                return cellFormated;
                        }
                    });
                } ());
            }

            return dataRow;
        },

        genFooterRow: function (gridOption, listSubTotalRows, colSpanSub, totalRow, colSpanTotal) {
            var columns = gridOption.columns;
            var footerRows = [];
            var subRowLength = listSubTotalRows.length;
            var subRowCurrency = listSubTotalRows[0];

            if (subRowLength > 1 || (subRowLength == 1 && subRowCurrency["Currency"] != System.Currency)) {
                var realSubTotalclass = subRowLength > 0 ? 'cell-subtotal real-cell' : '';
                var pseudoSubTotalclass = subRowLength > 0 ? 'cell-subtotal pseudo-cell' : '';


                $.each(listSubTotalRows, function (idx, item) {
                    var colSubTotal = [];

                    if (idx == 0) {
                        colSubTotal.push({ Value: Lang.SubTotal, Name: "RowID", MergeColNumber: colSpanSub, MergeRowNumber: subRowLength, TdClass: realSubTotalclass });
                    }

                    colSubTotal = Payment.genOneRow(columns, item, colSubTotal, colSpanSub);
                    footerRows.push({ Columns: colSubTotal, CssClass: "real-subtotal" });
                });

                // pseudo sub total row
                var colSubTotal = [];
                colSubTotal.push({ Value: Lang.SubTotal, Name: "RowID", MergeColNumber: colSpanSub, MergeRowNumber: 1, TdClass: pseudoSubTotalclass });
                colSubTotal.push({ Value: "", Name: "Currency" });
                colSubTotal.push({ Value: "", Name: $("#BaseCurrency").val() != "0" ? "ConvertedAmt" : "Amt" });
                footerRows.push({ Columns: colSubTotal, CssClass: "pseudo-subtotal" });

            }

            //Total Row
            var colTotal = [];
            colTotal.push({ Value: Lang.Total, Name: "RowID", MergeColNumber: colSpanTotal });
            colTotal = Payment.genOneRow(columns, totalRow[0], colTotal, colSpanSub);
            footerRows.push({ Columns: colTotal, CssClass: "total" });

            return footerRows;
        },

        updatePayment: function (transId) {
            AFFILIATE.openDialogFrame("Payment/UpdatePayment?transId=" + transId, "Trxn.ID " + transId, { width: 500, height: 572 });
        }
    }
} (jQuery);

$(function () {
    Payment.init();
});