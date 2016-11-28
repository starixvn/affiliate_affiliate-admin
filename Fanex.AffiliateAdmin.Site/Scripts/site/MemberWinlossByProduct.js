/*global AFFILIATE,samplePlaytechCasinoHeader,samplePShareCol,samplePWinCol,sampleNetGamingCol,MemberWinLossByOverview*/
var MemberWinLossByProduct = (function ($) {
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
        return getProductColIndex() + 1;
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
    var customizeGridOption = function (sampleGridOption) {

        // TODO: more
        var byAffiliate = $('#ByAffiliate').length > 0;

        gridOption = $.extend(true, {}, sampleGridOption);

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

    var getSubTotalRowsOfGroup = function (rows, idxLevel1, itemLevel1, subTotalCol) {
        var numProducts = itemLevel1.MergeRowNumber;
        var productColIdx = getProductColIndex();
        var currency = itemLevel1.PlayerCurrency.toLowerCase();

        $.each(itemLevel1.MemberWinLossByProductList, function (idxLevel2, itemLevel2) {
            var columns = [];
            if (idxLevel1 === 0 && idxLevel2 === 0) {
                columns.push(subTotalCol);
            }

            if (idxLevel2 === 0) {
                columns.push({
                    TdClass: numProducts > 1 ? "toggler toggle-{0}".format(currency) : "",
                    Value: itemLevel1.PlayerCurrency,
                    Name: "PlayerCurrency",
                    MergeRowNumber: itemLevel1.MergeRowNumber,
                    MergeColNumber: hasExRateColumn() ? 2 : 1
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
                totalColumns.push({ Value: itemLevel1.TotalMemberWinLossByProduct[col.Name], Name: col.Name });
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
            Name: "WinlostDateStr",
            MergeRowNumber: totalMergeRowNumber,
            MergeColNumber: getMergeColNumberOfTotalCol(),
            TdClass: "bold"
        };

        $.each(subTotalData, function (idxLevel1, itemLevel1) {
            getSubTotalRowsOfGroup(rows, idxLevel1, itemLevel1, subTotalCol);
        });
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

    var hasExRateColumn = function () {
        if (submitOptions.GroupBy === GROUP_BY_DATE && submitOptions.BaseCurrency !== MEMBER_CURRENCY_OPTION) {
            return true;
        }

        return false;
    };

    var getProductColIndex = function () {
        if (hasExRateColumn()) {
            return getMergeColNumberOfTotalCol() + 2;
        }

        return getMergeColNumberOfTotalCol() + 1;
    };

    var getFirstColumns = function (columns, itemLevel1, itemLevel2, rowIndex) {
        var productColIdx = getProductColIndex();
        var indexNumberColumn = gridOption.indexNumberColumn;

        for (var i = 0; i < productColIdx; i++) {

            var col = gridOption.columns[i];
            columns.push({
                Value: indexNumberColumn === i ? (rowIndex + 1) : itemLevel2[col.Name],
                Name: col.Name,
                MergeRowNumber: itemLevel1.MergeRowNumber,
                CustomCellRender: AFFILIATE.getCustomCellRender(0, itemLevel2, col)
            });
        }
    };

    var getRowsOfGroup = function (rows, idxLevel1, itemLevel1) {
        var currentPage = gridOption.pagerOption.currentPage || 0;
        var rowIndex = currentPage * gridOption.pagerOption.itemsPerPage + idxLevel1;
        var numProducts = itemLevel1.MergeRowNumber;
        var productColIdx = getProductColIndex();

        $.each(itemLevel1.MemberWinLossByProductList, function (idxLevel2, itemLevel2) {
            var columns = [];

            if (idxLevel2 === 0) {
                getFirstColumns(columns, itemLevel1, itemLevel2, rowIndex);
            }

            for (var i = productColIdx; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                columns.push({
                    Value: itemLevel2[col.Name], 
                    Name: col.Name,
                    CustomCellRender: AFFILIATE.getCustomCellRender(0, itemLevel2, col)
                });
            }

            rows.push({ Columns: columns });
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
                    Value: itemLevel1.TotalMemberWinLossByProduct[col.Name], 
                    Name: col.Name,
                    CustomCellRender: AFFILIATE.getCustomCellRender(0, itemLevel1.TotalMemberWinLossByProduct, col)
                });
            }

            rows.push({ Columns: totalColumns, CssClass: "sub-total-group bold" });
        }
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
            getRowsOfGroup(bodyRows, idxLevel1, itemLevel1);
        });

        if (hasShowTotalByCurrency(hasMultipleRows, hasMultipleCurrencyRows, firstCurrency)) {
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
        var currency = 'base-currency';

        var totalColumn = {
            TdClass: numProducts > 1 ? "toggler toggle-{0}".format(currency) : "",
            Value: "<div align='center'>{0}</div>".format(window.MSG.TotalIn.format(System.Currency)),
            Name: "WinlostDateStr",
            MergeColNumber: productColIdx,
            MergeRowNumber: numProducts
        };

        $.each(totalRowData.MemberWinLossByProductList, function (idxLevel2, itemLevel2) {
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
                totalColumns.push({ Value: totalRowData.TotalMemberWinLossByProduct[colName], Name: col.Name });
            }

            footerRows.push({ Columns: totalColumns, CssClass: "total-whole-range bold" });
        }

        return footerRows;
    };

    return {
        submitRequest: function (dataRequest) {
            submitOptions = dataRequest;
            MemberWinLossByProduct.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        GetGridOption: function () {
            return gridOption;
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            var baseCurrency = $("#BaseCurrency").val();
            submitOptions["pageIndex"] = currentPage + 1;
            submitOptions["pageSize"] = itemsPerPage;

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("MemberWinLossByProduct/GetData"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {

                    AFFILIATE.PreHandleAjaxResult(data);

                    customizeGridOption(window.sampleProductGridOption);

                    renameCurrencyColumns(baseCurrency);

                    changeHeaderCaptions(baseCurrency);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: MemberWinLossByProduct.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.MemberWinLossByProductTitle.format(data.TotalItem);

                    if (data.TotalItem > 0) {

                        gridOption.bodyRows
                            = getBodyRows(data.Groups, data.SubTotalData, data.HasMultipleRows, data.HasMultipleCurrencyRows, data.FirstCurrency);

                        gridOption.footerRows = getFooterRows(data.TotalRowData, data.HasMultipleRows);
                    }
                    
                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem === 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', window.Lang.NoDataInfo);
                        return;
                    } else {
                        setTimeout(function () {
                            AFFILIATE.resizeGridHeight();
                        }, 200);

                        ToggleManager.InitForProduct();
                    }
                },
                error: function () {
                    alert("Occur the error when requesting the member win loss by product report!");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    };
})(jQuery);
