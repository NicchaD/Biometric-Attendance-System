<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddAdditionalAccounts.aspx.vb"
    Inherits="YMCAUI.AddAdditionalAccounts" %>

<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<html>
<head>
<title> YMCA YRS 
</title>
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
<script language="javascript">
    function IsValidDate(sender, args) {
        fmt = "MM/DD/YYYY";
        if (fnvalidateGendate_tmp(args, fmt)) {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
        }
    }

    function GetFirstDayOfCurrentMonth(controlname) {

        var inDate = document.getElementById(controlname).value;


        if (inDate == '')
            return false;

        var dtNew = new Date(inDate);

        var currMon = dtNew.getMonth() + 1;
        var strNewDate = "";

        strNewDate = (currMon) + "/" + "1" + "/" + (dtNew.getFullYear());
        if (strNewDate == 'NaN/1/NaN') {
            strNewDate = document.getElementById(controlname).value;
        }
        document.getElementById(controlname).value = strNewDate;


    }

    function fnvalidateGendate_tmp(value1, fmt) {
        switch (fmt) {
            case ("MM/DD/YYYY"):
                //alert("Inside MMDDYYY");
                for (q = 0; q < fnvalidateGendate_tmp.arguments.length - 1; q++) {
                    indatefieldtext = fnvalidateGendate_tmp.arguments[q];
                    indatefield = value1.Value;
                    if (indatefield.indexOf("-") != -1) {
                        var sdate = indatefield.split("-");
                    }
                    else {
                        var sdate = indatefield.split("/");
                    }
                    var cmpDate;
                    var chkDate = new Date(Date.parse(indatefield))

                    var cmpDate1 = (chkDate.getMonth() + 1) + "/" + (chkDate.getDate()) + "/" + (chkDate.getFullYear());
                    var cmpDate2 = (chkDate.getMonth() + 1) + "/" + (chkDate.getDate()) + "/" + (chkDate.getYear());

                    var indate2 = (Math.abs(sdate[0])) + "/" + (Math.abs(sdate[1])) + "/" + (Math.abs(sdate[2]));

                    var num = sdate[2];
                    var num1 = num + "8";

                    var num2 = num1.length;
                    if (num2 == 3) {
                        cmpDate = cmpDate2;
                    }
                    if (num2 == 5) {
                        cmpDate = cmpDate1;
                    }
                    if (indate2 != cmpDate) {
                        //alert("before invalid");
                        //alert("Invalid date or date format on field "+value1.id);
                        //indatefieldtext.focus();
                        return false;
                    }
                    else {
                        if (cmpDate == "NaN/NaN/NaN") {
                            //alert("before invalid1");
                            //alert("Invalid date or date format on field "+value1.id);
                            //indatefieldtext.focus();
                            return false;
                        }
                    }
                }
                return true;
                break;


            case ("DD/MM/YYYY"):
                //alert("Inside DDMMYYYY");
                for (q = 0; q < fnvalidateGendate_tmp.arguments.length - 1; q++) {
                    indatefieldtext = fnvalidateGendate_tmp.arguments[q];
                    indatefield = value1.Value;
                    if (indatefield.indexOf("-") != -1) {
                        var sdate = indatefield.split("-");
                    }
                    else {
                        var sdate = indatefield.split("/");
                    }

                    var cmpDate;
                    indatefield = (Math.abs(sdate[1])) + "/" + (Math.abs(sdate[0])) + "/" + (Math.abs(sdate[2]));
                    var chkDate = new Date(Date.parse(indatefield))

                    var cmpDate1 = (chkDate.getDate()) + "/" + (chkDate.getMonth() + 1) + "/" + (chkDate.getFullYear());
                    var cmpDate2 = (chkDate.getDate()) + "/" + (chkDate.getMonth() + 1) + "/" + (chkDate.getYear());
                    var indate2 = (Math.abs(sdate[0])) + "/" + (Math.abs(sdate[1])) + "/" + (Math.abs(sdate[2]));


                    //alert(indate2)
                    //alert(cmpDate2)
                    var num = sdate[2];
                    var num1 = num + "8";

                    var num2 = num1.length;
                    if (num2 == 3) {
                        cmpDate = cmpDate2;
                    }
                    if (num2 == 5) {
                        cmpDate = cmpDate1;
                    }

                    if (indate2 != cmpDate) {

                        //alert("Invalid date or date format on field " + value1.id);
                        //indatefieldtext.focus();
                        return false;
                    }
                    else {
                        if (cmpDate == "NaN/NaN/NaN") {

                            //alert("Invalid date or date format on field "+value1.id);
                            //indatefieldtext.focus();
                            return false;
                        }
                    }
                }
                return true;
                break;

        }
    }
    var theform;
    var isIE;
    var isNS;

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
</head>
<body>

<form id="Form1" method="post" runat="server">
<div class="Div_Center">
    <table width="700">
        <tr>
            <td class="Td_HeadingFormContainer" align="left">
                <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                    ShowHomeLinkButton="false" ShowReleaseLinkButton="false" />
                <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
            </td>
        </tr>
        <%--<tr>
            <td align="left" class="td_Text">
                Update Addition Account
            </td>
        </tr>--%>
        <tr>
            <td>
                <table class="Table_WithBorder" width="100%" border="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <asp:label id="LabelYMCA" runat="server" cssclass="Label_Small">YMCA</asp:label>
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="DropDownListYmca" runat="server" cssclass="DropDown_Normal"
                                width="291px" autopostback="True">
						<asp:ListItem Selected="True"></asp:ListItem>
					</asp:dropdownlist>
                            &nbsp;
                            <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" errormessage="YMCA cannot be blank"
                                controltovalidate="DropDownListYmca">*</asp:requiredfieldvalidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:label id="LabelAccountType" runat="server" cssclass="Label_Small">AccountType</asp:label>
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="DropDownListAccountType" runat="server" cssclass="DropDown_Normal"
                                width="154px">
						            <asp:ListItem Selected="True"></asp:ListItem>
						            <asp:ListItem Value="AP">AP-After-Tax</asp:ListItem>
						            <asp:ListItem Value="TD">TD-TaxDeferred</asp:ListItem>
					        </asp:dropdownlist>
                            <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" errormessage="Account type cannot be blank"
                                controltovalidate="DropDownListAccountType" autopostback="True">*</asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:label id="LabelContractType" runat="server" cssclass="Label_Small">Contract Type</asp:label>
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="DropDownListContractType" runat="server" cssclass="DropDown_Normal"
                                width="154px" name="DropDownListContractType" autopostback="True">
						<asp:ListItem Selected="true" Value=""></asp:ListItem>
						<asp:ListItem Value="M">Dollar</asp:ListItem>
						<asp:ListItem Value="P">Percent</asp:ListItem>
                        <asp:ListItem Value="L">Lump Sum</asp:ListItem>
					</asp:dropdownlist>
                            <asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" errormessage="Contract type cannot be blank"
                                controltovalidate="DropDownListContractType">*</asp:requiredfieldvalidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:label id="LabelContribution" runat="server" cssclass="Label_Small">Contribution%</asp:label>
                        </td>
                        <td align="left">
                            <asp:textbox id="TextBoxContribution" runat="server" cssclass="TextBox_Normal"
                                enabled="False"></asp:textbox>
                            <asp:rangevalidator id="RangeValidator1" runat="server" errormessage="Contribution cannot exceed 100%"
                                controltovalidate="TextBoxContribution" type="Double" maximumvalue="100" minimumvalue="0">*</asp:rangevalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:label id="LabelContributionAmount" runat="server" cssclass="Label_Small">Contribution Amount</asp:label>
                        </td>
                        <td align="left">
                            <asp:textbox id="TextBoxContributionAmount" runat="server" cssclass="TextBox_Normal"
                                name="TextBoxContributionAmount" enabled="False"></asp:textbox>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="27">
                            <asp:label id="LabelEffectiveDate" runat="server" cssclass="Label_Small">Effective Date</asp:label>
                        </td>
                        <td align="left" height="27">
                            <asp:textbox id="TextBoxEffectiveDate" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                            <rjs:PopCalendar ID="PopcalendarDate" runat="server" Separator="/" Format="mm dd yyyy"
                                Control="TextBoxEffectiveDate" ScriptsValidators="No Validate"></rjs:PopCalendar>
                            <asp:customvalidator id="ValCustomEffDate" runat="server" errormessage="Invalid Effective Date"
                                controltovalidate="TextBoxEffectiveDate" display="Dynamic" clientvalidationfunction="IsValidDate">*</asp:customvalidator>
                            <asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" errormessage="Effective date can not be blank"
                                controltovalidate="TextBoxEffectiveDate" display="Dynamic">*</asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:label id="LabelTermDate" runat="server" cssclass="Label_Small">Term Date</asp:label>
                        </td>
                        <td align="left">
                            <asp:textbox id="TextBoxTermDate" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                            <rjs:PopCalendar ID="Popcalendar1" runat="server" Separator="/" Format="mm dd yyyy"
                                Control="TextBoxTermDate" ScriptsValidators="No Validate"></rjs:PopCalendar>
                            <asp:customvalidator id="valCustomTermDate" runat="server" controltovalidate="TextBoxTermDate"
                                display="Dynamic" clientvalidationfunction="IsValidDate" errormessage="Invalid Term Date">*</asp:customvalidator>
                            <asp:comparevalidator id="CompareValidator1" runat="server" errormessage="Term date must be greater than Effective date."
                                controltovalidate="TextBoxTermDate" type="Date" display="Dynamic" operator="GreaterThanEqual"
                                controltocompare="TextBoxEffectiveDate">*</asp:comparevalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                        </td>
                        <td align="left">
                            <asp:validationsummary id="ValidSumm" runat="server" width="496px" height="48px"></asp:validationsummary>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="Td_ButtonContainer" align="right" colspan="2">
                            <asp:button id="ButtonOK" runat="server" cssclass="Button_Normal" width="73px" text="OK"></asp:button>
                            <asp:button id="ButtonCancel" runat="server" cssclass="Button_Normal" width="73px"
                                text="Cancel" causesvalidation="False"></asp:button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<div class="Div_Center" border="0">
    <asp:textbox id="TextboxHireDate" runat="server" width="8px" bordercolor="White"
        forecolor="White" borderstyle="None" readonly="true"></asp:textbox>
    <asp:textbox id="TextboxEnrolldate" runat="server" width="8px" bordercolor="White"
        forecolor="White" borderstyle="None" readonly="true"></asp:textbox>
</div>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
</body>
</html>