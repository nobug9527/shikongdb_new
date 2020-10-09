//查询帮助中心
function QueryHelpCenterList() {
    var index = layer.load(2);
    var pageSize = "15";
    var pageIndex = $("#pageIndex").html();
    //var strWhere = $("#txtStrWhere").val();//搜索条件
    var type = $("#sltType").val;
    var data = {
        type: "GetHelpCenter",
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Article";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "article/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">';
                    html += '<td>' + + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1)+ '</td>';
                    html += '<td>' + obj[i]["id"] + '</td>';
                    html += '<td>' + obj[i]["title"] + '</td>';
                    html += '<td>' + obj[i]["class_layer"] + '</td>';
                    html += '<td id="content' + i + '" style="display:none">' + obj[i]["content"] + '</td>';
                    html += '<td class="f-14 td-manage">';
                    html += '<a style="text-decoration:none" onClick="OpenHelpCenterDt(\'' + obj[i]["id"] + '\',\'' + obj[i]["title"] + '\',\'' + i + '\',\'' + obj[i]["sort_id"] + '\')" href="javascript:;" title="编辑"><i class="Hui-icolor-red">编辑</i></a>';
                    html += '</td>'; 
                    html += '</tr>';
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                var recordCount = obj["recordCount"];
                var pageCount = obj["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                layer.close(index);
            }
            if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            }
            if (type == '2') {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            layer.alert("加载失败！", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
        }
    })
}
//跳转详情
function OpenHelpCenterDt(id, title, i, sort_id)
{
    var content = $("#content" + i + "").html();
    var url = "down_helpcenter_editor.html";
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id + "&title=" + encodeURIComponent(title) + "&content=" + encodeURIComponent(content) + "&sort_id=" + sort_id
    });
    layer.full(index);
}
///保存信息
function SaveArticleInfo() {
    var index = layer.load(2);
    var id = GetQueryString("id");
    var sort_id = $("#txtSortId").val();
    var title = $("#txtTitle").val();
    if (title == "") {
        layer.alert("标题不能位空", {
            icon: 2,
            skin: 'layer-ext-moon',
            yes: function () { return; }
        });
    }
    var ue = UE.getEditor('editor');
    content = ue.getContent();
  
    var data = {
        type: "SaveHelpCenter",
        id: id,
        sort_id:sort_id,
        title: title,
        content: content
    };
    var proc = "Proc_Article";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.document.getElementById("btnClick").click(); layer_close(); }
                });
                parent.location.reload();
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