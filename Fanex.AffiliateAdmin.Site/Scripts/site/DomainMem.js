/*global AFFILIATE,gridOption,MSG,lang*/
var DOMAIN = (function ($) {
    "use strict";
    return {
        init: function() {
            // Event click login
            $("#addDomain").click(function() {
                DOMAIN.Insert();
            });
            if ($('#gridId').length > 0) {
                DOMAIN.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

            $(".main-content").on("click", ".icon-add-affDomain", function() {
                DOMAIN.Insert();
            });
        },
        ReRenderAfterPaged: function(currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);  // jshint ignore:line

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Domain/MemDomainList"),
                type: 'post',
                //data: "pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                success: function(data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        maxItemsPerPage: 200,
                        totalItem: data.List.length - 1
                    };
                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);
                }
            });
        },
        Insert: function() {
            AFFILIATE.openDialogFrame("Domain/MemDomainAdd", lang.AddDomain, { width: 450, height: 230 });
        },
        ShowUpdate: function(id) {
            AFFILIATE.openDialogFrame("Domain/MemDomainUpd/" + id, lang.EditDomain, { width: 450, height: 230 });
        },
        Remove: function(id) {
            if (confirm(lang.DeleteDomain)) {
                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("Domain/MemDomainRemove/" + id),
                    type: 'post',
                    success: function(result) {
                        AFFILIATE.PreHandleAjaxResult(result);

                        if (!result.Success) {
                            alert(result.Message);
                        } else {
                            DOMAIN.ReRenderAfterPaged(0, $("#itemperpage").val());
                        }
                    }
                });
            }
        }
    };
})(jQuery);

$(function () {
    "use strict";
    DOMAIN.init();
});