//获取客户信息
function GetCustomerInfo(obj) {
    var index = layer.load(2);
    var status = $("#sltIsShowId").val();
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
        type: "GetCustomerList",
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Admin_CustomerList";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "members/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj=result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">'
                    html += '<td><input type="checkbox" name="customer" value="' + obj[i]["businessid"] + '"></td>'
                    html += '<td>' + obj[i]["businesscode"] + '</td>'
                    html += "<td><u style='cursor:pointer' class='text-primary' onclick=\"CustomerEnditOpen('" + obj[i]["businessname"] + "','customer_editor.html','" + obj[i]["businessid"] + "','" + obj[i]["entid"] + "')\">" + obj[i]["businessname"] + "</u></td>";
                    html += '<td>' + obj[i]["clienttype"] + '</td>'
                    html += '<td>' + obj[i]["shortname"] + '</td>'
                    html += '<td>' + obj[i]["wtr"] + '</td>'
                    html += '<td>' + obj[i]["wtsyxq"] + '</td>'
                    html += '<td>' + obj[i]["xkzyxq"] + '</td>'
                    html += '<td>' + obj[i]["yyzzyxq"] + '</td>'
                    html += '<td>' + obj[i]["gspzsyxq"] + '</td>'
                    html += '<td>' + obj[i]["add_time"] + '</td>'
                    var beactive = obj[i]["beactive"]
                    html += "<td>";
                    if (beactive == "Y") {
                        html += '<span class="label label-success radius">已启用</span>';
                    }
                    else {
                        html += '<span class="label label-danger radius">已禁用</span>';
                    }
                    html += "</td><td>";
                    if (beactive == "N") {
                        html += '<a style="text-decoration:none" onClick="AuditCustomer(\'' + obj[i]["businessid"] + '\',\'Y\',\'' + obj[i]["entid"] + '\')" href="javascript:;" title="启用"><i class="Hui-icolor-blue">启用</i></a>';
                        html += '<a style="text-decoration:none" class="ml-5" onClick="CustomerEnditOpen(\'' + obj[i]["businessname"] + '\',\'customer_editor.html\',\'' + obj[i]["businessid"] + '\',\'' + obj[i]["entid"] + '\')" href="javascript:;" title="编辑"><i class="Hui-icolor-yellow">编辑</i></a> ';
                    }
                    else {
                        html += '<a style="text-decoration:none" onClick="AuditCustomer(\'' + obj[i]["businessid"] + '\',\'N\',\'' + obj[i]["entid"] + '\')" href="javascript:;" title="禁用"><i class="Hui-icolor-red">禁用</i></a>';
                    }
                    html += '</td>'
                    html += '</tr>'
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
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
            }
            else {
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
//客户审核
function AuditCustomer(id, status,entId) {
    //下架
    var msg = "";
    if (status == "N") {
        msg = "确认要禁用吗？";
    }
    else if (status =="Y") {
        msg = "确认要启用吗？";
    }
    layer.confirm(msg, function (index) {
        var index = layer.load(2);
        var data = {
            entId:entId,
            businessId: id,
            status: status,
        };
        var proc = "";//存储过程名
        var type = "AuditCustomer";
        ///加载页面数据
        $.ajax({
            type: "Post",
            cache: false,
            url: "members/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var obj = data;
                var type = obj.flag;
                if (type == '0') {
                    layer.alert(obj.message, {
                        icon: 1,
                        skin: 'layer-ext-moon',
                        yes: function () { layer.closeAll(); GetCustomerInfo(); }
                    });
                }
                else {
                    layer.closeAll();
                    layer.alert(obj.message, {
                        icon: 2,
                        skin: 'layer-ext-moon'
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
                layer.closeAll();
            }
        })
    });
}
//修改客户信息
function CustomerEnditOpen(title, url, id,entId) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url + "?id=" + id + "&entId=" + entId
    });
    layer.full(index);
}
///客户编辑加载数据
function LoadCustomerInfo()
{
    var index = layer.load(2);
    var businessId = GetQueryString("id");
    var entId = GetQueryString("entId");
    var json = {
        type: "GetCustomerMt",
        businessId: businessId,
        KhEntId: entId
    };
    var proc = "Proc_Admin_CustomerList";//存储过程名
    var type = "ReturnList";
    $.ajax({
        type: "Post",
        cache: false,
        url: "members/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
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
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { layer_close(); }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer_close();
        }
    })
}
//选择企业/客户信息
function UserChoseInfo(type) {
    var strWhere = $("#txtEntName").val();
    layer_show("选择企业", "SearchInfo.html?type=entDoc&proc=Proc_Admin_SearchInfo&sqlType=GetEntDoc&strWhere=" + encodeURI(strWhere), 1000, 600);
}
//修改客户信息
function SaveCustomerInfo()
{
    var index = layer.load(2);
    var businesscode = $("#txtBusinessCode").val();
    var businessname = $("#txtBusinessName").val();
    var businessid = $("#txtBusinessId").val();
    var entid = $("#txtEntId").val();
    var clienttype = $("#txtClientType").val();
    var beactive = $("#sltStatus").val();
    var shortname = $("#txtShortName").val();
    var businesscont = $("#txtBusinesscont").val();
    var address = $("#txtAddress").val();
    var wtr = $("#txtWtr").val();
    var wtsyxq = $("#txtWtsyxq").val();
    var xkzyxq = $("#txtXkzyxq").val();
    var yyzzyxq = $("#txtYyzzyxq").val();
    var gspzsyxq = $("#txtGspzsyxq").val();
    var oldentid = $("#txtOldEntId").val();
    var json = {
        businesscode: businesscode,
        businessname: businessname,
        businessid: businessid,
        entid: entid,
        oldentid:oldentid,
        clienttype: clienttype,
        beactive: beactive,
        shortname: shortname,
        businesscont: businesscont,
        address: address,
        wtr: wtr,
        wtsyxq: wtsyxq,
        xkzyxq: xkzyxq,
        yyzzyxq: yyzzyxq,
        gspzsyxq: gspzsyxq
    }
    var proc = "";//存储过程名
    var type = "SaveCustomer";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "members/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(json)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                layer.alert(obj.message, {
                    icon: 1,
                    skin: 'layer-ext-moon',
                    yes: function () { layer.closeAll(); }
                });
                parent.location.reload();
            }
            else {
                layer.alert(obj.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes:function(){ layer.closeAll();}
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}
function GetCustomerInfo_CS() {

    var index = layer.load(2);
    var status = $("#sltIsShowId").val();
    var strWhere = $("#txtStrWhere").val();
    var pageSize = "15";
    var pageIndex = $("#pageIndex").html();
    var data = {
        type: "GetCustomerList",
        status: status,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Admin_CustomerList";//存储过程名
    var type = "ReturnList";
    var url = "members/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc;
    var vm = new Vue({
        el: '.page-container',
        data: { list: {},recordCount:0,pageCount:0},
        mounted: function () {
            this.post();
        },
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        success:function(){}.bind(this),
        methods: {
            post: function () {
                //发送get请求
                this.$http.post(url, data, { emulateJSON: true }).then(function (result) {
                    var obj = result.body;
                    var type = obj.flag;
                    if (type == '0') {
                        vm.list = {};
                        vm.list = obj.data;
                        vm.recordCount = obj.recordCount;
                        vm.pageCount = obj.pageCount;
                        layer.close(index);
                    }
                    else {
                        layer.close(index);
                        layer.alert(obj.message, {
                            icon: 2,
                            skin: 'layer-ext-moon'
                        });
                    }
                },function () {
                    layer.close(index);
                    layer.alert("请求失败", {
                        icon: 2,
                        skin: 'layer-ext-moon'
                    });
                });
            }
        }
    });
}