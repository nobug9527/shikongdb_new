///加载商品选项
function LoadCheckBox(checkType)
{
    var index = layer.load(2);
    var data = {
        type: "GoodsAttributeBox",
    };
    var proc = "Proc_Admin_GoodsList";//存储过程名
    var type = "LoadCheckBox";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc + "&CheckType=" + checkType,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                //$('#TbShows').empty();
                ///加载商品分类
                if (obj.categoryList != "" && $("#sltCategory").length>0)
                {
                    $("#sltCategory").append(obj.categoryList);
                }
                ///加载商品属性
                if (obj.attributeList != "" && $("#sltAttribute").length>0)
                {
                    $("#sltAttribute").append(obj.attributeList);
                }
                if (obj.attributeList != "" && $("#isRecommend").length > 0)
                {
                    $("#isRecommend").append(obj.attributeList);
                }
                layer.close(index);
            }
            else{
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
//上传单个图片
var image = '';
function selectImage(file) {
    if (!file.files || !file.files[0]) {
        return;
    }
    var reader = new FileReader();
    reader.onload = function (evt) {
        document.getElementById('image').src = evt.target.result;
        image = evt.target.result;
    }
    reader.readAsDataURL(file.files[0]);
}
function uploadImage(obj) {
    var formData = new FormData();
    formData.append('photo', $('#input_file')[0].files[0]);
    //ajax请求
    $.ajax({
        type: "post",
        url: "goods/ashx/returnJson.ashx?type=&json=&proc=",
        data: formdata,
        dataType: 'json',
        processData: false, // 告诉jQuery不要去处理发送的数据
        contentType: false, // 告诉jQuery不要去设置Content-Type请求头
        xhrFields: { withCredentials: true },
        async: true,    //默认是true：异步，false：同步。
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') { }
            else
            {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (data) {
            layer.msg('请求异常');
        },
    });
}

function UpLoadImage(obj)
{
    var index = layer.load(2);
    if (!obj.files || !obj.files[0]) {
        return;
    }
    //ajax请求
    $.ajax({
        type: "post",
        url: "goods/ashx/returnJson.ashx?type=UploadImage&json=&proc=",
        data: obj.files[0],
        dataType: 'json',
        processData: false, // 告诉jQuery不要去处理发送的数据
        contentType: false, // 告诉jQuery不要去设置Content-Type请求头
        xhrFields: { withCredentials: true },
        async: true,    //默认是true：异步，false：同步。
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') { layer.close(index); }
            else
            {
                layer.close(index);
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
            }
        },
        error: function (data) {
            layer.close(index);
            layer.msg('请求异常');
        },
    });
}