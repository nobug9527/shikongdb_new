///页面加载数据
function GetChoseInfo() {
    var type = GetQueryString("type");
    if (type == "GetBrandGoods") {
        GetBrandGoods();
    } else if (type == 'SetXFCoupons') {
        GetXFCoupons();
    }
    else if (type == "PromGoodsDetail") {
        GetDataList();
    }
    else if (type == "PromGoodsGroup") {
        GetPromGoodsGroupS();
    }
}
function GetDataList() {
    var index = layer.load(2);
    var proc = GetQueryString("proc");//存储过程名称
    var strWhere = GetQueryString("sqlvalue");//查询条件
    var sqlvalue = $("#txtSearchValue").val();
    if (sqlvalue == "") {
        sqlvalue = strWhere;
        $("#txtSearchValue").val(sqlvalue);
    }
    var type = "ReturnDataSet";
    var pageIndex = $("#SpageIndex").html();  //表示当前页
    var pageSize = "15";    //表示每页显示的条目数
    var data = {
        type: GetQueryString("sqlType") ,
        strWhere: sqlvalue,
        PageIndex: pageIndex,
        pageSize: pageSize
    };
    document.getElementById("autodiv").style.width ="950px";
    GetData(proc, data, type, "Y", index, pageIndex, pageSize);
}

function GetPromGoodsGroupS() {
    var index = layer.load(2);
    var proc = GetQueryString("proc");//存储过程名称
    var strWhere = GetQueryString("sqlvalue");//查询条件
    var sqlvalue = $("#txtSearchValue").val();
    var status = 2;
    var type = "ReturnDataSet";
    var pageIndex = $("#SpageIndex").html();  //表示当前页
    var pageSize = "15";    //表示每页显示的条目数
    var data = {
        type: GetQueryString("sqlType"),
        status: status,
        strWhere: sqlvalue,
        PageIndex: pageIndex,
        pageSize: pageSize
    };
    document.getElementById("autodiv").style.width = "950px";
    GetData(proc, data, type, "Y", index, pageIndex, pageSize);
}
//选择商品
function GetBrandGoods() {
    var index = layer.load(2);
    var spmch = GetQueryString("sqlvalue");
    var sqlvalue = $("#txtSearchValue").val();
    if (sqlvalue == "") {
        sqlvalue = spmch;
        $("#txtSearchValue").val(sqlvalue);
    }
    var fabh = GetQueryString("fabh");
    var pageIndex = $("#SpageIndex").html();  //表示当前页
    var pageSize = "15";    //表示每页显示的条目数
    var data = {
        type: "GoodsListPP",
        strWhere: sqlvalue,
        PageIndex: pageIndex,
        pageSize: pageSize
    };
    var proc = "Proc_Admin_SearchInfo";//存储过程名
    var type = "ReturnDataSet";
    var isChose="Y"
    GetData(proc, data, type, isChose, index, pageIndex, pageSize);
}
//选择商品
function GetXFCoupons() {
    var index = layer.load(2);
    var sqlvalue = $("#txtSearchValue").val();
    var pageIndex = $("#SpageIndex").html();  //表示当前页
    var pageSize = "15";    //表示每页显示的条目数
    var data = {
        type: "XFCouponsList",
        strWhere: sqlvalue,
        PageIndex: pageIndex,
        pageSize: pageSize
    };
    var proc = "Proc_Admin_SearchInfo";//存储过程名
    var type = "ReturnDataSet";
    var isChose = "Y"
    $.ajax({
        url: "ashx/MainReturnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
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
                if (isChose == "Y") {
                    contHtml += "<th>选择</th>";
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
                        contHtml += "<tr class='text-c'  ondblclick='ChoseInfo(\"" + i + "\")'>";
                        if (isChose == "Y") {
                            contHtml += "<th ><input type=\"checkbox\" class=\"CheckInfo\"  value=\"" + itm["keyid"] + "\"></th>";
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
///加载数据
function GetData(proc, data, type, isChose, index, pageIndex, pageSize) {
    $.ajax({
        url: "ashx/MainReturnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
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
                if (isChose == "Y") {
                    contHtml += "<th>选择</th>";
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
                        contHtml += "<tr class='text-c'  ondblclick='ChoseInfo(\"" + i + "\")'>";
                        if (isChose == "Y") {
                            contHtml += "<th ><input type=\"checkbox\" class=\"CheckInfo\"  value=\"" + itm["keyid"] + "\"></th>";
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

function QuanxYwy(obj) {
    var name = $(obj).text();
    if (name == "全选") {
        $(obj).text("取消");
        $(".CheckInfo").prop("checked", true);
    } else {
        $(obj).text("全选");
        $(".CheckInfo").prop("checked", false);
    }
}
function xzInfo() {
    var list = "";
    $.each($("#conTable_Body input:checked"), function (i, itm) {
        list += itm.value + ",";
    })
    return list;
}
///保存选择的信息
function SaveChoseInfo() {
    var type = GetQueryString("type");
    if (type == "GetBrandGoods") {
        SaveBrandGoods();

    } else if (type == "SetXFCoupons") {
        SaveCoupons();
    }
    else if (type == "PromGoodsDetail") {
        SavePromGoods();
    }
    else if (type =="PromGoodsGroup") {
        SavePromGroup();
    }
}

///保存选择的优惠券
function SaveCoupons() {
    var index = layer.load(2);
    var couponcodes = xzInfo();
    if (couponcodes == "") {
        layer.alert("请选择优惠券");
        return;
    }
    var userfabh = GetQueryString("userfabh");
    var data = {
        type: "SaveUserCouponList",
        couponcodes: couponcodes,
        userfabh: userfabh
    };
    var proc = "Proc_UserInfo";//存储过程名
    var type = "ReturnNumber";
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/MainReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                layer.msg("优惠券下发成功", { time: 3000 });
                layer_close();
                //parent.location.closeAll();
                //layer.close(index);
            }
            else {
                layer.msg(obj.message, { time: 3000 });
                layer.close(index);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}
///保存选择的商品信息
function SaveBrandGoods() {
    var index = layer.load(2);
    var articleList = xzInfo();
    if (articleList == "") {
        layer.alert("请选择商品");
        layer.close(index);
        return;
    }
    var fabh = GetQueryString("fabh");
    var data = {
        type: "SaveBrandGoodList",
        articleList: articleList,
        fabh: fabh
    };
    var proc = "Proc_Admin_Brand";//存储过程名
    var type = "ReturnNumber";
    SaveData(data, proc, type, index)
}

function SaveData(data, proc, type, index) {
    var fabh = data.fabh;
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/MainReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                // layer.msg("品牌商品添加成功", { time: 3000 });
                layer.alert("品牌商品添加成功", function (index) {
                    window.parent.RefreshQueryReproList(fabh);
                    layer.close(index);
                    layer_close();
                });
            }
            else {
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}

function SavePromGoods() {
    var index = layer.load(2);
    var groupName = parent.$("#txtGroupName").val();
    var groupId = parent.$("#txtGroupId").val();
    var sortId = parent.$("#txtSort").val();
    var articleList = xzInfo();
    var genre = parent.$("#sltZqType").val();
    var data = {
        type: "SavePromGoodsGroup",
        groupName: groupName,
        groupId: groupId,
        ArticleList: articleList,
        SortId: sortId,
        Genre: genre
    };
    var proc = "Proc_PromGoodsGroup";//存储过程名
    var type = "ReturnListNumber";
    $.ajax({
        type: "Get",
        cache: false,
        url: "ashx/MainReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (obj) {
            if (obj.flag == 0) {
                layer.msg("存盘成功", { time: 1000 }, function () {
                    parent.$("#txtGroupId").val(obj.message);
                    parent.document.getElementById("btnPromClick").click();
                    layer_close();
                });
            }
            else {
                layer.msg(obj.message, { time: 3000 });
                layer.close(index);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}
function SavePromGroup() {
    var index = layer.load(2);
    var fabh = parent.$("#tdFabh").html();
    var articleList = xzInfo();
    var data = {
        type: "SavePromGroupFabh",
        fabh: fabh,
        ArticleList: articleList
    };
    var proc = "Proc_PromGoodsGroup";//存储过程名
    var type = "ReturnListNumber";
    $.ajax({
        type: "Get",
        cache: false,
        url: "ashx/MainReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (obj) {
            if (obj.flag == 0) {
                layer.msg("存盘成功", { time: 1000 }, function () {
                    parent.document.getElementById("btnPromClick").click();
                    layer_close();
                });
            }
            else {
                layer.msg(obj.message, { time: 3000 });
                layer.close(index);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}