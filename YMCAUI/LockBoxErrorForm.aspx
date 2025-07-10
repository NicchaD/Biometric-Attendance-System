<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LockBoxErrorForm.aspx.vb" Inherits="YMCAUI.LockBoxErrorForm"%>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left" height="11"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					Lock Box Import Errors
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
			</tr>
			<tr>
				<td>
					The Following Errors Were Found In the Imported LockBox File :
				</td>
			</tr>
		</table>
	</div>
	<table width="690" class="Table_WithBorder">
		<tr>
			<td>
				<div style="OVERFLOW: auto; WIDTH: 672px; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 240px; BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridLockBoxImportError" CssClass="DataGrid_Grid" Width="660px" Runat="server"
						Cellpadding="2">
						<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
						<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
					</asp:datagrid>
				</div>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;
			</td>
		</tr>
		<tr>
			<td align="center">
				<asp:Button id="ButtonPrint" runat="server" Text="Print" enabled="false" cssclass="Button_Normal"
					width="80"></asp:Button>&nbsp;&nbsp;
				<asp:Button id="ButtonOK" runat="server" Text="OK" cssclass="Button_Normal" width="80"></asp:Button>
			</td>
		</tr>
	</table>
</form>
<!--#include virtual="bottom.html"-->
