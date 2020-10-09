///客户礼品统计
function PresentStatistical()
{
    openLoading();
    var ksRq = $("#startDate").val();
    var jsRq = $("#endDate").val();
    var keyWord = $("#txtKeyword").val();
    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();
    var SqlType = "QueryGift";
    var fatype = $("#FaTypeId").val();

    var data = {
        Type: SqlType,
        fatype: fatype,
        SqlValue: keyWord,
        KsRq: ksRq,
        JsRq: jsRq,
        PageSize: pageSize,
        PageIndex: pageIndex
    };
    var proc = "Proc_PresentStatistical";//存储过程名
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
                if (fatype == "HG") {
                    tb = "<tr><th>行号</th><th>单据编号</th><th>日期</th><th>客户编号</th><th>客户名称</th><th>商品名称</th><th>商品规格</th><th>生产厂家</th><th>数量</th><th>价格</th><th>备注</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr  id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["order_no"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["add_time"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["danwbh"] + "</td>";
                        tb = tb + "<td align='center' >" + JSON["data"][i]["nick_name"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["sub_title"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["drug_spec"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["factories_choosing"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["quantity"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["real_price"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["beizhu"] + "</td>";
                        tb = tb + "</tr>";
                    }
                }
                else if (fatype == "MZ")
                {
                    tb = "<tr><th>行号</th><th>单据编号</th><th>日期</th><th>客户编号</th><th>客户名称</th><th>赠品信息</th><th>备注</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr  id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["order_no"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["add_time"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["danwbh"] + "</td>";
                        tb = tb + "<td align='center' >" + JSON["data"][i]["nick_name"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["decaribe"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["beizhu"] + "</td>";
                        tb = tb + "</tr>";
                    }
                }
                else if (fatype == "ZH")
                {
                    tb = "<tr><th>行号</th><th>单据编号</th><th>日期</th><th>客户编号</th><th>客户名称</th><th>商品名称</th><th>商品规格</th><th>生产厂家</th><th>数量</th><th>价格</th><th>优惠金额</th><th>组合数量</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr  id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["order_no"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["add_time"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["danwbh"] + "</td>";
                        tb = tb + "<td align='center' >" + JSON["data"][i]["nick_name"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["sub_title"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["drug_spec"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["factories_choosing"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["quantity"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["real_price"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["payment_fee"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["zhshl"] + "</td>";
                        tb = tb + "</tr>";
                    }
                }
                else if (fatype == "CJ")
                {
                    tb = "<tr><th>行号</th><th>单据编号</th><th>日期</th><th>客户编号</th><th>客户名称</th><th>奖品名称</th><th>数量</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr  id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["order_no"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["rq"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["danwbh"] + "</td>";
                        tb = tb + "<td align='center' >" + JSON["data"][i]["nick_name"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["zengp"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["shl"] + "</td>";
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
                var tba = "";
                if (fatype == "HG")
                {
                    tba = "<tr><th>行号</th><th>单据编号</th><th>日期</th><th>客户编号</th><th>客户名称</th><th>商品名称</th><th>商品规格</th><th>生产厂家</th><th>数量</th><th>价格</th><th>备注</th></tr>";
                }
                else if (fatype == "MZ")
                {
                    tba = "<tr><th>行号</th><th>单据编号</th><th>日期</th><th>客户编号</th><th>客户名称</th><th>赠品信息</th><th>备注</th></tr>";
                }
                else if (fatype == "ZH")
                {
                    tba = "<tr><th>行号</th><th>单据编号</th><th>日期</th><th>客户编号</th><th>客户名称</th><th>商品名称</th><th>商品规格</th><th>生产厂家</th><th>数量</th><th>价格</th><th>优惠金额</th><th>组合数量</th></tr>";
                }
                else if (fatype == "CJ")
                {
                    tba = "<tr><th>行号</th><th>单据编号</th><th>日期</th><th>客户编号</th><th>客户名称</th><th>奖品名称</th><th>数量</th></tr>";
                }
                tba += "<tr><td colspan=5></td><td colspan=8>暂无数据</td></tr>"
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
///导出数据
function DCExcel()
{
    var ksRq = $("#startDate").val();
    var jsRq = $("#endDate").val();
    var keyWord = $("#txtKeyword").val();
    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();
    var SqlType = "QueryGift";
    var fatype = $("#FaTypeId").val();

    var data = {
        Type: SqlType,
        fatype: fatype,
        SqlValue: keyWord,
        KsRq: ksRq,
        JsRq: jsRq,
        PageSize: pageSize,
        PageIndex: pageIndex
    };
    var proc = "Proc_PresentStatistical";//存储过程名
    var type = "ReturnList";
    window.location = "DcExcel.aspx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc);
}