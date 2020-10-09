///创建选择商品UI
function CreatGoodsInfoUI() {
    var contHtml = "";
    contHtml += "<a class='closeBtn' onclick='closeCover();'><img src='../img/close.png'/></a>"
    contHtml += "<h3 class='winPopTit' id='maskTitle'>商品选择</h3>"
    contHtml += "<div class='winPopCon'>"
    contHtml += "<h2 class='winPoph2' id='maskTitle_left'></h2>"
    contHtml += "<div id='maskContent'>"
    contHtml += "<div class='conTable'>"
    contHtml += "<table class=\"dataintable\" id='TBSHOW'>"
    contHtml += "<tr class='chcol'>"
    contHtml += "<tr><th>正在加载类容........</th></tr>"
    contHtml += "</table>"
    contHtml += "</div>"
    contHtml += "<div class='default' id='fanye'>"
    contHtml += "<p class='fl'>"
    contHtml += "<a onclick='Gbtnfirst(Get_GoodsInfo)'>首页</a>"
    contHtml += "<a style='color:Blue' onclick='Gbtnup(Get_GoodsInfo)'>上一页</a>"
    contHtml += "<a style='color:Blue' onclick='Gbtnnext(Get_GoodsInfo)'>下一页</a>"
    contHtml += "<a onclick='Gbtnlast(Get_GoodsInfo)'>尾页</a>"
    contHtml += "<span class='fr'>每页显示<b id='GPageSize'>15</b>条，共<b id='GRecordCount'>0</b>条,"
    contHtml += "当前页<b id='GPageIndex'>1</b>/<b id='GPageCount'>1</b></span>"
    contHtml += "</p>"
    contHtml += "</div>"
    contHtml += "</div>"
    contHtml += "</div>"
    document.getElementById("winPop").innerHTML = contHtml;
}
//获取商品信息
function Search_GoodsInfo() {
    closeCover();
    openCover(); //生成弹窗
    CreatGoodsInfoUI();
    Get_GoodsInfo();
}
function Get_GoodsInfo() {
    openLoading();
    var sqlvalue = $("#GoodsCodeTxt").val();
    var PageSize = $("#GPageSize").html();
    var PageIndex = $("#GPageIndex").html();
    var sqltype = "GetGoodsInfo";
    var Procedure = "Proc_ImgAdmin";
    var Type = "SearchGoodsInfo";
    var codevalue = "";
    var paramcont = "&PageSize=" + PageSize + "&PageIndex=" + PageIndex + "&sqlvalue=" + encodeURI(sqlvalue) + "&sqltype=" + sqltype + "&Procedure=" + Procedure;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/CXmanager.ashx?Type=" + Type + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBSHOW').empty();
                var tb = "<tr class='chcol'><th>序号</th><th>商品编号</th><th>商品名称</th><th>商品规格</th><th>中包装</th><th>生产厂家</th><th>库存</th><th>操作</th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='select" + i + "' ondblclick='ChooseGoods(\"" + i + "\")' onMouseOver='changecolor(\"" + i + "\")' onMouseOut='deletetcolor(\"" + i + "\")'>"
                                + "<td>" + ((parseInt(PageIndex) - 1) * parseInt(PageSize) + parseInt(i) + 1) + "</td>"
                                + "<td id='xspbh" + i + "'>" + JSON["data"][i]["call_index"] + "</td>"
                                + "<td id='xspmch" + i + "'>" + JSON["data"][i]["sub_title"] + "</td>"
                                + "<td id='xshpgg" + i + "'>" + JSON["data"][i]["drug_spec"] + "</td>"
                                + "<td id='xzbz" + i + "'>" + JSON["data"][i]["min_package"] + "</td>"
                                + "<td id='xshpgg" + i + "'>" + JSON["data"][i]["factories_choosing"] + "</td>"
                                + "<td id='xquantity" + i + "'>" + JSON["data"][i]["stock_quantity"] + "</td>"
                                + "<td id='xarticleid" + i + "' style='display:none'>" + JSON["data"][i]["article_id"] + "</td>"
                                + "<td><span style='color:blue' ><a onclick='ChooseGoods(\"" + i + "\")'>选择</a></td></tr>"
                }
                $("#TBSHOW").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                document.getElementById("GRecordCount").innerHTML = recordCount;
                document.getElementById("GPageCount").innerHTML = pageCount;
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
function ChooseGoods(i) {
    openLoading();
    var spmch = $("#xspmch" + i + "").html();
    var spbh = $("#xspbh" + i + "").html();
    var Articleid = $("#xarticleid" + i + "").html();
    var valdate = $("#valdate" + i + "").html();
    var zbz = $("#xzbz"+i+"").html();

    $("#qglimit").val(zbz);
    $("#OneNum").val(zbz);
    $("#GoodsNametxt").val(spmch);
    $("#GoodsCodeTxt").val(spbh);
    $("#ArticleIdtxt").val(Articleid);

    closeLoading();
    closeCover();
}

//查询商品图片信息
function QueryImg() {
    
    document.getElementById('imgdialog').style.display = 'block';
    openLoading();
    var LongType = $("#FATypeID").val();
    var Articleid = $("#ArticleIdtxt").val();  //唯一主键
    var sqltype = "Query_Img";
    var Procedure = "Proc_ImgAdmin";
    var Type = "Query_Img";
    var paramcont = "Type=" + Type + "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&LongType=" + LongType + "&Articleid=" + Articleid;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/CXmanager.ashx?" + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#contnet').empty();
                var tb = "<tr class='chcol'><th>选择</th><th>序号</th><th>图片编号</th><th>商品名称</th><th>商品图片</th><th>是否发布</th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='select" + i + "' onMouseOver='changecolor(\"" + i + "\")' onMouseOut='deletetcolor(\"" + i + "\")'>"
                                + "<td align='center' ><input type='radio' name='testradio' value='" + JSON["data"][i]["img_url"] + "'/></td>"
                                + "<td align='center'>" + (parseInt(i) + 1) + "</td>"
                                + "<td id='ximgid" + i + "' align='center'>" + JSON["data"][i]["ID"] + "</td>"
                                + "<td id='xtitle" + i + "' align='center'>" + JSON["data"][i]["title"] + "</td>"
                                + "<td id='xurl" + i + "' align='center' ><img src='" + JSON["data"][i]["img_url"] + "' alt='' style='width:30px;height:30px;'  /></td>"
                                + "<td id='xis_zx" + i + "' align='center'>" + JSON["data"][i]["is_zx"] + "</td></tr>"
                }
                $("#contnet").append(tb);
                closeLoading();
            }
            if (TYPE == '1') {
                $("#contnet").append("暂无相关图片信息");
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
//将图片的路径赋值
function getImg() {
    var imgurl = $('input[name="testradio"]:checked').val();
    $("#spimg").val(imgurl);
    document.getElementById('imgdialog').style.display = 'none';
}
//获取图片类型
function GetImgType()
{
    openLoading();
    var sqltype = "GetImgType";
    var Procedure = "Proc_ImgAdmin";
    var Type = "GetImgType";
    var paramcont = "Type=" + Type + "&Procedure=" + Procedure + "&sqltype=" + sqltype;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/CXmanager.ashx?" + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                if (JSON["data"].length > 0) {
                    $('#ImgSelectId').empty();
                    var htmlString = "";
                    for (var i = 0; i < JSON["data"].length; i++) {
                        htmlString = htmlString + "<option value='" + JSON["data"][i]["ImgType"] + "'>" + JSON["data"][i]["TypeName"] + "</option>"
                    }

                   $("#ImgSelectId").append(htmlString);
                }
                closeLoading();
            }
           else {
                closeLoading();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeLoading();
        }
    })
}
///创建选择赠品商品UI
function CreatFreeGoodsInfoUI(lx) {
    var contHtml = "";
    contHtml += "<a class='closeBtn' onclick='closeCover();'><img src='../img/close.png'/></a>"
    if (lx == "MZ") {
        contHtml += "<h3 class='winPopTit' id='maskTitle'>选择赠品</h3>"
    }
    else {
        contHtml += "<h3 class='winPopTit' id='maskTitle'>选择优惠券</h3>"
    }
    contHtml += "<div class='winPopCon'>"
    contHtml += "<h2 class='winPoph2' id='maskTitle_left'></h2>"
    contHtml += "<div id='maskContent'>"
    //contHtml += "<div class='conForm' id='cxtjdiv'>"
    //contHtml += "<p><label>商品编号：</label><input type='text' id='spbhtxt'  placeholder='关键词搜索'></p>"
    //contHtml += "<a class='btn btn-success' onclick='Get_GoodsInfo()'><i class='searchIcon btnIcon'></i>查 询</a>"
    //contHtml += "</div>"
    contHtml += "<div class='conTable'>"
    contHtml += "<table class=\"dataintable\" id='TBSHOW'>"
    contHtml += "<tr class='chcol'>"
    contHtml += "<tr><th>正在加载类容........</th></tr>"
    contHtml += "</table>"
    contHtml += "</div>"
    contHtml += "</div>"
    contHtml += "</div>"
    document.getElementById("winPop").innerHTML = contHtml;
}
//生成查询赠品信息弹窗
function Search_FreeGoodsInfo() {
    closeCover();
    openCover(); //生成弹窗
    var lx = $("#FATypeID").val();
    CreatFreeGoodsInfoUI(lx);
    if (lx == "MZ" || lx=="HG") {
        Query_freegoods();
    }
    else {
        Query_Coupon();
    }
}
//选择优惠券信息
function Query_Coupon() {
    openLoading();
    var sqltype = "CouponSearch";
    var Procedure = "Proc_ImgAdmin";
    var Type = "SearchCouponInfo";
    var paramcont = "Type=" + Type + "&Procedure=" + Procedure + "&sqltype=" + sqltype;
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/CXmanager.ashx?" + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBSHOW').empty();
                var tb = "<tr class='chcol'><th>序号</th><th>编号</th><th>优惠券名</th><th>满足金额</th><th>优惠金额</th><th>开始日期</th><th>截止日期</th><th>客户类型</th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='select" + i + "' ondblclick='ChooseCoupon(\"" + i + "\")' onMouseOver='changecolor(\"" + i + "\")' onMouseOut='deletetcolor(\"" + i + "\")'>"
                        + "<td>" + (parseInt(i) + 1) + "</td>"
                        + "<td id='code" + i + "'>" + JSON["data"][i]["code"] + "</td>"
                        + "<td id='name"+i+"'>" + JSON["data"][i]["CouponName"] + "</td>"
                        + "<td >" + JSON["data"][i]["MeetCount"] + "</td>"
                        + "<td >" + JSON["data"][i]["DisCount"] + "</td>"
                        + "<td >" + JSON["data"][i]["StartDate"] + "</td>"
                        + "<td >" + JSON["data"][i]["EndDate"] + "</td>"
                        + "<td >" + JSON["data"][i]["pricerelation"] + "</td>"
                        + "</tr>"
                }
                $("#TBSHOW").append(tb);
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
    });
}
//选中优惠券
function ChooseCoupon(i) {
    openLoading();
    var couponCode = $("#code" + i + "").html();
    var couponName = $("#name" + i + "").html();

    $("#cxspid").val(couponCode);
    $("#cuxiaoZP").val(couponName);
    closeLoading();
    closeCover();
}
//选择促销赠品信息
function Query_freegoods() {
    openLoading();
    var sqltype = "GetFreeGoodsInfo";
    var Procedure = "Proc_ImgAdmin";
    var Type = "SearchFreeGoodsInfo";
    var CXType = $("#FATypeID").val();
    var sqlvalue = $("#cuxiaoZP").val();
    var paramcont = "Type=" + Type + "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&CXType=" + encodeURI(CXType) + "&sqlvalue=" + encodeURI(sqlvalue);
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/CXmanager.ashx?" + paramcont,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TBSHOW').empty();
                var tb = "<tr class='chcol'><th>序号</th><th>商品编号</th><th>商品名称</th><th>商品规格</th><th>包装单位</th><th>库存</th></tr>"
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='chcol' id='select" + i + "' ondblclick='ChooseFreeGoods(\"" + i + "\")' onMouseOver='changecolor(\"" + i + "\")' onMouseOut='deletetcolor(\"" + i + "\")'>"
                                + "<td>" + (parseInt(i) + 1) + "</td>"
                                + "<td id='xspbh" + i + "'>" + JSON["data"][i]["goodscode"] + "</td>"
                                + "<td id='xspmch" + i + "'>" + JSON["data"][i]["goodsname"] + "</td>"
                                + "<td id='xshpgg" + i + "'>" + JSON["data"][i]["drug_spec"] + "</td>"
                                + "<td id='xshpgg" + i + "'>" + JSON["data"][i]["package_unit"] + "</td>"
                                + "<td>" + JSON["data"][i]["quantity"] + "</td>"
                                + "<td id='xgoodsid" + i + "' style='display:none'>" + JSON["data"][i]["goodsid"] + "</td>"
                                + "</tr>"
                }
                $("#TBSHOW").append(tb);
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
//赋值
function ChooseFreeGoods(i) {
    openLoading();
    var goodsid = $("#xgoodsid" + i + "").html();
    var goodsname = $("#xspmch" + i + "").html();

    $("#cxspid").val(goodsid);
    $("#cuxiaoZP").val(goodsname);
    closeLoading();
    closeCover();
}
//点击保存确认提交数据
function Creat_QG_Promotion() {
    openLoading();
    var nullNum = 0;
    //公用的数据部分
    var LongType = $("#FATypeID").val();
    var FaBu = $('input[name="ReleaseID"]:checked').val();
    var Show = $('input[name="ShowID"]:checked').val();  //获取单选框中的值
    //var spbh = $("#GoodsCodeTxt").val();   //通过唯一的主键进行关联，因此不需要传递自身的值
    //var spmch = $("#GoodsNametxt").val();
    var spimg = $("#spimg").val();
    var ksrq = $("#startDate").val();
    var jsrq = $("#endDate").val();
    var kssj = $("#hour_kstime").val() + ":" + $("#minute_kstime").val();
    var jssj = $("#hour_jstime").val() + ":" + $("#minute_jstime").val();
    var miaoshu = $("#fangan").val();     //方案描述
    var qgamount = $("#qgamount").val();  //抢购总数
    var Articleid = $("#ArticleIdtxt").val();  //唯一主键
    var qglimit = $("#qglimit").val();   //客户限购
    var OneNum = $("#OneNum").val();     //单次数量
    var zpshl = $("#ZPShl").val();//赠品数量
    var zphshj = $("#ZPHshj").val();//赠品价格
    var kefl = $("#itemlist").val();///客户类型
    //特殊部分
    var koul = $("#koul").val();
    if (koul == "") {
        var koul = "100.00";
    }
    //是否促销价
    var mzprice = $("#btprice").val();
    //促销价格
    var mz_editprice = $("#cxprice").val();
    if (mz_editprice == "") {
        mz_editprice = 0;
    }
    //赠品id
    var goodsid = $("#cxspid").val();
    //判断促销价大于零
    if (mzprice == "是" && LongType != "MJ" && parseFloat(mz_editprice) <= 0) {
        alertFun("促销价格不能小于0！", function () { closeLoading() }, 'f');
        return;
    }
    //验证是否选择赠品
    if ((LongType == "MZ" || LongType == "HG") && goodsid == "") {
        alertFun("请选择赠品！", function () { closeLoading() }, 'f');
        return;
    }
    if (LongType == "SYHJ" && goodsid == "") {
        alertFun("请选择优惠券！", function () { closeLoading() }, 'f');
        return;
    }
    if (LongType == "SYHJ" && isNaN(parseFloat(zpshl))) {
        alertFun("满足金额格式错误！", function () { closeLoading() }, 'f');
        return;
    }
    if (LongType == "HG" && parseFloat(zphshj) <= 0) {
        alertFun("赠品价格不能小于0！", function () { closeLoading() }, 'f');
        return;
    }
    if (LongType == "SYHJ") {
        var arryList = [LongType, FaBu, Show, ksrq, jsrq, kssj, jssj, miaoshu, Articleid, mzprice, spimg, kefl]
    }
    else {
        var arryList = [LongType, FaBu, Show, ksrq, jsrq, kssj, jssj, miaoshu, qgamount, Articleid, qglimit, OneNum, mzprice, spimg, kefl]
    }
    $.each(arryList, function (n, value) {
        if (value == '') {
            nullNum++;
        }
    });
    if (LongType != "SYHJ") { 
        if (isNaN(parseFloat(qgamount)) || isNaN(parseFloat(mz_editprice))) {
            alertFun("输入的数量格式不正确！", function () { closeLoading() }, 'f');
            return;
        }
        else if (parseFloat(qgamount) <= 0) {
            alertFun("抢购总数量不能小于0！", function () { closeLoading() }, 'f');
            return;
        }
    }
    if (nullNum > 0) {
        alertFun("文本框中内容不能为空", function () { closeLoading() }, 'f');
    }
    else {
            var sqltype = "CreateQG";
            if (LongType == "QG") {
                sqltype = "CreateQG";
            }
            else if (LongType == "MZ") {
                sqltype = "CreateMZ";
            }
            else if (LongType == "MJO") {
                sqltype = "CreateMJ";
            }
            else if (LongType == "XQ") {
                sqltype = "CreateXQ";
            }
            else if (LongType == "HG") {
                sqltype = "CreateHG"
            }
            else if (LongType == "SYHJ") {
                sqltype = "CreateSYHJ"
            }
            var Type = "CreateQG";
            var Procedure = "Promotion_Plan_Create";
            var paramcont = "Type=" + Type + "&LongType=" + LongType + "&FaBu=" + encodeURI(FaBu) + "&Show=" + encodeURI(Show) + "&ksrq=" + ksrq + "&jsrq=" + jsrq + "&kssj=" + kssj + "&jssj=" + jssj
                + "&miaoshu=" + encodeURI(miaoshu) + "&qgamount=" + encodeURI(qgamount) + "&Articleid=" + Articleid + "&qglimit=" + qglimit + "&OneNum=" + encodeURI(OneNum) + "&spimg=" + (spimg)
                + "&Procedure=" + Procedure + "&sqltype=" + sqltype + "&mzprice=" + encodeURI(mzprice) + "&mz_editprice=" + mz_editprice + "&goodsid=" + encodeURI(goodsid) + "&koul=" + encodeURI(koul)
                + "&zpshl=" + zpshl + "&zphshj=" + encodeURI(zphshj) + "&kefl=" + kefl;

            $.ajax({
                type: "Post",
                cache: false,
                url: "ashx/CXmanager.ashx?" + paramcont,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var JSON = data;
                    var TYPE = JSON["return_code"];
                    if (TYPE == '0') {
                        alertFun("存盘成功！", function () { closeLoading(); window.location.reload(); });
                    }
                    else {
                        var msg = JSON["data"][0]["message"];
                        alertFun(msg, function () { closeLoading() }, 'f');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                    closeLoading()
                }
            })

        }
    
}