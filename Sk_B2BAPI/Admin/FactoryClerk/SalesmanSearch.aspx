<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesmanSearch.aspx.cs" Inherits="DTcms.Web.admin.FactoryClerk.SalesmanSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>业务员查询</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script src="../js/layeralert/layer.js"></script>
    <script src="../ImgUrl/js/layeralert/laydate.js"></script>
    <script src="../js/main.js"></script>
    <link href="../ImgUrl/css/laydate.css" rel="stylesheet" />
    <link href="../ImgUrl/js/layeralert/skin/laydate.css" rel="stylesheet" />
    <script src="js/SearchInfo.js"></script>
        <script type="text/javascript">
            $(window).load(function () {
                GetSalesmanInfo();
            });
      </script>
    <style>
        .toolbar-wrap span {
            font-size :16px;
            font-family: "Microsoft YaHei";
           
        }
    </style>
   
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
       <div class="location">
          <a href="SalesmanSearch.aspx" class="back"><i></i><span>返回列表页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
          <a href="SalesmanSearch.aspx"><span>厂家业务员</span></a>
          <i class="arrow"></i>
         <span>业务员查询</span>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
       <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                   <a class="menu-btn"></a>
                    <div class="l-list">
                        <div class="menu-list">
                            级别：
                            <div class="rule-single-select">
                                  <select id="SatausID">
                                      <option value="">全部</option>
                                      <option value="1">一级</option>
                                      <option value="2">二级</option>
                                      <option value="3">三级</option>
                                  </select>
                          </div>
                         
                        </div>
                    </div>
                     <div class="r-list" >
                        <input type="text" id="sqlvalue" class="keyword" />
                        <input type="button" class="btn-search" value="查询"  onclick="GetSalesmanInfo()" />
                    </div>
                   
                </div>
            </div>
        </div>
        <!--/工具栏-->
                <!--列表-->
       
             <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable" >
                <thead>
                  <tr>
                     <th>序号</th>
                      <th>代理编号</th>
                      <th>代理名称</th>
                      <th>上级编号</th>
                      <th>级别</th>
                      <th>绑定客户</th>
                      <th>绑定商品</th>
                      <th>客户管理</th>
                      <th>商品管理</th>
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
                 <a onclick='btnfirst(GetSalesmanInfo)'>首页</a>
                 <a style='color:Blue' onclick='btnup(GetSalesmanInfo)'>上一页</a>
                 <a style='color:Blue' onclick='btnnext(GetSalesmanInfo)'>下一页</a>
                 <a onclick='btnlast(GetSalesmanInfo)'>尾页</a>
                <span class='fr'>每页显示<b id="PageSize">15</b>条，共<b id='RecordCount'>0</b>条,当前页<b id='PageIndex'>1</b>/<b id='PageCount'>1</b></span>
          </div>
        </div>
        <!--/内容底部-->
    </form>
</body>
</html>
