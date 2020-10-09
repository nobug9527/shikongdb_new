<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateIntegral.aspx.cs" Inherits="DTcms.Web.admin.FactoryClerk.UpdateIntegral" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../pinpaiZG/css/common.css" rel="stylesheet" />
     <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script src="../js/layeralert/layer.js"></script>
    <script src="../js/main.js"></script>
    <script src="js/UpdateIntegral.js"></script>
    <title>修改积分</title>
</head>
<body runat="server">
      <div class="location">
          <a href="../center.aspx" class="back"><i></i><span>返回列表页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
          <a href="FlashSale_Design.aspx"><span>促销管理</span></a>
          <i class="arrow"></i>
         <span>修改积分</span>
        </div>
        <div class="line10"></div>
        <div class="tab-content">
            <dl>
                <dt>客户名称</dt>
                <dd>
                    <input type="text" id="txtKhmch" class="input normal" runat="server" readonly="readonly"/>
                </dd>
            </dl>
            <dl>
                <dt>现有积分</dt>
                <dd>
                   <input type="text" id="txtXyjf" class="input normal" runat="server" readonly="readonly"/>
                </dd>
            </dl>
             <dl>
                <dt>改动积分</dt>
                <dd>
                    <input type="text" id="txtGdjf" class="input normal" placeholder="请填写增加或者减少的积分"/>
                </dd>
            </dl>
              <dl>
                <dt>积分操作</dt>
                <dd>
                    <div class="rule-multi-radio" id="Jfcz">
                      <label><input type="radio" checked="checked" name="ShowID" value="txtZjjf" />增加积分</label>
                      <label><input type="radio" name="ShowID" value="txtJsjf" />减少积分</label>
                    </div>
                </dd>
            </dl>
     </div>
    <!--/工具栏-->
     <div class="page-footer" >
      <div id="Div1" class="btn-wrap">
        <input  type="button" value="提交保存" class="btn" onclick="SubmitIntegral();" />
        <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
      </div>
    </div>
   
</body>
</html>
