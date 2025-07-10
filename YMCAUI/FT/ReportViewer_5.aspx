<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=12.0.1100.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportViewer_5.aspx.vb" Inherits="YMCAUI.ReportViewer_5" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ReportViewer</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<asp:Button id="btnFirst" style="Z-INDEX: 105; LEFT: 384px; POSITION: absolute; TOP: 16px" runat="server"
				Height="24px" Width="32px" Text="<<" Font-Bold="True" Font-Size="Small" CssClass="Button_Normal"
				BorderStyle="Inset" ToolTip="First Page" Visible="False"></asp:Button>
			<asp:Button id="btnNext" style="Z-INDEX: 104; LEFT: 448px; POSITION: absolute; TOP: 16px" runat="server"
				Height="24px" Width="32px" Text=">" Font-Bold="True" Font-Size="Small" CssClass="Button_Normal"
				BorderStyle="Inset" ToolTip="Next Page" Visible="False"></asp:Button>
			<asp:Button id="btnPrevious" style="Z-INDEX: 103; LEFT: 416px; POSITION: absolute; TOP: 16px"
				runat="server" Height="24px" Width="32px" Text="<" Font-Bold="True" Font-Size="Small" CssClass="Button_Normal"
				BorderStyle="Inset" ToolTip="Previous page" Visible="False"></asp:Button>
			<asp:Button id="btnLast" style="Z-INDEX: 102; LEFT: 480px; POSITION: absolute; TOP: 16px" runat="server"
				Text=">>" Height="24px" Width="32px" Font-Bold="True" Font-Size="Small" CssClass="Button_Normal"
				BorderStyle="Inset" ToolTip="Last page" Visible="False"></asp:Button>
			<asp:Button id="btnExport" style="Z-INDEX: 106; LEFT: 528px; POSITION: absolute; TOP: 16px"
				runat="server" Text="Export to PDF" Font-Bold="True" CssClass="Button_Normal" BorderStyle="Inset"
				Visible="False"></asp:Button>
			<CR:CrystalReportViewer id="CrystalReportViewer1" style="Z-INDEX: 107; LEFT: 0px; POSITION: absolute; TOP: 8px"
				runat="server" AutoDataBind="true" Width="350px" Height="50px" PrintMode="ActiveX"></CR:CrystalReportViewer></form>
	<script src="../JS/YMCA_CRScript.js" type="text/javascript"></script>
	</body>
</HTML>
