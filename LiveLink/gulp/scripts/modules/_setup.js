var setup = {
	init: function() {

		$(".ui.dropdown:not(.ui-dropdown-done)").each(function() {
			$(this).dropdown();
			$(this).addClass("ui-dropdown-done");
		});
	}

};

$(function() {
	setup.init();
	site.ajaxComplete(function() {
		setup.init();
	});
});