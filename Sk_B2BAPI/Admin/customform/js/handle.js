// 增加字段选项
function AddField() {
    $(".field_panel").append(field_html);
}
// 取消字段选项
function CancelField(t) {
    $(t).parents(".field_item").remove();
}
var field_html = '<div class="row field_item">'
    + '<div class="formControls col-sm-12">'
    + '<input type="text" class="input-text field_def" maxlength="20" autocomplete="off" placeholder="查询名" style="width:150px;margin-right:10px;">'
    + '<input type="text" class="input-text field_name" maxlength="20" autocomplete="off" placeholder="参数名" style="width:150px;margin-right:10px;">'
    + '<input type="text" class="input-text field_value" maxlength="20" autocomplete="off" placeholder="默认值" style="width:165px;">'
    + '<label style="width:100px;padding-left:10px;">是否展示：</label>'
    + '<span class="select-box" style="width:50px;margin-right:10px;">'
    + '<select class="select is_show" size="1">'
    + '<option value="true" selected>是</option>'
    + '<option value="false">否</option>'
    + '</select>'
    + '</span>'
    + '<label style="width:100px;padding-left:10px;">参数类型：</label>'
    + '<span class="select-box" style="width:100px;margin-right:10px;">'
    + '<select class="select field_type" size="1">'
    + '<option value="text" selected>文本</option>'
    + '<option value="int">下拉</option>'
    + '<option value="date">日期</option>'
    + '</select>'
    + '</span>'
    + '<input class="btn btn-danger radius" type="button" value="取消" onclick="CancelField(this)">'
    + '</div>'
    + '</div>';
// 字段类型选择控制
function chooseInput(type) {
    if (type == "date") {
        var today = new Date();
        var y = today.getFullYear();
        var M = (today.getMonth() + 1) < 10 ? ('0' + (today.getMonth() + 1)) : (today.getMonth() + 1);
        var d = today.getDate() < 10 ? '0' + today.getDate() : today.getDate();
        var h = today.getHours() < 10 ? '0' + today.getHours() : today.getHours();
        var m = today.getMinutes() < 10 ? '0' + today.getMinutes() : today.getMinutes();
        var s = today.getSeconds() < 10 ? '0' + today.getSeconds() : today.getSeconds();
        var submitTime = y + '-' + M + '-' + d + ' ' + h + ":" + m + ":" + s;
        return '<input type="text" value="' + submitTime + '" class="input-text field_value Wdate" onfocus="WdatePicker({dateFmt:\'yyyy-MM-dd HH:mm:ss\',readOnly:true,isShowClear:false})" maxlength="20" autocomplete="off" style="width:165px;">';
    }
    else if (type == "int") {
        return '<input type="text" placeholder="点击编辑下拉选项" ut="' + new Date().getTime() + '" onfocus="open_select(this);" class="input-text field_value" maxlength="20" autocomplete="off" style="width:165px;">';
    }
    else {
        return '<input type="text" class="input-text field_value" maxlength="20" autocomplete="off" placeholder="默认值" style="width:165px;">';
    }
}
var tempSelectData = null;
// 弹出子下拉选项页面
function open_select(t,sd) {
    var url = 'customform_es.html?ut=' + $(t).attr("ut");
    if (sd != null) {
        tempSelectData = sd;
    }
    else if (sd == null) {
        var str = "[";
        var len = $(t).siblings("select").children("option").length;
        $(t).siblings("select").children("option").each(function (i) {
            str += "{"
                + "\"text\":\"" + $(this).text() + "\","
                + "\"value\":\"" + $(this).val() + "\""
                + "}";
            if (i < len - 1) {
                str += ",";
            }
        });
        str += "]";
        sd = JSON.parse(str);
        tempSelectData = sd;
    }
    window.ut_selectdata = tempSelectData;
    var index = layer.open({
        title: '编辑下拉选项',
        type: 2,
        shade: 0.2,
        maxmin: true,
        shadeClose: true,
        area: ['450px', '450px'],
        content: url
    });
}
// ut为时间戳，sd是子页面要传回来的下拉选项json数据
function set_select(ut, sd) {
    var selectstr = '<span class="select-box field_value" style="width:195px;display:inline-block" ut="' + ut + '">'
        + '<select class="select" size="1" style="width:150px;margin-right:2px">';
    for (var i = 0; i < sd.length; i++) {
        selectstr += '<option value="' + sd[i].value + '"' + (i == 0 ? ' selected' : '') + '>' + sd[i].text + '</option>';
    }
    selectstr += '</select>' + '<a href="javascript:;" style="color:blue;" onclick=\'open_select(this,' + JSON.stringify(sd) + ');\' ut="' + ut + '">修改</a>' + '</span>';
    $("#fieldrows").find("input[ut='" + ut + "'],span[ut='" + ut + "']").replaceWith(selectstr);
}
// 关闭子iframe
function closeFrame() {
    var iframeIndex = parent.layer.getFrameIndex(window.name);
    parent.layer.close(iframeIndex);
    return false;
}
// 获取url参数
function getUrlParam(par) {
    var reg = new RegExp("(^|&)" + par + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return decodeURI(r[2]);
    return null;
}