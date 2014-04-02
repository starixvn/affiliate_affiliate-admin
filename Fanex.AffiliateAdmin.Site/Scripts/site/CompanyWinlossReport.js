var CompanyWinlossReport = function ($) {
    var submitOptions = {};
    var MEMBER_CURRENCY_OPTION = '0';
    var SYS_CURRENCY_OPTION = '1';
    var AFF_CURRENCY_OPTION = '2';
    var GROUP_BY_DATE = "0";
    var GROUP_BY_MONTH = "1";
    var EXRATE_INDEX = 2;
    // number of all common columns (except product columns).
    var _freezeColumnCount = 3;
    var _gridOption = {};
    var _currencyThreshold = 1;

    if ($('#ByAffiliate').length > 0) {
        var _playerWinlossUrl = AFFILIATE.getAbsoluteUrl("Report/PlayerWinlossByAffiliate");
    } else {
        var _playerWinlossUrl = AFFILIATE.getAbsoluteUrl("Report/PlayerWinloss");
    }

    // isUseBaseCurrency: add postfix "Base" to all currency columns
    function initProductColumnsForGridOption() {
        var startTurnoverPosition = turnoverPosition;

        if ($("#GroupBy").val() === GROUP_BY_MONTH) {
            // remove exchange rate column.
            _gridOption.groupHeaderRows[0].Columns.splice(exRatePosition, 1);
            _gridOption.columns.splice(exRatePosition, 1);

            // update the position of turnover column.
            startTurnoverPosition = startTurnoverPosition - 1;
            _gridOption.numberOfFrozenColumn = 2;

            _gridOption.groupHeaderRows[0].Columns[0].HeaderText = MSG.Month;
            _gridOption.columns[0].HeaderText = MSG.Month;
            _gridOption.columns[0].FormatDateTime = AFFILIATE.DATE_FORMAT_MONTH;
        } else {
            switch ($('#BaseCurrency').val()) {
                case MEMBER_CURRENCY_OPTION:
                    _gridOption.groupHeaderRows[0].Columns.splice(EXRATE_INDEX, 1);
                    _gridOption.columns.splice(EXRATE_INDEX, 1);

                    startTurnoverPosition = startTurnoverPosition - 1;
                    _gridOption.numberOfFrozenColumn = 2;

                    break;
                case AFF_CURRENCY_OPTION:
                    _gridOption.groupHeaderRows[0].Columns[exRatePosition].HeaderText = MSG.UnitPerBaseCurrency.format($('#BaseCurrency option:selected').text());
                    _gridOption.groupHeaderRows[0].Columns[exRatePosition].Name = "AffExRate";
                    _gridOption.columns[exRatePosition].Name = "AffExRate";
                    break;
            }
        }

        // generarte turnover/winlost columns based on products
        var productCount = PRODUCTS.length;

        // for turnover cols
        if (productCount > 1) {
            for (var i = 0; i < productCount; i++) {
                var name = nomalizeProductName(PRODUCTS[i].Name);

                if (i == 0) {
                    var turnoverHeader = $.extend(true, {}, sampleTurnoverHeader);
                    turnoverHeader.Name = name + "Turnover";
                    turnoverHeader.NameCache = name + "Turnover";
                    turnoverHeader.MergeColNumber = productCount + 1;
                    _gridOption.groupHeaderRows[0].Columns.splice(startTurnoverPosition, 0, turnoverHeader);
                }

                var turnoverCol = { HeaderCaption: MSG[name], HeaderText: MSG[name], Width: 110, Name: name + "Turnover", NameCache: name + "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true };
                _gridOption.columns.splice(startTurnoverPosition + i, 0, turnoverCol);
            }
        }

        var totalTurnoverCol = $.extend(true, {}, sampleTotalTurnoverCol);

        if (productCount > 1) {
            totalTurnoverCol.HeaderCaption = MSG['Total'];
            totalTurnoverCol.HeaderText = MSG['Total'];
            delete totalTurnoverCol.IsCurrency;
            _gridOption.columns.splice(startTurnoverPosition + productCount, 0, totalTurnoverCol);

        } else {
            _gridOption.columns.splice(startTurnoverPosition + productCount, 0, totalTurnoverCol);
        }

        // for win-loss cols
        if (productCount > 2) {
            for (var i = 0; i < productCount; i++) {
                var name = nomalizeProductName(PRODUCTS[i].Name);

                if (i == 0) {
                    var companyWLHeader = $.extend(true, {}, sampleCompanyWLHeader);
                    companyWLHeader.Name = name + "Winlost";
                    companyWLHeader.NameCache = name + "Winlost";
                    companyWLHeader.MergeColNumber = productCount + 1;
                    _gridOption.groupHeaderRows[0].Columns.push(companyWLHeader);
                }

                var winlostCol = { HeaderCaption: MSG[name], HeaderText: MSG[name], Width: 110, Name: name + "Winlost", NameCache: name + "Winlost", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true };
                _gridOption.columns.push(winlostCol);
            }
        }

        var totalWinlostCol = $.extend(true, {}, sampleTotalWinlostCol);

        if (productCount > 1) {
            totalWinlostCol.HeaderCaption = MSG['Total'];
            totalWinlostCol.HeaderText = MSG['Total'];
            delete totalWinlostCol.IsCurrency;
            _gridOption.columns.splice(_gridOption.columns.length, 0, totalWinlostCol);

        } else {
            _gridOption.columns.splice(_gridOption.columns.length, 0, totalWinlostCol);
        }

        if (productCount == 1) {
            _gridOption.customizeHeader = false;
        }
    }

    function renameCurrencyColumns(baseCurrency) {
        var suffix = '';

        switch (baseCurrency) {
            case SYS_CURRENCY_OPTION:
                suffix = 'Base';
                break;
            case AFF_CURRENCY_OPTION:
                suffix = '_Aff';
                break;
        }

        _freezeColumnCount = _gridOption.numberOfFrozenColumn;
        for (var i = _freezeColumnCount; i < _gridOption.columns.length; i++) {
            var col = _gridOption.columns[i];
            if (col.NameCache) {
                col.Name = col.NameCache + suffix;
            }
        }

        for (var i = _freezeColumnCount; i < _gridOption.groupHeaderRows[0].Columns.length; i++) {
            var col = _gridOption.groupHeaderRows[0].Columns[i];
            if (col.NameCache) {
                col.Name = col.NameCache + suffix;
            }
        }
    }

    function getBodyRows(groups, sampleRequest) {
        if (groups.length == 0)
            return [];

        var bodyRows = [];
        var mergeCols = _gridOption.numberOfFrozenColumn;
        $.each(groups, function (idx, group) {
            var companyWinlossInMemberCurrencies = group.CompanyWinlossInMemberCurrencies;
            var companyWinlossInSystemCurrency = group.CompanyWinlossInSystemCurrency;
            var memberCurrencyCount = companyWinlossInMemberCurrencies.length;
            var groupBy = $("#GroupBy").val();
            var groupByText = $.trim($("#GroupBy option:selected").text());
            var formatDateTime = groupBy == 1 ? AFFILIATE.DATE_FORMAT_MONTH : AFFILIATE.DATE_FORMAT;

            if ((groupBy == 0 && groupByText == group.Level) || (groupBy == 1 && groupByText == group.Level)) {
                $.each(companyWinlossInMemberCurrencies, function (idx, item) {
                    var columns = [];
                    if (idx == 0) {
                        columns.push({
                            Value: group.GroupInfo,
                            Name: "WinlostDate",
                            MergeRowNumber: memberCurrencyCount,
                            CustomCellRender: function (cell, cellFormated) {
                                var obj = moment(cell.Value);
                                // create request object to redirect to player winloss report
                                var request = $.extend(true, {}, sampleRequest);

                                if (groupBy == 0) {
                                    var fromDate = obj.format(AFFILIATE.DATE_FORMAT);
                                    var toDate = obj.format(AFFILIATE.DATE_FORMAT);
                                } else {
                                    var firstDay = obj.date(1);
                                    var fromDate = firstDay.format(AFFILIATE.DATE_FORMAT);

                                    var lastDay = firstDay.add('months', 1).add('days', -1);
                                    var toDate = lastDay.format(AFFILIATE.DATE_FORMAT);
                                }

                                request.FromDate = fromDate;
                                request.ToDate = toDate;

                                // get paras for request object
                                var url = _playerWinlossUrl + "?" + jQuery.param(request);

                                return "<a href={0}>".format(url) + obj.format(formatDateTime) + "</a>";

                            }
                        });
                    }

                    for (var i = 1; i < _gridOption.columns.length; i++) {
                        var col = _gridOption.columns[i];
                        columns.push({ Value: item[col.Name], Name: col.Name });
                    }

                    bodyRows.push({ Columns: columns });
                });

                if (groups.length > 1) {
                    var firstMemberCurrency = companyWinlossInMemberCurrencies[0].Currency;

                    if (hasShowTotalInSystemCurrency(memberCurrencyCount, firstMemberCurrency, System.Currency, $('#BaseCurrency').val())) {
                        var totalColums = [];
                        totalColums.push({ Value: MSG.SubTotalIn.format(System.Currency), Name: "WinlostDate", MergeColNumber: mergeCols });
                        var startIndex = mergeCols;

                        for (var i = startIndex; i < _gridOption.columns.length; i++) {
                            var col = _gridOption.columns[i];
                            var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

                            totalColums.push({ Value: companyWinlossInSystemCurrency[colName], Name: col.Name });
                        }
                        bodyRows.push({ Columns: totalColums, CssClass: "sub-total-group" });
                    }
                }
            } else {
                renderSubTotal(bodyRows, group);
            }
        });
        return bodyRows;
    }

    function hasShowTotalInSystemCurrency(currencyCount, firstCurrency, systemCurrency, selectedBaseCurrency) {
        var minCurrencyCount = 1;

        if (currencyCount > minCurrencyCount) {
            return true;
        }

        if (firstCurrency !== systemCurrency && selectedBaseCurrency !== SYS_CURRENCY_OPTION) {
            return true;
        }

        return false;
    }

    function renderSubTotal(bodyRows, group) {
        var companyWinlossInMemberCurrencies = group.CompanyWinlossInMemberCurrencies;
        var companyWinlossInSystemCurrency = group.CompanyWinlossInSystemCurrency;
        var memberCurrencyCount = companyWinlossInMemberCurrencies.length;
        var mergeCols = _gridOption.numberOfFrozenColumn;
        var groupBy = $("#GroupBy").val();
        var baseCurrency = $('#BaseCurrency').val();

        var suffix = getSuffix(group, groupBy);

        $.each(companyWinlossInMemberCurrencies, function (idx, item) {
            var columns = [];
            var startIndex = 1;
            if (idx == 0) {
                columns.push({ Value: MSG.Total + suffix, Name: "WinlostDate", MergeRowNumber: memberCurrencyCount });

                if (groupBy === GROUP_BY_DATE) {
                    if (baseCurrency == MEMBER_CURRENCY_OPTION) {
                        columns.push({ Value: item["Currency"], Name: "Currency", MergeColNumber: 1 });
                        startIndex += 1;
                    } else {
                        columns.push({ Value: item["Currency"], Name: "Currency", MergeColNumber: 2 });
                        startIndex += 2;
                    }
                } else {
                    columns.push({ Value: item["Currency"], Name: "Currency", MergeColNumber: 1 });
                    startIndex += 1;
                }
            }

            for (var i = startIndex; i < _gridOption.columns.length; i++) {
                var col = _gridOption.columns[i];
                columns.push({ Value: item[col.Name], Name: col.Name });
            }

            bodyRows.push({ Columns: columns, CssClass: 'currency-range' });
        });

        if (memberCurrencyCount > _currencyThreshold || companyWinlossInMemberCurrencies[0].Currency !== System.Currency) {
            var totalColums = [];
            totalColums.push({ Value: MSG.TotalRangeIn.format(suffix, System.Currency), Name: "WinlostDate", MergeColNumber: mergeCols });
            var startIndex = mergeCols;

            for (var i = startIndex; i < _gridOption.columns.length; i++) {
                var col = _gridOption.columns[i];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

                totalColums.push({ Value: companyWinlossInSystemCurrency[colName], Name: col.Name });
            }

            bodyRows.push({ Columns: totalColums, CssClass: 'total-range' });
        }
    }

    function getSuffix(group, groupBy) {
        var suffix = " ";

        if (groupBy === "0") {
            suffix += moment(group.GroupInfo).format(AFFILIATE.DATE_FORMAT_MONTH);
        } else {
            suffix += moment(group.GroupInfo).format(AFFILIATE.DATE_FORMAT_YEAR);
        }

        return suffix;
    }

    function getFooterRows(totalData, hasShowSubTotalRow) {
        var mergeCols = _gridOption.numberOfFrozenColumn;
        var companyWinlossInMemberCurrencies = totalData.CompanyWinlossInMemberCurrencies;
        var memberCurrencyCount = companyWinlossInMemberCurrencies.length;
        var groupBy = $("#GroupBy").val();
        var baseCurrency = $('#BaseCurrency').val();
        var footerRows = [];

        if (hasShowSubTotalRow === true) {
            var realSubTotalclass = memberCurrencyCount > 1 ? 'cell-subtotal real-cell' : '';
            var pseudoSubTotalclass = memberCurrencyCount > 1 ? 'cell-subtotal pseudo-cell' : '';
            subTotalStatus = memberCurrencyCount > 1 ? subTotalStatus : EXPAND_SUBTOTAL_STATUS;

            $.each(companyWinlossInMemberCurrencies, function (idx, item) {
                var columns = [];
                var startIndex = 1;
                if (idx == 0) {
                    columns.push({ Value: MSG.TotalAll, Name: "WinlostDate", MergeRowNumber: memberCurrencyCount, TdClass: realSubTotalclass });
                }

                if (groupBy === GROUP_BY_DATE) {
                    if (baseCurrency == MEMBER_CURRENCY_OPTION) {
                        columns.push({ Value: item["Currency"], Name: "Currency", MergeColNumber: 1 });
                        startIndex += 1;
                    } else {
                        columns.push({ Value: item["Currency"], Name: "Currency", MergeColNumber: 2 });
                        startIndex += 2;
                    }
                } else {
                    columns.push({ Value: item["Currency"], Name: "Currency", MergeColNumber: 1 });
                    startIndex += 1;
                }

                for (var i = startIndex; i < _gridOption.columns.length; i++) {
                    var col = _gridOption.columns[i];
                    columns.push({ Value: item[col.Name], Name: col.Name });
                }

                footerRows.push({ Columns: columns, CssClass: 'currency-whole-range real-subtotal' });
            });

            // pseudo sub total row
            var columns = [];
            var startIndex = 1;
            columns.push({ Value: MSG.TotalAll, Name: "WinlostDate", MergeRowNumber: 1, TdClass: pseudoSubTotalclass });

            if (groupBy === GROUP_BY_DATE) {
                if (baseCurrency == MEMBER_CURRENCY_OPTION) {
                    columns.push({ Value: '', Name: "Currency", MergeColNumber: 1 });
                    startIndex += 1;
                }
                else {
                    columns.push({ Value: '', Name: "Currency", MergeColNumber: 2 });
                    startIndex += 2;
                }

            } else {
                columns.push({ Value: '', Name: "Currency", MergeColNumber: 1 });
            }

            for (var i = startIndex; i < _gridOption.columns.length; i++) {
                var col = _gridOption.columns[i];
                columns.push({ Value: '', Name: col.Name });
            }

            footerRows.push({ Columns: columns, CssClass: "pseudo-subtotal" });
        }

        // Total row in system 
        var firstCurrency = companyWinlossInMemberCurrencies[0].Currency;

        if (hasShowTotalInSystemCurrency(memberCurrencyCount, firstCurrency, System.Currency, baseCurrency)) {
            var totalColums = [];
            var companyWinlossInSystemCurrency = totalData.CompanyWinlossInSystemCurrency;
            totalColums.push({ Value: MSG.TotalAllIn.format(System.Currency), Name: "WinlostDate", MergeColNumber: mergeCols });
            var startIndex = mergeCols;

            for (var i = startIndex; i < _gridOption.columns.length; i++) {
                var col = _gridOption.columns[i];
                var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

                totalColums.push({ Value: companyWinlossInSystemCurrency[colName], Name: col.Name });
            }

            footerRows.push({ Columns: totalColums, CssClass: 'total-whole-range' });
        }

        return footerRows;
    }

    function changeHeaderCaptions(baseCurrency) {
        var text;

        switch (baseCurrency) {
            case SYS_CURRENCY_OPTION:
                text = " (" + System.Prefix + ")";
                break;
            case AFF_CURRENCY_OPTION:
                text = " (" + $('#AffiliateCurrencySymbol').val() + ")";
                break;
            default:
                text = "";
                break;
        }

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

    return {
        init: function () {
            $('#FromDate, #ToDate').addClass("datepicker monthpicker");

            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });

            if ($('#gridId').length > 0) {
                CompanyWinlossReport.submitRequest();
            }

            $('form').submit(function () {
                CompanyWinlossReport.submitRequest();
                return false;
            });

            $('#btnCancel').click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });

            $("#GroupBy").change(function () {
                if (this.value === "0") {
                    if ($(".datepicker").hasClass("mtz-monthpicker-widgetcontainer")) {
                        $(".datepicker").monthpicker('destroy');
                    }

                    var format = "DD MMM YYYY";

                    var minDate = $('#FromDate').attr('data-mindate');
                    var maxDate = $('#FromDate').attr('data-maxdate');

                    var fromDate = $('#FromDate').val();
                    var toDate = $('#ToDate').val();

                    if (fromDate.split(' ').length == 2) {
                        // get the first day of month;
                        fromDate = '01 ' + fromDate;

                        if (AFFILIATE.compareDate(minDate, fromDate) === 1) {
                            fromDate = moment(minDate).format(format);
                        }

                        // get the last day of month
                        var obj = moment('01 ' + toDate);
                        toDate = obj.add('months', 1).add('days', -1).format(format);

                        if (AFFILIATE.compareDate(maxDate, toDate) === -1) {
                            toDate = moment(maxDate).format(format);
                        }
                    }

                    $('#FromDate').val(fromDate);
                    $('#FromDate').attr('data-value', fromDate);
                    $('#ToDate').val(toDate);
                    $('#ToDate').attr('data-value', toDate);

                    AFFILIATE.initDatePicker();
                } else {
                    if ($(".datepicker").hasClass("hasDatepicker")) {
                        $(".datepicker").datepicker('destroy');
                    }

                    AFFILIATE.initMonthPicker();
                }
            });

            $('#gridId').on('click', '.toggle-indicator', function () {
                subTotalStatus = subTotalStatus == COLLAPSE_SUBTOTAL_STATUS ? EXPAND_SUBTOTAL_STATUS : COLLAPSE_SUBTOTAL_STATUS;
                AFFILIATE.resetToggleIndicator(_gridOption);
            });

        },
        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

            var value = $('#GroupBy').val();
            if (value == 1) {
                var fromDate = moment('01 ' + $('#FromDate').val());
                var toDate = moment('01 ' + $('#ToDate').val());
            } else {
                var fromDate = moment($('#FromDate').val());
                var toDate = moment($('#ToDate').val());
            }

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

                var diffMonth = (toMonth + 12) - fromMonth;

                if (diffMonth < 0 || diffMonth > 2) {
                    $('#FromDate, #ToDate').addClass('input-validation-error');
                    return false;
                }

                return true;
            }
        },

        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            if (!CompanyWinlossReport.checkRangeTime()) {
                alert(MSG.RangeTime3Months);
            } else {
                submitOptions = $('form').serializeObject2();
                submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                submitOptions["SortBy"] = $('#SortBy').val();
                CompanyWinlossReport.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            var baseCurrency = $("#BaseCurrency").val();

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Report/CompanyWinloss"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    _gridOption = $.extend(true, {}, sampleGridOption);
                    _commonColumnCount = _gridOption.columns.length;
                    initProductColumnsForGridOption();

                    renameCurrencyColumns(baseCurrency);
                    changeHeaderCaptions(baseCurrency);

                    _gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: CompanyWinlossReport.ReRenderAfterPaged
                    };

                    _gridOption.titleInfo.text = MSG.CompanyWinlossTitle + ' [{0}]'.format(data.TotalItem);

                    if (data.TotalItem > 0) {
                        _gridOption.bodyRows = getBodyRows(data.Groups, submitOptions);
                        _gridOption.footerRows = getFooterRows(data.TotalData, data.HasShowSubTotalRow);
                    }

                    $('#gridId').removeClass("noinfo").fanexGrid(_gridOption);

                    if (data.TotalItem == 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    } else {
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
    CompanyWinlossReport.init();
});
