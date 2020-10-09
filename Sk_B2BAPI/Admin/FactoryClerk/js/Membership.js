///客户等级加入购物车
function AddCartLevel()
{
    openLoading();
    var LevelId = $("#LevelIdX").html();
    var AmountId = $("#AmountIdX").val();
    var DiscountId = $("#DiscountIdX").val();
    if (AmountId == "")
    {
        alertFun("请填写当前等级金额！", function () { closeLoading() }, 'f');
    }
    else if (DiscountId == "")
    {
        alertFun("请填写当前等级扣率！", function () { closeLoading() }, 'f');
    }
    else if (isNaN(parseFloat(AmountId)))
    {
        alertFun("填写的金额格式错误！", function () { closeLoading() }, 'f');
    }
    else if (isNaN(parseFloat(DiscountId)) || DiscountId < 0 || DiscountId > 100) {
        alertFun("填写的金额格式错误！", function () { closeLoading() }, 'f');
    }
    else {
        var SqlProcedure = "Proc_UserLevel";
        var Type = "ExecuteNonQuery";
        var Json = {
            Type: "AddCart",
            Procedure: SqlProcedure,
            Level: LevelId,
            Amount: AmountId,
            Discount: DiscountId
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
                    QueryCart();
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
}
//查询等级购物车
function QueryCart() {
    openLoading();
    var SqlType = "QueryCart";
    var SqlProcedure = "Proc_UserLevel";
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
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
                    tb += "<tr>";
                    tb += "<td style=\"color:red\" id=\"LevelId" + i + "\" >" + JSON["data"][i]["Level"] + "</td>"
                    tb += "<td><input type=\"text\" id=\"AmountId" + i + "\"   class=\"input normal\" value='" + JSON["data"][i]["Amount"] + "'/></td>";
                    tb += "<td><input type=\"text\" id=\"DiscountId" + i + "\" class=\"input normal\" value='" + JSON["data"][i]["Discount"] + "'/></td>";
                    tb += "<td><input type=\"button\" value=\"删除\"  class=\"btn yellow\" onclick=\"DLTCart('"+i+"');\" /></td>";
                    tb += "</tr>";
                }
                tb += "<tr>";
                tb += "<td style=\"color:red\" id=\"LevelIdX\" >" + (JSON["data"].length + 1) + "</td>"
                tb += "<td><input type=\"text\" id=\"AmountIdX\"   class=\"input normal\"/></td>";
                tb += "<td><input type=\"text\" id=\"DiscountIdX\" class=\"input normal\"/></td>";
                tb += "<td><input type=\"button\" value=\"保存\"  class=\"btn\" onclick=\"AddCartLevel();\" /></td>";
                tb += "</tr>";
                $("#TBShow").append(tb);
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading();
                $('#TBShow').empty();
                var tb = "";
                tb += "<tr>";
                tb += "<td style=\"color:red\" id=\"LevelIdX\" >1</td>"
                tb += "<td><input type=\"text\" id=\"AmountIdX\"   class=\"input normal\"/></td>";
                tb += "<td><input type=\"text\" id=\"DiscountIdX\" class=\"input normal\"/></td>";
                tb += "<td><input type=\"button\" value=\"保存\"  class=\"btn\" onclick=\"AddCartLevel();\" /></td>";
                tb += "</tr>";
                $("#TBShow").append(tb);
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
///删除中间表数据
function DLTCart(i) {
    openLoading();
    var LevelId = $("#LevelId" + i + "").html();

    var SqlProcedure = "Proc_UserLevel";
    var Type = "ExecuteNonQuery";
    var Json = {
        Type: "DLTCart",
        Procedure: SqlProcedure,
        Level: LevelId,
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
                QueryCart();
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
///生成会员等级规则
function CreateLevel()
{
    openLoading();
    var TypeId = $("#TypeId").val();
    var ksrq = $("#startDate").val();
    var jsrq = $("#endDate").val();
    var name = $("#nametxt").val();
    if (TypeId == "")
    {
        alertFun("请选择客户类型！", function () { closeLoading() }, 'f');
    }
    else if (ksrq == "" || jsrq == "") {
        alertFun("请选择开始日期或结束日期！", function () { closeLoading() }, 'f');
    }
    else {
        var SqlProcedure = "Proc_UserLevel";
        var Type = "ExecuteNonQuery";
        var Json = {
            Type: "CreateLevel",
            Procedure: SqlProcedure,
            UserType: TypeId,
            name:name,
            ksrq: ksrq,
            jsrq: jsrq
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

}

///规则汇总查询
function QueryGZHZ()
{
    openLoading();
    var sqlvalue = $("#sqlvalue").val();//关键字
    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();
    var status = $("#SatausID").val();//状态
    var userType = $("#TypeId").val();//状态
    var SqlType = "QueryLVHZ";
    var SqlProcedure = "Proc_UserLevel";
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        Status: status,
        name: sqlvalue,
        userType:userType,
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
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='odd_bg' id='select" + i + "' >"
                    tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["code"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["name"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["UserType"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["ksrq"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["jsrq"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["beactive"] + "</td>";
                    tb = tb + "<td align='center'>"
                    if (JSON["data"][i]["beactive"] == "Y") {
                        tb = tb + "<a onclick='UpdateGZZT(\"" + JSON["data"][i]["code"] + "\",\"N\")'>禁用|</a>";
                    }
                    else if (JSON["data"][i]["beactive"] == "N")
                    {
                        tb = tb + "<a onclick='UpdateGZZT(\"" + JSON["data"][i]["code"] + "\",\"Y\")'>启用|</a>";
                    }
                    tb = tb + "<a color='red' onclick='UpdateGZZT(\"" + JSON["data"][i]["code"] + "\",\"Q\")'>删除</a>|"
                    tb = tb + "<a onclick='OpenMX(\"" + JSON["data"][i]["code"] + "\")'>详情</a>"
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}

function OpenMX(code)
{
    window.location = "MembershipDetails.aspx?&code=" + code;
}
//修改规则状态
function UpdateGZZT(code,status)
{
    var msg = "确定？";
    if (status == "N")
    {
        msg = "是否禁用";
    }
    else if (status == "Y")
    {
        msg = "是否启用";
    }
    else if (status == "Q")
    {
        msg = "是否删除";
    }
    if (confirm(msg))
    {
        openLoading();
        var SqlProcedure = "Proc_UserLevel";
        var Type = "ExecuteNonQuery";
        var Json = {
            Type: "UpdateZT",
            Procedure: SqlProcedure,
            status:status,
            code: code,
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
                    QueryGZHZ();
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
}

//查询明细
function QueryGZMX()
{
    openLoading();
    var sqlvalue = $("#sqlvalue").val();//关键字
    var pageSize = $("#PageSize").html();
    var pageIndex = $("#PageIndex").html();
    var code = GetQueryString("code");
    var SqlType = "QueryLVMX";
    var SqlProcedure = "Proc_UserLevel";
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        code: code,
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
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='odd_bg' id='select" + i + "' >"
                    tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["code"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["name"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["UserType"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["level"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["amount"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["discount"] + "</td>";
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}