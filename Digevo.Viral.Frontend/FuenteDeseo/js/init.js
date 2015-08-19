var day = get('sel1');
var month = get('sel2');
var year = get('sel3');

/*******************************/
/*            Misc             */
/*******************************/

function get(name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    else {
        return results[1] || 0;
    }
}

function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

/*******************************/
/*       Event triggers        */
/*******************************/

//function onDialogBirthdate(target, form) {
//    if ($("#sel1").val() <= 0)
//        $("#formDay").addClass("has-error");
//    if ($("#sel2").val() <= 0)
//        $("#formMonth").addClass("has-error");
//    if ($("#sel3").val() <= 0)
//        $("#formYear").addClass("has-error");

//    if ($("#sel1").val() > 0 && $("#sel2").val() > 0 && $("#sel3").val() > 0) { //Ready to calculate
//        var birthArcane = getArcane($("#sel1").val() + $("#sel2").val() + $("#sel3").val());
//        var currentArcane = getArcane($("#sel1").val() + $("#sel2").val() + (new Date().getFullYear()));
//        userData.Arcane = arcanes.indexOf(birthArcane);

//        window.location = target + "?campaignId=2&birthday=" + birthday + "&bA=" + birthArcane.toString() + "&=cA" + currentArcane.toString();
//        window.location = target + "?" + form;

//        $("#imgBirthArcane").attr("src", birthArcane.src);
//        $("#imgCurrentArcane").attr("src", currentArcane.src);

//        $("#dialogBirthdate").toggle();
//        $("#dialogArcane").toggle();

//        onUserdataChanged(false);
//    }
//}

function ready()
{
    var birthArcane = getArcane(parseInt(day) + parseInt(month) + parseInt(year));
    var currentArcane = getArcane(parseInt(day + month) + (new Date().getFullYear()));

    $("#arcano-o").attr("src", birthArcane.src);
    $("#arcano-a").attr("src", currentArcane.src);
    
    userData.Arcane = arcanes.indexOf(birthArcane);
    onUserdataChanged(false);
}

function onModalPhoneSend() {
    if ($("#mobilenumber").val().length < 8) {
        $("#mobilenumber").addClass("has-error");
        return;
    }

    $("#modalEnterPhone").modal('hide');
    $("#modalCompleteConversion").modal('show');
    onUserdataChanged(true);
}

function onModalCompleteConversion() {
    $("#modalCompleteConversion").modal('hide');
    onUserdataChanged(true);
}


function onDialogSubmit() {
    onUserdataChanged(true);
    $("#modalEnterPhone").modal('show');
}

var arcanes = [{ "titulo": "El MAGO", "src": "img/arc/1.jpg" }, { "titulo": "LA SACERDOTISA", "src": "img/arc/2.jpg" }, { "titulo": "LA EMPERATRIZ", "src": "img/arc/3.jpg" }, { "titulo": "EL EMPERADOR", "src": "img/arc/4.jpg" }, { "titulo": "EL SUMO SACERDOTE", "src": "img/arc/5.jpg" }, { "titulo": "LOS ENAMORADOS", "src": "img/arc/6.jpg" }, { "titulo": "EL CARRO", "src": "img/arc/7.jpg" }, { "titulo": "LA FUERZA", "src": "img/arc/8.jpg" }, { "titulo": "EL HERMITAÑO", "src": "img/arc/9.jpg" }];
var userData = { "Name": "", "Phone": "", "Birthday": "", "Email": "", "Arcane": "0", "ArcaneQuery": "" };
var birthday = new Date(year, (month > 0 ? month : 1) - 1, day);

function onUserdataChanged(triggerServiceCall) {
    userData.Name = $("#txtName").val();
    userData.Phone = $("#mobilenumber").val();
    userData.Birthday = birthday;
    userData.ArcaneQuery = $("#txtArcaneQuery").val();

    // Calling web service by update request
    if (triggerServiceCall) {
        $.ajax({
            type: "POST",
            cache: false,
            url: "http://54.207.51.154:3567/api/landing?campaignId=2",
            //url: "http://localhost:3567/api/landing?campaignId=2",
            data: "=" + encodeURIComponent(JSON.stringify(userData)),
            crossDomain: true,
            processData: false,
            success: function () {
                // Success callback
            },
            error: function () {
                // Failure callback
            }
        });
    }
}

/*******************************/
/*      Arcane calculator      */
/*******************************/

function parseArcaneDate(value) {
    if (value <= arcanes.length)
        return value;

    var total = 0;
    for (var i = 0; i < value.toString().length; i++)
        total += parseInt(value.toString().substring(i, i + 1));

    return total <= arcanes.length ? total : parseArcaneDate(total);
}

function getArcane(value) {
    return arcanes[parseArcaneDate(value) - 1];
}

/*******************************/
/*            Load             */
/*******************************/

$(window).load(function () {
    if (day == null || month == null || year == null || !isNumeric(day) || !isNumeric(month) || !isNumeric(year))
        window.location = "index.html";
    else {
        $("#date").html(day + "-" + month + "-" + year);
        ready();
    }

    $("#mobilenumber").intlTelInput({
          onlyCountries: ["co"],
          nationalMode: false,
          autoPlaceholder: true,
          defaultCountry: "co"
      });
});