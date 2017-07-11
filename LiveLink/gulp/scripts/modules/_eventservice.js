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
		//console.log(venueEventsResponse);
		map.markers = [];
		map.infoWindows = [];

		for (var i = 0; i < venueEventsResponse.length; i++) {
			var venue = venueEventsResponse[i];

			venue.Events[0].Active = true;

			var infoWindowContent = eventService.infoWindowTemplate.render(venue);

			var marker = new google.maps.Marker({
				title: venue.Title,
				position: { lat: venue.Latitude, lng: venue.Longitude },
				icon: venue.Logo.Url + "?width=35&height=35&mode=crop&format=png&s.roundcorners=1000",
				map: map.map,
				infoWindowContent: infoWindowContent,
				id: i
			});

			map.markers.push(marker);
			
			google.maps.event.addListener(marker, 'click', (function (marker, infoWindowContent) {
				return function () {
					map.infoWindow.close();
					map.infoWindow.setContent(infoWindowContent);
					map.infoWindow.addListener('domready', function () {
						infowindow.init();
					});
					map.infoWindow.marker = marker;
					map.infoWindow.open(map.map, marker);

					//map.map.panTo(map.infoWindow.getPosition());
					//map.infoWindow.content = infoWindowContent;
					//map.infoWindow.open(map.map, marker);
					//;
				};
			})(marker, infoWindowContent));
		}

		if (map.infoWindow.marker != null) {
			map.infoWindow.open(map.map, map.infoWindow.marker);
		}
		//console.log(map.markers);
	},

	search: function () {
		ajax.execute(
			"/API/venue-events",
			'',//mapfilter.form.serialize(),
			function (response) {
				//console.log(response.Data);
				response = JSON.parse(response);
				eventService.bind(response.Data);
			});
	},

	render: function (id) {
		overlay.showLoader();
		ajax.execute(
			"/API/event",
			"id=" + id,
			function (response) {
				overlay.render(response, function () {
					eventMap.init();
				});
				overlay.hideLoader();
			});
		overlay.show();
	} 

};

$(function () {
	eventService.init();
	site.ajaxComplete(function () {
		eventService.init();
	});
});