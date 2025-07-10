<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MonthlyExpDividend.aspx.vb" Inherits="YMCAUI.MonthlyExpDividend" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
		<td class="Td_BackGroundColorMenu" align="left">
							<cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
								CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover"
								DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2" mouseovercssclass="MouseOver">
								<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
							</cc1:menu>
						</td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Special Dividends
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<table class="Table_WithBorder" width="700">
		<tr>
			<td align="left"><asp:label id="LabelCheckDate" runat="server" CssClass="Label_Small" width="110">Check Date:</asp:label></td>
			<td align="left">
				<asp:TextBox id="TextboxCheckDate" CssClass="TextBox_Normal" runat="server" Width="146px" ReadOnly="True"></asp:TextBox></td>
		</tr>
		<tr>
			<td align="left"><asp:label id="LabelUSCheckNo" runat="server" CssClass="Label_Small" width="128px">Next US Check No:</asp:label></td>
			<td align="left"><asp:textbox id="TextboxUSCheckNo" runat="server" CssClass="TextBox_Normal" Width="146px"></asp:textbox></td>
		</tr>
		<tr>
			<td align="left"><asp:label id="LabelCanadianCheckNo" runat="server" CssClass="Label_Small" width="192px">Next Canadian Check No:</asp:label></td>
			<td align="left"><asp:textbox id="TextboxCanadianCheckNo" runat="server" CssClass="TextBox_Normal" Width="146px"></asp:textbox></td>
		</tr>
		<tr>
			<td align="left"><asp:label id="LabelProofReport" runat="server" CssClass="Label_Small" width="192px">Proof Report Only</asp:label></td>
			<td align="left"><asp:checkbox id="CheckBoxProofReport" runat="server" text=" " Checked="True"></asp:checkbox></td>
		</tr>
		<tr>
			<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" width="80" Text="Cancel"></asp:button></td>
			<td class="Td_ButtonContainer" align="left"><asp:button id="ButtonRun" runat="server" CssClass="Button_Normal" width="80" Text="Run"></asp:button></td>
		</tr>
	</table>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
