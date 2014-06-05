/*global AFFILIATE,MSG*/
var AFFINFO = (function ($) {
    "use strict";
    var statusTimeOut;
    var langTimeOut;
    return {
        init: function () {
            $('#btnAdd').click(function () {
                $("#AffiliateInfo_Username").val($.trim($("#AffiliateInfo_Username").val()));
                window.top.location.href = AFFILIATE.getAbsoluteUrl("AffiliateInfo") + "?Username=" + $("#AffiliateInfo_Username").val();
                return false;
            });

            $('#left_show_i').click(function () {
                $("#left_show").hide();
                $("#left_hide").show();
                parent.AFFMASTER.colBox();
            });
            $('#left_hide_i').click(function () {
                $("#left_hide").hide();
                $("#left_show").show();
                parent.AFFMASTER.expBox();
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
                        data: "PreferredLanguage=" + $(this).val(),
                        beforeSend: function () {
                            $.blockUI();
                        },
                        success: function (data) {
                            AFFILIATE.PreHandleAjaxResult(data);

                            if (data.Success) {
                                alert(data.Message);
                                $("#spLanguage").html($("#slLanguage").find(":selected").text());
                                $("#slLanguage").hide();
                                $("#spLanguage").show();
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
                        data: "AffType=" + $(this).val(),
                        beforeSend: function () {
                            $.blockUI();
                        },
                        success: function (data) {
                            AFFILIATE.PreHandleAjaxResult(data);
                            if (!data.Success) {
                                alert(data.Message);
                            } else {
                                $("#spAffType").html($("#slAffType").find(":selected").text());
                                $("#slAffType").hide();
                                $("#spAffType").show();
                            }
                            $('#slAffType').blur();
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
                var username = $("#lnkUN").html();
                if (username === undefined) {
                    username = $("#spUN").html();
                }
                parent.AFFMASTER.changePassword(username);
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
                    usernames: $("#Username").val()
                };
                parent.AFFILIATE.openDialogFrame("Affiliate/ChangeAccountType?" + jQuery.param(param), MSG.ChangeAccountType, { width: 500, height: 300 });
            });
        },
        changeOpenClose: function () {
            var status = $("#spClosed").html() === MSG.Yes ? false : true;
            var msg = status ? MSG.ConfirmClose : MSG.ConfirmOpen;

            if (confirm(msg)) {
                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("AffiliateInfo/ChangeOpenClose"),
                    type: 'post',
                    data: "Closed=" + status,
                    beforeSend: function () {
                        $.blockUI();
                    },
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);

                        if (data.Success) {
                            alert(data.Message);
                            document.location.reload();
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
                langTimeOut = setTimeout(function() { AFFINFO.changeAffType(true, false); }, 4000);
            }
        },
        changeStatus: function (isChange, setValue) {
            statusTimeOut = null;
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
                        affID: affID,
                        usernames: userName,
                        currency: currency,
                        status: status
                    };
                    parent.AFFILIATE.openDialogFrame("Affiliate/ChangeCommissionApprove?" + jQuery.param(param), MSG.AssignComGroup, { width: 550, height: 300 });  // jshint ignore:line
                } else {
                    param = {
                        status: $("#slStatus").val()
                    };
                    parent.AFFMASTER.updateAffInfo(param, false);
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