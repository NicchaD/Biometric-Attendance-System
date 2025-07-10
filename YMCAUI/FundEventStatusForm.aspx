<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FundEventStatusForm.aspx.vb" Inherits="YMCAUI.FundEventStatusForm"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" width="700">
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
				Fund Event Status
			</td>
		</tr>
		<tr>
			<td>&nbsp;
			</td>
		</tr>
	</table>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="698">
			<tr>
				<td>
					<table class="Table_WithOutBorder" width="690">
						<tr>
							<td align="left" colSpan="2">
								<table class="Table_WithOutBorder" width="690">
									<tr>
										<td align="left"><asp:label id="LabelLookFor" CssClass="Label_Large" Runat="server">Look For</asp:label><asp:textbox id="TextBoxLookUp" CssClass="TextBox_Normal" Runat="server"></asp:textbox>&nbsp;
											<asp:button id="ButtonSearch" CssClass="Button_Normal" Runat="Server" Text="Search" CausesValidation="False"></asp:button><asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Small" width="130" visible="false">No Record Found</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td>
								<div style="OVERFLOW: auto; WIDTH: 350px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 200px; BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridFundStatus" CssClass="DataGrid_Grid" Runat="server" Width="330px" allowsorting="true">
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
									<tr>
										<td align="left"><asp:label id="LabelStatusType" CssClass="Label_Small" Runat="server" text="Status Type"></asp:label></td>
										<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxFundStatusType" runat="server" CssClass="TextBox_Normal" Width="88px"
												readonly="true" MaxLength="2"></asp:textbox>
											<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Status Type cannot be blank"
												ControlToValidate="TextBoxFundStatusType">*</asp:RequiredFieldValidator></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelShortDesc" CssClass="Label_Small" Runat="server" text="Short Desc"></asp:label></td>
										<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxShortDesc" runat="server" CssClass="TextBox_Normal" Width="136px" readonly="true"
												MaxLength="20"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelDesc" CssClass="Label_Small" Runat="server" text="Desc"></asp:label></td>
										<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxDesc" runat="server" CssClass="TextBox_Normal" Width="197px" readonly="true"
												MaxLength="100"></asp:textbox></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelCanChange" CssClass="Label_Small" Runat="server" text="Can Change"></asp:label></td>
										<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxCanChange" runat="server" CssClass="TextBox_Normal" Width="34px" readonly="true"
												MaxLength="1"></asp:textbox>
											<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxCanChange"
												ErrorMessage="Enter T/F in Can Change" ValidationExpression="T|F">*</asp:RegularExpressionValidator></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelInterest" CssClass="Label_Small" Runat="server" text="Interest"></asp:label></td>
										<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxInterest" runat="server" CssClass="TextBox_Normal" Width="34px" readonly="true"
												MaxLength="1"></asp:textbox>
											<asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ControlToValidate="TextBoxInterest"
												ErrorMessage="Enter T/F in Interest" ValidationExpression="T|F">*</asp:RegularExpressionValidator></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelDeposits" CssClass="Label_Small" Runat="server" text="Deposits"></asp:label></td>
										<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxDeposits" runat="server" CssClass="TextBox_Normal" Width="34px" readonly="true"
												MaxLength="1"></asp:textbox>
											<asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" ControlToValidate="TextBoxDeposits"
												ErrorMessage="Enter T/F in Deposits" ValidationExpression="T|F">*</asp:RegularExpressionValidator></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelService" CssClass="Label_Small" Runat="server" text="Service"></asp:label></td>
										<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxService" runat="server" CssClass="TextBox_Normal" Width="34px" readonly="true"
												MaxLength="1"></asp:textbox>
											<asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server" ControlToValidate="TextBoxService"
												ErrorMessage="Enetr T/F in Service" ValidationExpression="T|F">*</asp:RegularExpressionValidator></td>
									</tr>
									<tr>
										<td align="left"><asp:label id="LabelAutoTerm" CssClass="Label_Small" Runat="server" text="Auto Term"></asp:label></td>
										<td align="left">&nbsp;&nbsp;<asp:textbox id="TextBoxAutoTerm" runat="server" CssClass="TextBox_Normal" Width="34px" readonly="true"
												MaxLength="1"></asp:textbox>
											<asp:RegularExpressionValidator id="RegularExpressionValidator5" runat="server" ControlToValidate="TextBoxAutoTerm"
												ErrorMessage="Enter T/F in Auto Term" ValidationExpression="T|F">*</asp:RegularExpressionValidator></td>
									</tr>
								</table>
								<asp:ValidationSummary id="ValidationSummary1" runat="server" Width="288px"></asp:ValidationSummary></td>
						</tr>
					</table>
					</td>
			</tr>
			<tr>
				<td align="right"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Text="Save" Width="66px"
						enabled="false"></asp:button>&nbsp;
					<asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Text="Cancel" Width="66px"
						enabled="false" CausesValidation="False"></asp:button>&nbsp;
					<asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" Text="Delete" Width="66px"
						enabled="false" CausesValidation="False"></asp:button>&nbsp;
					<asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Text="Add" Width="66px" CausesValidation="False"></asp:button>&nbsp;
					<asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Text="OK" Width="66px" CausesValidation="False"></asp:button></td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
