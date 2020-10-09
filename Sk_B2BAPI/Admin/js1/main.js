//开始日期的设置
var start = {
    elem: '#startDate',
    choose: function (datas) {
        end.min = datas; //开始日选好后，重置结束日的最小日期
        end.start = datas; //将结束日的初始值设定为开始日
    }
};
//结束日期
var end = {
    elem: '#endDate',
    choose: function (datas) {
        start.max = datas; //结束日选好后，重置开始日的最大日期
    }
};

var starts = {
    elem: '#startDate',
    choose: function (datas) {
        ends.min = datas; //开始日选好后，重置结束日的最小日期
        ends.start = datas; //将结束日的初始值设定为开始日
    }
};
//结束日期
var ends = {
    elem: '#endDate',
    choose: function (datas) {
        starts.max = datas; //结束日选好后，重置开始日的最大日期
    }
};

$(function () {
//    1、初始化开始日期和结束日期
        var newDate = new Date();
        var year = newDate.getFullYear();
        var month = newDate.getMonth() + 1;
        if (month < 10) {
            month = "0" + month;
        }
        var day = newDate.getDate(); 
		if (day < 10) {
        day = "0" + day;
    }
        var startDate = year + "-" + month + "-01";
        var endDate = year + "-" + month + "-" + day;
        $("#startDate").val(startDate); //初始化开始日期
        $("#endDate").val(endDate); //初始化结束日期
    //2、设置body的最小高度，控制底部footer的位置
    var winH = $(window).height();
    var bodyH = $("body").outerHeight();
    var footerH = $(".footer").outerHeight();
    var minH = winH - footerH;
    $("body").css({ "position": "relative", "padding-bottom": footerH, "min-height": winH });

    //点击遮罩层空白区域关闭弹框
    $("#mask").click(function () {
        $(".popup").fadeOut().addClass("hide");
        $("#mask").fadeOut().addClass("hide");
    });
});


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
function confirmFun(text, callback) {
    layconfirm(text, '提示', '确认', function () {
        if (typeof callback == 'function')
            callback();
    }, '取消', function () { });
}
function checkStaus(status, message, callback) {
    if (status == "2") {
        alertFun("登陆超时，请重新登陆！", function () { window.location.href = "../Default.aspx"; }, 'w');
    } else if (status == "3") {
        alertFun("软件注册验证失败！", function () { }, 'f');
    } else if (status == "0") {
        if (typeof callback == "function") {
            callback();
        }
    }
    else {
        alertFun(message,'','f');
    }
}
//获取时间控件方法
function setSJHTML(id) {
    var contHtml = "";
    contHtml += "<select id=\"hour_" + id + "\"  class=\"input normal\" style=\"width:5%;margin-left:30px;\">";
    contHtml += "<option value=\"00\">00</option>";
    contHtml += "<option value=\"01\">01</option>";
    contHtml += "<option value=\"02\">02</option>";
    contHtml += "<option value=\"03\">03</option>";
    contHtml += "<option value=\"04\">04</option>";
    contHtml += "<option value=\"05\">05</option>";
    contHtml += "<option value=\"06\">06</option>";
    contHtml += "<option value=\"07\">07</option>";
    contHtml += "<option value=\"08\">08</option>";
    contHtml += "<option value=\"09\">09</option>";
    contHtml += "<option value=\"10\">10</option>";
    contHtml += "<option value=\"11\">11</option>";
    contHtml += "<option value=\"12\">12</option>";
    contHtml += "<option value=\"13\">13</option>";
    contHtml += "<option value=\"14\">14</option>";
    contHtml += "<option value=\"15\">15</option>";
    contHtml += "<option value=\"16\">16</option>";
    contHtml += "<option value=\"17\">17</option>";
    contHtml += "<option value=\"18\">18</option>";
    contHtml += "<option value=\"19\">19</option>";
    contHtml += "<option value=\"20\">20</option>";
    contHtml += "<option value=\"21\">21</option>";
    contHtml += "<option value=\"22\">22</option>";
    contHtml += "<option value=\"23\">23</option>";
    contHtml += "</select>";
    contHtml += "<span style=\"height:34px; padding:6px 12px;\">：</span>";
    contHtml += "<select id=\"minute_" + id + "\"  class=\"input normal\" style=\"width:5%;mrgin-left:30px;\">";
    contHtml += "<option value=\"00\">00</option>";
    contHtml += "<option value=\"01\">01</option>";
    contHtml += "<option value=\"02\">02</option>";
    contHtml += "<option value=\"03\">03</option>";
    contHtml += "<option value=\"04\">04</option>";
    contHtml += "<option value=\"05\">05</option>";
    contHtml += "<option value=\"06\">06</option>";
    contHtml += "<option value=\"07\">07</option>";
    contHtml += "<option value=\"08\">08</option>";
    contHtml += "<option value=\"09\">09</option>";
    contHtml += "<option value=\"10\">10</option>";
    contHtml += "<option value=\"11\">11</option>";
    contHtml += "<option value=\"12\">12</option>";
    contHtml += "<option value=\"13\">13</option>";
    contHtml += "<option value=\"14\">14</option>";
    contHtml += "<option value=\"15\">15</option>";
    contHtml += "<option value=\"16\">16</option>";
    contHtml += "<option value=\"17\">17</option>";
    contHtml += "<option value=\"18\">18</option>";
    contHtml += "<option value=\"19\">19</option>";
    contHtml += "<option value=\"20\">20</option>";
    contHtml += "<option value=\"21\">21</option>";
    contHtml += "<option value=\"22\">22</option>";
    contHtml += "<option value=\"23\">23</option>";
    contHtml += "<option value=\"24\">24</option>";
    contHtml += "<option value=\"25\">25</option>";
    contHtml += "<option value=\"26\">26</option>";
    contHtml += "<option value=\"27\">27</option>";
    contHtml += "<option value=\"28\">28</option>";
    contHtml += "<option value=\"29\">29</option>";
    contHtml += "<option value=\"30\">30</option>";
    contHtml += "<option value=\"31\">31</option>";
    contHtml += "<option value=\"32\">32</option>";
    contHtml += "<option value=\"33\">33</option>";
    contHtml += "<option value=\"34\">34</option>";
    contHtml += "<option value=\"35\">35</option>";
    contHtml += "<option value=\"36\">36</option>";
    contHtml += "<option value=\"37\">37</option>";
    contHtml += "<option value=\"38\">38</option>";
    contHtml += "<option value=\"39\">39</option>";
    contHtml += "<option value=\"40\">40</option>";
    contHtml += "<option value=\"41\">41</option>";
    contHtml += "<option value=\"42\">42</option>";
    contHtml += "<option value=\"43\">43</option>";
    contHtml += "<option value=\"44\">44</option>";
    contHtml += "<option value=\"45\">45</option>";
    contHtml += "<option value=\"46\">46</option>";
    contHtml += "<option value=\"47\">47</option>";
    contHtml += "<option value=\"48\">48</option>";
    contHtml += "<option value=\"49\">49</option>";
    contHtml += "<option value=\"50\">50</option>";
    contHtml += "<option value=\"51\">51</option>";
    contHtml += "<option value=\"52\">52</option>";
    contHtml += "<option value=\"53\">53</option>";
    contHtml += "<option value=\"54\">54</option>";
    contHtml += "<option value=\"55\">55</option>";
    contHtml += "<option value=\"56\">56</option>";
    contHtml += "<option value=\"57\">57</option>";
    contHtml += "<option value=\"58\">58</option>";
    contHtml += "<option value=\"59\">59</option>";
    contHtml += "</select>";
    return contHtml;
}






function changecolor(i) {
    $("#select" + (i) + "").removeClass("chcol")
    document.getElementById("select" + (i) + "").style.backgroundColor = "#87CEFA";
}
function deletetcolor(i) {
    $("#select" + (i) + "").removeClass("chcol")
    document.getElementById("select" + (i) + "").style.backgroundColor = "#FFFFFF";
}



function xchangecolor(i) {
    $("#xselect" + (i) + "").removeClass("chcol")
    document.getElementById("xselect" + (i) + "").style.backgroundColor = "#87CEFA";
}
function xdeletetcolor(i) {
    $("#xselect" + (i) + "").removeClass("chcol")
    document.getElementById("xselect" + (i) + "").style.backgroundColor = "#FFFFFF";
}
//分页控件
//首页
function btnfirst(QueGoods) {
    document.getElementById("PageIndex").innerHTML = 1;
    QueGoods();
}
//尾页
function btnlast(QueGoods) {
    var page = document.getElementById("PageCount").innerHTML;
    document.getElementById("PageIndex").innerHTML = page;
    QueGoods();
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
        QueGoods();
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
        QueGoods();
    }
}


//分页控件2---用于弹窗
//首页
function Gbtnfirst(QueGoods) {
    document.getElementById("GPageIndex").innerHTML = 1;
    QueGoods();
}
//尾页
function Gbtnlast(QueGoods) {
    var page = document.getElementById("GPageCount").innerHTML;
    document.getElementById("GPageIndex").innerHTML = page;
    QueGoods();
}
//上翻页
function Gbtnup(QueGoods) {
    var page = document.getElementById("GPageCount").innerHTML;
    var number = document.getElementById("GPageIndex").innerHTML;
    if (parseInt(number) <= 1) {
        alertFun("当前页面为首页，无法继续翻页", function () { }, 'f');
    }
    else if (parseInt(number) > 1) {
        var newpage = parseInt(number) - 1
        document.getElementById("GPageIndex").innerHTML = newpage;
        QueGoods();
    }
}
//下翻页
function Gbtnnext(QueGoods) {

    var page = document.getElementById("GPageCount").innerHTML;
    var number = document.getElementById("GPageIndex").innerHTML;
    if (parseInt(number) >= parseInt(page)) {
        alertFun("单位页面为尾页，无法继续翻页", function () { }, 'f');
    }
    else if (parseInt(number) < parseInt(page)) {
        var newpage = parseInt(number) + 1
        document.getElementById("GPageIndex").innerHTML = newpage;
        QueGoods();
    }
}


//获取url参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}




//弹窗分页首页
function Xbtnfirst(QueGoods) {
    document.getElementById("XPageIndex").innerHTML = 1;
    QueGoods();
}
//尾页
function Xbtnlast(QueGoods) {
    var page = document.getElementById("XPageCount").innerHTML;
    document.getElementById("XPageIndex").innerHTML = page;
    QueGoods();
}
//上翻页
function Xbtnup(QueGoods) {
    var page = document.getElementById("XPageCount").innerHTML;
    var number = document.getElementById("XPageIndex").innerHTML;
    if (parseInt(number) <= 1) {
        alertFun("当前页面为首页，无法继续翻页", function () { }, 'f');
    }
    else if (parseInt(number) > 1) {
        var newpage = parseInt(number) - 1
        document.getElementById("XPageIndex").innerHTML = newpage;
        QueGoods();
    }
}
//下翻页
function Xbtnnext(QueGoods) {

    var page = document.getElementById("XPageCount").innerHTML;
    var number = document.getElementById("XPageIndex").innerHTML;
    if (parseInt(number) >= parseInt(page)) {
        alertFun("单位页面为尾页，无法继续翻页", function () { }, 'f');
    }
    else if (parseInt(number) < parseInt(page)) {
        var newpage = parseInt(number) + 1
        document.getElementById("XPageIndex").innerHTML = newpage;
        QueGoods();
    }
}