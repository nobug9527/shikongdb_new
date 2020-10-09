///厂家业务员查询
function GetSalesmanInfo() {
    openLoading();
    var SqlType = "SalesmanSearch";
    var SqlProcedure = "Proc_CJCX_Admin";
    var sqlvalue = $("#sqlvalue").val();//关键字
    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();
    var status = $("#SatausID").val();//状态
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        Status: status,
        SqlValue: sqlvalue,
        PageSize: pageSize,
        PageIndex: pageIndex
    };
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnDataTable.ashx?Type=" + Type + "&Json=" + encodeURI(JSON.stringify(Json)),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBShow').empty();
                var tb = "<tr class='odd_bg'>";
                tb += "</tr>";
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr  id='select" + i + "' >"
                    tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["flcode"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["flname"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["sjflcode"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["jibie"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["kh"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["sp"] + "</td>";
                    if (JSON["data"][i]["status"] == "0") {
                        tb = tb + "<td align='center'>启用</td>";
                    }
                    else if (JSON["data"][i]["status"] == "2") {
                        tb = tb + "<td align='center'>禁用</td>";
                    }
                    tb = tb + "<td align='center'><a onclick='OpenSalesmanDeatil(\"" + i + "\",\"kh\")'>操作</a></td>";
                    tb = tb + "<td align='center'><a onclick='OpenSalesmanDeatil(\"" + i + "\",\"sp\")'>操作</a></td>";
                    tb = tb + "<td style='display:none' id='userid" + i + "' align='center'>" + JSON["data"][i]["flcode"] + "</td>";
                    tb = tb + "</tr>";
                }
                $("#TBShow").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("RecordCount").innerHTML = recordCount;
                document.getElementById("PageCount").innerHTML = pageCount;
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading();
                $('#TBShow').empty();
                var tba = "<tr><td colspan=5></td><td colspan=8>暂无数据</td></tr>"
                $("#TBShow").append(tba);
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

function OpenSalesmanDeatil(i,type)
{
    var userid = $("#userid"+i+"").html();
    window.location = "SalesmanDeatil.aspx?userid="+encodeURI(userid)+"&type="+encodeURI(type);
}

///查询业务员绑定信息

function GetBDInfo()
{
    openLoading();
    var SqlType = "";
    var userid = GetQueryString("userid");
    var type = GetQueryString("type");
    if (type == "kh") {
        SqlType = "BD_KH"
    }
    else {
        SqlType = "BD_SP";
    }
    var SqlProcedure = "Proc_CJCX_Admin";
    var sqlvalue = $("#sqlvalue").val();//关键字
    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        Ywyid: userid,
        SqlValue: sqlvalue,
        PageSize: pageSize,
        PageIndex: pageIndex
    };
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnDataTable.ashx?Type=" + Type + "&Json=" + encodeURI(JSON.stringify(Json)),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBShow').empty();
                var tb = "";
                if (type == "kh") {
                    tb = "<tr class='odd_bg'><th>序号</th><th>客户ID</th><th>客户编号</th><th>客户名称</th><th>客户类型</th><th>修改日期</th><th>操作</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr  id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["user_id"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["danwbh"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["nick_name"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["kehufl"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["lastmodifytime"] + "</td>";
                        tb = tb + "<td align='center'><a onclick='UpdateBD(\"" + userid + "\",\"" + JSON["data"][i]["user_id"] + "\",\""+type+"\",\"1\")'>删除</a></td>";
                        tb = tb + "<td style='display:none' id='ywyid" + i + "' align='center'>" + JSON["data"][i]["login_id"] + "</td>";
                        tb = tb + "<td style='display:none' id='khid" + i + "' align='center'>" + JSON["data"][i]["user_id"] + "</td>";
                        tb = tb + "</tr>";
                    }
                }
                else {
                    tb = "<tr class='odd_bg'><th>序号</th><th>商品编号</th><th>商品名称</th><th>商品规格</th><th>包装单位</th><th>生产厂家</th><th>操作</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr  id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["call_index"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["sub_title"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["drug_spec"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["package_unit"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["factories_choosing"] + "</td>";
                        tb = tb + "<td align='center'><a onclick='UpdateBD(\"" + userid + "\",\"" + JSON["data"][i]["article_id"] + "\",\"" + type + "\",\"1\")'>删除</a></td>";
                        tb = tb + "<td style='display:none' id='ywyid" + i + "' align='center'>" + JSON["data"][i]["login_id"] + "</td>";
                        tb = tb + "<td style='display:none' id='articleid" + i + "' align='center'>" + JSON["data"][i]["article_id"] + "</td>";
                        tb = tb + "</tr>";
                    }
                }

                $("#TBShow").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("RecordCount").innerHTML = recordCount;
                document.getElementById("PageCount").innerHTML = pageCount;
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading();
                $('#TBShow').empty();
                var tba = "<tr><td colspan=5></td><td colspan=8>暂无数据</td></tr>"
                $("#TBShow").append(tba);
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

///创建选择信息UI
function CreatInfoUI() {
    var contHtml = "";
    contHtml += "<a class='closeBtn' onclick='closeCover();'><img src='../img/close.png'/></a>"
    contHtml += "<h3 class='winPopTit' id='maskTitle'>选择信息</h3>"
    contHtml += "<div class='winPopCon'>"
    contHtml += "<h2 class='winPoph2' id='maskTitle_left'></h2>"
    contHtml += "<div id='maskContent'>"
    contHtml += "<div class='conForm' id='cxtjdiv'>"
    contHtml += "<p ><label>关键字：</label><input type='text' id='sqlvalueid'  placeholder='关键词搜索'>"
    contHtml += "<a class='btn btn-success' onclick='GetChoseInfo()' style=\"margin-left:10px;\"><i class='searchIcon btnIcon'></i>查 询</a>"   
    contHtml += "</p>"
    contHtml += "</div>"
    contHtml += "<div class='conTable'>"
    contHtml += "<table class=\"dataintable\" id='XTBSHOW'>"
    contHtml += "<tr class='chcol'>"
    contHtml += "<tr><th>正在加载类容........</th></tr>"
    contHtml += "</table>"
    contHtml += "</div>"
    contHtml += "<div class='conPage clearfix' id='fanye'>"
    contHtml += "<p class='fl'>"
    contHtml += "<a onclick='spphbtnfirst(GetChoseInfo)'>首页</a>"
    contHtml += "<a style='color:Blue' onclick='Xbtnup(GetChoseInfo)'>上一页</a>"
    contHtml += "<a style='color:Blue' onclick='Xbtnnext(GetChoseInfo)'>下一页</a>"
    contHtml += "<a onclick='spphbtnlast(GetChoseInfo)'>尾页</a>"
    contHtml += "<span class='fr'>每页显示<b id='XPageSize'>15</b>条，共<b id='XRecordCount'>0</b>条,"
    contHtml += "当前页<b id='XPageIndex'>1</b>/<b id='XPageCount'>1</b></span>"
    contHtml += "</div>"
    contHtml += "</div>"
    document.getElementById("winPop").innerHTML = contHtml;
}
//获取选择信息
function GetInfo() {
    closeCover();
    openCover(); //生成弹窗
    CreatInfoUI();
    GetChoseInfo();
}
//获取选择信息
function GetChoseInfo()
{
    openLoading();
    var SqlType = "SalesmanSearch";
    var SqlProcedure = "Proc_CJCX_Admin";
    var sqlvalue = $("#sqlvalueid").val();//关键字
    var userid = GetQueryString("userid");
    var type = GetQueryString("type");
    var pageSize = $("#XPageSize").html();
    var pageIndex = $("#XPageIndex").html();
    if (type == "kh") {
        SqlType = "GetKHInfo"
    }
    else {
        SqlType = "GetSPInfo";
    }
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        Status: status,
        SqlValue: sqlvalue,
        ywyid: userid,
        PageSize: pageSize,
        PageIndex: pageIndex
    };
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnDataTable.ashx?Type=" + Type + "&Json=" + encodeURI(JSON.stringify(Json)),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#XTBSHOW').empty();
                var tb = "";
                if (type == "kh") {
                    tb = "<tr class='odd_bg'><th align='center'>序号</th><th align='center'>用户ID</th><th align='center'>客户编号</th><th align='center'>客户名称</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr style=\"background:none\" class='odd_bg' id='xselect" + i + "' ondblclick='UpdateBD(\"" + userid + "\",\"" + JSON["data"][i]["id"] + "\",\"" + type + "\",\"0\")' onMouseOver='xchangecolor(\"" + i + "\")' onMouseOut='xdeletetcolor(\"" + i + "\")' >"
                        tb = tb + "<td style=\"background:none\" align='center'>" + (parseInt(i) + 1) + "</td>";
                        tb = tb + "<td style=\"background:none\" align='center'>" + JSON["data"][i]["id"] + "</td>";
                        tb = tb + "<td style=\"background:none\" align='center'>" + JSON["data"][i]["danwbh"] + "</td>";
                        tb = tb + "<td style=\"background:none\" align='center'>" + JSON["data"][i]["nick_name"] + "</td>";
                        tb = tb + "<td  style='display:none' id='xkhid" + i + "' align='center'>" + JSON["data"][i]["id"] + "</td>";
                        tb = tb + "</tr>";
                    }
                }
                else {
                    tb = "<tr class='odd_bg'><th>序号</th><th>商品编号</th><th>商品名称</th><th>商品规格</th><th>包装单位</th><th>生产厂家</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr  id='xselect" + i + "' ondblclick='UpdateBD(\"" + userid + "\",\"" + JSON["data"][i]["id"] + "\",\"" + type + "\",\"0\")' onMouseOver='xchangecolor(\"" + i + "\")' onMouseOut='xdeletetcolor(\"" + i + "\")' >"
                        tb = tb + "<td align='center'>" + (parseInt(i) + 1) + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["call_index"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["sub_title"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["drug_spec"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["package_unit"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["factories_choosing"] + "</td>";
                        tb = tb + "<td style='display:none' id='xarticleid" + i + "' align='center'>" + JSON["data"][i]["id"] + "</td>";
                        tb = tb + "</tr>";
                    }
                }
                $("#XTBSHOW").append(tb);

                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("XRecordCount").innerHTML = recordCount;
                document.getElementById("XPageCount").innerHTML = pageCount;

                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading();
                $('#XTBSHOW').empty();
                var tba = "<tr><td colspan=2></td><td colspan=3>暂无数据</td></tr>"
                $("#XTBSHOW").append(tba);
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

////修改绑定关系
function UpdateBD(userid,sqlvalue,sqltype,type)
{
    openLoading();
    var SqlType = "";
    if (sqltype == "kh")
    {
        SqlType = "Update_HHBD";
    }
    else if (sqltype == "sp")
    {
        SqlType = "Update_SPBD";
    }
    var SqlProcedure = "Proc_CJCX_Admin";
    var status = type;
    var Type = "ExecuteNonQuery";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        Status: status,
        SqlValue: sqlvalue,
        ywyid: userid
    };
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnDataTable.ashx?Type=" + Type + "&Json=" + encodeURI(JSON.stringify(Json)),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                closeLoading();
                //closeCover();
                GetBDInfo();
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
