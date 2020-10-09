///加载商品选项
function LoadJfFloor() {
    var index = layer.load(2);
    var data = {
        type: "GiftFloor",
    };
    var proc = "Proc_Admin_GiftList";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                ///加载商品分类
                var obj = result.data;
                for(var i=0;i<obj.length;i++)
                {
                    $("#sltCategory").append("<option value='" + obj[i]["id"] + "'>" + obj[i]["title"] + "</option>");
                }
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
            });
        }
    })
}
///查询商品资料
function GoodsGift(obj) {
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
    var status = $("#sltStatus").val();//礼品状态
    var strWhere = $("#txtStrWhere").val();//搜索条件
    var data = {
        type: "GiftList",
        strWhere: strWhere,
        status: status,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Admin_GiftList";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "Coupon/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += "<tr class='text-c'>";
                    html += "<td><input type='checkbox' value='" + obj[i]["goodsid"] + "' name='GiftList'></td>";
                    html += "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                    html += "<td>" + obj[i]["goodsid"] + "</td>";
                    html += "<td><u style='cursor:pointer' class='text-primary' onclick=\"GiftEnditOpen('" + obj[i]["goodsname"] + "','goods_Integral_editor.html','" + obj[i]["goodsid"] + "')\">" + obj[i]["goodsname"] + "</u></td>";
                    html += "<td>" + obj[i]["drug_spec"] + "</td>";
                    html += "<td>" + obj[i]["package_unit"] + "</td>";
                    html += "<td>" + obj[i]["drug_factory"] + "</td>";
                 
                    html += "<td>" + obj[i]["quantity"] + "</td>";
                    //html += "<td>" + obj[i]["integral"] + "</td>";
                    html += "<td>" + obj[i]["price"] + "</td>";
                    html += "<td>"
                    if (obj[i]["status"] == "1") {
                        html += "<a class=\"btn btn-danger-outline radius\"  class=\"ml-5\" style=\"text-decoration:none\">已下架</a>";
                    }
                    else{
                        html += "<a class=\"btn btn-success-outline radius\"  class=\"ml-5\" style=\"text-decoration:none\">上架中</a>";
                    }
                    html +=  "</td>";
                   
                    html += "<td class='td-manage'>";
                    var status = obj[i]["status"];
                    html += "<a title=\"编辑\" class=\"btn btn-warning radius\" onclick=\"GiftEnditOpen('" + obj[i]["goodsname"] + "','goods_Integral_editor.html','" + obj[i]["goodsid"] + "')\" class=\"ml-5\" style=\"text-decoration:none\">编辑</a>";
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
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//打开商品编辑页面
function GiftEnditOpen(title, url, id) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id
    });
    layer.full(index);
}


//礼品编辑页面加载信息
function LoadGiftsInfo() {
    var index = layer.load(2);
    var id = GetQueryString("id");
    var data = {
        type: "GiftDt",
        id: id
    };
    var proc = "Proc_Admin_GiftList";//存储过程名
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
                ///加载商品基础信息
                var obj = result.data;
                if (obj != "" && obj.length > 0) {
                    var list = obj[0];
                    new Vue({
                        el: '.page-container',
                        data: list
                    });

                    $("#is_prom").val(list["is_prom"]);
                    $("#is_jfsc").val(list["is_jfsc"]);
                    $("#sltCategory").val(list["flootId"]);
                }
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

///赠品信息存盘
function GiftSave() {
    var index = layer.load(2);
    var id = GetQueryString("id");
    var sort_id = $("#txtSortId").val();//商品排序
    var category_id = $("#sltCategory").val();//商品分类
    var sub_title = $("#txtGoodsName").val();//商品名称
    var mnemonic_code = $("#txtMnemonicCode").val();//助记码
    var drug_spec = $("#txtDrugSpec").val();//商品规格
    var package_unit = $("#txtPackageUnit").val();//包装单位
    var drug_factory = $("#txtDrugFactory").val();//生产厂家
    var price = $("#txtCkPrice").val();//建议零售价
    var integral = $("#txtIntegral").val();//积分
    var quantity = $("#txtQuantity").val();//库存
    var img_url = $("#txtGoodsImg").val();//封面图片
    var status = $("#sltIsShowId").val();//状态
    var is_prom = $("#is_prom").val();
    var is_jfsc = $("#is_jfsc").val();

    var data = {
        type:"GiftSave",
        id: id,
        category: category_id,
        goodsname: sub_title,
        sort_id: sort_id,
        mnemonic_code: mnemonic_code,
        drug_spec: drug_spec,
        package_unit: package_unit,
        drug_factory: drug_factory,
        price: price,
        img_url: img_url,
        status: status,
        integral: integral,
        quantity: quantity,
        is_prom: is_prom,
        is_jfsc: is_jfsc
    }
    var proc = "Proc_Admin_GiftList";//存储过程名
    var type = "UpdateGoodsInfo";
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
                layer.alert(result.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { layer_close(); }
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
            layer.alert("存盘失败", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
        }
    })
}