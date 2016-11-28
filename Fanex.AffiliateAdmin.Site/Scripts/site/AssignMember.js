/*global AFFILIATE,gridOption,MSG*/

var AssignMember = (function ($) {
    "use strict";
    return {
        init: function() {
            $("form").submit(function() {
                var data = $(".popup-content input").serializeData();
                if (!data.MemberName) {
                    AFFILIATE.showErrorMsg(MSG.InputMem);
                    return false;
                }
                if (!data.AffName) {
                    AFFILIATE.showErrorMsg(MSG.InputAff);
                    return false;
                }
            });
        }
    };
})(jQuery);
