/*global AFFILIATE,MAXAM,MAXQM,MSG,numeral*/
var CommGroupPopup = (function ($) {
    "use strict";

    var MaxTier = 50;

    function formatInt(value) {
        return numeral(value).format(AFFILIATE.FORMAT_INT_NUMBER);
    }

    function newTierRow(tier, minAM, maxAM, minQM, maxQM, commission) {
        var $tr = $("#sampleTierRow tr").clone();
        $tr.find(".tierOrder").html(tier);
        $tr.find(".minAM").val(formatInt(minAM || 0));
        $tr.find(".maxAM").val(formatInt(maxAM || 0));
        $tr.find(".minQM").val(formatInt(minQM || 0));
        $tr.find(".maxQM").val(formatInt(maxQM || 0));

        var dropdown = $("#sampleCommissionList select").clone();
        dropdown.val(commission || "0.00");
        $tr.find(".commissionCol").html(dropdown);

        return $tr[0];
    }

    function addTier(productID) {
        var $table = $("#tab_content" + productID + " #tblTier" + productID + ">tbody");
        var noTier = $table.find("tr.tier").length;

        if (noTier >= MaxTier) {
            var productname = $("#tab_header" + productID).data("productname");
            alert(MSG.InvalidSettingFor.replace("{item}", productname) + " " + MSG.MaxTierAllow);
            return;
        }

        var minAM, maxAM, minQM, maxQM, commission, newTierID;

        if ($table.find("tr.tier").length === 0) { // noinfo row
            minAM = maxAM = minQM = maxQM = 0;
            commission = "0.00";
            newTierID = 1;
            $table.find("tr:first").hide();
        }
        else {
            var $lastRow = $table.find("tr:last");
            $lastRow.find("input[type=checkbox]").prop("disabled", true);
            minAM = parseInt($lastRow.find(".maxAM").val().replace(/,/g, "")) + 1;
            maxAM = minAM;
            minQM = parseInt($lastRow.find(".maxQM").val().replace(/,/g, "")) + 1;
            maxQM = minQM;

            if (minAM >= MAXAM - 1 || minQM >= MAXQM - 1) {
                $("#tab_content" + productID).find("button:first").prop("disabled", true);
            }
            commission = $lastRow.find(".commission").val();
            newTierID = noTier + 1;
        }
        $table.find(".icon-remove:last").addClass("icon-white").prop("disabled", true);
        $table[0].appendChild(newTierRow(newTierID, minAM, maxAM, minQM, maxQM, commission));
    }

    function disableProduct(productId) {
        var $tab = $("#tab_header" + productId);

        var productname = $tab.data("productname");

        if ($tab.find("#" + productId).prop("checked")) {
            alert("Please uncheck copy all tier settings on the " + productname + " product");
            return false;
        }

        var msg = window.MSG.ConfirmRemoveFromGroup.replace("{item}", productname);

        if (!confirm(msg)) {
            return false;
        }

        $("#tab_content" + productId).hide();

        $tab.find("#" + productId).prop("disabled", true);

        $tab.data("disabled", true);

        $tab.find("#product_header" + productId).removeClass("enabled").addClass("disabled");

        $tab.find("#header_icon" + productId).removeClass("icon-ok-circle").addClass("icon-ban-circle");

        return true;
    }

    function enableProduct(productId) {
        var $tab = $("#tab_header" + productId);

        $("#tab_content" + productId).show();

        $tab.find("#" + productId).prop("disabled", false);

        $tab.data("disabled", false);

        $tab.find("#product_header" + productId).removeClass("disabled").addClass("enabled");

        $tab.find("#header_icon" + productId).removeClass("icon-ban-circle").addClass("icon-ok-circle");
    }

    function copyTierSettings(productId) {
        var $tab = $("#tab_header" + productId);

        if ($tab.data("disabled")) {
            return;
        }

        $("#fieldsetTier" + productId).hide();

        $tab.find("#" + productId).prop("disabled", true);

        $tab.data("copied", true);

        $tab.find("#product_header" + productId).removeClass("enabled").addClass("copied");
    }

    function uncopyTierSettings(productId) {
        var $tab = $("#tab_header" + productId);

        $tab.data("copied", false);

        if (!$tab.data("disabled")) {

            $("#fieldsetTier" + productId).show();

            $tab.find("#" + productId).prop("disabled", false);

            $tab.find("#product_header" + productId).removeClass("copied").addClass("enabled");
        }
    }

    function disableCopyCheckboxes() {
        var enabledIds = [];
        var i;
        for (i = 0; i < window.PRODUCTS.length; i++) {
            var productid = window.PRODUCTS[i];
            var $tab = $("#tab_header" + productid);
            if (!$tab.data("disabled")) {
                enabledIds.push(productid);
            }
        }

        if (enabledIds.length === 1) {
            $("#" + enabledIds[0]).prop("disabled", true);
        } else if (enabledIds.length > 1) {
            for (i = 0; i < enabledIds.length; i++) {
                $("#" + enabledIds[i]).prop("disabled", false);
            }
        }

        $("#ActiveAll").prop("checked", enabledIds.length === window.PRODUCTS.length);
    }

    var changeCommissionOnTierSetting = function () {
        var $this = $(this);
        var $tr = $this.closest("tr");
        var value = this.value;
        var $pre = $tr.prev("tr").find(".commission:first");
        if ($pre.length > 0) {
            var preValue = $pre[0].value;
            if (preValue > value) {
                this.value = preValue;
                return;
            }
        }

        window.setTimeout(function () {
            var $next = $tr.nextAll("tr").find(".commission");
            $next.each(function () {
                if (this.value < value) {
                    this.value = value;
                }
            });
        }, 1);
    };

    var changeInfiniteCheckbox = function () {
        var $this = $(this);
        var $tr = $this.closest("tr");

        if (this.checked === true) {
            $tr.find(".maxAM, .maxQM").prop("disabled", true).val("∞");
            $tr.find("input[type=checkbox]").prop("checked", true);
            $tr.closest("div.tab_content").find("button").prop("disabled", true);
        } else {
            $tr.find(".maxAM, .maxQM").prop("disabled", false);
            var minAM = $tr.find(".minAM").val();
            var minQM = $tr.find(".minQM").val();
            $tr.find(".maxAM").val(minAM);
            $tr.find(".maxQM").val(minQM);

            var disableButton = false;
            if (minAM >= MAXAM - 1 && minQM >= MAXQM - 1) {
                disableButton = true;
            }

            $tr.find("input[type=checkbox]").prop("checked", false);
            $tr.closest("div.tab_content").find("button").prop("disabled", disableButton);
        }
    };

    var changeMaxAMTAndMaxQM = function () {
        var $this = $(this);
        var className = $this.hasClass("maxAM") ? "maxAM" : "maxQM";
        var maxvalue = $this.hasClass("maxAM") ? MAXAM : MAXQM;

        var value = parseInt(this.value.replace(/,/g, ""));

        if (value === maxvalue) {
            value = maxvalue - 1;
        }

        var $button = $this.closest(".tab_content").find("button:first");

        if (!$this.closest("tbody").find("tr:last input[type=checkbox]").prop("checked")) {
            $button.prop("disabled", value >= maxvalue);
        }

        this.value = formatInt(value);

        var $preInput = $this.closest("tr").prev("tr").find("." + className);

        if ($preInput.length > 0) {
            var preValue = $preInput.val();
            preValue = parseInt(preValue.replace(/,/g, ""));

            if (preValue >= value) {
                value = preValue + 1;
                this.value = formatInt(value);

                if (value >= maxvalue) {
                    $button.prop("disabled", true);
                }

                return;
            }
        }

        value++;

        var $tr = $this.closest("tr").nextAll("tr");
        var maxClassName = "." + className;
        var minClassName = maxClassName.replace("max", "min");

        if (value + $tr.length >= maxvalue) {
            value = maxvalue - $tr.length;
            this.value = formatInt(value - 1);
        }

        $tr.each(function () {
            var $self = $(this);
            var minTextBox = $self.find(minClassName)[0];
            var maxTextBox = $self.find(maxClassName)[0];

            var v = maxTextBox.value.replace(/,/g, "");
            minTextBox.value = formatInt(value);

            if (v <= value) {
                maxTextBox.value = formatInt(value);
            }

            value = parseInt(maxTextBox.value.replace(/,/g, "")) + 1;
        });
    };

    var checkRequiredFields = function () {
        var groupname = $("#GroupName").val().trim();
        if (groupname === "") {
            alert(window.MSG.GroupNameEmpty);
            return false;
        }

        var amount = $("#QualifiedAmount").val().replace(/,/g, "");
        if (amount === "" || amount <= 0) {
            alert(window.MSG.ValidQualifiedAmount);
            return false;
        }

        return true;
    };

    var getEnabledProducts = function () {
        var products = window.PRODUCTS.slice(0);

        // get deductions for each Product
        for (var i = 0; i < window.PRODUCTS.length; i++) {
            var productid = window.PRODUCTS[i];
            var $tab = $("#tab_header" + productid);

            if ($tab.data("disabled")) {
                var index = products.indexOf(productid);
                products.splice(index, 1);
                continue;
            }
        }

        return products;
    };

    var isCopiedProduct = function (productid) {
        var $tab = $("#tab_header" + productid);

        return $tab.data("copied");
    };

    var checkTierSettings = function (products) {
        var msg;

        for (var i = 0; i < products.length; i++) {
            var productid = products[i];
            var $tab = $("#tab_header" + productid);
            var productname = $tab.data("productname");

            if (isCopiedProduct(productid)) {
                continue;
            }

            var $tr = $("#tblTier" + productid).find("tr.tier");

            if ($tr.length === 0) {
                msg = window.MSG.InvalidSettingFor.replace("{item}", productname) + " " + window.MSG.TierSettingAtLeastTier;
                alert(msg);
                return false;
            }

            if ($tr.last().find("input[type=checkbox]:first").is(":checked") === false) {
                msg = window.MSG.InvalidSettingFor.replace("{item}", productname) + " " + window.MSG.LastTierSettingHaveMaxValue;
                alert(msg);
                return false;
            }
        }

        return true;
    };

    var getDeductionSetting = function (productid) {
        var deductionList = "", deductedPercentageList = "";
        var $deductions = $("#tab_content" + productid + " .deductions input[type=checkbox]:checked");

        $deductions.each(function () {
            var percent = $(this).closest("tr").find("select.deductionlist").val();
            deductionList += $(this).data("id") + ",";

            deductedPercentageList += percent + ",";
        });

        return {
            deductionList: deductionList,
            deductedPercentageList: deductedPercentageList
        };
    };

    var collectDeductionSettings = function (products) {
        var deductionList = "", deductedPercentageList = "";
        var finalProducts = [];

        finalProducts = finalProducts.concat(products);

        for (var i = 0; i < finalProducts.length; i++) {
            var localSetting = getDeductionSetting(finalProducts[i]);
            deductionList += localSetting.deductionList;
            deductedPercentageList += localSetting.deductedPercentageList;
        }

        return {
            deductionList: deductionList,
            deductedPercentageList: deductedPercentageList
        };
    };

    var getTierSetting = function (productid) {
        var tierList, minAmountList, maxAmountList, minQMList, maxQMList, commissionList;
        tierList = minAmountList = maxAmountList = minQMList = maxQMList = commissionList = "";

        var $tr = $("#tblTier" + productid).find("tr.tier");

        tierList += $tr.length + ";";

        $tr.each(function () {
            var $this = $(this);
            var minAM = $this.find(".minAM").val().replace(/,/g, "");
            var maxAM = $this.find(".maxAM").val().replace(/,/g, "");
            var minQM = $this.find(".minQM").val().replace(/,/g, "");
            var maxQM = $this.find(".maxQM").val().replace(/,/g, "");

            if (maxAM === '∞') {
                maxAM = MAXAM;
            }

            if (maxQM === '∞') {
                maxQM = MAXQM;
            }

            var commission = $this.find(".commission").val();

            minAmountList += minAM + ",";
            maxAmountList += maxAM + ",";
            minQMList += minQM + ",";
            maxQMList += maxQM + ",";
            commissionList += commission + "@";
        });

        return {
            tierList: tierList,
            minAmountList: minAmountList,
            maxAmountList: maxAmountList,
            minQMList: minQMList,
            maxQMList: maxQMList,
            commissionList: commissionList
        };
    };

    var collectTierSettings = function (products) {
        var tierList, minAmountList, maxAmountList, minQMList, maxQMList, commissionList;
        tierList = minAmountList = maxAmountList = minQMList = maxQMList = commissionList = "";

        // get deductions for each Product
        for (var i = 0; i < products.length; i++) {
            var productid = products[i];

            if (isCopiedProduct(productid)) {
                continue;
            }

            var localTierSetting = getTierSetting(productid);
            tierList += localTierSetting.tierList;
            minAmountList += localTierSetting.minAmountList;
            maxAmountList += localTierSetting.maxAmountList;
            minQMList += localTierSetting.minQMList;
            maxQMList += localTierSetting.maxQMList;
            commissionList += localTierSetting.commissionList;

            minAmountList = minAmountList.removeLast() + ";";
            maxAmountList = maxAmountList.removeLast() + ";";
            minQMList = minQMList.removeLast() + ";";
            maxQMList = maxQMList.removeLast() + ";";
            commissionList = commissionList.removeLast() + ";";
        }

        return {
            tierList: tierList,
            minAmountList: minAmountList,
            maxAmountList: maxAmountList,
            minQMList: minQMList,
            maxQMList: maxQMList,
            commissionList: commissionList
        };
    };

    var changeCurrency = function (currencyId, currencyCode) {
        $.ajax({ url: AFFILIATE.getAbsoluteUrl("CommGroup/GetCurrencyNotification"),
            type: 'post',
            data: {
                currencyid: currencyId
            },
            success: function (data) {
                AFFILIATE.PreHandleAjaxResult(data);

                if (data === "") {
                    $("#currNotify").hide();
                }
                else {
                    $("#currNotify").html(data).show();
                }
            }
        });

        $.ajax({ url: AFFILIATE.getAbsoluteUrl("CommGroup/GetDefaultPspExchange"),
            type: 'post',
            data: {
                CurrencyCode: currencyCode
            },
            success: function (data) {
                AFFILIATE.PreHandleAjaxResult(data);
                $('.totaldeposit').val(data.DepositPercent);
                $('.totalwithdrawal').val(data.WithdrawalPercent);
            }
        });
    };

    var collectCommGroupInfo = function (products) {
        return {
            GroupName: $("#GroupName").val().trim(),
            Description: $("#Description").val().trim(),
            QualifiedAmount: $("#QualifiedAmount").val().replace(/,/g, ""),
            Currency: $("#Currency").val(),
            CopyForAllProducts: $("div.tab_title input.copy:checked").length > 0,

            ProductIDList: products.toString().replace(/,/g, ";"),
            DepositPercent: $(".totaldeposit").val(),
            WithdrawalPercent: $(".totalwithdrawal").val()
        };
    };

    var changeActiveAll = function (checked) {
        var i;
        if (checked) {
            for (i = 0; i < window.PRODUCTS.length; i++) {
                enableProduct(window.PRODUCTS[i]);
            }

        } else {
            var all = true;

            for (i = 0; i < window.PRODUCTS.length; i++) {
                if (disableProduct(window.PRODUCTS[i])) {
                    all = false;
                }
            }

            if (all) {
                this.checked = true;
            }
        }
    };

    return {
        init: function () {

            $("#Currency").change(function () {
                changeCurrency(this.value, $(this).find(":selected").text());
            });

            var $tabcontent = $(".tab_content");

            var $tabheader = $(".tab_header");

            $tabcontent.find("input.deduction").change(function (e) {
                e.stopPropagation();
                $(this).closest("tr").find("select").prop("disabled", !this.checked);
            });

            $("#QualifiedAmount").numeric();

            $("#QualifiedAmount").blur(function () {
                var value = parseInt(this.value.replace(/,/g, ""));
                if (!isNaN(value)) {
                    if (value > 100000000000000) {
                        value = 100000000000000;
                    }
                    this.value = formatInt(value);
                }
            });

            $tabcontent.find("input[type=text]").numeric();

            disableCopyCheckboxes();

            $tabheader.find(".header_icon").click(function (e) {
                e.stopPropagation();
                var productid = $(this).closest(".tab_header").data("productid");
                var status = $(this).closest(".tab_header").find("#header_icon" + productid).hasClass("icon-ok-circle");

                if (status) {
                    disableProduct(productid);
                } else {
                    enableProduct(productid);
                }

                disableCopyCheckboxes();
            });

            $tabheader.find(".product_header").click(function () {
                var $tab = $(this).closest(".tab_header");

                if ($tab.data("disabled") || $tab.data("copied")) {
                    return;
                }

                $tab.next(".tab_content").toggle();

                $(this).closest("div").toggleClass("expand collapse");
            });

            $tabcontent.on("click", ".icon-remove", function () {
                if ($(this).hasClass("icon-white")) {
                    return;
                }

                $(this).closest(".tab_content").find("button").prop("disabled", false);
                var $table = $(this).closest("table");
                $(this).closest("tr").remove();

                if ($table.find("tbody tr").length === 1) // noinfo row
                {
                    $table.find("tbody tr").show();
                }
                else {
                    var $tr = $table.find("tbody tr:last");
                    $tr.find("input[type=checkbox]").prop("disabled", false);
                    $tr.find(".icon-remove:last").removeClass("icon-white").prop("disabled", false);
                }
            });

            $("#ActiveAll").change(function () {

                changeActiveAll(this.checked);


                disableCopyCheckboxes();
            });

            $tabheader.on("change", "input[type=checkbox]", function () {
                var productid = parseInt(this.id, 10);

                for (var i = 0; i < window.PRODUCTS.length; i++) {
                    var id = window.PRODUCTS[i];

                    if (productid !== id) {
                        if (this.checked) {
                            copyTierSettings(id);
                        } else {
                            uncopyTierSettings(id);
                        }
                    }
                }
            });

            $tabcontent.find("button.addtier").click(function () {
                var productID = $(this).data("productid");
                addTier(productID);
            });

            $tabcontent.on("change", "input[type=checkbox]", changeInfiniteCheckbox);

            $tabcontent.on("change", "select.commission", changeCommissionOnTierSetting);

            $tabcontent.on("blur", "input.maxAM, input.maxQM", changeMaxAMTAndMaxQM);

            $tabcontent.on("change", ".psp-deduction", function () {
                if ($(this).val() === "0.00") {
                    $('.totaldeposit').prop("disabled", true);
                    $('.totalwithdrawal').prop("disabled", true);
                } else {
                    $('.totaldeposit').prop("disabled", false);
                    $('.totalwithdrawal').prop("disabled", false);
                }
            });

            $("#btnSubmit").click(function () {

                if (!checkRequiredFields()) {
                    return;
                }

                var products = getEnabledProducts();

                if (products.length === 0) {
                    alert(window.MSG.EnableProduct);
                    return;
                }

                if (!checkTierSettings(products)) {
                    return;
                }

                var data = collectCommGroupInfo(products);

                var deductionSettings = collectDeductionSettings(products);

                var tierSettings = collectTierSettings(products);

                data = $.extend(true, data, deductionSettings, tierSettings);

                if ($("#GroupID").val()) {
                    data.GroupID = $("#GroupID").val();
                }

                var action = $("#CommGroupSettingsForm").attr("action");
                $.ajax({
                    url: action,
                    type: 'post',
                    data: data,
                    success: function (result) {
                        AFFILIATE.PreHandleAjaxResult(result);
                        AFFILIATE.showUpdateStatus(result.ErrorCode, result.Message, (parent.COMMGROUP ? parent.COMMGROUP.ReRenderAfterPaged : undefined));
                    }
                });
            });
        }
    };
})(jQuery);

$(function () {
    "use strict";
    CommGroupPopup.init();

    $(".deductions").createSearchPanel();
});