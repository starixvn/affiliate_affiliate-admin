var CommissionReport = function ($) {
    var submitOptions = {};
    var SYS_CURRENCY_OPTION = 1;
    var GROUP_BY_MONTH = 0;
    var MONTH_COL = 1, COMMGROUP_COL = 4;
    // number of all common columns (except product columns).
    var _commonColumnCount = 0;
    var _freezeColumnCount = 4;
    var _hideColumns = [MONTH_COL, COMMGROUP_COL];
    var _indexProductColumns = [];
    var _isSimpleMode = false;
    var _gridOption = {};
    
    // isUseBaseCurrency: add postfix "Base" to all currency columns
    function initProductColumnsForGridOption() {
        var hasGroupByMonth = $("#GroupBy").val() == GROUP_BY_MONTH;
        var mergeColNumber = hasGroupByMonth ? 7 : 5;

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
                { IsCurrency: true, HeaderCaption: MSG.Turnover, HeaderText: MSG.Turnover, Width: 70, Name: name + "Turnover", NameCache: name + "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)' }
            );
            _gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Winloss, HeaderText: MSG.Winloss, Width: 80, Name: name + "Winloss", NameCache: name + "Winloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)' }
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
                            if (cellFormated)
                                return "<a class='qm' data-currency='" + record.Currency + "' data-affname='" + record.Username + "' data-affid='" + record.AffId + "' data-date='" + moment(record.WinlostDate).format("MMM YYYY") + "' href='javascript:void(0)'>" + cellFormated + "</a>";
                            return 0;
                        }
                    }
                );
                // add CommPercent column to hidden list
                _gridOption.columns.push(
                    { IsCurrency: false, HeaderCaption: MSG.CommPercent, HeaderText: MSG.CommPercent, Width: 80, Name: name + "CommissionPercent", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)' }
                );
            }

            _gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Commission, HeaderText: MSG.Commission, Width: 80, Name: name + "Commission", NameCache: name + "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)' }
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

    function getFooterRows(subTotalData, totalRowData) {
        var mergeCols = 5;
        var subTotalRowCount = subTotalData.length;

        var footerRows = [];

        if (subTotalRowCount > 1) {
            $.each(subTotalData, function (idx, item) {

                var columns = [];

                if (idx == 0) {
                    columns.push({ Value: MSG.SubTotal, Name: "Username", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount });
                }

                for (var i = mergeCols; i < _gridOption.columns.length; i++) {
                    var col = _gridOption.columns[i];
                    columns.push({ Value: item[col.Name], Name: col.Name });
                }

                footerRows.push({ Columns: columns });
            });
        }

        // Total row
        var totalColums = [];
        totalColums.push({ Value: "<div align='center'>" + MSG.Total + "</div>", Name: "Username", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount });

        for (var i = mergeCols; i < _gridOption.columns.length; i++) {
            var col = _gridOption.columns[i];
            var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

            totalColums.push({ Value: totalRowData[colName], Name: col.Name });
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
        if ($("#GroupBy").val() == GROUP_BY_MONTH) {
            $("#gridId").fanexGrid("hideColumns", _gridOption, columns);
        }
        else {
            columns = columns.concat(_hideColumns.clone());
            $("#gridId").fanexGrid("hideColumns", _gridOption, columns);
            $(".footer-container table th").show();
            $("#gridId .row_footer table tr th").show();
        }

        if (_isSimpleMode)
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
                params["GroupBy"] = 1;
                params.PlayerCurrencies = params.Currencies;
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
                    monthdate: $(this).data("date")
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

            $('#btnCancel').click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });

            $("#GroupBy").change(function () {

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
                    _gridOption = $.extend(true, {}, sampleGridOption);
                    _indexProductColumns.clear();
                    _commonColumnCount = _gridOption.columns.length;
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
                        totalItem: data.List.length
                    };

                    _gridOption.titleInfo.text = MSG.CommissionTitle + ' [{0}]'.format(data.TotalItem);

                    _gridOption.footerRows = getFooterRows(data.SubTotalData, data.TotalRowData);
                    _gridOption.bodyRows = AFFILIATE.genGridRows(data.List, _gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(_gridOption);

                    hideColumns();

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
