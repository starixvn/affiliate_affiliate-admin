/*global AFFILIATE,gridOption,PageVar*/

var Management = (function ($) {
    "use strict";
    var submitOptions = {};
    return {
        init: function () {
            submitOptions = $('form').serializeObject();

            if ($('#gridId').length > 0) {
                Management.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }

            $(".main-content").on("click", ".icon-add-group", function () {
                Management.addGroup();
            });

            if (!canUpdateData) {
                gridOption.columns.removeItem('Name', 'Update');
                gridOption.titleInfo.customButtons = null;
            }
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);  // jshint ignore:line

            if (gridOption && gridOption.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage : gridOption.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOption.pagerOption.itemsPerPage;
            }

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Management/GroupDataList"),
                type: 'post',
                data: jQuery.param(submitOptions),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    if (data.List.length === 0) {
                        $('#gridId').addClass("noinfo").html(MSG.NoInfo);
                        return;
                    }
                    gridOption.pagerOption = {
                        itemsPerPage: itemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: currentPage,
                        totalItem: data.List.length
                    };
                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        addGroup: function () {
            AFFILIATE.openDialogFrame("Management/AddGroup", window.MSG.AddNewGroup, { width: 500, height: 400 });
            return false;
        },
        viewGroup: function (groupId, groupName) {
            var param = {
                groupId: groupId
            };
            
            AFFILIATE.openDialogFrame("Management/UserInGroup?" + jQuery.param(param), window.MSG.Group + ' - ' + groupName, { width: 540, height: 600 });
        },
        editRolePolicy: function (groupId, groupName) {
            var param = {
                groupId: groupId
            };

            AFFILIATE.openDialogFrame("Management/ChangePolicyGroup?" + jQuery.param(param), window.MSG.GroupPolicyOfGroup.format(groupName), { width: 380, height: 540 });
        },
        editGroup: function (groupId, groupName) {
            var param = {
                groupId: groupId
            };

            AFFILIATE.openDialogFrame("Management/EditGroup?" + jQuery.param(param), groupName, { width: 480, height: 440 });
        },
        deleteGroup: function (groupId, groupName) {
            if (confirm(MSG.ConfirmDelGroup.format(groupName))) {
                var param = {
                    groupId: groupId
                };

                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("Management/DeleteGroup"),
                    type: 'post',
                    data: jQuery.param(param),
                    beforeSend: function () {
                        $.blockUI();
                    },
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);

                        Management.showMsg(data);
                    },
                    complete: function () {
                        $.unblockUI();
                    }
                });
            }
        },
        updateStatus: function (custId, status) {
            if (!confirm($("#changeStatus").val())) {
                return false;
            }
            var param = {
                custId: custId,
                closed: status
            };

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Management/ChangeStatus"),
                type: 'post',
                data: jQuery.param(param),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    Management.showMsg(data);
                    if (data.ErrorCode !== 0) {
                        return false;
                    }
                },
                complete: function () {
                    $.unblockUI();
                }
            });
            return true;
        },
        showMsg: function (data) {
            if (data.ErrorCode !== 0) {
                AFFILIATE.showErrorMsg(data.Message);
                setTimeout(AFFILIATE.hideMsg, 3000);
            } else {
                Management.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
            }
        }
    };
})(jQuery);

$(function () {
    "use strict";
    Management.init();
});