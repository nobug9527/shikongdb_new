///业务员列表
function GetSalesManList(obj) {
    var index = layer.load(2);
    var status = $("#sltIsShowId").val();
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
        type: "GetSalesManList",
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "proc_dkxd_salesman";//存储过程名
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
                    html += '<td>' + obj[i]["username"] + '</td>'
                    var status = obj[i]["status"]
                    html += "<td>";
                    if (status == "2") {
                        html += '<span class="label label-success radius">已启用</span>';
                    }
                    else {
                        html += '<span class="label label-danger radius">已禁用</span>';
                    }
                    html += "</td>";
                    html += '<td>' + obj[i]["name"] + '</td>'
                    html += '<td>' + obj[i]["sex"] + '</td>'
                    html += '<td>' + obj[i]["telphone"] + '</td>'
                    html += '<td>' + obj[i]["add_time"] + '</td>'
                    html += '<td>' + obj[i]["GroupId"] + '</td>'
                    html += '<td>' + obj[i]["GroupName"] + '</td>'
                    html += '<td>';
                    if (status == "1") {
                        html += '<a style="text-decoration:none" class="ml-5" onClick="SalesManEnditOpen(\'' + obj[i]["userId"] + '\',\'' + obj[i]["entid"] + '\')" href="javascript:;" title="绑定分组"><i class="Hui-icolor-yellow">绑定分组</i></a> ';
                    }
                    else {
                        html += '<a style="text-decoration:none" class="ml-5" onClick="SalesManEnditOpen(\'' + obj[i]["userId"] + '\',\'' + obj[i]["entid"] + '\')" href="javascript:;" title="绑定分组"><i class="Hui-icolor-yellow">绑定分组</i></a> ';
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

//业务员绑定分组
function SalesManEnditOpen(ywyid, entId) {
    layer_show("选择会员分组", "SearchInfo_dkxd.html?type=groupBind&proc=proc_dkxd_salesman&sqlType=GetMemberGroupList&ywyId=" + encodeURI(ywyid) + "&entId=" + encodeURI(entId), 1100, 700);
}
///会员分组
function GetMemberGroupList(obj) {
    var index = layer.load(2);
    var status = $("#sltIsShowId").val();
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
        type: "GetMemberGroupList",
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "proc_dkxd_salesman";//存储过程名
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
                    var status = obj[i]["status"]
                    html += "<td>";
                    if (status == "2") {
                        html += '<span class="label label-success radius">已启用</span>';
                    }
                    else {
                        html += '<span class="label label-danger radius">已禁用</span>';
                    }
                    html += "</td>";
                    html += '<td>' + obj[i]["GroupId"] + '</td>'
                    html += '<td>' + obj[i]["GroupName"] + '</td>'
                    html += '<td>' + obj[i]["MemberNumber"] + '</td>'
                    html += '<td>' + obj[i]["lastmodifytime"] + '</td>'
                    html += '<td>';
                    if (status == "1") {
                        html += '<a style="text-decoration:none" onClick="UpdateMemberGroup(\'' + obj[i]["GroupId"] + '\',\'2\')" href="javascript:;" title="启用"><i class="Hui-icolor-blue">启用  </i></a>';

                        html += '<a style="text-decoration:none" onClick="UpdateMemberGroup(\'' + obj[i]["GroupId"] + '\',\'0\')" href="javascript:;" title="删除"><i class="Hui-icolor-red">删除</i></a>';
                    }
                    else {
                        html += '<a style="text-decoration:none" onClick="UpdateMemberGroup(\'' + obj[i]["GroupId"] + '\',\'1\')" href="javascript:;" title="禁用"><i class="Hui-icolor-red">禁用</i></a>';
                    }
                    html += '<a style="text-decoration:none" class="ml-5" onClick="MemberGroupEnditOpen(\'会员绑定(' + obj[i]["GroupName"] + ')\',\'member_group_editor.html\',\'' + obj[i]["GroupId"] + '\',\'' + obj[i]["entId"] + '\')" href="javascript:;" title="绑定会员"><i class="Hui-icolor-yellow">绑定会员</i></a> '

                    html += '<a style="text-decoration:none" class="ml-5" onClick="MemberGroupEnditOpen(\'绑定货主(' + obj[i]["GroupName"] + ')\',\'shipper_group_editor.html\',\'' + obj[i]["GroupId"] + '\',\'' + obj[i]["entId"] + '\')" href="javascript:;" title="绑定货主"><i class="Hui-icolor-blue">绑定货主</i></a> '

                    html += '<a style="text-decoration:none" class="ml-5" onClick="MemberGroupEnditOpen(\'商品绑定(' + obj[i]["GroupName"] + ')\',\'goods_group_editor.html\',\'' + obj[i]["GroupId"] + '\',\'' + obj[i]["entId"] + '\')" href="javascript:;" title="商品绑定"><i class="Hui-icolor-yellow">商品绑定</i></a> '
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
///修改会员分组状态
function UpdateMemberGroup(groupId, status) {
    //下架
    var msg = "";
    if (status == "1") {
        msg = "确认要禁用吗？";
    }
    else if (status == "2") {
        msg = "确认要启用吗？";
    }
    else if (status == "0") {
        msg = "确认要删除吗？";
    }
    layer.confirm(msg, function (index) {
        var index = layer.load(2);
        var data = {
            type: "UpdateMemberGroup",
            groupId: groupId,
            status: status,
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
                    layer.alert(obj.message, {
                        icon: 1,
                        skin: 'layer-ext-moon',
                        yes: function () { layer.closeAll(); GetMemberGroupList(); }
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
//会员分组编辑
function MemberGroupEnditOpen(title, url, id, entId) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id + "&entId=" + entId
    });
    layer.full(index);
}
////查询分组绑定业务员信息
function GetMemberGroupBind(obj) {
    var index = layer.load(2);
    var groupId = GetQueryString("id");
    var status = $("#sltIsShowId").val();
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
        type: "GetMemberGroup",
        groupId: groupId,
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "proc_dkxd_salesman";//存储过程名
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
                    html += '<td><input type="checkbox" name="checkbox" value="' + obj[i]["id"] + '"></td>'
                    html += '<td>' + (i + 1) * pageIndex + '</td>'
                    html += '<td>' + obj[i]["GroupId"] + '</td>'
                    html += '<td>' + obj[i]["addTime"] + '</td>'
                    html += '<td>' + obj[i]["username"] + '</td>'
                    html += '<td>' + obj[i]["businesscode"] + '</td>'
                    html += '<td>' + obj[i]["businessname"] + '</td>'
                    html += '<td>';
                    html += '<a style="text-decoration:none" onClick="DeleteMember(\'' + obj[i]["id"] + '\',\'1\')" href="javascript:;" title="删除"><i class="Hui-icolor-red">删除</i></a>';
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

function DeleteMember(keyid) {
    var index = layer.load(2);
    var groupId = GetQueryString("id");
    var data = {
        type: "DeleteGroupMember",
        keyid: keyid,
        groupId: groupId
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
                GetMemberGroupBind();
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
function DeleteMemberList() {
    var index = layer.load(2);
    var list = "";
    var groupId = GetQueryString("id");
    $.each($("#TbShows input:checked"), function (i, itm) {
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
        type: "DeleteGroupMember",
        list: list,
        groupId:groupId
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
                $('#checkboxMb').prop('checked', false)
                GetMemberGroupBind();
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
//选择会员信息
function OpenChoseMember() {
    var groupId = GetQueryString("id");
    var entId = GetQueryString("entId");
    layer_show("选择会员", "SearchInfo_dkxd.html?type=member&proc=proc_dkxd_salesman&sqlType=GetMemberList&groupId=" + encodeURI(groupId) + "&entId=" + encodeURI(entId), 1100, 700);
}
////添加会员分组member_group_add
function MemberGroupAddOpen() {
    var url = "member_group_add.html";
    var title = "添加会员分组";
    var index = layer.open({
        type: 2,
        title: title,
        content: url
    });
    layer.full(index);
}
//选择会员信息
function OpenChoseEntDoc() {
    layer_show("选择机构", "SearchInfo_dkxd.html?type=GetEntdoc&proc=proc_dkxd_salesman&sqlType=GetEntdoc", 1100, 700);
}
//保存分组
function SaveGroup() {
    var index = layer.load(2);
    var entId = $("#txtEntId").val();
    var title = $("#txtName").val();
    if (entId == "") {
        layer.close(index);
        layer.alert("请选择机构名称", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    if (title == "") {
        layer.close(index);
        layer.alert("分组名称不能为空", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var data = {
        type: "SaveGroup",
        list: title,
        xentId: entId
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
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); location.reload(); }
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
}

//----------------------业务员角色管理----------------
///业务员列表
function GetSalesManRoleList(obj) {
    var index = layer.load(2);
    var status = $("#sltIsShowId").val();
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
        type: "GetSalesManRole",
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "proc_dkxd_salesman";//存储过程名
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
                    html += '<td>' + obj[i]["username"] + '</td>'
                    var status = obj[i]["status"]
                    html += "<td>";
                    if (status == "2") {
                        html += '<span class="label label-success radius">已启用</span>';
                    }
                    else {
                        html += '<span class="label label-danger radius">已禁用</span>';
                    }
                    html += "</td>";
                    html += '<td>' + obj[i]["name"] + '</td>'
                    html += '<td>' + obj[i]["telphone"] + '</td>'
                    html += '<td>' + obj[i]["add_time"] + '</td>'
                    html += '<td>' + obj[i]["RoleName"] + '</td>'
                    html += '<td>';
                    if (status == "1") {
                        html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"UpdateSalesMan('" + obj[i]["userId"] + "',2)\"  title=\"启用\" class=\"btn btn-warning radius\">启用</a>";
                        html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"UpdateSalesMan('" + obj[i]["userId"] + "','" + obj[i]["entId"] + "')\"  title=\"启用\" class=\"btn btn-warning radius\">分配角色</a>";
                    }
                    else {
                        html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"UpdateSalesMan('" + obj[i]["userId"] + "',1)\"  title=\"禁用\" class=\"btn btn-danger radius\">禁用</a>";
                        html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"QueryRole('" + obj[i]["userId"] + "','" + obj[i]["entId"] + "')\"  title=\"启用\" class=\"btn btn-warning radius\">分配角色</a>";
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
///商品组合选择商品信息
function QueryRole(userId, entId) {
    var strWhere = "";
    layer_show("选择角色", "../SearchInfo.html?type=YwtRole&proc=proc_dkxd_salesman&sqlType=GetYwtRole&strWhere=" + encodeURI(strWhere) + "&userId=" + userId + "&entId=" + entId, 1000, 600);
}
///修改业务员状态
function UpdateSalesMan(ywyid, status) {
    //下架
    var msg = "";
    if (status == "1") {
        msg = "确认要禁用吗？";
    }
    else if (status == "2") {
        msg = "确认要启用吗？";
    }
    layer.confirm(msg, function (index) {
        var index = layer.load(2);
        var data = {
            type: "UpdateSalesManStatus",
            ywyid: ywyid,
            status: status,
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
                    layer.alert(obj.message, {
                        icon: 1,
                        skin: 'layer-ext-moon',
                        yes: function () { layer.closeAll(); GetSalesManRoleList(); }
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