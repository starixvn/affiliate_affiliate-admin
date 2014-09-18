/*global AFFILIATE,MSG,Lang,sampleNetGamingCol,samplePlaytechCasinoHeader,samplePShareCol,samplePWinCol*/
var CompanyWinlossByProduct = (function ($) {
    "use strict";
    var submitOptions = {};

    var MEMBER_CURRENCY_OPTION = '0';

    var SYS_CURRENCY_OPTION = '1';

    var AFF_CURRENCY_OPTION = '2';

    var GROUP_BY_DATE = "0";

    var GROUP_BY_MONTH = "1";

    var gridOption = {};

    var EXRATE_COL_INDEX = 2;

    var PRODUCT_COL_INDEX = 3;

    var playerWinlossUrl;

    var removeExchangeRateCol = function () {
        gridOption.groupHeaderRows[0].Columns.splice(EXRATE_COL_INDEX, 1);

        gridOption.columns.splice(EXRATE_COL_INDEX, 1);
    };

    var changeHeaderOfWinlostDateColumn = function () {
        gridOption.groupHeaderRows[0].Columns[0].HeaderText = window.MSG.Month;
        gridOption.columns[0].HeaderText = window.MSG.Month;
    };

    var changeHeaderOfExchangeRateColumn = function () {
        gridOption.groupHeaderRows[0].Columns[EXRATE_COL_INDEX].HeaderText = window.MSG.UnitPerBaseCurrency.format($('#BaseCurrency option:selected').text());
        gridOption.groupHeaderRows[0].Columns[EXRATE_COL_INDEX].Name = "AffExRate";
        gridOption.columns[EXRATE_COL_INDEX].Name = "AffExRate";
    };

    var changeGridOptionWhenGroupByMonth = function () {

        removeExchangeRateCol();

        changeHeaderOfWinlostDateColumn();
    };

    var changeGridOptionWhenGroupByDate = function () {

        switch (submitOptions.BaseCurrency) {
            case MEMBER_CURRENCY_OPTION:

                removeExchangeRateCol();

                break;
            case AFF_CURRENCY_OPTION:

                changeHeaderOfExchangeRateColumn();

                break;
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

    // isUseBaseCurrency: add postfix "Base" to all currency columns
    var customizeGridOption = function (sampleProductGridOption) {

        gridOption = $.extend(true, {}, sampleProductGridOption);

        var byAffiliate = $('#ByAffiliate').length > 0;

        playerWinlossUrl = byAffiliate ? AFFILIATE.getAbsoluteUrl("Report/MemberWinlossByAffiliate") : AFFILIATE.getAbsoluteUrl("Report/MemberWinloss");

        switch (submitOptions.GroupBy) {
            case GROUP_BY_MONTH:
                changeGridOptionWhenGroupByMonth();
                break;
            case GROUP_BY_DATE:
                changeGridOptionWhenGroupByDate(byAffiliate);
                break;
        }

        var products = getSelectProducts(window.PRODUCTS);

        addPlaytechCasinoColumns(products);

    };

    var getColumnSuffix = function (baseCurrency) {
        var suffix;

        switch (baseCurrency) {
            case SYS_CURRENCY_OPTION:
                suffix = 'Base';
                break;
            case AFF_CURRENCY_OPTION:
                suffix = '_Aff';
                break;
            default:
                suffix = '';
                break;
        }

        return suffix;
    };

    var renameCurrencyColumns = function (baseCurrency) {
        var suffix = getColumnSuffix(baseCurrency);
        var startIndex = getProductColIndex();
        var col;
        var i;

        for (i = startIndex; i < gridOption.columns.length; i++) {
            col = gridOption.columns[i];
            if (col.NameCache) {
                col.Name = col.NameCache + suffix;
            }
        }

        for (i = startIndex; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.NameCache) {
                col.Name = col.NameCache + suffix;
            }
        }
    };

    var hasRemoveExRateCol = function () {
        if (submitOptions.GroupBy === GROUP_BY_MONTH) {
            return true;
        }

        if (submitOptions.BaseCurrency === MEMBER_CURRENCY_OPTION) {
            return true;
        }

        return false;
    };

    var getProductColIndex = function () {
        return hasRemoveExRateCol() ? PRODUCT_COL_INDEX - 1 : PRODUCT_COL_INDEX;
    };

    var getHeaderExRate = function () {
        if (hasRemoveExRateCol()) {
            return '';
        }

        switch (submitOptions.BaseCurrency) {
            case SYS_CURRENCY_OPTION:
                return "ExRate";
            case AFF_CURRENCY_OPTION:
                return "AffExRate";
            default:
                return "";
        }
    };

    var getRowsOfMemberCurrency = function (rows, idxLevel2, itemLevel2, dateColumn, currencyColumn, exRateColumn) {
        var startIndex = getProductColIndex();
        var numProducts = itemLevel2.CompanyWinlossByProductList.length;

        $.each(itemLevel2.CompanyWinlossByProductList, function (idxLevel3, itemLevel3) {
            var columns = [];

            if (idxLevel2 === 0 && idxLevel3 === 0) {
                columns.push(dateColumn);
            }

            if (idxLevel3 === 0) {
                columns.push(currencyColumn);
                if (!hasRemoveExRateCol()) {
                    columns.push(exRateColumn);
                }
            }

            for (var i = startIndex; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                columns.push({ Value: itemLevel3[col.Name], Name: col.Name });
            }

            rows.push({ Columns: columns });
        });

        if (numProducts > 1) {
            var totalColumns = [];

            totalColumns.push({
                Name: "ProductName",
                Value: MSG.Total
            });

            for (var i = startIndex + 1; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                totalColumns.push({ Value: itemLevel2.TotalCompanyWinlossByProduct[col.Name], Name: col.Name });
            }

            rows.push({ Columns: totalColumns, CssClass: "sub-total-group bold" });
        }
    };

    var getRowsOfGroup = function (rows, itemLevel1, sampleDateColumn) {
        var dateColumn = $.extend(true, {}, {
            Value: itemLevel1.GroupInfo,
            Name: "WinlostDate",
            MergeRowNumber: itemLevel1.MergeRowNumber
        }, sampleDateColumn);

        $.each(itemLevel1.CompanyWinlossInMemberCurrencies, function (idxLevel2, itemLevel2) {
            var currencyColumn = {
                Value: itemLevel2.Currency,
                Name: "Currency",
                MergeRowNumber: itemLevel2.MergeRowNumber
            };

            var exRateColumn = {
                Value: itemLevel2[getHeaderExRate()] === 0 ? '' : itemLevel2[getHeaderExRate()],
                Name: getHeaderExRate(),
                MergeRowNumber: itemLevel2.MergeRowNumber
            };

            getRowsOfMemberCurrency(rows, idxLevel2, itemLevel2, dateColumn, currencyColumn, exRateColumn);
        });
    };

    var getTotalAllRowsOfGroup = function (rows, itemLevel1, sampleDateColumn) {
        var numCurrencies = itemLevel1.CompanyWinlossInMemberCurrencies.length;
        var firstCurrency = itemLevel1.CompanyWinlossInMemberCurrencies[0].Currency;

        if (hasShowSubTotalRows(numCurrencies, firstCurrency, submitOptions.BaseCurrency)) {
            var dateColumn = $.extend(true, {}, {
                Value: itemLevel1.GroupInfo,
                Name: "WinlostDate",
                MergeRowNumber: itemLevel1.MergeRowNumber
            }, sampleDateColumn);

            $.each(itemLevel1.CompanyWinlossInMemberCurrencies, function(idxLevel2, itemLevel2) {
                var currencyColumn = {
                    Value: itemLevel2.Currency,
                    Name: "Currency",
                    MergeRowNumber: itemLevel2.MergeRowNumber
                };

                var exRateColumn = {
                    Value: itemLevel2[getHeaderExRate()] === 0 ? '' : itemLevel2[getHeaderExRate()],
                    Name: getHeaderExRate(),
                    MergeRowNumber: itemLevel2.MergeRowNumber
                };

                getTotalAllRowsOfMemberCurrency(rows, idxLevel2, itemLevel2, dateColumn, currencyColumn, exRateColumn);
            });
        }
    };

    var getTotalAllRowsOfMemberCurrency = function (rows, idxLevel2, itemLevel2, dateColumn, currencyColumn, exRateColumn) {
        var startIndex = getProductColIndex();
        var numProducts = itemLevel2.CompanyWinlossByProductList.length;

        $.each(itemLevel2.CompanyWinlossByProductList, function (idxLevel3, itemLevel3) {
            var columns = [];

            if (idxLevel2 === 0 && idxLevel3 === 0) {
                columns.push(dateColumn);
            }

            if (idxLevel3 === 0) {
                columns.push(currencyColumn);
                if (!hasRemoveExRateCol()) {
                    columns.push(exRateColumn);
                }
            }

            for (var i = startIndex; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                columns.push({ Value: itemLevel3[col.Name], Name: col.Name });
            }

            rows.push({ Columns: columns, CssClass: "real-subtotal total-of-currencies" });
        });

        if (numProducts > 1) {
            var totalColumns = [];

            totalColumns.push({
                Name: "ProductName",
                Value: window.MSG.Total
            });

            for (var i = startIndex + 1; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                totalColumns.push({ Value: itemLevel2.TotalCompanyWinlossByProduct[col.Name], Name: col.Name });
            }

            rows.push({ Columns: totalColumns, CssClass: "real-subtotal total-of-currencies bold" });
        }
    };

    var getBodyRows = function (groups, totaldata) {
        if (groups.length === 0) {
            return [];
        }

        var bodyRows = [];

        $.each(groups, function (idxLevel1, itemLevel1) {
            getRowsOfGroup(bodyRows, itemLevel1);
        });

        var sampleDateColumn =
        {
            Value: MSG.TotalAll,
            TdClass: "cell-subtotal real-cell bold"
            
        };

        getTotalAllRowsOfGroup(bodyRows, totaldata, sampleDateColumn);

        return bodyRows;
    };

    var hasShowSubTotalRows = function (numCurrencies, firstMemberCurrency, selectedBaseCurrency) {
        if (numCurrencies > 1) {
            return true;
        }

        if (firstMemberCurrency !== window.System.Currency && selectedBaseCurrency !== SYS_CURRENCY_OPTION) {
            return true;
        }

        return false;
    };

    var getFooterRows = function (itemLevel1) {
        var footerRows = [];

        var startIndex = getProductColIndex();
        var itemLevel2 = itemLevel1.CompanyWinlossInSystemCurrency;
        var numProducts = itemLevel2.CompanyWinlossByProductList.length;

        var dateColumn = {
            Value: MSG.TotalAllIn.format(System.Currency),
            Name: "WinlostDate",
            MergeRowNumber: itemLevel1.MergeRowNumber,
            MergeColNumber: startIndex
        };

        $.each(itemLevel2.CompanyWinlossByProductList, function (idxLevel3, itemLevel3) {
            var columns = [];
            if (idxLevel3 === 0) {
                columns.push(dateColumn);
            }

            for (var i = startIndex; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;
                columns.push({ Value: itemLevel3[colName], Name: col.Name });
            }

            footerRows.push({ Columns: columns, CssClass: "total-whole-range" });
        });

        if (numProducts > 1) {
            var totalColumns = [];

            totalColumns.push({
                Name: "ProductName",
                Value: MSG.Total
            });

            for (var i = startIndex + 1; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;
                totalColumns.push({ Value: itemLevel2.TotalCompanyWinlossByProduct[colName], Name: col.Name });
            }

            footerRows.push({ Columns: totalColumns, CssClass: "total-whole-range bold" });
        }

        return footerRows;
    };

    var getSymbolCurrency = function (baseCurrency) {
        var symbolCurrency;

        switch (baseCurrency) {
            case SYS_CURRENCY_OPTION:
                symbolCurrency = " (" + window.System.Prefix + ")";
                break;
            case AFF_CURRENCY_OPTION:
                symbolCurrency = " (" + $('#AffiliateCurrencySymbol').val() + ")";
                break;
            default:
                symbolCurrency = "";
                break;
        }

        return symbolCurrency;
    };

    var changeHeaderCaptions = function (baseCurrency) {
        var text = getSymbolCurrency(baseCurrency);
        var i, col;

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

    return {
        submitRequest: function (dataRequest) {

            $.extend(true, submitOptions, dataRequest);

            CompanyWinlossByProduct.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        GetGridOption: function () {
            return gridOption;
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("CompanyWinlossByProduct/GetData"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    customizeGridOption(window.sampleProductGridOption);

                    renameCurrencyColumns(submitOptions.BaseCurrency);

                    changeHeaderCaptions(submitOptions.BaseCurrency);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: CompanyWinlossByProduct.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.CompanyWinLossByProductTitle.format(data.TotalItem);

                    if (data.TotalItem > 0) {
                        gridOption.bodyRows = getBodyRows(data.Groups, data.TotalData);
                        gridOption.footerRows = getFooterRows(data.TotalData, data.HasShowSubTotalRow);
                    }

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem === 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    }
                },
                error: function () {
                    alert("Occur the error when requesting the Company Winloss report! :(");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    };
})(jQuery);
