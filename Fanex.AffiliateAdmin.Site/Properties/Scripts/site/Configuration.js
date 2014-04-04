var Configuration = function ($) {
    var submitOptions = {};

    return {
        init: function () {
            if ($('#gridId').length > 0) {
                Configuration.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

            $('.search-panel form').submit(function () {
                Configuration.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });

            $(".main-content").on("click", ".icon-add-config", function () {
                Configuration.addConfiguration();
            });

            $('.form-popup').submit(function () {
                AFFILIATE.trimBeforeSubmit();
            });
            
            $('#btnDeleteConfiguration').click(function () {
                Configuration.deleteConfiguration();
            });

            $('#btnAddConfiguration').click(function () {
                Configuration.addConfiguration();
            });

            $('#btnCancel').click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = parent.AFFILIATE.resizeHeightDataGrid(gridOption);

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject();
            
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Configuration/GetSystemSettingList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, Configuration.ReRenderAfterPaged);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        addConfiguration: function () {
            var url = "Configuration/AddConfiguration";
            AFFILIATE.openDialogFrame(url, MSG.AddNewConfiguration, { width: 560, height: 480 });
        },

        deleteConfiguration: function (id) {
            if (id === undefined) {
                alert(MSG.InvalidConfiguration);
            } else {
                if (confirm(MSG.DeleteConfigurationMsg)) {
                    var param = {
                        idlist: id
                    };

                    $.ajax({
                        url: AFFILIATE.getAbsoluteUrl("Configuration/DeleteConfiguration"),
                        type: 'post',
                        data: param,
                        success: function (result) {
                            AFFILIATE.PreHandleAjaxResult(result);
                            if (!result.Success) {
                                alert(result.Message);
                            } else {
                                AFFILIATE.showUpdateStatus(result.ErrorCode, result.Message, parent.Configuration.ReRenderAfterPaged);
                            }
                        }
                    });
                }
            }
        },

        showConfigurationDlg: function (sid) {
            var url = "Configuration/ConfigurationDetail";
            if (sid) {
                url += "?Id=" + sid;
            }
            AFFILIATE.openDialogFrame(url, MSG.UpdateConfiguration, { width: 560, height: 480 });
        }
    };
} (jQuery);

$(function () {
    Configuration.init();
});