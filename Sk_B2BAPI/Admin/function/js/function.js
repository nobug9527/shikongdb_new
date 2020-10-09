//查询管理员操作日志
function QueryAdminLog(obj)
{
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var index = layer.load(2);
    var pageSize = "15";
    //var pageIndex = $("#pageIndex").html();
    var dateMin = $("#dateMin").val();//开始日期
    var dateMax = $("#dateMax").val();//结束日期
    var strWhere = $("#txtStrWhere").val();//搜索条件
    var data = {
        type:"Pc_GetAdminLogList",
        mindate: dateMin,
        maxdate: dateMax,
        username: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Pc_Log"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "function/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj=result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">';
                    html += '<td>' + obj[i]["Id"] + '</td>';
                    html += '<td>' + obj[i]["AddTime"] + '</td>';
                    html += '<td color="orange">' + obj[i]["UserName"] + '</td>';
                    html += '<td>' + obj[i]["LogMessage"] + '</td>';
                    html += '</tr>';
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                layer.close(index);
            }
            if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            }
            if (type == '2') {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });
                
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            layer.alert("加载失败！", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
        }
    })
}

//查询管理员操作日志
function QueryUserIPLog(obj) {
    var pageIndex = $("#pageIndex").html();
    switch (obj) {
        case 1:
            pageIndex = '1';
            $("#pageIndex").html(1)
            break;
        default:
            break;
    }
    var index = layer.load(2);
    var pageSize = "15";
    //var pageIndex = $("#pageIndex").html();
    var dateMin = $("#dateMin").val();//开始日期
    var dateMax = $("#dateMax").val();//结束日期
    var strWhere = $("#txtStrWhere").val();//搜索条件
    var data = {
        type: "Pc_GetUserIPList",
        mindate: dateMin,
        maxdate: dateMax,
        username: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Pc_Log"//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "function/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += '<tr class="text-c">';
                    html += '<td>' + obj[i]["EntID"] + '</td>';
                    html += '<td>' + obj[i]["username"] + '</td>';
                    html += '<td>' + obj[i]["Host"] + '</td>';
                    html += '<td>' + obj[i]["Addr"] + '</td>';
                    html += '<td>' + obj[i]["Port"] + '</td>';
                    html += '<td>' + obj[i]["RecordDate"] + '</td>';
                    html += '</tr>';
                }
                $('#TbShows').empty();
                $("#TbShows").append(html);
                var recordCount = result["recordCount"];
                var pageCount = result["pageCount"];
                $("#recordCount").html(recordCount);
                $("#pageCount").html(pageCount);
                layer.close(index);
            }
            if (type == '1') {
                $("#recordCount").html(0);
                $("#pageCount").html(1);
                $('#TbShows').empty();
                layer.close(index);
            }
            if (type == '2') {
                layer.close(index);
                layer.alert(result.message, {
                    icon: 2,
                    skin: 'layer-ext-moon',
                    yes: function () { parent.location.replace("login.html") }
                });

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            layer.close(index);
            layer.alert("加载失败！", {
                icon: 2,
                skin: 'layer-ext-moon'
            });
        }
    })
}