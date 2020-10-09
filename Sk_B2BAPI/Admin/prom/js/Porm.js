////获取促销方案
function GetPromPlan(obj) {
    var index = layer.load(2);
    var startDate = $("#dateMin").val();
    var endDate = $("#dateMax").val();
    var strWhere = $("#txtStrWhere").val();
    var faType = $("#sltShowType").val();
    var status = $("#sltStatus").val();
    //if (startDate == "" || endDate == "")
    //{
    //    layer.close(index);

    //    layer.alert("请选择日期", {
    //        icon: 2,
    //        skin: 'layer-ext-moon'
    //    });
    //    return;
    //}
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
        type: "GetPromPlanDt",
        faType: faType,
        status: status,
        startDate: startDate,
        endDate: endDate,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Admin_PromQuery";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "prom/ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                var html = "";
                for (var i = 0; i < obj["data"].length; i++) {
                    html = html + "<tr class='text-c'>"
                    html = html + "<td><input type='checkbox' value='" + obj["data"][i]["fabh"] + "' name='PromList'></td>"
                    html = html + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>"
                    html = html + "<td onClick='PormDtOpen(\"促销详情\",\"prom_detail.html\",\"" + obj["data"][i]["fabh"] + "\",\"" + obj["data"][i]["entid"] + "\")'>" + obj["data"][i]["fabh"] + "</td>"
                    html = html + "<td>" + obj["data"][i]["faTitle"] + "</td>"
                    html = html + "<td>" + obj["data"][i]["faType"] + "</td>"
                    html = html + "<td>" + obj["data"][i]["startDate"] + "</td>"
                    html = html + "<td>" + obj["data"][i]["endDate"] + "</td>"
                    //html = html + "<td ><div class=\"text-overflow\" style=\"width: 200px;\">" + obj["data"][i]["describe"] + "</div></td>"
                    var status = obj["data"][i]["status"];
                    html = html + "<td>" + obj["data"][i]["addtime"] + "</td>"
                    if (status == "1") {
                        html = html + "<td class='td-status'><span class='btn btn-danger-outline radius'>未发布</span></td>";
                    }
                    else if (status == "2") {
                        html = html + "<td class='td-status'><span class='btn btn-success-outline radius'>已发布</span></td>";
                    }
                    else if (status == "0") {
                        html = html + "<td class='td-status'><span class='btn btn-success-outline  radius'>已删除</span></td>";
                    }
                    html = html + "<td class='td-manage'>"
                    if (status == "2") {
                        html = html + "<a style=\"text-decoration:none\" onClick=\"AuditProm('" + obj["data"][i]["fabh"] + "','1')\"  title=\"下架\" class=\"btn btn-danger radius\">下架</a>"
                    }
                    else if (status == "1") {
                        html = html + "<a style=\"text-decoration:none;margin-right:5px;\" onClick=\"AuditProm('" + obj["data"][i]["fabh"] + "','2')\"  title=\"上架\" class=\"btn btn-success radius\">上架</a>"
                        //html = html + "<a title=\"编辑\"  onclick=\"member_edit('编辑','member-add.html','4','','510')\" class=\" btn btn-warning  radius\" style=\"text-decoration:none;margin-right:5px;\">编辑</a>"
                        html = html + "<a title=\"删除\" href=\"javascript:;\" onclick=\"AuditProm('" + obj["data"][i]["fabh"] + "','0')\" class=\"btn btn-danger radius\" style=\"text-decoration:none;margin-right:5px;\">删除</a>"
                    }
                    html = html + "</td>"
                    html = html + "</tr>";
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                var recordCount = obj["recordCount"];
                var pageCount = obj["pageCount"];
                document.getElementById("recordCount").innerHTML = recordCount;
                document.getElementById("pageCount").innerHTML = pageCount;
                layer.close(index);
            }
            if (type == '1') {
                $('#TbShows').empty();
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                layer.close(index);
            }
            if (type == '2') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
               
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            layer.msg("加载失败", { time: 3000 });
        }
    })
}

///////促销方案维护
function AuditProm(id, status) {
    var index = layer.load(2);
    var proc = "Proc_Admin_PromQuery";//存储过程名
    var type = "ReturnNumber";
    var json = {
        type: "AuditProm",
        fabh: id,
        status: status
    };
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "brand/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                GetPromPlan();
            }
            else {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            layer.msg("加载失败", { time: 3000 });
        }
    })
}

////加载详情信息
function LoadPromDetail() {
    var index = layer.load(2);
    var fabh = GetQueryString("id");
    var data = {
        type: "GetPromDetail",
        fabh: fabh
    };
    var proc = "Proc_Admin_PromQuery";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "order/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result;
                new Vue({
                    el: '.tab-content',
                    data: obj
                });
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
            layer.close(index);
            layer.msg("未知错误", { time: 3000 });
        }
    })
}

//订单详情
function PormDtOpen(title, url, id, entid) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id + "&entid=" + entid
    });
    layer.full(index);
}

//修改促销
function Edit() {
    var index = layer.load(2);
    var data = {
        type: "EditProm",
        fabh: $('#fabhtext').text(),
        fabs: $('#fabstext').val(),
        describe: $('#describetext').val(),
        startDate: $('#datemin').val(),
        endDate: $('#datemax').val()
    }
    var proc = "Proc_Admin_PromQuery";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "order/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.alert(result.message, {
                    icon: 1,
                    skin: 'layer-ext-moon'
                });
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
            layer.close(index);
            layer.msg("未知错误", { time: 3000 });
        }
    })
}



