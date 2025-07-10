<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CopyFilesUtilityForm.aspx.vb" Inherits="YMCAUI.CopyFilesUtilityForm" %>
	<!--#include virtual="top.html"-->
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CopyFilesUtilityForm</title> 	
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="750" align="center">
				<tr align="center">
					<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
							CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
							MenuFadeDelay="2" mouseovercssclass="MouseOver"><SELECTEDMENUITEMSTYLE BackColor="#FBC97A" ForeColor="#3B5386"></SELECTEDMENUITEMSTYLE>
						</cc1:menu></td>
				</tr>
				<tr>
					<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
						Copy Files Utility
					</td>
				</tr>
				<tr>
					<td>&nbsp;
					</td>
				</tr>
			</table>
			<div class="Div_Center">
				<table class="Table_WithBorder" width="730" align="center">
					<tr>
						<td>
							<div class="Div_Center">
								<table class="Table_WithoutBorder" width="740">
									<TBODY>
										<tr>
											<td align="left"></td>
											<td align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
										</tr>
										<tr>
											<td align="left" width="40%"><asp:label id="LabelIDM" runat="server" CssClass="Label_Small"> IDM</asp:label></td>
											<td align="left"><asp:checkbox id="CheckBoxIDM" runat="server" CssClass="CheckBox_Normal" Checked="True" Text=" "></asp:checkbox></td>
										</tr>
										<tr>
											<td align="left" width="40%"><asp:label id="LabelDelinquency" runat="server" CssClass="Label_Small">
													Delinquency CSV Files</asp:label></td>
											<td align="left"><asp:checkbox id="CheckBoxDELINQ" runat="server" CssClass="CheckBox_Normal" Checked="True" Text=" "></asp:checkbox></td>
										</tr>
										<tr>
											<td align="left" width="40%"><asp:label id="LabelACH" runat="server" CssClass="Label_Small">ACH CSV Files</asp:label></td>
											<td align="left"><asp:checkbox id="CheckBoxACH" runat="server" CssClass="CheckBox_Normal" Checked="True" Text=" "></asp:checkbox></td>
										</tr>
										<tr>
											<td align="left" width="40%"><asp:label id="LabelPayroll" runat="server" CssClass="Label_Small"> PayRoll</asp:label></td>
											<td align="left"><asp:checkbox id="CheckBoxPayroll" runat="server" CssClass="CheckBox_Normal" Checked="True" Text=" "></asp:checkbox></td>
										</tr>
										<tr>
											<td align="left"></td>
											<td align="left"></td>
										</tr>
										<tr>
											<td align="left"></td>
											<td align="left"></td>
										</tr>
										<tr>
											<td align="left"></td>
											<td align="left"></td>
										</tr>
										<tr>
											<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonStartProcess" CssClass="Button_Normal" Text="Start Process" Runat="server"
													Width="100px"></asp:button></td>
											<td class="Td_ButtonContainer" align="left"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Text="OK" Width="72px"></asp:button></td>
						</td>
					</tr>
				</table>
				<table width="695">
					<tr>
						<td id="prgSt" width="34%">&nbsp;</td>
						<td id="prgTD" width="1%">&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
				</table>
			</div>
			</TD></TR></TBODY></TABLE></DIV>
			<asp:placeholder id="MessagePlaceHolder" runat="server"></asp:placeholder>
			</form>
	</body>
</HTML>
