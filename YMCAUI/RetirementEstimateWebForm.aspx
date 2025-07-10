<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RetirementEstimateWebForm.aspx.vb"
    Inherits="YMCAUI.RetirementEstimateWebForm" %>

<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Toolbar_WebUserControl" Src="UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%--2012-09-22    Anudeep A           BT-1126/Additional change YRS 5.0-1246 : : Handling DLIN records  --%>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="YRSCustomControls" %> <%-- PPP | 2015.09.28 | YRS-AT-2596 --%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>
    <title>YMCA YRS</title>
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <%-- START: PPP | 2015.09.28 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) --%>
    <script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" rel="stylesheet">
    <%-- END: PPP | 2015.09.28 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) --%>
    <%--START: MMR | 2017.02.22 | YRS-AT-2625 |Add style to set close icon display property--%>
    <style>
        .no-close .ui-dialog-titlebar-close {display: none; }
    </style>
    <%--EMD: MMR | 2017.02.22 | YRS-AT-2625 |Add style to set close icon display property--%>

    <script language="javascript" type="text/javascript">
        function IsValidStartDate(sender, args) {
            args.IsValid = false;
        }

        function IsValidStopDate(sender, args) {
            args.IsValid = false;
        }

        function _OnBlur_ModifiedSalary() {
            var intValue1 = document.Form1.all.TextBoxModifiedSal.value;
            if (intValue1 == "") {

                document.Form1.all.TextBoxModifiedSal.value = "0.00";

            }
            return true;
        }

        function _OnBlur_ModifiedSal() {
        }

        function _OnBlur_AnnualSalaryIncreased() {
        }

        function _OnBlur_AnnualSalaryIncrease() {
        }

        function ValidateNumeric() {
            if ((event.keyCode < 48) || (event.keyCode > 57)) {
                event.returnValue = false;
            }
        }

        function ValidateDecimal() {
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 46) {
                event.returnValue = false;
            }
        }

        function IsNumeric() {
            var intValue1 = document.Form1.all.TextBoxModifiedSal.value;
            if (isNaN(intValue1)) {
                //alert("Please enter a Numeric Value");'commented on 22-sep for BT-1126
                alert('<%= getmessage("MESSAGE_RETIREMENT_ESTIMATE_PLEASE_ENTER_NUMERIC")%>');
                document.Form1.all("TextBoxModifiedSal").value = "";
                return false;
            }

            var intValue2 = document.Form1.all.TextBoxFutureSalary.value;
            if (isNaN(intValue2)) {
                //alert("Please enter a Numeric Value");'commented on 22-sep for BT-1126
                alert('<%= getmessage("MESSAGE_RETIREMENT_ESTIMATE_PLEASE_ENTER_NUMERIC")%>');
                document.Form1.all("TextBoxFutureSalary").value = "";
                return false;
            }

            var intValue3 = document.Form1.all.TextboxContribAmtSav.value;

            if (isNaN(intValue3)) {
                //alert("Please enter a Numeric Value");'commented on 22-sep for BT-1126
                alert('<%= getmessage("MESSAGE_RETIREMENT_ESTIMATE_PLEASE_ENTER_NUMERIC")%>');
                document.Form1.all("TextboxContribAmtSav").value = "";
                return false;
            }

            var intValue4 = document.Form1.all.TextboxContribAmtRet.value;
            if (isNaN(intValue4)) {
                //alert("Please enter a Numeric Value");'commented on 22-sep for BT-1126
                alert('<%= getmessage("MESSAGE_RETIREMENT_ESTIMATE_PLEASE_ENTER_NUMERIC")%>');
                document.Form1.all("TextboxContribAmtRet").value = "";
                return false;
            }
        }

        function getAge(dateStringBirth, dateStringRetire) {
            var Birth = new Date(dateStringBirth.substring(6, 10), dateStringBirth.substring(3, 5) - 1, dateStringBirth.substring(0, 2));
            var yearBirth = Birth.getYear();
            var monthBirth = Birth.getMonth();
            var dateBirth = Birth.getDate();
            var Retire = new Date(dateStringRetire.substring(6, 10), dateStringRetire.substring(3, 5) - 1, dateStringRetire.substring(0, 2));
            var yearRetire = Retire.getYear();
            var monthRetire = Retire.getMonth();
            var dateRetire = Retire.getDate();

            yearAge = yearRetire - yearBirth;
            if (monthRetire >= yearBirth)
                var monthAge = monthRetire - monthBirth;
            else {
                yearAge--;
                var monthAge = 12 + monthRetire - monthBirth;
            }

            if (dateRetire >= dateBirth)
                var dateAge = dateRetire - dateBirth;
            else {
                monthAge--;
                var dateAge = 31 + dateRetire - dateBirth;

                if (monthAge < 0) {
                    monthAge = 11;
                    yearAge--;
                }
            }
            if (yearAge > 1900) {
                yearAge = yearAge - 1900;
            }
            return yearAge;
        }

        function AgeCalc() {
            var dateStringRetire;
            var dateStringBirth;
            var age;
            dateStringBirth = document.Form1.all.TextBoxRetireeBirthday.value;
            dateStringRetire = document.Form1.all.TextBoxRetirementDate.value;
            alert(dateStringBirth);
            alert(dateStringRetire);
            age = getAge(dateStringBirth, dateStringRetire);
            document.Form1.all.TextBoxRetirementAge.value = age;
        }

        function RetireeDate() {
            var retDate;
            var age;
            var dateStringBirth;
            var intValue = document.Form1.all.TextBoxRetirementAge.value;

            if (intValue != '') {
                if (isNaN(intValue)) {
                    //alert("Please enter a Numeric Value");'commented on 22-sep for BT-1126
                    document.Form1.all.TextBoxRetirementAge.value = "";
                    return false;
                    alert('<%= getmessage("MESSAGE_RETIREMENT_ESTIMATE_PLEASE_ENTER_NUMERIC")%>');
                }

                age = document.Form1.all.TextBoxRetirementAge.value;
                dateStringBirth = document.Form1.all.TextBoxRetireeBirthday.value;
                retDate = getRetireeDate(dateStringBirth, age);

                document.Form1.all.TextBoxRetirementDate.value = retDate;

            } else {
                document.Form1.all.TextBoxRetirementDate.value = '';
            }

            document.Form1.submit();
        }

        function getRetireeDate(dateStringBirth, age) {
            var newMonth;
            var newDate;
            var newDate1;
            var newDate2;
            var temp;
            if (!document.Form1.all.TextBoxRetirementAge.value == '') {
                newDate = parseInt((document.Form1.all.TextBoxRetireeBirthday.value).substring(6, 10)) + parseInt(document.Form1.all.TextBoxRetirementAge.value);

                newDate1 = parseInt(newDate) + 1;

                newMonth = parseInt(new Date(document.Form1.all.TextBoxRetireeBirthday.value).getMonth() + 1);

                temp = parseInt(new Date(document.Form1.all.TextBoxRetireeBirthday.value).getDay());
                temp = parseInt((document.Form1.all.TextBoxRetireeBirthday.value).substring(3, 5));

                if (temp == 1) {
                    newDate2 = parseInt(newMonth);
                } else {
                    newDate2 = parseInt(newMonth) + 1;
                }

                if (parseInt((document.Form1.all.TextBoxRetireeBirthday.value).substring(0, 2)) == 12) {
                    if (temp == 1) {
                        return '12/01/' + newDate;
                    } else {
                        return '01/01/' + newDate1
                    }
                }
                else {
                    if (parseInt(newDate2) <= 9) {
                        return '0' + newDate2 + '/01/' + newDate;
                    }
                    else {
                        return newDate2 + '/01/' + newDate;
                    }
                }
            }
        }

        function getRetireeRetirementDate() {

            var newAge = parseFloat((document.Form1.all.TextBoxRetirementDate.value).substring(6, 10)) - parseFloat((document.Form1.all.TextBoxRetireeBirthday.value).substring(6, 10));
            var newDay = parseFloat((document.Form1.all.TextBoxRetirementDate.value).substring(3, 5));
            var oldDay = parseFloat((document.Form1.all.TextBoxRetireeBirthday.value).substring(3, 5));

            var newMonth = parseFloat((document.Form1.all.TextBoxRetirementDate.value).substring(0, 2));
            var oldMonth = parseFloat((document.Form1.all.TextBoxRetireeBirthday.value).substring(0, 2));
            if (parseFloat(newDay) > 1) {
                if (parseFloat(newMonth) <= 9) {
                    document.Form1.all.TextBoxRetirementDate.value = '0' + newMonth + '/01/' + parseFloat((document.Form1.all.TextBoxRetirementDate.value).substring(6, 10));
                }
                else {
                    document.Form1.all.TextBoxRetirementDate.value = newMonth + '/01/' + parseFloat((document.Form1.all.TextBoxRetirementDate.value).substring(6, 10));
                }
            }

            var bool_Age = false;
            if (parseFloat(oldMonth) == parseFloat(newMonth)) {
                if (parseFloat(newDay) >= parseFloat(oldDay)) {
                    if (parseFloat(oldDay) == 1) {
                        document.Form1.all.TextBoxRetirementAge.value = newAge;
                        bool_Age = true;
                    }
                }
            }
            if (bool_Age == false) {
                if (parseFloat(oldMonth) + 1 > parseFloat(newMonth)) {
                    document.Form1.all.TextBoxRetirementAge.value = parseFloat(newAge) - 1;
                }
                else {
                    document.Form1.all.TextBoxRetirementAge.value = newAge;

                }
            }

            document.Form1.submit();
        }

        function openReportPrinter() {
            //var wndo=window.open('FT\\ReportPrinter.aspx', '','width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
            //wndo.moveTo(20000,20000);
            window.open('FT\\ReportPrinter.aspx', '', 'width=450,height=250, menubar=no, resizable=yes,top=200,left=150, scrollbars=yes, status=yes');
        }

        function openReportViewer() {
            window.open('FT\\ReportViewer.aspx', 'ReportPopUp', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
        }

        <%-- START: PPP | 2015.10.05 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788)
        "ValidateDate" javascript function is part of the CustomControl.Resources.CalenderTextBox.js file which can be accessed without adding the javascript reference --%>
        ////2012.05.22 SP:   BT-976/YRS 5.0-1507 - Reopned issue
        //function PostBackCalendarBenef() {
        //    document.Form1.submit();
        //}
        function PostBackCalendarBenef(control) {
            if (ValidateDate(control.value, '')) {
            document.Form1.submit();
        }
        }
        <%-- END: PPP | 2015.10.05 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) --%>


        function DisableCopyPaste(e) {
            // Message to display
            var message = "Cntrl key/ Right Click Option disabled";
            // check mouse right click or Ctrl key press
            var kCode = event.keyCode || e.charCode;
            //FF and Safari use e.charCode, while IE use e.keyCode
            if (kCode == 17 || kCode == 2) {
                alert(message);
                return false;
            }
        }
        <%-- Comment by Chandrasekar this logic coded in the aspx.vb file 
        <%--Start-PPP | 2015.10.01 | YRS-AT-2635 - We need a conditional message in Retirement tab of estimates
        function ApplyNewRDBRule() {
            if (document.Form1.all.TextBoxRetireeBirthday.value != '') {
                var age = parseFloat(<%=Convert.ToDecimal(DateDiff(DateInterval.Day, Convert.ToDateTime(TextBoxRetireeBirthday.Text), Convert.ToDateTime("01/01/2019")) / 365.25).ToString.Substring(0, 2));
                if (age < 55) {
                    document.getElementById('lblNewRDBRuleMessage').style.display = 'inline-block';
                }
            }
        }
       End-PPP | 2015.10.01 | YRS-AT-2635 - We need a conditional message in Retirement tab of estimates--%>
        <%--START: MMR | 2017.02.22 | YRS-AT-2625 | Displaying manual transactions list in pop up --%>
        $(document).ready(function () {
            BindEvents();
        });

        function BindEvents() {
            $('#divTransactionList').dialog({
                autoOpen: false,
                resizable: false,
                dialogClass: 'no-close',
                draggable: true,
                width: 630, minheight: 200,
                height: 300, 
                closeOnEscape: false,
                title: "Manage Manual Transactions",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });

            //Check/uncheck all checkboxes in list according to main checkbox 
            $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeHeader']:checkbox").click(function () {
                //Header checkbox is checked or not
                var bool = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeHeader']:checkbox").is(':checked');
                //check and check the checkboxes on basis of Boolean value
                $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox").attr('checked', bool);
            });


            $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox").click(function () {
                //Get number of checkboxes in list either checked or not checked
                var totalCheckboxes = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox").size();
                //Get number of checked checkboxes in list
                var checkedCheckboxes = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox:checked").size();
                //check and uncheck the header checkbox on the basis of difference of both values
                $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeHeader']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);

            });
        }

        function ShowDialog(source) {
            var isOpen = true;
            $('#hdnSourceManualTransaction').val(source);
            $(document).ready(function () {
                $("#divTransactionList").dialog("option", "title", "Manage Manual Transactions");               
                switch (source) {
                    case "1": <%--"1" indicates dialog is opened through link --%>
                        $('#divTransactionList').dialog("open");
                        var totalCheckboxes = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox").size();
                        //Get number of checked checkboxes in list
                        var checkedCheckboxes = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox:checked").size();
                        //check and uncheck the header checkbox on the basis of difference of both values
                        $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeHeader']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);
                        break;
                    case "2": <%--"2" indicates dialog is opened through calculate button --%>
                    <%-- START: MMR | 2017.02.27 | YRS-AT-2625 | Added validation to check if client side validation is true before post back to avoid error--%>
                        if (Page_ClientValidate("")) {
                            if ($('#hdnManualTransaction').val() == "2" && $('#DropDownListPlanType').val().toUpperCase() != "SAVINGS") { //Added validation dialog should not open if selected plan type is savings
                                $('#divTransactionList').dialog("open");
                                isOpen = false;
                            }
                            
                        }
                        else {
                            isOpen = false;
                        }
                    <%-- END: MMR | 2017.02.27 | YRS-AT-2625 | Added validation to check if client side validation is true before post back to avoid error--%>
                        break;
                    case "3": <%--"3" indicates dialog is opened through Disability Form button --%>
                        if ($('#hdnManualTransaction').val() == "2" && $('#DropDownListPlanType').val().toUpperCase() != "SAVINGS") { //Added validation dialog should not open if selected plan type is savings
                            $('#divTransactionList').dialog("open");
                            isOpen = false;
                        }
                        break;
                }
            });

            return isOpen;
        }

        function CloseDialog() {
            $('#divTransactionList').dialog("close");
        }


        function getSelectedManualTransaction() {
            var getAllUnSelectedUniqueId = "";
            // Hidden field value 3 indicates manual transaction selected
            $('#hdnManualTransaction').val("3");
            var checkedCheckboxes = $("#<%=DatagridManualTransactionList.ClientID%> input[id*='chkAccountTypeRow']:checkbox:not(:checked)")
            checkedCheckboxes.each(function (chk) {
                var getUniqueId = $($(checkedCheckboxes[chk]).closest('tr').find('.hideGridColumn')).text();
                getAllUnSelectedUniqueId = getAllUnSelectedUniqueId + getUniqueId + ",";
            });

            $.ajax({
                type: "POST",
                url: "RetirementEstimateWebForm.aspx/GetSelectedManualTransactions",
                data: "{'uniqueIDs':'" + getAllUnSelectedUniqueId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    CloseDialog();                   
                    switch ($('#hdnSourceManualTransaction').val()) {
                        case "1":
                            $('#lblMessageManualTransaction').hide();
                            break;
                        case "2":
                            $('#Buttoncalculate').click();
                            break;
                        case "3":
                            $('#btnPRADisability').click();
                            break;
                    }
                }
            });
        }
        <%--END: MMR | 2017.02.22 | YRS-AT-2625 | Displaying manual transactions list in pop up --%>
    </script>

</head>
<%--START: PPP | 07/12/2017 | YRS-AT-3771 | OnLoad event is checking 2017 limit is applied for 2018 onwards or not?--%>
<body onload="CheckMessagesTemp();">
<%--
<body> < %-- PPP | 2015.10.01 | YRS-AT-2635 - Handled OnLoad event--% >
END: PPP | 07/12/2017 | YRS-AT-3771 | OnLoad event is checking 2017 limit is applied for 2018 onwards or not?--%>
    <form id="Form1" method="post" runat="server">
    <div class="Div_Center">
        <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="740">
            <tr>
                <td>
                    <YRSControls:YMCA_Toolbar_WebUserControl ID="YMCA_Toolbar_WebUserControl1" runat="server"
                        ShowLogoutLinkButton="true" ShowHomeLinkButton="true" ShowReleaseLinkButton="false">
                    </YRSControls:YMCA_Toolbar_WebUserControl>
                </td>
            </tr>
            <tr>
                <td class="Td_BackGroundColorMenu" align="left">
                    <cc1:Menu ID="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="false"
                        CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown"
                        DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup" zIndex="100"
                        MenuFadeDelay="1" mouseovercssclass="MouseOver">
                        <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                    </cc1:Menu>
                </td>
            </tr>
        </table>
    </div>
    <div class="Div_Center">
        <table class="Table_WithoutBorder" width="740" cellpadding="0" cellspacing="0">
            <%-- <tr>
                <td class="Td_HeadingFormContainer" align="left">
                    <img title="image" height="10" alt="image" src="images/spacer.gif" width="10">
                    Retirement Estimate
                    <asp:Label ID="LabelTitle" runat="server" Width="432px"></asp:Label>
                </td>
            </tr>--%>
            <tr>
                <td class="Td_HeadingFormContainer" align="left">
                    <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div class="Div_Center">
        <table class="Table_WithBorder" width="740">
            <tbody>
                <tr>
                    <td>
                        <table class="Table_WithOutBorder" width="100%" align="center">
                            <tr>
                                <td>
                                    <table cellspacing="1" cellpadding="0" width="345" align="center">
                                        <tr valign="top">
                                            <td nowrap align="right" height="3">
                                                <asp:Label ID="LabelRetireeBirthday" runat="server" Width="139px" CssClass="Label_Small">Retiree BirthDay </asp:Label>
                                            </td>
                                            <td align="left" height="3">
                                                <asp:TextBox ID="TextBoxRetireeBirthday" runat="server" CssClass="TextBox_Normal"
                                                    name="TextBoxRetireeBirthday"></asp:TextBox>
                                            </td>
                                            <!--Code Add Start by Hafiz on 16Feb06-->
                                            <td align="left" height="3">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td nowrap align="right" height="3">
                                                <asp:Label ID="LabelRetirementType" runat="server" CssClass="Label_Small" Width="110px">Retirement Type </asp:Label>
                                            </td>
                                            <td align="left" height="5">
                                                <asp:DropDownList ID="DropDownListRetirementType" runat="server" Width="135" CssClass="DropDown_Normal"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Value="NORMAL">Normal</asp:ListItem>
                                                    <asp:ListItem Value="DISABL">Disabled</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <!--Code Add End by Hafiz on 16Feb06-->
                                        </tr>
                                        <tr valign="top">
                                            <td nowrap align="right" height="19">
                                                <asp:Label ID="LabelRetirementDate" runat="server" CssClass="Label_Small">Retirement Date</asp:Label>
                                            </td>
                                            <td nowrap align="left" height="19">
                                                <asp:TextBox ID="TextBoxRetirementDate" runat="server" Width="104px" CssClass="TextBox_Normal"
                                                    name="TextBoxRetirementDate" ReadOnly="True"></asp:TextBox><rjs:PopCalendar ID="PopcalendarDate"
                                                        runat="server" Format="mm dd yyyy" Control="TextBoxRetirementDate" Separator="/">
                                                    </rjs:PopCalendar>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxRetirementDate"
                                                    ErrorMessage="Retirement Date cannot be blank.">*</asp:RequiredFieldValidator>
                                            </td>
                                            <!--Code Add Start by Hafiz on 16Feb06-->
                                            <td align="left" height="20">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td nowrap align="right" height="20">
                                                <asp:Label ID="LabelPlanType" runat="server" CssClass="Label_Small" Width="100px">Plan Type</asp:Label>
                                            </td>
                                            <!--<Please donot remove ListBoxProjectedInterest control this will hamper the result>-->
                                            <td align="left" height="20">
                                                <asp:DropDownList ID="DropDownListPlanType" runat="server" Width="135" CssClass="DropDown_Normal"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Value="BOTH">Both</asp:ListItem>
                                                    <asp:ListItem Value="RETIREMENT">Retirement</asp:ListItem>
                                                    <asp:ListItem Value="SAVINGS">Savings</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <!--Code Add End by Hafiz on 16Feb06-->
                                        </tr>
                                        <tr valign="top">
                                            <td nowrap align="right">
                                                <asp:Label ID="LabelRetirementAge" runat="server" CssClass="Label_Small">Retirement Age</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TextBoxRetirementAge" runat="server" Width="104px" CssClass="TextBox_Normal"
                                                    name="TextBoxRetirementAge"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                                        runat="server" ControlToValidate="TextBoxRetirementAge" ErrorMessage="Retirement Age cannot be blank.">*</asp:RequiredFieldValidator>
                                            </td>
                                            <!--Code Add Start by Hafiz on 16Feb06-->
                                            <td align="left">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td nowrap align="right">
                                                <asp:Label ID="LabelProjected" runat="server" CssClass="Label_Small">Projected Interest Rates (%)</asp:Label>
                                            </td>
                                            <td align="right">
                                                <!--<Please donot remove ListBoxProjectedYearInterest control this will hamper the result>-->
                                                <asp:ListBox ID="ListBoxProjectedYearInterest" runat="server" Width="136px" CssClass="DropDown_Normal"
                                                    Height="22" Visible="false"></asp:ListBox>
                                                <asp:DropDownList ID="DropDownListProjInterest2" runat="server" CssClass="DropDown_Normal"
                                                    Width="135px" Height="22" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <%-- START: MMR | 2017.02.22 | YRS-AT-2625 | Added to show link for manual transactions --%>
                                        <tr>
                                            <td colspan="3"></td>
                                            <td colspan="2" align="right" class="Link_SmallBold" style="height:20px; vertical-align:bottom;">
                                                <a href="#" id="lnkManualTransaction" runat="server" style="font-size:11px" visible="false" onclick="ShowDialog('1');">Manage Manual Transactions</a>
                                            </td>
                                        </tr>
                                        <%-- END: MMR | 2017.02.22 | YRS-AT-2625 | Added to show link for manual transactions --%>
                                        <tr valign="top">
                                            <td valign="top" align="right">
                                                <asp:Label ID="labelPRAAssumption" runat="server" CssClass="Label_Small" Width="100px"
                                                    Visible="false" Text="PRAAssumption"></asp:Label>
                                            </td>
                                            <td valign="top" nowrap align="left" colspan="4">
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:TextBox ID="TextBoxPraAssumption" runat="server" Width="285px" Height="82px"
                                                                Visible="false" TextMode="MultiLine" MaxLength="1000"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="Panel1" runat="server" Visible="false">
                                                                <table id="tblPrintOption" border="0" cellspacing="0" cellpadding="0" class="Table_WithBorder"
                                                                    runat="server" width="160">
                                                                    <tr>
                                                                        <td class="Td_HeadingFormContainer" align="center">
                                                                            Print Options</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <%-- Start - Manthan Rajguru YRS-AT-2151 Commented code --%>
                                                                   <%-- <tr height="30px">
                                                                        <td valign="center" align="center">
                                                                            <asp:Button ID="btnPRAColor" runat="server" CssClass="Button_Normal" Width="138"
                                                                                Text="Color Short Form" CommandName="PRACOLORSHORT"></asp:Button>
                                                                        </td>
                                                                    </tr>--%>
                                                                    <%--<tr height="30px">
                                                                        <td align="center" valign="center">
                                                                            <asp:Button ID="btnPRAShort" runat="server" CssClass="Button_Normal" Width="138"
                                                                                Text="Draft Short Form" CommandName="PRADRAFTSHORT"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr height="30px">
                                                                        <td align="center" valign="center">
                                                                            <asp:Button ID="ButtonPRA" runat="server" CssClass="Button_Normal" Width="138" Text="Color Full Form"
                                                                                CommandName="PRACOLORFULL"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr height="30px">
                                                                        <td align="center" valign="center">
                                                                            <asp:Button ID="ButtonPRAFull" runat="server" CssClass="Button_Normal" Width="138"
                                                                                Text="Draft Full Form" CommandName="PRADRAFTFULL"></asp:Button>
                                                                        </td>
                                                                    </tr>--%>
                                                                    
                                                                    <tr height="30px">
                                                                        <td align="center" valign="center">
                                                                            <asp:Button ID="ButtonPRA" runat="server" CssClass="Button_Normal" Width="138" Text="Color Form"
                                                                                CommandName="PRACOLORFULL"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr height="30px">
                                                                        <td align="center" valign="center">
                                                                            <asp:Button ID="ButtonPRAFull" runat="server" CssClass="Button_Normal" Width="138"
                                                                                Text="B & W Form" CommandName="PRADRAFTFULL"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <%-- End - Manthan Rajguru YRS-AT-2151 Commented code --%>
                                                    </tr>
                                                </table>
                                                </asp:Panel>
                                                <asp:Panel ID="PanelDisability" runat="server" Visible="false">
                                                    <table id="Table1" border="0" cellspacing="0" cellpadding="0" class="Table_WithBorder"
                                                        runat="server" width="160">
                                                        <tr>
                                                            <td class="Td_HeadingFormContainer" align="center">
                                                                Print Options</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr height="15px">
                                                            <td valign="center" align="center">
                                                            </td>
                                                        </tr>
                                                        <tr height="40px">
                                                            <td valign="center" align="center">
                                                                <asp:Button ID="btnPRADisability" runat="server" CssClass="Button_Normal" Width="138"
                                                                    Text="Disability Form" CommandName="PRADISABL" OnClientClick="javascript:return ShowDialog('3');"></asp:Button> <%-- MMR | 2017.02.27 | YRS-AT-2625 | Added javascript fucntion on click event to open dialog--%>
                                                            </td>
                                                        </tr>
                                                        <tr height="15px">
                                                            <td valign="center" align="center">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="PanelAlternatePayee" runat="server" Visible="false">
                                                    <table id="Table2" border="0" cellspacing="0" cellpadding="0" class="Table_WithBorder"
                                                        runat="server" width="160">
                                                        <tr>
                                                            <td class="Td_HeadingFormContainer" align="center">
                                                                Print Options</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr height="15px">
                                                            <td valign="center" align="center">
                                                            </td>
                                                        </tr>
                                                        <tr height="40px">
                                                            <td valign="center" align="center">
                                                                <asp:Button ID="btnAlternatePayee" runat="server" CssClass="Button_Normal" Width="155"
                                                                    Text="AlternatePayee Form" CommandName="PRAALTPAYEE"></asp:Button>
                                                            </td>
                                                        </tr>
                                                        <tr height="15px">
                                                            <td valign="center" align="center">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                </td>
                                <td align="center">
                                </td>
                                <td align="left">
                                </td>
                                <td align="left" colspan="2">
                                    <%--Start: 'Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request --%>
                                    <table border="0" align="right" id="tblIDMcheck" runat="server" class="Table_WithoutBorder">
                                        <tr align="right">
                                            <td>
                                                <asp:CheckBox ID="chkIDM" Text="Copy Estimate to IDM" valign="down" CssClass="CheckBox_Normal"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <%--End: 'Anudeep:29.11.2012 Added forBt-623 YRS 5.0-848 : Annuity Estimates enhancement -IDM request --%>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    &nbsp;
                                </td>
                                <td align="center">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td align="right" colspan="2">
                                    <asp:Button ID="Buttoncalculate" runat="server" CssClass="Button_Normal" Width="80"
                                        Text="Calculate" OnClientClick="javascript:return ShowDialog('2');"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   <%-- MMR | 2017.02.27 | YRS-AT-2625 | Added javascript fucntion on click event to open dialog--%>
                                    <asp:Button ID="ButtonPrint" runat="server" CssClass="Button_Normal" Width="70" Text="Print">
                                    </asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummaryRetirementEstimate" DisplayMode="List" runat="server" CssClass="Error_Message" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
            <%--START: PPP | 11/05/2018 | YRS-AT-4183 | Applying changes for 01/01/2019--%>
            <%--START: PPP | 07/12/2017 | YRS-AT-3771 | Temp messages added which are applicable for Y-2017--%>
			<!-- temporary message IRS LIMITS -->
			<%--<asp:Label ID="LabelIRSLimits2017" runat="server" CssClass="Label_Small" style="display:none;">2017 IRS Limits are currently used for calculations.</asp:Label><br/>
			<asp:Label ID="LabelIRSLimits2018" runat="server" CssClass="Label_Small" style="display:none;">2018 IRS Limits will be used in calculations after January 1, 2018.</asp:Label><br/>--%>
            <%--END: PPP | 07/12/2017 | YRS-AT-3771 | Temp messages added which are applicable for Y-2017--%>
			<asp:Label ID="LabelIRSLimits2018" runat="server" CssClass="Label_Small" style="display:none;">2018 IRS Limits are currently used for calculations.</asp:Label><br/>
			<asp:Label ID="LabelIRSLimits2019" runat="server" CssClass="Label_Small" style="display:none;">2019 IRS Limits will be used in calculations after January 1, 2019.</asp:Label><br/>
            <%--END: PPP | 11/05/2018 | YRS-AT-4183 | Applying changes for 01/01/2019--%>

			
                        <asp:Label ID="LabelEstimateDataChangedMessage" runat="server" CssClass="Error_Message" Visible="False">Label</asp:Label><br>
                        <asp:Label ID="LabelWarningMessage" runat="server" CssClass="Error_Message" Visible="False">Label</asp:Label><br>
                        <asp:Label ID="LabelRefundMessage" runat="server" CssClass="Error_Message" Visible="False">Label</asp:Label><br>
                        <asp:Label ID="LabelAcctBalExceedTresholdLimit" runat="server" CssClass="Error_Message" Visible="False">Label</asp:Label>
                        <asp:Label ID="LabelMultipleEmpExists" runat="server" CssClass="Error_Message" Visible="False">Label</asp:Label>
                        <asp:Label ID="LabelPartialWithdrawal" runat="server" CssClass="Error_Message" Visible="False">Label</asp:Label>
                         <asp:Label ID="LabelDeathBenefitRestrictionMessage" runat="server" Visible="false"  CssClass="Error_Message"></asp:Label> <!--Chandra sekar | 2015.11.23 | YRS-AT-2610 - We need a conditional message in Retirement tab of estimates -->
                        <asp:Label ID="lblMessageManualTransaction" runat="server" CssClass="Error_Message" Visible="False">Label</asp:Label> <%-- MMR | 2017.03.01 | YRS-AT-2625 | Added label to display warning message for manual transaction link--%>
                       <%--START: SR | 11/26/2018 | YRS-AT-4106 | Temperory  messages removed since the problem resolved with this ticket--%>
                        <%--<asp:Label ID="lblStaticMessageForFutureSalary" runat="server" CssClass="Error_Message" Visible="True">Note: If you input a future salary effective date, the calculator will ignore it when determining whether compensation limit is reached; the calculator will assume the future salary is earned for the entire year and may reduce it unnecessarily. A fix is pending.</asp:Label>  SR | 2018.08.24 | YRS-AT-3790 | Added label to display temperory warning message untill future hotfix for this Gemini is addressed--%>
                       <%--END: SR | 11/26/2018 | YRS-AT-4106 | Temperory  messages removed since the problem resolved with this ticket--%>
                        <br/><asp:Label ID="LabelRDBWarningMessage" runat="server" CssClass="Error_Message" Visible="False">Label</asp:Label> <%--BD : 2018.10.31 : YRS-AT-4133 - label to display message for No Active Death Benefit if first enrolled on or after 1/1/2019--%>
                </tr>
                <tr>
                    <td align="left">
                        <table cellspacing="0" cellpadding="0" width="700" align="center" border="0">
                            <tbody>
                                <tr>
                                    <td>
                                        <iewc:TabStrip ID="tabStripRetirementEstimate" runat="server" Height="30px" Width="730"
                                            AutoPostBack="True" causesvalidation="true" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
                                            TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:79;height:21;text-align:center;">
                                            <iewc:Tab Text="Summary"></iewc:Tab>
                                            <iewc:Tab Text="Employment"></iewc:Tab>
                                            <iewc:Tab Text="Retirement"></iewc:Tab>
                                            <iewc:Tab Text="Accounts"></iewc:Tab>
                                        </iewc:TabStrip>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <iewc:MultiPage ID="MultiPageRetirementEstimate" runat="server">
                                            <iewc:PageView>
                                                <div class="Div_Center">
                                                    <table cellpadding="0" cellspacing="0" width="100%" height="200" class="Table_WithBorder"
                                                        border="0">
                                                        <tr>
                                                            <td align="left" class="td_Text" colspan="2">
                                                                Summary
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="430" rowspan="3">
                                                                <table cellpadding="0" cellspacing="0" width="430" align="center">
                                                                    <tr align="center" valign="middle" height="48">
                                                                        <td align="left" valign="middle" height="48">
                                                                            <div style="overflow: auto; width: 440px; border-top-style: none; border-right-style: none;
                                                                                border-left-style: none; position: static; height: 180px; border-bottom-style: none">
                                                                                <asp:DataGrid ID="DatatGridSocialSecurityLevel" Width="420" runat="server" CssClass="DataGrid_Grid">
                                                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                                </asp:DataGrid>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 7px" valign="middle">
                                                                            <asp:Label ID="LabelSSLWarningMessage" runat="server" Visible="false">**N/A = Not Available. Social Security Reduction would result in a negative payment.</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 7px" valign="middle">
                                                                            <%--START : ML | 2020.02.07 |YRS-AT-4769 | Message database driven--%>
                                                                            <%--<asp:Label ID="LabelJAnnuityUnAvailMessage" runat="server" Visible="false">*N/A = Due to the age difference between you and the Survivor you have selected, this option is not available.</asp:Label>--%>
                                                                            <asp:Label ID="LabelJAnnuityUnAvailMessage" runat="server" Visible="false"></asp:Label>
                                                                            <%--END : ML | 2020.02.07 |YRS-AT-4769 | Message database driven--%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td valign="top">
                                                                <div class="Div_Center" align="right">
                                                                    <table cellpadding="0" cellspacing="0" width="250" class="Table_WithBorder" align="right">
                                                                        <tr valign="top">
                                                                            <td colspan="2" class="Td_HeadingFormContainer">
                                                                                &nbsp;Social Security Leveling
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="top">
                                                                            <td align="left" width="60%">
                                                                                <asp:Label ID="LabelSSDecrease" Width="120" runat="server" CssClass="Label_Small">SS Benefit</asp:Label>
                                                                            </td>
                                                                            <td align="left" valign="middle">
                                                                                <asp:TextBox ID="TextboxFromBenefitValue" runat="server" CssClass="TextBox_Normal"
                                                                                    ReadOnly="false"></asp:TextBox>
                                                                            </td>
                                                                            <td align="right" valign="middle">
                                                                                <asp:TextBox ID="TextBoxPage1SSDecrease" runat="server" class="TextBox_Normal" Visible="false"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="top">
                                                                            <td align="left" valign="middle" width="60%">
                                                                                <asp:Label ID="LabelSSIncrease" runat="server" Width="120px" CssClass="Label_Small">SS Increase</asp:Label>
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <asp:TextBox ID="TextboxSSIncrease" runat="server" ReadOnly="true" CssClass="TextBox_Normal"></asp:TextBox>
                                                                            </td>
                                                                            <td align="right" valign="middle">
                                                                                <asp:TextBox ID="TextBoxPage1SSIncrease" runat="server" class="TextBox_Normal" Visible="false"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <div align="right">
                                                                    <table cellpadding="0" cellspacing="0" width="250" border="0" class="Table_WithBorder">
                                                                        <tr valign="top">
                                                                            <td align="left" width="60%">
                                                                                <asp:Label ID="lblProjectedReserves" Width="120px" runat="server" CssClass="Label_Small">Projected Reserves</asp:Label>
                                                                            </td>
                                                                            <td align="left" valign="middle">
                                                                                <asp:TextBox ID="txtProjectedReserves" runat="server" CssClass="TextBox_Normal" ReadOnly="True">0.00</asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="top">
                                                                            <td align="left" width="60%">
                                                                                <asp:Label ID="lblDeathBenefitUsed" Width="120px" runat="server" CssClass="Label_Small">Death Benefit Used</asp:Label>
                                                                            </td>
                                                                            <td align="right" valign="middle">
                                                                                <asp:TextBox ID="txtDeathBenefitUsed" runat="server" CssClass="TextBox_Normal" ReadOnly="True">0.00</asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <%--2012.07.11 SP : BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page -Start--%>
                                                        <tr>
                                                            <td valign="top">
                                                                <div align="right">
                                                                    <table cellpadding="0" cellspacing="0" width="250" border="0" class="Table_WithBorder">
                                                                        <tr valign="top">
                                                                            <td align="left" width="60%">
                                                                                <asp:Label ID="LabelProjFinalYrsSalary" Width="120px" runat="server" CssClass="Label_Small">Projected Final Year's Salary</asp:Label>
                                                                            </td>
                                                                            <td align="left" valign="middle">
                                                                                <asp:TextBox ID="TextBoxProjFinalYrsSalary" runat="server" CssClass="TextBox_Normal"
                                                                                    ReadOnly="True">0.00</asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <%--2012.07.11 SP : BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page -End--%>
                                                    </table>
                                                </div>
                                            </iewc:PageView>
                                            <iewc:PageView>
                                                <div class="Div_Center">
                                                    <table cellspacing="0" cellpadding="0" width="100%" class="Table_WithBorder">
                                                        <tr>
                                                            <td align="left" class="td_Text" colspan="2">
                                                                Employment History
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:DataGrid ID="DataGridEmployment" Width="670" runat="server" CssClass="DataGrid_Grid"
                                                                    AutoGenerateColumns="false">
                                                                    <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:TemplateColumn ItemStyle-Width="5%">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageButtonSelect" runat="server" ToolTip="Select" CommandName="Select"
                                                                                    CausesValidation="False" ImageUrl="images\select.gif" AlternateText="Select">
                                                                                </asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="PersID" ReadOnly="True" HeaderText="PersID" Visible="false" />
                                                                        <asp:BoundColumn DataField="guiYmcaID" ReadOnly="True" HeaderText="YmcaID" Visible="false" />
                                                                        <asp:BoundColumn DataField="YMCA" ReadOnly="True" HeaderText="YMCA" Visible="True" />
                                                                        <asp:BoundColumn DataField="Start" ReadOnly="True" HeaderText="Start" Visible="True" />
                                                                        <asp:BoundColumn DataField="End" ReadOnly="True" HeaderText="End" Visible="false" />
                                                                        <asp:BoundColumn DataField="guiEmpEventId" ReadOnly="True" HeaderText="End" Visible="false" />
                                                                        <asp:TemplateColumn HeaderText="End">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTempTerminationDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "End") %>'>  
                                                                                    
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">
                                                                &nbsp;&nbsp;&nbsp;
                                                                <div class="Div_Center">
                                                                    <table cellspacing="0" cellpadding="0" width="688" border="0">
                                                                        <tr>
                                                                            <td align="left" width="50%">
                                                                                <table cellpadding="0" cellspacing="0" width="100%" class="Table_WithoutBorder" border="0">
                                                                                    <tr>
                                                                                        <td align="left">
                                                                                            &nbsp;
                                                                                            <asp:Label ID="LabelModifyCurrentemployment" runat="server" Text="Modify Selected Employment Event"
                                                                                                CssClass="Label_Medium"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr valign="middle">
                                                                                        <td align="left">
                                                                                            <div class="Div_Center">
                                                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                                                    <!--Commented for YRS 5.0-445 Ashish
																														<tr valign="top">
																															<td align="left">&nbsp;
																																<asp:Label id="LabelSalaryAverage" cssclass="Label_Small" runat="server" text="Salary Average"></asp:Label>&nbsp;
																															</td>
																															<td align="left">
																																<asp:TextBox id="TextBoxSalaryAverage" runat="server" Width="90" cssclass="TextBox_Normal" ReadOnly="True"></asp:TextBox>&nbsp;Per 
																																Month
																															</td>
																														</tr>
																														-->
                                                                                                    <tr valign="top">
                                                                                                        <td align="left">
                                                                                                            &nbsp;
                                                                                                            <asp:Label ID="LabelModifiedSal" CssClass="Label_Small" runat="server">Salary Average</asp:Label>&nbsp;
                                                                                                        </td>
                                                                                                        <td align="left" colspan="2" nowrap>
                                                                                                            <asp:TextBox ID="TextBoxModifiedSal" Name="TextBoxModifiedSal" ReadOnly="true" runat="server"
                                                                                                                Width="90" CssClass="TextBox_Normal" onpaste="return false"></asp:TextBox>&nbsp;Per
                                                                                                            Month
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <!--Commented for YRS 5.0-445 Ashish old Label text - "Last Paid Month" rename to "Last Reported Compensation" - YREN-2634
																														<tr valign="top">
																															<td align="left" nowrap>&nbsp; 
																																
																																<asp:Label id="LabelLastPaidMonth" cssclass="Label_Small" runat="server" text="Last Reported Compensation"></asp:Label>&nbsp;
																															</td>
																															<td align="left">
																																<asp:TextBox id="TextBoxLastPaidSalary" runat="server" Width="90" cssclass="TextBox_Normal" ReadOnly="True"></asp:TextBox>
																																<asp:Label id="LabelLastPaidMonthDate" cssclass="Label_Small" runat="server"></asp:Label>
																															</td>
																														</tr>
																														-->
                                                                                                    <!--Commented for YRS 5.0-445:Ashish
																														<tr valign="top">
																															<td align="left">&nbsp;
																																<asp:Label id="LabelModifiedSal1" cssclass="Label_Small" runat="server">Modified Salary</asp:Label>&nbsp;
																															</td>
																															<td align="left">
																																<asp:TextBox id="TextBoxModifiedSal1" Name="TextBoxModifiedSal" runat="server" Width="90" cssclass="TextBox_Normal"
																																	onpaste="return false"></asp:TextBox>&nbsp;Per Month
																															</td>
																														</tr>
																														-->
                                                                                                    <tr valign="top">
                                                                                                        <td align="left">
                                                                                                            &nbsp;
                                                                                                            <asp:Label ID="LabelFutureSalary" CssClass="Label_Small" runat="server">Future Salary</asp:Label>&nbsp;
                                                                                                        </td>
                                                                                                        <td align="left" colspan="2" nowrap>
                                                                                                            <asp:TextBox ID="TextBoxFutureSalary" Name="TextBoxFutureSalary" onpaste="return false"
                                                                                                                runat="server" Width="90" CssClass="TextBox_Normal" value="0.00"></asp:TextBox>&nbsp;Per
                                                                                                            Month
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr valign="top">
                                                                                                        <td align="left" nowrap>
                                                                                                            &nbsp;
                                                                                                            <!--Label text - "Retirement Plan Eligibility Date" rename to "Future Salary Effective Date" - YRS 5.0-445 By Ashish-->
                                                                                                            <asp:Label ID="LabelFutureSalaryEffDate" CssClass="Label_Small" runat="server">Future Salary Effective Date</asp:Label>&nbsp;
                                                                                                        </td>
                                                                                                        <td align="left" colspan="2" >
                                                                                                             <!--Commented by CS: 12/14/2015 YRS-AT-2329 : For Changing the DateUserControl to Custom Calender Controls-->
                                                                                                        <%--   <uc1:DateUserControl ID="TextBoxFutureSalaryEffDate" FormatValidatorErrorMessage1="Invalid Date"
                                                                                                                runat="server" onpaste="return false"></uc1:DateUserControl>--%>

                                                                                                            <!--Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Adding new Future Salary Effective Date Custom Control -->
                                                                                                             <YRSCustomControls:CalenderTextBox ID="TextBoxFutureSalaryEffDate" MaxLength="10" runat="server"  Width="75"  CssClass="TextBox_Normal" EnableCustomValidator="true" IsFocusRequired="false" TrackDateChange="true" yearRange="1900:c+100"></YRSCustomControls:CalenderTextBox>
                                                                                                         <!--    </td>
                                                                                                   <td align="left">-->
                                                                                                            <asp:CustomValidator ID="CustomValidatorFutureSalaryDate" runat="server" CssClass="Error_Message"
                                                                                                                ErrorMessage="" OnServerValidate="ValidateFutureSalaryDate" ControlToValidate="TextBoxFutureSalaryEffDate" EnableClientScript="True"
                                                                                                                Display="Dynamic">*</asp:CustomValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr valign="top">
                                                                                                        <td align="left">
                                                                                                            &nbsp;
                                                                                                            <asp:Label ID="LabelStartWorkDate" CssClass="Label_Small" runat="server">Start Work date</asp:Label>&nbsp;
                                                                                                        </td>
                                                                                                        <td align="left" colspan="2">
                                                                                                            <asp:TextBox ID="TextBoxStartWorkDate" runat="server" Width="90" CssClass="TextBox_Normal"
                                                                                                                onpaste="return false" ReadOnly="True"></asp:TextBox>
                                                                                                        </td>
                                                                                                       
                                                                                                    </tr>
                                                                                                    <tr valign="top">
                                                                                                        <td align="left">
                                                                                                            &nbsp;
                                                                                                            <asp:Label ID="LabelEndWorkDate" CssClass="Label_Small" runat="server">End Work Date</asp:Label>&nbsp;
                                                                                                        </td>
                                                                                                        <td align="left" colspan="2">
                                                                                                              <!--Commented by CS: 12/14/2015 YRS-AT-2329 : For Changing the DateUserControl to Custom Calender Controls-->
                                                                                                         <%--   <uc1:DateUserControl ID="TextBoxEndWorkDate" runat="server" AutoPostBack="false"
                                                                                                                onpaste="return false"></uc1:DateUserControl> --%>
                                                                                                             <!--Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Adding new End Work Date Custom Control -->
                                                                                                              <YRSCustomControls:CalenderTextBox ID="TextBoxEndWorkDate" MaxLength="10" runat="server" CssClass="TextBox_Normal"  Width="75" EnableCustomValidator="true" IsFocusRequired="false" TrackDateChange="true" yearRange="1900:c+100"></YRSCustomControls:CalenderTextBox>
                                                                                                                <asp:CustomValidator ID="CustomValidatorEndWorkDate" runat="server" CssClass="Error_Message"
                                                                                                                ErrorMessage="" OnServerValidate="ValidateFutureSalaryDate" EnableClientScript="True" ControlToValidate="TextBoxEndWorkDate"
                                                                                                                Display="Dynamic">*</asp:CustomValidator>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr valign="top">
                                                                                                        <td align="left" nowrap>
                                                                                                            &nbsp;
                                                                                                            <asp:Label ID="LabelAnnualSalaryIncrease" CssClass="Label_Small" runat="server">Annual Salary Increase</asp:Label>&nbsp;
                                                                                                        </td>
                                                                                                        <td align="left" colspan="2">
                                                                                                            <asp:DropDownList ID="DropDownAnnualSalaryIncrease" name="DropDownAnnualSalaryIncrease"
                                                                                                                runat="Server" Width="96" Height="" CssClass="DropDown_Normal">
                                                                                                            </asp:DropDownList>
                                                                                                            &nbsp;%                                                                                                           
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr valign="top">
                                                                                                        <td align="left" nowrap>
                                                                                                            &nbsp;
                                                                                                            <asp:Label ID="LabelAnnualSalaryIncreaseMonth" CssClass="Label_Small" runat="server">Annual Salary Increase Month</asp:Label>&nbsp;
                                                                                                        </td>
                                                                                                        <td align="left" nowrap colspan="2">
                                                                                                              <!--Commented by CS: 12/14/2015 YRS-AT-2329 : For Changing the DateUserControl to Custom Calender Controls-->
                                                                                                           <%-- <uc1:DateUserControl ID="TextBoxAnnualSalaryIncreaseEffDate" FormatValidatorErrorMessage1="Invalid Date"
                                                                                                                runat="server" onpaste="return false"></uc1:DateUserControl>--%>
                                                                                                            <!--Added by chandrasekar.c on 2015.12.14 : YRS-AT-2329: Annuity Estimate Calculator - Adding new Annual salary Increase Date Custom Control -->
                                                                                                             <YRSCustomControls:CalenderTextBox ID="TextBoxAnnualSalaryIncreaseEffDate" MaxLength="10" runat="server"  Width="75"  CssClass="TextBox_Normal" EnableCustomValidator="true" IsFocusRequired="false" TrackDateChange="true" yearRange="1900:c+100"></YRSCustomControls:CalenderTextBox>
                                                                                                       <!-- </td>
                                                                                                        <td align="left">-->
                                                                                                            <asp:CustomValidator ID="CustomValidatorAnnualSalaryIncreaseEffDate" runat="server" CssClass="Error_Message"
                                                                                                                ErrorMessage="" OnServerValidate="ValidateFutureSalaryDate"  ControlToValidate="TextBoxAnnualSalaryIncreaseEffDate" EnableClientScript="True"
                                                                                                                Display="Dynamic">*</asp:CustomValidator>
                                                                                                             <!--START: Shilpa N | 04/08/2019 | YRS-AT-3392 | Added new cutom validator-->
                                                                                                            <asp:CustomValidator ID="CustomValidatorDDLAnnualSalaryIncreaseEffDate" runat="server" CssClass="Error_Message"
                                                                                                                ErrorMessage="" OnServerValidate="ValidateFutureSalaryDate"  ControlToValidate="DropDownAnnualSalaryIncrease" EnableClientScript="True"
                                                                                                                Display="Dynamic">*</asp:CustomValidator>
                                                                                                             <!--END: Shilpa N | 04/08/2019 | YRS-AT-3392 | Added new cutom validator-->
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td align="left" valign="top">
                                                                                <div class="Div_Center">
                                                                                    <table cellpadding="0" cellspacing="0" width="60%" border="0">
                                                                                        <tr>
                                                                                            <td colspan="2" align="left">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:Button ID="ButtonUpdateEmployment" runat="server" CssClass="Button_Normal" Text="Update Employment"
                                                                                                    CausesValidation="True"></asp:Button>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                            </td>
                                                                                            <td align="left">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <br>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </iewc:PageView>
                                            <iewc:PageView>
                                                <!-- Page 3 Start -->
                                                <div class="Div_Center">
                                                    <table cellspacing="0" cellpadding="0" width="100%" height="220" class="Table_WithBorder">
                                                        <tr>
                                                            <td colspan="2" align="left" valign="top" class="td_Text">
                                                                <asp:Label ID="LabelAnnuitantBeneficiaryR" runat="server">Existing Beneficiaries Details</asp:Label>
                                                            </td>
                                                            <td colspan="2" align="left" valign="top" class="td_Text">
                                                                <asp:Label ID="LabelAnnuitantBeneficiaryS" runat="server" Visible="false">Annuitant Beneficiary - Savings</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <%-- commented for issue 1507   <tr>
                                                            <td align="left">
                                                                &nbsp;
                                                                <asp:Label ID="LabelFirstName" runat="server" CssClass="Label_Small">First Name</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="TextboxFirstName" runat="server" CssClass="TextBox_Normal"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                &nbsp;
                                                                <asp:Label ID="LabelLastName" runat="server" CssClass="Label_Small">Last Name</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="TextboxLastName" runat="server" CssClass="TextBox_Normal"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                &nbsp;
                                                                <asp:Label ID="LabelBirthDate" runat="server" CssClass="Label_Small">Birth Date</asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="TextboxBeneficiaryBirthDate" runat="server" CssClass="TextBox_Normal"
                                                                    Enabled="True" ReadOnly="True"></asp:TextBox>
                                                                <rjs:PopCalendar ID="Popcalendar3" runat="server" Separator="/" Control="TextboxBeneficiaryBirthDate"
                                                                    Format="mm dd yyyy" ScriptsValidators="No Validate" Enabled="true"></rjs:PopCalendar>
                                                            </td>
                                                            <td colspan="2">
                                                            </td>
                                                        </tr>--%>
                                                        <%--  SP : 2012.02.24 - BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary --%>
                                                        <tr>
                                                            <td colspan="2" align="left" valign="top">
                                                                <asp:DataGrid ID="DataGridAnnuityBeneficiaries" runat="server" Width="380" CssClass="DataGrid_Grid"
                                                                    AutoGenerateColumns="false">
                                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                                    <Columns>
                                                                        <asp:TemplateColumn>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageButtonAccounts" runat="server" ToolTip="Select" CommandName="Select"
                                                                                    CausesValidation="False" ImageUrl="images\select.gif" AlternateText="Select">
                                                                                </asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn HeaderText="guiUniqueID" DataField="guiUniqueID" Visible="false" />
                                                                        <asp:BoundColumn HeaderText="SSN No." DataField="chrBeneficiaryTaxNumber" />
                                                                        <asp:BoundColumn HeaderText="FirstName" DataField="BenFirstName" />
                                                                        <asp:BoundColumn HeaderText="LastName" DataField="BenLastName" />
                                                                        <asp:BoundColumn HeaderText="BirthDate" DataField="BenBirthDate" DataFormatString="{0:MM/dd/yyyy}" />
                                                                        <asp:BoundColumn HeaderText="Rel" DataField="chvRelationshipCode" Visible="false" />
                                                                        <asp:BoundColumn HeaderText="Rel" DataField="Relationship" />
                                                                        <%--<asp:BoundColumn HeaderText="BeneficiaryTypeCode" DataField="chvBeneficiaryTypeCode"
																			Visible="false" />--%>
                                                                        <asp:BoundColumn HeaderText="Group" DataField="chvBeneficiaryGroupCode" />
                                                                        <%--	<asp:BoundColumn HeaderText="Pctg" DataField="intBenefitPctg" DataFormatString="{0:0.##}"
																			Visible="false" />--%>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                                <asp:Label ID="LabelNoBeneficiary" Text="No Beneficiary is defined" runat="server"
                                                                    Visible="false"></asp:Label>
                                                            </td>
                                                            <td colspan="2" align="right" valign="top">
                                                                <table border="0" cellpadding="0" cellspacing="0" class="Table_WithBorder">
                                                                    <tr>
                                                                        <td colspan='2' class="td_Text" align="left">
                                                                            Annuitant Beneficiary
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            &nbsp;
                                                                            <asp:Label ID="LabelFirstName" runat="server" CssClass="Label_Small">First Name</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TextboxFirstName" runat="server" CssClass="TextBox_Normal"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            &nbsp;
                                                                            <asp:Label ID="LabelLastName" runat="server" CssClass="Label_Small">Last Name</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TextboxLastName" runat="server" CssClass="TextBox_Normal"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            &nbsp;
                                                                            <asp:Label ID="LabelBirthDate" runat="server" CssClass="Label_Small">Birth Date</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%-- START: PPP | 2015.09.28 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) --%>
                                                                            <%-- 2012.05.21   SP   BT-976/YRS 5.0-1507 - Reopned issue--%>
                                                                            <%--<asp:TextBox ID="TextboxBeneficiaryBirthDate" MaxLength="10" runat="server" CssClass="TextBox_Normal"
                                                                                AutoPostBack="false"></asp:TextBox>
                                                                            <rjs:PopCalendar ID="Popcalendar3" runat="server" Separator="/" Control="TextboxBeneficiaryBirthDate"
                                                                                Format="mm dd yyyy"></rjs:PopCalendar>--%>
                                                                            <YRSCustomControls:CalenderTextBox ID="TextboxBeneficiaryBirthDate" MaxLength="10" runat="server" CssClass="TextBox_Normal" EnableCustomValidator="true" IsFocusRequired="false" TrackDateChange="true" yearRange="1900:c+100"></YRSCustomControls:CalenderTextBox>
                                                                            <%-- END: PPP | 2015.09.28 | YRS-AT-2596: allow typing in Birth Date field for annuity beneficiary(TrackIT 23788) --%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            &nbsp;
                                                                            <asp:Label ID="labelRelationship" runat="server" CssClass="Label_Small">Relation</asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList ID="DropDownRelationShip" Width="135px" CssClass="DropDown_Normal"
                                                                                runat="server" AutoPostBack="true">
                                                                                <asp:ListItem Text="Select" Value=""> </asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <%--2012.05.21   SP   BT-976/YRS 5.0-1507 - Reopned (adding tr tag with button --%>
                                                                        <td align="left">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Button ID="ButtonClearBeneficiary" runat="server" Text="Clear" CssClass="Button_Normal" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <%--  SP : 2012.02.24 - BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -End --%>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:Label ID="LabelDeathBenefit" runat="server" CssClass="Label_Medium">Death Benefit</asp:Label>
                                                            </td>
                                                            <%--<td>
                                                                &nbsp;
                                                            </td>--%>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <table border="0" cellpadding="0" cellspacing="0" align="left">
                                                                    <tr>
                                                                        <td align="left">
                                                                            &nbsp;
                                                                            <asp:Label ID="LabelRetiredDeathBenefit" runat="server" CssClass="Label_Small">Retired Death Benefit</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TextboxRetiredDeathBenefit" runat="server" ReadOnly="True" CssClass="TextBox_Normal"
                                                                                value="0.00" onpaste="return false"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            &nbsp;<asp:Label ID="LabelPercentageToUse" runat="server" CssClass="Label_Small">Percentage To Use</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList ID="DropdownlistPercentageToUse" runat="server" Width="136px" AutoPostBack="True"
                                                                                CssClass="DropDown_Normal">
                                                                            </asp:DropDownList>
                                                                           %
                                                                        </td>
                                                                        <td align="left">
                                                                           
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            &nbsp;
                                                                            <asp:Label ID="LabelAmountToUse" runat="server" CssClass="Label_Small">$ Amount To Use</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TextboxAmountToUse" runat="server" CssClass="TextBox_Normal" value="0.00"
                                                                                AutoPostBack="true" onpaste="return false" ReadOnly="true"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <!-- Page 3 End -->
                                            </iewc:PageView>
                                            <iewc:PageView>
                                                <!--  Accounts Start -->
                                                <div class="Div_Center">
                                                    <table cellspacing="1" cellpadding="0" width="100%" class="Table_WithBorder">
                                                        <tr>
                                                            <td colspan="4" align="left" class="td_Text">
                                                                Accounts
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="LabelEmployment" runat="server" CssClass="Label_Small">Employment</asp:Label>&nbsp;&nbsp;
                                                            </td>
                                                            <td align="left" colspan="3">
                                                                <asp:DropDownList ID="DropDownListEmployment" runat="server" Width="350" AutoPostBack="True"
                                                                    CssClass="DropDown_Normal">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="LabelMultipleEmp" runat="server" Visible="False" CssClass="Label_Small">Multiple active employments exist</asp:Label>
                                                                <asp:CustomValidator ID="CustomvalidatorDataGridElectiveAccounts" runat="server"
                                                                    Display="Dynamic" EnableClientScript="True" OnServerValidate="ValidateElectiveAccounts"
                                                                    ErrorMessage=""  CssClass="Error_Message">*</asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr bgcolor="#ffcc33">
                                                            <td colspan="4" align="left">
                                                                <!-- <asp:CheckBox Text="Retirement Plan" id="CheckBoxRetirementPlan" cssclass="CheckBox_Normal" runat="server"
																						AutoPostBack="True" visible="true"></asp:CheckBox> -->
                                                                <asp:Label ID="LabelRetirementPlan" runat="server" CssClass="Label_Small" BackColor="#ffcc33"
                                                                    Visible="True">Retirement Plan</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="width:100%" align="left">
                                                            <td colspan="2" style="width:50%">
                                                                <asp:CheckBox runat="server" ID="chkRetirementAccount" Text="Partial Amount" CssClass="Label_Small"  AutoPostBack="true"/>
                                                                &nbsp;
                                                                <asp:TextBox runat="server" CssClass="TextBox_Normal" ID="txtRetirementAccount" AutoPostBack="true" Enabled="false" oncopy="return false" onpaste="return false" oncut="return false"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2" style="width:50%"  align="left">
                                                                &nbsp;<asp:Label ID="lblRetirementPartialAmountEligible" runat="server" Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" align="left">
                                                                <asp:DataGrid ID="DatagridElectiveRetirementAccounts" runat="server" Visible="True"
                                                                    Width="100%" ShowFooter="True">
                                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    <Columns>
                                                                        <asp:TemplateColumn>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox Text="" ID="CheckboxRet" runat="server" AutoPostBack="True" OnCheckedChanged="DatagridElectiveRetirementAccounts_OnCheckedChanged">
                                                                                </asp:CheckBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="chrAcctType" ReadOnly="True" HeaderText="Account Type" />
                                                                        <asp:BoundColumn DataField="AcctTotal" ReadOnly="True" HeaderText="Account Total"
                                                                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" />
                                                                        <asp:TemplateColumn>
                                                                            <HeaderTemplate>
                                                                                <b>Projected Balance</b>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="LabelProjectedBalRet" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "mnyProjectedBalance","{0:0.00}") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="LabelProjectedTotalBalRet" runat="server" CssClass="Label_Small">0 </asp:Label>
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                            <FooterStyle HorizontalAlign="Right" CssClass="Label_Small"></FooterStyle>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="ExistingCntType" ReadOnly="True" HeaderText="Existing Contri."
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundColumn DataField="ExistingCntRate" ReadOnly="True" HeaderText="Contri. Rate"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:TemplateColumn>
                                                                            <HeaderTemplate>
                                                                                <b>ContributionType </b>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:DropDownList ID="DropdownlistContribTypeRet" runat="server" Width="120" CssClass="DropDown_Normal">
                                                                                    <asp:ListItem Value="">Select Contribution</asp:ListItem>
                                                                                    <asp:ListItem Value="M">Dollar</asp:ListItem>
                                                                                    <asp:ListItem Value="P">Percent</asp:ListItem>
                                                                                    <asp:ListItem Value="L">Lump Sum</asp:ListItem>
                                                                                    <asp:ListItem Value="Y">Annual Lump Sum</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn>
                                                                            <HeaderTemplate>
                                                                                <b>Contribution </b>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="TextboxContribAmtRet" Name="TextboxContribAmt" runat="server" Width="70"
                                                                                    CssClass="TextBox_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "mnyAddlContribution") %>'
                                                                                    onpaste="return false" onkeypress="ValidateDecimal()">
                                                                                </asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn>
                                                                            <HeaderTemplate>
                                                                                <b>Start Date </b>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <uc1:DateUserControl ID="DateusercontrolStartRet" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "dtmEffDate") %>'
                                                                                    onpaste="return false"></uc1:DateUserControl>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn>
                                                                            <HeaderTemplate>
                                                                                <b>Stop Date </b>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <uc1:DateUserControl ID="DateusercontrolStopRet" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "dtsTerminationDate") %>'
                                                                                    onpaste="return false"></uc1:DateUserControl>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="guiEmpEventID" Visible="False" ReadOnly="True" HeaderText="EmpEventId" />
                                                                        <asp:BoundColumn DataField="bitRet_Voluntary" Visible="False" ReadOnly="True" HeaderText="VoluntaryAccount" />
                                                                        <asp:BoundColumn DataField="bitBasicAcct" Visible="False" ReadOnly="True" HeaderText="BasicAccount" />
                                                                        <asp:BoundColumn DataField="PlanType" Visible="False" ReadOnly="True" HeaderText="PlanType" />
                                                                        <asp:BoundColumn DataField="YMCATotal" Visible="False" ReadOnly="True" HeaderText="YMCATotal" />
                                                                        <asp:BoundColumn DataField="PersonalTotal" Visible="False" ReadOnly="True" HeaderText="PersonalTotal" />
                                                                        <asp:BoundColumn DataField="chrLegacyAcctType" Visible="False" ReadOnly="True" HeaderText="LegacyAcctType" />
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr bgcolor="#ffcc33">
                                                            <td colspan="4" align="left">
                                                                <!-- <asp:CheckBox Text="Tax Deferred Savings Plan" id="CheckboxSavingsPlan" runat="server" cssclass="CheckBox_Normal"
																						AutoPostBack="True" visible="True"></asp:CheckBox>-->
                                                                <asp:Label ID="LabelSavingsPlan" runat="server" CssClass="Label_Small" BackColor="#ffcc33"
                                                                    Visible="True">Savings Plan</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr  align="left" style="width:100%">
                                                            <td colspan="2" style="width:50%">
                                                                <asp:CheckBox runat="server" ID="chkSavingPartialAmount" Text="Partial Amount" CssClass="Label_Small"  AutoPostBack="true"/>
                                                                &nbsp;
                                                                <asp:TextBox runat="server" CssClass="TextBox_Normal" ID="txtSavingPartialAmount" AutoPostBack="true" Enabled="false" oncopy="return false" onpaste="return false" oncut="return false"></asp:TextBox>
                                                            </td>
                                                            <td colspan="2" style="width:50%"  align="left">
                                                                &nbsp;<asp:Label ID="lblSavingPartialAmountEligible" runat="server" Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:DataGrid ID="DatagridElectiveSavingsAccounts" runat="server" CssClass="DataGrid_Grid"
                                                                    Width="100%" Visible="True" ShowFooter="True">
                                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    <Columns>
                                                                        <asp:TemplateColumn>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox Text="" ID="CheckboxSav" runat="server" AutoPostBack="True" OnCheckedChanged="DatagridElectiveSavingsAccounts_OnCheckedChanged">
                                                                                </asp:CheckBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="chrAcctType" ReadOnly="True" HeaderText="Account Type" />
                                                                        <asp:BoundColumn DataField="AcctTotal" ReadOnly="True" HeaderText="Account Total"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:TemplateColumn>
                                                                            <HeaderTemplate>
                                                                                <b>Projected Balance</b>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="LabelProjectedBalSav" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "mnyProjectedBalance") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="LabelProjectedTotalBalSav" runat="server" CssClass="Label_Small">0 </asp:Label>
                                                                            </FooterTemplate>
                                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                            <FooterStyle HorizontalAlign="Right" CssClass="Label_Small"></FooterStyle>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="ExistingCntType" ReadOnly="True" HeaderText="Existing Contri."
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:BoundColumn DataField="ExistingCntRate" ReadOnly="True" HeaderText="Contri. Rate"
                                                                            ItemStyle-HorizontalAlign="Right" />
                                                                        <asp:TemplateColumn>
                                                                            <HeaderTemplate>
                                                                                <b>ContributionType </b>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:DropDownList ID="DropdownlistContribTypeSav" runat="server" Width="120" CssClass="DropDown_Normal">
                                                                                    <asp:ListItem Value="">Select Contribution</asp:ListItem>
                                                                                    <asp:ListItem Value="M">Dollar</asp:ListItem>
                                                                                    <asp:ListItem Value="P">Percent</asp:ListItem>
                                                                                    <asp:ListItem Value="L">Lump Sum</asp:ListItem>
                                                                                    <asp:ListItem Value="Y">Annual Lump Sum</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn>
                                                                            <HeaderTemplate>
                                                                                <b>Contribution </b>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="TextboxContribAmtSav" Name="TextboxContribAmt" runat="server" Width="70"
                                                                                    CssClass="TextBox_Normal" Text='<%# DataBinder.Eval(Container.DataItem, "mnyAddlContribution") %>'
                                                                                    onpaste="return false" onkeypress="ValidateDecimal()">
                                                                                </asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn>
                                                                            <HeaderTemplate>
                                                                                <b>Start Date </b>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <uc1:DateUserControl ID="DateusercontrolStartSav" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "dtmEffDate") %>'
                                                                                    onpaste="return false"></uc1:DateUserControl>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn>
                                                                            <HeaderTemplate>
                                                                                <b>Stop Date </b>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <uc1:DateUserControl ID="DateusercontrolStopSav" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "dtsTerminationDate") %>'
                                                                                    onpaste="return false"></uc1:DateUserControl>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="guiEmpEventID" Visible="False" ReadOnly="True" HeaderText="Emp Event Id" />
                                                                        <asp:BoundColumn DataField="bitRet_Voluntary" Visible="False" ReadOnly="True" HeaderText="VoluntaryAccount" />
                                                                        <asp:BoundColumn DataField="bitBasicAcct" Visible="False" ReadOnly="True" HeaderText="BasicAccount" />
                                                                        <asp:BoundColumn DataField="PlanType" Visible="False" ReadOnly="True" HeaderText="PlanType" />
                                                                        <asp:BoundColumn DataField="chrLegacyAcctType" Visible="False" ReadOnly="True" HeaderText="LegacyAcctType" />
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <!-- Account End -->
                                            </iewc:PageView>
                                        </iewc:MultiPage>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="td_Text">
                                        <asp:Button ID="ButtonCancel" runat="server" CssClass="Button_Normal" Width="70"
                                            Text="Exit" CausesValidation="False"></asp:Button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:HiddenField runat="server" ID="hdnRetirementTotal" />
                        <asp:HiddenField runat="server" ID="hdnSavingTotal" />
                        <asp:HiddenField runat="server" ID="hdnBitIsPartial" />
                        <asp:HiddenField runat="server" ID="hdnBitIsSaving" />
                        <asp:HiddenField runat="server" ID="hdnDecPartialValue" />
                        <asp:HiddenField runat="server" ID="hdnDecSavingValue" />
                        <asp:HiddenField runat="server" ID ="hdnManualTransaction" Value="1" /> <%-- MMR | 2017.03.01 | YRS-AT-2625 | Added hidden field with default value as 1 indicating manual transaction not exist--%>
                        <asp:HiddenField runat="server" ID ="hdnSourceManualTransaction" Value="1" /> <%-- MMR | 2017.03.01 | YRS-AT-2625 | Added hidden field for manual Transaction link--%>
                    </td>
                </tr>
            </tbody>
        </table>
        </TD></TR></TBODY></TABLE>
        <table width="740">
            <tr>
                <td>
                    <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server">
                    </YRSControls:YMCA_Footer_WebUserControl>
                </td>
            </tr>
        </table>
    </div>
    <td>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </td>
        <%--START: MMR | 2017.02.22 | YRS-AT-2625 |Add Grid to display manual transaction list--%>
        <div id="divTransactionList" runat="server" style="display:none;overflow:auto;" >
            <div>
            <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td class="Label_Small">The following manual transactions exist.<br />Please select the ones which are to be considered for computing average salary prior to retirement. This average is used to project contributions and interest through age 60.
                    </td>
                </tr>
                <tr><td height="10px"></td></tr> <%--Added TR element for space between text and grid--%>
                <tr>
                    <td>
                        <div style="overflow:auto;height: 150px">
                            <asp:DataGrid ID="DatagridManualTransactionList" runat="server" Visible="True" Width="95%" AutoGenerateColumns="false">
                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                <Columns>
                                    <asp:TemplateColumn ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAccountTypeHeader" runat="server" Checked="true">
                                            </asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate >
                                            <asp:CheckBox ID="chkAccountTypeRow" runat="server" Checked='<%#Eval("Selected")%>'>
                                            </asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>                                 
                                    <asp:BoundColumn DataField="UniqueId" ReadOnly="True" HeaderText="UniqueID" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn"/>
                                    <asp:BoundColumn DataField="AccountType" ReadOnly="True" HeaderText="Account Type" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="TransactType" ReadOnly="True" HeaderText="Transact Type" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" Visible="false"/>
                                    <asp:BoundColumn DataField="MonthComp" ReadOnly="True" HeaderText="Month Comp." ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" HeaderStyle-HorizontalAlign="Center"/>                                                                       
                                    <asp:BoundColumn DataField="PersonalPreTax" ReadOnly="True" HeaderText="Personal Pre Tax" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="PersonalPostTax" ReadOnly="True" HeaderText="Personal Post Tax" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="YmcaPreTax" ReadOnly="True" HeaderText="Ymca Pre Tax" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="TransactDate" ReadOnly="True" HeaderText="Transact Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="ReceivedDate" ReadOnly="True" HeaderText="Received Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"/>
                                    <asp:BoundColumn DataField="FundedDate" ReadOnly="True" HeaderText="Funded Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center"/>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                  </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;">
                        <input type="button" name="btnTransactionListOk" value="  OK  " class="Button_Normal" onclick="getSelectedManualTransaction();" />&nbsp;
                        <input type="button" name="btnTransactionListCancel" value="Cancel" class="Button_Normal" onclick="CloseDialog()" />
                    </td>
                </tr>
            </table>
        </div>
     </div>
    <%--END: MMR | 2017.02.22 | YRS-AT-2625 |Add Grid to display manual transaction list--%>
    </form>
</body>
        <%--START: PPP | 11/05/2018 | YRS-AT-4183 | Applying changes for 01/01/2019--%>
        <%--START: PPP | 07/12/2017 | YRS-AT-3771 | JS Function will check the date if it belongs to Y-2017 and annual limits warnings are appearing on screen then only new temp messages will be displayed--%>
        <script language="javascript" type="text/javascript">
            function CheckMessagesTemp() {
                var today = new Date();
                <%--START: PPP | 11/05/2018 | YRS-AT-4183 | Applying changes for 01/01/2019--%>
                <%--
                var startOf2018 = new Date(2018, 0, 1); // Jan 01, 2018
                $("#< %=LabelIRSLimits2017.ClientID%>").hide();
                $("#< %=LabelIRSLimits2018.ClientID%>").hide();

                if (today < startOf2018) {
                    if ($("#< %=LabelWarningMessage.ClientID%>").length > 0) {
                        var messages = $("#< %=LabelWarningMessage.ClientID%>").html();
                        if (messages.toLowerCase().indexOf("maximum annual salary") >= 0) {
                            $("#< %=LabelIRSLimits2017.ClientID%>").show();
                            $("#< %=LabelIRSLimits2018.ClientID%>").show();
                        }
                        else if (messages.toLowerCase().indexOf("maximum annual tax-deferred") >= 0) {
                            $("#< %=LabelIRSLimits2017.ClientID%>").show();
                            $("#< %=LabelIRSLimits2018.ClientID%>").show();
                        }
                        else if (messages.toLowerCase().indexOf("maximum annual contribution") >= 0) {
                            $("#< %=LabelIRSLimits2017.ClientID%>").show();
                            $("#< %=LabelIRSLimits2018.ClientID%>").show();
                        }
                    }
                }
                --%>
                var startOf2019 = new Date(2019, 0, 1); // Jan 01, 2019
                $("#<%=LabelIRSLimits2018.ClientID%>").hide();
                $("#<%=LabelIRSLimits2019.ClientID%>").hide();

                if (today < startOf2019) {
                    if ($("#<%=LabelWarningMessage.ClientID%>").length > 0) {
                        var messages = $("#<%=LabelWarningMessage.ClientID%>").html();
                        if (messages.toLowerCase().indexOf("maximum annual salary") >= 0) {
                            $("#<%=LabelIRSLimits2018.ClientID%>").show();
                            $("#<%=LabelIRSLimits2019.ClientID%>").show();
                        }
                        else if (messages.toLowerCase().indexOf("maximum annual tax-deferred") >= 0) {
                            $("#<%=LabelIRSLimits2018.ClientID%>").show();
                            $("#<%=LabelIRSLimits2019.ClientID%>").show();
                        }
                        else if (messages.toLowerCase().indexOf("maximum annual contribution") >= 0) {
                            $("#<%=LabelIRSLimits2018.ClientID%>").show();
                            $("#<%=LabelIRSLimits2019.ClientID%>").show();
                        }
                    }
                }
                <%--END: PPP | 11/05/2018 | YRS-AT-4183 | Applying changes for 01/01/2019--%>
            }
        </script>
        <%--END: PPP | 07/12/2017 | YRS-AT-3771 | JS Function will check the date if it belongs to Y-2017 and annual limits warnings are appearing on screen then only new temp messages will be displayed--%>
        <%--END: PPP | 11/05/2018 | YRS-AT-4183 | Applying changes for 01/01/2019--%>

</html>
