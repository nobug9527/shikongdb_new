<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PresentStatistical.aspx.cs" Inherits="DTcms.Web.admin.FactoryClerk.PresentStatistical" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
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
    <script src="js/PresentStatistical.js"></script>
    <title>礼品对付明细</title>
    <style>
        .toolbar-wrap span {
            font-size :16px;
            font-family: "Microsoft YaHei";
        }
    </style>
    <script>
        $(window).load(function () {
            PresentStatistical();
        })
    </script>
</head>
<body>
        <!--导航栏-->
       <div class="location">
          <a href="PresentStatistical.aspx?channel_id=" class="back"><i></i><span>返回列表页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
          <a href="PresentStatistical.aspx?channel_id="><span>业务员管理</span></a>
          <i class="arrow"></i>
         <span>礼品对付明细</span>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
       <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                   <a class="menu-btn"></a>
                    <div class="l-list">
                        <div class="menu-list">
                            起始日期： <input type="text" id="startDate" onclick="laydate(starts)" class="input normal" style="width:150px;"/>
                            截止日期:<input type="text" id="endDate" onclick="laydate(ends)" class="input normal" style="width:150px;"/>                           
                        </div>
                           促销类型：
                           <div class="rule-single-select">
                               <select id="FaTypeId">
                                 <option  value="HG">换购</option>
                                 <option  value="MZ">满赠</option>
                                 <option  value="ZH">组合</option>
                                 <option  value="CJ">抽奖</option>
                               </select>
                             </div>
                        <a class="cx btn_b" onclick="DCExcel()">导出到Excel</a>
                    </div>
                    
                    <div class="r-list" >
                         <input type="text" id="txtKeyword"  class="keyword" />
                        <input type="button" onclick="PresentStatistical()" class="btn-search" style="width: 32px; height: 32px;"/>
                    </div>
                </div>
            </div>
        </div>
        <!--/工具栏-->
                <!--列表-->
             <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable" id="TBShow">
                <thead>
                  <tr>
                    <th>行号</th>
                    <th>单据编号</th>
                    <th>日期</th>
                    <th>客户编号</th>
                    <th>客户名称</th>
                    <th>商品名称</th>
                    <th>商品规格</th>
                    <th>生产厂家</th>
                    <th>数量</th>
                    <th>价格</th>
                    <th>备注</th>
                  </tr>
                </thead>
                <tbody></tbody>
            </table>
        <!--/列表-->

        <!--内容底部-->
        <div class="line20"></div>
        <div class="pagelist">
            <div class='default' id='fanye'>
              <p class='fl'>
                 <a onclick='btnfirst(PresentStatistical)'>首页</a>
                 <a style='color:Blue' onclick='btnup(PresentStatistical)'>上一页</a>
                 <a style='color:Blue' onclick='btnnext(PresentStatistical)'>下一页</a>
                 <a onclick='btnlast(PresentStatistical)'>尾页</a>
                <span class='fr'>每页显示<b id="PageSize">15</b>条，共<b id='RecordCount'>0</b>条,当前页<b id='PageIndex'>1</b>/<b id='PageCount'>1</b></span>
          </div>
        </div>
        <!--/内容底部-->
</body>
</html>

