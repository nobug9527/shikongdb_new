
//保存促销活动
function SavePromDesign() {
    var index = layer.load(2);
    var FaTitle = $("#txtFaTitle").val();//活动名称
    if (FaTitle == '') {
        layer.msg("活动名称未填写！", { time: 3000 });
        layer.close(index);
        return;
    }
    var fabs = $("#txtFabs").val();//活动标识
    if (fabs == '') {
        layer.msg("活动标识未填写！", { time: 3000 });
        layer.close(index);
        return;
    }
    var showFloor = $("#sltFaType").val();//促销类型
    if (showFloor == '') {
        layer.msg("请选择促销类型！", { time: 3000 });
        layer.close(index);
        return;
    }
    var startDates = $("#datemin").val();//开始时间
    if (startDates == '') {
        layer.msg("开始时间未填写！", { time: 3000 });
        layer.close(index);
        return;
    }
    var endDates = $("#datemax").val();//结束时间
    if (endDates == '') {
        layer.msg("结束时间未填写！", { time: 3000 });
        layer.close(index);
        return;
    }
    var ruleCode = $("#ruleCode").val();
    var salesCode = $("#salesCode").val();
    var data = {
        type: "SaveProm",
        faname: FaTitle,
        ruleCode: ruleCode,
        salesCode: salesCode,
        fabs: fabs,
        faType: showFloor,
        startDates: startDates,
        endDates: endDates,
    };
    //var type = "ReturnNumber";
    //var proc = "Proc_Admin_PromUpdate"
    $.ajax({
        url: "prom/ashx/PromSales.ashx?json=" + encodeURIComponent(JSON.stringify(data)),// + "&type=" + encodeURIComponent(type),// + "&proc=" + encodeURIComponent(proc),
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

function GetPromRule() {
    var html = '';
    html += '<div>';
    html += '    <div class= "page-container text-c" id = "conver">';
    html += '        <input type="hidden" value="" id="layerIndex" />';
    html += '            <input type="text" class="input-text" style="width:250px" placeholder="名称/编号" id="txtSearchValue" name="" />';
    html += '   <button type="submit" class="btn btn-success radius" name="" onclick="SearchInfo(1)"><i class="Hui-iconfont">&#xe665;</i> 搜索</button>';
    html += '   </div>';
    html += '   <div class="cl pd-5 bg-1 bk-gray">';
    html += '       <span style="float:right">';
    html += '           <a href="javascript:Sbtnup(SearchInfo)">上一页</a>';
    html += '           <b id="SpageIndex">1</b>/<b id="SpageCount">1</b>';
    html += '           <a href="javascript:Sbtnnext(SearchInfo)">下一页</a>';
    html += '           <span class="r">共有数据：<strong id="SrecordCount">0</strong> 条</span>';
    html += '       </span>';
    html += '   </div>';
    html += '   <div style="height:336px; overflow-y:auto" id="conTable_body">';
    html += '       <table id="table_body" class="table table-border table-bordered table-hover table-bg table-sort" style="width:100%">'
    html += '          <thead >';
    html += '           <tr class="text-c">';
    html += '               <th>序号</th>';
    html += '               <th>机构</th>';
    html += '               <th>规则编号</th>';
    html += '               <th>规则标题</th>';
    html += '               <th>规则类型</th>';
    html += '               <th style="width:300px">规则描述</th>';
    html += '               <th>操作</th>';
    html += '           </tr>';
    html += '         </thead >';
    html += '         <tbody id="TbShows"></tbody>';
    html += '      </table>';
    html += '    </div>';
    html += '</div>';
    var index = 0;
    index = layer.open({
        type: 1,
        title: false,
        closeBtn: 2,
        area: '800px;',
        shadeClose: true,
        skin: 'yourclass',
        content: html,
        success: function () {
            SearchInfo(1);
        }
    });
    $("#layerIndex").val(index);
}
function SearchInfo(obj) {
    var index = layer.load(2);
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var pageSize = '10';

    var faType = $("#sltFaType").val();
    var strWhere = $("#txtSearchValue").val();
    var data = {
        type: "QueryPromList",
        faType: faType,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    $.ajax({
        url: "prom/ashx/PromRuleList.ashx?json=" + encodeURIComponent(JSON.stringify(data)),// + "&type=" + encodeURIComponent(type),// + "&proc=" + encodeURIComponent(proc),
        type: "Post",
        chche: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">';
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>';
                    html += '<td>' + obj[i]["entid"] + '</td>';
                    html += '<td>' + obj[i]["ruleCode"] + '</td>';

                    html += '<td>' + obj[i]["ruleTitle"] + '</td>';
                    html += '<td>' + obj[i]["faName"] + '"</td>';
                    html += '<td>' + obj[i]["describe"] + '</td>';
                    html += '<td><a style="text-decoration:none" onClick="SelectProm(\'' + obj[i]["ruleCode"] + '\',\'' + obj[i]["entid"] + '\',this)" href="javascript:;"  class="btn btn-primary radius" title="选择">选择</a></td>';

                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                layer.close(index);
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
            } else if (type == '2') {
                layer.close(index);
                var msg = result.message;
                layer.alert(msg, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });

            }
            else if (type == '4') {
                layer.msg('出错了，请重试');
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
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}

function SelectProm(ruleCode, entid, obj) {
    $("#ruleBh").val(ruleCode);
    $("#ruleCode").val(ruleCode);
    $("#promDetail").css("display", "block");
    var index = $("#layerIndex").val();
    layer.close(index);
    MeetCountDisplayChange();
    var faType = $("#sltFaType").val();
    if (faType == "DQG" || faType == "GZH")
        LoadZuH(entid, ruleCode)
    else
        LoadDanP(entid, ruleCode);
}


function MeetCountDisplayChange() {
    var faType = $("#sltFaType").val();//促销类型
    console.log("sltFaType:" + faType);
    $("#mzshljdj").hide();
    $("#ZuHCuXiaoDiv").hide();
    $("#DanPCuXiaoDiv").show();
    if (faType == "DZK") {
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
        $(".zk").hide();
        $(".zp").show();
        $("#txtJieTi").show();
        $(".zpslorhgsl").show();
        $(".zpjgormjjg").show();
    } else if (faType == "DMJ") {
        $("#txtJieTi").show();
        $(".zp").hide();
        $(".zk").hide();
        $(".zpslorhgsl").hide();
        $(".zpjgormjjg").show();
        $("#mzshljdj").show();
    } else if (faType == "DQG") {
        $("#ZuHCuXiaoDiv").show();
        $("#DanPCuXiaoDiv").hide();
        $("#spzl").css("display", "none");
        $("#xgsl").css("display", "none");
        $("#xgsl").css("display", "none");
        $("#txtTotalQuantity").removeAttr("readonly");
        $("#txtOnlyQuantity").removeAttr("readonly");
        $("#txtGoodsQuantity").attr("readonly", "readonly");
        $(".qgcyshl").show();
        $(".qgmzshl").show();
        $(".qgxgshl").show();
        $(".zhcyshl").hide();
    }
    else if (faType == "GZH") {
        $("#ZuHCuXiaoDiv").show();
        $("#DanPCuXiaoDiv").hide();
        $("#spzl").css("display", "inherit");
        $("#xgsl").css("display", "inherit");
        $("#txtTotalQuantity").removeAttr("readonly");
        $("#txtOnlyQuantity").attr("readonly", "readonly");
        $("#txtGoodsQuantity").removeAttr("readonly");
        $(".qgcyshl").hide();
        $(".qgmzshl").hide();
        $(".qgxgshl").hide();
        $(".zhcyshl").show();
    }
    else {
        layer.msg("请选择正确的促销类型！", { time: 3000 });
        return;
    }
}

function bianhua() {
    var contentType = $('input[name="contentType"]:checked').val();//促销模式
    if (contentType == 1) {
        $("#mztjtitle").html("满足金额");
    } else {
        $("#mztjtitle").html("满足数量");
    }

    $("#hgorzpjg").html("金额");
    if (contentType == 2)
        $("#hgorzpjg").html("减少的单价");
}
function gainData(sceneType) {
    console.log("sceneType:" + sceneType);
    if (sceneType == '') {
        layer.msg("范围类型不能为空", { time: 3000 });
        return;
    }
    switch (sceneType) {
        case "1"://分类商品cxsl
            $("#cxfl").css("display", "inherit");
            $("#spfz").css("display", "none");
            $("#dcxsp").css("display", "none");
            $("#cxpp").css("display", "none");
            $("#XianGouShl").css("display", "none");
            $("#cxsl").css("display", "none");

            break;
        case "2"://品牌商品
            $("#cxfl").css("display", "none");
            $("#spfz").css("display", "none");
            $("#cxpp").css("display", "inherit");
            $("#dcxsp").css("display", "none");
            $("#XianGouShl").css("display", "inherit");
            $("#cxsl").css("display", "inherit");

            break;
        case "3"://独立商品
            $("#cxpp").css("display", "none");
            $("#spfz").css("display", "none");
            $("#cxfl").css("display", "none");
            $("#dcxsp").css("display", "inherit");
            $("#XianGouShl").css("display", "none");
            $("#cxsl").css("display", "none");

            break;
        case "4"://商品分组spfz
            $("#cxfl").css("display", "none");
            $("#spfz").css("display", "inherit");
            $("#cxpp").css("display", "none");
            $("#dcxsp").css("display", "none");
            $("#XianGouShl").css("display", "inherit");
            $("#cxsl").css("display", "inherit");

            break;
        default:
            $("#cxpp").css("display", "none");
            $("#spfz").css("display", "none");
            $("#cxfl").css("display", "none");
            $("#dcxsp").css("display", "none");
            $("#XianGouShl").css("display", "inherit");
            $("#cxsl").css("display", "inherit");
            break;
    }
}

function LoadDanP(entid, ruleCode) {
    var index = layer.load(5);
    var data = {
        type: "QuerySingleProm",
        entid: entid,
        ruleCode: ruleCode
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
                console.log(obj);
                $("#source").val(JSON.stringify(data));
                $("#lblFaTitle").text(obj.promRule.ruleTitle);
                $("#lbldescribe").text(obj.promRule.describe);
                $("#lblPromQuantity").text(obj.promRule.Amount);
                $("#lblSaleQuantity").text(obj.promRule.limitAmount);
                $("#sceneType").text(GetSpFw(obj.promRule.PromScenario));
                $("input[name=contentType][value=" + obj.promRule.contentType + "]").prop("checked", "checked");
                $("input[name=contentJieTi][value=" + obj.promRule.moreBuy + "]").prop("checked", "checked");
                $("#lblShowFloor").text($("#sltFaType").find("option:selected").text());
                gainData(obj.promRule.PromScenario);
                var len = 1;
                $("#TbMeetCount").empty();
                $.each(obj.conditions, function (i, item) {
                    var tb = "";
                    tb = tb + "<tr class='text-c'>";
                    tb = tb + '<td width="30px" id="xtdSortId' + len + '">' + len + '</td>';
                    tb = tb + "<td class=\"mztj\">" + item.meetCount + "</span></td>";
                    tb = tb + "<td class=\"zk\"><span  style=\"text-align:left\">" + item.discount + "</span></td>";
                    tb = tb + "<td class=\"zp\"><span  style=\"text-align:left\">" + item.giftName + "</span><span style=\"display:none\">" + item.giftGoodsID + "</span></td>";
                    tb = tb + "<td class=\"zpslorhgsl\"><span  style=\"text-align:left\">" + item.giftQuantity + "</span></td>";
                    tb = tb + "<td class=\"zpjgormjjg\"><span  style=\"text-align:left\">" + item.giftPrice + "</span></td>";
                    tb = tb + "</tr>";
                    $("#TbMeetCount").append(tb);
                    len++;
                })
                var lx = obj.promRule.customerType;
                $.each($("#khTypeId").children("input[type=checkbox]"), function (i, item) {
                    var val = $(item).val();
                    if (lx.indexOf(val) != -1)
                        $(item).prop("checked", "checked");
                });


                //品牌
                if (obj.promRule.PromScenario == 2) {
                    var secnario = "";
                    $.each(obj.replenishes, function (i, item) {
                        secnario += item.ScnarioID + ",";
                    });
                    $.each($("#sceneBrand").children("input[type=checkbox]"), function (i, item) {
                        var val = $(item).val();
                        if (secnario.indexOf(val) != -1)
                            $(item).prop("checked", "checked");
                    });
                }
                $("#TbCxfl").empty();
                //商品分类
                if (obj.promRule.PromScenario == 1) {
                    var num = 1;
                    $.each(obj.replenishes, function (i, item) {
                        var str = "<tr class=\"text-c\">";
                        str += "<td> " + num + "</td>";
                        str += "<td>";
                        str += "  <span  style=\"width:15%\">" + item.SecnarioCode + "</span>";
                        str += "  <span  style=\"width:25%\">" + item.SecnarioName + "</span>";
                        str += "  <input type=\"hidden\" id=\"txtCategryId" + num + "\" value=\"" + item.ScnarioID + "\" \">";
                        str += " </td>";
                        str += " <td><span> " + item.maxQuantity + "</span></td>";
                        str += " <td><span> " + item.limitQuantity + "</span></td>";
                        str += "</tr> ";
                        $("#TbCxfl").append(str);
                        num++;
                    });
                }
                //商品分组
                if (obj.promRule.PromScenario == 4) {
                    var secnario = "";
                    $.each(obj.replenishes, function (i, item) {
                        secnario += item.ScnarioID + ",";
                    });
                    $.each($("#GoodsGroup").children("input[type=checkbox]"), function (i, item) {
                        var val = $(item).val();
                        if (secnario.indexOf(val) != -1)
                            $(item).prop("checked", "checked");
                    });
                }
                $("#TbDcxsp").empty();
                //独立商品
                if (obj.promRule.PromScenario == "3") {
                    var num = 1;
                    $.each(obj.replenishes, function (i, item) {
                        var str = "<tr class=\"text-c\">";
                        str += "<td> " + num + "</td>";
                        str += "<td>";
                        str += "  <span  style=\"width:15%;display:block;float: left;\">" + item.SecnarioCode + "</span>";
                        str += "  <span class=\"\" style=\"width:70%;display:block;float: left;text-align: left;margin-left: 10px;\">" + item.SecnarioName + "</span>";
                        str += " </td>";
                        str += " <td><span> " + item.maxQuantity + "</span></td>";
                        str += " <td><span> " + item.limitQuantity + "</span></td>";
                        str += "</tr> ";
                        $("#TbDcxsp").append(str);
                        num++;
                    });
                }

                MeetCountDisplayChange();
                layer.close(index);

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
function LoadZuH(entid, ruleCode) {
    var index = layer.load(5);
    var data = {
        type: "QuerySingleProm",
        entid: entid,
        ruleCode: ruleCode
    };
    //var type = "ReturnNumber";
    //var proc = "Proc_Admin_PromUpdate"
    $.ajax({
        url: "prom/ashx/PromGroupRule.ashx?json=" + encodeURIComponent(JSON.stringify(data)),// + "&type=" + encodeURIComponent(type),// + "&proc=" + encodeURIComponent(proc),
        type: "Post",
        chche: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var status = obj.flag;
            if (status == 0) {
                //$("#source").val(JSON.stringify(data));
                //$("#txtFaTitle").val(obj.promRule.ruleTitle);
                //$("#txtdescribe").val(obj.promRule.describe);
                //$("#txtPromQuantity").val(obj.promRule.Amount);
                //$("#txtSaleQuantity").val(obj.promRule.limitAmount);

                $("#source").val(JSON.stringify(data));
                $("#lblFaTitle").text(obj.promRule.ruleTitle);
                $("#lbldescribe").text(obj.promRule.describe);
                $("#lblPromQuantity").text(obj.promRule.Amount);
                $("#lblSaleQuantity").text(obj.promRule.limitAmount);


                var len = 1;
                $.each(obj.conditions, function (i, item) {
                    var str = "";
                    str += "<tr class=\"text-c\">";
                    str += "        <td>" + len + "</td>";
                    str += "        <td>" + item.goodscode + "</td>";
                    str += "        <td>";
                    str += "            <span style=\"text-align:left\">" + item.sub_title + "</span>";
                    str += "        <input type=\"hidden\" value=\"" + item.giftGoodsID + "\">";
                    str += "        </td>";
                    str += "        <td>" + item.drug_spec + "</td>";
                    str += "        <td>" + item.min_package + "</td>";
                    str += "        <td>" + item.price + "</td>";
                    str += "        <td>" + item.stock_quantity + "</td>";
                    //str += "        <td><span>" + quantity+"</span></td>";
                    str += "        <td class=\"zhcyshl\"><span style=\"text-align:left\">" + item.giftQuantity + "</span></td>";
                    str += "        <td><span style=\"text-align:left\">" + item.giftPrice + "</span></td>";
                    str += "        <td class=\"qgcyshl\"><span style=\"text-align:left\">" + item.maxQuantity + "</span></td>";
                    str += "        <td class=\"qgmzshl\"><span style=\"text-align:left\">" + item.meetCount + "</span></td>";
                    str += "        <td class=\"qgxgshl\"><span style=\"text-align:left\">" + item.limitQuantity + "</span></td>";
                    str += "        <td> <a onclick=\"DeleteGroupGoods(this)\" class=\"btn btn-danger radius\">删除</a></td>";
                    str += " </tr >";
                    $("#TbGoods").append(str);
                    len++;
                })
                var lx = obj.promRule.customerType;
                $.each($("#khTypeId").children("input[type=checkbox]"), function (i, item) {
                    var val = $(item).val();
                    if (lx.indexOf(val) != -1)
                        $(item).prop("checked", "checked");
                });

                MeetCountDisplayChange();
                layer.close(index);

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

///返回商品范围的汉语意思
function GetSpFw(obj) {
    if (obj == "0")
        return "全部商品";
    if (obj == "1")
        return "分类商品";
    if (obj == "2")
        return "品牌商品";
    if (obj == "3")
        return "独立商品";
    if (obj == "4")
        return "商品分组";
    return "";
}

