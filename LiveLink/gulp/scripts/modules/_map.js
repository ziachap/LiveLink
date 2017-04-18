﻿var map = {
	map: null,

	markers: [],

	infoWindow: null,

	timer: null,

	init: function () {
		

		$(".map:not(.js-map-done)").each(function () {
			var events = JSON.parse($("#events-json").html());
			var form = $('#js-map-controls');
			map.map = new google.maps.Map(this, {
				zoom: 12,
				center: { lat: 51.449517, lng: -2.575963 },
				styles: mapstyle
			});
			//eventService.bind(events);

			google.maps.event.addListener(map.map, 'bounds_changed', function () {
				clearTimeout(map.timer);
				//map.timer = setTimeout(eventService.search(), 300);
			});

			google.maps.event.addListener(map.map, 'click', function () {
				map.infoWindow.close();
			});

			$(this).addClass("js-map-done");

			eventService.search();

			map.infoWindow = new InfoBox({
				//content: infoWindowContent,
				alignBottom: true,
				disableAutoPan: false,
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