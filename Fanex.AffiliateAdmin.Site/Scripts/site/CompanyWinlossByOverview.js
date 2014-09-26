/*global AFFILIATE,MSG,System,Lang,sampleNetGamingCol,samplePlaytechCasinoHeader,samplePShareCol,samplePWinCol*/
var CompanyWinlossByOverview = (function ($) {
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

        gridOption.numberOfFrozenColumn = 2;
    };

    var changeGridOptionWhenGroupByDate = function () {

        switch (submitOptions.BaseCurrency) {
            case MEMBER_CURRENCY_OPTION:

                removeExchangeRateCol();

                gridOption.numberOfFrozenColumn = 2;

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

    var hasShowSubTotalRows = function (memberCurrencyCount, firstMemberCurrency, selectedBaseCurrency) {
        if (memberCurrencyCount > 1) {
            return true;
        }

        if (firstMemberCurrency !== window.System.Currency && selectedBaseCurrency !== SYS_CURRENCY_OPTION) {
            return true;
        }

        return false;
    };

    var getRowsOfMemberCurrency = function (rows, idxLevel2, itemLevel2, dateColumn, currencyColumn, exRateColumn) {
        var startIndex = getProductColIndex();

        var columns = [];

        if (idxLevel2 === 0) {
            columns.push(dateColumn);
        }

        columns.push(currencyColumn);

        if (!hasRemoveExRateCol()) {
            columns.push(exRateColumn);
        }

        for (var i = startIndex; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            columns.push({ Value: itemLevel2[col.Name], Name: col.Name });
        }

        rows.push({ Columns: columns });
    };

    var getSubTotalRowOfMemberCurrency = function (rows, itemLevel2) {
        var startIndex = getProductColIndex();

        var columns = [];

        var dateColumn = {
            Value: MSG.SubTotalIn.format(System.Currency),
            Name: "WinlostDate",
            MergeColNumber: startIndex
        };

        columns.push(dateColumn);

        for (var i = startIndex; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            var colName = col.NameCache ? col.NameCache + "Base" : col.Name;
            columns.push({ Value: itemLevel2[colName], Name: col.Name });
        }

        rows.push({ Columns: columns, CssClass: "sub-total-group" });
    };

    var getRowsOfGroup = function (rows, itemLevel1, sampleDateColumn) {

        var dateColumn = $.extend(true, {}, {
            Value: itemLevel1.GroupInfo,
            Name: "WinlostDate",
            MergeRowNumber: itemLevel1.MergeRowNumber
        }, sampleDateColumn);

        var numCurrencies = itemLevel1.MergeRowNumber;
        var firstCurrency = itemLevel1.CompanyWinlossInMemberCurrencies[0].Currency;

        $.each(itemLevel1.CompanyWinlossInMemberCurrencies, function (idxLevel2, itemLevel2) {
            var currencyColumn = {
                Value: itemLevel2.Currency,
                Name: "Currency"
            };

            var exRateColumn = {
                Value: itemLevel2[getHeaderExRate()] === 0 ? '' : itemLevel2[getHeaderExRate()],
                Name: getHeaderExRate()
            };

            getRowsOfMemberCurrency(rows, idxLevel2, itemLevel2, dateColumn, currencyColumn, exRateColumn);
        });

        if (hasShowSubTotalRows(numCurrencies, firstCurrency, submitOptions.BaseCurrency)) {
            getSubTotalRowOfMemberCurrency(rows, itemLevel1.CompanyWinlossInSystemCurrency);
        }
    };

    var getBodyRows = function (groups) {
        if (groups.length === 0) {
            return [];
        }

        var bodyRows = [];

        $.each(groups, function (idxLevel1, itemLevel1) {
            getRowsOfGroup(bodyRows, itemLevel1);
        });

        return bodyRows;
    };

    var getTotalAllRowsOfGroup = function (rows, itemLevel1, sampleDateColumn) {
        var numCurrencies = itemLevel1.MergeRowNumber;

        var dateColumn = $.extend(true, {}, {
            Value: itemLevel1.GroupInfo,
            Name: "WinlostDate",
            MergeRowNumber: itemLevel1.MergeRowNumber,
            TdClass: numCurrencies > 1 ? "subtotal-cell real-cell" : ''
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

            getTotalAllRowsOfMemberCurrency(rows, idxLevel2, itemLevel2, dateColumn, currencyColumn, exRateColumn);
        });
    };

    var getTotalAllRowsOfMemberCurrency = function (rows, idxLevel2, itemLevel2, dateColumn, currencyColumn, exRateColumn) {
        var startIndex = getProductColIndex();
        var columns = [];

        if (idxLevel2 === 0) {
            columns.push(dateColumn);
        }

        columns.push(currencyColumn);
        if (!hasRemoveExRateCol()) {
            columns.push(exRateColumn);
        }

        for (var i = startIndex; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            columns.push({ Value: itemLevel2[col.Name], Name: col.Name });
        }

        rows.push({ Columns: columns, CssClass: "real-row" });
    };

    var getSubTotalAllRowsOfGroup = function (rows, itemLevel2) {
        var startIndex = getProductColIndex();

        var columns = [];

        var dateColumn = {
            Value: MSG.TotalIn.format(System.Currency),
            Name: "WinlostDate",
            MergeColNumber: startIndex
        };

        columns.push(dateColumn);

        for (var i = startIndex; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            var colName = col.NameCache ? col.NameCache + "Base" : col.Name;
            columns.push({ Value: itemLevel2[colName], Name: col.Name });
        }

        rows.push({ Columns: columns, CssClass: "total-whole-range" });
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

    var getFooterRows = function (itemLevel1, hasMultipleRows, hasMultipleCurrencyRows) {
        var footerRows = [];

        if (hasShowTotalByCurrency(hasMultipleRows, hasMultipleCurrencyRows)) {
            var sampleDateColumn =
                {
                    Value: window.MSG.Total,
                    TdClass: itemLevel1.MergeRowNumber > 1 ? "subtotal-cell real-cell" : ''
                };

            getTotalAllRowsOfGroup(footerRows, itemLevel1, sampleDateColumn);

            footerRows.push(getPseudoRow(itemLevel1.MergeRowNumber));
        }

        if (hasMultipleRows) {
            getSubTotalAllRowsOfGroup(footerRows, itemLevel1.CompanyWinlossInSystemCurrency);
        }

        return footerRows;
    };

    var getPseudoRow = function (numCurrenies) {
        // pseudo sub total row
        var columns = [];
        var startIndex = getProductColIndex();
        var pseudoSubTotalclass = numCurrenies > 1 ? 'subtotal-cell pseudo-cell' : '';

        columns.push({
            Value: window.MSG.TotalAll,
            Name: "WinlostDate",
            MergeRowNumber: 1,
            TdClass: pseudoSubTotalclass
        });

        columns.push({ Value: '', Name: "Currency" });

        if (!hasRemoveExRateCol()) {
            columns.push({ Value: '', Name: getHeaderExRate() });
        }

        for (var i = startIndex; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            columns.push({ Value: '', Name: col.Name });
        }

        return { Columns: columns, CssClass: "pseudo-row" };
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

    var getToggleStatus = function (numCurrencies) {
        var collapseStatus = 0, expandStatus = 1;
        return numCurrencies > 1 ? collapseStatus : expandStatus;
    };

    return {
        submitRequest: function (dataRequest) {

            $.extend(true, submitOptions, dataRequest);

            CompanyWinlossByOverview.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        GetGridOption: function () {
            return gridOption;
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("CompanyWinLossByOverview/GetData"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    customizeGridOption(window.sampleOverviewGridOption);

                    renameCurrencyColumns(submitOptions.BaseCurrency);

                    changeHeaderCaptions(submitOptions.BaseCurrency);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: CompanyWinlossByOverview.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.CompanyWinLossOverviewTitle.format(data.TotalItem);

                    if (data.TotalItem > 0) {
                        gridOption.bodyRows = getBodyRows(data.Groups);
                        gridOption.footerRows = getFooterRows(data.TotalData, data.HasMultipleRows, data.HasMultipleCurrencyRows);
                    }

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem === 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', window.Lang.NoDataInfo);
                        return;
                    } else {

                        setTimeout(function () {
                            AFFILIATE.resizeGridHeight();
                        }, 200);

                        ToggleManager.InitForOverview(getToggleStatus());
                    }
                },
                error: function () {
                    alert("Occur the error when requesting the company win loss overview report!");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    };
})(jQuery);
