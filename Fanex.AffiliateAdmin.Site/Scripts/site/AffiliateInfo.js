/*global AFFILIATE,MSG*/
var AFFINFO = (function ($) {
    "use strict";
    var statusTimeOut;
    var langTimeOut;

    function hideLeftPanel() {
        $("#left_show").hide();
        $("#left_hide").show();
        $("#c-panel").animate({
            left: "20px"
        }, 300, function () {
            $("#l-panel").addClass("l-panel-hide");
        });
    }

    function showLeftPanel() {
        $("#left_hide").hide();
        $("#left_show").show();
        $("#c-panel").animate({
            left: "256px"
        }, 300, function () {
            $("#l-panel").removeClass("l-panel-hide");
        });
    }

    return {
        init: function () {
            $('#btnAdd').click(function () {
                $("#AffiliateInfo_Username").val($.trim($("#AffiliateInfo_Username").val()));
                parent.location.href = AFFILIATE.getAbsoluteUrl("AffiliateInfo") + "?Username=" + $("#AffiliateInfo_Username").val();
                return false;
            });

            $('#left_show_i').click(function () {
                hideLeftPanel();
            });
            $('#left_hide_i').click(function () {
                showLeftPanel();
            });
            $('#spClosed').click(function () {
                AFFINFO.changeOpenClose();
            });
            $('#spLockedOut').click(function () {
                AFFINFO.lockedOut();
            });
            $('#spStatus').click(function () {
                var value = $('#slStatus option').filter(function () { return $(this).html() === $('#spStatus').text(); }).val();

                $('#slStatus').val(value);
                $("#slStatus").show();
                $(this).hide();
            });
            $('#slStatus').change(function () {
                AFFINFO.updateStatus($("#AffId").val(), $("#Username").val(), $("#Currency").val());
            });
            $('#slStatus').blur(function () {
                AFFINFO.setStatusTimeOut(false);
            });
            $('#slStatus').focus(function () {
                AFFINFO.setStatusTimeOut(true);
            });
            $('#spLanguage').click(function () {
                $("#slLanguage").show();
                $(this).hide();
            });
            $('#slLanguage').change(function () {
                if (confirm(MSG.ConfirmChangeLanguage)) {
                    $.ajax({
                        url: AFFILIATE.getAbsoluteUrl("AffiliateInfo/ChangeLanguage"),
                        type: 'post',
                        data: "PreferredLanguage=" + $(this).val() + "&affid=" + $("#AffId").val(),
                        beforeSend: function () {
                            $.blockUI();
                        },
                        success: function (data) {
                            AFFILIATE.PreHandleAjaxResult(data);

                            if (data.Success) {
                                $("#spLanguage").html($("#slLanguage").find(":selected").text());
                                $("#slLanguage").hide();
                                $("#spLanguage").show();
                            }
                            else {
                                alert(data.Message);
                            }

                            $('#slLanguage').blur();
                        },
                        complete: function () {
                            $.unblockUI();
                        }
                    });
                }
            });
            $('#slLanguage').blur(function () {
                AFFINFO.setLanguageTimeOut(false);
            });
            $('#slLanguage').focus(function () {
                AFFINFO.setLanguageTimeOut(true);
            });

            ////Affiliate Type
            $('#spAffType').click(function () {
                $("#slAffType").show();
                $(this).hide();
            });
            $('#slAffType').change(function () {
                if (confirm(MSG.ConfirmChangeAffType)) {
                    $.ajax({
                        url: AFFILIATE.getAbsoluteUrl("AffiliateInfo/ChangeAffType"),
                        type: 'post',
                        data: "AffType=" + $(this).val() + "&affid=" + $("#AffId").val(),
                        beforeSend: function () {
                            $.blockUI();
                        },
                        success: function (data) {
                            AFFILIATE.PreHandleAjaxResult(data);
                            if (!data.Success) {
                                alert(data.Message);
                            } else {
                                $("#AffiliateInfo_Username").val($.trim($("#AffiliateInfo_Username").val()));
                                window.top.location.href = AFFILIATE.getAbsoluteUrl("AffiliateInfo") + "?Username=" + $("#AffiliateInfo_Username").val();
                            }
                        },
                        complete: function () {
                            $.unblockUI();
                        }
                    });
                }
            });
            $('#slAffType').blur(function () {
                AFFINFO.setAffTypeTimeOut(false);
            });
            $('#slAffType').focus(function () {
                AFFINFO.setAffTypeTimeOut(true);
            });

            $('#spPassword').click(function () {
                AFFINFO.changePassword($("#AffId").val(), $("#Username").val());
            });

            $('#spEdit').click(function () {
                var param = {
                    affids: $("#AffId").val(),
                    usernames: $("#Username").val(),
                    currency: $("#Currency").val(),
                    groupId: $("#GroupId").val()
                };

                parent.AFFILIATE.openDialogFrame("Affiliate/ChangeCommissionGroup?" + jQuery.param(param), MSG.ChangeComGroup, { width: 450, height: 300 });
            });

            $('#groupLink').click(function () {
                var param = {
                    groupid: $("#GroupId").val()
                };
                parent.AFFILIATE.openDialogFrame("CommGroup/ViewSetting?" + jQuery.param(param), MSG.CommissionGroup, { width: 1000, height: 450 });
            });

            $('#accountType').click(function () {
                var param = {
                    affids: $("#AffId").val(),
                    usernames: $("#Username").val(),
                    testlevel: $(this).data('test-level')
                };
                parent.AFFILIATE.openDialogFrame("Affiliate/ChangeAccountType?" + jQuery.param(param), MSG.ChangeAccountType, { width: 500, height: 300 });
            });
        },
        changePassword: function (affId, affName) {
            AFFILIATE.openDialogFrame("AffiliateInfo/ChangePassword?affid=" + affId + "&affname=" + affName, affName + ' - ' + window.Lang.ChangePassword, { width: 500, height: 280 });
        },
        changeAffInfo: function (param) {
            AFFINFO.updateAffInfo(param, true);
        },
        updateAffInfo: function (param, isReload) {
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("AffiliateInfo/ChangeStatus"),
                type: 'post',
                data: jQuery.param(param),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    if (data.Success) {
                        alert(data.Message);
                        if (isReload) {
                            window.location.reload();
                        }
                        AFFINFO.setStatus();
                    } else {
                        alert(data.Message);
                    }
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        openNewWindow: function (url) {
            window.open(url);
        },
        changeOpenClose: function () {
            var status = $("#spClosed").html() === MSG.Yes ? false : true;
            var msg = status ? MSG.ConfirmClose : MSG.ConfirmOpen;

            if (confirm(msg)) {
                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("AffiliateInfo/ChangeOpenClose"),
                    type: 'post',
                    data: "Closed=" + status + "&affid=" + $("#AffId").val(),
                    beforeSend: function () {
                        $.blockUI();
                    },
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);

                        if (data.Success) {
                            document.location.reload();
                        }
                        else {
                            alert(data.Message);
                        }
                    },
                    complete: function () {
                        $.unblockUI();
                    }
                });
            }
        },
        lockedOut: function () {
            if (confirm(MSG.ConfirmUnLocked)) {
                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("AffiliateInfo/UnLocked"),
                    type: 'post',
                    data: {
                        affId: $("#AffId").val()
                    },
                    beforeSend: function () {
                        $.blockUI();
                    },
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);
                        if (data.Success) {
                            $("#spLockedOut").html(MSG.No).removeClass("link-box-active");
                            $("#spLockedOut").unbind("click");
                        } else {
                            alert(data.Message);
                        }
                    },
                    complete: function () {
                        $.unblockUI();
                    }
                });
            }
        },
        setStatusTimeOut: function (isShow) {
            clearTimeout(statusTimeOut);
            if (!isShow) {
                statusTimeOut = setTimeout(function () { AFFINFO.changeStatus(false, true); }, 4000);
            } else {
                statusTimeOut = setTimeout(function () { AFFINFO.changeStatus(true, false); }, 4000);
            }
        },
        setLanguageTimeOut: function (isShow) {
            clearTimeout(langTimeOut);
            if (!isShow) {
                langTimeOut = setTimeout(function () { AFFINFO.changeLanguage(false, true); }, 4000);
            } else {
                langTimeOut = setTimeout(function () { AFFINFO.changeLanguage(true, false); }, 4000);
            }
        },
        setAffTypeTimeOut: function (isShow) {
            clearTimeout(langTimeOut);
            if (!isShow) {
                langTimeOut = setTimeout(function () { AFFINFO.changeAffType(false, true); }, 4000);
            } else {
                langTimeOut = setTimeout(function () { AFFINFO.changeAffType(true, false); }, 4000);
            }
        },
        changeStatus: function (isChange, setValue) {
            clearTimeout(statusTimeOut);
            if (isChange) {
                $("#slStatus").show();
                $("#spStatus").hide();
                $("#slStatus").focus();

                if (setValue) {
                    var value = $('#slStatus option').filter(function () {
                        return $(this).html() === $('#spStatus').text();
                    }).val();

                    $("#slStatus").val(value);
                }
            } else {
                $("#slStatus").hide();
                $("#spStatus").show();
            }
        },
        changeLanguage: function (isChange, setValue) {
            langTimeOut = null;
            if (isChange) {
                $("#slLanguage").show();
                $("#spLanguage").hide();
                $("#slLanguage").focus();

                if (setValue) {
                    $("#slLanguage").val($("#spLanguage").html());
                }
            } else {
                $("#slLanguage").hide();
                $("#spLanguage").show();
            }
        },
        changeAffType: function (isChange, setValue) {
            langTimeOut = null;
            if (isChange) {
                $("#slAffType").show();
                $("#spAffType").hide();
                $("#slAffType").focus();

                if (setValue) {
                    $("#slAffType").val($("#spAffType").html());
                }
            } else {
                $("#slAffType").hide();
                $("#spAffType").show();
            }
        },
        updateStatus: function (affID, userName, currency) {
            if (confirm(MSG.ConfirmChangeStatus)) {
                var status = $('#slStatus').val();
                var param = {};
                if (status === "Approved") {
                    param = {
                        affIDs: affID,
                        usernames: userName,
                        currency: currency,
                        status: status
                    };
                    AFFILIATE.openDialogFrame("Affiliate/ChangeCommissionApprove?" + jQuery.param(param), MSG.AssignComGroup, { width: 550, height: 300 });  // jshint ignore:line
                } else {
                    param = {
                        affID: affID,
                        status: $("#slStatus").val()
                    };
                    AFFINFO.updateAffInfo(param, false);
                }
            } else {
                $('#slStatus').val($('#spStatus').text());
            }
        },
        setStatus: function () {
            var value = $('#slStatus option:selected').html();
            $("#spStatus").html(value);
            AFFINFO.changeStatus(false, false);
        },
        refreshPage: function () {
            location.href = location.href;
        }
    };
})(jQuery);

$(function () {
    "use strict";
    AFFINFO.init();
});