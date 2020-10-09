<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignLogQuery.aspx.cs" Inherits="DTcms.Web.admin.SignLog.SignLogQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <title>签到规则查询</title>
     <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js1/laymain.js"></script>
    <%--<script type="text/javascript" charset="utf-8" src="../js1/common.js"></script>--%>
   <script src="../js1/layeralert/layer.js"></script>
    <script src="../js1/layeralert/laydate.js"></script>
   <%-- <script src="../ImgUrl/js/layeralert/laydate.js"></script>--%>
    <script src="../js1/main.js"></script>
    <%--<link href="../ImgUrl/css/laydate.css" rel="stylesheet" />--%>
    <%--<link href="../ImgUrl/js/layeralert/skin/laydate.css" rel="stylesheet" />--%>
    <link href="css/laydate.css" rel="stylesheet" />
    <link href="../js1/layeralert/css/laydate.css" rel="stylesheet" />
    <%--<script src="../pinpaiZG/js/common.js"></script>--%>
    <script src="../js1/common.js"></script>
    <script src="js/signLogQuery.js"></script>
    <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
<link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
</head>
<body>
     <!--导航栏-->
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 签到管理 <span class="c-gray en">&gt;</span> 促销管理 <a class="btn btn-success radius r" style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i class="Hui-iconfont">&#xe68f;</i></a></nav>
       
      <%-- <div class="location">
          <a href="article_list.aspx?channel_id=" class="back"><i></i><span>返回列表页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
          <a href="article_list.aspx?channel_id="><span></span></a>
          <i class="arrow"></i>
         <span>组合方案查询</span>
        </div>--%>
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
                               <select id="slStatus">
                                 <option  value="N">未发布</option>
                                 <option  value="Y" selected="selected">已发布</option>
                               </select>
                             </div>
                            <div class="rule-single-select">
                                  <select id="slType">
                                      <%--<option value="1">当日签到</option>--%>
                                      <option value="2">连续签到</option>
                                  </select>
                          </div>
                        </div>
                    </div>
                    <div class="r-list" >
                         <input type="text" id="txtKeyWord"  class="keyword" />
                        <input type="button" onclick="GetSignLog()" class="btn-search" style="width: 32px; height: 32px;"/>
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
                    <th>方案编号</th>
                    <th>签到类型</th>
                    <th>客户类型</th>
                    <th>奖励形式</th>
                    <th>持续天数</th>
                    <th>每次奖励</th>
                    <th>添加时间</th>
                    <th>规则描述</th>
                    <th>发布状态</th>
                    <th>操作</th>
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
                 <a onclick='btnfirst(GetSignLog)'>首页</a>
                 <a style='color:Blue' onclick='btnup(GetSignLog)'>上一页</a>
                 <a style='color:Blue' onclick='btnnext(GetSignLog)'>下一页</a>
                 <a onclick='btnlast(GetSignLog)'>尾页</a>
                <span class='fr'>每页显示<b id="PageSize">10</b>条，共<b id='RecordCount'>0</b>条,当前页<b id='PageIndex'>1</b>/<b id='PageCount'>1</b></span>
          </div>
        </div>
      
</body>
</html>
<style>
     .cr {
            background-color: #009688;
        }
        .bz {
            color: #fff;
            line-height: 25px;
            height: 40px;
            cursor:pointer;
            padding:0 5px;
        }
        .cl {
            background-color: #ffb800;
        }
         .co {
            background-color: #1e9fff;
        }
    a {
        text-decoration:none!important;
    }
    .btnmx {
        float:right;
    display: inline-block;
    height: 38px;
    line-height: 38px;
    padding: 0 18px;
    background-color: #009688;
    color: #fff;
    white-space: nowrap;
    text-align: center;
    font-size: 14px;
    border: none;
    border-radius: 2px;
    cursor: pointer;
    }
</style>
<script>
      $(function () {
           GetSignLog()
        });
</script>