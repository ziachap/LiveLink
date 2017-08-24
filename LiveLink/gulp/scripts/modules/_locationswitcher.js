var locationswitcher = {

	init: function () {

		$(".js-feed-controls:not(.js-feed-controls-done)").each(function () {
			var form = $(this);

			$(this).find('select,input').each(function () {

				$(this).on('change', function () {
					locationswitcher.search(form);
				});

			});

			$(this).addClass("js-feed-controls-done");
		});
	},

	search: function (form) {
		loader.show();
		locationswitcher.hideContent();
		updateQueryString(form.serialize());
		ajax.execute(
			urls.feed,
			form.serialize(),
			function (response) {
				// TODO: Check for success


				$(".js-feed-container").each(function () {
					$(this).replaceWith(response);
				});

			},
			function () {
				locationswitcher.showContent();
				loader.hide();
			});

		function updateQueryString(querystring) {
			var url = window.location.href.split('?')[0];
			history.pushState(null, null, url + "?" + querystring);
		}
	},

	hideContent: function() {
		$(".js-location-content").each(function () {
			$(this).hide();
		});
	},

	showContent: function () {
		$(".js-location-content").each(function () {
			$(this).show();
		});
	}
};

$(function () {
	locationswitcher.init();
	site.ajaxComplete(function () {
		locationswitcher.init();
	});
});