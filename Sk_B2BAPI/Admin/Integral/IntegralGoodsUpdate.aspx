﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IntegralGoodsUpdate.aspx.cs" Inherits="DTcms.Web.admin.IntegralGoods.IntegralGoodsUpdate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>编辑内容</title>
    <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" />
    <link href="../skin/default/style.css" rel="stylesheet" />
    <link href="../promotion/css/cxgz.css" rel="stylesheet" />
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
    <script src="../Promotion/js/PromotionMain.js"></script>
    <script src="js/PostFile.js"></script>
    <script src="js/IntegralGoodsUpdate.js"></script>
    <script>
        $(function () {
            //初始化验证
            $('#form1').initValidform();
            evaluate();
        });
    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server" style="position: absolute; z-index: 1;">
        <!--导航栏-->
        <p id="TEST"></p>
        <div class="location">
            <a href="#" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="#"><span>积分商城</span></a>
            <i class="arrow"></i>
            <span>积分商品</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">积分商品修改</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="tab-content">
            <dl>
                <dt>商品编号：</dt>
                <dd>
                    <input type="text" id="goodsId" value="" class="input normal" readonly="readonly"/>
                    <span class="Validform_checktip">*商品编号不能修改</span>
                </dd>
            </dl>
            <dl>
                <dt>商品名称：</dt>
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
                    <select id="classification" style="height:32px;width:122px;border: 1px solid #eee;">
                        <option value="" selected="selected">请选择！</option>
                        <option value="热门兑换">热门兑换</option>
                        <option value="家用电器">家用电器</option>
                        <option value="移动电器">移动电器</option>
                        <option value="办公用品">办公用品</option>
                        <option value="积分兑换">积分兑换</option>
                        <option value="积分抽奖">积分抽奖</option>
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
                        <option value="积分兑换">其它</option>
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
                    <input type="text" id="price" value="" class="input normal" datatype="/^\d{1,11}$|^\d{0,11}\.\d{1,3}$/" errormsg="数值长度最多11位，小数点后最多保留3位" sucmsg=" "/>
                    <span class="Validform_checktip">*设置价格</span>
                </dd>
            </dl>
            <dl>
                <dt>积分：</dt>
                <dd>
                    <input type="text" id="integral" value="" class="input normal" datatype="/^[0-9]{1,}$/" errormsg="积分必须为整数" sucmsg=" "/>
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
                <input type="button" id="btnSubmit" value="提交" class="btn" onclick="updateGoods()" />
                <%--<input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />--%>
            </div>
        </div>
        <!--/工具栏-->
    </form>
</body>
</html>
