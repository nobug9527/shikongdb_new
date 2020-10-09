<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrizeSetting.aspx.cs" Inherits="DTcms.Web.admin.LuckyDraw.PrizeSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <title>抽奖奖品设置</title>
  <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" />
  <link href="../skin/default/style.css" rel="stylesheet" />
  <link href="../css/pagination.css" rel="stylesheet" />
  <link href="../skin/default/cxgz.css" rel="stylesheet" />
  <link href="../skin/base.css" rel="stylesheet" />
  <script type="text/javascript" src="../js/jquery.min.js"></script>
  <script type="text/javascript" src="../../js/config.js"></script>
  <script src="../js1/layeralert/layer.js"></script>
  <%--<script src="../ImgUrl/js/layeralert/layer.js"></script>--%>
  <%--<script src="../../aspx/Promotion/js/PromotionMain.js"></script>--%>
  <script type="text/javascript" src="js/Common.js"></script>
  <script type="text/javascript" src="js/PrizeSetting.js"></script>
  <style>
    .table1 thead {
      display: block;
      width: 100%;
    }

    .table1 thead th {
      width: 4.8%;
    }

    .table1 tbody {
      display: block;
      height: 300px;
      overflow: auto;
    }

    .table1 tbody td {
      width: 10%;
    }

    .table2 thead {
      display: block;
      width: 100%;
    }

    .table2 thead th {
      width: 1%;
    }

    .table2 tbody {
      display: block;
      height: 230px;
      overflow: auto;
    }

    .table2 tbody td {
      width: 1%;
    }

    #tbd img {
      width: 100px;
    }
  </style>
  <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
  <link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
</head>

<body>
  <form id="form1" runat="server" style="position: absolute; z-index: 1;">
    <!--导航栏-->
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 奖品管理 <span
        class="c-gray en">&gt;</span> 抽奖记录 <a class="btn btn-success radius r"
        style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i
          class="Hui-iconfont">&#xe68f;</i></a></nav>


    <!--/导航栏-->
    <!--工具栏-->
    <div id="floatHead" class="toolbar-wrap">
      <div class="toolbar">
        <div class="box-wrap">
          <a class="menu-btn"></a>
          <div class="l-list">
            <ul class="icon-list" style="margin: 0 10px 0 12px;">
              <li><a class="add" href="PrizeUpdate.aspx"><i></i><span>新增</span></a></li>
            </ul>
          </div>
          <div class="r-list">
            <input type="text" id="keyword" value="" class="keyword" />
            <input type="button" onclick="coupon_search()" class="btn-search" style="width: 32px; height: 32px;" />
          </div>
        </div>
      </div>
    </div>
    <!--/工具栏-->
    <div class="table-container">
      <!--文字列表-->
      <table style="width: 100%; border: none;" cellspacing="0" cellpadding="0" class="ltable">
        <thead>
          <tr>
            <th>编号</th>
            <th>图片</th>
            <th>奖品名称(5个字)</th>
            <th>奖品类型</th>
            <th>价值数量(积分，金钱)</th>
            <th>最后修改时间</th>
            <th>操作</th>
          </tr>
        </thead>
        <tbody id="tbd">
        </tbody>
      </table>
      <div class="line20"></div>
      <div class="pagelist">
        <div class='default' id='fanye'>
          <p class='fl'>
            <a id="firstPage_list" href="javascript:void(0);" onclick="firstPage(0)">首页</a>
            <a id="prePage_list" href="javascript:void(0);" onclick="prePage(0)">上一页</a>
            <a id="nextPage_list" href="javascript:void(0);" onclick="nextPage(0)">下一页</a>
            <a id="lastPage_list" href="javascript:void(0);" onclick="lastPage(0)">尾页</a>&nbsp;&nbsp;
            <span class="fr">每页显示<b id="pageSize_list">15</b>条，共<b id="recordCount_list">0</b>条，当前页<b
                id="pageIndex_list">0</b> /<b id="pageCount_list">0</b> </span>
          </p>
        </div>
      </div>
      <!--/文字列表-->
    </div>
  </form>
</body>

</html>