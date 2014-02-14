var Policy = function ($) {
    return {
        init: function () {
            Policy.ReRenderAfterPaged();

            $('#gridId').hide();

            $("#btnCancel").click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
            $("#btnUpdate").click(function () {
                var grs = $("[name=GroupRead]");
                var ghs = $("[name=GroupHost]");
                var count = grs.length;
                var policys = [];
                var funcId;
                for (var index = 0; index < count; index++) {
                    funcId = $(ghs[index]).attr("id").split("_")[1];

                    if ($(ghs[index]).prop("checked")) {
                        policys[index] = $("#GroupId").val() + ',' + funcId + ',' + 3;
                    }
                    else if ($(grs[index]).prop("checked")) {
                        policys[index] = $("#GroupId").val() + ',' + funcId + ',' + 1;
                    }
                    else {
                        policys[index] = $("#GroupId").val() + ',' + funcId + ',' + 0;
                    }
                }
                $("#GroupPolicy").val(policys.join(';'));
                return true;
            });
        },
        reInit: function () {
            $(".p-check").change(function () {
                var funcId = $(this).attr("id").split("_")[1];
                if ($(this).prop("checked")) {
                    if ($(this).attr("name") == "GroupHost") {
                        $("#gr_" + funcId).prop("checked", true);
                    }
                }
                else {
                    if ($(this).attr("name") == "GroupRead") {
                        $("#gh_" + funcId).prop("checked", false);
                    }
                }
            });
        },
        ReRenderAfterPaged: function () {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            var param = {
                groupId: $("#GroupId").val()
            }
            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Management/ListPolicyGroup"),
                type: 'post',
                data: param,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    
                    gridOption.pagerOption = {
                        itemsPerPage: 100,
                        itemsPerPageArray: [100],
                        currentPage: 0,
                        totalItem: 0
                    };

                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    $('#gridId').show().fanexGrid(gridOption);

                    Policy.reInit();
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
    }
} (jQuery);

$(function () {
    Policy.init();
});