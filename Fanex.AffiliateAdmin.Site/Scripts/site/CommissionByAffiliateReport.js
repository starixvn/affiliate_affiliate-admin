var CommissionReport = function ($) {
    var submitOptions = {};
    var SYS_CURRENCY_OPTION = 1;
    var GROUP_BY_MONTH = 0;
    var DEDUCTION_PERCENT_COL = 9;
    var DEDUCTION_AMOUNT_COL = 10;
    var MONTH_COL = 1, COMMGROUP_COL = 4;
    // number of all common columns (except product columns).
    var _commonColumnCount = 0;
    var _freezeColumnCount = 4;
    var _hideColumns = [];
    var _bonusColumns = [DEDUCTION_PERCENT_COL, DEDUCTION_AMOUNT_COL];
    var _indexProductColumns = [];
    var _isSimpleMode = false;
    // isUseBaseCurrency: add postfix "Base" to all currency columns
    function initProductColumnsForGridOption(gridOption, isUseBaseCurrency) {

        for (var i = 0; i < PRODUCTS.length; i++) {
            var name = nomalizeProductName(PRODUCTS[i].Name);

            gridOption.groupHeaderRows[0].Columns.push(
                { HeaderText: MSG[name], Name: name + "ActivePlayers", MergeColNumber: 7 }
            );
            gridOption.columns.push(
                { HeaderText: MSG.ActivePlayers, Width: 90, Name: name + "ActivePlayers", HeaderAlign: "center", CellAlign: "right" }
            );
            gridOption.columns.push(
                { HeaderText: MSG.BetCount, Width: 60, Name: name + "BetCount", HeaderAlign: "center", CellAlign: "right" }
            );
            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Turnover, HeaderText: MSG.Turnover, Width: 70, Name: name + "Turnover", NameCache: name + "Turnover", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMAT_INT_NUMBER, FormatNegative: '($)', IsAbs: true }
            );
            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Winloss, HeaderText: MSG.Winloss, Width: 80, Name: name + "Winloss", NameCache: name + "Winloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );
            gridOption.columns.push(
                { IsCurrency: false, HeaderCaption: MSG.QM, HeaderText: MSG.QM, Width: 80, Name: name + "QM", HeaderAlign: "center", CellAlign: "right",
                    Formatter: function (rIndex, record, cell, cellFormated) {
                        if (cellFormated)
                            return "<a class='qm' data-currency='" + record.Currency + "' data-affname='" + record.Username + "' data-affid='" + record.AffId + "' data-date='" + moment(record.WinlostDate).format("MMM YYYY") + "' href='javascript:void(0)'>" + cellFormated + "</a>";
                        return 0;
                    }
                }
            );
            gridOption.columns.push(
                { IsCurrency: false, IsPercent: true, HeaderCaption: MSG.CommPercent, HeaderText: MSG.CommPercent, Width: 80, Name: name + "CommissionPercent", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );

            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Commission, HeaderText: MSG.Commission, Width: 80, Name: name + "Commission", NameCache: name + "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );
        }
    }
    function renameCurrencyColumns(gridOption, isUseBaseCurrency) {
        for (var i = _freezeColumnCount; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            if (col.NameCache) {
                var name = col.NameCache;
                if (isUseBaseCurrency)
                    name += "Base";
                col.Name = name;
            }
        }

        for (var i = _freezeColumnCount; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            var col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.NameCache) {
                var name = col.NameCache;
                if (isUseBaseCurrency)
                    name += "Base";
                col.Name = name;
            }
        }
    }

    function getFooterRows(gridOption, subTotalData, totalRowData) {
        var mergeCols = 2;
        var subTotalRowCount = subTotalData.length;

        var footerRows = [];

        if (subTotalRowCount > 1) {
            $.each(subTotalData, function (idx, item) {

                var columns = [];

                if (idx == 0) {
                    columns.push({ Value: MSG.SubTotal, Name: "WinlostDate", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount });
                }

                for (var i = mergeCols; i < gridOption.columns.length; i++) {
                    var col = gridOption.columns[i];

                    if (col.IsPercent) {
                        columns.push({ Value: MSG.NA, Name: col.Name });
                    } else {
                        columns.push({ Value: item[col.Name], Name: col.Name });
                    }
                }

                footerRows.push({ Columns: columns });
            });
        }

        // Total row
        var totalColums = [];
        totalColums.push({ Value: "<div align='center'>" + MSG.Total + "</div>", Name: "WinlostDate", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount });
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
    function changeHeaderCaptions(gridOption, showCurrencySymbol) {
        var text = showCurrencySymbol ? " (" + System.Prefix + ")" : "";
        for (var i = 0; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            if (col.IsCurrency)
                col.HeaderText = col.HeaderCaption + text;
        }

        for (var i = 0; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            var col = gridOption.groupHeaderRows[0].Columns[i];
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

        if (HasCashbackDeduction === false) {
            columns = columns.concat(_bonusColumns.clone());
        }

        if ($("#GroupBy").val() == GROUP_BY_MONTH) {
            $("#gridId").fanexGrid("hideColumns", gridOption, columns);
        }
        else {
            columns = columns.concat(_hideColumns.clone());
            $("#gridId").fanexGrid("hideColumns", gridOption, columns);
            $(".footer-container table th").show();
            $("#gridId .row_footer table tr th").show();
        }

        if (_isSimpleMode && HasCashbackDeduction === false)
            $("#gridId table:lt(4) th div").height(30);
    }
    return {
        init: function () {
            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });
            _commonColumnCount = gridOption.columns.length;
            initProductColumnsForGridOption(gridOption);

            for (var i = _commonColumnCount; i < gridOption.columns.length; i++) {
                _indexProductColumns.push(i);
            }

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
                params.PlayerCurrencies = params.Currencies;
                delete params.Currencies;

                window.location = AFFILIATE.getAbsoluteUrl("Report/CompanyWinloss?") + jQuery.param(params);
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

            renameCurrencyColumns(gridOption, baseCurrency == SYS_CURRENCY_OPTION);
            changeHeaderCaptions(gridOption, baseCurrency == SYS_CURRENCY_OPTION);

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Report/CommissionByAffiliate"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.List.length
                    };

                    gridOption.titleInfo.text = MSG.CommissionTitle + ' [{0}]'.format(data.TotalItem);
                    gridOption.footerRows = getFooterRows(gridOption, data.SubTotalData, data.TotalRowData);
                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

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
