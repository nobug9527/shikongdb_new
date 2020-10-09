
///加载促销满足条件
function LoadMeetCount() {
    $("#TbMeetCount").children("tr").each(function () {
        var num = $(this).children().eq(0).text();

        if (num != "0") {
            $(this).remove();
        }
    });
    MeetCountDisplayChange();
}

function MeetCountDisplayChange() {
    var faType = $("#sltShowFloor").val();//促销类型
    $("#mzshljdj").hide();
    if (faType == "DZK") {
        document.getElementById("txtJieTi").readOnly = false
        document.getElementById("txtDiscount").readOnly = false;
        document.getElementById("txtGiftName").readOnly = true
        document.getElementById("txtGiftId").readOnly = true
        document.getElementById("txtGiftQuantity").readOnly = true
        document.getElementById("txtGiftPrice").readOnly = true
        $(".zk").show();
        $(".zp").hide();
        $("#txtJieTi").show();
        $(".zpslorhgsl").hide();
        $(".zpjgormjjg").hide();
    } else if (faType == "DHG" || faType == "DMZ") {
        if (faType == "DHG") {
            $("#hgorzptitle").html("换购商品");
        } else if (faType == "DMZ") {
            $("#hgorzptitle").html("赠品");
        }
        document.getElementById("txtJieTi").readOnly = true
        document.getElementById("txtDiscount").readOnly = true;
        document.getElementById("txtGiftName").readOnly = false
        document.getElementById("txtGiftId").readOnly = false
        document.getElementById("txtGiftQuantity").readOnly = false
        document.getElementById("txtGiftPrice").readOnly = false
        $(".zk").hide();
        $(".zp").show();
        $("#txtJieTi").show();
        $(".zpslorhgsl").show();
        $(".zpjgormjjg").show();
    } else if (faType == "DMJ") {
        $("#txtJieTi").show();
        document.getElementById("txtDiscount").readOnly = true;
        document.getElementById("txtGiftName").readOnly = true
        document.getElementById("txtGiftId").readOnly = true
        document.getElementById("txtGiftQuantity").readOnly = true
        document.getElementById("txtGiftPrice").readOnly = false
        $(".zp").hide();
        $(".zk").hide();
        $(".zpslorhgsl").hide();
        $(".zpjgormjjg").show();
        $("#mzshljdj").show();
    }
    else {
        layer.msg("请选择正确的促销类型！", { time: 3000 });
        layer.close(index);
        return;
    }
}

///保存促销满足条件
function AddMeetCount() {

    var JieTi = $('input[name="contentJieTi"]:checked').val();
    var len = $("#TbMeetCount").children("tr").length;
    if (JieTi == "0" && len > 1) {
        layer.msg("非阶梯递增模式下，仅能输入一个满足条件！", { time: 3000 });
        return;
    }

    var index = layer.load(2);
    var faType = $("#sltShowFloor").val();//促销类型
    if (faType == '') {
        layer.msg("请选择促销类型！", { time: 3000 });
        layer.close(index);
        return;
    }
    var meetCount = $("#txtMeetCount").val();
    var discount = $("#txtDiscount").val();
    var giftId = $("#txtGiftId").val();
    var giftQuantity = $("#txtGiftQuantity").val();
    var giftPrice = $("#txtGiftPrice").val();
    var txtGiftName = $("#txtGiftName").val();
    if ((faType == "DHG" || faType == "DMZ") && giftId == "") {
        layer.close(index);

        layer.alert("请选择赠品", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    if ((faType == "DHG" || faType == "DMZ") && giftQuantity <= 0) {
        layer.close(index);

        layer.alert("赠品数量不能小于0", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }

    var tb = "";
    tb = tb + "<tr class='text-c'>";
    tb = tb + '<td width="30px" id="xtdSortId' + len + '">' + len + '</td>';
    tb = tb + "<td class=\"mztj\"><span class=\"select-box\" style=\"text-align:left\">" + meetCount + "</span></td>";
    tb = tb + "<td class=\"zk\"><span class=\"select-box\" style=\"text-align:left\">" + discount + "</span></td>";
    tb = tb + "<td class=\"zp\"><span class=\"select-box\" style=\"text-align:left\">" + txtGiftName + "</span><span style=\"display:none\">" + giftId + "</span></td>";
    tb = tb + "<td class=\"zpslorhgsl\"><span class=\"select-box\" style=\"text-align:left\">" + giftQuantity + "</span></td>";
    tb = tb + "<td class=\"zpjgormjjg\"><span class=\"select-box\" style=\"text-align:left\">" + giftPrice + "</span></td>";
    tb = tb + '<td width=\"100px\"><a onclick="DltMeetCount(this)" class="btn btn-danger radius">删除</a></td>';
    tb = tb + "</tr>";
    $("#TbMeetCount").append(tb);

    $("#txtMeetCount").val("0");
    $("#txtDiscount").val("100");
    $("#txtGiftId").val("");
    $("#txtGiftQuantity").val("0");
    $("#txtGiftPrice").val("0");
    $("#txtGiftName").val("");
    MeetCountDisplayChange();
    layer.close(index);

}
//删除促销满足条件
function DltMeetCount(obj) {
    $(obj).parent().parent().remove();
    var num = 0;
    $("#TbMeetCount").children("tr").each(function () {
        $(this).children().eq(0).text(num++);
    });

}
//查询满足条件
function QueryMeetCount() {
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
        url: "prom/ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                var tb = "";
                for (var i = 0; i < obj["data"].length; i++) {
                    tb = tb + "<tr class='text-c'>";
                    tb = tb + '<td width="30px" id="xtdSortId' + i + '">' + obj["data"][i]["sort_Id"] + '</td>';
                    tb = tb + "<td class=\"mztj\">" + obj["data"][i]["meetCount"] + "</td>";
                    tb = tb + "<td class=\"zk\">" + obj["data"][i]["discount"] + "</td>";
                    tb = tb + "<td class=\"zp\">" + obj["data"][i]["giftname"] + "</td>";
                    tb = tb + "<td class=\"zpslorhgsl\">" + obj["data"][i]["giftquantity"] + "</td>";
                    tb = tb + "<td class=\"zpjgormjjg\">" + obj["data"][i]["giftprice"] + "</td>";
                    tb = tb + '<td width=\"100px\"><a onclick="DltMeetCount(\'' + i + '\')" class="btn btn-danger radius">删除</a></td>';
                    tb = tb + "</tr>";
                }
                tb = tb + "<tr class='text-c'>";
                tb = tb + "<td id='tdSort_Id'>" + (obj["data"].length + 1) + "</td>";
                tb = tb + "<td  class=\"mztj\"><input type='text' id='txtMeetCount' value='1' class='input-text'/></td>";
                tb = tb + "<td  class=\"zk\"><input type='text' id='txtDiscount' value='100' class='input-text' readonly='readonly'/></td>";
                tb = tb + "<td  class=\"zp\"><input type='text' id='txtGiftName' value='' class='input-text' ondblclick='ChoseGift()'/>";
                tb = tb + "<input type='hidden' id='txtGiftId' value='' class='input-text'/></td>";
                tb = tb + "<td  class=\"zpslorhgsl\"><input type='text' id='txtGiftQuantity' value='0' class='input-text'/></td>";
                tb = tb + "<td  class=\"zpjgormjjg\"><input type='text' id='txtGiftPrice' value='0' class='input-text'/></td>";
                tb = tb + "<td><a onclick='AddMeetCount()' class=\"btn btn-secondary radius\">存盘</a></td></tr>";
                $('#TbMeetCount').empty();
                $("#TbMeetCount").append(tb);
                layer.close(index);
            }
            if (type == '1') {
                var tb = "";
                tb = tb + "<tr class='text-c'>";
                tb = tb + "<td id='tdSort_Id'>1</td>";
                tb = tb + "<td class=\"mztj\"><input type='text' id='txtMeetCount' value='1' class='input-text'/></td>";
                tb = tb + "<td class=\"zk\"><input type='text' id='txtDiscount' value='100' class='input-text' readonly='readonly'/></td>";
                tb = tb + "<td class=\"zp\"><input type='text' id='txtGiftName' value='' class='input-text' ondblclick='ChoseGift()'/>";
                tb = tb + "<input type='hidden' id='txtGiftId' value='' class='input-text'/></td>";
                tb = tb + "<td  class=\"zpslorhgsl\"><input type='text' id='txtGiftQuantity' value='0' class='input-text'/></td>";
                tb = tb + "<td  class=\"zpjgormjjg\"><input type='text' id='txtGiftPrice' value='0' class='input-text'/></td>";
                tb = tb + "<td><a onclick='AddMeetCount()' class=\"btn btn-secondary radius\">存盘</a></td></tr>";
                $('#TbMeetCount').empty();
                $("#TbMeetCount").append(tb);
                layer.close(index);
            }
            if (type == '2') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
            }
            LoadMeetCount();
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
    if (FaTitle == '') {
        layer.msg("方案名称未填写！", { time: 3000 });
        layer.close(index);
        return;
    }
    var fabs = $("#txtFabs").val();//方案标识
    if (fabs == '') {
        layer.msg("方案标识未填写！", { time: 3000 });
        layer.close(index);
        return;
    }
    var showFloor = $("#sltShowFloor").val();//促销类型
    if (showFloor == '') {
        layer.msg("请选择促销类型！", { time: 3000 });
        layer.close(index);
        return;
    }
    var khType = "";
    $("#khTypeId").children("input:checked").each(function () {
        khType += $(this).val() + ",";
    });//客户类型
    if (khType == '') {
        layer.msg("客户类型未选择！", { time: 3000 });
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
    var describe = $("#txtdescribe").val();//促销描述
    var contentType = $('input[name="contentType"]:checked').val();//促销模式
    if (contentType == '') {
        layer.msg("促销模式未填写！", { time: 3000 });
        layer.close(index);
        return;
    }
    var JieTi = $('input[name="contentJieTi"]:checked').val();//促销模式
    //if (JieTi == '') {
    //    layer.msg("阶梯模式未选择！", { time: 3000 });
    //    layer.close(index);
    //    return;
    //}


    var SceneCoding = $("#sceneType").val();
    if (SceneCoding == '') {
        layer.msg("范围类型不能为空", { time: 3000 });
        layer.close(index);
        return;
    }
    var SceneId = '';
    var n = 0;
    switch (SceneCoding) {
        case "1"://分类商品
            //SceneId = $("#txtCategryId").val();
            $("#TbCxfl").children().each(function () {
                if (n != 0) {
                    var str = $("#txtCategryId" + n).val();
                    str += "_" + $("#txtCategryId" + n).parent().next().children("input").val();
                    str += "_" + $("#txtCategryId" + n).parent().next().next().children("input").val();
                    SceneId += str + "|"
                }
                n++;
            });
            break;
        case "2"://品牌商品
            $("#sceneBrand").children("input:checked").each(function () {
                SceneId += $(this).val() + "|";
            });
            break;
        case "3"://独立商品
            //SceneId = $("#txtArticleId").val();
            $("#TbDcxsp").children().each(function () {
                if (n != 0) {
                    var str = $("#txtArticleId" + n).val();
                    str += "_" + $("#txtArticleId" + n).parent().next().children("input").val();
                    str += "_" + $("#txtArticleId" + n).parent().next().next().children("input").val();
                    SceneId += str + "|"
                }
                n++;
            });
            break;
        case "4"://分组商品
            $("#GoodsGroup").children("input:checked").each(function () {
                SceneId += $(this).val() + "|";
            });
            break;
    }

    var promQuantity = $("#txtPromQuantity").val();//促销总数
    if (SceneCoding != 1 && SceneCoding != 3 && promQuantity == '') {
        layer.msg("促销总数数据格式不正确！", { time: 3000 });
        layer.close(index);
        return;
    }

    var saleQuantity = $("#txtSaleQuantity").val();//客户限购
    if (SceneCoding != 1 && SceneCoding != 3 && saleQuantity == '') {
        layer.msg("客户限购数据格式不正确！", { time: 3000 });
        layer.close(index);
        return;
    }
    if (SceneCoding != 0 && SceneId == '') {
        layer.msg("范围规则不完整，请补充完整", { time: 3000 });
        layer.close(index);
        return;
    }
    //var lsid = $("#lsid").html();
    //if (lsid == '') {
    //    layer.msg("请先填写促销满足条件", { time: 3000 });
    //    layer.close(index);
    //    return;
    //}
    var lsid = "[";
    $("#TbMeetCount").children("tr").each(function () {
        var num = $(this).children().eq(0).text();
        if (num != 0) {
            var str = "{\"mztj\":\"" + $(this).children().eq(1).children("span").eq(0).text() + "\",";
            str += "\"zk\":\"" + $(this).children().eq(2).children("span").eq(0).text() + "\",";
            str += "\"zpmc\":\"" + $(this).children().eq(3).children("span").eq(0).text() + "\",";
            str += "\"zpid\":\"" + $(this).children().eq(3).children("span").eq(1).text() + "\",";
            str += "\"shl\":\"" + $(this).children().eq(4).children("span").eq(0).text() + "\",";
            str += "\"jg\":\"" + $(this).children().eq(5).children("span").eq(0).text() + "\"}";
            lsid += str + ",";
        }
    });
    lsid += "]";
    var ruleCode = $("#ruleCode").val();
    var data = {
        type: "SaveProm",
        faname: FaTitle,
        ruleCode: ruleCode,
        fabs: fabs,
        faType: showFloor,
        customerType: khType,
        //startDates: startDates,
        //endDates: endDates,
        describe: describe,
        contentType: contentType,
        JieTi: JieTi,
        //goodsid: ArticleId,
        Quantity: promQuantity,
        xgAmount: saleQuantity,
        lsid: lsid,
        ScenarioId: SceneId,
        PromScenario: SceneCoding
    };
    //var type = "ReturnNumber";
    //var proc = "Proc_Admin_PromUpdate"
    $.ajax({
        url: "prom/ashx/PromRule.ashx?json=" + encodeURIComponent(JSON.stringify(data)),// + "&type=" + encodeURIComponent(type),// + "&proc=" + encodeURIComponent(proc),
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

//分类多选
function AddCxfl() {

    var code = $("#txtCategryCode").val();
    var name = $("#txtCategryName").val();
    var id = $("#txtCategryId").val();
    var shl = $("#txtCategryQuantity").val();
    var jg = $("#txtCategryXg").val();
    var num = $("#TbCxfl").children("tr").length;
    var str = "<tr class=\"text-c\">";
    str += "<td> " + num + "</td>";
    str += "<td>";
    str += "  <span class=\"select-box\" style=\"width:15%\">" + code + "</span>";
    str += "  <span class=\"select-box\" style=\"width:25%\">" + name + "</span>";
    str += "  <input type=\"hidden\" id=\"txtCategryId" + num + "\" value=\"" + id + "\" \">";
    str += " </td>";
    str += " <td><input type=\"number\" value=\"" + shl + "\" class=\"input-text\" /></td>";
    str += " <td><input type=\"number\" value=\"" + jg + "\" class=\"input-text\" /></td>";
    str += " <td><a onclick=\"DeleteCxfl(this)\" class=\"btn  btn-danger radius\">删除</a></td>";
    str += "</tr> ";
    $("#TbCxfl").append(str);
    $("#txtCategryCode").val("");
    $("#txtCategryName").val("");
    $("#txtCategryId").val("");
    $("#txtCategryXg").val("0");
    $("#txtCategryQuantity").val("0");
}
function DeleteCxfl(obj) {
    $(obj).parent().parent().remove();
    var num = 0;
    $("#TbCxfl").children("tr").each(function () {
        $(this).children().eq(0).text(num++);
    });
}

//商品多选
function AddDcxsp() {

    var code = $("#txtGoodsCode").val();
    var name = $("#txtGoodsName").html();
    var id = $("#txtArticleId").val();
    var shl = $("#txtGoodsQuantity").val();
    var jg = $("#txtGoodsXg").val();
    var num = $("#TbDcxsp").children("tr").length;
    var str = "<tr class=\"text-c\">";
    str += "<td> " + num + "</td>";
    str += "<td>";
    str += "  <span class=\"select-box\" style=\"width:15%;display:block;float: left;\">" + code + "</span>";
    str += "  <span class=\"\" style=\"width:70%;display:block;float: left;text-align: left;margin-left: 10px;\">" + name + "</span>";
    str += "  <input type=\"hidden\" id=\"txtArticleId" + num + "\" value=\"" + id + "\" \">";
    str += " </td>";
    str += " <td><input type=\"number\" value=\"" + shl + "\" class=\"input-text\" /></td>";
    str += " <td><input type=\"number\" value=\"" + jg + "\" class=\"input-text\" /></td>";
    str += " <td><a onclick=\"DeleteDcxsp(this)\" class=\"btn  btn-danger radius\">删除</a></td>";
    str += "</tr> ";
    $("#TbDcxsp").append(str);
    $("#txtGoodsCode").val("");
    $("#txtGoodsName").html("");
    $("#txtArticleId").val("");
    $("#txtGoodsQuantity").val("0");
    $("#txtGoodsXg").val("0");
}
function DeleteDcxsp(obj) {
    $(obj).parent().parent().remove();
    var num = 0;
    $("#TbDcxsp").children("tr").each(function () {
        $(this).children().eq(0).text(num++);
    });
}