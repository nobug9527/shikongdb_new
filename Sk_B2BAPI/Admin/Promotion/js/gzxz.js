
$(function () {
    $('#d1').hide();
    $('#d2').hide();
    //$('#d3').hide();
    $('#Dd3').hide();
    $('#Dd4').hide();
    $('#Dd1').hide();
    $('#Dd2').hide();
    $('#con').hide();
    returnKHJB();
    $.each($("input[name=hdsz]"), function (index) {
        //这里就是对单选按钮的单击事件，需要一个each，遍历出所有的radio,然后对每个radio绑定一个click
        $(this).click(function () {
            changeFavourable();
        });
    });
    $.each($("input[name='hdjg']"), function (index) {
        $(this).click(function () {
            var s = $("input[name='hdjg']:checked").val();
            if (s == "SumMoney") {
                $('#d2').hide();
                $('#Dd3').hide();
                $('#Dd4').hide();
                $('#Dd1').hide();
                $('#Dd2').hide();
                $('#d1').show();
                document.getElementById('SumQuality1').value = "";
                document.getElementById('SumQuality2').value = "";
            } else if (s == "SumQuality") {
                $('#d1').hide();
                $('#Dd1').hide();
                $('#Dd2').hide();
                $('#Dd3').hide();
                $('#Dd4').hide();
                $('#d2').show();
                document.getElementById('SumMoney1').value = "";
                document.getElementById('SumMoney2').value = "";
            }
        });
    });
    //优惠对象
    $.each($("input[name='hddx']"), function (index) {
        $(this).click(function () {
            var d = $("input[name='hddx']:checked").val();
            if (d == "Brand") {
                $('#con').show();
            } else {
                $('#con').hide();
            }
        });
    });
    //初始化表单验证
    $("#form1").initValidform();
})
//满足条件出现输入框
function jeHidden(con1, con2, con3, con4, con5, con6) {
    var c1 = $("#" + con1 + "").val();
    var c2 = $("#" + con2 + "").val();
    if (c1 != "" && typeof (c1) != undefined && c2 != "" && typeof (c2) != undefined) {
        $("#" + con3 + "").show();
    } else {
        document.getElementById(con4).value = "";
        document.getElementById(con5).value = "";
        $("#" + con3 + "").hide();
        $("#" + con6 + "").hide();
    }
}

//打开内容选择
function XZGZ() {
    var hddx = $('input[name="hddx"]:checked').val();
    if (!hddx) {
        alert('促销对象为必选项！');
    } else {
        document.getElementById('gzxz').style.display = 'block';
        var category = $('input[name=hddx]:checked').val();
        document.getElementById('gsCode').value = "";
        document.getElementById('gsWord').value = "";
        if (category == "SingleGoods") {
            document.getElementById('code').innerHTML = "商品编号：";
            document.getElementById('name').innerHTML = "商品名称:";
            goodsSearch(1);
        } else if (category == "Brand") {
            document.getElementById('code').innerHTML = "品牌编号：";
            document.getElementById('name').innerHTML = "品牌名称:";
            brandSearch(1);
        }
    }
}
//商品搜索
function goodsSearch(index) {
    var gdCode = $('#gsCode').val();
    var kyWord = $('#gsWord').val();
    var pgSize = 9;
    var pgIndex = index;
    $('#contnet').empty();
    var tableHtml = " <thead><tr><th>选择</th><th>编码</th><th>名称</th><th>批准文号</th><th>企业</th><th>商品编号</th></tr></thead><tbody>";
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + 'Goods/GoodsList?goodsCode=' + gdCode + '&keyword=' + kyWord + '&pageSize=' + pgSize + '&pageIndex=' + pgIndex,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var index1 = data.PageIndex;
            var page = data.PageSize; 
            var total = data.TotalCount;
            var Source = data.Source;
            setRecordCount('0', page, total, index1);
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    tableHtml += "<tr style='text-align:center'>"
                    + "<td><input type=\"radio\" name=\"shangpin\" value='" + Source[i]["sub_title"] + "'/></td>"
                    + "<td>" + Source[i]["article_id"] + "</td>"
                    + "<td>" + Source[i]["sub_title"] + "</td>"
                    + "<td>" + Source[i]["approval_number"] + "</td>"
                    + "<td>" + Source[i]["factories_choosing"] + "</td>"
                    + "<td>" + Source[i]["goods_no"] + "</td>"
                    + "</tr>"
                } tableHtml += "</tbody>";
                $('#contnet').append(tableHtml);
            }
            else {
                $('#contnet').empty();
                tableHtml += "<tr><td colspan='6' style='text-align: center;'>暂无内容</td></tr></tbody>";
                $('#contnet').append(tableHtml);
            }
        }
    });
}
//关键词搜索
function keywordSeacher() {
    var category = $('input[name=hddx]:checked').val()
    if (category == "SingleGoods") {
        goodsSearch(1);
    } if (category == "Brand") {
        brandSearch(1);
    } if (category == "CustomsGroup") {
        return false;
    }
}
//品牌搜索
function brandSearch(index) {
    var gdCode = $('#gsCode').val();
    var kyWord = $('#gsWord').val();
    var pgSize = 3;
    var pgIndex = index;
    $('#contnet').empty();
    var tableHtml = " <thead><tr><th>选择</th><th>品牌编码</th><th>品牌名称</th><th>商标</th><th>编号</th></tr></thead><tbody id=\"tbodyContent\">";
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + "Goods/BrandList?brandCode=" + gdCode + "&keyword=" + kyWord + "&pageSize=" + pgSize + "&pageIndex=" + pgIndex,
        dataType: 'json',
        success: function (data) {
            var Source = data.Source;
            var PageIndex = data.PageIndex;
            var PageSize = data.PageSize;
            var Total = data.TotalCount;
            setRecordCount("0", PageSize, Total, PageIndex);
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    tableHtml += "<tr style='text-align:center'>"//
                    + "<td><input type=\"radio\" name=\"pinpai\" value='" + Source[i]["Billno"] + "'/></td>"
                    + "<td>" + Source[i]["Billno"] + "</td>"
                    + "<td>" + Source[i]["ManuFacturer"] + "</td>"
                    + "<td>" + Source[i]["ImgType"] + "</td>"
                    + "<td>" + Source[i]["id"] + "</td>"
                    + "</tr>";
                }
                tableHtml += "</tbody>";
                $('#contnet').append(tableHtml);
            } else {
                tableHtml += "<tr><td colspan='6' style='text-align: center;'>暂无内容</td></tr></tbody>";
                $('#contnet').append(tableHtml);
            }
        }
    });
}
//关闭Dialog
function closeDialog() {
    document.getElementById('gzxz').style.display = 'none';
}
//首页
function firstPage(m) {
    if (m == "zp") {//赠品
        Premium(1);
    }
    else {
        var category = $('input[name=hddx]:checked').val()
        if (category == "SingleGoods") {
            goodsSearch(1);
        } if (category == "Brand") {
            brandSearch(1);
        }
        if (category == "CustomsGroup") {
            return false;
        }
    }
}
//末页
function lastPage(m) {
    if (m == "zp") {//赠品
        var count = document.getElementById('B4').innerHTML;
        var pgCount = parseInt(count);
        Premium(pgCount);
    } else {
        var category = $('input[name=hddx]:checked').val()
        var count = document.getElementById('pageCount_list').innerHTML;
        var pgCount = parseInt(count);
        if (category == "SingleGoods") {
            goodsSearch(pgCount);
        } if (category == "Brand") {
            brandSearch(pgCount);
        } if (category == "CustomsGroup") {
            return false;
        }
    }
}
//下一页
function nextPage(m) {
    if (m == "zp") {
        var index = document.getElementById('B3').innerHTML;
        var count = document.getElementById('B4').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        Premium(pgNum);
    } else {
        var category = $('input[name=hddx]:checked').val()
        var index = document.getElementById('pageIndex_list').innerHTML;
        var count = document.getElementById('pageCount_list').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        if (category == "SingleGoods") {
            goodsSearch(pgNum);
        } if (category == "Brand") {
            brandSearch(pgNum);
        } if (category == "CustomsGroup") {
            return false;
        }
    }
}
//上一页
function prePage(m) {
    if (m == "zp") {
        var index = document.getElementById('B3').innerHTML;
        var pgNum = parseInt(index);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        Premium(pgNum);
    } else {
        var category = $('input[name=hddx]:checked').val()
        var index = document.getElementById('pageIndex_list').innerHTML;
        var pgNum = parseInt(index);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        if (category == "SingleGoods") {
            goodsSearch(pgNum);
        } if (category == "Brand") {
            brandSearch(pgNum);
        } if (category == "CustomsGroup") {
            return false;
        }
    }
}

function setRecordCount(m, page, total, index1) {
    if (m == "zp") {
        document.getElementById('B1').innerHTML = page;
        document.getElementById('B2').innerHTML = total;
        document.getElementById('B3').innerHTML = index1;
        if (parseInt(total / page) > 0 && total % page == 0) {
            document.getElementById('B4').innerHTML = parseInt(total / page);
        }
        if (parseInt(total / page) >= 0 && total % page > 0) {
            document.getElementById('B4').innerHTML = Math.ceil(total / page);
        }
    }
    document.getElementById('pageSize_list').innerHTML = page;
    document.getElementById('recordCount_list').innerHTML = total;
    document.getElementById('pageIndex_list').innerHTML = index1;
    if (parseInt(total / page) > 0 && total % page == 0) {
        document.getElementById('pageCount_list').innerHTML = parseInt(total / page);
    }
    if (parseInt(total / page) >= 0 && total % page > 0) {
        document.getElementById('pageCount_list').innerHTML = Math.ceil(total / page);
    }
}
//确定按钮事件confirm
function confirm() {
    //遍历获取选中行的某些值
    var tbodyObj = document.getElementById('tbodyContent'); //zpSelectTable
    $("input[name='pinpai']").each(function (key, value) {
        if ($(value).prop('checked')) {
            var billno = tbodyObj.rows[key].cells[1].innerHTML;
            var manuFacturer = tbodyObj.rows[key].cells[2].innerHTML;
            document.getElementById('hidden').value = billno;
            document.getElementById('gstext').value = manuFacturer;
            closeDialog();
        }
    })
}

//提交按钮事件
function addGZ() {
    var hdsz = $('input[name="hdsz"]:checked').val();
    var name = $('#gzName').val();
    var hdjg = $('input[name="hdjg"]:checked').val();
    var hddx = $('input[name="hddx"]:checked').val();
    var text = document.getElementById('hidden').value;
    var itemlist = document.getElementById('itemlist').value;
    if (!name) {
        alert('规则名称不能为空！');
    }
    else if (!hdsz) {
        alert('促销方式为必选项！');
    }
    else if (!hdjg) {
        alert('促销金额为必选项！');
    }
    else if (!hddx) {
        alert('促销对象为必选项！');
    }
    else if (hddx == "Brand" && !text) {
        alert('促销内容为必选项！');
    }
    else {
        if (hddx == "allGoods") {
            text = "AllGoods";
        }
        if (hdjg == "SumMoney") {
            var value1 = parseInt($('#SumMoney1').val());
            var value2 = $('#SumMoney2').val();
            var text1 = parseInt($('#Text1').val());
            var text2 = $('#Text2').val();
            var text3 = parseInt($('#Text3').val());
            var text4 = $('#Text4').val(); 
            if ((value1 == "" || typeof (value1) == undefined || value1 == null) || (value2 == "" || typeof (value2) == undefined || value2 == null)) {
                alert("不能为空");
            } else if ((text1 == "" || typeof (text1) == undefined || text1 == null) || (text2 == "" || typeof (text2) == undefined || text2 == null)) {
                if (isNaN(value1)) {
                    alert("金额必须为数字");
                } else {
                    updata1(code, name, value1, value2, hddx, hdjg, text, hdsz, itemlist);
                }
            } else if ((text3 == "" || typeof (text3) == undefined || text3 == null) || (text4 == "" || typeof (text4) == undefined || text4 == null)) {
                if (isNaN(text1)) {
                    alert("金额必须为数字");
                } else {
                    updata2(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2, itemlist);
                }
            } else {
                if (isNaN(text3)) {
                    alert("金额必须为数字");
                } else {
                    updata3(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2, text3, text4, itemlist);
                }
            }
        }
        else if (hdjg == "SumQuality") {
            var value1 = parseInt($('#SumQuality1').val());
            var value2 = $('#SumQuality2').val();
            var text1 = parseInt($('#Text5').val());
            var text2 = $('#Text6').val();
            var text3 = parseInt($('#Text7').val());
            var text4 = $('#Text8').val();
            if ((value1 == "" || typeof (value1) == undefined || value1 == null) || (value2 == "" || typeof (value2) == undefined || value2 == null)) {
                alert("不能为空");
            } else if ((text1 == "" || typeof (text1) == undefined || text1 == null) || (text2 == "" || typeof (text2) == undefined || text2 == null)) {
                if (isNaN(value1)) {
                    alert("金额必须为数字");
                } else {
                    updata1(code, name, value1, value2, hddx, hdjg, text, hdsz, itemlist);
                }
            } else if ((text3 == "" || typeof (text3) == undefined || text3 == null) || (text4 == "" || typeof (text4) == undefined || text4 == null)) {
                if (isNaN(text1)) {
                    alert("金额必须为数字");
                } else {
                    updata2(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2, itemlist);
                }
            } else {
                if (isNaN(text3)) {
                    alert("金额必须为数字");
                } else {
                    updata3(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2, text3, text4, itemlist);
                }
            }
        }
    }
}
//规则新增（一条）
function updata1(code, name, value1, value2, hddx, hdjg, text, hdsz, itemlist) {
    var isUse = "False";
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/PromotionUpdate",
        data: { PromoCode: 0, PromName: name, MeetCount: value1, discount: value2, PromType: hddx, IsUse: isUse, ContentType: hdjg, Content: text, PromRule: hdsz, PriceRelation: itemlist },
        dataType: "json",
        success: function (data) {
            if (data.Code == "0") {
                alert('操作成功！');
            } else {
                alert('操作失败！原因：' + data.Message);
            }
        }
    });
}
//规则新增（两条）
function updata2(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2, itemlist) {
    var isUse = "False";
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/PromotionUpdate",
        data: { PromoCode: 0, PromName: name, MeetCount: value1, discount: value2, PromType: hddx, IsUse: isUse, ContentType: hdjg, Content: text, PromRule: hdsz, MeetCount2: text1, Discount2: text2, PriceRelation: itemlist },
        dataType: "json",
        success: function (data) {
            if (data.Code == "0") {
                alert('操作成功！');
            } else {
                alert('操作失败！原因：' + data.Message);
            }
        }
    });
}
//规则新增（三条）
function updata3(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2, text3, text4, itemlist) {
    var isUse = "False";
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/PromotionUpdate",
        data: { PromoCode: 0, PromName: name, MeetCount: value1, discount: value2, PromType: hddx, IsUse: isUse, ContentType: hdjg, Content: text, PromRule: hdsz, MeetCount2: text1, Discount2: text2, MeetCount3: text3, DisCount3: text4, PriceRelation: itemlist },
        dataType: "json",
        success: function (data) {
            if (data.Code == "0") {
                alert('操作成功！');
            } else {
                alert('操作失败！原因：' + data.Message);
            }
        }
    });
}
//改变优惠类型
function changeFavourable() {
    var v = $('input[name="hdsz"]:checked').val();
    if (v == "Discount") {
        v = "折扣";
        $('#SumMoney2').prop("readonly", false);
        $('#Text2').prop("readonly", false);
        $('#Text4').prop("readonly", false);
        $('#SumQuality2').prop("readonly", false);
        $('#Text6').prop("readonly", false);
        $('#Text8').prop("readonly", false);
        $('#a1').empty();
        $('#da1').empty();
        $('#da2').empty();
        $('#a2').empty();
        $('#da3').empty();
        $('#da4').empty();
        $('#SumMoney2').attr("placeholder", "输入0-100以内的整数");
        $('#Text2').attr("placeholder", "输入0-100以内的整数");
        $('#Text4').attr("placeholder", "输入0-100以内的整数");
        $('#SumQuality2').attr("placeholder", "输入0-100以内的整数");
        $('#Text6').attr("placeholder", "输入0-100以内的整数");
        $('#Text8').attr("placeholder", "输入0-100以内的整数");
    } else if (v == "FJ") {
        v = "减";
        $('#SumMoney2').prop("readonly", false);
        $('#Text2').prop("readonly", false);
        $('#Text4').prop("readonly", false);
        $('#SumQuality2').prop("readonly", false);
        $('#Text6').prop("readonly", false);
        $('#Text8').prop("readonly", false);
        $('#SumMoney2').attr("placeholder", "请输入优惠金额");
        $('#Text2').attr("placeholder", "请输入优惠金额");
        $('#Text4').attr("placeholder", "请输入优惠金额");
        $('#SumQuality2').attr("placeholder", "请输入优惠金额");
        $('#Text6').attr("placeholder", "请输入优惠金额");
        $('#Text8').attr("placeholder", "请输入优惠金额");
        $('#a1').empty();
        $('#da1').empty();
        $('#da2').empty();
        $('#a2').empty();
        $('#da3').empty();
        $('#da4').empty();
    } else {
        v = "赠";
        $('#SumMoney2').prop("readonly", true);
        $('#Text2').prop("readonly", true);
        $('#Text4').prop("readonly", true);
        $('#SumQuality2').prop("readonly", true);
        $('#Text6').prop("readonly", true);
        $('#Text8').prop("readonly", true);
        $('#SumMoney2').attr("placeholder", "请选择赠品");
        $('#Text2').attr("placeholder", "请选择赠品");
        $('#Text4').attr("placeholder", "请选择赠品");
        $('#SumQuality2').attr("placeholder", "请选择赠品");
        $('#Text6').attr("placeholder", "请选择赠品");
        $('#Text8').attr("placeholder", "请选择赠品");
        $('#a1').empty();
        $('#da1').empty();
        $('#da2').empty();
        $('#a2').empty();
        $('#da3').empty();
        $('#da4').empty();
        var div1 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('SumMoney2')\"><span>选择内容</span></a></div>";
        var div2 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('Text2')\"><span>选择内容</span></a></div>";
        var div3 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('Text4')\"><span>选择内容</span></a></div>";
        var div4 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('SumQuality2')\"><span>选择内容</span></a></div>";
        var div5 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('Text6')\"><span>选择内容</span></a></div>";
        var div6 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('Text8')\"><span>选择内容</span></a></div>";
        $('#a1').append(div1);
        $('#da1').append(div2);
        $('#da2').append(div3);
        $('#a2').append(div4);
        $('#da3').append(div5);
        $('#da4').append(div6);
    }
    document.getElementById('fs1').innerHTML = v;
    document.getElementById('fs2').innerHTML = v;
    //document.getElementById('fs3').innerHTML = v;
    document.getElementById('Span1').innerHTML = v;
    document.getElementById('Span2').innerHTML = v;
    document.getElementById('Span3').innerHTML = v;
    document.getElementById('Span4').innerHTML = v;
}
//赠品选择按钮
function cli(id) {
    var ID = id;
    open(ID);
}
//赠品选择弹框
function open(id) {
    document.getElementById('Div1').style.display = "block";
    document.getElementById('hiddenID').value = id;
    var UserType = $('input[name="hdsz"]:checked').val();
    //选择赠品
    if (UserType == "FZ") {
        Premium(1);
    }
        //选择优惠券
    else if (UserType == "FC") {
        GetCoupon(1);
    }
}
//搜索赠品
function keywordPremium() {
    var UserType = $('input[name="hdsz"]:checked').val();
    //选择赠品
    if (UserType == "FZ") {
        Premium(1);
    }
    //选择优惠券
    else if (UserType == "FC") {
        GetCoupon(1);
    }
}
function closeDia() {
    document.getElementById('Div1').style.display = 'none';
}
//赠品查询
function Premium(index) {
    var gdCode = $('#Text9').val();
    var kyWord = $('#Text10').val();
    var pgSize = 8;
    var pgIndex = index;
    $('#Table1').empty();
    var td = " <thead><tr><th>选择</th><th>编号</th><th>商品编码</th><th>商品名称</th><th>容量</th><th>单位</th><th>数量</th><th>标记</th></tr></thead><tbody>";
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + 'Goods/PremiumList?goodCode=' + gdCode + '&keyword=' + kyWord + '&pageSize=' + pgSize + '&pageIndex=' + pgIndex,
        dataType: 'json',
        success: function (data) {
            var index1 = data.PageIndex;
            var page = data.PageSize;
            var total = data.TotalCount;
            var Source = data.Source;
            setRecordCount('zp', page, total, index1);
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    td += "<tr style='text-align:center'>"
                    + "<td> <input type=\"radio\" name=\"premium\" value=\"" + Source[i]["goodscode"] + "\"></td>"
                    + "<td>" + Source[i]["goodsid"] + "</td>"
                    + "<td>" + Source[i]["goodscode"] + "</td>"
                    + "<td>" + Source[i]["goodsname"] + "</td>"
                    + "<td>" + Source[i]["drug_spec"] + "</td>"
                    + "<td>" + Source[i]["package_unit"] + "</td>"
                    + "<td>" + Source[i]["quantity"] + "</td>"
                    + "<td>" + Source[i]["is_zx"] + "</td></tr></tbody>";
                }
                $('#Table1').append(td);
            } else {
                td += "<tr><td colspan='8' style='text-align: center;'>暂无内容</td></tr></tbody>";
                $('#Table1').append(td);
            }
        }
    });
}
//查询优惠券
function GetCoupon(pageIndex)
{
    openLoading();
    var kelb = $("#itemlist").val();
    if (kelb == "") {
        alertFun("请选择客户类型", function () { closeLoading() }, 'f');
    }
    else {
        var sqlvalue = $('#keyword').val();
        var pageSize = 8;
        var index = pageIndex;
        var data = {
            type: "CXGetCoupon",
            UserType:kelb,
            sqlvalue: sqlvalue,
            PageIndex: index,
            pageSize: pageSize,
        };
        var proc = "Proc_Coupons_Admin";//存储过程名
        var type = "ReturnList";

        $.ajax({
            type: 'Post',
            cache: false,
            url: "ashx/returnJson.ashx?json=" + encodeURIComponent(JSON.stringify(data)) + "&type=" + encodeURIComponent(type) + "&proc=" + encodeURIComponent(proc),
            dataType: "json",
            success: function (data) {
                var JSON = data;
                var TYPE = JSON["return_code"];
                var count = JSON["count"];
                if (TYPE == '0') {
                    $('#Table1').empty();
                    var tableHtml = " <thead><tr><th>选择</th><th>编号</th><th>名称</th><th>类别</th><th>满足金额</th><th>优惠金额</th><th>用户分类</th><th>优惠券类型</th><th>开始时间</th><th>截止时间</th><th>可用数量</th></tr></thead><tbody>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tableHtml += "<tr style='text-align:center'>"
                       + "<td> <input type=\"radio\" name=\"premium\" value=\"" + JSON["data"][i]["Code"] + "\"></td>"
                       + "<td>" + JSON["data"][i]['Code'] + "</td>"
                       + "<td>" + JSON["data"][i]['CouponName'] + "</td>"
                       + "<td>" + JSON["data"][i]['CouponContent'] + "</td>"
                       + "<td>" + JSON["data"][i]['MeetCount'] + "</td>"
                       + "<td>" + JSON["data"][i]['DisCount'] + "</td>"
                       + "<td>" + JSON["data"][i]['jibie'] + "</td>"//PriceRelation
                       + "<td>" + JSON["data"][i]['UserType'] + "</td>"
                       + "<td>" + JSON["data"][i]['StartDate'] + "</td>"
                       + "<td>" + JSON["data"][i]['EndDate'] + "</td>"
                       + "<td>" + JSON["data"][i]['Amount'] + "</td>"
                       + "</tr>"
                    }
                    $('#Table1').append(tableHtml);
                    var recordCount = JSON["recordCount"];
                    var pageCount = JSON["pageCount"];
                    closeLoading();
                    setRecordCount("zp", pageCount, recordCount, index);
                }
                if (TYPE == '1') {
                    closeLoading()
                    $("#Table1").empty();
                    var tableHtml = " <thead><tr><th>编号</th><th>名称</th><th>类别</th><th>满足金额</th><th>优惠金额</th><th>用户分类</th><th>优惠券类型</th><th>开始时间</th><th>截止时间</th><th>可用数量</th></tr></thead><tbody>";
                    tableHtml += "<tr><td colspan='11' style='text-align: center;'>暂无内容</td></tr>";
                    $('#Table1').append(tableHtml);
                }
                if (TYPE == '2') {
                    alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                closeLoading();
            }
        });
    }
}
//规则新增页面获取赠品code
function code() {
    var co = $("input[name='premium']:checked").val();
    if (co != "" && co != null) {
        var hi = $('#hiddenID').val();
        document.getElementById(hi).value = co;
        closeDia();
        if (hi == "SumMoney2") {
            $('#Dd1').show();
        } else if (hi == "Text2") {
            $('#Dd2').show();
        } else if (hi == "SumQuality2") {
            $('#Dd3').show();
        } else if (hi == "Text6") {
            $('#Dd4').show();
        }
    }
    else {
        alertFun("请选择", function () { },'w')
    }
}
//返回客户级别
function returnKHJB() {
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + 'Promotion/PriceRelation',
        dataType: 'json',
        success: function (data) {
            var source = data.Source;
            $('#itemlist').empty();
            var list = "";
            if (source.length>0) {
                for (var i = 0; i < source.length; i++) {
                    list += "<option value=\"" + source[i]["pricelev"] + "\">" + source[i]["jibie"] + "</option>";//pricelev  companyid
                }
                $('#itemlist').append(list);
            } else {
                list += "<option>" + 暂无数据 + "</option>";
                $('#itemlist').append(list);
            }
        }
    });
}