//Подсветка выбранной строки
var $objKpr, $objNop;  
    $('.table-focus-color').live("click", function () {
        if (typeof $objKpr != 'undefined') {
            $objKpr.closest('tr').removeClass("focus-color");
        }
        $objKpr = $(this);
        $objKpr.closest('tr').addClass("focus-color");
        //alert("КУ");
    });
    $('.ww').live("click", function () {
        if (typeof $objNop != 'undefined') {
            $objNop.closest('tr').removeClass("focus-color");
        }
        $objNop = $(this);
        $objNop.closest('tr').addClass("focus-color");
        //alert("КУ");
    });
    