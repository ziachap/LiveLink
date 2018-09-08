var packeryService = {
	init: function() {
		$(".packed__grid").packery({
			itemSelector: ".packed__grid-item",
			gutter: 12
		});

	}
};


$(function() {
	packeryService.init();
	site.ajaxComplete(function() {
		packeryService.init();
	});
});