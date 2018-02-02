var feed = {
	itemsPerPage: 12,

	page: 1,

	feedItemsTemplate: null,

	form: null,

	init: function () {
		feed.feedItemsTemplate = Hogan.compile($('#feedItemsTemplate').html());

		$(".js-feed-controls:not(.js-feed-controls-done)").each(function () {
			var form = $(this);
			feed.form = form;

			var navForm = $("#js-browse-controls-id");

			$(this).find('select,input').each(function () {

				$(this).on('change', function () {
					feed.search(feed.form, navForm);
				});

			});

			$(".js-feed-see-more:not(.js-feed-see-more-done)").each(function () {

				$(this).each(function () {
					$(this).on('click', function () {
						feed.searchItems(feed.form, navForm, feed.page + 1);
					});
				});

				$(this).addClass("js-feed-see-more-done");
			});

			$(this).addClass("js-feed-controls-done");
		});

		$('.js-browse-controls:not(.js-browse-controls-done)').each(function () {
			var navForm = $(this);

			navForm.bind('submit', function (e) {
				e.preventDefault();
				$(this).on('change', function () {
					feed.search(feed.form, navForm);
				});
				return false;
			});

			navForm.find("input[data-role='filter']").change(function () {
				feed.search(feed.form, navForm);
			});

			$(this).addClass('js-browse-controls-done');
		});
	},

	search: function (form, navForm) {
		loader.show('.js-feed-loader');
		feed.hideContent();
		updateQueryString(form.serialize());
		ajax.execute(
			urls.feed,
			form.serialize() + "&" + navForm.serialize(),
			function (response) {
				// TODO: Check for success (might be tricky since we're just getting back html)
				$(".js-feed-container").each(function () {
					$(this).replaceWith(response);
				});
				feed.page = 1;
			},
			function () {
				feed.showContent();
				loader.hide('.js-feed-loader');
			});

		function updateQueryString(querystring) {
			var url = window.location.href.split('?')[0];
			history.pushState(null, null, url + "?" + querystring);
		}
	},

	searchItems: function (form, navForm, page) {
		loader.hide('.js-feed-see-more');
		ajax.execute(
			urls.searchFeedEvents,
			createQueryString(form, navForm, page),
			function (response) {
				// TODO: Check for success

				response = JSON.parse(response);
				$(".js-feed-items").each(function () {
					var content = feed.feedItemsTemplate.render(response);
					$(this).append(content);
				});

				// TODO: this is not a good way of deciding if there are any more results left
				if (response.Data.length < feed.itemsPerPage || response.Data.length === 0) {
					$(".js-feed-see-more").hide();
				}

				feed.page++;
			},
			function () {
				feed.showContent();
				//loader.show('.js-feed-see-more');
			});

		function createQueryString(form, navForm, page) {
			return form.serialize() + "&" + navForm.serialize() + "&itemsPerPage=" + feed.itemsPerPage + "&page=" + page;
		}
	},
	
	hideContent: function() {
		$(".js-location-content").each(function () {
			$(this).hide();
		});
	},

	showContent: function () {
		$(".js-location-content").each(function () {
			$(this).show();
		});
	}
};

$(function () {
	feed.init();
	site.ajaxComplete(function () {
		feed.init();
	});
});