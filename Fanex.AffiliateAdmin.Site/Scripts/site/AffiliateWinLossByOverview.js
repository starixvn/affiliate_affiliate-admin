/*global AFFILIATE,MSG,moment,System,COLLAPSE_SUBTOTAL_STATUS,EXPAND_SUBTOTAL_STATUS,subTotalStatus,Lang*/
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
            return "WinlostDate";
        }

        return "Username";
    };

    var hasShowSubTotalRows = function (numCurrencies, firstCurrency, selectedBaseCurrency) {
        if (numCurrencies > 1) {
            return true;
        }

        if (firstCurrency !== window.System.Currency && selectedBaseCurrency !== SYS_CURRENCY_OPTION) {
            return true;
        }

        return false;
    };

    var getSubTotalRows = function (footerRows, subTotalData, totalItem, hasMultipleCurrency) {
        var mergeCols = getMergeColNumberOfTotalCol();
        var numCurrencies = subTotalData.length;
        var firstCurrency = subTotalData[0].Currency;
        var fistColumName = getFirstColumName();

        if (hasMultipleCurrency === true) {
            if (hasShowSubTotalRows(numCurrencies, firstCurrency, submitOptions.BaseCurrency)) {
                var realSubTotalclass = numCurrencies > 1 ? 'cell-subtotal real-cell' : '';

                window.subTotalStatus = numCurrencies > 1 ? window.subTotalStatus : window.EXPAND_SUBTOTAL_STATUS; // jshint ignore:line

                $.each(subTotalData, function (idx, item) {

                    var columns = [];

                    if (idx === 0) {
                        columns.push({
                            Value: MSG.SubTotal,
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
                            column = { Value: MSG.NA, Name: col.Name };
                        } else {
                            column = { Value: item[col.Name], Name: col.Name };
                        }

                        if (i === mergeCols && idx !== 0) {
                            column.PrevRowsColMergeNumber = mergeCols;
                        }
                        columns.push(column);
                    }

                    footerRows.push({ Columns: columns, CssClass: "real-subtotal" });
                });

                getPseudoSubTotalRow(footerRows, numCurrencies);
            }
        }
    };

    var getPseudoSubTotalRow = function (footerRows, numCurrencies) {
        var pseudoSubTotalclass = numCurrencies > 1 ? 'cell-subtotal pseudo-cell' : '';
        var mergeCols = getMergeColNumberOfTotalCol();

        var columns = [];
        columns.push({
            Value: window.MSG.SubTotal,
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

        footerRows.push({ Columns: columns, CssClass: "pseudo-subtotal" });
    };

    function getFooterRows(subTotalData, totalRowData, totalItem, hasMultipleCurrency) {
        var mergeCols = getMergeColNumberOfTotalCol();
        var subTotalRowCount = subTotalData.length;

        var fistColumName = getFirstColumName();

        var footerRows = [];

        getSubTotalRows(footerRows, subTotalData, totalItem, hasMultipleCurrency);

        // Total row
        var totalColums = [];
        totalColums.push({ Value: "<div align='center'>" + window.MSG.Total + "</div>",
            Name: fistColumName, MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount
        });

        for (var i = mergeCols; i < gridOption.columns.length; i++) {
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

            $.extend(true, submitOptions, dataRequest);

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
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
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
                    
                    gridOption.footerRows = getFooterRows(data.SubTotalData, data.TotalRowData, data.TotalItem, data.hasMultipleCurrency);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);


                    if (data.List.length === 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    } else {
                        $('.status').show();
                        AFFILIATE.resetToggleIndicator(gridOption);
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