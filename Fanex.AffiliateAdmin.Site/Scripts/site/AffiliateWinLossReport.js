/*global AFFILIATE,MSG,moment,System,COLLAPSE_SUBTOTAL_STATUS,EXPAND_SUBTOTAL_STATUS,subTotalStatus,Lang*/
var AffiliateWinLossReport = (function ($) {
    "use strict";
    var submitOptions = {};

    var gridOption = {};

    var byAffiliate = function () {
        return $('#ByAffiliate').length > 0;
    };

    return {
        init: function () {
         
            AFFILIATE.initMonthPicker();

            var $maincontent = $(".main-content");

            // Comm group link event
            $maincontent.on("click", ".commgroup", function () {
                var param = {
                    groupid: $(this).data("groupid"),
                    date: $(this).data("date")
                };
                parent.AFFILIATE.openDialogFrame("CommGroup/ViewSetting?"
                    + jQuery.param(param), MSG.CommGroupSettingFor.replace("{0}", '"'
                    + $(this).html() + '" ' + $(this).data("date")), { width: 900, height: 450 });
            });

            $maincontent.on("click", "a.afflink", function () {
                var params = $('form').serializeObject2();
                params.AffName = $(this).data("affname");
                params["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                params["SortBy"] = $('#SortBy').val();
                params["GroupBy"] = 1;

                delete params.Currencies;

                params["FromDate"] = '01 ' + params["FromDate"];
                var toDateObj = window.moment('01 ' + params["ToDate"]);
                var lastDateObj = toDateObj.add("months", 1).add("days", -1);
                params["ToDate"] = lastDateObj.format("DD MMM YYYY");

                var url = AFFILIATE.getAbsoluteUrl(byAffiliate() ? "Report/MemberWinlossByAffiliate?" : "Report/MemberWinloss?") + jQuery.param(params) + "&" + QueryString.getQueryString(window.location.href);
                window.open(url, "_blank");
            });

            $maincontent.on("click", "a.monthlink", function () {
                var params = $('form').serializeObject2();
                params.AffName = $(this).data("affname");
                params["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                params["SortBy"] = $('#SortBy').val();
                params["GroupBy"] = 0;
                delete params.Currencies;

                var winlostDateObj = window.moment($(this).attr('data-winlostdate'));
                var firstDayObj = winlostDateObj.date(1);
                var firstDay = firstDayObj.format("DD MMM YYYY");
                var lastDay = firstDayObj.add("months", 1).add("days", -1).format("DD MMM YYYY");

                params["FromDate"] = firstDay;
                params["ToDate"] = lastDay;
                var url = AFFILIATE.getAbsoluteUrl(byAffiliate() ? "Report/MemberWinlossByAffiliate?" : "Report/MemberWinloss?") + jQuery.param(params) + "&" + QueryString.getQueryString(window.location.href);
                window.open(url, "_blank");
            });

            $maincontent.on("click", "a.qm", function () {
                var param = {
                    affid: $(this).data("affid"),
                    monthdate: $(this).data("date"),
                    basecurrency: $(this).data("basecurrency"),
                    currency: $(this).data("currency"),
                    products: $(this).data("products"),
                    groupid: $(this).data("groupid")
                };

                var title = MSG.AffQualifiedMemReportDetailFor
                    .format($(this).data("affname"), param.monthdate,
                            $(this).data("currency"), $(this).data("commgroup"));
                parent.AFFILIATE.openDialogFrame("Report/QualifiedMembers?" + jQuery.param(param), title, { width: 900, height: 450 });
            });

            if ($('#gridId').length > 0) {
                AffiliateWinLossReport.submitRequest();
            }

            $('#btnSubmit').click(function () {
                AffiliateWinLossReport.submitRequest();
                return false;
            });

            $('.main-content').on('click', '.tab-title', function () {
                if ($(this).hasClass('tag-overview')) {
                    $('#ViewMode').val(0);
                } else {
                    $('#ViewMode').val(1);
                }

                AffiliateWinLossReport.submitRequest();
            });
        },

        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

            var fromDate = moment('01 ' + $('#FromDate').val());
            var toDate = moment('01 ' + $('#ToDate').val());

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
            if (!AffiliateWinLossReport.checkRangeTime()) {
                alert(window.MSG.RangeTime3Months);
                return;
            }

            if (!AffiliateWinLossReport.checkAtLeastOneSelectedProduct()) {
                alert(window.MSG.ChooseAtLeastOneProduct);
                return;
            }

            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
            submitOptions["SortBy"] = $('#SortBy').val();

            if (submitOptions.ViewMode === '0') {
                AffiliateWinLossByOverview.submitRequest(submitOptions);
            } else {
                AffiliateWinLossByProduct.submitRequest(submitOptions);
            }
        }
    };
})(jQuery);

$(function () {
    "use strict";
    AffiliateWinLossReport.init();
});
