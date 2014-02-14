var DETAIL = function ($) {

    function deleteMessage(msgList) {
        try {

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("EmailHistory/Delete"),
                type: 'post',
                data: {
                    listID: msgList
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    
                    if (!data.Success) {
                        AFFILIATE.showUpdateStatus(data.ErrorCode, data.Message, parent.ME.ReRenderAfterPaged);
                    } else {
                        AFFILIATE.showUpdateStatus(data.ErrorCode, data.Message, parent.ME.ReRenderAfterPaged,1000);
                    }
                }
            });

        }
        catch (e) {
            alert(e);
        }
    }

    return {
        init: function () {
            $("#btnDelete").click(function () {
                if (!confirm(MSG.ConfirmDeleteEmail))
                    return;
                deleteMessage($("#Mid").val());
            });
            $("#btnCancel").click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
        },
        openMail: function (url) {
            window.open(url, "_blank");
        }
    }
} (jQuery);
$(function () {
    DETAIL.init();
});