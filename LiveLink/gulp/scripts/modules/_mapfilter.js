var mapfilter = {

	form: null,

	init: function () {
		$('.js-map-controls').each(function () {
			var form = $(this);
			mapfilter.form = form;
			form.find('input').change(function () {
				eventService.search();
			});
		});
		
	}
};


$(function () {
	mapfilter.init();
	site.ajaxComplete(function () {
		mapfilter.init();
	});
});