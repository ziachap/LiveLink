var map = {
	map: null,

	markers: [],

	infoWindow: null,

	init: function () {

		mapfilter.init();

		$(".map:not(.js-map-done)").each(function () {
			var events = JSON.parse($("#events-json").html());
			var form = $('#js-map-controls');
			map.map = new google.maps.Map(this, {
				zoom: 12,
				center: { lat: 51.449517, lng: -2.575963 },
				styles: mapstyle
			});
			//eventService.bind(events);

			google.maps.event.addListener(map.map, 'idle', function () {
				eventService.search();

				

			});

			google.maps.event.addListener(map.map, 'click', function () {
				map.infoWindow.close();
				initInfoWindow();
			});

			$(this).addClass("js-map-done");

			eventService.search();

			initInfoWindow();

			function initInfoWindow() {
				map.infoWindow = new InfoBox({
					//content: infoWindowContent,
					alignBottom: true,
					disableAutoPan: true,
					maxWidth: 0,
					pixelOffset: new google.maps.Size(-170, -50),
					zIndex: null,
					boxClass: "infowindow",
					closeBoxURL: "",
					infoBoxClearance: new google.maps.Size(20, 20),
					isHidden: false,
					pane: "floatPane",
					enableEventPropagation: false
				});
			}
		});

		


		//map.infoWindow = new google.maps.InfoWindow();

		
	}
};
$(function () {
	map.init();
	site.ajaxComplete(function () {
		map.init();
	});
});

$(document).ready(function () {
	$('.carousel').carousel({
		interval: 2000
	});
});