var loader = {
	show: function () {
		$('.js-overlay-loader').each(function () {
			$(this).show();
		});
	},
	hide: function () {
		$('.js-overlay-loader').each(function () {
			$(this).hide();
		});
	}
};