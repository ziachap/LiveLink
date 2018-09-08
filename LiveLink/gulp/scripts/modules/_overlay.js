var overlay = {
	init: function() {

		$(".js-overlay:not(.js-overlay-done)").each(function() {

			$(this).find(".js-overlay-close").each(function() {
				$(this).click(function() {
					overlay.hide();
				});
			});

			$(document).on("click",
				function(event) {
					if (!$(event.target).closest(".overlay__inner").length) {
						overlay.hide();
					}
				});

			$(this).addClass("js-overlay-done");
		});
	},
	hide: function() {
		$(".js-overlay").each(function() {
			$(this).hide();
		});
	},
	show: function() {
		$(".js-overlay").each(function() {
			$(this).show();
		});
	},
	showLoader: function() {
		$(".js-overlay-content").each(function() {
			$(this).hide();
		});
		$(".js-overlay-loader").each(function() {
			$(this).show();
		});
	},
	hideLoader: function() {
		$(".js-overlay-content").each(function() {
			$(this).show();
		});
		$(".js-overlay-loader").each(function() {
			$(this).hide();
		});
	},
	render: function(content, callback) {
		$(".js-overlay-content").each(function() {
			$(this).html(content).promise().done(function() {
				if (callback !== null) {
					callback();
				}
			});;
		});
	}
};

$(function() {
	overlay.init();
	site.ajaxComplete(function() {
		overlay.init();
	});
});