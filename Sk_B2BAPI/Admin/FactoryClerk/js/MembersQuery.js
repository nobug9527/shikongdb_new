///会员查询
function MembersInfo() {
    openLoading();
    var hyJb = $("#slHyJb").val();   //获取会员级别
    var SqlType = "MembersQuery";
    var SqlProcedure = "Proc_HjSalesMan";
    var sqlvalue = $("#sqlvalue").val();//关键字
    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        SqlValue: sqlvalue,
        HyJb:hyJb,
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
                    tb = tb + "<td align='center'>" + JSON["data"][i]["danwbh"] + "</td>";
                    tb = tb + "<td align='center' id='dwmch"+i+"'>" + JSON["data"][i]["nick_name"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["kehufl"] + "</td>";
                    if (JSON["data"][i]["u_level"] != '0') {
                        tb = tb + "<td align='center' style=\"color:#ff0330!important;font-size:16px;\">" + JSON["data"][i]["u_level"] + "</td>";
                    }
                    else {
                        tb = tb + "<td align='center' >" + JSON["data"][i]["u_level"] + "</td>";
                    }
                    tb = tb + "<td align='center' id='point"+i+"'>" + JSON["data"][i]["point"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["jgjb"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["salesman"] + "</td>";
                    tb = tb + "<td align='center'><a class='cx btn_b' onclick='FenpCoupon(\"" + i + "\")'>操作</a></td>";
                    tb = tb + "<td align='center'><a class='cx btn_c' onclick='UpdateIntegral(\"" + i + "\")'>操作</a></td>";
                    tb = tb + "<td style='display:none' id='dwid" + i + "' align='center'>" + JSON["data"][i]["id"] + "</td>";
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
                document.getElementById("RecordCount").innerHTML = 1;
                document.getElementById("PageCount").innerHTML = 1;
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
//分配优惠券
function FenpCoupon(i) {
   
    var dwId = $("#dwid" + i + "").html();
    window.location.href = "CouponList.aspx?dwId="+dwId;
    encodeURI
}
//修改积分
function UpdateIntegral(i) {
    var dwId = $("#dwid" + i + "").html();
    var dwMch = $("#dwmch" + i + "").html();
    var point = $("#point"+i+"").html();
    window.location.href = "UpdateIntegral.aspx?dwId=" + dwId + "&dwMch=" + encodeURI(dwMch) + "&point=" + point;
}
//导出excel
