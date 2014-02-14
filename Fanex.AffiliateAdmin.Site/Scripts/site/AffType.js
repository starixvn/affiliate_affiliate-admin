var AffType = function ($) {
    var submitOptions = {};

    return {
        init: function () {
            if ($('#gridId').length > 0) {
                AffType.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

            $('.search-panel form').submit(function () {
                AffType.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });

            $(".main-content").on("click", ".icon-add-config", function () {
                AffType.addAffType();
            });

            $('.form-popup').submit(function () {
                AFFILIATE.trimBeforeSubmit();
            });

            $('#btnCancel').click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject();

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Configuration/GetAffTypeList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    gridOption.titleInfo.text = MSG.Title + ' [{0}]'.format(data.List.length);
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, AffType.ReRenderAfterPaged);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },

        addAffType: function () {
            var url = "Configuration/AddAffType";
            AFFILIATE.openDialogFrame(url, MSG.AddNewAffType, { width: 580, height: 300 });
        },

        deleteAffType: function (id) {
            if (id === undefined) {
                alert(MSG.InvalidAffType);
            }
            else {
                if (confirm(MSG.DeleteAffTypeMsg)) {
                    $.ajax({ url: AFFILIATE.getAbsoluteUrl("Configuration/DeleteAffType?idlist=" + id),
                        type: 'post',
                        success: function (result) {
                            AFFILIATE.PreHandleAjaxResult(result);
                            if (!result.Success) {
                                alert(result.Message);
                            }
                            else {
                                AFFILIATE.showUpdateStatus(result.ErrorCode, result.Message, parent.AffType.ReRenderAfterPaged);
                            }
                        }
                    });
                }
            }
        },

        showAffTypeDlg: function (id) {
            var url = "Configuration/AffTypeDetail";
            if (id) {
                url += "?Id=" + id;
            }
            AFFILIATE.openDialogFrame(url, MSG.UpdateAffType, { width: 580, height: 300 });
        }


    }
} (jQuery);

$(function () {
    AffType.init();
});