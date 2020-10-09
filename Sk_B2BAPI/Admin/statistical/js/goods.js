///查询商品资料
function GoodsClick(obj) {
    var index = layer.load(2);
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
    var startDate = $("#dateMin").val();
    var endDate = $("#dateMax").val();
    var strWhere = $("#txtStrWhere").val();//搜索条件
    var data = {
        type: "GoodsClick",
        startDate: startDate,
        endDate: endDate,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Admin_Statistical";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += "<tr class='text-c'>";
                    html += "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                    html += "<td>" + obj[i]["goodscode"] + "</td>";
                    html += "<td>" + obj[i]["sub_title"] + "</td>";
                    html += "<td>" + obj[i]["drug_spec"] + "</td>";
                    html += "<td>" + obj[i]["package_unit"] + "</td>";
                    html += "<td>" + obj[i]["drug_factory"] + "</td>";
                    html += "<td>" + obj[i]["category"] + "</td>"; 
                    html += '<td class="td-status"><span class="label label-danger radius">' + obj[i]["num"] + '</span></td>';
                    html += "</tr>";
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
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}

///查询商品销量
function GoodsSale(obj) {
    var index = layer.load(2);
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
    var startDate = $("#dateMin").val();
    var endDate = $("#dateMax").val();
    var strWhere = $("#txtStrWhere").val();//搜索条件
    var data = {
        type: "GoodsSales",
        startDate: startDate,
        endDate: endDate,
        strWhere: strWhere,
        PageIndex: pageIndex,
        PageSize: pageSize,
    };
    var proc = "Proc_Admin_Statistical";//存储过程名
    var type = "ReturnList";
    ///加载页面数据
    $.ajax({
        type: "Post",
        cache: false,
        url: "goods/ashx/ReturnJson.ashx?type=" + type + "&json=" + encodeURIComponent(JSON.stringify(data)) + "&proc=" + proc,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var type = result.flag;
            if (type == '0') {
                var obj = result.data;
                var html = "";
                for (var i = 0; i < obj.length; i++) {
                    html += "<tr class='text-c'>";
                    html += "<td align='center'>" + ((parseInt(pageIndex) - 1) * parseInt(pageSize) + parseInt(i) + 1) + "</td>";
                    html += "<td>" + obj[i]["goodscode"] + "</td>";
                    html += "<td>" + obj[i]["sub_title"] + "</td>";
                    html += "<td>" + obj[i]["drug_spec"] + "</td>";
                    html += "<td>" + obj[i]["package_unit"] + "</td>";
                    html += "<td>" + obj[i]["drug_factory"] + "</td>";
                    html += '<td class="td-status"><span class="label label-danger radius">' + obj[i]["num"] + '</span></td>';
                    html += "</tr>";
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
            alert("出错了，请重试！" + "\\nreadyState:" + jqXHR.readyState + "\\nresponseText:" + jqXHR.responseText + "\\nstatus:" + jqXHR.status + "\\nstatusText:" + jqXHR.statusText + "\\ntextStatus:" + textStatus + "\\nerrorThrown:" + errorThrown);
            layer.close(index);
        }
    })
}