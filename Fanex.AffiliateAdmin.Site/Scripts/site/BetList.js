/* global AFFILIATE, QueryString */
/* exported showCombMP, showCombMPDetail, showMP, ViewResult */
"use strict";

var BetList = (function ($) {
    return {
        initialize: function () {
        },

        getMixParlayResultUrl: function (refno, winlostdate, bettype, username) {
            var url = AFFILIATE.getAbsoluteUrl('BetList/BetListResults');
            url = QueryString.setParam(url, "refno", refno);
            url = QueryString.setParam(url, "winlossdate", winlostdate);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "matchid", 1);
            url = QueryString.setParam(url, "username", username);

            return url;
        },

        getSportBookResultUrl: function (matchid, bettype, raceNo, sporttype, league, isOutright, username) {
            var url = AFFILIATE.getAbsoluteUrl('BetList/BetListResults');
            url = QueryString.setParam(url, "matchid", matchid);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "raceno", raceNo);
            url = QueryString.setParam(url, "sporttype", sporttype);
            url = QueryString.setParam(url, "isoutright", isOutright);
            url = QueryString.setParam(url, "username", username);

            return url;
        },

        showResult: function (matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright) {
            var url = bettype === 9 || bettype === 29
                    ? BetList.getMixParlayResultUrl(refnoMixParlay, winlostdate, bettype, username)
                    : BetList.getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright, username);

            var title = "Results";
            AFFILIATE.openDialogUrl(url, title, { width: 720, height: 400 });
        },

        showMixParlayDetail: function (transId, divEvent) {
            var url = AFFILIATE.getAbsoluteUrl('BetList/BetListParlay');
            var para = $('form').serializeObject2();
            url = QueryString.setParam(url, "transId", transId);
            url = QueryString.setParam(url, "exchangeRate", para.ExchangeRate);

            if (divEvent.css("display") === 'none') {
                $.ajax({
                    type: "GET",
                    contentType: 'application/json',
                    dataType: "text",
                    cache: false,
                    url: url,
                    error: function () {
                    },
                    success: function (response) {
                        divEvent.html(response);
                        divEvent.css("display", "");
                    },
                    beforeSend: function () {

                    },
                    complete: function () {
                    }
                });
            }
            else {
                divEvent.css("display", "none");
                divEvent.html("");
            }
        },

        showSystemParlayDetail: function (transId, refNo, betId, custId, betTeam, baseCurrency) {
            if (transId === 0) {
                return;
            }

            var url = AFFILIATE.getAbsoluteUrl('BetList/SystemParlayDetail');
            var para = $('form').serializeObject2();
            url = QueryString.setParam(url, "transId", transId);
            url = QueryString.setParam(url, "exchangeRate", para.ExchangeRate);
            url = QueryString.setParam(url, "refno", refNo);
            url = QueryString.setParam(url, "betid", betId);
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "betteam", betTeam);
            url = QueryString.setParam(url, "basecurrency", baseCurrency);

            var height = 450;
            var width = 750;

            var dialogForm = '<div title="' + betTeam + '"><iframe frameborder="0" width="' + width;
            dialogForm += '" height="' + height + '" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: width + 25,
                height: height + 50
            });
        },

        ViewCasinoHandHistory: function (refNo) {
            var url = AFFILIATE.getAbsoluteUrl('BetList/RNGCasinoData');
            var para = $('form').serializeObject2();


            var urlParam = $("#casino-hand-history-" + refNo).attr('url-param');
            url += urlParam;
            url = QueryString.setParam(url, "exchangeRate", para.ExchangeRate);
            url = QueryString.setParam(url, "toCurrencyId", para.ToCurrencyId);
            url = QueryString.setParam(url, "toCurrencyCode", para.ToCurrencyCode);

            var title = "Results";
            AFFILIATE.openDialogUrl(url, title, { width: 1097, height: 500 });
        }
    };
})(jQuery);

var ViewResult = BetList.showResult;

var showMP = function (transId) {
    var divEvent = $('#divEvent_' + transId);
    divEvent.addClass("mx-parlay"); //to make line-through for void-parlay-ticket
    BetList.showMixParlayDetail(transId, divEvent);
};

var showCombMP = function (transId, index) {
    var divEvent = $('#divEvent_' + transId + "" + index);
    divEvent.addClass("mx-parlay"); //to make line-through for void-parlay-ticket
    BetList.showMixParlayDetail(transId, divEvent);
};

var showCombMPDetail = BetList.showSystemParlayDetail;