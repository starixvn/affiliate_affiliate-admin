var MAXQM = 1000000000;
var MAXAM = 100000000000000;

var COMMGROUP = function ($) {
    var INDEX_PRODUCT_COL = 8;
    var INDEX_DEDUCTION_COL = 15;
    var INDEX_GENERAL_DEDUCTION_COL = 16;
    var INDEX_ACTION_COL = 17;

    var genGridRows = function (list, gridOption) {
        var len = list.length;

        $("#gridId").removeClass('hover');
        var columns = gridOption.columns;
        var bodyRows = [];

        var preProduct = -1;
        var cssClass = "";
        var tdClass = "sub_odd";

        for (var i = 0; i < len; i++) {

            var record = list[i];
            for (var j = 0; j < record.Details.length; j++) {
                cssClass = "";
                var dataRow = [];
                for (var k = 0; k < columns.length; k++) {

                    var colName = columns[k].Name;
                    var col = columns[k];
                    var data = null;
                    if (k < INDEX_PRODUCT_COL) {
                        data = record;
                    }
                    else {
                        data = record.Details[j];
                    }

                    if (j == 0 || (k >= INDEX_PRODUCT_COL && k < INDEX_GENERAL_DEDUCTION_COL)) {
                        if (j == 0)
                            cssClass = i % 2 == 1 ? "rowmain_odd" : "rowmain_even";
                        if (k == INDEX_PRODUCT_COL || k == INDEX_DEDUCTION_COL) {
                            if (preProduct != data.ProductID) {
                                var value = "";
                                if (k == INDEX_PRODUCT_COL)
                                    tdClass = tdClass == "sub_odd" ? "sub_even" : "sub_odd";

                                if (k == INDEX_DEDUCTION_COL) {
                                    var deductions = record.Products[data.ProductID].Deductions;

                                    for (var property in deductions) {
                                        if (deductions.hasOwnProperty(property)) {
                                            var deductionValue = deductions[property];
                                            value += "<input disabled type='checkbox' " + (deductionValue >= 0 ? " checked='checked' " : null) + " /> " + property;

                                            if (deductionValue >= 0)
                                                value += " (" + numeral(deductionValue).format(AFFILIATE.FORMATNUMBER) + ") ";
                                            value += "<br />";
                                        }
                                    }
                                }
                                else {
                                    value = data[colName];
                                }

                                if (k > INDEX_PRODUCT_COL + 2 && k < INDEX_DEDUCTION_COL - 1) {
                                    if (value == MAXAM || value == MAXQM)
                                        value = "∞";
                                    else
                                        value = numeral(value).format(AFFILIATE.FORMAT_INT_NUMBER);
                                }

                                dataRow.push({
                                    TdClass: tdClass,
                                    Name: colName,
                                    Value: value,
                                    MergeRowNumber: record.Products[data.ProductID].NumOfItems
                                });
                            }
                        } else {

                            var value = null;

                            if (k == INDEX_GENERAL_DEDUCTION_COL) {
                                value = '';
                                var deductions = record.GeneralDeductions;

                                for (var property in deductions) {
                                    if (deductions.hasOwnProperty(property)) {
                                        var deductionValue = deductions[property];
                                        value += "<input disabled type='checkbox' " + (deductionValue >= 0 ? " checked='checked' " : "") + " /> " + property;

                                        if (deductionValue >= 0)
                                            value += " (" + numeral(deductionValue).format(AFFILIATE.FORMATNUMBER) + ") ";
                                        value += "<br />";
                                    }
                                }
                            }else if (k == INDEX_ACTION_COL) {

                                var deleteButton = record.IsAllowedToDelete ?
                                            "<a class='remove' data-groupname='" + record.GroupName + "' data-groupid='" + data.GroupId + "' href='#' title='{0}'><i class='icon-remove'></i></a>".format(MSG.Remove)
                                            : "<a title='{0}'><i class='icon-remove icon-white'></i></a>".format(MSG.Remove);
                                var editButton = (IS_IT || record.CurrencyID != 0) ?
                                            "<a class='edit'  title='{0}' href='#' data-groupid='".format(MSG.Edit) + data.GroupId + "'><i class='icon-edit'></i></a>"
                                            : "<a title='{0}'><i class='icon-edit icon-white'></i></a>".format(MSG.Edit);
                                value = editButton + deleteButton;
                            } else if (colName == "NoOfAffiliate") {
                                value = "<a href='" + AFFILIATE.baseUrl + "Affiliate?ExcludeTest=false&groupid=" + data.GroupID + "'>" + data[colName] + "</a>";
                            } else if (k == 0) {
                                value = i + 1;
                            }
                            else {
                                value = data[colName];
                            }

                            if (k > INDEX_PRODUCT_COL + 1 && k < INDEX_DEDUCTION_COL - 1) {
                                if (value == MAXAM || value == MAXQM)
                                    value = "∞";
                                else
                                    value = numeral(value).format(AFFILIATE.FORMAT_INT_NUMBER);
                            }

                            dataRow.push({
                                TdClass: k >= INDEX_PRODUCT_COL && k < INDEX_GENERAL_DEDUCTION_COL ? tdClass : null,
                                Name: colName,
                                Value: value,
                                MergeRowNumber: (j == 0 && (k < INDEX_PRODUCT_COL || k == INDEX_GENERAL_DEDUCTION_COL || k == INDEX_ACTION_COL) ? record.Details.length : undefined)
                            });
                        }
                    }
                }
                preProduct = record.Details[j].ProductID;

                bodyRows.push({
                    Columns: dataRow,
                    CssClass: cssClass
                });
            }
            preProduct = -1;
        }
        return bodyRows;
    };
    return {
        init: function () {
            AFFILIATE.initMonthPicker();

            $("#gridId").resize(function () {
                if ($("#gridId").width() > AFFILIATE.MIN_CONTENT_WIDTH) {
                    $(".search-panel").width($("#gridId").width());
                } else {
                    $(".search-panel").width(AFFILIATE.MIN_CONTENT_WIDTH);
                }
            });

            if ($("#gridId").length > 0) {
                COMMGROUP.submitRequest();
            }

            $('.search-panel form').submit(function () {
                COMMGROUP.submitRequest();
                return false;
            });

            $("#gridId").on("click", "a.edit", function () {
                var groupid = $(this).data("groupid");
                var url = "CommGroup/Add?groupid=" + groupid;

                AFFILIATE.openDialogFrame(url, "Edit Commission Group", { width: 900, height: 700 });
            });

            $("#gridId").on("click", "a.remove", function () {
                var groupid = $(this).data("groupid");
                var groupname = $(this).data("groupname");

                if (confirm(MSG.ConfirmDelGroup.format(groupname))) {
                    $.ajax({
                        url: AFFILIATE.getAbsoluteUrl("CommGroup/Delete"),
                        type: 'post',
                        data: {
                            groupid: groupid
                        },
                        success: function (data) {
                            if (data != "")
                                alert(data);
                            else
                                COMMGROUP.ReRenderAfterPaged();
                        }
                    });
                }
            });
        },
        submitRequest: function () {
            AFFILIATE.trimBeforeSubmit();
            COMMGROUP.ReRenderAfterPaged(0, AFFILIATE.pageSizes[0]);
        },
        showAddPopup: function () {
            //   var queryString = jQuery.param(param);
            var url = "CommGroup/Add?"; // +queryString;

            AFFILIATE.openDialogFrame(url, "Add Commission Group", { width: 900, height: 700 });
        },
        checkAll: function (value) {
            $('#gridId input[type=checkbox].check').prop('checked', value);
        },
        ReRenderAfterPaged: function (currentPage, itemsPerPage) {
            // resize height of datagrid
            gridOption = AFFILIATE.resizeHeightDataGrid(gridOption);

            if (gridOption && gridOption.pagerOption) {
                currentPage = currentPage >= 0 ? currentPage : gridOption.pagerOption.currentPage;
                itemsPerPage = itemsPerPage || gridOption.pagerOption.itemsPerPage;
            }
            var currency = $("#currencylist").val();
            var date = $("#month").monthpicker("getDate");
            var groupname = $('#SearchForm #groupname').val().trim();
            $('#SearchForm #groupname').val(groupname);
            $.ajax({
                url: AFFILIATE.getAbsoluteUrl("CommGroup/GetList"),
                type: 'post',
                data: {
                    groupname: groupname,
                    currencies: currency ? currency.toString() : undefined,
                    date: date.toShortDate(),
                    pageIndex: (currentPage + 1),
                    pageSize: itemsPerPage
                },
                success: function (data) {

                    gridOption.pagerOption = {
                        itemsPerPage: AFFILIATE.pageSizes[0],
                        itemsPerPageArray: AFFILIATE.pageSizes,
                        currentPage: 0,
                        totalItem: data.List.length,
                        pagerSelectCallBack: COMMGROUP.ReRenderAfterPaged
                    };

                    gridOption.bodyRows = genGridRows(data.List, gridOption);

                    $('#gridId').removeClass("noinfo").fanexGrid(gridOption);

                    if (data.HasCashBackDeduction == false) {
                        setTimeout(function () {
                            $("#gridId").fanexGrid("hideColumns", gridOption, [16]);
                        }, 100);
                    }

                    $(".btnAdd").addClass("icon-add-commgrp");

                    if (date.getMonth() == (new Date()).getMonth()) {
                        $("#gridId").fanexGrid("hideColumns", gridOption, []);
                        // remove tooltips of two last columns
                        $("#gridId tr ").find("td.free-grid-td-2 div,td.free-grid-td-16 div,td.free-grid-td-15 div").prop("title", "");
                    } else {
                        $("#gridId").fanexGrid("hideColumns", gridOption, [17]);
                    }
                    if (data.TotalItem == 0) {
                        AFFILIATE.showNoDataMsgInGrid('#gridId', Lang.NoDataInfo);
                    }
                }
            });
        }
    };
} (jQuery);