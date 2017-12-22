$(document).ready(function () {
    $('nav ul li').hover(function () {
        $('ul', this).slideUp(0).stop(true, true).slideDown(300);
    },
	function () {
	    $('ul', this).css("display", "block").stop(true, true).delay(100).slideUp(300);
	});

    var mWidth = $('#main').width();

    if ($('aside').width() == null)
        $('#maincol').width('100%');
    else

        $('#maincol').width(mWidth - $('aside').width() - 45);

    $('.accordionButton').click(function () {
        $('.accordionButton').parent().removeClass('on');
        $('.accordionContent').slideUp('fast');
        if ($(this).next().is(':hidden') == true) {
            $(this).parent().addClass('on');
            $(this).next().slideDown('fast');
        }
    });
    $('.accordionButton').mouseover(function () {
        $(this).addClass('over');

    }).mouseout(function () {
        $(this).removeClass('over');
    });
    $('.accordionContent').hide();

    $(function () {
        var marginBox = Math.round($('#searchBoxTop').width() / 2) + 25;
        var tablewidth = $('#listHead').width();
        var searchwidth = $('#searchBoxTop').width();
        var searchHeight = $('#searchBoxTop').height() + 25;
        var smallMenu = false;
        var sticky_navigation_offset_top = $('#searchBoxTop').offset().top;

        var sticky_navigation = function () {
            var scroll_top = $(window).scrollTop();
            var addit = 220;
            var wHeight = $(window).height();
            var dHeight = $(document).height();
            var allowed = dHeight > (wHeight + addit);
            if (allowed) {
                if (scroll_top > sticky_navigation_offset_top) {
                    if (smallMenu == true) {
                        $('#searchBoxTop').css({ 'position': 'fixed', 'top': 0, 'left': '50%', 'width': searchwidth, 'margin-left': -marginBox });
                        $('.invisible').css({ 'display': 'block' });
                        $('#accordionList #listHead').css({ 'position': 'fixed', 'top': searchHeight, 'left': '50%', 'width': tablewidth, 'margin-left': -marginBox, 'padding-top': '10px' });
                        $('#accordionList').css({'margin-top':'180px'});
                        smallMenu = false;
                    }
                } else {
                    if (smallMenu == false) {
                        $('#searchBoxTop').css({ 'position': 'relative', 'top': '0px', 'left': '0px', 'width': 'auto', 'margin-left': '0px' });
                        $('.invisible').css({ 'display': 'none' });
                        $('#accordionList #listHead').css({ 'position': 'relative', 'top': '0px', 'left': '0px', 'width': 'auto', 'margin-left': '0px', 'padding-top': '0px' });
                        $('#accordionList').css({ 'margin-top': '0px' });
                        smallMenu = true;
                    }
                }
            }
        };
        sticky_navigation();
        $(window).scroll(function () {
            sticky_navigation();
        });
        $('a[href="#"]').click(function (event) {
            event.preventDefault();
        });
    });
});