﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IntegralGoodsEntry.aspx.cs" Inherits="DTcms.Web.admin.IntegralGoods.IntegralGoodsEntry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>编辑内容</title>
    <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" />
    <link href="../skin/default/style.css" rel="stylesheet" />
    <link href="../Promotion/css/cxgz.css" rel="stylesheet" />
    <script src="../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script src="../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script src="../scripts/datepicker/WdatePicker.js"></script>
    <script src="../scripts/artdialog/dialog-plus-min.js"></script>
    <script src="../scripts/webuploader/webuploader.min.js"></script>
    <script src="../editor/kindeditor-min.js"></script>
    <script src="../js1/uploader.js"></script>
    <script src="../js1/laymain.js"></script>
    <script src="../js1/common.js"></script>
    <script src="../js1/layeralert/layer.js"></script>
    <%--<script src="../../aspx/Promotion/js/PromotionMain.js"></script>--%>
     <script src="../LuckyDraw/js/PromotionMain.js"></script>
    <script src="js/PostFile.js"></script>
   
    <script src="js/IntegralGoodsEnter.js"></script>

    <script>
        $(function () {
            //初始化验证
            $('#form1').initValidform();
        });
    </script>
  <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
<link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
</head>
<body class="mainbody">
    <form id="form1" runat="server" style="position: absolute; z-index: 1;">
       <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 商品管理 <span class="c-gray en">&gt;</span> 积分商品添加 <a class="btn btn-success radius r" style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i class="Hui-iconfont">&#xe68f;</i></a></nav>
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">商品新增</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="tab-content">
           
            <dl>
                <dt id="spmch">商品名称：</dt>
                <dd>
                    <input type="text" id="goodsName" value="" class="input normal" datatype="/^(?!_)(?!.*?_$)[a-zA-Z0-9_\u4e00-\u9fa5]+$/" errormsg="请填写正确的规则名称" sucmsg=" "  />
                    <span class="Validform_checktip">*填写商品名称</span>
                </dd>
            </dl>
            <dl>
                <dt>商品规格：</dt>
                <dd>
                    <input type="text" id="goodsSpecifications" value="" class="input normal" />
                    <span class="Validform_checktip">*填写商品规格</span>
                </dd>
            </dl>
            <dl>
                <dt>包装单位：</dt>
                <dd>
                    <input type="text" id="unit" value="" class="input normal" />
                    <span class="Validform_checktip">*选择包装规格</span>
                </dd>
            </dl>
            <dl>
                <dt>商品分类：</dt>
                <dd>
                    <%--<div class="rule-single-select single-select"></div>--%>
                    <select id="classification" style="height:32px;width:122px;border: 1px solid #eee;">
                        <option value="" selected="selected">请选择！</option>
                        <option value="热门兑换">热门兑换</option>
                        <option value="家用电器">家用电器</option>
                        <option value="移动电器">移动电器</option>
                        <option value="办公用品">办公用品</option>
                        <option value="赠品">赠品</option>
                        <option value="其它">其它</option>
                    </select>
                    <span class="Validform_checktip">*选择商品分类</span>
                </dd>
            </dl>
            <dl>
                <dt>展示楼层：</dt>
                <dd>
                    <select id="floor" style="height:32px;width:122px;border: 1px solid #eee;">
                        <option value="" selected="selected">请选择！</option>
                        <option value="热门兑换">热门兑换</option>
                        <option value="家用电器">家用电器</option>
                        <option value="移动电器">移动电器</option>
                        <option value="办公用品">办公用品</option>
                        <option value="赠品">赠品</option>
                        <option value="">其它</option>
                    </select>
                    <span class="Validform_checktip">*选择楼层分类</span>
                </dd>
            </dl>
            <dl>
                <dt>上传图片：</dt>
                <dd>
                    <input type="text" id="pictureText" class="input normal" value="" readonly="readonly" />
                    <input type="file" id="file" style="display:none;" onchange="PostFile(this)"/>
                    <input type="button" value="浏览..." onclick="fileBtn()" class="anniu"/>
                    <input type="hidden" value="" id="hiddenUrl" />
                </dd>
            </dl>
            <dl>
                <dt>生产厂家：</dt>
                <dd>
                    <input type="text" id="vendel" value="" class="input normal" />
                    <span class="Validform_checktip">*填写生产厂家</span>
                </dd>
            </dl>
            <dl>
                <dt>价格：</dt>
                <dd>
                    <input type="text" id="price" value="" class="input normal" datatype="/^\d{1,11}$|^\d{0,11}\.\d{1,3}$/" errormsg="价格长度最多11位，小数点后最多保留3位" sucmsg=" "/>
                    <span class="Validform_checktip">*设置价格</span>
                </dd>
            </dl>
            <dl>
                <dt>积分：</dt>
                <dd>
                    <input type="text" id="integral" value="" class="input normal" datatype="/^([1-9][0-9]*)$/" errormsg="积分必须为整数" sucmsg=" "/>
                    <span class="Validform_checktip">*填写积分</span>
                </dd>
            </dl>
            <dl>
                <dt>库存：</dt>
                <dd>
                    <input type="text" id="Quantity" value="" class="input normal" datatype="/^[0-9]{1,}$/" errormsg="库存必须为整数" sucmsg=" "/>
                    <span class="Validform_checktip">*填写库存</span>
                </dd>
            </dl>
        </div>
        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <input type="button" id="btnSubmit" value="提交" class="btn green" onclick="luru()" />
                <%--<input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />--%>
               <%-- <a href="IntegralGoodsUpdate.aspx?goodsid=ZP0000022">点击</a>--%>
            </div>
        </div>
        <!--/工具栏-->
    </form>
</body>
</html>

