var ONLINELIST = function ($) {
    return {
        init: function () {
            ONLINELIST.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Home/UserOnlineList"),
                type: 'post',
                data: {
                    pageIndex: (currentPage + 1),
                    pageSize: itemsPerPage
                },
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, ONLINELIST.ReRenderAfterPaged);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    }
} (jQuery);
$(function () {
    ONLINELIST.init();
});