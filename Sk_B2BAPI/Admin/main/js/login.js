///后台管理员登陆
function Login()
{
    var username = $("#txtUserName").val();
    var password = $("#txtPassWord").val();
    var code = $("#code").val();
    if (username == "") {
        layer.tips('请输入账号', '#txtUserName', { tips: 1 });
        return;
    }
    if (password == "") {
        layer.tips('请输入密码', '#txtPassWord', { tips: 1 });
        return;
    }
    if (code == "") {
        layer.tips('请输入验证码', '#code', { tips: 1 });
        return;
    }
    var index = layer.load(2);
    var json = {
        type: "AdminLogin",
        username: username,
        password: password,
        code: code
    }
   
    var proc = "Proc_Admin_MembersQuery";//存储过程名
    var type = "login";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "main/ashx/UserJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                window.location = "index.html"
            }
            else {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon'
                });
                $('#codepic').click();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
///退出登陆

function Logout()
{
    var index = layer.load(2);
    var json = "";
    var proc = "";//存储过程名
    var type = "loginOut";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "main/ashx/UserJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                window.location = "login.html"
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
///后台管理员登陆
function ordermonry() {
    layer_show("订单限制", "AddConfig.html", 450, 200);
}

//function AppUplod() {
//    layer_show("APP版本更新", "AddApp.html", 750, 550);
//}

//function Guanggao() {
//    layer_show("广告位设置", "IndexAdvert.html", 750, 550);
//}
///修改密码
function OpenPassWord() {
    layer_show("密码修改", "UpdatePwd.html", 450, 200);
}