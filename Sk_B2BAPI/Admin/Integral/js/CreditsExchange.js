///创建选择商品UI
function CreatGoodsInfoUI() {
    var contHtml = "";
    contHtml += "<a class='closeBtn' onclick='closeCover();'><img src='../img/close.png'/></a>"
    contHtml += "<h3 class='winPopTit' id='maskTitle'>选择信息</h3>"
    contHtml += "<div class='winPopCon'>"
    contHtml += "<h2 class='winPoph2' id='maskTitle_left'></h2>"
    contHtml += "<div id='maskContent'>"
    contHtml += "<div class='conTable'>"
    contHtml += "<table class=\"dataintable\" id='TBWindows'>"
    contHtml += "<tr class='chcol'>"
    contHtml += "<tr><th>正在加载类容........</th></tr>"
    contHtml += "</table>"
    contHtml += "</div>"
    contHtml += "<div class='default' id='fanye'>"
    contHtml += "<p class='fl'>"
    contHtml += "<a onclick='Gbtnfirst(GetInfo)'>首页</a>"
    contHtml += "<a style='color:Blue' onclick='Gbtnup(GetInfo)'>上一页</a>"
    contHtml += "<a style='color:Blue' onclick='Gbtnnext(GetInfo)'>下一页</a>"
    contHtml += "<a onclick='Gbtnlast(GetInfo)'>尾页</a>"
    contHtml += "<span class='fr'>每页显示<b id='GPageSize'>15</b>条，共<b id='GRecordCount'>0</b>条,"
    contHtml += "当前页<b id='GPageIndex'>1</b>/<b id='GPageCount'>1</b></span>"
    contHtml += "</p>"
    contHtml += "</div>"
    contHtml += "</div>"
    contHtml += "</div>"
    document.getElementById("winPop").innerHTML = contHtml;
}
//页面加载方法
function LoadPage() {
    closeCover();
    openCover(); //生成弹窗
    CreatGoodsInfoUI();
    GetCoupon();
}
//选择优惠券
function GetCoupon() {
    openLoading();
    var PageSize = $("#GPageSize").html();
    var PageIndex = $("#GPageIndex").html();

    var SqlType = "GetCoupon";
    var SqlProcedure = "Proc_HjSalesMan";
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        PageSize: PageSize,
        PageIndex: PageIndex
    };
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnDataTable.ashx?Type=" + Type + "&Json=" + encodeURI(JSON.stringify(Json)),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBWindows').empty();
                var tb = "<tr class='odd_bg'><th>序号</th><th>编号</th><th>优惠券名</th><th>满足金额</th><th>优惠金额</th><th>客户类型</th><th>开始日期</th><th>截止日期</th><th>使用标志</th><th>可用数量</th>";
                tb += "</tr>";
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr  id='select" + i + "' ondblclick=\"ChoseCoupon(" + i + ")\" onmouseover=\"changecolor(" + i + ")\" onmouseout=\"deletetcolor("+i+")\">"
                    tb = tb + "<td align='center'>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>";
                    tb = tb + "<td align='center' id='xcode" + i + "'>" + JSON["data"][i]["Code"] + "</td>";
                    tb = tb + "<td align='center' id='xcodeName"+i+"'>" + JSON["data"][i]["CouponName"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["MeetCount"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["DisCount"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["PriceRelation"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["StartDate"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["EndDate"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["usertype"] + "</td>";
                    tb = tb + "<td align='center'>" + JSON["data"][i]["Amount"] + "</td>";
                    tb = tb + "</tr>";
                }
                $("#TBWindows").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("GRecordCount").innerHTML = recordCount;
                document.getElementById("GPageCount").innerHTML = pageCount;
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading();
                $('#TBWindows').empty();
                var tba = "<tr><td colspan=5></td><td colspan=8>暂无数据</td></tr>"
                $("#TBWindows").append(tba);
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
//选中单据
function ChoseCoupon(i) {
    var codeName = $("#xcodeName"+i+"").html();
    var code = $("#xcode" + i + "").html();
    $("#txtCoupon").val(codeName);
    $("#txtCode").val(code);
    closeCover();
}
//提交信息
function AddInfo() {
    var integralParatement = RegExp("^([1-9][0-9]*)$");
    var codename = $("#txtCoupon").val();
    var code = $("#txtCode").val();
    var classification = $('#classification').val();//分类
    var floor = $('#floor').val();//楼层
    var hiddenUrl = $('#hiddenUrl').val();//图片路径
    var integral = $('#txtInteger').val();//积分
    var AmountId = $('#CjAmountId').val();//抽奖次数
    var Name = $('#CJnametxt').val();//商品名称
    if (codename == "" && classification == "积分兑换") {
        alertFun("优惠券不能为空！");
    } else if (classification == "") {
        alertFun("分类不能为空！");
    } else if (floor == "") {
        alertFun("楼层不能为空！");
    } else if (hiddenUrl == "" && classification != '赠品') {
        alertFun("图片路径不能为空！");
    } else if (integral == "") {
        alertFun("积分不能为空！");
    } else if (!integralParatement.test(integral)) {
        alertFun("积分必须为整数！");
    }
    else if (parseFloat(AmountId) < 0 && classification == "积分抽奖")
    {
        alertFun("抽奖次数必须大于0！");
    }
    else if (Name == "" && classification == "积分抽奖")
    {
        alertFun("商品名称不能为空！");
    }
    else {
        var SqlType = "InsertCoupon";
        var SqlProcedure = "Proc_Integral";
        var Type = "ExecuteNonQuery";
        var Json = {
            Type: SqlType,
            Procedure: SqlProcedure,
            floottype: floor,
            goodstype: classification,
            integral: integral,
            imageurl: hiddenUrl,
            Code: code,
            quantity: AmountId,
            goodsname: Name
        };
        $.ajax({
            type: "Post",
            cache: false,
            url: "ashx/ReturnDataTable.ashx?Type=" + Type + "&Json=" + encodeURI(JSON.stringify(Json)),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var JSON = data;
                var TYPE = JSON["return_code"];
                var count = JSON["count"];
                if (TYPE == '0') {
                    alertFun(JSON["data"][0]["message"], function () { closeLoading(); window.location.reload(); });

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
///积分商品类型选择事件
function ChoseGoodsType()
{
    var Type=$("#classification").val();
    if (Type == "积分抽奖") {
        document.getElementById("YhgTxtId").style.display = "none";
        document.getElementById("CjDivId").style.display = "block";
        document.getElementById("CJnameDiv").style.display = "block";
    }
    else {
        document.getElementById("YhgTxtId").style.display = "block";
        document.getElementById("CjDivId").style.display = "none";
        document.getElementById("CJnameDiv").style.display = "none";
    }
}
