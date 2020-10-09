//$(function () {
//    if ($(document.body).height() < 500) {
//        window.parent.reinitIframeEND(500);
//    } else {
//        window.parent.reinitIframeEND($(document.body).height());
//    }
//});
//框架中刷新当前页面
function reload() {
    window.parent.reloadIframe();
}
//弹窗
function showPage(title, url, width, height) {
    window.parent.layer.open({
        type: 2,
        shade: [0.4, '#393D49'],
        scrollbar: false,
        move: false,
        shift: 1,
        area: [width, height],
        title: title,
        content: url
    });
}