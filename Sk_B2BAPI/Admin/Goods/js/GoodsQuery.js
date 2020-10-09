///查询商品资料
function GoodsQuery(obj)
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
    var category = $("#sltCategory").val();//商品分类
    var attribute = $("#sltAttribute").val();//商品属性
    var status = $("#sltStatus").val();//商品状态
    var strWhere = $("#txtStrWhere").val();//搜索条件
    var promtype = $("#sltPromType").val();//专区类型
    var promstatus = $("#sltPromStatus").val();//状态
    var spStatus = $("#spStatus").val();//状态
    var data = {
        type: "GoodsList",
        category: category,
        attribute: attribute,
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
        promtype: promtype,
        promstatus: promstatus,
        spStatus: spStatus
    };
    var proc = "Proc_Admin_GoodsList";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        async: true,
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
                    html += "<tr class='text-c'>";
                    html += "<td><input type='checkbox' value='" + obj[i]["article_id"] + "' name='GoodsList'></td>";
                    html += "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                     //html = html + "<td ><div class=\"text-overflow\" style=\"width: 200px;\">" + obj["data"][i]["describe"] + "</div></td>"
                    html += "<td style='display:none'  title=\"" + obj[i]["entname"] + "\"><div class=\"text-overflow\" style=\"width: 80px;\">" + obj[i]["entname"] + "</div></td>";
                    html += "<td style='display:none' >" + obj[i]["article_id"] + "</td>";
                    html += "<td >" + obj[i]["goodscode"] + "</td>";
                    html += "<td class='td-manage'>";
                    var status = obj[i]["status"];
                    if (obj[i]["adminentid"] != 'superintendent') {
                        if (status == "1") {
                            html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"AuditGoods('" + obj[i]["article_id"] + "',2,1)\"  title=\"启用\" class=\"btn btn-success   radius\">上架</a>";
                            html += "<a title=\"编辑\"  onclick=\"GoodsEnditOpen('" + obj[i]["sub_title"] + "','goods_editor.html','" + obj[i]["article_id"] + "')\" class=\"btn btn-warning radius\" style=\"text-decoration:none;margin-right: 5px\">编辑</a>";
                            //html += "<a title=\"产品图管理\"  onclick=\"GoodsEnditOpen('" + obj[i]["sub_title"] + "','goods_imglist.html','" + obj[i]["article_id"] + "')\" class=\"btn btn-secondary  radius\" style=\"text-decoration:none\">产品图管理</a>";
                        }
                        else if (status == "2" || status == "3") {
                            html += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"AuditGoods('" + obj[i]["article_id"] + "',1,1)\"  title=\"停用\" class=\"btn btn-danger radius\">下架</a>";
                            html += "<a title=\"编辑\"  onclick=\"GoodsEnditOpen('" + obj[i]["sub_title"] + "','goods_editor.html','" + obj[i]["article_id"] + "')\" class=\"btn btn-warning radius\" style=\"text-decoration:none;margin-right: 5px\">编辑</a>";
                            //html += "<a title=\"产品图管理\"  onclick=\"GoodsEnditOpen('" + obj[i]["sub_title"] + "','goods_imglist.html','" + obj[i]["article_id"] + "')\" class=\"btn btn-secondary radius\" style=\"text-decoration:none\">产品图管理</a>";
                        }
                    }
                    html += "</td>";
                    html += "<td>" + obj[i]["sub_title"] + "</td>";//<u style='cursor:pointer' class='text-primary' onclick=\"GoodsEnditOpen('" + obj[i]["sub_title"] + "','goods_editor.html','" + obj[i]["article_id"] + "')\"></u>
                    html += "<td>" + obj[i]["drug_spec"] + "</td>";
                    html += "<td>" + obj[i]["package_unit"] + "</td>";
                    html += "<td>" + obj[i]["drug_factory"] + "</td>";
                    html += "<td>" + obj[i]["category"] + "</td>";
                    html += "<td>" + obj[i]["stock_quantity"] + "</td>";
                    html += "<td>" + obj[i]["price"] + "</td>";
                    html += "<td>" + obj[i]["valdate"] + "</td>";
                    html += "<td>" + obj[i]["Remarks"] + "</td>";
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
            if (type == '4') {
                $('body').html('<img src="../UploadFile/素材/权限不足.png" style="width:75%"/>');
            }
            $('body').removeAttr("style");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//打开商品资料编辑页面
function GoodsEnditOpen(title,url,id) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url+"?article_id="+id
    });
    layer.full(index);
}
//商品资料状态修改
function AuditGoods(article_id, status, type)//type 1 单用 2 批量
{
    if (status != 2 && status != 1) {
        layer.alert('方法执行异常！', {
            icon: 2,
            skin: 'layer-ext-moon',
            yes: function () { layer.closeAll(); }
        });
        return;
    }
    var remarks = '';
    if (type == 2) {
        var s = '';
        $('input[name="GoodsList"]:checked').each(function () {
            s += $(this).val() + ',';
        });
        if (s.length > 0) {
            //得到选中的checkbox值序列 
            s = s.substring(0, s.length - 1);
        }
        if (s == '') {
            alert('你还没有选择任何内容！')
            return;
        } else {
            article_id = s;
        }
    }
    var person = prompt("请输入上下架备注信息", "常规操作");
    if (person != null && person != "") {
        remarks = person;
    } 
    var index = layer.load(2);
    var proc = "Proc_Admin_GoodsList";//存储过程名
    var type = "ReturnNumber";
    var json = {
        type: "AuditGoods",
        article_id: article_id,
        status: status,
        remarks: remarks
    };
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                GoodsQuery();
            }
            else {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
        }
    })
}