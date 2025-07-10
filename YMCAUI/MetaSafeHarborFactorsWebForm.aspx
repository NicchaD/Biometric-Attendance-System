<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MetaSafeHarborFactorsWebForm.aspx.vb" Inherits="YMCAUI.MetaSafeHarborFactorsWebForm"%>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!--#include virtual="top.html"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table class="Table_WithoutBorder" id="TableMenuContainer" cellSpacing="0" cellPadding="0"
			width="700">
			<tr>
				<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
						CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
						MenuFadeDelay="2" mouseovercssclass="MouseOver">
						<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
					</cc1:menu>
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table class="Table_WithOutBorder" width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="SpacerImage" height="10" alt="Image" src="images/spacer.gif" width="10">
					Meta Safe Harbor Factors
				</td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<tr>
				<td>
					<div style="OVERFLOW: auto; WIDTH: 300px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; POSITION: static; HEIGHT: 250px; BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridMetaSafeHarborFactors" runat="server" width="280px" CSSCLASS="DataGrid_Grid"
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
					<table class="Table_WithoutBorder" width="320">
						<tr>
							<td align="left"><asp:label id="LabelFactorGroup" runat="server" CssClass="Label_Small">
										Factor Group</asp:label>&nbsp;&nbsp;&nbsp;
							</td>
							<td align="left"><asp:textbox id="TextBoxFactorGroup" runat="server" CssClass="TextBox_Normal" readonly="true"></asp:textbox></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelRetireDateLow" runat="server" CssClass="Label_Small">
										 Retire Date Low </asp:label>&nbsp;&nbsp;&nbsp;
							</td>
							<td align="left"><uc1:DateUserControl id="TextBoxRetireDateLow" runat="server"></uc1:DateUserControl></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelRetireDateHigh" runat="server" CssClass="Label_Small">
										 Retire Date High</asp:label>&nbsp;&nbsp;&nbsp;
							</td>
							<td align="left"><uc1:DateUserControl id="TextBoxRetireDateHigh" runat="server"></uc1:DateUserControl></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelAgeLow" runat="server" CssClass="Label_Small">
										Age Low</asp:label>&nbsp;&nbsp;&nbsp;
							</td>
							<td align="left"><asp:textbox id="TextBoxAgeLow" runat="server" CssClass="TextBox_Normal" readonly="true" Width="56px"
									MaxLength="4"></asp:textbox>
								<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Age Low should be numeric"
									ValidationExpression="[0-9]*" ControlToValidate="TextBoxAgeLow">*</asp:RegularExpressionValidator></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelAgeHigh" runat="server" CssClass="Label_Small">
										Age High</asp:label>&nbsp;&nbsp;&nbsp;
							</td>
							<td align="left"><asp:textbox id="TextBoxAgeHigh" runat="server" CssClass="TextBox_Normal" readonly="true" MaxLength="4"></asp:textbox>
								<asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server" ErrorMessage="Age high should be numeric"
									ValidationExpression="[0-9]*" ControlToValidate="TextBoxAgeHigh">*</asp:RegularExpressionValidator></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelFactor" runat="server" CssClass="Label_Small">
										Factor</asp:label>&nbsp;&nbsp;&nbsp;
							</td>
							<td align="left"><asp:textbox id="TextBoxFactor" runat="server" CssClass="TextBox_Normal" readonly="true" MaxLength="4"></asp:textbox>
								<asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server" ErrorMessage="Factor should be numeric"
									ValidationExpression="[0-9]*" ControlToValidate="TextBoxFactor">*</asp:RegularExpressionValidator></td>
						</tr>
						<tr>
							<td>&nbsp;
							</td>
						</tr>
						<tr>
							<td>&nbsp;
							</td>
						</tr>
					</table>
					<asp:ValidationSummary id="ValidationSummary1" runat="server" Width="310px"></asp:ValidationSummary>
				</td>
			</tr>
			<tr>
				<td></td>
				<td>
					<table width="350">
						<tr>
							<td class="Td_ButtonContainer">&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" enabled="false" Width="64px"
									Text="Save"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" enabled="false" Width="61px"
									Text="Cancel" CausesValidation="False"></asp:button></td>
							<td class="Td_ButtonContainer">&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:button id="ButtonEdit" runat="server" CssClass="Button_Normal" enabled="false" Width="64px"
									Text="Edit" CausesValidation="False"></asp:button></td>
							<td class="Td_ButtonContainer"><asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Width="64px" Text="OK" CausesValidation="False"></asp:button></td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
