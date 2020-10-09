///获取会员信息
function GetLoadInfo() {
    var index = layer.load(2);
    var data = {
        type: "getLoadInfo",
    };
    var proc = "proc_Ywt_XsdInfo";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.flag == '0') {
                var list = result.data[0];
                $("#txtKprq").val(list.kprq);
                $("#txtCaozuoY").val(list.name);
                layer.close(index);
            }
            else if (result.flag == '1') {
                layer.close(index);
                layer.alert("业务员加载失败", {
                    icon: 2,
                    skin: 'layer-ext-moon'
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