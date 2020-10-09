///选品牌品信息
function QueryBrand() {
    var strWhere = $("#txtBrandId").val();
    layer_show("选择商品", "SearchInfo.html?type=goodsBrand&proc=Proc_Admin_SearchInfo&sqlType=goodsBrand&strWhere=" + encodeURI(strWhere), 1000, 600);
}
//加载页面商品信息
function LoadGoodsInfo()
{
    var index = layer.load(2);
    var article_id = GetQueryString("article_id");
    var data = {
        type: "GetGoodsDetail",
        strWhere: article_id
    };
    var proc = "Proc_Admin_GoodsList";//存储过程名
    var type = "ReturnDataSet";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                ///加载商品基础信息
                if (obj.table0 != "" && obj.table0.length > 0) {
                    var goodsList = obj.table0[0];
                    $("#txtSortId").val(goodsList.sort_id);//商品排序
                    $("#txtBrandId").val(goodsList.brandId);//品牌Id
                    //$("#sltIsShowId").val(goodsList.status);//商品状态
                    $("#sltCategory").val(goodsList.category_id);//商品分类
                    $("#txtGoodsName").val(goodsList.sub_title);//商品名称
                    $("#txtMnemonicCode").val(goodsList.mnemonic_code);//助记码
                    $("#txtGeneric").val(goodsList.generic);//通用名
                    $("#txtDrugSpec").val(goodsList.drug_spec);//商品规格
                    $("#txtPackageUnit").val(goodsList.package_unit);//包装单位
                    $("#txtMinPackage").val(goodsList.min_package);//中包装
                    $("#txtBigPackage").val(goodsList.big_package);//计量规格
                    $("#txtDrugFactory").val(goodsList.drug_factory);//生产厂家
                    $("#txtApprovalNumber").val(goodsList.approval_number);//批准文号
                    $("#txtDosageForm").val(goodsList.dosage_form);//剂型
                    $("#txtLeiBie").val(goodsList.category);//类别
                    $("#txtStorageConditions").val(goodsList.Storage_conditions);//储存条件
                    $("#txtRate").val(goodsList.rate);//税率
                    $("#txtCkPrice").val(goodsList.price);//建议零售价
                    $("#txtGoodsBrandImg").val(goodsList.brand_img_url);//首页banner图
                    $("#txtGoodsImg").val(goodsList.img_url);//封面图片
                    $("#txtGoodsPic").val(goodsList.left_pic);//左侧大图
                    $("#zhaiyao").val(goodsList.zhaiyao);//商品摘要
                    var isZbz = goodsList.scattered;
                    if (isZbz == null || isZbz == "") {
                        isZbz = "N";
                    }
                    $("#sltIsZbz").val(isZbz);
                    var isDbz = goodsList.packControl;
                    if (isDbz == null || isDbz == "") {
                        isDbz = "N";
                    }
                    $("#sltIsDbz").val(isDbz);
                   
                    ///加载商品推荐属性
                    var RecommendList = obj.table3;
                    if (RecommendList != "" && RecommendList.length > 0) {
                        $.each(RecommendList, function (i) {
                            $('input[name="isRecommend"][value="' + RecommendList[i].id + '"]').attr('checked', true);
                        });
                    }
                    var ue = UE.getEditor('editor');
                    // editor准备好之后才可以使用
                    ue.addListener("ready", function () {
                        //赋值
                        var contentbase64 = $.base64.atob(goodsList.content, true, 'utf8')
                        ue.setContent(contentbase64);

                        ////取值
                        //var content = ue.getContent();
                    });
                }
                //加载商品价格
                if (obj.table1 != "" && obj.table1.length > 0) {
                }
                //加载商品图片
                if (obj.table2 != "" && obj.table2.length > 0) {
                }
                layer.close(index);
            }
            else {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//修改信息
function UpdateGoodsInfo()
{
    var index = layer.load(2);
    var article_id = GetQueryString("article_id");
    var sort_id=$("#txtSortId").val();//商品排序
    var category_id = $("#sltCategory").val();//商品分类
    if (category_id=='') {
        layer.msg("请先选择商品分类，请检查", { time: 3000 });
        layer.close(index);
        return;
    }
    var sub_title = $("#txtGoodsName").val();//商品名称
    if (category_id == '') {
        layer.msg("请填写商品名称，请检查", { time: 3000 });
        layer.close(index);
        return;
    }
    var mnemonic_code=$("#txtMnemonicCode").val();//助记码
    var generic=$("#txtGeneric").val();//通用名
    var drug_spec=$("#txtDrugSpec").val();//商品规格
    var package_unit=$("#txtPackageUnit").val();//包装单位
    var min_package=$("#txtMinPackage").val();//中包装
    var big_package = $("#txtBigPackage").val();//计量规格
    var isZbz = $("#sltIsZbz").val();//是否中包装控制
    var isDbz = $("#sltIsDbz").val();//是否大包装控制
    if (big_package == '') {
        layer.msg("请填写计量规格，请检查", { time: 3000 });
        layer.close(index);
        return;
    }
    var drug_factory = $("#txtDrugFactory").val();//生产厂家
    if (drug_factory == '') {
        layer.msg("请填写生产厂家，请检查", { time: 3000 });
        layer.close(index);
        return;
    }
    var approval_number = $("#txtApprovalNumber").val();//批准文号
    if (approval_number == '') {
        layer.msg("请填写批准文号，请检查", { time: 3000 });
        layer.close(index);
        return;
    }
    var dosage_form=$("#txtDosageForm").val();//剂型
    var category = $("#txtLeiBie").val();//类别datemin
    var Storage_conditions=$("#txtStorageConditions").val();//储存条件
    var rate="0";//税率
    var price = $("#txtCkPrice").val();//建议零售价
    if (price < 0) {
        layer.msg("请填写正确的建议零售价，请检查", { time: 3000 });
        layer.close(index);
        return;
    }
    var zhaiyao = $("#txtzhaiyao").val();
    var brand_img_url = "";//首页banner图
    var img_url = $("#txtGoodsImg").val();//封面图片
    var pattern = /^[a-zA-Z]:(\\(!?))/;
    if (pattern.test(img_url) == true) {
        layer.alert("图片未正确上传！", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        layer.close(index);
        return;
    }
    var left_pic = "";//左侧大图
    //var status = $("#sltIsShowId").val();//商品状态
    var content = "";//商品说明书
    var ue = UE.getEditor('editor');
    content = ue.getContent();
    ////获取商品推荐类型
    var recommendList = "";
    $("#isRecommend input[type=checkbox]:checked").each(function () { //由于复选框一般选中的是多个,所以可以循环输出选中的值  
        recommendList = $(this).val() + "," + recommendList;
    });
    var BrandCode = $("#txtBrandId").val();
    var json = {
        article_id: article_id,
        category_id: category_id,
        sub_title: sub_title,
        sort_id: sort_id,
        mnemonic_code: mnemonic_code,
        generic: generic,
        drug_spec: drug_spec,
        package_unit: package_unit,
        min_package: min_package,
        big_package: big_package,
        drug_factory: drug_factory,
        approval_number: approval_number,
        dosage_form: dosage_form,
        category: category,
        Storage_conditions: Storage_conditions,
        rate: rate,
        price: price,
        brand_img_url: brand_img_url,
        img_url: img_url,
        left_pic: left_pic,
        min_package_astrict: "N",
        //status: status,
        content: content,
        recommendList: recommendList,
        BrandCode: BrandCode,
        isZbz: isZbz,
        isDbz: isDbz,
        zhaiyao:zhaiyao
    };
    //var proc = "";//存储过程名
    //var type = "UpdateGoodsInfo";
    ///加载页面数据
    $.ajax({
        url: "../Goods/UpdateProduct",
        type: "Post",
        cache: false,
        data: json,
        dataType: "json",
        success: function (data) {
            var obj = JSON.parse(data);
            var type = obj.flag;
            if (type == '0') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.document.getElementById("searchBtn").click(); layer_close(); }
                });
            }
            else {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}

