//获取渠道类型列表
function QueryCouponTypeList(obj) {
    var index = layer.load(2);
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var pageSize = '7';
    var txtStrCode = $("#txtStrCode").val();
    var txtStrName = $("#txtStrName").val();
    var data = {
        type: "Pc_GetTypeCoupon",
        typeCode: txtStrCode,
        typeName: txtStrName,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Pc_Coupon"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "coupon/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="typecode" value="' + obj[i]["typeCoding"] + '"></td>'
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>'
                    html += '<td>' + obj[i]["typeCoding"] + '</td>'
                    html = html + '<td style="text-align:left">' + obj[i]["typeName"] + '</td>'
                    html += '<td>' + obj[i]["channelName"] + '</td>'
                    html += '<td>' + obj[i]["instructionManual"] + '</td>'
                    html += '<td class="f-14 td-manage">';
                    html += '<a class="btn btn-success radius" style="text-decoration:none" class="ml-5" onclick="Open(\'编辑设备类型\',\'coupon_type_Add.html\',' + obj[i]["typeCoding"] + ',\'\',\'\')" >编辑</i></a> ';
                    
                    html += '</td>';
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                layer.close(index);
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                layer.close(index);
            }
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            } else if (type == '2') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
               
            }
            else if (type == '4') {
                $('body').html('<img src="../UploadImages/素材/权限不足.png" style="width:75%"/>');
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
            $('body').removeAttr("style");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}

//获取优惠券列表
function QueryCouponList(obj) {
    var index = layer.load(2);
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var pageSize = '15';
    var txtStrCode = $("#txtStrCode").val();
    var txtStrName = $("#txtStrName").val();
    var sltGZ = $("#sltGZ").val();
    var sltSB = $("#sltSB").val();
    var sltLQ = $("#sltLQ").val();
    var sltCJ = $("#sltCJ").val();
    var status = $("#sltStatus").val();
    
    var data = {
        type: "Pc_GetCoupon",
        couponCode: txtStrCode,
        couponName: txtStrName,
        PageIndex: pageIndex,
        PageSize: pageSize,
        sltGZ: sltGZ,
        sltSB: sltSB,
        sltLQ: sltLQ,
        sltCJ: sltCJ,
        status: status
    };
    var proc = "Pc_Coupon"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "coupon/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="coupon" value="' + obj[i]["couponCode"] + '"></td>'
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>'
                    html += '<td>';
                    if (obj[i]["status"] == '下架') {
                        html += '<a style="margin-left:10px" class="btn btn-success radius" onclick="dataStatus(\'' + obj[i]["couponCode"] + '\',' + 1 + ')" href="javascript:;"><i class="Hui-iconfont">&#xe679;</i> 上架</a>'
                        html += '<a style="margin-left:10px" class="btn btn-danger  radius" onclick="dataStatus(\'' + obj[i]["couponCode"] + '\',' + 2 + ')" href="javascript:;"><i class="Hui-iconfont">&#xe674;</i> 删除</a>'
                    } else {
                        html += '<a style="margin-left:10px" class="btn btn-warning  radius" onclick="dataStatus(\'' + obj[i]["couponCode"] + '\',' + 0 + ')" href="javascript:;"><i class="Hui-iconfont">&#xe674;</i> 下架</a>'
                    }

                    html += '</td>';
                    html += '<td>' + obj[i]["couponCode"] + '</td>'
                    html += "<td onClick='CouponDtOpen(\"优惠卷详情\",\"coupon_detail.html\",\"" + obj[i]["couponCode"] + "\",\"" + obj[i]["entid"] + "\")'>" + obj[i]["couponName"] + "</td>"
                  
                    html += '<td>' + obj[i]["couponsNumber"] + '</td>'
                    html += '<td>' + obj[i]["typetitle"] + '</td>'
                    html += '<td>' + obj[i]["receivingType"] + '</td>'
                    html += '<td>' + obj[i]["sceneName"] + '</td>'
                    html += '<td>' + obj[i]["SceneSon"] + '</td>'
                   
                    html += '<td>' + obj[i]["coupontype"] + '</td>'
                    html += '<td>' + obj[i]["startingTime"] + '</td>'
                    html += '<td>' + obj[i]["endTime"] + '</td>'
                    if (obj[i]["status"] == '上架') {
                        html += '<td class="td-status"><span class="label label-success radius">已上架</span></td>'
                    } else {
                        html += '<td class="td-status"><span class="label label-danger radius">已下架</span></td>'
                    }
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                layer.close(index);
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                layer.close(index);
            } else if (type == '2') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
            }
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            }else if (type == '4') {
                $('body').html('<img src="../UploadImages/素材/权限不足.png" style="width:75%"/>');
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
            $('body').removeAttr("style");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}

function Open(title, url, id,code,name) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id + "&title=" + title + "&code=" + code + "&name=" + name
    });
    layer.full(index);
}

//优惠卷详情
function CouponDtOpen(title, url, id, entid) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id + "&entid=" + entid
    });
    layer.full(index);
}

//库存修改
function kucun_update(obj) {
    var index = layer.load(2);
    var id = $(obj).attr("data-id");
    if ($(obj).val() <0 & $(obj).val() != "undefined") {
        layer.alert('数值不允许小于0', {
            icon: 2,
            skin: 'layer-ext-moon',
            yes: function () { layer.closeAll(); }
        });
    }
    var data = {
        type: "PC_UpCouponNumber",
        couponCode: id,
        couponsNumber: $(obj).val()
    };
    var proc = "Pc_Coupon"//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "coupon/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (obj) {
            var type = obj.flag;
            if (type == '0') {
                layer.close(index);
                layer.alert('库存修改成功', {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
                QueryCouponList();
            }
            if (type == '1') {
                layer.alert('失败', {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
            else if (type == '2') {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            closeLoading()
        }
    })
}
function tuchuinput(id) {
    $("#spanlink" + id).css("display", "none");
    $("#inputlink" + id).css("display", "initial");
}

///优惠券客户范围选择
function ChooseKhkz() {
    var khkz = $("#sltkhkz").val();
    if (khkz != "0") {
        $("#divKhfw").show();
        ChoseHykzInfo();
    }
    else {
        $("#divKhfw").hide();
    }
}
function ChoseHykzInfo(title, type, entId, khkzType) {
    layer_show(title, "SearchInfo_yhq.html?type=" + type + "&proc=proc_dkxd_salesman&sqlType=GetMemberList&entId=" + encodeURI(entId) + "&khkzType=" + khkzType, 1100, 700);
}

