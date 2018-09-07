var eventOverlay = {
    init: function() {
        $('.js-event-map:not(.js-event-map-done)').each(function() {
            var lng = parseFloat($(this).attr("data-lng"));
            var lat = parseFloat($(this).attr("data-lat"));

            var map = new google.maps.Map(this,
                {
                    zoom: 14,
                    center: { lat: lat, lng: lng }
                });

            google.maps.event.addListenerOnce(map,
                'idle',
                function() {
                    google.maps.event.trigger(map, 'resize');
                    map.panTo({ lat: lat, lng: lng });
                });

            $(this).addClass("js-event-map-done");
        });


        // TODO: Move this to somewhere more appropriate
        $('.js-event-show-more:not(.js-event-show-more-done)').each(function() {
            $(this).readmore({
                collapsedHeight: 400,
                speed: 500,
                moreLink: '<a href="#">Show more</a>',
                lessLink: '<a href="#">Show less</a>'
            });
            $(this).addClass("js-event-show-more-done");
        });
    }

};

$(function() {
    eventOverlay.init();
    site.ajaxComplete(function() {
        eventOverlay.init();
    });
});