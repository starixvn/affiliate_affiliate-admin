$(function () {
    var baseUrl = $("#BaseUrl").val();
    var internalSessionUrl = baseUrl + 'OneAuthorization/SessionIsAlive';
    var internalSignOutUrl = baseUrl + 'Notify/OneTokenRestriction?code=400';
    setInterval(function () {
        $.getJSON(internalSessionUrl, function (data) {
            if (data === false) {
                document.location.href = internalSignOutUrl;
            }
        });
    }, 30000);
});