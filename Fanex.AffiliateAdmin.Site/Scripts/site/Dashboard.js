var DASHBOARD = (function ($) {
    return {
        init: function() {
            $("#btnAlert").click(function() {
                DASHBOARD.TestMultiple();
            });

            // Event click on product link
            $("#toggleOnlineList").click(function() {
                DASHBOARD.ToggleOnlineList();
            });
        },
        ToggleOnlineList: function() {
            var onlineList = $("#gridId");
            var icon = $("#toggleOnlineList").children();

            if (icon.hasClass("icon-plus")) {
                icon.removeClass("icon-plus").addClass("icon-minus");
                onlineList.toggle("blind");
            } else {
                icon.removeClass("icon-minus").addClass("icon-plus");
                onlineList.toggle("blind");
            }
        },
        TestMultiple: function() {
            alert($('#s5').val());
        }
    };
}) (jQuery);
$(function () {
    DASHBOARD.init();
});