
(function ($) {

    "use strict";
    var markerIcon = {
        anchor: new google.maps.Point(22, 16),
        url: 'images/marker.png',
    }

    var centerLat = parseFloat(document.getElementById("centerLat").value);
    var centerLng = parseFloat(document.getElementById("centerLng").value);
    var zoomLevel = parseInt(document.getElementById("zoom").value);

    var purpose = document.getElementById("purpose").value;
    var propertyType = parseInt(document.getElementById("cityId").value);
    var cityId = parseInt(document.getElementById("cityId").value);
    var societyId = parseInt(document.getElementById("societyId").value);
    var phaseId = parseInt(document.getElementById("phaseId").value);
    var blockId = parseInt(document.getElementById("blockId").value);
    var priceFrom = parseInt(document.getElementById("priceFrom").value);
    var priceTo = parseInt(document.getElementById("priceTo").value);
    var areaFrom = parseInt(document.getElementById("areaFrom").value);
    var areaTo = parseInt(document.getElementById("areaTo").value);
    var unit = document.getElementById("unit").value;

    function locationData(locationURL, locationImg, locationTitle, locationAddress, price, area, areaUnit,purpose,type,city,soc,phase,block,plotNo,id,phone) {
        var html = '' +
            '<a href="' + locationURL + '" class="listing-img-container">' +
            '<div class="infoBox-close"><i class="fa fa-times"></i></div>' +
            '<img src="' + locationImg + '" alt="">' +
            '<div class="listing-item-content">' +
            '<h3 style"color:red">' + locationTitle + '</h3>' +
            '<span class="tag"><b style="color:white;font-size:9px">' + type + '</b></span>' +
            '</div>' +
            '</a>' +
            '<div class="listing-content">' +
            '<div class="listing-title">';
        if (area != null) { html = html + '<span class="label label-success " style="font-size:13px"><b style="color:whit;">' + area + ' ' + areaUnit + '</b></span>' }
        if (price != "") { html = html + '<span class="label label-warning" style="font-size:13px"><b style="color:white">' + 'PKR-' + price + '/-' + '</b></span></p><br>' }
        if (id != null) { html = html + '<p><span class="label label-warning" style="font-size:13px"><b style="color:white">' + "id:" + id + '</b></span>' }
        if (purpose != "") { html = html + '<span class="label label-info" style="font-size:13px"><b style="color:white">' + purpose + '</b></span></p><br>' }
        if (city != "") { html = html + '<p><span class="label label-info" style="font-size:13px"><b style="color:white">' + city + '</b></span>' }
        if (soc != "") { html = html + '<span class="label label-success" style="font-size:13px"><b style="color:white">' + soc + '</b></span></p><br>' }
        if (phase != "") { html = html + '<p><span class="label label-warning" style="font-size:13px"><b style="color:white">' + phase + '</b></span>' }
        if (block != "") { html = html + '<span class="label label-info" style="font-size:13px"><b style="color:white">' + block + '</b></span>' }
        if (plotNo != "") { html = html + '<span class="label label-success" style="font-size:13px"><b style="color:white">' + "PlotNo:" + plotNo + '</b></span></p><br>' }
        if (phone != "") { html = html + '<p><span class="label label-danger" style="font-size:18px"><b style="color:white">' + "Call:" + phone + '</b></span></p><br>' }
       

        html = html + '</p></div>';
        
        return (html)
    }
    var locations = []
    
    $.ajax({
        url: '/Property/ListingJson?purpose=' + purpose + '&propertyType=' + propertyType + '&cityId=' + cityId + '&societyId=' + societyId + '&phaseId=' + phaseId + '&blockId=' + blockId + '&priceFrom=' + priceFrom + '&priceTo=' + priceTo + '&areaFrom=' + areaFrom + '&areaTo=' + areaTo + '&unit=' + unit,
        dataType: "json",
        type: "GET",
        success: function (data) {
            
            $.each(data, function (i, item) {

                locations[i] = [];
                for (var j = 0; j < 5; j++) {
                    if (j == 0) {
                        var img = item.image.split(",");
                        var purpose
                        if (item.purpose = "sell") { purpose = "For Sale"; } if (item.purpose = "rent") { purpose = "For Rent"; }
                        var type = item.plot_type + ' ' + item.type_name
                        locations[i][j] = locationData('/Property/Details/' + item.id, img[0], item.title, 'address', item.price_with_comma, item.area, item.area_unit, purpose, type, item.city_name, item.society_name, item.phase_name, item.block_name, item.plot_no, item.id,item.dealer_phone );
                    }
                    else if (j == 1) {
                        locations[i][j] = parseFloat(item.lat);

                    }
                    else if (j == 2) {
                        locations[i][j] = parseFloat(item.lng);

                    }
                    else if (j == 3) {
                        locations[i][j] = 1;
                    }
                    else if (j == 4) {
                        locations[i][j] = markerIcon;
                    }
 
                }
                
                
            });
        },
        error: function (ex) {
            alert("problem in loading map");

        },
    });
 
    function mainMap() {
      
        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: zoomLevel,

            center: new google.maps.LatLng(centerLat, centerLng),
            mapTypeId: google.maps.MapTypeId.ROADMAP,
           

            animation: google.maps.Animation.BOUNCE,
            gestureHandling: 'cooperative',
            styles: [{
                "featureType": "administrative",
                "elementType": "labels.text.fill",
                "stylers": [{
                    "color": "#444444"
                }]
            }]
        });

  
        var boxText = document.createElement("div");
        boxText.className = 'map-box'
        var currentInfobox;
        var boxOptions = {
            content: boxText,
            disableAutoPan: true,
            alignBottom: true,
            maxWidth: 0,
            pixelOffset: new google.maps.Size(-145, -45),
            zIndex: null,
            boxStyle: {
                width: "260px"
            },
            closeBoxMargin: "0",
            closeBoxURL: "",
            infoBoxClearance: new google.maps.Size(1, 1),
            isHidden: false,
            pane: "floatPane",
            enableEventPropagation: false,
        };

        var markerCluster, marker, i;
        for (i = 0; i < locations.length; i++) {
         
            marker = new google.maps.Marker({
                position: new google.maps.LatLng(locations[i][1], locations[i][2]),
                id: i,
                map: map
            });

            var ib = new InfoBox();
            google.maps.event.addListener(ib, "domready", function () {
             
            });
            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                return function () {
                    ib.setOptions(boxOptions);
                    boxText.innerHTML = locations[i][0];
                    ib.close();
                    ib.open(map, marker);
                    currentInfobox = marker.id;
                    var latLng = new google.maps.LatLng(locations[i][1], locations[i][2]);
                    map.panTo(latLng);
                    map.panBy(0, -180);
                    google.maps.event.addListener(ib, 'domready', function () {
                        $('.infoBox-close').click(function (e) {
                            e.preventDefault();
                            ib.close();
                        });
                    });
                }
            })(marker, i));
        }


    }
    

    var map = document.getElementById('map');

    if (typeof (map) != 'undefined' && map != null) {
        google.maps.event.addDomListener(window, 'load', mainMap);
    }

})(this.jQuery);