//加载品牌详情信息
function QueryConfig(obj) {
    var index = layer.load(2);
    var json = {
        type: "PC_GetConfigurationValue",
        id: obj
    }
    var proc = "Pc_Log";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "configuration/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data[0];
                $("#sltStatus").val(obj["Status"]);
                $("#txtName").val(obj["Name"]);
                $("#txtValue").val(obj["Value"]);
                $("#txtFee").val(obj["Money"]);
                $("#txtId").val(obj["Id"]);
            }
            layer.close(index);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })

}
//加载品牌详情信息
function QueryImgType(obj) {
    var index = layer.load(2);
    var json = {
        type: "PC_GetImgTypeValue",
        id: obj
    }
    var proc = "Pc_Log";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "configuration/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data[0];
                $("#txtName").val(obj["TypeName"]);
                $("#txtId").val(obj["ID"]);
                $("#sort").val(obj["sort"]);
                $("#txtlinkurl").val(obj["link_name"]);
                if (obj["imgurl"] != '') {
                    $("#txtTemImgUrl").val(obj["imgurl"]);
                    $('#imgId').attr('src', obj["imgurl"]);
                    $("#divimg").show();
                }
            }
            layer.close(index);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })

}