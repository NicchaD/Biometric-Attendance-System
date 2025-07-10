<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SpecialDividendRates.aspx.vb" Inherits="YMCAUI.SpecialDividendRates"%>
<!--#include virtual="top.html"-->
<script language="javascript">
function ValidatePercentage(sender,args)
{
	var val=document.Form1.all.TextBoxPercentage.value
	
		
		if ((val > 0) && (val <= 100))
		{
			
			args.IsValid = true;
		}
		else
		{	
			args.IsValid = false;
		}
}
	
function ValidateNumeric()
{
	if ((event.keyCode < 48)||(event.keyCode > 57))
	{
		event.returnValue = false;
	}
	if (event.keyCode == 46)
	{
		event.returnValue = true;
	}
}	

</script>
<form id="Form1" method="post" runat="server">
	<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="780">
		<tr>
			<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False" Cursor="Pointer"
					CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown" DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup"
					MenuFadeDelay="2" mouseovercssclass="MouseOver">
					<SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
				</cc1:menu></td>
		</tr>
		<tr>
			<td class="Td_HeadingFormContainer" align="left" colSpan="2" height="19"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
				Special Dividend Rates
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
		</tr>
	</table>
	<table class="Table_WithBorder" width="720" border="0">
		<tr>
			<td width="60%">
				<table class="Table_WithoutBorder" width="400" border="0">
					<tr>
						<asp:label id="LabelNoRecordFound" runat="server" CssClass="Label_Small" visible="false" width="130">No Record Found</asp:label></tr>
					<tr>
						<td>
							<div style="OVERFLOW: auto; WIDTH: 400px; BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; HEIGHT: 200px; BORDER-BOTTOM-STYLE: none"><asp:datagrid id="DataGridSpecialDividendData" runat="server" CssClass="DataGrid_Grid" AutoGenerateColumns="False"
									allowsorting="true">
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
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
										<asp:BoundColumn DataField="UniqueId" SortExpression="UniqueId" HeaderText="UniqueId" Visible="False"></asp:BoundColumn>
										<asp:BoundColumn DataField="PayRollDate" SortExpression="PayrollDate" HeaderText="Payroll Date" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
										<asp:BoundColumn DataField="Percentage" SortExpression="Percentage" HeaderText="Percentage"></asp:BoundColumn>
										<asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status"></asp:BoundColumn>
										<asp:BoundColumn DataField="CompletedOn" SortExpression="CompletedOn" HeaderText="Completed On" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
										<asp:BoundColumn DataField="CompletedBy" SortExpression="CompletedBy" HeaderText="Completed By"></asp:BoundColumn>
									</Columns>
									<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
								</asp:datagrid></div>
						</td>
					</tr>
				</table>
			</td>
			<td width="40%">
				<table class="Table_WithoutBorder" width="300" border="0">
					<tr>
						<td align="left" nowrap><asp:label id="LabelPayRollDate" runat="server" CssClass="Label_Small" width="150px">Payroll Date</asp:label></td>
						<td align="left"><uc1:dateusercontrol id="TextBoxPayRollDate" runat="server" width="200"></uc1:dateusercontrol></td>
					</tr>
					<tr>
						<td align="left" nowrap><asp:label id="LabelPercentage" runat="server" CssClass="Label_Small" width="150px">
								Percentage</asp:label></td>
						<td align="left"><asp:textbox id="TextBoxPercentage" runat="server" CssClass="TextBox_Normal_Amount" width="100"
								readonly="true"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidatorForNumPercentage" runat="server" ControlToValidate="TextBoxPercentage"
								ToolTip="Percentage cannot be blank">*</asp:requiredfieldvalidator><asp:customvalidator id="CustomValidator1" runat="server" ControlToValidate="TextBoxPercentage" ErrorMessage="Percentage cannot be less than 0 and greater than 100"
								ClientValidationFunction="ValidatePercentage"></asp:customvalidator></td>
					</tr>
					<tr>
						<td align="left" nowrap><asp:label id="LabelStatus" runat="server" CssClass="Label_Small">Status</asp:label></td>
						<td align="left"><asp:dropdownlist id="DropdownlistStatus" runat="server" width="105" enabled="false" AutoPostBack="true"
								cssclass="DropDown_Normal">
								<asp:ListItem Value="P"></asp:ListItem>
								<asp:ListItem Value="C"></asp:ListItem>
							</asp:dropdownlist></td>
					</tr>
					<tr>
						<td align="left" nowrap><asp:label id="LabelCompletedOn" runat="server" CssClass="Label_Small" width="150px">
										Completed On</asp:label></td>
						<td align="left"><asp:textbox id="TextboxCompletedOn" runat="server" CssClass="TextBox_Normal" width="100" readonly="true"></asp:textbox></td>
					</tr>
					<tr>
						<td align="left" nowrap><asp:label id="LabelCompletedBy" runat="server" CssClass="Label_Small" width="150px">
								Completed By</asp:label></td>
						<td align="left"><asp:textbox id="TextboxCompletedBy" runat="server" CssClass="TextBox_Normal" width="100" readonly="true"></asp:textbox></td>
					</tr>
				</table>
				<table class="Table_WithoutBorder" width="300" border="0">
					<tr>
						<td class="Td_ButtonContainer"><asp:button id="ButtonAdd" runat="server" CssClass="Button_Normal" CausesValidation="False"
								Text="Add" Width="55px"></asp:button></td>
						<td class="Td_ButtonContainer"><asp:button id="ButtonEdit" runat="server" CssClass="Button_Normal" visible="false" enabled="true"
								CausesValidation="False" Text="Edit" Width="55px"></asp:button></td>
						<td class="Td_ButtonContainer"><asp:button id="ButtonDelete" runat="server" CssClass="Button_Normal" enabled="true" CausesValidation="False"
								Text="Delete" Width="55px"></asp:button></td>
						<td class="Td_ButtonContainer"><asp:button id="ButtonSave" runat="server" CssClass="Button_Normal" enabled="false" CausesValidation="True"
								Text="Save" Width="55px"></asp:button></td>
						<td class="Td_ButtonContainer"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" enabled="true" CausesValidation="False"
								Text="Cancel" Width="55px"></asp:button></td>
						<td class="Td_ButtonContainer"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" CausesValidation="False" Text="OK"
								Width="55px"></asp:button></td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colSpan="2"></td>
		</tr>
	</table>
	<asp:placeholder id="MessageBoxPlaceHolder" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
