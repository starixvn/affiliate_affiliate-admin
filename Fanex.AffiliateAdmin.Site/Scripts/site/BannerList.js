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
                                AFFILIATE.showUpdateStatus(result.ErrorCode, result.Message, window.location.reload(false));
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