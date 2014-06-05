/*global AFFILIATE,gridOption,MSG*/
var Message = (function ($) {
    "use strict";
    var submitOptions = {};

    return {
        init: function () {
            if ($('#gridId').length > 0) {
                Message.submitRequest();
            }

            $(".main-content").on("click", ".icon-add-message", function () {
                Message.addMessage();
            });

            $(".main-content").on("click", ".icon-edit-message", function () {
                Message.editMessage();
            });

            $('.search-panel form').submit(function () {
                Message.submitRequest();
                return false;
            });

            $('.form-popup').submit(function () {
                AFFILIATE.trimBeforeSubmit();
            });

            $('#DateChecked').click(function () {
                var isChecked = $(this).prop('checked');
                $('#FromDate, #ToDate').prop('disabled', !isChecked);
            });
        },
        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject();
            Message.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);  // jshint ignore:line

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Message/DataList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, Message.ReRenderAfterPaged);

                    if (data.TotalItem === 0) {
                        $(".icon-edit-message").hide();
                    }
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        addMessage: function () {
            AFFILIATE.openDialogFrame("Message/AddMessage", MSG.AddMessage, { width: 580, height: 530 });
        },
        deleteMessage: function (id) {
            if (id === undefined) {
                alert(MSG.InvalidMessage);
            } else {
                if (confirm(MSG.DeleteMessageMsg)) {
                    $.ajax({
                        url: AFFILIATE.getAbsoluteUrl("Message/DeleteMessage"),
                        type: 'post',
                        data: { msgids: id },
                        success: function (result) {
                            AFFILIATE.PreHandleAjaxResult(result);
                            
                            if (!result.Success) {
                                alert(result.Message);
                            } else {
                                AFFILIATE.showUpdateStatus(result.ErrorCode, result.Message, parent.Message.ReRenderAfterPaged);
                            }
                        }
                    });
                }
            }
        },
        editMessage: function (msgId) {
            var url = "Message/EditMessage";
            if (msgId) {
                url += "?msgId=" + msgId;
            }
            AFFILIATE.openDialogFrame(url, MSG.EditMessage, { width: 580, height: 600 });
        }
    };
}) (jQuery);

$(function () {
    "use strict";
    Message.init();
});