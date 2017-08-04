var search = {
	searchResultsTemplate: null,

	request: null,

	init: function() {
		search.searchResultsTemplate = Hogan.compile($('#searchResultsTemplate').html());
		
		$(".js-search-input:not(.js-search-input-done)").each(function () {
			var input = $(this);

			input.bind("propertychange change click keyup input paste", function (event) {
				var text = $(this).val();
				if (text.length > 2) {
					ajax.execute(
						"/API/content-search",
						"text=" + text,
						function(response) {
							// TODO: Check for success
							response = JSON.parse(response);
							var content = search.searchResultsTemplate.render(response.Data);

							$(".js-search-results").each(function() {
								$(this).html(content);
								$(this).addClass("active");
							});
						});
				} else {
					search.close();
				}
			});

			input.addClass("js-search-input-done");
		});

		$(".js-search-results:not(.js-search-results-done)").each(function () {
			var results = $(this);

			results.bind('clickoutside', function (event) {
				search.close();
			});

			$(this).addClass("js-search-results-done");
		});
	},

	close: function () {
		$(".js-search-results").each(function () {
			$(this).removeClass("active");
		});
	}

};

$(function () {
	search.init();
	site.ajaxComplete(function () {
		search.init();
	});
});