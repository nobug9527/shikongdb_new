///
function SearchInfo(obj) {
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#SpageIndex").html(1);
            break;
        default:
            break;
    }
    var proc = GetQueryString("proc");//存储过程名称
    var strWhere = GetQueryString("strWhere");//查询条件
    var spid = GetQueryString("spid");//查询条件
    var sqlType = GetQueryString("sqlType");//查询类型
    var choseType = GetQueryString("type");
    if (spid != undefined & spid != '' & spid != NaN) {
        GetData(proc, strWhere, sqlType, choseType, spid);
    } else {
        GetData(proc, strWhere, sqlType, choseType);
    }

}
function GetData(proc, strWhere, sqlType, choseType) {
    GetData(proc, strWhere, sqlType, choseType, '');
}
///加载数据
function GetData(proc, strWhere, sqlType, choseType, spid) {

    var index = layer.load(2);
    var searchValue = $("#txtSearchValue").val();
    if (searchValue != "") {
        strWhere = searchValue;
    }
    else { $("#txtSearchValue").val(strWhere); }
    var pageIndex = $("#SpageIndex").html();  //表示当前页

    var pageSize = "20";    //表示每页显示的条目数
    var data;
    if (choseType == "businessDoc" || choseType == "YwtRole") {
        data = {
            type: sqlType,
            xentId: GetQueryString("entId"),
            strWhere: strWhere,
            pageIndex: pageIndex,
            pageSize: pageSize,
            spid: spid
        };
    }
    else {
        data = {
            type: sqlType,
            strWhere: strWhere,
            pageIndex: pageIndex,
            pageSize: pageSize,
            spid: spid
        };
    }
    var type = "ReturnDataSet";
    $.ajax({
        url: "ashx/MainReturnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        type: "Post",
        chche: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var status = obj.flag;
            if (status == 0) {
                $("#conTable_Top").empty();
                $("#conTable_Body").empty();
                //显示的页数和条目数
                var recordCount = data.recordCount;
                var pageCount = data.pageCount;

                ///加载表头
                var contHtml = "";
                contHtml += "<tr class=\"text-c\">";
                var fdname = data.fdname;
                var n = 0;
                contHtml += "<th>序号</th>";
                for (var i in fdname) {
                    if (i.indexOf("key") != -1) {
                        contHtml += "<th style='display:none'>" + fdname[i].trim() + "</th>";
                    }
                    else {
                        contHtml += "<th>" + fdname[i].trim() + "</th>";
                    }
                }
                $("#conTable_Top").append(contHtml);
                contHtml = "";
                //加载内容
                if (parseInt(recordCount) > 0) {
                    $.each(data.content, function (i, itm) {
                        contHtml += "<tr class='text-c'  ondblclick='ChoseInfo(\"" + i + "\",\"" + choseType + "\")'>";
                        contHtml += "<td>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>"
                        for (var m in itm) {
                            if (m.indexOf("key") != -1 || m == "hshj") {
                                contHtml += "<td style='display:none' id='x" + m + i + "'>" + itm[m].trim() + "</td>"
                            }
                            else {
                                contHtml += "<td id='x" + m + i + "'>" + itm[m].trim() + "</td>"
                            }
                        }
                        contHtml += "</tr>"
                    });
                    $("#conTable_Body").append(contHtml);

                    //显示的页数和条目数
                    $("#SrecordCount").html(recordCount);
                    $("#SpageCount").html(pageCount);
                }
                layer.close(index);
            }
            else if (status == 1) {
                $("#conTable_Body").empty();
                $("#SrecordCount").val(0);
                $("#SpageCount").val(1);
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
    });
}
///选择信息
function ChoseInfo(i, type) {
    layer.msg(type);
    ///单品促销选择商品
    if (type == "single") {
        var keyid = $("#xkeyid" + i + "").html();
        var goodsCode = $("#xgoodscode" + i + "").html();
        var name = $("#xsub_title" + i + "").html();
        parent.$("#txtGoodsName").val(name);
        parent.$("#txtGoodsCode").val(goodsCode);
        parent.$("#txtArticleId").val(keyid);
        layer_close();
        Load();
    }
    else if (type == "categry")///选择分类
    {
        var keyid = $("#xkeyid" + i + "").html();
        var name = $("#x分类" + i + "").html();
        parent.$("#txtCategryName").val(name);
        parent.$("#txtCategryCode").val(keyid);
        parent.$("#txtCategryId").val(keyid);
        layer_close();
    }
    else if (type == "kxproduct") {
        var keyid = $("#xkeyid" + i + "").html();
        var goodsCode = $("#xgoodscode" + i + "").html();
        var name = $("#xsub_title" + i + "").html();
        var drug_spec = $("#xdrug_spec" + i + "").html();
        var drug_factory = $("#xdrug_factory" + i + "").html();
        var approval_number = $("#x国药准字" + i + "").html();
        parent.$("#goodsname").html(name);
        parent.$("#goodscode").html(goodsCode);
        parent.$("#drug_spec").html(drug_spec);
        parent.$("#drug_factory").html(drug_factory);
        parent.$("#approval_number").html(approval_number);
        parent.$("#txtGoodsName").val(name);
        parent.$("#txtArticleId").val(keyid);
        layer_close();
    }
    else if (type == "kxcompany") {
        var val = $("#xkeyid" + i + "").html();
        var name = $("#x单位名称" + i + "").html();
        var abc = 0;
        parent.$(".Companykx0,.Companykx1").each(function () {
            if (parent.$(this).attr("data-value") == val) {
                abc = 1;
                return;
            }
        });
        if (abc == 1) {
            layer.alert("您请选择资料已在控销列表！", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
            return;
        }
        parent.$("#CompanyList1").before("<tr  class='text-c'><td data-value='" + val + "' class='Companykx1' style='border-right: 1px solid #ddd;height:57px;color:green'><img src='../UploadFile/素材/组 586@2x.png' style='float: right; margin - top: -20px;' onclick='del(this)' />" + name + "</td></tr>")

        layer_close();
    }
    else if (type == "kxtype") {
        var val = $("#xkeyid" + i + "").html();
        var name = $("#x客户类型" + i + "").html();
        var abc = 0;
        parent.$(".Typekx0,.Typekx1").each(function () {
            if (parent.$(this).attr("data-value") == val) {
                abc = 1;
                return;
            }
        });
        if (abc == 1) {
            layer.alert("您请选择资料已在控销列表！", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
            return;
        }
        parent.$("#TypeList1").before("<tr  class='text-c'><td data-value='" + val + "' class='Typekx1' style='border-right: 1px solid #ddd;height:57px;color:green'><img src='../UploadFile/素材/组 586@2x.png' style='float: right; margin - top: -20px;' onclick='del(this)' />" + name + "</td></tr>")
        layer_close();
    }
    else if (type == "kxregion") {
        var val = $("#xkeyid" + i + "").html();
        var name = $("#x地址" + i + "").html();
        var abc = 0;
        parent.$(".Regionkx0,.Regionkx1").each(function () {
            if (parent.$(this).attr("data-value") == val) {
                abc = 1;
                return;
            }
        });
        if (abc == 1) {
            layer.alert("您请选择资料已在控销列表！", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
            return;
        }
        parent.$("#RegionList1").before("<tr  class='text-c'><td data-value='" + val + "' class='Regionkx1' style='border-right: 1px solid #ddd;height:57px;color:green'><img src='../UploadFile/素材/组 586@2x.png' style='float: right; margin - top: -20px;' onclick='del(this)' />" + name + "</td></tr>")
        layer_close();
    }
    else if (type == "bkxcompany") {
        var val = $("#xkeyid" + i + "").html();
        var name = $("#x单位名称" + i + "").html();
        var abc = 0;
        parent.$(".Companykx0,.Companykx1").each(function () {
            if (parent.$(this).attr("data-value") == val) {
                abc = 1;
                return;
            }
        });
        if (abc == 1) {
            layer.alert("您请选择资料已在控销列表！", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
            return;
        }
        parent.$("#CompanyList0").before("<tr  class='text-c'><td data-value='" + val + "' class='Companykx0' style='border-right: 1px solid #ddd;height:57px;color:red'><img src='../UploadFile/素材/组 586@2x.png' style='float: right; margin - top: -20px;' onclick='del(this)' />" + name + "</td></tr>")
        layer_close();
    }
    else if (type == "bkxtype") {
        var val = $("#xkeyid" + i + "").html();
        var name = $("#x客户类型" + i + "").html();
        var abc = 0;
        parent.$(".Typekx0,.Typekx1").each(function () {
            if (parent.$(this).attr("data-value") == val) {
                abc = 1;
                return;
            }
        });
        if (abc == 1) {
            layer.alert("您请选择资料已在控销列表！", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
            return;
        }
        parent.$("#TypeList0").before("<tr  class='text-c'><td data-value='" + val + "' class='Typekx0' style='border-right: 1px solid #ddd;height:57px;color:red'><img src='../UploadFile/素材/组 586@2x.png' style='float: right; margin - top: -20px;' onclick='del(this)' />" + name + "</td></tr>")

        layer_close();
    }
    else if (type == "bkxregion") {
        var val = $("#xkeyid" + i + "").html();
        var name = $("#x地址" + i + "").html();
        var abc = 0;
        parent.$(".Regionkx0,.Regionkx1").each(function () {
            if (parent.$(this).attr("data-value") == val) {
                abc = 1;
                return;
            }
        });
        if (abc == 1) {
            layer.alert("您请选择资料已在控销列表！", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
            return;
        }
        parent.$("#RegionList0").before("<tr  class='text-c'><td data-value='" + val + "' class='Regionkx0' style='border-right: 1px solid #ddd;height:57px;color:red'><img src='../UploadFile/素材/组 586@2x.png' style='float: right; margin - top: -20px;' onclick='del(this)' />" + name + "</td></tr>")
        layer_close();
    }
    else if (type == "gift")///选择赠品
    {
        var keyid = $("#xkeyid" + i + "").html();
        var name = $("#xgoodsname" + i + "").html();
        parent.$("#txtGiftName").val(name);
        parent.$("#txtGiftId").val(keyid);
        layer_close();
    }
    else if (type == "singlegift") {
        var keyid = $("#xkeyid" + i + "").html();
        var goodsCode = $("#xgoodscode" + i + "").html();
        var name = '商品：' + $("#xsub_title" + i + "").html() + '<br>生产公司:' + $("#xdrug_factory" + i + "").html() + '<br>规格:' + $("#xdrug_spec" + i + "").html() + '<br>中包装：' + $("#xmin_package" + i + "").html() + '<br>价格：' + $("#xprice_ck" + i + "").html() + '<br>库存：' + $("#xstock_quantity" + i + "").html() + ''
        parent.$("#txtGoodsName").html(name);
        parent.$("#txtGoodsCode").val(goodsCode);
        parent.$("#txtArticleId").val(keyid);
        layer_close();
    }
    else if (type == "singleHG") {
        var keyid = $("#xkeyid" + i + "").html();
        var name = $("#xsub_title" + i + "").html();
        parent.$("#txtGiftName").val(name);
        parent.$("#txtGiftId").val(keyid);
        layer_close();
    }
    else if (type == "groups") {
        parent.$("#tdGoodsCode").html($("#xgoodscode" + i + "").html());
        parent.$("#txtGoodsName").val($("#xsub_title" + i + "").html());
        parent.$("#txtArticleId").val($("#xkeyid" + i + "").html());
        parent.$("#tdDrugSpec").html($("#xdrug_spec" + i + "").html());
        parent.$("#tdMinPackage").html($("#xmin_package" + i + "").html());
        parent.$("#txtGoodsQuantity").val($("#xmin_package" + i + "").html());
        parent.$("#txtGoodsPrice").val($("#xprice_ck" + i + "").html());
        var taxAmount = (parseInt($("#xmin_package" + i + "").html()) * parseInt($("#xprice_ck" + i + "").html())).toFixed(3)
        parent.$("#tdTaxAmount").html(taxAmount);
        parent.$("#tdPrice").html($("#xprice_ck" + i + "").html());
        parent.$("#tdStock_Quantity").html($("#xstock_quantity" + i + "").html());
        layer_close();
    }
    else if (type == "groups") {
        parent.$("#txtEntId").val($("#xkeyid" + i + "").html());
        parent.$("#txtEntName").val($("#xentname" + i + "").html());
        var khEntId = parent.$("#khEntId").val();
        if ($("#xkeyid" + i + "").html() != khEntId) {
            parent.$("#txtBusinessName").val("");
            parent.$("#txtBusinessId").val("");
            parent.$("#khEntId").val("");
        }
        layer_close();
    }
    else if (type == "businessDoc") {
        parent.$("#txtBusinessName").val($("#xbusinessname" + i + "").html());
        parent.$("#txtBusinessId").val($("#xkeyid" + i + "").html());
        parent.$("#khEntId").val($("#xent_keyid" + i + "").html());
        layer_close();
    }
    else if (type == "Brand") {
        BrandChoseGoods($("#xdrug_factory" + i + "").html());
    }
    else if (type == "goodsBrand") {
        parent.$("#txtBrandId").val($("#xbillno" + i + "").html());
        layer_close();
    }
    else if (type == "YwtRole") {
        var keyid = $("#xKeyId" + i + "").html()
        UpdateYwyRole(keyid);
    }
}
//品牌选择商品
function BrandChoseGoods(drugFactory) {
    var index = layer.load(2);
    var lsid = GetQueryString("lsid");
    var proc = "Proc_Admin_Brand";//存储过程名
    var type = "ReturnNumber";
    var json = {
        type: "Add_Brand_Ls",
        lsid: lsid,
        strWhere: drugFactory
    };
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/MainReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                parent.document.getElementById("clickId").click();
                layer_close();
            }
            else {
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}
//更新业务员角色
function UpdateYwyRole(keyid) {
    var index = layer.load(2);
    var userId = GetQueryString("userId");//存储过程名称
    var entId = GetQueryString("entId");//存储过程名称
    var lsid = GetQueryString("lsid");
    var proc = "proc_dkxd_salesman";//存储过程名
    var type = "ReturnNumber";
    var json = {
        type: "UpdateSalesManRole",
        keyid: keyid,
        ywyid: userId,
        xentId: entId
    };
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/MainReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                parent.document.getElementById("btnClick").click();
                layer_close();
            }
            else {
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);

        }
    })
}