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
//获取url参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
//首页
function btnfirst(SearchInfo) 
{
    $("#pageIndex").html(1);
    var pageCount = $("#pageCount").html();
    SearchInfo();
}
//尾页
function btnlast(SearchInfo) {
    var pageCount = $("#pageCount").html();
    $("#pageIndex").html(pageCount);
    SearchInfo();
}
//上翻页
function btnup(SearchInfo) {
     var pageCount = $("#pageCount").html();
     var PageIndex=$("#pageIndex").html();
     if (parseInt(PageIndex) <= 1) {
        alertFun("当前页面为首页，无法继续翻页", function () { }, 'f');
    }
     else if (parseInt(PageIndex) > 1) {
         var npageIndex = parseInt(PageIndex) - 1
         $("#pageIndex").html(npageIndex);
         SearchInfo();
    }
}
//下翻页
function btnnext(SearchInfo) {
    var pageCount = $("#pageCount").html();
    var PageIndex = $("#pageIndex").html();
    if (parseInt(PageIndex) >= parseInt(pageCount)) {
        alertFun("单位页面为尾页，无法继续翻页", function () { }, 'f');
    }
    else if (parseInt(PageIndex) < parseInt(pageCount)) {
        var nPageIndex = parseInt(PageIndex) + 1
        $("#pageIndex").html(nPageIndex);
        SearchInfo();
    }
}

//一周销量排行查询
function Search_SalesVolume() {
    openLoading();
    var Type = "SalesVolume";
    var sqltype = "GetTJGoods";
    var Procedure = "Search_GoodsInfo";
    var paramcont = "Type=" + Type+"&SqlType=" + sqltype + "&Procedure=" + Procedure;
    $.ajax({
        type: "Post",
        cache: false,
        url: "../ashx/GetInfoList.ashx?" + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                var tb = "";
                $("#SalesRankUL").empty();
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb += "<li class='child' style='width:250px;height:210px'>";
                    tb += "<a href=\"/goods/show-" + JSON["data"][i]["id"] + ".html\" class='list'>";
                    tb += "<img style='width:250px;height:210px' class='fly_img4530' src=\"" + JSON["data"][i]["img_url"] + "\" alt=\"" + JSON["data"][i]["title"] + "\"/>";
                    tb += "</a>";
                    tb += "</li>";
                }
                $("#SalesRankUL").append(tb);
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading();
                $("#SalesRankUL").empty();
                var tb = "<li>暂无<li>"
                $("#SalesVolumeTB").append(tb);
            }
            if (TYPE == '2') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading(); }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            //alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading();
        }
    })
}
function LogOut()
{
    $.ajax({
        type: "Post",
        cache: false,
        url: "../ashx/GetInfoList.ashx?Type=LogOut",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                closeLoading();
                window.location.href = "/login.html";
            }
            else {
                alertFun("退出失败", function () { closeLoading(); }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeLoading();
        }
    })
}

//删除元素
function hintRemove(obj) {
    $(obj).remove();
}
//confirm 方法

(function ($) {

    $.alerts = {
        alert: function (title, message, callback) {
            if (title == null) title = 'Alert';
            $.alerts._show(title, message, null, 'alert', function (result) {
                if (callback) callback(result);
            });
        },

        confirm: function (title, message, callback) {
            if (title == null) title = 'Confirm';
            $.alerts._show(title, message, null, 'confirm', function (result) {
                if (callback) callback(result);
            });
        },


        _show: function (title, msg, value, type, callback) {

            var _html = "";

            _html += '<div id="mb_box"></div><div id="mb_con"><span id="mb_tit">' + title + '</span>';
            _html += '<div id="mb_msg">' + msg + '</div><div id="mb_btnbox">';
            if (type == "alert") {
                _html += '<input id="mb_btn_ok" type="button" value="同意" />';
            }
            if (type == "confirm") {
               // _html += '<input id="mb_btn_ok" type="button" value="同意" />';
                _html += '<input id="mb_btn_no" type="button" value="返回" />';
            }
            _html += '</div></div>';

            //必须先将_html添加到body，再设置Css样式
            $("body").append(_html); GenerateCss();

            switch (type) {
                case 'alert':

                    $("#mb_btn_ok").click(function () {
                        $.alerts._hide();
                        callback(true);
                    });
                    $("#mb_btn_ok").focus().keypress(function (e) {
                        if (e.keyCode == 13 || e.keyCode == 27) $("#mb_btn_ok").trigger('click');
                    });
                    break;
                case 'confirm':

                    $("#mb_btn_ok").click(function () {
                        $.alerts._hide();
                        if (callback) callback(true);
                    });
                    $("#mb_btn_no").click(function () {
                        $.alerts._hide();
                        if (callback) callback(false);
                    });
                    $("#mb_btn_no").focus();
                    $("#mb_btn_ok, #mb_btn_no").keypress(function (e) {
                        if (e.keyCode == 13) $("#mb_btn_ok").trigger('click');
                        if (e.keyCode == 27) $("#mb_btn_no").trigger('click');
                    });
                    break;


            }
        },
        _hide: function () {
            $("#mb_box,#mb_con").remove();
        }
    }
    // Shortuct functions
    zdalert = function (title, message, callback) {
        $.alerts.alert(title, message, callback);
    }

    zdconfirm = function (title, message, callback) {
        $.alerts.confirm(title, message, callback);
    };




    //生成Css
    var GenerateCss = function () {

        $("#mb_box").css({
            width: '100%', height: '100%', zIndex: '99999', position: 'fixed',
            filter: 'Alpha(opacity=60)', backgroundColor: 'black', top: '0', left: '0', opacity: '0.6'
        });

        $("#mb_con").css({
            zIndex: '999999', width: '50%', position: 'fixed',
            backgroundColor: 'White', borderRadius: '15px'
        });

        $("#mb_tit").css({
            display: 'block', fontSize: '14px', color: '#444', padding: '10px 15px',
            backgroundColor: '#DDD', borderRadius: '15px 15px 0 0',
            borderBottom: '3px solid #009BFE', fontWeight: 'bold'
        });

        $("#mb_msg").css({
            padding: '20px', lineHeight: '20px',
            borderBottom: '1px dashed #DDD', fontSize: '13px'
        });

        $("#mb_ico").css({
            display: 'block', position: 'absolute', right: '10px', top: '9px',
            border: '1px solid Gray', width: '18px', height: '18px', textAlign: 'center',
            lineHeight: '16px', cursor: 'pointer', borderRadius: '12px', fontFamily: '微软雅黑'
        });

        $("#mb_btnbox").css({ margin: '15px 0 10px 0', textAlign: 'center' });
        $("#mb_btn_ok,#mb_btn_no").css({ width: '85px', height: '30px', color: 'white', border: 'none' });
        $("#mb_btn_ok").css({ backgroundColor: '#168bbb' });
        $("#mb_btn_no").css({ backgroundColor: 'gray', marginLeft: '20px' });


        //右上角关闭按钮hover样式
        $("#mb_ico").hover(function () {
            $(this).css({ backgroundColor: 'Red', color: 'White' });
        }, function () {
            $(this).css({ backgroundColor: '#DDD', color: 'black' });
        });

        var _widht = document.documentElement.clientWidth; //屏幕宽
        var _height = document.documentElement.clientHeight; //屏幕高

        var boxWidth = $("#mb_con").width();
        var boxHeight = $("#mb_con").height();

        //让提示框居中
        $("#mb_con").css({ top: (_height - boxHeight) / 2 + "px", left: (_widht - boxWidth) / 2 + "px" });
    }


})(jQuery);

///代客下单跳转到购物车
function OpenCart() {
    var user_id = GetQueryString("user_id");
    if (user_id == "" || user_id == null) {
        alertFun("请选择客户！", function () { }, 'f');
    }
    else {
        window.location = "../ValetOrder/gwc.aspx?user_id=" + encodeURI(user_id);
    }
}