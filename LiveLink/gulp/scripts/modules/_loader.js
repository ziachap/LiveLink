var loader = {
	show: function (elementClass) {
		if (elementClass == null) {
			elementClass = '.js-overlay-loader';
		}
		$(elementClass).each(function () {
			$(this).show();
		});
	},
	hide: function (elementClass) {
		if (elementClass == null) {
			elementClass = '.js-overlay-loader';
		}
		$(elementClass).each(function () {
			$(this).hide();
		});
	}
};