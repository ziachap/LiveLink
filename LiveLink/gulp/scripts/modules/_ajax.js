﻿var ajax = {
	request: null,

	execute: function (url, data, callback) {

		//var loader = $(".js-loader");
		if (ajax.request != null) {
			ajax.request.abort();
			ajax.request = null;
		}
		// show loader
		//loader.show();
		// ajax
		ajax.request = $.ajax({
			url: url,
			data: data,
			success: function(response) {
				response = JSON.parse(response);
				callback(response);

				// remove loader
				loader.hide();
			},
			error: function(e) {
				if (e.statusText != "abort") {
					console.log(e);
					notification.show("Something went wrong: " + e.responseText, 'error');
					//popups.confirm("Error", "Sorry, there was an error (" + e.statusText.toLowerCase() + ").", "", true);
				}
				// remove loader
				//loader.hide();
			}
		});
	}
};