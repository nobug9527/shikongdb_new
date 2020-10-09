
//提交修改积分的内容
function SubmitIntegral() {
    openLoading();
    var dwId = GetQueryString("dwId");
    var gdJf = $("#txtGdjf").val();
    var xyJf = $("#txtXyjf").val();   //现有积分
    var GdPoint = "";
    var JF = "";
    var jfCz = $("#Jfcz input[name='ShowID']:checked").val();
    if (isNaN(parseFloat(gdJf))) {
        alertFun("数据格式不正确！", function () { closeLoading(); });
    }
    else {
        if (jfCz == "txtZjjf") {
            JF = parseFloat(xyJf) + parseFloat(gdJf);
            GdPoint = "+" + gdJf;
        }
        else {
            JF = parseFloat(xyJf) - parseFloat(gdJf);
            GdPoint = "-" +gdJf;
        }
        var SqlType = "UpdateIntegral";
        var SqlProcedure = "Proc_HjSalesMan";
        var Type = "ExecuteNonQuery";
        $.ajax({
            type: "Post",
            cache: false,
            url: "ashx/UpdateIntegral.ashx?Type=" + Type + "&SqlType=" + SqlType + "&SqlProcedure=" + SqlProcedure + "&dwId=" + dwId + "&JF=" + JF + "&GdPoint=" + GdPoint + "&xyJf=" + xyJf,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var JSON = data;
                var TYPE = JSON["return_code"];
                var count = JSON["count"];
                if (TYPE == '0') {
                    alertFun(JSON["data"][0]["message"], function () { window.location.href = "MembersQuery.aspx?"; closeLoading() });
                }
                if (TYPE == '1') {
                    alertFun(JSON["data"][0]["message"], function () { closeLoading() }, 'f');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                closeLoading()
            }
        });
    }
}