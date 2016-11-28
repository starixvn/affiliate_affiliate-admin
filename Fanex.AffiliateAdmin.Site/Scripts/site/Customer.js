/*global AFFILIATE,gridOption,MSG*/
var CUSTOMER = (function ($) {
    "use strict";
    var submitOptions = {};

    return {
        Loaded: false,
        init: function () {

            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });
            
            CUSTOMER.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);

            $('.search-panel form').submit(function () {
                CUSTOMER.submitRequest();
                return false;
            });

            $('.form-popup').submit(function () {
                AFFILIATE.trimBeforeSubmit();
            });

            $("#DateOption").change(function () {
                $("#FromDate, #ToDate").prop("disabled", this.value === '0');
            });

            $("#gridId").parent().on("click", "#btnAssign", function () {
                var url = "Affiliate/AssignMember";

                if ($('#ByAffiliate').length > 0) {
                    url += "?ByAffiliate=True";
                }

                AFFILIATE.openDialogFrame(url, window.MSG.AssignMemToAff, { width: 420, height: 260 });
            });
        },

        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            CUSTOMER.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid            
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);  // jshint ignore:line

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject2();

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Affiliate/MemberList"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    AFFILIATE.renderGrid($('#gridId'), data, gridOption, CUSTOMER.ReRenderAfterPaged);
                    $(".btnAdd").addClass("icon-add-config");
                }
            });
        },
        Remove: function (username) {
            if (confirm(MSG.ConfirmRemoveMem)) {
                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("Affiliate/CustomerRemove"),
                    type: 'post',
                    data: "username=" + username,
                    success: function (result) {
                        AFFILIATE.PreHandleAjaxResult(result);
                        
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
}) (jQuery);

/////// ASSIGN MEMBER TO AFFILIATE //////////////////////

/*jshint ignore:start */
var AssignMemberPopup = (function ($) {
    "use strict";
    return {
        init: function() {
            $("form").submit(function() {
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
    };
})(jQuery);
/*jshint ignore:end */
