/*global AFFILIATE,MSG,samplePlaytechCasinoHeader,samplePShareCol,samplePWinCol,sampleNetGamingCol*/
var MemberWinlossReport = (function ($) {
    "use strict";

    var submitOptions = {};

    return {
        init: function () {
            if ($('#gridId').length > 0) {
                MemberWinlossReport.submitRequest();
            }

            $('#btnSubmit').click(function () {
                MemberWinlossReport.submitRequest();
                return false;
            });

            $('#gridId').on('click', '.member-info', function () {
                var member = $(this).data('member');
                AFFILIATE.openDialogFrame("Member?UserName={0}".format(member), MSG.MemberInfo, { width: 350, height: 470 });
            });

            $('.main-content').on('click', '.tab-title', function () {
                if ($(this).hasClass('tag-overview')) {
                    $('#ViewMode').val(0);
                } else {
                    $('#ViewMode').val(1);
                }

                MemberWinlossReport.submitRequest();
            });
        },

        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

            var fromDate = window.moment($('#FromDate').val());
            var toDate = window.moment($('#ToDate').val());

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

            if (!MemberWinlossReport.checkRangeTime()) {
                alert(window.MSG.RangeTime3Months);
                return;
            }

            if (!MemberWinlossReport.checkAtLeastOneSelectedProduct()) {
                alert(MSG.ChooseAtLeastOneProduct);
                return;
            }

            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');

            if (submitOptions.ViewMode === '0') {
                MemberWinLossByOverview.submitRequest(submitOptions);
            } else {
                MemberWinLossByProduct.submitRequest(submitOptions);
            }
        }
    };
})(jQuery);

$(function () {
    "use strict";
    MemberWinlossReport.init();
});