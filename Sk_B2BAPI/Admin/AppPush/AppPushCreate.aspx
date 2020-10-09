<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppPushCreate.aspx.cs" Inherits="DTcms.Web.admin.AppPush.AppPushCreate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>App消息推送维护</title>
    <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script src="../js1/layeralert/layer.js"></script>
    <script src="../js1/layeralert/laydate.js"></script>
    <script src="../js1/main.js"></script>
    <link href="../js1/layeralert/css/laydate.css" rel="stylesheet" />
    <link href="../SignLog/css/laydate.css" rel="stylesheet" />
    <script src="../LuckyDraw/js/layDate-v5.0.9/laydate/laydate.js"></script>
    <script src="js/AppPush.js"></script>
        <script type="text/javascript">
            $(window).load(function () {
                 laydate.render({
                elem: '#txtStartDate'
            });
            laydate.render({
                elem: '#txtKsTime'
              , type: 'time'
            });
            });
      </script>
    <link rel="stylesheet" type="text/css" href="../static/h-ui/css/H-ui.min.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/H-ui.admin.css" />
<link rel="stylesheet" type="text/css" href="../lib/Hui-iconfont/1.0.8/iconfont.css" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/skin/default/skin.css" id="skin" />
<link rel="stylesheet" type="text/css" href="../static/h-ui.admin/css/style.css" />
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 消息推送 <span class="c-gray en">&gt;</span> App维护 <a class="btn btn-success radius r" style="line-height: 1.6em; margin-top: 3px" href="javascript:location.replace(location.href);" title="刷新"><i class="Hui-iconfont">&#xe68f;</i></a></nav>
        
        <div class="tab-content">
            <dl class="bt1" >
                <dt>消息类型</dt>
                <dd>
                    <div class="rule-single-select">
                    <select id="TypeId" >
                        <option value="公告">公告</option>
                        <option value="积分">积分</option>
                    </select>
                    </div>
                </dd>
            </dl>
               <dl class="bt1" >
                <dt>客户类型</dt>
                <dd>
                    <div class="rule-single-select">
                    <select id="KhTypeId" >
                        <option value="">全部</option>
                        <option value="F001">终端</option>
                        <option value="F002">连锁</option>
                        <option value="F003">批发</option>
                    </select>
                    </div>
                </dd>
            </dl>
           <%-- <dl>
                <dt>日期</dt>
                <dd>
                   <input type="text" id="startDate" class="input normal" onclick="laydate(starts)" />
                </dd>
            </dl>--%>
            <dl>
                <dt>开始日期</dt>
                <dd>
                   <input type="text" id="txtStartDate" class="input normal"  placeholder="请输入开始日期"/>
                    开始时间 <input id="txtKsTime"  type="text" class="input normal" placeholder="请输入开始时间"/>
                </dd>
            </dl>
            <dl>
                <dt>标题</dt>
                <dd>
                   <input type="text"  class="input normal" id="nametxt"/>
                </dd>
            </dl>
            <dl>
               <dt>Url地址</dt>
                <dd>
                   <input type="text"  class="input normal" id="Urltxt"/>
                </dd>
            </dl>
            <dl>
               <dt>类容</dt>
                <dd>
                    <textarea class="input normal" id="notetxt" ></textarea>
                </dd>
            </dl>
     </div>
    <!--/工具栏-->
     <div class="page-footer" >
      <div id="Div1" class="btn-wrap">
        <input  type="button" value="提交保存" class="btn green" onclick="CreatePush();" />
        <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
      </div>
    </div>
    </form>

</body>
</html>

