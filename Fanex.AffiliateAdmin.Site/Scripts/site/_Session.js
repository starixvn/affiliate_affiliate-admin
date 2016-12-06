$(function () {
    var baseUrl = $("#BaseUrl").val();
    var lastUpdate = 0;
    var internalSessionUrl = baseUrl + 'OneAuthorization/SessionIsAlive';
    var internalSignOutUrl = baseUrl + 'Notify/OneTokenRestriction?code=400';
    var checkInterval = setInterval(function () {
       if (new Date().getTime() - lastUpdate >= 900000) {
            $.getJSON(internalSessionUrl, function (data) {
                if (data === false) {
                    document.location.href = internalSignOutUrl;
                }
            });
        }
    }, 180000);
    $(document).keydown(function () {
        lastUpdate = new Date().getTime();
    });
});