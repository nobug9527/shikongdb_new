
//加载图片类型
function LoadImgType(jibie,id)
{
    //$("#stlSource").append("<option value=''>请选择分类</option>");
    var index = layer.load(2);   //页面正在加载提示
    var source = $("#stlSource").val();
    if (source == "")
    {
        layer.close(index);
        layer.alert("请选择所属类别", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var imgType = "";
    if (jibie == "2")
    {
        imgType = $("#sltImgType").val();
        //if (imgType != "APPModule" & imgType != "SY") {
        //    layer.close(index);
        //    return;
        //}
    }
    var data = {
        type: "GetImgType",
        source: source,
        jibie: jibie,
        imgType: imgType
    };
    var proc = "Proc_Admin_Img";//存储过程名
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
                $("#" + id + " option").remove(); //删除Select的Option
                $("#" + id + "").append("<option value=''>请选择分类</option>");
                for (var i = 0; i < obj.length; i++) {
                    $("#" + id + "").append("<option value='" + obj[i]["ImgType"] + "'>" + obj[i]["TypeName"] + "</option>");
                }
                if (jibie == 1) {
                    $("#sltImgTypeS option").remove(); //删除Select的Option
                    $("#sltImgTypeS").append("<option value=''>暂无分类</option>");
                }
                layer.close(index);
            }
            else if (type == "1")
            {
                layer.close(index);
                $("#" + id + " option").remove(); //删除Select的Option
                $("#" + id + "").append("<option value=''>暂无分类</option>");
                if (jibie == 1) {
                    $("#sltImgTypeS option").remove(); //删除Select的Option
                    $("#sltImgTypeS").append("<option value=''>暂无分类</option>");
                }
            }
            else {
                $("#" + id + " option").remove(); //删除Select的Option
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
            
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            $("#" + id + " option").remove(); //删除Select的Option
            layer.alert("加载失败", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
        }
    })
}

//加载图片类型
function LoadUrlType() {
    var num = $("#sltUrlType").val();
    if (num != "2") {
        return;
    }
    var data = {
        type: "GetImgType",
        source: "APP",
        jibie: 2,
        imgType:'APPModule'
    };
    var proc = "Proc_Admin_Img";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        async: false,
        cache: false,
        url: "brand/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                $("#sltAndroidModel option").remove(); //删除Select的Option
                for (var i = 0; i < obj.length; i++) {
                    $("#sltAndroidModel").append("<option value='" + obj[i]["ImgType"] + "'>" + obj[i]["TypeName"] + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            $("#" + id + " option").remove(); //删除Select的Option
            layer.alert("加载失败", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
        }
    })
}

//上传图片
function UpLoadImg() {

    var index = layer.load(2);   //页面正在加载提示

    var fileName = $('#file').val();
    var files = $('#file').get(0).files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append("uploadfile", files[i]);
    }
    var imgType = $("#sltImgType").val();
    if (imgType == "")
    {
        layer.close(index);
        layer.msg("上传失败,请选择图片分类", { time: 3000 });
        $('#file').attr('src', "");
        $('#txtImgUrl').val("");
        return;
    }
    $.ajax({
        type: "post",
        url: "/Admin/ashx/UploadImg.ashx?type=UploadImage&imgType=" + imgType,
        data: fileData,
        cache: false,
        dataType: "json",
        contentType: false,
        processData: false,
        success: function (result) {
            layer.close(index);
            if (result.flag == "0") {
                $('#file').attr('src', result.message);
                $("#txtImgUrl").val(result.message);
                $('#imgId').attr('src', result.message);
                $("#divimg").show();
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
//存盘
function Save_Img() {
    var index = layer.load(2);
    var title = $("#txtTitle").val();
    var source = $("#stlSource").val();
    var ImgType = $("#sltImgType").val();
    var ImgTypeS = $("#sltImgTypeS").val();
    var imgUrl = $("#txtImgUrl").val();
    var linkUrl = $("#txtLinkUrl").val();
    var note = $("#txtNote").val();
    var sortId = $("#txtSortId").val();
    var UrlType = $("#sltUrlType").val();
    var AndroidLinkUrl = '';
    switch (UrlType) {
        case '1':
            AndroidLinkUrl=$("#txtAndroidLinkUrl").val();
            break;
        case '2':
            AndroidLinkUrl = $("#sltAndroidModel").val();
            break;
    }
    if (title == "" || imgUrl == "" || ImgType=="")
    {
        layer.msg("条件不满足，请检查", { time: 3000 });
        layer.close(index);
        return;
    }
    if (ImgType == "APPModule" && sltImgTypeS == "") {
        layer.msg("条件不满足，请检查", { time: 3000 });
        layer.close(index);
        return;
    }
    var json = {
        type: "Save_Img",
        title: title,
        source: source,
        linkUrl: linkUrl,
        imgUrl: imgUrl,
        ImgType: ImgType,
        ImgTypeS: ImgTypeS,
        beizhu: note,
        sort_id: sortId,
        urltype: UrlType,
        androidlinkurl: AndroidLinkUrl
    };
    var proc = "Proc_Admin_Img";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "main/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.alert("存盘成功", {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { window.location.reload(); }
                });
                parent.location.reload();
            }
            else {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}

function urlbianhua(urltype) {
    if (urltype == "1") {
        $("#lj").css("display", "inherit");
        $("#mk").css("display", "none");
    } else if (urltype == "2") {
        $("#mk").css("display", "inherit");
        $("#lj").css("display", "none");
        LoadUrlType();
    } else {
        $("#lj").css("display", "none");
        $("#mk").css("display", "none");
    }
}
//获取图片列表
function GetImgList(obj)
{
    var index = layer.load(2);
    var status = $("#sltIsShowId").val();
    var source = $("#stlSource").val();
    var imgType = $("#sltImgType").val();
    var strWhere = $("#txtStrWhere").val();
    var pageSize = "15";
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var data = {
        type: "GetImgList",
        status: status,
        source: source,
        imgType:imgType,
        title: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Admin_Img";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "main/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="" value="' + obj[i]["id"] + '"></td>'
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>'
                    html = html + '<td><u style="cursor:pointer" class="text-primary" onClick="ImgDtOpen(\'' + obj[i]["id"] + '\',\'picture_editor.html\',\'' + obj[i]["id"] + '\')">' + obj[i]["id"] + '</u></td>'
                    html += '<td>' + obj[i]["typeName"] + '</td>'
                    html += '<td>' + obj[i]["title"] + '</td>'
                    html += '<td>' + obj[i]["source"] + '</td>'
                    html += '<td>' + obj[i]["beizhu"] + '</td>'
                    html += '<td>' + obj[i]["ejTypeName"] + '</td>'
                    html += '<td>' + obj[i]["add_time"] + '</td>'
                    html += '<td>' + obj[i]["sort_id"] + '</td>'
                    var status = obj[i]["status"];
                    if (status == "1") {
                        html += '<td class="td-status"><span class="label label-danger radius">未上架</span></td>'
                    }
                    else if (status == "2") {
                        html += '<td class="td-status"> <span class="label label-success radius">已上架</span></td>'
                    }
                    html += '<td class="f-14 td-manage">';
                    if (status == "1") {
                        html += '<a style="text-decoration:none;margin-right: 3px;"  onClick="AuditImg(\'' + obj[i]["id"] + '\',\'2\')" href="javascript:;" class="btn btn-success radius" title="上架">上架</a>';
                        html += '<a style="text-decoration:none;margin-right: 3px;" class="ml-5 btn btn-warning radius" onClick="ImgDtOpen(\'' + obj[i]["id"] + '\',\'picture_editor.html\',\'' + obj[i]["id"] + '\')" href="javascript:;" title="编辑">编辑</a> ';
                        html += '<a style="text-decoration:none;margin-right: 3px;" class="ml-5 btn btn-danger radius" onClick="AuditImg(\'' + obj[i]["id"] + '\',\'0\')" href="javascript:;" title="删除">删除</a>'
                    }
                    else if (status == "2") {
                        html += '<a style="text-decoration:none" onClick="AuditImg(\'' + obj[i]["id"] + '\',\'1\')" href="javascript:;"  class="btn btn-danger  radius" title="下架">下架</a>';
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
//品牌方案审核
function AuditImg(id, status) {
    var index = layer.load(2);
    var proc = "Proc_Admin_Img";//存储过程名
    var type = "ReturnNumber";
    var json = {
        type: "AuditImg",
        id: id,
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
                GetImgList();
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
//方案编辑
function ImgDtOpen(title, url, id) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id
    });
    layer.full(index);
}
//加载图片信息
function LoadImgInfo()
{
    var index = layer.load(2);
    var billno = GetQueryString("id");
    var json = {
        type: "QueryImg",
        id: billno,
    }
    var proc = "Proc_Admin_Img";//存储过程名
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
                $("#txtTitle").val(obj["title"]);
                $("#stlSource").val(obj["source"]);
                $("#sltImgType").val(obj["ImgType"]);
                $("#sltImgTypeS").val(obj["imgTypeS"]);
                $("#stlSource").val(obj["source"]);
                $("#stlSource").val(obj["source"]);
                $("#txtImgUrl").val(obj["img_url"]);
                $("#imgId").attr('src', obj["img_url"]);
                $("#divimg").show();
                $("#txtLinkUrl").val(obj["link_url"]);
                $("#txtNote").val(obj["beizhu"]);
                $("#txtSortId").val(obj["Sort_id"]);
                var linktype = obj["androidlinktype"]
                $("#sltUrlType").val(linktype);
                //$("#sltUrlType option").removeAttr("selected");
                //$("#sltUrlType option[value='" + linktype + "']").attr("select", "selected"); 
                if (linktype == "1") {
                    $("#txtAndroidLinkUrl").val(obj["androidlinkurl"]);
                } else {
                    $("#sltImgTypeA").val(obj["androidlinkurl"]);
                }
                urlbianhua(linktype);
               
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
//修改图片信息
function UpdateImg()
{
    var index = layer.load(2);
    var billno = GetQueryString("id");
    var title = $("#txtTitle").val();
    var imgUrl = $("#txtImgUrl").val();
    var linkUrl = $("#txtLinkUrl").val();
    var note = $("#txtNote").val();
    var sortId = $("#txtSortId").val();
    var UrlType = $("#sltUrlType").val();
    if (title == "" || imgUrl == "" || isNaN(parseInt(sortId))) {
        layer.alert("数据不完整请补全数据！");
        layer.close(index);
        return;
    }
    //if (UrlType!=1 && (linkUrl.indexOf("http://") == -1 || linkUrl.indexOf("https://") == -1)) {
    //    layer.close(index);
    //    layer.alert("连接格式错误，请加前缀http://");
    //    return;
    //}
    var AndroidLinkUrl = '';
    switch (UrlType) {
        case '1':
            AndroidLinkUrl = $("#txtAndroidLinkUrl").val();
            break;
        case '2':
            AndroidLinkUrl = $("#sltAndroidModel").val();
            break;
    }
    var json = {
        type: "UpdateImg",
        imgUrl: imgUrl,
        title: title,
        linkUrl: linkUrl,
        beizhu: note,
        sort_id: sortId,
        id: billno,
        urltype: UrlType,
        androidlinkurl: AndroidLinkUrl
    };
    var proc = "Proc_Admin_Img";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "main/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.alert("存盘成功", {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { layer_close(); }
                });
                parent.location.reload();
            }
            else {
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
//批量删除图片
//批量删除
function DltImgList() {
    var index = layer.load(2);
    ////获取删除分类Id
    var list = "";
    $(".text-c input[type=checkbox]:checked").each(function () { //由于复选框一般选中的是多个,所以可以循环输出选中的值  
        list = $(this).val() + "," + list;
    });
    if (list == "") {
        layer.close(index);
        return;
    }
    var data = {
        type:"UpdateList",
        listId: list,
    };
    var proc = "Proc_Admin_Img";//存储过程名
    var type = "ReturnNumber";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "main/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                layer.close(index);
                layer.msg("删除成功", { time: 3000 });
                GetImgList();
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
            layer.close(index);
            layer.alert("加载失败", {
                icon: 2,
                skin: 'layer-ext-moon'
            })
        }
    });
}
