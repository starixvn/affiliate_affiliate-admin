var Country = function ($) {
    var submitOptions = {};

    return {
        init: function () {
            if ($('#gridId').length > 0) {
                Country.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

            $('.search-panel form').submit(function () {
                Country.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });

            $(".main-content").on("click", ".icon-add-config", function () {
                Country.addCountry();
            });

            $('.form-popup').submit(function () {
                AFFILIATE.trimBeforeSubmit();
            });

            $('#btnCancel').click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });

            if ($('#DisplayOrder').length > 0) {
                $("#DisplayOrder").keyup(function () {
                }).ForceIntegerOnly();
            }
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject();
            
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Configuration/GetCountryList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, Country.ReRenderAfterPaged);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },

        selectCountry: function (self) {
            if ($(self).prop('checked')) {
                $('#gridId input[type=checkbox]').prop('checked', true);
            } else {
                $('#gridId input[type=checkbox]').prop('checked', false);
            }
        },

        addCountry: function () {
            var url = "Configuration/AddCountry";
            AFFILIATE.openDialogFrame(url, MSG.AddNewCountry, { width: 580, height: 450 });
        },

        deleteCountry: function (id) {
            if (id === undefined) {
                alert(MSG.InvalidCountry);
            } else {
                if (confirm(MSG.DeleteCountryMsg)) {
                    $.ajax({
                        url: AFFILIATE.getAbsoluteUrl("Configuration/DeleteCountry?idlist=" + id),
                        type: 'post',
                        success: function (result) {
                            AFFILIATE.PreHandleAjaxResult(result);
                            if (!result.Success) {
                                alert(result.Message);
                            } else {
                                AFFILIATE.showUpdateStatus(result.ErrorCode, result.Message, parent.Country.ReRenderAfterPaged);
                            }
                        }
                    });
                }
            }
        },

        showCountryDlg: function (id) {
            var url = "Configuration/CountryDetail";
            if (id) {
                url += "?Id=" + id;
            }
            AFFILIATE.openDialogFrame(url, MSG.UpdateCountry, { width: 580, height: 450 });
        }
    };
} (jQuery);

$(function () {
    Country.init();
});