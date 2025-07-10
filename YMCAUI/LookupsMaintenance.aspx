<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LookupsMaintenance.aspx.vb" Inherits="YMCAUI.LookupsMaintenance"%>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="top.html"-->
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
				Lookups Maintenance
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
					<div>
						<table class="Table_WithoutBorder" width="680">
							<tr>
								<td align="left" width="83"><asp:label id="LabelLook" runat="server" CssClass="Label_Small" Width="66px">
										Look For</asp:label></td>
								<td align="left" width="111"><asp:textbox id="TextBoxFind" runat="server" CssClass="TextBox_Normal" Width="112px"></asp:textbox></td>
								<td align="left"><asp:button id="ButtonSearch" runat="server" CssClass="Button_Normal" Width="64px" Text="Search"
										CausesValidation="False"></asp:button>&nbsp;
									<asp:label id="LabelNoRecordFound" runat="server" Width="128px" Visible="False">No Records Found</asp:label></td>
							</tr>
						</table>
					</div>
					<div class="Div_Center">
						<table class="Table_WithoutBorder" width="680">
							<tr>
								<td align="left" valign="top">
									<table width="280">
										<tr>
											<td align="left" valign="top">
												<div style="OVERFLOW: auto; WIDTH: 270px; HEIGHT: 310px" DESIGNTIMEDRAGDROP="33"><asp:datagrid id="DataGridLookupMaintenance" runat="server" CssClass="DataGrid_Grid" Width="248px"
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
										</tr>
									</table>
								</td>
								<td>
									<table class="Table_WithoutBorder" width="380">
										<TBODY>
											<tr>
												<td align="left"><asp:label id="LabelGroup" runat="server" CssClass="Label_Small" width="100">
													Group</asp:label></td>
												<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxGroup" runat="server" CssClass="TextBox_Normal" width="136px" ReadOnly="True"
														MaxLength="50"></asp:textbox>
													<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Group cannot be blank"
														ControlToValidate="TextBoxGroup">*</asp:requiredfieldvalidator></td>
											</tr>
											<tr>
												<td align="left"><asp:label id="LabelSubGroup1" runat="server" CssClass="Label_Small" Width="100">
													Subgroup1</asp:label></td>
												<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxSubGroup1" runat="server" CssClass="TextBox_Normal" width="136px" ReadOnly="True"
														MaxLength="10"></asp:textbox></td>
											</tr>
											<tr>
												<td align="left"><asp:label id="LabelSubGroup2" runat="server" CssClass="Label_Small" Width="100">
													Subgroup2</asp:label></td>
												<td>
													<table class="Table_WithoutBorder" width="280">
														<tr>
															<td align="left"><asp:textbox id="TextBoxSubGroup2" runat="server" CssClass="TextBox_Normal" width="136px" ReadOnly="True"
																	MaxLength="10"></asp:textbox></td>
															<td align="left"><asp:label id="LabelActive" runat="server" CssClass="Label_Small">
																Active</asp:label></td>
															<td align="left"><asp:textbox id="TextBoxActive" runat="server" CssClass="TextBox_Normal" Width="16px" ReadOnly="True"
																	MaxLength="1"></asp:textbox></td>
														</tr>
													</table>
												</td>
								</td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelSubGroup3" runat="server" CssClass="Label_Small" Width="100">
													Subgroup3</asp:label></td>
								<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxSubGroup3" runat="server" CssClass="TextBox_Normal" width="136px" ReadOnly="True"
										MaxLength="10"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelCodeOrder" runat="server" CssClass="Label_Small" Width="100">
													Code Order</asp:label></td>
								<td>
									<table class="Table_WithoutBorder" width="280">
										<tr>
											<td align="left"><asp:textbox id="TextBoxCodeOrder" runat="server" CssClass="TextBox_Normal" width="136px" ReadOnly="True"
													MaxLength="9"></asp:textbox><asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Code order should be a number"
													ControlToValidate="TextBoxCodeOrder" ValidationExpression="[0-9]*">*</asp:regularexpressionvalidator></td>
											<td align="left"><asp:label id="LabelEditable" runat="server" CssClass="Label_Small">
																Editable</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxEditable" runat="server" CssClass="TextBox_Normal" Width="16px" MaxLength="1"></asp:textbox></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelCodeValue" runat="server" CssClass="Label_Small" Width="100">
													Code Value</asp:label></td>
								<td align="left">&nbsp;
									<asp:textbox id="TextBoxCodeValue" runat="server" CssClass="TextBox_Normal" width="136px" ReadOnly="True"
										MaxLength="6"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ErrorMessage="Code value Cannot be blank"
										ControlToValidate="TextBoxCodeValue">*</asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelShortDesc" runat="server" CssClass="Label_Small" Width="88px">
													Short Desc</asp:label></td>
								<td align="left" height="32">&nbsp;&nbsp;<asp:textbox id="TextBoxShortDesc" runat="server" CssClass="TextBox_Normal" width="136px" ReadOnly="True"
										MaxLength="20"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelDesc" runat="server" CssClass="Label_Small" Width="64px">
													Desc</asp:label></td>
								<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxDesc" runat="server" CssClass="TextBox_Normal" width="136px" ReadOnly="True"
										MaxLength="400"></asp:textbox></td> <%--// PK | 03-13-2019 |YRS-AT-4362 | Increased maxLength from 70 to 400--%>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelEffDate" runat="server" CssClass="Label_Small" Width="64px">
													Eff Date</asp:label></td>
								<td align="left" height="26">&nbsp;&nbsp;<uc1:DateUserControl id="TextBoxEffDate" runat="server"></uc1:DateUserControl></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelUniqueId" runat="server" CssClass="Label_Small" Width="64px">
													Unique Id</asp:label></td>
								<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxUniqueId" runat="server" CssClass="TextBox_Normal" width="136px" ReadOnly="True"></asp:textbox></td>
							</tr>
						</table>
						<asp:validationsummary id="ValidationSummary1" runat="server"></asp:validationsummary>
				</td>
			</tr>
			<tr>
				<td></td>
				<td>
					<table class="Table_WithoutBorder" width="390">
						<tr>
							<td class="Td_ButtonContainer"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="50px" Text="Save"
									Enabled="False"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="50px" Text="Cancel"
									CausesValidation="False" Enabled="False"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" Width="50px" Text="Delete"
									CausesValidation="False" enabled="false"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Width="50px" Text="Add" CausesValidation="False"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="50px" Text="OK" CausesValidation="False"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"--> </TD></TR></TBODY></TABLE></DIV>
