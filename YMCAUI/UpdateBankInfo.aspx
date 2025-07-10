<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UpdateBankInfo.aspx.vb"
    Inherits="YMCAUI.UpdateBankInfo" %>

<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<title>YMCA YRS </title>
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
<script language="javascript">
    function show() {
        //window.opener
        //window.opener.document.forms(1).submit()
    }
</script>
<form id="FormUpdateBankInfo" method="post" runat="server">
<div class="Div_Center">
    <table width="700" cellspacing="0" border="0">
        <%--<tr>
				<td class="Td_HeadingFormContainer" align="left"><IMG title="Image" height="10" alt="Image" src="images/spacer.gif" width="10">
					&nbsp;Add/View Bank Information<asp:label id="LabelHdr" runat="server" CssClass="Td_HeadingFormContainer"></asp:label></td>
			</tr>--%>
        <tr>
            <td class="Td_HeadingFormContainer" align="left">
                <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                    ShowHomeLinkButton="false" />
                <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</div>
<div class="Div_Center">
    <table class="Table_WithBorder" width="97%" cellspacing="0" border="0">
        <tr>
            <td align="left" height="26">
                <asp:label id="LabelBankName" runat="server" cssclass="Label_Small">Bank Name</asp:label>
            </td>
            <td align="left" height="26">
                <asp:textbox id="TextBoxBankName" runat="server" cssclass="TextBox_Normal" readonly="true"></asp:textbox>
                <asp:button id="ButtonBanks" runat="server" cssclass="Button_Normal" causesvalidation="False"
                    width="87" text="Banks"></asp:button>
                <asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" controltovalidate="TextBoxBankName"
                    errormessage="Bank Name cannot be blank">*</asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:label id="LabelBankABANo" runat="server" cssclass="Label_Small">Bank ABA #</asp:label>
            </td>
            <td align="left">
                <asp:textbox id="TextBoxABANumber" runat="server" cssclass="TextBox_Normal" readonly="true" MaxLength="9"></asp:textbox>
                <asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" controltovalidate="TextBoxABANumber"
                    errormessage="Bank ABA# cannot be blank">*</asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:label id="LabelAccountNumber" runat="server" cssclass="Label_Small">Account Number</asp:label>
            </td>
            <td align="left">
                <asp:textbox id="TextBoxAccountNumber" runat="server" cssclass="TextBox_Normal" MaxLength="17" ></asp:textbox>
                <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" controltovalidate="TextBoxAccountNumber"
                    errormessage="Account Number cannot be blank">*</asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr id="trFundType" runat="server">
            <td align="left" height="20">
                <asp:label id="Label1" runat="server" cssclass="Label_Small" width="88px">Type of Funds</asp:label>
            </td>
            <td id="trPartcipant" runat="server" align="left" height="20">
                <asp:radiobuttonlist id="rdoFundtype" runat="server" width="112px" repeatdirection="Horizontal"
                    visible="False">
						<asp:ListItem Value="U">American</asp:ListItem>
						<asp:ListItem Value="C">Canadian</asp:ListItem>
					</asp:radiobuttonlist>
            </td>
        </tr>
        <tr>
            <td align="left" height="31">
                <asp:label id="LabelPaymentMethod" runat="server" cssclass="Label_Small">Payment Method</asp:label>
            </td>
            <td align="left" height="31">
                <asp:dropdownlist id="DropDownListPaymentMethod" runat="server" cssclass="DropDown_Normal"
                    width="154px">
                     	<asp:ListItem Value="EFT">EFT</asp:ListItem>
					</asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td align="left" height="9">
                <asp:label id="LabelAccuntType" runat="server" cssclass="Label_Small">Account Type</asp:label>
            </td>
            <td align="left" height="9">
                <asp:dropdownlist id="DropdownAccountType" runat="server" cssclass="DropDown_Normal"
                    width="154px"></asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:label id="LabelEffectiveDate" runat="server" cssclass="Label_Small">Effective Date</asp:label>
            </td>
            <td align="left">
                <uc1:DateUserControl ID="TextBoxEffectiveDate" runat="server"></uc1:DateUserControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td nowrap align="left">
                <asp:validationsummary id="ValidationSummary1" runat="server"></asp:validationsummary>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="Td_ButtonContainer" align="right">
                <asp:button id="ButtonOK" runat="server" cssclass="Button_Normal" width="73" text="OK"></asp:button>
                <asp:button id="ButtonCancel" runat="server" cssclass="Button_Normal" causesvalidation="False"
                    width="73" text="Cancel"></asp:button>
            </td>
        </tr>
    </table>
</div>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
