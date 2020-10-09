//上传图片
function UpLoadImg(id) {

    var index = layer.load(2);   //页面正在加载提示

    var fileName = $("#file_"+id+"").val();
    var files = $("#file_" + id + "").get(0).files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        fileData.append("uploadfile", files[i]);
    }
    $.ajax({
        type: "post",
        url: "/Admin/ashx/UploadImg.ashx?type=UploadImage&imgType=main",
        data: fileData,
        cache: false,
        dataType: "json",
        contentType: false,
        processData: false,
        success: function (result) {
            layer.close(index);
            if (result.flag == "0") {
                $("#file_" + id + "").attr('src', result.message);
                $("#txtImg_" + id + "").val(result.message);
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
///提交网站设置
function SaveBase() {
    var index = layer.load(2);   //页面正在加载提示

    var title = $("#txtTitle").val();//网站名称
    var ip = $("#txtIp").val();//网站Ip
    var company = $("#txtCompany").val();//公司名称
    var complaints = $("#txtComplaints").val();
    var xxjyz = $("#txtXxjyz").val();//互联网药品交易服务资格证
    var xxfwz = $("#txtXxfwz").val();//互联网药品信息服务资格证
    var ICP = $("#txtICP").val();//ICP备案证书号
    var beizhu = $("#txtBeizhu").val();
    //图片路径
    var logo_url = $("#txtImg_Logo").val();//logo路径
    var app_url = $("#txtImg_App").val();//app下载图片路径
    var left_url = $("#txtImg_Left").val();//底部左侧图
    var right_url = $("#txtImg_Right").val();//底部右侧图
    var kf_url = $("#txtImg_Kf").val();//客服图
    var kf_link = $("#txtLink_kf").val();//客服图连接

    var json = {
        title: title,
        ip: ip,
        company: company,
        complaints: complaints,
        xxjyz: xxjyz,
        xxfwz: xxfwz,
        ICP: ICP,
        beizhu: beizhu,
        logo_url: logo_url,
        app_url: app_url,
        left_url: left_url,
        right_url: right_url,
        kf_url: kf_url,
        kf_link: kf_link
    };
    var proc = "";//存储过程名
    var type = "SaveBase";
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
                layer.close(index);
                layer.alert(result.message, {
                    icon: 1,
                    skin: 'layer-ext-moon'
                });
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
            layer.msg("未知错误", { time: 300 });
        }
    })
}
//加载网站设置
function LoadWebBase()
{
    var index = layer.load(2);   //页面正在加载提示
    var proc = "";//存储过程名
    var type = "GetWebSiteBase";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "main/ashx/ReturnJson.ashx?type=" + type + "&json=&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data[0]
                new Vue({
                    el: '#list',
                    data: obj
                });
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
            layer.close(index);
            layer.msg("未知错误", { time: 3000 });
        }
    })

}