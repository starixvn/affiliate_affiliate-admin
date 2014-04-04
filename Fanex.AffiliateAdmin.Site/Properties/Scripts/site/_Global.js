var AFFILIATE = (function ($) {
    return {
        pageSizes: [10, 20, 50, 100, 200, 300],
        DATE_FORMAT: "MM/DD/YYYY",
        DATE_FORMAT_FULL: "MM/DD/YYYY hh:mm:ss",
        DATE_FORMAT_MONTH: "MM/YYYY",
        DATE_FORMAT_YEAR: "YYYY",
        FORMATNUMBER: '0,0.00',
        FORMAT_INT_NUMBER: '0,0',
        MIN_CONTENT_WIDTH: '1110',
        scrollTop: function () {
            $('html, body').animate({ scrollTop: $('.container-fluid').offset().top }, 400);
        },
        initDatePicker: function () {
            var datePickers = $('.datepicker');

            if (datePickers.length > 0) {
                var isValidDate = function (dateString) {
                    // Parse the date parts to integers
                    var parts = dateString.split("/");
                    var day = parseInt(parts[1], 10);
                    var month = parseInt(parts[0], 10);
                    var year = parseInt(parts[2], 10);

                    // Check the ranges of month and year
                    if (year < 1000 || year > 3000 || month === 0 || month > 12) {
                        return false;
                    }

                    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

                    // Adjust for leap years
                    if (year % 400 === 0 || (year % 100 !== 0 && year % 4 === 0)) {
                        monthLength[1] = 29;
                    }

                    // Check the range of the day
                    return day > 0 && day <= monthLength[month - 1];
                };
                var validateDate = function (target) {
                    var minDate = target.datepicker("option", "minDate");
                    var maxDate = target.datepicker("option", "maxDate");
                    var inputDate = target.val(); // the visible value on screen
                    var currentDate = target.datepicker("getDate");
                    var errMsg = '';

                    // if selected date is invalid
                    if (!isValidDate(inputDate)) {
                        errMsg = ApoRes.InvalidDate;
                    }

                    if (errMsg.length === 0 && currentDate < minDate) {
                        errMsg = ApoRes.DateIsOutOfRange;
                    }

                    if (errMsg.length === 0 && maxDate < currentDate) {
                        errMsg = ApoRes.DateIsOutOfRange;
                    }

                    target.attr('title', errMsg).tooltip({
                        position: { my: "left top+2", at: "left bottom" },
                        tooltipClass: "warning"
                    });

                    if (errMsg.length > 0) {
                        target.tooltip('open');
                        return false;
                    }

                    target.tooltip('destroy');
                    target.removeAttr('title');
                };
                var limitDate = function (target, limit) { // limit: mindate/maxdate
                    var _limit = target.attr(limit);

                    if (_limit) { // if limit attribute exists
                        _limit = _limit.split("/");

                        var year = _limit[0],
								month = _limit[1],
								day = _limit[2];

                        return new Date(year, month - 1, day);
                    }

                    return null;
                };
                var shiftDate = function (target, increment) {
                    var currDate = target.datepicker('getDate');
                    var d = currDate.getDate() + increment;
                    var m = currDate.getMonth();
                    var y = currDate.getFullYear();

                    target.datepicker('setDate', new Date(y, m, d));
                };

                datePickers.each(function () {
                    var $this = $(this);

                    $this.datepicker({
                        defaultDate: new Date($this.val()),
                        dateFormat: "dd M yy",
                        minDate: limitDate($this, "data-mindate"),
                        maxDate: limitDate($this, "data-maxdate"),
                        beforeShow: function (input, inst) {
                            //inst.dpDiv.css({ marginTop: '2px' });
                        },
                        onSelect: function (dateText, inst) {
                            $(this).attr('data-value', dateText);
                        }
                    });

                    $this.next('.icon-calendar').click(function () {
                        $this.datepicker("show");
                    });
                });
            }
        },
        // Show language Box
        showLanguageBox: function () {
            $("#arrow").removeClass("arrUp");
            $("#arrow").addClass("arrDown");
            $("#langList").show();
        },
        // Hide language Box
        hideLanguageBox: function () {
            $("#arrow").removeClass("arrDown");
            $("#arrow").addClass("arrUp");
            $("#langList").hide();
        },
        // Change language
        changeLanguage: function (lang) {
            var param = {
                "lang": lang
            };
            $.ajax({
                type: "POST",
                url: AFFILIATE.getAbsoluteUrl("Home/ChangeLanguage"),
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(param),
                success: function (result) {
                    AFFILIATE.PreHandleAjaxResult(result);
                    top.window.location.reload();
                }
            });
        },

        // Dialog
        hidePopupBlockLoading: function () {
            $("iframe").siblings('.block-loading').hide();
        },
        dialogOpt: {
            position: { my: "top+30%", at: "center top", of: window },
            modal: true,
            width: 'auto'
        },
        currentOpenDialog: null,
        openDialog: function (target, title, options) {
            options = options || {};

            if (title) {
                options.title = title;
            }

            if (target instanceof jQuery === false) {
                target = $(target);
            }

            AFFILIATE.currentOpenDialog = target.dialog($.extend({}, AFFILIATE.dialogOpt, options));

            var iframeLoading = target.find('iframe').get(0);
            if (iframeLoading) {
                iframeLoading.contentWindow.onload = function () {
                    AFFILIATE.hidePopupBlockLoading();
                };
            }

            return AFFILIATE.currentOpenDialog;
        },
        openDialogFrame: function (url, title, options) {
            var frameHtml = '<iframe src="' + AFFILIATE.getAbsoluteUrl(url) + '" width="100%" height="97%" frameborder="0" border="0"></iframe>';
            var frame = $("#aff-dialog-frame");

            if (frame.length > 0) {
                frame.html(frameHtml);
            } else {
                var blockLoading = '<div class="block-loading"></div>';
                frame = '<div id="aff-dialog-frame">' + blockLoading + frameHtml + '</div>';
            }
            return AFFILIATE.openDialog(frame, title, options);
        },
        openDialogUrl: function (url, title, options) {
            var frameHtml = '<iframe src="' + url + '" width="100%" height="97%" frameborder="0" border="0"></iframe>';
            var frame = $("#aff-dialog-frame");

            if (frame.length > 0) {
                frame.html(frameHtml);
            } else {
                var blockLoading = '<div class="block-loading"></div>';
                frame = '<div id="aff-dialog-frame">' + blockLoading + frameHtml + '</div>';
            }
            return AFFILIATE.openDialog(frame, title, options);
        },
        openDialogContent: function (content, title, options) {
            var frameHtml = content;
            var frame = $("#aff-dialog-frame");

            if (frame.length > 0) {
                frame.html(frameHtml);
            } else {
                var blockLoading = '<div class="block-loading"></div>';
                frame = '<div id="aff-dialog-frame">' + blockLoading + frameHtml + '</div>';
            }
            return AFFILIATE.openDialog(frame, title, options);
        },
        closeDialog: function (target) {
            target.dialog("close");
        },
        resizeDialog: function (width, height) {
            if (AFFILIATE.currentOpenDialog && AFFILIATE.currentOpenDialog.dialog("isOpen")) {
                AFFILIATE.currentOpenDialog.dialog("option", { width: width + 30, height: height + 50 });
            }
        },
        setDialogHeight: function (target, height) {
            target.dialog("option", "height", height);
        },

        // Error message
        showErrorMsg: function (msg) {
            var msgWrp = $('.validation-summary-valid');
            msgWrp.show();
            msgWrp.addClass('validation-summary-errors')
					.find('li:first').html(msg).show();
        },
        showSuccessMsg: function (msg) {
            var msgWrp = $('.validation-summary-valid');
            msgWrp.show();
            msgWrp.addClass('success').find('li:first').html(msg).show();
        },
        hideMsg: function () {
            var msgWrp = $('.validation-summary-valid');
            msgWrp.removeClass("validation-summary-errors")
                    .removeClass("success");
            msgWrp.hide();
        },
        isValidDate: function (year, month, day) {
            var beginDate = new Date(parseFloat(year) + "/" + month + "/" + day);
            var begYear = beginDate.getYear() < 200 ? beginDate.getYear() + 1900 : beginDate.getYear();


            if (begYear !== parseFloat(year) || beginDate.getMonth() + 1 !== parseFloat(month) || beginDate.getDate() !== parseFloat(day)) {
                return false;
            }

            return true;
        },

        //Clock 
        clock: function () {
            sysSecond++;
            if (sysSecond === 60) {
                sysSecond = 0;
                sysMinute++;
            }
            if (sysMinute === 60) {
                sysMinute = 0;
                sysHour++;
            }
            if (sysHour === 24) {
                sysHour = 0;
                sysSecond = 0;
                sysMinute = 0;
                sysDay++;
            }
            if (sysMinute < 10) {
                strMin = "0" + sysMinute;
            }
            else {
                strMin = sysMinute;
            }
            if (sysSecond < 10) {
                strSec = "0" + sysSecond;
            }
            else {
                strSec = sysSecond;
            }

            if (!AFFILIATE.isValidDate(sysYear, sysMonth, sysDay)) {
                sysMonth++;
                sysDay = 1;
            }
            if (!AFFILIATE.isValidDate(sysYear, sysMonth, sysDay)) {
                sysYear++;
                sysMonth = 1;
                sysDay = 1;
            }

            if (sysHour < 10) {
                strHrs = "0" + sysHour;
            }
            else {
                strHrs = sysHour;
            }
            $('#lblDateTime').html(strHrs + ":" + strMin + ":" + strSec + " " + sysDay + "-" + sysMonthName[sysMonth - 1] + "-" + sysYear + sysGmt);
        },
        genGridRows: function (list, options) {
            var columns = options.columns;
            var rowCssClass = options.rowCssClass; // custom option
            var rowHeight = options.RowHeight;
            var indexNumberColumn = options.indexNumberColumn; // custom option

            var bodyRows = [];
            var len = list.length;

            var currentPage = options.pagerOption.currentPage || 0;
            var rowIndex = currentPage * options.pagerOption.itemsPerPage;
            for (var i = 0; i < len; i++) {
                var record = list[i];
                var dataRow = [];

                for (var j = 0; j < columns.length; j++) {

                    (function () {
                        var rIndex = rowIndex;
                        var rowData = record;
                        var col = columns[j];
                        var colName = col.Name;

                        dataRow.push({
                            Name: colName,
                            Value: (indexNumberColumn === j) ? rIndex + 1 : record[colName],
                            CustomCellRender: function (cell, cellFormated) {
                                if (col.Formatter) {
                                    return col.Formatter(rIndex, rowData, cell, cellFormated);
                                }

                                return cellFormated;
                            }
                        });
                    } ());
                }

                bodyRows.push({
                    Columns: dataRow,
                    CssClass: rowCssClass ? rowCssClass(rowIndex, record) : undefined,
                    RowHeight: rowHeight ? rowHeight(rowIndex, record) : undefined
                });

                rowIndex++;
            }
            return bodyRows;
        },

        genGridData: function (columns, list, rowCssClass) {
            var bodyRows = [];
            var len = list.length;
            for (var i = 0; i < len; i++) {
                var record = list[i];
                var dataRow = [];

                for (var j = 0; j < columns.length; j++) {
                    (function () {
                        var colName = columns[j].Name;
                        var col = columns[j];
                        dataRow.push({
                            Name: colName,
                            Value: record[colName],
                            CustomCellRender: function (cell, cellFormated) {
                                if (col.Formatter) {
                                    return col.Formatter(record, cell, cellFormated);
                                }

                                return cellFormated;
                            }
                        });
                    } ());
                }


                bodyRows.push({
                    Columns: dataRow,
                    CssClass: rowCssClass ? rowCssClass(record) : undefined
                });
            }
            return bodyRows;
        },

        showUpdateStatus: function (errCode, msg, callback, timeout) {
            if (errCode === 0) {
                AFFILIATE.showSuccessMsg(msg);

                if (callback) {
                    callback(0, parent.$("#itemperpage").val()); //Refresh parent page
                }

                setTimeout("AFFILIATE.refreshParent()", timeout || 3000);
            }
            else {
                AFFILIATE.showErrorMsg(msg);

                if (callback) {
                    callback();
                }
            }
        },
        refreshParent: function () {
            if (parent.AFFILIATE.currentOpenDialog) {
                parent.AFFILIATE.currentOpenDialog.dialog("close");
            }
        },
        closeDialogNotMsg: function (errCode, msg) {
            if (errCode !== 0) {
                AFFILIATE.showErrorMsg(msg);
            }
            else {
                if (parent.AFFILIATE.currentOpenDialog) {
                    parent.AFFILIATE.currentOpenDialog.dialog("close");
                }
            }
        },
        ChangePassword: function () {
            AFFILIATE.openDialogFrame("Account/ChangePassword", "Change Password", { width: 450, height: 300 });
        },
        showNoDataMsgInGrid: function (gridId, msg) {
            $(gridId).html(msg);
            $(gridId).addClass('noinfo');
        },
        createButtonOnTitle: function (btnId, title, handler) {
            return '<input class="{0}" type="button" title="{1}" value="" name="{0}" onclick="{2}" />'.format(btnId, title, handler);
        },
        trimBeforeSubmit: function () {
            $('form').find('input:text, textarea').each(function () {
                $(this).val($.trim($(this).val()));
            });

            $('form').find('input:text').each(function () {
                $(this).val($.trim($(this).val()));
            });
        },
        renderGrid: function (gridObject, data, gridOption, callback) {
            gridObject.show();
            gridOption.pagerOption = {
                itemsPerPage: data.ItemsPerPage,
                itemsPerPageArray: AFFILIATE.pageSizes,
                currentPage: data.CurrentPage,
                totalItem: data.TotalItem,
                pagerSelectCallBack: callback
            };

            var bodyRows = AFFILIATE.genGridRows(data.List, gridOption);
            gridOption.bodyRows = bodyRows;

            gridObject.removeClass("noinfo").fanexGrid(gridOption);

            if (data.List.length === 0) {
                gridObject.addClass("noinfo").html(Lang.NoDataInfo);
            }
        },
        updateSearchPanel: function (gridObject) {
            $('.search-panel').width(gridObject.width());
        },
        resizeHeightDataGrid: function (gridOption) {
            var colHeight = $("#gridId").attr('colHeight'),
                expHeight = $("#gridId").attr('expHeight');
            if ($(".search-form").is(":visible")) {
                gridOption.gridExpandHeight = expHeight;
            } else {
                gridOption.gridExpandHeight = colHeight;
            }
            return gridOption;
        },
        setWidthOfFormSearch: function () {
            var search_panel = $('.search-panel'), dataGrid = $('.freegrid');
            // Set default width for form search with width of datagrid
            if (search_panel !== undefined && dataGrid !== undefined) {
                if (dataGrid.width() > search_panel.width()) {
                    search_panel.width(dataGrid.width());
                }
            }
        },
        initMonthPicker: function () {
            $('.monthpicker').each(function (index, element) {
                var $el = $(element);
                $el.removeAttr("data-val-date");
                $el.removeAttr("data-val");
                var date = new Date($el.attr("data-value"));
                var minDate = new Date($el.attr("data-mindate"));
                var maxDate = new Date($el.attr("data-maxdate"));

                $el.monthpicker({
                    pattern: 'mmm yyyy',
                    minDate: minDate,
                    maxDate: maxDate,
                    year: date.getFullYear(),
                    startYear: minDate.getFullYear(),
                    finalYear: maxDate.getFullYear(),
                    month: date.getMonth(),
                    'monthpicker-hide': function (p1, p2, p3) {
                        alert('aba');
                    }
                });
            });
        },
        compareDate: function (date1, date2) {
            /* Convention:
            -1: date1 < date2
            0: date1 = date2
            1: date1 > date2
            */
            var obj1 = moment(date1);
            var obj2 = moment(date2);

            if (obj1.year() < obj2.year()) {
                return -1;
            } else if (obj1.year() === obj2.year()) {
                if (obj1.month() < obj2.month()) {
                    return -1;
                } else if (obj1.month() === obj2.month()) {
                    if (obj1.date() < obj2.date()) {
                        return -1;
                    } else if (obj1.date() === obj2.date()) {
                        return 0;
                    } else {
                        return 1;
                    }
                } else {
                    return 1;
                }
            } else {
                return 1;
            }
        },
        resetToggleIndicator: function (gridOption) {
            var isCollapseStatus = subTotalStatus === COLLAPSE_SUBTOTAL_STATUS;
            var width = 0;

            if (isCollapseStatus) {
                $('.real-subtotal').hide();
                $('.pseudo-subtotal').show();
                width = $('.pseudo-subtotal .cell-subtotal').width();

            } else {
                $('.real-subtotal').show();
                $('.pseudo-subtotal').hide();
                width = $('.real-subtotal .cell-subtotal').width();
            }

            if ($('.real-cell').find('.toggle-indicator').length === 0) {
                $('.real-cell').prepend("<div class='toggle-indicator collapse' title='{0}'></div>".format(MSG.ToggleSubTotal));
            }

            if ($('.pseudo-cell').find('.toggle-indicator').length === 0) {
                $('.pseudo-cell').prepend("<div class='toggle-indicator expand' title='{0}'></div>".format(MSG.ToggleSubTotal));
            }

            $('.toggle-indicator').css('left', width / 2 - 15 + 'px');

            $('#gridId').fanexGrid("resizeGrid", gridOption);
        },
        PreHandleAjaxResult: function (data) {
            if (typeof data === "object") {
                if (data.hasOwnProperty("Error") && data.hasOwnProperty("LogOnUrl")) {
                    window.top.location.href = AFFILIATE.getAbsoluteUrl(data.LogOnUrl);

                }
            }
        }
    };
})(jQuery);

$(function () {

    $(".ic-reload").click(function () {
        if ($("form :submit").length > 0) {
            $("form :submit").click();
        }
        else {
            location.href = location.href;
        }
    });

    $(".ic-open-window").click(function () {
        if (parent.AFFMASTER !== undefined) {
            parent.AFFMASTER.openNewWindow(location.href);
        }
    });
    
    if ($(".ic-open-window") && parent.AFFMASTER === undefined) {
        $(".ic-open-window").hide();
    }

    // scroll top page
    $('#changePassword').click(function () {
        AFFILIATE.ChangePassword();
    });
    $('input, textarea').placeholder();
    $(".dropdownchecklist").createDropdownchecklist();
    $(".search-title").createSearchPanel();
    //$('.datepicker').datepicker();
    AFFILIATE.initDatePicker();

    //clock
    if ($("#lblDateTime").length > 0) {
        AFFILIATE.clock();
        setInterval("AFFILIATE.clock();", 1000);
    }
    // Show or Hide language Box
    $('#language').mouseover(function () {
        AFFILIATE.showLanguageBox();
    });

    $('#language').mouseout(function () {
        AFFILIATE.hideLanguageBox();
    });

    // Change language
    $('.langListRp A').click(function () {
        AFFILIATE.changeLanguage($(this).attr('class'));
    });

    $("form").submit(function (event) {
        AFFILIATE.trimBeforeSubmit();
    });

    // Resize browser when datagrid has the horizontal scroll
    //    $(window).resize(function () {
    //        var searchPanel = $('.search-panel');
    //        if (searchPanel !== undefined) {
    //            if ($(window).width() > AFFILIATE.MIN_CONTENT_WIDTH) {
    //                searchPanel.width($('.freegrid').width());
    //            } else {
    //                searchPanel.width("auto");
    //            }
    //        }
    //    });
});

Date.prototype.toShortDate = function() {
    return (this.getMonth() + 1) + "/" + this.getDate() + "/" + this.getFullYear();
};

String.prototype.isNumber = function() {
    var regs = new RegExp(/^-?\d+(\.\d+)?\s*$/);
    return (regs.test(this));
};

String.prototype.removeLast = function(count) {
    if(!count) 
        count = 1;
    return this.substring(0,this.length - count);    
};

if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (obj, start) {
        for (var i = (start || 0), j = this.length; i < j; i++) {
            if (this[i] === obj) { return i; }
        }
        return -1;
    }
}

// First, checks if it isn't implemented yet.
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] !== 'undefined'
        ? args[number]
        : match
      ;
        });
    };
}
if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    }
}
if (!String.prototype.endsWith) {
    String.prototype.endsWith = function (suffix) {
        return this.indexOf(suffix, this.length - suffix.length) !== -1;
    };
}
if (!Array.prototype.clone) {
    Array.prototype.clone = function () {
        return this.slice(0);
    };
}
$.fn.serializeObject = function () {
    var o = {};
    // find disabled inputs
    var inputs = this.find(':disabled');
    // enable them
    inputs.prop('disabled', false);

    var a = this.serializeArray();

    // then, disable them
    inputs.prop('disabled', true);

    $.each(a, function () {
        if (o[this.name] !== undefined) {            
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

$.fn.serializeObject2 = function () {
    var o = {};
    // find disabled inputs
    var inputs = this.find(':disabled');
    // enable them
    inputs.prop('disabled', false);

    var a = this.serializeArray();

    // then, disable them
    inputs.prop('disabled', true);

    $.each(a, function () {
        if (o[this.name] !== undefined) {
            o[this.name] = o[this.name] + ',' + this.value;
        } else {
            o[this.name] = this.value || '';
        }
    });

    return o;
};

$.fn.serializeData = function () {
    var o = {};

    $.each(this, function () {
        var name = this.id || this.name;
        var value = $(this).is(':checkbox') ? this.checked : $(this).val();

        if (value instanceof Array) {
            value = value.toString();
        }

        if (value !== null && value !== "") {
            o[name] = typeof value === "string" ? value.trim() : value;
        }
    });

    return o;
};

$.fn.createDropdownchecklist = function () {    
    $(this).each(function (index, element) {
        var $el = $(element);
        if ($el.hasClass('single-select')) {
            if ($el.hasClass('width-140')) {
                $el.multiselect({
                    multiple: false,
                    header: false,
                    noneSelectedText: "Select an Option",
                    selectedList: 1,
                    minWidth: 145,
                    height: 100
                });
            } else {
                $el.multiselect({
                    multiple: false,
                    header: false,
                    noneSelectedText: "Select an Option",
                    selectedList: 1,
                    minWidth: 145,
                    height: 100
                });
            }
        } else {
            if ($el.hasClass('width-140')) {
                $el.multiselect({
                    minWidth: 145,
                    multiple: true,
                    checkAllText: 'All',
                    uncheckAllText: 'Uncheck'
                });
            } else {
                $el.multiselect();
            }
        }
    });
};

$.fn.createSearchPanel = function () {
    $(this).each(function (index, element) {
        var $el = $(element);
        var $dialog = $el.find("#info-dialog").dialog({
            autoOpen: false
        });
        $el.find("#info").click(function (e) {
            e.stopPropagation();
            $dialog.dialog("open");
        });

        $el.find("#excel").click(function (e) {
            e.stopPropagation();
            alert("excel click");
        });

        var $contentPanel = $($el.attr("data-selector"));
        if ($contentPanel.length > 0) {
            $el.click(function () {
                var collapse = $(".search-title .collapse"),
                    colHeight = $("#gridId").attr('colHeight'),
                    expHeight = $("#gridId").attr('expHeight');
                collapse.addClass("expand");

                // keep width of datagrid
                if (!$(".search-form").is(":visible")) {
                    // resize height of datagrid
                    $("#gridId").fanexGrid('resizeHeight', expHeight);
                }

                $contentPanel.stop().slideToggle(function () {
                    if ($(".search-form").is(":visible")) {
                        collapse.removeClass("expand");                        
                    } else {
                        // resize height of datagrid
                        $("#gridId").fanexGrid('resizeHeight', colHeight);
                    }
                });
            });
        }
    });
};

$.fn.pVal = function() {
    var $this = $(this),
        val = $this.eq(0).val();
    if (val === $this.attr('placeholder'))
        return '';
    else
        return val;
};
Array.prototype.clear = function () {
    while (this.length > 0) {
        this.pop();
    }
};