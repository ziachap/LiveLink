var infowindow = {

	init: function () {
		$(".infowindow:not(.js-infowindow-done)").each(function () {

			$(this).find('.carousel').carousel({
			});

			$(this).find('.carousel-control').off('click').on('click', function (e) {
				$('.carousel').carousel($(this).attr('data-slide'));
				return false;
			});

			$(this).find(".js-watch:not(.js-watch-done)").each(function () {
				var id = $(this).attr("data-id");
				$(this).click(function () {

					userservice.watch(id, this,
						function (response, button) {
							if (response.Success) {
								if (response.Data.Active) {
									$(button).addClass("active");
									notification.show("<i class=\"fa fa-eye\"></i> Now watching <b>" + response.Data.Title + "</b>", 'success');
								} else {
									$(button).removeClass("active");
									notification.show("<i class=\"fa fa-eye\"></i> Stopped watching <b>" + response.Data.Title + "</b>", 'warning');
								}

							} else {
								notification.show("Watch failed: " + response.Message, 'warning');
							}
						});
				});
				$(this).addClass("js-watch-done");
			});

			$(this).find(".js-open-event:not(.js-open-event-done)").each(function () {
				var id = $(this).attr("data-id");
				$(this).click(function () {
					eventService.render(id);
					return false;
				});
				$(this).addClass("js-open-event-done");
			});


			$(this).addClass("js-infowindow-done");
		});
	}
};