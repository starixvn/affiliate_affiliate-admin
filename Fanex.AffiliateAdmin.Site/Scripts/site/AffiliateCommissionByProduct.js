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
        var numProducts = itemLevel1.MergeRowNumber;
        var indexNumberColumn = gridOption.indexNumberColumn; // custom option

        for (var i = 0; i < productColIdx; i++) {

            var col = gridOption.columns[i];
            columns.push({
                Value: (indexNumberColumn === i) ? idxLevel1 + 1 : itemLevel2[col.Name],
                Name: col.Name,
                MergeRowNumber: numProducts > 1 ? numProducts - 1 : numProducts,
                CustomCellRender: AFFILIATE.getCustomCellRender(0, itemLevel2, col)
            });
        }
    };

    var getRowsOfGroup = function (rows, idxLevel1, itemLevel1) {
        var numProducts = itemLevel1.MergeRowNumber;
        var productColIdx = getProductColIndex();
        var rowCssClass = gridOption.rowCssClass; // custom option
        var rowHeight = gridOption.RowHeight;
        var totalPayment = 0;

        $.each(itemLevel1.AffiliateCommissionByProductList, function (idxLevel2, itemLevel2) {
            var columns = [];

            if (idxLevel2 === 0) {
                getFirstColumns(columns, idxLevel1, itemLevel1, itemLevel2);
            }

            for (var i = productColIdx; i < gridOption.columns.length - 1; i++) {
                var col = gridOption.columns[i];
                columns.push({
                    Value: itemLevel2[col.Name],
                    Name: col.Name
                });
            }

            if (idxLevel2 === 0) {
                var idx = gridOption.columns.length - 1;
                var col = gridOption.columns[idx];
                totalPayment = itemLevel2[col.Name];
                columns.push({
                    Value: totalPayment,
                    Name: col.Name,
                    MergeRowNumber: numProducts > 1 ? numProducts - 1 : numProducts
                });
            }

            rows.push({
                Columns: columns,
                CssClass: rowCssClass ? rowCssClass(0, itemLevel2) : undefined,
                RowHeight: rowHeight ? rowHeight(0, itemLevel2) : undefined
            });
        });
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
        var currency = itemLevel1.Currency.toLowerCase();
        var totalPayment = 0;

        $.each(itemLevel1.AffiliateCommissionByProductList, function (idxLevel2, itemLevel2) {
            var columns = [];
            if (idxLevel1 === 0 && idxLevel2 === 0) {
                columns.push(subTotalCol);
            }

            if (idxLevel2 === 0) {
                columns.push({
                    TdClass: numProducts > 1 ? "toggler toggle-{0}".format(currency) : "",
                    Value: itemLevel1.Currency,
                    Name: "Currency",
                    MergeRowNumber: itemLevel1.MergeRowNumber
                });
            }

            for (var i = productColIdx; i < gridOption.columns.length - 1; i++) {
                var col = gridOption.columns[i];
                columns.push({
                    TdClass: currency,
                    Value: itemLevel2[col.Name],
                    Name: col.Name
                });
            }

            if (idxLevel2 === 0) {
                var idx = gridOption.columns.length - 1;
                var col = gridOption.columns[idx];
                totalPayment = itemLevel2[col.Name];
                columns.push({
                    TdClass: currency,
                    Value: itemLevel2[col.Name],
                    Name: col.Name,
                    MergeRowNumber: numProducts > 1 ? numProducts - 1 : numProducts
                });
            }

            rows.push({ Columns: columns, CssClass: "total-of-currencies" });
        });

        if (numProducts > 1) {
            var totalColumns = [];

            totalColumns.push({
                Name: "ProductName",
                Value: window.MSG.All
            });

            for (var i = productColIdx + 1; i < gridOption.columns.length - 1; i++) {
                var col = gridOption.columns[i];
                totalColumns.push({
                    Value: col.IsPercent ? "N/A" : itemLevel1.TotalAffiliateCommissionByProduct[col.Name],
                    Name: col.Name
                });
            }

            var idx = gridOption.columns.length - 1;
            var col = gridOption.columns[idx];
            totalColumns.push({
                Value: totalPayment,
                Name: col.Name
            });

            rows.push({ Columns: totalColumns, CssClass: "total-of-currencies bold" });
        }
    };

    var getSubTotalRows = function (rows, subTotalData) {

        var totalMergeRowNumber = 0;
        $.each(subTotalData, function (idxLevel1, itemLevel1) {
            totalMergeRowNumber += itemLevel1.MergeRowNumber;
        });
        var subTotalCol = {

            Value: "<div align='center'>{0}</div>".format(window.MSG.Total),
            Name: "No",
            MergeRowNumber: totalMergeRowNumber,
            MergeColNumber: getMergeColNumberOfTotalCol(),
            TdClass: "bold"
        };

        $.each(subTotalData, function (idxLevel1, itemLevel1) {
            getSubTotalRowsOfGroup(rows, idxLevel1, itemLevel1, subTotalCol);
        });
    };

    var hasShowTotalByCurrency = function (hasMultipleRows, hasMultipleCurrencyRows) {
        if (!hasMultipleRows) {
            return false;
        }

        if (hasMultipleCurrencyRows) {
            return true;
        }

        if (submitOptions.BaseCurrency !== SYS_CURRENCY_OPTION) {
            return true;
        }

        return false;
    };

    var getBodyRows = function (groups, subTotalData, hasMultipleRows, hasMultipleCurrencyRows) {
        if (groups.length === 0) {
            return [];
        }

        var bodyRows = [];

        $.each(groups, function (idxLevel1, itemLevel1) {
            getRowsOfGroup(bodyRows, idxLevel1, itemLevel1);
        });

        if (hasShowTotalByCurrency(hasMultipleRows, hasMultipleCurrencyRows)) {
            getSubTotalRows(bodyRows, subTotalData);
        }

        return bodyRows;
    };

    var getFooterRows = function (totalRowData, hasMultipleRows) {
        if (!hasMultipleRows && submitOptions.BaseCurrency === SYS_CURRENCY_OPTION) {
            return [];
        }

        var footerRows = [];
        var numProducts = totalRowData.MergeRowNumber;
        var productColIdx = getProductColIndex();
        var currency = "base-currency";
        var totalPayment = 0;
        var totalColumn = {
            TdClass: numProducts > 1 ? "toggler toggle-{0}".format(currency) : '',
            Value: "<div align='center'>{0}</div>".format(window.MSG.TotalIn.format(System.Currency)),
            Name: "No",
            MergeColNumber: getMergeColNumberOfTotalCol() + 1,
            MergeRowNumber: numProducts
        };

        $.each(totalRowData.AffiliateCommissionByProductList, function (idxLevel2, itemLevel2) {
            var columns = [];

            if (idxLevel2 === 0) {
                columns.push(totalColumn);
            }

            for (var i = productColIdx; i < gridOption.columns.length - 1; i++) {
                var col = gridOption.columns[i];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;
                columns.push({
                    TdClass: currency,
                    Value: itemLevel2[colName],
                    Name: col.Name
                });
            }

            if (idxLevel2 === 0) {
                var idx = gridOption.columns.length - 1;
                var col = gridOption.columns[idx];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

                totalPayment = itemLevel2[colName];
                columns.push({
                    TdClass: currency,
                    Value: totalPayment,
                    Name: col.Name,
                    MergeRowNumber: numProducts > 1 ? numProducts - 1 : numProducts
                });
            }

            footerRows.push({ Columns: columns, CssClass: "total-whole-range" });
        });

        if (numProducts > 1) {
            var totalColumns = [];

            totalColumns.push({
                Name: "ProductName",
                Value: window.MSG.All
            });

            for (var i = productColIdx + 1; i < gridOption.columns.length - 1; i++) {
                var col = gridOption.columns[i];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;
                totalColumns.push({
                    Value: col.IsPercent ? "N/A" : totalRowData.TotalAffiliateCommissionByProduct[colName],
                    Name: col.Name
                });
            }

            var idx = gridOption.columns.length - 1;
            var col = gridOption.columns[idx];
            totalColumns.push({
                Value: totalPayment,
                Name: col.Name
            });

            footerRows.push({ Columns: totalColumns, CssClass: "total-whole-range bold" });
        }

        return footerRows;
    };

    var byAffiliate = function () {
        return $('#ByAffiliate').length > 0;
    };

    var getUrl = function () {
        if (byAffiliate()) {
            return AFFILIATE.getAbsoluteUrl("AffiliateCommission/GetDataByAffiliate");
        }

        return AFFILIATE.getAbsoluteUrl("AffiliateCommission/GetData");
    };

    return {
        submitRequest: function (dataRequest) {
            $.extend(true, submitOptions, dataRequest);

            AffiliateCommissionByProduct.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {

            $.ajax({
                url: getUrl(),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageCurrent=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    var gridObject = $('#gridId');
                    gridObject.show();

                    customizeGridOption();

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

                    gridOption.bodyRows = getBodyRows(data.Groups, data.SubTotalData, data.HasMultipleRows, data.HasMultipleCurrencyRows);

                    gridOption.footerRows = getFooterRows(data.TotalRowData, data.HasMultipleRows);

                    gridObject.removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem === 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', window.Lang.NoDataInfo);
                        return;
                    } else {

                        setTimeout(function () {
                            AFFILIATE.resizeGridHeight();
                        }, 200);

                        $('.status').show();
                        ToggleManager.InitForProduct();
                    }
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
