var TestWebService = function ($) {
    var submitOptions = {};

    function showHide(selectedValue) {
        $("#form1 tr.tab").hide().find("input").attr("disabled", true);
        $("#form1 tr.tab[id=" + selectedValue + "]").show().find("input").removeAttr("disabled");
    }

    return {
        init: function () {
            showHide($("#method").val());

            $("#method").change(function () {
                showHide($(this).val());

            });

            $('#submit').click(function () {
                var paras = $('form').serialize();

                $.ajax({
                    url: AFFILIATE.getAbsoluteUrl("TestWebService/CallWebSerive"),
                    type: 'post',
                    data: paras,
                    success: function (result) {
                        AFFILIATE.PreHandleAjaxResult(result);
                        
                        $('#result').text(result);
                    }
                });
            })
        }
    };
} (jQuery);

$(function () {
    TestWebService.init();
});