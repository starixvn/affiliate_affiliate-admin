/*global AFFILIATE,MSG,Lang,samplePShareCol,samplePWinCol,samplePlaytechCasinoHeader,sampleNetGamingCol*/
var AffiliateWinLossByProduct = (function ($) {

    "use strict";
    var submitOptions = {};
    var SYS_CURRENCY_OPTION = "1";
    var GROUP_BY_AFFILIATE = "0";
    var MONTH_COL = 1, COMMGROUP_COL = 4;

    var gridOption = {};

    var naValue = "<span title='{0}'>{1}</span>".format(window.MSG.NATooltip, window.MSG.NA);

    var byAffiliate = function () {
        return $('#ByAffiliate').length > 0;
    };

    var removeRedundantColumns = function () {
        if (!byAffiliate() && submitOptions.GroupBy === GROUP_BY_AFFILIATE) {
            gridOption.groupHeaderRows[0].Columns.splice(MONTH_COL, 1);
            gridOption.columns.splice(MONTH_COL, 1);

            gridOption.groupHeaderRows[0].Columns.splice(COMMGROUP_COL - 1, 1);
            gridOption.columns.splice(COMMGROUP_COL - 1, 1);

            gridOption.numberOfFrozenColumn = gridOption.numberOfFrozenColumn - 2;
        }
    };

    var getSelectProducts = function (products) {
        var selectedProducts = $.map(submitOptions.Products.split(','), function (item) {
            return parseInt(item, 10);
        });

        return $.grep(products, function (item) {
            return $.inArray(item.Id, selectedProducts) >= 0 ? true : false;
        });
    };

    var addPlaytechCasinoColumns = function (products) {

        var hasPlaytechCasino = false;

        $.each(products, function () {
            if (this.Name === "Playtech Casino") {
                hasPlaytechCasino = true;
                return false;
            }
        });

        if (hasPlaytechCasino === true) {
            if (products.length > 1) {
                gridOption.groupHeaderRows[0].Columns.push(samplePlaytechCasinoHeader);

                var pShareCol = $.extend(true, {}, samplePShareCol);
                var pWinCol = $.extend(true, {}, samplePWinCol);
                var netGamingCol = $.extend(true, {}, sampleNetGamingCol);

                delete pShareCol["IsCurrency"];
                delete pWinCol["IsCurrency"];
                delete netGamingCol["IsCurrency"];

                gridOption.columns.push(pShareCol);
                gridOption.columns.push(pWinCol);
                gridOption.columns.push(netGamingCol);
            } else {
                gridOption.columns.push(samplePShareCol);
                gridOption.columns.push(samplePWinCol);
                gridOption.columns.push(sampleNetGamingCol);
            }
        }
    };

    var getNumberOfFrozenColumn = function () {
        return getProductColIndex() + 1;
    };

    function customGridOption() {

        removeRedundantColumns();

        var products = getSelectProducts(window.PRODUCTS);

        addPlaytechCasinoColumns(products);

        gridOption.numberOfFrozenColumn = getNumberOfFrozenColumn();
    }

    function renameCurrencyColumns(isUseBaseCurrency) {
        var startIndex = getProductColIndex();
        var i;
        var col;
        var name;

        for (i = startIndex; i < gridOption.columns.length; i++) {
            col = gridOption.columns[i];
            if (col.NameCache) {
                name = col.NameCache;
                if (isUseBaseCurrency) {
                    name += "Base";
                }
                col.Name = name;
            }
        }

        for (i = startIndex; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.NameCache) {
                name = col.NameCache;
                if (isUseBaseCurrency) {
                    name += "Base";
                }
                col.Name = name;
            }
        }
    }

    var getMergeColNumberOfTotalCol = function () {
        if (byAffiliate()) {
            return 2;
        }

        if (submitOptions.GroupBy === GROUP_BY_AFFILIATE) {
            return 3;
        }

        return 5;
    };

    var getFirstColumName = function () {
        if (byAffiliate()) {
            return "WinlostDateStr";
        }

        return "Username";
    };

    var getProductColIndex = function () {
        return getMergeColNumberOfTotalCol() + 1;
    };

    var getFirstColumns = function (columns, itemLevel1, itemLevel2) {
        var productColIdx = getProductColIndex();

        for (var i = 0; i < productColIdx; i++) {

            var col = gridOption.columns[i];
            columns.push({
                Value: itemLevel2[col.Name],
                Name: col.Name,
                MergeRowNumber: itemLevel1.MergeRowNumber,
                CustomCellRender: AFFILIATE.getCustomCellRender(0, itemLevel2, col)
            });
        }
    };

    var getRowsOfGroup = function (rows, itemLevel1) {
        var numProducts = itemLevel1.MergeRowNumber;
        var productColIdx = getProductColIndex();

        $.each(itemLevel1.AffiliateWinLossByProductList, function (idxLevel2, itemLevel2) {
            var columns = [];

            if (idxLevel2 === 0) {
                getFirstColumns(columns, itemLevel1, itemLevel2);
            }

            for (var i = productColIdx; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                columns.push({ Value: itemLevel2[col.Name], Name: col.Name });
            }

            rows.push({
                Columns: columns,
                CssClass: itemLevel2.Status.toLowerCase()
            });
        });

        if (numProducts > 1) {
            var totalColumns = [];

            totalColumns.push({
                Name: "ProductName",
                Value: window.MSG.All
            });

            for (var i = productColIdx + 1; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                totalColumns.push({
                    Value: col.IsNA ? naValue : itemLevel1.TotalAffiliateWinLossByProduct[col.Name], 
                    Name: col.Name
                });
            }

            rows.push({ Columns: totalColumns, CssClass: "sub-total-group bold" });
        }
    };

    var getSubTotalRowsOfGroup = function (rows, idxLevel1, itemLevel1, subTotalCol) {
        var numProducts = itemLevel1.MergeRowNumber;
        var productColIdx = getProductColIndex();
        var currency = itemLevel1.Currency.toLowerCase();

        $.each(itemLevel1.AffiliateWinLossByProductList, function (idxLevel2, itemLevel2) {
            var columns = [];
            if (idxLevel1 === 0 && idxLevel2 === 0) {
                columns.push(subTotalCol);
            }

            if (idxLevel2 === 0) {
                columns.push({
                    TdClass: numProducts > 1 ? "toggler toggle-{0}".format(currency) : '',
                    Value: itemLevel1.Currency,
                    Name: "Currency",
                    MergeRowNumber: itemLevel1.MergeRowNumber
                });
            }

            for (var i = productColIdx; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                columns.push({
                    TdClass: currency,
                    Value: itemLevel2[col.Name],
                    Name: col.Name
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

            for (var i = productColIdx + 1; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                totalColumns.push({
                    Value:  col.IsNA ? naValue : itemLevel1.TotalAffiliateWinLossByProduct[col.Name], 
                    Name: col.Name
                });
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
            Value: "<div align='center'>{0}</div>".format(window.MSG.Total),
            Name: getFirstColumName(),
            MergeRowNumber: totalMergeRowNumber,
            MergeColNumber: getMergeColNumberOfTotalCol(),
            TdClass: "bold"
        };

        $.each(subTotalData, function (idxLevel1, itemLevel1) {
            getSubTotalRowsOfGroup(rows, idxLevel1, itemLevel1, subTotalCol);
        });
    };

    var hasShowTotalByCurrency = function (hasMultipleRows, hasMultipleCurrencyRows, firstCurrency) {
        if (!hasMultipleRows) {
            return false;
        }

        if (hasMultipleCurrencyRows) {
            return true;
        }

        if (firstCurrency === window.System.Currency) {
            return false;
        }

        if (submitOptions.BaseCurrency !== SYS_CURRENCY_OPTION) {
            return true;
        }

        return false;
    };

    var getBodyRows = function (groups, subTotalData, hasMultipleRows, hasMultipleCurrencyRows, firstCurrency) {
        if (groups.length === 0) {
            return [];
        }

        var bodyRows = [];

        $.each(groups, function (idxLevel1, itemLevel1) {
            getRowsOfGroup(bodyRows, itemLevel1);
        });

        if (hasShowTotalByCurrency(hasMultipleRows, hasMultipleCurrencyRows, firstCurrency)) {
            getSubTotalRows(bodyRows, subTotalData);
        }

        return bodyRows;
    };

    function getFooterRows(totalRowData, hasMultipleRows) {
        if (!hasMultipleRows && submitOptions.BaseCurrency === SYS_CURRENCY_OPTION) {
            return [];
        }

        var footerRows = [];
        var numProducts = totalRowData.MergeRowNumber;
        var productColIdx = getProductColIndex();
        var currency = "base-currency";

        var totalColumn = {
            TdClass: numProducts > 1 ? "toggler toggle-{0}".format(currency) : '',
            Value: "<div align='center'>{0}</div>".format(MSG.TotalIn.format(System.Currency)),
            Name: getFirstColumName(),
            MergeColNumber: getProductColIndex(),
            MergeRowNumber: numProducts
        };

        $.each(totalRowData.AffiliateWinLossByProductList, function (idxLevel2, itemLevel2) {
            var columns = [];

            if (idxLevel2 === 0) {
                columns.push(totalColumn);
            }

            for (var i = productColIdx; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;
                columns.push({
                    TdClass: currency,
                    Value: itemLevel2[colName],
                    Name: col.Name
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

            for (var i = productColIdx + 1; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;
                totalColumns.push({
                    Value: col.IsNA ? naValue : totalRowData.TotalAffiliateWinLossByProduct[colName], 
                    Name: col.Name
                });
            }

            footerRows.push({ Columns: totalColumns, CssClass: "total-whole-range bold" });
        }

        return footerRows;
    }

    function changeHeaderCaptions(showCurrencySymbol) {
        var text = showCurrencySymbol ? " (" + window.System.Prefix + ")" : "";
        var col, i;

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
    }

    var getUrl = function () {
        if (byAffiliate()) {
            return AFFILIATE.getAbsoluteUrl("AffiliateWinLossByProduct/GetDataByAffiliate");
        }

        return AFFILIATE.getAbsoluteUrl("AffiliateWinLossByProduct/GetData");
    };

    return {
        submitRequest: function (dataRequest) {

            $.extend(true, submitOptions, dataRequest);

            AffiliateWinLossByProduct.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        GetGridOption: function () {
            return gridOption;
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            var baseCurrency = $("#BaseCurrency").val();

            $.ajax({
                url: getUrl(),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {

                    AFFILIATE.PreHandleAjaxResult(data);
                    gridOption = $.extend(true, {}, window.sampleProductGridOption);

                    customGridOption();

                    renameCurrencyColumns(baseCurrency === SYS_CURRENCY_OPTION);

                    changeHeaderCaptions(baseCurrency === SYS_CURRENCY_OPTION);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: AffiliateWinLossByProduct.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.AffiliateWinLossByProductTitle.format(data.TotalItem);
                   
                    
                    gridOption.bodyRows = getBodyRows(data.Groups, data.SubTotalData, data.HasMultipleRows, data.HasMultipleCurrencyRows, data.FirstCurrency);

                    gridOption.footerRows = getFooterRows(data.TotalRowData, data.HasMultipleRows, data.HasMultipleCurrencyRows);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem === 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
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
                    alert("Occur the error when requesting the Affiliate Win Loss report! :(");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    };
})(jQuery);