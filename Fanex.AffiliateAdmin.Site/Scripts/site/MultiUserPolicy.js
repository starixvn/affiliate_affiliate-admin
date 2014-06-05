/*global AFFILIATE,gridOption*/
var Policy = (function ($) {
    "use strict";
    return {
        init: function () {
            Policy.ReRenderAfterPaged();

            $('#gridId').hide();

            $("#btnUpdate").click(function () {
                var irs = $("[name=IndividualRead]");
                var ihs = $("[name=IndividualHost]");
                var count = irs.length;
                var policys = [];
                var permission;

                var custIds = $("#CustIds").val().split(',');

                for (var index = 0; index < count; index++) {
                    permission = Policy.getPolicy(irs, ihs, index);

                    for (var custIndex = 0; custIndex < custIds.length; custIndex++) {
                        policys.push(custIds[custIndex] + ',' + permission);
                    }
                }

                $("#IndividualPolicy").val(policys.join(';'));
                return true;
            });
        },
        getPolicy: function (IndividualReads, IndividualHosts, index) {
            var permission;
            var funcId = $(IndividualReads[index]).attr("id").split("_")[1];

            if ($(IndividualHosts[index]).prop("checked")) {
                permission = 3;
            } else if ($(IndividualReads[index]).prop("checked")) {
                permission = 1;
            } else {
                permission = 0;
            }

            return funcId + ',' + permission;
        },
        reInit: function () {
            $(".p-check").change(function () {
                var funcId = $(this).attr("id").split("_")[1];
                if ($(this).prop("checked")) {
                    if ($(this).attr("name") === "IndividualHost") {
                        $("#ir_" + funcId).prop("checked", true);
                    }
                } else {
                    if ($(this).attr("name") === "IndividualRead") {
                        $("#ih_" + funcId).prop("checked", false);
                    }
                }
            });
        },
        ReRenderAfterPaged: function () {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption); // jshint ignore:line

            var param = {
                custIds: $("#CustIds").val()
            };
            
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Management/ListPolicyMultiUser"),
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
    };
})(jQuery);

$(function () {
    "use strict";
    Policy.init();
});