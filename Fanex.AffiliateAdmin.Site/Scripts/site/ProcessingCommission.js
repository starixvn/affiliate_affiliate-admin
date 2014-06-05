/*global AFFILIATE*/
var ProcessingCommission = (function ($) {
    "use strict";

    var submitOptions = {};
    var SYS_CURRENCY_OPTION = "1";
    var _freezeColumnCount = 5;
    var gridOption = {};

    var getSelectedProducts = function (products) {
        var selectedProducts = $.map(submitOptions.Products.split(','), function (item) {
            return parseInt(item, 10);
        });

        return $.grep(products, function (item) {
            return $.inArray(item.Id, selectedProducts) >= 0 ? true : false;
        });
    };

    function customizeGridOption(hasCashbackDeduction) {

        gridOption = $.extend(true, {}, window.sampleGridOption);
       
        var products = getSelectedProducts(window.PRODUCTS);

        var productCount = products.length;

        for (var i = 0; i < productCount; i++) {
            var name = nomalizeProductName(products[i].Name);

            gridOption.groupHeaderRows[0].Columns.push(
                { HeaderCaption: window.MSG[name], HeaderText: window.MSG[name], IsCurrency: true, 
                    Name: name + "Winloss", NameCache: name + "Winloss", MergeColNumber: 4 }
            );

            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: window.MSG.Winloss, HeaderText: window.MSG.Winloss,
                    Width: 100, Name: name + "Winloss", NameCache: name + "Winloss", HeaderAlign: "center", 
                    CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );
            
            /*jshint ignore:start */
            gridOption.columns.push(
                { IsCurrency: false, HeaderCaption: window.MSG.QM, HeaderText: window.MSG.QM, 
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
                    FormatNumber: AFFILIATE.FORMATNUMBER }
            );

            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: window.MSG.Commission, HeaderText: window.MSG.Commission,
                    Width: 100, Name: name + "Commission", NameCache: name + "Commission", HeaderAlign: "center", 
                    CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );
        }

        addCashbackColumns(hasCashbackDeduction);

        gridOption.groupHeaderRows[0].Columns.push(window.totalPaymentHeader);

        gridOption.columns.push(window.totalPaymentCol);
    }

    function addCashbackColumns(hasCashbackDeduction) {

        if (hasCashbackDeduction) {

            gridOption.groupHeaderRows[0].Columns.push(
                { HeaderCaption: window.MSG.Cashback, HeaderText: window.MSG.Cashback, IsCurrency: true, Name: "CashbackPercent", MergeColNumber: 2 }
            );

            gridOption.columns.push(
                { IsCurrency: false, HeaderCaption: window.MSG.DeductionPercent, HeaderText: window.MSG.DeductionPercent,
                    Width: 80, Name: "CashbackPercent", HeaderAlign: "center", CellAlign: "right", 
                    FormatNumber: AFFILIATE.FORMATNUMBER }
            );

            /*jshint ignore:start */
            gridOption.columns.push(
                {
                    IsCurrency: true, HeaderCaption: window.MSG.Amount, HeaderText: window.MSG.Amount,
                    Width: 100, Name: "CashbackAmount", NameCache: "CashbackAmount", HeaderAlign: "center", 
                    CellAlign: "left", FormatNumber: AFFILIATE.FORMATNUMBER,
                    Formatter: function (rIndex, record) {
                        var currentValue = submitOptions.BaseCurrency === SYS_CURRENCY_OPTION ? record.CashbackAmountBase : record.CashbackAmount;

                        if (isNaN(currentValue) || currentValue === '' || currentValue === null) {
                            return '';
                        }

                        if (currentValue < 0) {
                            currentValue = -1 * currentValue;
                        }

                        currentValue = numeral(currentValue).format(AFFILIATE.FORMATNUMBER);


                        if (record.CashbackAmount.Amount < 0) {
                            currentValue = '<span class="format-negative">' + "($)".replace('$', currentValue) + '</span>';
                        }

                        var winlostdate = window.moment(record.WinlostDate).format(AFFILIATE.DATE_FORMAT);

                        if (!record.EnableCashback) {
                            return '<div style="text-align: right;">' + currentValue + '</div>';
                        }

                        
                        var html = '<div class="amount-region"><div class="amount-info" style="text-align: right;">' + currentValue + '&nbsp;&nbsp; <i class="icon-edit"></i></div>';
                        html += '<div class="amount-edit" style="display: none;"><input type="text" value="{0}" data-affid="{1}" data-winlostdate="{2}"  style="display:inline-block; width: 40px; margin-right: 5px;"><i class="icon-ok"></i>&nbsp;&nbsp;<i class="icon-remove"></i><div class="edit-amount">'.format(record.CashbackAmount, record.AffId, winlostdate);
                        html += '</div>';

                        return html;
                        
                    }
                }
            );
            /*jshint ignore:end */
        }
    }

    var renameCurrencyColumns = function (isUseBaseCurrency) {
        var i;
        var col;
        var name;

        // for Normal columns
        for (i = _freezeColumnCount; i < gridOption.columns.length; i++) {
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
        for (i = _freezeColumnCount; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
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
                ProcessingCommission.submitRequest();
                return false;
            });

            if ($('#gridId').length > 0) {
                ProcessingCommission.submitRequest();
            }

            var mainContentObject = $('.main-content');

            mainContentObject.on("click", ".icon-edit", function () {
                var parent = $(this).parents(".amount-region").get(0);
                var $parent = $(parent);
                $parent.find(".amount-info").hide();
                $parent.find(".amount-edit").show();
                $parent.find("input").ForceDecimalOnly();
            });

            mainContentObject.on("click", ".icon-ok", function () {
                var parent = $(this).parents(".amount-region").get(0);
                var $parent = $(parent);
                ProcessingCommission.UpdateCashBack($parent.find("input"));
            });

            mainContentObject.on("click", ".icon-remove", function () {
                var parent = $(this).parents(".amount-region").get(0);
                var $parent = $(parent);
                $parent.find(".amount-info").show();
                $parent.find(".amount-edit").hide();
            });

            mainContentObject.on("click", "#transferAll", function () {
                ProcessingCommission.MonthlyTransfer();
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

            if (!ProcessingCommission.checkAtLeastOneSelectedProduct()) {
                alert(window.MSG.ChooseAtLeastOneProduct);
                return;
            }

            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
            ProcessingCommission.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Payment/ProcessingCommission"),
                type: 'post',
                data: jQuery.param(submitOptions) + "&pageIndex=" + (currentPage + 1) + "&pageSize=" + itemsPerPage,
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
                        pagerSelectCallBack: ProcessingCommission.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = window.MSG.ProcessingCommissionTitle + ' [{0}]'.format(data.TotalItem);

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
                complete: function () {
                    $.unblockUI();
                }
            });
        },

        UpdateCashBack: function (input) {
            var self = $(input);

            var data = {
                AffId: self.data("affid"),
                MonthDate: self.data("winlostdate"),
                Amount: self.val()
            };

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Payment/UpdateCashBack"),
                type: 'post',
                data: $.param(data),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    if (data.Success) {
                        ProcessingCommission.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                    } 
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        MonthlyTransfer: function () {
            if (confirm("Do you want to transfer all?")) {
                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("Payment/MonthlyTransfer"),
                    type: 'post',
                    beforeSend: function () {
                        $.blockUI();
                    },
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);

                        if (data.Success) {
                            ProcessingCommission.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                        } else {
                            alert(data.Message);
                        }
                    },
                    error: function () {
                        alert(window.MSG.OccurMonthlyTransferError);
                    },
                    complete: function () {
                        $.unblockUI();
                    }
                });
            }
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
    
    ProcessingCommission.init();
});
