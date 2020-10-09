
//获取渠道类型列表
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
    var status = $("#txtStrStatus").val();
    var txtStrName = $("#txtStrWhere").val();
    var data = {
        type: "PC_GetCriticismsList",
        status: status,
        strwhere: txtStrName,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_OperationCriticisms"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "qualification/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
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
                    if (obj[i]["status"] == "1") {
                        html += '<td>未审核</td>'
                    } else if (obj[i]["status"] == "2") {
                        html += '<td>审核通过</td>'
                    } else if (obj[i]["status"] == "-1") {
                        html += '<td>拒绝展示</td>'
                    }
                    html += '<td>' + obj[i]["name"] + '</td>'
                    html = html + '<td style="text-align:left">' + obj[i]["sub_title"] + '</td>'
                    html += '<td>' + obj[i]["starLevel"] + '</td>'
                    html += '<td>' + obj[i]["addtime"] + '</td>'
                    html += '<td>' + obj[i]["content"] + '</td>'
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