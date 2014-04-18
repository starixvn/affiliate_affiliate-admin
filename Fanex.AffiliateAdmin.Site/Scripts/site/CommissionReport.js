var CommissionReport = function ($) {
    var submitOptions = {};
    var SYS_CURRENCY_OPTION = 1;
    var GROUP_BY_MONTH = "0";
    var MONTH_COL = 1, COMMGROUP_COL = 4;
    var DEDUCTION_PERCENT_COL = 9;
    var DEDUCTION_AMOUNT_COL = 10;
    // number of all common columns (except product columns).
    var _commonColumnCount = 0;
    var _freezeColumnCount = 4;
    var _hideColumns = [MONTH_COL, COMMGROUP_COL];
    var _bonusColumns = [DEDUCTION_PERCENT_COL, DEDUCTION_AMOUNT_COL];
    var _indexProductColumns = [];
    var _isSimpleMode = false;
    var _gridOption = {};

    // isUseBaseCurrency: add postfix "Base" to all currency columns
    function initProductColumnsForGridOption() {
        var hasGroupByMonth = $("#GroupBy").val() === GROUP_BY_MONTH;
        var mergeColNumber = hasGroupByMonth ? 7 : 5;

        if (HasCashbackDeduction) {
            if (hasGroupByMonth) {
                _gridOption.groupHeaderRows[0].Columns.splice(DEDUCTION_PERCENT_COL, 0, sampleCashbackHeader);
                _gridOption.columns.splice(DEDUCTION_PERCENT_COL, 0, sampleDeductedPercentageCol, sampleAmountCol);
            } else {
                var cashbackHeader = $.extend(true, {}, sampleCashbackHeader);
                cashbackHeader.Name = "Cashback";
                cashbackHeader.NameCache = "Cashback";
                cashbackHeader.MergeColNumber = 1;
                cashbackHeader.MergeRowNumber = 2;
                cashbackHeader.IsCurrency = true;

                _gridOption.groupHeaderRows[0].Columns.splice(DEDUCTION_PERCENT_COL, 0, cashbackHeader);
                _gridOption.columns.splice(DEDUCTION_PERCENT_COL, 0, sampleAmountCol);
            }
        }

        _indexProductColumns.clear();
        _commonColumnCount = _gridOption.columns.length;

        for (var i = 0; i < PRODUCTS.length; i++) {
            var name = nomalizeProductName(PRODUCTS[i].Name);

            _gridOption.groupHeaderRows[0].Columns.push(
                { HeaderText: MSG[name], Name: name + "ActivePlayers", MergeColNumber: mergeColNumber }
            );
            _gridOption.columns.push(
                { HeaderText: MSG.ActivePlayers, Width: 90, Name: name + "ActivePlayers", HeaderAlign: "center", CellAlign: "right" }
            );
            _gridOption.columns.push(
                { HeaderText: MSG.BetCount, Width: 60, Name: name + "BetCount", HeaderAlign: "center", CellAlign: "right" }
            );
            _gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Turnover, HeaderText: MSG.Turnover, Width: 70, Name: name + "Turnover", NameCache: name + "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );
            _gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Winloss, HeaderText: MSG.Winloss, Width: 80, Name: name + "Winloss", NameCache: name + "Winloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );

            // add QM column to hidden list
            if (hasGroupByMonth) {

                _gridOption.columns.push(
                    {
                        IsCurrency: false,
                        HeaderCaption: MSG.QM,
                        HeaderText: MSG.QM,
                        Width: 80,
                        Name: name + "QM",
                        HeaderAlign: "center",
                        CellAlign: "right",
                        Formatter: function (rIndex, record, cell, cellFormated) {
                            var actualBaseCurreny = $('#BaseCurrency').val() == "0" ? "2" : "1";
                            var currency = actualBaseCurreny == "1" ? System.Currency : record.Currency;

                            if (cellFormated)
                                return "<a class='qm' data-currency='{0}' data-affname='{1}' data-affid='{2}' data-date='{3}' data-basecurrency='{4}' href='javascript:void(0)'>{5}</a>".format(currency, record.Username, record.AffId, moment(record.WinlostDate).format("MMM YYYY"), actualBaseCurreny, cellFormated);
                            return 0;
                        }
                    }
                );
                // add CommPercent column to hidden list
                _gridOption.columns.push(
                    { IsCurrency: false, IsPercent: true, HeaderCaption: MSG.CommPercent, HeaderText: MSG.CommPercent, Width: 80, Name: name + "CommissionPercent", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
                );
            }

            _gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Commission, HeaderText: MSG.Commission, Width: 80, Name: name + "Commission", NameCache: name + "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );
        }
    }
    function renameCurrencyColumns(isUseBaseCurrency) {
        for (var i = _freezeColumnCount; i < _gridOption.columns.length; i++) {
            var col = _gridOption.columns[i];
            if (col.NameCache) {
                var name = col.NameCache;
                if (isUseBaseCurrency)
                    name += "Base";
                col.Name = name;
            }
        }

        for (var i = _freezeColumnCount; i < _gridOption.groupHeaderRows[0].Columns.length; i++) {
            var col = _gridOption.groupHeaderRows[0].Columns[i];
            if (col.NameCache) {
                var name = col.NameCache;
                if (isUseBaseCurrency)
                    name += "Base";
                col.Name = name;
            }
        }
    }

    function getFooterRows(subTotalData, totalRowData, totalItem, hasShowSubTotalRow) {
        var mergeCols = 5;
        var subTotalRowCount = subTotalData.length;

        var footerRows = [];

        if (hasShowSubTotalRow === true && totalItem > 1) {
            if (subTotalRowCount > 1 || (subTotalRowCount == 1 && subTotalData[0].Currency != System.Currency && $('#BaseCurrency').val() != '1')) {
                var realSubTotalclass = subTotalRowCount > 1 ? 'cell-subtotal real-cell' : '';
                var pseudoSubTotalclass = subTotalRowCount > 1 ? 'cell-subtotal pseudo-cell' : '';
                subTotalStatus = subTotalRowCount > 1 ? subTotalStatus : EXPAND_SUBTOTAL_STATUS;

                $.each(subTotalData, function (idx, item) {

                    var columns = [];

                    if (idx == 0) {
                        columns.push({ Value: MSG.SubTotal, Name: "Username", HeaderAlign: "center", CellAlign: "center", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount, TdClass: realSubTotalclass });
                    }

                    for (var i = mergeCols; i < _gridOption.columns.length; i++) {
                        var col = _gridOption.columns[i];
                        var column = {};
                        if (col.IsPercent) {
                            column = { Value: MSG.NA, Name: col.Name };                            
                        } else {
                            column = { Value: item[col.Name], Name: col.Name };
                        }

                        if (i == mergeCols && idx != 0) {
                            column.PrevRowsColMergeNumber = mergeCols;
                        }
                        columns.push(column);
                    }

                    footerRows.push({ Columns: columns, CssClass: "real-subtotal" });
                });

                // pseudo sub total row.
                var columns = [];
                columns.push({ Value: MSG.SubTotal, Name: "Username", HeaderAlign: "center", CellAlign: "center", MergeColNumber: mergeCols, MergeRowNumber: 1, TdClass: pseudoSubTotalclass });

                for (var i = mergeCols; i < _gridOption.columns.length; i++) {
                    var col = _gridOption.columns[i];
                    columns.push({ Value: '', Name: col.Name });
                }

                footerRows.push({ Columns: columns, CssClass: "pseudo-subtotal" });
            }
        }

        // Total row
        var totalColums = [];
        totalColums.push({ Value: "<div align='center'>" + MSG.Total + "</div>", Name: "Username", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount });

        for (var i = mergeCols; i < _gridOption.columns.length; i++) {
            var col = _gridOption.columns[i];
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
        for (var i = 0; i < _gridOption.columns.length; i++) {
            var col = _gridOption.columns[i];
            if (col.IsCurrency)
                col.HeaderText = col.HeaderCaption + text;
        }

        for (var i = 0; i < _gridOption.groupHeaderRows[0].Columns.length; i++) {
            var col = _gridOption.groupHeaderRows[0].Columns[i];
            if (col.IsCurrency)
                col.HeaderText = col.HeaderCaption + text;
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
            $("#gridId").fanexGrid("hideColumns", _gridOption, columns);
        }
        else {
            columns = columns.concat(_hideColumns.clone());
            $("#gridId").fanexGrid("hideColumns", _gridOption, columns);
        }

        var hasGroupByMonth = submitOptions.GroupBy === GROUP_BY_MONTH;

        if ((_isSimpleMode && !HasCashbackDeduction) || (_isSimpleMode && HasCashbackDeduction && !hasGroupByMonth))
            $("#gridId table:lt(4) th div").height(30);
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
                AFFILIATE.resetToggleIndicator(_gridOption);

            });
            // Comm group link event
            $maincontent.on("click", ".commgroup", function () {
                var param = {
                    groupid: $(this).data("groupid"),
                    date: $(this).data("date")
                };
                parent.AFFILIATE.openDialogFrame("CommGroup/ViewSetting?" + jQuery.param(param), MSG.CommGroupSettingFor.replace("{0}", '"' + $(this).html() + '" ' + $(this).data("date")), { width: 900, height: 450 });
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
                window.location = AFFILIATE.getAbsoluteUrl("Report/PlayerWinloss?") + jQuery.param(params);
            });
            $maincontent.on("click", "a.monthlink", function () {
                var params = $('form').serializeObject2();
                params.AffName = $(this).data("affname");
                params["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                params["SortBy"] = $('#SortBy').val();
                params["GroupBy"] = 0;
                params.PlayerCurrencies = params.Currencies;
                delete params.Currencies;

                var winlostDateObj = moment($(this).attr('data-winlostdate'));
                var firstDayObj = winlostDateObj.date(1);
                var firstDay = firstDayObj.format("DD MMM YYYY");
                var lastDay = firstDayObj.add("months", 1).add("days", -1).format("DD MMM YYYY");

                params["FromDate"] = firstDay;
                params["ToDate"] = lastDay;
                window.location = AFFILIATE.getAbsoluteUrl("Report/PlayerWinloss?") + jQuery.param(params);
            });

            $maincontent.on("click", "a.qm", function () {
                var param = {
                    affid: $(this).data("affid"),
                    monthdate: $(this).data("date"),
                    basecurrency: $(this).data("basecurrency"),
                    currency: $(this).data("currency")
                };
                var title = MSG.AffQualifiedMemReportDetailFor.replace("{0}", $(this).data("affname"));
                title = title.replace("{1}", param.monthdate) + " - " + MSG.Currency + ": " + $(this).data("currency");
                parent.AFFILIATE.openDialogFrame("Report/QualifiedMembers?" + jQuery.param(param), title, { width: 900, height: 450 });
            });

            $('#gridId').on('click', '.toggle-indicator', function () {
                subTotalStatus = subTotalStatus == COLLAPSE_SUBTOTAL_STATUS ? EXPAND_SUBTOTAL_STATUS : COLLAPSE_SUBTOTAL_STATUS;
                AFFILIATE.resetToggleIndicator(_gridOption);
            });

            if ($('#gridId').length > 0) {
                CommissionReport.submitRequest();
            }

            $('form').submit(function () {
                CommissionReport.submitRequest();
                return false;
            });

            $('#btnCancel').click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
        },

        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

            var value = $('#GroupBy').val();

            var fromDate = moment('01 ' + $('#FromDate').val());
            var toDate = moment('01 ' + $('#ToDate').val());


            var fromYear = fromDate.year();
            var toYear = toDate.year();
            var diffYear = toYear - fromYear;

            if (diffYear < 0 || diffYear > 1)
                return false;

            var fromMonth = fromDate.month();
            var toMonth = toDate.month();

            if (fromDate.year() === toDate.year()) {

                var diffMonth = toMonth - fromMonth;

                if (diffMonth < 0 || diffMonth > 2) {
                    $('#FromDate, #ToDate').addClass('input-validation-error');
                    return false;
                }

                return true;

            } else {

                var diffMonth = (toMonth + 11) - fromMonth;

                if (diffMonth < 0 || diffMonth > 2) {
                    $('#FromDate, #ToDate').addClass('input-validation-error');
                    return false;
                }

                return true;
            }
        },

        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            if (!CommissionReport.checkRangeTime()) {
                alert(MSG.RangeTime3Months);
            } else {
                submitOptions = $('form').serializeObject2();
                submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                submitOptions["SortBy"] = $('#SortBy').val();
                CommissionReport.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            var baseCurrency = $("#BaseCurrency").val();

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Report/Commission"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    _gridOption = $.extend(true, {}, sampleGridOption);

                    initProductColumnsForGridOption();

                    for (var i = _commonColumnCount; i < _gridOption.columns.length; i++) {
                        _indexProductColumns.push(i);
                    }

                    renameCurrencyColumns(baseCurrency == SYS_CURRENCY_OPTION);
                    changeHeaderCaptions(baseCurrency == SYS_CURRENCY_OPTION);

                    _gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: CommissionReport.ReRenderAfterPaged
                    };

                    _gridOption.titleInfo.text = MSG.CommissionTitle + ' [{0}]'.format(data.TotalItem);

                    _gridOption.footerRows = getFooterRows(data.SubTotalData, data.TotalRowData, data.TotalItem, data.hasShowSubTotalRow);
                    _gridOption.bodyRows = AFFILIATE.genGridRows(data.List, _gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(_gridOption);

                    hideColumns();

                    if (_isSimpleMode) {
                        $('#btnChangeViewMode').removeClass('icon-circle-arrow-left').addClass('icon-circle-arrow-right');
                    } else {
                        $('#btnChangeViewMode').removeClass('icon-circle-arrow-right').addClass('icon-circle-arrow-left');
                    }

                    if (data.List.length == 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    } else {
                        $('.status').show();
                        AFFILIATE.resetToggleIndicator(_gridOption);
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
    CommissionReport.init();
});
