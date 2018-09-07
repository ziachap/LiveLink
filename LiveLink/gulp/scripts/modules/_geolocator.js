var geolocator = {
    update: function() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function(position) {
                    var pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    };
                    map.map.setCenter(pos);

                },
                function() {
                    handleLocationError(true);
                },
                { enableHighAccuracy: true });
        } else {
            handleLocationError(false);
        }

        function handleLocationError(browserHasGeolocation) {
            console.log(browserHasGeolocation
                ? 'Error: The Geolocation service failed.'
                : 'Error: Your browser doesn\'t support geolocation.');
        }
    }
};