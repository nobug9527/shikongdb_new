<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LuckyDraw.aspx.cs" Inherits="DTcms.Web.admin.LuckyDraw.LuckyDraw" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>抽奖编辑</title>
    <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" />
    <link href="../skin/default/style.css" rel="stylesheet" />
    <link href="../promotion/css/cxgz.css" rel="stylesheet" />
    <script src="../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script src="../js1/laymain.js"></script>
     <script src="../js1/common.js"></script>
    <script src="../js1/layeralert/layer.js"></script>
    <script src="js/layDate-v5.0.9/laydate/laydate.js"></script>
    <%--<script src="../js1/layeralert/laydate.js"></script>--%>
    <script src="../../js/config.js"></script>
    <%--<script type="text/javascript" src="../js1/config.js"></script>--%>
    <%--<script src="../coupon/js/layDate-v5.0.9/layDate-v5.0.9/laydate/laydate.js"></script>--%>
    <script type="text/javascript" src="js/Common.js"></script>
    <%--<script src="../../aspx/Promotion/js/PromotionMain.js"></script>--%>
    <script src="js/PromotionMain.js"></script>
    <script src="js/PrizeUpdate.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/LuckyDraw.js"></script>
    <style type="text/css">
        .dialog_div {
            top: 50px;
            left: 100px;
        }
        .table-container {
            overflow: auto;
        }
        #content-table img {
            width: 100px;
            height: 100px;
        }

        #tab {
            width: 100%;
            max-width: 1000px;
            border: 1px solid;
            border-spacing: 10px;
            border-radius: 5px;
            height: 800px;
            border-collapse: separate;
        }

            #tab td {
                margin-bottom: 10px;
                margin-right: 10px;
                border-radius: 10px;
                width: 30%;
                border: 1px solid;
                text-align: center;
            }

            #tab tr:first-child {
                margin-top: 10px;
            }

            #tab tr td:first-child {
                margin-left: 10px;
            }

            #tab tr td img {
                width: 150px;
                height: 150px;
                margin: 5px auto;
                display: block;
            }

            #tab tr td input[type="text"] {
                width: 40%;
                margin: 5px auto;
                border-radius: 2px;
                border: 1px solid;
            }

            #tab tr td input[type="button"] {
                width: 20%;
                margin: 5px auto;
                border-radius: 2px;
                border: 1px solid;
            }

            #tab tr td span {
                display: block;
                margin: 5px auto;
            }
    </style>
    <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
<link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 抽奖管理 <span class="c-gray en">&gt;</span> 抽奖编辑 <a class="btn btn-success radius r" style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i class="Hui-iconfont">&#xe68f;</i></a></nav>
        
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">抽奖编辑</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="tab-content">
            <dl>
                <dt>截止日期</dt>
                <dd>
                    <input type="hidden" id="luckyDrawId" />
                    <input type="text" readonly="true" placeholder="选择截止时间" class="input normal" id="ExpirationTime" />
                </dd>
            </dl>
            <dl>
                <dt>抽奖内容
                </dt>
                <dd>
                    <table id="tab">
                        <tr>
                            <td>
                                <input type="hidden" id="bhPrize1" value="" />
                                <img src="#" id="imgPrize1" />
                                <span id="namePrize1">&nbsp;</span>
                                数量：<input type="text" value="" id="countPrize1" />
                                <input type="button" value="选择" id="btnPrize1" onclick="XZGZ(1)" />
                            </td>
                            <td>
                                <input type="hidden" id="bhPrize2" value="" />
                                <img src="#" id="imgPrize2" />
                                <span id="namePrize2">&nbsp;</span>
                                数量：<input type="text" value="" id="countPrize2" />
                                <input type="button" value="选择" id="btnPrize2" onclick="XZGZ(2)" />
                            </td>
                            <td>
                                <input type="hidden" id="bhPrize3" value="" />
                                <img src="#" id="imgPrize3" />
                                <span id="namePrize3">&nbsp;</span>
                                数量：<input type="text" value="" id="countPrize3" />
                                <input type="button" value="选择" id="btnPrize3" onclick="XZGZ(3)" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="hidden" id="bhPrize4" value="" />
                                <img src="#" id="imgPrize4" />
                                <span id="namePrize4">&nbsp;</span>
                                数量：<input type="text" value="" id="countPrize4" />
                                <input type="button" value="选择" id="btnPrize4" onclick="XZGZ(4)" />
                            </td>
                            <td>开始抽奖</td>
                            <td>
                                <input type="hidden" id="bhPrize5" value="" />
                                <img src="#" id="imgPrize5" />
                                <span id="namePrize5">&nbsp;</span>
                                数量：<input type="text" value="" id="countPrize5" />
                                <input type="button" value="选择" id="btnPrize5" onclick="XZGZ(5)" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="hidden" id="bhPrize6" value="" />
                                <img src="#" id="imgPrize6" />
                                <span id="namePrize6">&nbsp;</span>
                                数量：<input type="text" value="" id="countPrize6" />
                                <input type="button" value="选择" id="btnPrize6" onclick="XZGZ(6)" />

                            </td>
                            <td>
                                <input type="hidden" id="bhPrize7" value="" />
                                <img src="#" id="imgPrize7" />
                                <span id="namePrize7">&nbsp;</span>
                                数量：<input type="text" value="" id="countPrize7" />
                                <input type="button" value="选择" id="btnPrize7" onclick="XZGZ(7)" />
                            </td>
                            <td>
                                <input type="hidden" id="bhPrize8" value="" />
                                <img src="#" id="imgPrize8" />
                                <span id="namePrize8">&nbsp;</span>
                                数量：<input type="text" value="" id="countPrize8" />
                                <input type="button" value="选择" id="btnPrize8" onclick="XZGZ(8)" />
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
            <dl id="infoDl">
                <dt>抽奖描述</dt>
                <dd>
                    <input type="button" class="btn green" id="Add" value="添加" onclick="AddDl()" /></dd>
            </dl>
        </div>
        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <input type="button" id="btnSubmit" value="提交" class="btn green" onclick="Sub()" /><input type="button" id="btnCreate" value="生效" class="btn" onclick="    CreateLottery()" />
                <input type="button" id="btnCancel" value="撤回" class="btn violet" onclick="BackOut()" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
                <%-- <a href="IntegralGoodsUpdate.aspx?goodsid=ZP0000022">点击</a>--%>
            </div>
        </div>
        <!--/工具栏-->
    </form>
    <!--内容选择开始-->
    <div id="gzxz" class="dialog_div">
        <div class="div1"><span>选择内容</span></div>
        <div id="content">
            <input type="hidden" id="nowId" />
            <div style="font-size: 12px; color: #444;">
                <span id="name"></span>
                <input type="text" id="gsWord" value="" />
                <input type="button" onclick="GetPrize()" value="搜索" />
            </div>
            <div class="table-container" style="height: 350px">
                <table id="content-table" style="width: 100%; border: 0;" cellspacing="0" cellpadding="0" class="ltable">
                </table>
            </div>
            <div class="line20"></div>
            <div class="pagelist">
                <div class='default' id='fanye'>
                    <p class='fl'>
                        <a id="firstPage_list" href="javascript:void(0);" onclick="firstPage(0)">首页</a>
                        <a id="prePage_list" href="javascript:void(0);" onclick="prePage(0)">上一页</a>
                        <a id="nextPage_list" href="javascript:void(0);" onclick="nextPage(0)">下一页</a>
                        <a id="lastPage_list" href="javascript:void(0);" onclick="lastPage(0)">尾页</a>&nbsp;&nbsp;
                          <span class="fr">每页显示<b id="pageSize_list">10</b>条，共<b id="recordCount_list">0</b>条，当前页<b id="pageIndex_list">0</b> /<b id="pageCount_list">0</b> </span>
                    </p>
                </div>
            </div>

        </div>
        <div class="div3">
            <input type="button" value="确定" onclick="confirm()" class="btn" />
            <input type="button" value="取消" onclick="closeDialog()" class="btn yellow" />
        </div>
    </div>
    <!--内容选择结束-->
</body>
</html>
