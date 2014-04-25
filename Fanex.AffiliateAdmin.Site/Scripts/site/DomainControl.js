var DOMAINCONTROL = function ($) {
    return {
        init: function () {
            // Event click login
            $("#addDomain").click(function () {
                DOMAINCONTROL.Insert();
            });
            if ($('#gridId').length > 0) {
                DOMAINCONTROL.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

            $(".main-content").on("click", ".icon-add-affDomain", function () {
                DOMAINCONTROL.Insert();
            });
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Domain/DomainControlList"),
                type: 'post',
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    if (data.List.length == 0) {
                        $('#gridId').addClass("noinfo").html(MSG.NoInfo);
                        return;
                    }
                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
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
        Insert: function () {
            AFFILIATE.openDialogFrame("Domain/DomainControlAdd", lang.AddDomain, { width: 500, height: 370 });
        },
        ShowUpdate: function (id) {
            AFFILIATE.openDialogFrame("Domain/DomainControlUpd/" + id, lang.EditDomain, { width: 500, height: 370 });
        },
        Remove: function (id) {
            if (confirm(lang.DeleteDomain)) {
                $.ajax({ url: AFFILIATE.getAbsoluteUrl("Domain/DomainControlRemove/" + id),
                    type: 'post',
                    success: function (result) {
                        AFFILIATE.PreHandleAjaxResult(result);
                        
                        if (!result.Success) {
                            alert(result.Message);
                        }
                        else {
                            DOMAINCONTROL.ReRenderAfterPaged(0, $("#itemperpage").val());
                        }
                    }
                });
            }
        }
    }
} (jQuery);

$(function () {
    DOMAINCONTROL.init();
});