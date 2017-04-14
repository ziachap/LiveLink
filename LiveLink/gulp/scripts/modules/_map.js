

var map = {
	map: null,

	markers: [],

	infoWindow: null,

	timer: null,

	init: function () {
		var events = JSON.parse($("#events-json").html());
		var form = $('#js-map-controls');

		$(".map").each(function () {

			map.map = new google.maps.Map(this, {
				zoom: 12,
				center: { lat: 51.449517, lng: -2.575963 },
				styles: mapstyle
			});
			eventService.bind(events);

			google.maps.event.addListener(map.map, 'bounds_changed', function () {
				clearTimeout(map.timer);
				map.timer = setTimeout(eventService.search(), 300);
			});

		});
		map.infoWindow = new google.maps.InfoWindow();

		eventService.search();
	}
};
