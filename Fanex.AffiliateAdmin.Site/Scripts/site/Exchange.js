var EXCHANGE = function ($) {
    var _gridOption = {};
    var CHECKBOX_COLUMN = 0;

    var editExchangeRateFormatter = function (rowIndex, rowData, cell, cellFormated) {
        var className = cell.Name == rowData.Code ? "c" : "linkpopup";
        return "<div class='" + className + "' data-exchangeid='" + rowData.ExchangeID +
                "' data-codefrom='" + rowData.Code +
                "' data-codeto='" + cell.Name +
                "' data-rate='" + cellFormated + "'>" + cellFormated + "</div>";
    };

    var viewExchangeRateFormatter = function (rowIndex, rowData, cell, cellFormated) {
        return "<div>" + cellFormated + "</div>";
    };

    var formatNumber = "0,0.00000";

    function initExRateColumns(data) {
        var currencies = data.Currencies;
        var isToday = data.IsToday

        for (var i = 0; i < currencies.length; i++) {
            var currency = currencies[i];

            var exRateColumn = {
                HeaderText: "{0} {1}".format(currency, MSG.Rate),
                Width: 120,
                Code: currency,
                Name: currency,
                HeaderAlign: "center",
                CellAlign: "center",
                Formatter: isToday ? editExchangeRateFormatter : viewExchangeRateFormatter,
                FormatNumber: formatNumber
            };

            var marketExRateColumn = {
                HeaderText: "{0} {1}".format(currency, MSG.MarketRate),
                Width: 120,
                Code: currency,
                Name: "{0}_Market".format(currency),
                HeaderAlign: "center",
                CellAlign: "center",
                FormatNumber: formatNumber
            };

            _gridOption.columns.splice(_gridOption.columns.length - 1, 0, exRateColumn, marketExRateColumn);
        }
    }

    function removeAutoUpdateColumn(data) {
        var isToday = data.IsToday;

        if (!isToday) {
            _gridOption.columns.splice(CHECKBOX_COLUMN, 1);
            _gridOption.columns.splice(_gridOption.columns.length - 1, 1);
            _gridOption.numberOfFrozenColumn = 2;
            _gridOption.indexNumberColumn = 0;
        }
    }

    return {
        init: function () {
            $("#gridId").resize(function () {
                $(".search-panel").width($("#gridId").width());
            });

            if ($("#gridId").length > 0) {
                EXCHANGE.ReRenderAfterPaged();

                $('#btnSubmit').click(function () {
                    EXCHANGE.ReRenderAfterPaged();
                });
            }

            $('.form-popup').submit(function () {
                AFFILIATE.trimBeforeSubmit();
            });

            if ($('#rate').length > 0) {
                $("#rate").keyup(function () {
                }).ForceDecimalOnly();
            }

            $("#gridId").parent().on("click", "#btnCopy", function () {

                var listChk = $("#gridId table input.check:checked");
                if (listChk.length == 0) {
                    alert(MSG.SelRate);
                    return;
                }
                if (!confirm(MSG.ConfirmCopyRate))
                    return;
                var listID = "";
                for (var j = 0; j < listChk.length; j++) {
                    var mid = $(listChk[j]).attr("data-mid");

                    if (mid.isNumber()) {
                        listID += mid + ",";
                    }
                }

                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("Exchange/CopyRate"),
                    type: 'post',
                    data: {
                        exchangeidlist: listID
                    },
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);

                        if (!data.Success) {
                            alert(data.Message);
                        } else
                            EXCHANGE.ReRenderAfterPaged();
                    }
                });
            });

            $("#gridId").on("click", "div.linkpopup", function () {
                var $this = $(this);
                var url = AFFILIATE.getAbsoluteUrl("Exchange/UpdateExRate?");
                AFFILIATE.openDialogUrl(url + "exchangeid=" + $this.attr("data-exchangeid") +
                    "&codefrom=" + $this.attr("data-codefrom") + "&codeto=" + $this.attr("data-codeto") +
                    "&rate=" + parseFloat($this.attr("data-rate").replace(/,/g, ''), 10), "Ex.Rate", { width: 450, height: 300 });
            });
        },

        toggleAutoUpdate: function (self) {

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Exchange/ToggleAutoUpdate"),
                type: 'post',
                data: {
                    exchangeid: $(self).attr("data-id")
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    if (!data.Success) {
                        alert(data.Message);
                        self.checked = !self.checked;
                    }
                }
            });
        },
        checkAll: function (value) {
            $('#gridId input[type=checkbox].check').prop('checked', value);
        },
        ReRenderAfterPaged: function () {
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Exchange/GetList"),
                type: 'post',
                data: {
                    Time: $('#ExchangeRateForm #Time').datepicker("getDate").toShortDate()
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);



                    if (data.Notification) {
                        $("#notification-header").show();
                        $("#notification").html(data.Notification);
                    } else
                        $("#notification-header").hide();

                    _gridOption = $.extend(true, {}, sampleGridOption);

                    initExRateColumns(data);

                    removeAutoUpdateColumn(data);

                    _gridOption.pagerOption = {
                        itemsPerPage: 100,
                        itemsPerPageArray: [100],
                        currentPage: 0,
                        totalItem: data.List.length,
                        pagerSelectCallBack: EXCHANGE.ReRenderAfterPaged
                    };

                    _gridOption.titleInfo.text = MSG.Title + ' [{0}]'.format(data.List.length);

                    _gridOption.bodyRows = AFFILIATE.genGridRows(data.List, _gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(_gridOption);

                    if (data.List.length == 0) {
                        $('#gridId').addClass("noinfo").html(MSG.NoInfo);
                        return;
                    }
                },
                complete: function () {
                    AFFILIATE.setWidthOfFormSearch();
                }
            });
        }
    };
} (jQuery);

$(function () {
    EXCHANGE.init();
});
