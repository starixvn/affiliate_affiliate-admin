var TextList = function ($) {
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
            });

            $("#btnCancel").click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject();

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Media/TextDataList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, TextList.ReRenderAfterPaged);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        addText: function () {
            AFFILIATE.openDialogFrame("Media/AddText", MSG.AddText, { width: 580, height: 420 });
        },

        editText: function (textId) {
            AFFILIATE.openDialogFrame("Media/EditText/" + textId, MSG.EditText, { width: 580, height: 400 });
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
                        url: AFFILIATE.getAbsoluteUrl("Media/DeleteText"),
                        type: 'post',
                        data: param,
                        success: function (result) {
                            if (!result.Success) {
                                alert(result.Message);
                            } else {
                                AFFILIATE.showUpdateStatus(result.ErrorCode, result.Message, parent.TextList.ReRenderAfterPaged);
                            }
                        }
                    });
                }
            }
        }
    };
} (jQuery);

$(function () {
    TextList.init();
});