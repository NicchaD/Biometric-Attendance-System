<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ErrorForSession.aspx.vb" Inherits="YMCAUI.ErrorForSession"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ErrorForSession</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<!--#include virtual="TopNew.htm"-->
		<form id="Form1" method="post" runat="server">
			<div class="Div_Center">
				<table class="Table_WithoutBorder" width="700">
					<tr>
						<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
							Error Page
						</td>
					</tr>
				</table>
				<table class="Table_WithoutBorder" width="700">
					<tr>
						<td>&nbsp;
						</td>
					</tr>
					<tr>
						<td>&nbsp;
						</td>
					</tr>
					<tr>
						<td align="center">
							<asp:TextBox id="TextBoxMess" runat="server" TextMode="MultiLine" ReadOnly="True" BackColor="#FFFFC0"
								Height="70px" Width="656px"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td>&nbsp;
						</td>
					</tr>
					<tr>
						<td>&nbsp;
						</td>
					</tr>
				</table>
				<asp:button id="ButtonHome" runat="server" CssClass="Button_Normal" Width="64px" Text="Home"
					Visible="False"></asp:button>
				<asp:Button id="ButtonClose" runat="server" Text="Close" CssClass="Button_Normal" Width="64px"
					Visible="False"></asp:Button>
			</div>
		</form>
	</body>
</HTML>
