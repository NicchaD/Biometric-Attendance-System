<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="YMCATelephoneWebForm.aspx.vb" Inherits="YMCAUI.YMCATelephoneWebForm"%>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<!--#include virtual="TopNew.htm"-->
<script language="javascript">
function OnSpace()
	{
		
	if ((event.keyCode < 48)||(event.keyCode > 57))
		{
			event.returnValue = false;
		}
	}
function CheckAccess(controlname)
{
	var str=String(document.Form1.all.HiddenSecControlName.value);
	if (str.match(controlname)!= null)
	{
		//alert("Sorry, You are not authorized to do this activity.");
		document.Form1.all.HiddenText.value="";
		document.Form1.all.HiddenText.value="0";
	}
	else
	{
		document.Form1.all.HiddenText.value="";
		document.Form1.all.HiddenText.value="1";
	}
}	
</script>
<form id="Form1" method="post" runat="server">
	<div class="Div_Center">
		<table class="Table_WithoutBorder" width="740">
			<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					Telephone Information
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
			<tr vAlign="top">
				<td vAlign="top">
					<DIV style="OVERFLOW: auto; WIDTH: 280px; HEIGHT: 200px; TEXT-ALIGN: left"><asp:datagrid id="DataGridYMCATelephone" runat="server" AutoGenerateColumns="False" width="260px"
							cellpadding="1" CssClass="DataGrid_Grid" allowsorting="True" OnSortCommand="SortCommand_OnClick">
							<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
							<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
							<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
							<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="guiUniqueId"></asp:BoundColumn>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
											CommandName="Select" ToolTip="Select"></asp:ImageButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Telephone" SortExpression="Telephone" HeaderText="Telephone"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></DIV>
					<asp:label id="LabelNoRecord" runat="server" Visible="False" Width="152px">No record found</asp:label></td>
				<td>
					<table width="376">
						<TBODY>
							<tr>
								<td align="left">
									<table >
										<TBODY>
											<tr>
												<td align="left" height="41"><asp:label id="LabelType" runat="server" Width="40px" cssclass="Label_Small">Type</asp:label></td>
												<td align="left" height="41"><asp:dropdownlist id="DropdownType" runat="server" CssClass="DropDown_Normal" Width="88px" enabled="false"></asp:dropdownlist></td>
												<td align="right" height="41"><asp:checkbox id="CheckBoxPrimary" CssClass="CheckBox_Normal" enabled="false" Runat="server" Text="Primary"
														TextAlign="Left"></asp:checkbox></td>
												<td align="right" height="41"><asp:checkbox id="CheckBoxActive" CssClass="CheckBox_Normal" enabled="false" Runat="server" Text="Active"
														TextAlign="Left"></asp:checkbox></td>
											</tr>
											<tr>
												<td align="left"><asp:label id="LabelEffectiveDate" runat="server" cssclass="Label_Small">Effective Date:</asp:label></td>
												<td align="left"><uc1:dateusercontrol id="TextBoxEffectiveDate" runat="server"></uc1:dateusercontrol></td>
											</tr>
											<tr>
												<td align="left"><asp:label id="LabelTelephone" runat="server" cssclass="Label_Small">Telephone:</asp:label></td>
												<td align="left"><asp:textbox id="TextBoxTelephone" runat="server" width="180" CssClass="TextBox_Normal" MaxLength="25"></asp:textbox><asp:requiredfieldvalidator  id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxTelephone"
														ErrorMessage="Telephone cannot be blank">*</asp:requiredfieldvalidator><asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxTelephone"
														ErrorMessage="Please provide valid Telephone number" ValidationExpression="[0-9]*">*</asp:regularexpressionvalidator></td>
                										<%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
														ErrorMessage="Telephone should be numeric" ValidationExpression="[0-9]*">*</asp:regularexpressionvalidator></td>
                										END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
												<td align="right"><asp:label id="LabelExt" runat="server" cssclass="Label_Small">Ext:</asp:label></td>
												<td align="left"><asp:textbox id="TextBoxExt" runat="server" width="70" CssClass="TextBox_Normal" MaxLength="9"></asp:textbox></td>
											</tr>
										</TBODY></table>
								</td>
							</tr>
						</TBODY></table>
                        <!-- Anudeep :15.01.2013 User Permissions - YMCA Maintenance -->
					<asp:validationsummary id="ValidationSummary1" validationgroup = "validate" runat="server" Width="184px"></asp:validationsummary></td>
			</tr>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
	</div>
	<div class="Div_Center">
		<table width="740">
			<tr>
				<td class="Td_ButtonContainer">
					<table width="270">
						<tr>
							<td>&nbsp;</td>
						</tr>
					</table>
				</td>
				<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonTelephoneSave"  CssClass="Button_Normal" Width="80" enabled="false" Runat="server"
						Text="Save"></asp:button></td>
				<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonCancel" CssClass="Button_Normal" Width="80" enabled="false" Runat="server"
						Text="Cancel" CausesValidation="False"></asp:button></td>
				<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonTelephoneAdd" CssClass="Button_Normal" Width="80" Runat="server" Text="Add"
						CausesValidation="False"></asp:button></td>
				<td class="Td_ButtonContainer" align="center"><asp:button id="ButtonOK" CssClass="Button_Normal" Width="80" Runat="server" Text="OK" CausesValidation="False"></asp:button></td>
			</tr>
		</table>
	</div>
	<INPUT id="HiddenText" type="hidden" name="HiddenText" runat="server"> <INPUT id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server">
	<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
