/*global AFFILIATE,MSG,moment,System,Lang,moment,sampleNetGamingCol,samplePlaytechCasinoHeader,samplePShareCol,samplePWinCol*/
var CompanyWinlossReport = (function ($) {
    "use strict";
    var submitOptions = {};

    var GROUP_BY_DATE = 0;

    return {
        init: function () {
            $('#FromDate, #ToDate').addClass("datepicker monthpicker");

            if ($('#GroupBy').val() === GROUP_BY_DATE) {
                AFFILIATE.initDatePicker();
            } else {
                AFFILIATE.initMonthPicker();
            }

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

            $('.main-content').on('click', '.tab-title', function () {
                if ($(this).hasClass('tag-overview')) {
                    $('#ViewMode').val(0);
                } else {
                    $('#ViewMode').val(1);
                }

                CompanyWinlossReport.submitRequest();
            });
        },

        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

            var value = $('#GroupBy').val();
            var fromDate;
            var toDate;

            if (value === "1") {
                fromDate = window.moment('01 ' + $('#FromDate').val());
                toDate = window.moment('01 ' + $('#ToDate').val());
            } else {
                fromDate = window.moment($('#FromDate').val());
                toDate = window.moment($('#ToDate').val());
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

            if (submitOptions.ViewMode === '0') {
                CompanyWinlossByOverview.submitRequest(submitOptions);
            } else {
                CompanyWinlossByProduct.submitRequest(submitOptions);
            }
        }
    };
})(jQuery);

$(function () {
    "use strict";
    CompanyWinlossReport.init();
});
