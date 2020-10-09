function changecolor(i) {
    $("#select" + (i) + "").removeClass("chcol")
    document.getElementById("select" + (i) + "").style.backgroundColor = "#87CEFA";
}
function deletetcolor(i) {
    $("#select" + (i) + "").removeClass("chcol")
    document.getElementById("select" + (i) + "").style.backgroundColor = "#FFFFFF";
}
function openCover() {
    var html = "";
    html += "<div class=\"mask\" id=\"mask\" onclick=\"closeCover();\">";
    html += "</div>";
    html += "<div class=\"popup winPop\" id=\"winPop\">";
    html += "    <a class=\"closeBtn\" onclick=\"closeCover();\">";
    html += "        <img src=\"../img/close.png\" /></a>";
    html += "    <h3 class=\"winPopTit\" id=\"maskTitle\"></h3>";
    html += "    <div class=\"winPopCon\">";
    html += "        <h2 class=\"winPoph2\" id=\"maskTitle_left\"></h2>";
    html += "        <div id=\"maskContent\">";
    html += "        </div>";
    html += "    </div>";
    html += "    <div class=\"winPopConf\" id=\"maskFooter\">";
    html += "        <a class=\"btn btn-primary\">确 认</a>";
    html += "    </div>";
    html += "</div>";
    $("body").append(html);
    $("#winPop").removeClass("hide").fadeIn();
    $("#mask").removeClass("hide").fadeIn();
}
function closeCover() {
    $("#winPop").fadeOut().addClass("hide");
    $("#mask").fadeOut().addClass("hide");
    $("#mask").remove();
    $("#winPop").remove();
}
var index;
function openLoading() {
    index = layloading('g3', "加载中...");
}
function closeLoading() {
    layclose(index);
}
function alertFun(text, callback, type) {
    if (type == undefined)//s=success,w=waring,f=fail
        type = 's';
    layalert(type, text, '信息', '确定', function () {
        if (typeof callback == 'function')
            callback();
    });
}



//分页控件
//首页
function btnfirst(QueGoods) {
    document.getElementById("PageIndex").innerHTML = 1;
    Search_uploadimgInfo();
}
//尾页
function btnlast(QueGoods) {
    var page = document.getElementById("PageCount").innerHTML;
    document.getElementById("PageIndex").innerHTML = page;
    Search_uploadimgInfo();
}
//上翻页
function btnup(QueGoods) {
    var page = document.getElementById("PageCount").innerHTML;
    var number = document.getElementById("PageIndex").innerHTML;
    if (parseInt(number) <= 1) {
        alertFun("当前页面为首页，无法继续翻页", function () { }, 'f');
    }
    else if (parseInt(number) > 1) {
        var newpage = parseInt(number) - 1
        document.getElementById("PageIndex").innerHTML = newpage;
        Search_uploadimgInfo();
    }
}
//下翻页
function btnnext(QueGoods) {
    var page = document.getElementById("PageCount").innerHTML;
    var number = document.getElementById("PageIndex").innerHTML;
    if (parseInt(number) >= parseInt(page)) {
        alertFun("单位页面为尾页，无法继续翻页", function () { }, 'f');
    }
    else if (parseInt(number) < parseInt(page)) {
        var newpage = parseInt(number) + 1
        document.getElementById("PageIndex").innerHTML = newpage;
        Search_uploadimgInfo();
    }
}
//跳转页
function btntz(QueGoods) {
    var page = document.getElementById("PageCount").innerHTML;
    var number = document.getElementById("pagety").value();
    if (parseInt(number) >= parseInt(page)) {
        alertFun("单位页面为尾页，无法继续翻页", function () { }, 'f');
    }
    else if (parseInt(number) < parseInt(page)) {
        document.getElementById("pagety").value() = number;
        Search_uploadimgInfo();
    }
}

