<%@ Page Language="vb" AutoEventWireup="false" Codebehind="YRSErrorForm.aspx.vb" Inherits="YMCAUI.YRSErrorForm"%>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table class="Table_WithoutBorder" width="700" border="0">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					Error Page<asp:Label id="LabelHdr" runat="server" CssClass="Td_HeadingFormContainer"></asp:Label>
				</td>
			</tr>
		</table>
		<table width="700" border="0" height="370">
			<tr>
				<td colspan="2">&nbsp;</td>
			</tr>
			<tr>
				<td align="center" colspan="2"><asp:label id="LabelDBError" runat="server" Visible="False" Width="200px" cssClass="Label_Medium"></asp:label></td>
			</tr>
			<tr>
				<td colspan="2">&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2" align="center" height="21">
					<asp:LinkButton id="LinkButtonErrorDetails" runat="server" Width="162px" Font-Size="XX-Small" Font-Names="Arial">See Details</asp:LinkButton>&nbsp;&nbsp;&nbsp;<asp:LinkButton id="LinkbuttonHideDetails" runat="server" Width="181px" Font-Size="XX-Small" CausesValidation="False"
						Font-Names="Arial">Hide Details</asp:LinkButton>
				</td>
			</tr>
			<tr>
				<td colspan="2">&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2">&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2" height="220">
					<asp:TextBox id="TextBoxErrorMessage" runat="server" Width="656px" TextMode="MultiLine" ReadOnly="True"
						BackColor="#FFFFC0" Height="200px" Wrap="False"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					&nbsp;
				</td>
			</tr>
			<tr>
				<td colspan="2">
					&nbsp;
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:button id="ButtonHome" runat="server" CssClass="Button_Normal" Width="64px" Text="Home"
						Visible="False"></asp:button>
					<asp:Button id="ButtonClose" runat="server" Text="Close" CssClass="Button_Normal" Width="64px"
						Visible="False"></asp:Button>
				</td>
			</tr>
		</table>
	</div>
</form>
<!--#include virtual="bottom.html"-->
