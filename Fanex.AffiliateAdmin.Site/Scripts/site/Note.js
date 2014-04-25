var NOTE = function ($) {
    return {
        init: function () {
            $(".main-content").on("click", ".icon-add-note", function () {
                NOTE.addNote();
            });

            NOTE.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            if (gridOption && gridOption.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage : gridOption.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOption.pagerOption.itemsPerPage
            }

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Note/ListNote"),
                type: 'post',
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, NOTE.ReRenderAfterPaged);

                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        addNote: function () {
            AFFILIATE.openDialogFrame("Note/AddNote", MSG.AddNote, { width: 600, height: 220 });
            return false;
        },
        editNote: function (noteId) {
            AFFILIATE.openDialogFrame("Note/EditNote?noteId=" + noteId, "Edit Note", { width: 600, height: 220 });
            return false;
        }
    }
} (jQuery);

$(function () {
    NOTE.init();
});