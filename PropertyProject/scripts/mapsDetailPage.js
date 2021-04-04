(function ($) {
    function singleLocationMap() {
        var Lat = parseFloat(document.getElementById("singleLat").value);
        var Lng = parseFloat(document.getElementById("singleLng").value);
        var map = new google.maps.Map(document.getElementById('singlePlaceMap'), {
            zoom: 15,
            center: new google.maps.LatLng(Lat, Lng),
            mapTypeId: google.maps.MapTypeId.ROADMAP,


        })
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(Lat, Lng),
            map: map,
            title: 'This Location!'
        });
    }
    google.maps.event.addDomListener(window, 'load', singleLocationMap);
})(this.jQuery);