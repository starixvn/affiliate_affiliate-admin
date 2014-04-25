var CommissionReport = function ($) {
    var submitOptions = {};
    var SYS_CURRENCY_OPTION = 1;
    var DEPOSIT_COL = 6;
    var _commonColumnCount = 0;
    var _freezeColumnCount = 4;
    var _hideColumns = [];
    var _indexProductColumns = [];
    var _isSimpleMode = false;
    var _gridOption = {};

    function initProductColumnsForGridOption() {

        insertCashbackColumns();

        _indexProductColumns.clear();
        _commonColumnCount = _gridOption.columns.length;

        for (var i = 0; i < PRODUCTS.length; i++) {
            var name = nomalizeProductName(PRODUCTS[i].Name);

            _gridOption.groupHeaderRows[0].Columns.push(
                { HeaderText: MSG[name], Name: name + "ActivePlayers", MergeColNumber: 7 }
            );
            _gridOption.columns.push(
                { HeaderText: MSG.ActivePlayers, Width: 90, Name: name + "ActivePlayers", HeaderAlign: "center", CellAlign: "right" }
            );
            _gridOption.columns.push(
                { HeaderText: MSG.BetCount, Width: 60, Name: name + "BetCount", HeaderAlign: "center", CellAlign: "right" }
            );
            _gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Turnover, HeaderText: MSG.Turnover, Width: 70, Name: name + "Turnover", NameCache: name + "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMAT_INT_NUMBER, FormatNegative: '($)', IsAbs: true }
            );
            _gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Winloss, HeaderText: MSG.Winloss, Width: 80, Name: name + "Winloss", NameCache: name + "Winloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );
            _gridOption.columns.push(
                { IsCurrency: false, HeaderCaption: MSG.QM, HeaderText: MSG.QM, Width: 80, Name: name + "QM", HeaderAlign: "center", CellAlign: "right",
                    Formatter: function (rIndex, record, cell, cellFormated) {
                        var actualBaseCurreny = $('#BaseCurrency').val() == "0" ? "2" : "1";
                        var currency = actualBaseCurreny == "1" ? System.Currency : record.Currency;

                        if (cellFormated)
                            return "<a class='qm' data-currency='{0}' data-affname='{1}' data-affid='{2}' data-date='{3}' data-basecurrency='{4}' href='javascript:void(0)'>{5}</a>".format(currency, record.Username, record.AffId, moment(record.WinlostDate).format("MMM YYYY"), actualBaseCurreny, cellFormated);
                        return 0;
                    }
                }
            );

            _gridOption.columns.push(
                { IsCurrency: false, IsPercent: true, HeaderCaption: MSG.CommPercent, HeaderText: MSG.CommPercent, Width: 80, Name: name + "CommissionPercent", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );

            _gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Commission, HeaderText: MSG.Commission, Width: 80, Name: name + "Commission", NameCache: name + "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );
        }
    }

    function insertCashbackColumns() {
        if (HasCashbackDeduction) {
            _gridOption.groupHeaderRows[0].Columns.splice(DEPOSIT_COL, 0, sampleCashbackHeader);
            _gridOption.columns.splice(DEPOSIT_COL, 0, sampleDeductedPercentageCol, sampleAmountCol);
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
        var mergeCols = 2;
        var subTotalRowCount = subTotalData.length;

        var footerRows = [];

        if (hasShowSubTotalRow === true && totalItem > 1) {
            if (subTotalRowCount > 1 || (subTotalRowCount == 1 && subTotalData[0].Currency != System.Currency && $('#BaseCurrency').val() != SYS_CURRENCY_OPTION)) {
                $.each(subTotalData, function (idx, item) {

                    var columns = [];

                    if (idx == 0) {
                        columns.push({ Value: MSG.SubTotal, Name: "WinlostDate", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount });
                    }

                    for (var i = mergeCols; i < _gridOption.columns.length; i++) {
                        var col = _gridOption.columns[i];

                        if (col.IsPercent) {
                            columns.push({ Value: MSG.NA, Name: col.Name });
                        } else {
                            columns.push({ Value: item[col.Name], Name: col.Name });
                        }
                    }

                    footerRows.push({ Columns: columns });
                });
            }
        }

        // Total row
        var totalColums = [];
        totalColums.push({ Value: "<div align='center'>" + MSG.Total + "</div>", Name: "WinlostDate", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount });
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

        $("#gridId").fanexGrid("hideColumns", _gridOption, columns);
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
            });
            // Comm group link event
            $maincontent.on("click", ".commgroup", function () {
                var param = {
                    groupid: $(this).data("groupid")
                };
                parent.AFFILIATE.openDialogFrame("CommGroup/ViewSetting?" + jQuery.param(param), MSG.CommGroupSettingFor.replace("{0}", '"' + $(this).html() + '" ' + $(this).data("date")), { width: 900, height: 450 });
            });
            $maincontent.on("click", "a.afflink", function () {
                var params = $('form').serializeObject2();
                params.AffName = $(this).data("affname");
                params["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                params["SortBy"] = $('#SortBy').val();
                delete params.Currencies;

                window.location = AFFILIATE.getAbsoluteUrl("Report/CompanyWinloss?") + jQuery.param(params);
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

            if ($('#gridId').length > 0) {
                CommissionReport.submitRequest();
            }

            $('form').submit(function () {
                CommissionReport.submitRequest();
                return false;
            });

            $("#GroupBy").change(function () {

            });
        },

        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

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
                url: AFFILIATE.getAbsoluteUrl("Report/CommissionByAffiliate"),
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
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.TotalItem
                    };

                    _gridOption.titleInfo.text = MSG.CommissionTitle + ' [{0}]'.format(data.TotalItem);
                    _gridOption.footerRows = getFooterRows(data.SubTotalData, data.TotalRowData, data.TotalItem, data.hasShowSubTotalData);
                    _gridOption.bodyRows = AFFILIATE.genGridRows(data.List, _gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(_gridOption);

                    if (data.List.length == 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    } else {
                        $('.status').show();
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
