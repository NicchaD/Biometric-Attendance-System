<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MetaConfigMaintenance.aspx.vb" Inherits="YMCAUI.MetaConfigMaintenance"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
					CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
					MenuFadeDelay="2" mouseovercssclass="MouseOver">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Meta Configuration Maintenance
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<tr>
				<td>
					<div class="Div_Center">
						<table class="Table_WithoutBorder" width="680">
							<tr>
								<td align="right" width="83"><asp:label id="LabelLook" runat="server" CssClass="Label_Small" Width="78px">
										Look For</asp:label></td>
								<td align="left" width="111"><asp:textbox id="TextBoxFind" runat="server" CssClass="TextBox_Normal" Width="112px"></asp:textbox></td>
								<td><asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Small" visible="false" width="130">No Record Found</asp:label></td>
								<td align="left"><asp:button id="ButtonSearch" runat="server" CssClass="Button_Normal" Width="64px" CausesValidation="False"
										Text="Search"></asp:button></td>
							</tr>
						</table>
					</div>
					<div class="Div_Center">
						<table class="Table_WithoutBorder" width="680">
							<tr>
								<td>
									<div style="OVERFLOW: auto; WIDTH: 320px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 250px; BORDER-BOTTOM-STYLE: none"
										align="left"><asp:datagrid id="DataGridMetaConfig" runat="server" CssClass="DataGrid_Grid" width="300px" allowsorting="true">
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
								<td>
									<table class="Table_WithoutBorder" height="240" width="296">
										<tr>
											<td align="left"><asp:label id="LabelKey" runat="server" CssClass="Label_Small" width="60">
													Key</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxKey" runat="server" CssClass="TextBox_Normal" width="191px" readonly="true"
													MaxLength="50"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxKey" ErrorMessage="Key cannot be blank">*</asp:requiredfieldvalidator></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelCatCode" runat="server" CssClass="Label_Small" width="60">
													Cat Code</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxCatCode" runat="server" CssClass="TextBox_Normal" width="192px" readonly="true"
													MaxLength="6"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelDataType" runat="server" CssClass="Label_Small" width="60">
													Data Type</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxDataType" runat="server" CssClass="TextBox_Normal" width="192px" readonly="true"
													MaxLength="1"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelValue" runat="server" CssClass="Label_Small" width="60">
													Value</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxValue" runat="server" CssClass="TextBox_Normal" width="192px" readonly="true"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelShortDesc" runat="server" CssClass="Label_Small" width="60">
													Short Desc</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxShortDesc" runat="server" CssClass="TextBox_Normal" width="192px" readonly="true"
													MaxLength="20"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelDesc" runat="server" CssClass="Label_Small" width="60" readonly="true">
													Desc</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxDesc" runat="server" CssClass="TextBox_Normal" width="192px"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelKeywordId" runat="server" CssClass="Label_Small" width="60">
													Keyword ID</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxKeywordId" runat="server" CssClass="TextBox_Normal" width="192px" readonly="true"></asp:textbox></td>
										</tr>
									</table>
									<asp:validationsummary id="ValidationSummary1" runat="server" CssClass="Error_Message"></asp:validationsummary></td>
							</tr>
						</table>
					</div>
				</td>
			</tr>
			<tr>
				<td align="right">
					<table class="Table_WithoutBorder" width="380">
						<tr>
							<td width="600" colSpan="2">&nbsp;</td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="50px" Text="Save"
									enabled="false"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="50px" CausesValidation="False"
									Text="Cancel" enabled="false"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" Width="50px" CausesValidation="False"
									Text="Delete" enabled="false"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Width="50px" CausesValidation="False"
									Text="Add"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Width="50px" CausesValidation="False"
									Text="OK"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></form>
<script language="javascript">
document.title ="YMCA YRS- Configuration Maintenance"
</script>
<!--#include virtual="bottom.html"-->
