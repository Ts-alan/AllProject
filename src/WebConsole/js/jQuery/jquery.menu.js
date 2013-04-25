if (jQuery) (function () {
    $.extend($.fn, {
        menu: function (properties, callback) {
            if (properties.menu == undefined) return false;
            var openSelector = $(this);
            var menuSelector = "#" + properties.menu;
            $(menuSelector).find(".MenuItem").hover(
            function () {
                $(this).addClass("active");
            },
            function () {
                $(this).removeClass("active");
            })
            .unbind("click")
            .click(
            function () {
                if ($(this).hasClass("disabled")) return;
                $(menuSelector).removeClass("active");
                if (callback) {
                    callback($(this).attr('val'));
                }
            });

            $(openSelector).click(
            function () {
                if ($(menuSelector).hasClass("disabled")) return;
                if (!$(menuSelector).hasClass("active")) {
                    $(menuSelector).css({ top: $(openSelector).offset().top + $(openSelector).outerHeight(),
                        left: $(openSelector).offset().left
                    });
                    setTimeout(function () { // Delay for Mozilla
                        var handler = function () {
                            $(menuSelector).removeClass("active");
                            $(document).unbind('click', handler);
                        }
                        $(document).bind('click', handler)
                    }, 0);
                }
                $(menuSelector).toggleClass("active");
            }
            );
        },

        disableMenuItems: function (val) {
            if (val == undefined) return;
            var menuSelector = $(this);
            $(menuSelector).find(".MenuItem").each(function () {
                if ($(this).attr('val') == val) {
                    $(this).addClass("disabled")
                }
            });
            return ($(this));
        },

        enableMenuItems: function (val) {
            if (val == undefined) return;
            var menuSelector = $(this);
            $(menuSelector).find(".MenuItem").each(function () {
                if ($(this).attr('val') == val) {
                    $(this).removeClass("disabled")
                }
            });
            return ($(this));
        },

        disableMenu: function () {
            $(this).addClass('disabled');
        },

        enableMenu: function () {
            $(this).removeClass('disabled');
        }

    });
})(jQuery);