///选择业务员
function OpenYwyInfo() {
    var sqlValue = $("#txtYwyMch").val();
    layer_show("选择业务员", "SearchInfo.html?type=Ywy&proc=proc_Ywt_XsdInfo&sqlType=getYwy&sqlValue=" + encodeURI(sqlValue), 1100, 700);
}
//选择货主
function OpenHzInfo() {
    var sqlValue = $("#txtHzMch").val();
    layer_show("选择货主", "SearchInfo.html?type=Hz&proc=proc_Ywt_XsdInfo&sqlType=GetHz&sqlValue=" + encodeURI(sqlValue), 1100, 700);
}
//选择客户
function OpenKhInfo() {
    var sqlValue = $("#txtdwMch").val();
    layer_show("选择客户", "SearchInfo.html?type=Kh&proc=proc_Ywt_XsdInfo&sqlType=GetKh&sqlValue=" + encodeURI(sqlValue), 1100, 700);
}
//选择商品
function OpenSpInfo() {
    var ywyId = $("#txtYwyId").val();
    var hzId = $("#txtHzId").val();
    var khId = $("#txtDwId").val();
    var sqlValue = $("#txtStrWhere").val();
    if (ywyId == "") {
        layer.alert("请选择业务员", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    if (hzId == "") {
        layer.alert("请选择货主", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    if (khId == "") {
        layer.alert("请选择客户", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    layer_show("选择商品", "SearchInfo.html?type=Sp&proc=proc_Ywt_XsdInfo&sqlType=GetSp&sqlValue=" + encodeURI(sqlValue) + "&ywyId=" + encodeURI(ywyId) + "&hzId=" + encodeURI(hzId) + "&khId=" + encodeURI(khId), 1100, 700);

}