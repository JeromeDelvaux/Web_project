var logoSelect = document.getElementById("logo-label"),
    logoPreview = document.getElementById("logoPreview");

var nombreMaxPhoto;

//vérifie si il y a un nombre de photo max si oui l'assigne
function NombrePhoto() {
    if ($("#nbPhotos").length) {
        nombreMaxPhoto = $("#nbPhotos").val();
    }
    else {
        nombreMaxPhoto = 1;
    }
}



const tailleMaxImage = $("#tailleMaxImage").val();
const tailleMaxLogo = $("#tailleMaxLogo").val();


$(document).ready(function ()
{
    $("#logo-input").on("change", function ()
    {
        if (fileName !== null) {
            //vérifie la taille du fichier
            if (checkSize(this, tailleMaxLogo)) {
                var fileName = $(this).val().split("\\").pop();
                $(this).next("#logo-label").html(fileName);
            }
            else {
                toastRMaxSize("le logo", tailleMaxLogo);
            }

        }
        else {
            $(this).next("#logo-label").innerHTML = "Sélectionner l'image";
        }
    })

    $("[id*='photo-input-']").on("change", function () {
        if (fileName !== null)
        {
            //vérifie la taille du fichier
            if (checkSize(this, tailleMaxImage)) {
                var num = $(this).attr('id').split("-")[2];
                var labelTextBox = "#photo-label-" + num;

                //affiche le nom du fichier dans la textbox
                var fileName = $(this).val().split("\\").pop();
                $(this).next(labelTextBox).html(fileName);

                //affiche le premier bouton caché
                var nextButton = $("div[style*='display: none;']").filter("[id^='photo-']").first();
                if (nextButton !== null) {
                    nextButton.show();
                }
            }
            else {
                toastRMaxSize("une image",tailleMaxImage);
            }
        }
    })

    

    
});



window.onload = function () {

    //initialise le nombre de photos max
    NombrePhoto();

    //vérifie si le bouton précédement a un fichier sinon se cache
 
    for (var i = 1; i < nombreMaxPhoto; i++) {
        var logoId = "#photo-input-" + (i - 1);

        if ($(logoId).fileName !== null) {

            var photo = "#photo-" + i;
            $(photo).hide();
        }
        
    }

    checkValidation();
}

window.URL = window.URL || window.webkitURL;

function checkSize(element, tailleMax) {
    if (element.files[0].size > tailleMax) {

        //supprime le fichier
        element.value = "";

        return false;
    }    
    else
        return true;
}

function checkValidation() {
    if ($("#validation-summary").find("li")[0].innerHTML !== "") {

        toastr["warning"]("Si vous aviez chargé des images (photos, logo), elles doivent être rechargées")

        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    }
}

function toastRMaxSize(name, tailleMax) {

    var tailleMaxImageLisible = tailleMax / 1000;
    var message = "La taille maximum autorisée pour "+name+" est de " + tailleMaxImageLisible + " Ko";
    toastr["error"](message, "Image trop lourde");

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}


function handleLogo(files) {

    if (files[0].size <= tailleMaxImage) {
        logoPreview.innerHTML = "";

        
        var list = document.createElement("ul");
        list.id = "ulReviewLogo";
        list.className = "list-group";
        logoPreview.appendChild(list);

        var li = document.createElement("li");
        li.id = "";
        li.className = "list-group-item d-flex";
        list.appendChild(li);
        
        var div = document.createElement("div");
        div.id = "divReviewLogo";
        logoPreview.appendChild(div);

        var img = document.createElement("img");
        img.src = window.URL.createObjectURL(files[0]);
        img.height = 60;
        img.onload = function () {
            window.URL.revokeObjectURL(this.src);
        }
        img.className = " align-self-start p-2";
        li.appendChild(img);

        var buttonDelete = document.createElement("div");
        buttonDelete.className = "btn btn-danger align-self-center ml-auto p2";
        buttonDelete.innerHTML = "Supprimer";
        buttonDelete.onclick = supprimerLogo;
        buttonDelete.id = "deleteLogo";
        li.appendChild(buttonDelete);
    }
}


function handlePhotos(files, nummer) {

    if (files[0].size <= tailleMaxImage) {

        var photoPreview = document.getElementById("PhotosPreview");

        var list = document.createElement("ul");
        list.id = "ulReviewPhoto-" + nummer;
        list.className = "list-group";
        photoPreview.appendChild(list);

        var li = document.createElement("li");
        li.id = "";
        li.className = "list-group-item d-flex";
        list.appendChild(li);

        var img = document.createElement("img");
        img.src = window.URL.createObjectURL(files[0]);
        img.height = 60;
        img.className = " align-self-start p-2";
        img.onload = function () {
            window.URL.revokeObjectURL(this.src);
        }
        li.appendChild(img);

        var buttonDelete = document.createElement("div");
        buttonDelete.className = "btn btn-danger align-self-center ml-auto p2";
        buttonDelete.innerHTML = "Supprimer";
        buttonDelete.onclick = supprimerPhoto;
        buttonDelete.id = "deletePhoto-" + nummer;
        li.appendChild(buttonDelete);
    }   
}



function supprimerLogo() {

    //supprime le File
    var controle = $("#logo-input");
    controle.replaceWith(controle.val('').clone(true));

    logoSelect.innerHTML = "Sélectionner l'image";
    logoPreview.innerHTML = "";
}

function supprimerPhoto() {
    
    var num = $(this).attr('id').split("-")[1];
    var idName = "#photo-input-" + num;

    //supprime le File
    var controle = $(idName);
    controle.replaceWith(controle.val('').clone(true));

    //supprime la preview
    var previewName = "ulReviewPhoto-" + num;

    //supprime la preview
    const element = document.getElementById(previewName);
    element.parentNode.removeChild(element);

    reinitializebutton(num);
}


function reinitializebutton(num) {

    //reinitialise le texte dans le champ
    var selectorName = "photo-label-" + num;
    var labelTextBox = document.getElementById(selectorName);
    labelTextBox.innerHTML = "Sélectionner l'image"; 

    //cache le button si il en reste plus d'un
    var lBoutonDesactives = $("div[style*='display: none;']").filter("[id^='photo-']");

    if (lBoutonDesactives.length !== (nombreMaxPhoto-1)) {
        var bouton = "#photo-" + num;
        $(bouton).hide();
    }
}



