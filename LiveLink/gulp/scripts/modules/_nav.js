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
	//nav.init();
	site.ajaxComplete(function () {
		//nav.init();
	});
});