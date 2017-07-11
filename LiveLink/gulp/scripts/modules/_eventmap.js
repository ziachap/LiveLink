var eventMap = {

	init: function() {
		$('.js-event-map:not(.js-event-map-done)').each(function () {
			var lng = parseFloat($(this).attr("data-lng"));
			var lat = parseFloat($(this).attr("data-lat"));

			console.log(lng);
			console.log(lat);

			var map = new google.maps.Map(this,
			{
				zoom: 15,
				center: { lat: lat, lng: lng }
			});

			var marker = new google.maps.Marker({
				position: { lat: lat, lng: lng },
				map: map
			});

			google.maps.event.addListenerOnce(map, 'idle', function () {
				google.maps.event.trigger(map, 'resize');
				map.panTo({ lat: lat, lng: lng });
			});

			$(this).addClass("js-event-map-done");
		});
	}

};

$(function () {
	eventMap.init();
	site.ajaxComplete(function () {
		eventMap.init();
	});
});