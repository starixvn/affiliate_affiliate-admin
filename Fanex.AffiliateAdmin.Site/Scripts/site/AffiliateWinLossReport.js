/*global AFFILIATE,MSG,moment,System,COLLAPSE_SUBTOTAL_STATUS,EXPAND_SUBTOTAL_STATUS,subTotalStatus,Lang*/
var AffiliateWinLossReport = (function ($) {
    "use strict";
    var submitOptions = {};
    var SYS_CURRENCY_OPTION = "1";
    var GROUP_BY_AFFILIATE = "0";
    var MONTH_COL = 1, COMMGROUP_COL = 4;
    var _commonColumnCount = 0;
    var _indexProductColumns = [];

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

    // isUseBaseCurrency: add postfix "Base" to all currency columns
    function customGridOption() {
        removeRedundantColumns();
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
        totalColums.push({ Value: "<div align='center'>" + MSG.Total + "</div>",
            Name: fistColumName, MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount
        });

        for (var i = mergeCols; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

            if (col.IsPercent) {
                totalColums.push({ Value: MSG.NA, Name: col.Name });
            } else {
                totalColums.push({ Value: totalRowData[colName], Name: col.Name });
            }
        }

        footerRows.push({ Columns: totalColums, CssClass: 'total-whole-range' });

        return footerRows;
    }

    function changeHeaderCaptions(showCurrencySymbol) {
        var text = showCurrencySymbol ? " (" + System.Prefix + ")" : "";
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

    return {
        init: function () {
            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });

            AFFILIATE.initMonthPicker();

            var $maincontent = $(".main-content");

            // Comm group link event
            $maincontent.on("click", ".commgroup", function () {
                var param = {
                    groupid: $(this).data("groupid"),
                    date: $(this).data("date")
                };
                parent.AFFILIATE.openDialogFrame("CommGroup/ViewSetting?"
                    + jQuery.param(param), MSG.CommGroupSettingFor.replace("{0}", '"'
                    + $(this).html() + '" ' + $(this).data("date")), { width: 900, height: 450 });
            });

            $maincontent.on("click", "a.afflink", function () {
                var params = $('form').serializeObject2();
                params.AffName = $(this).data("affname");
                params["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                params["SortBy"] = $('#SortBy').val();
                params["GroupBy"] = 1;

                delete params.Currencies;

                params["FromDate"] = '01 ' + params["FromDate"];
                var toDateObj = moment('01 ' + params["ToDate"]);
                var lastDateObj = toDateObj.add("months", 1).add("days", -1);
                params["ToDate"] = lastDateObj.format("DD MMM YYYY");

                var url = AFFILIATE.getAbsoluteUrl(byAffiliate() ? "Report/MemberWinlossByAffiliate?" : "Report/MemberWinloss?") + jQuery.param(params);
                window.open(url, "_blank");
            });

            $maincontent.on("click", "a.monthlink", function () {
                var params = $('form').serializeObject2();
                params.AffName = $(this).data("affname");
                params["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                params["SortBy"] = $('#SortBy').val();
                params["GroupBy"] = 0;
                delete params.Currencies;

                var winlostDateObj = moment($(this).attr('data-winlostdate'));
                var firstDayObj = winlostDateObj.date(1);
                var firstDay = firstDayObj.format("DD MMM YYYY");
                var lastDay = firstDayObj.add("months", 1).add("days", -1).format("DD MMM YYYY");

                params["FromDate"] = firstDay;
                params["ToDate"] = lastDay;
                window.location = AFFILIATE.getAbsoluteUrl(byAffiliate() ? "Report/MemberWinlossByAffiliate?" : "Report/MemberWinloss?") + jQuery.param(params);
            });

            $maincontent.on("click", "a.qm", function () {
                var param = {
                    affid: $(this).data("affid"),
                    monthdate: $(this).data("date"),
                    basecurrency: $(this).data("basecurrency"),
                    currency: $(this).data("currency"),
                    products: $(this).data("products"),
                    groupid: $(this).data("groupid")
                };

                var title = MSG.AffQualifiedMemReportDetailFor
                    .format($(this).data("affname"), param.monthdate,
                            $(this).data("currency"), $(this).data("commgroup"));
                parent.AFFILIATE.openDialogFrame("Report/QualifiedMembers?" + jQuery.param(param), title, { width: 900, height: 450 });
            });

            $('#gridId').on('click', '.toggle-indicator', function () {
                subTotalStatus = subTotalStatus === COLLAPSE_SUBTOTAL_STATUS ? EXPAND_SUBTOTAL_STATUS : COLLAPSE_SUBTOTAL_STATUS; // jshint ignore:line
                AFFILIATE.resetToggleIndicator(gridOption);
            });

            if ($('#gridId').length > 0) {
                AffiliateWinLossReport.submitRequest();
            }

            $('#btnSubmit').click(function () {
                AffiliateWinLossReport.submitRequest();
                return false;
            });

            $('.main-content').on('click', '.tab-title', function () {
                if ($(this).hasClass('tag-overview')) {
                    $('#ViewMode').val(0);
                } else {
                    $('#ViewMode').val(1);
                }

                AffiliateWinLossReport.submitRequest();
            });
        },

        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

            var fromDate = moment('01 ' + $('#FromDate').val());
            var toDate = moment('01 ' + $('#ToDate').val());

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
                diffMonth = (toMonth + 11) - fromMonth;
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
            if (!AffiliateWinLossReport.checkRangeTime()) {
                alert(window.MSG.RangeTime3Months);
                return;
            }

            if (!AffiliateWinLossReport.checkAtLeastOneSelectedProduct()) {
                alert(window.MSG.ChooseAtLeastOneProduct);
                return;
            }

            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
            submitOptions["SortBy"] = $('#SortBy').val();

            if (submitOptions.ViewMode === '0') {
                AffiliateWinLossByOverview.submitRequest(submitOptions);
            } else {
                AffiliateWinLossByProduct.submitRequest(submitOptions);
            }
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            var baseCurrency = $("#BaseCurrency").val();

            $.ajax({
                url: $('#SearchForm').attr("action"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    gridOption = $.extend(true, {}, window.sampleOverviewGridOption);

                    customGridOption();

                    for (var i = _commonColumnCount; i < gridOption.columns.length; i++) {
                        _indexProductColumns.push(i);
                    }

                    renameCurrencyColumns(baseCurrency === SYS_CURRENCY_OPTION);
                    changeHeaderCaptions(baseCurrency === SYS_CURRENCY_OPTION);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: AffiliateWinLossReport.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.AffiliateWinLossOverviewTitle.format(data.TotalItem);

                    gridOption.footerRows = getFooterRows(data.SubTotalData, data.TotalRowData, data.TotalItem, data.hasShowSubTotalRow);

                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.List.length === 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', window.Lang.NoDataInfo);
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

$(function () {
    "use strict";
    AffiliateWinLossReport.init();
});
