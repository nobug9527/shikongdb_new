//获取URL传递的参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return decodeURI(r[2]);
    return null;
}

/*关闭弹出框口*/
function layer_close() {
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}
//分页控件
//首页
function btnfirst(QueryWhere) {
    document.getElementById("pageIndex").innerHTML = 1;
    QueryWhere();
}
//尾页
function btnlast(QueryWhere) {
    var page = document.getElementById("pageCount").innerHTML;
    document.getElementById("pageIndex").innerHTML = page;
    QueryWhere();
}
//上翻页
function btnup(QueryWhere) {
    var page = document.getElementById("pageCount").innerHTML;
    var number = document.getElementById("pageIndex").innerHTML;
    if (parseInt(number) <= 1) {
        //layer.alert("当前页面为首页，无法继续翻页", {
        //    icon: 2,
        //    skin: 'layer-ext-moon'
        //});
        return;
    }
    else if (parseInt(number) > 1) {
        var newpage = parseInt(number) - 1
        document.getElementById("pageIndex").innerHTML = newpage;
        QueryWhere();
    }
}
//下翻页
function btnnext(QueryWhere) {

    var page = document.getElementById("pageCount").innerHTML;
    var number = document.getElementById("pageIndex").innerHTML;
    if (parseInt(number) >= parseInt(page)) {
        //layer.alert("单位页面为尾页，无法继续翻页", {
        //    icon: 2,
        //    skin: 'layer-ext-moon'
        //});
        return;
    }
    else if (parseInt(number) < parseInt(page)) {
        var newpage = parseInt(number) + 1
        document.getElementById("pageIndex").innerHTML = newpage;
        QueryWhere();
    }
}
//跳转页
function btntz(QueryWhere) {
    var page = document.getElementById("pageCount").innerHTML;
    var number = document.getElementById("pagety").value;
    if (parseInt(number) >= parseInt(page)) {
        //alertFun("单位页面为尾页，无法继续翻页", function () { }, 'f');
    } else if (parseInt(number)<1) {
        //alertFun("跳转页面不能小于0，无法继续跳页", function () { }, 'f');
    }
    else if (parseInt(number) < parseInt(page)) {
        document.getElementById("pagety").value = number;
        document.getElementById("pageIndex").innerHTML = number;
        QueryWhere();
    }
}
//搜索页

//上翻页
function Sbtnup(QueryWhere) {
    var page = document.getElementById("SpageCount").innerHTML;
    var number = document.getElementById("SpageIndex").innerHTML;
    if (parseInt(number) <= 1) {
        //layer.alert("当前页面为首页，无法继续翻页", {
        //    icon: 2,
        //    skin: 'layer-ext-moon'
        //});
        return;
    }
    else if (parseInt(number) > 1) {
        var newpage = parseInt(number) - 1
        document.getElementById("SpageIndex").innerHTML = newpage;
        QueryWhere();
    }
}
//下翻页
function Sbtnnext(QueryWhere) {

    var page = document.getElementById("SpageCount").innerHTML;
    var number = document.getElementById("SpageIndex").innerHTML;
    if (parseInt(number) >= parseInt(page)) {
        //layer.alert("单位页面为尾页，无法继续翻页", {
        //    icon: 2,
        //    skin: 'layer-ext-moon'
        //});
        return;
    }
    else if (parseInt(number) < parseInt(page)) {
        var newpage = parseInt(number) + 1
        document.getElementById("SpageIndex").innerHTML = newpage;
        QueryWhere();
    }
}
///设置默认日期
function initDate() {
    //设置结束日期为当前日期
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var end = date.getFullYear() + seperator1 + month + seperator1 + strDate;
    var endTime = end + " " + date.getHours() + seperator2 + date.getMinutes() +
          seperator2 + date.getSeconds();

    //设置结束日期
    $("#dateTimeMax").val(endTime);
    $("#dateMax").val(end);
    //设置开始日期
    $("#dateTimeMin").val(endTime);
    $("#dateMin").val(end);
}
//上传图片
function UpLoadPicture(type,id,name) {

    var index = layer.load(2);   //页面正在加载提示

    var fileName = $("#"+id+"").val();
    var files = $("#" + id + "").get(0).files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append("" + name + "", files[i]);
    }
    $.ajax({
        type: "post",
        url: "/Admin/ashx/UploadImg.ashx?type=UploadImage&imgType=" + type,
        data: fileData,
        cache: false,
        dataType: "json",
        contentType: false,
        processData: false,
        success: function (result) {
            layer.close(index);
            if (result.flag == "0") {
                $("#" + id + "").attr('src', result.message);
                $("#txt" + id + "").val(result.message);
               
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

//上传图片
function UpLoadAPK(type, id, name) {

    var index = layer.load(2);   //页面正在加载提示

    var fileName = $("#" + id + "").val();
    var files = $("#" + id + "").get(0).files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append("" + name + "", files[i]);
    }
    $.ajax({
        type: "post",
        url: "/Admin/ashx/UploadImg.ashx?type=UploadApk&apptype=" + type,
        data: fileData,
        cache: false,
        dataType: "json",
        contentType: false,
        processData: false,
        success: function (result) {
            layer.close(index);
            if (result.flag == "0") {
                $("#" + id + "").attr('src', result.message);
                $("#txt" + id + "").val(result.message);
                $("#txtDownloadAddress").val(result.message);
                layer.alert("版本更新成功！");
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