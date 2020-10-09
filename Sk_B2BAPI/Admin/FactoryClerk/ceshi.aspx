<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ceshi.aspx.cs" Inherits="DTcms.Web.admin.FactoryClerk.ceshi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml" > 
<head id="Head1" runat="server"> 
<title>Asp.net生成静态页的两个例子</title> 
</head> 
<body> 
<form id="form1" runat="server"> 
<div> 
标题：<asp:TextBox ID="txtTitle" runat="server" Width="352px"></asp:TextBox><br /> 
内容：<asp:TextBox ID="txtContent" runat="server" Height="179px" TextMode="MultiLine" 
Width="350px"></asp:TextBox><br /> 
<br /> 
<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="根据模板生成" /><br /> 
<br /> 
<br /> 
Url地址：<asp:TextBox ID="txtUrl" runat="server" ToolTip="请确认Url地址的存在" Width="359px"></asp:TextBox> 
<br /> 
<br /> 
<asp:Button ID="Button2" runat="server" Text="根据Url地址生成" OnClick="Button2_Click" /></div> 
</form> 
</body> 
</html> 
