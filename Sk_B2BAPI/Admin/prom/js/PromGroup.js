function QueryGoods() {
    layer_show("选择商品", "../SearchInfo_new.html?type=PromGoodsDetail&proc=Proc_PromGoodsGroup&sqlType=GoodsListProm&sqlvalue=" + encodeURI(""), 1000, 600);
}
////查询分组绑定商品信息
function GetPromGoodsDetail(obj) {
    var index = layer.load(2);
    var groupId = $("#txtGroupId").val();
    var strWhere = $("#txtStrWhere").val();
    var pageSize = "15";
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var data = {
        type: "GetPromGoodsDetail",
        groupId: groupId,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_PromGoodsGroup";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="checkbox" value="' + obj[i]["Id"] + '"></td>'
                    html += '<td>' + (i + 1) * pageIndex + '</td>'
                    html += '<td>' + obj[i]["GroupId"] + '</td>'
                    html += '<td>' + obj[i]["goodscode"] + '</td>'
                    html += '<td>' + obj[i]["sub_title"] + '</td>'
                    html += '<td>' + obj[i]["drug_spec"] + '</td>'
                    html += '<td>' + obj[i]["package_unit"] + '</td>'
                    html += '<td>' + obj[i]["big_package"] + '</td>'
                    html += '<td>' + obj[i]["min_package"] + '</td>'
                    html += '<td>' + obj[i]["drug_factory"] + '</td>'
                    html += '<td>' + obj[i]["AddTime"] + '</td>'
                    html += '<td>';
                    html += '<a style="text-decoration:none" onClick="DeleteGoods(\'' + obj[i]["Id"] + '\',\'0\')" href="javascript:;" title="删除"><i class="Hui-icolor-red">删除</i></a>';
                    html += '</td>'
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                $("#tdGoodsCount").html(recordCount);
                if (recordCount > 0) {
                    $("#txtGroupName").val(obj[0]["GroupName"]);
                }
                layer.close(index);
            }
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                $("#txtGroupId").val(0)
                layer.close(index);
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
///删除商品
function DeleteGoods(id,status) {
    var index = layer.load(2);
    var groupId = $("#txtGroupId").val();
    var data = {
        type: "DltPromGoods",
        GroupId: groupId,
        keyId: id,
    };
    var proc = "Proc_PromGoodsGroup";//存储过程名
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
                layer.msg("删除成功", { time: 1000 }, function () {
                    layer.closeAll();
                    GetPromGoodsDetail();
                });
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
    });
}
//批量删除
function DltPromGoodsList() {
    var groupId = $("#txtGroupId").val();
    var index = layer.load(2);
    var list = "";
    $.each($("#conTable_Body input:checked"), function (i, itm) {
        list += itm.value + ",";
    })
    if (list == "") {
        layer.msg("请选择商品", { time: 1000 }, function () {
            layer.closeAll();
        });
        return;
    }
    var data = {
        type: "DltPromGoods",
        GroupId: groupId,
        keyId: list,
    };
    var proc = "Proc_PromGoodsGroup";//存储过程名
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
                layer.msg("删除成功", { time: 1000 }, function () {
                    layer.closeAll();
                    GetPromGoodsDetail();
                });
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
    });
}
///查询促销商品分组列表
function GetPromGoodsGroup(obj) {
    var index = layer.load(2);
    var status = $("#sltStatus").val();
    var strWhere = $("#txtStrWhere").val();
    var pageSize = "15";
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var data = {
        type: "GetPromGoodsGroup",
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_PromGoodsGroup";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td>' + (i + 1) * pageIndex + '</td>'
                    html += '<td>' + obj[i]["entname"] + '</td>'
                    html += '<td>' + obj[i]["GroupId"] + '</td>'
                    html += '<td>' + obj[i]["GroupName"] + '</td>'
                    var status = obj[i]["Status"];
                    if (status == "1") {
                        html += '<td><span class="label label-warning radius">未启用</span></td>'
                    }
                    else if (status == "2") {
                        html += '<td><span class="label label-success radius">已启用</span></td>'
                    }
                    else {
                        html += '<td></td>'
                    }
                    html += '<td>' + obj[i]["GoodsCount"] + '</td>'
                    html += '<td>' + obj[i]["addTime"] + '</td>'
                    html += '<td>';
                    if (status == "1") {
                        html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"UpdatePromGoodsGroup('" + obj[i]["GroupId"] + "',2)\"  title=\"启用\" class=\"btn btn-success   radius\">启用</a>";
                        html += "<a title=\"编辑\"  onclick=\"GoodsEnditOpen('" + obj[i]["GroupId"] + "','PromGoodsDetail.html','" + obj[i]["GroupId"] + "')\" class=\"btn btn-warning radius\" style=\"text-decoration:none;margin-right: 5px\">编辑</a>";
                        html += "<a title=\"删除\"  onclick=\"UpdatePromGoodsGroup('" + obj[i]["GroupId"] + "',0)\" class=\"btn btn-danger radius\" style=\"text-decoration:none;margin-right: 5px\">删除</a>";
                    }
                    else if (status == "2") {
                        html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"UpdatePromGoodsGroup('" + obj[i]["GroupId"] + "',1)\"  title=\"禁用\" class=\"btn btn-danger radius\">禁用</a>";
                        html += "<a title=\"编辑\"  onclick=\"GoodsEnditOpen('" + obj[i]["GroupId"] + "','PromGoodsDetail.html','" + obj[i]["GroupId"] + "')\" class=\"btn btn-warning radius\" style=\"text-decoration:none;margin-right: 5px\">编辑</a>";
                    }
                    html += '</td>'
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                layer.close(index);
            }
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                $("#txtGroupId").val(0)
                layer.close(index);
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
///修改分组状态
function UpdatePromGoodsGroup(groupId,status) {
    var index = layer.load(2);
    var msg = "";
    if (status == 1) {
        msg = "是否禁用";
    }
    else if (status == 2) {
        msg = "是否启用";
    }
    else if (status == 0) {
        msg = "是否删除";
    }
    layer.confirm(msg, function () {
        var data = {
            type: "UpdatePromGoodsGroupStatus",
            GroupId: groupId,
            status: status
        };
        var proc = "Proc_PromGoodsGroup";//存储过程名
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
                    layer.msg("操作成功", { time: 1000 }, function () {
                        layer.closeAll();
                        GetPromGoodsGroup();
                    });
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
        });
    }, function () { layer.closeAll(); });
}
//打开商品资料编辑页面
function GoodsEnditOpen(title, url, id) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?GroupId=" + id
    });
    layer.full(index);
}
///查询促销商品列表
function GetGPromGoodsFabh(obj) {
    var index = layer.load(2);
    var strWhere = $("#txtStrWhere").val();
    var pageSize = "15";
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var data = {
        type: "GetPromGoodsFabh",
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_PromGoodsGroup";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td>' + (i + 1) * pageIndex + '</td>'
                    html += '<td>' + obj[i]["fabh"] + '</td>'
                    html += '<td>' + obj[i]["faTitle"] + '</td>'
                    html += '<td>' + obj[i]["faType"] + '</td>'
                    html += '<td>' + obj[i]["startDate"] + '</td>'
                    html += '<td>' + obj[i]["endDate"] + '</td>'
                    var status = obj[i]["status"];
                    if (status == "1") {
                        html += '<td><span class="label label-warning radius">为上架</span></td>'
                    }
                    else if (status == "2") {
                        html += '<td><span class="label label-success radius">已上架</span></td>'
                    }
                    else {
                            html += '<td></td>'
                    }
                    html += '<td>' + obj[i]["GroupCount"] + '</td>'
                    html += '<td>';
                    html += "<a style=\"text-decoration:none;margin-right: 5px;\"  onclick=\"GoodsEnditOpen('促销方案绑定商品分组','PromGoodsFabhDetail.html','" + obj[i]["fabh"] + "')\"  title=\"绑定分组\" class=\"btn btn-success   radius\">绑定分组</a>";
                    html += '</td>'
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                layer.close(index);
            }
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                $("#txtGroupId").val(0)
                layer.close(index);
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
///选择商品分组
function QueryGoodsProm() {
    layer_show("选择商品分组", "../SearchInfo_new.html?type=PromGoodsGroup&proc=Proc_PromGoodsGroup&sqlType=GetPromGoodsGroup&sqlvalue=" + encodeURI(""), 1000, 600);
}
////查询分组绑定商品信息
function GetPromGoodsFabhDeatil(obj) {
    var index = layer.load(2);
    var fabh = $("#tdFabh").html();
    var strWhere = $("#txtStrWhere").val();
    var pageSize = "15";
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var data = {
        type: "GetPromGoodsFabhDeatil",
        fabh: fabh,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_PromGoodsGroup";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="checkbox" value="' + obj[i]["Id"] + '"></td>'
                    html += '<td>' + (i + 1) * pageIndex + '</td>'
                    html += '<td>' + obj[i]["GroupId"] + '</td>'
                    html += '<td>' + obj[i]["GroupName"] + '</td>'
                    html += '<td>' + obj[i]["AddTime"] + '</td>'
                    html += '<td>' + obj[i]["GoodsCount"] + '</td>'
                    html += '<td>';
                    html += '<a style="text-decoration:none" onClick="DltPromGoodsFabhList(\'' + obj[i]["Id"] + '\',0)" href="javascript:;" title="删除"><i class="Hui-icolor-red">删除</i></a>';
                    html += '</td>'
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                $("#tdGoodsCount").html(recordCount);
                layer.close(index);
            }
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//批量删除
function DltPromGoodsFabhList(id,type) {
    var groupId = $("#txtGroupId").val();
    var index = layer.load(2);
    var list = "";
    if (type == 1) {
        $.each($("#conTable_Body input:checked"), function (i, itm) {
            list += itm.value + ",";
        })
    }
    else if (type == 0) {
        list = id;
    }
    if (list == "") {
        layer.msg("请选择分组", { time: 1000 }, function () {
            layer.closeAll();
        });
        return;
    }
    var data = {
        type: "DltPromGoodsFabhDeatil",
        keyId: list,
    };
    var proc = "Proc_PromGoodsGroup";//存储过程名
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
                layer.msg("删除成功", { time: 1000 }, function () {
                    layer.closeAll();
                    GetPromGoodsFabhDeatil();
                });
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
    });
}

