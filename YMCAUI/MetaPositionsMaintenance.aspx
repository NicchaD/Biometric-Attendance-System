<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MetaPositionsMaintenance.aspx.vb" Inherits="YMCAUI.MetaPositionsMaintenance"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!--#include virtual="top.html"-->
<!--<script type=" text/javascript">
function ConfirmDelete()
{
	alert('aaa');
	var decision = confirm("Are you sure you want to delete this Position Type ?");
	alert(decision);
}
</script>-->
<form id="Form1" method="post" runat="server">
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
			<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Meta Positions Maintenance
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
								<td align="left"><asp:label id="LabelLook" runat="server" CssClass="Label_Small">
										Look For</asp:label><asp:textbox id="TextBoxFind" runat="server" CssClass="TextBox_Normal"></asp:textbox><asp:button id="ButtonSearch" runat="server" CssClass="Button_Normal" CausesValidation="False"
										Width="64px" Text="Search"></asp:button>&nbsp;</td>
								<td align="left">&nbsp;
									<asp:label id="LabelNoRecordFound" runat="server" width="130" CssClass="Label_Small" visible="false">No Record Found</asp:label></td>
							</tr>
						</table>
					</div>
					<br>
					<div class="Div_Center">
						<table class="Table_WithoutBorder" width="680">
							<tr>
								<td>
									<div style="OVERFLOW: auto; WIDTH: 270px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 230px; BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridMetaPosition" runat="server" width="250px" CssClass="DataGrid_Grid"
											allowsorting="true">
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
									<table class="Table_WithoutBorder" width="380">
										<tr>
											<td align="left"><asp:label id="LabelPositionType" runat="server" width="60" CssClass="Label_Small">
													Position Type</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxPositionType" runat="server" width="146" CssClass="TextBox_Normal" readonly="true"
													MaxLength="6"></asp:textbox>
												<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxPositionType"
													ErrorMessage="Position Type cannot be blank">*</asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelShortDesc" runat="server" width="60" CssClass="Label_Small">
													Short Desc</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxShortDesc" runat="server" width="146" CssClass="TextBox_Normal" readonly="true"
													MaxLength="20"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelDesc" runat="server" width="60" CssClass="Label_Small">
													Desc</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxDesc" runat="server" width="146" CssClass="TextBox_Normal" readonly="true"
													MaxLength="100"></asp:textbox></td>
										</tr>
									</table>
									<asp:ValidationSummary id="ValidationSummary1" runat="server" Width="288px"></asp:ValidationSummary></td>
							</tr>
							<tr>
								<td></td>
								<td>
									<table class="Table_WithoutBorder" width="400">
										<tr>
											<td class="Td_ButtonContainer"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="50px" Text="Save"
													Enabled="False"></asp:button></td>
											<td class="Td_ButtonContainer"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" CausesValidation="False"
													Width="50px" Text="Cancel" Enabled="False"></asp:button></td>
											<td class="Td_ButtonContainer"><asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" CausesValidation="False"
													Width="50px" Text="Delete" enabled="false"></asp:button></td>
											<td class="Td_ButtonContainer"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" CausesValidation="False"
													Width="50px" Text="Add"></asp:button></td>
											<td class="Td_ButtonContainer"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" CausesValidation="False" Width="50px"
													Text="OK"></asp:button></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</div>
				</td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
