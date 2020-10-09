$(function () {
    QueryBindFa();
});
function QueryBindFa() {
    openLoading();
    var fabh = GetQueryString("fabh");
    var data = {
        type: "QueryYyFa",
        fabh: fabh
    };
    var proc = "Proc_PromotionRuleOut";//存储过程名
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
                $('#TBShow').empty();
                var tb = "";
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb += "<tr style='text-align:center'>"
                    tb += "<td align='center'> "+parseInt(i+1)+"</td>";
                    tb += "<td id='xdjbh" + i + "'>" + JSON["data"][i]['djbh'] + "</td>";
                    tb += "<td>" + JSON["data"][i]['CodeName'] + "</td>";
                    tb += "<td>" + JSON["data"][i]['add_time'] + "</td>";
                    tb += "<td><a class='cx btn_c' onclick=\"DeleteFa("+i+")\">删除</a></td>";
                    tb += "</tr>";
                }
                $('#TBShow').append(tb);
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading()
                $("#TBShow").empty();
                var tb = " ";
                tb += "<tr><td colspan='11' style='text-align: center;'>暂无内容</td></tr>";
                $('#TBShow').append(tb);
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
//删除方案
function DeleteFa(i) {
    openLoading();
    var fabh = GetQueryString("fabh");
    var tags = $("#xdjbh" + i + "").html();
    var data = {
        type: "DeleteYyFa",
        Tags: tags,
        fabh: fabh
    };
    var proc = "Proc_PromotionRuleOut";//存储过程名
    var type = "ReturnNumber";
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
                alertFun(JSON["data"][0]["message"], function () { QueryBindFa(); closeLoading() });
            }
            if (TYPE == '1') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
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