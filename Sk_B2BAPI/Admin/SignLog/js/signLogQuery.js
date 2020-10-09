function GetSignLog() {
    openLoading();
   
    var fbStatus = $("#slStatus").val();    //获取发布状态
    var SignModel = $("#slType").val();      //获取签到模式
    var keyWord = $("#txtKeyWord").val();     //获取关键字
    var PageSize = $("#PageSize").html();
    var PageIndex = $("#PageIndex").html();

    var sqltype = "GetSign";
    var Procedure = "Proc_SignLog";
    var Type = "GetSign";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&fbStatus=" + encodeURI(fbStatus) + "&SignModel=" + encodeURI(SignModel) + "&keyWord=" + encodeURI(keyWord) + "&PageSize=" + PageSize + "&PageIndex=" + PageIndex;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnData.ashx?Type=" + Type + paramcont,
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
                    tb += "<tr class='chcol' align='center' id='select" + i + "'>"
                    tb += "<td>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                    tb += "<td id='xRewardFa"+i+"' align='center'>" + JSON["data"][i]["RewardFa"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["type"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["KuHu"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["RewardForm"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["SignRule"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["SignReward"] + "</td>"
                    tb += "<td  align='center'>" + JSON["data"][i]["addtime"] + "</td>"
                    if (JSON["data"][i]["Content"].length >= 13) {
                        var RuDa = JSON["data"][i]["Content"].substring(0, 13);
                        tb += "<td  align='center' title='" + JSON["data"][i]["Content"]+"'>" + RuDa + "...</td>"
                    }
                    else {
                        tb += "<td  align='center'>" + JSON["data"][i]["Content"] + "</td>"
                    }
                    if (JSON["data"][i]["status"] == "Y") {
                        tb += "<td  align='center'>已发布</td>"
                        tb += "<td>"
                        tb += "<span onclick=\"UpdateData(" + i + ",'Cancle')\"class='bz cr'>取消</span><span onclick=\"UpdateData(" + i + ",'Delete')\" class='bz cl'>删除</span>"
                        tb += "</td>"
                    }
                    else {
                        tb += "<td  align='center'>未发布</td>"
                        tb += "<td>"
                        tb += "<a><span onclick=\"UpdateData(" + i + ",'Update')\" class='bz cl'>发布</span></a>"
                        tb += "</td>"
                    }
                    tb += "</tr>";
                }

                $("#TBShow").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("RecordCount").innerHTML = recordCount;
                document.getElementById("PageCount").innerHTML = pageCount;
                closeLoading();
            }
            if (TYPE == '1') {
                $('#TBShow').empty();
                var tb = "";
                tb = "<tr><td colspan='11' align='center'>无数据</td></tr>"
                $('#TBShow').append(tb);
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
//更改数据状态
function UpdateData(i,t) {
    var FaBh = $("#xRewardFa" + i + "").html();
    var sqltype = "UpdateSign";
    var Procedure = "Proc_SignLog";
    var Type = "UpdateData";
    var paramcont = "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&FaBh=" + FaBh+"&T="+t;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnData.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                alertFun(JSON["data"][0]["message"], function () { GetSignLog(); closeLoading() });
                closeLoading();
            }
            if (TYPE == '1') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
            if (TYPE=='2') {
                alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}