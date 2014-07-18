/*global AFFILIATE,gridOption,MSG,Lang,moment*/
var Report = (function ($) {
    "use strict";
    var submitOptions = {};
    var DATE_COL = 1;
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
            case "1":
                return AFFILIATE.DATE_FORMAT;
            case "2":
                return AFFILIATE.DATE_FORMAT_MONTH;
            default:
                return "";
        }
    }

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
            var disable = value === "0";
            $("#FromDate").prop("disabled", disable);
            $("#ToDate").prop("disabled", disable);
            $("#FromMonth").prop("disabled", disable);
            $("#ToMonth").prop("disabled", disable);
            var previous = $('#PreviousGroupBy').val();

            switch (value) {
                case "1":
                    var format = "";
                    var fromDate, toDate;

                    if (previous === "2") {
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
                case "2":
                    if (previous === "1") {
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
            submitOptions = $('form').serializeObject();
            Report.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = $.extend(true, {}, window.sampleGridOption);
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);  // jshint ignore:line
            var postdata = $('.search-form').find('input, select').serializeData();
            postdata.pageIndex = currentPage + 1;
            postdata.pageSize = itemsPerPage;

            if (postdata.GroupBy === "2") { // month
                postdata.FromDate = "1 " + postdata.FromMonth;
                postdata.ToDate = "1 " + postdata.ToMonth;
            }
            postdata.FromMonth = undefined;
            postdata.ToMonth = undefined;

            if (postdata.GroupBy !== "0" && !checkRangeTime(postdata.FromDate, postdata.ToDate)) {
                alert(window.MSG.RangeTime3Months);
                return;
            }

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Report/MemberStatistics"),
                type: 'post',
                data: postdata,
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
                    gridOption.columns[DATE_COL].FormatDateTime = getDateFormat(postdata.GroupBy);

                    if (postdata.GroupBy === "0") {
                        gridOption.columns.splice(DATE_COL, 1);
                    }

                    gridOption.footerRows = genTotalRow(data.SubTotal);
                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.List.length === 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                        return;
                    } else {
                        $('.status').show();
                    }

                    if (postdata.GroupBy === 0) // All
                    {
                        $("#gridId").fanexGrid("hideColumns", gridOption, [DATE_COL]);
                        // show hidden column in total row
                        $(".footer-container table th").show();
                    }
                    else {
                        $("#gridId").fanexGrid("hideColumns", gridOption, []);
                        $(".header-container table tr th:eq(1) div").html(postdata.GroupBy === 1 ? "Date" : "Month");

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