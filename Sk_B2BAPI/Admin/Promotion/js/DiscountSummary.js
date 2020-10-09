$(function () {
    SummaryKoul();
})

function SummaryKoul() {

    var ksrq = $("#startDate").val();
    var jsrq = $("#endDate").val();
    var spmch = $("#spmch").val();
    var PageSize = $("#PageSize").html();
    var PageIndex = $("#PageIndex").html();
    var sqltype = "ZKHZ";
    var Procedure = "Promotion_GK";
    var Type = "HZ";
    var paramcont = "&PageSize=" + PageSize + "&PageIndex=" + PageIndex + "&sqltype=" + sqltype + "&Procedure=" + Procedure + "&ksrq=" + ksrq + "&spmch=" + spmch+"&jsrq="+jsrq;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/DiscountSummary.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBShow').empty();
                var tb = "";
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' align='center' id='select" + i + "'  >"
                                + "<td>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                                + "<td id='xspbh" + i + "'     align='center'>" + JSON["data"][i]["goods_no"] + "</td>"
                                + "<td id='xspmch" + i + "'     align='center'>" + JSON["data"][i]["sub_title"] + "</td>"
                                + "<td id='xshpgg" + i + "'     align='center'>" + JSON["data"][i]["drug_spec"] + "</td>"
                                + "<td id='xdw" + i + "'        align='center'>" + JSON["data"][i]["package_unit"] + "</td>"
                                + "<td id='xdw" + i + "'        align='center'>" + JSON["data"][i]["real_price"] + "</td>"
                                + "<td id='xdw" + i + "'        align='center'>" + JSON["data"][i]["hshj"] + "</td>"
                                + "<td id='xdw" + i + "'        align='center'>" + JSON["data"][i]["ZPrice"] + "</td>"
                                + "<td id='xdw" + i + "'        align='center'>" + JSON["data"][i]["YHJE"] + "</td>"
                             
                                + "<td id='xshpgg" + i + "'     align='center'>" + JSON["data"][i]["factories_choosing"] + "</td></tr>"

                }
                //tb = tb + "<tr><td colspan=\"5\"></td><td align='center'>总销售：" + JSON["data"][0]["ZPrice"] + "</td><td align='center'>实际售额：" + JSON["data"][0]["ZJ"] + "</td><td align='center'>折扣总额：</td><td colspan=\"2\"></td></tr>";
                $("#TBShow").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("RecordCount").innerHTML = recordCount;
                document.getElementById("PageCount").innerHTML = pageCount;
                closeLoading();
            }
            if (TYPE == '1') {
                closeLoading()
            }
            if (TYPE == '2') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })


}