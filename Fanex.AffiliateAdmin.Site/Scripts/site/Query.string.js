var QueryString = (function ($) {
    "use strict";

    var parseQueryString = function (queryString) {
        var args = queryString.split('&');
        var argsList = {};

        for (var i = 0; i < args.length; i++) {
            var arg = unescape(args[i]);
            if (arg.length > 0) {
                if (arg.indexOf('=') === -1) {
                    argsList[$.trim(arg.toLowerCase())] = null;
                }
                else {
                    var keyValuePair = arg.split('=');
                    argsList[$.trim(keyValuePair[0].toLowerCase())] = $.trim(keyValuePair[1]);
                }
            }
        }

        return argsList;
    };

    var getQueryString = function (url) {
        var index = url.indexOf("?");
        return index < 0 ? "" : url.substring(index + 1, url.length);
    };

    var getRawUrl = function (url) {
        var index = url.indexOf("?");
        return index < 0 ? url : url.substring(0, index);
    };

    function getParam(url, name) {
        var queryString = getQueryString(url);
        var args = parseQueryString(queryString);

        for (var key in args) {
            if (key.toLowerCase() === name.toLowerCase()) {
                return args[key];
            }
        }

        return null;
    }

    return {
        getQueryString: getQueryString,
        getRawUrl: getRawUrl,
        setParam: function(url, name, value) {
            var rawUrl = getRawUrl(url);
            var queryString = getQueryString(url);
            var args = parseQueryString(queryString);

            if (name === null || name === undefined) {
                return url;
            }

            // Update value
            var found = false;
            var key;
            
            for (key in args) {
                if (key.toLowerCase() === name.toLowerCase()) {
                    // Does not update data if value = undefined
                    if (value !== undefined) {
                        args[key] = value;
                    }
                    found = true;
                }
            }

            if (!found) {
                args[name.toLowerCase()] = value;
            }

            var queryArray = [];

            // Rebuild query string, ignore null value
            for (key in args) {
                if (args[key] !== null && args[key] !== undefined) {
                    queryArray.push(key.toLowerCase() + "=" + args[key]);
                }
            }

            return rawUrl + "?" + queryArray.join("&");
        },
        getCurrentParam: function(name) {
            return getParam(window.location.href, name);
        },
        getParam: getParam
    };
})(jQuery);