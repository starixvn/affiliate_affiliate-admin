/*global MARKETING,QueryString*/

var paging = function() {
    "use strict";
    this.pageGo = function(index) {
        var pageSize = $("select.items-per-page option:selected").val();
        var url = location.href;
        url = QueryString.setParam(url, "pagesize", pageSize);
        url = QueryString.setParam(url, "pageindex", index);
        if (typeof(MARKETING) !== 'undefined') {
            url = MARKETING.getUrl(url);
        }
        location.href = url;
    };
};

$(window).load(function () {
    "use strict";

    var pagingControl = new paging();
    var currentPage = parseInt($("div.paging-info input.pagingCurrent").val(), 10);

    $("div.paging-info input.pagingFirst").click(function () {
        pagingControl.pageGo(1);
    });
    $("div.paging-info input.pagingPrev").click(function () {
        if (currentPage > 1) {
            pagingControl.pageGo(currentPage - 1);
        }
    });
    $("div.paging-info input.pagingLast").click(function () {
        pagingControl.pageGo(parseInt($(".pagingCurrent").attr('total-item'), 10));
    });
    $("div.paging-info input.pagingNext").click(function () {
        pagingControl.pageGo(currentPage + 1);
    });

    $("select.items-per-page").change(function () {
        pagingControl.pageGo(1);
    });

    $("div.paging-info input.pagingCurrent").keyup(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code === 13) {
            var value = parseInt($(this).val());
            var limit = parseInt($(this).attr('total-item'), 10);
            if (value > 0 && value <= limit) {
                $(this).val(value);
                pagingControl.pageGo(value);
            }
        }
    });

    // Allow only numbers in TextBox
    $("div.paging-info input.pagingCurrent").keypress(function (e) {
        var charCode = (e.which) ? e.which : e.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    });
});

