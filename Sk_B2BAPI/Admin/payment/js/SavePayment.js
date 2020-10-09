//支付方式存盘
function SavePayment() {
    var index = layer.load(2);
    var title = $("#txtTitle").val();
    var enable = $("input:radio:checked").val();
    var data = {
        type:"SavePayment",
        pay: title,
        enable: enable
    };
    var proc = "Proc_OperationPayment";//存储过程名
    var type = "SavePayment";
    //数据存盘
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
                layer.alert(result.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { window.location.reload(); }
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