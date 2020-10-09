
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
    var startDate = $("#dateMin").val();
    var endDate = $("#dateMax").val();
    var isactive = $("#txtStrActive").val();
    var status = $("#txtStrStatus").val();
    var txtStrName = $("#txtStrWhere").val();
    var data = {
        type: "PC_RechargeOrderList",
        status: status,
        startDate: startDate,
        endDate: endDate,
        isactive: isactive,
        strwhere: txtStrName,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "PC_Recharge"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "rechargeorder/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
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
                    html += '<td>' + obj[i]["orderNo"] + '</td>'
                    html += '<td>' + obj[i]["fee"] + '</td>'
                    html += '<td>' + obj[i]["payment"] + '</td>'
                    if (obj[i]["status"] == "1") {
                        html += '<td>成功</td>'
                    } else if (obj[i]["status"] == "-1") {
                        html += '<td>失败</td>'
                    }
                    if (obj[i]["operation"] == "1") {
                        html += '<td>充值</td>'
                    } else if (obj[i]["operation"] == "2") {
                        html += '<td>退款</td>'
                    }
                    html += '<td>' + obj[i]["addTime"] + '</td>'
                    html += '<td>' + obj[i]["name"] + '</td>'
                    html += '<td>' + obj[i]["telphone"] + '</td>'
                    html += '<td>' + obj[i]["title"] + '</td>'
                    //html += '<td>' + obj[i]["productblance"] + '</td>'
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
function QueryUserMoneyList(obj) {
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
    var startDate = $("#dateMin").val();
    var endDate = $("#dateMax").val();
    var txtStrName = $("#txtStrWhere").val();
    var data = {
        type: "PC_UserMoneyList",
        startDate: startDate,
        endDate: endDate,
        strwhere: txtStrName,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "PC_Recharge"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "rechargeorder/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
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
                    html += '<td>' + obj[i]["OrderNo"] + '</td>'
                    html += '<td>' + obj[i]["name"] + '</td>'
                    html += '<td>' + obj[i]["telphone"] + '</td>'
                    html += '<td>' + obj[i]["PayType"] + '</td>'
                    html += '<td>' + obj[i]["Money"] + '</td>'
                    html += '<td>' + obj[i]["Platform"] + '</td>'
                    if (obj[i]["Status"] == "1") {
                        html += '<td>成功</td>'
                    } else if (obj[i]["Status"] == "-1") {
                        html += '<td>失败</td>'
                    }
                    html += '<td>' + obj[i]["Remake"] + '</td>'
                    html += '<td>' + obj[i]["Addtime"] + '</td>'
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

function QueryRechargeGoodsList(obj) {
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
    var data = {
        type: "PC_RechargeGoods",
        status: status,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "PC_Recharge"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "rechargeorder/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="typecode" value="' + obj[i]["productId"] + '"></td>'
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>'
                    html += '<td>' + obj[i]["entId"] + '</td>'
                    html += '<td>' + obj[i]["title"] + '</td>'
                    if (obj[i]["type"] == "2") {
                        html += '<td>自定义充值</td>'
                    } else if (obj[i]["type"] == "1") {
                        html += '<td>固定充值</td>'
                    }
                    html += '<td>' + obj[i]["fee"] + '</td>'
                    html += '<td>' + obj[i]["remark"] + '</td>'
                    html += '<td>' + obj[i]["addTime"] + '</td>'
                    if (obj[i]["status"] == "1") {
                        html += '<td><lable class="label label-success radius">上架</lable></td>'
                    } else if (obj[i]["status"] == "0") {
                        html += '<td><lable class="label label-danger radius">下架</lable></td>'
                    }
                    html += '<td><a style="text-decoration:none" onClick="AddRlues(\'' + obj[i]["productId"] + '\','+ obj[i]["status"] + ')" href="javascript:;"  class="btn btn-primary radius" title="活动规则">活动规则</a></td>'
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
//加载品牌详情信息
function QueryRechargeRlue(obj) {
    var index = layer.load(2);
    var json = {
        type: "PC_RechargeRlue",
        productId: obj
    }
    var proc = "PC_Recharge";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "rechargeorder/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data[0];
                $("#sltType").val(obj["isActive"]);
                $("#txtFee").val(obj["bonusAmount"]);
                $("#txtNumber").val(obj["bonusNum"]);
                $("#dateMin").val(obj["startTime"]);
                $("#dateMax").val(obj["endTime"]);
                $("#txtId").val(obj["ruleId"]);
            }
            layer.close(index);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })

}

//function QueryRechargeRlue(obj) {
//    var index = layer.load(2);
//    var json = {
//        type: "PC_RechargeRlue",
//        productId: obj
//    }
//    var proc = "PC_Recharge";//存储过程名
//    var type = "ReturnList";
//    ///加载页面数据
//    $.ajax({
//        type: "Post",
//        cache: false,
//        url: "rechargeorder/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            var type = result.flag;
//            if (type == '0') {
//                var obj = result.data[0];
//                $("#sltType").val(obj["Status"]);
//                $("#sltType").val(obj["Status"]);
//                $("#txtFee").val(obj["Money"]);
//                $("#txtNumber").val(obj["Value"]);
//                $("#txtId").val(obj["Id"]);
//            }
//            layer.close(index);
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
//            layer.close(index);
//        }
//    })

//}
