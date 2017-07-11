var userservice = {
	form: null,

	watch: function (id, button, callback) {
		ajax.execute("/API/user/watch", "id=" + id, function (response) {
			response = JSON.parse(response);
			console.log(response);

			if (callback != null) {
				callback(response, button);
			}
		});
	},

	login: function (loginForm, callback) {
		//var query = "username=" + username + "&password=" + password;
		ajax.execute("/API/user/login", loginForm.serialize(), function (response) {
			response = JSON.parse(response);
			console.log(response);

			if (response.Success) {
				notification.show('<i class="fa fa-sign-in fa-fw"></i> Welcome back <b>' + response.Data.Name + '</b>', 'success');
			} else {
				notification.show(response.Message, 'error');
			}
			console.log(callback);

			if (callback != null) {
				callback(response);
			}
		});
	},

	logout: function (callback) {
		ajax.execute("/API/user/logout", '', function (response) {
			response = JSON.parse(response);
			console.log(response);

			notification.show('<i class="fa fa-sign-out fa-fw"></i> You have been logged out', 'warning');

			if (callback != null) {
				callback(response);
			}
		});
	}
};