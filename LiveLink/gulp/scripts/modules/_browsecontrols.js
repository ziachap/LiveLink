var browsecontrols = {

	form: null,

	init: function () {
		$('.js-browse-controls:not(.js-browse-controls-done)').each(function () {
			var form = $(this);
			browsecontrols.form = form;

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
			//console.log(browsecontrols.form.serialize());

			$(this).addClass('js-browse-controls-done');
		});
		
	}
};


$(function () {
	browsecontrols.init();
	site.ajaxComplete(function () {
		browsecontrols.init();
	});
});