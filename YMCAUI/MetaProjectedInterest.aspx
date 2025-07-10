<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MetaProjectedInterest.aspx.vb" Inherits="YMCAUI.MetaProjectedInterest"%>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
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
				Meta Projected Interest
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<tr>
				<td align="left"><asp:label id="LabelLook" runat="server" cssclass="Label_Small" Width="78px">
										Look For</asp:label><asp:textbox id="TextBoxFind" runat="server" Width="110px"></asp:textbox></td>
				<td align="left" width="111"><asp:button id="ButtonSearch" runat="server" CssClass="Button_Normal" CausesValidation="False"
						Text="Search"></asp:button></td>
				<td><asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Small" width="130" visible="false">No Record Found</asp:label></td>
				<td align="left"></td>
			</tr>
			<tr>
				<td align="left">
					<div style="OVERFLOW: auto;WIDTH: 200px;BORDER-TOP-STYLE: none;BORDER-RIGHT-STYLE: none;BORDER-LEFT-STYLE: none;HEIGHT: 200px;BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridProjectedInterest" runat="server" CssClass="DataGrid_Grid" allowsorting="true"
							Width="180px">
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
				<td colSpan="2">
					<table width="375">
						<tr>
							<td align="left"><asp:label id="LabelInterestYear" runat="server" CssClass="Label_Small">Interest Year</asp:label></td>
							<td align="left"><asp:textbox id="TextBoxInterestYear" runat="server" CssClass="TextBox_Normal" Width="88px" readonly="true"></asp:textbox>
								<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="Interest Year cannot be blank"
									ControlToValidate="TextBoxInterestYear">*</asp:RequiredFieldValidator></td>
						</tr>
						<tr>
							<td align="left" height="26"><asp:label id="LabelInterestRate" runat="server" CssClass="Label_Small">Interest 
												Rate</asp:label></td>
							<td align="left" height="26"><asp:textbox id="TextBoxInterestRate" runat="server" CssClass="TextBox_Normal" Width="88px" readonly="true"></asp:textbox>
								<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxInterestRate"
									ErrorMessage="Interest Rate should be numeric" ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelInterestEndDate" runat="server" CssClass="Label_Small">Interest End Date</asp:label></td>
							<td align="left"><uc1:DateUserControl id="TextBoxInterestEndDate" runat="server"></uc1:DateUserControl></td>
						</tr>
						<tr>
							<td align="left"><asp:label id="LabelEffectiveDate" runat="server" CssClass="Label_Small">Effective Date</asp:label></td>
							<td align="left"><uc1:DateUserControl id="TextBoxEffectiveDate" runat="server"></uc1:DateUserControl></td>
						</tr>
					</table>
					<asp:ValidationSummary id="ValidationSummary1" runat="server" Width="358px"></asp:ValidationSummary></td>
			</tr>
			<tr>
				<td align="right">&nbsp;&nbsp;&nbsp;&nbsp;</td>
				<td class="Td_ButtonContainer" align="right" colSpan="2">
					<asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" Width="50px" Text="Save"
						enabled="false" Height="20px"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" Width="50px" Text="Cancel"
						enabled="false" Height="20" CausesValidation="False"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" Width="50px" Text="Delete"
						enabled="false" Height="20px" CausesValidation="False"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" Width="50px" Text="Add" Height="20px"
						CausesValidation="False"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:button id="ButtonOk" runat="server" CssClass="Button_Normal" Width="50px" Text="OK" Height="20px"
						CausesValidation="False"></asp:button></td>
			</tr>
		</table>
	</div>
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
