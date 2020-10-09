////查询分组绑定商品信息
function GetGoodsGroupBind(obj) {
    var index = layer.load(2);
    var groupId = GetQueryString("id");
    var status = $("#sltIsShowId").val();
    var strWhere = $("#txtStrWhere").val();
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
        type: "GetGoodsGroupList",
        groupId: groupId,
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "proc_dkxd_GoodsInfo";//存储过程名
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
                    html += '<td><input type="checkbox" name="checkbox" value="' + obj[i]["id"] + '"></td>'
                    html += '<td>' + (i + 1) * pageIndex + '</td>'
                    html += '<td>' + obj[i]["GroupId"] + '</td>'
                    html += '<td>' + obj[i]["goodscode"] + '</td>'
                    html += '<td>' + obj[i]["sub_title"] + '</td>'
                    html += '<td>' + obj[i]["drug_spec"] + '</td>'
                    html += '<td>' + obj[i]["package_unit"] + '</td>'
                    html += '<td>' + obj[i]["big_package"] + '</td>'
                    html += '<td>' + obj[i]["min_package"] + '</td>'
                    html += '<td>' + obj[i]["drug_factory"] + '</td>'
                    html += '<td>';
                    html += '<a style="text-decoration:none" onClick="DeleteGoods(\'' + obj[i]["id"] + '\',\'1\')" href="javascript:;" title="删除"><i class="Hui-icolor-red">删除</i></a>';
                    html += '</td>'
                    html += '</tr>'
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
    })
}
//选择商品信息
function OpenChoseGoods() {
    var groupId = GetQueryString("id");
    var entId = GetQueryString("entId");
    layer_show("选择商品", "SearchInfo_dkxd.html?type=goods&proc=proc_dkxd_GoodsInfo&sqlType=GetGoodsList&groupId=" + encodeURI(groupId) + "&entId=" + encodeURI(entId), 1100, 700);
}

function DeleteGoods(keyid) {
    var index = layer.load(2);
    var groupId = GetQueryString("id");
    var data = {
        type: "DeleteGroupGoods",
        keyid: keyid,
        groupId: groupId
    };
    var proc = "proc_dkxd_GoodsInfo";//存储过程名
    var type = "ReturnNumber";

    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                layer.close(index);
                GetGoodsGroupBind();
            }
            else {
                layer.closeAll();
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.closeAll();
        }
    })

}
function DeletGoodsList() {
    var index = layer.load(2);
    var list = "";
    var groupId = GetQueryString("id");
    $.each($("#TbShows input:checked"), function (i, itm) {
        list += itm.value + ",";
    })
    if (list == "") {
        layer.close(index);
        layer.alert("请选择商品", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var data = {
        type: "DeleteGroupGoods",
        list: list,
        groupId: groupId
    };
    var proc = "proc_dkxd_GoodsInfo";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                layer.close(index);
                $('#checkboxMb').prop('checked', false)
                GetGoodsGroupBind();
            }
            else {
                layer.closeAll();
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.closeAll();
        }
    })

}