/*global AFFILIATE,gridOption,MSG,Lang,moment*/
var Report = (function ($) {
    "use strict";
    var submitOptions = {};
    var DATE_COL = 1;
    var GROUP_BY_ALL = "0";
    var GROUP_BY_DATE = "1";
    var GROUP_BY_MONTH = "2";
    var gridOption = {};

    function genTotalRow(data) {

        var footerRows = [];
        footerRows.push({
            Columns: [
                { Value: "Total", Name: "No", MergeColNumber: 6 },
                { Value: data.TotalClick, Name: "TotalClick" },
                { Value: data.UniqueClick, Name: "UniqueClick" },
                { Value: data.Signups, Name: "Signups" },
                { Value: data.FDM, Name: "FDM" },
                { Value: data.NonDeposit, Name: "NonDeposit" },
                { Value: data.ActivePlayers, Name: "ActivePlayers" }
            ],
            CssClass: 'total-whole-range'
        });

        if (submitOptions.GroupBy === GROUP_BY_ALL) {
            footerRows.Columns.splice(DATE_COL, 1);
            footerRows.Columns[0].MergeColNumber = footerRows.Columns[0].MergeColNumber - 1;
        }

        return footerRows;
    }

    function checkRangeTime(fromDate, toDate) {
        var groupBy = $('#GroupBy').val();
        var inputSelector = groupBy === "2" ? "#FromMonth, #ToMonth" : "#FromDate, #ToDate";
        var $input = $(inputSelector);
        $input.removeClass('input-validation-error');

        var fromDateObj = moment(fromDate);
        var toDateObj = moment(toDate);

        var fromYear = fromDateObj.year();
        var toYear = toDateObj.year();
        var diffYear = toYear - fromYear;

        if (diffYear < 0 || diffYear > 1) {
            return false;
        }

        var fromMonth = fromDateObj.month();
        var toMonth = toDateObj.month();
        var diffMonth = 0;

        if (fromDateObj.year() === toDateObj.year()) {

            diffMonth = toMonth - fromMonth;

            if (diffMonth < 0 || diffMonth > 2) {
                $input.addClass('input-validation-error');
                return false;
            }

            return true;
        } else {

            diffMonth = (toMonth + 11) - fromMonth;

            if (diffMonth < 0 || diffMonth > 2) {
                $input.addClass('input-validation-error');
                return false;
            }

            return true;
        }
    }

    function getDateFormat(groupBy) {
        switch (groupBy) {
            case GROUP_BY_DATE:
                return AFFILIATE.DATE_FORMAT;
            case GROUP_BY_MONTH:
                return AFFILIATE.DATE_FORMAT_MONTH;
            default:
                return "";
        }
    }

    var handleDateColumn = function (groupBy) {
        if (groupBy === GROUP_BY_ALL) {
            gridOption.columns.splice(DATE_COL, 1);
            return;
        }

        gridOption.columns[DATE_COL].FormatDateTime = getDateFormat(groupBy);
        gridOption.columns[DATE_COL].HeaderText = groupBy === GROUP_BY_DATE ? MSG.Date : MSG.Month;
    };

    var toggleRangeTime = function (value) {
        var disabled = value === GROUP_BY_ALL;

        if (disabled) {
            $("#FromDate").attr("disabled", "disabled");
            $("#ToDate").attr("disabled", "disabled");
            $("#FromMonth").attr("disabled", "disabled");
            $("#ToMonth").attr("disabled", "disabled");
        } else {
            $("#FromDate").removeAttr("disabled");
            $("#ToDate").removeAttr("disabled");
            $("#FromMonth").removeAttr("disabled");
            $("#ToMonth").removeAttr("disabled");
        }
    };

    return {
        init: function () {

            AFFILIATE.initMonthPicker();

            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });

            $("#GroupBy").change(function () {
                Report.changeGroupBy($(this).val());
            });

            if ($('#gridId').length > 0) {
                Report.submitRequest();
            }

            $('form').submit(function () {
                Report.submitRequest();
                return false;
            });

            $('#DateChecked').click(function () {
                var isChecked = $(this).prop('checked');
                $('#FromDate, #ToDate').prop('disabled', !isChecked);
            });
        },

        changeGroupBy: function (value) {
            var previous = $('#PreviousGroupBy').val();
            toggleRangeTime(value);

            switch (value) {
                case GROUP_BY_DATE:
                    var format = "";
                    var fromDate, toDate;

                    if (previous === GROUP_BY_MONTH) {
                        format = "DD MMM YYYY";

                        var minDate = $('#FromDate').attr('data-mindate');
                        var maxDate = $('#FromDate').attr('data-maxdate');

                        fromDate = $('#FromMonth').val();
                        toDate = $('#ToMonth').val();

                        if (fromDate.split(' ').length === 2) {
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

                        $("#dateSearch").show();
                        $("#monthSearch").hide();
                    }

                    $('#PreviousGroupBy').val("1");
                    break;
                case GROUP_BY_MONTH:
                    if (previous === GROUP_BY_DATE) {
                        format = "MMM YYYY";

                        fromDate = $('#FromDate').val();
                        toDate = $('#ToDate').val();

                        $('#FromMonth').val(moment(fromDate).format(format));
                        $('#FromMonth').attr('data-value', fromDate);
                        $('#ToMonth').val(moment(toDate).format(format));
                        $('#ToMonth').attr('data-value', toDate);
                    }
                    $("#dateSearch").hide();
                    $("#monthSearch").show();
                    $('#PreviousGroupBy').val("2");
                    break;
            }
        },
        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject2();

            if (submitOptions.GroupBy === GROUP_BY_MONTH) { // month
                submitOptions.FromDate = "1 " + submitOptions.FromMonth;
                submitOptions.ToDate = "1 " + submitOptions.ToMonth;
            }

            delete submitOptions.FromMonth;
            delete submitOptions.ToMonth;

            if (submitOptions.GroupBy !== GROUP_BY_ALL && !checkRangeTime(submitOptions.FromDate, submitOptions.ToDate)) {
                alert(window.MSG.RangeTime3Months);
                return;
            }

            Report.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = $.extend(true, {}, window.sampleGridOption);
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);  // jshint ignore:line
            submitOptions.pageIndex = currentPage + 1;
            submitOptions.pageSize = itemsPerPage;

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Report/MemberStatistics"),
                type: 'post',
                data: submitOptions,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    if (data.TotalItem === 0) {
                        $('.legend').hide();
                    } else {
                        $('.legend').show();
                    }

                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.List.length
                    };

                    gridOption.titleInfo.text = window.MSG.MemberStatisticsTitle + ' [{0}]'.format(data.TotalItem);
                    handleDateColumn(submitOptions.GroupBy);

                    gridOption.footerRows = genTotalRow(data.SubTotal);
                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem === 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
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
    Report.init();
});