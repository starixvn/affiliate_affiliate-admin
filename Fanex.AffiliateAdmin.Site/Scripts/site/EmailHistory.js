var ME = function ($) {
    return {
        init: function () {
            //AFFILIATE.RefreshGridData = ME.ReRenderAfterPaged;
            $("#SearchOption").change(function () {

                if (this.value >= 0)
                    $("#SearchText").show();
                else
                    $("#SearchText").hide();

                $("#SearchText").val("").focus();
            });

            $("#IsAll").change(function () {

                $("#FromDate, #ToDate").attr("disabled", this.checked);
            });

            $("#gridId").on("click", "table a.subject", function () {
                var param = {
                    id: $(this).attr("data-mid")
                };

                var queryString = jQuery.param(param);
                var url = "EmailHistory/Detail?" + queryString;

                AFFILIATE.openDialogFrame(url, "Email Detail", { width: 500, height: 400 });

            });

            ME.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);

            $('#btnSubmit').click(function () {
                ME.submitRequest();
                return false;
            });
        },
        Delete: function (id) {
            try {
                if (!id || id < 1)
                    return;
                if (!confirm(MSG.ConfirmDeleteEmail))
                    return;
                $.ajax({ url: AFFILIATE.getAbsoluteUrl("EmailHistory/Delete"),
                    type: 'post',
                    data: {
                        listID: id
                    },
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);
                        
                        if (!data.Success) {
                            alert(data.Message);
                        } else {
                            ME.ReRenderAfterPaged();
                        }
                    }
                });

            }
            catch (e) {
                alert(e);
            }
        },
        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            ME.ReRenderAfterPaged();
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            if (gridOption && gridOption.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage : gridOption.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOption.pagerOption.itemsPerPage
            }

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("EmailHistory/EmailHistory"),
                type: 'post',
                data: $('#SearchForm').find('select,input').serialize() + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    
                    if (data.TotalItem == 0) {
                        $('#gridId').addClass("noinfo").html(MSG.NoInfo);
                        return;
                    }
                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: ME.ReRenderAfterPaged
                    };
                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                }
            });
        }
    }
} (jQuery);
$(function () {
    ME.init();
});