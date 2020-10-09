function luru() {
    var nameParatement = RegExp("^(?!_)(?!.*?_$)[a-zA-Z0-9_\u4e00-\u9fa5]+$");
    var priceParatement = RegExp("^[0-9]{1,11}$|^[0-9]{0,11}\.[0-9]{1,3}$");
    var integralParatement = RegExp("^([1-9][0-9]*)$");
    var goodsName = $('#goodsName').val();//名称
    var goodsSpecifications = $('#goodsSpecifications').val();//规格
    var unit = $('#unit').val();//单位
    var classification = $('#classification').val();//分类
    var floor = $('#floor').val();//楼层
    var hiddenUrl = $('#hiddenUrl').val();//图片路径
    var vendel = $('#vendel').val();//厂家
    var price = $('#price').val();//价格
    var integral = $('#integral').val();//积分
    var Quantity = $('#Quantity').val();
    var type = "Integral_Goods_Insert";
    var Procedure = "Proc_Integral";
    if (goodsName == "") {
        alert("名称不能为空！");
    } else if (!nameParatement.test(goodsName)) {
        alert("商品名称不符合规格！");
    } else if (goodsSpecifications == "") {
        alert("规格不能为空！");
    } else if (unit == "") {
        alert("单位不能为空！");
    } else if (classification == "") {
        alert("分类不能为空！");
    } else if (floor == "") {
        alert("楼层不能为空！");
    } else if (hiddenUrl == "" && classification !='赠品') {
        alert("图片路径不能为空！");
    } else if (vendel == "") {
        alert("厂家不能为空！");
    } else if (price == "") {
        alert("价格不能为空！");
    } else if (integral == "") {
        alert("积分不能为空！");
    } else if (!integralParatement.test(integral)) {
        alert("积分必须为整数！");
    } else if (!priceParatement.test(price)) {
        alert("价格不符合规则！");
    } else {
        var Parameter = "GoodsName=" + encodeURI(goodsName) + "&GoodsSpecifications=" + encodeURI(goodsSpecifications) + "&Unit=" + encodeURI(unit) + "&Classification=" + encodeURI(classification)
            + "&Floor=" + encodeURI(floor) + "&HiddenUrl=" + encodeURI(hiddenUrl) + "&Vendel=" + encodeURI(vendel) + "&Price=" + encodeURI(price) + "&Integral=" + encodeURI(integral)
            + "&type=" + encodeURI(type) + "&Procedure=" + encodeURI(Procedure) + "&Quantity=" + Quantity;
        $.ajax({
            type: 'Post',
            cache: false,
            url: 'ashx/IntegralGoodsOperation.ashx?' + Parameter,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.return_code == "0") {
                    alertFun(data.data[0]["message"], function () { window.location.reload(); }, 's');
                } else if (data.return_code == "1") {
                    alertFun(data.data[0]["message"], function () { }, 'f');
                }
            }
        });
    }
}