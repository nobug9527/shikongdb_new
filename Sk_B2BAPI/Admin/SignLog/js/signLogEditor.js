///加载客户分类
function LoadClientType() {
    var index = layer.load(2);
    var type = "GetClientType";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/SignLogEditor.ashx?type=" + type,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var list = result.data;
                if (list.length > 0) {
                    var htmlStr = "";
                    $.each(list, function (i, itm) {
                        htmlStr += " <option value=\"" + itm["TypeID"] + "\">" + itm["ClientType"] + "</option>";
                    });
                    $("#sltCategory").html(htmlStr);
                }
                layer.close(index);
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
//页面加载时获取方法
function QuerySignRole() {
    var index = layer.load(2);
    var data = {
        type: "QueryLog"
    };
    var proc = "Proc_SignLog";//存储过程名
    var type = "ReturnList";
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            layer.close(index);
            var htmlStr = "";
            if (result.flag == 0) {
                htmlStr += "";
                $.each(result.data, function (i,itm) {
                    htmlStr += "<tr class=\"text-c\">";
                    htmlStr += "<td>" + (i+1) + "</td>";
                    htmlStr += "<td>" + itm.rewardform + "</td>";
                    htmlStr += "<td><input type=\"text\" value=\"" + itm.signrule + "\"  class=\"input-text\" style=\"width:80% \" /><span id=\"spSignRule\">天</span></td>";
                    htmlStr += "<td><input type=\"text\" value=\"" + itm.signreward + "\" class=\"input-text\" style=\"width:80%\"  /><span id=\"spRewardRule\">积分</span></td>";
                    htmlStr += "<td><a onclick=\"DltSignRole("+itm.id+")\" class=\"btn btn-danger radius\">删除</a></td>";
                    htmlStr += "</tr>";
                });
                htmlStr += "<tr class=\"text-c\">";
                htmlStr += "<td id=\"signXh\">" + (result.data.length+1) + "</td>";
                htmlStr += "<td>";
                htmlStr += "<select id=\"stlSignLx\" class=\"input-text\">";
                htmlStr += "<option value=\"积分\">积分</option>";
                htmlStr += "<option value=\"优惠券\">优惠券</option>";
                htmlStr += "<option value=\"礼品\">礼品</option>";
                htmlStr += "</select>";
                htmlStr += "</td>";
                htmlStr += "<td>";
                htmlStr += "<input type=\"text\" value=\"0\" id=\"txtSignRule\" class=\"input-text\" style=\"width:80% \"  /><span id=\"spSignRule\">天</span>";
                htmlStr += "</td>";
                htmlStr += "<td>";
                htmlStr += "<input type=\"text\" id=\"slRewardRule\" value=\"0\" class=\"input-text\" style=\"width:80%\"  /><span id=\"spRewardRule\">积分</span>";
                htmlStr += "</td>";
                htmlStr += "<td><a onclick=\"AddSignRole()\" class=\"btn btn-secondary radius\">保存</a></td>";
                htmlStr += "</tr>";
                $("#TbGoods").html(htmlStr);
            }
            else if (result.flag == 1) {
                htmlStr = "<tr class=\"text - c\">";
                htmlStr += "<td id=\"signXh\">1</td>";
                htmlStr += "<td>";
                htmlStr += "<select id=\"stlSignLx\" class=\"input - text\">";
                htmlStr += "<option value=\"积分\">积分</option>";
                htmlStr += "<option value=\"优惠券\">优惠券</option>";
                htmlStr += "<option value=\"礼品\">礼品</option>";
                htmlStr += "</select>";
                htmlStr += "</td>";
                htmlStr += "<td>";
                htmlStr += "<input type=\"text\" value=\"0\" id=\"txtSignRule\" class=\"input-text\" style=\"width:80% \"  /><span id=\"spSignRule\">天</span>";
                htmlStr += "</td>";
                htmlStr += "<td>";
                htmlStr += "<input type=\"text\" id=\"slRewardRule\" value=\"0\" class=\"input-text\" style=\"width:80%\"  /><span id=\"spRewardRule\">积分</span>";
                htmlStr += "</td>";
                htmlStr += "<td><a onclick=\"AddSignRole()\" class=\"btn btn-secondary radius\">保存</a></td>";
                htmlStr += "</tr>";
                $("#TbGoods").html(htmlStr);
            }
            else {
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
            layer.close(index);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })

}
//创建生成积分规则
function AddSignRole() {
    var index = layer.load(2);
    var signLx = $("#stlSignLx").val();
    var signRule = $("#txtSignRule").val();
    if (signRule == "" || signRule<=0) {
        layer.closeAll();
        layer.alert("签到规则不能为空", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var rewardRule = $("#slRewardRule").val();
    var signXh = $("#signXh").html();
    var data = {
        type: "InsertSign",
        RewardForm: signLx,
        SignRule: signRule,
        SignReward:rewardRule
    };
    var proc = "Proc_SignLog";//存储过程名
    var type = "ReturnNumber";

    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (obj) {
            if (obj.flag == '0') {
                QuerySignRole();
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
            closeLoading()
        }
    });
}
///删除规则
function DltSignRole(id) {
    var index = layer.load(2);
    var data = {
        type: "DelSign",
        Id: id,
    };
    var proc = "Proc_SignLog";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (result.flag == 0) {
                layer.alert("操作成功", {
                    icon: 1,
                    skin: 'layer-ext-moon'
                });
                QuerySignRole();
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
function SaveSignRole() {
    var index = layer.load(2);
    var planName = $("#txtPlanName").val();
    var clientType = $("#sltCategory").val();
    var signModel= $("#sltSignModel").val();
    var ue = UE.getEditor('editor');
    var content = ue.getContent();
    if (planName == "") {
        layer.alert("方案名称不能为空", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var data = {
        type: "CreateSign",
        planName: planName,
        clientType: clientType,
        signModel: signModel,
        content: content
    };
    var proc = "Proc_SignLog";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (result.flag == 0) {
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { location.reload();}
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
///////查询签到规则列表
////查询签到规则列表
function GetSigmRoleList(obj) {
    var index = layer.load(2);
    var pageSize = "30";
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var status = $("#sltIsShowId").val();
    var sqlWhere = $("#txtStrWhere").val();
    var data = {
        type: "GetSign",
        status: status,
        sqlWhere: sqlWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_SignLog";//存储过程名
    var type = "ReturnList";
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.flag == 0) {
                var htmlStr = "";
                $.each(result.data, function (i, itm) {
                    htmlStr += "<tr class='text-c'>"
                    htmlStr += "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>"
                    htmlStr += "<td align='center'>" + itm.entname + "</td>"
                    htmlStr += "<td align='center'>" + itm.fabh + "</td>"
                    htmlStr += "<td align='center'>" + itm.faName + "</td>"
                    htmlStr += "<td align='center'>" + itm.faType + "</td>"
                    htmlStr += "<td align='center'>" + itm.clienttype + "</td>"
                    var status = itm.status;
                    var html = "";
                    if (status == 1) {
                        htmlStr += "<td align='center'><span class='label label-danger radius'>未启用</span></td>"
                        html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"UpdateSignRoleStatus('" + itm.fabh+"',2)\"  title=\"启用\" class=\"btn btn-warning radius\">启用</a>";
                        html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"UpdateSignRoleStatus('" + itm.fabh+"',0)\"  title=\"删除\" class=\"btn btn-danger radius\">删除</a>";
                    }
                    else {
                        htmlStr += "<td align='center'><span class='label label-success radius'>已启用</span></td>"
                html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"UpdateSignRoleStatus('" + itm.fabh +"',1)\"  title=\"禁用\" class=\"btn btn-danger radius\">禁用</a>";
                    }
                    htmlStr += "<td align='center'>" + itm.addTime + "</td>"
                    htmlStr += "<td>" + html + "</tb>";
                    htmlStr += "</tr>";
                });
                $("#TbShows").html(htmlStr);
                var recordCount = result.recordCount;
                var pageCount = result.pageCount;
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                layer.close(index);
            }
            if (result.flag == '1') {
                $('#TbShows').empty();
                layer.close(index);
            }
            if (result.flag == '2') {
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
function UpdateSignRoleStatus(fabh,status) {
    var index = layer.load(2);
    var data = {
        type: "UpdateSign",
        id: fabh,
        status: status
    };
    var proc = "Proc_SignLog";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.flag == 0) {
                layer.alert("操作成功", {
                    icon: 1,
                    skin: 'layer-ext-moon'
                });
                GetSigmRoleList();
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


