//查看订单列表
function GetYwyOrderList(obj) {
    var index = layer.load(2);
    var status = $("#sltStatus").val();
    var payStatus = $("#sltPayStatus").val();
    var ywyName = $("#txtywyName").val();
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
        type: "GetYwyOrderList",
        status: status,
        ywyName: ywyName,
        startDate: startDate,
        endDate: endDate,
        payStatus: payStatus,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize
    };
    var proc = "proc_dkxd_orders";//存储过程名
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
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>'
                    html += '<td  title=\"' + obj[i]["entname"] + '\"><div class=\"text-overflow\" style=\"width: 80px;\" >' + obj[i]["entname"] + '</td>'
                    html += '<td class="td - manage"> '
                    if (obj[i]["status"] == "未审核") {
                        html += '<a class="btn btn-success radius" style = "text-decoration:none" class="ml-5" onClick = "AuditOrder(\'' + obj[i]["order_no"] + '\',2)" href = "javascript:;" title = "审核通过" > 审核通过</a >'
                        html += '<a class="btn btn-danger radius" style = "text-decoration:none;margin-left:5px" class="ml-5" onClick = "AuditOrder(\'' + obj[i]["order_no"] + '\',-1)" href = "javascript:;" title = "审核驳回" > 审核驳回</a >'
                    }
                    html += '</td>'
                    html += '<td>' + obj[i]["ywyName"] + '</td>'
                    html = html + '<td><u style="cursor:pointer" class="text-primary" onClick="OrderDtOpen(\'订单详情\',\'../order_detail.html\',\'' + obj[i]["order_no"] + '\')">' + obj[i]["order_no"] + '</u></td>'
                    html += '<td>' + obj[i]["businesscode"] + '</td>'
                    html += '<td>' + obj[i]["businessname"] + '</td>'
                    html += '<td>' + obj[i]["accept_name"] + '</td>'
                    html += '<td>' + obj[i]["PayType"] + '</td>'
                    var payStatus = obj[i]["payment_status"];
                    if (payStatus == "未支付") {
                        html += '<td><span class="label label-warning radius">' + obj[i]["payment_status"] + '</span></td>'
                    }
                    else if (payStatus == "已支付") {
                        html += '<td><span class="label label-success radius">' + obj[i]["payment_status"] + '</span></td>'
                    }
                    else {
                        html += '<td>货到付款</td>'
                    }
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