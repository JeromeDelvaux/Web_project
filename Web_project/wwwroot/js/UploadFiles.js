$(document).ready(function () {
    $('.custom-file-input').on("change", function () {
        var fileLabel = $(this).next('.custom-file-label');
        var files = $(this)[0].files;
        if (files.length > 1) {
            fileLabel.html(files.length + ' fichiers sélectionnés');

            if (files.length > 5) {

            }
        }
        else if (files.length == 1) {
            fileLabel.html(files[0].name)
        }
    });
});
function HidePhotos () {
    if ($("#ulPreviewPhoto-0") == null) {
        $("#photodownload").show();
    }
    else {
        $("#photodownload").hide();
    }
}

function supprimerPhoto() {
    const element = document.getElementById("photoPreview");//recup de tout l'element photoPreview et suppression de tout les elements enfants.
    element.parentNode.removeChild(element);

    $("#photodownload").show();
}
