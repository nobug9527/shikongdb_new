///获取优惠券列表
function GetCoupon() {
    openLoading();
    var khId = GetQueryString("dwId");
    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();

    var SqlType = "CouponQuery";
    var SqlProcedure = "Proc_HjSalesMan";
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        UserID: khId,
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
                var tb = "<tr class='odd_bg'><th>序号</th><th>编号</th><th>优惠券名</th><th>满足金额</th><th>优惠金额</th><th>开始日期</th><th>截止日期</th><th>客户类型</th><th>操作</th>";
                    tb += "</tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr  id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td align='center' id='code"+i+"'>" + JSON["data"][i]["Code"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["CouponName"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["MeetCount"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["DisCount"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["StartDate"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["EndDate"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["PriceRelation"] + "</td>";
                        tb = tb + "<td align='center'><a onclick='AllocationCoupon(\""+i+"\")'>分发优惠券</a></td>";
                        tb = tb + "<td><input id='dwid" + i + "' hidden=\"hidden\" value=\"\"/></td>";
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
    });
}
//为客户分配优惠券
function AllocationCoupon(i) {
    openLoading();
    var code = $("#code" + i + "").html();
    var khId = GetQueryString("dwId");
    var SqlType = "InsertCoupon";
    var SqlProcedure = "Proc_HjSalesMan";
    var Type = "ExecuteNonQuery";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        UserID: khId,
        Code:code
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
                layer.alert("操作成功", { icon: 1, skin: 'layui-layer-molv', closeBtn: 0 }, function () { GetCoupon(); })
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