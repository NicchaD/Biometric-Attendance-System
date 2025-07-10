<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InterestRatesWebForm.aspx.vb" Inherits="YMCAUI.InterestRatesWebForm"%>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
			<td class="td_backgroundcolorwhite" colSpan="2"></td>
		</tr>
		<tr>
			<td class="td_backgroundcolorwhite" colSpan="2"></td>
		</tr>
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2" DefaultMouseUpCssClass="mouseup"
					DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown" DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer"
					HighlightTopMenu="False" Layout="Horizontal">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
	</table>
	<div class="center">
		<table width="700" border="0">
			<tr>
				<td class="Td_HeadingFormContainer"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					Meta Interest Rates Maintenance
				</td>
			</tr>
		</table>
	</div>
	<table width="700" border="0">
		<tr>
			<td>&nbsp;
			</td>
		</tr>
	</table>
	<div class="center">
		<table class="Table_WithBorder" cellSpacing="0" cellPadding="0" width="700">
			<tr>
				<td align="left"><asp:label id="LabelLook" runat="server" Width="78px" cssclass="Label_Medium">
							Look For</asp:label><asp:textbox id="TextBoxFind" runat="server" CssClass="TextBox_Normal" Width="112px"></asp:textbox><asp:button id="ButtonSearch" CssClass="Button_Normal" Width="50" CausesValidation="False" Text="Search"
						Runat="server"></asp:button><asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Small" visible="false" width="130">No Record Found</asp:label></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
			</tr>
			<tr vAlign="top">
				<td vAlign="top">
					<div style="OVERFLOW: auto; WIDTH: 320px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 300px; BORDER-BOTTOM-STYLE: none">
						<asp:datagrid id="DataGridInterestRates" runat="server" CssClass="DataGrid_Grid" Width="300px"
							allowsorting=true>
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
					<table width="300">
						<TBODY>
							<tr>
								<td align="left"><asp:label id="LabelAcctType" runat="server" Width="100px" cssclass="Label_Small">Accttype:</asp:label></td>
								<td align="left"><asp:textbox id="TextBoxAccountType" runat="server" CssClass="TextBox_Normal" width="100" readonly="true"
										MaxLength="2"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxAccountType"
										ErrorMessage="AcctType cannot be blank">*</asp:requiredfieldvalidator></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelYear" runat="server" Width="100px" cssclass="Label_Small">Year:</asp:label></td>
								<td align="left"><asp:textbox id="TextBoxYear" runat="server" CssClass="TextBox_Normal" width="50" readonly="true"
										MaxLength="4"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelShortDescription" runat="server" Width="100px" cssclass="Label_Small">Short Desc:</asp:label></td>
								<td align="left"><asp:textbox id="TextBoxShortDescription" runat="server" CssClass="TextBox_Normal" width="150"
										readonly="true" MaxLength="20"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left" height="21"><asp:label id="LabelDescription" runat="server" Width="100px" cssclass="Label_Small">Desc:</asp:label></td>
								<td align="left" height="21"><asp:textbox id="TextBoxDescription" runat="server" CssClass="TextBox_Normal" width="200" readonly="true"
										MaxLength="100"></asp:textbox></td>
							</tr>
							<tr>
								<td align="left"><asp:label id="LabelInterestRate" runat="server" Width="100px" cssclass="Label_Small">Interest Rate:</asp:label></td>
								<td align="left"><asp:textbox id="TextBoxInterestRate" runat="server" CssClass="TextBox_Normal" width="150" readonly="true"
										MaxLength="9"></asp:textbox><asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxInterestRate"
										ErrorMessage="Interest Rate should be numeric" ValidationExpression="[0-9]*.[0-9]*">*</asp:regularexpressionvalidator></td>
							</tr>
							<tr>
								<td colSpan="2">&nbsp;
									<asp:validationsummary id="ValidationSummary1" runat="server" Width="278px"></asp:validationsummary></td>
							</tr>
							<tr>
								<td>&nbsp;</td>
							</tr>
							<tr>
								<td>&nbsp;</td>
							</tr>
						</TBODY></table>
					<table cellSpacing="0">
						<tr vAlign="bottom">
							<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonSave" CssClass="Button_Normal" Width="50" Text="Save" Runat="server" enabled="false"></asp:button></td>
							<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" CssClass="Button_Normal" Width="50" CausesValidation="False" Text="Cancel"
									Runat="server" enabled="false"></asp:button></td>
							<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonDelete" CssClass="Button_Normal" Width="50" CausesValidation="False" Text="Delete"
									Runat="server" enabled="false"></asp:button></td>
							<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonAdd" CssClass="Button_Normal" Width="50" CausesValidation="False" Text="Add"
									Runat="server"></asp:button></td>
							<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Width="50" CausesValidation="False"
									Text="OK"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
