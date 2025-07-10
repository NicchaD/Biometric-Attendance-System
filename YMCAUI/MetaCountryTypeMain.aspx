<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MetaCountryTypeMain.aspx.vb" Inherits="YMCAUI.MetaCountryTypeMain"%>
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
			<td class="Td_HeadingFormContainer" align="left" height="19"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Meta Country Types Maintenance
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<div class="Div_Center">
		<table class="Table_WithBorder" height="415" width="696">
			<tr vAlign="top">
				<td align="left">
					<div>
						<table class="Table_WithOutBorder" width="680">
							<tr>
								<td align="left"><asp:label id="LabelLook" runat="server" CssClass="Label_Small" Width="278px">Look For</asp:label>&nbsp;<asp:textbox id="TextBoxFind" runat="server" CssClass="TextBox_Normal" Width="112px"></asp:textbox>&nbsp;
									<asp:button id="ButtonSearch" runat="server" CssClass="Button_Normal" CausesValidation="False"
										text="Search" width="80px"></asp:button><asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Small" Width="117px" visible="false">No Record found</asp:label></td>
							</tr>
						</table>
					</div>
					<div class="Div_Center">
						<table class="Table_WithoutBorder" height="341" width="680">
							<tr>
								<td>
									<div style="OVERFLOW: auto; WIDTH: 300px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 250px; BORDER-BOTTOM-STYLE: none"
										DESIGNTIMEDRAGDROP="28"><asp:datagrid id="DataGridMetaCountry" runat="server" CssClass="DataGrid_Grid" width="280px" allowsorting="true">
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
									<table class="Table_WithoutBorder" height="206" width="312">
										<tr>
											<td align="left"><asp:label id="LabelAbbreviation" runat="server" CssClass="Label_Small" width="60">
										Abbreviation</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxAbbreviation" runat="server" CssClass="TextBox_Normal" width="146" readonly="true"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxAbbreviation"
													ErrorMessage="Abbreviation cannot be blank">*</asp:requiredfieldvalidator></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelCodeValue" runat="server" CssClass="Label_Small" width="60">
										Code Value</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxCodeValue" runat="server" CssClass="TextBox_Normal" width="146" readonly="true"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelDesc" runat="server" CssClass="Label_Small" width="60">
										Desc</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxDesc" runat="server" CssClass="TextBox_Normal" width="146" readonly="true"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelActive" runat="server" CssClass="Label_Small" width="60">
										Active</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxActive" runat="server" CssClass="TextBox_Normal" width="146" readonly="true"
													MaxLength="1"></asp:textbox><asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxActive"
													ErrorMessage="Enter T/F" ValidationExpression="T|F">*</asp:regularexpressionvalidator></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelEditable" runat="server" CssClass="Label_Small" width="60">
										Editable</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxEditable" runat="server" CssClass="TextBox_Normal" width="146" readonly="true"
													MaxLength="1"></asp:textbox><asp:regularexpressionvalidator id="RegularExpressionValidator2" runat="server" ControlToValidate="TextBoxEditable"
													ErrorMessage="Enter T/F" ValidationExpression="T|F">*</asp:regularexpressionvalidator></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelEffDate" runat="server" CssClass="Label_Small" width="60">
										Eff Date</asp:label></td>
											<td align="left"><uc1:dateusercontrol id="TextBoxEffDate" runat="server"></uc1:dateusercontrol></td>
										</tr>
									</table>
									<asp:validationsummary id="ValidationSummary1" runat="server"></asp:validationsummary></td>
							</tr>
							<tr>
								<td width="270"></td>
								<td>
									<table class="Table_WithoutBorder" height="30" width="365">
										<tr>
											<td class="Td_ButtonContainer"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="50px" enabled="false"
													Text="Save"></asp:button></td>
											<td class="Td_ButtonContainer"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="50px" CausesValidation="False"
													enabled="false" Text="Cancel"></asp:button></td>
											<td class="Td_ButtonContainer"><asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" Width="50px" CausesValidation="False"
													enabled="false" Text="Delete"></asp:button></td>
											<td class="Td_ButtonContainer"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Width="50px" CausesValidation="False"
													Text="Add"></asp:button></td>
											<td class="Td_ButtonContainer"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="50px" CausesValidation="False"
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
