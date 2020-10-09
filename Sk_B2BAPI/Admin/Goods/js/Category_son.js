
//获取商品分类列表
function QueryTwoCategoryList(obj) {
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
    var data = {
        type: "GetGoodsCategory_ej",
        erid: getQueryString("id"),
        PageIndex: pageIndex,
        PageSize: pageSize,

    };
    var proc = "Proc_Admin_Category";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="" value="' + obj[i]["id"] + '"></td>'
                    html += '<td>' + (parseInt(i) + 1) + '</td>'
                    html += '<td>' + obj[i]["id"] + '</td>'
                    if (obj[i]["class_layer"] == "1") {
                        html = html + '<td style="text-align:left"><u style="cursor:pointer" class="text-primary" onClick="CategoryOpen(\'' + obj[i]["title"] + '\',\'goods_category_editor.html\',\'' + obj[i]["id"] + '\',\'' + obj[i]["class_layer"] + '\')">' + obj[i]["title"] + '</u></td>'
                    }
                    else {
                        html = html + '<td style="text-align:left">&nbsp;&nbsp;├<u style="cursor:pointer" class="text-primary" onClick="CategoryOpen(\'' + obj[i]["title"] + '\',\'goods_category_editor.html\',\'' + obj[i]["id"] + '\',\'' + obj[i]["class_layer"] + '\')">' + obj[i]["title"] + '</u></td>'
                    }
                    html += '<td>' + obj[i]["class_layer"] + '</td>'
                    html += '<td>' + obj[i]["sort_id"] + '</td>'
                    html += '<td class="f-14 td-manage">';
                    html += '<a style="text-decoration:none;margin-right:5px;" class="btn btn-warning radius" onClick="CategoryOpen(\'' + obj[i]["title"] + '\',\'goods_category_editor.html\',\'' + obj[i]["id"] + '\',\'' + obj[i]["class_layer"] + '\')" href="javascript:;" title="编辑">编辑</a> ';
                    html += '<a style="text-decoration:none;margin-right:5px;" class="btn btn-danger radius" onClick="DltCategory(\'' + obj[i]["id"] + '\')" href="javascript:;" title="删除">删除</a>'
                    html += '</td>';
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
//修改方案信息
function CategoryOpen(title, url, id, class_layer) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id + "&class_layer=" + class_layer + "&title=" + GetQueryString("name") + '&fatherid=' + GetQueryString("id") 
    });
    layer.full(index);
}
//加载一级分类选项
function LoadCategory() {
    var index = layer.load(2);
    var id = GetQueryString("id");
    var class_layer = GetQueryString("class_layer");
    if (class_layer == 2) {
        var data = {
            type: "GetGoodsCategory_yj"
        };
        var proc = "Proc_Admin_Category";//存储过程名
        var type = "ReturnList";
        ///加载页面数据
        $.ajax({
            type: "Post",
            cache: false,
            url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var type = result.flag;
                if (type == '0') {
                    var obj = result.data;
                    $("#sltParentId option").remove(); //删除Select的Option
                    for (var i = 0; i < obj.length; i++) {
                        $("#sltParentId").append("<option value='" + obj[i]["id"] + "'>" + obj[i]["title"] + "</option>");
                    }
                    if (id!='') {
                        $("#sltParentId option[value='" + id + "']").attr("selected", "selected")
                    }
                    layer.close(index);
                }
                else if (type == "1") {
                    layer.close(index);
                    $("#sltParentId option").remove(); //删除Select的Option
                    $("#sltParentId").append("<option value=''>无父级分类</option>");
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
                layer.alert("加载失败", {
                    icon: 2,
                    skin: 'layer-ext-moon'
                })
            }
        });
    }
    else {
        layer.close(index);
    }
}
///加载详细信息
function LoadDetail() {
    var index = layer.load(2);
    var id = GetQueryString("id");
    var class_layer = GetQueryString("class_layer");
    var data = {
        type: "GetGoodsCategory_detail",
        id: id
    };
    var proc = "Proc_Admin_Category";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data[0];
                if (class_layer == 2) {
                    $("#sltParentId").val(obj["parent_id"]);
                }
                $("#txtSortId").val(obj["sort_id"]);
                $("#txtName").val(obj["title"]);
                $("#txtAlias").val(obj["call_index"]);
                $("#txtSeoTitle").val(obj["seo_title"]);
                $("#txtContent").val(obj["content"]);
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
            layer.alert("加载失败", {
                icon: 2,
                skin: 'layer-ext-moon'
            })
        }
    });
}
//分类存盘
function Save_Category() {
    var index = layer.load(2);
    var class_layer = GetQueryString("class_layer");
    var parent_id = $("#sltParentId").val();
    var sort_id = $("#txtSortId").val();
    var title = $("#txtName").val();
    var call_index = $("#txtAlias").val();
    var seo_title = $("#txtSeoTitle").val();
    var content = $("#txtContent").val();
    var id = GetQueryString("id");
    if (id == "") {
        id = "";
    }
    if (title == "") {
        layer.close(index);
        layer.msg("类别名称不能为空", { time: 3000 });
        return;
    }
    var data = {
        type: "SaveGoodsCategory",
        class_layer: class_layer,
        parent_id: parent_id,
        sort_id: sort_id,
        title: title,
        call_index: call_index,
        seo_title: seo_title,
        content: content,
        id: id
    };
    var proc = "Proc_Admin_Category";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                layer.msg("提交成功", { time: 3000 });
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
            layer.close(index);
            layer.alert("加载失败", {
                icon: 2,
                skin: 'layer-ext-moon'
            })
        }
    });

}
//删除分类
function DltCategory(id) {
    var index = layer.load(2);
    var data = {
        id: id,
        type: "DltCategory"
    };
    var proc = "Proc_Admin_Category";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                QueryTwoCategoryList();
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
            layer.alert("加载失败", {
                icon: 2,
                skin: 'layer-ext-moon'
            })
        }
    });
}
//批量删除
function DltCategoryList() {
    var index = layer.load(2);
    ////获取删除分类Id
    var list = "";
    $(".text-c input[type=checkbox]:checked").each(function () { //由于复选框一般选中的是多个,所以可以循环输出选中的值  
        list = $(this).val() + "," + list;
    });
    if (list == "") {
        layer.close(index);
        return;
    }
    var data = {
        list: list,
        type: "DltCategoryList"
    };
    var proc = "Proc_Admin_Category";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                layer.msg("删除成功", { time: 3000 });
                QueryTwoCategoryList();
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
            layer.alert("加载失败", {
                icon: 2,
                skin: 'layer-ext-moon'
            })
        }
    });
}

//获取地址栏参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function tz() {
    window.location = "goods_Son_category_list.html"

}