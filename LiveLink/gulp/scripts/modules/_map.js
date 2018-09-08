// Not currently in use

var map = {
	map: null,

	markers: [],

	infoWindow: null,

	init: function() {

		browsecontrols.init();

		$(".js-map:not(.js-map-done)").each(function() {
			map.map = new google.maps.Map(this,
				{
					zoom: 13,
					center: { lat: 51.449517, lng: -2.575963 },
					styles: mapstyle
				});

			google.maps.event.addListener(map.map,
				"idle",
				function() {
					updateBoundInputs();
					eventService.search();
				});

			function updateBoundInputs() {
				var bounds = map.map.getBounds();
				var ne = bounds.getNorthEast();
				var sw = bounds.getSouthWest();
				var minX = sw.lng(), maxX = ne.lng();
				var minY = sw.lat(), maxY = ne.lat();

				browsecontrols.form.find(".js-bounds-min-x").val(minX);
				browsecontrols.form.find(".js-bounds-max-x").val(maxX);
				browsecontrols.form.find(".js-bounds-min-y").val(minY);
				browsecontrols.form.find(".js-bounds-max-y").val(maxY);
			};

			google.maps.event.addListener(map.map,
				"click",
				function() {
					map.infoWindow.close();
					initInfoWindow();
				});

			$(this).addClass("js-map-done");

			eventService.search();

			initInfoWindow();

			function initInfoWindow() {
				map.infoWindow = new InfoBox({
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

			geolocator.update();

		});
	}
};
$(function() {
	map.init();
	site.ajaxComplete(function() {
		map.init();
	});
});