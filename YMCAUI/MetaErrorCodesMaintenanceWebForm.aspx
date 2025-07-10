<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MetaErrorCodesMaintenanceWebForm.aspx.vb" Inherits="YMCAUI.MetaErrorCodesMaintenanceWebForm"%>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
			<tr>
				<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
						CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
						MenuFadeDelay="2" mouseovercssclass="MouseOver">
						<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
					</cc1:menu>
				</td>
			</tr>
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="SpacerImage" height="10" alt="Image" src="images/spacer.gif" width="10">
					Meta Error Codes Maintenance
				</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<tr>
				<td colSpan="2">
					<div class="Div_Center">
						<table class="Table_WithOutBorder" width="680">
							<tr>
								<td align="left">
									<asp:label id="LabelLook" runat="server" CssClass="Label_Small" Width="278px">Look For</asp:label><asp:textbox id="TextBoxFind" runat="server" CssClass="TextBox_Normal" Width="112px"></asp:textbox>&nbsp;
									<asp:button id="ButtonSearch" runat="server" CssClass="Button_Normal" width="80px" text="Search"
										CausesValidation="False"></asp:button><asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Small" Width="117px" visible="false">No Record found</asp:label></td>
							</tr>
						</table>
					</div>
				</td>
			</tr>
			<tr>
				<td class="Table_WithOutBorder">
					<div style="OVERFLOW: auto; WIDTH: 250px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 250px; BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridMetaErrorCodesMaintenance" runat="server" CssClass="DataGrid_Grid" allowsorting="true">
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
				<td class="Table_WithOutBorder">
					<table width="400">
						<tr>
							<td height="25" width="85" align="left"><asp:label id="LabelErrorCode" runat="server" CssClass="Label_Small">
				Error Code</asp:label>
							</td>
							<td height="25" align="left"><asp:textbox id="TextBoxErrorCode" runat="server" CssClass="TextBox_Normal" MaxLength="2" readonly="true"
									Width="120px"></asp:textbox>
								<asp:RequiredFieldValidator id="Mandatory" runat="server" ErrorMessage="Error Code cannot Be blank" ControlToValidate="TextBoxErrorCode">*</asp:RequiredFieldValidator></td>
						</tr>
						<tr>
							<td width="85" align="left"><asp:label id="LabelDescription" runat="server" CssClass="Label_Small">
       		Description</asp:label>
							</td>
							<td><asp:textbox id="TextBoxDescription" runat="server" CssClass="TextBox_Normal" readonly="true"
									MaxLength="100" Width="304px"></asp:textbox></td>
						</tr>
						<tr>
							<td width="85"><asp:label id="LabelCategory" runat="server" CssClass="Label_Small">
							Category
					</asp:label>
							</td>
							<td align="left"><asp:textbox id="TextBoxCategory" runat="server" CssClass="TextBox_Normal" MaxLength="10" readonly="true"></asp:textbox></td>
						</tr>
						<tr>
							<td width="85"><asp:label id="LabelErrorLevel" runat="server" CssClass="Label_Small">
									<b>Error Level</b></asp:label>
							</td>
							<td align="left"><asp:textbox id="TextBoxErrorLevel" runat="server" CssClass="TextBox_Normal" MaxLength="1" readonly="true"></asp:textbox></td>
						</tr>
					</table>
					<asp:ValidationSummary id="ValidationSummary1" runat="server" Width="378px"></asp:ValidationSummary>
				</td>
			</tr>
			<tr>
				<td></td>
				<td>
					<table width="400">
						<tr>
							<td class="Td_ButtonContainer"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="64px" Text="Save"
									enabled="false"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="61px" Text="Cancel"
									enabled="false" CausesValidation="False"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" Width="64px" Text="Delete"
									enabled="false" CausesValidation="False"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Width="64px" Text="Add" CausesValidation="False"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Width="64px" Text="OK" CausesValidation="False"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
