//获取用户信息
function GetUserInfo(obj)
{
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
        type: "GetUserInfo",
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Admin_MembersQuery";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "members/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html = html + "<tr class='text-c'>";
                    html += "<td><input type='checkbox' value='" + obj[i]["userid"] + "' name='UserList'></td>";
                    html += "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                    html += "<td><u style='cursor:pointer' class='text-primary' onclick=\"UserEnditOpen('" + obj[i]["name"] + "','member_editor.html','" + obj[i]["userid"] + "')\">" + obj[i]["username"] + "</u></td>";
                    html += '<td class="f-14 td-manage">';
                    html += '<a style="margin-right:5px" class="btn btn-warning radius" href="javascript:;" onclick=\"QueryCoupons(\''+ obj[i]["userid"]+'\')\">优惠券发放</a>'
                    if (obj[i]["status"] == "1") {
                        html += '<a style="text-decoration:none" onClick="AuditUser(\'' + obj[i]["userid"] + '\',\'2\')" href="javascript:;" title="审核(启用)" class="btn btn-success radius">审核(启用)</a>';
                        html += '<a style="text-decoration:none" class="ml-5 btn btn-primary radius" onClick="UserEnditOpen(\'' + obj[i]["name"] + '\',\'member_editor.html\',\'' + obj[i]["userid"] + '\')" href="javascript:;" title="编辑" >编辑</a> ';
                        //html += '<a style="text-decoration:none" class="ml-5" onClick="AuditUser(\'' + obj[i]["userid"] + '\',\'0\')" href="javascript:;" title="注销"><i class="Hui-icolor-red" class="btn btn-success radius">注销</i></a>'
                    }
                    else if (obj[i]["status"] == "2") {
                        html += '<a style="text-decoration:none" onClick="AuditUser(\'' + obj[i]["userid"] + '\',\'1\')" href="javascript:;" title="禁用" class="btn btn-danger radius">禁用</a>';
                    }
                    html += '</td>';
                    html += "<td>";
                    if (obj[i]["status"] == "2") {
                        html += '<span class="label label-success radius">正常</span>';
                    } else if (obj[i]["status"] == "1") {
                        html += '<span class="label label-danger radius">未审核或冻结</span>';
                    }
                    else {
                        html += '<span class="label label-danger radius"></span>';
                    }
                    html += "</td>"
                    
                    html += "<td>" + obj[i]["name"] + "</td>";
                    html += "<td>" + obj[i]["sex"] + "</td>";
                    html += "<td>" + obj[i]["telphone"] + "</td>";
                    html += "<td>" + obj[i]["businesscode"] + "</td>";
                    html += "<td>" + obj[i]["businessname"] + "</td>";
                    html += "<td>" + obj[i]["shortname"] + "</td>";
                    html += "<td>" + obj[i]["khType"] + "</td>";
                    html += "<td  title=\"" + obj[i]["entname"] + "\"><div class=\"text-overflow\" style=\"width: 80px;\">" + obj[i]["entname"] + "</td>";
                    html += "<td>" + obj[i]["add_time"] + "</td>";
                   
                   
                    html += "</tr>";
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                layer.close(index);
            }
            if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            }
            if (type == '2') {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
               
            } if (type == '4') {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () {  }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//修改用户信息
function UserEnditOpen(title, url, id) {
    if (title == '' || title == undefined) {
        title='会员详情'
    }
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id
    });
    layer.full(index);
}
//加载用户信息
function LoadUserInfo()
{
    var index = layer.load(2);
    var id = GetQueryString("id");
    var json = {
        type: "GetUserInfoDt",
        khId: id
    };
    var proc = "Proc_Admin_MembersQuery";//存储过程名
    var type = "ReturnList";
    $.ajax({
        type: "Post",
        cache: false,
        url: "members/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj =result.data[0]
                new Vue({
                    el: '#list',
                    data: obj
                });
                layer.close(index);
            }
            else {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer_close();}
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer_close();
        }
    })
}
///修改用户信息
function UpdateUserInfo()
{
    var index = layer.load(2);
    var userName = $("#txtUserName").val();
    var name = $("#txtName").val();
    var sex = $('#radio input[name="sex"]:checked').val();
    var telphone = $("#txtTelphone").val();
    var birthday = $("#txtBirthday").val();
    var point = $("#txtPoint").val();
    var status = $('#status input[name="status"]:checked').val();
    var pwd = $("#txtPwd").val();
    var pwdn = $("#txtPwdN").val();
    var entId = $("#txtEntId").val();
    var businessName = $("#txtBusinessName").val();
    var businessId = $("#txtBusinessId").val();
    var userId = GetQueryString("id");
    if (pwd!=pwdn)
    {
        layer.close(index);
        layer.alert("错误：确认密码验证失败！", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    if (sex == undefined) {
        sex = "";
    }
    if (entId == "")
    {
        layer.close(index);
        layer.alert("所属企业不能为空，请重新选择！", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    if (businessId == "") {
        layer.close(index);
        layer.alert("所属客户不能为空，请重新选择！", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var json = {
        type:"UpdateUserInfo",
        userName: userName,
        name: name,
        sex: sex,
        telphone: telphone,
        birthday: birthday,
        point: point,
        status: status,
        pwd: pwd,
        entId: entId,
        businessName: businessName,
        businessId: businessId,
        userId:userId
    }
    var proc = "";//存储过程名
    var type = "UpdateUserInfo";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "members/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll();}
                });
                parent.location.reload();
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
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//选择企业/客户信息
function UserChoseInfo(type)
{
    if (type == "businessDoc") {
        var entid = $("#txtEntId").val();
        if (entid == "")
        {
            layer.alert("所属企业不能为空，请重新选择！", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
            return;
        }
        var strWhere = $("#txtBusinessName").val();
        layer_show("选择客户", "SearchInfo.html?type=businessDoc&proc=Proc_Admin_SearchInfo&sqlType=GetBusinessDoc&strWhere=" + encodeURI(strWhere) + "&entId=" + entid, 1000, 600);
    }
    else if (type == "entDoc") {
        var strWhere = $("#txtEntName").val();
        layer_show("选择企业", "SearchInfo.html?type=entDoc&proc=Proc_Admin_SearchInfo&sqlType=GetEntDoc&strWhere=" + encodeURI(strWhere), 1000, 600);
    }
}
//会员审核
function AuditUser(userId, status) {
    //下架
    var msg = "";
    if (status == 1) {
        msg = "确认要禁用吗？";
    }
    else if (status == 2) {
        msg = "确认要启用吗？";
    }
    else if (status = 0) {
        msg = "确认要注销吗？";
    }
    layer.confirm(msg, function (index) {
        var index = layer.load(2);
        var data = {
            type:"AuditUser",
            userId: userId,
            status: status,
        };
        var proc = "";//存储过程名
        var type = "AuditUser";
        ///加载页面数据
        $.ajax({
            type: "Post",
            cache: false,
            url: "members/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var obj = data;
                var type = obj.flag;
                if (type == '0') {
                    layer.alert(obj.message, {
                        icon: 1,
                        skin: 'layer-ext-moon',
                        yes: function () { layer.closeAll(); GetUserInfo(); }
                    });
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
                alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                layer.close(index);
            }
        })
    });
}