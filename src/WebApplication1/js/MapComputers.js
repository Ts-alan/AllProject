$(document).ready(function() {
    $('.item').bind('click', function() {
        var name = $(this).attr('name');
        window.location = "CompInfo.aspx?CompName=" + name;
    });
});