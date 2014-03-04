var EXCHANGE = function ($) {
    var _gridOption = {};

    var formatter = function (rowIndex, rowData, cell, cellFormated) {
        var className = cell.Name == rowData.Code ? "c" : "linkpopup";
        return "<div class='" + className + "' data-exchangeid='" + rowData.ExchangeID +
                "' data-codefrom='" + rowData.Code +
                "' data-codeto='" + cell.Name +
                "' data-rate='" + cellFormated + "'>" + cellFormated + "</div>";
    };

    var formatNumber = "0,0.00000";

    function initExRateColumns(data) {
        var currencies = data.Currencies;

        for (var i = 0; i < currencies.length; i++) {
            var currency = currencies[i];

            var exRateColumn = {
                HeaderText: "{0} {1}".format(currency, MSG.Rate),
                Width: 80,
                Code: currency,
                Name: currency,
                HeaderAlign: "center",
                CellAlign: "center",
                Formatter: formatter,
                FormatNumber: formatNumber
            };

            var marketExRateColumn = {
                HeaderText: "{0} {1}".format(currency, MSG.MarketRate),
                Width: 80,
                Code: currency,
                Name: "{0}_Market".format(currency),
                HeaderAlign: "center",
                CellAlign: "center",
                FormatNumber: formatNumber
            };

            _gridOption.columns.splice(_gridOption.columns.length - 1, 0, exRateColumn, marketExRateColumn);
        }
    }

    return {
        init: function () {
            $("#gridId").resize(function () {
                $(".search-panel").width($("#gridId").width());
            });

            AFFILIATE.RefreshGridData = EXCHANGE.ReRenderAfterPaged;
            EXCHANGE.ReRenderAfterPaged();
            $('#btnSubmit').click(function () {
                EXCHANGE.ReRenderAfterPaged();
            });

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
            $("#gridId").on("change", "input.autoupdate", function () {
                var self = this;
                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("Exchange/ToggleAutoUpdate"),
                    type: 'post',
                    data: {
                        exchangeid: $(this).attr("data-id")
                    },
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);

                        if (!data.Success) {
                            alert(data.Message);
                            self.checked = !self.checked;
                        }
                    }
                });
            });

            $("#gridId").on("click", "div.linkpopup", function () {
                var $this = $(this);
                var url = AFFILIATE.getAbsoluteUrl("Exchange/UpdateExRate?");
                AFFILIATE.openDialogUrl(url + "exchangeid=" + $this.attr("data-exchangeid") +
                    "&codefrom=" + $this.attr("data-codefrom") + "&codeto=" + $this.attr("data-codeto") +
                    "&rate=" + parseFloat($this.attr("data-rate")), "Ex.Rate", { width: 400, height: 240 });
            });
        },
        checkAll: function (value) {
            $('#gridId input[type=checkbox].check').prop('checked', value);
        },
        ReRenderAfterPaged: function () {
            // resize height of datagrid


            //_gridOption = AFFILIATE.resizeHeightDataGrid(_gridOption);

            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("Exchange/GetList"),
                type: 'post',
                data: {
                    Time: $('#SearchForm #Time').datepicker("getDate").toShortDate()
                },
                success: function (data) {
                    AFFILIATE.PreHandleAjaxResult(data);

                    if (data.List.length == 0) {
                        $('#gridId').addClass("noinfo").html(MSG.NoInfo);
                        return;
                    }

                    if (data.Notification) {
                        $("#notification-header").show();
                        $("#notification").html(data.Notification);
                    } else
                        $("#notification-header").hide();

                    _gridOption = $.extend(true, {}, sampleGridOption);

                    initExRateColumns(data);

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
                },
                complete: function () {
                    AFFILIATE.setWidthOfFormSearch();
                }
            });
        }
    };
} (jQuery);

/////////////////////////////////////////////////////////////////////////////////
var POPUP = function ($) {
    return {
        init: function () {
            $("#btnSubmit").click(function () {
                
                $.ajax({ url: AFFILIATE.getAbsoluteUrl("Exchange/UpdateExRate"),
                    type: 'post',
                    data: $("form input").serializeObject(),
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);
                        
                        if (!data.Success) {
                            alert(data.Message);
                        } else {
                            if (parent.AFFILIATE.currentOpenDialog)
                                parent.AFFILIATE.currentOpenDialog.dialog("close");
                            if (window.self !== window.top && parent.EXCHANGE)
                                parent.EXCHANGE.ReRenderAfterPaged();
                        }
                    }
                });
            });
        }
    }
} (jQuery);

