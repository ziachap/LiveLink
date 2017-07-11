var mapfilter = {

	form: null,

	init: function () {
		$('.js-map-controls').each(function () {
			var form = $(this);
			mapfilter.form = form;

			form.bind('submit', function (e) {
				e.preventDefault();
				eventService.search();
				return false;
			});

			form.find("input[data-role='filter']").change(function () {
				eventService.search();
			});

			form.find(".js-location").change(function() {
				console.log("location change");
			});
			//console.log(mapfilter.form.serialize());
		});
		
	}
};


$(function () {
	mapfilter.init();
	site.ajaxComplete(function () {
		mapfilter.init();
	});
});