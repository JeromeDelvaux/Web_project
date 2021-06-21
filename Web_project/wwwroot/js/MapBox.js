$(document).ready(function () {

    mapboxgl.accessToken = 'pk.eyJ1IjoicHRpYmljaG9uIiwiYSI6ImNrZDh2emJscDB5NWwycXNjeTNqMGZhMTMifQ.OrLb10CpurlWGR9gwPTUjA';
    var map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/ptibichon/ckd8wwx7a159j1iqfx8zk6tsu'
    });

    $.get("/Etablissement/GetAllEtab", function (result) {
        $.each(result, function (index, item) {
            var items = JSON.parse(item);
            var lat = items.etablissement.AddLatitude;
            var long = items.etablissement.AddLongitude;
            var nom = items.etablissement.Nom;
            var idEtbl = items.etablissement.Id;
            var description = items.etablissement.ZoneTexteLibre;
            var logo = "/images/" + (items.etablissement.LogoPath);
            var minAvantFermeture = items.MinAvantFermeture;
            var estAuthentifie = items.estAuthentifie;
            var shortUrl = items.etablissement.SiteWebShortUrl;
            var statut = "";

            if (estAuthentifie) {

                if (minAvantFermeture !== 0) {

                    if (minAvantFermeture <= 15) {
                        statut = "Status: Fermé dans " + minAvantFermeture + " min...";

                        if (shortUrl !== null) {
                            var popup = new mapboxgl.Popup()
                                .setHTML
                                ("<img src='" + logo + "' width='80'/><h4>" + nom + "</h4><a href=" + shortUrl + ">" + shortUrl +"</a><br><h6>" + description + "</h6><br><a href=Etablissement\\Details\\" + idEtbl + ">Détails</a><br>" + statut);

                        }
                        else {
                            var popup = new mapboxgl.Popup()
                                .setHTML
                                ("<img src='" + logo + "' width='80'/><h4>" + nom + "</h4><br><h6>" + description + "</h6><br><a href=Etablissement\\Details\\" + idEtbl + ">Détails</a><br>" + statut);

                        }
                        
                        var marker = new mapboxgl.Marker({ color: 'red' }) // initialize a new marker
                            .setLngLat([long, lat]) // Marker [lng, lat] coordinates
                            .setPopup(popup)
                            .addTo(map) // Add the marker to the map
                    }

                    else
                    {
                        alert('MinAvantFermeture = ' + minAvantFermeture)

                        statut = "Status: Ouvert";

                        if (shortUrl !== null) {
                            var popup = new mapboxgl.Popup()
                                .setHTML
                                ("<img src='" + logo + "' width='80'/><h4>" + nom + "</h4><a href=" + shortUrl + ">" + shortUrl +"</a><br><h6>" + description + "</h6><br><a href=Etablissement\\Details\\" + idEtbl + ">Détails</a><br>" + statut);
                        }

                        else {
                            var popup = new mapboxgl.Popup()
                                .setHTML
                                ("<img src='" + logo + "' width='80'/><h4>" + nom + "</h4><br><h6>" + description + "</h6><br><a href=Etablissement\\Details\\" + idEtbl + ">Détails</a><br>" + statut);
                        }

                        var marker = new mapboxgl.Marker({ color: 'green' }) // initialize a new marker
                            .setLngLat([long, lat]) // Marker [lng, lat] coordinates
                            .setPopup(popup)
                            .addTo(map) // Add the marker to the map
                    }
                }

            }
            else
            {
                if (shortUrl !==null) {
                    var popup = new mapboxgl.Popup()
                        .setHTML
                        ("<img src='" + logo + "' width='80'/><h4>" + nom + "</h4><a href=" + shortUrl + ">"+ shortUrl +"</a><br><h6>" + description + "</h6><br><a href=Etablissement\\Details\\" + idEtbl + ">Détails</a>");
                }

                else {
                    var popup = new mapboxgl.Popup()
                        .setHTML
                        ("<img src='" + logo + "' width='80'/><h4>" + nom + "</h4><br><h6>" + description + "</h6><br><a href=Etablissement\\Details\\" + idEtbl + ">Détails</a>");
                }
                var marker = new mapboxgl.Marker() // initialize a new marker
                    .setLngLat([long, lat]) // Marker [lng, lat] coordinates
                    .setPopup(popup)
                    .addTo(map) // Add the marker to the map
            }  
        })
    })

    ActualisationPage();
})
function ActualisationPage() {
    setTimeout("window.open(self.location, '_self');", 900000);
}