$(function () {
    QueryGk();
});
function QueryGk() {
    openLoading();
    var fabh = GetQueryString("fabh");
    var sqlvalue = $('#keywords').val();
    var PageSize = 15;
    var PageIndex = $("#PageIndex").html();
    var data = {
        type: "QueryFa",
        faname: sqlvalue,
        fabh: fabh,
        PageIndex: PageIndex,
        PageSize: PageSize,
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
                    tb += "<td align='center'>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>";
                    tb += "<td><input type='checkbox' name='XZ' class='XZ' value=\"" + JSON["data"][i]['djbh'] + "\"/></td>";
                    tb += "<td>" + JSON["data"][i]['djbh'] + "</td>"
                    tb += "<td>" + JSON["data"][i]['CodeName'] + "</td>"
                    tb += "<td>" + JSON["data"][i]['add_time'] + "</td>"
                    tb += "</tr>"
                }
                $('#TBShow').append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("RecordCount").innerHTML = recordCount;
                document.getElementById("PageCount").innerHTML = pageCount;
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
//保存绑定的方案
function SubmitGk() {
    openLoading();
    var fabh = GetQueryString("fabh");
    var list = "";
    $.each($('#TBShow input:checked'), function (i,itm) {
        list += itm.value;
       list += ',';
    })
    list = list.substring(0, list.length - 1);
 
    var data = {
        type: "BindFa",
        fabh: fabh,
        List: list
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
                alertFun(JSON["data"][0]["message"], function () { QueryGk();closeLoading() });
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