var Policy = function ($) {
    return {
        init: function () {

            if ($('#gridId').length > 0) {
                Policy.ReRenderAfterPaged();
                $('#gridId').hide();
            }
            if ($("#chkAll").length > 0) {
                Policy.ReCheckAll();
            }

            $("#btnCancel").click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
            $("#btnUpdate").click(function () {
                var irs = $("[name=IndividualRead]");
                var ihs = $("[name=IndividualHost]");
                var count = irs.length;
                var policys = [];
                var funcId;
                for (var index = 0; index < count; index++) {
                    funcId = $(ihs[index]).attr("id").split("_")[1];

                    if ($(ihs[index]).prop("checked")) {
                        policys[index] = $("#CustId").val() + ',' + funcId + ',' + 3;
                    }
                    else if ($(irs[index]).prop("checked")) {
                        policys[index] = $("#CustId").val() + ',' + funcId + ',' + 1;
                    }
                    else {
                        policys[index] = $("#CustId").val() + ',' + funcId + ',' + 0;
                    }
                }
                $("#IndividualPolicy").val(policys.join(';'));
                return true;
            });
            $("#chkAll").change(function () {
                $("[name=chkCurrency]").prop("checked", $("#chkAll").prop("checked"));
            });

            $("#btnSubmit").click(function () {
                var currencyList = $("[name=chkCurrency]");
                var number = currencyList.length;
                var currencies = [];

                for (var index = 0; index < number; index++) {
                    if ($(currencyList[index]).prop("checked")) {
                        currencies.push($(currencyList[index]).val());
                    }
                }

                $("#Currencies").val(currencies.join(';'));

                return true;
            });

            $("[name=chkCurrency]").change(function () {
                if (!$(this).prop("checked")) {
                    $("#chkAll").prop("checked", "");
                }
                else {
                    Policy.ReCheckAll();
                }
            });
        },
        reInit: function () {
            $(".p-check").change(function () {
                var funcId = $(this).attr("id").split("_")[1];
                if ($(this).prop("checked")) {
                    if ($(this).attr("name") == "IndividualHost") {
                        $("#ir_" + funcId).prop("checked", true);
                    }
                }
                else {
                    if ($(this).attr("name") == "IndividualRead") {
                        $("#ih_" + funcId).prop("checked", false);
                    }
                }
            });
        },
        ReRenderAfterPaged: function () {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            var param = {
                custId: $("#CustId").val(),
                groupId: $("#GroupId").val()
            }
            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Management/ListPolicyUser"),
                type: 'post',
                data: param,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
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
        },
        ReCheckAll: function () {
            var currencyList = $("[name=chkCurrency]");
            var number = currencyList.length;
            var counter = 0;

            for (var index = 0; index < number; index++) {
                if ($(currencyList[index]).prop("checked")) {
                    counter++;
                }
            }
            if (counter == number) {
                $("#chkAll").prop("checked", "checked");
            }
        }
    }
} (jQuery);

$(function () {
    Policy.init();
});