/*global AFFILIATE*/
var MemberWinLossByOverview = (function ($) {
    "use strict";

    var submitOptions = {};

    var MEMBER_CURRENCY_OPTION = '0';

    var SYS_CURRENCY_OPTION = '1';

    var AFF_CURRENCY_OPTION = '2';

    var GROUP_BY_DATE = '0';

    var GROUP_BY_MEMBER = '1';

    var EXRATE_COL_INDEX = 5;

    var AFFILIATE_COL_INDEX = 1;

    // number of all common columns (except product columns).
    var gridOption = {};

    //#region change custom grid option.

    var byAffiliate = function () {
        return $('#ByAffiliate').length > 0;
    };

    var changeGridOptionWhenGroupByMember = function () {
        replaceWinlostDateColWithCardinalColToGridOption();

        removeExchangeRateCol();

        changeIndexColumnAndnumberOfFrozenColumn();
    };

    var replaceWinlostDateColWithCardinalColToGridOption = function () {
        gridOption.groupHeaderRows[0].Columns.splice(0, 1, window.cardinalHeaderCol);
        gridOption.columns.splice(0, 1, window.cardinalCol);
    };

    var removeExchangeRateCol = function () {
        gridOption.groupHeaderRows[0].Columns.splice(EXRATE_COL_INDEX, 1);
        gridOption.columns.splice(EXRATE_COL_INDEX, 1);
    };

    var changeIndexColumnAndnumberOfFrozenColumn = function () {
        gridOption.indexNumberColumn = 0;
        gridOption.numberOfFrozenColumn = getNumberOfFrozenColumn();
    };

    var changeGridOptionWhenGroupByDate = function () {
        switch (submitOptions.BaseCurrency) {
            case MEMBER_CURRENCY_OPTION:
                removeExchangeRateCol();
                break;
            case AFF_CURRENCY_OPTION:
                gridOption.groupHeaderRows[0].Columns[EXRATE_COL_INDEX].HeaderText
                    = window.MSG.UnitPerBaseCurrency.format($('#BaseCurrency option:selected').text());
                gridOption.groupHeaderRows[0].Columns[EXRATE_COL_INDEX].Name = "AffExRate";
                gridOption.columns[EXRATE_COL_INDEX].Name = "AffExRate";
                break;
        }
    };

    var getSelectedProducts = function (products) {
        var selectedProducts = $.map(submitOptions.Products.split(','), function (item) {
            return parseInt(item, 10);
        });

        return $.grep(products, function (item) {
            return $.inArray(item.Id, selectedProducts) >= 0 ? true : false;
        });
    };

    //#endregion

    var removeRedundantColumnsWhenViewInAffInfo = function () {
        if (byAffiliate()) {
            // remove Affiliate and Currency columns when view in AffInfo.
            var removedColumnCount = 2;
            gridOption.groupHeaderRows[0].Columns.splice(AFFILIATE_COL_INDEX, removedColumnCount);
            gridOption.columns.splice(AFFILIATE_COL_INDEX, removedColumnCount);
        }
    };

    var getNumberOfFrozenColumn = function () {
        if (submitOptions.GroupBy === GROUP_BY_DATE && submitOptions.BaseCurrency !== MEMBER_CURRENCY_OPTION) {
            return getMergeColNumberOfTotalCol() + 2;
        }

        return getMergeColNumberOfTotalCol() + 1;
    };

    var getMergeColNumberOfTotalCol = function () {
        if (byAffiliate()) {
            return 2;
        }

        if (submitOptions.GroupBy === GROUP_BY_DATE) {
            return 4;
        }

        return 4;
    };

    var addPlaytechCasinoColumns = function (products) {

        var hasPlaytechCasino = false;

        $.each(products, function () {
            if (this.Name === "Playtech Casino") {
                hasPlaytechCasino = true;
                return;
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
    var customizeGridOption = function (sampleOverviewGridOption) {

        // TODO: more
        var byAffiliate = $('#ByAffiliate').length > 0;

        gridOption = $.extend(true, {}, sampleOverviewGridOption);

        switch (submitOptions.GroupBy) {
            case GROUP_BY_MEMBER:
                changeGridOptionWhenGroupByMember();
                break;
            case GROUP_BY_DATE:
                changeGridOptionWhenGroupByDate(byAffiliate);
                break;
        }

        gridOption.numberOfFrozenColumn = getNumberOfFrozenColumn();

        var products = getSelectedProducts(window.PRODUCTS);

        removeRedundantColumnsWhenViewInAffInfo(byAffiliate);

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
        var startIndex = gridOption.numberOfFrozenColumn;
        var i, col;
        var length = gridOption.columns.length;

        for (i = startIndex; i < length; i++) {
            col = gridOption.columns[i];
            if (col.NameCache) {
                col.Name = col.NameCache + suffix;
            }
        }

        length = gridOption.groupHeaderRows[0].Columns.length;
        for (i = startIndex; i < length; i++) {
            col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.NameCache) {
                col.Name = col.NameCache + suffix;
            }
        }
    };

    var getPseudoSubTotalRow = function (pseudoSubTotalclass) {
        var columns = [];
        var mergeCols = getMergeColNumberOfTotalCol();
        var startIndex = getStartIndexForFooterRows();

        columns.push({ Value: window.MSG.Total, Name: "WinlostDateStr", MergeColNumber: mergeCols, MergeRowNumber: 1, TdClass: pseudoSubTotalclass });

        createPlayerCurrencyColumn(columns);

        for (var i = startIndex; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            columns.push({ Value: '', Name: col.Name });
        }

        return [{ Columns: columns, CssClass: "pseudo-row"}];
    };

    var createPlayerCurrencyColumn = function (columns, value) {
        value = typeof value !== "undefined" ? value : '';

        if (submitOptions.GroupBy === GROUP_BY_MEMBER) {
            columns.push({ Value: value, Name: "PlayerCurrency", MergeColNumber: 1 });
            return;
        }

        if (submitOptions.BaseCurrency === MEMBER_CURRENCY_OPTION) {

            columns.push({ Value: value, Name: "PlayerCurrency", MergeColNumber: 1 });
            return;
        }

        columns.push({ Value: value, Name: "PlayerCurrency", MergeColNumber: 2 });
    };

    var getActualSubTotalRows = function (subTotalData, realSubTotalclass) {
        var mergeCols = getMergeColNumberOfTotalCol();
        var subTotalRowCount = subTotalData.length;
        var startIndex = getStartIndexForFooterRows();
        var actualSubTotalRows = [];

        $.each(subTotalData, function (idx, item) {
            var columns = [];

            if (idx === 0) {
                columns.push({ Value: window.MSG.Total, Name: "WinlostDateStr",
                    MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount, TdClass: realSubTotalclass
                });
            }

            createPlayerCurrencyColumn(columns, item['PlayerCurrency']);

            for (var i = startIndex; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                columns.push({ Value: item[col.Name], Name: col.Name });
            }

            actualSubTotalRows.push({ Columns: columns, CssClass: "real-row" });
        });

        return actualSubTotalRows;
    };

    var getSubTotalRows = function (subTotalData, hasCurrencyOverTwoRows) {
        var subTotalRows = [];

        if (hasShowSubTotalRows(subTotalData, hasCurrencyOverTwoRows)) {
            var subTotalRowCount = subTotalData.length;
            var realSubTotalclass = subTotalRowCount > 1 ? 'subtotal-cell real-cell' : '';
            var pseudoSubTotalclass = subTotalRowCount > 1 ? 'subtotal-cell pseudo-cell' : '';

            var actualSubTotalRows = getActualSubTotalRows(subTotalData, realSubTotalclass);
            var pseudoSubTotalRow = getPseudoSubTotalRow(pseudoSubTotalclass);

            subTotalRows = actualSubTotalRows.concat(pseudoSubTotalRow);
        }

        return subTotalRows;
    };

    var getStartIndexForFooterRows = function () {
        var mergeCols = getMergeColNumberOfTotalCol();

        if (submitOptions.GroupBy === GROUP_BY_MEMBER) {
            return mergeCols + 1;
        }

        if (submitOptions.BaseCurrency === MEMBER_CURRENCY_OPTION) {
            return mergeCols + 1;
        }

        return mergeCols + 2;
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

    var getFooterRows = function (subTotalData, totalRowData, hasMultipleRows, hasMultipleCurrencyRows, firstCurrency) {
        var footer = [];

        if (hasShowTotalByCurrency(hasMultipleRows, hasMultipleCurrencyRows, firstCurrency)) {
            footer = getSubTotalRows(subTotalData);
        }

        if (hasMultipleRows || submitOptions.BaseCurrency !== SYS_CURRENCY_OPTION) {
            var totalRow = getTotalRow(totalRowData);
            footer = footer.concat(totalRow);
        }

        return footer;
    };

    var getTotalRow = function (totalRowData) {
        var startIndex = getStartIndexForFooterRows();
        var columns = [];

        columns.push({
            Value: "<div align='center'>" + window.MSG.TotalIn.format(System.Currency) + "</div>",
            Name: "WinlostDateStr",
            MergeColNumber: startIndex
        });

        var length = gridOption.columns.length;
        for (var i = startIndex; i < length; i++) {
            var col = gridOption.columns[i];
            var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

            columns.push({ Value: totalRowData[colName], Name: col.Name });
        }

        return [{ Columns: columns, CssClass: 'total-whole-range'}];
    };

    function hasShowSubTotalRows(subTotalData, hasCurrencyOverTwoRows) {
        var subTotalRowCount = subTotalData.length;

        if (subTotalRowCount <= 0 || hasCurrencyOverTwoRows === false) {
            return false;
        }

        if (subTotalRowCount > 1) {
            return true;
        }

        return subTotalData[0].PlayerCurrency !== window.System.Currency && submitOptions.BaseCurrency !== SYS_CURRENCY_OPTION;
    }

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
        var symbolCurrency = getSymbolCurrency(baseCurrency);
        var i, col;
        var length = gridOption.columns.length;

        for (i = 0; i < length; i++) {
            col = gridOption.columns[i];
            if (col.IsCurrency) {
                col.HeaderText = col.HeaderCaption + symbolCurrency;
            }
        }

        length = gridOption.groupHeaderRows[0].Columns.length;
        for (i = 0; i < length; i++) {
            col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.IsCurrency) {
                col.HeaderText = col.HeaderCaption + symbolCurrency;
            }
        }
    };

    var getToggleStatus = function (numCurrencies) {
        var collapseStatus = 0, expandStatus = 1;
        return numCurrencies > 1 ? collapseStatus : expandStatus;
    };

    return {
        submitRequest: function (dataRequest) {
            submitOptions = dataRequest;
            MemberWinLossByOverview.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        GetGridOption: function () {
            return gridOption;
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            var baseCurrency = $("#BaseCurrency").val();
            submitOptions["pageIndex"] = currentPage + 1;
            submitOptions["pageSize"] = itemsPerPage;

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("MemberWinLossByOverview/GetData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {

                    AFFILIATE.PreHandleAjaxResult(data);

                    customizeGridOption(window.sampleOverviewGridOption);

                    renameCurrencyColumns(baseCurrency);

                    changeHeaderCaptions(baseCurrency);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: MemberWinLossByOverview.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.MemberWinLossOverviewTitle.format(data.TotalItem);

                    if (data.TotalItem > 0) {

                        gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                        gridOption.footerRows
                            = getFooterRows(data.SubTotalData, data.TotalRowData, data.HasMultipleRows, data.HasMultipleCurrencyRows, data.FirstCurrency);
                    }

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem === 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', window.Lang.NoDataInfo);
                        return;
                    } else {
                        setTimeout(function () {
                            AFFILIATE.resizeGridHeight();
                        }, 200);

                        ToggleManager.InitForOverview(getToggleStatus(data.SubTotalData.length));
                    }

                },
                error: function () {
                    alert("Occur the error when requesting the member win loss overview report!");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    };
})(jQuery);
