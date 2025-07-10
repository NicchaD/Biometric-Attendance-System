<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BeneficiaryInformation.aspx.vb"
    Inherits="YMCAUI.BeneficiaryInformation" EnableEventValidation="false"  %> <%--PK |12/05/2019| YRS-AT-4605 | Added EnableEventValidation= false attribute --%>

<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="YRSControls" TagName="AddressWebUserControl" Src="UserControls/AddressUserControl_new.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_ToolBar_WebUserControl" Src="~/UserControls/YMCA_Toolbar_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="~/UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="YRSStateTaxControls" TagName="StateWithholdingListing_WebUserControl" Src="~/UserControls/StateWithholdingListingControl.ascx" %><%-- PK |10.15.2019| YRS-AT-4605 |Added reference of Statewithholding User control --%>
<html>
<head>
    <script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
    <script language="javascript" src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>
    <link id="Link1" href="CSS/CustomStyleSheet.css" type="text/css" runat="server" rel="stylesheet" />

    <script type="text/javascript" language="javascript">
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
        function ValidateRange(objControl, minvalue, maxvalue) {
            if (objControl.value < minvalue) {
                alert("Beyond range. Less than Minimum Value");
                //	objControl.focus()
                return false;

            }
            if (objControl.value > maxvalue * 1) {
                alert("Beyond range. Greater than Maximum Value");
                //	objControl.focus()
                return false
            }
        }

        /*
        This function is responsible for filtering the keys pressed and the maintain the amount format of the 
        value in the Text box
        */
        function HandleAmountFilteringWithNoDecimals(ctl) {
            var iKeyCode, objInput;
            var iMaxLen
            var reValidChars = /[0-9]/;
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
        <%--Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Added function to validate incorrect date input--%>
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
                            return false;
                        }
                        else {
                            if (cmpDate == "NaN/NaN/NaN") {                               
                                return false;
                            }
                        }
                    }
                    return true;
                    break;


                case ("DD/MM/YYYY"):                    
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
                            return false;
                        }
                        else {
                            if (cmpDate == "NaN/NaN/NaN") {                             
                                return false;
                            }
                        }
                    }
                    return true;
                    break;

            }
        }
        <%--End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Added function to validate incorrect date input--%>        
        $(document).ready(function () {
            $("div#config").hide();
            //toggle the componenet with class msg_body
            $("#Expand").click(function () {
                $("div#Expand").hide();
                $("div#Collapse").show();
                $("div#config").slideToggle(500);
            });
            $("#Collapse").click(function () {
                $("div#Collapse").hide();
                $("div#Expand").show();
                $("div#config").slideToggle(500);
            });
        });
        function ValidateNumeric() {
            if ((event.keyCode < 48) || (event.keyCode > 57)) {
                event.returnValue = false;
            }
        }
        function ValidateTelephoneNo(str) {

            if (str.id == "TextBoxTelephone") {
                if (document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "US" || document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "CA") {
                    var val = str.value
                    if (((val.length < 10) || (val.length > 10)) && (val.length != 0)) {
						<%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                        alert('Telephone number must be 10 digits.')--%>
                        alert('Please provide valid Telephone number.')
						<%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
                        str.focus()
                    }
                }
            }
            else if (str.id == "TextBoxSecTelephone") {
                if (document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value == "US" || document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value == "CA") {
                    var val = str.value
                    if (((val.length < 10) || (val.length > 10)) && (val.length != 0)) {
						<%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                        alert('Telephone number must be 10 digits.')--%>
                        alert('Please provide valid Telephone number.')
						<%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
                        str.focus()
                    }
                }
            }

        }
        function Validatephone(str) {
            if (str.id == "TextBoxTelephone") {
                if (document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "US" || document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value == "CA") {
                    str.maxLength = 10;
                }
                else {
                    str.maxLength = 25;
                }
            }
            else if (str.id == "TextBoxSecTelephone") {
                if (document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value == "US" || document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value == "CA") {
                    str.maxLength = 10;
                }
                else {
                    str.maxLength = 25;
                }
            }
        }
        //Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
        $(document).ready(function () {
            $("#divWSMessage").dialog
                            ({
                                modal: true,
                                open: function (event, ui) { $(this).parent('div').find('button:contains("SAVE")').focus(); },
                                autoOpen: false,
                                title: "Process Restricted",
                                width: 570, height: 200,
                                position: ['center', 170],
                                buttons: [{ text: "OK", click: CloseWSMessage }]
                            });
           <%--Start - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
            $('#divDeductions').dialog
                ({
                    autoOpen: false,
                    resizable: false,
                    draggable: true,
                    closeOnEscape: false,
                    close: false,
                    width: 285, height: 525,
                    title: "Deductions",
                    modal: true,
                    buttons: [{ text: "Save", click: SaveDeduction }, { text: "Cancel", click: CloseDeductionsDialog }],
                    open: function (type, data) {
                        $(this).parent().appendTo("form");
                        $('a.ui-dialog-titlebar-close').remove();
                    }                   
                });
            <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
        });

        function CloseWSMessage() {
            $(document).ready(function () {
                $("#divWSMessage").dialog('close');
            });
        }

        function openDialog(str, type) {
            $(document).ready(function () {
                if (type == 'Bene') {
                    str = 'Beneficiary add,edit and delete operation can not be performed due to following reason(s).<br/>' + str
                }
                else {
                    str = 'Beneficiary Settlement process can not be performed due to following reasons(s).<br/>' + str;
                }
                $("#divWSMessage").html(str);
                $("#divWSMessage").dialog('open');
                return false;
            });
        }
        //End,Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)

        <%--Start - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>     
        function ShowDeductionDialog(type) {
            $('#divDeductions').dialog('open');          
        }     

        function ValidateNumeric() {
            if (!(event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57))
            { event.returnValue = false; }
        }

        function CloseDeductionsDialog() {
            $("#divDeductions").dialog('close');
            HideMessage();
            ClearValues();
        }

        function EnableTextbox(rownumber) {           
            var txt = $($($("#dgDeductions input[id*='chkBoxDeduction']:checkbox")[rownumber]).closest('tr').find('input[id*="txtFundCostAmt"]'));           
            txt.val('');
            if (txt.is(':disabled')) {
                txt.removeAttr('disabled');
            }
            else {
                txt.attr('disabled', true);
            }           
        }

        function ShowMessage(strMessage, type) {

            $("#divMessage").html(strMessage);
            $("#divMessage").css('display', 'block');
            if (type == "error") {
                $("#divMessage")[0].className = "error-msg";
            }
        }

        function HideMessage() {
            $("#divMessage").css('display', 'none');
        }

        // Show validation message and save deductions on save button in dialog
        function SaveDeduction() {
            HideMessage();           
            if (!($("#<%=dgDeductions.ClientID%> input[id*='txtFundCostAmt']:text").is(':disabled')) && ($("#<%=dgDeductions.ClientID%> input[id*='txtFundCostAmt']:text").val() == "" || $("#<%=dgDeductions.ClientID%> input[id*='txtFundCostAmt']:text").val() == "0.00")) {
                ShowMessage('Please provide Fund Costs', 'error');
                return;
            }
            getSelectedDeductions();
            getTotalDeductions();
        }

        function ClearValues() {
            $("#<%=dgDeductions.ClientID%> input[id*='chkBoxDeduction']:checkbox:checked").removeAttr('checked');
            $("#<%=dgDeductions.ClientID%> input[id*='txtFundCostAmt']:text").val('');          
            $("#<%=dgDeductions.ClientID%> input[id*='txtFundCostAmt']:text").prop('disabled', true);
            
        }

       // Display total amt of selected deductions from grid in label
        function getTotalDeductions() {
            var totDed = 0.00;            
            var lblAmt;
            var checkedCheckboxes = $("#<%=dgDeductions.ClientID%> input[id*='chkBoxDeduction']:checkbox:checked");
            checkedCheckboxes.each(function (chk) {
                var txtFundcost = $($(checkedCheckboxes[chk]).closest('tr').find('input[id*="txtFundCostAmt"]:text'));
                if (typeof (txtFundcost[0]) != "undefined" && !(txtFundcost.is(':disabled'))) {
                    totDed = parseFloat(totDed) + parseFloat(txtFundcost.val());
                }
                else {
                    lblAmt = $($(checkedCheckboxes[chk]).closest('tr').find('span[id*="lblAmount"]'));
                    totDed = parseFloat(totDed) + parseFloat(lblAmt.text());
                }
            });
            
            $("#lblDeductionamt").text(parseFloat(totDed).toFixed(2).toString());
            CloseDeductionsDialog();
        }

        // Display selected deductions in grid previously selected for first time on click of deductions link
        function selectedDedcutionValues(str)
        {
            if (str != null){
                var strSplit = str.substring(0, str.length - 1).split("#");
 
                for (var i = 0 ; i < strSplit.length; i++) {
                    if (!(strSplit[i] == '')) {
                        if (strSplit[i].indexOf("|") > - 1) {
                            var strtextfundcost = strSplit[i].split("|");
                            $("#" + strtextfundcost[0]).attr('checked', 'true');
                            var txtFndcosts =$($("#" + strtextfundcost[0]).closest('td').next('td').next('td').find('input[id*="txtFundCostAmt"]:text'));                            
                            txtFndcosts.prop('disabled', false);
                            txtFndcosts.val(strtextfundcost[1]);
                        }
                        else {
                            $("#" + strSplit[i]).attr('checked', 'true');
                        }
                    }
                }
            }
            ShowDeductionDialog('open');
        }

        //Concatenating all the selected deductions in grid in a string variable and accessing it through web method
        function getSelectedDeductions() {
            var checkedChkboxes = $("#<%=dgDeductions.ClientID%> input[id*='chkBoxDeduction']:checkbox:checked");           
            var getDedVal = "";
            var getappendedDedVal = "";
            var getValue = "";
            checkedChkboxes.each(function (chk) {                        
                var getDesc = $($(checkedChkboxes[chk]).closest('td').next('td')).text();
                var txtFundcost = $($(checkedChkboxes[chk]).closest('tr').find('input[id*="txtFundCostAmt"]:text'));
                if (typeof (txtFundcost[0]) != "undefined" && !(txtFundcost.is(':disabled'))) {
                    getValue = txtFundcost.val();
                }
                else {
                    getValue = $($(checkedChkboxes[chk]).closest('td').next('td').next('td')).text();

                }
                var getChkID = $(this).attr('id');
                if (getDesc == 'Fund Costs') {
                    getDedVal = getChkID + ":" + getDesc + ":" + getValue;
                }
                else
                {
                    getDedVal = getChkID + ":" + getDesc;
                }
                
                    getappendedDedVal += getDedVal.concat("", "##");                
            });
          
            var strDedVal = getappendedDedVal.substring(0,getappendedDedVal.length - 2);          
            $.ajax({
                type: "POST",
                url: "BeneficiaryInformation.aspx/SaveDeductionValues",
                data: "{'strDeductionval':'" + strDedVal + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    CloseDeductionsDialog();
                        
                },
                 failure: function (msg) {
                     ShowMessage(msg.d, "error");                     
                 }
             });
        }


        //Calling web method to get values for selected deductions in grid
        function getSelectedDedValues() {
            $.ajax({
                type: "POST",
                url: "BeneficiaryInformation.aspx/GetSelectedDeductionVal",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var str = msg.d;
                    selectedDedcutionValues(str);                   
                },
                failure: function (msg) {
                    ShowMessage(msg.d, "error");
                }
            });
        }
            <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>

        <%--START: SB | 2018.05.28 | YRS-AT-3922 -  YRS bug - Death Settlement double-click causes multiple bitActive and bitPrimary set for same Address --%>
        $(document).ready(function () {
            $("#ConfirmDialog").dialog
					({
					    modal: true,
					    open: function (event, ui) { $(this).parent('div').find('button:contains("SAVE")').focus(); },
					    autoOpen: false,
					    title: "YMCA YRS - Information",
					    width: 500, height: 190
					});
            OpenProgressDialog();
        });

        function OpenProgressDialog() {
            $('#divProgress').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 350, height: 150,
                title: "Saving Information",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }

        function ShowProcessingDialog() {
            $("#divProgress").dialog('open');
            $('#labelMessage').text('Please wait while information is being saved.');
        }

        function callProgressDialogDiv() {
            ShowProcessingDialog();  
            return true;
        }
        <%--END: SB | 2018.05.28 | YRS-AT-3922 -  YRS bug - Death Settlement double-click causes multiple bitActive and bitPrimary set for same Address --%>

    </script>

</head>
<body>
    <form id="Form1" method="post" runat="server">  
       
         <div class="Div_Center">
            <!--SR:2014.07.15 -BT 2593: UI changes in Beneficiary information page. Changes made throghout the page -->
            <table class="Table_WithoutBorder" cellspacing="0" width="100%">
                <tr>
                    <td class="Td_HeadingFormContainer" align="left">
                        <YRSControls:YMCA_ToolBar_WebUserControl ID="Toolbar_Control" runat="server" ShowLogoutLinkButton="false"
                            ShowHomeLinkButton="false" ShowReleaseLinkButton="false" />
                        <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                    </td>
                </tr>
                <tr>
                    <td class="Td_BackGroundColorMenu" align="left">
                        <cc1:Menu ID="MenuRetireesInformation" runat="server" mouseovercssclass="MouseOver"
                            MenuFadeDelay="1" zIndex="100" DefaultMouseUpCssClass="mouseup" DefaultMouseOverCssClass="mouseover"
                            DefaultMouseDownCssClass="mousedown" DefaultCssClass="menuitem" CssClass="menustyle"
                            Width="780px" Font-Names="Verdana" Cursor="Pointer" ItemSpacing="0" ItemPadding="4"
                            HighlightTopMenu="True" Layout="Horizontal">
                        </cc1:Menu>
                    </td>
                </tr>
                <%--START: PK |12/05/2019 |YRS-AT-4605 |Commenting this tr as this is creating extra space.--%>
                <%--<tr>
                    <td>
                       &nbsp;
                    </td>
                </tr>--%>
                <%--END: PK |12/05/2019 |YRS-AT-4605 |Commenting this tr as this is creating extra space.--%>
            </table>
         <div id="divErrorMsg" class="error-msg" runat="server" style="text-align: left;" enableviewstate="false" visible="false"></div> <%--START: PK |12/05/2019 |YRS-AT-4605 |Added this div to display an error message in header.--%>
        <div class="Div_Center">
            <table id="TableContainer" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td width="100%">
                            <iewc:TabStrip ID="TabStripRetireesInformation" runat="server" BorderStyle="None"
                                AutoPostBack="True" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;text-align:center;border:solid 1px White;border-bottom:none"
                                TabHoverStyle="background-color:#93BEEE;color:#4172A9;text-align:center" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
                                Width="400px" Height="30px">
                                <iewc:Tab Text=" General" ID="TabStripTabGeneral"></iewc:Tab>
                                 <%--START : PK |10.04.2019| YRS-AT-4605 |Change Tab Display Text  --%>
                               <%-- <iewc:Tab Text=" Withholding" ID="TabStripTabGeneralWithholding"></iewc:Tab>--%>
                                <iewc:Tab Text="Tax Withholding" ID="TabStripTabGeneralWithholding"></iewc:Tab>
                                 <%--END : PK |10.04.2019| YRS-AT-4605 |Change Tab Display Text --%>
                            </iewc:TabStrip>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <iewc:MultiPage ID="MultiPageRetireesInformation" runat="server">
                                <iewc:PageView>
                                    <!-- General PageView Start -->
                                    <table class="Table_WithBorder" width="100%">
                                        <tr>
                                            <td align="left" class="td_Text">General
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" border="1" >
                                                    <tr>
                                                        <td width="60%"> <%-- Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the table width for propert alignment of birth date label--%> 
                                                            <table width="100%" border="0">
                                                                <tr>
                                                                    <td width="10%" align="left">
                                                                        <asp:Label ID="LabelGeneralSalute" runat="server" Width="15px" CssClass="Label_Small">Sal.</asp:Label>
                                                                    </td>
                                                                    <td width="25%" align="left">
                                                                        <asp:DropDownList ID="cboSalute" runat="server" Width="50" CssClass="DropDown_Normal">
                                                                            <asp:ListItem Value=""></asp:ListItem>
                                                                            <asp:ListItem Value="Mr.">Mr.</asp:ListItem>
                                                                            <asp:ListItem Value="Mrs.">Mrs.</asp:ListItem>
                                                                            <asp:ListItem Value="Dr.">Dr.</asp:ListItem>
                                                                            <asp:ListItem Value="Ms.">Ms.</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td width="20%" align="left">
                                                                        <asp:Label ID="LabelGeneralFundNo" runat="server" CssClass="Label_Small">Fund No.</asp:Label>
                                                                    </td>
                                                                    <td width="45%" align="left">
                                                                        <asp:TextBox ID="TextBoxGeneralFundNo" runat="server" CssClass="TextBox_Normal"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="10%" align="left">
                                                                        <asp:Label ID="LabelGeneralFirstName" runat="server" Width="32px" CssClass="Label_Small">First</asp:Label>
                                                                    </td>
                                                                    <td width="35%" align="left">
                                                                        <!--Updated by sanjay on 08 Nov 09 to ristrict First Name length-->
                                                                        <asp:TextBox Width="140px" runat="server" ID="TextBoxGeneralFirstName" CssClass="TextBox_Normal"
                                                                            MaxLength="20"></asp:TextBox>
                                                                    </td>
                                                                    <td width="20%" align="left">
                                                                        <asp:Label ID="LabelGeneralSSNo" runat="server" CssClass="Label_Small">SS No.  </asp:Label>
                                                                    </td>
                                                                    <td width="35%" align="left">
                                                                        <asp:TextBox ID="TextBoxGeneralSSNo" runat="server" CssClass="TextBox_Normal"></asp:TextBox>
                                                                        <!--<asp:Button id="ButtonGeneralSSNoEdit" runat="server" Text="Edit" CssClass="Button_Normal"></asp:Button>-->
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="10%" align="left">
                                                                        <asp:Label ID="LabelGeneralMiddleName" runat="server" Width="32px" CssClass="Label_Small">Middle</asp:Label>
                                                                    </td>
                                                                    <td width="35%" align="left">
                                                                        <!--Updated by sanjay on 08 Nov 09 to ristrict Middle Name length-->
                                                                        <asp:TextBox Width="140px" runat="server" ID="TextBoxGeneralMiddleName" CssClass="TextBox_Normal"
                                                                            MaxLength="20"></asp:TextBox>
                                                                    </td>
                                                                    <td width="20%" align="left">
                                                                        <asp:Label ID="LabelGeneralGender" runat="server" CssClass="Label_Small">Gender</asp:Label>
                                                                    </td>
                                                                    <td width="35%" align="left" colspan="2">
                                                                        <asp:DropDownList ID="DropDownListGeneralGender" runat="server" Width="100" CssClass="DropDown_Normal">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="10%" align="left">
                                                                        <asp:Label ID="LabelGeneralLastName" runat="server" Width="32px" CssClass="Label_Small">Last</asp:Label>
                                                                    </td>
                                                                    <td width="35%" align="left">
                                                                        <asp:TextBox Width="140px" runat="server" ID="TextBoxGeneralLastName" CssClass="TextBox_Normal"
                                                                            MaxLength="30"></asp:TextBox>
                                                                        <!--Updated by sanjay on 08 Nov 09 to ristrict Last Name length-->
                                                                    </td>
                                                                    <td width="20%" align="left">
                                                                        <asp:Label ID="LabelGeneralMaritalStatus" runat="server" CssClass="Label_Small">Marital Status</asp:Label>
                                                                    </td>
                                                                    <td width="35%" align="left">
                                                                        <%-- Changed by Anudeep:02-11-2012 For BT-1321-YRS 5.0-1712:lengthen marital status field  --%>
                                                                        <asp:DropDownList ID="cboGeneralMaritalStatus" runat="server" CssClass="DropDown_Normal">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="10%" align="left">
                                                                        <asp:Label ID="LabelGeneralSuffixName" runat="server" Width="32px" CssClass="Label_Small">Suffix</asp:Label>
                                                                    </td>
                                                                    <td width="35%" align="left">
                                                                        <asp:TextBox Width="40px" runat="server" ID="TextBoxGeneralSuffix" CssClass="TextBox_Normal"></asp:TextBox>
                                                                    </td>
                                                                    <td width="20%" align="left">
                                                                        <asp:Label ID="LabelGeneralDOB" runat="server" CssClass="Label_Small">Date of Birth</asp:Label>
                                                                    </td>
                                                                    <td width="35%" align="left">
                                                                        <asp:TextBox ID="TextBoxGeneralDOB" runat="server" ReadOnly="true" CssClass="TextBox_Normal" Width="80px"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="PopcalendarRecDate" runat="server" Separator="/" Control="TextBoxGeneralDOB"
                                                                            Format="mm dd yyyy" ScriptsValidators="No Validate"></rjs:PopCalendar>
                                                                        <!--<asp:Button id="ButtonGeneralDOBEdit" runat="server" Width="40px" Text="Edit" CssClass="Button_Normal"></asp:Button>-->                                                                      
                                                                        <asp:customvalidator id="cvBirthDate" runat="server"
                                                                        controltovalidate="TextBoxGeneralDOB" display="Dynamic" clientvalidationfunction="IsValidDate" CssClass="Error_Message" Text="Invalid Date"></asp:customvalidator> <%--Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Added custom validator to show error message on invalid date input--%> 
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="40%"  valign="top"> <%-- Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the table width for propert alignment of birth date label--%> 
                                                            <table width="100%">
                                                                <tr >
                                                                    <td width="100%" >
                                                                      <div id="divRetirementPlanAnnuityoption" style="overflow: auto; width: 100%;" runat="server">
                                                                        <table class="Table_WithBorder" width="100%"  >
                                                                            <tr>
                                                                                <td align="left" valign="top">
                                                                                    <asp:Label ID="LabelAnnuityOption_RetirementPlan" runat="server" CssClass="Label_Small">Retirement Plan Annuity Option</asp:Label>
                                                                                </td>
                                                                                <td align="left" valign="top">
                                                                                    <asp:RadioButtonList ID="RadioButtonAnnuityType_RetirementPlan" runat="server" Height="4px" CssClass="Label_Small" RepeatDirection="Vertical">
                                                                                        <asp:ListItem Value="M" Selected="True">Annuity Type M</asp:ListItem>
                                                                                        <asp:ListItem Value="C">Annuity Type C</asp:ListItem>
                                                                                    </asp:RadioButtonList>

                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                      </div>
                                                                    </td>
                                                                </tr>
                                                                <tr >
                                                                    <td width="100%">
                                                                        <div id="divSavingPlanAnnuityoption" style="overflow: auto; width: 100%;" runat="server">
                                                                            <table class="Table_WithBorder" width="100%" >
                                                                                <tr>
                                                                                    <td align="left" valign="top">
                                                                                        <asp:Label ID="LabelAnnuityOption_SavingsPlan" runat="server" CssClass="Label_Small">TD Savings Plan Annuity Option</asp:Label>
                                                                                    </td>
                                                                                    <td align="left" valign="top">
                                                                                        <asp:RadioButtonList ID="RadioButtonAnnuityType_SavingsPlan" runat="server" Height="4px" RepeatDirection="Vertical" CssClass="Label_Small">
                                                                                            <asp:ListItem Value="M" Selected="True">Annuity Type M</asp:ListItem>
                                                                                            <asp:ListItem Value="C">Annuity Type C</asp:ListItem>
                                                                                        </asp:RadioButtonList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%">
                                                    <tr valign="top">
                                                        <td align="left">
                                                            <table width="100%" class="Table_WithOutBorder">
                                                                <tr valign="top">
                                                                    <td width="100%" align="left">
                                                                        <table class="Table_WithOutBorder" width="100%"  >
                                                                            <tr>
                                                                                <td class="td_Text" width="100%" colspan="4">Primary Address &nbsp;
                                                                                    <asp:LinkButton ID="lnkParticipantAddress1" runat="server" CssClass="Link_Small" Text="Use Participant's Address"></asp:LinkButton>
                                                                                </td>
                                                                                <%--<td class="td_Text" align="left" width="75%"  colspan="3" >
                                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="Link_Small" Text="Use Participant's Address"></asp:LinkButton>
                                                                                </td>--%>
                                                                            </tr>
                                                                            <tr valign="top">
                                                                                <td colspan="2" width="40%">
                                                                                    <YRSControls:AddressWebUserControl ID="AddressWebUserControl1" runat="server" AllowEffDate="true" AllowNote="true" IsFromBenificarySettlement="true" PopupHeight="930"></YRSControls:AddressWebUserControl>
                                                                                </td>
                                                                                <td colspan="2" width="60%">
                                                                                    <table border="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="25%"></td>
                                                                                            <td align="left" width="25%">
                                                                                                <asp:Label ID="LabelTelephone" runat="server" CssClass="Label_Small">Telephone</asp:Label>
                                                                                            </td>
                                                                                            <td align="left" width="50%">
                                                                                                <asp:TextBox ID="TextBoxTelephone" runat="server" CssClass="TextBox_Normal" Width="215px" align="left"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" width="25%"></td>
                                                                                            <td align="left" width="25%">
                                                                                                <asp:Label ID="Label4" runat="server" CssClass="Label_Small"> Email Id</asp:Label>
                                                                                            </td>
                                                                                            <td align="left" width="50%">
                                                                                                <asp:TextBox ID="TextBoxEmailId" runat="server" CssClass="TextBox_Normal" Width="215px"></asp:TextBox>
                                                                                                <asp:RegularExpressionValidator ID="regPrimaryMail" runat="server" ControlToValidate="TextBoxEmailId" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic">*</asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="td_Text">
                                                                                <td align="left" colspan="4">Secondary Address &nbsp;
                                                                                    <asp:LinkButton ID="lnkParticipantAddress2" runat="server" CssClass="Link_Small" Text="Use Participant's Address"></asp:LinkButton>
                                                                                </td>                                                                               
                                                                            </tr>                                                                            
                                                                            <tr valign="top">
                                                                                <td colspan="2" width="40%">
                                                                                    <YRSControls:AddressWebUserControl ID="AddressWebUserControl2" AllowEffDate="true" AllowNote="true" IsFromBenificarySettlement="true" runat="server" PopupHeight="930"></YRSControls:AddressWebUserControl>
                                                                                </td>
                                                                                <td colspan="2" width="60%">
                                                                                    <table border="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="left" width="25%"></td>
                                                                                            <td align="left" width="25%">
                                                                                                <asp:Label ID="LabelSecTelephone" runat="server" CssClass="Label_Small">Telephone</asp:Label>
                                                                                            </td>
                                                                                            <td align="left" width="50%">
                                                                                                <asp:TextBox ID="TextBoxSecTelephone" runat="server" CssClass="TextBox_Normal" Width="215px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" width="25%"></td>
                                                                                            <td align="left" width="25%">
                                                                                                <asp:Label ID="LabelSecEmail" runat="server" CssClass="Label_Small">Email Id</asp:Label>
                                                                                            </td>
                                                                                            <td align="left" width="50%">
                                                                                                <asp:TextBox ID="TextBoxSecEmail" runat="server" CssClass="TextBox_Normal" Width="215px"></asp:TextBox>
                                                                                                <asp:RegularExpressionValidator ID="regSecondaryMail" runat="server" ControlToValidate="TextBoxSecEmail"
                                                                                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic">*</asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>                                                                            
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                               
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%">

                                                    <tr valign="top">
                                                        <td align="left" width="130">
                                                            <asp:Label runat="server" CssClass="Label_Small" ID="LabelPOA">POA/Other Rep.</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Button ID="ButtonGeneralPOA" runat="server" Text="Show/Edit all" CssClass="Button_Normal"
                                                                Width="150px"></asp:Button>
                                                            <table>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <div id="config">                                                                          
                                                                            <asp:TextBox ID="TextBoxGeneralPOA" runat="server" CssClass="TextBox_Normal" Visible="false"></asp:TextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                        </td>
                    </tr>
            </table>
            <!-- General PageView End -->
            </iewc:Pageview>

        <iewc:PageView>
            <div class="Div_Center">
                <table class="Table_WithBorder" width="100%" style="height: 350px" cellspacing="0">
                    <tr valign="top">
                        <td width="100%">                             
                            <table width="100%" cellspacing="0">
                                <tr>
                                     <%--START : PK |10.04.2019| YRS-AT-4605 |Header text changes--%>
                                    <%--<td align="left" class="Td_ButtonContainer">Withholding--%> 
                                    <td align="left" class="Td_text">Federal Withholding  
                                     <%--END : PK |10.04.2019| YRS-AT-4605 |Header text changes--%>
                                    </td>
                                    <td align="right" class="Td_ButtonContainer">
                                        <asp:Button ID="ButtonGeneralWithholdAdd" runat="server" Width="90px" Text="Add ..."
                                            CssClass="Button_Normal" CausesValidation="False"></asp:Button>  <%--PK |10.04.2019| YRS-AT-4605 |Change button Width 110 to 90--%>
                                    </td>
                                </tr>

                                <tr valign="top">
                                    <td align="center" class="Table_WithOutBorder" style="height:80px;"> <%-- PK |10.22.2019 | YRS-AT-4605 |Added height to datagrid --%>                                                   
                                        <asp:DataGrid ID="DataGridGeneralWithhold" runat="server" Width="600" CssClass="DataGrid_Grid"
                                            AutoGenerateColumns="False" AllowSorting="true">
                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                            <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                      <asp:ImageButton ID="imgEditWithdrawal" runat="server" ToolTip="Edit" CommandName="Edit"
                                                            CausesValidation="False" ImageUrl="images\edits.gif" AlternateText="Edit" ></asp:ImageButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn HeaderText="Exemptions" DataField="Exemptions" />
                                                <asp:BoundColumn HeaderText="Add'l Amount" DataField="Add'l Amount" />
                                                <asp:BoundColumn HeaderText="Type" DataField="Type" />
                                                <asp:BoundColumn HeaderText="Tax Entity" DataField="Tax Entity" />
                                                <asp:BoundColumn HeaderText="Marital Status" DataField="Marital Status" />
                                                <asp:BoundColumn HeaderText="ID" DataField="FedWithdrawalID" Visible="false" />
                                            </Columns>
                                        </asp:DataGrid>&nbsp; &nbsp;
                                        
                                    </td>
                                </tr>
                                <%--START : PK |10.04.2019| YRS-AT-4605 |State withholding user control added  --%>
                                <tr>
                                    <td style="height:80px;">
                                        <YRSStateTaxControls:StateWithholdingListing_WebUserControl ID="stwListUserControl" ClientIDMode="Static" PreFixID="beneficiaryInformation" runat="server" />
                                    </td> 
                                </tr>
                                <%--END : PK |10.04.2019| YRS-AT-4605 |State withholding user control added  --%>

                                <%--Start - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
                                <tr valign="top">
                                    <td width="100%" colspan="2">                             
                                        <table width="100%" cellspacing="0">
                                            <tr>
                                                <%--START : PK |10.04.2019| YRS-AT-4605 |Header text changes--%>
                                               <%-- <td align="left" class="Td_ButtonContainer">Deductions</td>  --%>
                                                 <td align="left" class="Td_text">Deductions</td>  
                                                <%--END : PK |10.04.2019| YRS-AT-4605 |Header text changes--%>                                              
                                            </tr>
                                            <tr>
                                                <td height="18" style="padding-left:10px;"><a href="#" ID="lnkDeductions" style="font-size:smaller;" onclick="javascript:getSelectedDedValues(); return false;">Deductions:</a>&nbsp;<asp:Label ID="lblDeductionamt" runat="server" CssClass="Label_Small" Text="0.00"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td><asp:Label ID="lblDedunctionsmsg" runat="server" CssClass="Label_Small" Style="color: #f00">(Please click Deductions link to apply fees and deductions)</asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                            </tr>                          
                    <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>

                            </table>                          
                        </td>
                    </tr>
                 </table>
            </div>
        </iewc:PageView>
            </iewc:multipage>
        </div>
        <div class="Div_Center">
            <table class="Td_ButtonContainer" id="Table6" cellspacing="1" cellpadding="1" width="100%" >
                <tr>
                    <td align="right">
                        <asp:Button ID="ButtonRetireesInfoSave" runat="server" CssClass="Button_Normal" Width="100PX"
                            Text="Save" OnClientClick="return callProgressDialogDiv();"></asp:Button>   <%-- SB | 2018.05.28 | YRS-AT-3922 | On Click of Save button a modal dialog box will appear to avoid multiple clicks on Save button --%>
                    </td>
                    <td align="right" width="120px">
                        <asp:Button ID="ButtonRetireesInfoCancel" runat="server" CssClass="Button_Normal"
                            Text="Cancel" Width="100PX" CausesValidation="False"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%">
            <tr>
                <td width="100%">
                    <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server"></YRSControls:YMCA_Footer_WebUserControl>
                </td>
            </tr>

        </table>
        <div id="divWSMessage" runat="server" style="display: none;">
            <table width="690px">
                <tr>
                    <td valign="top" align="left">
                        <span id="spntext"></span>
                    </td>
                </tr>
            </table>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        <input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server" value="" />
        <%--Start - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
        <div id="divDeductions" runat="server" style="OVERFLOW: auto;display:block;">
            <table width="100%">
                <tr>
                    <td align="center">
            <div id="divMessage" style="width: 75%;">               
             </div>
                </td>
                </tr>
                <tr>
                    <td>
            <asp:datagrid id="dgDeductions" runat="server" Width="200px" CssClass="DataGrid_Grid" Autogeneratecolumns="false" align="center">
						<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
						<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
						<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
						<Columns>						
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:CheckBox id="chkBoxDeduction" runat="server"></asp:CheckBox>
								</ItemTemplate>                               
							</asp:TemplateColumn>                                                
							<asp:BoundColumn DataField="CodeValue" HeaderText="Deductions" visible="False">
								<HeaderStyle Width="10px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ShortDescription" HeaderText="Deductions">
								<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
                              <asp:TemplateColumn HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount")%>'></asp:Label>
                                    <asp:TextBox ID ="txtFundCostAmt" runat="server" Visible="false" Width="70px" Enabled ="false" onkeypress="javascript: ValidateNumeric();" onchange="javascript: $(this).val(CurrencyFormatted($(this).val()));"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>   
						</Columns>
					</asp:datagrid>
                </td>
                </tr>
            </table>
        </div>
        <%--End - Manthan | 2016.04.22 | YRS-AT-2206 | Applying Fees and deductions to death payments - Part A.1  --%>
       
         <%--START: SB | 2018.05.28 | YRS-AT-3922 -  YRS bug - Death Settlement double-click causes multiple bitActive and bitPrimary set for same Address --%>
        <div id="divProgress" style="overflow: visible;">
            <div>
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                    <tr>
                        <td>
                            <asp:Label ID="labelMessage" CssClass="Label_Small" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <%--END: SB | 2018.05.28 | YRS-AT-3922 -  YRS bug - Death Settlement double-click causes multiple bitActive and bitPrimary set for same Address --%>

    </form>
</body>
</html>
