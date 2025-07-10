<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AccountTypesForm.aspx.vb" Inherits="YMCAUI.AccountTypesForm"%>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" 
					mouseovercssclass="MouseOver" MenuFadeDelay="2" DefaultMouseUpCssClass="mouseup"
					DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown" 
					DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer"
					HighlightTopMenu="False" Layout="Horizontal" ViewStateMode="Disabled" 
					EnableViewState="False">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left" colSpan="2"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Account Types
			</td>
		</tr>
		<tr>
			<td>&nbsp;
			</td>
		</tr>
	</table>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<tr>
				<td>
					<div class="Div_Center">
						<table class="Table_WithoutBorder" width="680">
							<TBODY>
								<tr>
									<td align="left" width="303"><asp:label id="LabelLook" runat="server" CssClass="Label_Small" width="50">
							Look For</asp:label><asp:textbox id="TextBoxFind" runat="server" CssClass="TextBox_Normal" Width="112px"></asp:textbox><asp:button id="ButtonSearch" runat="server" CssClass="Button_Normal" Width="64px" CausesValidation="False"
											Text="Search"></asp:button>&nbsp;&nbsp;</td>
									<asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Small" width="130" visible="false">No Record Found</asp:label>
				</td>
			</tr>
			<tr>
				<td width="303">
					<table width="270">
						<tr>
							<td align="left" valign="top">
								<div style="OVERFLOW: auto; WIDTH: 280px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 300px; BORDER-BOTTOM-STYLE: none">
									<asp:datagrid id="DataGridAccountTypes" allowsorting="True" runat="server" CssClass="DataGrid_Grid"
										Width="260px" AutoGenerateColumns="False">
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
											<asp:BoundColumn DataField="Acct. Type" HeaderText="Acct. Type" 
												SortExpression="Acct. Type"></asp:BoundColumn>
											<asp:BoundColumn DataField="Short Description" HeaderText="Short Description" 
												SortExpression="Short Description"></asp:BoundColumn>
										</Columns>
										<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
									</asp:datagrid></div>
							</td>
						</tr>
					</table>
				</td>
				<td>
					<table class="Table_WithoutBorder" height="280" width="368">
						<tr>
							<td align="left"><asp:label id="LabelAcctType" runat="server" CssClass="Label_Small" width="120">
										Acct Type</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxAcctType" runat="server" CssClass="TextBox_Normal" width="100" MaxLength="2"
									readonly="true"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxAcctType"
									Display="Dynamic" ErrorMessage="Account type cannot be blank">*</asp:requiredfieldvalidator></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelShortDesc" runat="server" CssClass="Label_Small" width="90">
										Short Desc</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxShortDesc" runat="server" CssClass="TextBox_Normal" width="146" MaxLength="20"
									readonly="true"></asp:textbox></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelLongDesc" runat="server" CssClass="Label_Small" width="90">
										Long Desc</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxLongDesc" runat="server" CssClass="TextBox_Normal" Width="215" MaxLength="70"
									readonly="true"></asp:textbox></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelEffDate" runat="server" CssClass="Label_Small" Width="90px">
										Eff Date</asp:label></td>
							<td align="left"><uc1:DateUserControl id="TextBoxEffDate" runat="server"></uc1:DateUserControl></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelTermDate" runat="server" CssClass="Label_Small" Width="90px">
										Term Date</asp:label></td>
							<td align="left"><uc1:DateUserControl id="TextBoxTermDate" runat="server"></uc1:DateUserControl></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelRefund" runat="server" CssClass="Label_Small" Width="100px">
										Refund Priority</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxRefund" runat="server" CssClass="TextBox_Normal" width="146" MaxLength="4"
									readonly="true"></asp:textbox><asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxRefund"
									ErrorMessage="Refund Priority should be numeric" ValidationExpression="[0-9]*">*</asp:regularexpressionvalidator></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelAccountGroup" runat="server" CssClass="Label_Small" Width="100px">
										Group</asp:label></td>
							<td align="left">
								<asp:dropdownlist id="DropDownListAccountGroup" runat="server" CssClass="DropDown_Normal" Width="165px"
									enabled="false"></asp:dropdownlist>
							</td>
						</tr>
						<tr>
							<td align="left">
								<asp:radiobutton id="RadioButtonRetirementPlan" runat="server" 
									Text="  Retirement Plan" GroupName="GrpPlan" Checked="True" enabled="false"></asp:radiobutton>
							</td>
							<td align="left">
								<asp:radiobutton id="RadioButtonSavingsPlans" runat="server" 
									Text="   Savings Plan" Checked="False"
									GroupName="GrpPlan" enabled="false"></asp:radiobutton></td>
						</tr>
						<tr>
							<td align="left"><asp:checkbox id="CheckBoxBasicAcct1" runat="server" CssClass="CheckBox_Normal" width="150" Text="Basic Acct"
									enabled="false"></asp:checkbox></td>
							<td align="left"><asp:checkbox id="CheckBoxEmployerMoney" runat="server" CssClass="CheckBox_Normal" Text="Employer Money"
									enabled="false"></asp:checkbox></td>
						</tr>
						<tr>
							<td align="left"><asp:checkbox id="CheckBoxVestReq" runat="server" CssClass="CheckBox_Normal" Text="Vest Req" enabled="false"></asp:checkbox></td>
							<td align="left"><asp:checkbox id="CheckBoxEmployeeMoney" runat="server" CssClass="CheckBox_Normal" Text="Employee Money"
									enabled="false"></asp:checkbox></td>
						</tr>
						<tr>
							<td align="left"><asp:checkbox id="CheckBoxBasicAcct2" runat="server" CssClass="CheckBox_Normal" Text="Basic Account"
									enabled="false"></asp:checkbox></td>
							<td align="left"><asp:checkbox id="CheckBoxEmpTaxDefer" runat="server" CssClass="CheckBox_Normal" Text="Employee Tax Defer"
									enabled="false"></asp:checkbox></td>
						</tr>
						<tr>
							<td align="left"><asp:checkbox id="CheckBoxLumpSum" runat="server" CssClass="CheckBox_Normal" Text="Lump Sum" enabled="false"></asp:checkbox></td>
							<td align="left"><asp:checkbox id="CheckBoxIncDeathBen" runat="server" CssClass="CheckBox_Normal" Text="Included Death Bene"
									enabled="false"></asp:checkbox></td>
						</tr>
					</table>
					<asp:validationsummary id="ValidationSummary1" runat="server" Width="334px"></asp:validationsummary></td>
			</tr>
			<tr>
				<td width="310"></td>
				<td>
					<table class="Table_WithoutBorder" width="370">
						<tr>
							<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="60px" Text="Save"
									enabled="false"></asp:button></td>
							<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="60px" CausesValidation="False"
									Text="Cancel" enabled="false"></asp:button></td>
							<td class="Td_ButtonContainer" align="center">&nbsp;</td>
							<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Width="60px" CausesValidation="False"
									Text="Add"></asp:button></td>
							<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="60px" CausesValidation="False"
									Text="OK"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	</TD></TR></TBODY></TABLE></DIV>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
	</form>
<!--#include virtual="bottom.html"-->
