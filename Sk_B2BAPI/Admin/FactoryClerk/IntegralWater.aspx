<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IntegralWater.aspx.cs" Inherits="DTcms.Web.admin.FactoryClerk.IntegralWater" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <meta name="viewport"
    content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
  <meta name="apple-mobile-web-app-capable" content="yes" />
  <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
  <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
  <link href="../css/pagination.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="../scripts/jquery/jquery-1.11.2.min.js"></script>
  <script type="text/javascript" src="../scripts/artdialog/dialog-plus-min.js"></script>
  <script type="text/javascript" charset="utf-8" src="../js1/laymain.js"></script>
  <script type="text/javascript" charset="utf-8" src="../js1/common.js"></script>
  <script src="../js1/layeralert/layer.js"></script>
  <%-- <script src="../js1/layeralert/laydate.js"></script>--%>
  <script src="../js1/main.js"></script>
  <%-- <link href="../css/laydate.css" rel="stylesheet" />--%>
  <%-- <link href="../js1/layeralert/css/laydate.css" rel="stylesheet" />--%>
  <%--<link href="../js1/layeralert/skin/laydate.css" rel="stylesheet" />--%>

  <script src="../js1/layeralert/laydate.js"></script>
  <link href="../SignLog/css/laydate.css" rel="stylesheet" />
  <script src="js/SearchInfo.js"></script>
  <script src="../js1/common.js"></script>
  <script src="js/IntegralWater.js"></script>
  <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
  <link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
  <link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
  <title>积分流水</title>
  <style>
    .toolbar-wrap span {
      font-size: 16px;
      font-family: "Microsoft YaHei";
    }
  </style>
  <script>
    $(window).load(function () {
      IntegralWater();
    })
  </script>
</head>

<body>
  <!--导航栏-->
  <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 订单管理 <span
      class="c-gray en">&gt;</span> 积分流水列表 <a class="btn btn-success radius r"
      style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i
        class="Hui-iconfont">&#xe68f;</i></a></nav>
  <%--<div class="location">
          
          <a href="article_list.aspx?channel_id=" class="back"><i></i><span>返回列表页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
          <a href="article_list.aspx?channel_id="><span>业务员管理</span></a>
          <i class="arrow"></i>
         <span>积分流水</span>
        </div>--%>
  <!--/导航栏-->
  <!--工具栏-->
  <div id="floatHead" class="toolbar-wrap">
    <div class="toolbar">
      <div class="box-wrap">
        <a class="menu-btn"></a>
        <div class="l-list">

          <div class="menu-list">
            起始日期：
            <input type="text" id="startDate" onclick="laydate(starts)" class="input normal" style="width: 150px;" />
            截止日期:<input type="text" id="endDate" onclick="laydate(ends)" class="input normal" style="width: 150px;" />

          </div>
        </div>
        <div class="r-list">
          <input type="text" id="txtKeyword" class="keyword" />
          <input type="button" onclick="IntegralWater()" class="btn-search" style="width: 32px; height: 32px;" />
        </div>
      </div>
    </div>
  </div>
  <!--/工具栏-->
  <!--列表-->
  <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
    <thead>
      <tr>
        <th>行号</th>
        <th>客户编号</th>
        <th>客户名称</th>
        <th>类型</th>
        <%--  <th>会员等级</th>--%>
        <th>积分变动</th>
        <th>变动后积分</th>
        <th>现存积分</th>
        <th>改动时间</th>
        <th>电商单号</th>
        <th>ERP单号</th>
      </tr>
    </thead>
    <tbody id="TBShow"></tbody>
  </table>
  <!--/列表-->

  <!--内容底部-->
  <div class="line20"></div>
  <div class="pagelist">
    <div class='default' id='fanye'>
      <p class='fl'>
        <a onclick='btnfirst(IntegralWater)'>首页</a>
        <a style='color: Blue' onclick='btnup(IntegralWater)'>上一页</a>
        <a style='color: Blue' onclick='btnnext(IntegralWater)'>下一页</a>
        <a onclick='btnlast(IntegralWater)'>尾页</a>
        <span class='fr'>每页显示<b id="PageSize">15</b>条，共<b id='RecordCount'>0</b>条,当前页<b id='PageIndex'>1</b>/<b
            id='PageCount'>1</b></span>
    </div>
  </div>
  <!--/内容底部-->
</body>

</html>