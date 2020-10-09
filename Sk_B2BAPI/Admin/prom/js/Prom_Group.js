///商品组合选择商品信息
function QueryGoods() {
    var strWhere = $("#txtGoodsName").val();
    layer_show("选择商品", "SearchInfo.html?type=groups&proc=Proc_Admin_SearchInfo&sqlType=GoodsList&strWhere=" + encodeURI(strWhere), 1000, 600);
}
//添加组合商品
function AddGroupGoods() {
    //var index = layer.load(2);
    //var sort_Id = $("#tdSort_Id").html();
    var article_Id = $("#txtArticleId").val();
    var article_Name = $("#txtGoodsName").val();

    if (article_Id == '') {
        layer.msg("请选择商品！", { time: 3000 });
        layer.close(index);
        return;
    }
    var fa_type = $("#sltFaType").val();
    var quantity = $("#txtGoodsQuantity").val();
    var totalQuantity = $("#txtTotalQuantity").val();//抢购促销总数
    var limitQuantity = $("#txtlimitQuantity").val();//抢购至少购买数量
    var onlyQuantity = $("#txtOnlyQuantity").val();//客户限购
    switch (fa_type) {
        case 'DQG':
            var totalQuantity = $("#txtTotalQuantity").val();//抢购促销总数
            var onlyQuantity = $("#txtOnlyQuantity").val();//客户限购
            if (totalQuantity == '') {
                layer.msg("抢购商品参与活动数量格式不正确！", { time: 3000 });
                layer.close(index);
                return;
            }
            if (onlyQuantity == '') {
                layer.msg("客户限购数量格式不正确！", { time: 3000 });
                layer.close(index);
                return;
            }
            break;
        case 'GZH':
            if (quantity == '' || quantity <= 0) {
                layer.msg("请输入套餐商品的数量！", { time: 3000 });
                layer.close(index);
                return;
            }
            break;
    }
    var price = $("#txtGoodsPrice").val();
    if (price <= 0) {
        layer.msg("请正确输入商品价格！", { time: 3000 });
        layer.close(index);
        return;
    }
    var len = $("#TbGoods").children("tr").length;
    var str = "";
    str += "<tr class=\"text-c\">";
    str += "        <td>" + len + "</td>";
    str += "        <td>" + $("#tdGoodsCode").html() + "</td>";
    str += "        <td>";
    str += "            <span  class=\"select-box\" style=\"text-align:left\">" + article_Name + "</span>";
    str += "        <input type=\"hidden\" value=\"" + article_Id + "\">";
    str += "        </td>";
    str += "        <td>" + $("#tdDrugSpec").html() + "</td>";
    str += "        <td>" + $("#tdMinPackage").html() + "</td>";
    str += "        <td>" + $("#tdPrice").html() + "</td>";
    str += "        <td>" + $("#tdStock_Quantity").html() + "</td>";
    //str += "        <td><span  class=\"select-box\">" + quantity+"</span></td>";
    str += "        <td class=\"zhcyshl\"><span  class=\"select-box\" style=\"text-align:left\">" + quantity + "</span></td>";
    str += "        <td><span  class=\"select-box\" style=\"text-align:left\">" + price + "</span></td>";
    str += "        <td class=\"qgcyshl\"><span  class=\"select-box\" style=\"text-align:left\">" + totalQuantity + "</span></td>";
    str += "        <td class=\"qgmzshl\"><span  class=\"select-box\" style=\"text-align:left\">" + limitQuantity + "</span></td>";
    str += "        <td class=\"qgxgshl\"><span  class=\"select-box\" style=\"text-align:left\">" + onlyQuantity + "</span></td>";
    str += "        <td> <a onclick=\"DeleteGroupGoods(this)\" class=\"btn btn-danger radius\">删除</a></td>";
    str += " </tr >";
    $("#TbGoods").append(str);
    bianhua();


    $("#tdGoodsCode").html("");
    $("#txtGoodsName").val("");
    $("#txtArticleId").val("");
    $("#tdDrugSpec").html("");
    $("#tdMinPackage").html("");
    $("#txtGoodsQuantity").val("");
    $("#txtGoodsPrice").val("");
    $("#tdTaxAmount").html("");
    $("#tdPrice").html("");
    $("#tdStock_Quantity").html("");
    $("#txtTotalQuantity").val("");
    $("#txtlimitQuantity").val("");
    $("#txtOnlyQuantity").val("");
    //var lsid = $("#lsid").html();
    //var data = {
    //    type: "AddMeetCount",
    //    sort_Id: sort_Id,
    //    faType: fa_type,
    //    goodsid: article_Id,
    //    quantity: quantity,
    //    totalQuantity: totalQuantity,
    //    onlyQuantity: onlyQuantity,
    //    limitQuantity:limitQuantity,
    //    price: price,
    //    lsid: lsid
    //};
    //var type = "ReturnNumber";
    //var proc = "Proc_Admin_PromUpdate"
    //$.ajax({
    //    url: "prom/ashx/ReturnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
    //    type: "Post",
    //    chche: false,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        var obj = data;
    //        var status = obj.flag;
    //        if (status == 0) {
    //            layer.close(index);
    //            QueryGroupGoods();
    //        }
    //        else {
    //            layer.close(index);
    //            layer.alert(obj.message, {
    //                icon: 2,
    //                skin: 'layer-ext-moon'
    //            });
    //        }
    //    },
    //    error: function (jqXHR, textStatus, errorThrown) {
    //        layer.close(index);
    //        alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

    //    }
    //})
}

function DeleteGroupGoods(obj) {
    $(obj).parent().parent().remove();
    var num = 0;
    $("#TbGoods").children("tr").each(function () {
        $(this).children().eq(0).text(num++);
    });
}

//查询组合商品
function QueryGroupGoods() {
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
        url: "prom/ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
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
                    html += '<td>' + obj["data"][i]["stock_quantity"] + '</td>';
                    html += '<td>' + obj["data"][i]["totalQuantity"] + '</td>';
                    html += '<td>' + obj["data"][i]["limitQuantity"] + '</td>';
                    html += '<td>' + obj["data"][i]["onlyQuantity"] + '</td>';
                    html += '<td><a onclick="DltGroupGoods(\'' + i + '\')" class="btn btn-danger radius">删除</a></td>';
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
                html += '<td><input type="text" id="txtTotalQuantity" value="0" class="input-text" /></td>';
                html += '<td><input type="text" id="txtlimitQuantity" value="0" class="input-text" /></td>';
                html += '<td><input type="text" id="txtOnlyQuantity" value="0" class="input-text" /></td>';
                html += '<td><a onclick="AddGroupGoods()" class="btn btn-secondary radius">保存</a></td>';
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
                html += '<td><input type="text" id="txtTotalQuantity" value="0" class="input-text" /></td>';
                html += '<td><input type="text" id="txtlimitQuantity" value="0" class="input-text" /></td>';
                html += '<td><input type="text" id="txtOnlyQuantity" value="0" class="input-text" /></td>';
                html += '<td><a onclick="AddGroupGoods()" class="btn btn-secondary radius">保存</a></td>';
                html += '</tr>';
                $('#TbGoods').empty();
                $("#TbGoods").append(html);
                layer.close(index);
            }
            if (type == '2') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { location.replace("login.html") }
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
function DltGroupGoods(i) {
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
        url: "prom/ashx/ReturnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
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
    if (FaTitle == '') {
        layer.msg("请填写方案名称！", { time: 3000 });
        layer.close(index);
        return;
    }
    var fabs = $("#txtFabs").val();//方案标识
    if (fabs == '') {
        layer.msg("请填写方案标识！", { time: 3000 });
        layer.close(index);
        return;
    }
    //var showFloor = $("#sltFaType").val();//显示位置
    var khType = "";//客户类型
    $("#khTypeId").children("input:checked").each(function () {
        khType += $(this).val() + ",";
    });
    if (khType == '') {
        layer.msg("请选择客户类型！", { time: 3000 });
        layer.close(index);
        return;
    }
    //var startDates = $("#datemin").val();//开始时间
    //if (startDates == '') {
    //    layer.msg("开始时间未填写！", { time: 3000 });
    //    layer.close(index);
    //    return;
    //}
    //var endDates = $("#datemax").val();//结束时间
    //if (endDates == '') {
    //    layer.msg("结束时间未填写！", { time: 3000 });
    //    layer.close(index);
    //    return;
    //}
    //var isShow = $('input[name="isShow"]:checked').val();//是否显示
    var describe = $("#txtdescribe").val();//促销描述
    var faType = $("#sltFaType").val();//促销模式
    if (faType == '') {
        layer.msg("促销模式不能为空！", { time: 3000 });
        layer.close(index);
        return;
    }
    var promQuantity = $("#txtPromQuantity").val();//促销总数
    var saleQuantity = $("#txtSaleQuantity").val();//客户限购
    
    switch (faType) {
        case 'DQG':
            break;
        case 'GZH':
            if (promQuantity == '') {
                layer.msg("促销总数数据格式不正确！", { time: 3000 });
                layer.close(index);
                return;
            }
            if (saleQuantity == '') {
                layer.msg("客户限购数据格式不正确！", { time: 3000 });
                layer.close(index);
                return;
            }
            break;
    }
    var lsid = "[";
    $("#TbGoods").children("tr").each(function () {
        var num = $(this).children().eq(0).text();
        if (num != 0) {
            var str = "{\"GoodsId\":\"" + $(this).children().eq(2).children("input").eq(0).val() + "\",";
            str += "\"shl\":\"" + $(this).children().eq(7).children("span").eq(0).text() + "\",";
            str += "\"Price\":\"" + $(this).children().eq(8).children("span").eq(0).text() + "\",";
            str += "\"Total\":\"" + $(this).children().eq(9).children("span").eq(0).text() + "\",";
            str += "\"MeetCount\":\"" + $(this).children().eq(10).children("span").eq(0).text() + "\",";
            str += "\"Limit\":\"" + $(this).children().eq(11).children("span").eq(0).text() + "\"}";
            lsid += str + ",";
        }
    });
    lsid += "]";
    console.log(lsid);
    if (lsid == '') {
        layer.msg("请先填写促销满足条件", { time: 3000 });
        layer.close(index);
        return;
    }
    var ruleCode = $("#ruleCode").val();
    var data = {
        type: "SaveProm",
        faname: FaTitle,
        ruleCode: ruleCode,
        //fabs: fabs,
        faType: faType,
        customerType: khType,
        //startDates: startDates,
        //endDates: endDates,
        //is_show: isShow,
        describe: describe,
        //contentType: faType,
        //goodsid: "",
        Quantity: promQuantity,
        xgAmount: saleQuantity,
        lsid: lsid,
    };
    //var type = "ReturnNumber";
    //var proc = "Proc_Admin_PromUpdate"
    $.ajax({
        url: "prom/ashx/PromGroupRule.ashx?json=" + encodeURIComponent(JSON.stringify(data)),// + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
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
                    yes: function () { parent.document.getElementById("searchBtnA").click(); layer_close(); }
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