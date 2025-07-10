<%@ Page Language="vb" AutoEventWireup="false" EnableEventValidation="false" Codebehind="MetaAnnuityTypesMain.aspx.vb" Inherits="YMCAUI.MetaAnnuityTypesMain"%>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2" DefaultMouseUpCssClass="mouseup"
					DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown" DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer"
					HighlightTopMenu="False" Layout="Horizontal">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Meta Annuity Types Maintenance
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<table class="Table_WithBorder" width="700">
		<tr>
			<td>
				<div class="Div_Center">
					<table class="Table_WithoutBorder" width="680">
						<tr>
							<td></td>
							<td align="left" width="83"><asp:label id="LabelLook" runat="server" CssClass="Label_Small" Width="78px">
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
							<td width="290">
								<table width="280">
									<tr>
										<td>
											<div style="OVERFLOW: auto; WIDTH: 280px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 500px; BORDER-BOTTOM-STYLE: none">
                                            <asp:datagrid id="DataGridMetaAnnuityMain" runat="server" CssClass="DataGrid_Grid" Width="248px" allowsorting="true">
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
							<td align="left">
								<table class="Table_WithoutBorder" width="380">
									<tr vAlign="top">
										<td align="left"><asp:label id="LabelAnnuityType" runat="server" CssClass="Label_Small" width="100px">
										Annuity Type</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxAnnuityType" runat="server" CssClass="TextBox_Normal" width="146" MaxLength="4"
												readonly="true"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxAnnuityType"
												ErrorMessage="Annuity Type cannot be blank">*</asp:requiredfieldvalidator></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelAnnuityBaseType" runat="server" CssClass="Label_Small" width="100">
										Annuity Base Type</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxAnnuityBaseType" runat="server" CssClass="TextBox_Normal" width="146"
												MaxLength="4" readonly="true"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelAnnCatCode" runat="server" CssClass="Label_Small" width="100">
										Annuity Category Code</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxAnnCatCode" runat="server" CssClass="TextBox_Normal" width="146" MaxLength="6"
												readonly="true"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelShortDesc" runat="server" CssClass="Label_Small" Width="100">
										Short Description</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxShortDesc" runat="server" CssClass="TextBox_Normal" width="146" MaxLength="20"
												readonly="true"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelDesc" runat="server" CssClass="Label_Small" Width="100">
										Description</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxDesc" runat="server" CssClass="TextBox_Normal" Width="296px" MaxLength="100"
												readonly="true"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelCodeOrder" runat="server" CssClass="Label_Small" Width="100">
										Code Order</asp:label></td>
										<td align="left"><asp:textbox id="TextBoxCodeOrder" runat="server" CssClass="TextBox_Normal" width="146" MaxLength="2"
												readonly="true"></asp:textbox><asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxCodeOrder"
												ErrorMessage="Code order should be numeric" ValidationExpression="-[0-9]*|[0-9]*">*</asp:regularexpressionvalidator></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelEffDate" runat="server" CssClass="Label_Small" Width="100">
										Eff Date</asp:label></td>
										<td align="left"><uc1:dateusercontrol id="TextBoxEffDate" runat="server"></uc1:dateusercontrol></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelTerminDate" runat="server" CssClass="Label_Small" Width="100">
										Termination Date</asp:label></td>
										<td align="left"><uc1:dateusercontrol id="TextBoxTerminDate" runat="server"></uc1:dateusercontrol></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelJointPctg" runat="server" CssClass="Label_Small" width="100">
										Joint Survivor pctg</asp:label></td>
										<td align="left">
											<table class="Table_WithoutBorder" width="295">
												<tr>
													<td align="left" width="122"><asp:textbox id="TextBoxJointPctg" runat="server" CssClass="TextBox_Normal" width="104px" MaxLength="8"
															readonly="true"></asp:textbox><asp:regularexpressionvalidator id="RegularExpressionValidator2" runat="server" ControlToValidate="TextBoxJointPctg"
															ErrorMessage="Joint Survivor pctg should be numeric" ValidationExpression="-[0-9]*.[0-9]*|[0-9]*.[0-9]*">*</asp:regularexpressionvalidator></td>
													<td align="left"><asp:label id="LabelIcrease" runat="server" CssClass="Label_Small" Width="50">
													Increase pctg</asp:label></td>
													<td align="left"><asp:textbox id="TextBoxIcrease" runat="server" CssClass="TextBox_Normal" Width="43" MaxLength="2"
															readonly="true"></asp:textbox><asp:regularexpressionvalidator id="RegularExpressionValidator3" runat="server" ControlToValidate="TextBoxIcrease"
															ErrorMessage="Increase Pctg should be numeric" ValidationExpression="[0-9]*.[0-9]*">*</asp:regularexpressionvalidator></td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td align="left" colspan="2">
											<table class="Table_WithoutBorder" width="295">
												<tr>
													<td align="left" width="50%"><asp:checkbox id="CheckBoxIncreasing" runat="server" CssClass="CheckBox_Normal" Text="Increasing"
															enabled="false"></asp:checkbox></td>
													<td align="left" width="50%"><asp:checkbox id="CheckboxPopup" runat="server" CssClass="CheckBox_Normal" Text="Popup" enabled="false"></asp:checkbox></td>
												</tr>
											</table>
										</td>
									</tr>
									<TR>
										<td align="left" colspan="2">
											<table class="Table_WithoutBorder" width="295">
												<tr>
													<td align="left" width="50%"><asp:checkbox id="CheckboxLastToDie" runat="server" CssClass="CheckBox_Normal" Text="Last To Die"
															enabled="false"></asp:checkbox></td>
													<td align="left" width="50%"><asp:checkbox id="CheckboxSsLevelling" runat="server" CssClass="CheckBox_Normal" Text="SsLevelling"
															enabled="false"></asp:checkbox></td>
												</tr>
											</table>
										</td>
									</TR>
									<tr>
										<td align="left" colspan="2">
											<table class="Table_WithoutBorder" width="295">
												<tr>
													<td align="left" width="50%"><asp:checkbox id="CheckboxJointSurv" runat="server" CssClass="CheckBox_Normal" Text="Joint Survivor"
															enabled="false"></asp:checkbox></td>
													<td align="left" width="50%"><asp:checkbox id="CheckboxInsReserve" runat="server" CssClass="CheckBox_Normal" Text="Ins Reserve"
															enabled="false"></asp:checkbox></td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
								<asp:validationsummary id="ValidationSummary1" runat="server"></asp:validationsummary></td>
						</tr>
						<tr>
							<td width="360"></td>
							<td>
								<table class="Table_WithoutBorder" cellSpacing="0" width="380">
									<tr>
										<td class="Td_ButtonContainer"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="64px" Text="Save"
												enabled="false"></asp:button></td>
										<td class="Td_ButtonContainer"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="64px" CausesValidation="False"
												Text="Cancel" enabled="false"></asp:button></td>
										<td class="Td_ButtonContainer"><asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" Width="64px" CausesValidation="False"
												Text="Delete" enabled="false"></asp:button></td>
										<td class="Td_ButtonContainer"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Width="64px" CausesValidation="False"
												Text="Add"></asp:button></td>
										<td class="Td_ButtonContainer"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="64px" CausesValidation="False"
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
	<DIV></DIV>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
