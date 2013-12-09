var LOG = function ($) {
    return {
        init: function () {
            AFFILIATE.RefreshGridData = LOG.ReRenderAfterPaged;
            $("#btnSubmit").click(function () {
                LOG.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            });
            LOG.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("ViewLog/ListLog"),
                type: 'post',
                data: "LogType=" + $("#LogType").val() + "&DateFrom=" + $("#DateFrom").val() +
                        "&DateTo=" + $("#DateTo").val() + "&PageIndex=" + (currentPage + 1) + "&PageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, LOG.ReRenderAfterPaged);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    }
} (jQuery);

$(function () {
    LOG.init();
});