/*global AFFILIATE,MSG*/
var AFFEDIT = (function ($) {
    "use strict";
    var inValidEle = null;

    function isValidTextBox(text) {
        var regex = new RegExp("[~!#$%^&*();\\[\\]<>{}\\|]", "g");

        if (regex.test(text)) {
            return false;
        }

        return true;
    }

    function showErrorMessage(message) {
        AFFILIATE.showErrorMsg(message);

        $("input[type=text]").each(function () {
            $(this).removeClass("input-validation-error");
        });

        $(inValidEle).addClass("input-validation-error");
    }

    return {
        init: function() {
            $('#btnAddSite').click(function() {
                AFFEDIT.addUrl();
            });
            $('#btnSubmit').click(function() {
                if (!AFFEDIT.validateTextBox()) {
                    showErrorMessage(MSG.DontEnterSpecialCharacters);
                    return false;
                }

                AFFEDIT.submitForm();
            });
        },

        validateTextBox: function() {
            var isValidURL = true;

            $("input[type=text]").each(function() {
                var value = $.trim($(this).val());
                isValidURL = isValidTextBox(value);

                if (!isValidURL) {
                    inValidEle = this;
                    return false;
                }
            });

            return isValidURL;
        },
        addUrl: function () {
            /*jshint ignore:start */
            var sites = $(".sites");
            var numberSite = sites.length;
            if (sites[numberSite - 1]) {
                if ($(sites[numberSite - 1]).val() === '') {
                    alert(MSG.MoreSite + numberSite);
                    sites[numberSite - 1].focus();
                    return;
                }
            }

            numberSite++;
            var rowHtml = "<tr id='site_{0}'>".format(numberSite) +
                "<td><label class=\"control-label\" for=\"Affiliate_SiteURL_{0}\">".format(numberSite) + MSG.SiteUrl + ' ' + numberSite + "</label></td>" +
                "<td></td>" +
                "<td><input type='text' class='sites more-site control-text valid' id='Affiliate_SiteURL_{0}' maxlength = '50'/></td>".format(numberSite) +
                "<td><button type='button' class='btn btn-default' onclick='AFFEDIT.delUrl(this);' style='vertical-align: top'>" + MSG.Delete + "</button></td>" +
                "</tr>";
            $("#siteinfo").append(rowHtml);

            if ($("#more_" + numberSite)) {
                $("#more_" + numberSite).focus();
            }
            /*jshint ignore:end */
        },
        delUrl: function(obj) {
            var index = $(obj).parents("tr").attr("id").replace("site_", "");
            $("#site_" + index).remove();
            AFFEDIT.reOrderRows();
        },
        reOrderRows: function() {
            var rows = $("#siteinfo").find("tr");
            for (var rowIndex = 1; rowIndex < rows.length; rowIndex++) {
                var index = (rowIndex + 1);
                $(rows[rowIndex]).attr("id", 'site_' + index);
                var labelEle = $(rows[rowIndex]).find("label").filter(":first");
                labelEle.attr("for", "Affiliate_SiteURL_" + index);
                labelEle.text(MSG.SiteUrl + ' ' + index);
                $(rows[rowIndex]).find("input[type='text']").filter(":first").attr("id", "Affiliate_SiteURL_" + index);
            }
        },
        submitForm: function() {
            // find disabled inputs
            var inputs = $('form').find(':disabled');
            // enable them
            inputs.prop('disabled', false);

            var moreSiteUrls = '';
            $(".more-site").each(function() {
                if (moreSiteUrls !== '') {
                    moreSiteUrls += ';';
                }
                moreSiteUrls += $.trim($(this).val());
            });
            $("#moreSiteUrls").val(moreSiteUrls);
        },
        hideMsg: function() {
            setTimeout(AFFILIATE.hideMsg, 3000);
        }
    };
}) (jQuery);

$(function () {
    "use strict";
    AFFEDIT.init();
});