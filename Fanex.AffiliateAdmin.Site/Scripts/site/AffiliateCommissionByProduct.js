/*global AFFILIATE,samplePShareCol,samplePWinCol,sampleNetGamingCol*/
var AffiliateCommissionByProduct = (function ($) {

    "use strict";

    var submitOptions = {};
    var SYS_CURRENCY_OPTION = "1";
    var gridOption = {};
    var MONTH_COL = 2;
    var GROUP_BY_AFFILIATE = "0";

    var QM_COL_INDEX = 7;

    var PRODUCT_COL_INDEX = 5;

    var getSelectedProducts = function (products) {
        var selectedProducts = $.map(submitOptions.Products.split(','), function (item) {
            return parseInt(item, 10);
        });

        return $.grep(products, function (item) {
            return $.inArray(item.Id, selectedProducts) >= 0 ? true : false;
        });
    };

    var removeRedundantColumns = function () {
        if (submitOptions.GroupBy === GROUP_BY_AFFILIATE) {
            gridOption.groupHeaderRows[0].Columns.splice(MONTH_COL, 2);
            gridOption.columns.splice(MONTH_COL, 2);
        }
    };

    var getNumberOfFrozenColumn = function () {
        if (submitOptions.GroupBy === GROUP_BY_AFFILIATE) {
            return 4;
        }

        return 6;
    };

    var insertQMColumn = function () {
        gridOption.groupHeaderRows[0].Columns.splice(QM_COL_INDEX, 0,
                {
                    HeaderCaption: "<span title='{0}'>{1}</span>".format(window.MSG.QualifiedMembers, window.MSG.QM),
                    HeaderText: "<span title='{0}'>{1}</span>".format(window.MSG.QualifiedMembers, window.MSG.QM),
                    IsCurrency: false,
                    Name: "TotalQM",
                    MergeRowNumber: 2
                }
            );

        gridOption.columns.splice(QM_COL_INDEX, 0,
            {
                HeaderCaption: "<span title='{0}'>{1}</span>".format(window.MSG.QualifiedMembers, window.MSG.QM),
                HeaderText: "<span title='{0}'>{1}</span>".format(window.MSG.QualifiedMembers, window.MSG.QM),
                Name: "TotalQM",
                IsCurrency: false,
                Width: 80,
                HeaderAlign: "center", CellAlign: "right",
                Formatter: function (rIndex, record, cell, cellFormated) {
                    if (submitOptions.GroupBy === GROUP_BY_AFFILIATE) {
                        return cellFormated;
                    }

                    var actualBaseCurreny = "2";
                    var currency = record.Currency;

                    if (cellFormated) {
                        return "<a class='qm' data-currency='{0}' data-affname='{1}' data-affid='{2}' data-date='{3}' data-basecurrency='{4}' href='javascript:void(0)' data-products='{5}' data-groupid='{6}' data-commgroup='{7}'>{8}</a>"
                                .format(currency, record.Username, record.AffId, window.moment(record.WinlostDate).format("MMM YYYY"), actualBaseCurreny, submitOptions.Products, record.GroupID, record.GroupName, cellFormated);
                    }

                    return 0;
                }
            }
        );
    };

    function customizeGridOption() {

        gridOption = $.extend(true, {}, window.sampleGridOption);

        var products = getSelectedProducts(window.PRODUCTS);

        insertQMColumn();

        removeRedundantColumns();

        gridOption.numberOfFrozenColumn = getNumberOfFrozenColumn();
    }

    var renameCurrencyColumns = function (isUseBaseCurrency) {
        var i;
        var col;
        var name;

        var freezeColumnCount = getNumberOfFrozenColumn();

        // for Normal columns
        for (i = freezeColumnCount; i < gridOption.columns.length; i++) {
            col = gridOption.columns[i];
            if (col.NameCache) {
                name = col.NameCache;
                if (isUseBaseCurrency) {
                    name += "Base";
                }

                col.Name = name;
            }
        }

        // for Header columns
        for (i = freezeColumnCount; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.NameCache) {
                name = col.NameCache;
                if (isUseBaseCurrency) {
                    name += "Base";
                }
                col.Name = name;
            }
        }
    };

    var changeHeaderCaptions = function (showCurrencySymbol) {
        var text = showCurrencySymbol ? " (" + window.System.Prefix + ")" : "";
        var i;
        var col;

        for (i = 0; i < gridOption.columns.length; i++) {
            col = gridOption.columns[i];
            if (col.IsCurrency) {
                col.HeaderText = col.HeaderCaption + text;
            }
        }

        for (i = 0; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.IsCurrency) {
                col.HeaderText = col.HeaderCaption + text;
            }
        }
    };

    var getProductColIndex = function () {
        if (submitOptions.GroupBy === GROUP_BY_AFFILIATE) {
            return PRODUCT_COL_INDEX - 2;
        }

        return PRODUCT_COL_INDEX;
    };

    var getFirstColumns = function (columns, idxLevel1, itemLevel1, itemLevel2) {
        var productColIdx = getProductColIndex();
        var indexNumberColumn = gridOption.indexNumberColumn; // custom option

        for (var i = 0; i < productColIdx; i++) {

            var col = gridOption.columns[i];
            columns.push({
                Value: (indexNumberColumn === i) ? idxLevel1 + 1 : itemLevel2[col.Name],
                Name: col.Name,
                MergeRowNumber: itemLevel1.MergeRowNumber,
                CustomCellRender: AFFILIATE.getCustomCellRender(0, itemLevel2, col)
            });
        }
    };

    var getRowsOfGroup = function (rows, idxLevel1, itemLevel1) {
        var numProducts = itemLevel1.MergeRowNumber;
        var productColIdx = getProductColIndex();
        var rowCssClass = gridOption.rowCssClass; // custom option
        var rowHeight = gridOption.RowHeight;

        $.each(itemLevel1.AffiliateCommissionByProductList, function (idxLevel2, itemLevel2) {
            var columns = [];

            if (idxLevel2 === 0) {
                getFirstColumns(columns, idxLevel1, itemLevel1, itemLevel2);
            }

            for (var i = productColIdx; i < gridOption.columns.length - 1; i++) {
                var col = gridOption.columns[i];
                columns.push({ Value: itemLevel2[col.Name], Name: col.Name });
            }

            if (idxLevel2 === 0) {
                columns.push({
                    Value: itemLevel2.TotalPayment,
                    Name: "TotalPayment",
                    MergeColNumber: 1,
                    MergeRowNumber: itemLevel1.MergeRowNumber
                });
            }

            rows.push({
                Columns: columns,
                CssClass: rowCssClass ? rowCssClass(0, itemLevel2) : undefined,
                RowHeight: rowHeight ? rowHeight(0, itemLevel2) : undefined
            });
        });

        if (numProducts > 1) {
            var totalColumns = [];

            totalColumns.push({
                Name: "ProductName",
                Value: window.MSG.Total
            });

            for (var i = productColIdx + 1; i < gridOption.columns.length - 1; i++) {
                var col = gridOption.columns[i];
                totalColumns.push({ Value: itemLevel1.TotalAffiliateCommissionByProduct[col.Name], Name: col.Name });
            }

            rows.push({ Columns: totalColumns, CssClass: "sub-total-group bold" });
        }
    };


    var getMergeColNumberOfTotalCol = function () {

        if (submitOptions.GroupBy === GROUP_BY_AFFILIATE) {
            return 2;
        }

        return 4;
    };

    var getSubTotalRowsOfGroup = function (rows, idxLevel1, itemLevel1, subTotalCol) {
        var numProducts = itemLevel1.MergeRowNumber;
        var productColIdx = getProductColIndex();

        $.each(itemLevel1.AffiliateCommissionByProductList, function (idxLevel2, itemLevel2) {
            var columns = [];
            if (idxLevel1 === 0 && idxLevel2 === 0) {
                columns.push(subTotalCol);
            }

            if (idxLevel2 === 0) {
                columns.push({
                    Value: itemLevel1.Currency,
                    Name: "Currency",
                    MergeRowNumber: itemLevel1.MergeRowNumber
                });
            }

            for (var i = productColIdx; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                columns.push({ Value: itemLevel2[col.Name], Name: col.Name });
            }

            rows.push({ Columns: columns, CssClass: "total-of-currencies" });
        });

        if (numProducts > 1) {
            var totalColumns = [];

            totalColumns.push({
                Name: "ProductName",
                Value: window.MSG.Total
            });

            for (var i = productColIdx + 1; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                totalColumns.push({ Value: itemLevel1.TotalAffiliateCommissionByProduct[col.Name], Name: col.Name });
            }

            rows.push({ Columns: totalColumns, CssClass: "total-of-currencies bold" });
        }
    };

    var getSubTotalRows = function (rows, subTotalData) {

        var totalMergeRowNumber = 0;
        $.each(subTotalData, function (idxLevel1, itemLevel1) {
            totalMergeRowNumber += itemLevel1.MergeRowNumber;
        });

        var subTotalCol = {
            Value: "<div align='center'>{0}</div>".format(window.MSG.SubTotal),
            Name: "No",
            MergeRowNumber: totalMergeRowNumber,
            MergeColNumber: getMergeColNumberOfTotalCol(),
            TdClass: "bold"
        };

        $.each(subTotalData, function (idxLevel1, itemLevel1) {
            getSubTotalRowsOfGroup(rows, idxLevel1, itemLevel1, subTotalCol);
        });
    };

    var getBodyRows = function (groups, subTotalData) {
        if (groups.length === 0) {
            return [];
        }

        var bodyRows = [];

        $.each(groups, function (idxLevel1, itemLevel1) {
            getRowsOfGroup(bodyRows, idxLevel1, itemLevel1);
        });

        getSubTotalRows(bodyRows, subTotalData);

        return bodyRows;
    };

    var getFooterRows = function(totalRowData) {
        var footerRows = [];
        var numProducts = totalRowData.MergeRowNumber;
        var productColIdx = getProductColIndex();

        var totalColumn = {
            Value: "<div align='center'>{0}</div>".format(window.MSG.Total),
            Name: "No",
            MergeColNumber: getMergeColNumberOfTotalCol(),
            MergeRowNumber: numProducts
        };

        var currencyColumn = {
            Value: "",
            Name: "Currency",
            MergeRowNumber: numProducts
        };

        $.each(totalRowData.AffiliateCommissionByProductList, function(idxLevel2, itemLevel2) {
            var columns = [];

            if (idxLevel2 === 0) {
                columns.push(totalColumn);
                columns.push(currencyColumn);
            }

            for (var i = productColIdx; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;
                columns.push({ Value: itemLevel2[colName], Name: col.Name });
            }

            footerRows.push({ Columns: columns, CssClass: "total-whole-range" });
        });

        if (numProducts > 1) {
            var totalColumns = [];

            totalColumns.push({
                Name: "ProductName",
                Value: window.MSG.Total
            });

            for (var i = productColIdx + 1; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;
                totalColumns.push({ Value: totalRowData.TotalAffiliateCommissionByProduct[colName], Name: col.Name });
            }

            footerRows.push({ Columns: totalColumns, CssClass: "total-whole-range bold" });
        }

        return footerRows;
    };

    return {
        submitRequest: function (dataRequest) {
            $.extend(true, submitOptions, dataRequest);

            AffiliateCommissionByProduct.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("AffiliateCommission/GetData"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageCurrent=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    var gridObject = $('#gridId');
                    gridObject.show();

                    customizeGridOption(data.HasCashbackDeduction);

                    var baseCurrency = $("#BaseCurrency").val();

                    renameCurrencyColumns(baseCurrency === SYS_CURRENCY_OPTION);

                    changeHeaderCaptions(baseCurrency === SYS_CURRENCY_OPTION);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: AffiliateCommissionByProduct.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.AffiliateCommissionTitle + ' [{0}]'.format(data.TotalItem);

                    gridOption.bodyRows = getBodyRows(data.Groups, data.SubTotalData);

                    gridOption.footerRows = getFooterRows(data.TotalRowData);

                    gridObject.removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem === 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', window.Lang.NoDataInfo);
                        return;
                    } else {
                        $('.status').show();
                    }

                    AFFILIATE.updateSearchPanel(gridObject);
                },
                error: function () {
                    alert("Occur the error when requesting affiliate commission report!");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },

        showCommGroup: function (groupId) {
            var param = {
                groupid: groupId
            };
            parent.AFFILIATE.openDialogFrame(AFFILIATE.getAbsoluteUrl("CommGroup/ViewSetting?")
                + $.param(param), window.MSG.CommissionGroup, { width: 1000, height: 450 });
        }
    };
})(jQuery);
