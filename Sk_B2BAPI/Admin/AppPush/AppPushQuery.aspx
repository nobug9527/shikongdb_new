<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppPushQuery.aspx.cs" Inherits="DTcms.Web.admin.AppPush.AppPushQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <meta name="viewport"
    content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
  <meta name="apple-mobile-web-app-capable" content="yes" />
  <title>会员查询</title>
  <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
  <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
  <link href="../css/pagination.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="../scripts/jquery/jquery-1.11.2.min.js"></script>
  <script type="text/javascript" src="../scripts/artdialog/dialog-plus-min.js"></script>
  <script type="text/javascript" charset="utf-8" src="../js1/laymain.js"></script>
  <script type="text/javascript" charset="utf-8" src="../js1/common.js"></script>
  <script src="../js1/layeralert/layer.js"></script>
  <script src="../js1/layeralert/laydate.js"></script>
  <script src="../js1/main.js"></script>
  <link href="../js1/layeralert/css/laydate.css" rel="stylesheet" />
  <script src="js/AppPush.js"></script>
  <script type="text/javascript">
    $(window).load(function () {
      AppPushQuery();
    });
  </script>
  <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
  <link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
</head>

<body>
  <form id="form1" runat="server">
    <!--导航栏-->

    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 消息推送 <span
        class="c-gray en">&gt;</span> 消息查询 <a class="btn btn-success radius r"
        style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i
          class="Hui-iconfont">&#xe68f;</i></a></nav>

    <!--/导航栏-->
    <!--工具栏-->
    <div id="floatHead" class="toolbar-wrap">
      <div class="toolbar">
        <div class="box-wrap">
          <a class="menu-btn"></a>
          <div class="l-list">

          </div>
          <div class="r-list">
            <input type="text" id="sqlvalue" class="keyword" />
            <input type="button" class="btn-search" value="查询" onclick="AppPushQuery()"
              style="width: 32px; height: 32px;" />
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
            <th>序号</th>
            <th>类型</th>
            <th>推送范围</th>
            <th>推送时间</th>
            <th>标题</th>
            <th>Url地址</th>
            <th>是否推送</th>
            <th>是否发布</th>
            <th>类容</th>
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
          <a onclick='btnfirst(AppPushQuery)'>首页</a>
          <a style='color:Blue' onclick='btnup(AppPushQuery)'>上一页</a>
          <a style='color:Blue' onclick='btnnext(AppPushQuery)'>下一页</a>
          <a onclick='btnlast(AppPushQuery)'>尾页</a>
          <span class='fr'>每页显示<b id="PageSize">15</b>条，共<b id='RecordCount'>0</b>条,当前页<b id='PageIndex'>1</b>/<b
              id='PageCount'>1</b></span>
      </div>
    </div>
    <!--/内容底部-->
  </form>
</body>

</html>