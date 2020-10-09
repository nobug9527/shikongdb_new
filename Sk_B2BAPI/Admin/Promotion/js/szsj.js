
$(function () {
    laydate.render({
        elem: '#startrq'
    });
    laydate.render({
        elem: '#endrq'
    });
    load(1);
    //初始化表单验证
    $('#form2').initValidform();
});

/*页面加载数据
 *返回促销生效时间控制列表
*/
function load(index) {
    var kyWord = document.getElementById('gzName').value;
    var IsUse = "all";
    var PageSize = 10;
    var PageIndex = index;
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + "PromoteTime/PromoteTimeList?keyword=" + kyWord + "&isUse=" + IsUse + "&pageSize=" + PageSize + "&pageIndex=" + PageIndex,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var index1 = data.PageIndex;
            var page = data.PageSize;
            var total = data.TotalCount;
            var Source = data.Source;
            setRecordCount("0", page, total, index1);
            $("#tbd").empty();
            var tableHtml = "";
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    tableHtml += "<tr style='text-align:center'>"
                    + "<td>" + Source[i]['Code'] + "</td>"
                    + "<td>" + Source[i]['TimeName'] + "</td>"
                    + "<td>" + Source[i]['PromoCode'] + "</td>"
                    + "<td>" + Source[i]['PromoName'] + "</td>";
                    if (Source[i]['IsUse']) {
                        tableHtml += "<td>" + "是" + "</td>";
                    }
                    else {
                        tableHtml += "<td>" + "否" + "</td>";
                    }
                    tableHtml += "<td>" + Source[i]['EffectiveStartDate'] + "</td>"
                    + "<td>" + Source[i]['EffectiveEndDate'] + "</td>"
                    + "<td>" + Source[i]['EffectiveStartTime'] + "</td>"
                    + "<td>" + Source[i]['EffectiveEndTime'] + "</td>";
                    if (Source[i]['IsForever']) {
                        tableHtml += "<td>" + "是" + "</td>";
                    }
                    else {
                        tableHtml += "<td>" + "否" + "</td>";
                    }
                    if (Source[i]['IsUse']) {
                        tableHtml += "<td><a href=\"javascript:void(0);\" onclick=\"change('" + Source[i]['Code'] + "','" + Source[i]['IsUse'] + "')\"><span>取消发布</span></td></tr>";
                    }
                    else {
                        tableHtml += "<td><a href=\"javascript:void(0);\" onclick=\"change('" + Source[i]['Code'] + "','" + Source[i]['IsUse'] + "')\"><span>发布|</span></a><a href=\"javascript:void(0);\" onclick=\"UpdateDialog('" + Source[i]['Code'] + "','" + Source[i]['TimeName'] + "','" + Source[i]['IsForever'] + "','" + Source[i]['EffectiveStartDate'] + "','" + Source[i]['EffectiveEndDate'] + "','" + Source[i]['EffectiveStartTime'] + "','" + Source[i]['EffectiveEndTime'] + "','" + Source[i]['PromoCode'] + "')\"><span>修改|</span></a><a href='javascript:void(0);' onclick=\"remove('" + Source[i]['PromoCode'] + "','" + Source[i]['Code'] + "')\"><span>删除</span></a></td></tr>";
                    }
                }
                $('#tbd').append(tableHtml);
            } else {
                $("#tbd").empty();
                tableHtml += "<tr><td colspan=\"11\" style=\"text-align:center;\">暂无内容</td></tr>";
                $('#tbd').append(tableHtml);
            }
        }
    });
}

//关键词搜索
function selectLoad() {
    load(1);
}

//首页
function firstPage(m) {
    if (m == "0") {
        load(1);
    } else {
        getGZ(1);
    }
}

//末页
function lastPage(m) {
    if (m == "0") {
        var count = document.getElementById('pageCount_list').innerHTML;
        var pgCount = parseInt(count);
        load(pgCount);
    } else {
        var count = document.getElementById('B4').innerHTML;
        var pgCount = parseInt(count);
        getGZ(pgCount);
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
        load(pgNum);
    } else {
        var index = document.getElementById('B3').innerHTML;
        var count = document.getElementById('B4').innerHTML;
        var pgNum = parseInt(index);
        var pgCount = parseInt(count);
        pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
        getGZ(pgNum);
    }
}

//上一页
function prePage(m) {
    if (m == "0") {
        var index = document.getElementById('pageIndex_list').innerHTML;
        var pgNum = parseInt(index);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        load(pgNum);
    } else {
        var index = document.getElementById('B3').innerHTML;
        var pgNum = parseInt(index);
        pgNum = pgNum == 1 ? 1 : pgNum - 1;
        getGZ(pgNum);
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
    } else {
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

//时间日期插件
$(function () {
    laydate.render({
        elem: '#startDate'
    });
    laydate.render({
        elem: '#endDate'
    });
    laydate.render({
        elem: '#startTime'
      , type: 'time'
    });
    laydate.render({
        elem: '#endTime'
      , type: 'time'
    });
});

//删除促销生效时间规则
function remove(ProCode, code) {
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "PromoteTime/PromoteTimeDelete",
        data: { PromoCode: ProCode, Code: code },
        dataType: 'json',
        success: function (data) {
            if (data.Code == "0") {
                alert("操作成功！");
            } else {
                alert("操作失败！");
            }
            var index = document.getElementById("pageIndex_list").innerHTML;
            load(index);
        }
    });
}

//UpdateDialog（弹出更新框并加载数据）
function UpdateDialog(code, timeName, isforever, startrq, endrq, startsj, endsj, proCode) {
    document.getElementById('updatePage').style.display = 'block';
    document.getElementById('code').value = code;
    document.getElementById('timeName').value = timeName;
    if (isforever == "true") {
        $('#byj').prop("checked", false);
        $('#yj').prop("checked", true);
    } else {
        $('#yj').prop("checked", false);
        $('#byj').prop("checked", true);
    }
    document.getElementById("startDate").value = startrq;
    document.getElementById("endDate").value = endrq;
    document.getElementById("startTime").value = startsj;
    document.getElementById("endTime").value = endsj;
    document.getElementById('gzSelect').value = proCode;

}

//更新弹框中确定按钮事件
function update1() {
    var code = document.getElementById('code').value;
    var proCode = document.getElementById('gzSelect').value;
    var startrq = document.getElementById("startDate").value;
    var endrq = document.getElementById("endDate").value;
    var startsj = document.getElementById("startTime").value;
    var endsj = document.getElementById("endTime").value;
    var isForever = $('input[name="sjsz"]:checked').val();
    var timeName = document.getElementById('timeName').value;
    var index = document.getElementById("pageIndex_list").innerHTML;
    var isUse = "false";
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "PromoteTime/PromoteTimeUpdate",
        data: { Code: code, TimeName: timeName, IsUse: isUse, IsForever: isForever, EffectiveStartDate: startrq, EffectiveEndDate: endrq, EffectiveStartTime: startsj, EffectiveEndTime: endsj, PromoCode: proCode },
        dataTye: 'json',
        success: function (data) {
            if (data.Code == "0") {
                alert("操作成功！");
                closeDialog1();
                load(index);
            } else {
                alert("操作失败！");
            }
        }
    });
}

//修改发布状态
function change(code, isUse) {
    if (isUse == "false") {
        isUse = "true";
    } else {
        isUse = "false";
    }
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + "PromoteTime/PromotieTimeEffective",
        data: { Code: code, isUse: isUse },
        dataType: 'json',
        success: function (data) {
            if (data.Code == "0") {
                alert("操作成功！");
                var index = document.getElementById("pageIndex_list").innerHTML;
                load(index);
            } else {
                alert("操作失败！");
            }
        }
    });
}

//搜索
function gz_search() {
    getGZ(1);
}

//关闭UpdateDialog
function closeDialog1() {
    document.getElementById('updatePage').style.display = 'none';
    var index = document.getElementById("pageIndex_list").innerHTML;
    load(index);
}

//打开更新选项中规则选择弹框
function XZGZ(n) {
    document.getElementById('gzxz').style.display = n ? 'block' : 'none'; 
    document.getElementById('keyword').value = "";
    getGZ(1); 
}

//选择规则
function getGZ(pageIndex) {
    //var iU = $("input[name='cxgz']:checked").val();
    var kd = $('#keyword').val();
    var iU = "all";
    var size = 8;
    var index = pageIndex;
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + "Promotion/PromotionList?keyword=" + kd + "&isUse=" + iU + "&pageSize=" + size + "&pageIndex=" + index,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var index1 = data.PageIndex;
            var page = data.PageSize;
            var total = data.TotalCount;
            var Source = data.Source;
            setRecordCount("1", page, total, index1);
            $("#tbd2").empty();
            var tableHtml = "";
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    tableHtml += "<tr style='text-align:center'>"
                    + "<td><input type=\"checkbox\" value='" + Source[i]["PromoCode"] + "'/></td>"
                    + "<td>" + Source[i]['PromoCode'] + "</td>"
                    + "<td>" + Source[i]['PromName'] + "</td>"
                    + "<td>" + Source[i]['MeetCount'] + "</td>"
                    + "<td>" + Source[i]['discount'] + "</td>";
                    if (Source[i]['PromType'] == "SingleGoods") {
                        tableHtml += "<td>" +"单品" + "</td>";
                    }
                    if (Source[i]['PromType'] == "Brand") {
                        tableHtml += "<td>" + "品牌" + "</td>";
                    }
                    if (Source[i]['PromType'] == "CustomsGroup") {
                        tableHtml += "<td>" + "客户分组" + "</td>";
                    }
                    if (Source[i]['IsUse']) {
                        tableHtml += "<td>" + "是" + "</td>";
                    } else {
                        tableHtml += "<td>" + "否" + "</td>";
                    }
                    if (Source[i]['ContentType'] == "SumMoney") {
                        tableHtml += "<td>" + "满足指定金额" + "</td>";
                    }
                    if (Source[i]['ContentType'] == "SumQuality") {
                        tableHtml += "<td>" + "满足指定件数" + "</td>";
                    }
                    if (Source[i]['ContentType'] == "ArriveNum") {
                        tableHtml += "<td>" + "累计满足指定件数" + "</td>";
                    }
                    tableHtml += "<td>" + Source[i]['ManuFacturer'] + "</td>";
                    if (Source[i]['ExcludeGoods'] == null) {
                        tableHtml += "<td>" + "-" + "</td>";
                    } else {
                        tableHtml += "<td>" + Source[i]['ExcludeGoods'] + "</td>";
                    }
                    if (Source[i]['PromRule'] == "Discount") {
                        tableHtml += "<td>" + "折扣" + "</td>";
                    }
                    if (Source[i]['PromRule'] == "FJ") {
                        tableHtml += "<td>" + "满减" + "</td>";
                    } if (Source[i]['PromRule'] == "FZ") {
                        tableHtml += "<td>" + "满赠" + "</td>";
                    }
                }
                $('#tbd2').append(tableHtml);
            }
            else {
                $("#tbd2").empty();
                tableHtml += "<tr><td colspan='11' style='text-align: center;'>暂无内容</td></tr>";
                $('#tbd2').append(tableHtml);
            }
        }
    });
}

//关闭更新选项中规则选择弹框
function closeDialog() {
    document.getElementById('gzxz').style.display = 'none';
}

//更新选项中规则选择弹框确定按钮事件
function confirm() {
    var text = $("input[type='checkbox']:checked").val();
    document.getElementById('gzSelect').value = text;
    closeDialog();
}