/*global MSG,moment,subTotalStatus,Lang,moment,COLLAPSE_SUBTOTAL_STATUS,sysGmt,sysYear,sysMonth,sysDay,sysHour,sysMinute,sysSecond,sysMonthName*/
var AFFILIATE = (function ($) {
    "use strict";

    if (window.sysGmt !== undefined) {
        var systemGmt = window.sysGmt;
        var systemYear = window.sysYear;
        var systemMonth = window.sysMonth;
        var systemDay = window.sysDay;
        var systemHour = window.sysHour;
        var systemMinute = window.sysMinute;
        var systemSecond = window.sysSecond;
        var systemMonthName = window.sysMonthName;
    }
    var _isIT = false;

    return {
        pageSizes: [20, 50, 100],
        DATE_FORMAT: "MM/DD/YYYY",
        DATE_FORMAT_FULL: "MM/DD/YYYY HH:mm:ss",
        DATE_FORMAT_MONTH: "MM/YYYY",
        DATE_FORMAT_YEAR: "YYYY",
        FORMATNUMBER: '0,0.00',
        FORMAT_INT_NUMBER: '0,0',
        MIN_CONTENT_WIDTH: '1110',
        scrollTop: function () {
            $('html, body').animate({ scrollTop: $('.container-flsuid').offset().top }, 400);
        },
        init: function(){
            _isIT = $("#IsIT").val() === '1';
        },
        initDatePicker: function () {
            var datePickers = $('.datepicker');

            if (datePickers.length > 0) {
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

                datePickers.each(function () {
                    var $this = $(this);

                    $this.datepicker({
                        defaultDate: new Date($this.val()),
                        dateFormat: "dd M yy",
                        minDate: limitDate($this, "data-mindate"),
                        maxDate: limitDate($this, "data-maxdate"),
                        beforeShow: function () {
                            //inst.dpDiv.css({ marginTop: '2px' });
                        },
                        onSelect: function (dateText) {
                            $(this).attr('data-value', dateText);
                        }
                    });

                    $this.next('.icon-calendar').click(function () {
                        $this.datepicker("show");
                    });
                });
            }
        },
        isIT: function () {
            return _isIT;
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
            options.closeText = window.Lang.Close;

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
            systemSecond++;
            if (systemSecond === 60) {
                systemSecond = 0;
                systemMinute++;
            }
            if (systemMinute === 60) {
                systemMinute = 0;
                systemHour++;
            }
            if (systemHour === 24) {
                systemHour = 0;
                systemSecond = 0;
                systemMinute = 0;
                systemDay++;
            }

            var strMin;
            if (systemMinute < 10) {
                strMin = "0" + systemMinute;
            }
            else {
                strMin = systemMinute;
            }

            var strSec;
            if (systemSecond < 10) {
                strSec = "0" + systemSecond;
            }
            else {
                strSec = systemSecond;
            }

            if (!AFFILIATE.isValidDate(systemYear, systemMonth, systemDay)) {
                systemMonth++;
                systemDay = 1;
            }
            if (!AFFILIATE.isValidDate(systemYear, systemMonth, systemDay)) {
                systemYear++;
                systemMonth = 1;
                systemDay = 1;
            }

            var strHrs;
            if (systemHour < 10) {
                strHrs = "0" + systemHour;
            }
            else {
                strHrs = systemHour;
            }
            $('#lblDateTime').html(strHrs + ":" + strMin + ":" + strSec + " " + systemDay
                + "-" + systemMonthName[systemMonth - 1] + "-" + systemYear + systemGmt);
        },

        /*jshint ignore:start */
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
                            CustomCellRender: AFFILIATE.getCustomCellRender(rIndex, rowData, col)
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

        /*jshint ignore:end */

        getCustomCellRender: function (rowIndex, rowData, col) {
            var customCellRender = function (cell, cellFormated) {

                if (col.Formatter) {
                    return col.Formatter(rowIndex, rowData, cell, cellFormated);
                }

                return cellFormated;
            };

            return customCellRender;
        },

        /*jshint ignore:start */
        genGridData: function (columns, list, rowCssClass) {
            var bodyRows = [];
            var len = list.length;
            for (var i = 0; i < len; i++) {
                var record = list[i];
                var dataRow = [];

                for (var j = 0; j < columns.length; j++) {
                    var colName = columns[j].Name;
                    var col = columns[j];
                    dataRow.push({
                        Name: colName,
                        Value: record[colName],
                        CustomCellRender: AFFILIATE.getCustomCellRender(0, record, col)
                    });
                }

                bodyRows.push({
                    Columns: dataRow,
                    CssClass: rowCssClass ? rowCssClass(record) : undefined
                });
            }
            return bodyRows;
        },
        /*jshint ignore:end */
        showUpdateStatus: function (errCode, msg, callback, timeout) {
            if (errCode === 0) {
                AFFILIATE.showSuccessMsg(msg);

                if (callback) {
                    callback(0, parent.$("#itemperpage").val()); //Refresh parent page
                }

                setTimeout(function () { AFFILIATE.refreshParent(); }, timeout || 3000);
            }
            else {
                AFFILIATE.showErrorMsg(msg);

                if (callback) {
                    callback(0, parent.$("#itemperpage").val());
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
            AFFILIATE.openDialogFrame("Account/ChangePassword", window.Lang.ChangePassword, { width: 450, height: 300 });
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
            var height = 0;
            if ($(".search-form").is(":visible")) {
                height = $("#gridId").data('expheight') || gridOption.gridExpandHeight;
            } else {
                height = $("#gridId").data('colheight');
            }

            gridOption.gridExpandHeight = height;
            return gridOption;
        },
        setWidthOfFormSearch: function () {
            var searchPanel = $('.search-panel'), dataGrid = $('.freegrid');
            // Set default width for form search with width of datagrid
            if (searchPanel !== undefined && dataGrid !== undefined) {
                if (dataGrid.width() > searchPanel.width()) {
                    searchPanel.width(dataGrid.width());
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
                    'monthpicker-hide': function () {
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
        PreHandleAjaxResult: function (data) {
            if (typeof data === "object") {
                if (data.hasOwnProperty("Error") && data.hasOwnProperty("LogOnUrl")) {
                    window.top.location.href = AFFILIATE.getAbsoluteUrl(data.LogOnUrl);

                }
            }
        },
        getBaseUrl: function () {
            var protocol = location.protocol;
            var host = location.host;

            return "{0}://{1}".format(protocol, host);
        },
        resizeGridHeight: function () {
            var colHeight = $("#gridId").data('colheight'),
            expHeight = $("#gridId").data('expheight');

            if ($(".search-form").is(":visible")) {
                $("#gridId").fanexGrid('resizeHeight', expHeight);
            } else {
                $("#gridId").fanexGrid('resizeHeight', colHeight);
            }
        }
    };
})(jQuery);

$(function () {
    "use strict";
    AFFILIATE.init();

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
            var submitOptions = $('form').serializeObject2();
            submitOptions["ExcludeTest"] = $('#ExcludeTest').prop('checked');
            var url = location.href + "?" + $.param(submitOptions);

            parent.AFFMASTER.openNewWindow(url);
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
        setInterval(function () { AFFILIATE.clock(); }, 1000);
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

    $("form").submit(function () {
        AFFILIATE.trimBeforeSubmit();
    });

    jQuery.browser = {};
    (function () {
        jQuery.browser.msie = false;
        jQuery.browser.version = 0; if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
            jQuery.browser.msie = true; jQuery.browser.version = RegExp.$1;
        }
    })();
});

/*jshint ignore:start */

Date.prototype.toShortDate = function () {
    "use strict";
    return (this.getMonth() + 1) + "/" + this.getDate() + "/" + this.getFullYear();
};

String.prototype.isNumber = function () {
    "use strict";
    var regs = new RegExp(/^-?\d+(\.\d+)?\s*$/);
    return (regs.test(this));
};

String.prototype.removeLast = function (count) {
    "use strict";
    if (!count) {
        count = 1;
    }
    return this.substring(0, this.length - count);
};

if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (obj, start) {
        "use strict";
        for (var i = (start || 0), j = this.length; i < j; i++) {
            if (this[i] === obj) {
                return i;
            }
        }
        return -1;
    };
}

// First, checks if it isn't implemented yet.
if (!String.prototype.format) {
    String.prototype.format = function () {
        "use strict";
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
        "use strict";
        return this.replace(/^\s+|\s+$/g, '');
    };
}
if (!String.prototype.endsWith) {
    String.prototype.endsWith = function (suffix) {
        "use strict";
        return this.indexOf(suffix, this.length - suffix.length) !== -1;
    };
}
if (!Array.prototype.clone) {
    Array.prototype.clone = function () {
        "use strict";
        return this.slice(0);
    };
}
$.fn.serializeObject = function () {
    "use strict";
    var o = {};
    // find disabled inputs
    var inputs = this.find(':disabled');
    // enable them
    inputs.prop('disabled', false);

    var a = this.serializeArray();

    // then, disable them
    inputs.prop('disabled', true);

    $.each(a, function () {
        if (o[this.name] === undefined || o[this.name] === null) {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

Array.prototype.clear = function () {
    "use strict";
    while (this.length > 0) {
        this.pop();
    }
};

Array.prototype.removeItem = function (propertyName, propertyValue) {
    "use strict";
    var i = 0;
    while (i < this.length) {
        if(this[i][propertyName] == propertyValue) {
            this.splice(i, 1);
        }
        else
        {
            i++;
        }
    }
};

/*jshint ignore:end */

$.fn.serializeObject2 = function () {
    "use strict";
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
    "use strict";
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
    "use strict";
    $(this).each(function (index, element) {
        var $el = $(element);
        var translateOptions = {
            noneSelectedText: window.Lang.SelectAnOption,
            checkAllText: window.Lang.All,
            uncheckAllText: window.Lang.Uncheck,
            selectedText: "# " + window.Lang.Selected
        };
        
        if ($el.hasClass('single-select')) {
            if ($el.hasClass('width-140')) {
                $el.multiselect($.extend({
                    multiple: false,
                    header: false,
                    selectedList: 1,
                    minWidth: 145,
                    height: 100
                }, translateOptions));
            } else {
                $el.multiselect($.extend({
                    multiple: false,
                    header: false,
                    selectedList: 1,
                    minWidth: 145,
                    height: 100
                }, translateOptions));
            }
        } else {
            if ($el.hasClass('width-140')) {
                $el.multiselect($.extend({
                    minWidth: 145,
                    multiple: true,
                }, translateOptions));
            } else {
                $el.multiselect(translateOptions);
            }
        }
    });
};

$.fn.createSearchPanel = function () {
    "use strict";
    $(this).each(function (index, element) {
        var $el = $(element);
        var $dialog = $el.find("#info-dialog").hide();
        $el.find("#info").click(function (e) {
            e.stopPropagation();
            $dialog.show();
        });

        $dialog.find("#info-close").click(function (e) {
            e.stopPropagation();
            $dialog.hide();
        });

        $dialog.on("click", "#info-close", function (e) {
            e.stopPropagation();
            $dialog.hide();
        });

        $el.find("#excel").click(function (e) {
            e.stopPropagation();

            alert("excel click");
        });

        var $contentPanel = $($el.attr("data-selector"));
        if ($contentPanel.length > 0) {
            $el.click(function () {
                var collapse = $(".search-title .collapse"),
                    colHeight = $("#gridId").data('colheight'),
                    expHeight = $("#gridId").data('expheight');
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

        $dialog.click(function (e) {
            e.stopPropagation();
        });
    });
};

$.fn.pVal = function () {
    "use strict";
    var $this = $(this),
        val = $this.eq(0).val();
    if (val === $this.attr('placeholder')) {
        return '';
    } else {
        return val;
    }
};
