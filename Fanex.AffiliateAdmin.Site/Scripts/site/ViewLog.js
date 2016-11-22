/*global AFFILIATE,gridOption*/
var LOG = (function ($) {
    "use strict";
    return {
        init: function() {
            $("#btnSubmit").click(function() {
                LOG.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            });
            LOG.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        ReRenderAfterPaged: function(currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);  // jshint ignore:line

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("ViewLog/ListLog"),
                type: 'post',
                data: "LogType=" + $("#LogType").val() + "&DateFrom=" + $("#DateFrom").val() +
                    "&DateTo=" + $("#DateTo").val() + "&PageIndex=" + (currentPage + 1) + "&PageSize=" + itemsPerPage + "&affid=" + QueryString.getCurrentParam("AffId"),
                beforeSend: function() {
                    $.blockUI();
                },
                success: function(data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, LOG.ReRenderAfterPaged);
                },
                complete: function() {
                    $.unblockUI();
                }
            });
        }
    };
})(jQuery);

$(function () {
    "use strict";
    LOG.init();
});