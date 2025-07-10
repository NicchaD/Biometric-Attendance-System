<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MetaVestingTypeCode.aspx.vb" Inherits="YMCAUI.MetaVestingTypeCode"%>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table cellSpacing="0" cellPadding="0" width="700" class="Table_WithoutBorder">
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
				Meta Vesting Type Code
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
								<td align="left" width="83"><asp:label id="LabelLook" runat="server" CssClass="Label_Small" Width="78px">
										Look For</asp:label></td>
								<td align="left" width="111"><asp:textbox id="TextBoxFind" runat="server" CssClass="TextBox_Normal" Width="112px"></asp:textbox></td>
								<td align="left"><asp:button id="ButtonSearch" runat="server" CssClass="Button_Normal" Width="64px" Text="Search"
										CausesValidation="False"></asp:button>
									<asp:Label id="LabelNoRecordFound" runat="server" Width="128px" Visible="False">No Record Found</asp:Label></td>
							</tr>
						</table>
					</div>
					<br>
					<div class="Div_Center">
						<table width="680" class="Table_WithoutBorder">
							<tr>
								<td align="left">
									<div style="OVERFLOW: auto; WIDTH: 270px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 230px; BORDER-BOTTOM-STYLE: none">
										<asp:datagrid id="DataGridMetaVestingType" runat="server" Width="250px" CssClass="DataGrid_Grid"
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
										</asp:datagrid>
									</div>
								</td>
								<td>
									<table width="380" class="Table_WithoutBorder">
										<tr>
											<td align="left"><asp:label id="LabelVestingTypeCode" runat="server" CssClass="Label_Small" width="60">
													Vesting Type Code</asp:label>
											</td>
											<td align="left"><asp:textbox id="TextBoxVestingTypeCode" runat="server" CssClass="TextBox_Normal" width="143"
													ReadOnly="True" MaxLength="2"></asp:textbox>
												<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxVestingTypeCode"
													ErrorMessage="Vesting Type Cannot be blank">*</asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelShortDesc" runat="server" CssClass="Label_Small" width="60">
													Short Desc</asp:label>
											</td>
											<td align="left"><asp:textbox id="TextBoxShortDesc" runat="server" CssClass="TextBox_Normal" width="143" ReadOnly="True"
													MaxLength="20"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelDesc" runat="server" CssClass="Label_Small" width="60">
													Desc</asp:label>
											</td>
											<td align="left"><asp:textbox id="TextBoxDesc" runat="server" CssClass="TextBox_Normal" width="143" ReadOnly="True"
													MaxLength="100"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelEligMonth" runat="server" CssClass="Label_Small" width="60">
													Elig Month</asp:label>
											</td>
											<td align="left"><asp:textbox id="TextBoxEligMonth" runat="server" CssClass="TextBox_Normal" width="143" ReadOnly="True"
													MaxLength="5"></asp:textbox>
												<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Elig Month should be numeric"
													ControlToValidate="TextBoxEligMonth" ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelVestedMonth" runat="server" CssClass="Label_Small" width="60">
													Vested Month</asp:label>
											</td>
											<td align="left"><asp:textbox id="TextBoxVestedMonth" runat="server" CssClass="TextBox_Normal" width="143" ReadOnly="True"></asp:textbox>
												<asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ErrorMessage="Vested Month should be numeric"
													ControlToValidate="TextBoxVestedMonth" ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator></td>
										</tr>
									</table>
									<asp:ValidationSummary id="ValidationSummary1" runat="server" Width="336px"></asp:ValidationSummary>
								</td>
							</tr>
							<tr>
								<td>
								</td>
								<td>
									<table width="380" class="Table_WithoutBorder">
										<tr>
											<td class="Td_ButtonContainer">
												<asp:Button id="ButtonSave" runat="server" Text="Save" Width="50px" CssClass="Button_Normal"
													Enabled="False"></asp:Button></td>
											<td class="Td_ButtonContainer">
												<asp:Button id="ButtonCancel" runat="server" Text="Cancel" Width="50px" CssClass="Button_Normal"
													CausesValidation="False" Enabled="False"></asp:Button></td>
											<td class="Td_ButtonContainer">
												<asp:Button id="ButtonDelete" runat="server" Text="Delete" Width="50px" CssClass="Button_Normal"
													enabled="false" CausesValidation="False"></asp:Button></td>
											<td class="Td_ButtonContainer">
												<asp:Button id="ButtonAdd" runat="server" Text="Add" Width="50px" CssClass="Button_Normal" CausesValidation="False"></asp:Button></td>
											<td class="Td_ButtonContainer">
												<asp:Button id="ButtonOK" runat="server" Text="OK" Width="50px" CssClass="Button_Normal" CausesValidation="False"></asp:Button></td>
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
	<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
