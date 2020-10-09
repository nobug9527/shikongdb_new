<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchIntegralGoods.aspx.cs" Inherits="DTcms.Web.admin.IntegralGoods.SearchIntegralGoods" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>促销活动查询</title>
    <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js1/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js1/common.js"></script>
    <script src="../js1/layeralert/layer.js"></script>
    <script src="../js1/main.js"></script>
    <script src="js/IntegralGoodsUpdate.js"></script>
     <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
<link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
    <style>
        .toolbar-wrap span {
            font-size :16px;
            font-family: "Microsoft YaHei";
           
        }
    </style>
        <script type="text/javascript">
            $(window).load(function () {
                $("#sqlvalue").keydown(function (even) {
                    if (event.keyCode == 13) {
                        SearchGoodsInfo();
                    }
                });
                SearchGoodsInfo();
            });
      </script>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 商品管理 <span class="c-gray en">&gt;</span> 积分商品列表 <a class="btn btn-success radius r" style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i class="Hui-iconfont">&#xe68f;</i></a></nav>
        <!--/导航栏-->
        <!--工具栏-->
       
       <div id="floatHead" class="toolbar-wrap">
           
            <div class="toolbar">
                <div class="box-wrap">
                   <a class="menu-btn"></a>
                    <div class="l-list">
                       
                        <div class="menu-list">
                            是否发布：
                             <div class="rule-single-select">
                               <select id="ZXSelect">
                                 <option  value="是">已发布</option>
                                 <option  value="否">未发布</option>
                                 <option  value="清">已取消</option>
                                 <option  value="赠">赠品</option>
                               </select>
                             </div>
                            <div class="rule-single-select">
                                  <select id="FloorId">
                                      <option value="">全部分类</option>
                                      <option value="热门兑换">热门兑换</option>
                                      <option value="家用电器">家用电器</option>
                                      <option value="移动电器">移动电器</option>
                                      <option value="办公用品">办公用品</option>
                                      <option value="积分兑换">其它</option>
                                      <option value="赠品">赠品</option>
                                  </select>
                          </div>
                        </div>
                    </div>
                    <div class="r-list">
                        <input type="text" id="sqlvalue"  class="keyword" />
                        <input type="button" onclick="SearchGoodsInfo(1)" class="btn-search" style="width: 32px; height: 32px;"/>
                    </div>
                </div>
            </div>
        </div>
        <!--/工具栏-->
                <!--列表-->
        <div class="table-container">
             <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                <thead>
                  <tr>
                    <th>行号</th>
                    <th>商品编号</th>
                    <th>商品名称</th>
                    <th>商品规格</th>
                    <th>包装单位</th>
                    <th>生产厂家</th>
                    <th>商品分类</th>
                    <th>价格</th>
                    <th>兑换积分</th>
                    <th>是否发布</th>
                    <th>操作</th>
                  </tr>
                </thead>
                <tbody id="TBShow"></tbody>
            </table>
        </div>
        <!--/列表-->

        <!--内容底部-->
        <div class="line20"></div>
        <div class="pagelist">
            <div class='default' id='fanye'>
              <p class='fl'>
                 <a onclick='btnfirst(SearchGoodsInfo)'>首页</a>
                 <a style='color:Blue' onclick='btnup(SearchGoodsInfo)'>上一页</a>
                 <a style='color:Blue' onclick='btnnext(SearchGoodsInfo)'>下一页</a>
                 <a onclick='btnlast(SearchGoodsInfo)'>尾页</a>
                <span class='fr'>每页显示<b id="PageSize">15</b>条，共<b id='RecordCount'>0</b>条,当前页<b id='PageIndex'>1</b>/<b id='PageCount'>1</b></span>
          </div>
        </div>
        <!--/内容底部-->
    </form>
</body>
</html>
