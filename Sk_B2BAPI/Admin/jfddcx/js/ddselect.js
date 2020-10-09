//计算返回data数据的页数
function getpage(count) {
    var page = Math.ceil(parseInt(count) / 15);
    document.getElementById("PageCount").innerHTML = page;
    document.getElementById("RecordCount").innerHTML = count;
}
//首页
function btnfirst(obj) {
    document.getElementById("PageIndex").innerHTML = 1;
    obj();
}
//尾页
function btnlast(obj) {
    var page = document.getElementById("PageCount").innerHTML;
    document.getElementById("PageIndex").innerHTML = page;
    obj();
}
//上翻页
function btnup(obj) {
    var page = document.getElementById("PageCount").innerHTML;

    var number = document.getElementById("PageIndex").innerHTML;
    if (parseInt(number) <= 1) {
        alert("当前页面为首页，无法继续翻页", function () { }, 'f');
    }
    else if (parseInt(number) > 1) {
        var newpage = parseInt(number) - 1
        document.getElementById("PageIndex").innerHTML = newpage;
        obj();
    }
}
//下翻页
function btnnext(obj) {
    var page = document.getElementById("PageCount").innerHTML;

    var number = document.getElementById("PageIndex").innerHTML;
    if (parseInt(number) >= parseInt(page)) {
        alert("单位页面为尾页，无法继续翻页", function () { }, 'f');
    }
    else if (parseInt(number) < parseInt(page)) {
        var newpage = parseInt(number) + 1
        document.getElementById("PageIndex").innerHTML = newpage;
        obj();
    }
}
$(function () {
    ddSelect();

});
function ddSelect() {
    var username = $('#username').val();
    var page = $('#PageIndex').html();
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/dd_select_hz.ashx?username=" + encodeURI(username)
        + "&page=" + encodeURI(page),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var type = JSON["return_code"];
            var count = JSON["pageCount"];
            if (type == "0") {
                $('#TBShow').empty();
                var tb = "";
                getpage(count);
                for (var i = 0; i < JSON["data"].length; i++) {
                    var zt = JSON["data"][i]["zt"];
                    var msg = "";
                    if (zt==0) {
                        msg = "已兑换";
                    }
                    tb = tb + "<tr class='chcol' id='spzztr" + i + "' >"
                                + "<td align='center'>" + JSON["data"][i]["djbh"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["nick_name"] + "</td>"                  
                                + "<td align='center'>" + JSON["data"][i]["rq"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["ontime"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["salesman"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["shl"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["totaljf"] + "</td>"
                                + "<td align='center'>" + msg + "</td>"
                                + "<td align='center'><a href='#' onclick=showmx('" + JSON["data"][i]["djbh"] + "')>查看明细</a></td>"
                     + "</tr>"
                }
                $("#TBShow").append(tb);

            } else {
                $('#TBShow').empty();
                var tb = "<tr><td colspan='20' style='text-align:center'>暂无数据</td></tr>";
                $('#TBShow').append(tb);
            }
        }
    });

}

function showmx(djbh) {
    layer.open({
        type: 1,
        shadeClose: true,
        content: $("#showmx"),
        area: ["60%", "70%"],
        title: '单据编号：' + djbh + "明细",
        end: function () {
            $("#showmx").hide();
        },
        cancel: function () {
            $("#showmx").hide();
        }
    });
    showmxx(djbh);
}

function showmxx(djbh) {
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ddmx.ashx?djbh=" + encodeURI(djbh),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var type = JSON["return_code"];
            var count = JSON["pageCount"];
            if (type == "0") {
                $('#Tbody1').empty();
                var tb = "";
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='spzztr" + i + "' >"
                                + "<td align='center'>" + JSON["data"][i]["djbh"] + "</td>"
                              + "<td align='center'>" + JSON["data"][i]["rq"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["ontime"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["goodsname"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["dw"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["shl"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["jf"] + "</td>"
                                + "<td align='center'>" + JSON["data"][i]["totaljf"] + "</td>"
                     + "</tr>"
                }
                $("#Tbody1").append(tb);

            } else {
                $('#Tbody1').empty();
                var tb = "<tr><td colspan='20' style='text-align:center'>暂无数据</td></tr>";
                $('#Tbody1').append(tb);
            }
        }
    });
}