<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="YMCAAddressWebForm.aspx.vb" Inherits="YMCAUI.YMCAAddressWebForm"%>
<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl.ascx" %>

<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<!--#include virtual="TopNew.htm"-->
<script language="javascript" src="JS/YMCA_JScript_Warn.js"></script>
<script language="javascript">

function CheckAccess(controlname)
{
	var str=String(document.Form1.all.HiddenSecControlName.value);
	if (str.match(controlname)!= null)
	{
	    //Commented By Anudeep for Bt-1208
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
	<body oncontextmenu="return false">
		<div class="Div_Center">
			<table width="700" border="0">
				<tr>
					<td class="Td_HeadingFormContainer" align="left"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
						Address Information
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
		<div class="Div_Center">
			<table class="Table_WithBorder" cellSpacing="0" cellPadding="0" width="700">
				<tr style="vertical-align:top;">
					<td>
						<DIV style="OVERFLOW: auto; WIDTH: 280px; HEIGHT: 200px; TEXT-ALIGN: left">
							<asp:datagrid id="DataGridYMCAAddress" runat="server" CssClass="DataGrid_Grid" cellpadding="1"
								width="264px" AutoGenerateColumns="False" AllowSorting="True" OnSortCommand="SortCommand_OnClick">
								<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
								<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
								<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
								<HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="UniqueId"></asp:BoundColumn>
									<asp:TemplateColumn>
										<ItemTemplate>
											<asp:ImageButton id="ImageButtonSelect" runat="server" ImageUrl="images\select.gif" CausesValidation="False"
												CommandName="Select" ToolTip="Select"></asp:ImageButton>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="addr1" SortExpression="addr1" HeaderText="Address"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></DIV>
						<P><asp:label id="LabelNoRecords" runat="server" ForeColor="Red" Visible="False" Width="152px">No records found</asp:label></P>
						<P>&nbsp;</P>
						<P></P>
					</td>
					<td>
						<table width="416">
							<tr style="vertical-align:top;">
								<td>
                                <!--Anudeep:16.01.2013 Bt-1298-Address Information - Make this Primary -->
									<table width="408">
										<tr style="vertical-align:top;">
											<td align="left" width="92"><asp:label id="LabelType" runat="server" Width="40px" cssclass="Label_Small">Type</asp:label></td>
											<td align="left" width="145"><asp:dropdownlist id="DropDownType" runat="server" CssClass="DropDown_Normal" Width="88px" enabled="false"></asp:dropdownlist>&nbsp;
												<asp:requiredfieldvalidator id="ReqFldValType" runat="server" ControlToValidate="DropDownType" ErrorMessage="Type cannot be blank">*</asp:requiredfieldvalidator>&nbsp;
											</td>
											<td align="left" width="201"><asp:checkbox id="CheckBoxPrimary" CssClass="CheckBox_Normal" Width="153px" enabled="false" autopostback="true"
													TextAlign="Left" Text="Make this Primary" Runat="server"></asp:checkbox></td>
											<td><asp:checkbox id="CheckBoxActive" CssClass="CheckBox_Normal" Width="70" enabled="false" TextAlign="Left"
													Text="Active" Runat="server"></asp:checkbox></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr style="vertical-align:top;">
								<td>
									<table width="408">

										<tr style="vertical-align:top;">
                                            <td style="text-align:left" cssclass="Label_Small">Address:</td>
											<td align="left" rowspan="2"><NewYRSControls:New_AddressWebUserControl id="AddressWebUserControl1" AllowNote="false" AllowEffDate="true" PopupHeight="530" runat="server"></NewYRSControls:New_AddressWebUserControl></td>
										</tr>
                                        <tr style="vertical-align:bottom; margin-top:2px; ">
											<td style="text-align:left" ><asp:label id="LabelEffectiveDate" runat="server" cssclass="Label_Small">Effective Date</asp:label></td>
											<%--<TD align="left" ><YRSControls:dateusercontrol id="TextBoxEffectiveDate" runat="server"></YRSControls:dateusercontrol></TD>--%>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</div>
		<div class="Div_Center">
			<table class="Td_ButtonContainer" width="700" DESIGNTIMEDRAGDROP="379">
				<tr>
					<td>
						<table width="270">
							<tr>
								<td>&nbsp;</td>
							</tr>
						</table>
					</td>
					<td  align="center"><asp:button id="ButtonAddressSave" CssClass="Button_Normal" Width="80" enabled="false" Text="Save"
							Runat="server"></asp:button></td>
					<td  align="center"><asp:button id="ButtonCancel" CssClass="Button_Normal" Width="80" enabled="false" Text="Cancel"
							Runat="server" CausesValidation="False"></asp:button></td>
					<td  align="center"><asp:button id="ButtonAddressAdd" CssClass="Button_Normal" Width="80" Text="Add" Runat="server"
							CausesValidation="False"></asp:button></td>
					<td  align="center"><asp:button id="ButtonOK" CssClass="Button_Normal" Width="80" Text="OK" Runat="server" CausesValidation="False"></asp:button></td>
				</tr>
			</table>
		</div>
		<INPUT id="HiddenText" type="hidden" name="HiddenText" runat="server"> <INPUT id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server">
		<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder></form>
<!--#include virtual="bottom.html"-->
</body>
