<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddEmployment.aspx.vb"
    Inherits="YMCAUI.AddEmployment" %>

<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
<title>YMCA YRS - </title>
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
</script>
<form id="Form1" method="post" runat="server">
<div class="Div_Center">
    <table width="700" cellspacing="0" border="0">
        <tr>
            <td class="Td_HeadingFormContainer" align="left">
                <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                    ShowHomeLinkButton="false" ShowReleaseLinkButton="false"/>
                <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
            </td>
        </tr>
        <tr>
            <td>
                <table class="Table_WithBorder" width="100%" border="0" cellspacing="0" cellspacing="0">
                    <tr>
                        <td align="left" height="40">
                            <asp:label id="LabelHireDate" runat="server" cssclass="Label_Small">Hire Date</asp:label>
                        </td>
                        <td align="left" height="40" colspan="4">
                            <asp:textbox id="TextBoxHireDate" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                            <rjs:PopCalendar ID="PopcalendarDate" runat="server" Separator="/" Control="TextBoxHireDate"
                                Format="mm dd yyyy" ScriptsValidators="IsValidDate"></rjs:PopCalendar>
                            <asp:customvalidator id="valCustomDOB" runat="server" display="Dynamic" controltovalidate="TextBoxHireDate"
                                clientvalidationfunction="IsValidDate">*</asp:customvalidator>
                            <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" controltovalidate="TextBoxHireDate"
                                errormessage="*"></asp:requiredfieldvalidator>
                            <asp:comparevalidator id="CompareValidator1" runat="server" controltovalidate="TextBoxHireDate"
                                errormessage="Hire Date is 30 days in advance." operator="LessThanEqual" controltocompare="TextBox1"></asp:comparevalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:label id="LabelTermDate" runat="server" cssclass="Label_Small">Term Date</asp:label>
                        </td>
                        <td align="left" colspan="4">
                            <asp:textbox id="TextBoxTermDate" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                            <rjs:PopCalendar ID="Popcalendar1" runat="server" Separator="/" Format="mm dd yyyy"
                                ScriptsValidators="IsValidDate" Control="TextBoxTermDate"></rjs:PopCalendar>
                            <asp:customvalidator id="Customvalidator1" runat="server" display="Dynamic" controltovalidate="TextBoxTermDate"
                                clientvalidationfunction="IsValidDate">*</asp:customvalidator>
                            <asp:comparevalidator id="CompareValidator3" runat="server" controltovalidate="TextBoxTermDate"
                                errormessage="Term Date must be greater than Hire Date" operator="GreaterThan"
                                controltocompare="TextBoxHireDate" type="Date"></asp:comparevalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:label id="LabelEligibilityDate" runat="server" cssclass="Label_Small">Eligibility Date</asp:label>
                        </td>
                        <td align="left" colspan="4">
                            <asp:textbox id="TextBoxEligibilityDate" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                            <rjs:PopCalendar ID="Popcalendar2" runat="server" Separator="/" Control="TextBoxEligibilityDate"
                                Format="mm dd yyyy" ScriptsValidators="IsValidDate"></rjs:PopCalendar>
                            <asp:customvalidator id="Customvalidator2" runat="server" display="Dynamic" controltovalidate="TextBoxEligibilityDate"
                                clientvalidationfunction="IsValidDate">*</asp:customvalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:label id="LabelEnrollmentDate" runat="server" cssclass="Label_Small">Enrollment Date</asp:label>
                        </td>
                        <td align="left" colspan="4">
                            <asp:textbox id="TextBoxEnrollmentdate" runat="server" cssclass="TextBox_Normal"
                                wrap="False" readonly="True"></asp:textbox>
                            <rjs:PopCalendar ID="Popcalendar3" runat="server" Separator="/" Control="TextBoxEnrollmentdate"
                                Format="mm dd yyyy" ScriptsValidators="IsValidDate" MessageAlignment="RightCalendarControl">
                            </rjs:PopCalendar>
                            <asp:customvalidator id="Customvalidator3" runat="server" controltovalidate="TextBoxEnrollmentdate"
                                clientvalidationfunction="IsValidDate" display="Dynamic">*</asp:customvalidator>
                            <asp:requiredfieldvalidator id="valRequiredErollmentDate" runat="server" controltovalidate="TextBoxEnrollmentdate"
                                errormessage="*" enabled="False" display="Dynamic"></asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:label id="LabelPriorService" runat="server" cssclass="Label_Small">Prior Service</asp:label>
                        </td>
                        <td align="left" colspan="4">
                            <asp:textbox id="TextBoxPriorService" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="127">
                            <asp:label id="LabelStatusType" runat="server" cssclass="Label_Small">Status Type</asp:label>
                        </td>
                        <td align="left">
                            <asp:dropdownlist id="DropDownListStatusType" runat="server" cssclass="DropDown_Normal"
                                width="154px">
						<asp:ListItem Selected="true" Value="A">Active</asp:ListItem>
						<asp:ListItem Value="L">Leave of Absence</asp:ListItem>
						<asp:ListItem Value="T">Inactive</asp:ListItem>
					</asp:dropdownlist>
                        </td>
                        <td align="left">
                            <asp:label id="LabelStatusDate" runat="server" cssclass="Label_Small">Status Date</asp:label>
                        </td>
                        <td align="left">
                            <asp:textbox id="TextBoxStatusDate" runat="server" cssclass="TextBox_Normal" readonly="True"></asp:textbox>
                            <rjs:PopCalendar ID="Popcalendar4" runat="server" Separator="/" Control="TextBoxStatusDate"
                                Format="mm dd yyyy" ScriptsValidators="No Validate"></rjs:PopCalendar>
                        </td>
                        <td align="left">
                            &nbsp;
                            <asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" controltovalidate="TextBoxStatusDate"
                                errormessage="*"></asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="127">
                            <asp:label id="LabelTitle" runat="server" cssclass="Label_Small">Title</asp:label>
                        </td>
                        <td align="left">
                            <asp:textbox id="TextBoxTitle" runat="server" cssclass="TextBox_Normal"></asp:textbox>
                            &nbsp;<asp:button id="ButtonTitles" runat="server" cssclass="Button_Normal" causesvalidation="False"
                                width="87" text="Titles"></asp:button>
                        </td>
                        <td align="left" colspan="3">
                            &nbsp;<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" controltovalidate="TextBoxTitle"
                                errormessage="*"></asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="5">
                            <table border="0" cellspacing="0">
                                <tr>
                                    <td align="left">
                                        <asp:checkbox class="CheckBox_Normal" id="CheckBoxProfessional" runat="server" text=" Professional"></asp:checkbox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:checkbox class="CheckBox_Normal" id="CheckBoxExempt" runat="server" text=" Exempt"></asp:checkbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:checkbox class="CheckBox_Normal" id="CheckBoxFullTime" runat="server" text=" Full Time"></asp:checkbox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="127">
                            <asp:label id="LabelYMCA" runat="server" cssclass="Label_Small">YMCA</asp:label>
                        </td>
                        <td align="left" colspan="4">
                            <asp:textbox id="TextBoxYMCA" runat="server" cssclass="TextBox_Normal" width="300"></asp:textbox>
                            <asp:button id="ButtonYMCA" runat="server" cssclass="Button_Normal" causesvalidation="False"
                                width="87" text="YMCA"></asp:button>
                            <asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" controltovalidate="TextBoxYMCA"
                                errormessage="*"></asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" width="108">
                            <asp:label id="LabelYMCABranch" runat="server" cssclass="Label_Small">YMCA Branch</asp:label>
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td align="left" colspan="4">
                            <asp:textbox id="TextBoxYMCABranch" runat="server" cssclass="TextBox_Normal" width="300"></asp:textbox>
                            <asp:button id="ButtonYMCABranch" runat="server" cssclass="Button_Normal" causesvalidation="False"
                                width="87" text="YMCA"></asp:button>
                        </td>
                    </tr>
                    <tr>
                        <td class="Td_ButtonContainer" align="right" colspan="5">
                            <asp:textbox id="TextBox1" runat="server" visible="False"></asp:textbox>
                            <asp:button id="ButtonOK" runat="server" cssclass="Button_Normal" width="73" text="OK"></asp:button>
                            <asp:button id="ButtonCancel" runat="server" cssclass="Button_Normal" causesvalidation="False"
                                width="73" text="Cancel"></asp:button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<asp:placeholder id="PlaceHolder1" runat="server"></asp:placeholder>
</form>
<!--#include virtual="bottom.html"-->
