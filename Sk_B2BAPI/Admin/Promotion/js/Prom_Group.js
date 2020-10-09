///商品组合选择商品信息
function QueryGoods() {
    var strWhere = $("#txtGoodsName").val();
    layer_show("选择商品", "SearchInfo.html?type=groups&proc=Proc_Admin_SearchInfo&sqlType=GoodsList&strWhere=" + encodeURI(strWhere), 1000, 600);
}
//添加组合商品
function AddGroupGoods()
{
    var index = layer.load(2);
    var sort_Id = $("#tdSort_Id").html();
    var article_Id = $("#txtArticleId").val();
    var quantity = $("#txtGoodsQuantity").val();
    var price = $("#txtGoodsPrice").val();
    var lsid = $("#lsid").html();

    if (article_Id == "")
    {
        layer.close(index);

        layer.alert("请选择商品！", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }

    if (quantity <= 0)
    {
        layer.close(index);

        layer.alert("数量不能小于0", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }

    if (price <= 0)
    {
        layer.close(index);

        layer.alert("价格不能小于0", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var data = {
        type: "AddMeetCount",
        sort_Id: sort_Id,
        faType: "GZH",
        goodsid: article_Id,
        quantity: quantity,
        price: price,
        lsid: lsid
    };
    var type = "ReturnNumber";
    var proc = "Proc_Admin_PromUpdate"
    $.ajax({
        url: "Promotion/ashx/ReturnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        type: "Post",
        chche: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var status = obj.flag;
            if (status == 0) {
                layer.close(index);
                QueryGroupGoods();
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
            layer.close(index);
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}
//查询组合商品
function QueryGroupGoods()
{
    var index = layer.load(2);
    var lsid = $("#lsid").html();
    var index = layer.load(2);
    var data = {
        type: "QueryGroupGoods",
        lsid: lsid,
    };
    var proc = "Proc_Admin_PromUpdate";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "Promotion/ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                var html = "";
                for (var i = 0; i < obj["data"].length; i++) {
                    html += '<tr class="text-c">';
                    html += '<td id="xtdSortId' + i + '">' + obj["data"][i]["sort_Id"] + '</td>';
                    html += '<td>' + obj["data"][i]["goodscode"] + '</td>';
                    html += '<td>' + obj["data"][i]["sub_title"] + '</td>';
                    html += '<td>' + obj["data"][i]["drug_spec"] + '</td>';
                    html += '<td>' + obj["data"][i]["min_package"] + '</td>';
                    html += '<td>' + obj["data"][i]["giftquantity"] + '</td>';
                    html += '<td>' + obj["data"][i]["giftprice"] + '</td>';
                    html += '<td>' + obj["data"][i]["taxAmount"] + '</td>';
                    html += '<td>' + obj["data"][i]["price_ck"] + '</td>';
                    html += '<td>' +obj["data"][i]["stock_quantity"]+ '</td>';
                    html += '<td><a onclick="DltGroupGoods(\'' + i + '\')">删除</a></td>';
                    html += '</tr>';
                }
                html += '<tr class="text-c">';
                html += '<td id="tdSort_Id">' + (obj["data"].length + 1) + '</td>';
                html += '<td id="tdGoodsCode"></td>';
                html += '<td><input type="text" id="txtGoodsName" value="" class="input-text" ondblclick="QueryGoods()"/> <input type="hidden" id="txtArticleId" value=""> </td>';
                html += '<td id="tdDrugSpec"></td>';
                html += '<td id="tdMinPackage"></td>';
                html += '<td><input type="text" id="txtGoodsQuantity" value="0" class="input-text"/></td>';
                html += '<td><input type="text" id="txtGoodsPrice" value="0" class="input-text"/></td>';
                html += '<td id="tdTaxAmount"></td>';
                html += '<td id="tdPrice"></td>';
                html += '<td id="tdStock_Quantity"></td>';
                html += '<td><a onclick="AddGroupGoods()">保存</a></td>';
                html += '</tr>';
                $('#TbGoods').empty();
                $("#TbGoods").append(html);
                layer.close(index);
            }
            if (type == '1') {
                var html = "";
                html += '<tr class="text-c">';
                html += '<td id="tdSort_Id">1</td>';
                html += '<td id="tdGoodsCode"></td>';
                html += '<td><input type="text" id="txtGoodsName" value="" class="input-text" ondblclick="QueryGoods()"/> <input type="hidden" id="txtArticleId" value=""> </td>';
                html += '<td id="tdDrugSpec"></td>';
                html += '<td id="tdMinPackage"></td>';
                html += '<td><input type="text" id="txtGoodsQuantity" value="0" class="input-text"/></td>';
                html += '<td><input type="text" id="txtGoodsPrice" value="0" class="input-text"/></td>';
                html += '<td id="tdTaxAmount"></td>';
                html += '<td id="tdPrice"></td>';
                html += '<td id="tdStock_Quantity"></td>';
                html += '<td><a onclick="AddGroupGoods()">保存</a></td>';
                html += '</tr>';
                $('#TbGoods').empty();
                $("#TbGoods").append(html);
                layer.close(index);
            }
            if (type == '2') {
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
//删除组合商品
function DltGroupGoods(i)
{
    var index = layer.load(2);
    var sort_Id = $("#xtdSortId" + i + "").html();
    var lsid = $("#lsid").html();
    var data = {
        type: "DltMeetCount",
        sort_Id: sort_Id,
        lsid: lsid
    };
    var type = "ReturnNumber";
    var proc = "Proc_Admin_PromUpdate"
    $.ajax({
        url: "Promotion/ashx/ReturnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        type: "Post",
        chche: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var status = obj.flag;
            if (status == 0) {
                layer.close(index);
                QueryGroupGoods();
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
            layer.close(index);
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}
//保存促销方案
function SaveGroupDesign() {
    var index = layer.load(2);
    var FaTitle = $("#txtFaTitle").val();//方案名称
    var fabs = $("#txtFabs").val();//方案标识
    var showFloor = $("#sltFaType").val();//显示位置
    var khType = $("#khTypeId").val();//客户类型
    var startDates = $("#datemin").val();//开始时间
    var endDates = $("#datemax").val();//结束时间
    //var reg = /^[1-9]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])\s+(20|21|22|23|[0-1]\d):[0-5]\d:[0-5]\d$/;
    //var regExp = new RegExp(reg);
    //if (!regExp.test(startDates)) {
    //    alert("开始时间格式不正确,正确格式为: 2020-01-01 12:00:00 ");
    //    return;
    //}
    //if (!regExp.test(endDates)) {
    //    alert("结束时间格式不正确,正确格式为: 2020-01-01 12:00:00 ");
    //    return;
    //}
    var isShow = $('input[name="isShow"]:checked').val();//是否显示
    var dscribe = $("#txtDdscribe").val();//促销描述
    var contentType = "0";//促销模式
    var promQuantity = $("#txtPromQuantity").val();//促销总数
    var saleQuantity = $("#txtSaleQuantity").val();//客户限购
    var contentType = $("#contentTypeId").val();//促销模式
    var lsid = $("#lsid").html();
    var data = {
        type: "SaveProm",
        faname: FaTitle,
        fabs: fabs,
        faType: showFloor,
        customerType: khType,
        startDates: startDates,
        endDates: endDates,
        is_show: isShow,
        dscribe: dscribe,
        contentType: contentType,
        goodsid: "",
        Quantity: promQuantity,
        xgAmount: saleQuantity,
        contentType: contentType,
        lsid: lsid,
    };
    var type = "ReturnNumber";
    var proc = "Proc_Admin_PromUpdate"
    $.ajax({
        url: "Promotion/ashx/ReturnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        type: "Post",
        chche: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var status = obj.flag;
            if (status == 0) {
                layer.close(index);
                layer.alert("存盘成功", {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes:function(){ window.location.reload();}
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
            layer.close(index);
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}