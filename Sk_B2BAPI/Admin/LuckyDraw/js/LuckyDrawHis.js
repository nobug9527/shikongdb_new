$(function () {
    var myDate = new Date();
    $("#startDate").val(myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate());
    $("#endDate").val(myDate.getFullYear() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getDate());
    laydate.render({
        elem: '#startDate'
    });
    laydate.render({
        elem: '#endDate'
    });
    getGZ(1);
});

function coupon_search() {
    getGZ(1);
}
function getGZ(pageIndex) {
    var kd = $('#keyword').val();
    var selectType = $("input[name='cxgz']:checked").val();
    var size = 15;
    var index = pageIndex;
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + "Lottery/LotteryHis?keyword=" + kd + "&selectType=" + selectType + "&pageSize=" + size + "&pageIndex=" + index + "&start=" + $("#startDate").val() + "&end=" + $("#endDate").val(),
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var code = data.Code;
            var index1 = data.PageIndex;
            var page = data.PageSize;
            var total = data.TotalCount;
            var source = data.Source;
            setRecordCount("0", page, total, index1);
            $("#tbd").empty();
            var tableHtml = "";
            if (code !== 0) {
                alert(code + "    " + data.Message);
                return;
            }
            if (source.length > 0) {
                for (var i = 0; i < source.length; i++) {
                    tableHtml += "<tr style='text-align:center'>"
                        + "<td>" + source[i]['ID'] + "</td>"
                        + "<td>" + source[i]['UserID'] + "</td>"
                        + "<td>" + source[i]['UserName'] + "</td>"
                        + "<td><img src='" + source[i]['ImgUrl'] + "' /></td>"
                        + "<td>" + source[i]['PrizeBH'] + "</td>"
                        + "<td>" + source[i]['PrizeName'] + "</td>"
                        + "<td>" + source[i]['PrizeType'] + "</td>"
                            + "<td>" + source[i]['PrizeCount'] + "</td>"
                            + "<td>" + source[i]['LuckyTime'] + "</td>";
                    //tableHtml += "<td><a href=\"javascript:void(0);\" onclick=\"PrizeUpdate('" + source[i]['BH'] + "','" + source[i]['ImgUrl'] + "','" + source[i]['PrizeName'] + "','" + source[i]['PrizeType'] + "','" + source[i]['PrizeCount'] + "')\"><span>修改</span></a><a href=\"javascript:void(0);\" onclick=\"PrizeDelete('" + source[i]['BH'] + "')\"><span>删除</span></a></td></tr>";


                }
                $('#tbd').append(tableHtml);
                return;
            }
            $("#tbd").empty();
            tableHtml += "<tr><td colspan='11' style='text-align: center;'>暂无内容</td></tr>";
            $('#tbd').append(tableHtml);

        }
    });
}