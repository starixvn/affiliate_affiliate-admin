/*global AFFILIATE,samplePShareCol,samplePWinCol,sampleNetGamingCol*/
var AffiliateCommissionReport = (function ($) {

    "use strict";

    var submitOptions = {};

    var byAffiliate = function () {
        return $('#ByAffiliate').length > 0;
    };

    return {
        init: function () {

            $('.search-panel form').submit(function () {
                AffiliateCommissionReport.submitRequest();
                return false;
            });

            AFFILIATE.initMonthPicker();

            if ($('#gridId').length > 0) {
                AffiliateCommissionReport.submitRequest();
            }

            var mainContentObject = $('.main-content');

            mainContentObject.on("click", ".icon-edit", function () {
                var parent = $(this).parents(".amount-region").get(0);
                var $parent = $(parent);
                $parent.find(".amount-info").hide();
                $parent.find(".amount-edit").show();
                $parent.find("input").ForceDecimalOnly();
            });

            mainContentObject.on("click", ".icon-remove", function () {
                var parent = $(this).parents(".amount-region").get(0);
                var $parent = $(parent);
                $parent.find(".amount-info").show();
                $parent.find(".amount-edit").hide();
            });

            /*jshint ignore:start */
            mainContentObject.on("click", "a.qm", function () {
                var param = {
                    affid: $(this).data("affid"),
                    monthdate: $(this).data("date"),
                    basecurrency: $(this).data("basecurrency"),
                    currency: $(this).data("currency"),
                    products: $(this).data("products"),
                    groupid: $(this).data("groupid")
                };

                var title = MSG.AffQualifiedMemReportDetailFor.format($(this).data("affname"), param.monthdate, $(this).data("currency"), $(this).data("commgroup"));
                parent.AFFILIATE.openDialogFrame("Report/QualifiedMembers?" + $.param(param), title, { width: 900, height: 450 });
            });
            /*jshint ignore:end */

            mainContentObject.on("click", "a.commgroup", function () {
                var param = {
                    groupid: $(this).data('groupid'),
                    date: $(this).data('date')
                };
                AFFILIATE.openDialogFrame("CommGroup/ViewSetting?"
                    + $.param(param), window.MSG.CommGroupSettingFor.replace("{0}", '"'
                    + $(this).html() + '" ' + $(this).data("date")), { width: 1000, height: 450 });
            });

            mainContentObject.on("click", ".afflink", function () {
                var params = $('form').serializeObject2();
                params.AffName = $(this).data("affname");
                params["ExcludeTest"] = $('#ExcludeTest').prop('checked');

                delete params.SortBy;
                delete params.TransferStatus;

                window.location = AFFILIATE.getAbsoluteUrl(byAffiliate() ? "Report/AffiliateWinlossByAffiliate?" : "Report/AffiliateWinloss?") + jQuery.param(params);
            });

            mainContentObject.on("click", ".monthlink", function () {
                var params = $('form').serializeObject2();
                params.AffName = $(this).data("affname");
                params["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                params["FromDate"] = $(this).data("winlostdate");
                params["ToDate"] = $(this).data("winlostdate");

                delete params.SortBy;
                delete params.TransferStatus;

                window.location = AFFILIATE.getAbsoluteUrl(byAffiliate() ? "Report/AffiliateWinlossByAffiliate?" : "Report/AffiliateWinloss?") + jQuery.param(params);
            });
        },

        checkAtLeastOneSelectedProduct: function () {
            var selectedProducts = $('#Products').val();

            if (selectedProducts === null || selectedProducts === undefined || selectedProducts.length === 0) {
                return false;
            }

            return true;
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

        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();

            if (!AffiliateCommissionReport.checkRangeTime()) {
                alert(window.MSG.RangeTime3Months);
                return;
            }

            if (!AffiliateCommissionReport.checkAtLeastOneSelectedProduct()) {
                alert(window.MSG.ChooseAtLeastOneProduct);
                return;
            }

            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');

            AffiliateCommissionByProduct.submitRequest(submitOptions);
        }
    };
})(jQuery);

$(function () {
    "use strict";
    AffiliateCommissionReport.init();
});
