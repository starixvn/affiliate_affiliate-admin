/*global AFFILIATE,gridOption,MSG*/
var EmailHistory = (function ($) {
    "use strict";
    return {
        init: function () {

            $("#SearchOption").change(function () {

                if (this.value >= 0) {
                    $("#SearchText").show();
                } else {
                    $("#SearchText").hide();
                }

                $("#SearchText").val("").focus();
            });

            $("#IsAll").change(function () {

                $("#FromDate, #ToDate").attr("disabled", this.checked);
            });

            $("#gridId").on("click", "table a.subject", function () {
                var param = {
                    id: $(this).attr("data-mid")
                };

                var queryString = jQuery.param(param);
                var url = "EmailHistory/Detail?" + queryString;

                AFFILIATE.openDialogFrame(url, "Email Detail", { width: 500, height: 400 });

            });

            EmailHistory.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);

            $('#btnSubmit').click(function () {
                EmailHistory.submitRequest();
                return false;
            });
        },
        deleteEmail: function (id) {
            try {
                if (!id || id < 1) {
                    return;
                }

                if (!confirm(MSG.ConfirmDeleteEmail)) {
                    return;
                }
                
                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("EmailHistory/Delete"),
                    type: 'post',
                    data: {
                        listID: id
                    },
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);

                        if (!data.Success) {
                            alert(data.Message);
                        } else {
                            EmailHistory.ReRenderAfterPaged();
                        }
                    }
                });

            } catch (e) {
                alert(e);
            }
        },
        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            EmailHistory.ReRenderAfterPaged();
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);  // jshint ignore:line

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("EmailHistory/EmailHistory"),
                type: 'post',
                data: $('#SearchForm').find('select,input').serialize() + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, EmailHistory.ReRenderAfterPaged);
                },
                error: function () {
                    alert("Occur the error when requesting email history.");
                }
            });
        }
    };
})(jQuery);
$(function () {
    "use strict";
    EmailHistory.init();
});