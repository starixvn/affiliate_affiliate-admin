/* global AFFILIATE, QueryString */
/* exported showCombMP, showCombMPDetail, showMP, showRnGCasinoDetails, ViewResult */
"use strict";

var BetList = (function ($) {
    var SPORT_RNG_GAME = 208;
    var SPORT_RNG_MINIGAME = 209;
    var SPORT_RNG_MOBILE = 210;

    return {
        initialize: function () {
        },

        getMixParlayResultUrl: function (refno, winlostdate, bettype, username) {
            var para = $('form').serializeObject2();
            var url = AFFILIATE.getAbsoluteUrl('BetList/BetListResults');
            url = QueryString.setParam(url, "refno", refno);
            url = QueryString.setParam(url, "winlossdate", winlostdate);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "matchid", 1);
            url = QueryString.setParam(url, "username", username);
            url = QueryString.setParam(url, "affId", para.AffId);

            return url;
        },

        getSportBookResultUrl: function (matchid, bettype, raceNo, sporttype, league, isOutright, username) {
            var para = $('form').serializeObject2();
            var url = AFFILIATE.getAbsoluteUrl('BetList/BetListResults');
            url = QueryString.setParam(url, "matchid", matchid);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "raceno", raceNo);
            url = QueryString.setParam(url, "sporttype", sporttype);
            url = QueryString.setParam(url, "isoutright", isOutright);
            url = QueryString.setParam(url, "username", username);
            url = QueryString.setParam(url, "affId", para.AffId);

            return url;
        },

        showResult: function (matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay,
                                league, isOutright, betid, betcheck, custid) {
            var title = MSG.Results;
            if (sporttype === SPORT_RNG_GAME || sporttype === SPORT_RNG_MINIGAME || sporttype === SPORT_RNG_MOBILE) {
                BetList.viewRngCasinoResult(title, refno);
            }
            else {
                var url = bettype === 9 || bettype === 29
                        ? BetList.getMixParlayResultUrl(refnoMixParlay, winlostdate, bettype, username)
                        : BetList.getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright, username);
              
                AFFILIATE.openDialogUrl(url, title, { width: 720, height: 400 });
            }
        },

        viewRngCasinoResult: function (title, refno) {
            var url = AFFILIATE.getAbsoluteUrl('BetList/RngCasinoSingleWalletResult');

            var params = { "refNo": refno };
            $.ajax({
                type: "POST",
                contentType: 'application/json',
                dataType: "json",
                data: JSON.stringify(params),
                cache: false,
                url: url,
                success: function (response) {
                    if (response.Success) {
                        BetList.showRngCasinoResultPopup(title, response.Result);
                    }
                },
                beforeSend: function () {
                    $.blockUI();
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        },

        showRngCasinoResultPopup: function (title, url) {
            if (url === "") {
                return;
            }

            var dialogForm = '<div title="' + title + '"><iframe frameborder="0" style="width:100%; height:99%;" src="' + url + '" ></iframe></div></div>';
            $(dialogForm).dialog({
                modal: true,
                width: 800,
                height: 450
            });
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

        showNewParlayDetail: function (transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam) {
            var $detaillDiv = $('#divEvent_' + transId + "" + transIndex);
            var $nameDiv = $('#combinationOfferName_' + transId + transIndex);
            $detaillDiv.addClass("mx-parlay"); //to make line-through for void-parlay-ticket

            if ($detaillDiv.is(':hidden')) {
                var para = $('form').serializeObject2();
                var url = AFFILIATE.getAbsoluteUrl('Account/NewBetListParlay');
                url = QueryString.setParam(url, "transId", transId);
                url = QueryString.setParam(url, "refno", refNo);
                url = QueryString.setParam(url, "exchangeRate", para.ExchangeRate);
                url = QueryString.setParam(url, "handicap1", handicap1);
                url = QueryString.setParam(url, "handicap2", handicap2);
                url = QueryString.setParam(url, "betcheck", betcheck);
                url = QueryString.setParam(url, "betteam", betteam);
                $.ajax({
                    type: "GET",
                    contentType: 'application/json',
                    dataType: "text",
                    cache: false,
                    url: url,
                    error: function () {
                    },
                    success: function (response) {
                        $detaillDiv.html(response).show();
                        $nameDiv.hide();
                    },
                    beforeSend: function () {

                    },
                    complete: function () {
                    }
                });
            }
            else {
                $detaillDiv.hide().html("");
                $nameDiv.show();
            }
        },

        ViewCasinoHandHistory: function (refNo) {
            var url = AFFILIATE.getAbsoluteUrl('BetList/RNGCasinoData');
            var para = $('form').serializeObject2();

            var urlParam = $("#casino-hand-history-" + refNo).attr('url-param');
            url += urlParam;
            url = QueryString.setParam(url, "exchangeRate", para.ExchangeRate);
            url = QueryString.setParam(url, "toCurrencyId", para.ToCurrencyId);
            url = QueryString.setParam(url, "toCurrencyCode", para.ToCurrencyCode);
            url = QueryString.setParam(url, "affId", para.AffId);

            var title = MSG.Results;
            AFFILIATE.openDialogUrl(url, title, { width: 1097, height: 500 });
        },

        showRnGCasinoDetails: function (transId, winlostDate, custId, transType) {
            var url = AFFILIATE.getAbsoluteUrl('BetList/RngCasinoDetailTicket');
            url = QueryString.setParam(url, "transId", transId);
            url = QueryString.setParam(url, "custId", custId);

            var popH = Math.min($(window).height() - 100, 550), popW = 1020;
            var title = MSG.TitleRngCasinoTicketDetail;
            var dialogForm = '<div title="' + title + '"><iframe scrolling="yes" frameborder="0" width="' +
                             popW + '" height="' + popH + '" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 10,
                height: popH + 55
            });
        },

        showAGCasinoDetails: function (transId, winlostDate, custId, transType) {
            var url = AFFILIATE.getAbsoluteUrl('BetList/AgCasinoDetailTicket');
            url = QueryString.setParam(url, "transid", transId);
            url = QueryString.setParam(url, "custid", custId);

            var popH = Math.min($(window).height() - 100, 550), popW = 1020;
            var title = MSG.TitleAgCasinoTicketDetail;
            var dialogForm = '<div title="' + title + '"><iframe scrolling="no" frameborder="0" width="' +
                             popW + '" height="99%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 10,
                height: popH + 50
            });
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
var showRnGCasinoDetails = BetList.showRnGCasinoDetails;
var showAGCasinoDetails = BetList.showAGCasinoDetails;var showCombMPDetail = BetList.showSystemParlayDetail;
var showNewCombMP = BetList.showNewParlayDetail;