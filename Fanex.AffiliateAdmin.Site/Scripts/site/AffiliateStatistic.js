var AFFSTATISTIC = function ($) {
    var timeOut;
    return {
        init: function () {

            AFFILIATE.initMonthPicker();

            $('#monthyear').monthpicker().bind('monthpicker-hide', function () {
                AFFSTATISTIC.loadData(moment($(this).monthpicker("getDate")));
            });

            $('#monthyear').blur(function () {
                AFFSTATISTIC.setMonthTimeOut(false);
            });

            $('#monthyear').focus(function () {
                AFFSTATISTIC.setMonthTimeOut(true);
            });

            $('#spProduct').click(function () {
                $("#slProduct").val($('#spProduct').html());
                $("#slProduct").show();
                $(this).hide();
            });

            $('#slProduct').change(function () {
                $("#spProduct").html($("#slProduct").val());
                var product = $("#slProduct").find(":selected").attr('data-product');
                $("a.qm").text($("#slProduct option:selected").attr("data-qualified"));
                $("a.qm").attr("data-product", product);
                $("#spProduct").show();
                $(this).hide();
            });

            $('#slProduct').blur(function () {
                AFFSTATISTIC.setProductTimeOut(false);
            });

            $('#slProduct').focus(function () {
                AFFSTATISTIC.setProductTimeOut(true);
            });

            $('.link').click(function () {
                var href = parent.location.protocol + "//" + parent.location.host + $(this).attr("href");
                parent.window.Mgmt.ifReport.attr('src', href);
                return false;
            });

            $('a.qm').click(function () {
                var param = {
                    affid: $(this).data("affid"),
                    monthdate: $(this).data("date"),
                    basecurrency: $(this).data("basecurrency"),
                    currency: $(this).data("currency"),
                    products: $(this).attr("data-product"),
                    groupid: $(this).data("groupid")
                };
                var title = MSG.AffQualifiedMemReportDetailFor.format($(this).data("affname"), param.monthdate, $(this).data("currency"), $(this).data("commgroup"));
                parent.AFFILIATE.openDialogFrame("Report/QualifiedMembers?" + jQuery.param(param), title, { width: 900, height: 450 });
            });

            $('#spStatistic').click(function () {
                if ($(this).attr("class") == 'statistic-show') {
                    $(this).attr("class", 'statistic-hide');
                    $("#stContent").hide();
                    parent.AFFMASTER.colContent();
                }
                else {
                    $(this).attr("class", 'statistic-show');
                    $("#stContent").show();
                    parent.AFFMASTER.expContent();
                }
            });
        },
        setMonthTimeOut: function (isShow) {
            clearTimeout(AFFSTATISTIC.timeOut);
            status = null;
            if (!isShow) {
                AFFSTATISTIC.timeOut = setTimeout("AFFSTATISTIC.changeMonth(false, 0);", 4000);
            }
            else {
                AFFSTATISTIC.timeOut = setTimeout("AFFSTATISTIC.changeMonth(true, 0);", 4000);
            }
        },
        changeMonth: function (isChange, setValue) {
            if (isChange) {
                $("#monthyear").show();
                $("#spMonth").hide();
                $("#monthyear").focus();
            }
            else {
                $("#monthyear").hide();
                $("#spMonth").show();
            }
            clearTimeout(AFFSTATISTIC.timeOut);
        },
        setProductTimeOut: function (isShow) {
            clearTimeout(AFFSTATISTIC.timeOut);
            status = null;
            if (!isShow) {
                AFFSTATISTIC.timeOut = setTimeout("AFFSTATISTIC.changeProduct(false, 0);", 4000);
            }
            else {
                AFFSTATISTIC.timeOut = setTimeout("AFFSTATISTIC.changeProduct(true, 0);", 4000);
            }
        },
        changeProduct: function (isChange, setValue) {
            if (isChange) {
                $("#slProduct").show();
                $("#spProduct").hide();
                $("#slProduct").focus();
            }
            else {
                $("#slProduct").hide();
                $("#spProduct").show();
            }
            clearTimeout(AFFSTATISTIC.timeOut);
        },
        loadData: function (date) {
            var param = {
                month: date.format("MM"),
                year: date.format("YYYY")
            };
            location.href = AFFILIATE.getAbsoluteUrl("AffiliateInfo/Statistic") + "?" + jQuery.param(param);
        }
    }
} (jQuery);

$(function () {
    AFFSTATISTIC.init();
});