/*global AFFILIATE,MSG,moment,System,COLLAPSE_SUBTOTAL_STATUS,EXPAND_SUBTOTAL_STATUS,
    HasCashbackDeduction,subTotalStatus,Lang,samplePShareCol,samplePWinCol,sampleNetGamingCol*/
var CommissionReport = (function ($) {
    "use strict";
    var submitOptions = {};
    var SYS_CURRENCY_OPTION = "1";
    var GROUP_BY_MONTH = "0";
    var MONTH_COL = 1, COMMGROUP_COL = 4;
    var DEDUCTION_PERCENT_COL = 9;
    // number of all common columns (except product columns).
    var _commonColumnCount = 0;
    var _hideColumns = [MONTH_COL, COMMGROUP_COL];
    var _indexProductColumns = [];
    var _isSimpleMode = false;

    var gridOption = {};

    var getMergeColNumberForProducColumn = function () {
        var mergeColNumber = 5;
        return mergeColNumber;
    };

    var getSelectedProducts = function (products) {
        var selectedProducts = $.map(submitOptions.Products.split(','), function (item) {
            return parseInt(item, 10);
        });

        return $.grep(products, function (item) {
            return $.inArray(item.Id, selectedProducts) >= 0 ? true : false;
        });
    };

    // isUseBaseCurrency: add postfix "Base" to all currency columns
    function initProductColumnsForGridOption() {
        var mergeColNumber = getMergeColNumberForProducColumn();

        _indexProductColumns.clear();
        _commonColumnCount = gridOption.columns.length;

        var products = getSelectedProducts(window.PRODUCTS);
        var productCount = products.length;

        for (var i = 0; i < productCount; i++) {
            var name = nomalizeProductName(products[i].Name);

            gridOption.groupHeaderRows[0].Columns.push(
                { HeaderText: window.MSG[name], Name: name + "ActivePlayers", MergeColNumber: mergeColNumber }
            );
            gridOption.columns.push(
                { HeaderText: window.MSG.ActivePlayers, Width: 90, Name: name + "ActivePlayers", HeaderAlign: "center", CellAlign: "right" }
            );
            gridOption.columns.push(
                { HeaderText: window.MSG.BetCount, Width: 60, Name: name + "BetCount", HeaderAlign: "center", CellAlign: "right" }
            );
            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: window.MSG.Turnover, HeaderText: window.MSG.Turnover,
                    Width: 80, Name: name + "Turnover", NameCache: name + "Turnover", HeaderAlign: "center",
                    CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true
                }
            );

            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: window.MSG.PlayerComm, HeaderText: window.MSG.PlayerComm,
                    Width: 80, Name: name + "PlayerComm", NameCache: name + "PlayerComm", HeaderAlign: "center",
                    CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true
                }
            );
            
            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: window.MSG.Winloss, HeaderText: window.MSG.Winloss,
                    Width: 80, Name: name + "Winloss", NameCache: name + "Winloss", HeaderAlign: "center",
                    CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true
                }
            );
        }
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

    function getFooterRows(subTotalData, totalRowData, totalItem, hasShowSubTotalRow) {
        var mergeCols = 5;
        var subTotalRowCount = subTotalData.length;

        var footerRows = [];
        var col, i;
        if (hasShowSubTotalRow === true && totalItem > 1) {
            if (subTotalRowCount > 1 || (subTotalRowCount === 1 && subTotalData[0].Currency !== System.Currency && $('#BaseCurrency').val() !== '1')) {
                var realSubTotalclass = subTotalRowCount > 1 ? 'cell-subtotal real-cell' : '';
                var pseudoSubTotalclass = subTotalRowCount > 1 ? 'cell-subtotal pseudo-cell' : '';
                subTotalStatus = subTotalRowCount > 1 ? subTotalStatus : window.EXPAND_SUBTOTAL_STATUS;  // jshint ignore:line

                $.each(subTotalData, function (idx, item) {

                    var columns = [];

                    if (idx === 0) {
                        columns.push({ Value: MSG.SubTotal, Name: "Username", HeaderAlign: "center",
                            CellAlign: "center", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount,
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

                // pseudo sub total row.
                var columns = [];
                columns.push({ Value: MSG.SubTotal, Name: "Username", HeaderAlign: "center",
                    CellAlign: "center", MergeColNumber: mergeCols, MergeRowNumber: 1,
                    TdClass: pseudoSubTotalclass
                });

                for (i = mergeCols; i < gridOption.columns.length; i++) {
                    col = gridOption.columns[i];
                    columns.push({ Value: '', Name: col.Name });
                }

                footerRows.push({ Columns: columns, CssClass: "pseudo-subtotal" });
            }
        }

        // Total row
        var totalColums = [];
        totalColums.push({ Value: "<div align='center'>" + MSG.Total + "</div>",
            Name: "Username", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount
        });

        for (i = mergeCols; i < gridOption.columns.length; i++) {
            col = gridOption.columns[i];
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
    function nomalizeProductName(name) {
        return name.replace(/ /g, "");
    }
    function hideColumns() {
        var columns = [];
        if (_isSimpleMode) {
            columns = _indexProductColumns.clone();
        }

        if ($("#GroupBy").val() === GROUP_BY_MONTH) {
            $("#gridId").fanexGrid("hideColumns", gridOption, columns);
        }
        else {
            columns = columns.concat(_hideColumns.clone());
            $("#gridId").fanexGrid("hideColumns", gridOption, columns);
        }

        var hasGroupByMonth = submitOptions.GroupBy === GROUP_BY_MONTH;

        if ((_isSimpleMode && !HasCashbackDeduction) || (_isSimpleMode && HasCashbackDeduction && !hasGroupByMonth)) {
            $("#gridId table:lt(4) th div").height(30);
        }
    }
    return {
        init: function () {
            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });

            AFFILIATE.initMonthPicker();
            var $maincontent = $(".main-content");

            // change view mode event
            $maincontent.on("click", "#btnChangeViewMode", function () {
                _isSimpleMode = !_isSimpleMode;
                $(this).attr("title", _isSimpleMode ? "Full Mode" : "Simple Mode")
                    .attr("class", _isSimpleMode ? "icon-circle-arrow-right" : "icon-circle-arrow-left");

                hideColumns();
                AFFILIATE.resetToggleIndicator(gridOption);

            });

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
                window.location = AFFILIATE.getAbsoluteUrl("Report/MemberWinloss?") + jQuery.param(params);
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
                window.location = AFFILIATE.getAbsoluteUrl("Report/MemberWinloss?") + jQuery.param(params);
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
                CommissionReport.submitRequest();
            }

            $('#btnSubmit').click(function () {
                CommissionReport.submitRequest();
                return false;
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
            if (!CommissionReport.checkRangeTime()) {
                alert(window.MSG.RangeTime3Months);
                return;
            }

            if (!CommissionReport.checkAtLeastOneSelectedProduct()) {
                alert(window.MSG.ChooseAtLeastOneProduct);
                return;
            }

            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
            submitOptions["SortBy"] = $('#SortBy').val();
            CommissionReport.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
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
                    gridOption = $.extend(true, {}, window.sampleGridOption);

                    initProductColumnsForGridOption();

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
                        pagerSelectCallBack: CommissionReport.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = MSG.AffiliateWinLossTitle + ' [{0}]'.format(data.TotalItem);

                    gridOption.footerRows = getFooterRows(data.SubTotalData, data.TotalRowData, data.TotalItem, data.hasShowSubTotalRow);
                    
                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    hideColumns();

                    if (_isSimpleMode) {
                        $('#btnChangeViewMode').removeClass('icon-circle-arrow-left').addClass('icon-circle-arrow-right');
                    } else {
                        $('#btnChangeViewMode').removeClass('icon-circle-arrow-right').addClass('icon-circle-arrow-left');
                    }

                    if (data.List.length === 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    } else {
                        $('.status').show();
                        AFFILIATE.resetToggleIndicator(gridOption);
                    }

                    AFFILIATE.updateSearchPanel($('#gridId'));
                },
                error: function () {
                    alert("Occur the error when requesting the Affiliate Commission report! :(");
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
    CommissionReport.init();
});
