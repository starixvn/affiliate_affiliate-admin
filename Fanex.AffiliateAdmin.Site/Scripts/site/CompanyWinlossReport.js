/*global AFFILIATE,MSG,moment,System,subTotalStatus,Lang,EXPAND_SUBTOTAL_STATUS,
    moment,sampleNetGamingCol,samplePlaytechCasinoHeader,samplePShareCol,samplePWinCol*/
var CompanyWinlossReport = (function ($) {
    "use strict";
    var submitOptions = {};
    var MEMBER_CURRENCY_OPTION = '0';
    var SYS_CURRENCY_OPTION = '1';
    var AFF_CURRENCY_OPTION = '2';

    var GROUP_BY_DATE = "0";
    var GROUP_BY_MONTH = "1";

    // number of all common columns (except product columns).
    var gridOption = {};

    var EXRATE_COL_INDEX = 2;
    var TURNOVER_COL_INDEX = 8;
    var playerWinlossUrl;

    var levels = ["Date", "Month"];

    var removeExchangeRateCol = function () {
        gridOption.groupHeaderRows[0].Columns.splice(EXRATE_COL_INDEX, 1);
        gridOption.columns.splice(EXRATE_COL_INDEX, 1);
    };

    var changeHeaderOfWinlostDateColumn = function () {
        gridOption.groupHeaderRows[0].Columns[0].HeaderText = window.MSG.Month;
        gridOption.columns[0].HeaderText = window.MSG.Month;
        gridOption.columns[0].FormatDateTime = AFFILIATE.DATE_FORMAT_MONTH;
    };

    var changeHeaderOfExchangeRateColumn = function () {
        gridOption.groupHeaderRows[0].Columns[EXRATE_COL_INDEX].HeaderText = window.MSG.UnitPerBaseCurrency.format($('#BaseCurrency option:selected').text());
        gridOption.groupHeaderRows[0].Columns[EXRATE_COL_INDEX].Name = "AffExRate";
        gridOption.columns[EXRATE_COL_INDEX].Name = "AffExRate";
    };

    var changeGridOptionWhenGroupByMonth = function () {

        removeExchangeRateCol();

        changeHeaderOfWinlostDateColumn();

        gridOption.numberOfFrozenColumn = 2;

        return TURNOVER_COL_INDEX - 1;
    };

    var changeGridOptionWhenGroupByDate = function () {

        switch (submitOptions.BaseCurrency) {
            case MEMBER_CURRENCY_OPTION:
                removeExchangeRateCol();

                gridOption.numberOfFrozenColumn = 2;

                return TURNOVER_COL_INDEX - 1;

            case AFF_CURRENCY_OPTION:
                changeHeaderOfExchangeRateColumn();

                return TURNOVER_COL_INDEX;

            default:
                return TURNOVER_COL_INDEX;
        }
    };

    var addTurnoverColumnsWhenOnlyOneProduct = function (product, startIndex) {
        var name = nomalizeProductName(product.Name);

        var turnoverHeader = $.extend(true, {}, window.sampleTurnoverHeader);
        turnoverHeader.Name = turnoverHeader.NameCache = name + "Turnover";
        gridOption.groupHeaderRows[0].Columns.splice(startIndex, 0, turnoverHeader);

        var turnoverCol = $.extend(true, {}, window.sampleTurnoverCol);
        turnoverCol.Name = turnoverCol.NameCache = name + "Turnover";
        gridOption.columns.splice(startIndex, 0, turnoverCol);

        gridOption.customizeHeader = false;
    };

    var addTurnoverColumnsWhenOverTwoProducts = function (products, startIndex) {
        var productCount = products.length;

        for (var i = 0; i < productCount; i++) {
            var name = nomalizeProductName(products[i].Name);

            if (i === 0) {
                var turnoverHeader = $.extend(true, {}, window.sampleTurnoverHeader);
                turnoverHeader.Name = name + "Turnover";
                turnoverHeader.NameCache = name + "Turnover";
                turnoverHeader.MergeColNumber = productCount + 1;
                gridOption.groupHeaderRows[0].Columns.splice(startIndex, 0, turnoverHeader);
            }

            var turnoverCol = $.extend(true, {}, window.sampleTurnoverCol);
            turnoverCol.HeaderText = turnoverCol.HeaderCaption = window.MSG[name];
            turnoverCol.Name = turnoverCol.NameCache = name + "Turnover";
            delete turnoverCol.IsCurrency;
            gridOption.columns.splice(startIndex + i, 0, turnoverCol);
        }

        // add total turnover column.
        var totalTurnoverCol = $.extend(true, {}, window.sampleTurnoverCol);
        totalTurnoverCol.HeaderCaption = window.MSG['Total'];
        totalTurnoverCol.HeaderText = window.MSG['Total'];
        delete totalTurnoverCol.IsCurrency;
        gridOption.columns.splice(startIndex + productCount, 0, totalTurnoverCol);
    };

    var addTurnoverColumnsForProducts = function (products, startIndex) {
        var productCount = products.length;

        if (productCount === 0) {
            return;
        }

        if (productCount === 1) {
            addTurnoverColumnsWhenOnlyOneProduct(products[0], startIndex);
        } else {
            addTurnoverColumnsWhenOverTwoProducts(products, startIndex);
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

    var addWinlostColumnsWhenOnlyOneProduct = function (product, startHeaderIndex, startColumnIndex) {
        var name = nomalizeProductName(product.Name);

        var companyWLHeader = $.extend(true, {}, window.sampleCompanyWLHeader);
        companyWLHeader.Name = companyWLHeader.NameCache = name + "Winlost";
        gridOption.groupHeaderRows[0].Columns.splice(startHeaderIndex, 0, companyWLHeader);

        var winlostCol = $.extend(true, {}, window.sampleWinlostCol);
        winlostCol.Name = winlostCol.NameCache = name + "Winlost";
        gridOption.columns.splice(startColumnIndex, 0, winlostCol);
    };

    var addWinlostColumnsWhenOverTwoProducts = function (products, startHeaderIndex, startColumnIndex) {
        var productCount = products.length;

        for (var i = 0; i < productCount; i++) {
            var name = nomalizeProductName(products[i].Name);

            if (i === 0) {
                // for header
                var companyWLHeader = $.extend(true, {}, window.sampleCompanyWLHeader);
                companyWLHeader.Name = companyWLHeader.NameCache = name + "Winlost";
                companyWLHeader.MergeColNumber = productCount + 1;
                gridOption.groupHeaderRows[0].Columns.splice(startHeaderIndex, 0, companyWLHeader);
            }

            var winlostCol = $.extend(true, {}, window.sampleWinlostCol);
            winlostCol.HeaderCaption = winlostCol.HeaderText = window.MSG[name];
            winlostCol.Name = winlostCol.NameCache = name + "Winlost";
            delete winlostCol.IsCurrency;
            gridOption.columns.splice(startColumnIndex + i, 0, winlostCol);
        }

        var totalWinlostCol = $.extend(true, {}, window.sampleWinlostCol);
        totalWinlostCol.HeaderCaption = window.MSG['Total'];
        totalWinlostCol.HeaderText = window.MSG['Total'];
        delete totalWinlostCol.IsCurrency;
        gridOption.columns.splice(startColumnIndex + productCount, 0, totalWinlostCol);
    };

    var addWinlostColumnsForProducts = function (products, startHeaderIndex, startColumnIndex) {
        var productCount = products.length;

        if (productCount === 0) {
            return;
        }

        if (productCount === 1) {
            addWinlostColumnsWhenOnlyOneProduct(products[0], startHeaderIndex, startColumnIndex);
        } else {
            addWinlostColumnsWhenOverTwoProducts(products, startHeaderIndex, startColumnIndex);
        }
    };

    var addPlaytechCasinoColumns = function (products) {

        var hasPlaytechCasino = false;

        $.each(products, function () {
            if (this.Name === "Playtech Casino") {
                hasPlaytechCasino = true;
                return;
            }
        });

        if (hasPlaytechCasino === true) {
            if (products.length > 1) {
                gridOption.groupHeaderRows[0].Columns.push(samplePlaytechCasinoHeader);
                gridOption.columns.push(samplePShareCol);
                gridOption.columns.push(samplePWinCol);
                gridOption.columns.push(sampleNetGamingCol);
            } else {
                gridOption.columns.push(samplePShareCol);
                gridOption.columns.push(samplePWinCol);
                gridOption.columns.push(sampleNetGamingCol);
            }
        }
    };

    // isUseBaseCurrency: add postfix "Base" to all currency columns
    var customizeGridOption = function (sampleGridOption) {

        gridOption = $.extend(true, {}, sampleGridOption);
        var byAffiliate = $('#ByAffiliate').length > 0;
        playerWinlossUrl = byAffiliate ? AFFILIATE.getAbsoluteUrl("Report/MemberWinlossByAffiliate") : AFFILIATE.getAbsoluteUrl("Report/MemberWinloss");

        var startTurnoverPosition = TURNOVER_COL_INDEX;

        switch (submitOptions.GroupBy) {
            case GROUP_BY_MONTH:
                startTurnoverPosition = changeGridOptionWhenGroupByMonth();

                break;
            case GROUP_BY_DATE:
                startTurnoverPosition = changeGridOptionWhenGroupByDate(byAffiliate);
                break;
        }

        var products = getSelectProducts(window.PRODUCTS);

        addTurnoverColumnsForProducts(products, startTurnoverPosition);

        addWinlostColumnsForProducts(products, gridOption.groupHeaderRows[0].Columns.length, gridOption.columns.length);

        addPlaytechCasinoColumns(products);
    };

    var getColumnSuffix = function (baseCurrency) {
        var suffix;

        switch (baseCurrency) {
            case SYS_CURRENCY_OPTION:
                suffix = 'Base';
                break;
            case AFF_CURRENCY_OPTION:
                suffix = '_Aff';
                break;
            default:
                suffix = '';
                break;
        }

        return suffix;
    };

    var renameCurrencyColumns = function (baseCurrency) {
        var suffix = getColumnSuffix(baseCurrency);
        var startIndex = gridOption.numberOfFrozenColumn;
        var col;
        var i;

        for (i = startIndex; i < gridOption.columns.length; i++) {
            col = gridOption.columns[i];
            if (col.NameCache) {
                col.Name = col.NameCache + suffix;
            }
        }

        for (i = startIndex; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.NameCache) {
                col.Name = col.NameCache + suffix;
            }
        }
    };

    function getBodyRows(groups, sampleRequest) {
        if (groups.length === 0) {
            return [];
        }

        var bodyRows = [];
        var mergeCols = gridOption.numberOfFrozenColumn;

        var groupBy = $("#GroupBy").val();
        var groupByText = levels[parseInt(groupBy, 10)];
        var baseCurrency = $('#BaseCurrency').val();
        var formatDateTime = groupBy === 1 ? AFFILIATE.DATE_FORMAT_MONTH : AFFILIATE.DATE_FORMAT;

        $.each(groups, function (idx, group) {
            var companyWinlossInMemberCurrencies = group.CompanyWinlossInMemberCurrencies;
            var companyWinlossInSystemCurrency = group.CompanyWinlossInSystemCurrency;
            var memberCurrencyCount = companyWinlossInMemberCurrencies.length;
            var firstMemberCurrency = companyWinlossInMemberCurrencies[0].Currency;

            if ((groupBy === "0" && groupByText === group.Level) || (groupBy === "1" && groupByText === group.Level)) {
                $.each(companyWinlossInMemberCurrencies, function (idx, item) {
                    var columns = [];
                    if (idx === 0) {
                        columns.push({
                            Value: group.GroupInfo,
                            Name: "WinlostDate",
                            MergeRowNumber: memberCurrencyCount,
                            CustomCellRender: function (cell) {
                                var obj = moment(cell.Value);
                                // create request object to redirect to player winloss report
                                var request = $.extend(true, {}, sampleRequest);
                                var fromDate;
                                var toDate;

                                if (groupBy === "0") {
                                    fromDate = obj.format(AFFILIATE.DATE_FORMAT);
                                    toDate = obj.format(AFFILIATE.DATE_FORMAT);
                                } else {
                                    var firstDay = obj.date(1);
                                    fromDate = firstDay.format(AFFILIATE.DATE_FORMAT);
                                    var lastDay = firstDay.add('months', 1).add('days', -1);
                                    toDate = lastDay.format(AFFILIATE.DATE_FORMAT);
                                }

                                request.FromDate = fromDate;
                                request.ToDate = toDate;

                                // get paras for request object.
                                var url = playerWinlossUrl + "?" + jQuery.param(request);

                                return "<a href={0}>".format(url) + obj.format(formatDateTime) + "</a>";

                            }
                        });
                    }

                    for (var i = 1; i < gridOption.columns.length; i++) {
                        var col = gridOption.columns[i];
                        columns.push({ Value: item[col.Name], Name: col.Name });
                    }

                    bodyRows.push({ Columns: columns });
                });

                if (groups.length > 1 && hasShowSubTotalRows(memberCurrencyCount, firstMemberCurrency, baseCurrency)) {
                    var totalColums = [];
                    totalColums.push({ Value: MSG.SubTotalIn.format(System.Currency), Name: "WinlostDate", MergeColNumber: mergeCols });
                    var startIndex = mergeCols;

                    for (var i = startIndex; i < gridOption.columns.length; i++) {
                        var col = gridOption.columns[i];
                        var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

                        totalColums.push({ Value: companyWinlossInSystemCurrency[colName], Name: col.Name });
                    }

                    bodyRows.push({ Columns: totalColums, CssClass: "sub-total-group" });
                }
            } else {
                renderSubTotalInBodyReport(bodyRows, group);
            }
        });
        return bodyRows;
    }

    function hasShowSubTotalRows(memberCurrencyCount, firstMemberCurrency, selectedBaseCurrency) {
        if (memberCurrencyCount > 1) {
            return true;
        }

        if (firstMemberCurrency !== window.System.Currency && selectedBaseCurrency !== SYS_CURRENCY_OPTION) {
            return true;
        }

        return false;
    }

    function renderSubTotalInBodyReport(group) {
        var subTotalRows = [];

        var companyWinlossInMemberCurrencies = group.CompanyWinlossInMemberCurrencies;
        var companyWinlossInSystemCurrency = group.CompanyWinlossInSystemCurrency;
        var memberCurrencyCount = companyWinlossInMemberCurrencies.length;

        var mergeCols = gridOption.numberOfFrozenColumn;
        var groupBy = $("#GroupBy").val();
        var baseCurrency = $('#BaseCurrency').val();
        var firstMemberCurrency = companyWinlossInMemberCurrencies[0].Currency;
        var suffix = getSuffix(group, groupBy);

        if (hasShowSubTotalRows(memberCurrencyCount, firstMemberCurrency, baseCurrency)) {
            $.each(companyWinlossInMemberCurrencies, function (idx, item) {
                var columns = [];
                var startIndex = 1;
                if (idx === 0) {
                    columns.push({ Value: MSG.Total + suffix, Name: "WinlostDate", MergeRowNumber: memberCurrencyCount });

                    if (groupBy === GROUP_BY_DATE) {
                        if (baseCurrency === MEMBER_CURRENCY_OPTION) {
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

                for (var i = startIndex; i < gridOption.columns.length; i++) {
                    var col = gridOption.columns[i];
                    columns.push({ Value: item[col.Name], Name: col.Name });
                }

                subTotalRows.push({ Columns: columns, CssClass: 'currency-range' });
            });

            return subTotalRows;
        }

        var totalColums = [];
        totalColums.push({ Value: MSG.TotalRangeIn.format(suffix, System.Currency), Name: "WinlostDate", MergeColNumber: mergeCols });
        var startIndex = mergeCols;

        for (var i = startIndex; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

            totalColums.push({ Value: companyWinlossInSystemCurrency[colName], Name: col.Name });
        }

        subTotalRows.push({ Columns: totalColums, CssClass: 'total-range' });
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
        var mergeCols = gridOption.numberOfFrozenColumn;
        var companyWinlossInMemberCurrencies = totalData.CompanyWinlossInMemberCurrencies;
        var memberCurrencyCount = companyWinlossInMemberCurrencies.length;
        var groupBy = $("#GroupBy").val();
        var baseCurrency = $('#BaseCurrency').val();
        var firstMemberCurrency = companyWinlossInMemberCurrencies[0].Currency;
        var footerRows = [];
        var col, i;
        var startIndex;
        if (hasShowSubTotalRow === true && hasShowSubTotalRows(memberCurrencyCount, firstMemberCurrency, baseCurrency)) {
            var realSubTotalclass = memberCurrencyCount > 1 ? 'cell-subtotal real-cell' : '';
            var pseudoSubTotalclass = memberCurrencyCount > 1 ? 'cell-subtotal pseudo-cell' : '';
            subTotalStatus = memberCurrencyCount > 1 ? subTotalStatus : EXPAND_SUBTOTAL_STATUS;  // jshint ignore:line

            $.each(companyWinlossInMemberCurrencies, function (idx, item) {
                var columns = [];
                var startIndex = 1;
                if (idx === 0) {
                    columns.push({ Value: MSG.TotalAll, Name: "WinlostDate", MergeRowNumber: memberCurrencyCount, TdClass: realSubTotalclass });
                }

                if (groupBy === GROUP_BY_DATE) {
                    if (baseCurrency === MEMBER_CURRENCY_OPTION) {
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

                for (var i = startIndex; i < gridOption.columns.length; i++) {
                    var col = gridOption.columns[i];
                    columns.push({ Value: item[col.Name], Name: col.Name });
                }

                footerRows.push({ Columns: columns, CssClass: 'currency-whole-range real-subtotal' });
            });

            // pseudo sub total row
            var columns = [];
            startIndex = 1;
            columns.push({ Value: MSG.TotalAll, Name: "WinlostDate", MergeRowNumber: 1, TdClass: pseudoSubTotalclass });

            if (groupBy === GROUP_BY_DATE) {
                if (baseCurrency === MEMBER_CURRENCY_OPTION) {
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

            for (i = startIndex; i < gridOption.columns.length; i++) {
                col = gridOption.columns[i];
                columns.push({ Value: '', Name: col.Name });
            }

            footerRows.push({ Columns: columns, CssClass: "pseudo-subtotal" });
        }

        // Total row in system 
        var totalColums = [];
        var companyWinlossInSystemCurrency = totalData.CompanyWinlossInSystemCurrency;
        totalColums.push({ Value: MSG.TotalAllIn.format(System.Currency), Name: "WinlostDate", MergeColNumber: mergeCols });
        startIndex = mergeCols;
        for (i = startIndex; i < gridOption.columns.length; i++) {
            col = gridOption.columns[i];
            var colName = col.NameCache ? col.NameCache + "Base" : col.Name;

            totalColums.push({ Value: companyWinlossInSystemCurrency[colName], Name: col.Name });
        }

        footerRows.push({ Columns: totalColums, CssClass: 'total-whole-range' });


        return footerRows;
    }

    var getSymbolCurrency = function (baseCurrency) {
        var symbolCurrency;

        switch (baseCurrency) {
            case SYS_CURRENCY_OPTION:
                symbolCurrency = " (" + window.System.Prefix + ")";
                break;
            case AFF_CURRENCY_OPTION:
                symbolCurrency = " (" + $('#AffiliateCurrencySymbol').val() + ")";
                break;
            default:
                symbolCurrency = "";
                break;
        }

        return symbolCurrency;
    };

    var changeHeaderCaptions = function (baseCurrency) {
        var text = getSymbolCurrency(baseCurrency);
        var i, col;

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
    };

    var nomalizeProductName = function (name) {
        return name.replace(/ /g, "");
    };

    return {
        init: function () {
            $('#FromDate, #ToDate').addClass("datepicker monthpicker");

            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });

            if ($('#gridId').length > 0) {
                CompanyWinlossReport.submitRequest();
            }

            $('#btnSubmit').click(function () {
                CompanyWinlossReport.submitRequest();
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

                    if (fromDate.split(' ').length === 2) {
                        // get the first day of month;
                        fromDate = '01 ' + fromDate;

                        if (AFFILIATE.compareDate(minDate, fromDate) === 1) {
                            fromDate = window.moment(minDate).format(format);
                        }

                        // get the last day of month
                        var obj = window.moment('01 ' + toDate);
                        toDate = obj.add('months', 1).add('days', -1).format(format);

                        if (AFFILIATE.compareDate(maxDate, toDate) === -1) {
                            toDate = window.moment(maxDate).format(format);
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
                window.subTotalStatus = window.subTotalStatus === window.COLLAPSE_SUBTOTAL_STATUS
                    ? window.EXPAND_SUBTOTAL_STATUS : window.COLLAPSE_SUBTOTAL_STATUS;
                AFFILIATE.resetToggleIndicator(gridOption);
            });
        },

        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

            var value = $('#GroupBy').val();
            var fromDate;
            var toDate;

            if (value === 1) {
                fromDate = moment('01 ' + $('#FromDate').val());
                toDate = moment('01 ' + $('#ToDate').val());
            } else {
                fromDate = moment($('#FromDate').val());
                toDate = moment($('#ToDate').val());
            }

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

                diffMonth = (toMonth + 12) - fromMonth;

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
            if (!CompanyWinlossReport.checkRangeTime()) {
                alert(window.MSG.RangeTime3Months);
                return;
            }

            if (!CompanyWinlossReport.checkAtLeastOneSelectedProduct()) {
                alert(window.MSG.ChooseAtLeastOneProduct);
                return;
            }

            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
            submitOptions["SortBy"] = $('#SortBy').val();
            CompanyWinlossReport.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Report/CompanyWinloss"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    customizeGridOption(window.sampleGridOption);

                    renameCurrencyColumns(submitOptions.BaseCurrency);

                    changeHeaderCaptions(submitOptions.BaseCurrency);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: CompanyWinlossReport.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.CompanyWinlossTitle + ' [{0}]'.format(data.TotalItem);

                    if (data.TotalItem > 0) {
                        gridOption.bodyRows = getBodyRows(data.Groups, submitOptions);
                        gridOption.footerRows = getFooterRows(data.TotalData, data.HasShowSubTotalRow);
                    }

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem === 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    } else {
                        AFFILIATE.resetToggleIndicator(gridOption);
                    }

                    AFFILIATE.updateSearchPanel($('#gridId'));
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
    CompanyWinlossReport.init();
});
