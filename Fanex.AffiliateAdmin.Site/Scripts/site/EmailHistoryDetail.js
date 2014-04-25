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
                        AFFILIATE.showUpdateStatus(data.ErrorCode, data.Message, parent.EmailHistory.ReRenderAfterPaged);
                    } else {
                        AFFILIATE.showUpdateStatus(data.ErrorCode, data.Message, parent.EmailHistory.ReRenderAfterPaged, 1000);
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
        },
        openMail: function (url) {
            window.open(url, "_blank");
        }
    }
} (jQuery);
$(function () {
    DETAIL.init();
});