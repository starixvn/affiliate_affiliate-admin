/*global AFFILIATE,gridOption,MSG*/
var TextList = (function ($) {
    "use strict";

    var submitOptions = {};
    return {
        init: function () {

            if ($('#gridId').length > 0) {
                TextList.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

            $(".main-content").on("click", ".icon-add-banner", function () {
                TextList.addText();
            });

            $('.search-panel form').submit(function () {
                TextList.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });

            $('.form-popup').submit(function () {
                AFFILIATE.trimBeforeSubmit();
                // find disabled inputs
                var inputs = $(this).find(':disabled');
                // enable them
                inputs.prop('disabled', false);
            });
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption); // jshint ignore:line

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject();

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("MediaText/DataList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, TextList.ReRenderAfterPaged);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        addText: function () {
            AFFILIATE.openDialogFrame("MediaText/AddText", MSG.AddText, { width: 580, height: 420 });
        },

        editText: function (textId) {
            AFFILIATE.openDialogFrame("MediaText/EditText/" + textId, MSG.EditText, { width: 580, height: 400 });
        },

        deleteText: function (id, totalRequest) {
            if (id === undefined) {
                alert(MSG.InvalidText);
            } else {
                var totalReq = parseInt(totalRequest, 10);

                if (!isNaN(totalReq) && totalReq > 0) {
                    return;
                }

                if (confirm(MSG.DeleteTextMsg)) {
                    var param = { bannerIds: id };

                    $.ajax({
                        url: AFFILIATE.getAbsoluteUrl("MediaText/DeleteText"),
                        type: 'post',
                        data: param,
                        success: function (result) {
                            AFFILIATE.PreHandleAjaxResult(result);

                            if (result.IsSuccess) {
                                AFFILIATE.showUpdateStatus(result.ErrorCode, result.ErrorMessage, TextList.ReRenderAfterPaged);
                            } else {
                                alert(result.ErrorMessage);
                            }
                        }
                    });
                }
            }
        }
    };
})(jQuery);

$(function () {
    "use strict";
    TextList.init();
});