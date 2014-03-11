var MemberWinlossReport = function ($) {
    var submitOptions = {};
    var MEMBER_CURRENCY_OPTION = '0';
    var SYS_CURRENCY_OPTION = '1';
    var AFF_CURRENCY_OPTION = '2';
    var GROUP_BY_MEMBER = '0';
    var GROUP_BY_MEMBER = '1';
    var EXRATE_INDEX = 5;
    // number of all common columns (except product columns).
    var _freezeColumnCount = 4;
    var _gridOption = {};

    // isUseBaseCurrency: add postfix "Base" to all currency columns
    function initProductColumnsForGridOption() {
        var startTurnoverPosition = turnoverPosition;

        if ($("#GroupBy").val() == GROUP_BY_MEMBER) {
            // replace Winlostdate col by Cardinal col
            _gridOption.groupHeaderRows[0].Columns.splice(0, 1, cardinalHeaderCol);
            _gridOption.columns.splice(0, 1, cardinalCol);

            // remove exchange rate column.
            _gridOption.groupHeaderRows[0].Columns.splice(exRatePosition, 1);
            _gridOption.columns.splice(exRatePosition, 1);

            // set value for index column.
            _gridOption.indexNumberColumn = 0;

            // update the position of turnover column.
            startTurnoverPosition = startTurnoverPosition - 1;
        } else {
            switch ($('#BaseCurrency').val()) {
                case MEMBER_CURRENCY_OPTION:
                    _gridOption.enableHiddenColumns = false;
                    _gridOption.hiddenColumns = $('#ByAffiliate').length > 0 ? [EXRATE_INDEX - 2] : [EXRATE_INDEX];
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

        // for winlost cols
        if (productCount > 2) {
            for (var i = 0; i < productCount; i++) {
                var name = nomalizeProductName(PRODUCTS[i].Name);

                if (i == 0) {
                    var companyWLHeader = $.extend(true, {}, sampleCompanyWLHeader);
                    companyWLHeader.Name = name + "Winlost";
                    companyWLHeader.MergeColNumber = productCount + 1;
                    _gridOption.groupHeaderRows[0].Columns.splice(_gridOption.groupHeaderRows[0].Columns.length, 0, companyWLHeader);
                }

                var winlostCol = { HeaderCaption: MSG[name], HeaderText: MSG[name], Width: 110, Name: name + "Winlost", NameCache: name + "Winlost", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true };
                _gridOption.columns.splice(_gridOption.columns.length, 0, winlostCol);
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

        // hide Affiliate, Currency columns if viewing in AffInfo.
        if ($('#ByAffiliate').length > 0) {
            _gridOption.groupHeaderRows[0].Columns.splice(1, 2);
            _gridOption.columns.splice(1, 2);
            _gridOption.numberOfFrozenColumn = _gridOption.numberOfFrozenColumn - 2;
            _freezeColumnCount = _gridOption.numberOfFrozenColumn;
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
    function getFooterRows(subTotalData, totalRowData, hasShowSubTotalData) {
        var mergeCols = _gridOption.numberOfFrozenColumn - 1;
        var subTotalRowCount = subTotalData.length;
        var footerRows = [];

        var startIndex = ($("#GroupBy").val() == GROUP_BY_MEMBER) || ($("#GroupBy").val() != GROUP_BY_MEMBER && $('#BaseCurrency').val() == MEMBER_CURRENCY_OPTION) ? mergeCols + 1 : mergeCols + 2;

        if (hasShowSubTotalData === true) {
            if (subTotalRowCount > 1 || (subTotalRowCount == 1 && subTotalData[0].PlayerCurrency != System.Currency && $('#BaseCurrency').val() != SYS_CURRENCY_OPTION)) {
                var realSubTotalclass = subTotalRowCount > 1 ? 'cell-subtotal real-cell' : '';
                var pseudoSubTotalclass = subTotalRowCount > 1 ? 'cell-subtotal pseudo-cell' : '';
                subTotalStatus = subTotalRowCount > 1 ? subTotalStatus : EXPAND_SUBTOTAL_STATUS;

                $.each(subTotalData, function (idx, item) {

                    var columns = [];

                    if (idx == 0) {
                        columns.push({ Value: MSG.SubTotal, Name: "WinlostDate", MergeColNumber: mergeCols, MergeRowNumber: subTotalRowCount, TdClass: realSubTotalclass });
                    }

                    if ($("#GroupBy").val() == GROUP_BY_MEMBER) {
                        columns.push({ Value: item['PlayerCurrency'], Name: "PlayerCurrency", MergeColNumber: 1 });
                    } else {
                        if ($('#BaseCurrency').val() != MEMBER_CURRENCY_OPTION) {
                            columns.push({ Value: item['PlayerCurrency'], Name: "PlayerCurrency", MergeColNumber: 2 });
                        } else {
                            columns.push({ Value: item['PlayerCurrency'], Name: "PlayerCurrency", MergeColNumber: 1 });
                        }
                    }

                    for (var i = startIndex; i < _gridOption.columns.length; i++) {
                        var col = _gridOption.columns[i];
                        columns.push({ Value: item[col.Name], Name: col.Name });
                    }

                    footerRows.push({ Columns: columns, CssClass: "real-subtotal" });
                });

                // pseudo sub total row.
                var columns = [];
                columns.push({ Value: MSG.SubTotal, Name: "WinlostDate", MergeColNumber: mergeCols, MergeRowNumber: 1, TdClass: pseudoSubTotalclass });
                if ($("#GroupBy").val() == GROUP_BY_MEMBER) {
                    columns.push({ Value: '', Name: "PlayerCurrency", MergeColNumber: 1 });
                } else {
                    if ($('#BaseCurrency').val() != MEMBER_CURRENCY_OPTION) {
                        columns.push({ Value: '', Name: "PlayerCurrency", MergeColNumber: 2 });
                    } else {
                        columns.push({ Value: '', Name: "PlayerCurrency", MergeColNumber: 1 });
                    }
                }

                for (var i = startIndex; i < _gridOption.columns.length; i++) {
                    var col = _gridOption.columns[i];
                    columns.push({ Value: '', Name: col.Name });
                }

                footerRows.push({ Columns: columns, CssClass: "pseudo-subtotal" });
            }
        }

        // Total row
        var totalColums = [];
        totalColums.push({ Value: "<div align='center'>" + MSG.Total + "</div>", Name: "WinlostDate", MergeColNumber: mergeCols });

        if ($("#GroupBy").val() == GROUP_BY_MEMBER) {
            totalColums.push({ Value: System.Currency, Name: "PlayerCurrency", MergeColNumber: 1 });
        } else {
            if ($('#BaseCurrency').val() != MEMBER_CURRENCY_OPTION) {
                totalColums.push({ Value: System.Currency, Name: "PlayerCurrency", MergeColNumber: 2 });
            } else {
                totalColums.push({ Value: System.Currency, Name: "PlayerCurrency", MergeColNumber: 1 });
            }
        }

        for (var i = startIndex; i < _gridOption.columns.length; i++) {
            var col = _gridOption.columns[i];
            var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

            totalColums.push({ Value: totalRowData[colName], Name: col.Name });
        }

        footerRows.push({ Columns: totalColums, CssClass: 'total-whole-range' });

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
            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });

            var $maincontent = $(".main-content");

            if ($('#gridId').length > 0) {
                MemberWinlossReport.submitRequest();
            }

            $('form').submit(function () {
                MemberWinlossReport.submitRequest();
                return false;
            });

            $('#btnCancel').click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });

            $('#gridId').on('click', '.toggle-indicator', function () {
                subTotalStatus = subTotalStatus == COLLAPSE_SUBTOTAL_STATUS ? EXPAND_SUBTOTAL_STATUS : COLLAPSE_SUBTOTAL_STATUS;
                AFFILIATE.resetToggleIndicator(_gridOption);
            });
        },
        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

            var value = $('#GroupBy').val();

            var fromDate = moment($('#FromDate').val());
            var toDate = moment($('#ToDate').val());


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
            if (!MemberWinlossReport.checkRangeTime()) {
                alert(MSG.RangeTime3Months);
            } else {
                submitOptions = $('form').serializeObject2();
                submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                submitOptions["SortBy"] = $('#SortBy').val();
                MemberWinlossReport.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            var baseCurrency = $("#BaseCurrency").val();

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Report/PlayerWinloss"),
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
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.List.length
                    };

                    _gridOption.titleInfo.text = MSG.MemberWinloss + ' [{0}]'.format(data.TotalItem);
                    _gridOption.bodyRows = AFFILIATE.genGridRows(data.List, _gridOption);
                    _gridOption.footerRows = getFooterRows(data.SubTotalData, data.TotalRowData, data.HasShowSubTotalData);

                    $('#gridId').removeClass("noinfo").fanexGrid(_gridOption);

                    if (data.List.length == 0) {
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
    MemberWinlossReport.init();
});
