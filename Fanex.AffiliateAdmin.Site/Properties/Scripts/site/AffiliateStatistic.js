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
                $("a.qm").text($("#slProduct option:selected").attr("data-qualified"));
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
                    currency: $(this).data("currency")
                };
                
                var title = MSG.AffQualifiedMemReportDetailFor.replace("{0}", $(this).data("affname"));
                title = title.replace("{1}", param.monthdate) + " - " + MSG.Currency + ": " + $(this).data("currency");
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