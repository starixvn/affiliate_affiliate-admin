var Management = function ($) {
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
        },
        reInit: function () {
            $('input:checkbox').change(function () {
                if ($(this).val() == 'all') {
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
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            if (gridOption && gridOption.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage : gridOption.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOption.pagerOption.itemsPerPage
            }

            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Management/Users"),
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

                    if (data.List.length == 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', PageVar.NoInfo);
                    }
                    else {
                        Management.reInit();
                    }
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        addUser: function () {
            AFFILIATE.openDialogFrame("Management/AddUser", PageVar.AddNewUser, { width: 500, height: 400 });
            return false;
        },
        updatePolicy: function () {
            var multiCustName = '';
            var multiCustId = '';

            $("[name=user]").each(function () {
                if ($(this).prop("checked")) {
                    if (multiCustId != '') {
                        multiCustId += ',';
                        multiCustName += ',';
                    }
                    multiCustId += $(this).val();
                    multiCustName += $(this).attr("data-name");
                }

            });

            if (multiCustId != '') {
                var param = {
                    CustIds: multiCustId
                };

                AFFILIATE.openDialogFrame("Management/ChangePolicyMultiUser?" + jQuery.param(param), multiCustName, { width: 500, height: 430 });
            }
            return false;
        },
        editAccount: function (custId, userName) {
            var param = {
                custId: custId
            };

            AFFILIATE.openDialogFrame("Management/EditUser?" + jQuery.param(param), userName, { width: 500, height: 270 });
        },
        changeStatus: function (obj, custId) {
            if (!Management.updateStatus(custId, Management.getStatus(obj, custId)))
                Management.getStatus(obj);
            else {
                Management.ReRenderAfterPaged(0, $("#itemperpage").val());
            }
        },
        viewAccountPolicy: function (custId, userName, groupName) {
            var param = {
                custId: custId
            };

            AFFILIATE.openDialogFrame("Management/ChangePolicyUser?" + jQuery.param(param), groupName + ' - ' + userName, { width: 490, height: 530 });
        },
        viewGroup: function (groupId, groupName) {
            var param = {
                groupId: groupId
            }

            AFFILIATE.openDialogFrame("Management/UserInGroup?" + jQuery.param(param), 'Group - ' + groupName, { width: 960, height: 650 });
        },
        editPassword: function (custId, userName) {
            var param = {
                custId: custId,
                userName: userName
            }

            AFFILIATE.openDialogFrame("Management/ChangePassword?" + jQuery.param(param), userName + ' - Change Password', { width: 500, height: 250 });
        },
        getStatus: function (obj, custId) {
            //var className = $(obj).find("i:first").attr("class");
            var className = $("#i_" + custId).attr("class");
            if (className == "icon-ok-circle") {
                $(obj).find("i").attr("class", "icon-ban-circle");
                return false;
            }
            else {
                $(obj).find("i").attr("class", "icon-ok-circle");
                return true;
            }
        },
        updateStatus: function (custId, status) {
            if (!confirm(PageVar.ChangeStatus)) {
                return false;
            }
            var param = {
                custId: custId,
                closed: !status
            };
            $.ajax({ url: AFFILIATE.getAbsoluteUrl("Management/ChangeStatus"),
                type: 'post',
                data: jQuery.param(param),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    
                    Management.showMsg(data);
                    if (data.ErrorCode != 0) {
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
            if (data.ErrorCode != 0) {
                AFFILIATE.showErrorMsg(data.Message);
            }
            else {
                AFFILIATE.showSuccessMsg(data.Message);
            }
            setTimeout(AFFILIATE.hideMsg, 3000);
        }
    }
} (jQuery);

$(function () {
    Management.init();
});