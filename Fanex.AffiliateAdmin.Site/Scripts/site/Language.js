var Language = function ($) {
    var submitOptions = {};

    return {
        init: function () {
            if ($('#gridId').length > 0) {
                Language.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }
            
            $('.search-panel form').submit(function () {
                Language.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });

            $(".main-content").on("click", ".icon-add-config", function () {
                Language.addLanguage();
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

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Configuration/GetLanguageList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    gridOption.titleInfo.text = MSG.Title + ' [{0}]'.format(data.List.length);
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, Language.ReRenderAfterPaged);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },

        addLanguage: function () {
            var url = "Configuration/AddLanguage";
            AFFILIATE.openDialogFrame(url, MSG.AddNewLanguage, { width: 580, height: 320 });
        },

        deleteLanguage: function (id) {
            if (id === undefined) {
                alert(MSG.InvalidLanguage);
            }
            else {
                if (confirm(MSG.DeleteLanguageMsg)) {
                    $.ajax({ url: AFFILIATE.getAbsoluteUrl("Configuration/DeleteLanguage?idlist=" + id),
                        type: 'post',
                        success: function (result) {
                            if (!result.Success) {
                                alert(result.Message);
                            }
                            else {
                                AFFILIATE.showUpdateStatus(result.ErrorCode, result.Message, parent.Language.ReRenderAfterPaged);
                            }
                        }
                    });
                }
            }
        },

        showLanguageDlg: function (id) {
            var url = "Configuration/LanguageDetail";
            if (id) {
                url += "?Id=" + id;
            }
            AFFILIATE.openDialogFrame(url, MSG.UpdateLanguage, { width: 580, height: 320 });
        }
    }
} (jQuery);

$(function () {
    Language.init();
});