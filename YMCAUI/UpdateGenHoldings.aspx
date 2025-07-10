<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UpdateGenHoldings.aspx.vb"
    Inherits="YMCAUI.UpdateGenHoldings" %>

<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<title>YMCA YRS </title>
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
<%-- 'Anudeep A              2012-09-22      BT-1126/Additional change YRS 5.0-1246 : : Handling DLIN records --%>
<script language="JavaScript">
    function _OnBlur_TextBoxAmount() {

        var str = String(document.Form1.all.txtAddAmount.value);
        if (isNaN(parseInt(document.Form1.all.txtAddAmount.value))) {
            //alert('Amount cannot have characters.');//Commented for BT_1126
            alert('<%=Resources.withholding.MESSAGE_GENERAL_WITHOLDING_AMOUNT_CANNOT_CHARACTERS %>');
            document.Form1.all.txtAddAmount.value = '';
        }


    }
    function Cancel_OnClick() {
        self.close();
    }
    function ValidateNumeric() {
        if ((event.keyCode < 48) || (event.keyCode > 57)) {
            event.returnValue = false;
        }
    }
    /*
    Function to detect the Browser type.
    */
    function detectBrowser() {
        if (window.navigator.appName.toLowerCase().indexOf("netscape") > -1)
            theform = document.forms["Form1"];
        else
            theform = document.Form1;

        //browser detection
        var strUserAgent = navigator.userAgent.toLowerCase();
        isIE = strUserAgent.indexOf("msie") > -1;
        isNS = strUserAgent.indexOf("netscape") > -1;

    }

    /*
    This function will fire when the control leaves the Text Box.
    The function is responsible for formating the numbers to amount type.
    */
    function FormatAmtControl(ctl) {
        var vMask;
        var vDecimalAfterPeriod;
        var ctlVal;
        var iPeriodPos;
        var sTemp;
        var iMaxLen
        var ctlVal;
        var tempVal;
        ctlVal = ctl.value;
        vDecimalAfterPeriod = 2
        iMaxLen = ctl.maxLength;

        if (isNaN(ctlVal)) {
            // clear the control as this is not a num
            //ctl.value=""
        }
        else {
            ctlVal = ctl.value;
            iPeriodPos = ctlVal.indexOf(".");
            if (iPeriodPos < 0) {
                if (ctl.value.length > (iMaxLen - 3)) {
                    sTemp = ctl.value
                    tempVal = sTemp.substr(0, (iMaxLen - 3)) + ".00";
                }
                else
                    tempVal = ctlVal + ".00"
            }
            else {
                if ((ctlVal.length - iPeriodPos - 1) == 1)
                    tempVal = ctlVal + "0"
                if ((ctlVal.length - iPeriodPos - 1) == 0)
                    tempVal = ctlVal + "00"
                if ((ctlVal.length - iPeriodPos - 1) == 2)
                    tempVal = ctlVal;
                if ((ctlVal.length - iPeriodPos - 1) > 2) {
                    tempVal = ctlVal.substring(0, iPeriodPos + 3);
                }


            }
            ctl.value = tempVal;
        }
    }
    /*
    This function is responsible for filtering the keys pressed and the maintain the amount format of the 
    value in the Text box
    */
    function HandleAmountFiltering(ctl) {
        var iKeyCode, objInput;
        var iMaxLen
        var reValidChars = /[0-9.]/;
        var strKey;
        var sValue;
        var event = window.event || arguments.callee.caller.arguments[0];
        iMaxLen = ctl.maxLength;
        sValue = ctl.value;
        detectBrowser();

        if (isIE) {
            iKeyCode = event.keyCode;
            objInput = event.srcElement;
        } else {
            iKeyCode = event.which;
            objInput = event.target;
        }

        strKey = String.fromCharCode(iKeyCode);

        if (reValidChars.test(strKey)) {
            if (iKeyCode == 46) {
                if (objInput.value.indexOf('.') != -1)
                    if (isIE)
                        event.keyCode = 0;
                    else {
                        if (event.which != 0 && event.which != 8)
                            return false;
                    }
            }
            else {
                if (objInput.value.indexOf('.') == -1) {

                    if (objInput.value.length >= (iMaxLen - 3)) {
                        if (isIE)
                            event.keyCode = 0;
                        else {
                            if (event.which != 0 && event.which != 8)
                                return false;
                        }

                    }
                }
                if ((objInput.value.length == (iMaxLen - 3)) && (objInput.value.indexOf('.') == -1)) {
                    objInput.value = objInput.value + '.';

                }


            }

        }
        else {
            if (isIE)
                event.keyCode = 0;
            else {
                if (event.which != 0 && event.which != 8)
                    return false;
            }
        }

    }
</script>
<body onload="self.focus()">
    <form id="Form1" method="post" runat="server">
    <div align="left" class="Div_Center">
        <table width="700" cellspacing="0" border="0">
            <tr>
                <td class="Td_HeadingFormContainer" align="left">
                    <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                        ShowHomeLinkButton="false" />
                    <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="Table_WithBorder" id="Table1" width="100%" align="left" cellspacing="0"
                        border="0">
                        <tr>
                            <td nowrap align="left" width="139">
                                <asp:Label ID="lblWithHolding" runat="server" CssClass="Label_Small">Withholding Type</asp:Label>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbWithHoldingType" runat="server" CssClass="DropDown_Normal"
                                    DataTextField="Description" DataValueField="Value">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="139">
                                <asp:Label ID="lblAddAmount" runat="server" CssClass="Label_Small">Add'l Amount</asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtAddAmount" runat="server" CssClass="TextBox_Normal" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddAmount"
                                    ErrorMessage="Add'l Amount cannot be blank." ToolTip="Add'l Amount cannot be blank.">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="139">
                                <asp:Label ID="lblStartDate" runat="server" CssClass="Label_Small">Start Date</asp:Label>
                            </td>
                            <td align="left">
                                <uc1:DateUserControl ID="txtStartDate" runat="server"></uc1:DateUserControl>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" width="139">
                                <asp:Label ID="lblEndDate" runat="server" CssClass="Label_Small">End Date</asp:Label>
                            </td>
                            <td align="left">
                                <uc1:DateUserControl ID="txtEndDate" runat="server"></uc1:DateUserControl>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="Td_ButtonContainer" align="right">
                                <asp:Button ID="ButtonOK" runat="server" CssClass="Button_Normal" Text="OK" Width="73px">
                                </asp:Button>
                                <asp:Button ID="ButtonCancel" runat="server" CssClass="Button_Normal" Text="Cancel"
                                    CausesValidation="False" Width="73px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:PlaceHolder ID="PlaceHolderUpadteGenWithold" runat="server"></asp:PlaceHolder>
    </form>
    <!--#include virtual="bottom.html"-->
</body>
