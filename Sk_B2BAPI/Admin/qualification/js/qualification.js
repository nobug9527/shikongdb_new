
////加载客户分类
function LoadClientType() {
    var index = layer.load(2);
    var type = GetQueryString("Type");
    var data = {
        type: "GetClientType"
    };
    var proc = "PC_CustomerType"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "qualification/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                $("#sltTypeID").empty();//清空select列表数据
                var obj = result.data;
                var htmlStr = "";
                for (var i = 0; i < obj.length; i++) {
                    htmlStr += "<option value='" + obj[i]["TypeID"] + "'>" + obj[i]["clienttype"] + "</option>";
                }
                $("#sltTypeID").append(htmlStr);
                layer.close(index);
            }
            else{
                layer.close(index);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//加载erp客户分类
function LoadErpClientType() {
    var index = layer.load(2);
    var type = GetQueryString("Type");
    var data = {
        type: "GetErpClientType"
    };
    var proc = "PC_CustomerType"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "qualification/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                $("#sltErpClientType").empty();//清空select列表数据
                var obj = result.data;
                var htmlStr = "";
                for (var i = 0; i < obj.length; i++) {
                    htmlStr += "<option value='" + obj[i]["clienttype"] + "'>" + obj[i]["clienttype"] + "</option>";
                }
                $("#sltErpClientType").append(htmlStr);
                layer.close(index);
            }
            else {
                layer.close(index);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//获取渠道类型列表
function QueryList(obj) {
    var index = layer.load(2);
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var pageSize = '25'; 
    var status = $("#txtStrStatus").val();
    var txtStrCode = $("#txtStrCode").val();
    var txtStrName = $("#txtStrWhere").val();
    var data = {
        type: "PC_CustomerTypeBFList",
        typeid: txtStrCode,
        status: status,
        name: txtStrName,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "PC_CustomerType"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "qualification/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="typecode" value="' + obj[i]["ID"] + '"></td>'
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>'
                    html += '<td>' + obj[i]["clienttype"] + '</td>'
                    html = html + '<td style="text-align:left">' + obj[i]["typename"] + '</td>'
                    html += '<td>' + obj[i]["name"] + '</td>'
                    if (obj[i]["status"] == "0") {
                        html += '<td>正常</td>'
                    } else if (obj[i]["status"] == "-1") {
                        html += '<td>已删除</td>'
                    }
                    html += '<td class="f-14 td-manage">';
                    html += '<a style="text-decoration:none;margin-right:5px;" class="btn btn-primary radius"  href="QualificationsSonList.html?id=' + obj[i]["ID"] + '&name=' + obj[i]["clienttype"] + '&typeid=' + obj[i]["TypeID"]+ '" title="资质管理">资质管理</a> ';
                    html += '</td>';
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                layer.close(index);
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
                layer.close(index);
            } else if (type == '2') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });

            }
            else if (type == '4') {
                $('body').html('<img src="../UploadImages/素材/权限不足.png" style="width:75%"/>');
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
            $('body').removeAttr("style");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//获取渠道资质管理列表
function QuerySonList(obj) {
    var index = layer.load(2);
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var pageSize = '25';
    var txtStrName = $("#txtStrWhere").val();
    var data = {
        type: "PC_MaterialTypeList",
        typeid: GetQueryString("id"),
        name: txtStrName,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "PC_CustomerType"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "qualification/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="typecode" value="' + obj[i]["id"] + '"></td>'
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>'
                    html += '<td>' + GetQueryString("name") + '</td>'
                    html += '<td>' + obj[i]["materialName"] + '</td>'
                    html = html + '<td style="text-align:left">' + obj[i]["customerType"] + '</td>'
                    html += '<td>' + obj[i]["addTime"] + '</td>'
                    if (obj[i]["status"] == "0") {
                        html += '<td>正常</td>'
                    } else if (obj[i]["status"] == "-1") {
                        html += '<td>已删除</td>'
                    }
                    html += '<td>' + obj[i]["remark"] + '</td>'
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                layer.close(index);
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
                layer.close(index);
            } else if (type == '2') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });

            }
            else if (type == '4') {
                $('body').html('<img src="../UploadImages/素材/权限不足.png" style="width:75%"/>');
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
            $('body').removeAttr("style");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
function LoadClientTypeList() {
    var index = layer.load(2);
    var pageIndex = 1;
    var pageSize = 100;
    var strWhere = $("#txtStrWhere").val();
    var type = GetQueryString("Type");
    var data = {
        type: "GetClientType",
        strWhere: strWhere
    };
    var proc = "PC_CustomerType"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "qualification/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>'
                    html += '<td>' + obj[i]["name"] + '</td>'
                    html += '<td>' + obj[i]["clienttype"] + '</td>'
                    html += '<td class="f-14 td-manage">';
                    html += "<a style=\"text-decoration:none;margin-right:5px;\" class=\"btn btn-primary radius\" onclick=\"DltClientType(" + obj[i]["ID"]+")\"   title=\"删除\">删除</a>";
                    html += '</td>';
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                layer.close(index);
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
                layer.close(index);
            } else{
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
function DltClientType(keyId) {
    layer.confirm("是否删除", function (index) {
        var index = layer.load(2);
        var data = {
            type:"DltCustomerType",
            id: keyId
        };
        var proc = "PC_CustomerType";//存储过程名
        var type = "ReturnNumber";
        ///加载页面数据
        $.ajax({
            type: "Post",
            cache: false,
            url: "qualification/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var obj = data;
                var type = obj.flag;
                if (type == '0') {
                    layer.alert(obj.message, {
                        icon: 1,
                        skin: 'layer-ext-moon',
                        yes: function () { layer.closeAll(); LoadClientTypeList(); }
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
        })
    });
}




//存盘
function Save_Brand() {
    var index = layer.load(2);
    var TypeID = $("#sltTypeID").val();
    var Clienttype = $("#txtClienttype").val();
    if (TypeID == "" || Clienttype == "") {
        layer.close(index);
        layer.msg("数据不完整请填写完整后提交！", { time: 3000 });
        return;
    }
    var json = {
        type: "PC_AddCustomerType_bf",
        Clienttype: Clienttype,
        TypeID: TypeID
    };
    var proc = "PC_CustomerType";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "qualification/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                layer.msg("保存成功", { time: 3000 });
                parent.location.reload();
                layer_close();
            }
            else {
                layer.close(index);
                layer.msg("保存异常，请检查网络，以及联系维护人员", { time: 3000 });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
function LoadSelect() {
    var type = GetQueryString("Type");
    if (type == "B2BType") {
        var index = layer.load(2);
        $("#sltTypeID").empty();//清空select列表数据
        var htmlStr = "";
        htmlStr += "<option>终端</option>";
        htmlStr += "<option>连锁</option>";
        htmlStr += "<option>批发</option>";
        $("#sltTypeID").append(htmlStr);
        layer.close(index);
    }
    else {
        $("#DivB2B").hide();
        $("#DivErp").show();
        LoadClientType();
        LoadErpClientType();
    }
}
///保存客户分类
function SaveClientType() {
    var index = layer.load(2);
    var name = $("#sltTypeID").val();
    var Clienttype = $("#txtClienttype").val();
    if (name == "" || Clienttype == "") {
        layer.close(index);
        layer.msg("数据不完整请填写完整后提交！", { time: 3000 });
        return;
    }
    var json = {
        type: "SaveClientType",
        Clienttype: Clienttype,
        name: name
    };
    var proc = "PC_CustomerType";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "qualification/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                //layer.close(index);
                //layer.msg("保存成功", { time: 3000 });
                layer.alert("操作成功", {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.document.getElementById("btnQueryType").click(); layer_close(); }
                });
                
            }
            else {
                layer.close(index);
                layer.msg(result.message, { time: 3000 });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
///存盘
function Save() {
    var type = GetQueryString("Type");
    if (type == "B2BType") {
        SaveClientType();
    }
    else {
        SaveErpClientType();
    }
}
///保存客户分类
function SaveErpClientType() {
    var index = layer.load(2);
    var name = $("#sltTypeID").val();
    var Clienttype = $("#sltErpClientType").val();
    if (name == "" || Clienttype == "") {
        layer.close(index);
        layer.msg("数据不完整请填写完整后提交！", { time: 3000 });
        return;
    }
    var json = {
        type: "SaveErpClientType",
        Clienttype: Clienttype,
        name: name
    };
    var proc = "PC_CustomerType";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "qualification/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                //layer.close(index);
                //layer.msg("保存成功", { time: 3000 });
                layer.close(index);
                layer.alert("保存成功", {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () {
                        parent.location.reload();
                        layer_close();
                    }
                });
            }
            else {
                layer.close(index);
                layer.msg("保存异常，请检查网络，以及联系维护人员", { time: 3000 });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
