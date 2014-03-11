var ProcessingCommission = function ($) {
    var submitOptions = {};

    var SYS_CURRENCY_OPTION = 1;
    var _freezeColumnCount = 5;

    var gridOption = {};

    function customizeGridOption() {
        for (var i = 0; i < PRODUCTS.length; i++) {
            var name = nomalizeProductName(PRODUCTS[i].Name);

            gridOption.groupHeaderRows[0].Columns.push(
                { HeaderCaption: MSG[name], HeaderText: MSG[name], IsCurrency: true, Name: name + "Winloss", NameCache: name + "Winloss", MergeColNumber: 4 }
            );

            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Winloss, HeaderText: MSG.Winloss, Width: 80, Name: name + "Winloss", NameCache: name + "Winloss", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );
            gridOption.columns.push(
                { IsCurrency: false, HeaderCaption: MSG.QM, HeaderText: MSG.QM, Width: 80, Name: name + "QM", HeaderAlign: "center", CellAlign: "right",
                    Formatter: function (rIndex, record, cell, cellFormated) {
                        var actualBaseCurreny = $('#BaseCurrency').val() == "0" ? "2" : "1";
                        var currency = actualBaseCurreny == "1" ? System.Currency : record.Currency;

                        if (cellFormated)
                            return "<a class='qm' data-currency='{0}' data-affname='{1}' data-affid='{2}' data-date='{3}' data-basecurrency='{4}' href='javascript:void(0)'>{5}</a>".format(currency, record.Username, record.AffId, moment(record.WinlostDate).format("MMM YYYY"), actualBaseCurreny, cellFormated);
                        return 0;
                    }
                }
            );
            gridOption.columns.push(
                { IsCurrency: false, HeaderCaption: MSG.CommPercent, HeaderText: MSG.CommPercent, Width: 80, Name: name + "CommissionPercent", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER }
            );

            gridOption.columns.push(
                { IsCurrency: true, HeaderCaption: MSG.Commission, HeaderText: MSG.Commission, Width: 80, Name: name + "Commission", NameCache: name + "Commission", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER, FormatNegative: '($)', IsAbs: true }
            );
        }

        gridOption.groupHeaderRows[0].Columns.push(
            { HeaderCaption: MSG.Cashback, HeaderText: MSG.Cashback, IsCurrency: true, Name: "CashbackPercent", MergeColNumber: 2 }
        );

        gridOption.columns.push(
            { IsCurrency: false, HeaderCaption: MSG.DeductionPercent, HeaderText: MSG.DeductionPercent, Width: 80, Name: "CashbackPercent", HeaderAlign: "center", CellAlign: "right", FormatNumber: AFFILIATE.FORMATNUMBER }
        );

        gridOption.columns.push(
            {
                IsCurrency: true, HeaderCaption: MSG.Amount, HeaderText: MSG.Amount, Width: 100, Name: "CashbackAmount", NameCache: "CashbackAmount", HeaderAlign: "center", CellAlign: "left", FormatNumber: AFFILIATE.FORMATNUMBER,
                Formatter: function (rIndex, record, cell, cellFormated) {
                    var currentValue = record.CashbackAmount;

                    if (isNaN(currentValue) || currentValue === '' || currentValue === null)
                        return '';
                    
                    if (currentValue < 0) {
                        currentValue = -1 * currentValue;
                    }

                    currentValue = numeral(currentValue).format(AFFILIATE.FORMATNUMBER);
                    

                    if (record.CashbackAmount.Amount < 0) {
                        currentValue = '<span class="format-negative">' + "($)".replace('$', currentValue) + '</span>';
                    }

                    var winlostdate = moment(record.WinlostDate).format(AFFILIATE.DATE_FORMAT);

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

        gridOption.groupHeaderRows[0].Columns.push(totalPaymentHeader);
        gridOption.columns.push(totalPaymentCol);
    }

    function renameCurrencyColumns(isUseBaseCurrency) {
        for (var i = _freezeColumnCount; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            if (col.NameCache) {
                var name = col.NameCache;
                if (isUseBaseCurrency)
                    name += "Base";
                col.Name = name;
            }
        }

        for (var i = _freezeColumnCount; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            var col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.NameCache) {
                var name = col.NameCache;
                if (isUseBaseCurrency)
                    name += "Base";
                col.Name = name;
            }
        }
    }

    function changeHeaderCaptions(showCurrencySymbol) {
        var text = showCurrencySymbol ? " (" + System.Prefix + ")" : "";
        for (var i = 0; i < gridOption.columns.length; i++) {
            var col = gridOption.columns[i];
            if (col.IsCurrency)
                col.HeaderText = col.HeaderCaption + text;
        }

        for (var i = 0; i < gridOption.groupHeaderRows[0].Columns.length; i++) {
            var col = gridOption.groupHeaderRows[0].Columns[i];
            if (col.IsCurrency)
                col.HeaderText = col.HeaderCaption + text;
        }
    }

    function nomalizeProductName(name) {
        return name.replace(/ /g, "");
    }

    return {
        init: function () {
            $('.search-panel form').submit(function () {
                ProcessingCommission.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                return false;
            });

            if ($('#gridId').length > 0) {
                ProcessingCommission.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
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

            mainContentObject.on("click", "a.qm", function () {
                var param = {
                    affid: $(this).data("affid"),
                    monthdate: $(this).data("date"),
                    basecurrency: $(this).data("basecurrency"),
                    currency: $(this).data("currency")
                };
                var title = MSG.AffQualifiedMemReportDetailFor.replace("{0}", $(this).data("affname"));
                title = title.replace("{1}", param.monthdate) + " - " + MSG.Currency + ": " + $(this).data("currency");
                parent.AFFILIATE.openDialogFrame("Report/QualifiedMembers?" + jQuery.param(param), title, { width: 900, height: 450 });
            });

            mainContentObject.on("click", "a.commgroup", function () {
                var param = {
                    groupid: $(this).data('groupid'),
                    date: $(this).data('date')
                };
                AFFILIATE.openDialogFrame("CommGroup/ViewSetting?" + jQuery.param(param), MSG.CommGroupSettingFor.replace("{0}", '"' + $(this).html() + '" ' + $(this).data("date")), { width: 1000, height: 450 });
            });
        },

        ReRenderAfterPaged: function (currentPage, itemsPerPage) {

            AFFILIATE.trimBeforeSubmit();
            submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');

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

                    gridOption = $.extend(true, {}, sampleGridOption);

                    customizeGridOption();

                    var baseCurrency = $("#BaseCurrency").val();
                    renameCurrencyColumns(baseCurrency == SYS_CURRENCY_OPTION);
                    changeHeaderCaptions(baseCurrency == SYS_CURRENCY_OPTION);

                    gridOption.pagerOption = {
                        itemsPerPage: data.ItemsPerPage,
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: data.CurrentPage,
                        totalItem: data.TotalItem,
                        pagerSelectCallBack: ProcessingCommission.ReRenderAfterPaged
                    };

                    gridOption.titleInfo.text = MSG.ProcessingCommissionTitle + ' [{0}]'.format(data.TotalItem);

                    gridOption.bodyRows = AFFILIATE.genGridRows(data.List, gridOption);

                    gridObject.removeClass("noinfo").fanexGrid(gridOption);

                    if (data.List.length == 0) {
                        $('.status').hide();
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
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
                data: jQuery.param(data),
                beforeSend: function () {
                    $.blockUI();
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);
                    
                    if (data.Success) {
                        ProcessingCommission.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
                    } else {
                    }
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },
        MonthlyTransfer: function (input) {
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
            parent.AFFILIATE.openDialogFrame(AFFILIATE.getAbsoluteUrl("CommGroup/ViewSetting?") + jQuery.param(param), MSG.CommissionGroup, { width: 1000, height: 450 });
        }
    };
} (jQuery);

$(function () {
    ProcessingCommission.init();
});
