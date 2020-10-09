///厂家业务员查询
function SalesmanInfo() {
    openLoading();
    var SqlType = "QuerySalesMan";
    var SqlProcedure = "Proc_HjSalesMan";
    var sqlvalue = $("#sqlvalue").val();//关键字
    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
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
                    tb = tb + "<td align='center'>" + JSON["data"][i]["user_name"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["nick_name"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["flname"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["jibie"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["KCount"] + "</td>";
                    tb = tb + "<td align='center'><a class='cx btn_b' onclick='LocationPage(\"" + i + "\",\"kh\")'>绑定客户</a></td>";
                    tb = tb + "<td align='center'><a  class='cx btn_c' onclick='LocationPage(\"" + i + "\",\"khmx\")'>查看客户</a></td>";
                    tb = tb + "<td><input id='ywyid" + i + "' hidden=\"hidden\" value=\"" + JSON["data"][i]["id"] + "\"/></td>";
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
///绑定客户
function LocationPage(i,obj) {
    var ywyId = $("#ywyid"+i+"").val();   //业务员id
    window.location.href = "SalesmanAdmin.aspx?ywyId=" + ywyId+"&bz="+obj;
}
