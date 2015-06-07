var monthNames = ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"];

function FuckingIEHack(monthN) {
    var value = "This IE Sucks";
    for (var i = 0; i < monthNames.length; i++) {
        value = monthNames[i];
        if (i == monthN) {
            break;
        }
    }
    return value;
}

function AjaxBegin() {
    $('#ProgressBar').removeClass("progress-danger");
    $('#ProgressBar').addClass("active");
}

function AjaxError() {
    $('#ProgressBar').addClass("progress-danger");
    $('#ProgressBar').removeClass("active");
}

function AjaxSuccessEnd(n,data) {
    if (n == 2)
    $("#prib").html(" по прибору <span class='font' id='re'> " + data + "</span>");
    $("#PribMonthRepordData"+n).removeClass("loader1");
}
