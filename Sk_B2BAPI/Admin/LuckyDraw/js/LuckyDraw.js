$(function () {
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + "LuckyDraw/GetLuckyDraw",
        dataType: "json",
        success: function (data) {
            if (data.Code === 0) {
                $("#bhPrize1").val(data.Source.Prize1);
                $("#countPrize1").val(data.Source.PrizeCount1);
                $("#bhPrize2").val(data.Source.Prize2);
                $("#countPrize2").val(data.Source.PrizeCount2);
                $("#bhPrize3").val(data.Source.Prize3);
                $("#countPrize3").val(data.Source.PrizeCount3);
                $("#bhPrize4").val(data.Source.Prize4);
                $("#countPrize4").val(data.Source.PrizeCount4);
                $("#bhPrize5").val(data.Source.Prize5);
                $("#countPrize5").val(data.Source.PrizeCount5);
                $("#bhPrize6").val(data.Source.Prize6);
                $("#countPrize6").val(data.Source.PrizeCount6);
                $("#bhPrize7").val(data.Source.Prize7);
                $("#countPrize7").val(data.Source.PrizeCount7);
                $("#bhPrize8").val(data.Source.Prize8);
                $("#countPrize8").val(data.Source.PrizeCount8);
                $("#luckyDrawId").val(data.Source.ID);
                $("#ExpirationTime").val(data.Source.ExpirationTime);
                var str = data.Source.Information;
                var len = str.split('&&');
                for (var j = 0; j < len.length; j++) {
                    $("#infoDl").append("<dd><input type=\"text\" class=\"input normal\" value=\"" + len[j] + "\" /></dd>");
                }
                for (var i = 1; i <= 8; i++) {
                    GetPrize(i);
                }
            } else {
                alertFun(data.Message);
            }
        }
    });
    SetCreate(false);
    laydate.render({
        elem: '#ExpirationTime'
    });
});

function SetCreate(flag) {
    if (flag) {
        $("#btnCreate").css("background-color","green");
        $("#btnCreate").attr("disabled", false);
    } else {
        $("#btnCreate").css("background-color", "gray");
        $("#btnCreate").attr("disabled", true);
    }
}


function GetPrize(i) {
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + "Prize/PrizeOnly?code=" + $("#bhPrize" + i).val(),
        dataType: "json",
        success: function (da) {
            if (da.Code == 0) {
                $("#namePrize" + i).text(da.Source.PrizeName);
                $("#imgPrize" + i).attr("src", da.Source.ImgUrl);
            } else {
                alertFun(da.Message);
            }
        }
    });
}
function getGZ(pageIndex) {
    var kd = $('#gsWord').val();
    var size = 15;
    var index = pageIndex;
    $.ajax({
        type: 'Get',
        cache: false,
        url: P_Json.Ajax_Url + "Prize/GetPrizeList?keyword=" + kd + "&pageSize=" + size + "&pageIndex=" + index,
        dataType: "json",
        success: function (data) {
            var code = data.Code;
            var index1 = data.PageIndex;
            var page = data.PageSize;
            var total = data.TotalCount;
            var source = data.Source;
            setRecordCount("0", page, total, index1);
            $("#content-table").empty();
            var tableHtml = "";
            if (code !== 0) {
                alert(code + "    " + data.Message);
                return;
            }
            if (source.length > 0) {
                for (var i = 0; i < source.length; i++) {
                    tableHtml += "<tr style='text-align:center'>"
                        + "<td><input type=\"radio\" name=\"pinpai\" value='" + source[i]["BH"] + "'/></td>"
                        + "<td>" + source[i]['BH'] + "</td>"
                        + "<td><img src='" + source[i]['ImgUrl'] + "' /></td>"
                        + "<td>" + source[i]['PrizeName'] + "</td>"
                        + "<td>" + source[i]['PrizeType'] + "</td>"
                        + "<td>" + source[i]['PrizeCount'] + "</td>"
                        + "<td>" + source[i]['LastModifyTime'] + "</td>";
                }
                $('#content-table').append(tableHtml);
                return;
            }
            $("#content-table").empty();
            tableHtml += "<tr><td colspan='11' style='text-align: center;'>暂无内容</td></tr>";
            $('#content-table').append(tableHtml);
        }
    });
}

function closeDialog() {
    document.getElementById('gzxz').style.display = 'none';
}

function XZGZ(ind) {
    document.getElementById('gzxz').style.display = 'block';
    $("#nowId").val(ind);
    getGZ(1);
}

function confirm() {
    //遍历获取选中行的某些值
    var tbodyObj = document.getElementById('content-table'); //zpSelectTable
    $("input[name='pinpai']").each(function () {
        if ($(this).prop('checked')) {
            var ind = $("#nowId").val();
            var td = $(this).parent().parent().find("td");
            $("#bhPrize" + ind).val(td.eq(1).text());
            $("#namePrize" + ind).text(td.eq(3).text());
            $("#imgPrize" + ind).attr("src", td.eq(2).find("img").attr("src"));
            closeDialog();
            return;
        }
    });
}

function Sub() {
    var rq = $("#ExpirationTime").val();
    if (!rq.match(/^\d{4}-\d{1,2}-\d{1,2}$/)) {
        alertFun("日期不能为空");
        return;
    }
    var postStr = "{ID:'" + $("#luckyDrawId").val() + "',ExpirationTime:'" + rq + "',LotteryName:'华杰抽奖',";
    for (var i = 1; i <= 8; i++) {
        var bh = $("#bhPrize" + i).val();
        var count = $("#countPrize" + i).val();
        if (bh == "") {
            alertFun("奖品不能为空");
            return;
        }
        var reg = /^[0-9]*[1-9][0-9]*$/;
        if (!count.match(reg)) {
            alertFun("必须填写数量");
            return;
        }
        postStr += "Prize" + i + ":'" + bh + "',PrizeCount" + i + ":" + count + ",";
    }
    //postStr = postStr.substr(0, postStr.length - 1);
    var infoDd = $("#infoDl").find("dd").find("input[type='text']");
    var info = "";
    infoDd.each(function () {
        if ($(this).val() !== "")
            info += $(this).val() + "&&";
    });
    postStr += "Information:'" + info + "'";
    postStr += "}";
    $("#btnSubmit").attr("disabled", true);
    $.ajax({
        type: 'POST',
        cache: false,
        contentType: "application/json",
        url: P_Json.Ajax_Url + "LuckyDraw/UpdateLuckDraw",
        dataType: "json",
        data: postStr,
        success: function (data) {
            $("#btnSubmit").attr("disabled", false);
            if (data.Code === 0) {
                alertFun("提交成功", function () { SetCreate(true); });
            } else {
                alertFun(data.Message);
            }
        },
        error: function () {
            $("#btnSubmit").attr("disabled", false);
            alertFun("提交出错，请联系管理员！");
        }
    });
}

function AddDl() {
    $("#infoDl").append("<dd><input type=\"text\" class=\"input normal\"/></dd>");
}

function CreateLottery() {
    confirmFun("确认生成抽奖方案吗？", function () {
        $("#btnCreate").attr("disabled", true);
        $.ajax({
            type: 'POST',
            cache: false,
            contentType: "application/json",
            url: P_Json.Ajax_Url + "LuckyDraw/CreateDraw",
            success: function (data) {
                $("#btnCreate").attr("disabled", false);
                if (data.Code === 0) {
                    alertFun("提交成功", function () { SetCreate(false); });
                } else {
                    alertFun(data.Message);
                }
            },
            error: function () {
                $("#btnCreate").attr("disabled", false);
                alertFun("提交出错，请联系管理员！");
            }
        });
    });
}

function BackOut() {
    confirmFun("确认撤销抽奖方案吗？", function () {
        $("#btnCancel").attr("disabled", true);
        $.ajax({
            type: 'POST',
            cache: false,
            contentType: "application/json",
            url: P_Json.Ajax_Url + "LuckyDraw/BackOut",
            success: function (data) {
                $("#btnCancel").attr("disabled", false);
                if (data.Code === 0) {
                    alertFun("提交成功");
                } else {
                    alertFun(data.Message);
                }
            },
            error: function () {
                $("#btnCancel").attr("disabled", false);
                alertFun("提交出错，请联系管理员！");
            }
        });
    });
}