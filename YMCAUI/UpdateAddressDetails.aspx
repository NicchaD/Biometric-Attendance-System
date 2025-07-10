<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UpdateAddressDetails.aspx.vb" Inherits="YMCAUI.UpdateAddressDetails" %>
<%@ Register TagPrefix="YRSControls" TagName="AddressWebUserControl" Src="UserControls/AddressWebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl_new.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="~/UserControls/YMCA_Footer_WebUserControl.ascx" %>
<script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
        <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
    <script language="javascript" src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>
    <link id="Link1" href="CSS/CustomStyleSheet.css" type="text/css" runat="server" rel="stylesheet" />
<script language="javascript">

function ValidateNumeric()
{
	if ((event.keyCode < 48)||(event.keyCode > 57))
	{
		event.returnValue = false;
	}
}
function ValidateAlphaNumeric()
{
	
	if(((event.keyCode > 64)&&(event.keyCode < 91))||((event.keyCode > 96)&&(event.keyCode < 123))||((event.keyCode > 47)&&(event.keyCode< 58))||(event.keyCode == 45))
	{
		
		event.returnValue=true;
	}
	else
	{
		event.returnValue=false;	
	}
}


function ValidateTelephoneNo(str) 
{
    if (document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "US" || document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "CA") {
        str.maxLength = 10;
    }
    else {
        str.maxLength = 25;
    }
    
}
function CheckStatus(id)
{
alert(id.id)
var varId=id.id
var varHidId
var get1="";
//get1=document.Form1.all.HiddenStatus.Id.Value;
//TextBoxPhoneWork
//TextBoxPhoneHome
//TextBoxPhoneMobile
//TextBoxFaxPhone
//varHidId=document.Form1.all.HiddenStatus.ID
if(varId=='TextBoxPhoneWork')
{

//document.Form1.all.HiddenStatus.Value="HiddenStatus"
get1=document.Form1.all.HiddenStatus.Value
get1=get1+" Work "
document.Form1.all.HiddenStatus.Value=get1
alert(document.Form1.all.HiddenStatus.Value)
}
if(varId=='TextBoxPhoneHome')
{

get1=document.Form1.all.HiddenStatus.Value
get1=get1+" Home "
document.Form1.all.HiddenStatus.Value=get1
alert(document.Form1.all.HiddenStatus.Value)
}
if(varId=='TextBoxPhoneMobile')
{

get1=document.Form1.all.HiddenStatus.Value
get1=get1+" Mobile "
document.Form1.all.HiddenStatus.Value=get1
alert(document.Form1.all.HiddenStatus.Value)
}
if(varId=='TextBoxFaxPhone')
{

get1=document.Form1.all.HiddenStatus.Value
get1=get1+" Fax "
document.Form1.all.HiddenStatus.Value=get1
alert(document.Form1.all.HiddenStatus.Value)
}
}

</script>
<body oncontextmenu="return false">
<form id="Form1" method="post" runat="server">
	<div class="Div_Center" style="">
    
		<table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="100%" align="center">
			<tr>
            <td class="Td_HeadingFormContainer" align="left">
                <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                    ShowHomeLinkButton="false" />
                <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" PageTitle="Update Address" runat="server"></YRSControls:YMCA_Header_WebUserControl>
            </td>
        </tr>
            <tr>
				<%--<td class="Td_BackGroundColorMenu" align="left"><cc1:menu id="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2" DefaultMouseUpCssClass="mouseup"
						DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown" DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer"
						HighlightTopMenu="False" Layout="Horizontal"></cc1:menu><SELECTEDMENUITEMSTYLE ForeColor="#3B5386" BackColor="#FBC97A"></SELECTEDMENUITEMSTYLE></td>--%>
			</tr>
			<%--<tr>
				<td class="Td_HeadingFormContainer" align="left" colSpan="2"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
					
				</td>
			</tr>--%>
			<tr>
				<td>&nbsp;
				</td>
			</tr>
		</table>
		<table class="Table_WithBorder" id="tblMail" width="100%" align="center">
			<tr>
				<td class="td_Text" align="left" colSpan="2" height="15">Primary Address
				</td>
			</tr>
			<TR>
				<TD align="left"  colSpan="2" width="380" height="26">
					<%--<YRSControls:AddressWebUserControl id="AddressWebUserControl1" runat="server"></YRSControls:AddressWebUserControl>--%>
                    <NewYRSControls:New_AddressWebUserControl runat="server" ID="AddressWebUserControl1" IsPrimary="1" AllowNote="true" AllowEffDate="true"  PopupHeight="920" />
				</TD>
			</TR>
			<TR>
				<TD class="td_Text" align="left" colSpan="2" height="10">Telephone</TD>
			</TR>
			<TR>
				<TD align="left" width="24" height="24"><asp:label id="LabelWorkTelephone" runat="server" CssClass="Label_Small">Office</asp:label></TD>
				<TD align="left" height="24"><asp:textbox id="TextBoxPhoneWork" runat="server" CssClass="TextBox_Normal" width="180" maxlength="10"></asp:textbox></TD>
			</TR>
			<TR>
				<TD align="left" width="24"><asp:label id="LabelHomeTelephone" runat="server" CssClass="Label_Small">Home</asp:label></TD>
				<TD align="left"><asp:textbox id="TextBoxPhoneHome" runat="server" CssClass="TextBox_Normal" width="180" maxlength="10"></asp:textbox></TD>
			</TR>
			<TR>
				<TD align="left" width="24" height="24"><asp:label id="LabelMobile" runat="server" CssClass="Label_Small">Mobile</asp:label></TD>
				<TD align="left" height="24"><asp:textbox id="TextBoxPhoneMobile" runat="server" CssClass="TextBox_Normal" width="180" maxlength="10"></asp:textbox></TD>
			</TR>
			<TR>
				<TD align="left" width="24" height="21"><asp:label id="LabelFax" runat="server" CssClass="Label_Small">Fax</asp:label></TD>
				<TD align="left" height="21"><asp:textbox id="TextBoxFaxPhone" runat="server" CssClass="TextBox_Normal" width="180" maxlength="10"></asp:textbox></TD>
			</TR>
			<TR>
				<TD vAlign="top" align="left" width="100" height="19"><asp:label id="LabelEffDate" runat="server" CssClass="Label_Small" Width="100px">Effective Date</asp:label></TD>
				<TD vAlign="top" align="left" height="19"><YRSControls:dateusercontrol id="TextBoxEffDate" runat="server"></YRSControls:dateusercontrol></TD>
			</TR>
			<tr>
				<td class="td_Text" align="right" colSpan="2"><asp:button id="ButtonUpdate" runat="server" CssClass="Button_Normal" Width="100PX" Text="Save"></asp:button><asp:button id="ButtonCancel" runat="server" CausesValidation="False" CssClass="Button_Normal"
						Width="100PX" Text="Cancel"></asp:button></td>
			</tr>
		</table>
		<asp:datagrid id="DataGrid1" runat="server" visible="false"></asp:datagrid>
		<P>
			<asp:label id="Label1" runat="server" Width="279px" Visible="False"></asp:label></P>
		<P>&nbsp;</P>
		<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
        <input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server" value="" />
        <table class="Table_WithoutBorder" cellSpacing="0" cellPadding="0" width="100%" align="center">
            <tr>
                <td  width="100%">
                    <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server">
                    </YRSControls:YMCA_Footer_WebUserControl>
                </td>
            </tr>
        </table>
</div>
</form>
</body>

