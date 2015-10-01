/* ---------------------------------------------- /*
 * smoothscroll - Scroll animation
/* ---------------------------------------------- */

$(function () {

    var defaults = {
        offset: 0,
        duration: 1000
    };

    $.smoothscroll = {
        defaults: $.extend({}, defaults)
    };

    $(".smooth-scroll").on("click.smoothscroll", function (e) {
        var anchor = $(this),
            offset = anchor.data("offset") || $.smoothscroll.defaults.offset,
            duration = anchor.data("duration") || $.smoothscroll.defaults.duration;

        $("html, body").stop().animate({
            scrollTop: $(anchor.attr("href")).offset().top - offset
        }, duration);
        e.preventDefault();
    });

});
