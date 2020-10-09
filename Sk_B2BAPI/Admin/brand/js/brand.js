//上传图片
function UpLoadImg() {

    var index = layer.load(2);   //页面正在加载提示

    var fileName = $('#brandFile').val();
    var files = $('#brandFile').get(0).files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append("uploadfile", files[i]);
    }
    $.ajax({
        type: "post",
        url: "/Admin/ashx/UploadImg.ashx?type=UploadImage&imgType=brand",
        data: fileData,
        cache: false,
        dataType: "json",
        contentType: false,
        processData: false,
        success: function (result) {
            layer.close(index);
            if (result.flag=="0") {
                $('#brandFile').attr('src', result.message);
                $("#txtBrandImgUrl").val(result.message);
                $('#imgId').attr('src', result.message);
                $("#divimg").show();
            }
            else {
                layer.alert(result.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            layer.msg("未知错误", { time: 3000 });
        }
    });
}
//上传图片
function UpLoadImgDt() {

    var index = layer.load(2);   //页面正在加载提示

    var fileName = $('#brandDtFile').val();
    var files = $('#brandDtFile').get(0).files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append("uploadfileDt", files[i]);
    }
    $.ajax({
        type: "post",
        url: "/Admin/ashx/UploadImg.ashx?type=UploadImage&imgType=brand",
        data: fileData,
        cache: false,
        dataType: "json",
        contentType: false,
        processData: false,
        success: function (result) {
            layer.close(index);
            if (result.flag == "0") {
                $('#brandDtFile').attr('src', result.message);
                $("#txtBrandDtImgUrl").val(result.message);
            }
            else {
                layer.alert(result.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            layer.alert("出错了，请重试！" + "\\n readyState:" + jqXHR.readyState + "\\n responseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
        }
    });
}
//品牌维护选择商品
//修改客户信息
function BreadGoodsOpen() {
    var strWhere = $("#txtGoodCode").val();
    var lsid = $("#lsid").html();
    layer_show("选择品牌厂家", "SearchInfo.html?type=Brand&proc=Proc_Admin_SearchInfo&sqlType=GetDrugFactory&strWhere=" + encodeURI(strWhere) + "&lsid=" + lsid, 1000, 600);
}
function Query_Brand_Ls()
{
    var index = layer.load(2);
    var lsid = $("#lsid").html();
    var data = {
        type: "Query_Brand_Ls",
        lsid: lsid
    };
    var proc = "Proc_Admin_Brand";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "brand/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td>' + parseInt(i+1) + '</td>'
                    html += '<td>' + obj[i]["goodscode"] + '</td>'
                    html += '<td>' + obj[i]["sub_title"] + '</td>'
                    html += '<td>' + obj[i]["drug_spec"] + '</td>'
                    html += '<td>' + obj[i]["min_package"] + '</td>'
                    html += '<td>' + obj[i]["drug_factory"] + '</td>'
                    html += '<td>' + obj[i]["stock_quantity"] + '</td>'
                    html += '<td><input id="txtSortId' + i + '" type="text" value="' + obj[i]["sort_id"] + '"  style="width:50px;height:20px" class="input-text" onKeyDown="UpdateGoodsSort(\'' + obj[i]["id"] + '\',\'' + i + '\')" /></td>'
                    html += '<td><a style="text-decoration:none" onClick="Dlt_Brand_Ls(\'' + obj[i]["id"] + '\')" title="删除"><i class="Hui-iconfont">删除</i></a></td>'
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                layer.close(index);
            }
            else if (type == '1') {
                $('#TbShows').empty();
                layer.close(index);
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
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
//修改商品排序回车事件
function SortKeyDown()
{
    UpdateGoodsSort();
}
function UpdateGoodsSort(id, i) {
    if (event.keyCode == 13) {
        var index = layer.load(2);
        var sort_id = $("#txtSortId" + i + "").val();
        var proc = "Proc_Admin_Brand";//存储过程名
        var type = "ReturnNumber";
        var json = {
            type: "Update_Brand_Ls",
            listId: id,
            sort_id: sort_id
        };
        ///加载页面数据
        $.ajax({
            type: "Post",
            cache: false,
            url: "brand/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var type = result.flag;
                if (type == '0') {
                    layer.close(index);
                    layer.msg("修改成功", {time:2000});
                }
                else {
                    layer.alert(result.message, {
                        icon: 2,
                        skin: 'layer-ext-moon',
                        yes: function () { layer.closeAll(); }
                    });
                }
            }
        })
    }
}
//删除选中的商品
function Dlt_Brand_Ls(id)
{
    var index = layer.load(2);
    var proc = "Proc_Admin_Brand";//存储过程名
    var type = "ReturnNumber";
    var json = {
        type: "Dlt_Brand_Ls",
        listId: id,
    };
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "brand/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                Query_Brand_Ls();
            }
            else {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
        }
    })
}
//存盘
function Save_Brand()
{
    var index = layer.load(2);
    var lsid = $("#lsid").html();
    var name = $("#txtName").val();
    var imgUrl = $("#txtBrandImgUrl").val();
    var pattern = /^[a-zA-Z]:(\\(!?))/;
    if (pattern.test(imgUrl) == true) {
        layer.alert("图片未正确上传！", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        layer_close();
        return;
    }
    var linkUrl = $("#txtLinkUrl").val();
    var imgDtUrl = $("#txtBrandDtImgUrl").val();
    var note = $("#txtNote").val();
    var floorid=$("input[name='floorid']:checked").val();
    var sortId = $("#txtSortId").val();
    var billno = GetQueryString("id");
    if (name == "" || imgUrl == "" || imgDtUrl == "" || sortId == "")
    {
        layer.msg("数据不完整请填写完整后提交！", { time: 3000 });
        layer_close();
        return;
    }
    var json = {
        type: "SaveBrand",
        name: name,
        lsid: lsid,
        linkUrl: linkUrl,
        img_url: imgUrl,
        img_url_dt: imgDtUrl,
        beizhu: note,
        sort_id: sortId,
        billno: billno,
        floor_id: floorid
    };
    var proc = "Proc_Admin_Brand";//存储过程名
    var type = "ReturnListNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "brand/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index); 
                layer.msg("存盘成功", { time: 3000 });
                layer_close();
                parent.location.reload();
            }
            else {
                layer.close(index); 
                layer.msg("保存异常，请检查网络，以及联系维护人员", { time: 3000 });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}

//查看品牌列表
function QueryBrandList(obj)
{
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
    var status = $("#sltIsShowId").val();
    var strWhere = $("#txtStrWhere").val();
    var pageSize = "15";
    var data = {
        type: "Query_Brand",
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Admin_Brand";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "brand/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="" value=""></td>'
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) +'</td>'
                    html = html + '<td><u style="cursor:pointer" class="text-primary" onClick="BrandDtOpen(\'' + obj[i]["billno"] + '\',\'brand_editor.html\',\'' + obj[i]["billno"] + '\')">' + obj[i]["billno"] + '</u></td>'
                    html += '<td>' + obj[i]["name"] + '</td>'
                    html += '<td>' + obj[i]["faType"] + '</td>'
                    html += '<td>' + obj[i]["beizhu"] + '</td>'
                    html += '<td>' + obj[i]["add_time"] + '</td>'
                    html += '<td>' + obj[i]["sort_id"] + '</td>'
                    var status = obj[i]["status"];
                    if (status == "1")
                    {
                        html += '<td class="td-status"><span class="label label-danger radius">未上架</span></td>'
                    }
                    else if (status == "2")
                    {
                        html += '<td class="td-status"> <span class="label label-success radius">已上架</span></td>'
                    }
                    html += '<td class="f-14 td-manage">';
                    html += '<a class="btn btn-secondary radius" style="text-decoration:none;margin-right: 10px;" class="ml-5" onClick="BrandDtOpen(\'' + obj[i]["name"] + '\',\'brand_goods_new.html\',\'' + obj[i]["billno"] + '\')" href="javascript:;" title="品牌商品">品牌商品</a>';
                    if (status == "1") {
                        html += '<a class="btn btn-warning radius" style="text-decoration:none;margin-right: 10px;" onClick="AuditBrand(\'' + obj[i]["billno"] + '\',\'2\')" href="javascript:;" title="上架">上架</a>';
                        html += '<a class="btn btn-success radius" style="text-decoration:none;margin-right: 10px;" class="ml-5" onClick="BrandDtOpen(\'' + obj[i]["name"] + '\',\'brand_editor.html\',\'' + obj[i]["billno"] + '\')" href="javascript:;" title="编辑">编辑</a>';
                        html += '<a class="btn btn-danger radius" style="text-decoration:none" class="ml-5" onClick="AuditBrand(\'' + obj[i]["billno"] + '\',\'0\')" href="javascript:;" title="删除">删除</a>'
                    }
                    else if (status == "2") {
                        html += '<a class="btn btn-danger radius" style="text-decoration:none" onClick="AuditBrand(\'' + obj[i]["billno"] + '\',\'1\')" href="javascript:;" title="下架">下架</a>';
                    }
                    html += '</td>'; html += '</tr>'
                }
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                $('#TbShows').empty();
                $("#TbShows").append(html);
                layer.close(index);
            }
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            }
            else if (type == '2') {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
                
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//品牌方案审核
function AuditBrand(billno, status)
{
    var index = layer.load(2);
    var proc = "Proc_Admin_Brand";//存储过程名
    var type = "ReturnNumber";
    var json = {
        type: "AuditBrandStatus",
        billno: billno,
        status: status
    };
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "brand/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                QueryBrandList();
            }
            else {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
        }
    })
}
//修改方案信息
function BrandDtOpen(title, url, id) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id
    });
    layer.full(index);
}
//加载品牌详情信息
function LoadBrandDt()
{
    var index = layer.load(2);
    var lsid = $("#lsid").html();
    var billno = GetQueryString("id");
    var json = {
        type: "BrandEditor_Query",
        lsid: lsid,
        billno: billno
    }
    var proc = "Proc_Admin_Brand";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "brand/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data[0];
                $("#txtName").val(obj["name"]);
                $("#txtBrandImgUrl").val(obj["img_url"]);
                $('#imgId').attr('src', obj["img_url"]);
                $("#divimg").show();

                $("#txtLinkUrl").val(obj["linkUrl"]);
                $("#txtBrandDtImgUrl").val(obj["img_url_dt"]);
                $("#txtNote").val(obj["beizhu"]);
                $("#txtSortId").val(obj["sort_id"]);
                $("#txtName").val(obj["name"]);
                if (obj["floor_id"] == 44) {
                    $("#radio-1").attr("checked","checked")
                } else {
                    $("#radio-2").attr("checked", "checked")
                }
                //document.getElementById("clickId").click();
                layer.close(index);
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
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
