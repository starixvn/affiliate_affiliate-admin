/*global AFFILIATE,gridOption,MSG*/
var BannerList = (function ($) {
    "use strict";

    var submitOptions = {};

    return {
        init: function () {
            if ($('#gridId').length > 0) {
                BannerList.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

            $(".main-content").on("click", ".icon-add-banner", function () {
                BannerList.addBanner();
            });

            $('.search-panel form').submit(function () {
                BannerList.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });

            $('.form-popup').submit(function () {
                if (!BannerList.validateFileSize()) {
                    alert(window.MSG.MaxFileSizeMsg);
                    return false;
                } else {
                    AFFILIATE.trimBeforeSubmit();
                }
            });

            $('#Category').change(function () {
                if ($('#Category').val()) {
                    var isShowAll = false;
                    if ($('#gridId').length > 0) {
                        isShowAll = true;
                    }
                    BannerList.getSubCategories($('#Category').val(), isShowAll);
                } else {
                    $('#divSubCategories').hide().html();
                }
            });

            $('#File').change(function () {
                if (!BannerList.validateFileSize()) {
                    alert(window.MSG.MaxFileSizeMsg);
                } else {
                    BannerList.validateFileExtension(this);
                }
            });

            $('#ActionBannerForm').submit(function () {
                // find disabled inputs
                var inputs = $(this).find(':disabled');
                // enable them
                inputs.prop('disabled', false);
            });
        },
        validateFileSize: function () {
            var maxFileSize = parseInt(window.MSG.MaxFileSize, 10);
            var size = BannerList.findFileSize();
            if (size > maxFileSize) {
                return false;
            }

            return true;
        },
        findFileSize: function () {
            var size = 0;
            if ($.browser.msie) {
                return size;
            } else {
                var fileInput = $("#File")[0];
                size = fileInput.files !== undefined ? fileInput.files[0].size : size;
                return size;
            }
        },
        getSubCategories: function (category, isShowAll) {
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Media/SubCategories"),
                type: 'post',
                data: { category: category, isShowAll: isShowAll },
                beforeSend: function () {
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    if (data.length > 0) {
                        $('#divSubCategories').html(data).show();
                        $(".dropdownchecklist").createDropdownchecklist();
                    } else {
                        $('#divSubCategories').hide().html('');
                    }
                },
                complete: function () {
                }
            });
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption); // jshint ignore:line

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject();

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Media/BannerDataList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, BannerList.ReRenderAfterPaged);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        viewImage: function (imgElement) {
            var imgUrl = $(imgElement).attr('alt');
            AFFILIATE.openDialogUrl(imgUrl, window.MSG.OriginalView, { width: 800, height: 600 });
        },

        /*jshint ignore:start */
        createFlashHtml: function (url, width, height) {
            var htmlContent = "<object width='{1}' height='{2}' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0'>"
                + "<param name='movie' value='{0}'>"
                + "<param name='quality' value='high'>"
                + "<param value='transparent' name='wmode'>"
                + "<embed width='{1}' height='{2}' src='{0}' quality='high' wmode='transparent' pluginspage='http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash' type='application/x-shockwave-flash'>"
                + "</object>";

            return htmlContent.format(url, width, height);
        },
        /*jshint ignore:end */

        viewFlash: function (element, width, height) {
            var url = $(element).attr('alt');
            var htmlContent = BannerList.createFlashHtml(url, width, height);
            AFFILIATE.openDialogContent(htmlContent, "Orginal View", { width: 800, height: 600 });
        },

        validateFileExtension: function (fileElement) {
            var fullFilename = $(fileElement).val();
            var temp = fullFilename.split("\\");
            var fileName = temp[temp.length - 1];

            if (fileName !== "") {
                var ext = fileName.substring(fileName.lastIndexOf('.') + 1, fileName.length).toLowerCase();
                var allowedExtensions = ["jpg", "jpeg", "png", "gif", "swf"];

                if (allowedExtensions.indexOf(ext) === -1) {
                    alert(MSG.errFileExtension);
                    $("#FileName").val("");
                    $('#btnUpload').attr("disabled", "disabled");
                    $(fileElement).val(null);
                    return false;
                } else {

                    $("#FileName").val(fileName);
                    if (ext === "swf") {
                        $('#ImageType').val("2");
                    } else {
                        $('#ImageType').val("1");
                    }
                    $('#ImageType').createDropdownchecklist();

                    if ($('#btnUpload').length > 0) {
                        $('#btnUpload').removeAttr("disabled");
                    }
                }
            }

            return true;
        },
        addBanner: function () {
            AFFILIATE.openDialogFrame("Media/AddBanner", MSG.AddBanner, { width: 580, height: 500 });
        },
        editBanner: function (bannerId) {
            AFFILIATE.openDialogFrame("Media/EditBanner/" + bannerId, MSG.EditBanner, { width: 580, height: 500 });
        },
        deleteBanner: function (id, content, totalRequest) {
            if (id === undefined) {
                alert(MSG.InvalidBanner);
            } else {
                var totalReq = parseInt(totalRequest, 10);
                if (!isNaN(totalReq) && totalReq > 0) {
                    return;
                }

                if (confirm(MSG.DeleteBannerMsg)) {
                    var param = {
                        bannerIds: id,
                        contents: content
                    };
                    $.ajax({
                        url: AFFILIATE.getAbsoluteUrl("Media/DeleteBanner"),
                        type: 'post',
                        data: param,
                        success: function (result) {
                            AFFILIATE.PreHandleAjaxResult(result);
                            if (!result.Success) {
                                alert(result.Message);
                            } else {
                                AFFILIATE.showUpdateStatus(result.ErrorCode, result.Message, parent.BannerList.ReRenderAfterPaged);
                            }
                        }
                    });
                }
            }
        }
    };
})(jQuery);

$(function () {
    "use strict";
    BannerList.init();
});