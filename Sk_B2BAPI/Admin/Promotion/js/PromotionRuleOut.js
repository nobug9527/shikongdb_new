//促销方案查询
function GetCoupon() {
    openLoading();

    var faname = $('#faname').val();
    var sqlvalue = $('#keywords').val();
    var PageSize = 15;
    var PageIndex = $("#PageIndex").html();
    var data = {
        type: "GetCxfa",
        faname: faname,
        fabh:sqlvalue,
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
                var tableHtml = "<tr><th>编号</th><th>方案编号</th><th>方案类型</th><th>方案描述</th><th>用户分类</th><th>操作</th><th>操作</th></tr>";
                for (var i = 0; i < JSON["data"].length; i++) {
                    tableHtml += "<tr style='text-align:center'>"
                    tableHtml += "<td align='center'>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>";
                    tableHtml += "<td id='xfabh"+i+"'>" + JSON["data"][i]['fabh'] + "</td>"
                    tableHtml += "<td>" + JSON["data"][i]['faname'] + "</td>"
                    tableHtml += "<td>" + JSON["data"][i]['describe'] + "</td>"
                    var kelx = JSON["data"][i]['khfl']
                    if (kelx == "ALL"){tableHtml += "<td>全部</td>"}
                    else if (kelx == "F001") { tableHtml += "<td>终端</td>" }
                    else if (kelx == "F002") { tableHtml += "<td>连锁</td>" }
                    else if (kelx == "F003") { tableHtml += "<td>批发</td>" }
                    tableHtml += "<td><a class='cx btn_b' onclick=\"BdgxCX(" + i + ")\">选择</a></td>";
                    tableHtml += "<td><a class='cx btn_c' onclick=\"CkYyFa(" + i + ")\">查看已有方案</a></td>";
                    tableHtml += "</tr>"
                }
                $('#TBShow').append(tableHtml);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("RecordCount").innerHTML = recordCount;
                document.getElementById("PageCount").innerHTML = pageCount;
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading()
                $("#TBShow").empty();
                var tableHtml = " <thead><tr><th>编号</th><th>方案编号</th><th>方案类型</th><th>方案描述</th><th>用户分类</th><th>操作</th></tr></thead><tbody>";
                tableHtml += "<tr><td colspan='11' style='text-align: center;'>暂无内容</td></tr>";
                $('#TBShow').append(tableHtml);
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
//绑定关系查询
function BdgxCX(i)
{
    var fabh = $("#xfabh" + i + "").html();
    window.location.href = "BindGk.aspx?fabh=" + fabh;
}
//查看已有绑定的方案
function CkYyFa(i) {
    var fabh = $("#xfabh" + i + "").html();
    window.location.href = "YyFa.aspx?fabh=" + fabh;
}