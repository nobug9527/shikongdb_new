$(function () {
    $("#YhLx").click(function () {
        var a = $("#YhLx input[name='ShowID']:checked").val();
        if (a == "txtZdJg") {
            $("#dY").css("display", "none");
        }
        else {
            $("#dY").css("display", "block");
        }
    })
})
///创建选择商品UI
function CreatGoodsInfoUI() {
    var contHtml = "";
    contHtml += "<a class='closeBtn' onclick='closeCover();'><img src='../img/close.png'/></a>"
    contHtml += "<h3 class='winPopTit' id='maskTitle'>选择商品</h3>"
    contHtml += "<div class='winPopCon'>"
    contHtml += "<h2 class='winPoph2' id='maskTitle_left'></h2>"
    contHtml += "<div>"
    contHtml += "<span><label>关键字：</label><input type=\"text\" id=\"txtSqlValue\"/></span>"
    contHtml += " <a class=\"btn-success  btnnew\" onclick='JianSuoGoods()'>查询</a>"
    contHtml += " <a class=\"btn-success  btnnew\" onclick='ChooseGoods()'>提交数据</a>"

    contHtml += "</div>"
    contHtml += "<div id='maskContent'>"
    contHtml += "<div class='conTable'>"
    contHtml += "<table class=\"dataintable\" id='TBSHOW'>"
    contHtml += "<tr class='chcol'>"
    contHtml += "<tr><th>正在加载类容........</th></tr>"
    contHtml += "</table>"
    contHtml += "</div>"
    contHtml += "<div class='default' id='fanye'>"
    contHtml += "<p class='fl'>"
    contHtml += "<a onclick='Gbtnfirst(JianSuoGoods)'>首页</a>"
    contHtml += "<a style='color:Blue' onclick='Gbtnup(JianSuoGoods)'>上一页</a>"
    contHtml += "<a style='color:Blue' onclick='Gbtnnext(JianSuoGoods)'>下一页</a>"
    contHtml += "<a onclick='Gbtnlast(JianSuoGoods)'>尾页</a>"
    contHtml += "<span class='fr'>每页显示<b id='GPageSize'>15</b>条，共<b id='GRecordCount'>0</b>条,"
    contHtml += "当前页<b id='GPageIndex'>1</b>/<b id='GPageCount'>1</b></span>"
    contHtml += "</p>"
    contHtml += "</div>"
    contHtml += "</div>"
    contHtml += "</div>"
    document.getElementById("winPop").innerHTML = contHtml;
}
//查询商品
function QueryGoods() {
    closeCover();
    openCover(); //生成弹窗
    CreatGoodsInfoUI();
    JianSuoGoods();
}
//*********检索商品
function JianSuoGoods() {
    openLoading();
    var sqlvalue = $("#txtSqlValue").val();
    var pageIndex = $("#GPageIndex").html();  //表示当前页
    var pageSize = $("#GPageSize").html();    //表示每页显示的条目数
    var sqltype = "cxsp";
    var Procedure = "Proc_GroupGoods";
    var Type = "Search";
    var paramcont = "&PageSize=" + pageSize + "&PageIndex=" + pageIndex + "&sqlvalue=" + encodeURI(sqlvalue) + "&Procedure=" + Procedure + "&sqltype=" + sqltype;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/SearchInfoGoods.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBSHOW').empty();
                var tb = "<tr ><th>序号</th><th>选择</th><th>商品编号</th><th>商品名称</th><th>商品规格</th><th>中包装</th><th>包装单位</th><th>生产厂家</th><th>库存</th><th>国药准字</th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr  align='center' id='select" + i + "' onMouseOver='changecolor(\"" + i + "\")' onMouseOut='deletetcolor(\"" + i + "\")' >"
                                + "<td>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>"
                                + "<td><input type=\"checkbox\" name=\"Sp\" class=\"Sp\" value=\"" + JSON["data"][i]["article_id"] + "\"/></td>"
                                + "<td id='xspbh" + i + "'     align='center'>" + JSON["data"][i]["call_index"] + "</td>"
                                + "<td id='xspmch" + i + "'     align='center'>" + JSON["data"][i]["sub_title"] + "</td>"
                                + "<td id='xshpgg" + i + "'     align='center'>" + JSON["data"][i]["drug_spec"] + "</td>"
                                + "<td id='xzbz" + i + "'       align='center'>" + JSON["data"][i]["min_package"] + "</td>"
                                + "<td id='xdw" + i + "'        align='center'>" + JSON["data"][i]["package_unit"] + "</td>"
                                + "<td id='xshpgg" + i + "'     align='center'>" + JSON["data"][i]["factories_choosing"] + "</td>"
                                + "<td id='xquantity" + i + "'  align='center'>" + JSON["data"][i]["stock_quantity"] + "</td>"
                                + "<td id='xquantity" + i + "'  align='center'>" + JSON["data"][i]["approval_number"] + "</td>"
                                //+ "<td id='xquantity" + i + "'  align='center'>" + JSON["data"][i]["category"] + "</td>"
                                //+ "<td id='xquantity" + i + "'  align='center'>" + JSON["data"][i]["dosage_form"] + "</td>"
                                + "<td id='xarticleid" + i + "' align='center' style=\"display:none\">" + JSON["data"][i]["article_id"] + "</td></tr>";

                }

                $("#TBSHOW").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("GRecordCount").innerHTML = recordCount;
                document.getElementById("GPageCount").innerHTML = pageCount;
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading()
            }
            if (TYPE == '2') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })


}
//*****提交选中的商品
function ChooseGoods() {
    openLoading();
    var Lsid = $("#txtLsid").val();  //获取时间点

    var list = "";
    $.each($("#TBSHOW input:checked"), function (i, itm) {
        list += itm.value;
        list += ",";
    })
    list = list.substring(0, list.length - 1);
    var sqltype = "crsp";
    var Procedure = "Proc_GroupGoods";
    var Type = "InsertData";
    var paramcont = "&List=" + list + "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&Lsid=" + Lsid;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/SearchInfoGoods.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                closeCover();
                $("#dateUiHtml").css('display', 'block'); //显示出表格
                LoadTable();
                closeLoading();
            }
            if (TYPE == '1') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })


}
//加载新表格数据
function LoadTable() {
    var Lsid = $("#txtLsid").val();  //获取时间点
    var dyType = $("#YhLx input[name='ShowID']:checked").val();    //获取定义的优惠类型
    var sqltype = "cxtable";
    var Procedure = "Proc_GroupGoods";
    var Type = "SinagelData";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype+"&Lsid="+Lsid;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/SearchInfoGoods.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBShow').empty();
                if (dyType == "txtYhJe") {
                    var tb = "<tr class='chcol'><th ><span class=\"bz cl\" onclick='CheckQ(this)'>全选</span></th><th>是否主商品</th><th>商品编号</th><th>商品名称</th><th>商品规格</th><th>中包装</th><th>包装单位</th><th>数量</th><th>含税价格</th><th>总金额</th><th>生产厂家</th><th>库存</th><th><span class=\"bz co\">删除</span></th></tr>"
                }
                else {
                    var tb = "<tr class='chcol'><th ><span class=\"bz cl\" onclick='CheckQ(this)'>全选</span></th><th>是否主商品</th><th>商品编号</th><th>商品名称</th><th>商品规格</th><th>中包装</th><th>包装单位</th><th>数量</th><th>含税价格</th><th>总金额</th><th>生产厂家</th><th>库存</th><th><span class=\"bz co\">删除</span></th></tr>"
                }
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb += "<tr class='chcol' align='center' id='sp" + i + "'>"
                    tb += "<td><input type=\"checkbox\" id=\"Gj\" class=\"Gj\" name=\"qx\" value=\"" + JSON["data"][i]["article_id"] + "\"/></td>"
                    tb += "<td><input type=\"checkbox\" name=\"MainSp\" class=\"MainSp\" value=\"" + JSON["data"][i]["article_id"] + "\"/></td>"
                    tb += "<td id='xspbh" + i + "'     align='center'>" + JSON["data"][i]["goods_no"] + "</td>"
                    tb += "<td id='xspmch" + i + "'     align='center'>" + JSON["data"][i]["sub_title"] + "</td>"
                    tb += "<td id='xshpgg" + i + "'     align='center'>" + JSON["data"][i]["drug_spec"] + "</td>"
                    tb += "<td id='xzbz" + i + "'       align='center'>" + JSON["data"][i]["min_package"] + "</td>"
                    tb += "<td id='xdw" + i + "'        align='center'>" + JSON["data"][i]["package_unit"] + "</td>"
                    tb += "<td><input id='xshl" + i + "' onKeyDown='EditEntity(" + i + ")' style=\"text-align:center;width:80px;\" value=\"" + JSON["data"][i]["shl"] + "\"/></td>"
                    if (dyType == "txtZdJg") {
                        tb += "<td><input id='xprice" + i + "' onKeyDown='EditEntity(" + i + ")' style=\"text-align:center;width:80px;\" value=\"" + JSON["data"][i]["price"] + "\"/></td>"
                        tb += "<td id='xzje" + i + "' align='center'>" + JSON["data"][i]["zje"] + "</td>"
                    }
                    else {
                        tb += "<td ><input id='xprice" + i + "'  onKeyDown='EditEntity(" + i + ")' style=\"text-align:center;width:80px;\" value=\"" + JSON["data"][i]["price"] + "\"/></td>"
                        tb += "<td  id='xzje" + i + "' align='center'>" + JSON["data"][i]["zje"] + "</td>"
                    }
                    tb += "<td id='xshengccj" + i + "'     align='center'>" + JSON["data"][i]["factories_choosing"] + "</td>"
                    tb += "<td id='xquantity" + i + "'  align='center'>" + JSON["data"][i]["stock_quantity"] + "</td>"
                    tb += "<td><span onclick='ScSp(" + i + ")'class='bz cr'>删除</span></td>"
                    tb += "<td id='xarticleid" + i + "' align='center' style=\"display:none\">" + JSON["data"][i]["article_id"] + "</td></tr>";
                }

                $("#TBShow").append(tb);
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading();
                $('#TBShow').empty();
                var tba = "<tr><td colspan=5></td><td colspan=8>暂无数据</td></tr>"
                $("#TBShow").append(tba);

            }
            if (TYPE == '2') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}
//全选功能
function CheckQ(obj) {
    if ($(obj).text() == "全选") {
        $(obj).text("取消");
        $(".Gj").prop("checked", true);
    } else {
        $(obj).text("全选");
        $(".Gj").prop("checked", false);
    }

}
//修改商品属性
function EditEntity(i){
    if (event.keyCode == "13") {
        var Lsid = $("#txtLsid").val();  //获取时间点
        var spShl = $("#xshl"+i+"").val();   //获取商品数量
        var spPrice = $("#xprice" + i + "").val(); //获取商品价格
        var spId = $("#xarticleid" + i + "").html(); //获取商品id
        if (isNaN(parseInt(spShl))) {
            alertFun("输入的数量格式错误！", function () { closeLoading(); }, 'f');
        }
        else if (isNaN(parseFloat(spPrice))) {
            alertFun("输入的价格格式错误！", function () { closeLoading(); }, 'f');
        }
        else if (spPrice <= 0)
        {
            alertFun("价格不能小于0！", function () { closeLoading(); }, 'f');
        }
        else {
            var sqltype = "edsp";
            var Procedure = "Proc_GroupGoods";
            var Type = "UpdateSp";
            var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&SpId=" + spId + "&SpShl=" + spShl + "&SpPrice=" + spPrice + "&Lsid=" + Lsid;
            $.ajax({
                type: "Post",
                cache: false,
                url: "ashx/SearchInfoGoods.ashx?Type=" + Type + paramcont,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var JSON = data;
                    var TYPE = JSON["return_code"];
                    var count = JSON["count"];
                    if (TYPE == '0') {
                        $("#dateUiHtml").css('display', 'block'); LoadTable();
                        closeLoading();
                    }
                    if (TYPE == '1') {
                        alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                    closeLoading()
                }
            })

        }
        
    }
}
//删除对应的商品
function ScSp(i) {
    var Lsid = $("#txtLsid").val();  //获取时间点
    var spId = $("#xarticleid"+i+"").html(); //获取商品id
    var sqltype = "desp";
    var Procedure = "Proc_GroupGoods";
    var Type = "DeleteSp";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype+"&SpId="+spId+"&Lsid="+Lsid;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/SearchInfoGoods.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                alertFun(JSON["data"][0]["message"], function () { $("#dateUiHtml").css('display', 'block'); LoadTable(); closeLoading() });
                closeLoading();
            }
            if (TYPE == '1') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}
//创建出一个完整的组合（自定义价格）
function CreateZh() {
    openLoading();
    var zhType = $("#TypeId").val();    //获取组合的类型
    var khType = $("#KhLx").val();             //获取客户类型
    var dyType = $("#YhLx input[name='ShowID']:checked").val();    //获取定义的优惠类型
    var ksRq = $("#txtStartDate").val();      //获取开始日期
    var ksSj = $("#txtKsTime").val();      //获取开始时间
    var jsRq = $("#txtEndDate").val();      //获取结束日期
    var jsSj = $("#txtJsTime").val();      //获取结束时间
    var cxZs = $("#qgamount").val();      //获取促销总数
    var khXg = $("#qglimit").val();      //获取客户限购
    if (dyType == "txtZdJg") {
        var yhJe = "0.000";
    }
    else {
        var yhJe = $("#txtYhJe").val();        //获取优惠金额
    }

    var list = "";
    $.each($("#TBShow  input[name='qx']:checked"), function (i, itm) {
        list += itm.value;
        list += ",";
    })
    list = list.substring(0, list.length - 1);    //选中的商品数组
    var zlNum = $("#TBShow input[name='qx']:checked").length;                       //种类数
 

    var isMain = "";
    $.each($("#TBShow input[name='MainSp']:checked"), function (i, itm) {
        isMain += itm.value;
        isMain += ",";
    })
    isMain = isMain.substring(0, isMain.length - 1);    //获取主商品id

    var mainLength = $("#TBShow input[name='MainSp']:checked").length;   //获取选中的长度

    //var isMain = $("#TBShow input[name='MainSp']:checked").val();    
   
    if ((zlNum <= 1 || zlNum > 5) && zhType == "zuhe") {
        alertFun("组合类型的最少选择2个，最多选择五个！", function () { closeLoading(); }, 'w');
    }
    else if (ksRq == "" || ksSj == "" || jsRq == "" || jsSj == "") {
        alertFun("日期或时间不能为空！", function () { closeLoading(); }, 'w');
    }
    else if (cxZs == "") {
        alertFun("促销总数不能为空！", function () { closeLoading(); }, 'w');
    }
    else if (khXg == "") {
        alertFun("客户限购数量不能为空！", function () { closeLoading(); }, 'w');
    }
    else if (isNaN(parseInt(cxZs))) {
        alertFun("输入的促销总数格式错误！", function () { closeLoading(); }, 'w');
    }
    else if (isNaN(parseInt(khXg))) {
        alertFun("输入的客户限购数量格式错误！", function () { closeLoading(); }, 'w');
    }
    else if (yhJe == "") {
        alertFun("优惠金额不能为空！", function () { closeLoading(); }, 'w');
    }
    else if (isNaN(parseFloat(yhJe))) {
        alertFun("输入的优惠金额格式错误！", function () { closeLoading(); }, 'w');
    }
    else if (mainLength == 0) {
        alertFun("至少选择一个商品为主商品！", function () { closeLoading(); }, 'w');
    }
    else {
        var sqltype = "sczh";
        var Procedure = "Proc_GroupGoods";
        var Type = "CreateZh";
        var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&zhType=" + zhType + "&dyType=" + dyType + "&ksRq=" + ksRq + "&ksSj=" + ksSj + "&jsRq=" + jsRq + "&jsSj=" + jsSj + "&cxZs=" + cxZs + "&khXg=" + khXg + "&list=" + list + "&isMain=" + isMain + "&khType=" + khType + "&yhJe=" + yhJe + "&zlNum=" + zlNum;
        $.ajax({
            type: "Post",
            cache: false,
            url: "ashx/SearchInfoGoods.ashx?Type=" + Type + paramcont,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var JSON = data;
                var TYPE = JSON["return_code"];
                var count = JSON["count"];
                if (TYPE == '0') {
                    alertFun(JSON["data"][0]["message"], function () { window.location.reload(); closeLoading() });
                    closeLoading();
                }
                if (TYPE == '1') {
                    alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                closeLoading()
            }
        })
    }
}


