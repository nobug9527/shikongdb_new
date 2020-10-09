////管控业务员查询
function QueryOperatorControl()
{
    openLoading();
    var keyword = $("#keyword").val();
    var PageSize = $("#PageSize").html();
    var PageIndex = $("#PageIndex").html();
    var data = {
        type: "QueryYwyControl",
        sqlvalue: keyword,
        PageIndex: PageIndex,
        pageSize: PageSize,
    };
    var proc = "Proc_CJCX_Admin";//存储过程名
    var type = "ReturnList";

    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBShow').empty();
                var tb = "";
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb += "<tr class='chcol' align='center' id='select" + i + "'>"
                    tb += "<td>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["user_name"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["nick_name"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["flname"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["jibie"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["quyufl"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["AddTime"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["status"] + "</td>"
                    tb += "<td><a><span onclick='DltYwyControl(\"" + JSON["data"][i]["id"] + "\")'class='bz co'>删除</span></a></td>"
                    tb += "</tr>"
                }

                $("#TBShow").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("RecordCount").innerHTML = recordCount;
                document.getElementById("PageCount").innerHTML = pageCount;
                closeLoading();
            }
            if (TYPE == '1') {
                $('#TBShow').empty();
                var tb = "";
                tb = "<tr><td colspan='11' align='center'>无数据</td></tr>"
                $('#TBShow').append(tb);
                closeLoading()
            }
            if (TYPE == '2') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}
///删除绑定关系
function DltYwyControl(id)
{
    openLoading();
    var data = {
        type: "DltYwyControl",
        ywyid: id
    };
    var proc = "Proc_CJCX_Admin";//存储过程名
    var type = "ReturnNumber";

    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                closeLoading();
                QueryOperatorControl();
            }
            else {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}


///创建选择商品UI
function CreatGoodsInfoUI() {
    var contHtml = "";
    contHtml += "<a class='closeBtn' onclick='closeCover();'><img src='../img/close.png'/></a>"
    contHtml += "<h3 class='winPopTit' id='maskTitle'>选择业务员</h3>"
    contHtml += "<div class='winPopCon'>"
    contHtml += "<h2 class='winPoph2' id='maskTitle_left'></h2>"
    contHtml += "<div id='maskContent'>"
    contHtml += "<div class='conForm' id='cxtjdiv'>"
    contHtml += "<p><label>关键字：</label><input type='text' id='SqlValue'  placeholder='关键词搜索'>"
    contHtml += "<a class='btn btn-success' onclick='Get_YwyInfo()'><i class='searchIcon btnIcon'></i>查 询</a></p>"
    contHtml += "</div>"
    contHtml += "<div class='conTable'>"
    contHtml += "<table class=\"dataintable\" id='TBSHOW'>"
    contHtml += "<tr class='chcol'>"
    contHtml += "<tr><th>正在加载类容........</th></tr>"
    contHtml += "</table>"
    contHtml += "</div>"
    contHtml += "<div class='default' id='fanye'>"
    contHtml += "<p class='fl'>"
    contHtml += "<a onclick='Gbtnfirst(Get_YwyInfo)'>首页</a>"
    contHtml += "<a style='color:Blue' onclick='Gbtnup(Get_YwyInfo)'>上一页</a>"
    contHtml += "<a style='color:Blue' onclick='Gbtnnext(Get_YwyInfo)'>下一页</a>"
    contHtml += "<a onclick='Gbtnlast(Get_YwyInfo)'>尾页</a>"
    contHtml += "<span class='fr'>每页显示<b id='GPageSize'>15</b>条，共<b id='GRecordCount'>0</b>条,"
    contHtml += "当前页<b id='GPageIndex'>1</b>/<b id='GPageCount'>1</b></span>"
    contHtml += "</p>"
    contHtml += "</div>"
    contHtml += "</div>"
    contHtml += "</div>"
    document.getElementById("winPop").innerHTML = contHtml;
}
//获取商品信息
function Search_YwyInfo() {
    closeCover();
    openCover(); //生成弹窗
    CreatGoodsInfoUI();
    Get_YwyInfo();
}
function Get_YwyInfo() {
    openLoading();

    var sqlvalue = $("#SqlValue").val();
    var PageSize = $("#GPageSize").html();
    var PageIndex = $("#GPageIndex").html();
    var data = {
        type: "QueryYwy",
        sqlvalue: sqlvalue,
        PageIndex: PageIndex,
        PageSize: PageSize,
    };
    var proc = "Proc_CJCX_Admin";//存储过程名
    var type = "ReturnList";

    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBSHOW').empty();
                var tb = "<tr class='chcol'><th>序号</th><th>业务员编号</th><th>业务员名称</th><th>业务员分组</th><th>级别</th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='xselect" + i + "' ondblclick='ChooseGoods(\"" + JSON["data"][i]["id"] + "\")' onMouseOver='xchangecolor(\"" + i + "\")' onMouseOut='xdeletetcolor(\"" + i + "\")'>"
                                + "<td>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                                + "<td>" + JSON["data"][i]["user_name"] + "</td>"
                                + "<td>" + JSON["data"][i]["nick_name"] + "</td>"
                                + "<td>" + JSON["data"][i]["flname"] + "</td>"
                                + "<td>" + JSON["data"][i]["jibie"] + "</td>"
                                + "</tr>"
                }
                $("#TBSHOW").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("GRecordCount").innerHTML = recordCount;
                document.getElementById("GPageCount").innerHTML = pageCount;
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading()
            }
            if (TYPE == '2') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}
function ChooseGoods(id) {
    openLoading();
    openLoading();
    var data = {
        type: "AddYwyControl",
        ywyid: id
    };
    var proc = "Proc_CJCX_Admin";//存储过程名
    var type = "ReturnNumber";

    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                closeCover();
                QueryOperatorControl();
            }
            else {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}
