<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrizeUpdate.aspx.cs" Inherits="DTcms.Web.admin.LuckyDraw.PrizeUpdate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>添加奖品</title>
    <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" />
    <link href="../skin/default/style.css" rel="stylesheet" />
    <link href="../promotion/css/cxgz.css" rel="stylesheet" />
    <script src="../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script src="../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script src="../js1/uploader.js"></script>
    <script src="../js1/laymain.js"></script>
    <script type="text/javascript" src="../../js/config.js"></script>
    <script src="../js1/common.js"></script>
   <%-- <script src="../ImgUrl/js/layeralert/layer.js"></script>--%>
    <script src="../js1/layeralert/layer.js"></script>
    <script src="js/PromotionMain.js"></script>
    <script src="js/PostFile.js"></script>
    <script src="js/PrizeUpdate.js" type="text/javascript"></script>
    <script>
        function GetParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            //匹配目标参数  
            var r = window.location.search.substr(1).match(reg);
            //返回参数值  
            if (r != null) {
                return unescape(r[2]);
            }
            return "";
        }
        $(function () {
            //初始化验证
            $('#form1').initValidform();
            $.ajax({
                type: 'Get',
                cache: false,
                url: P_Json.Ajax_Url + "/Prize/PrizeType",
                dataType: "json",
                success: function (data) {
                    if (data.Code === 0) {
                        var source = data.Source;
                        var html = "<option value=\"\" selected=\"selected\">请选择！</option>";
                        for (var i = 0; i < source.length; i++) {
                            html += "<option value=\"" + source[i] + "\">" + source[i] + "</option>";
                        }
                        $("#classification").html(html);
                    }
                    Bind();
                }
            });
        });

        function Bind() {
            var bh = GetParam("bh");
            if (bh === "") {
                return;
            }
            $("#hidBH").val(bh);
            $("#PrizeName").val(decodeURI(GetParam("PrizeName")));
            $("#classification").val(decodeURI(GetParam("classification")));
            $("#pictureText").val(decodeURI(GetParam("pictureText")));
            $("#hiddenUrl").val(decodeURI(GetParam("pictureText")));
            $("#PrizeValue").val(decodeURI(GetParam("PrizeValue")));
        }
    </script>
    <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
<link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
</head>
<body class="mainbody">
     <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 抽奖管理 <span class="c-gray en">&gt;</span> 添加奖品 <a class="btn btn-success radius r" style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i class="Hui-iconfont">&#xe68f;</i></a></nav>
    <form id="form1" runat="server" style="position: absolute; z-index: 1;">
        <!--导航栏-->
       
       
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">奖品设置</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="tab-content">
            <dl>
                <dt>奖品名称(5个字)：</dt>
                <dd>
                    <input id="hidBH" value="" type="hidden" />
                    <input type="text" id="PrizeName" value="" class="input normal" datatype="/^(?!_)(?!.*?_$)[a-zA-Z0-9_\u4e00-\u9fa5]+$/" errormsg="请填写正确的规则名称" sucmsg=" " />
                    <span class="Validform_checktip">*填写奖品名称</span>
                </dd>
            </dl>
            <dl>
                <dt>商品分类：</dt>
                <dd>
                    <%--<div class="rule-single-select single-select"></div>--%>
                    <select id="classification" style="height: 32px; width: 122px; border: 1px solid #eee;">
                        <%--<option value="" selected="selected">请选择！</option>
                        <option value="热门兑换">热门兑换</option>
                        <option value="家用电器">家用电器</option>
                        <option value="移动电器">移动电器</option>
                        <option value="办公用品">办公用品</option>
                        <option value="赠品">赠品</option>
                        <option value="其它">其它</option>--%>
                    </select>
                    <span class="Validform_checktip">*选择奖品分类</span>
                </dd>
            </dl>

            <dl>
                <dt>上传图片：</dt>
                <dd>
                    <input type="text" id="pictureText" class="input normal" value="" readonly="readonly" />
                    <input type="file" id="file" style="display: none;" onchange="PostFile(this)" />
                    <input type="button" value="浏览..." onclick="fileBtn()" class="anniu" />
                    <input type="hidden" value="" id="hiddenUrl" />
                </dd>
            </dl>
            <dl>
                <dt>价值数量：</dt>
                <dd>
                    <input type="text" id="PrizeValue" value="" class="input normal" />
                    <span class="Validform_checktip">*填写奖品的价值</span>
                </dd>
            </dl>
        </div>
        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <input type="button" id="btnSubmit" value="提交" class="btn green" onclick="luru()" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
                <%-- <a href="IntegralGoodsUpdate.aspx?goodsid=ZP0000022">点击</a>--%>
            </div>
        </div>
        <!--/工具栏-->
    </form>
</body>
</html>

