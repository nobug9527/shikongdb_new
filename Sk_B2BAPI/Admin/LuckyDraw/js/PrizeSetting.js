$(function () {
    getGZ(1);
});

function coupon_search() {
    getGZ(1);
}
function getGZ(pageIndex) {
    var kd = $('#keyword').val();
    var size = 15;
    var index = pageIndex;
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + "Prize/GetPrizeList?keyword=" + kd + "&pageSize=" + size + "&pageIndex=" + index,
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
                        + "<td>" + source[i]['BH'] + "</td>"
                        + "<td><img src='" + source[i]['ImgUrl'] + "' /></td>"
                        + "<td>" + source[i]['PrizeName'] + "</td>"
                        + "<td>" + source[i]['PrizeType'] + "</td>"
                            + "<td>" + source[i]['PrizeCount'] + "</td>"
                            + "<td>" + source[i]['LastModifyTime'] + "</td>";
                    tableHtml += "<td><a href=\"javascript:void(0);\" onclick=\"PrizeUpdate('" + source[i]['BH'] + "','" + source[i]['ImgUrl'] + "','" + source[i]['PrizeName'] + "','" + source[i]['PrizeType'] + "','" + source[i]['PrizeCount'] + "')\"><span>修改</span></a><a href=\"javascript:void(0);\" onclick=\"PrizeDelete('" + source[i]['BH'] + "')\"><span>删除</span></a></td></tr>";


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

function PrizeUpdate(bh, imgUrl, prizeName, prizeType, prizeCount) {
    var url = "PrizeUpdate.aspx?bh=" + encodeURI(bh) + "&pictureText=" + encodeURI(imgUrl) + "&PrizeName=" + encodeURI(prizeName) + "&classification=" + encodeURI(prizeType) + "&PrizeValue=" + encodeURI(prizeCount);
    location.href = encodeURI(url);
}

function PrizeDelete(bh) {
    var count = document.getElementById('pageCount_list').innerHTML;
    var pgCount = parseInt(count);
    $.ajax({
        type: 'POST',
        cache: false,
        url: P_Json.Ajax_Url + "Prize/DeletePrize?code=" + bh,
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.Code == 0) {
                alertFun("操作成功！", function () { getGZ(pgCount) }, 's');
            }
        }
    });
}