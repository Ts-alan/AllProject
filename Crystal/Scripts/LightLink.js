//Для работы со скриптом необходимо добавить класс "lagting" для элемента..
$(document).ready(function () {
    $('.lagting').live("click", function () {
        if (typeof $objLight != 'undefined') {
            $objLight.closest('tr').css("background-color", "white");

        }
        $objLight = $(this);
        $objLight.closest('tr').css("background", "#ADDEFF");

    });
});