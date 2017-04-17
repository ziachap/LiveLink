

var eventService = {
	infoWindowTemplate: null,

	request: null,

	init: function() {
		eventService.infoWindowTemplate = Hogan.compile($('#infoWindowTemplate').html());
	},

	bind: function (venueEventsResponse) {
		if (map.markers == null) {
			map.markers = [];
		}

		for (var i = 0; i < map.markers.length; i++) {
			map.markers[i].setMap(null);
		}
		console.log(venueEventsResponse);
		map.markers = [];
		map.infoWindows = [];

		for (var i = 0; i < venueEventsResponse.length; i++) {
			var venue = venueEventsResponse[i];
			var firstEvent = venue.Events[0];

			var infoWindowContent = eventService.infoWindowTemplate.render(venue);

			var marker = new google.maps.Marker({
				position: { lat: venue.Latitude, lng: venue.Longitude },
				map: map.map,
				infoWindowContent: infoWindowContent,
				id: i
			});

			map.markers.push(marker);
			
			google.maps.event.addListener(marker, 'click', (function (marker, infoWindowContent) {
				return function () {
					console.log(marker);
					map.infoWindow.setContent(infoWindowContent);
					map.infoWindow.open(map, marker);
				};
			})(marker, infoWindowContent));
			
		}
		console.log(map.markers);
	},

	search: function () {

		//var loader = $(".js-loader");
		if (eventService.request != null) {
			eventService.request.abort();
			eventService.request = null;
		}
		// show loader
		//loader.show();
		// ajax
		eventService.request = $.ajax({
			url: "/API/venue-events",
			//data: mapfilter.form.serialize(),
			success: function (response) {
				response = JSON.parse(response);
				
				eventService.bind(response);

				if (response.Success) {

				} else {
					//alert("Sorry, there was an error");
					//popups.confirm("Error", "Sorry, there was an error (no events found). Please try again.", "", true);
				}
				// remove loader
				//loader.hide();
			},
			error: function (e) {
				if (e.statusText != "abort") {
					alert("Sorry, there was an AJAX error");
					//popups.confirm("Error", "Sorry, there was an error (" + e.statusText.toLowerCase() + ").", "", true);
				}
				// remove loader
				//loader.hide();
			}
		});
	}

};

$(function () {
	eventService.init();
	site.ajaxComplete(function () {
		eventService.init();
	});
});