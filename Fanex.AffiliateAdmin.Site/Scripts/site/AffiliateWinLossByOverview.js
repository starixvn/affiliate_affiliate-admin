/*global AFFILIATE,MSG,Lang*/
var AffiliateWinLossByOverview = (function ($) {

    "use strict";
    var submitOptions = {};
    var SYS_CURRENCY_OPTION = "1";
    var GROUP_BY_AFFILIATE = "0";
    var MONTH_COL = 1, COMMGROUP_COL = 4;

    var gridOption = {};

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
        return getMergeColNumberOfTotalCol() + 1;
    };

    function customGridOption() {

        removeRedundantColumns();

        var products = getSelectProducts(window.PRODUCTS);

        addPlaytechCasinoColumns(products);

        gridOption.numberOfFrozenColumn = getNumberOfFrozenColumn();
    }

    function renameCurrencyColumns(isUseBaseCurrency) {
        var startIndex = gridOption.numberOfFrozenColumn;
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

    var getToggleStatus = function (numCurrencies) {
        var collapseStatus = 0, expandStatus = 1;
        return numCurrencies > 1 ? collapseStatus : expandStatus;
    };

    var getSubTotalRows = function (footerRows, subTotalData) {
        var mergeCols = getMergeColNumberOfTotalCol();
        var numCurrencies = subTotalData.length;
        var fistColumName = getFirstColumName();


        var realSubTotalclass = numCurrencies > 1 ? 'subtotal-cell real-cell' : '';

        $.each(subTotalData, function (idx, item) {

            var columns = [];

            if (idx === 0) {
                columns.push({
                    Value: window.MSG.Total,
                    Name: fistColumName,
                    HeaderAlign: "center",
                    CellAlign: "center",
                    MergeColNumber: mergeCols,
                    MergeRowNumber: numCurrencies,
                    TdClass: realSubTotalclass
                });
            }

            for (var i = mergeCols; i < gridOption.columns.length; i++) {
                var col = gridOption.columns[i];
                var column = {};
                if (col.IsPercent) {
                    column = { Value: window.MSG.NA, Name: col.Name };
                } else {
                    column = { Value: item[col.Name], Name: col.Name };
                }

                if (i === mergeCols && idx !== 0) {
                    column.PrevRowsColMergeNumber = mergeCols;
                }
                columns.push(column);
            }

            footerRows.push({ Columns: columns, CssClass: "real-row" });
        });

        getPseudoSubTotalRow(footerRows, numCurrencies);

    };

    var getPseudoSubTotalRow = function (footerRows, numCurrencies) {
        var pseudoSubTotalclass = numCurrencies > 1 ? 'subtotal-cell pseudo-cell' : '';
        var mergeCols = getMergeColNumberOfTotalCol();

        var columns = [];
        columns.push({
            Value: window.MSG.Total,
            Name: getFirstColumName(),
            HeaderAlign: "center",
            CellAlign: "center",
            MergeColNumber: mergeCols,
            MergeRowNumber: 1,
            TdClass: pseudoSubTotalclass
        });

        for (var i = mergeCols; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            columns.push({ Value: '', Name: col.Name });
        }

        footerRows.push({ Columns: columns, CssClass: "pseudo-row" });
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

    var disabledBaseCurrency = function () {
        return $('#BaseCurrency').prop('disabled');
    };

    function getFooterRows(subTotalData, totalRowData, hasMultipleRows, hasMultipleCurrencyRows, firstCurrency) {
        if (subTotalData.length === 0) {
            return [];
        }

        var mergeCols = getMergeColNumberOfTotalCol();
        var fistColumName = getFirstColumName();

        var footerRows = [];

        if (hasShowTotalByCurrency(hasMultipleRows, hasMultipleCurrencyRows, firstCurrency)) {
            getSubTotalRows(footerRows, subTotalData);
        }

        if (!hasMultipleRows && (disabledBaseCurrency() === true || submitOptions.BaseCurrency === SYS_CURRENCY_OPTION)) {
            return footerRows;
        }

        // Total row
        var totalColums = [];
        totalColums.push({
            Value: "<div align='center'>" + window.MSG.TotalIn.format(window.System.Currency) + "</div>",
            Name: fistColumName,
            MergeColNumber: mergeCols + 1
        });

        for (var i = mergeCols + 1; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

            if (col.IsPercent) {
                totalColums.push({ Value: window.MSG.NA, Name: col.Name });
            } else {
                totalColums.push({ Value: totalRowData[colName], Name: col.Name });
            }
        }

        footerRows.push({ Columns: totalColums, CssClass: 'total-whole-range' });

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
            return AFFILIATE.getAbsoluteUrl("AffiliateWinLossByOverview/GetDataByAffiliate");
        }

        return AFFILIATE.getAbsoluteUrl("AffiliateWinLossByOverview/GetData");
    };

    return {
        submitRequest: function (dataRequest) {
            submitOptions = dataRequest;

            AffiliateWinLossByOverview.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        GetGridOption: function () {
            return gridOption;
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            var baseCurrency = $("#BaseCurrency").val();

            $.ajax({
                url: getUrl(),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage +
                    "&Currencies=" + QueryString.getCurrentParam("currencyid") + "&affname=" + QueryString.getCurrentParam("affname"),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    gridOption = $.extend(true, {}, window.sampleOverviewGridOption);

                    customGridOption();

                    renameCurrencyColumns(baseCurrency === SYS_CURRENCY_OPTION);

                    changeHeaderCaptions(baseCurrency === SYS_CURRENCY_OPTION);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: AffiliateWinLossByOverview.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = MSG.AffiliateWinLossOverviewTitle.format(data.TotalItem);

                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    gridOption.footerRows = getFooterRows(data.SubTotalData, data.TotalRowData, data.HasMultipleRows, data.HasMultipleCurrencyRows, data.FirstCurrency);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);


                    if (data.List.length === 0) {
                        setTimeout(function () {
                            AFFILIATE.resizeGridHeight();
                        }, 200);

                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    } else {
                        $('.status').show();
                        ToggleManager.InitForOverview(getToggleStatus(data.SubTotalData.length));
                    }
                },
                error: function () {
                    alert("Occur the error when requesting the affiliate win loss overview report!");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    };
})(jQuery);