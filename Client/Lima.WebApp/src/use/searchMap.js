import store from "../store";
export const initMap = () => {
  store.commit("SET_DETAILS", {});

  const input = document.getElementById("searchInput");

  const map = new google.maps.Map(document.getElementById("map"), {
    center: { lat: 41.311081, lng: 69.240562 },
    zoom: 13,
  });

  map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

  const autocomplete = new google.maps.places.Autocomplete(input);
  autocomplete.bindTo("bounds", map);

  const marker = new google.maps.Marker({
    position: { lat: 41.311081, lng: 69.240562 },
    map,
    // anchorPoint: new google.maps.Point(0, -29),
    dragabble: true,
  });

  const infowindow = new google.maps.InfoWindow();

  autocomplete.addListener("place_changed", function () {
    infowindow.close();
    marker.setVisible(true);
    let place = autocomplete.getPlace();
    if (!place.geometry) {
      window.alert("no geometry");
      return;
    }

    if (place.geometry.viewport) {
      map.fitBounds(place.geometry.viewport);
    } else {
      map.setCenter(place.geometry.location);
      map.setZoom(17);
    }

    marker.setPosition(place.geometry.location);

    let address = [];
    if (place.address_components) {
      address = [
        ((place.address_components[0] &&
          place.address_components[0].short_name) ||
          "",
        (place.address_components[1] &&
          place.address_components[1].short_name) ||
          "",
        (place.address_components[2] &&
          place.address_components[2].short_name) ||
          ""),
      ].join("");
    }
    infowindow.setContent(`<div><strong>${place.name}</strong><br>${address}`);

    infowindow.open(map, marker);

    // Location details
    document.getElementById("place").innerHTML = place.formatted_address;
    store.commit("SET_DETAILS", {
      latitude: place.geometry.location.lat(),
      longitude: place.geometry.location.lng(),
      address: place.formatted_address,
    });
  });
};
