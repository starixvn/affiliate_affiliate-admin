/*global AFFILIATE*/
var UpdatePayment = (function ($) {
    "use strict";

    function disableUpdateButtonByStatus() {
        if (!$('.rdStatus:checked').length) {
            $('#btnUpdate').prop('disabled', true);
        }
    }

    var _me = {
        init: function () {
            disableUpdateButtonByStatus();

            $('#btnUpdate').on("click", function () {
                $(this).prop("disabled", true);
                $.blockUI();
                $("#paymentRequestForm").submit();
                return false;
            });

            $('.rdStatus').on("change", function () {
                var disabledUpdateButton = $('#btnUpdate').prop('disabled');

                if (disabledUpdateButton) {
                    $('#btnUpdate').prop('disabled', false);
                }
            });
        }
    }

    return _me;
})(jQuery);

$(function () {
    "use strict";
    UpdatePayment.init();
});