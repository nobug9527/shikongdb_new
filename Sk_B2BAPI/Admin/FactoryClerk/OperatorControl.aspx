<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperatorControl.aspx.cs" Inherits="DTcms.Web.admin.FactoryClerk.OperatorControl" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>内容管理</title>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
  
    <link href="../pinpaiZG/css/common.css" rel="stylesheet" />
     <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script  type="text/javascript" charset="utf-8" src="../ImgUrl/js/main.js"></script>
    <script src="../js/layeralert/layer.js"></script>
    <script src="../ImgUrl/js/layeralert/laydate.js"></script>
    <script src="../js/main.js"></script>
    <link href="../ImgUrl/css/laydate.css" rel="stylesheet" />
    <link href="../ImgUrl/js/layeralert/skin/laydate.css" rel="stylesheet" />

    <link href="../skin/default/cxgz.css" rel="stylesheet" />
    <script src="js/OperatorControl.js"></script>
    <script>
        $(window).load(function () {
            QueryOperatorControl();
        })
    </script>
</head>
<body class="mainbody" style="overflow: auto;">
    <form id="form1" runat="server" style="position: absolute; z-index: 1;">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>业务员修改价格管控</span>
            <div class="l-list" style="float: right; margin-right: 150px; color: darkgreen;">
                按照如下进行搜索： 规则名称
            </div>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <ul class="icon-list" style="margin: 0 10px 0 12px;">
                            <li><a class="add" onclick="Search_YwyInfo()"><i></i><span>新增</span></a></li>
                        </ul>
                    </div>
                    <div class="r-list">
                        <input type="text" id="keyword" value="" class="keyword" />
                        <input type="button" onclick="QueryOperatorControl()" class="btn-search" style="width: 32px; height: 32px;" />
                    </div>
                </div>
            </div>
        </div>
        <!--/工具栏-->
            <div class="table-container">
             <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                <thead>
                  <tr>
                    <th>行号</th>
                    <th>业务员编号</th>
                    <th>业务员名称</th>
                    <th>业务员分组</th>
                    <th>业务员级别</th>
                    <th>区域</th>
                    <th>维护日期</th>
                    <th>是否启用</th>
                    <th>操作</th>
                  </tr>
                </thead>
                <tbody id="TBShow"></tbody>
            </table>
           </div>
                <!--内容底部-->
        <div class="line20"></div>
        <div class="pagelist">
            <div class='default' id='fanye'>
              <p class='fl'>
                 <a onclick='btnfirst(QueryOperatorControl)'>首页</a>
                 <a style='color:Blue' onclick='btnup(QueryOperatorControl)'>上一页</a>
                 <a style='color:Blue' onclick='btnnext(QueryOperatorControl)'>下一页</a>
                 <a onclick='btnlast(QueryOperatorControl)'>尾页</a>
                <span class='fr'>每页显示<b id="PageSize">10</b>条，共<b id='RecordCount'>0</b>条,当前页<b id='PageIndex'>1</b>/<b id='PageCount'>1</b></span>
          </div>
        </div>
        <!--/内容底部-->
    </form>
</body>
</html>

