/*global AFFILIATE,MSG,COLLAPSE_SUBTOTAL_STATUS,EXPAND_SUBTOTAL_STATUS,subTotalStatus,Lang,System,sampleGridOption*/
var AffiliateList = (function ($) {
    "use strict";

    var submitOptions = {};
    var gridOption = {};
    var COMMGROUP_COL = 12, BF_COL = 14, MEMBER_COL = 15;
    var SYSTEM_CURRENCY = "1";
    var _hideColums = [COMMGROUP_COL, BF_COL, MEMBER_COL];

    var getToggleStatus = function (numCurrencies) {
        var collapseStatus = 0, expandStatus = 1;
        return numCurrencies > 1 ? collapseStatus : expandStatus;
    };

    return {
        init: function () {
            $("#gridId").resize(function () {
                $(".search-panel").width($(this).width());
            });

            if ($('#gridId').length > 0) {
                AffiliateList.submitRequest();
            }

            $('.search-panel form').submit(function () {
                AffiliateList.submitRequest();
                return false;
            });

            $('.form-popup').submit(function () {
                AFFILIATE.trimBeforeSubmit();
            });

            $("#DateOption").change(function () {
                $("#FromDate, #ToDate").prop("disabled", this.value === "0");
            });

            $('#gridId').on('click', '#chkAll', function () {
                var checked = $(this).prop('checked');
                $('.chkItem').prop('checked', checked);
            });

            $('#gridId').on('click', '.chkItem', function () {
                var chkCount = $('.chkItem').length;
                var checkedCount = $('.chkItem:checked').length;

                if (checkedCount === 0) {
                    $('#chkAll').prop('checked', false);
                }

                if (chkCount === checkedCount) {
                    $('#chkAll').prop('checked', true);
                }
            });

            // resize browser
            $("#gridId").resize(function () {
                if ($("#gridId").width() >  AFFILIATE.MIN_CONTENT_WIDTH) {
                    $(".search-panel").width($("#gridId").width());
                } else {
                    $(".search-panel").width(AFFILIATE.MIN_CONTENT_WIDTH);
                }
            });

            var $maincontent = $('.main-content');

            $maincontent.on("click", ".icon-edit-account-type", function () {
                AffiliateList.changeAccountTypeOnMultiAff();
            });

            $maincontent.on("click", ".icon-edit-commission-group", function () {
                AffiliateList.changeCommissionGroupOnMultiAff();
            });
        },
        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
            AffiliateList.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = $.extend(true, {}, sampleGridOption);
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Affiliate/AffiliateList"),
                type: 'post',
                data: $.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    var gridObject = $('#gridId');
                    var legendObject = $('.legend');

                    gridObject.show();
                    legendObject.show();

                    var baseCurrency = $("#BaseCurrency").val();
                    var currencySymbol = baseCurrency === SYSTEM_CURRENCY ? " (" + window.System.Prefix + ")" : "";

                    $.each(gridOption.columns, function (idx, item) {
                        if (item.Name === "BF") {
                            item.HeaderText = item.HeaderCaption + currencySymbol;
                            return false;
                        }
                    });

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: AffiliateList.ReRenderAfterPaged
                    };

                    var bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    gridOption.bodyRows = bodyRows;

                    if (data.HideTotalRows) {
                        gridOption.showFooter = false;
                        gridOption.footerRows = [];
                    } else {
                        gridOption.footerRows = AffiliateList.getFooterRows(data.SubTotal, data.Total, 15, data.HasMultipleCurrency);
                    }

                    gridObject.removeClass("noinfo").fanexGrid(gridOption);

                    if (data.HideTotalRows) {
                        gridObject.fanexGrid("hideColumns", gridOption, _hideColums);
                    }

                    if (data.TotalItem === 0) {
                        gridObject.addClass("noinfo").html(window.Lang.NoDataInfo);
                        legendObject.hide();
                    } else if (!data.HideTotalRows) {
                        ToggleManager.InitForOverview(getToggleStatus(data.SubTotal.length));
                    }

                    $(".btnEditAccountType").addClass("icon-edit-account-type").attr("title", window.MSG.ChangeAccountType);
                    $(".btnEditComGroup").addClass("icon-edit-commission-group").attr("title", window.MSG.ChangeComGroup);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        getFooterRows: function (subTotalData, totalData, colspan, hasMultipleCurrency) {
            var subTotalRowCount = subTotalData.length;
            var footerRows = [];
            var columns = [];

            if (hasMultipleCurrency === true) {
                if (subTotalRowCount > 1 || (subTotalRowCount === 1 && subTotalData[0].CurrencyCode !== System.Currency)) {
                    var realSubTotalclass = subTotalRowCount > 1 ? 'subtotal-cell real-cell' : '';
                    var pseudoSubTotalclass = subTotalRowCount > 1 ? 'subtotal-cell pseudo-cell' : '';

                    $.each(subTotalData, function (idx, item) {
                        columns = [];

                        if (idx === 0) {
                            columns.push({ Value: window.Lang.SubTotal, Name: "ROW", MergeColNumber: colspan, MergeRowNumber: subTotalRowCount, TdClass: realSubTotalclass });  // jshint ignore:line
                        }
                        columns.push({ Value: item.CurrencyCode, Name: "Currency" });
                        columns.push({ Value: item.Total, Name: "BF" });
                        columns.push({ Value: item.TotalMembers, Name: "TotalMembers" });
                        footerRows.push({ Columns: columns, CssClass: "real-row" });
                    });

                    columns = [];
                    columns.push({ Value: window.Lang.SubTotal, Name: "ROW", MergeColNumber: colspan, MergeRowNumber: 1, TdClass: pseudoSubTotalclass });
                    columns.push({ Value: "", Name: "Currency" });
                    columns.push({ Value: "", Name: "BF" });
                    columns.push({ Value: "", Name: "TotalMembers" });
                    footerRows.push({ Columns: columns, CssClass: "pseudo-row" });
                }
            }

            columns = [];
            columns.push({ Value: window.Lang.Total, Name: "ROW", MergeColNumber: colspan });
            columns.push({ Value: totalData.CurrencyCode, Name: "Currency" });
            columns.push({ Value: totalData.Currency, Name: "BF" });
            columns.push({ Value: totalData.TotalMembers, Name: "TotalMembers" });
            footerRows.push({ Columns: columns, CssClass: "total" });

            return footerRows;
        },

        changePassword: function (affid, username) {
            var url = "Affiliate/ChangePassword?id=" + affid + '&username=' + username;
            AFFILIATE.openDialogFrame(url, MSG.ChangePassword, { width: 450, height: 400 });
        },
        changeAccountTypeOnMultiAff: function () {
            var checkedCount = $('.chkItem:checked').length;
            if (checkedCount === 0) {
                alert(MSG.SelectAtLeastOneAffiliate);
                return;
            }

            // get aff id list, username list
            var affList = [], usernameList = [], testlevelList = [];
            $.each($('.chkItem:checked'), function () {
                var self = $(this);
                affList.push(self.attr('data-affid'));
                usernameList.push($(this).attr('data-username'));
                testlevelList.push($(this).attr('data-testlevel'));
            });

            var affids = affList.join();
            var usernames = usernameList.join(", ");
            var testlevel = testlevelList.length > 1 ? -1 : testlevelList[0];
            AffiliateList.changeAccountType(affids, usernames, testlevel);
        },
        changeAccountType: function (affids, usernames, testlevel) {
            var url = "Affiliate/ChangeAccountType?affids=" + affids + '&usernames=' + usernames + '&testlevel=' + testlevel;
            AFFILIATE.openDialogFrame(url, MSG.ChangeAccountType, { width: 450, height: 300 });
        },
        changeCommissionGroupOnMultiAff: function () {
            var checkedCount = $('.chkItem:checked').length;
            if (checkedCount === 0) {
                alert(MSG.SelectAtLeastOneAffiliate);
                return;
            }

            var invalidAffiliateCount = $('.chkItem:checked[data-status="pending"], .chkItem:checked[data-status="disapproved"]').length;
            if (invalidAffiliateCount > 0) {
                alert(MSG.UncheckPendingDisapprovedAffiliates);
                return;
            }

            // get aff id list, username list
            var affList = [], usernameList = [], groupIdList = [], currency;
            var singleCurrency = true;
            $.each($('.chkItem:checked'), function (idx) {
                var self = $(this);
                affList.push(self.attr('data-affid'));
                usernameList.push(self.attr('data-username'));
                groupIdList.push(self.attr('data-groupid'));

                if (idx === 0) {
                    currency = self.attr('data-currency');
                } else {
                    if (currency !== self.attr('data-currency')) {
                        singleCurrency = false;
                        return false; // break loop in jQuery.
                    }
                }
            });

            // Make sure that the currency of selected affiliates must be the same.
            if (!singleCurrency) {
                alert(MSG.CurrencyOfAffiliatesMustBeSame);
                return;
            }

            var affids = affList.join();
            var usernames = usernameList.join(", ");
            var groupId = groupIdList.length > 1 ? -1 : groupIdList[0];

            AffiliateList.changeCommissionGroup(affids, usernames, currency, groupId);
        },
        changeCommissionGroup: function (affids, usernames, currency, groupdId) {
            var url = "Affiliate/ChangeCommissionGroup?affids=" + affids
                + '&usernames=' + usernames
                + '&currency=' + currency
                + '&groupId=' + groupdId;
            AFFILIATE.openDialogFrame(url, MSG.ChangeComGroup, { width: 450, height: 300 });
        },
        viewCommissionGroup: function (groupId) {
            var param = {
                groupid: groupId
            };
            AFFILIATE.openDialogFrame("CommGroup/ViewSetting?" + jQuery.param(param), MSG.CommissionGroup, { width: 900, height: 450 });
        },
        changeFavorite: function (me, affid) {
            // Convention:
            //   1: add favorite (default)
            //   -1: remove favorite
            var param = { affid: affid };
            var action = parseInt($(me).attr('data-action'), 10);

            if (action === 1) {
                AffiliateList.addFavorite(me, param, MSG.RemoveFromMyFavorite);
            } else {
                AffiliateList.removeFavorite(me, param, MSG.AddToMyFavorite);
            }
        },
        addFavorite: function (me, param, title) {
            var url = AFFILIATE.getAbsoluteUrl("Affiliate/AddFavorite");
            var self = $(me);
            $.ajax({
                url: url,
                type: 'post',
                data: param,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    if (!data.Success) {
                        alert(data.ErrorMessage);
                        return;
                    }

                    self.addClass('favor-check').removeClass('favor-uncheck');
                    self.attr('data-action', -1);
                    self.attr('title', title);
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        removeFavorite: function (me, param, title) {
            var url = AFFILIATE.getAbsoluteUrl("Affiliate/RemoveFavorite");
            var self = $(me);
            $.ajax({
                url: url,
                type: 'post',
                data: param,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    if (!data.Success) {
                        alert(data.ErrorMessage);
                        return;
                    }

                    self.addClass('favor-uncheck').removeClass('favor-check');
                    self.attr('data-action', 1);
                    self.attr('title', title);
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
    AffiliateList.init();
});