
//查询新闻资讯
function QueryArticleMt(obj)
{
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var index = layer.load(2);
    var pageSize = "15";
    var dateMin = $("#dateMin").val();//开始日期
    var dateMax = $("#dateMax").val();//结束日期
    var status = $("#sltStatus").val();//状态
    var strWhere = $("#txtStrWhere").val();//搜索条件
    var type = $("#sltType").val;
    var data = {
        type:"GetArticle",
        dateMin: dateMin,
        dateMax: dateMax,
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "";//存储过程名
    var type = "QueryArticle";
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
                var obj=result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">';
                    html += '<td><input type="checkbox" value="" name=""></td>';
                    html += '<td>' + obj[i]["id"] + '</td>';
                    html += '<td>' + obj[i]["typeName"] + '</td>';
                    html += '<td><u style="cursor:pointer" class="text-primary" onClick="article_edit(\'文章编辑\',\'article_editor.html\',\'' + obj[i]["id"] + '\')" title="查看">' + obj[i]["title"] + '</u></td>';
                    html += '<td title=\"' + obj[i]["zhaiyao"] + '\"><div class=\"text-overflow\" style=\"width: 350px;\">' + obj[i]["zhaiyao"] + '</div></td>';
                    html += '<td>' + obj[i]["add_time"] + '</td>';
                    html += '<td>' + obj[i]["click"] + '</td>'; 
                    html += '<td class="td-status">';
                    if (obj[i]["status"] == "2") {
                        html += '<span class="label label-success radius">已上架</span>';
                    }
                    else{
                        html += '<span class="label label-warning radius">已下架</span>';
                    }
                    html += '</td>';
                    html += '<td class="f-14 td-manage">';
                    if (obj[i]["status"] == "1") {
                        html += '<a style="text-decoration:none" class="btn btn-success radius"  onClick="UpdateArticle(\'' + obj[i]["id"] + '\',\'2\')" href="javascript:;" title="审核">审核</a>';
                        html += '<a style="text-decoration:none;margin-left: 6px;" class="btn btn-secondary radius" onClick="article_edit(\'文章编辑\',\'article_editor.html\',\'' + obj[i]["id"] + '\')" href="javascript:;" title="编辑">编辑</a> ';
                        html += '<a style="text-decoration:none" class="btn btn-danger radius" onClick="UpdateArticle(\'' + obj[i]["id"] + '\',\'0\')" href="javascript:;" title="删除">删除</a>'
                    }
                    else if (obj[i]["status"] == "2")
                    {
                        html += '<a style="text-decoration:none" class="btn btn-warning radius" onClick="UpdateArticle(\'' + obj[i]["id"] + '\',\'1\')" href="javascript:;" title="下架">下架</a>';
                    }
                    html += '</td>';
                    html += '</tr>';
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
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
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
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
//新闻资讯审核
function UpdateArticle(id,status)
{
    //下架
    var msg = "";
    if (status == 1) {
        msg = "确认要下架吗？";
    }
    else if (status == 2) {
        msg = "确认要上架吗？";
    }
    else if (status = 0) {
        msg = "确认要删除吗？";
    } else {
        msg = "请稍后...";
    }
    layer.confirm(msg, function (index) {
        var index = layer.load(2);
        var data = {
            type:'upArticlestatus',
            id: id,
            status: status,
        };
        var proc = "";//存储过程名
        var type = "UpdateArticle";
        ///加载页面数据
        $.ajax({
            type: "Post",
            cache: false,
            url: "article/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var obj = data;
                var type = obj.flag;
                if (type == '0') {
                    layer.alert(obj.message, {
                        icon: 1,
                        skin: 'layer-ext-moon',
                        yes: function () { layer.closeAll(); QueryArticleMt(); }
                    });
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
    });
}
/*资讯-编辑*/
function article_edit(title, url, id, w, h) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url+"?id="+id
    });
    layer.full(index);
}
//文章编辑加载信息
function QueryArticleDt()
{
    var index = layer.load(2);
    var id = GetQueryString("id");
    var data = { id: id, type:'QueryArticleMt' };
    var proc = "";//存储过程名
    var type = "QueryArticleMt";
    $.ajax({
        type: "Post",
        cache: false,
        url: "article/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                var list = obj.data[0];
                $("#txtId").val(id);
                $("#txtTitle").val(list.title);
                $("#txtTags").val(list.tags);
                $("#sltType").val(list.category_id);
                $("#sltStatus").val(list.status);
                $("#txtSortId").val(list.sort_id);
                $("#dateTimeMax").val(list.add_time);
                $("#txtZhaiYao").val(list.zhaiyao);
                var ue = UE.getEditor('editor');
                // editor准备好之后才可以使用
                ue.addListener("ready", function () {
                    //赋值
                    ue.setContent(list.content);
                    ////取值
                    //var content = ue.getContent();
                });
                layer.close(index);
            }
            else {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes:function(){layer_close();}
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
            layer_close();
            
        }
    })
}
//批量删除
function DltArticleList() {
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
    };
    var proc = "";//存储过程名
    var type = "UpdateArticleList";
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
                QueryCategoryList();
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