$(function () {
    var baseUrl = $("#BaseUrl").val();
    var lastUpdate = 0;
    var url = (window.location != window.parent.location)
    ? document.referrer
    : document.location.href;
    var sessionUrl = url + "/ExternalApi/SessionIsAlive";
    var internalSessionUrl = baseUrl + 'Utility/SessionIsAlive';
    var internalSignOutUrl = baseUrl + 'Utility/InternalSignOut';
    var checkInterval = setInterval(function () {
        if (new Date().getTime() - lastUpdate > 3000) {
            clearInterval(checkInterval);
        } else {
            $.getJSON(sessionUrl, function (data) {
                if (data === true) {
                    $.getJSON(internalSessionUrl, function (data) {
                        if (data === false) {
                            document.location.href = internalSignOutUrl;
                        }
                    });
                }
            });
        }
    }, 3000);

    $(document).keydown(function () {
        lastUpdate = new Date().getTime();
    });
});