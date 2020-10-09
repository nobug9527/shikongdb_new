<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jfddcx.aspx.cs" Inherits="DTcms.Web.admin.jfddcx.jfddcx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
  <title>积分订单查询</title>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
  <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
  <link href="../css/pagination.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="../scripts/jquery/jquery-1.11.2.min.js"></script>
  <script type="text/javascript" src="../scripts/artdialog/dialog-plus-min.js"></script>
  <script type="text/javascript" charset="utf-8" src="../js1/laymain.js"></script>
  <script src="../js1/common.js"></script>
  <script src="../js1/layeralert/layer.js"></script>
  <script src="../js1/main.js"></script>
  <script src="js/ddselect.js"></script>
  <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
  <link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
</head>

<body>
  <form id="form1" runat="server">
    <!--面包屑-->
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 订单管理 <span
        class="c-gray en">&gt;</span> 积分订单列表 <a class="btn btn-success radius r"
        style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i
          class="Hui-iconfont">&#xe68f;</i></a></nav>
    <!-- 搜索框 -->
    <div id="floatHead" class="toolbar-wrap">
      <div class="toolbar">
        <div class="box-wrap">
          <a class="menu-btn"></a>
          <div class="l-list">
            <div class="menu-list"></div>
          </div>
          <div class="r-list">
            <input type="text" id="username" class="keyword" placeholder="请输入客户名称" />
            <input type="button" class="btn-search" value="查询" onclick="ddSelect()" />
          </div>
        </div>
      </div>
    </div>
    <!-- 表格 -->
    <div class="table-container">
      <table border="0" cellspacing="0" cellpadding="0" class="ltable">
        <thead>
          <tr>
            <th>单据编号</th>
            <th>单位名称</th>
            <th>日期</th>
            <th>时间</th>
            <th>业务员</th>
            <th>数量</th>
            <th>积分数</th>
            <th>订单状态</th>
            <th>明细</th>
          </tr>
        </thead>
        <tbody id="TBShow"></tbody>
      </table>
    </div>
    <!-- 分页 -->
    <div class="line20"></div>
    <div class="pagelist">
      <div class='default' id='fanye'>
        <p class='fl'>
          <a onclick='btnfirst(ddSelect)'>首页</a>
          <a style='color:Blue' onclick='btnup(ddSelect)'>上一页</a>
          <a style='color:Blue' onclick='btnnext(ddSelect)'>下一页</a>
          <a onclick='btnlast(ddSelect)'>尾页</a>
          <span class='fr'>每页显示<b id="PageSize">15</b>条，共<b id='RecordCount'>0</b>条,当前页<b id='PageIndex'>1</b>/<b
              id='PageCount'>1</b></span>
      </div>
    </div>
    <!--内容底部-->
    <div class="table-container" id="showmx" style="display:none">
      <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
        <thead>
          <tr>
            <th>单据编号</th>
            <th>日期</th>
            <th>时间</th>
            <th>商品名称</th>
            <th>单位</th>
            <th>数量</th>
            <th>积分</th>
            <th>积分总额</th>
          </tr>
        </thead>
        <tbody id="Tbody1"></tbody>
      </table>
    </div>
  </form>
</body>

</html>