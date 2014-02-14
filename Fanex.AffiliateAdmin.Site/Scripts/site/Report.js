var REPORT = function ($) {
    return {
        init: function () {

            // Event click login
            $("#addDomain").click(function () {
                REPORT.Insert();
            });
            if ($('#gridId').length > 0) {
                REPORT.ReRenderAfterPaged(0, 10);
            }

            $("#btnCancel").click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
            $("#abc").createSearchPanel("abc");
            /*
            $("#dialog").dialog({
            autoOpen: false,
            show: {
            effect: "blind",
            duration: 1000
            },
            hide: {
            effect: "explode",
            duration: 1000
            }
            });

            $(document).tooltip();

            $("#info").click(function () {
            $("#dialog").dialog("open");
            });
            $("#excel").click(function () {
            alert("excel click");
            });
            */
            $("#collapse").click(function () {
                alert("test");
            });
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Domain/AffDomainList"),
                type: 'post',
                //data: "pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    
                    var bodyRows = AFFILIATE.genGridData(gridOption.columns, data.List);
                    for (var i = 0; i < bodyRows.length; i++) {
                        bodyRows[i].Columns[4].CustomCellRender = function (cell, cellFormated) {
                            return "<a href=\"javascript:REPORT.ShowUpdate('" + cell.Value + "')\"><i class=\"icon-edit\"></i></a>";
                        }
                        bodyRows[i].Columns[5].CustomCellRender = function (cell, cellFormated) {
                            return "<a href=\"javascript:REPORT.Remove('" + cell.Value + "')\"><i class=\"icon-remove\"></i></a>";
                        }
                    }
                    gridOption.bodyRows = bodyRows;
                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: [10, 20, 50, 100, 200],
                        currentPage: currentPage,
                        maxItemsPerPage: 200,
                        totalItem: bodyRows.length - 1
                    };

                    $('#gridId').fanexGrid(gridOption);
                }
            });
        },
        Insert: function () {
            AFFILIATE.openDialogFrame("Domain/AffDomainAdd", lang.AddDomain, { width: 450, height: 230 });
        },
        ShowUpdate: function (id) {
            AFFILIATE.openDialogFrame("Domain/AffDomainUpd/" + id, lang.EditDomain, { width: 450, height: 230 });
        },
        Remove: function (id) {
            if (confirm(lang.DeleteDomain)) {
                $.ajax({ url: AFFILIATE.getAbsoluteUrl("Domain/AffDomainRemove/" + id),
                    type: 'post',
                    success: function (result) {
                        AFFILIATE.PreHandleAjaxResult(result);
                        
                        if (!result.Success) {
                            alert(result.Message);
                        }
                        else {
                            REPORT.ReRenderAfterPaged(0, $("#itemperpage").val());
                        }
                    }
                });
            }
        }
    }
} (jQuery);

$(function () {
    REPORT.init();
});