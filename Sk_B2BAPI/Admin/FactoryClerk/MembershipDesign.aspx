<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MembershipDesign.aspx.cs" Inherits="DTcms.Web.admin.FactoryClerk.MembershipDesign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>会员等级维护</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script src="../js/layeralert/layer.js"></script>
    <script src="../ImgUrl/js/layeralert/laydate.js"></script>
    <script src="../js/main.js"></script>
    <link href="../ImgUrl/css/laydate.css" rel="stylesheet" />
    <link href="../ImgUrl/js/layeralert/skin/laydate.css" rel="stylesheet" />
    <script src="js/Membership.js"></script>
    <script src="js/SearchInfo.js"></script>
        <script type="text/javascript">
            $(window).load(function () {
                QueryCart();
            });
      </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
         <div class="location">
          <a href="../center.aspx" class="back"><i></i><span>返回列表页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
          <a href="MembershipDesign.aspx"><span>会员等级管理</span></a>
          <i class="arrow"></i>
         <span>会员等级规则查询</span>
        </div>
        <div class="line10"></div>
        <div class="tab-content">
            <dl>
                <dt>所属类别</dt>
                <dd>
                    <div class="rule-single-select">
                      <select id="FATypeID">
                          <option value="QG">会员等级</option>
                      </select>
                    </div>
                </dd>
            </dl>
            <dl class="bt1" >
                <dt>客户分类</dt>
                <dd>
                    <div class="rule-single-select">
                    <select id="TypeId" >
                        <option value="F001">终端</option>
                        <option value="F002">连锁</option>
                        <option value="F003">批发</option>
                    </select>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>开始日期</dt>
                <dd>
                   <input type="text" id="startDate" class="input normal" onclick="laydate(starts)" />
                </dd>
            </dl>
            <dl>
                <dt>结束日期</dt>
                <dd>
                   <input type="text"  class="input normal" id="endDate" onclick="laydate(ends)"/>
                </dd>
            </dl>
            <dl>
                <dt>规则名称</dt>
                <dd>
                   <input type="text"  class="input normal" id="nametxt"/>
                </dd>
            </dl>
            <div class="table-container" style="width:60%;padding-left:80px">
                 <table class="ltable" style="text-align:center" >
                    <thead>
                      <tr >
                          <th>会员等级</th>
                          <th>满足金额</th>
                          <th>折扣</th>
                          <th>操作</th>
                      </tr>
                    </thead>
                    <tbody id="TBShow">
                     <tr>
                         <td style="color:red" id="LevelIdX" >1</td>
                         <td><input type="text" id="AmountIdX"   class="input normal"/></td>
                         <td><input type="text" id="DiscountIdX" class="input normal"/></td>
                         <td><input type="button" value="保存"  class="btn" onclick="AddCartLevel();" /></td>
                     </tr>
                    </tbody>
                </table>
            </div>
     </div>
    <!--/工具栏-->
     <div class="page-footer" >
      <div id="Div1" class="btn-wrap">
        <input  type="button" value="提交保存" class="btn" onclick="CreateLevel();" />
        <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
      </div>
    </div>
    </form>

</body>
</html>
