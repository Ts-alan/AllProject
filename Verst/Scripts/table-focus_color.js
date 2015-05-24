var  $objKpr, $objNop;
$('.table-focus-color').live("click", function () {
    if (typeof $objKpr != 'undefined') {
        $objKpr.closest('tr').removeClass("focus-color");
    }
    $objKpr = $(this);
    $objKpr.closest('tr').addClass("focus-color");
    $('div').click(function() {
        alert($('table').index(this));
    });
});
