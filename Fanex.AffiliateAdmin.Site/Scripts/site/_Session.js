$(function () {
    var baseUrl = $("#BaseUrl").val();
    var lastUpdate = 0;
    var url = (window.location != window.parent.location)
    ? document.referrer
    : document.location.href;
    var sessionUrl = url + "/ExternalApi/SessionIsAlive";
    var internalSessionUrl = baseUrl + 'OneAuthorization/SessionIsAlive';
    var internalSignOutUrl = baseUrl + 'Notify/OneTokenRestriction?code=400';
    setInterval(function () {
        $.getJSON(sessionUrl, function (data) {
            if (data === true) {
                $.getJSON(internalSessionUrl, function (data) {
                    if (data === false) {
                        document.location.href = internalSignOutUrl;
                    }
                });
            }
        });
    }, 10000);
});