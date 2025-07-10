<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UpdateParticipantEmploymentService.aspx.vb"
    Inherits="YMCAUI.UpdateParticipantEmploymentService" %>

<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<!--#include virtual="topNew.htm"-->
<script language="javascript">
    function ValidateNumeric() {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            event.returnValue = false;
        }
    }
</script>
<form id="Form1" method="post" runat="server">
<table class="Table_WithoutBorder" cellspacing="0" width="700" align="center">
    <tr>
        <td class="Td_BackGroundColorMenu" align="left">
            <cc1:Menu ID="Menu1" runat="server" mouseovercssclass="MouseOver" MenuFadeDelay="2"
                DefaultMouseUpCssClass="mouseup" DefaultMouseOverCssClass="mouseover" DefaultMouseDownCssClass="mousedown"
                DefaultCssClass="menuitem" CssClass="menustyle" Cursor="Pointer" HighlightTopMenu="False"
                Layout="Horizontal">
                <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
            </cc1:Menu>
        </td>
    </tr>
  
    <tr>
        <td class="Td_HeadingFormContainer" align="left">
            <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
        </td>
    </tr>
    <tr>
        <td align="left">
            <!--<table class="Table_WithOutBorder" width="690" align="center" border="0">-->
            <table class="Table_WithOutBorder" width="550" align="center" border="0">
                <tr>
                    <td align="center" colspan="3">
                        <asp:label id="LabelService" runat="server" cssclass="Label_Medium">Service</asp:label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td align="left">
                        <asp:label id="LabelYears" runat="server" cssclass="Label_Small">Years</asp:label>
                    </td>
                    <td align="left">
                        <asp:label id="LabelMonths" runat="server" cssclass="Label_Small">Months</asp:label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="LabelPaid" runat="server" cssclass="Label_Small">Paid</asp:label>
                    </td>
                    <td align="left">
                        <asp:textbox id="TextBoxYear" runat="server" cssclass="TextBox_Normal" width="30px"
                            maxlength="4"></asp:textbox>
                        <asp:label id="LabelYear" runat="server" cssclass="Label_Small"></asp:label>
                        <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" errormessage="*"
                            controltovalidate="TextBoxYear"></asp:requiredfieldvalidator>
                    </td>
                    <td align="left" nowrap>
                        <asp:textbox id="TextBoxMonth" runat="server" cssclass="TextBox_Normal" width="30px"
                            maxlength="2"></asp:textbox>
                        <asp:label id="LabelMonth" runat="server" cssclass="Label_Small"></asp:label>
                        <asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" errormessage="*"
                            controltovalidate="TextBoxMonth"></asp:requiredfieldvalidator>
                        <asp:rangevalidator id="RangeValidator1" runat="server" errormessage="Month should be between 0 and 11"
                            controltovalidate="TextBoxMonth" maximumvalue="11" minimumvalue="0" type="Integer"></asp:rangevalidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="LabelTotal" runat="server" cssclass="Label_Small">Total</asp:label>
                    </td>
                    <td align="left">
                        <asp:textbox id="TextBoxYearTotal" runat="server" cssclass="TextBox_Normal" width="30px"
                            maxlength="4"></asp:textbox>
                        <asp:label id="LabelYearTotal" runat="server" cssclass="Label_Small"></asp:label>
                        <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" errormessage="*"
                            controltovalidate="TextBoxYearTotal"></asp:requiredfieldvalidator>
                    </td>
                    <td align="left" nowrap>
                        <asp:textbox id="TextBoxMonthTotal" runat="server" cssclass="TextBox_Normal" width="30px"
                            maxlength="2"></asp:textbox>
                        <asp:label id="LabelMonthTotal" runat="server" cssclass="Label_Small"></asp:label>
                        <asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" errormessage="*"
                            controltovalidate="TextBoxMonthTotal"></asp:requiredfieldvalidator>
                        <asp:rangevalidator id="RangeValidator2" runat="server" errormessage="Month should be between 0 and 11"
                            controltovalidate="TextBoxMonthTotal" maximumvalue="11" minimumvalue="0" type="Integer"></asp:rangevalidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table class="Table_WithOutBorder" width="690" align="center" border="0">
                <tr>
                    <td align="left" width="20%">
                        Vesting Date
                    </td>
                    <td align="left">
                        <uc1:DateUserControl ID="TextBoxVestingDate" runat="server"></uc1:DateUserControl>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="13">
                        Vesting Reason
                    </td>
                    <td align="left" height="13">
                        <asp:dropdownlist id="DropDownListVestingReason" runat="server" width="75px" cssclass="Dropdown_Normal"
                            autopostback="True">
								<asp:ListItem Selected="True" VALUE=""></asp:ListItem>
								<asp:ListItem VALUE="AGE"></asp:ListItem>
								<asp:ListItem VALUE="SERV"></asp:ListItem>
								<asp:ListItem VALUE="QDRO"></asp:ListItem>
							</asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="18">
                        Not Vested
                    </td>
                    <td align="left" height="18">
                        <asp:checkbox id="CheckBoxNotVested" runat="server" autopostback="True" cssclass="CheckBox_Normal"></asp:checkbox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="Td_ButtonContainer" align="right" colspan="2">
                        <asp:button id="ButtonSave" runat="server" cssclass="Button_Normal" width="73px"
                            text="OK"></asp:button>
                        <asp:button id="ButtonCancel" runat="server" cssclass="Button_Normal" width="73px"
                            text="Cancel" causesvalidation="False"></asp:button>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
