console.log("ready");

var nav = {
	init: function() {
		$(".nav-toggle").each(function () {
			$(this).click(function () {
				$("#nav-top").toggleClass("responsive");
			});
		});
	}

};

$(function() {
	nav.init();
	$(document).ajaxComplete(function() {
		nav.init();
	});
});