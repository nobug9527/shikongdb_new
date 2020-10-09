//查询方案信息
function GetFaInfo() {
    openLoading();
    var ksRq = $("#startDate").val();        //获取开始日期
    var jsRq = $("#endDate").val();        //获取结束日期
    var fbStatus = $("#slStatus").val();    //获取发布状态
    var zhType = $("#slType").val();      //获取组合类型
    var keyWord = $("#txtKeyWord").val();     //获取关键字
    var PageSize = $("#PageSize").html();
    var PageIndex = $("#PageIndex").html(); 

    var sqltype = "cxzhinfo";
    var Procedure = "Proc_GroupGoods";
    var Type = "SearchFa";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&ksRq=" + ksRq + "&jsRq=" + jsRq + "&fbStatus=" + encodeURI(fbStatus) + "&zhType=" + encodeURI(zhType) + "&keyWord=" + encodeURI(keyWord) + "&PageSize=" + PageSize + "&PageIndex=" + PageIndex;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/groupdesign_query.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBShow').empty();
                var tb = "";
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb += "<tr class='chcol' align='center' id='select" + i + "'>"
                    tb += "<td>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["id"] + "</td>"
                    if (JSON["data"][i]["GroupType"] == "zuhe") {
                        tb += "<td  align='center'>组合</td>"
                    }
                    else if (JSON["data"][i]["GroupType"] == "CpTj") {
                        tb += "<td  align='center'>产品推荐</td>"
                    }
                    else {
                        tb += "<td  align='center'>套餐</td>"
                    }
                    if (JSON["data"][i]["DyType"] == "txtZdJg") {
                        tb += "<td  align='center'>自定义价格</td>"
                    }
                    else {
                        tb += "<td  align='center'>优惠金额</td>"
                    }
                    tb += "<td  align='center'>" + JSON["data"][i]["KhType"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["Amount"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["LimitG"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["yhje"] + "</td>"
                    tb += "<td id='xfbzt" + i + "' align='center'>" + JSON["data"][i]["fbzt"] + "</td>"
                    tb += "<td id='xtimes" + i + "' align='center'>" + JSON["data"][i]["addtimes"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["KsRq"] + " " + JSON["data"][i]["KsSj"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["JsRq"] + " " + JSON["data"][i]["JsSj"] + "</td>"
                    if (JSON["data"][i]["fbzt"] == "否") {
                        tb += "<td>"
                        tb += "<a><span onclick='CxMx(" + i + ")'class='bz co'>查看明细</span></a>"
                        tb += "<a><span onclick='UpdateS(" + i + ")'class='bz cl'>发布</span></a>"
                        tb += "<a><span onclick='ScFa(" + i + ")'class='bz cr'>删除</span></a>"
                        tb += "</td>"
                    }
                    else {
                        tb += "<td>"
                        tb += "<a><span onclick='CxMx(" + i + ")'class='bz co'>查看明细</span></a>"
                        tb += "<a><span onclick='UpdateS(" + i + ")'class='bz cl'>下架</span></a>"
                        tb += "<a><span onclick='ScFa(" + i + ")'class='bz cr'>删除</span></a>"
                        tb += "</td>"
                    }
                    tb += "<td id='xgroupcode" + i + "' align='center' style=\"display:none\">" + JSON["data"][i]["GroupCode"] + "</td></tr>";
                }

                $("#TBShow").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("RecordCount").innerHTML = recordCount;
                document.getElementById("PageCount").innerHTML = pageCount;
                closeLoading();
            }
            if (TYPE == '1') {
                $('#TBShow').empty();
                var tb = "";
                tb = "<tr><td colspan='11' align='center'>无数据</td></tr>"
                $('#TBShow').append(tb);
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
//更改方案状态
function UpdateS(i) {
    var fbZt = $("#xfbzt" + i + "").html();   //获取发布状态
    var groupCode = $("#xgroupcode" + i + "").html();
    if (fbZt == "是") {
        var zt = "否";
    }
    else {
        var zt = "是";
    }
    var sqltype = "upfa";
    var Procedure = "Proc_GroupGoods";
    var Type = "UpdateDate";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&groupCode=" + groupCode + "&zt=" + encodeURI(zt);
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/groupdesign_query.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == 0) {
                alertFun(JSON["data"][0]["message"], function () { GetFaInfo(); closeLoading() });
                closeLoading();
            }
            if (TYPE == 1) {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}
///删除不要的方案
function ScFa(i) {
    var groupCode = $("#xgroupcode" + i + "").html();
    var sqltype = "scfa";
    var Procedure = "Proc_GroupGoods";
    var Type = "DelFa";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&groupCode=" + groupCode;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/groupdesign_query.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                alertFun(JSON["data"][0]["message"], function () { GetFaInfo();closeLoading() });
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
//查看明细
function CxMx(i) {
    openLoading();
    $("#dateUiHtml").css("display", "block");
    var groupCode = $("#xgroupcode" + i + "").html();

    var sqltype = "ckmx";
    var Procedure = "Proc_GroupGoods";
    var Type = "Mx";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&groupCode=" + groupCode;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/groupdesign_query.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#MxHtml').empty();
                var tb = "<tr class='chcol'><th>商品编号</th><th>是否主商品</th><th>商品名称</th><th>商品规格</th><th>中包装</th><th>包装单位</th><th>数量</th><th>活动价格</th><th>生产厂家<button class='btnmx' onclick='CloseMx()'>关闭明细</button></th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb += "<tr class='chcol' align='center' id='sp" + i + "'>"
                    tb += "<td  align='center'>" + JSON["data"][i]["goods_no"] + "</td>"
                    tb += "<td align='center'>" + JSON["data"][i]["IsMain"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["GoodsName"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["drug_spec"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["min_package"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["package_unit"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["GoodsCount"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["Price"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["factories_choosing"] + "</td>"
                   
                    tb += "<td id='xarticleid" + i + "' align='center' style=\"display:none\">" + JSON["data"][i]["GoodsCode"] + "</td></tr>";
                }

                $("#MxHtml").append(tb);
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
///关闭明细
function CloseMx() {
    $("#dateUiHtml").css("display", "none");
}