<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignLog.aspx.cs" Inherits="DTcms.Web.admin.SignLog.SignLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>签到功能</title>
     <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
   <%-- <link href="../pinpaiZG/css/common.css" rel="stylesheet" />--%>
   <%-- <link href="css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="css/common.css" rel="stylesheet" />
     <link href="../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script src="../js/jquery.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js1/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js1/common.js"></script>
    <script src="../js1/layeralert/layer.js"></script>
    <script src="../js1/layeralert/laydate.js"></script>
    <link href="../js1/layeralert/css/laydate.css" rel="stylesheet" />
    <link href="css/laydate.css" rel="stylesheet" />
    <script src="../js1/main.js"></script>
    <%--<script src="../layer/2.4/layer.js"></script>--%>
    <script src="js/signLog.js"></script>
    <script>
        $(window).load(function () {
             QuerySign();
        });
    </script>
    <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
<link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
</head>
<body>
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 签到管理 <span class="c-gray en">&gt;</span> 签到功能 <a class="btn btn-success radius r" style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i class="Hui-iconfont">&#xe68f;</i></a></nav>
        
       
       
        <div class="tab-content">
            <dl>
                <dt>客户类型</dt>
                <dd>
                     <div class="rule-single-select">
                      <select id="KhLx">
                          <option value="ALL" selected="selected">全部</option>
                          <option value="F001">终端</option>
                          <option value="F002">连锁</option>
                          <option value="F003">批发</option>
                      </select>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>签到模式</dt>
                <dd>
                     <div class="rule-single-select">
                      <select id="SignModel">
                         <%-- <option value="1" selected="selected">当天签到</option>--%>
                          <option value="2" selected="selected">连续签到</option>
                      </select>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>发布状态</dt>
                <dd>
                     <div class="rule-single-select">
                      <select id="ShowStatus">
                          <option value="N" selected="selected">不发布</option>
                          <option value="Y">发布</option>
                      </select>
                    </div>
                </dd>
            </dl>
            <div class="table-container" style="width:90%;padding-left:80px">
                 <table class="ltable" style="text-align:center" >
                    <thead>
                      <tr >
                          <th>奖励信息</th>
                          <th>奖励形式</th>
                          <th>签到规则</th>
                          <th>奖励规则</th>
                          <th>操作</th>
                      </tr>
                    </thead>
                    <tbody id="TBShow">
                     <tr >
                         <td style="color:red" id="LevelIdX" >1</td>
                         <td>
                             <div id="Re">
                               <input  type="radio" name="rewardForm" value="积分" checked="checked"/>积分
                               <%--<input  type="radio" name="rewardForm" value="优惠券"/>优惠券--%>
                            </div>
                         </td>
                         <td>
                              <input  type="text" id="slSignRule" value="" placeholder="请输入"/>天
                           
                         </td>
                         <td>
                              <input  type="text" id="slRewardRule" value="" placeholder="请输入"/>积分 
                         </td>
                         <td><input type="button" value="保存"  class="btn green" onclick="SaveSign();" /></td>
                     </tr>
                    </tbody>
                </table>
            </div>
            <dl>
                <dt>规则叙述</dt>
                <dd>
                    <textarea name="GzMx"   id="GzMx" class="input" rows="10" cols="40" style="width:700px!important;height:200px!important;"></textarea>
                    <span><label style="color:red;font-size:16px;">*</label>每条规则请分条陈述,在结束处添加'<label style="color:red;font-size:16px;">/</label>’</span>
                </dd>
            </dl>
     </div>
    <!--/工具栏-->
     <div class="page-footer" >
      <div id="Div1" class="btn-wrap">
        <input  type="button" value="提交保存" class="btn green" onclick="CreateSign();" />
        <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
      </div>
    </div>
</body>
</html>
