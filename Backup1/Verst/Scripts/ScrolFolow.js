//Получаем отступы для плавающей таблицы
var ie7 = $.browser.msie && $.browser.version == 7.0,
    fTop = 35,
    sTop;

if (ie7) {
    sTop = 160;
} else {
    sTop = 180;
}

if ($('#SideMenu').is(":visible")) { sTop += 30; }

//Возвращает верхний отступ плавающей таблицы при скроле
function GetTopForScroll() {
    return fTop;
}

//Возвращает отступ для таблицы поумолчаний - когда скрол неактивен/стартовая позиция
function GetTopDefault() {
    return sTop;
}