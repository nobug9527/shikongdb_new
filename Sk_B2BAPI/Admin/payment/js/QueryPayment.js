//查询支付方式
function QueryPayment(obj) {
    var index = layer.load(2);
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
    var status = $("#sltStatus").val();//状态
    var strWhere = $("#txtStrWhere").val();//搜索条件
    var data = {
        type: "QueryPayment",
        enable: status,
        strWhere: strWhere,
        pageIndex: pageIndex,
        pageSize: pageSize
    };
    var proc = "Proc_OperationPayment";//存储过程名
    var type = "ReturnList";
    //加载页面数据
    $.ajax({
        type: 'Post',
        url: 'payment/ashx/ReturnJson.ashx',
        data: { proc: proc, type: type, json: JSON.stringify(data) },
        async: true,
        dataType:'json',
        success: function (result) {
            var type = result.flag;
            if (type=='0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += "<tr class='text-c'>";
                    html += "<td><input type='checkbox' value='" + obj[i]["PayId"] + "' name='PaymentList'></td>";
                    html += "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                    html += "<td>" + obj[i]["PayId"] + "</td>";
                    html += "<td>" + obj[i]["Pay"] + "</td>";
                    html += "<td>" + obj[i]["Enable"] + "</td>";
                    html += "<td class='td-manage'>";
                    if (obj[i]["Enable"]=="否") {
                        html += "<a style=\"text-decoration:none\" onClick=\"EnablePayment('" + obj[i]["PayId"] + "','是')\"  title=\"启用\"><i class=\"Hui-icolor-blue\"启用</i>启用</a>";
                        html += "<a title=\"编辑\"  onclick=\"PaymentEnditOpen('" + obj[i]["Pay"] + "','payment_editor.html','" + obj[i]["PayId"] + "')\" class=\"ml-5\" style=\"text-decoration:none\"><i class=\"Hui-icolor-yellow\">编辑</i></a>";
                    }
                    else {
                        html += "<a style=\"text-decoration:none\" onClick=\"EnablePayment('" + obj[i]["PayId"] + "','否')\"  title=\"停用\"><i class=\"Hui-icolor-red\"停用</i>停用</a>";
                        html += "<a title=\"编辑\"  onclick=\"PaymentEnditOpen('" + obj[i]["Pay"] + "','payment_editor.html','" + obj[i]["PayId"] + "')\" class=\"ml-5\" style=\"text-decoration:none\"><i class=\"Hui-icolor-yellow\">编辑</i></a>";
                    }
                    html += "</td>";
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
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            }
            else if (type == '2') {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
               
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    });
}

//支付方式状态修改
function EnablePayment(payId,enable) {
    var index = layer.load(2);
    var proc = "Proc_OperationPayment";//存储过程名
    var type = "ReturnNumber";
    var data = {
        type: "EnablePayment",
        payId: payId,
        enable: enable
    };
    //修改状态
    $.ajax({
        async: true,
        type: 'Post',
        url: 'payment/ashx/ReturnJson.ashx',
        data: { proc: proc, type: type, json: JSON.stringify(data) },
        dataType: 'json',
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                QueryPayment();
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
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    });
}

//打开支付方式编辑页面
function PaymentEnditOpen(pay,url,payid) {
    var index = layer.open({
        type: 2,
        title: pay,
        content: url + "?payid=" + payid
    });
    layer.full(index);
}


//加载页面支付方式信息
function LoadPaymentInfo() {
    var index = layer.load(2);
    var payid = GetQueryString("payid");
    var data = {
        type: "GetPaymentDetial",
        payid: payid
    };
    var proc = "Proc_OperationPayment";//存储过程名
    var type = "ReturnDataTable";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "payment/ashx/ReturnJson.ashx",
        data: { proc: proc, type: type, json: JSON.stringify(data) },
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {

                $('#txtTitle').val(obj.data[0]["Pay"]);
                //$('input:radio').val(obj.data[0]["Enable"]);
                var bool = obj.data[0]["Enable"] == "是" ? true : false;
                alert(bool);
                if (bool) {
                    $('input[type="radio"][value="是"]').attr("checked", bool);
                } else {
                    $('input[type="radio"][value="否"]').attr("checked", !bool);
                }

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
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}

//支付方式维护
function UpdatePaymentInfo() {
    var index = layer.load(2);
    var payid = GetQueryString("payid");
    var pay = $('#txtTitle').val();
    var enable = $('input:radio:checked').val();
    var data = {
        type: "UpdatePayment",
        payid: payid,
        pay: pay,
        enable: enable
    };
    var proc = "Proc_OperationPayment";//存储过程名
    var type = "ReturnNumber";
    //维护信息
    $.ajax({
        async: true,
        type: 'Post',
        url: 'payment/ashx/ReturnJson.ashx',
        data: { proc: proc, type: type, json: JSON.stringify(data) },
        dataType: 'json',
        success: function (result) {
            if (result.flag=='0') {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { layer_close(); }
                });
                parent.location.reload();
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
    });
}