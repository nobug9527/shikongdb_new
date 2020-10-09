
//商品信息查询
function SearchGoodsInfo(obj) {
    var index = layer.load(2);
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var pageIndex = $("#pageIndex").html();
    var sqlvalue = $("#txtStrWhere").val();
    var pageSize = "30";
    var status = $("#sltStatus").val();
    var floorId = $("#FloorId").val();
    var sqltype = "Integral_Goods_Select";
    var procedure = "Proc_Integral";
    var Type = "DataTable";
    var Json = {
        Procedure: procedure,
        Type: sqltype,
        Goodsname: sqlvalue,
        Status: status,
        Floottype: floorId,
        PageIndex: pageIndex,
        PageSize: pageSize
    };
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnDataTable.ashx?Type=" + Type + "&Json=" + encodeURI(JSON.stringify(Json)),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            var count = JSON["count"];
            if (TYPE == '0') {
                $('#TbShows').empty();
                var tb = "";
                for (var i = 0; i < JSON["data"].length; i++) {
                    tb = tb + "<tr class='text-c'>"
                    tb = tb + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>"
                    tb = tb + "<td align='center'>" + JSON["data"][i]["entname"] + "</td>"
                    tb = tb + "<td align='center'>" + JSON["data"][i]["goodscode"] + "</td>"
                    tb = tb + "<td align='center'>" + JSON["data"][i]["goodsname"] + "</td>"
                    tb = tb + "<td align='center'>" + JSON["data"][i]["drug_spec"] + "</td>"
                    tb = tb + "<td align='center'>" + JSON["data"][i]["package_unit"] + "</td>"
                    tb = tb + "<td align='center'>" + JSON["data"][i]["drug_factory"] + "</td>"
                    tb = tb + "<td align='center'>" + JSON["data"][i]["FlootType"] + "</td>"
                    tb = tb + "<td align='center'>" + JSON["data"][i]["Price"] + "</td>"
                    tb = tb + "<td align='center'>" + JSON["data"][i]["Integral"] + "</td>"
                    var status = JSON["data"][i]["is_zx"];
                    tb = tb + "<td align='center'>" + JSON["data"][i]["is_zx"] + "</td>"
                    tb = tb + "<td id='xgoodsid" + i + "' style='display:none'>" + JSON["data"][i]["goodsid"] + "</td>"
                    tb += "<td class='td-manage'>";
                    if (status == "是") {
                        tb += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"GoodsXG('" + JSON["data"][i]["goodsid"] + "','1')\"  title=\"下架\" class=\"btn btn-danger radius\">下架</a>";
                        tb += "<a title=\"编辑\"  onclick=\"GoodsXG('" + JSON["data"][i]["goodsid"] + "',\"XG\")\" class=\"btn btn-warning radius\" style=\"text-decoration:none;margin-right: 5px\">编辑</a>";                    }
                    else {
                        tb += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"GoodsXG('" + JSON["data"][i]["goodsid"] + "','2')\"  title=\"上架\" class=\"btn btn-danger radius\">上架</a>";
                        tb += "<a title=\"编辑\"  onclick=\"GoodsXG('" + JSON["data"][i]["goodsid"] + "',\"XG\")\" class=\"btn btn-warning radius\" style=\"text-decoration:none;margin-right: 5px\">编辑</a>";
                        tb += "<a style=\"text-decoration:none;margin-right: 5px;\" onClick=\"GoodsXG('" + JSON["data"][i]["goodsid"] + "','0')\"  title=\"删除\" class=\"btn btn-danger radius\">删除</a>";
                    }

                        
                    //tb = tb + "<td align='center'><a onclick='GoodsXG(\"" + JSON["data"][i]["goodsid"] + "\",\"XG\")'>修改</a>|<a onclick='GoodsXG(\"" + JSON["data"][i]["goodsid"] + "\",\"Delete\")'> 删除</a></td>"
                    tb += "</tb>";
                    tb = tb + "</tr>";
                }
                $("#TbShows").append(tb);
                var recordCount = JSON["recordCount"];
                var pageCount = JSON["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                layer.close(index);
            }
            if (TYPE == '1') {
                $('#TbShows').empty();
                layer.close(index);
            }
            if (TYPE == '2') {
                layer.close(index);
                layer.alert(JSON["data"][0]["message"], {
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

//js中获取url参数值，并赋值给对象theRequest
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}
//读取theRequest对象，并赋值
function evaluate() {
    //读取
    var Request = new Object();
    Request = GetRequest();
    var goodsId = Request["goodsid"];//商品编号
    var type = "Integral_Goods_SelectByGoodsId";
    var Procedure = "Proc_Integral";
    var Parameter = "goodsid=" + encodeURI(goodsId) + "&type=" + encodeURI(type) + "&Procedure=" + encodeURI(Procedure);
    $.ajax({ 
        type: 'Post',
        cache: false,
        url: 'ashx/IntegralGoodsOperation.ashx?' + Parameter,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (date) {
            var dt = date.data;
            if (date.return_code == "0") {
                $('#goodsId').val(dt[0]["goodsid"]);
                $('#goodsName').val(dt[0]["goodsname"]);
                $('#goodsSpecifications').val(dt[0]["drug_spec"]);
                $('#unit').val(dt[0]["package_unit"]);
                $('#classification').val(dt[0]["GoodsType"]);
                $('#floor').val(dt[0]["FlootType"]);
                $('#pictureText').val(dt[0]["img_url"]);
                $('#hiddenUrl').val(dt[0]["img_url"]);
                $('#vendel').val(dt[0]["factories_choosing"]);
                $('#price').val(dt[0]["Price"]);
                $('#integral').val(dt[0]["Integral"]);
                $('#Quantity').val(dt[0]["quantity"]);
            } else {
                alert(dt[0].message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    });

}
//更新数据
function updateGoods() {
    var nameParatement = RegExp("^(?!_)(?!.*?_$)[a-zA-Z0-9_\u4e00-\u9fa5]+$");
    var priceParatement = RegExp("^[0-9]{1,11}$|^[0-9]{0,11}\.[0-9]{1,3}$");
    var integralParatement = RegExp("^[0-9]{1,}$");
    var goodsId = $('#goodsId').val();
    var goodsName = $('#goodsName').val();
    var goodsSpecifications = $('#goodsSpecifications').val();
    var unit = $('#unit').val();
    var classification = $('#classification').val();
    var floor = $('#floor').val();
    var pictureText = $('#pictureText').val();
    var hiddenUrl = $('#hiddenUrl').val();
    var vendel = $('#vendel').val();
    var price = $('#price').val();
    var integral = $('#integral').val();
    var type = "Integral_Goods_Update";
    var Procedure = "Proc_Integral";
    var Quantity = $('#Quantity').val();
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
    } else if (hiddenUrl == "" && classification != '赠品') {
        alert("图片路径不能为空！");
    } else if (vendel == "") {
        alert("厂家不能为空！");
    } else if (price == "") {
        alert("价格不能为空！");
    } else if (integral == "") {
        alert("积分不能为空！");
    }
    else if (!priceParatement.test(price)) {
        alert("价格长度最多11位，小数点后最多保留3位！");
    }
    else if (!integralParatement.test(integral)) {
        alert("积分必须为整数!");
    } else {
        var Parameter = "goodsid=" + goodsId + "&goodsname=" + encodeURI(goodsName) + "&drug_spec=" + encodeURI(goodsSpecifications) + "&package_unit=" + encodeURI(unit)
            + "&FlootType=" + encodeURI(floor) + "&GoodsType=" + encodeURI(classification) + "&Price=" + price + "&img_url=" + hiddenUrl + "&Integral="
            + encodeURI(integral) + "&factories_choosing=" + encodeURI(vendel) + "&type=" + encodeURI(type) + "&Procedure=" + Procedure + "&Quantity=" + Quantity;
        $.ajax({
            type: 'Post',
            cache: false,
            url: 'ashx/IntegralGoodsOperation.ashx?' + Parameter,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.return_code == "0") {
                    alertFun(data.data[0]["message"], function () { }, 's');
                    parent.location.reload();
                } else if (data.return_code == "1") {
                    alertFun(data.data[0]["message"], function () { }, 'f');
                }
            }
        });
    }
}
//商品修改
function GoodsXG(goodsid, type) {
    if (type == "0") {
        if (confirm("是否删除该商品")) {
            DeleteGoods(goodsid,"0");
        }
    }
    else if (type == "1" || type == "2")
    {
        DeleteGoods(goodsid, type);
    }
    else {
        window.location = "IntegralGoodsUpdate.aspx?goodsid=" + goodsid;
    }
}
///删除商品
function DeleteGoods(goodsid,status)
{
    var index = layer.load(2);
    var sqltype = "Integral_Goods_Delete";
    var procedure = "Proc_Integral";
    var Type = "ExecuteNonQuery";
    var codevalue = "";
    var Json = {
        Procedure: procedure,
        Type: sqltype,
        Goodsid: goodsid,
        status: status
    };
    $.ajax({
        type: "Post",
        cache: false,
        url: "ashx/ReturnDataTable.ashx?Type=" + Type + "&Json=" + encodeURI(JSON.stringify(Json)),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var JSON = data;
            var TYPE = JSON["return_code"];
            layer.close(index);
            if (TYPE == "0") {
                SearchGoodsInfo();
            }
            else {
                layer.alert(JSON["data"][0]["message"], {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}