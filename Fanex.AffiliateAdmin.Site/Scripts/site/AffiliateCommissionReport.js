/*global AFFILIATE,samplePShareCol,samplePWinCol,sampleNetGamingCol*/
var AffiliateCommissionReport = (function ($) {

    "use strict";

    var submitOptions = {};
    var SYS_CURRENCY_OPTION = "1";
    var gridOption = {};
    var MONTH_COL = 2;
    var GROUP_BY_AFFILIATE = "0";

    var getSelectedProducts = function (products) {
        var selectedProducts = $.map(submitOptions.Products.split(','), function (item) {
            return parseInt(item, 10);
        });

        return $.grep(products, function (item) {
            return $.inArray(item.Id, selectedProducts) >= 0 ? true : false;
        });
    };

    var getMergeColNumberForProductColumn = function () {
        var mergeColNumber = 4;
        return mergeColNumber;
    };

    var removeRedundantColumns = function () {
        if (submitOptions.GroupBy === GROUP_BY_AFFILIATE) {
            gridOption.groupHeaderRows[0].Columns.splice(MONTH_COL, 2);
            gridOption.columns.splice(MONTH_COL, 2);
        }
    };

    var getNumberOfFrozenColumn = function () {
        if (submitOptions.GroupBy === GROUP_BY_AFFILIATE) {
            return 3;
        }

        return 5;
    };

    var initProductColumnsForGridOption = function (products) {
        var productCount = products.length;
        var extraThreeColForPlayTech = 3;

        for (var i = 0; i < productCount; i++) {
            var name = nomalizeProductName(products[i].Name);
            var isPlaytechCasino = products[i].Name === "Playtech Casino";
            var mergeColNumber = getMergeColNumberForProductColumn();
            mergeColNumber = isPlaytechCasino ? mergeColNumber + extraThreeColForPlayTech : mergeColNumber;

            gridOption.groupHeaderRows[0].Columns.push(
                { HeaderCaption: window.MSG[name], HeaderText: window.MSG[name], IsCurrency: true,
                    Name: name + "Winloss", NameCache: name + "Winloss", MergeColNumber: mergeColNumber
                }
            );

            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: window.MSG.NetWinloss, HeaderText: window.MSG.NetWinloss,
                    Width: 100, Name: name + "Winloss", NameCache: name + "Winloss", HeaderAlign: "center",
                    CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true
                }
            );

            /*jshint ignore:start */
            gridOption.columns.push(
                { IsCurrency: false,
                    HeaderCaption: "<span title='{0}'>{1}</span>".format(window.MSG.QualifiedMembers, window.MSG.QM),
                    HeaderText: "<span title='{0}'>{1}</span>".format(window.MSG.QualifiedMembers, window.MSG.QM),
                    Width: 80, Name: name + "QM", HeaderAlign: "center", CellAlign: "right",
                    Formatter: function (rIndex, record, cell, cellFormated) {
                        var actualBaseCurreny = "2";
                        var currency = record.Currency;

                        if (cellFormated) {
                            return "<a class='qm' data-currency='{0}' data-affname='{1}' data-affid='{2}' data-date='{3}' data-basecurrency='{4}' href='javascript:void(0)' data-products='{5}' data-groupid='{6}' data-commgroup='{7}'>{8}</a>"
                                    .format(currency, record.Username, record.AffId, moment(record.WinlostDate).format("MMM YYYY"), actualBaseCurreny, submitOptions.Products, record.GroupID, record.GroupName, cellFormated);
                        }

                        return 0;
                    }
                }
            );
            /*jshint ignore:end */

            gridOption.columns.push(
                { IsCurrency: false, HeaderCaption: window.MSG.CommPercent, HeaderText: window.MSG.CommPercent,
                    Width: 80, Name: name + "CommissionPercent", HeaderAlign: "center", CellAlign: "right",
                    FormatNumber: AFFILIATE.FORMATNUMBER
                }
            );

            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: window.MSG.Commission, HeaderText: window.MSG.Commission,
                    Width: 100, Name: name + "Commission", NameCache: name + "Commission", HeaderAlign: "center",
                    CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true
                }
            );

            if (isPlaytechCasino) {
                gridOption.columns.push(samplePShareCol);
                gridOption.columns.push(samplePWinCol);
                gridOption.columns.push(sampleNetGamingCol);
            }
        }
    };

    function customizeGridOption() {

        gridOption = $.extend(true, {}, window.sampleGridOption);

        var products = getSelectedProducts(window.PRODUCTS);

        removeRedundantColumns();

        gridOption.numberOfFrozenColumn = getNumberOfFrozenColumn();

        initProductColumnsForGridOption(products);

        gridOption.groupHeaderRows[0].Columns.push(window.totalPaymentHeader);

        gridOption.columns.push(window.totalPaymentCol);
    }

    var renameCurrencyColumns = function (isUseBaseCurrency) {
        var i;
        var col;
        var name;

        var freezeColumnCount = getNumberOfFrozenColumn();

        // for Normal columns
        for (i = freezeColumnCount; i < gridOption.columns.length; i++) {
            col = gridOption.columns[i];
            if (col.NameCache) {
                name = col.NameCache;
                if (isUseBaseCurrency) {
                    name += "Base";
                }

                col.Name = name;
            }
        }

        // for Header columns
        for (i = freezeColumnCount; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.NameCache) {
                name = col.NameCache;
                if (isUseBaseCurrency) {
                    name += "Base";
                }
                col.Name = name;
            }
        }
    };

    var changeHeaderCaptions = function (showCurrencySymbol) {
        var text = showCurrencySymbol ? " (" + window.System.Prefix + ")" : "";
        var i;
        var col;

        for (i = 0; i < gridOption.columns.length; i++) {
            col = gridOption.columns[i];
            if (col.IsCurrency) {
                col.HeaderText = col.HeaderCaption + text;
            }
        }

        for (i = 0; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.IsCurrency) {
                col.HeaderText = col.HeaderCaption + text;
            }
        }
    };

    var nomalizeProductName = function (name) {
        return name.replace(/ /g, "");
    };

    return {
        init: function () {

            $('.search-panel form').submit(function () {
                AffiliateCommissionReport.submitRequest();
                return false;
            });

            AFFILIATE.initMonthPicker();

            if ($('#gridId').length > 0) {
                AffiliateCommissionReport.submitRequest();
            }

            var mainContentObject = $('.main-content');

            mainContentObject.on("click", ".icon-edit", function () {
                var parent = $(this).parents(".amount-region").get(0);
                var $parent = $(parent);
                $parent.find(".amount-info").hide();
                $parent.find(".amount-edit").show();
                $parent.find("input").ForceDecimalOnly();
            });

            mainContentObject.on("click", ".icon-remove", function () {
                var parent = $(this).parents(".amount-region").get(0);
                var $parent = $(parent);
                $parent.find(".amount-info").show();
                $parent.find(".amount-edit").hide();
            });

            /*jshint ignore:start */
            mainContentObject.on("click", "a.qm", function () {
                var param = {
                    affid: $(this).data("affid"),
                    monthdate: $(this).data("date"),
                    basecurrency: $(this).data("basecurrency"),
                    currency: $(this).data("currency"),
                    products: $(this).data("products"),
                    groupid: $(this).data("groupid")
                };

                var title = MSG.AffQualifiedMemReportDetailFor.format($(this).data("affname"), param.monthdate, $(this).data("currency"), $(this).data("commgroup"));
                parent.AFFILIATE.openDialogFrame("Report/QualifiedMembers?" + $.param(param), title, { width: 900, height: 450 });
            });
            /*jshint ignore:end */

            mainContentObject.on("click", "a.commgroup", function () {
                var param = {
                    groupid: $(this).data('groupid'),
                    date: $(this).data('date')
                };
                AFFILIATE.openDialogFrame("CommGroup/ViewSetting?"
                    + $.param(param), window.MSG.CommGroupSettingFor.replace("{0}", '"'
                    + $(this).html() + '" ' + $(this).data("date")), { width: 1000, height: 450 });
            });
        },

        checkAtLeastOneSelectedProduct: function () {
            var selectedProducts = $('#Products').val();

            if (selectedProducts === null || selectedProducts === undefined || selectedProducts.length === 0) {
                return false;
            }

            return true;
        },

        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();

            if (!AffiliateCommissionReport.checkAtLeastOneSelectedProduct()) {
                alert(window.MSG.ChooseAtLeastOneProduct);
                return;
            }

            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
            AffiliateCommissionReport.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Report/AffiliateCommission"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageCurrent=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    var gridObject = $('#gridId');
                    gridObject.show();

                    customizeGridOption(data.HasCashbackDeduction);

                    var baseCurrency = $("#BaseCurrency").val();

                    renameCurrencyColumns(baseCurrency === SYS_CURRENCY_OPTION);

                    changeHeaderCaptions(baseCurrency === SYS_CURRENCY_OPTION);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: AffiliateCommissionReport.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.AffiliateCommissionTitle + ' [{0}]'.format(data.TotalItem);

                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    gridObject.removeClass("noinfo").fanexGrid(gridOption);

                    if (data.List.length === 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', window.Lang.NoDataInfo);
                        return;
                    } else {
                        $('.status').show();
                    }

                    AFFILIATE.updateSearchPanel(gridObject);
                },
                error: function () {
                    alert("Occur the error when requesting processing commission report :(");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },

        showCommGroup: function (groupId) {
            var param = {
                groupid: groupId
            };
            parent.AFFILIATE.openDialogFrame(AFFILIATE.getAbsoluteUrl("CommGroup/ViewSetting?")
                + $.param(param), window.MSG.CommissionGroup, { width: 1000, height: 450 });
        }
    };
})(jQuery);

$(function () {
    "use strict";

    AffiliateCommissionReport.init();
});
