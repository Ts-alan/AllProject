//Создаём массив просмотренных страниц
var NavigationCache = new Array();
$(document).ready(function () {
    if ($('#PageTitle').val() != "Сопроводительный лист") {
        //Заносим в массив текущую страницу
        var pageObj = { page: $('#Content').html(), title: $('#PageTitle').val() };
        NavigationCache[window.location.pathname] = pageObj;
        history.pushState({ page: window.location.pathname, type: "page" }, $('#PageTitle').val(), window.location.pathname);
        //Устанавливаем заголовок страницы вручную получая его из скрытого поля
        document.title = $('#PageTitle').val();
    }
    return false;
});

function setPage(page) {
    $('#Content').html("<div class='centerDom'>Загрузка</div><div class='test_img'></div>");
    
    $.get(page, function (data) {
        $('#Content').html(data);
        document.title = $('#PageTitle').val();
        //Устанавливаем заголовок страницы вручную получая его из скрытого поля
        var pageObj = { page: data, title: $('#PageTitle').val() };
        NavigationCache[page] = pageObj;
        history.pushState({ page: page, type: "page" }, $('#PageTitle').val(), page);}, "html")
    .fail(function(xhr, status, error) {alert("Произошла ошибка! : " + status + "\nError: " + error); })
    .done(function () { });
    return false;
};

$(document).ready(function () {
    if (history.pushState) {
        window.onpopstate = function (event) {
            if (event.state != null) {
                if (event.state.type.length > 0) {
                    if (NavigationCache[event.state.page] != "undefined") {
                        $('#Content').html(NavigationCache[event.state.page].page);
                        document.title = NavigationCache[event.state.page].title;
                    }
                }
            }
        };

        $('.navigator').live("click", function () {
            setPage($(this).attr('href'));
            $('.dropdown').removeClass("open");
            return false;
        });
    }
});

//Добавление страницы в историю "вручную"
function ManualSave(page) {
    var pageObj = { page: $('#Content').html(), title: $('#PageTitle').val() };
        NavigationCache[page] = pageObj;
        history.pushState({ page: page, type: "page" }, $('#PageTitle').val(), page);

        $('#datepicker1').datepicker("setDate", new Date('@now.Year', '@now.Month' - 1, 01));
        $('#datepicker2').datepicker("setDate", new Date('@now.Year', '@now.Month' - 1, '@now.Day'));
}
