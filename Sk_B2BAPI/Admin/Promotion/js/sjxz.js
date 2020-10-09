
$(function () {
    //初始化表单验证
    $("#form1").initValidform();
    laydate.render({
        elem: '#startrq'
    });
    laydate.render({
        elem: '#endrq'
    });
    laydate.render({
        elem: '#startsj'
      , type: 'time'
    });
    laydate.render({
        elem: '#endsj'
      , type: 'time'
    });
});

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
            setRecordCount(page, total, index1);
            $("#tbd").empty();
            var tableHtml = "";
            if (Source.length > 0) {
                for (var i = 0; i < Source.length; i++) {
                    tableHtml = tableHtml + "<tr style='text-align:center'>"
                    + "<td><input type=\"checkbox\" value='" + Source[i]["PromoCode"] + "'/></td>"
                    + "<td>" + Source[i]['PromoCode'] + "</td>"
                    + "<td>" + Source[i]['PromName'] + "</td>"
                    + "<td>" + Source[i]['MeetCount'] + "</td>"
                    + "<td>" + Source[i]['discount'] + "</td>";
                    if (Source[i]['PromType'] == "SingleGoods") {
                        tableHtml += "<td>" + "单品" + "</td>";
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
                $('#tbd').append(tableHtml);
            }
            else {
                $("#tbd").empty();
                tableHtml = tableHtml + "<tr><td colspan='11' style='text-align: center;'>暂无内容</td></tr>";
                $('#tbd').append(tableHtml);
            }
        }
    });
}

//关闭Dialog
function closeDialog() {
    document.getElementById('gzxz').style.display = 'none';
}

//搜索按钮
function gz_search() {
    getGZ(1);
}

//首页
function firstPage() {
    getGZ(1);
}

//末页
function lastPage() {
    var count = document.getElementById('pageCount_list').innerHTML;
    var pgCount = parseInt(count);
    //var pgindex = $.trim($("#pageCount_" + typ).text());
    //var pgCount = parseInt(count);
    getGZ(pgCount);
}

//下一页
function nextPage() {
    var index = document.getElementById('pageIndex_list').innerHTML;
    var count = document.getElementById('pageCount_list').innerHTML;
    var pgNum = parseInt(index);
    var pgCount = parseInt(count);
    pgNum = pgNum + 1 > pgCount ? pgCount : pgNum + 1;
    getGZ(pgNum);
}

//上一页
function prePage() {
    var index = document.getElementById('pageIndex_list').innerHTML;
    var pgNum = parseInt(index);
    pgNum = pgNum == 1 ? 1 : pgNum - 1;
    getGZ(pgNum);
}

//显示总页数、当前页、总条数
function setRecordCount(page, total, index1) {
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

//确定按钮事件
function confirm() {
    var text = $("input[type='checkbox']:checked").val();
    document.getElementById('gzSelect').value = text;
    closeDialog();
}

//提交按钮事件
function btnSubmit_Click() {
    var timeName = $('#timeName').val();
    var isForever = $("input[name='sjsz']:checked").val();
    var startrq = $('#startrq').val();
    var endrq = $('#endrq').val();
    var startsj = $('#startsj').val();
    var endsj = $('#endsj').val();
    var proCode = $('#gzSelect').val(); 
    if (timeName=="",startrq == "" || endrq == "" || startsj == "" || endsj == "" || proCode == "") {
        alert("信息不能为空！");
    } else {
    addTimeGZ(proCode,startrq,endrq,startsj,endsj,isForever,timeName);
    }
}

//增加促销生效时间规则
function addTimeGZ(proCode, startrq, endrq, startsj, endsj, isforever, timeName) {
    var code = "0"; //{ Code: code, PromoCode: proCode, IsUse: isUse, EffectiveStartDate: startrq, EffectiveEndDate: endrq, EffectiveStartTime: startsj, EffectiveEndTime: endsj, IsForever: isforever, TimeName: timeName }
    var isUse = "false";
    $.ajax({
        type: 'Post',
        cache: false,
        url: P_Json.Ajax_Url + 'PromoteTime/PromoteTimeUpdate',
        data: { Code: code, PromoCode: proCode, IsUse: isUse, EffectiveStartDate: startrq, EffectiveEndDate: endrq, EffectiveStartTime: startsj, EffectiveEndTime: endsj, IsForever: isforever, TimeName: timeName },
        dataType: 'json',
        success: function (data) {
            if (data.Code=="0") {
                alert("操作成功");
            } else {
                alert("操作失败");
            }
        }
    });
}
