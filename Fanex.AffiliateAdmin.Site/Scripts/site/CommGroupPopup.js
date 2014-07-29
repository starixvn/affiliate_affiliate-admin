var CommGroupPopup = (function ($) {
    "use strict";
    function formatInt(value) {
        return numeral(value).format(AFFILIATE.FORMAT_INT_NUMBER);
    }
    function newTierRow(tier, minAM, maxAM, minQM, maxQM, commission) {
        var $tr = $("#tierRow tr").clone();
        $tr.find("#tier").html(tier);
        $tr.find("#minAM").val(formatInt(minAM || 0));
        $tr.find("#maxAM").val(formatInt(maxAM || 0));
        $tr.find("#minQM").val(formatInt(minQM || 0));
        $tr.find("#maxQM").val(formatInt(maxQM || 0));
        $tr.find(".commission option").html(commission || "0.00");
        window.setTimeout(function () {
            var dropdown = $("#commissionlist select").clone();
            dropdown.val(commission || "0.00");
            $tr.find("td#commission").html(dropdown);
        }, 1);
        return $tr[0];
    }

    function addTier(productID) {
        var $table = $("#tab_content" + productID + " #tblTier" + productID + ">tbody");
        var noTier = $table.find("tr.tier").length;

        if (noTier >= 50) {
            var productname = $("#tab_header" + productID).data("productname");
            alert(MSG.InvalidSettingFor.replace("{item}", productname) + " " + MSG.MaxTierAllow);
            return;
        }
        var minAM, maxAM, minQM, maxQM, commission, newTierID;

        if ($table.find("tr.tier").length == 0) { // noinfo row        
            minAM = maxAM = minQM = maxQM = 0;
            commission = "0.00";
            newTierID = 1;
            $table.find("tr:first").hide();
        }
        else {
            var $lastRow = $table.find("tr:last");
            $lastRow.find("input[type=checkbox]").prop("disabled", true);
            minAM = parseInt($lastRow.find("#maxAM").val().replace(/,/g, "")) + 1;
            maxAM = minAM;
            minQM = parseInt($lastRow.find("#maxQM").val().replace(/,/g, "")) + 1;
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

    function enableProduct(productID, value, changeIcon, showConfirm) {

        var $tab = $(productID == null ? ".tab_header" : "#tab_header" + productID);
        if (value == false && $tab.data("disabled") && !$tab.find("#header_icon" + productID).hasClass("icon-ok-circle")) {
            return;
        }

        if (value) {
            $(productID == null ? ".tab_content" : "#tab_content" + productID).show();
            if ($tab.data("disabled"))
                $tab.find("#product_header" + productID).css("color", "").addClass("header_hightlight");
        }
        else {
            $tab.find("#product_header" + productID).removeClass("header_hightlight");
            if (showConfirm) {
                var productname = $("#tab_header" + productID).data("productname");
                var msg = MSG.ConfirmRemoveFromGroup.replace("{item}", productname);
                if (confirm(msg)) {
                    $(productID == null ? ".tab_content" : "#tab_content" + productID).hide();
                }
                else
                    return false;
            } else {
                $(!productID ? ".tab_content" : "#tab_content" + productID).hide();
            }
        }

        $tab.find("#" + productID).prop("disabled", !value);

        $tab.data("disabled", !value).find("#product_header" + productID).css("color", !value ? "gray" : "black");

        if (changeIcon) {
            if (typeof (changeIcon) == "string")
                $tab.find("#header_icon" + productID).removeClass().addClass(changeIcon);
            else
                $tab.find("#header_icon" + productID).removeClass().addClass(value ? "icon-ok-circle" : "icon-ban-circle");
        }

        return true;
    }

    function disableCopyCheckboxes() {
        var enabledIds = [];
        for (var i = 0; i < PRODUCTS.length; i++) {
            var productid = PRODUCTS[i];
            var $tab = $("#tab_header" + productid);
            if (!$tab.data("disabled")) {
                enabledIds.push(productid);
            }
        }

        if (enabledIds.length == 1) {
            $("#" + enabledIds[0]).prop("disabled", true);
        } else if (enabledIds.length > 1) {
            for (var i = 0; i < enabledIds.length; i++) {
                $("#" + enabledIds[i]).prop("disabled", false);
            }

        }

        $("#ActiveAll").prop("checked", enabledIds.length == PRODUCTS.length);
    }

    return {
        init: function () {
            $("#Currency").change(function () {
                $.ajax({ url: AFFILIATE.getAbsoluteUrl("CommGroup/GetCurrencyNotification"),
                    type: 'post',
                    data: {
                        currencyid: this.value
                    },
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);

                        if (data == "") {
                            $("#currNotify").hide();
                        }
                        else {
                            $("#currNotify").html(data).show();
                        }
                    }
                });
            }).change();
            var $tabcontent = $(".tab_content");
            var $tabheader = $(".tab_header");

            $tabcontent.find("input.deduction").change(function () {
                $(this).closest("div").find("select.deductionlist:first").prop("disabled", !this.checked);
            });
            $("#QualifiedAmount").numeric();
            $("#QualifiedAmount").blur(function () {
                var value = parseInt(this.value.replace(/,/g, ""));
                if (!isNaN(value)) {
                    if (value > 100000000000000)
                        value = 100000000000000;
                    this.value = formatInt(value);
                }
            });
            $tabcontent.find("input[type=text]").numeric();

            $tabheader.each(function () {
                if ($(this).data("disabled")) {
                    enableProduct($(this).data("productid"), false, true, false);

                }
            });


            disableCopyCheckboxes();
            $tabheader.find(".header_icon").click(function (e) {
                e.stopPropagation();
                var productid = $(this).closest(".tab_header").data("productid");
                enableProduct(productid, !$(this).closest(".tab_header").find(".header_icon").hasClass("icon-ok-circle"), true, true);

                disableCopyCheckboxes();
            });
            $tabheader.find(".product_header").mouseout(function (e) {
                $(this).removeClass("header_hightlight");
            });
            $tabheader.find(".product_header").click(function () {
                var $tab = $(this).closest(".tab_header");

                if ($tab.data("disabled"))
                    return;

                $tab.next(".tab_content").toggle();

                $(this).closest("li").toggleClass("expand collapse");
            });

            $tabcontent.on("click", ".icon-remove", function () {
                if ($(this).hasClass("icon-white"))
                    return;
                $(this).closest(".tab_content").find("button").prop("disabled", false);
                var $table = $(this).closest("table");
                $(this).closest("tr").remove();

                if ($table.find("tbody tr").length == 1) // noinfo row
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

                if (this.checked) {
                    enableProduct(0, true, true);

                    // unactive specific deduction by product
                    for (var i = 0; i < PRODUCTS.length; i++) {
                        enableProduct(PRODUCTS[i], true, true);
                    }
                    
                } else {
                    var all = true;

                    // unactive general deduction
                    if (HasCashbackDeduction) {
                        if (enableProduct(0, false, true, true))
                            all = false;
                    }

                    // unactive specific deduction by product
                    for (var i = 0; i < PRODUCTS.length; i++) {
                        if (enableProduct(PRODUCTS[i], false, true, true))
                            all = false;
                    }

                    if (all)
                        this.checked = true;
                }

                disableCopyCheckboxes();
            });

            // Checkbox CopyTierSetting event
            $tabheader.on("change", "input[type=checkbox]", function () {
                var productid = this.id;
                for (var i = 0; i < PRODUCTS.length; i++) {
                    var id = PRODUCTS[i];
                    if (this.checked)
                        enableProduct(PRODUCTS[i], productid == id, "icon-ok-circle", false);
                    else
                        enableProduct(PRODUCTS[i], true, true, false);
                }
            });

            $tabcontent.find("button.addtier").click(function () {
                var productID = $(this).data("productid");
                addTier(productID);
            });

            // Checkbox maxAM/QM event
            $tabcontent.on("change", "input[type=checkbox]", function () {
                var $this = $(this);
                var $tr = $this.closest("tr");
                $tr.find("#maxAM, #maxQM").prop("disabled", this.checked).val("∞");
                $tr.find("input[type=checkbox]").prop("checked", this.checked);

                var disableButton = this.checked;
                if (disableButton == false && $tr.find("#minAM").val() >= MAXAM - 1 && $tr.find("#minQM").val() >= MAXQM - 1)
                    disableButton = true;
                $tr.closest("div.tab_content").find("button").prop("disabled", disableButton);

                if (!this.checked) {
                    var minAM = $tr.find("#minAM").val();
                    var minQM = $tr.find("#minQM").val();
                    $tr.find("#maxAM").val(minAM);
                    $tr.find("#maxQM").val(minQM);
                }
            });

            // Dropdown Commission event
            $tabcontent.on("change", "select.commission", function () {
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
                        if (this.value < value)
                            this.value = value;
                    });
                }, 1);
            });
            $tabcontent.on("blur", "input.maxAM, input.maxQM", function () {
                var $this = $(this);
                var maxvalue = this.id.indexOf("AM") > -1 ? MAXAM : MAXQM;

                var value = parseInt(this.value.replace(/,/g, ""));

                if (value == maxvalue)
                    value = maxvalue - 1;
                var $button = $this.closest(".tab_content").find("button:first");

                if (!$this.closest("tbody").find("tr:last input[type=checkbox]").prop("checked"))
                    $button.prop("disabled", value >= maxvalue);
                this.value = formatInt(value);
                var $preInput = $this.closest("tr").prev("tr").find("#" + this.id);

                var preValue = $preInput.val();
                if (preValue)
                    preValue = parseInt(preValue.replace(/,/g, ""));

                if (preValue >= value) {
                    value = preValue + 1;
                    $this.val(formatInt(value));
                    if (value >= maxvalue)
                        $button.prop("disabled", true);
                    return;
                }
                value++;
                var $tr = $this.closest("tr").nextAll("tr"); //.find("input." + this.id);
                var maxid = "#" + this.id;
                var minid = maxid.replace("max", "min");

                if (value + $tr.length >= maxvalue) {
                    value = maxvalue - $tr.length;
                    this.value = formatInt(value - 1);
                }

                $tr.each(function () {
                    var $self = $(this);
                    var min = $self.find(minid)[0];
                    var max = $self.find(maxid)[0];
                    var v = max.value.replace(/,/g, "");
                    min.value = formatInt(value);
                    if (v <= value) {
                        max.value = formatInt(value);
                    }
                    value = parseInt(max.value.replace(/,/g, "")) + 1;

                });
            });

            $("#btnSubmit").click(function () {
                var groupname = $("#GroupName").val().trim();
                if (groupname == "") {
                    alert(MSG.GroupNameEmpty);
                    return;
                }
                var amount = $("#QualifiedAmount").val().replace(/,/g, "");
                if (amount === "" || amount <= 0) {
                    alert(MSG.ValidQualifiedAmount);
                    return;
                }
                var groupid = $("#GroupID").val();
                if (groupid === "")
                    groupid = undefined;
                var products = PRODUCTS.slice(0);
                var data = {
                    GroupID: groupid,
                    GroupName: $("#GroupName").val().trim(),
                    Description: $("#Description").val().trim(),
                    QualifiedAmount: amount,
                    Currency: $("#Currency").val(),
                    CopyForAllProducts: PRODUCTS.length > 1 && $("div.tab_title input.copy:checked").length > 0
                };

                var tierList, minAmountList, maxAmountList, minQMList, maxQMList, commissionList, deductionList, deductedPercentageList;
                tierList = minAmountList = maxAmountList = minQMList = maxQMList = commissionList = deductionList = deductedPercentageList = "";

                // get Cashback deduction 
                var $deductions = $("#tab_content0 .deductions:first input[type=checkbox]:checked");

                $deductions.each(function () {
                    deductionList += $(this).data("id") + ",";

                    var percent = $(this).closest("div").find("select.deductionlist:first").val();

                    deductedPercentageList += percent + ",";
                });

                // get deductions for each Product
                for (var i = 0; i < PRODUCTS.length; i++) {
                    var productid = PRODUCTS[i];
                    var $tab = $("#tab_header" + productid);
                    var productname = $tab.data("productname");

                    var isdisabled = $tab.data("disabled");

                    if (isdisabled) {
                        var iscopy = $tab.find(".header_icon").hasClass("icon-ok-circle");
                        if (!iscopy) {
                            var index = products.indexOf(productid);
                            products.splice(index, 1);
                        }
                        continue;
                    }
                    var $tr = $("#tblTier" + productid).find("tr.tier");

                    // check for invalid setting
                    if ($tr.length == 0) {
                        var msg = MSG.InvalidSettingFor.replace("{item}", productname) + " " + MSG.TierSettingAtLeastTier;
                        alert(msg);
                        return;
                    }

                    if ($tr.last().find("input[type=checkbox]:first").is(":checked") == false) {
                        var msg = MSG.InvalidSettingFor.replace("{item}", productname) + " " + MSG.LastTierSettingHaveMaxValue;
                        alert(msg);
                        return;
                    }

                    var $deductions = $("#tab_content" + productid + " .deductions:first input[type=checkbox]:checked");

                    $deductions.each(function () {
                        deductionList += $(this).data("id") + ",";

                        var percent = $(this).closest("div").find("select.deductionlist:first").val();

                        deductedPercentageList += percent + ",";
                    });


                    tierList += $tr.length + ";";

                    $tr.each(function () {
                        var $this = $(this);
                        var minAM = $this.find(".minAM").val().replace(/,/g, "");
                        var maxAM = $this.find(".maxAM").val().replace(/,/g, "");
                        var minQM = $this.find(".minQM").val().replace(/,/g, "");
                        var maxQM = $this.find(".maxQM").val().replace(/,/g, "");
                        if (maxAM == '∞')
                            maxAM = MAXAM;
                        if (maxQM == '∞')
                            maxQM = MAXQM;

                        var commission = $this.find(".commission").val();

                        minAmountList += minAM + ",";
                        maxAmountList += maxAM + ",";
                        minQMList += minQM + ",";
                        maxQMList += maxQM + ",";
                        commissionList += commission + "@";

                    });
                    minAmountList = minAmountList.removeLast() + ";";
                    maxAmountList = maxAmountList.removeLast() + ";";
                    minQMList = minQMList.removeLast() + ";";
                    maxQMList = maxQMList.removeLast() + ";";
                    commissionList = commissionList.removeLast() + ";";
                }
                if (products.length == 0) {
                    alert(MSG.EnableProduct);
                    return;
                }

                data.DeductionList = deductionList;
                data.DeductedPercentageList = deductedPercentageList;
                data.TierList = tierList;
                data.MinAmountList = minAmountList;
                data.MaxAmountList = maxAmountList;
                data.MinQMList = minQMList;
                data.MaxQMList = maxQMList;
                data.CommissionList = commissionList;
                data.ProductIDList = products.toString().replace(/,/g, ";");


                $.ajax({ url: AFFILIATE.getAbsoluteUrl("CommGroup/Add"),
                    type: 'post',
                    data: data,
                    success: function (data) {
                        AFFILIATE.PreHandleAjaxResult(data);
                        AFFILIATE.showUpdateStatus(data.ErrorCode, data.Message, (parent.COMMGROUP ? parent.COMMGROUP.ReRenderAfterPaged : undefined));
                    }
                });
            });
        }
    };
})(jQuery);

