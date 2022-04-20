/*global AFFILIATE,gridOption,MSG*/
var Product = (function ($) {
    "use strict";
    var submitOptions = {};

    return {
        init: function() {
            if ($('#gridId').length > 0) {
                Product.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

            $('.search-panel form').submit(function() {
                Product.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });

            $(".main-content").on("click", ".icon-add-config", function() {
                Product.addProduct();
            });

            $('.form-popup').submit(function() {
                AFFILIATE.trimBeforeSubmit();
            });

            $('#btnDelete').click(function() {
                Product.deleteProduct();
            });

            $('#btnAdd').click(function() {
                Product.addProduct();
            });
        },

        ReRenderAfterPaged: function(currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);  // jshint ignore:line

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject();

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Configuration/GetProductList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function() {
                    $.blockUI();
                },
                success: function(data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    gridOption.titleInfo.text = MSG.Title + ' [{0}]'.format(data.List.length);
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, Product.ReRenderAfterPaged);
                },
                complete: function() {
                    $.unblockUI();
                }
            });
        },

        addProduct: function() {
            var url = "Configuration/AddProduct";
            AFFILIATE.openDialogFrame(url, MSG.AddNewProduct, { width: 580, height: 400 });
        },

        deleteProduct: function(id) {
            if (id === undefined) {
                alert(MSG.InvalidProduct);
            } else {
                if (confirm(MSG.DeleteProductMsg)) {
                    $.ajax({
                        url: AFFILIATE.getAbsoluteUrl("Configuration/DeleteProduct?idlist=" + id),
                        type: 'post',
                        success: function(result) {
                            AFFILIATE.PreHandleAjaxResult(result);

                            if (result.IsSuccess) {
                                AFFILIATE.showUpdateStatus(result.ErrorCode, result.ErrorMessage, Product.ReRenderAfterPaged);
                            } else {
                                alert(result.ErrorMessage);
                            }
                        }
                    });
                }
            }
        },

        showProductDlg: function(id) {
            var url = "Configuration/ProductDetail";
            if (id) {
                url += "?Id=" + id;
            }
            AFFILIATE.openDialogFrame(url, MSG.UpdateProduct, { width: 580, height: 400 });
        }
    };
}) (jQuery);

$(function () {
    "use strict";
    Product.init();
});