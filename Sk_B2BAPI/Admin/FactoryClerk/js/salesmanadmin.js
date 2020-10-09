//获取业务员信息
function CommonYwy() {
    openLoading();
    var salValue = $("#txtSqlvalue").val();
    var biaoShi = GetQueryString("bz");
    var ywyId = GetQueryString("ywyId");

    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();
    if (biaoShi == "kh") {
        $(".btnData").css("display", "block");
        var SqlType = "cxkh";
    }
    else {
        $(".btnData").css("display", "none");
        var SqlType = "khmx";
    }
    var SqlProcedure = "Proc_HjSalesMan";
    var Type = "DataTable";

    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        sqlvalue: salValue,
        YwyId:ywyId,
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
                if (biaoShi == "kh") {
                    var tb = "<tr class='odd_bg'><th>序号</th><th>选择</th><th>客户编号</th><th>客户名称</th>";
                    tb += "</tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr  id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td align='center'><input type=\"checkbox\" class=\"xz\" name=\"xz\" value=\"" + JSON["data"][i]["id"] + "\"/></td>"
                        tb = tb + "<td align='center'>" + JSON["data"][i]["danwbh"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["nick_name"] + "</td>";
                        tb = tb + "<td><input id='dwid" + i + "' hidden=\"hidden\" value=\"" + JSON["data"][i]["id"] + "\"/></td>";
                        tb = tb + "</tr>";
                    }
                }
                else {
                    var tb = "<tr class='odd_bg'><th>序号</th><th>客户编号</th><th>客户名称</th><th>操作</th>";
                    tb += "</tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr  id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["danwbh"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["nick_name"] + "</td>";
                        tb = tb + "<td align='center'><a class='cx btn_c' onclick='DeleteKh(\"" + i + "\")'>删除</a></td>"
                        tb = tb + "<td><input id='dwid" + i + "' hidden=\"hidden\" value=\"" + JSON["data"][i]["id"] + "\"/></td>";
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
//提交选择的客户信息
function SubmitKh() {
    openLoading();
    var ywyId = GetQueryString("ywyId");

    var list = "";
    $.each($("#TBShow input:checked"), function (i,itm) {
        list += itm.value;
        list += ",";
    });
    list = list.substring(0, list.length - 1);
    if (list.length == '0') {
        layer.alert("未选中信息", { icon: 2, skin: 'layui-layer-molv', closeBtn: 0 })
    }
    else { 
    var SqlType = "bdkh";
    var SqlProcedure = "Proc_HjSalesMan";
    var Type = "ExecuteNonQuery";
    var paramcont = "&ywyId=" + ywyId + "&list=" + encodeURI(list) + "&SqlType=" + SqlType + "&SqlProcedure=" + SqlProcedure;
   
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/InfoAdmin.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
               
                layer.alert("操作成功", { icon: 1, skin: 'layui-layer-molv', closeBtn: 0 }, function () { CommonYwy(); })
                closeLoading();
            }
            if (TYPE == '1') {
                layer.alert(JSON["data"][0]["message"], { icon: 2, skin: 'layui-layer-molv', closeBtn: 0 })
                closeLoading();
              
            }
            if (TYPE == '2') {
                layer.alert(JSON["data"][0]["message"], { icon: 2, skin: 'layui-layer-molv', closeBtn: 0 })
                //alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    });
    }
}
//删除不要的客户信息
function DeleteKh(i) {
    openLoading();
    var ywyId = GetQueryString("ywyId");
    var khId = $("#dwid" + i + "").val();
    var SqlType = "sckh";
    var SqlProcedure = "Proc_HjSalesMan";
    var Type = "ExecuteNonQuery";

    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        KhId: khId,
        YwyId: ywyId
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
                layer.alert("操作成功", { icon: 1, skin: 'layui-layer-molv', closeBtn: 0 }, function () { CommonYwy(); })
                closeLoading();
            }
            if (TYPE == '1') {
                layer.alert(JSON["data"][0]["message"], { icon: 2, skin: 'layui-layer-molv', closeBtn: 0 })
                closeLoading();

            }
            if (TYPE == '2') {
                layer.alert(JSON["data"][0]["message"], { icon: 2, skin: 'layui-layer-molv', closeBtn: 0 })
                //alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    });
}