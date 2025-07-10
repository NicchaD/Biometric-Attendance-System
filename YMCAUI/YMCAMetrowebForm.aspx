<%@ Page Language="vb" AutoEventWireup="false" Codebehind="YMCAMetrowebForm.aspx.vb" Inherits="YMCAUI.YMCAMetrowebForm" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="TopNew.htm"-->
<script language="javascript">
function FormatYMCANo()
{
	var diff;
	var flg = false;
	var str = String(document.Form1.all.TextBoxYMCANo.value);
	var len;
	len =str.length;
	if (len==0)
	{
		return false;
	}
	else if (len<6)
	{
		diff=(6-len);
		for (i=0;i<diff;i++)
		{
			str="0" + str
			
		}
	}
	document.Form1.all.TextBoxYMCANo.value=str		
}
</script>
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700" border="0">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					Select a YMCA
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<tr>
				<td align="left">
					<div style="OVERFLOW: auto; WIDTH: 350px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 200px; BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridYMCAMetro" runat="server" Allowsorting="true" width="340px" cellpadding="1"
							CssClass="DataGrid_Grid">
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
											CommandName="Select" ToolTip="Select"></asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
						</asp:datagrid></div>
				</td>
				<td align="left">
					<table>
						<tr>
							<td align="left"><asp:label id="LabelYMCANo" runat="server" Width="70px" cssclass="Label_Small">YMCA No:</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxYMCANo" runat="server" width="100" CssClass="TextBox_Normal"></asp:textbox></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelName" runat="server" Width="70px" cssclass="Label_Small">Name:</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxName" runat="server" width="100" CssClass="TextBox_Normal"></asp:textbox></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelCity" runat="server" Width="70px" cssclass="Label_Small">City:</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxCity" runat="server" width="100" CssClass="TextBox_Normal"></asp:textbox></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelState" runat="server" Width="70px" cssclass="Label_Small">State:</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxState" runat="server" width="100" CssClass="TextBox_Normal"></asp:textbox></td>
						</tr>
						<tr>
							<td align="left"><asp:button id="ButtonFind" CssClass="Button_Normal" Width="80" Runat="server" Text="Find"></asp:button></td>
							<td align="left"><asp:button id="ButtonClear" CssClass="Button_Normal" Width="80" Runat="server" Text="Clear"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td class="Td_ButtonContainer"></td>
				<td class="Td_ButtonContainer" align="left">
					<table>
						<tr>
							<td class="Td_ButtonContainer" align="left"></td>
							<td class="Td_ButtonContainer" align="left"><asp:button id="ButtonCancel" CssClass="Button_Normal" Width="80" Runat="server" Text="Cancel"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
