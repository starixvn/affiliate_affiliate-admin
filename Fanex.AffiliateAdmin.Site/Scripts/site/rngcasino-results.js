// ----------------------------------------------------------------------
// <copyright file="RNGCasinoResults.js" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/10">Created</change>
//   <change who="" date=""></change>
// </history>

/*global AFFILIATE, QueryString*/
"use strict";

var RNGCasinoResults = (function ($) {
    return {
        initialize: function () {
            RNGCasinoResults.initEvent();
        },

        initEvent: function () {
            $("body").on("click", ".hand-history-detail-link", function () {
                var url = AFFILIATE.getAbsoluteUrl('Account/RNGCasinoDetail');
                url += $(this).attr("data-url");
                RNGCasinoResults.viewCasinoDetail(url);
            });

            $("#chkIsShowPic").on('click', function () {
                var isShowPic = $("#chkIsShowPic").is(':checked');
                var url = window.location.href;
                url = QueryString.setParam(url, "IsShowPic", isShowPic);
                window.location.href = url;
            });
        },

        viewCasinoDetail: function (url) {
            var title = "Hand History Details";
            AFFILIATE.openDialogFrame(url, title, { width: 900, height: 450 });
        }
    };
})(jQuery);

$(window).load(function () {
    RNGCasinoResults.initialize();
});