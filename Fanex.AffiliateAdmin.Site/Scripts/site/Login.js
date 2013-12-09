var LOGIN = function ($) {
	return {
		init: function () {
			if ($("#Username").val() == "") {
				$("#Username").focus();
			} else {
				$("#Password").focus();
            }

			// Event click login
			$("#btnlogin").click(function () {
				LOGIN.submitForm();
            });

			$('.login input').bind('keypress', function (e) {
				if (e.keyCode == 13) {
					LOGIN.submitForm();
				}
			});
		},
		submitForm: function () {
			var username = $("#Username").val(),
				password = $("#Password").val();

			if (username == "" || password == "") {
				alert("Please input username and password.");
			}
			else {
				$.blockUI();
				var param = {
					"Username": username,
					"Password": password
				};

				$.ajax({
					type: "POST",
					url: AFFILIATE.getAbsoluteUrl("Account/Login"),
					contentType: "application/json",
					dataType: "json",
					data: JSON.stringify(param),
					success: function (result) {
						if (!result.Success) {
							alert(result.Message);
							$("#Password").val("");
						}
						else {
							parent.location.href = AFFILIATE.getAbsoluteUrl("");
						}
					},
					error: function () {
					},
					complete: function () {
						$.unblockUI();
					}
				});
			}
		}
	}
} (jQuery);
$(function () {
	LOGIN.init();
});