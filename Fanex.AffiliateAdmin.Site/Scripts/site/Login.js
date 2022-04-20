/*global CryptoJS*/
var LOGIN = (function ($) {
    "use strict";
    var _rootUrl;

    return {
        init: function () {
            _rootUrl = $("#RootUrl").val();

            if ($("#Username").val() === "") {
                $("#Username").focus();
            } else {
                $("#Password").focus();
            }

            // Event click login
            $("#btnlogin").click(function () {
                LOGIN.submitForm();
            });

            $('.login input').bind('keypress', function (e) {
                if (e.keyCode === 13) {
                    LOGIN.submitForm();
                }
            });
        },
        getAbsoluteUrl: function (relativeUrl) {
            return _rootUrl + (relativeUrl || "");
        },
        submitForm: function () {
            var $userName = $("#Username");
            var $password = $("#Password");

            if ($userName.val() === "" || $password.val() === "") {
                alert("Please input username and password.");
            } else {
                $.blockUI();
                LOGIN.encryptPassword($userName, $password);
                var param = {
                    "Username": $userName.val(),
                    "Password": $password.val(),
                    "LogInToken": $('#LogInToken').val()
                };

                $.ajax({
                    type: "POST",
                    url: LOGIN.getAbsoluteUrl("Account/Login"),
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(param),
                    success: function (result) {
                        if (result.IsSuccess) {
                            window.top.location = LOGIN.getAbsoluteUrl("Home");
                        } else {
                            $.unblockUI();
                            alert(result.ErrorMessage);
                            $("#Password").val("");
                        }
                    }
                });
            }
        },
        encryptPassword: function (userNameElement, passWordElement) {
            var $loginToken = $('#LogInToken');

            var userSalt = LOGIN.getUserSalt(userNameElement);

            var hashPassword = CryptoJS.SHA1(passWordElement.val() + userSalt).toString(CryptoJS.enc.Hex).toUpperCase();
            var encryptedPass = CryptoJS.SHA1(hashPassword + $loginToken.val() + userSalt).toString(CryptoJS.enc.Hex).toUpperCase();
            passWordElement.val(encryptedPass);
        },
        getUserSalt: function (userNameElement) {
            var salt;
            var userName = userNameElement.val();
            if (userName) {
                var url = LOGIN.getAbsoluteUrl("Account/GetUserSalt?userName=" + userName);
                $.ajax({
                    url: url,
                    type: "GET",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    success: function (result) {
                        salt = result;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status + ": " + thrownError);
                    }
                });
            }

            return salt;
        }
    };
})(jQuery);
$(function () {
    "use strict";
    LOGIN.init();
});