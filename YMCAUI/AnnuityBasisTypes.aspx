<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AnnuityBasisTypes.aspx.vb" Inherits="YMCAUI.AnnuityBasisTypes" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="700">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="True" ItemPadding="4"
					ItemSpacing="0" Cursor="Pointer" Font-Names="Verdana" width="690" CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown"
					DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup" zIndex="100" MenuFadeDelay="1" mouseovercssclass="MouseOver">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Annuity Basis Types
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
								<td></td>
								<td align="left"><asp:label id="LabelLook" runat="server" CssClass="Label_Small">
										Look For</asp:label>&nbsp;<asp:textbox id="TextBoxFind" runat="server" CssClass="TextBox_Normal" Width="112px"></asp:textbox>&nbsp;<asp:button id="ButtonSearch" runat="server" CssClass="Button_Normal" Width="64px" Text="Search"
										CausesValidation="False"></asp:button></td>
								<td><asp:label id="LabelNoRecordFound" runat="server" width="130" CssClass="Label_Small" visible="false">No Record Found</asp:label></td>
							</tr>
						</table>
					</div>
					<br>
					<div class="Div_Center">
						<table class="Table_WithoutBorder" width="680">
							<tr>
								<td>
									<DIV style="OVERFLOW: auto; WIDTH: 280px; HEIGHT: 200px; TEXT-ALIGN: left"><asp:datagrid id="DataGridAnnuityBasis" runat="server" width="260px" CssClass="DataGrid_Grid"
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
										</asp:datagrid></DIV>
								</td>
								<td>
									<table class="Table_WithoutBorder" width="380">
										<tr>
											<td align="left" height="23"><asp:label id="LabelAnnuityBasis" runat="server" width="130" CssClass="Label_Small">
													Annuity Basis</asp:label></td>
											<td align="left" height="23"><asp:textbox id="TextBoxAnnuityBasis" runat="server" width="100" CssClass="TextBox_Normal" MaxLength="5"
													readonly="true"></asp:textbox>
												<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Annuity Basis Cannot be blank"
													ControlToValidate="TextBoxAnnuityBasis">*</asp:RequiredFieldValidator></td>
										</tr>
										<tr>
											<td align="left" height="21"><asp:label id="LabelShortDescription" runat="server" width="130" CssClass="Label_Small">
													Short Desc</asp:label></td>
											<td align="left" height="21"><asp:textbox id="TextBoxShortDescription" runat="server" width="100" CssClass="TextBox_Normal"
													readonly="true"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelLongDescription" runat="server" width="130" CssClass="Label_Small">
													Long Desc</asp:label></td>
											<td align="left"><asp:textbox id="TextBoxLongDescription" runat="server" CssClass="TextBox_Normal" Width="100"
													readonly="true"></asp:textbox></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelEffectiveDate" runat="server" width="130" CssClass="Label_Small">
													Eff. Date</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
											</td>
											<td align="left"><uc1:DateUserControl id="TextBoxEffectiveDate" runat="server"></uc1:DateUserControl></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelTermDate" runat="server" width="130" CssClass="Label_Small">
													Term Date</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
											</td>
											<td align="left"><uc1:DateUserControl id="TextBoxTermDate" runat="server"></uc1:DateUserControl></td>
										</tr>
										<tr>
											<td align="left"><asp:label id="LabelAnnuityBasisPercentage" runat="server" width="130" CssClass="Label_Small"> 
											Annuity Basis%</asp:label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
											</td>
											<td align="left"><asp:textbox id="TextBoxAnnuityBasisPercentage" runat="server" width="146" CssClass="TextBox_Normal"
													readonly="true"></asp:textbox>
												<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxAnnuityBasisPercentage"
													ErrorMessage="Annuity Basis % should be numeric" ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator></td>
										</tr>
									</table>
									<asp:ValidationSummary id="ValidationSummary1" runat="server" Width="362px"></asp:ValidationSummary>
								</td>
							</tr>
							<tr>
								<td></td>
								<td>
									<table class="Table_WithoutBorder" width="380">
										<tr>
											<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="64px" Text="Save"
													enabled="false"></asp:button></td>
											<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="64px" Text="Cancel"
													enabled="false" CausesValidation="False"></asp:button></td>
											<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Width="64px" Text="Add" CausesValidation="False"></asp:button></td>
											<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Width="64px" Text="OK" CausesValidation="False"></asp:button></td>
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
<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>  <!-- Shilpa N | 02/22/2019/ | YRS-AT-4248 | Added Placeholder to show message box-->
<asp:PlaceHolder id="PlaceHolder2" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
