var NOTE = function ($) {
    return {
        init: function () {
            AFFILIATE.RefreshGridData = NOTE.ReRenderAfterPaged;

            $(".main-content").on("click", ".icon-add-note", function () {
                NOTE.addNote();
            });

            $("#btnCancel").click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
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
                    $('#gridId').removeClass('noinfo');

                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.List.length
                    };

                    gridOption.titleInfo.text = MSG.Title + ' [{0}]'.format(data.List.length);

                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);
                    $('#gridId').show().fanexGrid(gridOption);

                    if (data.List.length == 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', MSG.NoInfo);
                    }

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