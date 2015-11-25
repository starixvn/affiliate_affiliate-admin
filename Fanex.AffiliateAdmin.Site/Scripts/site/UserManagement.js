/*global AFFILIATE,gridOption, MSG*/
var Management = (function ($) {
    "use strict";
    var submitOptions = {};
    return {
        init: function () {
            if ($('#gridId').length > 0) {
                Management.submitRequest();
            }

            $(".main-content").on("click", "#btnAddgridId", function () {
                Management.addUser();
            });

            $('.search-panel form').submit(function () {
                Management.submitRequest();
                return false;
            });

            $(".popup").on("click", ".icon-assign-permission-to-multi-user", function () {
                Management.updatePolicy();
            });

            if (!AFFILIATE.isIT()) {
                gridOption.columns.removeItem('Name', 'Password');
                gridOption.titleInfo.customButtons = null;
            }
        },
        reInit: function () {
            $('input:checkbox').change(function () {
                if ($(this).val() === 'all') {
                    $("[name=user]").prop("checked", $(this).prop("checked"));
                }
            });
        },
        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject();
            Management.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption); // jshint ignore:line

            if (gridOption && gridOption.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage : gridOption.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOption.pagerOption.itemsPerPage;
            }

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Management/Users"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    $('#gridId').removeClass('noinfo');

                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.List.length
                    };
                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);
                    $('#gridId').show().fanexGrid(gridOption);

                    if (data.List.length === 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', MSG.NoInfo);
                    } else {
                        Management.reInit();
                    }

                    $('.search-panel').width($('.grid-title').width());
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        addUser: function () {
            AFFILIATE.openDialogFrame("Management/AddUser", window.MSG.AddNewUser, { width: 500, height: 250 });
            return false;
        },
        updatePolicy: function () {
            var multiCustName = '';
            var multiCustId = '';

            $("[name=user]").each(function () {
                if ($(this).prop("checked")) {
                    if (multiCustId !== '') {
                        multiCustId += ',';
                        multiCustName += ',';
                    }
                    multiCustId += $(this).val();
                    multiCustName += $(this).attr("data-name");
                }

            });

            if (multiCustId !== '') {
                var param = {
                    CustIds: multiCustId
                };

                AFFILIATE.openDialogFrame("Management/ChangePolicyMultiUser?" + jQuery.param(param), multiCustName, { width: 500, height: 430 });
            }
            return false;
        },
        viewAccountPolicy: function (custId, userName, groupName) {
            var param = {
                custId: custId
            };

            AFFILIATE.openDialogFrame("Management/ChangePolicyUser?" + jQuery.param(param),
                window.MSG.AccountPolicyOfUser.format(userName), 
                { width: 490, height: 530 });
        },
        viewGroup: function (groupId, groupName) {
            var param = {
                groupId: groupId
            };
            var width = AFFILIATE.isIT() ? 530 : 445;
            AFFILIATE.openDialogFrame("Management/UserInGroup?" + jQuery.param(param), window.MSG.Group + " - "
                + groupName, { width: width, height: 650 });
        },
        editPassword: function (custId, userName) {
            var param = {
                custId: custId,
                userName: userName
            };

            AFFILIATE.openDialogFrame("Management/ChangePassword?" + jQuery.param(param),
                window.MSG.ChangePassword + ' - ' + userName,
                { width: 500, height: 350 });
        },
        showMsg: function (data) {
            if (data.ErrorCode !== 0) {
                AFFILIATE.showErrorMsg(data.Message);
            } else {
                AFFILIATE.showSuccessMsg(data.Message);
            }
            setTimeout(AFFILIATE.hideMsg, 3000);
        }
    };
})(jQuery);

$(function () {
    "use strict";
    Management.init();
});