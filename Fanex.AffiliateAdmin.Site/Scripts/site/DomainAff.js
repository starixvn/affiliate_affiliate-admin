/*global AFFILIATE,gridOption,lang*/
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
                url: AFFILIATE.getAbsoluteUrl("Domain/AffDomainList"),
                type: 'post',
                success: function(data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage || AFFILIATE.pageSizes[0],
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        maxItemsPerPage: 200,
                        totalItem: data.List.length
                    };
                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);
                }
            });
        },
        Insert: function() {
            AFFILIATE.openDialogFrame("Domain/AffDomainAdd", lang.AddDomain, { width: 550, height: 230 });
        },
        ShowUpdate: function(id) {
            AFFILIATE.openDialogFrame("Domain/AffDomainUpd/" + id, lang.EditDomain, { width: 550, height: 230 });
        },
        Remove: function(id) {
            if (confirm(lang.DeleteDomain)) {
                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("Domain/AffDomainRemove/" + id),
                    type: 'post',
                    success: function(result) {
                        AFFILIATE.PreHandleAjaxResult(result);

                        if (result.IsSuccess) {
                            DOMAIN.ReRenderAfterPaged(0, $("#itemperpage").val());
                        } else {
                            alert(result.ErrorMessage);
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