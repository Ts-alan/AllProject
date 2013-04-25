$(document).ready(function() {
    $('.item').live('click', function() {
        var name = $(this).attr('name');
        window.location = "CompInfo.aspx?CompName=" + name;
    });
});