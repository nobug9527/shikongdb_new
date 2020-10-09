function luru() {
    var nameParatement = RegExp("^(?!_)(?!.*?_$)[a-zA-Z0-9_\u4e00-\u9fa5]+$");
    var goodsName = $('#PrizeName').val();//名称
    var classification = $('#classification').val();//分类
    var bh = $('#hidBH').val();//分类
    var hiddenUrl = $('#hiddenUrl').val();//图片路径
    var vendel = $('#PrizeValue').val();//厂家
    if (goodsName === "") {
        alertFun("名称不能为空！");
    } else if (!nameParatement.test(goodsName)) {
        alertFun("商品名称不符合规则！");
    } else if (classification === "") {
        alertFun("分类不能为空！");
    } else if (hiddenUrl === "") {
        alertFun("图片路径不能为空！");
    } else if (vendel === "" && classification === "会员积分") {
        alertFun("价值不能为空！");
    } else {
        if (classification === "空奖") {
            vendel = "0";
        }
        var json = { BH: bh, ImgUrl: encodeURI(hiddenUrl), PrizeName: goodsName, PrizeType: classification, PrizeCount: vendel };

        $.ajax({
            type: 'POST',
            cache: false,
            url: P_Json.Ajax_Url + 'Prize/UpdatePrize',
            //contentType: "text/plain; charset=utf-8",
            dataType: 'json',
            data: json,
            success: function (data) {
                if (data.Code == "0") {
                    alertFun("操作成功！", function () { location.href = "PrizeSetting.aspx"; }, 's');

                } else {
                    alertFun(data.Message, function () { }, 'f');
                }
            }
        });
    }
}