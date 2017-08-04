var tabs = {
	init: function () {
		$(".js-tabs:not(.js-tabs-done)").each(function () {
			console.log("TABS");
			$(".js-tab:not(.js-tab-done)").each(function () {
				console.log("tab");

				$(this).on('click', function () {
					var tab = $(this);

					var tabContentId = tab.attr('href');
					console.log(tabContentId);

					$('.js-tab-item').each(function () {
						$(this).removeClass('active');
					});

					$('.js-tab-content').each(function () {
						$(this).css('display', 'none');
					});

					$(tabContentId).css('display', 'block');

					tab.addClass('active');

					google.maps.event.trigger(map.map, 'resize');
					packeryService.init();

					return false;
				});

				$(this).addClass("js-tab-done");
			});

			// TODO - Set tabs to initial state
			$('.js-tab-content:not(.active)').each(function () {
				console.log('shieet');
				$(this).css('display', 'none');
			});

			$(this).addClass("js-tabs-done");
		});
	}
};


$(function () {
	tabs.init();
	site.ajaxComplete(function () {
		tabs.init();
	});
});