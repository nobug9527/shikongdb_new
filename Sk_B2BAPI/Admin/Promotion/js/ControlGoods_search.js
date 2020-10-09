$(function () {
    control_goods();
})

function control_goods() {
    var keywords = $("#keywords").val();
    var BZ = $("#slPaiChu").val();    //排除类型
    var PageSize = $("#PageSize").html();
    var PageIndex = $("#PageIndex").html();
    var sqltype = "Search_ControlGoods";
    var Procedure = "Promotion_GK";
    var Type = "Search";
    var paramcont = "&PageSize=" + PageSize + "&PageIndex=" + PageIndex + "&sqltype=" + sqltype + "&Procedure=" + Procedure + "&keywords=" + keywords + "&BZ=" + BZ;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ControlGoods_search.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBShow').empty();
                var tb = "";
                if (BZ == "goods") {
                    var tb = "<tr><th>行号</th><th>商品编号</th><th>商品名称</th> <th>商品规格</th><th>中包装</th><th>单  位</th><th>生产厂家</th><th>商品库存</th><th>操作</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr class='chcol' align='center' id='select" + i + "'  >"
                            + "<td>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                            + "<td id='xspbh" + i + "'     align='center'>" + JSON["data"][i]["goods_no"] + "</td>"
                            + "<td id='xspmch" + i + "'     align='center'>" + JSON["data"][i]["sub_title"] + "</td>"
                            + "<td id='xshpgg" + i + "'     align='center'>" + JSON["data"][i]["drug_spec"] + "</td>"
                            + "<td id='xzbz" + i + "'       align='center'>" + JSON["data"][i]["min_package"] + "</td>"
                            + "<td id='xdw" + i + "'        align='center'>" + JSON["data"][i]["package_unit"] + "</td>"
                            + "<td id='xshpgg" + i + "'     align='center'>" + JSON["data"][i]["factories_choosing"] + "</td>"
                            + "<td id='xquantity" + i + "'  align='center'>" + JSON["data"][i]["stock_quantity"] + "</td>"
                            + "<td align='center'><a onclick='deleteControl(\"" + i + "\")'>删除</a></td>"
                            + "<td id='xarticleid" + i + "' align='center' style=\"display:none\">" + JSON["data"][i]["article_id"] + "</td></tr>";

                    }
                }
                else {
                    var tb = "<tr><th>序号</th><th>单位编号</th><th>单位名称</th><th>操作</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr class='chcol' align='center' id='select" + i + "'  >"
                            + "<td>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                            + "<td id='xdwbh" + i + "'     align='center'>" + JSON["data"][i]["danwbh"] + "</td>"
                            + "<td id='xdwmch" + i + "'     align='center'>" + JSON["data"][i]["nick_name"] + "</td>"
                            + "<td align='center'><a onclick='deleteControl(\"" + i + "\")'>删除</a></td>"
                            + "<td id='xid" + i + "' align='center' style=\"display:none\">" + JSON["data"][i]["id"] + "</td></tr>";

                    }
                }
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

//删除管控的商品
function deleteControl(i) {

    var msg="";
    msg="确定删除！";
    if (confirm(msg)) {
        var BZ = $("#slPaiChu").val();    //排除类型
        if (BZ == "goods") {
            var id = $("#xarticleid" + i + "").html();
        }
        else {
            var id = $("#xid" + i + "").html();
        }
        var Type = "deletecontrol";
        var sqltype = "Delete_goods";
        var Procedure = "Promotion_GK";
        $.ajax({
            type: "Post",
            cache: false,
            url: "ashx/ControlGoods_search.ashx?Type=" + Type + "&sqltype=" + sqltype + "&Procedure=" + Procedure + "&id=" + encodeURI(id) + "&BZ=" + BZ,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var JSON = data;
                var TYPE = JSON["return_code"];
                var count = JSON["count"];
                if (TYPE == '0') {
                    alertFun(JSON["data"][0]["message"], function () { closeLoading() });
                    control_goods();
                }
                else {
                    alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                closeLoading()
            }
        })
    }
}