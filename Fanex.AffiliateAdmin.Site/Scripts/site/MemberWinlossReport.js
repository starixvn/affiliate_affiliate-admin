var MemberWinlossReport = function ($) {
    "use strict";
    
    var submitOptions = {};

    var MEMBER_CURRENCY_OPTION = '0';
    var SYS_CURRENCY_OPTION = '1';
    var AFF_CURRENCY_OPTION = '2';

    var GROUP_BY_DATE = '0';
    var GROUP_BY_MEMBER = '1';

    var EXRATE_COL_INDEX = 5;
    var TURNOVER_COL_INDEX = 11;
    var AFFILIATE_COL_INDEX = 1;

    // number of all common columns (except product columns).
    var gridOption = {};

    //#region change custom grid option.

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
        gridOption.numberOfFrozenColumn = gridOption.numberOfFrozenColumn - 1;
    };

    var changeGridOptionWhenGroupByDate = function (byAffiliate) {
        switch (submitOptions.BaseCurrency) {
            case MEMBER_CURRENCY_OPTION:
                gridOption.enableHiddenColumns = false;
                gridOption.hiddenColumns = byAffiliate ? [EXRATE_COL_INDEX - 2] : [EXRATE_COL_INDEX];
                break;
            case AFF_CURRENCY_OPTION:
                // TODO: more
                gridOption.groupHeaderRows[0].Columns[EXRATE_COL_INDEX].HeaderText = window.MSG.UnitPerBaseCurrency.format($('#BaseCurrency option:selected').text());
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

    //#region columns based on products

    var addTurnoverColumnsWhenOnlyOneProduct = function (product, startIndex) {
        var name = nomalizeProductName(product.Name);

        var turnoverHeader = $.extend(true, {}, window.sampleTurnoverHeader);
        turnoverHeader.Name = name + "Turnover";
        gridOption.groupHeaderRows[0].Columns.splice(startIndex, 0, turnoverHeader);

        var turnoverCol = $.extend(true, {}, window.sampleTurnoverCol);
        turnoverCol.Name = turnoverCol.NameCache = name + "Turnover";
        gridOption.columns.splice(startIndex, 0, turnoverCol);

        gridOption.customizeHeader = false;
    };

    var addTurnoverColumnsWhenOverTwoProducts = function (products, startIndex) {
        var productCount = products.length;

        for (var i = 0; i < productCount; i++) {
            var name = nomalizeProductName(products[i].Name);

            if (i === 0) {
                var turnoverHeader = $.extend(true, {}, window.sampleTurnoverHeader);
                turnoverHeader.Name = name + "Turnover";
                turnoverHeader.MergeColNumber = productCount + 1;
                gridOption.groupHeaderRows[0].Columns.splice(startIndex, 0, turnoverHeader);
            }

            var turnoverCol = $.extend(true, {}, window.sampleTurnoverCol);
            turnoverCol.HeaderCaption = turnoverCol.HeaderText = window.MSG[name];
            turnoverCol.Name = turnoverCol.NameCache = name + "Turnover";
            delete turnoverCol.IsCurrency;
            gridOption.columns.splice(startIndex + i, 0, turnoverCol);
        }

        // add total turnover column.
        var totalTurnoverCol = $.extend(true, {}, window.sampleTurnoverCol);
        totalTurnoverCol.HeaderCaption = window.MSG['Total'];
        totalTurnoverCol.HeaderText = window.MSG['Total'];
        delete totalTurnoverCol.IsCurrency;
        gridOption.columns.splice(startIndex + productCount, 0, totalTurnoverCol);
    };

    var addTurnoverColumnsForProducts = function (products, startIndex) {
        var productCount = products.length;

        if (productCount === 0) {
            return;
        }

        if (productCount === 1) {
            addTurnoverColumnsWhenOnlyOneProduct(products[0], startIndex);
        } else {
            addTurnoverColumnsWhenOverTwoProducts(products, startIndex);
        }
    };

    var addWinlostColumnsWhenOnlyOneProduct = function (product, startHeaderIndex, startColumnIndex) {
        var name = nomalizeProductName(product.Name);

        var companyWLHeader = $.extend(true, {}, window.sampleCompanyWLHeader);
        companyWLHeader.Name = name + "Winlost";
        gridOption.groupHeaderRows[0].Columns.splice(startHeaderIndex, 0, companyWLHeader);

        var winlostCol = $.extend(true, {}, window.sampleWinlostCol);
        winlostCol.Name = winlostCol.NameCache = name + "Winlost";
        gridOption.columns.splice(startColumnIndex, 0, winlostCol);
    };

    var addWinlostColumnsWhenOverTwoProducts = function (products, startHeaderIndex, startColumnIndex) {
        var productCount = products.length;

        for (var i = 0; i < productCount; i++) {
            var name = nomalizeProductName(products[i].Name);

            if (i === 0) {
                // for header
                var companyWLHeader = $.extend(true, {}, window.sampleCompanyWLHeader);
                companyWLHeader.Name = companyWLHeader.NameCache = name + "Winlost";
                companyWLHeader.MergeColNumber = productCount + 1;
                gridOption.groupHeaderRows[0].Columns.splice(startHeaderIndex, 0, companyWLHeader);
            }
            var winlostCol = $.extend(true, {}, window.sampleWinlostCol);
            winlostCol.HeaderCaption = winlostCol.HeaderText = window.MSG[name];
            winlostCol.Name = winlostCol.NameCache = name + "Winlost";
            delete winlostCol.IsCurrency;
            gridOption.columns.splice(startColumnIndex + i, 0, winlostCol);
        }

        var totalWinlostCol = $.extend(true, {}, window.sampleWinlostCol);
        totalWinlostCol.HeaderCaption = window.MSG['Total'];
        totalWinlostCol.HeaderText = window.MSG['Total'];
        delete totalWinlostCol.IsCurrency;
        gridOption.columns.splice(startColumnIndex + productCount, 0, totalWinlostCol);
    };

    var addWinlostColumnsForProducts = function (products, startHeaderIndex, startColumnIndex) {

        var productCount = products.length;

        if (productCount === 0) {
            return;
        }

        if (productCount === 1) {
            addWinlostColumnsWhenOnlyOneProduct(products[0], startHeaderIndex, startColumnIndex);
        } else {
            addWinlostColumnsWhenOverTwoProducts(products, startHeaderIndex, startColumnIndex);
        }
    };
    //#endregion

    var removeRedundantColumnsWhenViewInAffInfo = function (byAffiliate) {
        if (byAffiliate) {
            // remove Affiliate and Currency columns when view in AffInfo.
            var removedColumnCount = 2;

            gridOption.groupHeaderRows[0].Columns.splice(AFFILIATE_COL_INDEX, removedColumnCount);
            gridOption.columns.splice(AFFILIATE_COL_INDEX, removedColumnCount);
            gridOption.numberOfFrozenColumn = gridOption.numberOfFrozenColumn - removedColumnCount;
        }
    };

    // isUseBaseCurrency: add postfix "Base" to all currency columns
    var customizeGridOption = function (sampleGridOption) {
        var startTurnoverPosition = TURNOVER_COL_INDEX;
        // TODO: more
        var byAffiliate = $('#ByAffiliate').length > 0;

        gridOption = $.extend(true, {}, sampleGridOption);

        switch (submitOptions.GroupBy) {
            case GROUP_BY_MEMBER:
                changeGridOptionWhenGroupByMember();
                startTurnoverPosition = startTurnoverPosition - 1;
                break;
            case GROUP_BY_DATE:
                changeGridOptionWhenGroupByDate(byAffiliate);
                break;
        }

        //window.PRODUCTS = [{ Id: 1, Name: "Sportsbook"}];

        var products = getSelectedProducts(window.PRODUCTS);

        addTurnoverColumnsForProducts(products, startTurnoverPosition);

        addWinlostColumnsForProducts(products, gridOption.groupHeaderRows[0].Columns.length, gridOption.columns.length);

        removeRedundantColumnsWhenViewInAffInfo(byAffiliate);
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

    var getMergeColCountForFirstColumnOfFooterRows = function () {

        if (submitOptions.GroupBy === GROUP_BY_MEMBER) {
            return gridOption.numberOfFrozenColumn - 1;
        }

        return gridOption.numberOfFrozenColumn - 2;
    };

    var getPseudoSubTotalRow = function (pseudoSubTotalclass) {
        var columns = [];
        var mergeCols = getMergeColCountForFirstColumnOfFooterRows();
        var startIndex = getStartIndexForFooterRows();

        columns.push({ Value: window.MSG.SubTotal, Name: "WinlostDate", MergeColNumber: mergeCols, MergeRowNumber: 1, TdClass: pseudoSubTotalclass });

        createPlayerCurrencyColumn(columns);

        for (var i = startIndex; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            columns.push({ Value: '', Name: col.Name });
        }

        return [{ Columns: columns, CssClass: "pseudo-subtotal"}];
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
        var mergeCols = getMergeColCountForFirstColumnOfFooterRows();
        var subTotalRowCount = subTotalData.length;
        var startIndex = getStartIndexForFooterRows();
        var actualSubTotalRows = [];

        $.each(subTotalData, function (idx, item) {
            var columns = [];

            if (idx === 0) {
                columns.push({ Value: window.MSG.SubTotal, Name: "WinlostDate", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount, TdClass: realSubTotalclass });
            }

            createPlayerCurrencyColumn(columns, item['PlayerCurrency']);

            for (var i = startIndex; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                columns.push({ Value: item[col.Name], Name: col.Name });
            }

            actualSubTotalRows.push({ Columns: columns, CssClass: "real-subtotal" });
        });

        return actualSubTotalRows;
    };

    var getSubTotalRows = function (subTotalData, hasCurrencyOverTwoRows) {
        var subTotalRows = [];

        if (hasShowSubTotalRows(subTotalData, hasCurrencyOverTwoRows)) {
            var subTotalRowCount = subTotalData.length;
            var realSubTotalclass = subTotalRowCount > 1 ? 'cell-subtotal real-cell' : '';
            var pseudoSubTotalclass = subTotalRowCount > 1 ? 'cell-subtotal pseudo-cell' : '';

            var actualSubTotalRows = getActualSubTotalRows(subTotalData, realSubTotalclass);
            var pseudoSubTotalRow = getPseudoSubTotalRow(pseudoSubTotalclass);

            subTotalRows = actualSubTotalRows.concat(pseudoSubTotalRow);
        }

        return subTotalRows;
    };

    var getStartIndexForFooterRows = function () {
        var mergeCols = getMergeColCountForFirstColumnOfFooterRows();

        if (submitOptions.GroupBy === GROUP_BY_MEMBER) {
            return mergeCols + 1;
        }

        if (submitOptions.BaseCurrency === MEMBER_CURRENCY_OPTION) {
            return mergeCols + 1;
        }

        return mergeCols + 2;
    };

    var getFooterRows = function (subTotalData, totalRowData, hasCurrencyOverTwoRows) {

        window.subTotalStatus = subTotalData.length > 1 ? window.subTotalStatus : window.EXPAND_SUBTOTAL_STATUS;

        var subTotalRows = getSubTotalRows(subTotalData, hasCurrencyOverTwoRows);

        var totalRow = getTotalRow(totalRowData);

        return subTotalRows.concat(totalRow);
    };

    var getTotalRow = function (totalRowData) {
        var mergeCols = getMergeColCountForFirstColumnOfFooterRows();
        var startIndex = getStartIndexForFooterRows();
        var columns = [];

        columns.push({ Value: "<div align='center'>" + window.MSG.Total + "</div>", Name: "WinlostDate", MergeColNumber: mergeCols });

        createPlayerCurrencyColumn(columns, window.System.Currency);

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
            if (col.IsCurrency)
                col.HeaderText = col.HeaderCaption + symbolCurrency;
        }
    };

    function nomalizeProductName(name) {
        return name.replace(/ /g, "");
    }

    return {
        init: function () {
            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });

            if ($('#gridId').length > 0) {
                MemberWinlossReport.submitRequest();
            }

            $('#btnSubmit').submit(function () {
                MemberWinlossReport.submitRequest();
                return false;
            });

            $('#gridId').on('click', '.toggle-indicator', function () {
                window.subTotalStatus = window.subTotalStatus === window.COLLAPSE_SUBTOTAL_STATUS ? window.EXPAND_SUBTOTAL_STATUS : window.COLLAPSE_SUBTOTAL_STATUS;
                AFFILIATE.resetToggleIndicator(gridOption);
            });
        },

        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

            var fromDate = window.moment($('#FromDate').val());
            var toDate = window.moment($('#ToDate').val());

            var fromYear = fromDate.year();
            var toYear = toDate.year();
            var diffYear = toYear - fromYear;

            if (diffYear < 0 || diffYear > 1) {
                return false;
            }

            var fromMonth = fromDate.month();
            var toMonth = toDate.month();
            var diffMonth;

            if (fromDate.year() === toDate.year()) {
                diffMonth = toMonth - fromMonth;
                if (diffMonth < 0 || diffMonth > 2) {
                    $('#FromDate, #ToDate').addClass('input-validation-error');
                    return false;
                }

                return true;
            } else {
                diffMonth = (toMonth + 12) - fromMonth;
                if (diffMonth < 0 || diffMonth > 2) {
                    $('#FromDate, #ToDate').addClass('input-validation-error');
                    return false;
                }

                return true;
            }
        },

        checkAtLeastOneSelectedProduct: function () {
            var selectedProducts = $('#Products').val();

            if (selectedProducts === null || selectedProducts === undefined || selectedProducts.length === 0) {
                return false;
            }

            return true;
        },

        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();

            if (!MemberWinlossReport.checkRangeTime()) {
                alert(window.MSG.RangeTime3Months);
                return;
            }

            if (!MemberWinlossReport.checkAtLeastOneSelectedProduct()) {
                alert(MSG.ChooseAtLeastOneProduct);
                return;
            }

            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
            MemberWinlossReport.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            var baseCurrency = $("#BaseCurrency").val();
            submitOptions["pageIndex"] = currentPage + 1;
            submitOptions["pageSize"] = itemsPerPage;

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Report/MemberWinloss"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    customizeGridOption(window.sampleGridOption);

                    renameCurrencyColumns(baseCurrency);

                    changeHeaderCaptions(baseCurrency);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: MemberWinlossReport.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.MemberWinloss + ' [{0}]'.format(data.TotalItem);

                    if (data.TotalItem > 0) {
                        gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);
                        gridOption.footerRows = getFooterRows(data.SubTotalData, data.TotalRowData, data.HasCurrencyOverTwoRows);
                    }

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem == 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', window.Lang.NoDataInfo);
                        return;
                    } else {
                        AFFILIATE.resetToggleIndicator(gridOption);
                    }

                    AFFILIATE.updateSearchPanel($('#gridId'));
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    };
} (jQuery);

$(function () {
    "use strict";
    MemberWinlossReport.init();
});