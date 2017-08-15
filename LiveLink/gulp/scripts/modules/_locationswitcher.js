var locationswitcher = {
	feedViewTemplate: null,

	init: function() {
		locationswitcher.feedViewTemplate = Hogan.compile($('#feedViewTemplate').html());

		$(".js-location-input:not(.js-location-input-done)").each(function () {
			var locationId = $(this).val();
			locationswitcher.search(locationId);

			$(this).on('change', function () {
				var locationId = $(this).val();
				locationswitcher.search(locationId);
			});

			$(this).addClass("js-location-input-done");
		});
	},

	search: function (locationId) {
		loader.show();
		locationswitcher.hideContent();
		ajax.execute(
			urls.searchFeedEvents,
			"locationid=" + locationId,
			function(response) {
				// TODO: Check for success
				response = JSON.parse(response);
				console.log(response);

				var content = locationswitcher.feedViewTemplate.render(response.Data);

				$(".js-location-content").each(function() {
					$(this).html(content);
				});

			},
			function() {
				loader.hide();
				locationswitcher.showContent();
			});
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