var PlayerWinlossReport = function ($) {
    var submitOptions = {};

    return {
        init: function () {
            if ($('#gridId').length > 0) {
                PlayerWinlossReport.submitRequest();
            }

            $('form').submit(function () {
                PlayerWinlossReport.submitRequest();
                return false;
            });

            $('#btnCancel').click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
        },

        checkRangeTime: function () {
            $('#FromDate, #ToDate').removeClass('input-validation-error');

            var value = $('#GroupBy').val();
            var fromDate = moment($('#FromDate').val());
            var toDate = moment($('#ToDate').val());

            if (value === '1') {
                fromDate = moment('01 ' + $('#FromDate').val());
                toDate = moment('01 ' + $('#ToDate').val());
            }

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

                var diffMonth = (toMonth + 11) - fromMonth;

                if (diffMonth < 0 || diffMonth > 2) {
                    $('#FromDate, #ToDate').addClass('input-validation-error');
                    return false;
                }

                return true;
            }
        },

        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            if ($('#GroupBy').val() === '0' && !PlayerWinlossReport.checkRangeTime()) {
                alert(MSG.RangeTime3Months);
            } else {
                submitOptions = $('form').serializeObject2();
                submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
                PlayerWinlossReport.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Report/PlayerWinloss"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {

                    var gridObject = $('#gridId');
                    gridObject.show();

                    var gridOption = PlayerWLHelper.customzieGridOption(data);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: PlayerWinlossReport.ReRenderAfterPaged
                    };

                    PlayerWLHelper.genGridRows(data, gridOption);

                    gridObject.removeClass("noinfo").fanexGrid(gridOption);

                    if (data.TotalItem == 0) {
                        gridObject.addClass("noinfo").html(Lang.NoDataInfo);
                    }

                    AFFILIATE.updateSearchPanel(gridObject);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    };
} (jQuery);

$(function () {
    PlayerWinlossReport.init();
});
