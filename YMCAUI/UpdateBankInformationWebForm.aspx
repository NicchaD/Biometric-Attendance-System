<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UpdateBankInformationWebForm.aspx.vb" Inherits="YMCAUI.UpdateBankInformationWebForm"%>
<!--#include virtual="TopNew.htm"-->
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table width="700">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					&nbsp;Add Bank Information</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td></td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table class="Table_WithBorder" width="700">
			<tr>
				<td align="left"><asp:label id="LabelBankName" runat="server" CssClass="Label_Small">Bank Name</asp:label></td>
				<td align="left"><asp:textbox id="TextBoxBankName" runat="server" CssClass="TextBox_Normal" readonly="true"></asp:textbox><asp:button id="ButtonBanks" runat="server" CssClass="Button_Normal" CausesValidation="False"
						Width="87" Text="Banks"></asp:button><asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" ControlToValidate="TextBoxBankName"
						ErrorMessage="Bank Name cannot be blank">*</asp:requiredfieldvalidator></td>
			</tr>
			<tr>
				<td align="left"><asp:label id="LabelBankABANo" runat="server" CssClass="Label_Small">Bank ABA #</asp:label></td>
				<td align="left"><asp:textbox id="TextBoxABANumber" runat="server" CssClass="TextBox_Normal" MaxLength="9" readonly="true"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" ControlToValidate="TextBoxABANumber"
						ErrorMessage="Bank ABA# cannot be blank">*</asp:requiredfieldvalidator></td>
			</tr>
			<tr>
				<td align="left" height="24"><asp:label id="LabelAccountNumber" runat="server" CssClass="Label_Small">Account Number</asp:label></td>
				<td align="left" height="24"><asp:textbox id="TextBoxAccountNumber" runat="server" CssClass="TextBox_Normal" MaxLength="17"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxAccountNumber"
						ErrorMessage="Account Number cannot be blank">*</asp:requiredfieldvalidator>
					<asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ErrorMessage="Account number should be numeric"
						ControlToValidate="TextBoxAccountNumber" ValidationExpression="[0-9]*">*</asp:RegularExpressionValidator></td>
			</tr>
			<tr>
				<td align="left" height="31"><asp:label id="LabelPaymentMethod" runat="server" CssClass="Label_Small">Payment Method</asp:label></td>
				<td align="left" height="31"><asp:dropdownlist id="DropDownListPaymentMethod" runat="server" CssClass="DropDown_Normal" Width="154px"></asp:dropdownlist></td>
			</tr>
			<tr>
				<td align="left" height="9"><asp:label id="LabelAccuntType" runat="server" CssClass="Label_Small">Account Type</asp:label></td>
				<td align="left" height="9"><asp:dropdownlist id="DropdownAccountType" runat="server" CssClass="DropDown_Normal" Width="154px"></asp:dropdownlist></td>
			</tr>
			<tr>
				<td align="left"><asp:label id="LabelEffectiveDate" runat="server" CssClass="Label_Small">Effective Date</asp:label></td>
				<td align="left"><uc1:DateUserControl id="TextBoxEffectiveDate" runat="server"></uc1:DateUserControl></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td nowrap align="left"><asp:validationsummary id="ValidationSummary1" runat="server" Width="160px" Height="48px"></asp:validationsummary></td>
			</tr>
			<tr>
				<td class="Td_ButtonContainer" align="right"><asp:button id="ButtonCancel" runat="server" CssClass="Button_Normal" CausesValidation="False"
						Width="87" Text="Cancel"></asp:button></td>
				<td class="Td_ButtonContainer" align="left"><asp:button id="ButtonOK" runat="server" CssClass="Button_Normal" Width="87" Text="OK"></asp:button></td>
			</tr>
		</table>
	</div>
	<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
</form>
<!--#include virtual="bottom.html"-->
