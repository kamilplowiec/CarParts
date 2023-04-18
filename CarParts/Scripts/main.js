$(function () {

    $('.main .panel').hover(function () {
        $(this).find('.bottom-info').slideDown(100);
    }, function () {
        $(this).find('.bottom-info').slideUp(100);
    });

    
});