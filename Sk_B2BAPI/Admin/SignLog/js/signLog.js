function ChangeT() {
    var jlXs = $('#Re input:radio:checked').val();
    if (jlXs == "积分") {
        $("#slCoupon").css("display", "none");
        $("#slRewardRule").css("display", "block");
    }
    else {
        $("#slCoupon").css("display", "block");
        $("#slRewardRule").css("display", "none");
    }
}

//页面加载时获取方法
function QuerySign() {
    openLoading();
    var sqltype = "QueryLog";
    var Procedure = "Proc_SignLog";
    var Type = "Search";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnData.ashx?Type=" + Type + paramcont,
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
                    tb += "<tr>";
                    tb += "<td style=\"color: red\" id=\"LevelIdX\" >" + parseInt(i+1)+"</td>";
                    if (JSON["data"][i]["rewardform"] == "积分") {
                        tb += "<td><input type=\"radio\"  checked='checked'/>积分";
                        tb += "</td>";
                    }
                    else {
                        tb += "<td><input type=\"radio\"  checked='checked'/>优惠券";
                        tb += "</td>";
                    }
                    tb += "<td>";
                    tb += "<input type='text' readonly='readonly' style='text-align:center' value=\"" + JSON["data"][i]["signrule"]+"\"/> ";
                    tb += "</td>";
                    tb += "<td>";
                    tb += "<input type='text' readonly='readonly'style='text-align:center;width:400px;' value=\"" + JSON["data"][i]["signreward"] + "\"/>";
                    tb += "<input type='text' id='ID"+i+"' style='display:none' value=\"" + JSON["data"][i]["id"] + "\"/>";
                    tb += "</td>";
                    tb += "<td><input type=\"button\" value=\"删除\"  class=\"btn yellow\" onclick=\"DeSign('" + i + "');\" /></td>";
                    tb += "</tr>";
                }
                tb += "<tr >";
                tb += "<td style=\"color: red\" id=\"LevelIdX\" >" + parseInt(i + 1) +"</td>";
                tb += "<td><div id=\"Re\"><input type=\"radio\" name=\"rewardForm\" value=\"积分\" />积分";
                //tb += "<input type=\"radio\" name=\"rewardForm\" value=\"优惠券\" />优惠券";
                tb += "</div></td>";
                tb += "<td >";
                tb += "<input  type=\"text\" id=\"slSignRule\" value=\"\" placeholder=\"请输入\"/>天";
                tb += "</td>";
                tb += "<td>";
                tb += "<input type=\"text\" id=\"slRewardRule\" value=\"\" style=\"width:400px\" placeholder=\"请输入\" />积分";//ondblclick=\"Search_FreeGoodsInfo()\"
                //tb += "<input type=\"text\" id=\"slCoupon\" style=\"display:none\" value=\"\" placeholder=\"请选择优惠券\" />";
                tb += "</td>";
                tb += "<td><input type=\"button\" value=\"保存\" class=\"btn green\" onclick=\"SaveSign();\"/></td>";
                tb += "</tr>";
                $("#TBShow").append(tb);
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading();
                $('#TBShow').empty();
                var tb = "";
                tb += "<tr >";
                tb += "<td style=\"color: red\" id=\"LevelIdX\" >1</td>";
                tb += "<td><div id='Re'><input type=\"radio\" name=\"rewardForm\" value=\"积分\"/>积分";
                //tb += "<input type=\"radio\" name=\"rewardForm\" value=\"优惠券\" />优惠券";
                tb += "</div></td>";
                tb += "<td>";
                tb += "<input  type=\"text\" id=\"slSignRule\" value=\"\" placeholder=\"请输入\"/>天";
                tb += "</td>";
                tb += "<td >";
                tb += "<input type=\"text\" id=\"slRewardRule\" value=\"\" placeholder=\"请输入\" style=\"width:300px\" ondblclick=\"Search_FreeGoodsInfo()\"/>积分";
                //tb += "<input type=\"text\" id=\"slCoupon\" style=\"display:none\" value=\"\" placeholder=\"请选择优惠券\" ondblclick=\"Search_FreeGoodsInfo()\"/>";
                tb += "</td>";
                tb += "<td><input type=\"button\" value=\"保存\" class=\"btn green\" onclick=\"SaveSign();\"/></td>";
                tb += "</tr>";
                $("#TBShow").append(tb);
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
//保存签到规则
function SaveSign() {
    openLoading();
    //var jlXs = $("#Re input[name=\"rewardForm\"]:checked").val();  //获取单选按钮的值

    var jlXs = $('#Re input:radio:checked').val();
    //alert(jlXs);
    var qdGz = $("#slSignRule").val();
    var jlGz = $("#slRewardRule").val();
    if (jlXs == "积分") {
        if (isNaN(parseInt(qdGz)) && isNaN(parseInt(jlGz))) {
            alertFun("积分规则类型不对！", function () { closeLoading(); })
        }
    }

    if (qdGz == "" || jlGz == "") {
        alertFun("积分规则不能为空！", function () { closeLoading(); });
    }
    else {
    var sqltype = "InsertSign";
    var Type = "SaveSign";
    var Procedure = "Proc_SignLog"; 
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&jlXs=" + encodeURI(jlXs) + "&qdGz=" + encodeURI(qdGz) + "&jlGz=" + encodeURI(jlGz);
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnData.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                closeLoading();
                QuerySign();
            }
            else {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
        });
    }
}
//删除优惠券规则
function DeSign(i) {
    openLoading();
    var Id = $("#ID" + i + "").val();
    var sqltype = "DelSign";
    var Procedure = "Proc_SignLog";
    var Type = "delSign";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&Id=" + Id;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnData.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                closeLoading();
                QuerySign();
            }
            else {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}
//创建生成积分规则
function CreateSign() {
    openLoading();
    var FATypeID = $("#KhLx").val();
    var TypeId = $("#SignModel").val();
    var Status = $("#ShowStatus").val();
    var GzXs = $("#GzMx").val();
    var Procedure = "Proc_SignLog";
    var sqltype = "CreateSign";
    var Type = "CreateSign";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&FATypeID=" + FATypeID + "&TypeId=" + TypeId + "&Status=" + Status + "&GzXs=" + GzXs;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnData.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                alertFun("存盘成功！", function () { closeLoading(); window.location.reload(); });
            }
            else {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    });
}
//生成查询赠品信息弹窗
function Search_FreeGoodsInfo() {
    var jlXs = $('#Re input:radio:checked').val();
    if (jlXs == "积分") {
    }
    else {
        closeCover();
        openCover(); //生成弹窗
        CreatFreeGoodsInfoUI();
        Query_Coupon();
    }
}
///创建选择赠品商品UI
function CreatFreeGoodsInfoUI() {
    var contHtml = "";
    contHtml += "<a class='closeBtn' onclick='closeCover();'><img src='../img/close.png'/></a>"
    contHtml += "<h3 class='winPopTit' id='maskTitle'>选择优惠券</h3>"
    contHtml += "<div class='winPopCon'>"
    contHtml += "<h2 class='winPoph2' id='maskTitle_left'></h2>"
    contHtml += "<div id='maskContent'>"
    //contHtml += "<div class='conForm' id='cxtjdiv'>"
    //contHtml += "<p><label>商品编号：</label><input type='text' id='spbhtxt'  placeholder='关键词搜索'></p>"
    //contHtml += "<a class='btn btn-success' onclick='Get_GoodsInfo()'><i class='searchIcon btnIcon'></i>查 询</a>"
    //contHtml += "</div>"
    contHtml += "<div class='conTable'>"
    contHtml += "<table class=\"dataintable\" id='TBSHOW'>"
    contHtml += "<tr class='chcol'>"
    contHtml += "<tr><th>正在加载类容........</th></tr>"
    contHtml += "</table>"
    contHtml += "</div>"
    contHtml += "</div>"
    contHtml += "</div>"
    document.getElementById("winPop").innerHTML = contHtml;
}
//选择优惠券信息
function Query_Coupon() {
    openLoading();
    var data = {
        type: "CouponSearch"
    };
    var proc = "Proc_ImgAdmin";//存储过程名
    var type = "ReturnList";
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/returnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBSHOW').empty();
                var tb = "<tr class='chcol'><th>序号</th><th>编号</th><th>优惠券名</th><th>满足金额</th><th>优惠金额</th><th>开始日期</th><th>截止日期</th><th>客户类型</th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='select" + i + "' ondblclick='ChooseCoupon(\"" + i + "\")' onMouseOver='changecolor(\"" + i + "\")' onMouseOut='deletetcolor(\"" + i + "\")'>"
                        + "<td>" + (parseInt(i) + 1) + "</td>"
                        + "<td id='code" + i + "'>" + JSON["data"][i]["code"] + "</td>"
                        + "<td id='name" + i + "'>" + JSON["data"][i]["CouponName"] + "</td>"
                        + "<td >" + JSON["data"][i]["MeetCount"] + "</td>"
                        + "<td >" + JSON["data"][i]["DisCount"] + "</td>"
                        + "<td >" + JSON["data"][i]["StartDate"] + "</td>"
                        + "<td >" + JSON["data"][i]["EndDate"] + "</td>"
                        + "<td >" + JSON["data"][i]["pricerelation"] + "</td>"
                        + "</tr>"
                }
                $("#TBSHOW").append(tb);
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
    });
}
//选中优惠券
function ChooseCoupon(i) {
    openLoading();
    var couponCode = $("#code" + i + "").html();
    var couponName = $("#name" + i + "").html();

    $("#slRewardRule").val(couponCode);
  
    closeLoading();
    closeCover();
}