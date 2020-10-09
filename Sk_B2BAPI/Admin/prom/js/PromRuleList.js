
function QueryRuleList(obj) {
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
    var pageSize = '25';
    var status = $("#sltStatus").val();
    var faType = $("#sltShowType").val();
    var strWhere = $("#txtStrWhere").val();
    var data = {
        type: "QueryList",
        status: status,
        faType: faType,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    //var proc = "PC_Recharge"//存储过程名
    //var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "prom/ashx/PromRuleList.ashx?json=" + encodeURIComponent(JSON.stringify(data)),// + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">';
                    html += '<td>' + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + '</td>';
                    html += '<td>' + obj[i]["entid"] + '</td>';
                    html += '<td>' + obj[i]["ruleCode"] + '</td>';

                    html += '<td>' + obj[i]["ruleTitle"] + '</td>';
                    html += '<td>' + obj[i]["faName"] + '<input type="hidden" id="faType' + obj[i]["ruleCode"] + '" value="' + obj[i]["faType"] + '"></td>';
                    html += '<td>' + obj[i]["describe"] + '</td>';
                    if (obj[i]["isShow"] == "2") {
                        html += '<td><lable class="label label-primary radius">已上架</lable></td>';
                        html += '<td><a style="text-decoration:none" onClick="UpdateRlueDown(\'' + obj[i]["ruleCode"] + '\',\'' + obj[i]["entid"] + '\',this)" href="javascript:;"  class="btn btn-danger radius" title="下架">下架</a></td>';
                    } else if (obj[i]["isShow"] == "0") {
                        html += '<td><lable class="label label-danger radius">已删除</lable></td>';
                        html += '<td></td>';
                    }
                    else {
                        html += '<td><lable class="label label-success radius">未上架</lable></td>';
                        html += '<td><a style="text-decoration:none" onClick="UpdateRlueShow(\'' + obj[i]["ruleCode"] + '\',\'' + obj[i]["entid"] + '\',this)" href="javascript:;"  class="btn btn-primary radius" title="上架">上架</a>&nbsp;&nbsp;&nbsp;<a style="text-decoration:none" onClick="UpdateRlue(\'' + obj[i]["ruleCode"] + '\',\'' + obj[i]["entid"] + '\',this)" href="javascript:;"  class="btn btn-success radius" title="编辑">编辑</a>&nbsp;&nbsp;&nbsp;<a style="text-decoration:none" onClick="DeleteRlue(\'' + obj[i]["ruleCode"] + '\',\'' + obj[i]["entid"] + '\',this)" href="javascript:;"  class="btn btn-danger radius" title="删除">删除</a></td>';
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
            }
            else if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            } else if (type == '2') {
                layer.close(index);
                var msg = result.message;
                layer.alert(msg, {
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

function UpdateRlueDown(ruleCode, entid, doc) {
    var index = layer.load(2);
    var data = {
        type: "UpdateRlueDown",
        ruleCode: ruleCode,
        entid: entid
    };

    $.ajax({
        type: "Post",
        cache: false,
        url: "prom/ashx/PromRuleList.ashx?json=" + encodeURIComponent(JSON.stringify(data)),// + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                $(doc).parent().prev().html('<lable class="label label-success radius">未上架</lable>');
                $(doc).parent().html('<a style="text-decoration:none" onClick="UpdateRlueShow(\'' + ruleCode + '\',\'' + entid + '\',this)" href="javascript:;"  class="btn btn-primary radius" title="上架">上架</a>&nbsp;&nbsp;&nbsp;<a style="text-decoration:none" onClick="UpdateRlue(\'' + ruleCode + '\',\'' + entid + '\',this)" href="javascript:;"  class="btn btn-success radius" title="编辑">编辑</a>&nbsp;&nbsp;&nbsp;<a style="text-decoration:none" onClick="DeleteRlue(\'' + ruleCode + '\',\'' + entid + '\',this)" href="javascript:;"  class="btn btn-danger radius" title="删除">删除</a>');
                layer.close(index);
            }
            else if (type == '1') {
                layer.close(index);
            } else if (type == '2') {
                layer.close(index);
                var msg = result.message;
                layer.alert(msg, {
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
function UpdateRlueShow(ruleCode, entid, doc) {

    var index = layer.load(2);
    var data = {
        type: "UpdateRlueShow",
        ruleCode: ruleCode,
        entid: entid
    };

    $.ajax({
        type: "Post",
        cache: false,
        url: "prom/ashx/PromRuleList.ashx?json=" + encodeURIComponent(JSON.stringify(data)),// + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                $(doc).parent().prev().html('<lable class="label label-primary radius">已上架</lable>');
                $(doc).parent().html('<a style="text-decoration:none" onClick="UpdateRlueDown(\'' + ruleCode + '\',\'' + entid + '\',this)" href="javascript:;"  class="btn btn-danger radius" title="下架">下架</a>');
                layer.close(index);
            }
            else if (type == '1') {
                layer.close(index);
            } else if (type == '2') {
                layer.close(index);
                var msg = result.message;
                layer.alert(msg, {
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

function DeleteRlue(ruleCode, entid, doc) {
    var index = layer.load(2);
    var data = {
        type: "DeleteRlue",
        ruleCode: ruleCode,
        entid: entid
    };

    $.ajax({
        type: "Post",
        cache: false,
        url: "prom/ashx/PromRuleList.ashx?json=" + encodeURIComponent(JSON.stringify(data)),// + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                $(doc).parent().prev().html('<lable class="label label-danger radius">已删除</lable>');
                $(doc).parent().html('');
                layer.close(index);
            }
            else if (type == '1') {
                layer.close(index);
            } else if (type == '2') {
                layer.close(index);
                var msg = result.message;
                layer.alert(msg, {
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

function UpdateRlue(ruleCode, entid) {
    var cx = "DHG,DMZ,DZK,DMJ";
    var zh = "DQG,GZH,"
    var faType = $("#faType" + ruleCode).val();
    var url = zh.indexOf(faType) != -1 ? "promGroupRule.html" : "promRule.html";
    var index = layer.open({
        type: 2,
        title: "修改规则",
        content: url + "?entid=" + entid + "&ruleCode=" + ruleCode + "&fatype=" + faType
    });
    layer.full(index);
}