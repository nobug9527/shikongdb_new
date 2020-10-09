$('.jiahao').on('click', function () {
    $(this).toggleClass('roteta');
    $('.boss').toggle();
})
setInterval(function () {
    var aaa = $('.boss>div').width();
    var bbb = $('.boss>div>img').width();
    $('.boss').find('div').css('height', aaa * 0.7+ 'px');
    $('.boss').find('div').find('img').css('height', bbb * 1.2 + 'px');
    $('.boss').find('div').find('p').css('font-size', bbb / 5 + 'px');
}, 1)

//$('.dibu').find('a').eq(3).click(function () {
//    window.location.href = "./动态钱包.html"
//})
//$('.dibu').find('a').eq(4).click(function () {
//    window.location.href = "./静态钱包.html"
//})
//$('header').find('a').eq(0).click(function () {
//    window.location.href = "/M_Admin/Index"
//})
$('.boss').find('div').eq(0).click(function () {
    window.location.href = "/M_User/Index"
})
$('.boss').find('div').eq(1).click(function () {
    window.location.href = "/M_Profit/StaticIndex"
})
$('.boss').find('div').eq(2).click(function () {
    window.location.href = "/M_Transfer/OrderList"
})
$('.boss').find('div').eq(3).click(function () {
    window.location.href = "/M_User/UserTeam"
})
$('.boss').find('div').eq(4).click(function () {
    window.location.href = "/M_News/Index"
})
$('.boss').find('div').eq(5).click(function () {
    $.ajax({
        type: 'POST',
        url: '/M_Admin/LogoutJsonResult',
        dataType: 'json',
        success: function (data) {
            if (data.Code) {
                window.location.href = "/M_Admin/Login";
            } else {
                layer.msg(data.Msg);
            }
        },
        error: function (data) {
            layer.closeAll('loading');
            layer.msg("网络连接失败");
        }
    });
})



