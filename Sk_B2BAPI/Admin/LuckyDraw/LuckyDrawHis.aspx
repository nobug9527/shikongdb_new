<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LuckyDrawHis.aspx.cs" Inherits="DTcms.Web.admin.LuckyDraw.LuckyDrawHis" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <title>中奖纪录</title>
  <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" />
  <link href="../skin/default/style.css" rel="stylesheet" />
  <link href="../css/pagination.css" rel="stylesheet" />
  <link href="../skin/default/cxgz.css" rel="stylesheet" />
  <link href="../skin/base.css" rel="stylesheet" />
  <script src="../js/jquery.min.js"></script>
  <%--<script type="text/javascript" src="../js1/jquery.min.js"></script>--%>
  <script src="../../js/config.js"></script>
  <script src="../js1/layeralert/layer.js"></script>
  <script src="js/layDate-v5.0.9/laydate/laydate.js"></script>
  <%--<script src="../ImgUrl/js/layeralert/layer.js"></script>--%>
  <%--    <script src="../js1/layeralert/layer.js"></script>--%>
  <%--<script src="../js1/layeralert/laydate.js"></script>--%>
  <%--    <script src="../coupon/js/layDate-v5.0.9/layDate-v5.0.9/laydate/laydate.js"></script>--%>
  <script type="text/javascript" src="js/Common.js"></script>
  <script type="text/javascript" src="js/LuckyDrawHis.js"></script>

  <style type="text/css">
    #content-table img {
      width: 100px;
      height: 100px;
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
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 抽奖管理 <span
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
              <li><a>
                  <input type="radio" name="cxgz" value="All" />查看全部</a></li>
              <li><a>
                  <input type="radio" name="cxgz" value="False" checked="checked" />查看中奖</a></li>
              <li><a>
                  开始日期：<input type="text" readonly="readonly" id="startDate" /></a></li>
              <li><a>
                  截止日期：<input type="text" readonly="readonly" id="endDate" /></a></li>
            </ul>
          </div>
          <div class="r-list">
            <input type="text" id="keyword" value="" class="keyword" />
            <input type="button" onclick="coupon_search()" class="btn-search" style="width: 32px; height: 32px;" />
          </div>
        </div>
      </div>
    </div>
    <!-- 分页 -->
    <div class="table-container">
      <!--文字列表-->
      <table style="width: 100%; border: none;" cellspacing="0" cellpadding="0" class="ltable" id="content-table">
        <thead>
          <tr>
            <th>编号</th>
            <th>中奖客户编号</th>
            <th>中奖客户名称</th>
            <th>奖品图片</th>
            <th>奖品编号</th>
            <th>奖品名称</th>
            <th>奖品类型</th>
            <th>奖品价值</th>
            <th>中奖时间</th>
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