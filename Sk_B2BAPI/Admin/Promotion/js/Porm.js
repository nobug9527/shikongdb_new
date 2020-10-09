////获取促销方案
function GetPromPlan()
{
    var index=layer.load(2);
    var startDate = $("#datemin").val();
    var endDate = $("#datemax").val();
    var strWhere = $("#txtStrWhere").val();
    var faType = $("#sltShowType").val();
    var status = $("#sltStatus").val();
    if (startDate == "" || endDate == "")
    {
        layer.close(index);

        layer.alert("请选择日期", {
            icon: 2,
            skin: 'layer-ext-moon'
        });
        return;
    }
    var pageSize = "7";
    var pageIndex = $("#pageIndex").html();
    var data = {
        type: "GetPromPlanDt",
        faType: faType,
        status:status,
        startDate: startDate,
        endDate: endDate,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Admin_PromQuery";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "Promotion/ashx/returnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var obj = data;
            var type = obj.flag;
            if (type == '0') {
                $('#TbShows').empty();
                var tb = "";
                for (var i = 0; i < obj["data"].length; i++) {
                    tb = tb + "<tr class='text-c'>"
                                + "<td><input type='checkbox' value='" + obj["data"][i]["fabh"] + "' name='PromList'></td>"
                                + "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>"
                                + "<td><u style='cursor:pointer' class='text-primary' onclick=\"member_show('方案设计','prom_single_design.html','10001','360','400')\">" + obj["data"][i]["fabh"] + "</u></td>"
                                + "<td>" + obj["data"][i]["faTitle"] + "</td>"
                                + "<td>" + obj["data"][i]["fabs"] + "</td>"
                                + "<td>" + obj["data"][i]["faname"] + "</td>"
                                + "<td>" + obj["data"][i]["startDate"] + "</td>"
                                + "<td>" + obj["data"][i]["endDate"] + "</td>"
                                + "<td>" + obj["data"][i]["describe"] + "</td>"
                                + "<td class='td-status'><span class='label label-success radius'>" + obj["data"][i]["status"] + "</span></td>"
                                + "<td class='td-manage'>"
                                + "<a style=\"text-decoration:none\" onClick=\"member_stop(this,'10001')\"  title=\"停用\"><i class=\"Hui-iconfont\">&#xe631;</i></a>"
                                + "<a title=\"编辑\"  onclick=\"member_edit('编辑','member-add.html','4','','510')\" class=\"ml-5\" style=\"text-decoration:none\"><i class=\"Hui-iconfont\">&#xe6df;</i></a>"
                                + "<a title=\"删除\" href=\"javascript:;\" onclick=\"member_del(this,'1')\" class=\"ml-5\" style=\"text-decoration:none\"><i class=\"Hui-iconfont\">&#xe6e2;</i></a>"
                                +"</td>"
                                + "</tr>";
                }
                $("#TbShows").append(tb);
                var recordCount = obj["recordCount"];
                var pageCount = obj["pageCount"];
                document.getElementById("recordCount").innerHTML = recordCount;
                document.getElementById("pageCount").innerHTML = pageCount;
                layer.close(index);
            }
            if (type == '1') {
                $('#TbShows').empty();
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                layer.close(index);
            }
            if (type == '2') {
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
///////促销方案维护



