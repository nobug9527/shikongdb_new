//商品查询
function QueryPcsp(index)
{
    var SqlType = "Getpcsp";
    var SqlProcedure = "Proc_cxpcsp";
    var gdCode = $('#Text2').val();
    var kyWord = $('#Text1').val();
    var pageSize = "20";
    var pageIndex = index.toString();
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        spmch: kyWord,
        shengccj: gdCode,
        pgSize: pageSize,
        pgIndex: pageIndex
    };
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnDataTable.ashx?Type=" + Type + "&Json=" + encodeURI(JSON.stringify(Json)),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            $('#tbodyContent').empty();
            var tableHtml = "";
            if (TYPE == '0') {
                for (var i = 0; i < JSON["data"].length; i++) {
                    tableHtml = tableHtml + "<tr style='text-align:center'>"
                    + "<td><input type=\"checkbox\" name=\"checkGoods\" value='" + JSON["data"][i]["article_id"] + "'/></td>"
                    + "<td>" + JSON["data"][i]["call_index"] + "</td>"
                    + "<td>" + JSON["data"][i]["sub_title"] + "</td>"
                    + "<td>" + JSON["data"][i]["drug_spec"] + "</td>"
                    + "<td>" + JSON["data"][i]["package_unit"] + "</td>"
                    + "<td>" + JSON["data"][i]["factories_choosing"] + "</td>"
                    + "</tr>"
                }
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                setRecordCount(9,pageCount, recordCount,index);
                $('#tbodyContent').append(tableHtml);
            }
            else{
                $('#tbodyContent').empty();
                tableHtml = tableHtml + "<tr><td colspan='6' style='text-align: center;'>暂无内容</td></tr>"
                $('#tbodyContent').append(tableHtml);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
        }
    })
}


//排除商品查询
function QueryYPcsp(index) {
    var SqlType = "GetYpcsp";
    var SqlProcedure = "Proc_cxpcsp";
    var gdCode = $('#ShengccjId').val();
    var kyWord = $('#SpmchId').val();
    var pageSize = "20";
    var pageIndex = index.toString();
    var PromCode = $("#hiddenCode").val();
    var Type = "DataTable";
    var Json = {
        Type: SqlType,
        Procedure: SqlProcedure,
        spmch: kyWord,
        shengccj: gdCode,
        PromCode:PromCode,
        pgSize: pageSize,
        pgIndex: pageIndex
    };

    $.ajax({
        type: 'Post',
        cache: false,
        url: "ashx/ReturnDataTable.ashx?Type=" + Type + "&Json=" + encodeURI(JSON.stringify(Json)),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            $("#tbody").empty();
            var tableHtml = "";

            if (TYPE == '0') {
                for (var i = 0; i < JSON["data"].length; i++) {
                    tableHtml = tableHtml + "<tr style='text-align:center'>"
                    + "<td><input type=\"checkbox\" name=\"check\" value='" + JSON["data"][i]["article_id"] + "'/></td>"
                    + "<td>" + JSON["data"][i]["call_index"] + "</td>"
                    + "<td>" + JSON["data"][i]["sub_title"] + "</td>"
                    + "<td>" + JSON["data"][i]["drug_spec"] + "</td>"
                    + "<td>" + JSON["data"][i]["package_unit"] + "</td>"
                    + "<td>" + JSON["data"][i]["factories_choosing"] + "</td>"
                    + "</tr>"
                }
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                setRecordCount(10, pageCount, recordCount, index);
                $('#tbody').append(tableHtml);
            }
            else {
                $('#tbody').empty();
                tableHtml = tableHtml + "<tr><td colspan='6' style='text-align: center;'>暂无内容</td></tr>"
                $('#tbody').append(tableHtml);
            }
        }
    });
}