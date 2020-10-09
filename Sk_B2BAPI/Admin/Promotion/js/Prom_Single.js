///选择商品信息
function QueryGoods() {
    var strWhere = $("#txtGoodsCode").val();
    layer_show("选择商品", "SearchInfo.html?type=single&proc=Proc_Admin_SearchInfo&sqlType=GoodsList&strWhere=" + encodeURI(strWhere), 1000, 600);
}
///加载促销满足条件
function LoadMeetCount() {
    var faType = $("#sltFaType").val();
    if (faType == "DZK" || faType == "DQG" || faType == "DXQ") {
        document.getElementById("txtDiscount").readOnly = false;
        document.getElementById("txtGiftName").readOnly = true
        document.getElementById("txtGiftId").readOnly = true
        document.getElementById("txtGiftQuantity").readOnly = true
        document.getElementById("txtGiftPrice").readOnly = true
    }
    else {
        document.getElementById("txtDiscount").readOnly = true;
        $("#txtDiscount").val(100);
        document.getElementById("txtGiftName").readOnly = false;
        document.getElementById("txtGiftId").readOnly = false;
        document.getElementById("txtGiftQuantity").readOnly = false;
        document.getElementById("txtGiftPrice").readOnly = false;
    }
}
//选择商品或赠品
function ChoseGift() {
    var faType = $("#sltFaType").val();
    var strWhere = $("#txtGiftName").val();
    if (faType == "DMZ") {
        layer_show("选择赠品", "SearchInfo.html?type=gift&proc=Proc_Admin_SearchInfo&sqlType=GoodsListMZ&strWhere=" + encodeURI(strWhere), 1000, 600);
    }
    else if (faType == "DHG") {
        layer_show("选择赠品", "SearchInfo.html?type=gift&proc=Proc_Admin_SearchInfo&sqlType=GoodsListHG&strWhere=" + encodeURI(strWhere), 1000, 600);
    }
}
///保存促销满足条件
function AddMeetCount()
{
    var index = layer.load(2);
    var sort_Id = $("#tdSort_Id").html();
    var lsid = $("#lsid").html();
    var faType = $("#sltFaType").val();
    var meetCount = $("#txtMeetCount").val();
    var discount = $("#txtDiscount").val();
    var giftId = $("#txtGiftId").val();
    var giftQuantity = $("#txtGiftQuantity").val();
    var giftPrice = $("#txtGiftPrice").val();
    if ((faType == "DHG" || faType == "DMZ") && giftId=="")
    {
        layer.close(index);

        layer.alert("请选择赠品", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    if ((faType == "DHG" || faType == "DMZ") && giftQuantity <= 0)
    {
        layer.close(index);

        layer.alert("赠品数量不能小于0", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var data = {
        type: "AddMeetCount",
        sort_Id:sort_Id,
        faType: faType,
        meetCount: meetCount,
        discount: discount,
        goodsid: giftId,
        quantity: giftQuantity,
        price: giftPrice,
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
                QueryMeetCount();
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
//删除促销满足条件
function DltMeetCount(i)
{
    var index = layer.load(2);
    var sort_Id = $("#tdSort_Id"+i+"").html();
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
                QueryMeetCount();
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
//查询满足条件
function QueryMeetCount()
{
    var index = layer.load(2);
    var lsid = $("#lsid").html();
    var index = layer.load(2);
    var data = {
        type: "QueryMeetCount",
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
                var tb = "";
                for (var i = 0; i < obj["data"].length; i++) {
                    tb = tb + "<tr class='text-c'>";
                    tb = tb + '<td id="xtdSortId' + i + '">' + obj["data"][i]["sort_Id"] + '</td>';
                    tb = tb + "<td>" + obj["data"][i]["fabs"] + "</td>";
                    tb = tb + "<td>" + obj["data"][i]["meetCount"] + "</td>";
                    tb = tb + "<td>" + obj["data"][i]["discount"] + "</td>";
                    tb = tb + "<td>" + obj["data"][i]["giftname"] + "</td>";
                    tb = tb + "<td>" + obj["data"][i]["giftquantity"] + "</td>";
                    tb = tb + "<td>" + obj["data"][i]["giftprice"] + "</td>";
                    tb = tb + '<td><a onclick="DltMeetCount(\'' + i + '\')">删除</a></td>';
                    tb = tb + "</tr>";
                }
                tb = tb + "<tr class='text-c'>";
                tb = tb + "<td id='tdSort_Id'>" + (obj["data"].length +1)+ "</td>";
                tb = tb + "<td><span class='select-box'>";
                tb = tb + "<select class='select' id='sltFaType' onchange='LoadMeetCount()'>";
                tb = tb + "<option value='DMZ'>满赠</option>";
                tb = tb + "<option value='DHG'>换购</option>";
                tb = tb + "<option value='DZK'>折扣</option>";
                tb = tb + "<option value='DQG'>抢购</option>";
                tb = tb + "<option value='DXQ'>效期</option>";
                tb = tb + "</select></span></td>";
                tb = tb + "<td><input type='text' id='txtMeetCount' value='1' class='input-text'/></td>";
                tb = tb + "<td><input type='text' id='txtDiscount' value='100' class='input-text' readonly='readonly'/></td>";
                tb = tb + "<td><input type='text' id='txtGiftName' value='' class='input-text' ondblclick='ChoseGift()'/>";
                tb = tb + "<input type='hidden' id='txtGiftId' value='' class='input-text'/></td>";
                tb = tb + "<td><input type='text' id='txtGiftQuantity' value='0' class='input-text'/></td>";
                tb = tb + "<td><input type='text' id='txtGiftPrice' value='0' class='input-text'/></td>";
                tb = tb + "<td><a onclick='AddMeetCount()'>存盘</a></td></tr>";
                $('#TbMeetCount').empty();
                $("#TbMeetCount").append(tb);
                layer.close(index);
            }
            if (type == '1') {
                var tb = "";
                tb = tb + "<tr class='text-c'>";
                tb = tb + "<td id='tdSort_Id'>1</td>";
                tb = tb + "<td><span class='select-box'>";
                tb = tb + "<select class='select' id='sltFaType' onchange='LoadMeetCount()'>";
                tb = tb + "<option value='DMZ'>满赠</option>";
                tb = tb + "<option value='DHG'>换购</option>";
                tb = tb + "<option value='DZK'>折扣</option>";
                tb = tb + "<option value='DQG'>抢购</option>";
                tb = tb + "<option value='DXQ'>效期</option>";
                tb = tb + "</select></span></td>";
                tb = tb + "<td><input type='text' id='txtMeetCount' value='1' class='input-text'/></td>";
                tb = tb + "<td><input type='text' id='txtDiscount' value='100' class='input-text' readonly='readonly'/></td>";
                tb = tb + "<td><input type='text' id='txtGiftName' value='' class='input-text' ondblclick='ChoseGift()'/>";
                tb = tb + "<input type='hidden' id='txtGiftId' value='' class='input-text'/></td>";
                tb = tb + "<td><input type='text' id='txtGiftQuantity' value='0' class='input-text'/></td>";
                tb = tb + "<td><input type='text' id='txtGiftPrice' value='0' class='input-text'/></td>";
                tb = tb + "<td><a onclick='AddMeetCount()'>存盘</a></td></tr>";
                $('#TbMeetCount').empty();
                $("#TbMeetCount").append(tb);
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
//保存促销方案
function SavePromDesign() {
    var index = layer.load(2);
    var FaTitle = $("#txtFaTitle").val();//方案名称
    var fabs = $("#txtFabs").val();//方案标识
    var showFloor = $("#sltShowFloor").val();//显示位置
    var khType = $("#khTypeId").val();//客户类型
    var startDates = $("#datemin").val();//开始时间
    var endDates = $("#datemax").val();//结束时间
    var isShow = $('input[name="isShow"]:checked').val();//是否显示
    var dscribe = $("#txtDdscribe").val();//促销描述
    //var contentType = $("#contentTypeId").val();//促销模式
    var JieTi = $("#contentJieTi").val();//促销模式
    var ArticleId = $("#txtArticleId").val();//促销商品
    var promQuantity = $("#txtPromQuantity").val();//促销总数
    var saleQuantity = $("#txtSaleQuantity").val();//客户限购
    var contentType = $("#contentType").val();//促销模式
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
        goodsid: ArticleId,
        Quantity: promQuantity,
        xgAmount: saleQuantity,
        contentType: contentType,
        lsid: lsid,
        JieTi: JieTi
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
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { window.location.reload(); }
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