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

			form.find('input').change(function () {
				eventService.search();
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