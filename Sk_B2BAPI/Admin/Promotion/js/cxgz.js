
//加载规则数据
function getGZ(pageIndex) {
    var iU = $("input[name='cxgz']:checked").val();
    var kd = $('#keyword').val();
    var size = 15;
    var index = pageIndex;
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/PromotionList?keyword=" + kd + "&isUse=" + iU + "&pageSize=" + size + "&pageIndex=" + index,
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var index1 = data.PageIndexXZGZ
            var page = data.PageSize;
            var total = data.TotalCount;
            var Source = data.Source;
            setRecordCount("0", page, total, index);
            $("#tbd").empty();
            var tableHtml = "";
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    tableHtml += "<tr style='text-align:center'>"
                    + "<td>" + Source[i]['PromoCode'] + "</td>"
                    + "<td>" + Source[i]['PromName'] + "</td>"
                    + "<td>" + Source[i]['MeetCount'] + "</td>"
                    + "<td>" + Source[i]['Discount'] + "</td>"
                    + "<td>" + Source[i]['MeetCount2'] + "</td>"
                    + "<td>" + Source[i]['Discount2'] + "</td>"
                    + "<td>" + Source[i]['MeetCount3'] + "</td>"
                    + "<td>" + Source[i]['DisCount3'] + "</td>";
                    if (Source[i]['PromType'] == "SingleGoods") {
                        tableHtml += "<td>" + "单品" + "</td>";
                    }
                    else if (Source[i]['PromType'] == "Brand") {
                        tableHtml += "<td>" + "品牌" + "</td>";
                    }
                    else if (Source[i]['PromType'] == "allGoods") {
                        tableHtml += "<td>" + "全部" + "</td>";
                    }
                    else if (Source[i]['PromType'] == "CustomsGroup") {
                        tableHtml += "<td>" + "客户" + "</td>";
                    }
                    else if (Source[i]['IsUse']) {
                        tableHtml += "<td>" + "是" + "</td>";
                    } else {
                        tableHtml += "<td>" + "否" + "</td>";
                    }

                    if (Source[i]['ContentType'] == "SumMoney") {
                        tableHtml += "<td>" + "满足指定金额" + "</td>";
                    }
                   else if (Source[i]['ContentType'] == "SumQuality") {
                        tableHtml += "<td>" + "满足指定件数" + "</td>";
                    }
                   else if (Source[i]['ContentType'] == "ArriveNum") {
                        tableHtml += "<td>" + "累计满足指定件数" + "</td>";
                    }
                    if (Source[i]['ManuFacturer'] == null) {
                        tableHtml += "<td>" + "-" + "</td>";
                    } else {
                        tableHtml += "<td>" + Source[i]['ManuFacturer'] + "</td>";
                    }
                    if (Source[i]['PromRule'] == "Discount") {
                        tableHtml += "<td>" + "折扣" + "</td>";
                    }
                    else if (Source[i]['PromRule'] == "FJ") {
                        tableHtml += "<td>" + "满减" + "</td>";
                    }
                    else if (Source[i]['PromRule'] == "FZ") {
                        tableHtml += "<td>" + "满赠" + "</td>";
                    }
                    else if (Source[i]['PromRule'] == "FC") {
                        tableHtml += "<td>" + "满赠(优惠券)" + "</td>";
                    }
                    tableHtml += "<td>" + Source[i]["jibie"] + "</td>";
                    if (Source[i]['IsUse']) {
                        tableHtml += "<td><a href=\"javascript:void(0);\" onclick=\"Effective('" + Source[i]['PromoCode'] + "','" + Source[i]['IsUse'] + "')\"><span>取消发布</span></td></tr>";
                    }
                    else {
                        if ((Source[i]['MeetCount3'] == "" || Source[i]['MeetCount3'] == null || typeof (Source[i]['MeetCount3']) == undefined) || (Source[i]['DisCount3'] == "" || Source[i]['DisCount3'] == null || typeof (Source[i]['DisCount3']) == undefined)) {

                            if ((Source[i]['MeetCount2'] == "" || Source[i]['MeetCount2'] == null || typeof (Source[i]['MeetCount2']) == undefined) || (Source[i]['Discount2'] == "" || Source[i]['Discount2'] == null || typeof (Source[i]['Discount2']) == undefined)) {
                                
                                tableHtml += "<td><a href=\"javascript:void(0);\" onclick=\"Effective('" + Source[i]['PromoCode'] + "','" + Source[i]['IsUse'] + "')\"><span>发布|</span></a><a id=\"upId\" href=\"javascript:void(0);\"onclick=\"UpdateDialog1('" + Source[i]['PromoCode'] + "','" + Source[i]['PromName'] + "','" + Source[i]['MeetCount'] + "','" + Source[i]['Discount'] + "','" + Source[i]['PromType'] + "','" + Source[i]['ContentType'] + "','" + Source[i]['Content'] + "','" + Source[i]['ManuFacturer'] + "','" + Source[i]['PromRule'] + "')\"><span>修改|</span></a><a href='javascript:void(0);' onclick=\"removeGoods('" + Source[i]['PromName'] + "','" + Source[i]['PromoCode'] + "')\"><span>排除商品|</span></a><a href='javascript:void(0);' onclick=\"removeGZ('" + Source[i]['PromoCode'] + "')\"><span>删除</span></a></td></tr>";
                            } else {
                                tableHtml += "<td><a href=\"javascript:void(0);\" onclick=\"Effective('" + Source[i]['PromoCode'] + "','" + Source[i]['IsUse'] + "')\"><span>发布|</span></a><a id=\"upId\" href=\"javascript:void(0);\"onclick=\"UpdateDialog2('" + Source[i]['PromoCode'] + "','" + Source[i]['PromName'] + "','" + Source[i]['MeetCount'] + "','" + Source[i]['Discount'] + "','" + Source[i]['PromType'] + "','" + Source[i]['ContentType'] + "','" + Source[i]['Content'] + "','" + Source[i]['ManuFacturer'] + "','" + Source[i]['PromRule'] + "','" + Source[i]['MeetCount2'] + "','" + Source[i]['Discount2'] + "')\"><span>修改|</span></a><a href='javascript:void(0);' onclick=\"removeGoods('" + Source[i]['PromName'] + "','" + Source[i]['PromoCode'] + "')\"><span>排除商品|</span></a><a href='javascript:void(0);' onclick=\"removeGZ('" + Source[i]['PromoCode'] + "')\"><span>删除</span></a></td></tr>";
                            }
                        } else {
                            tableHtml += "<td><a href=\"javascript:void(0);\" onclick=\"Effective('" + Source[i]['PromoCode'] + "','" + Source[i]['IsUse'] + "')\"><span>发布|</span></a><a id=\"upId\" href=\"javascript:void(0);\"onclick=\"UpdateDialog3('" + Source[i]['PromoCode'] + "','" + Source[i]['PromName'] + "','" + Source[i]['MeetCount'] + "','" + Source[i]['Discount'] + "','" + Source[i]['PromType'] + "','" + Source[i]['ContentType'] + "','" + Source[i]['Content'] + "','" + Source[i]['ManuFacturer'] + "','" + Source[i]['PromRule'] + "','" + Source[i]['MeetCount2'] + "','" + Source[i]['Discount2'] + "','" + Source[i]['MeetCount3'] + "','" + Source[i]['DisCount3'] + "')\"><span>修改|</span></a><a href='javascript:void(0);' onclick=\"removeGoods('" + Source[i]['PromName'] + "','" + Source[i]['PromoCode'] + "')\"><span>排除商品|</span></a><a href='javascript:void(0);' onclick=\"removeGZ('" + Source[i]['PromoCode'] + "')\"><span>删除</span></a></td></tr>";
                        }
                    }
                }
                $('#tbd').append(tableHtml);
            }
            else {
                $("#tbd").empty();
                tableHtml += "<tr><td colspan='11' style='text-align: center;'>暂无内容</td></tr>";
                $('#tbd').append(tableHtml);
            }
        }
    });
}
//排除商品弹框
function removeGoods(PromName, PromCode) {
    document.getElementById('Div1').style.display = 'block';
    document.getElementById("hiddenCode").value = PromCode;
    //var proName = PromName; 
    var proCode = PromCode;
    //searchRemoveGoods(1, proCode);
    QueryYPcsp(1);
}
//排除商品查询
function searchRemoveGoods(pindex, proCode) {
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/ExcludeGoods?&promoCode=" + proCode,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var Source = data.Source;
            $("#tbody").empty();
            var tableHtml = "";
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    tableHtml += "<tr style='text-align:center'>"
                        + "<td><input type=\"checkbox\" name=\"check\"/></td>"
                    + "<td>" + Source[i]['PromoCode'] + "</td>"
                    + "<td>" + Source[i]['GoodsNo'] + "</td>";
                    //tableHtml += "<td><a href=\"javascript:void(0);\" onclick=\"delectRemoveGoods('" + Source[i]['PromoCode'] + "','" + Source[i]['GoodsNo'] + "')\"><span>删除</span></a></td></tr>";
                }
                $('#tbody').append(tableHtml);
            }
            else {
                $("#tbody").empty();
                tableHtml += "<tr><td colspan='3' style='text-align: center;'>暂无内容</td></tr>";
                $('#tbody').append(tableHtml);
            }
        }
    });
}
//打开商品选择弹框
function addGoods() {
    var hiddenCode = $('#hiddenCode').val();
    document.getElementById('Div3').style.display = 'block';
    document.getElementById("hiddenCode1").value = hiddenCode;
    QueryPcsp(1);
}
//商品选择弹框--商品查询
function goodsSearch1(index) {
    var gdCode = $('#Text2').val();
    var kyWord = $('#Text1').val();
    var pgSize = 8;
    var pgIndex = index;
    $('#tbodyContent').empty();
    //$("'#" + position + "'").empty();
    var tableHtml = "";
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
            setRecordCount("1", page, total, index1);
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    tableHtml = tableHtml + "<tr style='text-align:center'>"
                    + "<td><input style=\"width:30px\" type=\"checkbox\" name=\"checkGoods\" value='" + Source[i]["sub_title"] + "'/></td>"
                    + "<td>" + Source[i]["article_id"] + "</td>"
                    + "<td>" + Source[i]["sub_title"] + "</td>"
                    + "<td>" + Source[i]["approval_number"] + "</td>"
                    + "<td>" + Source[i]["factories_choosing"] + "</td>"
                    + "</tr>"
                } //tableHtml = tableHtml + "</tbody>";
                $('#tbodyContent').append(tableHtml);
            }
            else {
                $('#tbodyContent').empty();
                tableHtml = tableHtml + "<tr><td colspan='6' style='text-align: center;'>暂无内容</td></tr>"
                $('#tbodyContent').append(tableHtml);
            }
        }
    });
}
//排除商品新增弹框--确定
function confirm1() {
    var proCode = $('#hiddenCode1').val();
    var tbodyObj = document.getElementById('tbodyContent');
    var list = [];
    $("input[name='checkGoods']").each(function (key, value) {
        if ($(value).prop('checked')) {
            var code = $(this).val();
            var r = "r" + key;
            r = { GoodsNo: code, PromoCode: proCode };
            list.push(r);
        }
    })
    var msg = JSON.stringify(list);
    insertRemoveGoods(msg);
}
//排除商品新增弹框--取消
function closeGoods() {
    document.getElementById('Div3').style.display = 'none';
}
//插入排除的商品
function insertRemoveGoods(msg) {
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + 'Promotion/PromotionExcludeGoods',
        contentType: "application/json",
        data: msg,
        dataType: 'json',
        success: function (data) {
            if (data.Code == "0") {
                alert("添加成功");
                closeGoods();
                var proCode = parseInt($('#hiddenCode').val());
                //searchRemoveGoods(1, proCode);
                QueryYPcsp(1);
            } else {
                alert("添加失败。"+data.Message);
            }
        }
    });
}
//删除排除的商品
function delectRemoveGoods() {
    var proCode = parseInt($('#hiddenCode').val());
    var tbodyObj = document.getElementById('tbody'); //Table1
    var list1 = [];
    $("input[name='check']").each(function (key, value) {
        if ($(value).prop('checked')) {
            var code = $(this).val();
            var r = "r" + key;
            r = { GoodsNo: code, PromoCode: proCode };
            list1.push(r);
        }
    })
    var msg = JSON.stringify(list1);
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + 'Promotion/PromotionExcludeGoodsDelete',
        contentType: "application/json",
        data: msg,
        dataType: 'json',
        success: function (data) {
            if (data.Code == "99") {
                alert("删除失败");
                //alert(data.Message);
            } else {
                alert("删除成功");
                var proCode = $('#hiddenCode').val();
                QueryYPcsp(1);
            }
        }
    });
}
//排除商品弹框-关闭按钮
function cancel() {
    document.getElementById('Div1').style.display = 'none';
}
//排除商品弹框-确定按钮
function ensure() {
}
//首页
function firstPage(m) {
    if (m == "0") {
        getGZ(1);
    } else if (m == "zp") {
        Premium(1);
    }
    else if (m == 9)
    {
        QueryPcsp(1);
    }
    else if (m == 10) {
        QueryYPcsp(1);
    }
    else {
        var category = $('input[name=hddx]:checked').val();
        if (category == "SingleGoods") {
            goodsSearch(1);
        } else if (category == "Brand") {
            brandSearch(1);
        } else {
            return false;
        }
    }
}

//末页
function lastPage(m) {
    if (m == "0") {
        var count = document.getElementById('pageCount_list').innerHTML;
        var pgCount = parseInt(count);
        getGZ(pgCount);
    } else if (m == 9)
    {
        var pgCount = parseInt($("#XPage").html());
        QueryPcsp(pgCount);
    }
    else if (m == 10) {
        var pgCount = parseInt($("#XXPage").html());
        QueryYPcsp(pgCount);
    }
    else if (m == "zp") {
        var count = document.getElementById('B16').innerHTML;
        var pgCount = parseInt(count);
        Premium(pgCount);
    } else {
        var count = document.getElementById('B4').innerHTML;
        var pgCount = parseInt(count);
        var category = $('input[name=hddx]:checked').val();
        if (category == "SingleGoods") {
            goodsSearch(pgCount);
        } else if (category == "Brand") {
            brandSearch(pgCount);
        } else {
            return false;
        }
    }

}

//下一页
function nextPage(m) {
    if (m == "0") {
        var index = document.getElementById('pageIndex_list').innerHTML;
        var count = document.getElementById('pageCount_list').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        getGZ(pgNum);
    } else if (m == "zp") {
        var index = document.getElementById('B15').innerHTML;
        var count = document.getElementById('B16').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        Premium(pgNum);
    }
    else if (m == 9)
    {
        var index = document.getElementById('XIndex').innerHTML;
        var count = document.getElementById('XPage').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        QueryPcsp(pgNum);
    }
    else if (m == 10) {
        var index = document.getElementById('XXIndex').innerHTML;
        var count = document.getElementById('XXPage').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        QueryYPcsp(pgNum);
    }
    else {
        var index = document.getElementById('B3').innerHTML;
        var count = document.getElementById('B4').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        var category = $('input[name=hddx]:checked').val();
        if (category == "SingleGoods") {
            goodsSearch(pgNum);
        } else if (category == "Brand") {
            brandSearch(pgNum);
        } else {
            return false;
        }
    }
}

//上一页
function prePage(m) {
    if (m == "0") {
        var index = document.getElementById('pageIndex_list').innerHTML;
        var pgNum = parseInt(index);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        getGZ(pgNum);
    } else if (m == "zp") {
        var index = document.getElementById('B15').innerHTML;
        var count = document.getElementById('B16').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        Premium(pgNum);
    }
    else if (m == 9)
    {
        var index = document.getElementById('XIndex').innerHTML;
        var count = document.getElementById('XPage').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        QueryPcsp(pgNum);
    }
    else if (m == 10) {
        var index = document.getElementById('XXIndex').innerHTML;
        var count = document.getElementById('XXPage').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        QueryYPcsp(pgNum);
    }
    else {
        var index = document.getElementById('B3').innerHTML;
        var pgNum = parseInt(index);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        var category = $('input[name=hddx]:checked').val();
        if (category == "SingleGoods") {
            goodsSearch(pgNum);
        } else if (category == "Brand") {
            brandSearch(pgNum);
        } else {
            return false;
        }
    }
}

//显示总页数、当前页、总条数
function setRecordCount(m, page, total, index1) {
    if (m == "0") {
        document.getElementById('pageSize_list').innerHTML = page;
        document.getElementById('recordCount_list').innerHTML = total;
        document.getElementById('pageIndex_list').innerHTML = index1;
 
        if (parseInt(total / page) > 0 && total % page == 0) {
            document.getElementById('pageCount_list').innerHTML = parseInt(total / page);
        }
        if (parseInt(total / page) >= 0 && total % page > 0) {
            document.getElementById('pageCount_list').innerHTML = Math.ceil(total / page);
        }
    } else if (m == "zp") {
        document.getElementById('B13').innerHTML = page;
        document.getElementById('B14').innerHTML = total;
        document.getElementById('B15').innerHTML = index1;
        if (parseInt(total / page) > 0 && total % page == 0) {
            document.getElementById('B16').innerHTML = parseInt(total / page);
        }
        if (parseInt(total / page) >= 0 && total % page > 0) {
            document.getElementById('B16').innerHTML = Math.ceil(total / page);
        }
    } else if (m == 9)
    {
        document.getElementById('XPage').innerHTML = page;
        document.getElementById('XCount').innerHTML = total;
        document.getElementById('XIndex').innerHTML = index1;
    }
    else if (m == 10) {
        document.getElementById('XXPage').innerHTML = page;
        document.getElementById('XXCount').innerHTML = total;
        document.getElementById('XXIndex').innerHTML = index1;
    }
    else {
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
}

//遍历radio绑定click
$(function () {
    $('#Dd1').hide();
    $('#Dd3').hide();
    $('#Dd4').hide();
    $('#Dd2').hide();
    $('#Dd5').hide();
    $('#Dd6').hide();
    $('#d6').hide();
    document.getElementById('Div7').style.display = 'none';
    //初始化表单验证
    //$('#form2').initValidform();
    getGZ(1);
    //upde();
    $.each($("input[name=cxgz]"), function (index) {
        //这里就是对单选按钮的单击事件，需要一个each，遍历出所有的radio,然后对每个radio绑定一个click
        $(this).click(function () {
            getGZ(1);
        });
    });
    //优惠类型变动
    $.each($("input[name=hdsz]"), function (index) {
        //这里就是对单选按钮的单击事件，需要一个each，遍历出所有的radio,然后对每个radio绑定一个click
        $(this).click(function () {
            changeFavourable();
        });
    });

    $(":checkbox").click(function () {
        if ($(this).attr("checked") != undefined) {
            $(this).siblings().attr("checked", false);
            $(this).attr("checked", true);
        }
    });
    //优惠对象
    $.each($("input[name='hddx']"), function (index) {
        $(this).click(function () {
            var d = $("input[name='hddx']:checked").val();
            if (d == "Brand") {
                $('#d6').show();
            } else {
                $('#d6').hide();
            }
        });
    });


    //优惠方式变动
    $.each($("input[name='hdjg']"), function (index) {
        $(this).click(function () {
            var s = $("input[name='hdjg']:checked").val();
            if (s == "SumMoney") {
                $('#Dd2').hide();
                $('#Dd5').hide();
                $('#Dd6').hide();
                $('#Dd3').hide();
                $('#Dd4').hide();
                $('#Dd1').show();
                //$('#Dd3').show();
                //$('#Dd4').show();
                document.getElementById('SumQuality1').value = "";
                document.getElementById('SumQuality2').value = "";
            } else if (s == "SumQuality") {
                $('#Dd1').hide();
                $('#Dd3').hide();
                $('#Dd4').hide();
                $('#Dd5').hide();
                $('#Dd6').hide();
                //$('#Dd5').show();
                //$('#Dd6').show();
                $('#Dd2').show();
                document.getElementById('SumMoney1').value = "";
                document.getElementById('SumMoney2').value = "";
            }
        });
    });
});
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

//关键词搜索
function gz_search() {
    getGZ(1);
}

//删除规则操作
function removeGZ(ProCode) {
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/PromotionDelete",
        data: { PromoCode: ProCode },
        dataType: "json",
        success: function (data) {
            if (data.Code == "0") {
                alert("删除成功！");
                var index = document.getElementById('pageIndex_list').innerHTML;
                getGZ(index);
            }
            else if (data.Code == "99") {
                alert("删除失败" + data.Message);
            }
            else if (data.Code == "5") {
                alert("删除失败"+ data.Message);
            }
        }
    });
}

//清空更新框数据
function empty() {
    document.getElementById('updateCode').value = "";//编码
    document.getElementById('updateName').value = "";//名称

    document.getElementById('fs1').innerHTML = "";
    document.getElementById('fs2').innerHTML = "";

    document.getElementById('SumMoney1').value = "";
    document.getElementById('SumMoney2').value = "";
    document.getElementById('SumQuality1').value = "";
    document.getElementById('SumQuality2').value = "";

    document.getElementById('Text3').value = "";
    document.getElementById('Text4').value = "";
    document.getElementById('Text5').value = "";
    document.getElementById('Text6').value = "";
    document.getElementById('Text7').value = "";
    document.getElementById('Text8').value = "";
    document.getElementById('Text9').value = "";
    document.getElementById('Text10').value = "";

    document.getElementById('updateContent').value = "";
}

//UpdateDialog1（弹出更新框并加载数据）
function UpdateDialog1(ProCode, ProName, MtCount, Discount, Protype, ConType, Content, ManuFacturer, ProRule) {//, Discount2, DisCount3
    document.getElementById('updatePage').style.display = 'block';
    empty();
    document.getElementById('updateCode').value = ProCode;//编码
    document.getElementById('updateName').value = ProName;//名称

    if (ProRule == "Discount") {//打折Discount
        $('#mj').prop("checked", false);
        $('#mz').prop("checked", false);
        $('#zk').prop("checked", true);
    } else if (ProRule == "FJ") {//减MJO
        $('#zk').prop("checked", false);
        $('#mz').prop("checked", false);
        $('#mj').prop("checked", true);
    } else {//赠MZ
        $('#zk').prop("checked", false);
        $('#mj').prop("checked", false);
        $('#mz').prop("checked", true);
    }
    changeFavourable();
    if (ConType == "SumMoney") {//满多少元 SumMoney
        $('#Dd2').hide();
        $('#Dd5').hide();
        $('#Dd6').hide();
        $('#Dd1').show();
        $('#SumQuality').prop("checked", false);
        $('#SumMoney').prop("checked", true);
        document.getElementById('SumMoney1').value = MtCount;
        document.getElementById('SumMoney2').value = Discount;
        if ((MtCount == "" || MtCount == null || typeof (MtCount) == undefined) || (Discount == "" || Discount == null || typeof (Discount) == undefined)) {
            $('#Dd3').hide();
        } else {
            $('#Dd3').show();
            //document.getElementById('Text3').value = MtCount2;
            //document.getElementById('Text4').value = Discount2;
        }
    }
    if (ConType == "SumQuality") {//满多少件 SumQuality
        $('#Dd1').hide();
        $('#Dd4').hide();
        $('#Dd3').hide();
        $('#Dd2').show();
        $('#SumMoney').prop("checked", false);
        $('#SumQuality').prop("checked", true);
        document.getElementById('SumQuality1').value = MtCount;
        document.getElementById('SumQuality2').value = Discount;
        if ((MtCount == "" || MtCount == null || typeof (MtCount) == undefined) || (Discount == "" || Discount == null || typeof (Discount) == undefined)) {
            $('#Dd5').hide();
        } else {
            $('#Dd5').show();
            //document.getElementById('Text7').value = MtCount2;
            //document.getElementById('Text8').value = Discount2;
        }
    }
    if (Protype == "SingleGoods") {//单品
        $('#Brand').prop("checked", false);
        $('#CustomsGroup').prop("checkde", false);
        $('#allGoods').prop("checked", false);
        $('#SingleGoods').prop("checked", true);
    } else if (Protype == "Brand") {//品牌
        $('#SingleGoods').prop("checked", false);
        $('#CustomsGroup').prop("checkde", false);
        $('#allGoods').prop("checked", false);
        $('#Brand').prop("checked", true);
        $('#d6').show();
    } else if (Protype == "AllGoods") {
        $('#Brand').prop("checked", false);
        $('#CustomsGroup').prop("checkde", false);
        $('#SingleGoods').prop("checked", false);
        $('#allGoods').prop("checked", true);
    }
    else {//客户分组
        $('#SingleGoods').prop("checked", false);
        $('#Brand').prop("checked", false);
        $('#allGoods').prop("checked", false);
        $('#CustomsGroup').prop("checkde", "check");
    }
    document.getElementById('updateContent').value = ManuFacturer;
    document.getElementById('hidden').value = Content;
}
//UpdateDialog2（弹出更新框并加载数据）
function UpdateDialog2(ProCode, ProName, MtCount, Discount, Protype, ConType, Content, ManuFacturer, ProRule, MtCount2, Discount2) {//, Discount2, DisCount3
    document.getElementById('updatePage').style.display = 'block';
    empty();
    document.getElementById('updateCode').value = ProCode;//编码
    document.getElementById('updateName').value = ProName;//名称

    if (ProRule == "Discount") {//打折Discount
        $('#mj').prop("checked", false);
        $('#mz').prop("checked", false);
        $('#zk').prop("checked", true);
    } else if (ProRule == "FJ") {//减MJO
        $('#zk').prop("checked", false);
        $('#mz').prop("checked", false);
        $('#mj').prop("checked", true);
    } else {//赠MZ
        $('#zk').prop("checked", false);
        $('#mj').prop("checked", false);
        $('#mz').prop("checked", true);
    }
    changeFavourable();
    if (ConType == "SumMoney") {//满多少元 SumMoney
        $('#Dd2').hide();
        $('#Dd5').hide();
        $('#Dd6').hide();
        $('#Dd1').show();
        $('#SumQuality').prop("checked", false);
        $('#SumMoney').prop("checked", true);
        document.getElementById('SumMoney1').value = MtCount;
        document.getElementById('SumMoney2').value = Discount;
        if ((MtCount == "" || MtCount == null || typeof (MtCount) == undefined) || (Discount == "" || Discount == null || typeof (Discount) == undefined)) {
            $('#Dd3').hide();
        } else {
            $('#Dd3').show();
            document.getElementById('Text3').value = MtCount2;
            document.getElementById('Text4').value = Discount2;
        }
        if ((MtCount2 == "" || MtCount2 == null || typeof (MtCount2) == undefined) || (Discount2 == "" || Discount2 == null || typeof (Discount2) == undefined)) {
            $('#Dd4').hide();
        } else {
            $('#Dd4').show();
            //document.getElementById('Text5').value = MtCount3;
            //document.getElementById('Text6').value = DisCount3;
        }
    }
    if (ConType == "SumQuality") {//满多少件 SumQuality
        $('#Dd1').hide();
        $('#Dd4').hide();
        $('#Dd3').hide();
        $('#Dd2').show();
        $('#SumMoney').prop("checked", false);
        $('#SumQuality').prop("checked", true);
        document.getElementById('SumQuality1').value = MtCount;
        document.getElementById('SumQuality2').value = Discount;
        if ((MtCount == "" || MtCount == null || typeof (MtCount) == undefined) || (Discount == "" || Discount == null || typeof (Discount) == undefined)) {
            $('#Dd5').hide();
        } else {
            $('#Dd5').show();
            document.getElementById('Text7').value = MtCount2;
            document.getElementById('Text8').value = Discount2;
        }
        if ((MtCount2 == "" || MtCount2 == null || typeof (MtCount2) == undefined) || (Discount2 == "" || Discount2 == null || typeof (Discount2) == undefined)) {
            $('#Dd6').hide();
        } else {
            $('#Dd6').show();
            //document.getElementById('Text9').value = MtCount3; 
            //document.getElementById('Text10').value = DisCount2;
        }
    }
    if (Protype == "SingleGoods") {//单品
        $('#Brand').prop("checked", false);
        $('#CustomsGroup').prop("checkde", false);
        $('#allGoods').prop("checked", false);
        $('#SingleGoods').prop("checked", true);
    } else if (Protype == "Brand") {//品牌
        $('#SingleGoods').prop("checked", false);
        $('#CustomsGroup').prop("checkde", false);
        $('#allGoods').prop("checked", false);
        $('#Brand').prop("checked", true);
        $('#d6').show();
    } else if (Protype == "AllGoods") {
        $('#Brand').prop("checked", false);
        $('#CustomsGroup').prop("checkde", false);
        $('#SingleGoods').prop("checked", false);
        $('#allGoods').prop("checked", true);
    }
    else {//客户分组
        $('#SingleGoods').prop("checked", false);
        $('#Brand').prop("checked", false);
        $('#allGoods').prop("checked", false);
        $('#CustomsGroup').prop("checkde", "check");
    }
    document.getElementById('updateContent').value = ManuFacturer;
    document.getElementById('hidden').value = Content;
}
//UpdateDialog3（弹出更新框并加载数据）
function UpdateDialog3(ProCode, ProName, MtCount, Discount, Protype, ConType, Content, ManuFacturer, ProRule, MtCount2, Discount2, MtCount3, DisCount3) {//, Discount2, DisCount3
    document.getElementById('updatePage').style.display = 'block';
    empty();
    document.getElementById('updateCode').value = ProCode;//编码
    document.getElementById('updateName').value = ProName;//名称

    if (ProRule == "Discount") {//打折Discount
        $('#mj').prop("checked", false);
        $('#mz').prop("checked", false);
        $('#zk').prop("checked", true);
    } else if (ProRule == "FJ") {//减MJO
        $('#zk').prop("checked", false);
        $('#mz').prop("checked", false);
        $('#mj').prop("checked", true);
    } else {//赠MZ
        $('#zk').prop("checked", false);
        $('#mj').prop("checked", false);
        $('#mz').prop("checked", true);
    }
    changeFavourable();
    if (ConType == "SumMoney") {//满多少元 SumMoney
        $('#Dd2').hide();
        $('#Dd5').hide();
        $('#Dd6').hide();
        $('#Dd1').show();
        $('#SumQuality').prop("checked", false);
        $('#SumMoney').prop("checked", true);
        document.getElementById('SumMoney1').value = MtCount;
        document.getElementById('SumMoney2').value = Discount;
        if ((MtCount == "" || MtCount == null || typeof (MtCount) == undefined) || (Discount == "" || Discount == null || typeof (Discount) == undefined)) {
            $('#Dd3').hide();
        } else {
            $('#Dd3').show();
            document.getElementById('Text3').value = MtCount2;
            document.getElementById('Text4').value = Discount2;
        }
        if ((MtCount2 == "" || MtCount2 == null || typeof (MtCount2) == undefined) || (Discount2 == "" || Discount2 == null || typeof (Discount2) == undefined)) {
            $('#Dd4').hide();
        } else {
            $('#Dd4').show();
            document.getElementById('Text5').value = MtCount3;
            document.getElementById('Text6').value = DisCount3;
        }
    }
    if (ConType == "SumQuality") {//满多少件 SumQuality
        $('#Dd1').hide();
        $('#Dd4').hide();
        $('#Dd3').hide();
        $('#Dd2').show();
        $('#SumMoney').prop("checked", false);
        $('#SumQuality').prop("checked", true);
        document.getElementById('SumQuality1').value = MtCount;
        document.getElementById('SumQuality2').value = Discount;
        if ((MtCount == "" || MtCount == null || typeof (MtCount) == undefined) || (Discount == "" || Discount == null || typeof (Discount) == undefined)) {
            $('#Dd5').hide();
        } else {
            $('#Dd5').show();
            document.getElementById('Text7').value = MtCount2;
            document.getElementById('Text8').value = Discount2;
        }
        if ((MtCount2 == "" || MtCount2 == null || typeof (MtCount2) == undefined) || (Discount2 == "" || Discount2 == null || typeof (Discount2) == undefined)) {
            $('#Dd6').hide();
        } else {
            $('#Dd6').show();
            document.getElementById('Text9').value = MtCount3;
            document.getElementById('Text10').value = DisCount3;
        }
    }
    if (Protype == "SingleGoods") {//单品
        $('#Brand').prop("checked", false);
        $('#CustomsGroup').prop("checkde", false);
        $('#allGoods').prop("checked", false);
        $('#SingleGoods').prop("checked", true);
    } else if (Protype == "Brand") {//品牌
        $('#SingleGoods').prop("checked", false);
        $('#CustomsGroup').prop("checkde", false);
        $('#allGoods').prop("checked", false);
        $('#Brand').prop("checked", true);
        $('#d6').show();
    } else if (Protype == "AllGoods") {
        $('#Brand').prop("checked", false);
        $('#CustomsGroup').prop("checkde", false);
        $('#SingleGoods').prop("checked", false);
        $('#allGoods').prop("checked", true);
    }
    else {//客户分组
        $('#SingleGoods').prop("checked", false);
        $('#Brand').prop("checked", false);
        $('#allGoods').prop("checked", false);
        $('#CustomsGroup').prop("checkde", "check");
    }
    document.getElementById('updateContent').value = ManuFacturer;
    document.getElementById('hidden').value = Content;
}
//更新数据
function update() {
    var index = document.getElementById('pageIndex_list').innerHTML;
    var code = $('#updateCode').val();
    var name = $('#updateName').val();
    var hdsz = $('input[name="hdsz"]:checked').val();
    var hdjg = $('input[name="hdjg"]:checked').val();
    var hddx = $('input[name="hddx"]:checked').val();
    var text = document.getElementById('hidden').value;
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
            var value1 = $('#SumMoney1').val();
            var value2 = $('#SumMoney2').val();
            var text1 = parseInt($('#Text3').val());
            var text2 = $('#Text4').val();
            var text3 = parseInt($('#Text5').val());
            var text4 = $('#Text6').val();
            if ((value1 == "" || typeof (value1) == undefined || value1 == null) || (value2 == "" || typeof (value2) == undefined || value2 == null)) {
                alert("不能为空");
            } else if ((text1 == "" || typeof (text1) == undefined || text1 == null) || (text2 == "" || typeof (text2) == undefined || text2 == null)) {
                if (isNaN(value1)) {
                    alert("金额必须为数字");
                } else {
                    updata1(code, name, value1, value2, hddx, hdjg, text, hdsz);
                    closeDialog1();
                    getGZ(index);
                }
            } else if ((text3 == "" || typeof (text3) == undefined || text3 == null) || (text4 == "" || typeof (text4) == undefined || text4 == null)) {
                if (isNaN(text1)) {
                    alert("金额必须为数字");
                } else {
                    updata2(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2);
                    closeDialog1();
                    getGZ(index);
                }
            } else {
                if (isNaN(text3)) {
                    alert("金额必须为数字");
                } else {
                    updata3(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2, text3, text4);
                    closeDialog1();
                    getGZ(index);
                }
            }
        }
        else if (hdjg == "SumQuality") {
            var value1 = $('#SumQuality1').val();
            var value2 = $('#SumQuality2').val();
            var text1 = parseInt($('#Text7').val());
            var text2 = $('#Text8').val();
            var text3 = parseInt($('#Text9').val());
            var text4 = $('#Text10').val();
            if ((value1 == "" || typeof (value1) == undefined || value1 == null) || (value2 == "" || typeof (value2) == undefined || value2 == null)) {
                alert("不能为空");
            } else if ((text1 == "" || typeof (text1) == undefined || text1 == null) || (text2 == "" || typeof (text2) == undefined || text2 == null)) {
                if (isNaN(value1)) {
                    alert("金额必须为数字");
                } else {
                    updata1(code, name, value1, value2, hddx, hdjg, text, hdsz);
                    closeDialog1();
                    getGZ(index);
                }
            } else if ((text3 == "" || typeof (text3) == undefined || text3 == null) || (text4 == "" || typeof (text4) == undefined || text4 == null)) {
                if (isNaN(text1)) {
                    alert("金额必须为数字");
                } else {
                    updata2(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2);
                    closeDialog1();
                    getGZ(index);
                }
            } else {
                if (isNaN(text3)) {
                    alert("金额必须为数字");
                } else {
                    updata3(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2, text3, text4);
                    closeDialog1();
                    getGZ(index);
                }
            }
        }
    }

}
//规则更新（一条）
function updata1(code, name, value1, value2, hddx, hdjg, text, hdsz) {
    var isUse = "False";
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/PromotionUpdate",
        data: { PromoCode: code, PromName: name, MeetCount: value1, discount: value2, PromType: hddx, IsUse: isUse, ContentType: hdjg, Content: text, PromRule: hdsz },
        dataType: "json",
        success: function (data) {
            if (data.Code == "0") {
                alert('更新成功！');
                var index = document.getElementById('pageIndex_list').innerHTML;
                getGZ(index);
            } else {
                alert('更新失败！');
            }
        }
    });
}

//规则新增（两条）
function updata2(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2) {
    var isUse = "False";
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/PromotionUpdate",
        data: { PromoCode: code, PromName: name, MeetCount: value1, discount: value2, PromType: hddx, IsUse: isUse, ContentType: hdjg, Content: text, PromRule: hdsz, MeetCount2: text1, Discount2: text2 },
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
function updata3(code, name, value1, value2, hddx, hdjg, text, hdsz, text1, text2, text3, text4) {
    var isUse = "False";
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/PromotionUpdate",
        data: { PromoCode: code, PromName: name, MeetCount: value1, discount: value2, PromType: hddx, IsUse: isUse, ContentType: hdjg, Content: text, PromRule: hdsz, MeetCount2: text1, Discount2: text2, MeetCount3: text3, DisCount3: text4 },
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

//修改发布状态
function Effective(ProCode, IsU) {
    IsU = IsU == "true" ? "false" : "true";
    var index = document.getElementById('pageIndex_list').innerHTML;
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/PromotionEffective",
        data: { Code: ProCode, isUse: IsU },
        dataType: "json",
        success: function (data) {
            if (data.Code == "0") {
                alert("状态修改成功");
                getGZ(index);
            } else {
                alert(data.Message);
            }
        }
    });
}

//关闭Dialog1
function closeDialog1() {
    document.getElementById('updatePage').style.display = 'none';
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
        } else {
            document.getElementById('code').innerHTML = "";
            document.getElementById('name').innerHTML = "";
            //客户分组查询方法
        }
    }
}

//商品搜索 第n页：index    数据显示位置：position
function goodsSearch(index) {
    var gdCode = $('#gsCode').val();
    var kyWord = $('#gsWord').val();
    var pgSize = 9;
    var pgIndex = index;
    $('#contnet').empty();
    //$("'#" + position + "'").empty();
    var tableHtml = " <thead><tr><th>选择</th><th>编码</th><th>名称</th><th>批准文号</th><th>品牌名称</th><th>商品编号</th></tr></thead><tbody id=\"tbodyContent\">";
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
            setRecordCount("1", page, total, index1);
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    tableHtml = tableHtml + "<tr style='text-align:center'>"
                    + "<td><input type=\"radio\" name=\"shangpin\" value='" + Source[i]["sub_title"] + "'/></td>"
                    + "<td>" + Source[i]["article_id"] + "</td>"
                    + "<td>" + Source[i]["sub_title"] + "</td>"
                    + "<td>" + Source[i]["approval_number"] + "</td>"
                    + "<td>" + Source[i]["factories_choosing"] + "</td>"
                    + "<td>" + Source[i]["goods_no"] + "</td>"
                    + "</tr>"
                } tableHtml = tableHtml + "</tbody>";
                $('#contnet').append(tableHtml);
            }
            else {
                $('#contnet').empty();
                tableHtml = tableHtml + "<tr><td colspan='6' style='text-align: center;'>暂无内容</td></tr></tbody>"
                $('#contnet').append(tableHtml);
            }
        }
    });
}

//品牌搜索
function brandSearch(index) {
    var gdCode = $('#gsCode').val();
    var kyWord = $('#gsWord').val();
    var pgSize = 8;
    var pgIndex = index;
    $('#contnet').empty();
    var tableHtml = " <thead><tr><th>选择</th><th>品牌编码</th><th>企业</th><th>商标</th><th>商品编号</th><th>编号</th></tr></thead><tbody id=\"tbodyContent\">";
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
            setRecordCount("1", PageSize, Total, PageIndex);
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    tableHtml += "<tr style='text-align:center'>"
                    + "<td><input type=\"radio\" name=\"pinpai\" value='" + Source[i]["ManuFacturer"] + "'/></td>"
                    + "<td>" + Source[i]["Billno"] + "</td>"
                    //+ "<td>" + Source[i]["ImgUrl"] + "</td>"
                    //+ "<td><img style=\"width:80px;height:80px;\" src='" + Source[i]["ImgUrl"] + "'></td>"
                    + "<td>" + Source[i]["ManuFacturer"] + "</td>"
                    //+ "<td>" + Source[i]["entid"] + "</td>"
                    + "<td>" + Source[i]["ImgType"] + "</td>"
                    + "<td>" + Source[i]["is_zx"] + "</td>"
                    //+ "<td>" + Source[i]["ImgName"] + "</td>"
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

//确定按钮事件
function confirm() {
    var tbodyObj = document.getElementById('contnet');
    $("table :radio").each(function (key, value) {
        if ($(value).prop('checked')) {
            var billno = tbodyObj.rows[key].cells[1].innerHTML;
            var manuFacturer = tbodyObj.rows[key].cells[2].innerHTML;
            document.getElementById('hidden').value = billno;
            document.getElementById('updateContent').value = manuFacturer;
            closeDialog();
        }
    })
}

//选择内容中搜索按钮事件
function search() {
    var category = $('input[name="hddx"]:checked').val();
    if (category == "SingleGoods") {
        goodsSearch(1);
    } else if (category == "Brand") {
        brandSearch(1);
    } else {
        return false;
    }
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
        $('#Div5').empty();
        $('#da1').empty();
        $('#da2').empty();
        $('#Div6').empty();
        $('#da3').empty();
        $('#da4').empty();
    } else if (v == "FJ") {
        v = "减";
        $('#SumMoney2').prop("readonly", false);
        $('#Text2').prop("readonly", false);
        $('#Text4').prop("readonly", false);
        $('#SumQuality2').prop("readonly", false);
        $('#Text6').prop("readonly", false);
        $('#Text8').prop("readonly", false);
        $('#Div5').empty();
        $('#da1').empty();
        $('#da2').empty();
        $('#Div6').empty();
        $('#da3').empty();
        $('#da4').empty();
    } else {
        v = "赠";
        $('#SumMoney2').prop("readonly", true);
        $('#Text4').prop("readonly", true);
        $('#Text6').prop("readonly", true);
        $('#SumQuality2').prop("readonly", true);
        $('#Text8').prop("readonly", true);
        $('#Text10').prop("readonly", true);
        $('#Div5').empty();
        $('#da1').empty();
        $('#da2').empty();
        $('#Div6').empty();
        $('#da3').empty();
        $('#da4').empty();
        var div1 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('SumMoney2')\"><span>选择内容</span></a></div>";
        var div2 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('Text4')\"><span>选择内容</span></a></div>";
        var div3 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('Text6')\"><span>选择内容</span></a></div>";
        var div4 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('SumQuality2')\"><span>选择内容</span></a></div>";
        var div5 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('Text8')\"><span>选择内容</span></a></div>";
        var div6 = "<div class=\"anniu\" ><a href=\"javascript:void(0);\" onclick=\"cli('Text10')\"><span>选择内容</span></a></div>";
        $('#Div5').append(div1);
        $('#da1').append(div2);
        $('#da2').append(div3);
        $('#Div6').append(div4);
        $('#da3').append(div5);
        $('#da4').append(div6);
    }
    document.getElementById('fs1').innerHTML = v;
    document.getElementById('fs2').innerHTML = v;
    //document.getElementById('fs3').innerHTML = v;
    document.getElementById('Span3').innerHTML = v;
    document.getElementById('Span4').innerHTML = v;
    document.getElementById('Span5').innerHTML = v;
    document.getElementById('Span6').innerHTML = v;
}
//赠品选择按钮
function cli(id) {
    var ID = id; 
    open(ID); 
}
//赠品选择弹框
function open(id) {
    var ID = id;
    document.getElementById('Div7').style.display = "block";
    document.getElementById('hiddenID').value = ID;
    Premium(1);
}
//搜索赠品
function keywordPremium() {
    Premium(1);
}
function closeDia() {
    document.getElementById('Div7').style.display = 'none';
}
//赠品查询
function Premium(index) {
    var gdCode = $('#Text12').val();
    var kyWord = $('#Text11').val();
    var pgSize = 8;
    var pgIndex = index;
    $('#zpSelectTable').empty();
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
                $('#zpSelectTable').append(td);
            } else {
                td += "<tr><td colspan='8' style='text-align: center;'>暂无内容</td></tr></tbody>";
                $('#zpSelectTable').append(td);
            }
        }
    });
}
//规则新增页面获取赠品code
function code() {
    var co = $("input[name='premium']:checked").val();
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
