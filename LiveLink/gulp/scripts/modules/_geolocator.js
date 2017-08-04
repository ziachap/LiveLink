var geolocator = {
	update: function() {
		// Try HTML5 geolocation.
		if (navigator.geolocation) {
			navigator.geolocation.getCurrentPosition(function(position) {
				var pos = {
					lat: position.coords.latitude,
					lng: position.coords.longitude
				};
				/*
				var marker = new google.maps.Marker({
					title: "You are here",
					position: pos,
					//icon: "",
					map: map.map
				});
				*/
				map.map.setCenter(pos);

			}, function() {
				handleLocationError(true);
			}, { enableHighAccuracy: true });
		} else {
			// Browser doesn't support Geolocation
			handleLocationError(false);
		}
		
		function handleLocationError(browserHasGeolocation) {
			console.log(browserHasGeolocation ?
								  'Error: The Geolocation service failed.' :
								  'Error: Your browser doesn\'t support geolocation.');
		}
	}
};