var locationswitcher = {
	feedViewTemplate: null,

	init: function() {
		locationswitcher.feedViewTemplate = Hogan.compile($('#feedViewTemplate').html());
		
		$(".js-location-input:not(.js-location-input-done)").each(function () {
			$(this).on('change', function () {
				var locationId = $(this).val();
				ajax.execute(
					urls.searchEvents,
					"locationid=" + locationId,
					function(response) {
						// TODO: Check for success
						response = JSON.parse(response);
						console.log(response);
						
						var content = locationswitcher.feedViewTemplate.render(response);

						$(".js-location-content").each(function() {
							$(this).html(content);
						});
						
					});
			});

			$(this).addClass("js-location-input-done");
		});
	}
};

$(function () {
	locationswitcher.init();
	site.ajaxComplete(function () {
		locationswitcher.init();
	});
});