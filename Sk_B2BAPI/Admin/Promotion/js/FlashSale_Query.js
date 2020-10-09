function GetImgInfo() {
    openLoading();
    var Type = "SearchQG";
    var LongType = $("#FATypeID").val();
    var iz_zx = jQuery("#ZXSelect  option:selected").val(); //是否发布
    var PageSize = $("#PageSize").html();
    var PageIndex = $("#PageIndex").html();
    var ksrq = $("#startDate").val();
    var jsrq = $("#endDate").val();
    var namevalue = $("#gjztxt").val();
    var sqltype = "SearchQG";
    var Procedure = "Proc_ImgAdmin";
    var paramcont = "&PageSize=" + PageSize + "&PageIndex=" + PageIndex + "&ksrq="
        + ksrq + "&jsrq=" + jsrq + "&namevalue=" + encodeURI(namevalue)
        + "&sqltype=" + sqltype + "&Procedure=" + Procedure + "&iz_zx=" + encodeURI(iz_zx) + "&LongType=" + LongType;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/FlashSale_Query.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBShow').empty();
                var tb = "";
                if (LongType == "SYHJ") {
                    tb += "<tr class='odd_bg'><th>序号</th><th>发布编号</th><th>活动类型</th><th>商品名称</th><th>商品规格</th><th>促销库存</th><th>促销价</th><th>优惠券编号</th><th>开始日期</th><th>结束日期</th><th>单次数量</th><th>客户总数</th><th>客户类型</th><th>是否发布</th><th>操作</th></tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr class='chcol' id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td id='fabh" + i + "' align='center'>" + JSON["data"][i]["fabh"] + "</td>";
                        tb = tb + "<td align='center' id='fatype" + i + "'>" + JSON["data"][i]["fatype"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["sub_title"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["drug_spec"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["stock_quantity"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["price"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["goodsname"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["ksrq"] + " " + JSON["data"][i]["kssj"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["jsrq"] + " " + JSON["data"][i]["jssj"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["dcshl"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["khzdshl"] + "</td>";
                        var kelb = JSON["data"][i]["khfl"];
                        if (kelb == "ALL") {
                            tb = tb + "<td align='center'>全部</td>";
                        }
                        else if (kelb == "F001") {
                            tb = tb + "<td align='center'>终端</td>";
                        }
                        else if (kelb == "F002") {
                            tb = tb + "<td align='center'>连锁</td>";
                        }
                        else if (kelb == "F003") {
                            tb = tb + "<td align='center'>批发</td>";
                        }
                        //tb = tb + "<td align='center'><input type=\"text\" style=\"width: 50px;text-align:center;\" id='px" + i + "' onkeypress='getKey("+i+")'  value=\""+JSON["data"][i]["xh"] +"\"/></td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["is_zx"] + "</td>";
                        if (JSON["data"][i]["is_zx"] == "是") {
                            tb = tb + "<td align='center'><a onclick='UpdateFlashSale(\"" + i + "\",\"否\")'>下架</a></td>";
                        }
                        else if (JSON["data"][i]["is_zx"] == "否") {
                            tb = tb + "<td align='center'><a onclick='UpdateFlashSale(\"" + i + "\",\"是\")'>上架</a>|<a onclick='UpdateFlashSale(\"" + i + "\",\"清\")'>删除</a></td>";
                        }
                        tb = tb + "</tr>";
                    }
                }
                else {
                    tb = "<tr class='odd_bg'> <th>行号</th> <th>发布编号</th>   <th>活动类型</th> <th>商品编号</th><th>商品名称</th> <th>商品规格</th> <th>促销库存</th> <th>促销价</th> <th>赠 品</th> <th>赠品数量</th> <th>赠品价格</th> <th>折 扣</th> <th>开始日期</th> <th>结束日期</th>  <th>客户类型</th><th>是否发布</th><th>操作</th>";
                    tb += "</tr>";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        tb = tb + "<tr class='chcol' id='select" + i + "' >"
                        tb = tb + "<td align='center'>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>";
                        tb = tb + "<td id='fabh" + i + "' align='center'>" + JSON["data"][i]["fabh"] + "</td>";
                        tb = tb + "<td align='center' id='fatype" + i + "'>" + JSON["data"][i]["fatype"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["call_index"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["sub_title"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["drug_spec"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["stock_quantity"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["price"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["goodsname"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["zpshl"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["zpprice"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["koul"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["ksrq"] + " " + JSON["data"][i]["kssj"] + "</td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["jsrq"] + " " + JSON["data"][i]["jssj"] + "</td>";
                        var kelb = JSON["data"][i]["khfl"];
                        if (kelb == "ALL") {
                            tb = tb + "<td align='center'>全部</td>";
                        }
                        else if (kelb == "F001") {
                            tb = tb + "<td align='center'>终端</td>";
                        }
                        else if (kelb == "F002") {
                            tb = tb + "<td align='center'>连锁</td>";
                        }
                        else if (kelb == "F003") {
                            tb = tb + "<td align='center'>批发</td>";
                        }
                        //tb = tb + "<td align='center'><input type=\"text\" style=\"width: 50px;text-align:center;\" id='px" + i + "' onkeypress='getKey("+i+")'  value=\""+JSON["data"][i]["xh"] +"\"/></td>";
                        tb = tb + "<td align='center'>" + JSON["data"][i]["is_zx"] + "</td>";
                        if (JSON["data"][i]["is_zx"] == "是") {
                            tb = tb + "<td align='center'><a onclick='UpdateFlashSale(\"" + i + "\",\"否\")'>下架</a></td>";
                        }
                        else if (JSON["data"][i]["is_zx"] == "否") {
                            tb = tb + "<td align='center'><a onclick='UpdateFlashSale(\"" + i + "\",\"是\")'>上架</a>|<a onclick='UpdateFlashSale(\"" + i + "\",\"清\")'>删除</a></td>";
                        }
                        tb = tb + "</tr>";
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
                closeLoading();
                $('#TBShow').empty();
                
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
//修改排序
function getKey(i) {
    if (event.keyCode == 13) {
        var PaiXu = $("#px" + i + "").val();  //排序数字
        var faBh = $("#fabh" + i + "").html();    //发布编号
        var hdType = $("#fatype" + i + "").html();    //活动类型
        if (isNaN(parseInt(PaiXu))) {
            alertFun("输入的数量格式错误！", function () { closeLoading(); }, 'f');
        }
        var Type = "UpdatePaiXu";
        var sqltype = "UpdateXh";
        var Procedure = "Proc_ImgAdmin";
        $.ajax({
            type: "Post",
            cache: false,
            url: "ashx/FlashSale_Query.ashx?Type=" + Type + "&sqltype=" + sqltype + "&Procedure=" + Procedure + "&PaiXu=" + PaiXu + "&faBh=" + faBh + "&hdType=" + hdType,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var JSON = data;
                var TYPE = JSON["return_code"];
                var count = JSON["count"];
                if (TYPE == '0') {
                    alert("操作成功！");
                    GetImgInfo();
                }
                else {
                    alert("操作失败！");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                closeLoading()
            }
        });
    }
}

function UpdatePx(i) {
    if (event.KeyCode ==13) {
        var PaiXu = $("#px" + i + "").val();  //排序数字
        var faBh = $("#fabh" + i + "").html();    //发布编号
        var hdType = $("#fatype" + i + "").html();    //活动类型
        if (isNaN(parseInt(PaiXu))) {
            alertFun("输入的数量格式错误！", function () { closeLoading(); }, 'f');
        }
        var Type = "UpdatePaiXu";
        var sqltype = "UpdateXh";
        var Procedure = "Proc_ImgAdmin";
        $.ajax({
            type: "Post",
            cache: false,
            url: "ashx/FlashSale_Query.ashx?Type=" + Type + "&sqltype=" + sqltype + "&Procedure=" + Procedure + "&PaiXu=" + PaiXu + "&faBh=" + faBh + "&hdType=" + hdType,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var JSON = data;
                var TYPE = JSON["return_code"];
                var count = JSON["count"];
                if (TYPE == '0') {
                    alert("操作成功！");
                    GetImgInfo();
                }
                else {
                    alert("操作失败！");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                closeLoading()
            }
        }); 
     }
}
function UpdateFlashSale(i,is_zx)
{
    var fabh = $("#fabh" + i + "").html();
    var msg="";
    if (is_zx == "是")
    {
        msg = "确定上架！";
    }
    else if (is_zx == "否")
    {
        msg = "确定下架！";
    }
    else if (is_zx == "清")
    {
        msg = "确定删除！";
    }
    
    if (confirm(msg))
    {
        var Type = "UpdateFlashSale";

        var sqltype = "UpdateFlashSale";
        var Procedure = "Proc_ImgAdmin";
        $.ajax({
            type: "Post",
            cache: false,
            url: "ashx/FlashSale_Query.ashx?Type=" + Type + "&sqltype=" + sqltype + "&Procedure=" + Procedure + "&is_zx=" + encodeURI(is_zx) + "&fabh=" + fabh,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var JSON = data;
                var TYPE = JSON["return_code"];
                var count = JSON["count"];
                if (TYPE == '0') {
                    alert("操作成功！");
                    GetImgInfo();
                }
                else {
                    alert("操作失败！");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                closeLoading()
            }
        })
    }
}