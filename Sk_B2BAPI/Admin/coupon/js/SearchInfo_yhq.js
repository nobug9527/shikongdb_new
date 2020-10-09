///
function SearchInfo() {
    var proc = GetQueryString("proc");//存储过程名称
    var sqlType = GetQueryString("sqlType");//查询类型
    var choseType = GetQueryString("type");
    var jsonStr = "";
    var pageIndex = $("#SpageIndex").html();  //表示当前页
    var pageSize = "30";    //表示每页显示的条目数
    var searchValue = $("#txtSearchValue").val();
    if (choseType == "member" || choseType == "goods" || choseType == "shipper") {
        var groupId = GetQueryString("groupId");
        var entId = GetQueryString("entId");
        jsonStr = {
            type: sqlType,
            groupId: groupId,
            strWhere: searchValue,
            xentId: entId,
            pageIndex: pageIndex,
            pageSize: pageSize
        };
        GetData(proc, jsonStr, sqlType, choseType, "Y");
    }
    else if (choseType == "groupBind") {
        $("#btnSave").hide();
        var ywyid = GetQueryString("ywyId");
        var entId = GetQueryString("entId");
        jsonStr = {
            type: sqlType,
            ywyid: ywyid,
            status: 2,
            strWhere: searchValue,
            xentId: entId,
            pageIndex: pageIndex,
            pageSize: pageSize
        };
        GetData(proc, jsonStr, sqlType, choseType, "N");
    }
    else {
        $("#btnSave").hide();
        jsonStr = {
            type: sqlType,
            strWhere: searchValue,
            xentId: entId,
            pageIndex: pageIndex,
            pageSize: pageSize
        };
        GetData(proc, jsonStr, sqlType, choseType, "N");
    }
}
///加载数据
function GetData(proc, jsonStr, sqlType, choseType, isFxk) {
    var index = layer.load(2);
    var pageIndex = $("#SpageIndex").html();  //表示当前页
    var pageSize = "30";    //表示每页显示的条目数
    var type = "ReturnDataSet";
    $.ajax({
        url: "../ashx/MainReturnJson.ashx?json=" + encodeURIComponent(JSON.stringify(jsonStr)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        type: "Post",
        chche: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var status = obj.flag;
            if (status == 0) {
                $("#conTable_Top").empty();
                $("#conTable_Body").empty();
                //显示的页数和条目数
                var recordCount = data.recordCount;
                var pageCount = data.pageCount;

                ///加载表头
                var contHtml = "";
                contHtml += "<tr class=\"text-c\">";
                var fdname = data.fdname;
                var n = 0;
                if (isFxk == "Y") {
                    contHtml += '<th><input type="checkbox" name="memberMain" id="CheckBoxId" value="" onchange="CheckAllMember()">选择</th>'
                }
                contHtml += "<th>序号</th>";
                for (var i in fdname) {
                    if (i.indexOf("key") != -1) {
                        contHtml += "<th style='display:none'>" + fdname[i].trim() + "</th>";
                    }
                    else {
                        contHtml += "<th>" + fdname[i].trim() + "</th>";
                    }
                }
                $("#conTable_Top").append(contHtml);
                contHtml = "";
                //加载内容
                if (parseInt(recordCount) > 0) {
                    $.each(data.content, function (i, itm) {
                        if (isFxk == "Y") {
                            contHtml += "<tr class='text-c'>";
                            contHtml += '<td><input type="checkbox" name="member" class="checkInfo" value="' + itm["keyid"].trim() + '"></td>'
                        }
                        else {
                            contHtml += "<tr class='text-c'  ondblclick='ChoseInfo(\"" + i + "\",\"" + choseType + "\")'>";
                        }
                        contHtml += "<td>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>"
                        for (var m in itm) {
                            if (m.indexOf("key") != -1 || m == "hshj") {
                                contHtml += "<td style='display:none' id='x" + m + i + "'>" + itm[m].trim() + "</td>"
                            }
                            else {
                                contHtml += "<td id='x" + m + i + "'>" + itm[m].trim() + "</td>"
                            }
                        }
                        contHtml += "</tr>"
                    });
                    $("#conTable_Body").append(contHtml);

                    //显示的页数和条目数
                    $("#SrecordCount").html(recordCount);
                    $("#SpageCount").html(pageCount);
                }
                layer.close(index);
            }
            else if (status == 1) {
                $("#conTable_Body").empty();
                $("#SrecordCount").val(0);
                $("#SpageCount").val(1);
                layer.close(index);
            }
            else {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}
///选择信息
function ChoseInfo(i, type) {
    ///单品促销选择商品
    if (type == "groupBind") {
        var groupId = $("#xGroupId" + i + "").html();
        GroupBind(groupId);
    }
    else if (type == "GetEntdoc") {
        var entId = $("#xkeyid" + i + "").html();
        var entname = $("#xentname" + i + "").html();
        parent.$("#txtEntId").val(entId);
        parent.$("#txtEntName").val(entname);
        layer_close();
    }
}
//全选和反选
function CheckAllMember() {
    if (document.getElementById("CheckBoxId").checked) {
        $(".checkInfo").prop("checked", true);
    }
    else {
        $(".checkInfo").prop("checked", false);
    }
}
///选择客户
function SaveMember() {
    var index = layer.load(2);
    var groupId = GetQueryString("groupId");
    var list = "";
    $.each($("#conTable_Body input:checked"), function (i, itm) {
        list += itm.value + ",";
    })
    if (list == "") {
        layer.close(index);
        layer.alert("请选择会员", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var data = {
        type: "SaveMemberList",
        groupId: groupId,
        list: list,
    };
    var proc = "proc_dkxd_salesman";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                });
                parent.document.getElementById("btnClick").click();
                SearchInfo();
            }
            else {
                layer.closeAll();
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.closeAll();
        }
    })
}
///选择商品
function SaveGoods() {
    var index = layer.load(2);
    var groupId = GetQueryString("groupId");
    var list = "";
    $.each($("#conTable_Body input:checked"), function (i, itm) {
        list += itm.value + ",";
    })
    if (list == "") {
        layer.close(index);
        layer.alert("请选择商品", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var data = {
        type: "SaveGoodsList",
        groupId: groupId,
        list: list,
    };
    var proc = "proc_dkxd_GoodsInfo";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                });
                parent.document.getElementById("btnClick").click();
                SearchInfo();
            }
            else {
                layer.closeAll();
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.closeAll();
        }
    })

}
function SaveShipper() {
    var index = layer.load(2);
    var groupId = GetQueryString("groupId");
    var list = "";
    $.each($("#conTable_Body input:checked"), function (i, itm) {
        list += itm.value + ",";
    })
    if (list == "") {
        layer.close(index);
        layer.alert("请选择会员", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var data = {
        type: "SaveShipperList",
        groupId: groupId,
        list: list,
    };
    var proc = "proc_dkxd_ShipperInfo";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                });
                parent.document.getElementById("btnClick").click();
                layer_close();
            }
            else {
                layer.closeAll();
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.closeAll();
        }
    })
}
///订单填制选择客户
function GroupBind(groupId) {
    var index = layer.load(2);
    var ywyid = GetQueryString("ywyId");
    var data = {
        type: "SaveSalesManGroup",
        groupId: groupId,
        ywyid: ywyid,
    };
    var proc = "proc_dkxd_salesman";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                });
                parent.document.getElementById("btnClick").click();
                layer_close();
            }
            else {
                layer.closeAll();
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.closeAll();
        }
    })
}

function Save() {
    var choseType = GetQueryString("type");
    if (choseType == "member") {
        SaveMember();
    }
    else if (choseType == "goods") {
        SaveGoods();
    }
    else if (choseType == "shipper") {
        SaveShipper();
    }
}