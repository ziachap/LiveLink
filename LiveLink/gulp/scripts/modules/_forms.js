forms = {
	init: function () {
		forms.authentication();
		forms.datepicker();
	},

	datepicker: function() {
		$(".datepicker:not(.datepicker-done)").each(function () {
			$(this).datepicker();
			$(this).addClass("datepicker-done");
		});
	},

	authentication: function () {
		
		$(".js-login-form:not(.js-login-form-done)").each(function () {
			var form = $(this);

			form.bind('submit', function (e) {
				e.preventDefault();
				userservice.login(form, false);
				return false;
			});

			$(this).find(".js-login:not(.js-login-done)").each(function () {
				$(this).click(function () {
					userservice.login(form);
					return false;
				});
				$(this).addClass("js-login-done");
			});

			$(this).find(".js-logout:not(.js-logout-done)").each(function () {
				$(this).click(function () {
					userservice.logout();
					return false;
				});
				$(this).addClass("js-logout-done");
			});

			$(this).addClass("js-login-form-done");
		});
	}
}

$(function () {
	forms.init();
	site.ajaxComplete(function () {
		forms.init();
	});
});