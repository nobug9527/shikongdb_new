//查看订单列表
function QueryOrderList(obj) {
    var index = layer.load(2);
    var status = $("#sltStatus").val();
    var payStatus = $("#sltPayStatus").val();
    var strWhere = $("#txtStrWhere").val();
    var pageSize = "15";
    var startDate = $("#dateMin").val();
    var endDate = $("#dateMax").val();
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
        type: "GetOrderList",
        status: status,
        startDate: startDate,
        endDate:endDate,
        payStatus:payStatus,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize
    };
    var proc = "Proc_Admin_Order";//存储过程名
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
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>'
                    html += '<td  title=\"' + obj[i]["entname"] + '\"><div class=\"text-overflow\" style=\"width: 80px;\" >' + obj[i]["entname"] + '</td>'

                    html = html + '<td><u style="cursor:pointer" class="text-primary" onClick="OrderDtOpen(\'订单详情\',\'order_detail.html\',\'' + obj[i]["order_no"] + '\')">' + obj[i]["order_no"] + '</u></td>'
                    html += '<td>' + obj[i]["businesscode"] + '</td>'
                    html += '<td>' + obj[i]["businessname"] + '</td>'
                    html += '<td>' + obj[i]["accept_name"] + '</td>'
                    html += '<td>' + obj[i]["PayType"] + '</td>'
                    var status = obj[i]["status"];
                    if (status == "未审核") {
                        html += '<td><span class="label label-danger radius">' + obj[i]["status"] + '</span></td>'
                    }
                    else { html += '<td><span class="label label-success radius">' + obj[i]["status"] + '</span></td>' }
                    html += '<td>' + obj[i]["discount_amount"] + '</td>'
                    html += '<td>' + obj[i]["real_amount"] + '</td>'
                    html += '<td>' + obj[i]["order_amount"] + '</td>'
                    html += '<td>' + obj[i]["Source"] + '</td>'
                    html += '<td>' + obj[i]["add_time"] + '</td>'
                  
                    html += '</tr>'
                }
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];

                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                $('#TbShows').empty();
                $("#TbShows").append(html);
                layer.close(index);
            }
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            } else if (type == '2') {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });

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
function OrderDtOpen(title, url, id) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id
    });
    layer.full(index);
}

////加载详情信息
function LoadOrderDetail()
{
    var index = layer.load(2);
    var order_no = GetQueryString("id");
    var data = {
        type: "GetOrderDetail",
        order_no: order_no
    };
    var proc = "Proc_Admin_Order";//存储过程名
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

//订单审核
function AuditOrder(order_no, status) {
    var index = layer.load(2);
    var proc = "Proc_Admin_Order";//存储过程名
    var type = "ReturnNumber";
    var json = {
        type: "AuditOrder",
        order_no: order_no,
        status: status
    };
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                QueryOrderList();
            }
            else {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
        }
    })
}

//主动退款订单查询
function QueryList(obj) {
    var index = layer.load(2);
    var status = $("#sltStatus").val();
    var strWhere = $("#txtStrWhere").val();
    var startDate = $("#dateMin").val();
    var endDate = $("#dateMax").val();
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
        type: "ActiveRefund",
        status: status,
        startDate: startDate,
        endDate: endDate,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize
    };
    var proc = "Proc_Admin_Order";//存储过程名
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
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td>' + (parseInt(i) + 1) + '</td>'
                    html += '<td>' + obj[i]["order_no"] + '</td>'
                    html += '<td>' + obj[i]["businessname"] + '</td>'
                    html += '<td>' + obj[i]["businesscode"] + '</td>'
                    html += '<td>' + obj[i]["accept_name"] + '</td>'
                    html += '<td>' + obj[i]["add_time"] + '</td>'
                    html += '<td>' + obj[i]["order_amount"] + '</td>'
                    html += '<td>' + obj[i]["real_amount"] + '</td>'
                    html += '<td>' + obj[i]["discount_amount"] + '</td>'
                    html += '<td>' + obj[i]["refund_fee"] + '</td>'
                    html += '<td color="red" class="c-green" >' + obj[i]["refundstatus"] + '</td>'
                    html += '<td>' + obj[i]["origin"] + '</td>'
                    html += '<td class="f-14 td-manage">';
                    if (obj[i]["refundstatus"] != "无需退款") {
                        html += '<a style="text-decoration:none;margin-right:5px;" class="btn btn-primary radius" onclick="Open(\'订单详情\',\'ActiveRefundDetail.html\',\'' + obj[i]["order_no"] + '\',\'' + obj[i]["origin"] + '\',\'' + obj[i]["refund_fee"] + '\')"  title="查看">查看</a> ';
                    } else {
                        html += '<a style="text-decoration:none;margin-right:5px;" class="btn btn-danger radius" onclick="Delet(\'' + obj[i]["order_no"]+'\')"  title="删除">删除</a> ';
                    }
                    html += '</td>';
                    html += '</tr>'
                }
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                $('#TbShows').empty();
                $("#TbShows").append(html);
                layer.close(index);
            }
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            } else if (type == '2') {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
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

//订单未下发之前删除
function Delet(orderNo) {
    var data = {
        type: "OrderDelete",
        order_no: orderNo
    };
    var proc = "Proc_Admin_Order";//存储过程名
    var type = "ReturnNumber";
    $.post(
        'order/ashx/ReturnJson.ashx',
        { json: JSON.stringify(data), proc: proc, type: type },
        function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.alert(
                    result.message,
                    {
                        icon: 1,
                        skin: 'layer-ext-moon'
                    },
                    function () {
                        setTimeout('window.location.reload()', 1000);
                    }
                );
            }
            if (type == '1') {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
            else if (type == '2') {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
            }
        },
        'json');
}