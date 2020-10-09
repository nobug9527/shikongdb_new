//保存推送类容
function CreatePush()
{
    openLoading();
    var TypeId = $("#TypeId").val();
    var Date = $("#txtStartDate").val();
    var Time = $("#txtKsTime").val();
    var name = $("#nametxt").val();
    var Url = $("#Urltxt").val();
    var khlx = $("#KhTypeId").val();
    var notetxt = $("#notetxt").val();
    var SqlProcedure = "Proc_AppPuse";
    var Type = "ExecuteNonQuery";
    var Json = {
        Type: "CreateAppPuse",
        Procedure: SqlProcedure,
        TypeId: TypeId,
        khlx:khlx,
        Date: Date,
        Time: Time,
        name: name,
        Url: Url,
        notes:notetxt
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
                alertFun("存盘成功！", function () { closeLoading(); window.location.reload(); });
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
function AppPushQuery()
{
    openLoading();
    var sqlvalue = $('#sqlvalue').val();
    var proc = "Proc_AppPuse";//存储过程名
    var Type = "DataTable";
    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();
    var Json = {
        type: "QueryPush",
        Procedure: proc,
        name: sqlvalue,
        PageIndex: pageIndex,
        pageSize: pageSize,
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
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='select" + i + "' >"
                    tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["type"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["khlx"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["post_time"] + " " + JSON["data"][i]["post_sj"] +"</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["title"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["web_url"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["is_post"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["beactive"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["content"] + "</td>";
                    tb = tb + "<td align='center'>";
                    if (JSON["data"][i]["beactive"] == "N")
                    {
                        tb = tb + "<a onclick='DltAppPush(\"" + JSON["data"][i]["id"] + "\",\"Y\")'>启用</a>|";
                    }
                    tb = tb + "<a onclick='DltAppPush(\"" + JSON["data"][i]["id"] + "\",\"X\")'>删除</a>";
                    tb = tb + "</td></tr>";
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
        }
    });
}
function DltAppPush(id,beactive)
{
    openLoading();
    var SqlProcedure = "Proc_AppPuse";
    var Type = "ExecuteNonQuery";
    var Json = {
        Type: "Update",
        Procedure: SqlProcedure,
        TypeId: beactive,
        ID: id
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
                alertFun("操作成功！", function () { closeLoading(); AppPushQuery(); });
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