///创建选择商品UI
function CreatGoodsInfoUI() {
    var contHtml = "";
    contHtml += "<a class='closeBtn' onclick='closeCover();'><img src='../img/close.png'/></a>"
    contHtml += "<h3 class='winPopTit' id='maskTitle'>选择信息</h3>"
    contHtml += "<div class='winPopCon'>"
    contHtml += "<h2 class='winPoph2' id='maskTitle_left'></h2>"
    contHtml += "<div id='maskContent'>"
    contHtml += "<div class='conTable'>"
    contHtml += "<table class=\"dataintable\" id='TBWindows'>"
    contHtml += "<tr class='chcol'>"
    contHtml += "<tr><th>正在加载类容........</th></tr>"
    contHtml += "</table>"
    contHtml += "</div>"
    contHtml += "<div class='default' id='fanye'>"
    contHtml += "<p class='fl'>"
    contHtml += "<a onclick='Gbtnfirst(GetInfo)'>首页</a>"
    contHtml += "<a style='color:Blue' onclick='Gbtnup(GetInfo)'>上一页</a>"
    contHtml += "<a style='color:Blue' onclick='Gbtnnext(GetInfo)'>下一页</a>"
    contHtml += "<a onclick='Gbtnlast(GetInfo)'>尾页</a>"
    contHtml += "<span class='fr'>每页显示<b id='GPageSize'>15</b>条，共<b id='GRecordCount'>0</b>条,"
    contHtml += "当前页<b id='GPageIndex'>1</b>/<b id='GPageCount'>1</b></span>"
    contHtml += "</p>"
    contHtml += "</div>"
    contHtml += "</div>"
    contHtml += "</div>"
    document.getElementById("winPop").innerHTML = contHtml;
}

//加载弹窗信息
function ConditionStatus() {
    closeCover();
    openCover(); //生成弹窗
    CreatGoodsInfoUI();
    GetInfo();
}
///查询选择的信息
function GetInfo()
{
    var value = $('input[name="gk"]:checked').val();
    if (value == "Factories") {
        Get_FacTanChuang();
    }
    else if (value == "SinglGoods") {
        Get_GoodsInfo();
    }
    else {
        GetCustomers();
    }
}
//获取厂家弹窗
function Get_FacTanChuang() {
    var codevalue = $("#condition").val();
    var PageSize = $("#GPageSize").html();
    var PageIndex = $("#GPageIndex").html();
    var sqltype = "Get_fac";
    var Procedure = "Promotion_GK";
    var Type = "fac";
    var paramcont = "&PageSize=" + PageSize + "&PageIndex=" + PageIndex + "&sqltype=" + sqltype + "&Procedure=" + Procedure + "&codevalue=" + codevalue;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ControlGoods.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBWindows').empty();
                var tb = "<tr class='chcol'><th>序号</th><th>厂家名称</th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='select" + i + "' ondblclick='ChooseInfo(\"" + i + "\")' onMouseOver='changecolor(\"" + i + "\")' onMouseOut='deletetcolor(\"" + i + "\")'>"
                                + "<td>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                                + "<td id='xkeyid" + i + "'>" + JSON["data"][i]["factories_choosing"] + "</td></tr>"
                }
                $("#TBWindows").append(tb);
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
//获取排除的单品信息
function Get_GoodsInfo() {
    var codevalue = $("#condition").val();
    var PageSize = $("#GPageSize").html();
    var PageIndex = $("#GPageIndex").html();
    var sqltype = "pc_goods";
    var Procedure = "Promotion_GK";
    var Type = "goods";
    var paramcont = "&PageSize=" + PageSize + "&PageIndex=" + PageIndex + "&sqltype=" + sqltype + "&Procedure=" + Procedure + "&codevalue=" + codevalue;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ControlGoods.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBWindows').empty();
                var tb = "<tr class='chcol'><th align='center'>序号</th><th align='center'>商品编号</th><th align='center'>商品名称</th><th align='center'>商品规格</th><th align='center'>包装单位</th><th align='center'>生产厂家</th><th align='center'>库存</th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='select" + i + "' ondblclick='ChooseInfo(\"" + i + "\")'>"
                                + "<td align='center'>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                                + "<td id='xspbh" + i + "'   align='center'>" + JSON["data"][i]["call_index"] + "</td>"
                                + "<td id='xspmch" + i + "'   align='center'>" + JSON["data"][i]["sub_title"] + "</td>"
                                + "<td id='xshpgg" + i + "'   align='center'>" + JSON["data"][i]["drug_spec"] + "</td>"
                                + "<td id='xdw" + i + "'      align='center'>" + JSON["data"][i]["package_unit"] + "</td>"
                                + "<td id='xshpgg" + i + "'   align='center'>" + JSON["data"][i]["factories_choosing"] + "</td>"
                                + "<td id='xquantity" + i + "' align='center'>" + JSON["data"][i]["stock_quantity"] + "</td>"
                                + "<td id='xkeyid" + i + "' style='display:none'>" + JSON["data"][i]["keyid"] + "</td></tr>"
                }
                $("#TBWindows").append(tb);
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
//选择客户
function GetCustomers() {
    openLoading();
    var codevalue = $("#condition").val();
    var PageSize = $("#GPageSize").html();
    var PageIndex = $("#GPageIndex").html();
    var sqltype = "ControlCustomers";
    var Procedure = "Promotion_GK";
    var Type = "customer";
    var paramcont = "&PageSize=" + PageSize + "&PageIndex=" + PageIndex + "&sqltype=" + sqltype + "&Procedure=" + Procedure + "&codevalue=" + codevalue;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ControlGoods.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBWindows').empty();
                var tb = "<tr class='chcol'><th>序号</th><th>客户编号</th><th>客户名称</th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='select" + i + "' ondblclick='ChooseInfo(\"" + i + "\")'>"
                        + "<td align='center'>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                        + "<td id='xdanwbh" + i + "'   align='center'>" + JSON["data"][i]["danwbh"] + "</td>"
                        + "<td id='xdwmch" + i + "'   align='center'>" + JSON["data"][i]["nick_name"] + "</td>"
                        + "<td id='xkeyid" + i + "' style='display:none'>" + JSON["data"][i]["id"] + "</td></tr>"

                }
                $("#TBWindows").append(tb);
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
//选中信息
function ChooseInfo(i) {
    var value = $('input[name="gk"]:checked').val();
    var keyid = $("#xkeyid"+i+"").html();
    var lsid = $("#txtLsid").val();
    var data = {
        type: "AddCart",
        fatype: value,
        keyid: keyid,
        lsid:lsid
    };
    var proc = "Proc_PromotionRuleOut";//存储过程名
    var type = "ReturnNumber";
    $.ajax({
        type: 'Post',
        cache: false,
        url: "ashx/returnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                closeLoading();
                closeCover();
                CartQuery();
            }
            else {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    });
}
//查询选中的信息
function CartQuery()
{
    openLoading();

    var lsid = $("#txtLsid").val();
    var PageSize = 30;
    var PageIndex = $("#PageIndex").html();
    var data = {
        type: "CartQuery",
        lsid: lsid,
        PageIndex: PageIndex,
        PageSize: PageSize,
    };
    var proc = "Proc_PromotionRuleOut";//存储过程名
    var type = "ReturnList";

    $.ajax({
        type: 'Post',
        cache: false,
        url: "ashx/returnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBShow').empty();
                var tableHtml = "<tr><th>序号</th><th>类型</th><th>编号</th><th>名称</th><th>操作</th></tr>";
                for (var i = 0; i < JSON["data"].length; i++) {
                    tableHtml += "<tr style='text-align:center'>"
                    tableHtml += "<td align='center'>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>";
                    tableHtml += "<td>" + JSON["data"][i]['fatype'] + "</td>"
                    tableHtml += "<td>" + JSON["data"][i]['code'] + "</td>"
                    tableHtml += "<td>" + JSON["data"][i]['name'] + "</td>"
                    tableHtml += "<td><a onclick='DltCart(\"" + JSON["data"][i]['id'] + "\")'>删除</a></td>"
                    tableHtml += "</tr>"
                }
                $('#TBShow').append(tableHtml);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("RecordCount").innerHTML = recordCount;
                document.getElementById("PageCount").innerHTML = pageCount;
                closeLoading();
            }
           else if (TYPE == '1') {
                closeLoading()
                $("#TBShow").empty();
                var tableHtml = "<tr><th>序号</th><th>类型</th><th>编号</th><th>操作</th></tr>";
                tableHtml += "<tr><td colspan='11' style='text-align: center;'>暂无内容</td></tr>";
                $('#TBShow').append(tableHtml);
            }
          else{
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeLoading();
        }
    });
}

////删除Cart数据
function DltCart(keyid)
{
    openLoading();
    var lsid = $("#txtLsid").val();
    var data = {
        type: "DltCart",
        keyid: keyid,
        lsid: lsid
    };
    var proc = "Proc_PromotionRuleOut";//存储过程名
    var type = "ReturnNumber";
    $.ajax({
        type: 'Post',
        cache: false,
        url: "ashx/returnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                closeLoading();
                CartQuery();
            }
            else {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    });

}

//保存管控商品
function Save_GK() {
    openLoading();
    var faname = $("#fanametxt").val();
    var lsid = $("#txtLsid").val();
    if (faname == "") {
        alertFun("规则名称不能为空！", function () { closeLoading() }, 'f');
        return;
    }
    else {
        var data = {
            type: "Save",
            faname: faname,
            lsid: lsid
        };
        var proc = "Proc_PromotionRuleOut";//存储过程名
        var type = "ReturnNumber";
        $.ajax({
            type: 'Post',
            cache: false,
            url: "ashx/returnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
            dataType: "json",
            success: function (data) {
                var JSON = data;
                var TYPE = JSON["return_code"];
                var count = JSON["count"];
                if (TYPE == '0') {
                    alertFun("存盘成功！", function () { closeLoading(); window.location.reload(); }, 'w');
                }
                else {
                    alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                closeLoading()
            }
        });
    }
}



