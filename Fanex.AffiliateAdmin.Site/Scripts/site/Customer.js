var CUSTOMER = function ($) {
    var submitOptions = {};

    return {
        Loaded: false,
        init: function() {

            CUSTOMER.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);

            $('#btnSubmit').click(function() {
                CUSTOMER.submitRequest();
                return false;
            });

            $("#DateOption").change(function() {
                $("#FromDate, #ToDate").prop("disabled", this.value == 0);
            });

            $("#gridId").parent().on("click", "#btnAssign", function() {
                var groupid = $(this).data("groupid");
                var url = "Affiliate/AssignMember";

                AFFILIATE.openDialogFrame(url, "Assign Member To Affiliate", { width: 420, height: 260 });
            });
        },

        submitRequest: function() {
            AFFILIATE.trimBeforeSubmit();
            CUSTOMER.ReRenderAfterPaged();
        },

        ReRenderAfterPaged: function(currentPage, itemsPerPage) {
            // resize height of datagrid            
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject2();

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Affiliate/MemberList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                success: function(data) {
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, CUSTOMER.ReRenderAfterPaged);
                    $(".btnAdd").addClass("icon-add-config");
                }
            });
        },
        Remove: function(username) {
            if (confirm(MSG.ConfirmRemoveMem)) {
                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("Affiliate/CustomerRemove"),
                    type: 'post',
                    data: "username=" + username,
                    success: function(result) {
                        if (!result.Success) {
                            alert(result.Message);
                        } else {
                            CUSTOMER.ReRenderAfterPaged();
                        }
                    }
                });
            }
        }
    };
} (jQuery);

/////// ASSIGN MEMBER TO AFFILIATE //////////////////////

var POPUP = function ($) {
    return {
        init: function () {
            $("#btnCancel").click(function () {
                if (parent.AFFILIATE.currentOpenDialog)
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
            });
            $("form").submit(function () {
                var data = $(".popup-content input").serializeData();
                if (!data.MemberName) {
                    AFFILIATE.showErrorMsg(MSG.InputMem);
                    return false;
                }
                if (!data.AffName) {
                    AFFILIATE.showErrorMsg(MSG.InputAff);
                    return false;
                }
            });
        }
    }
} (jQuery);