<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%--BS:2012.05.10:yrs:1470:enhancement of user control for person maintenance--%>
<%--<%@ Register TagPrefix="YRSControls" TagName="AddressWebUserControl" Src="~/UserControls/AddressWebUserControl.ascx" %>--%>
<%@ Register TagPrefix="YRSControls" TagName="Enhance_AddressWebUserControl" Src="~/UserControls/Enhance_AddressWebUserControl.ascx" %>
<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl_new.ascx" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSTelephoneControls" TagName="Telephone_WebUserControl" Src="~/UserControls/Telephone_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="~/UserControls/YMCA_Footer_WebUserControl.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ParticipantsInformation.aspx.vb" Inherits="YMCAUI.ParticipantsInformation" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="YRSControls" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<!--#include virtual="top.html"-->
<html>
<head>
    <title>YMCA YRS -</title>
    <!--Included by Shashi Shekhar:2009-12-23:To use the common js function -->
    <!--Included by Prasad Jadhav:2011-08-26:For BT-895,YRS 5.0-1364 : prompt user to if changes not saved -->
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <script src="JS/jquery-ui/JScript-1.7.2.0.min.js" type="text/javascript"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="JS/YMCA_JScript.js" type="text/javascript"></script>
    <script language="javascript" src="JS/YMCA_JScript_Warn.js" type="text/javascript"></script>
    <!-- START : SC | 2019.05.13 | YRS-AT-2601 | Adding .js file for disabled controls-->
    <script type="text/javascript" src="JS/YMCA_JScript_DisableTextBox.js"></script>
    <!-- END : SC | 2019.05.13 | YRS-AT-2601 | Adding .js file for disabled controls-->
    <link id="Link1" href="CSS/CustomStyleSheet.css" type="text/css" runat="server" rel="stylesheet" />
    <script language="javascript" type="text/javascript">

        //Dinesh.K :2013.08.16 - YRS 5.0-1698:Cross check SSN when entering a date of death
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

        function CloseMessageDeathNotify() {
            $(document).ready(function () {
                $("#ConfirmDialog").dialog('close');
            });
        }

        function openDialogDeathNotify(str) {
            $(document).ready(function () {
                $('#lblMessage1').html(str);
                $("#ConfirmDialog").dialog('open');
                return false;
            });
        }
        //END Dinesh.K :2013.08.16 - YRS 5.0-1698:Cross check SSN when entering a date of death

        // START : SR | 2018.07.12 | YRS-AT-3858 | These methods can be used to show error messages.
        $(document).ready(function () {
            $('#divCommonConfirmDialog').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                close: false,
                width: 500, height: 175,
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        });


        function ShowCommonDialog(text, title) {
            $(document).ready(function () {
                $('#' + '<%=lblCommonMessage.ClientID%>').text(text);
                $('#divCommonConfirmDialog').dialog({ title: title });
                $('#divCommonConfirmDialog').dialog("open");
            });
            }
        function CloseCommonDialog() {
            $(document).ready(function () {
                $('#divCommonConfirmDialog').dialog('close');
            });
         }
        // END : SR | 2018.07.12 | YRS-AT-3858 | These methods can be used to show erroe messages.

        //setup Enrollment(403(b)) dialog
        $(document).ready(function () {
            $('#DivEditContacts').dialog({
                autoOpen: false,
                dialogClass: 'DialogStyle',
                draggable: false, modal: true,
                width: 700, minheight: 500,
                "position": "fixed",
                padding: 20,
                title: "Edit Contact Details.",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });
        });


        function showDialog(id) {
            $('#' + id).dialog("open");
            return true;<%--VC | 2018.11.14 | YRS-AT-4018 | Returning true--%>
        }




        function closeDialog(id) {
            $('#' + id).dialog("close");
            $("html, body").animate({ scrollTop: 0 }, "fast");
            return true;<%--VC | 2018.11.14 | YRS-AT-4018 | Returning true--%>
        }

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


        $(document).ready(function () {
            //$("div#LoanCollapes").hide();
            //toggle the componenet with class msg_body
            $("#LoanExpand").click(function () {
                $("div#LoanExpand").hide();
                $("div#LoanHide").show();
                $("div#LoanCollapes").slideToggle(500);
            });
            $("#LoanHide").click(function () {
                $("div#LoanHide").hide();
                $("div#LoanExpand").show();
                $("div#LoanCollapes").slideToggle(500);
            });
        });

        $(document).ready(function () {
            // $("div#LoanDetailsDiv").hide();
            //toggle the componenet with class msg_body
            $("#LoanDetailsExpand").click(function () {
                $("div#LoanDetailsExpand").hide();
                $("div#LoanDetailsCollapse").show();
                $("div#LoanDetailsDiv").slideToggle(500);
            });
            $("#LoanDetailsCollapse").click(function () {
                $("div#LoanDetailsCollapse").hide();
                $("div#LoanDetailsExpand").show();
                $("div#LoanDetailsDiv").slideToggle(500);
            });
            <%--START : VC | 2018.08.01 | YRS-AT-4018 | Added code to perform expand collapse on WEB loan details section--%>
            $("#tdWEBLoanDetailsExpand").click(function () {
                $("div#WEBLoanDetailsExpand").hide();
                $("div#WEBLoanDetailsCollapse").show();
                $("div#WEBLoanDetailsDiv").slideToggle(500);
        });
            $("#tdImgWEBLoanDetailsExpand").click(function () {
                $("div#WEBLoanDetailsExpand").hide();
                $("div#WEBLoanDetailsCollapse").show();
                $("div#WEBLoanDetailsDiv").slideToggle(500);
            });
            $("#tdWEBLoanDetailsCollapse").click(function () {
                $("div#WEBLoanDetailsCollapse").hide();
                $("div#WEBLoanDetailsExpand").show();
                $("div#WEBLoanDetailsDiv").slideToggle(500);
            });
            $("#tdImgWEBLoanDetailsCollapse").click(function () {
                $("div#WEBLoanDetailsCollapse").hide();
                $("div#WEBLoanDetailsExpand").show();
                $("div#WEBLoanDetailsDiv").slideToggle(500);
            });
            $('#divApplicationStatusHistory').dialog({
                autoOpen: false,
                resizable: false,
                dialogClass: 'no-close',
                draggable: true,
                width: 550, minheight: 200,
                height: 250,
                closeOnEscape: false,
                title: "Application Status History",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });
            <%--END : VC | 2018.08.01 | YRS-AT-4018 | Added code to perform expand collapse on WEB loan details section--%>
        });


        function CallFrame(PageID) {
            var frameelement = document.getElementsByTagName('Iframe');
            frameelement.src = PageID;
        }

        function fncKeyStop() {
            // Check if the control key is pressed.
            // If the Netscape way won't work (event.modifiers is undefined),
            // try the IE way (event.ctrlKey)
            var ctrl = typeof event.modifiers == 'undefined' ?
event.ctrlKey : event.modifiers & Event.CONTROL_MASK;

            // Check if the 'V' key is pressed.
            // If the Netscape way won't work (event.which is undefined),
            // try the IE way (event.keyCode)
            var v = typeof event.which == 'undefined' ?
event.keyCode == 86 : event.which == 86;

            // If the control and 'V' keys are pressed at the same time
            if (ctrl && v) {
                // ... discard the keystroke and clear the text box
                document.forms['Form1'].elements['TextBoxZip'].value = '';
                return false;
            }
            return true;
        }

        function fncKeySecStop() {
            // Check if the control key is pressed.
            // If the Netscape way won't work (event.modifiers is undefined),
            // try the IE way (event.ctrlKey)
            var ctrl = typeof event.modifiers == 'undefined' ?
event.ctrlKey : event.modifiers & Event.CONTROL_MASK;

            // Check if the 'V' key is pressed.
            // If the Netscape way won't work (event.which is undefined),
            // try the IE way (event.keyCode)
            var v = typeof event.which == 'undefined' ?
event.keyCode == 86 : event.which == 86;

            // If the control and 'V' keys are pressed at the same time
            if (ctrl && v) {
                // ... discard the keystroke and clear the text box
                document.forms['Form1'].elements['TextBoxSecZip'].value = '';
                return false;
            }
            return true;
        }

        function ValidateTelephoneNo(str, Value) {
            if (str != undefined) {
                var val = str.value
                if (Value == "US" || Value == "CA") {
                    if (((val.length < 10) || (val.length > 10)) && (val.length != 0)) {
                        //alert('Telephone number should be 10 characters.')
                        return false;
                    }
                    else { return true; }
                }
                else { return true; }
            }
            else { return true; }
        }
        function SetMaxLengthPhone(str, value) {
            if (value == "US" || value == "CA") {
                str.maxLength = 10;
            }
            else {
                str.maxLength = 25;
            }
        }
        function checkTelephoneLength() {
            if (!ValidateTelephoneNo(document.Form1.all.TextBoxTelephone, document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value)) {
                <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                alert('Office number must be 10 digits.'); --%>
                alert('Please provide valid Office number.');
                <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
                return false;
            }

            if (!ValidateTelephoneNo(document.Form1.all.TextBoxHome, document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value)) {
                <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                alert('Home number must be 10 digits.');--%>
                alert('Please provide valid Home number.');
                <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
                return false;
            }
            <%--PPP | 2015.10.20 | YRS-AT-2588 | Order changed validatig Mobile first and then Fax--%>
            if (!ValidateTelephoneNo(document.Form1.all.TextBoxMobile, document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value)) {
                <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                alert('Mobile number must be 10 digits.');--%>
                alert('Please provide valid Mobile number.');
                <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
                return false;
            }

            if (!ValidateTelephoneNo(document.Form1.all.TextBoxFax, document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value)) {
                <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                alert('Fax number must be 10 digits.');--%>
                alert('Please provide valid Fax number.');
                <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
                return false;
            }

            if (!ValidateTelephoneNo(document.Form1.all.TextBoxSecTelephone, document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value)) {
                <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                alert('Office number must be 10 digits.');--%>
                alert('Please provide valid Office number.');
                <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
                return false;
            }

            if (!ValidateTelephoneNo(document.Form1.all.TextBoxSecHome, document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value)) {
                <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                alert('Home number must be 10 digits.');--%>
                alert('Please provide valid Home number.');
                <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
                return false;
            }

            <%--PPP | 2015.10.20 | YRS-AT-2588 | Order changed validatig Mobile first and then Fax--%>
            if (!ValidateTelephoneNo(document.Form1.all.TextBoxSecMobile, document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value)) {
                <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                alert('Mobile number must be 10 digits.');--%>
                alert('Please provide valid Mobile number.');
                <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
                return false;
            }

            if (!ValidateTelephoneNo(document.Form1.all.TextBoxSecFax, document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value)) {
                <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
                alert('Fax number must be 10 digits.');--%>
                alert('Please provide valid Fax number.');
                <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
                return false;
            }

            if ($('#HiddenFieldDeathDate').val() != '') {
                if (IsValidUpdatedDeathDate() == false) {
                    $("#ButtonSaveParticipant").disableControl(false);
                    $(".DisableControl").each(function () { $(this).attr('disabled', 'disabled'); });
                    $(".EnableControl").each(function () { $(this).attr('enabled', 'enabled'); });
                    return false;
                }
            }
            (function ($) {
                jQuery.fn.disableControl = function (bDisable) {
                    if (bDisable) {
                        this.addClass("DisableControl").removeClass("EnableControl");
                    } else {
                        this.removeClass("DisableControl").addClass("EnableControl");
                    }
                }
            })(jQuery);

            <%--START: VC | 2018.11.14 | YRS-AT-4018 | Commented existing method call and passing message as paramter to the same method--%>
            //ShowProcessingDialog(); /*Anudeep A:2014.09.26 BT:2440:YRS 5.0-2318 - Primary Active address record being created twice. */
            ShowProcessingDialog('Please wait while information is being saved.');
            <%--END: VC | 2018.11.14 | YRS-AT-4018 | Commented existing method call and passing message as paramter to the same method--%>
            return true;
        }
        function fncKeyCityStop() {
            // Check if the control key is pressed.
            // If the Netscape way won't work (event.modifiers is undefined),
            // try the IE way (event.ctrlKey)
            var ctrl = typeof event.modifiers == 'undefined' ?
event.ctrlKey : event.modifiers & Event.CONTROL_MASK;

            // Check if the 'V' key is pressed.
            // If the Netscape way won't work (event.which is undefined),
            // try the IE way (event.keyCode)
            var v = typeof event.which == 'undefined' ?
event.keyCode == 86 : event.which == 86;

            // If the control and 'V' keys are pressed at the same time
            if (ctrl && v) {
                // ... discard the keystroke and clear the text box
                document.forms['Form1'].elements['TextBoxCity'].value = '';
                return false;
            }
            return true;
        }

        function fncKeySecCityStop() {
            // Check if the control key is pressed.
            // If the Netscape way won't work (event.modifiers is undefined),
            // try the IE way (event.ctrlKey)
            var ctrl = typeof event.modifiers == 'undefined' ?
event.ctrlKey : event.modifiers & Event.CONTROL_MASK;

            // Check if the 'V' key is pressed.
            // If the Netscape way won't work (event.which is undefined),
            // try the IE way (event.keyCode)
            var v = typeof event.which == 'undefined' ?
event.keyCode == 86 : event.which == 86;

            // If the control and 'V' keys are pressed at the same time
            if (ctrl && v) {
                // ... discard the keystroke and clear the text box
                document.forms['Form1'].elements['TextBoxSecCity'].value = '';
                return false;
            }
            return true;
        }

        function ValidateAlpha() {
            if (((event.keyCode > 64) && (event.keyCode < 91)) || ((event.keyCode > 96) && (event.keyCode < 123)) || ((event.keyCode == 32))) {
                event.returnValue = true;
            }
            else {
                event.returnValue = false;
            }
        }


        /* Shashi Shekhar:2009-12-31: comment and shift ValidateNumeric() in common external js file(JS/YMCA_JScript.js). Please check older version from SVN if needed. */
        function ValidateAlphaNumeric() {

            if (((event.keyCode > 64) && (event.keyCode < 91)) || ((event.keyCode > 96) && (event.keyCode < 123)) || ((event.keyCode > 47) && (event.keyCode < 58)) || (event.keyCode == 45)) {

                event.returnValue = true;
            }
            else {
                event.returnValue = false;
            }
        }
        function ValidateAlphaNumericForCity() {

            if (((event.keyCode > 64) && (event.keyCode < 91)) || ((event.keyCode > 96) && (event.keyCode < 123)) || ((event.keyCode > 47) && (event.keyCode < 58)) || (event.keyCode == 45) || (event.keyCode == 32)) {

                event.returnValue = true;
            }
            else {
                event.returnValue = false;
            }
        }

        /* commented by Shashi Shekhar:2009-12-23: Now this function CheckAccess(controlname) is in external JS/YMCA_JScript.js file */


        function CheckAccess1(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);
            //alert(controlname);
            //alert(str);

            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {
                NewWindow('AddEditQDRO.aspx', 'mywin', '750', '550', 'no', 'center')
                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }

        //Name Anita and Shubhrata Date:17-05-06 Reason:Secured Controls
        function CheckAccessNotes(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);
            //(controlname);
            //(str);
            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                //  window.open('UpdateNotes.aspx', 'CustomPopUp',width=650, height=350, menubar=no, Resizable=Yes,top=120,left=120, scrollbars=yes)
                return true;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }
        function CheckAccessParticipantSave(controlname) {
            var str = String(document.Form1.all.HiddenSecControlName.value);

            if (str.match(controlname) != null) {
                alert("Sorry, You are not authorized to do this activity.");
                return false;
                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="0";
            }
            else {

                return _OnBlur_TextBoxSSNo();

                //document.Form1.all.HiddenText.value="";
                //document.Form1.all.HiddenText.value="1";
            }

        }
        //by Aparna -YREN-3115
        function EnableSavebutton() {
            document.getElementById("ButtonSaveParticipant").visible = true;
            document.getElementById("ButtonSaveParticipant").enabled = true;
        }
        //function CheckAccess1(controlname)
        //{
        //var str=String(document.Form1.all.HiddenSecControlName.value);
        //alert(controlname);
        //alert(str);
        //if (str.match(controlname)!= null)
        //{
        //alert("Sorry, You are not authorized to do this activity.");
        //return false;
        //document.Form1.all.HiddenText.value="";
        //document.Form1.all.HiddenText.value="0";
        //}
        //else
        //{

        //  NewWindow('AddEditQDRO.aspx','mywin','730','500','yes','center')
        // return true;
        //document.Form1.all.HiddenText.value="";
        //document.Form1.all.HiddenText.value="1";
        //}

        //anita may 9th security on controls
        void function SetTextBox(txtbox, textValue) {
            // set the textbox's value
            txtbox.value = textValue;
            return false;
        }
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
        function formatCurrency(p, d) // p = control, d = delimeter
        {
            // round to 2 decimals if cents present
            var n = p.toString();
            n = (Math.round(n * 100) / 100).toString().split('.');
            var myNum = n[0].toString();
            var fmat = new Array();
            var len = myNum.length;
            var i = 1;
            var deci = (d == '.') ? ',' : '.';
            for (i; i < len + 1; i++) {
                fmat[i] = myNum.charAt(i - 1);
            }
            fmat = fmat.reverse();
            for (i = 1; i < len; i++) {
                if (i % 3 == 0) {
                    fmat[i] += d;
                }
            }
            var val = fmat.reverse().join('') + (n[1] == null ? deci + '00' : (deci + n[1]));
            return val;
        }
        function _OnBlur_TextBoxSSNo() {

            var str = String(document.Form1.all.TextBoxSSNo.value);
            var _arr = new Array(20);
            var flg = false;

            for (i = 0; i < str.length; i++) {
                _arr[i] = str.substr(i, 1);

            }

            for (i = 0; i < str.length - 1; i++) {

                if (_arr[i].toString() == '.') {
                    flg = true;

                }


            }

            if (flg) {
                alert('SSNo cannot contain decimal.');
                document.Form1.all.TextBoxSSNo.focus();
                return false;
            }

            if (isNaN(parseInt(document.Form1.all.TextBoxSSNo.value))) {
                alert('SSNo. cannot have characters.');
                document.Form1.all.TextBoxSSNo.focus();
                return false;
            }
            else {
                if (str.length != 9) {
                    alert('SSNo. must be 9 digits.');
                    document.Form1.all.TextBoxSSNo.focus();

                    return false;
                }
            }


        }
        function EnableControls() {
            if (document.getElementById("ButtonSaveParticipant")) {
                document.Form1.all.ButtonSaveParticipant.disabled = false;
                $('#ButtonSaveParticipant').click(function () {
                    return checkTelephoneLength();
                });
            }
            else if (document.getElementById("ButtonSaveParticipants")) {
                document.Form1.all.ButtonSaveParticipants.disabled = false;
                $('#ButtonSaveParticipants').click(function () {
                    return checkTelephoneLength();
                });
            }
            document.Form1.all.ButtonCancel.disabled = false;
            //document.Form1.all.ButtonAdd.disabled=true;
            document.Form1.all.ButtonOK.disabled = true;

        }

        function echeck(str) {

            var at = "@"
            var dot = "."
            var lat = str.indexOf(at)
            var lstr = str.length
            var ldot = str.indexOf(dot)
            if (str.indexOf(at) == -1) {
                alert("Invalid email address format.")

                return false
            }

            if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr) {
                alert("Invalid email address format.")
                return false
            }

            if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr) {
                alert("Invalid email address format.")
                return false
            }

            if (str.indexOf(at, (lat + 1)) != -1) {
                alert("Invalid email address format.")
                return false
            }

            if (str.substring(lat - 1, lat) == dot || str.substring(lat + 1, lat + 2) == dot) {
                alert("Invalid email address format.")
                return false
            }

            if (str.indexOf(dot, (lat + 2)) == -1) {
                alert("Invalid email address format.")
                return false
            }

            if (str.indexOf(" ") != -1) {
                alert("Invalid email address format.")
                return false
            }

            return true
        }

        function ValidateForm() {
            var emailID = document.Form1.all.TextBoxEmailId

            if (emailID.value != '') {

                //alert('notblank')
                if (echeck(emailID.value) == false) {

                    emailID.value = document.forms[0].EmailId.value
                    //	alert(document.Form1.BadEmail.value)

                    //	alert(document.Form1.TextOnly.value)
                    if (document.Form1.BadEmail.value == 'True') {

                        document.Form1.all.CheckboxBadEmail.checked = true
                    }
                    else
                        document.Form1.all.CheckboxBadEmail.checked = false
                    // alert(document.Form1.Unsubscribe.value)
                    if (document.Form1.Unsubscribe.value == 'True') {
                        //alert('inside if')
                        document.Form1.all.CheckboxUnsubscribe.checked = true
                    }
                    else
                        document.Form1.all.CheckboxUnsubscribe.checked = false

                    if (document.Form1.TextOnly.value == 'True')
                        document.Form1.all.CheckboxTextOnly.checked = true
                    else
                        document.Form1.all.CheckboxTextOnly.checked = false

                    emailID.focus()
                    return false
                }
                else {
                    //alert('ok')
                    document.Form1.all.CheckboxBadEmail.checked = false
                    document.Form1.all.CheckboxUnsubscribe.checked = false
                    document.Form1.all.CheckboxTextOnly.checked = false
                    return true
                }
            }
            else {
                document.Form1.all.CheckboxBadEmail.checked = false
                document.Form1.all.CheckboxUnsubscribe.checked = false
                document.Form1.all.CheckboxTextOnly.checked = false
                return true
            }

            return true
        }

        //--Added by imran on 30-July-2010 : YRS 5.0-1141 add Age format to the person maintanace screen.
        function getAge() {

            var datDate1 = new Date();
            var datDate2 = new Date();
            var numTotalNumberofDays;
            //BS:2011.08.11:YRS 5.0-1339:BT:852 - Reopen issue 
            if ($('#TextBoxDOB_TextBoxUCDate').val() == "") {
                $('#labelBoxAge').val('');
                return;
            }
            datDate1 = Date.parse($('#TextBoxDOB_TextBoxUCDate').val());
            if ($('#TextBoxDeceasedDate').val() != "") {
                datDate2 = Date.parse($('#TextBoxDeceasedDate').val());
            }

            var numTotalNumberofDays = (datDate2 - datDate1) / (24 * 60 * 60 * 1000)
            var numReminder = (numTotalNumberofDays % 365.2524)

            if (numReminder > 0) {
                numAge = Math.floor((numTotalNumberofDays - numReminder) / 365.2425) + (Math.floor(numReminder / 30.5) / 100)
            }
            else {
                numAge = Math.floor((numTotalNumberofDays - numReminder) / 365.2425)
            }

            var strnumAge = String(numAge)

            if (strnumAge.indexOf(".00") > -1) {
                strnumAge = strnumAge.replace(".00", "Y")
            }
            else {

                if (strnumAge.indexOf(".") > -1) {
                    //.1 means 10 month.In javascript retuns 10 month as .1
                    if (strnumAge.indexOf(".11") == -1) {
                        strnumAge = strnumAge.replace(".1", ".10")
                    }

                    strnumAge = strnumAge.replace(".", "Y/") + "M"
                }
                else { strnumAge = strnumAge + "Y" }
            }

            $('#labelBoxAge').val(strnumAge);
            document.getElementById('labelBoxAge').innerText = strnumAge;
        }


        //BS:2012.01.18:YRS 5.0-1497 :- here we have call IsValidUpdatedDeathDate() on ButtonSaveParticipant click  if it is false then enabled button
        //$(document).ready(function () {
        //    $("#ButtonSaveParticipant").click(function () {


        //    })
        //});
        //(function ($) {
        //    jQuery.fn.disableControl = function (bDisable) {
        //        if (bDisable) {
        //            this.addClass("DisableControl").removeClass("EnableControl");
        //        } else {
        //            this.removeClass("DisableControl").addClass("EnableControl");
        //        }
        //    }
        //})(jQuery);
        //BS:2012.01.18:YRS 5.0-1497 :-This method is validate updated death date value, updated death date has to be come with in original death month and year on client side same as done on server side
        function IsValidUpdatedDeathDate() {
            var NewDeathDate = new Date($('#TextBoxDeceasedDate').val());
            var CurrentDeathDate = new Date($('#HiddenFieldDeathDate').val());
            var CurrentDeathMonth = CurrentDeathDate.getMonth() + 1;
            var CurrentDeathYear = CurrentDeathDate.getFullYear();
            var NewDeathMonth = NewDeathDate.getMonth() + 1;
            var NewDeathYear = NewDeathDate.getFullYear();
            if (CurrentDeathMonth != NewDeathMonth || CurrentDeathYear != NewDeathYear) {
                alert("The new death date must be in the same month and year of the existing date of death (" + $('#HiddenFieldDeathDate').val() + ")");
                return false;
            }
            return true;

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
					    buttons: [{ text: "OK", click: CloseWSMessage }]
					});
        });

        function CloseWSMessage() {
            $(document).ready(function () {
                $("#divWSMessage").dialog('close');
            });
        }

        function openDialog(str, type) {
            $(document).ready(function () {
                //InitializeTerminationWatcherDialogBox();
                if (type == 'Bene') {
                    str = 'Beneficiary add,edit and delete operation can not be performed due to following reason(s). <br/>' + str
                }
                else {
                    str = 'Person\'s Marital Status can not be changed due to following reason(s).<br/>' + str;
                }
                $("#divWSMessage").html(str);
                $("#divWSMessage").dialog('open');
                return false;
            });
        }
        //End, Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)

        //SP 2014.02.05 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -Start
        function OpenViewSSNUpdatesWindow() {
            window.open('UpdateSSN.aspx?Name=person&Mode=ViewSSN', 'CustomPopUp', 'width=580, height=500 menubar=no, Resizable=No,top=70,left=120, scrollbars=yes');
            return false;
        }

        <%-- START : SB | 07/07/2016 | YRS-AT-2382 | Sending the unique beneficiary id while calling UpdateSSN Page --%>
        function OpenViewSSNUpdatesWindowAnnuities(url) {
            window.open(url, 'CustomPopUp', 'width=580, height=500 menubar=no, Resizable=No,top=70,left=120, scrollbars=yes');
            return false;
        }
        <%-- END : SB | 07/07/2016 | YRS-AT-2382 | Sending the unique beneficiary id while calling UpdateSSN Page --%>

        //Start:  Dinesh k           2014.05.15      BT-2537 : YRS 5.0-2370 - Change to validation on RMD tax rate 
        //Added New function to provide decimal number validation.
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

        function ValidateDecimalNumers(ctl) {
            var iKeyCode, objInput;
            var iMaxLen
            //var reValidChars = /[0-9],./;
            var reValidChars = "0123456789."
            //var reValidChars = /^\d*(\.\d+)?$/;
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

            if (reValidChars.indexOf(strKey) != -1) {

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
            if (((event.keyCode < 48) || (event.keyCode > 57)) && event.keyCode != 46) {
                event.returnValue = false;
            }
        }
        //End:  Dinesh k           2014.05.15      BT-2537 : YRS 5.0-2370 - Change to validation on RMD tax rate 

        //'SP 2014.02.05 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -End

       
        <%--START : VC | 2018.08.01 | YRS-AT-4018 | Method to do expand collapse on WEB or YRS loan details section--%>
        function ShowWebOrYRSLoanDetails(Application) {
            if (Application == "WEB") {
                $("div#WEBLoanDetailsExpand").hide();
                $("div#WEBLoanDetailsCollapse").show();
                $("#WEBLoanDetailsDiv").show();

                $("div#LoanDetailsCollapse").hide();
                $("div#LoanDetailsExpand").show();
                $("#LoanDetailsDiv").hide();
            }
            else {
                $("div#LoanDetailsExpand").hide();
                $("div#LoanDetailsCollapse").show();
                $("#LoanDetailsDiv").show();

                $("div#WEBLoanDetailsCollapse").hide();
                $("div#WEBLoanDetailsExpand").show();
                $("#WEBLoanDetailsDiv").hide()
            }

        }
        <%--END : VC | 2018.08.01 | YRS-AT-4018 | Method to do expand collapse on WEB or YRS loan details section--%>

        <%--START : VC | 2018.08.06 | YRS-AT-4018 | Enable/Disable button on selection of decline reason dropdown--%>
        function ddlDeclineReasonChanged(ChangeReason) {
            //If reason is changed then reset reason note
            if (ChangeReason == true) {
                $("#txtDeclineNotes").val("");
            }
            var ddlDeclineReason = $("#ddlDeclineReason").val()
            if (ddlDeclineReason == "Other") {
                if ($("#txtDeclineNotes").val().trim() == "") {
                    $("#btnConfirmDeclineWebLoan").attr("disabled", true);
                }
                else {
                    $("#btnConfirmDeclineWebLoan").attr("disabled", false);
                }
            }
            else if (ddlDeclineReason.indexOf("Select One") != -1) {
                $("#btnConfirmDeclineWebLoan").attr("disabled", true);
            }
            else {
                $("#btnConfirmDeclineWebLoan").attr("disabled", false);
            }
        }
        <%--END : VC | 2018.08.06 | YRS-AT-4018 | Enable/Disable button on selection of decline reason dropdown--%>

        <%--START : VC | 2018.08.06 | YRS-AT-4018 | Method to validate received date --%>
        function IsDocReceivedDateGreaterThanToday(sender, args) {

            var inputDate = new Date($("#txtWebDocreceivedDate").val());
            var todaysDate = new Date();
            if (fnvalidateGendate_tmp(args, fmt)) {
                if (inputDate.setHours(0, 0, 0, 0) <= todaysDate.setHours(0, 0, 0, 0)) {

                    args.IsValid = true;
                    $("#btnApproveWebLoan").attr("disabled", false);
                }
                else {
                    args.IsValid = false;
                    $("#btnApproveWebLoan").attr("disabled", true);
                    $('#CustomValidator3').css('display', 'block');
                }
            }

        }
        <%--END : VC | 2018.08.06 | YRS-AT-4018 | Method to validate received date --%>

        <%--START : VC | 2018.08.17 | YRS-AT-4018 | Method to to disable approve button if document received date is not entered--%>
        function DocreceivedDateChanged() {
            var inputDate = $("#txtWebDocreceivedDate").val();
            if (inputDate.trim() == "") {
                $("#btnApproveWebLoan").attr("disabled", true);
            }
        }
        <%--END : VC | 2018.08.17 | YRS-AT-4018 | Method to disable approve button if document received date is not entered--%>

        <%--START : VC | 2018.08.06 | YRS-AT-4018 | Method to validate received date --%>
        function IsValidDocreceivedDate(sender, args) {
            fmt = "MM/DD/YYYY";
            if (fnvalidateGendate_tmp(args, fmt)) {
                args.IsValid = true;
                $("#btnApproveWebLoan").attr("disabled", false);
            }
            else {
                args.IsValid = false;
                $("#btnApproveWebLoan").attr("disabled", true);
            }
        }
        <%--END : VC | 2018.08.06 | YRS-AT-4018 | Method to validate received date --%>

        <%--START : VC | 2018.08.01 | YRS-AT-4018 | Code for pop up div--%>
        $(document).ready(function () {
            var checkCount = 0;
            $('#DataGridLoans tr').each(function (index) {
                var myvalue = $(this).attr("class");
                if (myvalue == "DataGrid_SelectedStyle") {
                    $(this).find('td').each(function () {
                        if ($(this).html() == "YRS") {
                            ShowWebOrYRSLoanDetails("YRS");
                            checkCount = 1;
                            return false;
                        }
                        if ($(this).html() == "WEB") {
                            ShowWebOrYRSLoanDetails("WEB");
                            checkCount = 1;
                            return false;
                        }
                    });
                    if (checkCount != 0) {
                        return false;
                    }
                }
                if (checkCount != 0) {
                    return false;
                }
            });          
            /* Start : SC | 2019.05.13 | YRS-AT-2601 | All disabled textboxes would be having same style */
            //StyleForDisabledTextbox('WEBLoanDetailsDiv');
            //StyleForDisabledTextbox('LoanDetailsDiv');
            /* End : SC | 2019.05.13 | YRS-AT-2601 | All disabled textboxes would be having same style */
        });
        /* SC | 2019.05.13 | YRS-AT-2601 | Moved textbox disabled style application code to common js file*/

        function BindEvents() {
            $("#divDeclinePopUp").dialog
					({
					    modal: true,
					    open: function (event, ui) {
					    },
					    autoOpen: false,
					    title: "Decline Loan Application",
					    width: 450, height: 200,
					    open: function (type, data) {
					        $(this).parent().appendTo("form");
					        $('a.ui-dialog-titlebar-close').remove();
					    }
					});  
        }

        function showDialogConfirm(text, title) {
            $('#divConfirmDialog').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: false,
                title: "Approve Loan Application",
                close: false,
                width: 400,
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
            $('#lblMessageConfirm').text(text);
            $('#divConfirmDialog').dialog({ title: title });
            $('#divConfirmDialog').dialog("open");


        }
        function showDeclinePopUp() {
            //Reset decline dialog controls while clicking Decline button
            ddlDeclineReasonChanged(true);
            $("#ddlDeclineReason").prop('selectedIndex', 0);
            var declineButtonAuthorization = $("#hfDeclineButtonAuthorization").val();
            if (declineButtonAuthorization == 'True') {
                BindEvents()
                $('#divDeclinePopUp').dialog("open");
            }
            else {
                $("#DivSuccessAndErrorMessage").html(declineButtonAuthorization);
                $("#DivSuccessAndErrorMessage").addClass("error-msg");
            }
            
        }

        function closeDeclinePopUp() {
            $('#divDeclinePopUp').dialog('close');
        }
        <%--END : VC | 2018.08.01 | YRS-AT-4018 | Code for pop up div--%>

        <%--START : VC | 2018.08.17 | YRS-AT-4018 | Code to show approve confirmation message--%>
        function showApproveConfirmation() {
            var confirmationMessage = $("#hfApproveConfirmationMessage").val();
            var approveButtonAuthorization = $("#hfApproveButtonAuthorization").val();
            if (approveButtonAuthorization == 'True') {
                showDialogConfirm(confirmationMessage, 'Approve Loan Application');
            }
            else {
                $("#DivSuccessAndErrorMessage").html(approveButtonAuthorization);
                $("#DivSuccessAndErrorMessage").addClass("error-msg");
            }
            
        }
        <%--END : VC | 2018.08.17 | YRS-AT-4018 | Code to show approve confirmation message--%>
        <%--START : VC | 2018.09.05 | YRS-AT-4018 | Method to do close application status history dialog box--%>
        function ClossApplicationStatusHistoryDialog() {
            $('#divApplicationStatusHistory').dialog("close");
            return false;
        }
        <%--END : VC | 2018.09.05 | YRS-AT-4018 | Method to do close application status history dialog box--%>
        <%--START : VC | 2018.09.05 | YRS-AT-4018 | Method to do show application status history dialog box--%>
        function ShowApplicationStatusHistoryDialog() {
            $('#divApplicationStatusHistory').dialog("open");
            return false;
        }
        <%--END : VC | 2018.09.05 | YRS-AT-4018 | Method to do close application status history dialog box--%>
        <%-- START: MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
        function ShowPIIErrorMessage() {
            var messageText;
            var result = true;
            messageText = '<%= Me.PIIInformationRestrictionMessageText%>';
            if (messageText != null && messageText != '') {
                $("#DivSuccessAndErrorMessage").html(messageText);
                $("#DivSuccessAndErrorMessage").addClass("error-msg");
                result = false;
            }
            return result;
        }
        <%-- END: MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
    </script>
    <style type="text/css">
       /* SC | 2019.05.13 | YRS-AT-2601 | Moved textbox disabled style application code to common js file*/
        .hide {
            display: none;
        }
        .no-close .ui-dialog-titlebar-close {display: none; } /*PPP | 06/01/2017 | YRS-AT-3460 | CSS class will help to remove "X" button from dialog box*/
    </style>
</head>
<!--Added two SortExpressions/Dataformatstring for DataGridParticipantNotes by Swopna in response to YREN-4126-->
<body oncontextmenu="return false">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager runat="server" ID="scriptmanager">
        </asp:ScriptManager>
        <table class="Table_WithoutBorder" cellspacing="0" width="980" border="0">
            <tr>
                <td class="Td_BackGroundColorMenu" align="left">
                    <cc1:Menu ID="Menu1" runat="server" EnableViewState="False" Layout="Horizontal" HighlightTopMenu="False"
                        Cursor="Pointer" CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown"
                        DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2"
                        mouseovercssclass="MouseOver">
                        <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                    </cc1:Menu>
                </td>
                <td width="10%" class="Td_BackGroundColorMenu" nowrap align="left">
                    <asp:HyperLink ID="HyperLinkViewRetireesInfo" Font-Underline="False" BackColor="#000099"
                        ForeColor="#ffffff" NavigateUrl="RetireesInformationWebForm.aspx" runat="server">Retiree Information</asp:HyperLink>
                </td>
            </tr>
            <tr align="left">
                <td class="Td_HeadingFormContainer" align="left" colspan="2">
                    <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                </td>
            </tr>
            <tr>
                <%--  Changed to visble false by Anudeep on 2012.11.02 for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen ,start --%>
                <%--Reverted changes by Anudeep on 2012.11.14 for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen Changed from 'Proirity Handling' to 'Exhausted DB settlement efforts' --%>
                <td bgcolor="red" colspan="2">
                    <%-- Start: Bala: 01/19/2019: YRS-AT-2398: Priority is replaced with ExhaustedDBSettle--%>                    
                    <%--<asp:Label ID="LabelPriorityHdr" ForeColor="White" runat="server" Font-Size="12px"
                        Font-Bold="True">Exhausted DB / RMD Settlement Efforts </asp:Label>--%>
                    <asp:Label ID="LabelExhaustedDBSettleHdr" ForeColor="White" runat="server" Font-Size="12px"
                        Font-Bold="True">Exhausted DB / RMD Settlement Efforts </asp:Label>
                    <%-- End: Bala: 01/19/2019: YRS-AT-2398: Priority is replaced with ExhaustedDBSettle--%>
                </td>
                <%--  Changed to visble false by Anudeep on 2012.11.02 for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen ,end --%>
            </tr>
            <%-- Start: Bala: 01/19/2019: YRS-AT-2398: Alert message--%>
            <tr>
                <td bgcolor="red" colspan="2" style="text-align: left;">
                    <asp:Label ID="LabelSpecialHandling" ForeColor="White" runat="server" Font-Size="12px" Font-Bold="True" ></asp:Label> 
                    <a id="LinkButtonSpecialHandling" style="color: white;font-size:9px;cursor: pointer;" onclick="showToolTip('', 'OfficersDetails')" onmouseout="javascript: document.getElementById('lblComments').innerHTML = ''; hideToolTip();" >[view details]</a>
                    <asp:HiddenField id="HiddenFieldOfficerDetails" runat="server"/>
                </td>
            </tr>
            <%-- End: Bala: 01/19/2019: YRS-AT-2398: Alert message--%>
            <tr>
                <td align="left" colspan="2">
                    <asp:ValidationSummary ID="ValidationSummaryParticipants" runat="server" EnableClientScript="True"
                        CssClass="Error_Message"></asp:ValidationSummary>
                </td>
            </tr>
        </table>

        <div id="DivMainMessage" class="warning-msg" runat="server" style="text-align: left; display:none" enableviewstate="false"> <%--PK | 01.02.2019 | BT-12024 | Applied display:none to all dialog box to avoid appearance of it while loading the page.--%>
        </div>
        <%--START : VC | 2018.08.10 | YRS-AT-4018 | Added div element to display message --%>
        <div id="DivSuccessAndErrorMessage" runat="server" style="display:block;text-align: left;" enableviewstate="false">
        </div>
        <div id="DivEmailSuccessErrorMessage" runat="server" style="display:block;text-align: left;" enableviewstate="false">
        </div>
        <%--END : VC | 2018.08.10 | YRS-AT-4018 | Added div element to display message --%>
        <div class="Div_Center">
            <table cellspacing="0" width="980" border="0">
                <tbody>
                    <tr>
                        <td>
                            <iewc:TabStrip ID="TabStripParticipantsInformation" runat="server" Height="30px"
                                Width="980" TabSelectedStyle="background-color:#93BEEE;color:#000000;" TabHoverStyle="background-color:#93BEEE;color:#4172A9;"
                                TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:79;text-align:center;border:solid 1px White;border-bottom:none"
                                AutoPostBack="True" BorderStyle="None">
                                <iewc:Tab Text="General" ID="tabGeneral"></iewc:Tab>
                                <iewc:Tab Text="Employment" ID="tabEmployment"></iewc:Tab>
                                <%-- start CodeAdded by Shagufta 30/6/2011--%>
                                <%--<iewc:Tab Text="AddAccounts" ID="tabAddAccounts"></iewc:Tab>--%>
                                <iewc:Tab Text="Voluntary Accounts" ID="tabAddAccounts" />
                                <%--<iewc:Tab Text="AccContribution" ID="tabAccountContributions"></iewc:Tab>--%>
                                <iewc:Tab Text="Account Balances" ID="tabAccountContributions"></iewc:Tab>
                                <%-- End CodeAdded by Shagufta 30/6/2011--%>
                                <iewc:Tab Text="Beneficiaries" ID="tabBeneficiaries"></iewc:Tab>
                                <iewc:Tab Text="Notes" ID="tabNotes"></iewc:Tab>
                                <iewc:Tab Text="Documents" ID="tabDocuments"></iewc:Tab>
                                <iewc:Tab Text="Loans" ID="tabLoans"></iewc:Tab>
                            </iewc:TabStrip>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <iewc:MultiPage ID="MultiPageParticipantsInformation" runat="server" Width="100%">
                                <iewc:PageView Width="100%">
                                    <!-- General PageView Start -->
                                    <table id="Table1" class="Table_WithBorder" width="100%" cellspacing="0" border="0">
                                        <tr>
                                            <td align="left" width="100%" class="td_Text" colspan="6">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left" width="70%">General
                                                        <asp:Label ID="LabelGenHdr" runat="server"></asp:Label>
                                                            <!-- SR:2012.08.13:BT-957/YRS 5.0-1484 : Termination Watcher(Add TerminationWatcher Button)  -->
                                                        </td>
                                                        <td align="right" width="150px">
                                                            <input type="button" align="right" id="ButtonTerminationWatcher" value="Termination Watcher"
                                                                class="Button_Normal" runat="server" style="width: 150px" />
                                                        </td>
                                                        <td align="right" width="15%">
                                                            <input type="button" id="ButtonWebFrontEnd" style="width: 100px;" value="Web Acct Info"
                                                                class="Button_Normal" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="6" align="left">
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="LabelSal" runat="server" CssClass="Label_Small">Salute</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="DropDownListSal" runat="server" CssClass="DropDown_Normal Warn"
                                                                Width="56px" Enabled="false">
                                                                <asp:ListItem Selected="true" Value=""></asp:ListItem>
                                                                <asp:ListItem Value="Dr."></asp:ListItem>
                                                                <asp:ListItem Value="Mr."></asp:ListItem>
                                                                <asp:ListItem Value="Mrs."></asp:ListItem>
                                                                <asp:ListItem Value="Ms."></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="LabelFirst" runat="server" CssClass="Label_Small">First</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxFirst" runat="server" CssClass="TextBox_Normal Warn" Width="100px"
                                                                MaxLength="20" ReadOnly="true"></asp:TextBox>
                                                            <asp:RequiredFieldValidator CssClass="Label_Small" ID="RequiredFieldValidator1" runat="server"
                                                                ErrorMessage="First name required (General)" ControlToValidate="TextBoxFirst"
                                                                Display="Dynamic">*</asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="LabelMiddle" runat="server" CssClass="Label_Small">Middle</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxMiddle" runat="server" CssClass="TextBox_Normal Warn" Width="100px"
                                                                MaxLength="20" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="LabelLast" runat="server" CssClass="Label_Small">Last</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxLast" runat="server" CssClass="TextBox_Normal Warn" Width="100px"
                                                                MaxLength="30" ReadOnly="true"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" CssClass="Label_Small" runat="server"
                                                                ErrorMessage="Last name required (General)" ControlToValidate="TextBoxLast" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="LabelSuffix" runat="server" CssClass="Label_Small">Suffix</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxSuffix" runat="server" CssClass="TextBox_Normal Warn" Width="100px"
                                                                MaxLength="6" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Button ID="ButtonEdit" runat="server" CssClass="Button_Normal" Text="Edit" Width="73px"
                                                                CausesValidation="false" OnClientClick="javascript: return ShowPIIErrorMessage();"></asp:Button> <%--MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 18%">
                                                <asp:Label ID="LabelFundNo" runat="server" CssClass="Label_Small">Fund No</asp:Label>
                                            </td>
                                            <td align="left" style="width: 38%">
                                                <asp:TextBox ID="TextBoxFundNo" runat="server" CssClass="TextBox_Normal Warn" Width="90"
                                                    MaxLength="4" ReadOnly="true"></asp:TextBox>
                                            </td>
                                            <td rowspan="2" align="right" colspan="4" valign="top" style="width: 48%">
                                                <table class="Table_WithBorder" border="0" width="100%" cellspacing="0">
                                                    <tr>
                                                        <td align="left" class="td_Text" width="60%" colspan="2">QDRO
                                                        </td>
                                                        <td align="right" class="td_Text" width="40%" colspan="2">
                                                            <asp:Button ID="ButtonEditQDRO" runat="server" Text="Edit" CssClass="Button_Normal"
                                                                Width="73px" CausesValidation="false"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;<asp:Label ID="LabelQDROPending" runat="server" CssClass="Label_Small">Pending</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:CheckBox ID="CheckBoxQDROPending" runat="server" CssClass="Warn"></asp:CheckBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="LabelQDRODraftDate" runat="server" CssClass="Label_Small">Draft Date</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxQDRODraftDate" runat="server" CssClass="TextBox_Normal Warn"
                                                                Width="80"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;<asp:Label ID="LabelQDROStatusDate" runat="server" CssClass="Label_Small">Status</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxQDROStatusDate" runat="server" CssClass="TextBox_Normal Warn"
                                                                Width="80"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="LabelQDROType" runat="server" CssClass="Label_Small">Type</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxQDROType" runat="server" CssClass="TextBox_Normal Warn" Width="80"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="left">
                                                <asp:Label ID="LabelSSNo" runat="server" CssClass="Label_Small">SS No</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TextBoxSSNo" runat="server" CssClass="TextBox_Normal Warn" Width="90"
                                                    MaxLength="9" ReadOnly></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator3" CssClass="Label_Small" runat="server"
                                                ErrorMessage="SS No required (General)" ControlToValidate="TextBoxSSNo" Display="Dynamic">*</asp:RequiredFieldValidator>
                                                <asp:Button ID="ButtonEditSSno" runat="server" Text="Edit" CssClass="Button_Normal"
                                                    Width="73px" CausesValidation="false" OnClientClick="javascript: return ShowPIIErrorMessage();"></asp:Button> <%--MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                &nbsp;<asp:LinkButton ID="lnkbtnViewSSNUpdate" Text="View SSN Updates" OnClientClick="javascript:OpenViewSSNUpdatesWindow(); return false;" runat="server"></asp:LinkButton>
                                            </td>


                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LabelGender" runat="server" CssClass="Label_Small">Gender</asp:Label>
                                            </td>
                                            <td align="left" colspan="5">
                                                <asp:DropDownList ID="DropDownListGender" runat="server" Width="90" CssClass="DropDown_Normal Warn"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator CssClass="Label_Small" EnableClientScript="True" ID="rfvGender"
                                                    runat="server" ErrorMessage="Please select valid gender." Display="Dynamic" Enabled="false"
                                                    ControlToValidate="DropDownListGender" InitialValue="U">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="LabelMaritalStatus" runat="server" CssClass="Label_Small">Marital Status</asp:Label>
                                            </td>
                                            <td align="left">
                                                <%-- Changed by Anudeep:02-11-2012 For BT-1321-YRS 5.0-1712:lengthen marital status field  --%>
                                                <asp:DropDownList ID="DropDownListMaritalStatus" Width="150" runat="server" CssClass="DropDown_Normal Warn"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:Image ID="imgLock" runat="server" ImageUrl="Images/lock.png" Visible="false"
                                                    Width="15px" Height="15px" />
                                            </td>
                                            <td valign="top" id="td1" runat="server" rowspan="2" colspan="4" align="right" style="width: 48%">
                                                <table id="TABLE3" runat="server" class="Table_WithBorder" cellspacing="0" border="0"
                                                    width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td class="td_Text" align="left" colspan="1">Account Lock/Unlock
                                                            </td>
                                                            <td align="right" class="td_Text">
                                                                <asp:Button ID="btnAcctLockEdit" runat="server" Text="Edit" CssClass="Button_Normal"
                                                                    Width="73px" CausesValidation="false"></asp:Button>
                                                            </td>
                                                        </tr>
                                                        <!--Start: Lock reason Display Section row -->
                                                        <tr id="trLockResDisplay" runat="server">
                                                            <td align="left">
                                                                <table>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblLockstatus" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblLockResDetail" runat="server" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <!--End: Lock reason Display Section row -->
                                                        <!--Start: Lock reason Edit Section row -->
                                                        <tr id="trLockResEdit" runat="server">
                                                            <td colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td align="left" width="40%">
                                                                            <asp:Label ID="lblResCode" runat="server" CssClass="Label_Small">Reason Code:</asp:Label>
                                                                        </td>
                                                                        <td width="60%">
                                                                            <asp:DropDownList ID="ddlReasonCode" runat="server" CssClass="DropDown_Normal" Enabled="true"
                                                                                Width="98%">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" colspan="2">
                                                                            <asp:Button ID="btnLockUnlock" runat="server" Text="Lock Account" CssClass="Button_Normal"
                                                                                Width="45%" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <!--End: Lock reason Edit Section row -->
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 18%">
                                                <asp:Label ID="LabelDOB" runat="server" CssClass="Label_Small">DOB</asp:Label>
                                                <span style="color: Gray">
                                                    <asp:Label ID="Label2" runat="server">(Age:</asp:Label>
                                                    <asp:Label ID="labelBoxAge" runat="server" Width="55px"></asp:Label>) </span>
                                            </td>
                                            <td align="left" style="width: 38%">
                                                <table border="0" cellpadding="0" cellspacing="0" class="Table_WithOutBorder" width="100%">
                                                    <tr>
                                                        <td align="left" nowrap="nowrap" width="26%">
                                                            <YRSControls:DateUserControl ID="TextBoxDOB" runat="server" height="200px" CssClass="TextBox_Normal DateControl"></YRSControls:DateUserControl>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="ButtonEditDOB" runat="server" Text="Edit" CssClass="Button_Normal"
                                                                Width="73px" CausesValidation="False" OnClientClick="javascript: return ShowPIIErrorMessage();"></asp:Button> <%-- MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                        </td>
                                                        <!-----------------End-------------------------->
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="left">
                                                <asp:Label ID="LabelParticipationDate" runat="server" CssClass="Label_Small">Participation Date</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TextBoxParticipationDate" runat="server" CssClass="TextBox_Normal Warn"
                                                    Width="70" ReadOnly="true"></asp:TextBox>&nbsp;&nbsp;
                                            </td>
                                            <!--Code inserted by shashi on 27 jan 2010 For PS: YMCA PS Data Archive.Doc (Merging)-->
                                            <td valign="top" rowspan="2" align="right" colspan="4">
                                                <table id="tbltermPart" runat="server" class="Table_WithBorder" cellspacing="0" border="0"
                                                    width="98%" align="right">
                                                    <tbody>
                                                        <tr>
                                                            <td class="td_Text" align="left">Terminate Participation
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Button ID="ButtonTerminateParticipation" runat="server" Text="Terminate Participation"
                                                                    CssClass="Button_Normal" Width="170px" Visible="true"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="left" valign="top">
                                                <asp:Label ID="LabelDeceasedDate" runat="server" CssClass="Label_Small">Deceased Date</asp:Label>
                                            </td>
                                            <td align="left" colspan="5" valign="top">
                                                <asp:TextBox ID="TextBoxDeceasedDate" CssClass="TextBox_Normal Warn" runat="server"
                                                    Width="70" Enabled="false"></asp:TextBox>
                                                <rjs:PopCalendar ID="PopCalTextBoxDeceasedDate" runat="server" ScriptsValidators="IsValidDate"
                                                    Format="mm dd yyyy" Control="TextBoxDeceasedDate" Separator="/"></rjs:PopCalendar>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="IsValidDate"
                                                    ControlToValidate="TextBoxDeceasedDate" Display="Dynamic">*</asp:CustomValidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="ButtonDeathNotification" Width="140" runat="server" Text="Death Notification"
                                                CssClass="Button_Normal" Enabled="True" CausesValidation="false"></asp:Button>
                                                <asp:Button ID="btnEditDeathDatePer" Width="140" runat="server" Text="Edit Death Date"
                                                    CssClass="Button_Normal" Enabled="True" CausesValidation="false"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="left" valign="top">
                                                <asp:Label ID="LabelSpousealWiver" runat="server" CssClass="Label_Small">Spousal Waiver</asp:Label>
                                            </td>
                                            <td align="left" valign="top" colspan="1">
                                                <asp:TextBox ID="TextBoxSpousealWiver" runat="server" CssClass="TextBox_Normal DateControl Warn"
                                                    Width="70" Text=""></asp:TextBox>
                                                <rjs:PopCalendar ID="PopcalendarSpousealWiver" runat="server" ScriptsValidators="IsValidDate"
                                                    Format="mm dd yyyy" Control="TextBoxSpousealWiver" Separator="/"></rjs:PopCalendar>
                                                <asp:CustomValidator ID="valCustomDOB" runat="server" ClientValidationFunction="IsValidDate"
                                                    ControlToValidate="TextBoxSpousealWiver" Display="Dynamic">*</asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="ReqCustomDOB" CssClass="Label_Small" runat="server"
                                                    ControlToValidate="TextBoxSpousealWiver" ErrorMessage="Spousal Waiver required (General)"
                                                    Enabled="False" Display="Dynamic">*</asp:RequiredFieldValidator>
                                            </td>
                                            <!--Code inserted by shashi on Feb 01 2011 For YRS 5.0-1236, BT-698 -->
                                            <td valign="top" rowspan="2" align="right" colspan="2" id="tdRetrieveData" runat="server"
                                                width="50%">
                                                <table id="tblRetrieveData" runat="server" class="Table_WithBorder" cellspacing="0"
                                                    border="0" width="100%" align="right">
                                                    <tbody>
                                                        <tr>
                                                            <td class="td_Text" align="left">Data Archived
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Button ID="ButtonGetArchiveDataBack" runat="server" Text="Retrieve Data" CssClass="Button_Normal"
                                                                    CausesValidation="False"></asp:Button>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>--%>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <!-----------------End-------------------------->
                                        </tr>
                                        <tr>
                                            <td align="left" width="18%" valign="top">
                                                <asp:Label ID="LabelPOA" runat="server" CssClass="Label_Small">POA/Other Rep.</asp:Label>
                                            </td>
                                            <td align="left" style="width: 38%" colspan="5">
                                                <asp:Button ID="ButtonPOA" runat="server" CssClass="Button_Normal" Width="150" Text="Show/Edit all"
                                                    CausesValidation="False"></asp:Button>
                                                <asp:TextBox ID="TextBoxPOA" runat="server" CssClass="TextBox_Normal Warn" Visible="false"
                                                    Width="90"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr valign="top" style="width: 100%">
                                            <td align="left">
                                                <asp:Label ID="LabelPIN" runat="server" CssClass="Label_Small" Text="PIN No."></asp:Label>
                                            </td>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td width="50px">
                                                            <asp:TextBox ID="txtPIN" runat="server" CssClass="TextBox_Normal" ForeColor="Red" Width="35px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <input type="button" align="left" id="ButtonPINno" value="Edit" style="width: 70px;"
                                                                class="Button_Normal" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>

                                            <td align="left" valign="middle" width="22%" id="tdRMDTax" runat="server">
                                                <asp:Label ID="lblRMDTax" runat="server" CssClass="Label_Small" Text="RMD Tax Withholding (%)" Visible="false"></asp:Label>
                                            </td>
                                            <td valign="top" align="left" width="22%">
                                                <table runat="server" id="tblRMDTAX" visible="false">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxRMDTax" runat="server" CssClass="TextBox_Normal Warn" Width="60" Enabled="false" Style="text-align: right" onkeypress="ValidateDecimalNumers(this);" MaxLength="6"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnEditRMDTAX" Width="70" runat="server" Text="Edit" CssClass="Button_Normal"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <%--END : Dinesh Kanojia      2013.10.03          BT:2139: YRS 5.0-2165:RMD enhancements --%>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr valign="top">
                                                        <td align="left">
                                                            <%--<asp:Label ID="LabelPriority" runat="server" CssClass="Label_Small"></asp:Label>--%>
                                                            <%-- Start: Bala: 01/19/2019: YRS-AT-2398: Control name changed form CheckboxPriority to CheckboxExhaustedDBSettle--%>
                                                            <%--<asp:CheckBox ID="CheckboxPriority" runat="server" CssClass="Warn" Text="Exhausted DB / RMD Settlement Efforts"
                                                                AutoPostBack="True"></asp:CheckBox>--%>
                                                            <asp:CheckBox ID="CheckboxExhaustedDBSettle" runat="server" CssClass="Warn" Text="Exhausted DB / RMD Settlement Efforts"
                                                                AutoPostBack="True"></asp:CheckBox> 
                                                            <%-- End: Bala: 01/19/2019: YRS-AT-2398: Control name changed form CheckboxPriority to CheckboxExhaustedDBSettle--%>
                                                        </td>
                                                        <td align="left">
                                                            <%--<asp:Label ID="lblPersonalInfoSharingOptOut" runat="server" CssClass="Label_Small"></asp:Label>--%><asp:CheckBox
                                                                runat="server" ID="chkPersonalInfoSharingOptOut" CssClass="Warn" AutoPostBack="true"
                                                                Text="Personal Info Sharing Opt-out"></asp:CheckBox>
                                                        </td>
                                                        <td align="left">
                                                            <%--&nbsp;<asp:Label ID="lblGoPaperless" runat="server" CssClass="Label_Small"></asp:Label>--%><asp:CheckBox
                                                                runat="server" ID="chkGoPaperless" CssClass="Warn" AutoPostBack="true" Text="Go Paperless"></asp:CheckBox>
                                                        </td>
                                                        <td align="left">
                                                            <%--&nbsp;<asp:Label ID="lblMRD" runat="server" CssClass="Label_Small"></asp:Label>--%><asp:CheckBox
                                                                runat="server" ID="chkMRD" CssClass="Warn" AutoPostBack="true" Text="Requested Annual RMD" Visible="false"></asp:CheckBox> <%--SB | 2018.04.27 | YRS-AT-3844 | Display text changed from "Requested Annual MRD" to "Requested Annual RMD" --%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:CheckBox runat="server" ID="chkCannotLocateSpouse" CssClass=" CheckBox_Normal Warn" AutoPostBack="true" Text="Cannot Locate Spouse"></asp:CheckBox>
                                                            <asp:Label ID="lblCannotLocateSpouse" runat="server" CssClass="Label_Small" Style="color: Gray;"></asp:Label>
                                                        </td>
                                                        <%--Start: Bala: 01/05/2016: YRS-AT-1972: Added special death processing required check box--%>
                                                        <%--<td align="left">&nbsp;--%>
                                                        <td align="left">
                                                            <asp:CheckBox runat="server" ID="chkSpecialDeathProcess" CssClass=" CheckBox_Normal Warn" AutoPostBack="true" Text="Special Death Processing Required"></asp:CheckBox>
                                                        </td>
                                                        <%--End: Bala: 01/05/2016: YRS-AT-1972: Added special death processing required check box--%>
                                                        <%--START | Sanjay S. | 2016.02.03 - YRS-AT-2247 - Need bitflag that will not allow a participant to create a web account. This feature is turned on in 17.0.0--%>
                                                        <td align="left">
                                                            <asp:CheckBox runat="server" ID="chkNoWebAcctCreate" CssClass=" CheckBox_Normal Warn" AutoPostBack="true" Text="Restrict Web Acct. Creation"></asp:CheckBox>
                                                        </td>
                                                        <%--END | Sanjay S. | 2016.02.03 - YRS-AT-2247 - Need bitflag that will not allow a participant to create a web account. This feature is turned on in 17.0.0--%>
                                                        <td align="left">&nbsp;
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <div id="dvWebProcessing">
                                                    <label id="lblProcessing" class="Label_Small"></label>
                                                </div>
                                                <div id="divWebFront" runat="server" style="display: none">
                                                    <%--<table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LabelDispScreenName" runat="server" Text="Label" CssClass="Label_Small">Screen Name:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LabelScreenName" runat="server" Width="300" Font-Size="Smaller"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LabelDispPassword" runat="server" Text="Label" CssClass="Label_Small">Password:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LabelPassword" runat="server" Width="300" Font-Size="Smaller"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LabelDispSecurityQuestion" runat="server" Text="Label" CssClass="Label_Small">Security Question:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LabelSecurityQuestion" runat="server" Width="300" Font-Size="Smaller"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LabelDispSecurityWord" runat="server" Text="Label" CssClass="Label_Small">Security Word:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LabelSecurityWord" runat="server" Width="300" Font-Size="Smaller"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>--%>
                                                    <%--<div align="center" class="Label_Small">
                                                        Web Account information
                                                    </div>--%>
                                                    <div id="divWebAcctMessage" style="height: 35px; display: none">
                                                    </div>
                                                    <div id="divWebAcctDetails">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <label class="Label_Small">Screen Name :</label>
                                                                </td>
                                                                <td>
                                                                    <label id="lblScreenName" class="NormalMessageText"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <label class="Label_Small">Security Question :</label>
                                                                </td>
                                                                <td>
                                                                    <label id="lblSecurityQuestion" class="NormalMessageText"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <label class="Label_Small">Answer :</label>

                                                                </td>
                                                                <td>
                                                                    <label id="lblSecurityAnswer" class="NormalMessageText"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <%--<input id="chckAccountLocked" type="checkbox" class="CheckBox_Normal" disabled="disabled" />--%>
                                                                    <label class="CheckBox_Normal">Account Locked :</label>
                                                                </td>
                                                                <td>
                                                                    <label id="lblAccountLocked" class="NormalMessageText"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <label class="Label_Small">Email :</label>
                                                                </td>
                                                                <td>
                                                                    <label id="lblEmail" class="NormalMessageText"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <label class="Label_Small">Last Good Login :</label>
                                                                </td>
                                                                <td>
                                                                    <label id="lblLastGoodLogin" class="NormalMessageText"></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <label class="Label_Small">Last Bad Login :</label>
                                                                </td>
                                                                <td>
                                                                    <label id="lblLastBadLogin" class="NormalMessageText"></label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table>
                                                            <tr style="vertical-align: top;">
                                                                <td class="Label_Small">Account Activity :</td>
                                                                <td>
                                                                    <div id="divGridViewAcctActivity" style="overflow-x: hidden; overflow-y: auto; height: 200px;"></div>
                                                                </td>
                                                            </tr>
                                                        </table>


                                                        <asp:Label ID="Label13" CssClass="Label_Small" runat="server" Text=""></asp:Label>
                                                    </div>

                                                    <asp:Label ID="lblUserid" CssClass="Label_Small" runat="server" Text=""></asp:Label>
                                                    <asp:Label ID="lblName" CssClass="Label_Small" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" colspan="6">
                                                <div id="divTerminationWatcher" runat="server" style="display: none;">
                                                    <table width="690px">
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <div id="divTWGrid" style="overflow: scroll; width: 380px;" runat="server">
                                                                    <asp:GridView ID="gvPersonDetails" Width="360px" runat="server" AllowPaging="True"
                                                                        CssClass="DataGrid_Grid" BackColor="White" BorderColor="#E7E7FF" RowStyle-Width="50px"
                                                                        BorderStyle="None" BorderWidth="2px" CellPadding="3">
                                                                        <AlternatingRowStyle BackColor="#F7F7F7" />
                                                                        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                                                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                        <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                                                        <RowStyle Width="50px" BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                            <td style="background-color: Orange; color: red;"></td>
                                                            <td align="left">
                                                                <table width="310px">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <%-- 'Anudeep:06-nov-2012 - Changes Made as Per Observations listed in bugtraker For yrs 5.0-1484 on 06-nov 2012 --%>
                                                                            <asp:Label ID="lblPlan" runat="server" Text="Watch Type" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlPlan" runat="server" Font-Size="X-Small" Width="151" TabIndex="2"
                                                                                CssClass="DropDown_Normal warn">
                                                                                <asp:ListItem>Withdrawal</asp:ListItem>
                                                                                <asp:ListItem>Retirement</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:Label ID="lblPlanType" runat="server" Text="Plan" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlPlanType" Font-Size="X-Small" runat="server" TabIndex="2"
                                                                                Width="151" CssClass="DropDown_Normal">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:Label ID="lblNotes" runat="server" Text="Notes" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtTWNotes" Font-Size="X-Small" runat="server" Height="56px" Width="151px"
                                                                                TabIndex="3" TextMode="MultiLine" CssClass="TextBox_Normal"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:Label ID="lblImportant" runat="server" Text="Mark as Important" CssClass="Label_Small"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkImportant" Visible="True" runat="server" CssClass="Warn"></asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <%-- <tr>
                                    <td>
                                        <asp:Label ID="lblPlan_1" runat="server" Text="Action" CssClass="Label_Medium" font-size="Smaller"></asp:Label>
                                    </td>    
                                    <td>                                
                                        <asp:DropDownList ID="ddlPlan_1" runat="server" Height="20px" Width="143px" TabIndex="2" CssClass="DropDown_Normal" font-size="Smaller">
                                        <asp:ListItem>Withdrawal</asp:ListItem>
                                        <asp:ListItem>Retirement</asp:ListItem>                                   
                                        </asp:DropDownList>  
                                        </td>                         
                            </tr>  
                            <tr>
                                <td>
                                   <asp:Label ID="lblPlanType_1" runat="server" Text="Plan Type" CssClass="Label_Medium" font-size="Smaller"></asp:Label>                                
                                </td>    
                                <td>                                
                                    <asp:DropDownList ID="ddlPlanType_!" runat="server" Height="20px" Width="143px" TabIndex="2" font-size="Smaller" CssClass="DropDown_Normal">
                                    </asp:DropDownList>                                
                                </td>                         
                            </tr>   
                            <tr>
                                <td>                                
                                    <asp:Label ID="lblNotes_1" runat="server" Text="Notes" CssClass="Label_Medium" font-size="Smaller"></asp:Label>                                
                                </td>    
                                <td >
                                    <asp:TextBox ID="txtTWNotes_1" runat="server" Height="56px" Width="251px" TabIndex="3" TextMode="MultiLine" CssClass="TextBox_Normal" font-size="Smaller"></asp:TextBox>
                                </td>                         
                            </tr>
                            <tr> 
                                <td>                                
                                    <asp:Label ID="lblImportant_1" runat="server" Text="Is Important" CssClass="Label_Medium" font-size="Smaller"></asp:Label>                                
                                </td>  
                                <td >
                                    <asp:CheckBox ID="chkImportant_1" Visible="True" runat="server" CssClass="Warn" > </asp:CheckBox>
                                </td>
                            </tr> 
                            <tr>
                            <td> </td>
                                <td align="left">
                                  
                                         <asp:GridView ID="gvPersonDetails_1" runat="server" AllowPaging="True" CssClass="DataGrid_Grid"
                                            BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="0px" 
                                            CellPadding="3" GridLines="Horizontal">
                                        <AlternatingRowStyle BackColor="#F7F7F7" />
                                        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                        <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                                        <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                        </asp:GridView>    
                                                            
                                </td> 
                            </tr> --%>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" colspan="6">
                                                <div id="divPIN" runat="server" style="display: none;">
                                                    <div id="divMessage" style="width: 88%;">
                                                    </div>
                                                    <table width="100%">
                                                        <tr id="trPIN" runat="server">
                                                            <td class="Label_Small">
                                                                <label>Current PIN </label>
                                                            </td>
                                                            <td>
                                                                <label>:</label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPIN" runat="server" CssClass="Label_Small"></asp:Label>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="Label_Small">
                                                                <label>New PIN </label>
                                                            </td>
                                                            <td>
                                                                <label>:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" TextMode="Password" CssClass="TextBox_Normal" ID="txtPINno" MaxLength="4" OnKeyPress="Javascript: ValidatePIN();" onfocus="Javascript: ClearPIN('focus');" onPaste="return false;">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Label_Small">
                                                                <label>Confirm New PIN </label>
                                                            </td>
                                                            <td>
                                                                <label>:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" CssClass="TextBox_Normal" AutoCompleteType="Disabled" ID="txtConfirmPIN" MaxLength="4" OnKeyPress="Javascript: ValidatePIN();" onPaste="return false;">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <label>
                                                                    <b>Note:-</b>
                                                                    <br />
                                                                    1.&nbsp;&nbsp;PIN should be a four digit number.
                                                                        <br />
                                                                    2.&nbsp;&nbsp;It should not be repetition of last three Pins.
                                                                </label>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </div>
                                                <div id="dvPINConfirm" runat="server" style="display: none;">
                                                    <asp:Label ID="lblPINConfirmmessage" CssClass="Label_Small" runat="server"></asp:Label>
                                                </div>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left" width="160">
                                                <asp:Label Visible="False" ID="lblShareInfoAllowed" runat="server" CssClass="Label_Small">Sharing of Contact Info Allowed </asp:Label>
                                            </td>
                                            <td align="left" colspan="5">
                                                <asp:CheckBox ID="chkShareInfoAllowed" Visible="False" runat="server" CssClass="Warn"
                                                    AutoPostBack="True"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="6">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table border="0" class="Table_WithBorder" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td class="td_Text" id="tdPrimary" height="5px" colspan="3">
                                                                        <table>
                                                                            <tr>
                                                                                <td class="td_Text" align="left" style="cursor: pointer;" width="70%" onclick="TogglePrimaryDiv()">Primary Address / Contact Information
                                                                                <label id="tdPrimaryclick" class="Normaltext">
                                                                                    (Please click to hide details)</label>
                                                                                </td>
                                                                                <td class="td_Text" align="right" style="cursor: pointer;" width="30%" onclick="TogglePrimaryDiv()">
                                                                                    <img src="~/images/collapse.GIF" id="imgPrimary" runat="server" />
                                                                                </td>
                                                                                <td class="td_Text" align="right" width="0%">
                                                                                    <asp:Button ID="ButtonEditAddress" runat="server" CssClass="Button_Normal" Text="Edit"
                                                                                        Width="73px" CausesValidation="False" Visible="false"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <div id="divPrimary">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="left" valign="top" width="60%" class="Table_WithBorder">
                                                                                        <%--BS:2012.05.10:yrs:1470:enhancement of user control for person maintenance--%>
                                                                                        <%--  <YRSControls:AddressWebUserControl ID="AddressWebUserControl1" runat="server" AllowNote="true" AllowEffDate="true"></YRSControls:AddressWebUserControl>--%>
                                                                                        <%--<YRSControls:Enhance_AddressWebUserControl ID="AddressWebUserControl1" runat="server"
                                                                            AllowNote="true" AllowEffDate="true" />--%>
                                                                                        <NewYRSControls:New_AddressWebUserControl ID="AddressWebUserControl1" runat="server"
                                                                                            PopupHeight="530" AllowNote="true" AllowEffDate="true" />
                                                                                    </td>
                                                                                    <td colspan="2" width="40%" valign="top" align="left" class="Table_WithBorder" style="font-weight: normal;">
                                                                                        <table width="100%" runat="server" border="0">
                                                                                            <tr valign="top">
                                                                                                <td>
                                                                                                    <YRSTelephoneControls:Telephone_WebUserControl ID="TelephoneWebUserControl1" runat="server" />
                                                                                                </td>
                                                                                                <td align="right" width="100px">
                                                                                                    <%--<input id="btnEditPrimaryContact" type="button" runat="server" tabindex="50" onclick="showDialog('DivEditContacts');"
                                                                                                value="Edit Contacts" style="width: 100px" />--%>
                                                                                                    <%--<asp:Button ID="btnEditPrimaryContact" runat="server"    CssClass="Button_Normal"
                                                                                                        Text="Edit Contacts" Style="width: 100px; display:none;"  Visible="false" />--%>
                                                                                                    <asp:ImageButton ID="btnEditPrimaryContact" ImageUrl="images/Contacts_Brown.bmp"
                                                                                                        AlternateText="Add/Edit Phone/Email" Width="18" Height="18" runat="server" OnClientClick="javascript: return ShowPIIErrorMessage();"/> <%-- MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" align="right">
                                                                                                    <table border="0" width="100%" runat="server" id="tbPricontacts">
                                                                                                        <tr style="height: 120">
                                                                                                            <td align="left" width="5%">
                                                                                                                <asp:Label ID="LabelTelephone" runat="server" CssClass="Label_Small">Office</asp:Label>
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label runat="server" ID="lblOfficeContact"></asp:Label>
                                                                                                                <asp:TextBox ID="TextBoxTelephone" runat="server" CssClass="TextBox_Normal Warn"
                                                                                                                    MaxLength="10" Width="75%"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" width="5%">
                                                                                                                <asp:Label ID="LabelHomeTelephone" runat="server" CssClass="Label_Small">Home</asp:Label>
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label runat="server" ID="lblHomeContact"></asp:Label>
                                                                                                                <asp:TextBox ID="TextBoxHome" runat="server" CssClass="TextBox_Normal Warn" MaxLength="10"
                                                                                                                    Width="75%"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 120">
                                                                                                            <td align="left" width="5%">
                                                                                                                <asp:Label ID="LabelMobile" runat="server" CssClass="Label_Small">Mobile</asp:Label>
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label runat="server" ID="lblMobile"></asp:Label>
                                                                                                                <asp:TextBox ID="TextBoxMobile" runat="server" CssClass="TextBox_Normal Warn" MaxLength="10"
                                                                                                                    Width="75%"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" width="5%">
                                                                                                                <asp:Label ID="LabelFax" runat="server" CssClass="Label_Small">Fax</asp:Label>
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label runat="server" ID="lblFax"></asp:Label>
                                                                                                                <asp:TextBox ID="TextBoxFax" runat="server" CssClass="TextBox_Normal Warn" MaxLength="10"
                                                                                                                    Width="75%"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="td_Text" id="tdSecondary" colspan="3">
                                                                        <table>
                                                                            <tr>
                                                                                <td align="left" class="td_Text" style="cursor: pointer;" width="70%" onclick="ToogleSecondaryDiv()">Secondary Address / Contact Information
                                                                                <label id="tdSecondaryClick" class="Normaltext">
                                                                                    (Please click to hide details)</label>
                                                                                </td>
                                                                                <%--Start:Manthan Rajguru | 2016.02.24 | YRS-AT-2328 | Added Deactivate button control for Secondary address--%>
                                                                                <td align="right" width="15%" class="td_Text">
                                                                                    <asp:Button ID="btnDeactivateSecondaryAddrs" runat="server" CssClass="Button_Normal"
                                                                                        Text="Deactivate" Width="80px" CausesValidation="true" align="right" OnClientClick="javascript: return ShowPIIErrorMessage();"></asp:Button> <%-- MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                                </td>
                                                                                <%--End:Manthan Rajguru | 2016.02.24 | YRS-AT-2328 | Added Deactivate button control for Secondary address--%>
                                                                                <td align="right" width="15%" class="td_Text">
                                                                                    <asp:Button ID="ButtonActivateAsPrimary" runat="server" CssClass="Button_Normal"
                                                                                        Text="Activate as Primary" Width="140px" CausesValidation="true" align="right" OnClientClick="javascript: return ShowPIIErrorMessage();"></asp:Button> <%-- MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                                </td>
                                                                                <td class="td_Text" align="right" style="cursor: pointer;" width="1%" onclick="ToogleSecondaryDiv()">
                                                                                    <img src="~/images/collapse.GIF" id="imgSecondary" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <div id="divSecondary">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td nowrap align="left" width="60%" valign="top" class="Table_WithBorder">
                                                                                        <%--BS:2012.05.10:yrs:1470:enhancement of user control for person maintenance--%>
                                                                                        <%-- <YRSControls:AddressWebUserControl ID="AddressWebUserControl2" runat="server" AllowNote="true" AllowEffDate="true"></YRSControls:AddressWebUserControl>--%>
                                                                                        <%--<YRSControls:Enhance_AddressWebUserControl ID="AddressWebUserControl2" runat="server" AllowNote="true" AllowEffDate="true" />--%>
                                                                                        <NewYRSControls:New_AddressWebUserControl ID="AddressWebUserControl2" runat="server"
                                                                                            PopupHeight="530" AllowNote="true" AllowEffDate="true" />
                                                                                    </td>
                                                                                    <td colspan="2" width="40%" valign="top" align="left" class="Table_WithBorder">
                                                                                        <table width="100%" runat="server">
                                                                                            <tr valign="top">
                                                                                                <td>
                                                                                                    <YRSTelephoneControls:Telephone_WebUserControl ID="TelephoneWebUserControl2" runat="server" />
                                                                                                </td>
                                                                                                <td align="right">
                                                                                                    <%--<input id="btnEditSecondaryContact" type="button" runat="server" tabindex="50" onclick="showDialog('DivEditContacts');" value="Edit Contacts" style="width: 100px" />--%>
                                                                                                    <%--<asp:Button ID="btnEditSecondaryContact" runat="server" CssClass="Button_Normal" Text="Edit Contacts" Style="width: 100px;display:none;"  />--%>
                                                                                                    <asp:ImageButton ID="btnEditSecondaryContact" ImageUrl="images/Contacts_Brown.bmp"
                                                                                                        AlternateText="Add/Edit Phone/Email" Width="18" Height="18" runat="server" OnClientClick="javascript: return ShowPIIErrorMessage();"/> <%-- MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" align="right">
                                                                                                    <table border="0" width="100%" runat="server" id="tbSeccontacts">
                                                                                                        <tr>
                                                                                                            <td align="left" width="5%">
                                                                                                                <asp:Label ID="LabelSecTelephone" runat="server" CssClass="Label_Small">Office</asp:Label>
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label runat="server" ID="lblSecoffice"></asp:Label>
                                                                                                                <asp:TextBox ID="TextBoxSecTelephone" runat="server" CssClass="TextBox_Normal Warn"
                                                                                                                    MaxLength="10" Width="75%"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" width="5%">
                                                                                                                <asp:Label ID="LabelSecHome" runat="server" CssClass="Label_Small">Home</asp:Label>
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label runat="server" ID="lblSecHome"></asp:Label>
                                                                                                                <asp:TextBox ID="TextBoxSecHome" runat="server" CssClass="TextBox_Normal Warn" Enabled="false"
                                                                                                                    MaxLength="10" Width="75%"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" width="5%">
                                                                                                                <asp:Label ID="LabelSecMobile" runat="server" CssClass="Label_Small">Mobile</asp:Label>
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label runat="server" ID="lblSecMobile"></asp:Label>
                                                                                                                <asp:TextBox ID="TextBoxSecMobile" runat="server" CssClass="TextBox_Normal Warn"
                                                                                                                    MaxLength="10" Width="75%"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" width="5%">
                                                                                                                <asp:Label ID="LabelSecFax" runat="server" CssClass="Label_Small">Fax</asp:Label>
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label runat="server" ID="lblSecFax"></asp:Label>
                                                                                                                <asp:TextBox ID="TextBoxSecFax" runat="server" CssClass="TextBox_Normal Warn" Enabled="false"
                                                                                                                    MaxLength="10" Width="75%"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="3" class="td_Text">Email Details
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" colspan="3">
                                                                        <table>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="LabelEmailId" runat="server" CssClass="Label_Small">Email Id </asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="TextBoxEmailId" runat="server" CssClass="TextBox_Normal Warn" Width="230"
                                                                                        ReadOnly="true" MaxLength="70" Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="CheckboxBadEmail" runat="server" Width="100" Text="Bad Email" TextAlign="Left"
                                                                                        CssClass="CheckBox_Normal Warn" Enabled="false"></asp:CheckBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="CheckboxUnsubscribe" runat="server" Width="100" Text="Unsubscribe"
                                                                                        TextAlign="Left" CssClass="CheckBox_Normal Warn" Enabled="false"></asp:CheckBox>
                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="CheckboxTextOnly" runat="server" Width="90"
                                                                                        Text="Text Only" TextAlign="Left" CssClass="CheckBox_Normal Warn" Enabled="false"></asp:CheckBox>
                                                                                </td>
                                                                                <td align="right" width="160">
                                                                                    <asp:ImageButton ID="imgBtnEmail" ImageUrl="images/Contacts_Brown.bmp" AlternateText="Add/Edit Phone/Email"
                                                                                        Width="18" Height="18" runat="server" OnClientClick="javascript: return ShowPIIErrorMessage();"/> <%-- MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <%--<tr>
                                            <td colspan="2" align="left">
                                                <table border="0">
                                                    <tr>
                                                        <td nowrap align="left" width="45%">
                                                            <asp:Label ID="LabelSecEffDate" runat="server" CssClass="Label_Small">Effective Date</asp:Label>
                                                        </td>
                                                        <td nowrap align="left">
                                                            <YRSControls:DateUserControl ID="TextBoxSecEffDate" runat="server"></YRSControls:DateUserControl>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>--%>
                                                            <%--    <tr>
                                            <td nowrap align="left" colspan="2">
                                                <asp:CheckBox ID="CheckboxSecIsBadAddress" runat="server" Width="70" Text="IsBadAddress"
                                                    TextAlign="Left" CssClass="CheckBox_Normal Warn" Enabled="false"></asp:CheckBox>
                                            </td>
                                                    </tr>
                                        </tr>--%>
                                                            <%-- <tr >
                                            <td colspan="2" align="left">
                                                <table border="0">
                                                    <tr>
                                                        <td nowrap align="left" width="45%">
                                                            <asp:Label ID="LabelEffDate" runat="server" CssClass="Label_Small">Effective Date</asp:Label>
                                                        </td>
                                                        <td nowrap align="left">
                                                            <YRSControls:DateUserControl ID="TextBoxEffDate" runat="server" ></YRSControls:DateUserControl>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>--%>
                                                            <%--    <tr>
                                            <td nowrap align="left" colspan="2">
                                                <asp:CheckBox ID="CheckboxIsBadAddress" runat="server" Width="70" Text="IsBadAddress"
                                                    TextAlign="Left" CssClass="CheckBox_Normal Warn" Enabled="false"></asp:CheckBox>
                                            </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>--%>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                    <td colspan="2" align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                    </td>
                                                </tr>--%>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table class="Table_WithBorder" width="100%" height="350" cellspacing="0" border="0">
                                        <tr>
                                            <td align="left">
                                                <table width="100%" class="Table_WithOutBorder" border="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" class="td_Text">Employment - Service
                                                        </td>
                                                        <td align="right" class="td_Text">
                                                            <asp:Button ID="ButtonEditEmploymentService" runat="server" CssClass="Button_Normal"
                                                                Text="Edit" Width="73px" CausesValidation="false"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table class="Table_WithOutBorder" width="100%" border="0">
                                                    <tr>
                                                        <td align="center" colspan="2">Vested&nbsp;&nbsp;<asp:TextBox ID="TextBoxVested" runat="server" CssClass="TextBox_Normal"
                                                            ReadOnly="True" Width="30"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">&nbsp;
                                                        </td>
                                                        <td align="left">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" CssClass="Label_Medium" ID="LabelService">Service</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <asp:Label runat="server" CssClass="Label_Small" ID="LabelYears">Years</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Label runat="server" CssClass="Label_Small" ID="LabelMonths">Months</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Label runat="server" CssClass="Label_Small" ID="LabelPaid">Paid</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxYear" runat="server" CssClass="TextBox_Normal Warn" Width="30px"
                                                                ReadOnly="True"></asp:TextBox>
                                                            <asp:Label runat="server" CssClass="Label_Small" ID="LabelYear"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:TextBox ID="TextBoxMonth" runat="server" CssClass="TextBox_Normal Warn" Width="30px"
                                                            ReadOnly="True"></asp:TextBox>
                                                            <asp:Label runat="server" CssClass="Label_Small" ID="LabelMonth"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Label runat="server" CssClass="Label_Small" ID="LabelTotal">Total</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="TextBoxYearTotal" runat="server" CssClass="TextBox_Normal Warn"
                                                                Width="30px" ReadOnly="True"></asp:TextBox>
                                                            <asp:Label runat="server" CssClass="Label_Small" ID="LabelYearTotal" ReadOnly="True"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:TextBox ID="TextBoxMonthTotal" runat="server" CssClass="TextBox_Normal Warn"
                                                            Width="30px" ReadOnly="True"></asp:TextBox>
                                                            <asp:Label runat="server" CssClass="Label_Small" ID="LabelMonthTotal"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="Table_WithOutBorder">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="td_Text" align="left">Employment
                                                        </td>
                                                        <td class="Td_ButtonContainer" align="right">
                                                            <asp:Button ID="ButtonAddEmployment" Width="73px" Text="Add..." runat="server" class="Button_Normal"
                                                                Visible="true" CausesValidation="False"></asp:Button>
                                                            <asp:Button ID="ButtonUpdateEmployment" Width="90px" Text="Update Item" runat="server"
                                                                class="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                            <asp:Button ID="ButtonReactivate" Width="90px" Text="Reactivate" runat="server" class="Button_Normal"
                                                                OnClientClick="javascript:return false;" UseSubmitBehavior="false" CausesValidation="False"
                                                                Visible="false" Enabled="false"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="top">
                                                <table class="Table_WithOutBorder" width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <div style="overflow: auto; width: 100%; height: 120px; text-align: left">
                                                                <asp:DataGrid ID="DataGridParticipantEmployment" runat="server" Width="96%" CssClass="DataGrid_Grid"
                                                                    OnSortCommand="SortCommand_OnClick" AllowSorting="true" AutoGenerateColumns="False">
                                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                                    <Columns>
                                                                        <%--<asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ImageButtonEmployment" runat="server" ToolTip="Select" CommandName="Select"
                                                                                CausesValidation="False" ImageUrl="images\select.gif" AlternateText="Select">
                                                                            </asp:ImageButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>--%>
                                                                        <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageEditButtonEmployment" runat="server" ToolTip="Edit" CommandName="Edit"
                                                                                    CausesValidation="False" ImageUrl="~/images/edits.gif" AlternateText="Edit"></asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="UniqueId" HeaderText="UniqueId" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="PersonId" HeaderText="PerssId" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="YmcaId" HeaderText="YMCAId" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="FundEventId" HeaderText="FundeventId" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="HireDate" HeaderText="Hire Date" SortExpression="HireDate"
                                                                            DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="BasicPaymentDate" HeaderText="Enrollment Date" SortExpression="BasicPaymentDate"
                                                                            DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="15%"></asp:BoundColumn>
                                                                        <%--  <asp:BoundColumn DataField="Termdate" HeaderText="Term Date"   SortExpression="Termdate" DataFormatString="{0:MM/dd/yyyy}">
                                                </asp:BoundColumn>BS:2011.08.05:YRS 5.0-1380:BT:910 - to sort the Employment tab --%>
                                                                        <asp:BoundColumn DataField="Termdate" HeaderText="Term Date" SortExpression="EffectiveTermdate"
                                                                            DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="10%"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="EligibilityDate" HeaderText="Eligibility Date" Visible="False"
                                                                            DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Professional" HeaderText="Professional" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Salaried" HeaderText="Salaried" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="FullTime" HeaderText="FullTime" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="PriorService" HeaderText="PriorService" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="StatusType" HeaderText="StatusType" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="StatusDate" HeaderText="StatusDate" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Status" HeaderText="Status" SortExpression="Status" ItemStyle-Width="8%"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="YmcaName" HeaderText="YMCAName" SortExpression="YmcaName"></asp:BoundColumn>
                                                                        <asp:TemplateColumn HeaderText="YMCANo" SortExpression="YmcaNo" HeaderStyle-Width="8%">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkbtnYMCANo" CommandName="YmcaNo" CommandArgument='<%# Eval("YmcaNo") %>'
                                                                                    runat="server" CssClass="Warn_Dirty"><%# Eval("YmcaNo") %> </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="PositionType" HeaderText="PositionType" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="PositionDesc" HeaderText="PositionDesc" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="BranchId" HeaderText="BranchId" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Active" HeaderText="Active" Visible="False"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="BranchName" HeaderText="BranchName" Visible="False"></asp:BoundColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table class="Table_WithBorder" width="100%" height="350" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <table class="Table_WithOutBorder" width="100%" cellspacing="0" cellspacing="0">
                                                    <tr>
                                                        <td class="td_Text">Voluntary Accounts
                                                        </td>
                                                        <td class="Td_ButtonContainer" align="right">
                                                            <asp:Button ID="ButtonAddItem" Width="90px" Text="Add..." runat="server" class="Button_Normal"
                                                                CausesValidation="False"></asp:Button>
                                                            <asp:Button ID="ButtonAccountUpdateItem" Width="90px" Text="Update Item" runat="server"
                                                                class="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="top">
                                                <table class="Table_WithOutBorder" width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <div style="overflow: auto; width: 98%; height: 310px;">
                                                                <asp:DataGrid ID="DatagridAdditionalAccounts" runat="server" Width="70%" CssClass="DataGrid_Grid"
                                                                    AllowSorting="true" AutoGenerateColumns="False">
                                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                                    <Columns>
                                                                        <%--<asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ImageButtonAccounts" runat="server" ToolTip="Select" CommandName="Select"
                                                                                CausesValidation="False" ImageUrl="images\select.gif" AlternateText="Select">
                                                                            </asp:ImageButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>--%>
                                                                        <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageEditButtonAccounts" runat="server" ToolTip="Edit" CommandName="Edit"
                                                                                    CausesValidation="False" ImageUrl="~/images/edits.gif" AlternateText="Edit"></asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="UniqueId" HeaderText="UniqueId"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="EmpEventId" HeaderText="EmpEventID"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="YmcaId" HeaderText="YmcaId"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="AccountType" ItemStyle-Width="8%" HeaderText="Account Type"
                                                                            SortExpression="AccountType"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="YmcaNo" ItemStyle-Width="8%" HeaderText="YMCANo" SortExpression="YmcaNo"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="BasisCode" ItemStyle-Width="8%" HeaderText="BasisCode"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Descriptions" ItemStyle-Width="8%" HeaderText="Descriptions"
                                                                            SortExpression="Descriptions"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Contribution%" HeaderText="Contribution%" ItemStyle-Width="8%"
                                                                            SortExpression="Contribution%"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Contribution" HeaderText="Contribution" ItemStyle-Width="8%"
                                                                            SortExpression="Contribution"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="EffectiveDate" HeaderText="Effective Date" DataFormatString="{0:MM/dd/yyyy}"
                                                                            ItemStyle-Width="8%" SortExpression="EffectiveDate"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="TerminationDate" HeaderText="Termination Date" DataFormatString="{0:MM/dd/yyyy}"
                                                                            ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left" SortExpression="TerminationDate"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="HireDate" HeaderText="HireDate" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="EnrollmentDate" HeaderText="EnrollmentDate" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table width="100%" class="Table_WithBorder" cellspacing="0">
                                        <tr>
                                            <td align="left" class="td_Text">&nbsp;Account Balances<asp:Label ID="LabelAccContHdr" runat="server" CssClass="td_Text"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label runat="server" Width="64" CssClass="Label_Small" ID="LabelDate">Date</asp:Label>&nbsp;
                                                        <asp:TextBox runat="server" Width="80" CssClass="TextBox_Normal" ID="TextBoxDate"
                                                            AutoPostBack="True"></asp:TextBox>
                                                            <rjs:PopCalendar ID="Popcalendar2" runat="server" Separator="/" autopostback="true"
                                                                Format="mm dd yyyy" SelectionChanged="LoadAccountContributionsTab" ScriptsValidators="No Validate"></rjs:PopCalendar>
                                                            <asp:CustomValidator ID="CustomvalidatorDate" runat="server" ClientValidationFunction="IsValidDate"
                                                                ControlToValidate="TextBoxDate" Display="Dynamic">*</asp:CustomValidator>
                                                        </td>
                                                        <td width="140">&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:RadioButtonList ID="RadioButtonList1" RepeatDirection="Horizontal" runat="server"
                                                                AutoPostBack="True" CssClass="Label_Small">
                                                                <asp:ListItem Value="Funded Date">Funded Date</asp:ListItem>
                                                                <asp:ListItem Value="TransactionDate" Selected="True">Transaction Date</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="td_Text_Small" colspan="4">&nbsp;Retirement Plan
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <!--<DIV style="OVERFLOW: auto; WIDTH: 690px; TEXT-ALIGN: left">-->
                                                <asp:DataGrid ID="DataGridRetirementAccntContributions" runat="server" Width="98%"
                                                    CssClass="DataGrid_Grid" AutoGenerateColumns="False">
                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
                                                    <ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                    <SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Acct" HeaderText="Account" ItemStyle-Width="10%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpTaxable" HeaderText="EmpTaxable" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpNonTaxable" HeaderText="EmpNonTaxable" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpInterest" HeaderText="EmpInterest" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpTotal" HeaderText="EmpTotal" ItemStyle-Width="12%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="YMCATaxable" HeaderText="YMCATaxable" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="YMCAInterest" HeaderText="YMCAInterest" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="YMCATotal" HeaderText="YMCATotal" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AcctTotal" HeaderText="AcctTotal" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PlanType" HeaderText="PlanType" Visible="False"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                                <!--</DIV>-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="td_Text_Small" colspan="4">&nbsp;Savings Plan
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <!--<div style="OVERFLOW: auto; WIDTH: 700px; TEXT-ALIGN: left">-->
                                                <asp:DataGrid ID="DataGridSavingAccntContributions" runat="server" Width="98%" CssClass="DataGrid_Grid"
                                                    AutoGenerateColumns="False">
                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
                                                    <ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                    <SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Acct" HeaderText="Account" ItemStyle-Width="10%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpTaxable" HeaderText="EmpTaxable" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpNonTaxable" HeaderText="EmpNonTaxable" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpInterest" HeaderText="EmpInterest" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpTotal" HeaderText="EmpTotal" ItemStyle-Width="12%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="YMCATaxable" HeaderText="YMCATaxable" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="YMCAInterest" HeaderText="YMCAInterest" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="YMCATotal" HeaderText="YMCATotal" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AcctTotal" HeaderText="AcctTotal" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PlanType" HeaderText="PlanType" Visible="False"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                                <!--</div>-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="td_Text_Small" colspan="4">&nbsp;Grand Total
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <!--	<div style="OVERFLOW: auto; WIDTH: 690px; TEXT-ALIGN: left">-->
                                                <asp:DataGrid ID="DatagridAcctTotal" runat="server" Width="98%" CssClass="DataGrid_Grid"
                                                    AutoGenerateColumns="False" ShowHeader="False">
                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle_temp"></AlternatingItemStyle>
                                                    <ItemStyle CssClass="DataGrid_NormalStyle_temp"></ItemStyle>
                                                    <SelectedItemStyle CssClass="DataGrid_SelectedItem_Total"></SelectedItemStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField=" " HeaderText="      " ItemStyle-Width="9%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpTaxable" HeaderText="EmpTaxable" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpNonTaxable" HeaderText="EmpNonTaxable" ItemStyle-Width="14%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpInterest" HeaderText="EmpInterest" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EmpTotal" HeaderText="EmpTotal" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="YMCATaxable" HeaderText="YMCATaxable" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="YMCAInterest" HeaderText="YMCAInterest" ItemStyle-Width="12%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="YMCATotal" HeaderText="YMCATotal" ItemStyle-Width="11%"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="AcctTotal" HeaderText="AcctTotal" ItemStyle-Width="11%"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                                <!--</div>-->
                                            </td>
                                        </tr>
                                        <!--		<tr>
											<td align="center">
												<DIV style="OVERFLOW: auto; WIDTH: 690px; HEIGHT: 200px; TEXT-ALIGN: left">
													<asp:datagrid id="DataGridPartAcctContribution" runat="server" Width="690px" CssClass="DataGrid_Grid"
														allowsorting="false">
														<HeaderStyle cssclass="DataGrid_HeaderStyle"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
														<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
													</asp:datagrid>
													<asp:datagrid id="DatagridTotal" runat="server" Width="690px" CssClass="DataGrid_Grid" allowsorting="true">
														<HeaderStyle cssclass="DataGrid_HeaderStyle" ForeColor="#ffffff" height="1px" BackColor="#ffffff"></HeaderStyle>
														<AlternatingItemStyle CssClass="DataGrid_HeaderStyle" BackColor="#c9dbed" height="1px"></AlternatingItemStyle>
														<ItemStyle CssClass="DataGrid_HeaderStyle" BackColor="#c9dbed" height="1px"></ItemStyle>
														<SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
													</asp:datagrid>
												</DIV>
											</td>
										</tr>-->
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table width="100%" class="Table_WithBorder" height="350" cellspacing="0">
                                        <tr>
                                            <td align="left" class="td_Text">Beneficiaries
                                            <asp:Image ID="imgLockBeneficiary" runat="server" ImageUrl="Images/lock-yellow.png"
                                                Visible="false" Width="20px" Height="20px" />
                                            </td>
                                            <td align="right" class="td_Text" height="5%" nowrap>
                                                <asp:Button ID="ButtonAddActive" runat="server" Text="Add..." Width="73px" CssClass="Button_Normal"
                                                    CausesValidation="False"></asp:Button>
                                                <asp:Button ID="ButtonEditActive" runat="server" Text="Edit" Width="73px" CssClass="Button_Normal"
                                                    CausesValidation="False" Visible="false"></asp:Button>
                                                <asp:Button ID="ButtonDeleteActive" runat="server" Text="Delete" Width="73px" CssClass="Button_Normal"
                                                    Visible="false"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" colspan="2" style="width: 98%; height: 310px;">
                                                <table id="Table2" width="100%" border="0">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:Label runat="server" CssClass="Label_Large" ID="LabelNotSet">Beneficiaries cannot be set</asp:Label>
                                                            <div style="overflow: auto; width: 100%; height: 120px; text-align: left;">
                                                                <asp:DataGrid ID="DataGridActiveBeneficiaries" runat="server" Width="98%" CssClass="DataGrid_Grid"                                                                    
                                                                    AllowSorting="false"  AutoGenerateColumns="false">    <%-- // SB | 07/07/2016 | YRS-AT-2382 | Turned off auto generate columns--%>
                                                                    <%-- // SR | 2016.08.02 | YRS-AT-2382 | Turned off sorting columns--%>
                                                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                    <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                    <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                    <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                                    <Columns>
                                                                        <%--<asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="Imagebutton1" runat="server" ToolTip="Select" CommandName="Select"
                                                                                CausesValidation="False" ImageUrl="images\select.gif" AlternateText="Select">
                                                                            </asp:ImageButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>--%>
                                                                   <asp:TemplateColumn>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageEditBenefeciarybutton" runat="server" ToolTip="Edit" CommandName="Edit"
                                                                                    CausesValidation="False" ImageUrl="images\edits.gif" AlternateText="Edit"></asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageDeleteBenefeciarybutton" runat="server" ToolTip="Delete"
                                                                                    CommandName="Delete" CausesValidation="False" ImageUrl="images\Delete.gif" AlternateText="Delete"></asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                            <%--  // START : SB | 07/07/2016 | YRS-AT-2382 | Columns are now binded here manually--%>
                                                                            <asp:BoundColumn HeaderText="UniqueId" DataField="UniqueId"  Visible="false"/>
                                                                            <asp:BoundColumn HeaderText="PersId" DataField="PersId"  Visible="false"/>
                                                                            <asp:BoundColumn HeaderText="BenePersId" DataField="BenePersId"  Visible="false"/>
                                                                            <asp:BoundColumn HeaderText="BeneFundEventId" DataField="BeneFundEventId" Visible="false"/>
                                                                            <asp:BoundColumn HeaderText="Name" DataField="Name"  SortExpression="Name"/>
                                                                            <asp:BoundColumn HeaderText="Name2" DataField="Name2" SortExpression="Name2" />
                                                                            <asp:TemplateColumn HeaderText="TaxID" SortExpression="TaxID">
                                                                            <ItemTemplate >
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                             <asp:Label ID="lblSSNo" runat="server" Text='<%# Bind("TaxID")%>'></asp:Label>
                                                                                            </td>
                                                                                    <td>
                                                                                                 <asp:HyperLink runat="server" NavigateUrl='<%# Eval("UniqueId", "~/UpdateSSN.aspx?Name=personAndBeneficiary&Mode=ViewSSN&BenefID={0}")%>'  Target="_blank" ID="hypViewSSN"
                                                                                                     onclick="javascript: OpenViewSSNUpdatesWindowAnnuities(this.href); return false;"  >
                                                                                                    <span class="ui-button-icon-primary ui-icon ui-icon-clock" style="float: left; cursor: pointer" title="View SSN Updates"/>
                                                                                                     <%--onclick="window.open (this.href,'CustomPopUp','width=500,height=500,scrollbars,resizable'); return false;"  >--%>
                                                                                                 </asp:HyperLink>
                                                                                             </td>
                                                                                            </tr>
                                                                                    </table>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                            <%--Start -Manthan Rajguru | 2016.07.29 | YRS-AT-2382 |Commented existing code and added sort expression to header --%>
                                                                            <%--<asp:BoundColumn HeaderText="Rel" DataField="Rel"  Visible="true" ItemStyle-Width ="3%"/>--%> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Rel" DataField="Rel"  Visible="true" ItemStyle-Width ="3%" SortExpression ="Rel"/>
                                                                            <%--End -Manthan Rajguru | 2016.07.29 | YRS-AT-2382 |Commented existing code and added sort expression to header --%>
                                                                        <%--Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                                            <%--<asp:BoundColumn HeaderText="Birthdate" DataField="Birthdate" />--%>
                                                                            <asp:BoundColumn HeaderText="Birth/Estd. Date" DataField="Birthdate" SortExpression="Birthdate" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width ="11%"  />
                                                                            <%--End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                                            <asp:BoundColumn HeaderText="BeneficiaryTypeCode" DataField="BeneficiaryTypeCode" SortExpression="BeneficiaryTypeCode" ItemStyle-Width ="6%"  /> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Groups" DataField="Groups" SortExpression="Groups" ItemStyle-Width ="5%"/> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Lvl" DataField="Lvl" SortExpression="Lvl" ItemStyle-Width ="4%"  /> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="DeathFundEventStatus" DataField="DeathFundEventStatus"  Visible="false"/>
                                                                            <asp:BoundColumn HeaderText="BeneficiaryStatusCode" DataField="BeneficiaryStatusCode" Visible="false"/>
                                                                            <%--Start -Manthan Rajguru | 2016.07.29 | YRS-AT-2382 |Commented existing code and added sort expression to header --%>
                                                                            <%--<asp:BoundColumn HeaderText="Pct" DataField="Pct" ItemStyle-HorizontalAlign="Right" ItemStyle-Width ="6%"/>--%> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Pct" DataField="Pct" ItemStyle-HorizontalAlign="Right" ItemStyle-Width ="6%" SortExpression ="Pct"/>
                                                                            <%--End -Manthan Rajguru | 2016.07.29 | YRS-AT-2382 |Commented existing code and added sort expression to header --%>
                                                                            <asp:BoundColumn HeaderText="PlanType" DataField="PlanType" SortExpression="PlanType" ItemStyle-Width ="6%"  /> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Updated On" DataField="Updated On" SortExpression="Updated On" ItemStyle-Width ="16%"   /> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Updated By " DataField="Updated By" SortExpression="Updated By" ItemStyle-Width ="10%"  /> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="NewId" DataField="NewId"  Visible="false"/>
                                                                            <asp:BoundColumn HeaderText="RepFirstName" DataField="RepFirstName" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="RepLastName" DataField="RepLastName" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="RepSalutation" DataField="RepSalutation" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="RepTelephone" DataField="RepTelephone" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="IsExistingAudit" DataField="IsExistingAudit" Visible="false" />                                                                          
                                                                            <%--  // END : SB | 07/07/2016 | YRS-AT-2382 | Columns are now binded here manually--%>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" align="center">
                                                            <table id="Table4" cellpadding="0" width="35%" border="0">
                                                                <tr>
                                                                    <td colspan="5" align="center">
                                                                        <asp:Label ID="LabelPercentage1" runat="server" CssClass="Label_Small">Percentage</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="center" width="50%">
                                                                        <asp:Label ID="Label7" runat="server" CssClass="Label_Small">Member  %</asp:Label>
                                                                    </td>
                                                                    <td colspan="3" align="center" width="50%">
                                                                        <asp:Label ID="Label8" runat="server" CssClass="Label_Small">Saving  %</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <%--<tr>
                                                                <td colspan="2" align="center">
                                                                    <asp:Label ID="Label9" runat="server" CssClass="Label_Small">%</asp:Label>
                                                                </td>
                                                                <td colspan="3" align="center">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="Label_Small">%</asp:Label>
                                                                </td>
                                                            </tr>--%>
                                                                <tr>
                                                                    <td width="20%">
                                                                        <asp:Label ID="LabelPrimaryA" runat="server" CssClass="Label_Small">Primary</asp:Label>
                                                                    </td>
                                                                    <td width="35%">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="TextBoxPrimaryA" runat="server" Width="50px" CssClass="TextBox_Normal Warn"
                                                                                        Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Button ID="ButtonPriA" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                                                </td>
                                                                            </tr>

                                                                        </table>

                                                                    </td>
                                                                    <td width="5%">&nbsp;
                                                                    </td>
                                                                    <td width="40%">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="TextBoxPrimaryInsA" runat="server" Width="50px" CssClass="TextBox_Normal Warn"
                                                                                        Enabled="False"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Button ID="ButtonPriInsA" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                                                </td>
                                                                            </tr>

                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LabelCont1A" runat="server" CssClass="Label_Small">Cont1</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="TextBoxCont1A" runat="server" Width="50px" CssClass="TextBox_Normal Warn"
                                                                                        Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Button ID="ButtonCont1A" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="TextBoxCont1InsA" runat="server" Width="50px" CssClass="TextBox_Normal Warn"
                                                                                        Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Button ID="ButtonCont1InsA" runat="server" Text="E" CssClass="Button_Normal"
                                                                                        CausesValidation="False"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LabelCont2A" runat="server" CssClass="Label_Small">Cont2</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table width="90%" align="center">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="TextBoxCont2A" runat="server" Width="50px" CssClass="TextBox_Normal Warn"
                                                                                        Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Button ID="ButtonCont2A" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <table width="80%" align="center">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="TextBoxCont2InsA" runat="server" Width="50px" CssClass="TextBox_Normal Warn"
                                                                                        Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Button ID="ButtonCont2InsA" runat="server" Text="E" CssClass="Button_Normal"
                                                                                        CausesValidation="False" Visible="false"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LabelCont3A" runat="server" CssClass="Label_Small">Cont3</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table width="90%" align="center">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="TextBoxCont3A" runat="server" Width="50px" CssClass="TextBox_Normal Warn"
                                                                                        Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Button ID="ButtonCont3A" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <table width="80%" align="center">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="TextBoxCont3InsA" runat="server" Width="50px" CssClass="TextBox_Normal Warn"
                                                                                        Enabled="false"></asp:TextBox>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Button ID="ButtonCont3InsA" runat="server" Text="E" CssClass="Button_Normal"
                                                                                        CausesValidation="False" Visible="false"></asp:Button>
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
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table width="100%" class="Table_WithBorder" height="350" cellspacing="0">
                                        <tr>
                                            <td align="left" class="td_Text">Notes
                                            </td>
                                            <td align="right" class="Td_ButtonContainer">
                                                <asp:Button ID="ButtonView" runat="server" Width="90px" Enabled="False" Text="View Item"
                                                    CssClass="Button_Normal" CausesValidation="false" Visible="false"></asp:Button>&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="ButtonAddItemNotes" runat="server" Width="90px" Text="Add..." CssClass="Button_Normal"
                                                CausesValidation="False"></asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" colspan="2" style="width: 100%; height: 310px;">
                                                <div style="overflow: auto; width: 100%; height: 320px; text-align: left">
                                                    <asp:DataGrid ID="DataGridParticipantNotes" runat="server" Width="100%" CssClass="DataGrid_Grid"
                                                        AutoGenerateColumns="False" AllowSorting="true">
                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                        <Columns>
                                                            <%--<asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButtonNotes" runat="server" ToolTip="Select" CommandName="Select"
                                                                    CausesValidation="False" ImageUrl="images\select.gif" AlternateText="Select">
                                                                </asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>--%>
                                                            <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ImageButtonNotes" runat="server" ToolTip="View" CommandName="View"
                                                                        CausesValidation="False" ImageUrl="~/images/view.gif" AlternateText="View"></asp:ImageButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn HeaderText="Date" DataField="Date" SortExpression="Date" ItemStyle-Width="10%"
                                                                DataFormatString="{0:MM/dd/yyyy}" />
                                                            <asp:BoundColumn HeaderText="Creator" DataField="Creator" SortExpression="Creator"
                                                                ItemStyle-Width="10%" />
                                                            <asp:BoundColumn HeaderText="First Line Of Notes" DataField="Note" />
                                                            <asp:BoundColumn HeaderText="UniqueId" DataField="UniqueID" Visible="false" />
                                                            <asp:TemplateColumn HeaderText="Mark As Important" ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckBoxImportant" runat="server" CssClass="Warn" AutoPostBack="True"
                                                                        OnCheckedChanged="Check_Clicked" Enabled="False" Checked='<%# Databinder.Eval(Container.DataItem, "bitImportant") %>'></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <%--Start: Bala: 12/01/2016: YRS-AT-1718: Added Delete button--%>
                                                            <asp:TemplateColumn HeaderText ="Delete" ItemStyle-Width="10%" Visible="true">
                                                               <ItemTemplate>
                                                                   <asp:LinkButton ID="DeleteNotes" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                                                               </ItemTemplate>
                                                           </asp:TemplateColumn>
                                                            <%-- End: Bala: 12/01/2016: YRS-AT-1718: Added Delete button --%>
                                                        </Columns>
                                                    </asp:DataGrid>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table class="Table_WithBorder" width="100%" height="350" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <table class="Table_WithOutBorder" width="100%" cellspacing="0">
                                                    <tr>
                                                        <td align="left" class="td_Text" colspan="2">
                                                            <table class="td_Text" width="100%">
                                                                <tr>
                                                                    <td align="left">Documents
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:ImageButton runat="server" ID="LinkButtonIDM" ImageUrl="~/images/icon_new_window.gif"
                                                                            AlternateText="View documents in new window" ToolTip="View documents in new window" />
                                                                        <asp:ImageButton runat="server" ID="LinkRefereshIDM" ImageUrl="~/images/Refresh.jpg"
                                                                            AlternateText="Refresh IDM Page" Visible="false" ToolTip="Refresh IDM Page" />
                                                                        <%--<asp:LinkButton ID="LinkButtonIDM" runat="server">View IDM IN Window</asp:LinkButton>--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="width: 98%; height: 310px;">&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table class="Table_WithBorder" id="Table2" width="100%" border="0" height="auto" cellspacing="0"><%--VC | 2018.09.05 | YRS-AT-4018 | Changed fixed height to auto--%>
                                        <tbody>
                                            <tr>
                                                <td align="left" class="td_Text" colspan="4">Loans
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <div style="overflow: auto; width: 100%; height: 80px; text-align: left">
                                                        <%--ML | 2019.01.17 | YRS-AT-3157 | Reduced width from 99% to 98% to remove scroll bar after adding history icon column --%>
                                                        <asp:DataGrid ID="DataGridLoans" runat="server" Width="98%" CssClass="DataGrid_Grid"  
                                                            AllowSorting="false" AutoGenerateColumns="false">
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                            <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="Imagebutton3" runat="server" ToolTip="Select" CommandName="Select"
                                                                            CausesValidation="False" ImageUrl="images\select.gif" AlternateText="Select"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="PersId" HeaderText="PersId" Visible="False"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="EmpEventId" HeaderText="EmpEventId" Visible="False"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="YmcaId" HeaderText="YmcaId" Visible="False"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="LoanNumber" HeaderText="Loan Number" Visible="True"></asp:BoundColumn> <%--MMR | 2018.04.06 |YRS-AT-3935 | Added space in header text --%>
                                                                <asp:BoundColumn DataField="TDBalance" HeaderText="TD Balance" Visible="True"></asp:BoundColumn> <%--MMR | 2018.04.06 |YRS-AT-3935 | Added space in header text --%>
                                                                <asp:BoundColumn DataField="RequestedAmount" HeaderText="Requested Amount" Visible="True"></asp:BoundColumn> <%--MMR | 2018.04.06 |YRS-AT-3935 | Added space in header text --%>
                                                                <asp:BoundColumn DataField="PaymentMethodCode" HeaderText="Payment Method" Visible="True"></asp:BoundColumn> <%--MMR | 2018.04.06 |YRS-AT-3935 | Added new column --%>
                                                                <asp:BoundColumn DataField="RequestDate" HeaderText="Request Date" Visible="True"></asp:BoundColumn> <%--MMR | 2018.04.06 |YRS-AT-3935 | Added space in header text --%>
                                                                <asp:BoundColumn DataField="RequestStatus" HeaderText="Request Status" Visible="True"></asp:BoundColumn> <%--MMR | 2018.04.06 |YRS-AT-3935 | Added space in header text --%>
                                                                <asp:BoundColumn DataField="Application" HeaderText="Application" Visible="True"></asp:BoundColumn><%--VC | 2018.08.02 | YRS-AT-4018 | Added new column Application --%>
                                                                <asp:BoundColumn DataField="LoanRequestId" HeaderText="LoanRequestId" Visible="False"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="OriginalLoanNumber" HeaderText="Original Loan Number" Visible="True"></asp:BoundColumn> <%--MMR | 2018.04.06 |YRS-AT-3935 | Added space in header text --%>
                                                                <asp:BoundColumn DataField="PersBankingEFTID" HeaderText="PersBankingEFTID" Visible="False"></asp:BoundColumn> <%--MMR | 2018.04.06 |YRS-AT-3935 | Added new column --%>
                                                                <asp:BoundColumn DataField="ActualRequestStatus" HeaderText="ActualRequestStatus" Visible="True" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"></asp:BoundColumn><%--VC | 2018.10.11 | YRS-AT-4018 | Added ActualRequestStatus column  --%>
                                                                <asp:BoundColumn DataField="DisbursementEFTStatus" HeaderText="DisbursementEFTStatus" Visible="True" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"></asp:BoundColumn><%--VC | 2018.10.24 | YRS-AT-4018 | Added Disbursement status column  --%>
                                                              <%--START: ML | 2019.01.17 |YRS-AT-3157 | Added new column to display History Icon --%>
                                                                  <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                         <asp:ImageButton ID="Imageloanhistory" runat="server"  CommandName="ViewHistory"  ToolTip='<%#Eval("HistoryToolTip")%>'
                                                                            CausesValidation="False" ImageUrl="images\history.gif"  style="cursor: pointer;height:11px;" OnClientClick="javascript: ShowLoanFreezeUnfreezeHistoryDialog(this); return false;" ></asp:ImageButton>
                                                                        <asp:HiddenField ID ="hndloandetailId" runat="server" Value='<%#Eval("LoanDetailsId")%>'></asp:HiddenField>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <%--END: ML | 2019.01.17 |YRS-AT-3157 | Added new column to display History Icon --%>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                        <br />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                                   <%--START: VC | 2018.08.02 | YRS-AT-4018 | Aspx code to creae web loan details section --%>
                                            <tr style="cursor: pointer">
                                                <td colspan="4" align="left" class="td_Text">
                                                    <div id="WEBLoanDetailsExpand" style="display: none">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left" colspan="2" id="tdWEBLoanDetailsExpand" style="width:81%">WEB Loan Details
                                                                <asp:Label ID="Label14" runat="server" Text=" (Please click here to view details.)" Font-Size="Small"
                                                                    Font-Bold="false"></asp:Label>
                                                                </td>
                                                                 <td align="right">
                                                                    <asp:LinkButton Visible="false" ID="lnkReturnToLoanAdminExpand" runat="server" OnClick="lnkReturnToLoanAdmin_Click" Text="Return to Loan Admin"></asp:LinkButton>
                                                                </td>
                                                                <td align="right" id="tdImgWEBLoanDetailsExpand">
                                                                    <img src="~/images/expand.GIF" id="img5" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id="WEBLoanDetailsCollapse">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left" colspan="2" id="tdWEBLoanDetailsCollapse" style="width:81%">WEB Loan Details
                                                                <asp:Label ID="Label15" runat="server" Text=" (Please click here to hide details.)"
                                                                    Font-Size="Small" Font-Bold="false"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:LinkButton Visible="false" ID="lnkReturnToLoanAdmin" runat="server" OnClick="lnkReturnToLoanAdmin_Click" Text="Return to Loan Admin"></asp:LinkButton>
                                                                </td>
                                                                <td align="right" id="tdImgWEBLoanDetailsCollapse">
                                                                    <img src="~/images/collapse.GIF" id="img6" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <div id="WEBLoanDetailsDiv" >
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                             <%-- START : SC | 04/21/2020 | YRS-AT-4853 | Added new column Loan Type for COVID Care act --%>
                                                            <tr>
                                                                <td align="left" width="25%">&nbsp;<asp:Label ID="lblWebLoanNumber" runat="server" CssClass="Label_Small">Loan #</asp:Label>
                                                                </td>
                                                                <td align="left" width="25%">
                                                                    <asp:TextBox ID="txtWebLoanNumber" runat="server" CssClass="TextBox_Normal" Width="89px"
                                                                        ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td align="left" width="25%">&nbsp;<asp:Label ID="lblWebApplicationStatus" runat="server" CssClass="Label_Small">Application Status</asp:Label>
                                                                </td>
                                                                <td align="left" width="25%">
                                                                    <asp:TextBox ID="txtWebApplicationStatus" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                    <img src="images/history.gif" id="imgStatusHistory" runat="server" alt="History" style="cursor: pointer;height:11px;" onclick="javascript:ShowApplicationStatusHistoryDialog();"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="lblWebRequestedAmount" runat="server" CssClass="Label_Small">Requested Amount</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWebRequestedAmount" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                </td>--%>
                                                                <td align="left" width="25%">&nbsp;<asp:Label ID="lblWebLoanType" runat="server" CssClass="Label_Small">Loan Type</asp:Label>
                                                                </td>
                                                                <td align="left" width="25%">
                                                                <asp:TextBox ReadOnly="true" ID="txtWebLoanType" runat="server" CssClass="TextBox_Normal Warn" Width="89px" />
                                                                </td>
                                                                <td align="left">&nbsp;<asp:Label ID="lblWebStatusDateTime" runat="server" CssClass="Label_Small">Status Date / Time</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWebStatusDateTime" runat="server" ReadOnly="True"
                                                                        CssClass="TextBox_Normal Warn" Width="123px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td align="left" >&nbsp;<asp:Label ID="lblMaxAvailableLoanAmount" runat="server" CssClass="Label_Small">Max Available Loan Amount</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtMaxAvailableLoanAmount" runat="server" ReadOnly="True" CssClass="TextBox_Normal Warn"
                                                                        Width="89px"></asp:TextBox>
                                                                </td>--%>
                                                                <td align="left">&nbsp;<asp:Label ID="lblWebRequestedAmount" runat="server" CssClass="Label_Small">Requested Amount</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWebRequestedAmount" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                 <td align="left">&nbsp;<asp:Label ID="lblWebApplicationDateTime" runat="server" CssClass="Label_Small">Application Date / Time</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWebApplicationDateTime" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="123px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="lblWebRequestedTerm" runat="server" CssClass="Label_Small">Requested Term</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWebRequestedTerm" runat="server" ReadOnly="True" CssClass="TextBox_Normal Warn"
                                                                        Width="89px"></asp:TextBox>
                                                                </td>--%>
                                                                <td align="left" >&nbsp;<asp:Label ID="lblMaxAvailableLoanAmount" runat="server" CssClass="Label_Small">Max Available Loan Amount</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtMaxAvailableLoanAmount" runat="server" ReadOnly="True" CssClass="TextBox_Normal Warn"
                                                                        Width="89px"></asp:TextBox>
                                                                </td>
                                                                 <td align="left">&nbsp;<asp:Label ID="lblWebLoanPurpose" runat="server" CssClass="Label_Small">Loan Purpose</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWebLoanPurpose" runat="server" ReadOnly="True" CssClass="TextBox_Normal Warn"
                                                                        Width="123px" Text="TD Loan"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="lblWebPayrollFrequency" runat="server" CssClass="Label_Small">Payroll Frequency</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWebPayrollFrequency" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                </td>--%>
                                                                <td align="left">&nbsp;<asp:Label ID="lblWebRequestedTerm" runat="server" CssClass="Label_Small">Requested Term</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWebRequestedTerm" runat="server" ReadOnly="True" CssClass="TextBox_Normal Warn"
                                                                        Width="89px"></asp:TextBox>
                                                                </td>
                                                                <td align="left">&nbsp;<asp:Label ID="lblWebReasonNotes" runat="server" CssClass="Label_Small">Reason Notes</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWebReasonNotes" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="180px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">&nbsp;<asp:Label ID="lblWebPayrollFrequency" runat="server" CssClass="Label_Small">Payroll Frequency</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtWebPayrollFrequency" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                             <%-- END : SC | 04/21/2020 | YRS-AT-4853 | Added new column Loan Type for COVID Care act --%>
                                                            <tr>
                                                                <td colspan="2">&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table id="Table5" width="100%" class="Table_WithBorder" border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td align="left" width="25%">&nbsp;<asp:Label ID="lblWebPaymentMethod" runat="server" CssClass="Label_Small">Payment Method</asp:Label>
                                                                            </td>
                                                                            <td align="left" width="25%">
                                                                                <asp:TextBox ReadOnly="true" ID="txtWebPaymentMethod" runat="server" CssClass="TextBox_Normal Warn" Width="89px" ></asp:TextBox>
                                                                            </td>

                                                                            <td align="left" width="25%">&nbsp;<asp:Label ID="lblWebONDRequested" runat="server" CssClass="Label_Small">OND Requested</asp:Label>
                                                                            </td>
                                                                            <td align="left" width="25%">
                                                                                 <asp:TextBox ID="txtWebONDRequested" runat="server" ReadOnly="true"  CssClass="TextBox_Normal Warn" Width="89px" />
                                                                            </td>
                                                                        </tr>

                                                                        <tr id="trEFTBankInfo" runat="server">
                                                                            <td align="left">&nbsp;<asp:Label ID="lblWebBankName" runat="server" CssClass="Label_Small">Bank Name</asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ReadOnly="true" ID="txtWebBankName" runat="server" CssClass="TextBox_Normal Warn" Width="140px" />
                                                                            </td>

                                                                            <td align="left" >&nbsp;<asp:Label ID="lblWebBankABA" runat="server" CssClass="Label_Small">Bank ABA #</asp:Label>
                                                                            </td>
                                                                            <td align="left" >
                                                                                <asp:TextBox ReadOnly="true" ID="txtWebBankABA" runat="server" CssClass="TextBox_Normal Warn" Width="89px" />
                                                                            </td>
                                                                        </tr>

                                                                        <tr id="trEFTAccountInfo" runat="server">
                                                                            <td align="left">&nbsp;<asp:Label ID="lblWebAccount" runat="server" CssClass="Label_Small">Account #</asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ReadOnly="true" ID="txtWebAccount" runat="server" CssClass="TextBox_Normal Warn" Width="140px" />
                                                                            </td>

                                                                            <td align="left">&nbsp;<asp:Label ID="lblWebAccountType" runat="server" CssClass="Label_Small">Account Type</asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ReadOnly="true" ID="txtWebAccountType" runat="server" CssClass="TextBox_Normal Warn" Width="89px" />
                                                                            </td>
                                                                        </tr>

                                                                    </table>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table id="Table7" width="100%" class="Table_WithBorder" border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                           <%--Start: PK | 01/10/2019 | YRS-AT-4241 |This line is commented as change of text of Marital Status to Old Marital Status and also change of label and textbox id is done--%>
                                                                           <%-- <td align="left" width="25%">&nbsp;<asp:Label ID="lblWebMaritalStatus" runat="server" CssClass="Label_Small"> Marital Status</asp:Label> 
                                                                            </td>
                                                                            <td align="left" width="25%">
                                                                                <asp:TextBox ReadOnly="true" ID="txtWebMaritalStatus" runat="server" CssClass="TextBox_Normal Warn" Width="89px" />
                                                                            </td>--%>
                                                                           
                                                                            <td align="left" width="25%">&nbsp;<asp:Label ID="lblOldWebMaritalStatus" runat="server" CssClass="Label_Small">Old Marital Status</asp:Label> 
                                                                            </td>
                                                                            <td align="left" width="25%">
                                                                                <asp:TextBox ReadOnly="true" ID="txtOldWebMaritalStatus" runat="server" CssClass="TextBox_Normal Warn" Width="89px" />
                                                                            </td>
                                                                            <%--End: PK | 01/10/2019 | YRS-AT-4241 | This line is commented as change of text of Marital Status to Old Marital Status and also change of label and textbox id is done--%>
                                                                           <td align="left" width="25%">                                                                            
                                                                                <asp:Label ID="lblWebDocreceivedDate"  runat="server" CssClass="Label_Small"></asp:Label>
                                                                                <asp:Label ID="lblWebDocCode" Visible="false"   runat="server" >&nbsp;DocCode</asp:Label>
                                                                            </td>
                                                                            <td align="left" width="25%">
                                                                                <asp:TextBox ID="txtWebDocreceivedDate" onchange="DocreceivedDateChanged()" ReadOnly="true"  runat="server" Width="75px" CssClass="TextBox_Normal"
                                                                                    name="txtWebDocreceivedDate" /><rjs:PopCalendar ID="popCalendarDate" Enabled="false"
                                                                                        runat="server" Format="mm dd yyyy" Control="txtWebDocreceivedDate" Separator="/">
                                                                                    </rjs:PopCalendar>
                                                                                <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="IsValidDocreceivedDate"
                                                                                        ControlToValidate="txtWebDocreceivedDate" Display="Dynamic">*</asp:CustomValidator>
                                                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ClientValidationFunction="IsDocReceivedDateGreaterThanToday"
                                                                                        ControlToValidate="txtWebDocreceivedDate" Text Display="Static">*Date should not be future</asp:CustomValidator>
                                                                            </td>
                                                                        </tr>  
                                                                       <%--START: PK | 01/10/2019 | YRS-AT-4241 | Displaying new marital status>--%>
                                                                        <tr>
                                                                            <td align="left" width="25%">&nbsp;<asp:Label ID="lblNewWebMaritalStatus" runat="server" CssClass="Label_Small">New Marital Status</asp:Label>
                                                                            </td>
                                                                            <td align="left" width="25%">
                                                                                <asp:TextBox ReadOnly="true" ID="txtNewWebMaritalStatus" runat="server" CssClass="TextBox_Normal Warn" Width="89px" />
                                                                            </td>
                                                                        </tr>
                                                                        <%--END: PK | 01/10/2019 | YRS-AT-4241 | Displaying new marital status>--%>
                                                                        </tr>
                                                                        <tr>
                                                                           
                                                                        </tr>
                                                                      
                                                                    </table>

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4"></td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                
                                                                <td colspan="4" align="center">
                                                                    <table cellspacing="0%" cellpadding="15%">
                                                                        <tr>
                                                                            <td>
                                                                                <button class="Button_Normal Warn_Dirty" disabled="disabled" ID="btnApproveWebLoan" runat="server" onclick="showApproveConfirmation()">Approve</button>

                                                                            </td>
                                                                            <td>
                                                                                <button class="Button_Normal Warn_Dirty" disabled="disabled"  ID="btnDeclineWebLoan" runat="server" onclick="showDeclinePopUp();" style="width:80px;"  Text="Decline">Decline</button> 
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                           
                                                            </tr>
                                                        </table>
                                                    </div>
                                             
                                            <%--END: VC | 2018.08.02 | YRS-AT-4018 | Aspx code to creae web loan details section --%>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trLoanAccountDetails" runat="server" ><%--VC | 2018.08.02 | YRS-AT-4018 | Added Id and Runat server to the tr --%>
                                                <td colspan="4" class="td_Text" align="left">Loan Account Details
                                                </td>
                                            </tr>
                                            <tr id="trLoanAccountDetailsBody" runat="server" ><%--VC | 2018.08.02 | YRS-AT-4018 | Added Id and Runat server to the tr --%>
                                                <td colspan="4" align="left">
                                                    <div style="overflow: auto; width: 580px; height: 90px; text-align: left">
                                                        <asp:DataGrid ID="DataGridLoanAccountBreakdown" Width="440" runat="server" CssClass="DataGrid_Grid"
                                                            AutoGenerateColumns="false">
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                            <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                            <Columns>
                                                                <asp:BoundColumn DataField="Account Name" HeaderText="Account Name" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundColumn DataField="Account Balances" HeaderText="Account Balances as of loan date"
                                                                    DataFormatString="{0:##,##0.00}" Visible="False" ItemStyle-HorizontalAlign="Right"
                                                                    HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundColumn DataField="Loan Amounts" HeaderText="Loan Amounts" DataFormatString="{0:##,##0.00}"
                                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:BoundColumn DataField="Pct of Total" HeaderText="Pct of Total" DataFormatString="{0:##,##0.00}"
                                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" />
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                            </tr>

                                     

                                            <tr style="cursor: pointer">
                                                <td colspan="4" align="left" class="td_Text">
                                                    <div id="LoanDetailsExpand" style="display: none">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left" colspan="2">YRS Loan Details <%--VC | 2018.08.02 | YRS-AT-4018 | Changed text as YRS Loan Details from Loan Details --%>
                                                                    <asp:Label runat="server" Text=" (Please click here to view details.)" Font-Size="Small" Font-Bold="false"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <img src="~/images/expand.GIF" id="img1" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id="LoanDetailsCollapse">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left" colspan="2">YRS Loan Details <%--VC | 2018.08.02 | YRS-AT-4018 | Changed text as YRS Loan Details from Loan Details --%>
                                                                    <asp:Label ID="Label3" runat="server" Text=" (Please click here to hide details.)" Font-Size="Small" Font-Bold="false"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <img src="~/images/collapse.GIF" id="img4" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <div id="LoanDetailsDiv">
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td align="left" width="25%">&nbsp;<asp:Label ID="LabelLoanNumber" runat="server" CssClass="Label_Small">Loan #</asp:Label> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Changed the width from "35%" to "25%" to align textbox--%>
                                                                </td>
                                                                <td align="left" width="25%"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Changed the width from "13%" to "25%" to align textbox--%>
                                                                    <asp:TextBox ID="TextboxLoanNumber" runat="server" CssClass="TextBox_Normal" Width="89px"
                                                                        ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td align="left" width="25%">&nbsp;<asp:Label ID="LabelYmcaNo" runat="server" CssClass="Label_Small" Visible="True">YMCA</asp:Label> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added width 25% to align textbox --%>
                                                                </td>
                                                                <td align="left" width="25%"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added width 25% to align textbox --%>
                                                                    <asp:TextBox ID="TextboxYmcaNo" runat="server" CssClass="TextBox_Normal" Width="89px"
                                                                        ReadOnly="True" Visible="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <%-- START : SC | 04/21/2020 | YRS-AT-4853 | Added new column Loan Type for COVID Care act --%>
                                                            <tr>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="LabelLoanAmount" runat="server" CssClass="Label_Small">Loan Amt.</asp:Label>--%> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                <%--</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxLoanAmount" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                        Width="89px" ReadOnly="True" style="text-align:left;"></asp:TextBox>--%> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added style to align content to left in textbox --%>
                                                                <%--</td>--%>
                                                                <td align="left" width="25%">&nbsp;<asp:Label ID="lblLoanType" runat="server" CssClass="Label_Small">Loan Type</asp:Label>
                                                                </td>
                                                                <td align="left" width="25%">
                                                                <asp:TextBox ReadOnly="true" ID="txtLoanType" runat="server" CssClass="TextBox_Normal Warn" Width="89px" />
                                                                </td>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelLoanStatus" runat="server" CssClass="Label_Small">Loan Status</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxLoanStatus" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="140px" ReadOnly="True"></asp:TextBox><%-- VC | 2018.10.03 | YRS-AT-4018 | Changed textbox width--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="LabelLoanApprovalDate" runat="server" CssClass="Label_Small">Loan Approval Date</asp:Label> --%><%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                <%--</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxLoanApprovedDate" runat="server" ReadOnly="True" CssClass="TextBox_Normal Warn"
                                                                        Width="140px"></asp:TextBox>
                                                                </td>--%>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelLoanAmount" runat="server" CssClass="Label_Small">Loan Amt.</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxLoanAmount" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                        Width="89px" ReadOnly="True" style="text-align:left;"></asp:TextBox>
                                                                </td>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelSpousalConsentReceivedDate" runat="server" CssClass="Label_Small">Spousal Consent Rec'd Date</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxSpousalConsentReceivedDate" runat="server" ReadOnly="True"
                                                                        CssClass="TextBox_Normal Warn" Width="125px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="LabelFirstPaymentDate" runat="server" CssClass="Label_Small">First Payment Date</asp:Label> --%><%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                <%--</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxFirstPaymentDate" runat="server" ReadOnly="True" CssClass="TextBox_Normal Warn"
                                                                        Width="89px"></asp:TextBox>
                                                                </td>--%>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelLoanApprovalDate" runat="server" CssClass="Label_Small">Loan Approval Date</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxLoanApprovedDate" runat="server" ReadOnly="True" CssClass="TextBox_Normal Warn"
                                                                        Width="140px"></asp:TextBox>
                                                                </td>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelInterestRate" runat="server" CssClass="Label_Small">Interest Rate</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxInterestRate" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                        Width="89px" ReadOnly="True" style="text-align:left;"></asp:TextBox> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added style to align content to left in textbox --%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="LabelPaymentAmount" runat="server" CssClass="Label_Small">Payment Amt. (Per Pay Period)</asp:Label> --%><%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                <%--</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxPaymentAmount" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                        Width="88px" ReadOnly="True" style="text-align:left;"></asp:TextBox>--%> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added style to align content to left in textbox --%>
                                                                <%--</td>--%>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelFirstPaymentDate" runat="server" CssClass="Label_Small">First Payment Date</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxFirstPaymentDate" runat="server" ReadOnly="True" CssClass="TextBox_Normal Warn"
                                                                        Width="89px"></asp:TextBox>
                                                                </td>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelFinalPaymentDate" runat="server" CssClass="Label_Small">Final Payment Date</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxFinalPaymentDate" runat="server" ReadOnly="True" CssClass="TextBox_Normal Warn"
                                                                        Width="89px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="LabelPayrollFrequency" runat="server" CssClass="Label_Small">Payroll Frequency</asp:Label>--%> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                <%--</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxPayrollFrequency" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="88px" ReadOnly="True"></asp:TextBox>
                                                                </td>--%>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelPaymentAmount" runat="server" CssClass="Label_Small">Payment Amt. (Per Pay Period)</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxPaymentAmount" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                        Width="88px" ReadOnly="True" style="text-align:left;"></asp:TextBox> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added style to align content to left in textbox --%>
                                                                </td>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelLoanTerm" runat="server" CssClass="Label_Small">Loan Term</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxLoanTerm" runat="server" CssClass="TextBox_Normal Warn" Width="88px"
                                                                        ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelPayrollFrequency" runat="server" CssClass="Label_Small">Payroll Frequency</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxPayrollFrequency" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="88px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="LabelLoanOriginationFee" runat="server" CssClass="Label_Small">Loan Origination Fee</asp:Label>--%> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                <%--</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxLoanOriginationFee" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                        Width="89px" ReadOnly="True" style="text-align:left;"></asp:TextBox>--%> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added style to align content to left in textbox --%>
                                                                <%--</td>--%>
                                                            <%-- START: MMR | 2018.04.06 | YRS-AT-3935 | Display loan payment details --%>
                                                                <%--<td colspan="2"></td>--%>
                                                                <%-- START: VC | 2018.03.10 | YRS-AT-4018 | Commented code --%>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="lblPaymentMethod" runat="server" CssClass="Label_Small">Payment Method</asp:Label> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR
                                                                </td>
                                                               <%-- <td align="left">
                                                                    <asp:TextBox ID="txtPaymentMethod" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                    <img src="images/history.gif" id="imgPaymentDetailsHistory" runat="server" alt="History" style="cursor: pointer;height:11px;" onclick="javascript:ShowPaymentHistoryDialog();"/>
                                                                </td>--%>
                                                                <%-- END: VC | 2018.03.10 | YRS-AT-4018 | Commented code --%>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">&nbsp;<asp:Label ID="LabelLoanOriginationFee" runat="server" CssClass="Label_Small">Loan Origination Fee</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextboxLoanOriginationFee" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                        Width="89px" ReadOnly="True" style="text-align:left;"></asp:TextBox></td>                                                                
                                                                <%--<td align="left">&nbsp;<asp:Label ID="Label16" runat="server" CssClass="Label_Small">Payment Method</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                    <img src="images/history.gif" id="img7" runat="server" alt="History" style="cursor: pointer;height:11px;" onclick="javascript:ShowPaymentHistoryDialog();"/>
                                                                </td>--%>
                                                                <td align="left">&nbsp;<asp:Label ID="lblONDRequested" runat="server" CssClass="Label_Small">OND Requested</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtBoxONDRequested" runat="server" CssClass="TextBox_Normal Warn" Width="89px"
                                                                         ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">&nbsp;<asp:Label ID="lblPaymentMethod" runat="server" CssClass="Label_Small">Payment Method</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtPaymentMethod" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                    <img src="images/history.gif" id="imgPaymentDetailsHistory" runat="server" alt="History" style="cursor: pointer;height:11px;" onclick="javascript:ShowPaymentHistoryDialog();"/>
                                                                </td>
                                                            </tr>
                                                            <%-- END : SC | 04/21/2020 | YRS-AT-4853 | Added new column Loan Type for COVID Care act --%>
                                                            <%-- START: VC | 2018.03.10 | YRS-AT-4018 | Added Bank Name and Bank ABA--%>
                                                            <tr>
                                                                <td align="left">&nbsp;<asp:Label ID="lblBankName" runat="server" CssClass="Label_Small">Bank Name</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtBankName" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="140px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td align="left">&nbsp;<asp:Label ID="lblBankABA" runat="server" CssClass="Label_Small">Bank ABA #</asp:Label> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtBankABA" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <%-- END: VC | 2018.03.10 | YRS-AT-4018 | Added Bank Name and Bank ABA--%>
                                                            <tr>
                                                                <td align="left">&nbsp;<asp:Label ID="lblAccountNumber" runat="server" CssClass="Label_Small">Account #</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="140px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <%-- START: VC | 2018.03.10 | YRS-AT-4018 | Added account type --%>
                                                                <td align="left">&nbsp;<asp:Label ID="lblAccountType" runat="server" CssClass="Label_Small">Account Type</asp:Label> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtAccountType" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <%-- END: VC | 2018.03.10 | YRS-AT-4018 | Added account type --%>
                                                            </tr>
                                                            <tr>
                                                                <%-- START: VC | 2018.03.10 | YRS-AT-4018 | Commented existing code --%>
                                                                <%--<td align="left">&nbsp;<asp:Label ID="lblBankABA" runat="server" CssClass="Label_Small">Bank ABA #</asp:Label> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                <%--</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtBankABA" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                </td>--%>
                                                                <%-- END: VC | 2018.03.10 | YRS-AT-4018 | Commented existing code --%>
                                                                <td align="left">&nbsp;<asp:Label ID="lblPaymentStatus" runat="server" CssClass="Label_Small">Payment Status</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtPaymentStatus" runat="server" CssClass="TextBox_Normal Warn"
                                                                        Width="89px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <%-- END: MMR | 2018.04.06 | YRS-AT-3935 | Display loan payment details --%>
                                                            <tr>
                                                                <td align="left" >&nbsp;<asp:Label ID="lblOffsetReason" runat="server" Visible="false" CssClass="Label_Small">Offset Reason</asp:Label> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                </td>
                                                                <td align="left" colspan="3">
                                                                    <asp:Label ID="lblOffsetReasonText" runat="server" CssClass="Normaltext" Visible="false"></asp:Label>
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">&nbsp; <%-- MMR | 2018.08.31 | YRS-AT-4018 | changed colspan value from "2%" to "4%"--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table width="100%" class="Table_WithBorder" id="tblPayOff" runat="server">
                                                                        <tr>
                                                                            <td align="left" width="25%"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Changed the width from "35%" to "25%" to align textbox--%>
                                                                                <asp:Label ID="LabelPayoffAmount" runat="server" CssClass="Label_Small">Computed Payoff Amt.</asp:Label>
                                                                            </td>
                                                                            <td align="left" width="25%"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added width 25% to align textbox --%>
                                                                                <asp:TextBox ID="TextboxPayoffAmount" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                                    Width="89px" ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" width="25%"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Changed the width from "32%" to "25%" to align textbox--%>
                                                                                <asp:Label ID="LabelSavedPayoffAmount" runat="server" CssClass="Label_Small">Saved Payoff Amt.</asp:Label>
                                                                            </td>
                                                                            <td align="left" width="25%"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added width 25% to align textbox --%>
                                                                                <asp:TextBox ID="TextboxSavedPayoffAmount" runat="server" CssClass="TextBox_Normal_Amount Warn"
                                                                                    Width="89px" ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "35%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                                <asp:Label ID="LabelComputeOn" runat="server" CssClass="Label_Small">Saved On</asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="TextboxComputeOn" runat="server" CssClass="TextBox_Normal Warn"
                                                                                    Width="89px" ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Button ID="ButtonSavePayoffAmount" runat="server" CssClass="Button_Normal" Width="60"
                                                                                    Text="Save" CausesValidation="False" OnClientClick="javascript: return ShowPayOffConfirmationDialog();"></asp:Button>  <%--SB : YRS-AT-3460 | 05.31.2017 | Added onclientclick event to display confirmation message dialog box--%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table width="100%" class="Table_WithBorder" id="tblFreeze" runat="server">
                                                                        <%-- START: VC | 2018.03.10 | YRS-AT-4018 | A dded Defaulted Loan Date --%>
                                                                        <tr>
                                                                            <td align="left" width="25%"><asp:Label ID="lblDefaultedLoanDate" runat="server" CssClass="Label_Small">Loan Default Date</asp:Label>
                                                                            </td>
                                                                            <td align="left" width="25%">
                                                                                <asp:TextBox ID="txtDefaultedLoanDate" runat="server" CssClass="TextBox_Normal Warn" Width="89px"
                                                                                     ReadOnly="true" ></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                         <%-- END: VC | 2018.03.10 | YRS-AT-4018 | A dded Defaulted Loan Date --%>
                                                                        <tr>
                                                                            <td align="left" width="25%"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Changed the width from "35%" to "25%" to align textbox--%>
                                                                                <label id="Label6" class="Label_Small">Default Amt. (Unpaid Principal + Interest)</label>
                                                                            </td>
                                                                            <td align="left" width="25%"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added width 25% to align textbox --%>
                                                                                <asp:TextBox ID="txtDefaultAmt" runat="server" CssClass="TextBox_Normal_Amount"
                                                                                    Width="89px" ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left" width="25%"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Changed the width from "32%" to "25%" to align textbox--%>
                                                                                <label id="Label10" class="Label_Small">Frozen On</label>
                                                                            </td>
                                                                            <td align="left" width="25%"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added width 25% to align textbox --%>
                                                                                <asp:TextBox ID="txtFrozenOn" runat="server" CssClass="TextBox_Normal"
                                                                                    ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "32%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                                <label id="Label11" class="Label_Small">Phantom Interest</label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtPhantomInt" runat="server" CssClass="TextBox_Normal_Amount"
                                                                                    Width="89px" ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                            <td align="left"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "32%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                                <label id="Label9" class="Label_Small">Computed Frozen Amt.</label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtCompFrozenAmt" runat="server" CssClass="TextBox_Normal_Amount"
                                                                                    Width="89px" ReadOnly="True"></asp:TextBox>
                                                                            </td>

                                                                        </tr>

                                                                        <tr>
                                                                            <td align="left"> <%-- MMR | 2018.08.31 | YRS-AT-4018 | Removed width "32%" as alignment will be done automatically based on TD width's in first TR--%>
                                                                                <label id="Label12" class="Label_Small">Tot. Default Amt.(Default Amt.+ Phantom Int.)</label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtComputedAmt" runat="server" CssClass="TextBox_Normal_Amount"
                                                                                    Width="89px" ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Button ID="btnFreeze" runat="server" CssClass="Button_Normal" Width="60"
                                                                                    Text="Freeze" CausesValidation="False" Visible="false"></asp:Button>
                                                                                <asp:Button ID="btnUnfreeze" runat="server" CssClass="Button_Normal" Width="80"
                                                                                    Text="Unfreeze" CausesValidation="False" Visible="false"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td colspan="4">&nbsp; <%-- MMR | 2018.08.31 | YRS-AT-4018 | Added colspan value "4%"--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr id="trPaymentHistoryHeader" runat="server" style="cursor: pointer;"><%--VC | 2018.08.02 | YRS-AT-4018 | Added Id and Runat server to the tr --%>
                                                <td colspan="4" class="td_Text" align="left">
                                                    <div id="LoanExpand" style="display: none">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <asp:Label ID="LabelPaymentHistory" runat="server">Payment History</asp:Label>
                                                                    <asp:Label ID="Label4" runat="server" Text=" (Please click here to view details.)"
                                                                        Font-Size="Small" Font-Bold="false"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <img src="~/images/expand.GIF" id="img2" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id="LoanHide">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <asp:Label ID="Label1" runat="server">Payment History</asp:Label>
                                                                    <asp:Label ID="Label5" runat="server" Text=" (Please click here to hide details.)"
                                                                        Font-Size="Small" Font-Bold="false"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <img src="~/images/collapse.GIF" id="img3" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                                <%--<td colspan="2" class="td_Text" align="right">
                                            </td>--%>
                                            </tr>
                                            <tr id="trPaymentHistoryBody" runat="server"><%--VC | 2018.08.02 | YRS-AT-4018 | Added Id and Runat server to the tr --%>
                                                <td align="center" colspan="4">
                                                    <div id="LoanCollapes">
                                                        <div style="overflow: auto; width: 100%; height: 200px; text-align: left">
                                                            <asp:Label ID="lblMessage" runat="server" Text="No records found." Visible="false"></asp:Label>
                                                            <asp:DataGrid ID="DataGridPaymentHistory" runat="server" Width="97%" CssClass="DataGrid_Grid"
                                                                AllowSorting="false" AutoGenerateColumns="false">
                                                                <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="AmortizationId" HeaderText="AmortizationId" Visible="False"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Payment Due Date" HeaderText="Payment Due Date" Visible="True"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Principal" HeaderText="Principal" Visible="True" ItemStyle-HorizontalAlign="Right"
                                                                        DataFormatString="{0:#0.00}"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Interest" HeaderText="Interest" Visible="True" ItemStyle-HorizontalAlign="Right"
                                                                        DataFormatString="{0:#0.00}"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="PaymentAmount" HeaderText="PaymentAmount" Visible="False"
                                                                        DataFormatString="{0:#0.00}"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Date Received" HeaderText="Date Received" Visible="True"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Transmittal Number" HeaderText="Transmittal Number" Visible="True"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Date Funded" HeaderText="Date Funded" Visible="True"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trLoanReport" runat="server"><%--VC | 2018.08.02 | YRS-AT-4018 | Added Id and Runat server to the tr --%>
                                                <td colspan="4" align="center">
                                                    <asp:Button ID="ButtonLoanReport" runat="server" Text="Loan Report" CssClass="Button_Normal"
                                                        Enabled="false" Visible="true" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </iewc:PageView>
                            </iewc:MultiPage>
                        </td>
                    </tr>
                </tbody>
            </table>
            <!--Table1-->
            <table>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
            <table class="Table_WithBorder" cellspacing="0" width="980" border="0">
                <tr>
                    <td class="Td_ButtonContainer" align="center" width="25%">
                        <asp:Button class="Button_Normal" ID="ButtonSaveParticipant" runat="server" Width="80"
                            Enabled="false" Text="Save" CausesValidation="True" OnClientClick="return checkTelephoneLength();"></asp:Button><asp:Button class="Button_Normal" ID="ButtonSaveParticipants" runat="server"
                                Width="80" Text="Save" CausesValidation="True" OnClientClick="return checkTelephoneLength();"
                                Visible="False" Enabled="False"></asp:Button>
                    </td>
                    <td class="Td_ButtonContainer" align="center" width="25%">
                        <asp:Button class="Button_Normal" ID="ButtonCancel" runat="server" Width="80" Enabled="False"
                            Text="Cancel" CausesValidation="False"></asp:Button>
                    </td>
                    <td class="Td_ButtonContainer" align="center" width="25%">
                        <asp:Button class="Button_Normal" ID="ButtonPHR" runat="server" Width="80" Text="PHR"
                            CausesValidation="False"></asp:Button>
                    </td>
                    <td class="Td_ButtonContainer" align="center" width="25%">
                        <asp:Button class="Button_Normal Warn_Dirty" ID="ButtonOK" runat="server" Width="80"
                            Text="Close" CausesValidation="False"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <asp:TextBox ID="TextboxIFrame" runat="server" Width="50px" Visible="false" CssClass="TextBox_Normal Warn"></asp:TextBox>
        <!--<INPUT type="hidden" id="NotesFlag" name="NotesFlag" runat="server">-->
        <input id="EmailId" type="hidden" name="EmailId" runat="server" cssclass="Warn" />
        <input id="Unsubscribe" type="hidden" name="Unsubscribe" runat="server" cssclass="Warn" />
        <input id="TextOnly" type="hidden" name="TextOnly" runat="server" cssclass="Warn" />
        <input id="BadEmail" type="hidden" name="BadEmail" runat="server" cssclass="Warn" />
        <!--anita date may 9th-->
        <input id="HiddenText" type="hidden" name="HiddenText" runat="server" />
        <input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server" />
        <!--anita date may 9th-->
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        <asp:HiddenField ID="HiddenFieldDirty" Value="false" runat="server" />
        <asp:HiddenField ID="HiddenFieldDeathDate" Value="" runat="server" />
        <div id="dialog-confirm" title="Delete Web Account Info" style="display: none;">
            <p id="p_dialog_confirm" style="height: 55px">
            </p>
        </div>
        <div id="divPopup" style="display: none">
            <table class="Table_WithBorder" cellspacing="0" border="0">
                <tr>
                    <td align="left" id="lblReactive">
                        <label class="Label_Small">
                            Do you want to continue with employee reactivation?</label>
                    </td>
                    <td align="left">
                        <input type="radio" id="IsYes" value="IsYes" onclick="javascript: IsYes_click();"
                            name="IsYes" />
                        <label class="Label_Small" for="IsYes">
                            Yes</label>
                        <input id="IsNo" type="radio" name="IsNo" value="IsNo" onclick="javascript: IsNo_click();" />
                        <label class="Label_Small" for="IsNo">
                            No</label>
                    </td>
                </tr>
                <tr id="TDcontract" style="display: none">
                    <td align="left">
                        <label class="Label_Small">
                            Do you want to re-activate TD contract?</label>
                    </td>
                     <td align="left">
                        <input id="TDYes" type="radio" name="TDYes" value="TDYes" onclick="javascript: TDYes_click();" />
                        <label class="Label_Small" for="TDYes">
                            Yes</label>
                        <input id="TDNo" type="radio" name="TDNo" value="TDNo" onclick="javascript: TDNo_click();" />
                        <label class
                    </td>
                </tr>
            </table>
        </div>
        <div id="divRecdExist" style="display: none">
            <label class="Label_Small" id="lbl_Recd_Ymca_Exist">
            </label>
        </div>
        <div id="ConfirmDialog" runat="server" style="display: none;">
            <asp:UpdatePanel ID="UpdDialog" UpdateMode="Conditional" ChildrenAsTriggers="true"
                runat="server">
                <ContentTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td>
                                <div id="lblMessage1" style="color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; width: 470; height: 140"
                                    runat="server">
                                    <br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom"></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <div style="border: 1px solid #aaaaaa/*{borderColorContent}*/; background: #ffffff/*{bgColorContent}*/ url(images/ui-bg_flat_75_ffffff_40x100.png)/*{bgImgUrlContent}*/ 50%/*{bgContentXPos}*/ 50%/*{bgContentYPos}*/ repeat-x/*{bgContentRepeat}*/; color: #222222/*{fcContent}*/;">
                                </div>
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button runat="server" ID="btn_Ok_Message" Text="OK" CssClass="Button_Normal"
                                                    Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 10pt; font-weight: bold; height: 16pt;"
                                                    OnClientClick="CloseMessageDeathNotify()" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:HiddenField ID="hd_emp_reactive" runat="server" />
        <asp:HiddenField ID="hd_tdcontractexist" runat="server" />

        <div id="Tooltip" runat="server" style="z-index: 1000; width: auto; border-left: 1px solid silver; border-top: 1px solid silver; border-right: 2px solid black; border-bottom: 1px solid #cccccc; padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black; display: none; /* does not work in ie6 */	font-size: 7pt; font-family: verdana; margin: 0; overflow: visible; text-align: left;">
            <asp:Label runat="server" ID="lblComments" Style="display: block; width: auto; overflow: visible; font-size: x-small;"></asp:Label>
        </div>
        <div id="divProgress" style="overflow: visible;display:none;"><%--PK | 01.02.2019 | BT-12024 | Applied display:none to all dialog box to avoid appearance of it while loading the page.--%>
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
        <div id="divWSMessage" runat="server" style="display: none;">
            <table width="690px">
                <tr>
                    <td valign="top" align="left">
                        <span id="spntext"></span>
                    </td>
                </tr>
            </table>
        </div>
        <table width="980">
            <tr>
                <td width="100%">
                    <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server"></YRSControls:YMCA_Footer_WebUserControl>
                </td>
            </tr>

        </table>
        <%--START: SB | 2017.05.31 | YRS-AT-3460 | Add confirmation dialog box while saving Saved Payoff Amt. --%>
        <div id="divPayOffConfirmation" runat="server" style="display:none;overflow:auto; ruby-align:left" >
            <div>
            <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td> 
                        <div style="color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; height: 70px; text-align:left;"
                            ID="divPayOffConfirmationMessage" runat="server">
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
                        <input type="button" name="btnPayOffConfirmationYes" value="  Yes  " class="Button_Normal" Onclick="SavePayOffAmount()" />&nbsp;
                        <input type="button" name="btnPayOffConfirmationNo" value="  No   " class="Button_Normal" onclick="ClosePayOffConfirmationDialog()" />
                    </td>
                </tr>
            </table>
            </div>
         </div>
         <%--END: SB | 2017.05.31 | YRS-AT-3460 | Add confirmation dialog box while saving Saved Payoff Amt. --%>
        <%-- START: MMR | 2018.04.06 | YRS-AT-3935 | Display EFT payment history details--%>
        <div id="divPaymentMethodHistory" runat="server" style="display:none;overflow:auto;"><%--PK | 01.02.2019 | BT-12024 |  Applied display:none to all dialog box to avoid appearance of it while loading the page.. --%>
            <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td>
                        <div style="overflow:auto;height: 150px">
                            <asp:GridView ID="gvPaymentMethodHistory" AllowPaging="true" AllowSorting="false" runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" EmptyDataText="No records found.">
                            <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                            <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                            <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                            <RowStyle CssClass="DataGrid_NormalStyle" />
                            <Columns>
                                <asp:BoundField DataField="PaymentMethod" ReadOnly="True" HeaderText="Payment Method" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="17%"/>
                                <asp:BoundField DataField="AccountType" ReadOnly="True" HeaderText="Account Type" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="16%"/>
                                <asp:BoundField DataField="AccountNumber" ReadOnly="True" HeaderText="Account #" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="17%"/>
                                <asp:BoundField DataField="BankABANumber" ReadOnly="True" HeaderText="BankABA #" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="17%"/>
                                <asp:BoundField DataField="PaymentStatus" ReadOnly="True" HeaderText="Payment Status" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="18%"/>
                                <asp:BoundField DataField="StatusDate" ReadOnly="True" HeaderText="Status Date" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%" DataFormatString="{0:MM/dd/yyyy}"/>
                                <asp:BoundField DataField="EFTFileBatchID" ReadOnly="True" HeaderText="Attempted Date" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%" DataFormatString="{0:MM/dd/yyyy}"/><%--VC | 2018.10.11 | YRS-AT-4018 | Added Attempted date column--%>
                            </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><hr/></td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button class="Button_Normal" ID="btnClose" runat="server" Width="80" Text="OK" OnClientClick="javascript:return ClosePaymentHistoryDialog();"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <%-- END: MMR | 2018.04.06 | YRS-AT-3935 | Display EFT payment history details --%>
        
        <%--START: ML | 2019.01.14 |YRS-3157 | Popup design to Display FreezeUnfreezHistory details --%>
        <div id="divLoanFreezeUnfreezeHistory" runat="server" style="display: block; overflow: auto;">
            <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="95%">
                <tr>
                    <td>
                        <div style="overflow: auto; height: 150px">                       
                            <table id="tblLoanFreezeUnfreezeHistory" style="width:95%; BORDER-COLLAPSE: collapse" cellSpacing=0  rules=all border=1   class ="DataGrid_Grid">
                                <thead class="DataGrid_HeaderStyle" >
                                    <th>Status</th>
                                    <th>Date</th>
                                    <th>Amount</th>
                                    <th>User</th>
                                    <th>Reason</th>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button class="Button_Normal" ID="Button2" runat="server" Width="80" Text="OK" OnClientClick="javascript:return ClossLoanFreezeUnfreezeHistoryDialog();"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>        
        <%--END: ML | 2019.01.14 |YRS-3157 |Popup design to Display FreezeUnfreezHistory details --%>


        <%--START : SR | 2018.07.12 | YRS-AT-3858 | Add common confirmation dialog box for Error message --%>
        <div id="divCommonConfirmDialog" style="overflow: visible;display:none;"><%--PK | 01.02.2019 | BT-12024 |  Applied display:none to all dialog box to avoid appearance of it while loading the page.--%>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div>
                    <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                        <tr>
                            <td>
                                <asp:Label ID="lblCommonMessage" CssClass="Label_Small" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="bottom">                            
                                    <asp:Button runat="server" ID="btnOK" Text="OK" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        OnClientClick="Javascript: CloseCommonDialog();" />
                                    
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%--START: VC | 2018.08.02 | YRS-AT-4018 | Code to create confirmation dialog--%>
        <div id="divConfirmDialog" style="overflow: visible;display:none;"><%--PK | 01.02.2019 | BT-12024 |  Applied display:none to all dialog box to avoid appearance of it while loading the page.--%>
            
                    <div>
                        <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                            <tr>
                                <td align="left">
                                    <span id="lblMessageConfirm" class="Label_Small"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <IMG title="Image" alt="image" height="50" src="images/spacer.gif" width="10">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="bottom">
                                    <asp:Button  ID="btnYesLoanApprove" runat="server"  Text="Yes" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        OnClientClick="Javascript: closeDialog('divConfirmDialog');return ShowProcessingDialog($('#hfProcessInProgress').val());" />&nbsp;

                                    <input type="button" ID="btnNo" value="No" Class="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        OnClick="Javascript: closeDialog('divConfirmDialog');" />
                                </td>
                            </tr>
                        </table>
                    </div>
                
        </div>
        <%--END: VC | 2018.08.02 | YRS-AT-4018 | Code to create confirmation dialog--%>

        <%--START: VC | 2018.08.02 | YRS-AT-4018 | Code to create decline dialog box--%>
        <div id="divDeclinePopUp" runat="server" style="display: none">
            <div id="div2" style="height: 35px; display: none">
            </div>
            <div id="div3">
                <table cellspacing="0" border="0">
                    <tr>
                        <td align="left" style="width: 150px;" valign="top">
                            <span class="Label_Small">Reason:p">
                            <span class="Label_Small">Reason:</span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlDeclineReason" onchange="ddlDeclineReasonChanged(true)" runat="server" Width="206" CssClass="DropDown_Normal">

                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <span ID="LabelReasonNote" Class="Label_Small">Notes: </span>
                        </td>
                        <td align="left">
                            <textarea id="txtDeclineNotes" onkeyup="ddlDeclineReasonChanged(false)"   CssClass="TextBox_Normal Warn" cols="50" rows="4" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="bottom" colspan="2">
                             <asp:Button  ID="Button1" runat="server" Visible="false" Enabled="true" Text="SAVE"  CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        OnClientClick="Javascript: closeDialog('divDeclinePopUp');Javascript: ShowProcessingDialog($('#hfProcessInProgress').val());" />
                            <button class="Button_Normal" ID="btnConfirmDeclineWebLoan" onclick="closeDialog('divDeclinePopUp');ShowProcessingDialog($('#hfProcessInProgress').val());" onserverclick="btnConfirmDeclineWebLoan_ServerClick" disabled="disabled"  runat="server" >Save</button>
                            <input type="button" ID="btnCloseDeclinePopUp" value="Cancel" Class="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                OnClick="Javascript: closeDialog('divDeclinePopUp');" />
                        </td>
                    </tr>

                </table>
            </div>
        </div>
        <%--END: VC | 2018.08.02 | YRS-AT-4018 | Code to create decline dialog box--%>

        <%--START: VC | 2018.09.05 | YRS-AT-4018 | Code to create application status history dialog--%>
        <div id="divApplicationStatusHistory" runat="server" style="display: none; overflow: auto;"><%--PK | 01.02.2019 | BT-12024 | Applied display:none to all dialog box to avoid appearance of it while loading the page.--%>
            <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="95%">
                <tr>
                    <td>
                        <div style="overflow: auto; height: 150px">
                            <asp:GridView ID="gvApplicationStatusHistory" AllowPaging="false" AllowSorting="false" runat="server" CssClass="DataGrid_Grid" Width="95%" AutoGenerateColumns="false" EmptyDataText="No records found.">
                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                <Columns>
                                    <asp:BoundField DataField="Date/Time" ReadOnly="True" HeaderText="Date/Time" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="30%" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" />
                                    <asp:BoundField DataField="Status" ReadOnly="True" HeaderText="Status" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="15%" />
                                    <asp:BoundField DataField="User" ReadOnly="True" HeaderText="User" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="15%" />                            
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button class="Button_Normal" ID="btnCloseStatusHistory" runat="server" Width="80" Text="OK" OnClientClick="javascript:return ClossApplicationStatusHistoryDialog();"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <%--START: VC | 2018.09.05 | YRS-AT-4018 | Code to create application status history dialog--%>
    </div>
        <%--END: SR | 2018.07.12 | YRS-AT-3858 | Add common confirmation dialog box for Error message --%>

    </form>
</body>

<!----Added by imran on 30-July-2010 : YRS 5.0-1141 add Age format to the person maintanace screen.-->
<script language="javascript" type="text/javascript">

    <%--START: SB | 2017.05.31 | YRS-AT-3460 | Closing the SavePayOffAmt confirmation dialog box on "Cancel" and Saving the data  on "OK" click --%>
    $(document).ready(function () {
        $('#divPayOffConfirmation').dialog({
            autoOpen: false,
            resizable: false,
            dialogClass: 'no-close',
            draggable: true,
            width: 470, 
            minheight: 140,
            closeOnEscape: false,
            title: "Confirmation",
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
            }
        });
        <%-- START: MMR | 2018.04.06 | YRS-AT-3935 | Show EFT payment history details in dialog box --%>
        $('#divPaymentMethodHistory').dialog({
            autoOpen: false,
            resizable: false,
            dialogClass: 'no-close',
            draggable: true,
            width: 550, minheight: 200,
            height: 250,
            closeOnEscape: false,
            title: "Payment Method History",
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
            }
        });
        <%-- END: MMR | 2018.04.06 | YRS-AT-3935 | Show EFT payment history details in dialog box --%>
    });
    function ClosePayOffConfirmationDialog() {
        $('#divPayOffConfirmation').dialog("close");
    }

    function SavePayOffAmount() {
        __doPostBack('<%=ButtonSavePayoffAmount.ClientID%>', '');
        ClosePayOffConfirmationDialog();
    }

    function ShowPayOffConfirmationDialog() {
        $('#divPayOffConfirmation').dialog("open");
        return false;
    }
    <%--END: SB | 2017.05.31 | YRS-AT-3460 | Closing the SavePayOffAmt confirmation dialog box on "Cancel" and Saving the data  on "OK" click  --%>

    <%-- START: MMR | 2018.04.06 | YRS-AT-3935 | Open & close EFT payment history dialog box --%>
    function ClosePaymentHistoryDialog() {
        $('#divPaymentMethodHistory').dialog("close");
        return false;
    }

    function ShowPaymentHistoryDialog() {
        $('#divPaymentMethodHistory').dialog("open");
        return false;
    }
    <%-- END: MMR | 2018.04.06 | YRS-AT-3935 | Open & close EFT payment history dialog box --%>

    //BS:2012.06.15:BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
    var strrecordexist = '';
    var strtdcontractexist = '';
    var str_emp_reactive = '';
    var str_diff_ymcaexist = '';
    function initializeReactivateControl(recordexist, tdcontractexist, emp_reactive, diff_ymcaexist) {
        strrecordexist = recordexist;
        strtdcontractexist = tdcontractexist;
        str_emp_reactive = emp_reactive;
        str_diff_ymcaexist = diff_ymcaexist;
    }

    function Unchecked_Radio() {
        $('#IsYes').removeAttr('checked');
        $('#IsNo').removeAttr('checked');
        $('#TDYes').removeAttr('checked');
        $('#TDNo').removeAttr('checked');
        $('#TDcontract').hide();

    }

    //    

    function IsYes_click() {
        $('#IsNo').removeAttr('checked');
        //BS:2012.06.21:BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
        if (strtdcontractexist == 'True')
        { $('#TDcontract').show(); }
    }

    function IsNo_click() {
        $('#IsYes').removeAttr('checked');
        $('#TDcontract').hide();
        return false;
    }
    function TDNo_click() {
        $('#TDYes').removeAttr('checked');
    }
    function TDYes_click() {
        $('#TDNo').removeAttr('checked');
    }
    function OkPrompt() {
        $('#divRecdExist').dialog
                    ({
                        modal: true,
                        autoOpen: true,
                        closeOnEscape: false,
                        title: "Employee Reactivation",
                        width: 400, height: 200,
                        resizable: false,
                        buttons: [{
                            text: "Ok", click: function () {
                                $('#divRecdExist').dialog('destroy');

                            }
                        }
                        ]
                    });
    }

    function Proceed_Click() {
        if (($('#IsNo').filter(':checked').length == 0) && ($('#IsYes').filter(':checked').length == 0)) { return; }
        if ($('#IsNo').filter(':checked').length == 1) {
            $("#divPopup").dialog('close');
            $("#divPopup").dialog('destroy');
            return;
        }
        if ($('#TDYes').filter(':checked').length == 1) {
            $('#hd_emp_reactive').val('True');
            $('#hd_tdcontractexist').val('True');
        }
        else if ($('#IsYes').filter(':checked').length == 1) {
            $('#hd_emp_reactive').val('True');
        }

        <%= Page.ClientScript.GetPostBackEventReference(ButtonReactivate, "") %>;

        $("#divPopup").dialog('close');
        $("#divPopup").dialog('destroy');
    }
    function ConfirmPrompt() {
        $('#divPopup').dialog
                    ({
                        modal: true,
                        autoOpen: true,
                        closeOnEscape: false,
                        title: "Employee Reactivation",
                        width: 550, height: 230,
                        resizable: false,
                        buttons: [{ text: "Proceed", click: Proceed_Click },
                                    {
                                        text: "Cancel", click: function () {
                                            $('#divPopup').dialog('destroy');

                                        }
                                    }
                        ],
                        close: function (event, ui) {
                        }
                    });

    }

    //var text = document.getElementById('TextBoxDOB_TextBoxUCDate');
    var text = document.getElementById('TextBoxDOB_TextBoxUCDate');
    //text.onchange = function () { getAge(); text.onchange(); };   //function () { alert('This is onclick') };
    text.attachEvent("onchange", function () { getAge() });
    // alert('Setting up onChange event for ' + text.id);

    //Added by prasad for YRS 5.0-1469: Add link to Web Front End
    $(document).ready(function () {

        //BS:2012.06.18:BT:991:YRS 5.0-1530 - Need ability to reactivate a terminated employee
        if ($.browser.msie && $.browser.version < 8)
            $('input[type=radio],[type=checkbox]').live('click', function () {
                $(this).trigger('change');
            });
        $('#ButtonReactivate').bind('click', function () {
            Unchecked_Radio();
            //BS:2012.06.26:BT:991:YRS 5.0-1530:-if different ymca exist on same termination date then display prompt:“Multiple employment records exist with the same termination date. Please contact IT for Data Correction.”
            if (str_diff_ymcaexist == 'True') {
                $('#lbl_Recd_Ymca_Exist').text('Multiple employment records exist with the same termination date. Please contact IT for Data Correction.');
                OkPrompt();
            }
            if (strrecordexist == 'True') {
                $('#lbl_Recd_Ymca_Exist').text('Record exist after termination date.');
                OkPrompt();
            }
            if (str_emp_reactive == 'True') {
                ConfirmPrompt();
            }

        });

        InitializeYRelationDialogBox();
        $("#ButtonWebFrontEnd").click(function () { CheckAccessButtonWebFrontEnd('ButtonWebFrontEnd'); jQuery('.ui-dialog button:nth-child(1)').button('enable'); return false; });

        //2012.08.17 SR : BT-957/YRS 5.0-1484 : Termination Watcher 
        //        $("#ButtonTerminationWatcher").hide();
        //        ControlTerminationWatcherButton(); 
        LoadTerminationWatcher();
        InitializeTerminationWatcherDialogBox();
        $("#ButtonTerminationWatcher").click(function () {
            //Anudeep:20.12.2012 Bt-1525-Security Access rights needs to be provided for Termination watcher.
            if (CheckAccess("ButtonTerminationWatcher") == false) {
                return false;
            }
            $("#divTerminationWatcher").dialog('open'); return false;
        });
        //         $("#divTerminationWatcher").dialog('open'); return false;});
        InitializePINDialogBox();
        InitializePINConfirmDialogBox();
        $("#ButtonPINno").click(function () {
            GetPIN();
            ClearPIN('clear');
            return false;
        });

        <%--START: ML | 2019.11.01 | YRS-3157 | Dialog box design for Loan freeze unfreeze history --%>
        $('#divLoanFreezeUnfreezeHistory').dialog({
            autoOpen: false,
            resizable: false,
            dialogClass: 'no-close',
            draggable: true,
            width: 550, minheight: 200,
            height: 250,
            closeOnEscape: false,
            title: "Loan Freeze/Unfreeze History",
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
            }
        });
        <%--END: ML | 2019.11.01 | YRS-3157 | Dialog box design for Loan freeze unfreeze history --%>
        
    });
   
    <%--START: ML | 2019.01.14 | YRS-3157 | Get FreezeUnfreeze History detail and display data in dialogbox--%>
    function GetFreezeUnfreezHistory(loandetailid){                 
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ParticipantsInformation.aspx/GetFreezeUnfreezHistory",
            data: "{ 'requestedLoanDetailId': '" + loandetailid + "'}",
            datatype: "json",                
            success: function (data) {                   
                $("#tblLoanFreezeUnfreezeHistory tbody").html("");
                <%-- START: MMR | 2019.01.30 | Added To show if loan is in WEB and not in YRS --%>
                if (data.d == null) {
                    $("#tblLoanFreezeUnfreezeHistory").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>No records found.</td></tr>");
                }
                <%-- END: MMR | 2019.01.30 | Added To show if loan is in WEB and not in YRS --%>
                else if (data.d.length <= 0) {
                    $("#tblLoanFreezeUnfreezeHistory").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>No records found.</td></tr>");
                }
                else {
                    for (var i = 0; i < data.d.length; i++) {
                        if (i % 2 === 0) {
                            $("#tblLoanFreezeUnfreezeHistory").append("<tr class='DataGrid_NormalStyle'><td style='width:15%;'>" + data.d[i].Status + "</td><td style='width:15%;'>" + data.d[i].DateTime + "</td><td style='width:20%;text-align: right;'> " + data.d[i].Amount + "</td><td style='width:20%;'>" + data.d[i].User + "</td><td style='width:30%;'>" + data.d[i].Reason + "</td> </tr>"); <%-- MMR | 2019.01.30 | Added to align text right--%>
                        } else {
                            $("#tblLoanFreezeUnfreezeHistory").append("<tr class='DataGrid_AlternateStyle'><td style='width:15%;'>" + data.d[i].Status + "</td><td style='width:15%;'>" + data.d[i].DateTime + "</td><td style='width:20%;text-align: right;'> " + data.d[i].Amount + "</td><td style='width:20%;'>" + data.d[i].User + "</td><td style='width:30%;'>" + data.d[i].Reason + "</td> </tr>"); <%-- MMR | 2019.01.30 | Added to align text right--%>

                        }
                    }
                }
            },
            error: function (result) {
                $("#tblLoanFreezeUnfreezeHistory tbody").html("");
                $("#tblLoanFreezeUnfreezeHistory").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>"+ result.responseText+"</td></tr>");
            }
        });       
        
    }
    
    function ClossLoanFreezeUnfreezeHistoryDialog() {
        $('#divLoanFreezeUnfreezeHistory').dialog("close");
        return false;
    }

    function ShowLoanFreezeUnfreezeHistoryDialog(fosender) {      
        var loandetailid = $(fosender).parent().find("[id$=hndloandetailId]").val();      
        $('#divLoanFreezeUnfreezeHistory').dialog("open");
        GetFreezeUnfreezHistory(loandetailid);
        return false;
    }
    <%--END: ML | 2019.01.14 | YRS-3157 | Get FreezeUnfreeze History detail and display data in dialogbox --%>
       
    function InitializeYRelationDialogBox() {
        <%--START: JT | 2018.08.28 | YRS-AT-4031 | Based on key HIDE_WEB_ACCOUNT_PRINT value controlling the appearance of Print button--%>
        var isPrintButtonToBeHidden = '<%=Me.IsPrintButtonToBeHidden%>';
        var dynamicButton  = [{ text: "Un-Lock Account", click: UnlockWebAccount }, { text: "Send email with temp pass", click: SendMailTempPass }, { text: "Lock Account", click: LockWebAccount }, { text: "Close", click: OkWebFront }];
        if (isPrintButtonToBeHidden == 'False') {
            dynamicButton = [{ text: "Print", click: PrintReport }, { text: "Un-Lock Account", click: UnlockWebAccount }, { text: "Send email with temp pass", click: SendMailTempPass }, { text: "Lock Account", click: LockWebAccount }, { text: "Close", click: OkWebFront }];
        }
        <%--END: JT | 2018.08.28 | YRS-AT-4031 | Based on key HIDE_WEB_ACCOUNT_PRINT value controlling the appearance of Print button--%>

        $("#divWebFront").dialog
					({
					    modal: true,
					    open: function (event, ui) { $(this).parent('div').find('button:contains("Close")').focus(); },
					    autoOpen: false,
					    title: "Web Account Information",
					    width: 700, height: 500,
                        <%-- START: JT | 2018.08.28 | YRS-AT-4031 | Assigning button properties based on key HIDE_WEB_ACCOUNT_PRINT value --%>
					    <%--buttons: [{ text: "Print", click: PrintReport }, { text: "Un-Lock Account", click: UnlockWebAccount }, { text: "Send email with temp pass", click: SendMailTempPass }, { text: "Lock Account", click: LockWebAccount }, { text: "Close", click: OkWebFront }],--%>
					    buttons: dynamicButton,
                        <%-- END: JT | 2018.08.28 | YRS-AT-4031 | Assigning button properties based on key HIDE_WEB_ACCOUNT_PRINT value --%>
					    open: function (type, data) {
					        $('a.ui-dialog-titlebar-close').remove();
					    }
					});

        $('#dvWebProcessing').dialog({
            autoOpen: false,
            resizable: false,
            draggable: true,
            closeOnEscape: false,
            close: false,
            modal: true,
            width: 350, height: 150,
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });

    }

    function OkWebFront() {
        jQuery('.ui-dialog button:nth-child(1)').button('enable');
        $("#divWebFront").dialog('close');
    }



    //Check access.
    var WebAccountInfoResult;
    function GetWebAccountDetails() {
        $.ajax({
            type: "POST",
            url: "ParticipantsInformation.aspx/GetWebAccountDetails",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (returndata) {
                if (returndata.d.errMessage != undefined) {
                    //START: SN | 05/13/2019 | YRS-AT-4371 | Commented the existing code, Handled condition where UserName is empty
                    //if (returndata.d.UserName != null){
                    if (returndata.d.UserName != null && returndata.d.UserName != '') {
                    //END: SN | 05/13/2019 | YRS-AT-4371 | Commented the existing code, Handled condition where UserName is empty
                        //$("#divWebFront").parent('div').find('button:contains("Print")').attr("disabled", "disabled");   <%-- HT | 2019.04.05 | YRS-AT-4371 | Commented the code to enable the print button--%>
                        $("#divWebFront").parent('div').find('button:contains("Print")').attr("disabled", "disabled");   <%-- Shilpa N | 05/09/2019 | YRS-AT-4371 | Uncommented the existing code, To enable the print button for non register participants only--%>
                        $("#lblScreenName").text(returndata.d.UserName);
                        $("#lblSecurityQuestion").text(returndata.d.strQuestion);
                        $("#lblSecurityAnswer").text(returndata.d.questionAnswer);

                        if ($('#chkNoWebAcctCreate').is(":checked")) {
                            $("#divWebFront").parent('div').find('button:contains("Lock Account")').attr("disabled", "disabled");
                            $("#divWebFront").parent('div').find('button:contains("Un-Lock Account")').attr("disabled", "disabled");
                            $("#divWebFront").parent('div').find('button:contains("Send email with temp pass")').attr("disabled", "disabled");
                            if (returndata.d.accountLocked) {
                                $("#lblAccountLocked").text("Yes ( " + returndata.d.accountLockedMsg + " )");
                            }
                            else {
                                $("#lblAccountLocked").text("No");
                            }
                        }
                        else {
                            if (returndata.d.accountLocked) {
                                $("#divWebFront").parent('div').find('button:contains("Lock Account")').attr("disabled", "disabled");
                                $("#divWebFront").parent('div').find('button:contains("Un-Lock Account")').removeAttr("disabled");
                                $("#lblAccountLocked").text("Yes ( " + returndata.d.accountLockedMsg + " )");
                                $("#divWebFront").parent('div').find('button:contains("Send email with temp pass")').attr("disabled", "disabled");
                            }
                            else {
                                $("#divWebFront").parent('div').find('button:contains("Lock Account")').removeAttr("disabled");
                                $("#divWebFront").parent('div').find('button:contains("Un-Lock Account")').attr("disabled", "disabled");
                                $("#lblAccountLocked").text("No");
                                $("#divWebFront").parent('div').find('button:contains("Send email with temp pass")').removeAttr("disabled");
                            }
                        }
                        //$("#txtAccountLockedOn").text(returndata.d.accountLockedMsg);
                        $("#lblEmail").text(returndata.d.email2);
                        $("#lblLastGoodLogin").text(returndata.d.lastGoodLoginMsg);
                        $("#lblLastBadLogin").text(returndata.d.lastBadLoginMsg);

                        if (returndata.d.errMessage != '') {
                            $("#divWebAcctMessage").html(returndata.d.errMessage);
                            $("#divWebAcctMessage").css('display', 'block');
                            $("#divWebAcctMessage")[0].className = "error-msg";
                        }

                        $("#divWebAcctDetails").css('display', 'block');

                    }
                    else {
                        if (returndata.d.errMessage != '') {
                            $("#divWebAcctMessage").html('Participant not registered. ');
                            $("#divWebAcctMessage").css('display', 'block');
                            $("#divWebAcctDetails").css('display', 'none');
                            $("#divWebAcctMessage")[0].className = "warning-msg";
                            $("#divWebFront").parent('div').find('button:contains("Send email with temp pass")').attr("disabled", "disabled");
                            $("#divWebFront").parent('div').find('button:contains("Lock Account")').attr("disabled", "disabled");
                            $("#divWebFront").parent('div').find('button:contains("Un-Lock Account")').attr("disabled", "disabled");
                        }

                    }
                }
                else {
                    $("#divWebAcctMessage").html(returndata.d);
                    $("#divWebAcctMessage").css('display', 'block');
                    $("#divWebAcctDetails").css('display', 'none');
                    $("#divWebAcctMessage")[0].className = "error-msg";
                    $("#divWebFront").parent('div').find('button:contains("Send email with temp pass")').attr("disabled", "disabled");
                    $("#divWebFront").parent('div').find('button:contains("Lock Account")').attr("disabled", "disabled");
                    $("#divWebFront").parent('div').find('button:contains("Un-Lock Account")').attr("disabled", "disabled");
                    $("#divWebFront").parent('div').find('button:contains("Print")').attr("disabled", "disabled");
                }
                GetAdminActivity();
            },
            failure: function () {
                CloseWebProcessingDialog();
                return false;
            }
        });
    }

    function ShowWebProcessingDialog(Message, divTitle) {
        $('#dvWebProcessing').dialog({ title: divTitle });
        $('#dvWebProcessing').dialog("open");
        $('#lblProcessing').text(Message);

    }


    function CloseWebProcessingDialog() {
        $('#dvWebProcessing').dialog('close');
    }
    function GetAdminActivity() {
        $.ajax({
            type: "POST",
            url: "ParticipantsInformation.aspx/GetAdminActivity",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (returndata) {
                var strLockMessagetype = new String();
                var strLockMessage = new String();
                var arrlockmessge = returndata.d.split('|');
                strLockMessagetype = arrlockmessge[0]
                strLockMessage = arrlockmessge[1]
                if (strLockMessagetype == 'error') {
                    $("#divWebAcctMessage")[0].className = "error-msg";
                    $("#divWebAcctMessage").html('There was an error: ' + strLockMessage);
                    $("#divWebAcctMessage").css('display', 'block');
                }

                if (returndata.d != "0") {
                    $('#divGridViewAcctActivity').html(returndata.d);
                }
                CloseWebProcessingDialog();
            },
            failure: function () {
                CloseWebProcessingDialog();
                return false;
            }
        });
    }

    function UnlockWebAccount() {
        if (CheckAccess('btnPartWebLockUnLock') == false)
        { return false; }
        else {
            ShowWebProcessingDialog('Please wait process is in progress...', '');
            $.ajax({
                type: "POST",
                url: "ParticipantsInformation.aspx/UnlockWebAccount",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (returndata) {
                    DisplayReturnedMessage(returndata, 'UnlockWebAccount');
                    GetWebAccountDetails();
                    CloseWebProcessingDialog();
                },
                failure: function () {
                    CloseWebProcessingDialog();
                    return false;
                }
            });
        }
    }

    function LockWebAccount() {
        if (CheckAccess('btnPartWebLockUnLock') == false)
        { return false; }
        else {
            ShowWebProcessingDialog('Please wait process is in progress...', '');
            $.ajax({
                type: "POST",
                url: "ParticipantsInformation.aspx/LockWebAccount",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (returndata) {
                    DisplayReturnedMessage(returndata, 'LockWebAccount');
                    GetWebAccountDetails();
                    CloseWebProcessingDialog();
                },
                failure: function () {
                    CloseWebProcessingDialog();
                    return false;
                }
            });
        }
    }

    function SendMailTempPass() {
        if (CheckAccess('btnPartWebSendTempPass') == false)
        { return false; }
        else {
            ShowWebProcessingDialog('Please wait process is in progress...', '');
            $.ajax({
                type: "POST",
                url: "ParticipantsInformation.aspx/SendMailTempPass",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (returndata) {
                    DisplayReturnedMessage(returndata, 'SendMailTempPass');
                    GetWebAccountDetails();
                    CloseWebProcessingDialog();
                },
                failure: function () {
                    CloseWebProcessingDialog();
                    return false;
                }
            });
        }

    }

    function DisplayReturnedMessage(returndata, process) {

        var strMessageType = new String();
        var strMessage = new String();
        var arrMessge = returndata.d.split('|');

        strMessageType = arrMessge[0]
        strMessage = arrMessge[1]

        if (strMessage != '') {
            $("#divWebAcctMessage").css('display', 'block');
            if (strMessageType == 'error') {
                $("#divWebAcctMessage")[0].className = "error-msg";
                $("#divWebAcctMessage").html('There was an error: ' + strMessage);
            }
            else if (strMessageType == 'ok') {
                $("#divWebAcctMessage")[0].className = "success-msg";
                if (process == 'SendMailTempPass') {
                    <%--$("#divWebAcctMessage").html('Email with temporary password send to: ' + strMessage);--%>
                    $("#divWebAcctMessage").html('Email with temporary password sent to: ' + strMessage);<%--PK |05-04-2019| YRS-AT-4239 | Change of word 'send' to 'sent'--%>

                }
                else {
                    $("#divWebAcctMessage").html(strMessage);
                }
            }
        }
        else {
            $("#divWebAcctMessage").html('');
            $("#divWebAcctMessage").css('display', 'none');
        }
    }

    function CheckAccessButtonWebFrontEnd(controlname) {
        if (CheckAccess(controlname) == false)
        { return false; }
        else {
            $("#divWebAcctMessage").html('');
            $("#divWebAcctMessage").css('display', 'none');
            $("#divWebFront").dialog('open');
            ShowWebProcessingDialog('Please wait data is being loaded...', '');
            GetWebAccountDetails();

            //$.ajax({
            //    type: "POST",
            //    url: "ParticipantsInformation.aspx/DisplayLoginCredentialInformation",
            //    data: "{}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (returndata) {
            //        var info = new Array();
            //        info = returndata.d;
            //        WebAccountInfoResult = info[0]
            //        //BT:1031 New change for Gemini ID YRS 5.0-1561
            //        //If error occured then reset and print button will be disable                      }
            //        if ((info[0] == "2") || (info[0] == "3")) {
            //            $("#LabelDispScreenName, #LabelDispPassword, #LabelDispSecurityQuestion, #LabelDispSecurityWord").text('');
            //            if (info[0] == "2") { $("#LabelScreenName").text('You are not authorized to view data.'); }
            //            else if (info[0] == "3") { $("#LabelScreenName").text('Error occured. Please contact IT support.'); }

            //            $("#divWebFront").parent('div').find('button:contains("Reset")').attr("disabled", "disabled")
            //            $("#divWebFront").parent('div').find('button:contains("Print")').attr("disabled", "disabled");
            //        }
            //        else if (info[0] == "1") {
            //            $("#LabelDispScreenName, #LabelDispPassword, #LabelDispSecurityQuestion, #LabelDispSecurityWord").text('');
            //            $("#LabelScreenName").text('Participant not registered.');
            //        }
            //        else {
            //            //info[0] = "0"
            //            $("#LabelScreenName").text(info[1]);
            //            $("#LabelPassword").text(info[2]);
            //            $("#LabelSecurityQuestion").text(info[3]);
            //            $("#LabelSecurityWord").text(info[4]);
            //        }

            //        $("#divWebFront").dialog('open');
            //    },
            //    failure: function () {
            //        return false;
            //    }
            //});

            return true;
        }
    }
    ////YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
    //function DeleteRecord() {

    //    if (CheckAccess("ButtonWebFrontEndReset") == false) {
    //        return false;
    //    }
    //    else {
    //        //BT:1031 New change for Gemini ID YRS 5.0-1561 
    //        //Added confirmation dialogbox
    //        if (WebAccountInfoResult != "0") {
    //            $("#dialog-confirm").dialog({
    //                resizable: false,
    //                height: 150,
    //                modal: true,
    //                buttons: { Ok: function () { $(this).dialog("close"); return false; } }
    //            });

    //            var msg;
    //            if (WebAccountInfoResult == "1") msg = "You cannot delete record. Participant not registered.";
    //            else if (WebAccountInfoResult == "2") msg = "You cannot delete record. You are not authorized to view data.";
    //            else if (WebAccountInfoResult == "3") msg = "Error occured. Please contact IT support.";

    //            $("#p_dialog_confirm").html(msg);
    //        }
    //        else {
    //            $("#p_dialog_confirm").html('This user information will be deleted. <BR>Are you sure you want to proceed?');
    //            $("#dialog-confirm").dialog({
    //                resizable: false,
    //                height: 145,
    //                modal: true,
    //                buttons: {
    //                    "Yes": function () {
    //                        $(this).dialog("close");
    //                        $.ajax({
    //                            type: "POST",
    //                            url: "ParticipantsInformation.aspx/DeleteRecord",
    //                            data: "{}",
    //                            contentType: "application/json; charset=utf-8",
    //                            dataType: "json",
    //                            success: function (record) {
    //                                var i;
    //                                i = record.d;
    //                                if (i == 1) { alert("Record deleted successfully."); WebAccountInfoResult = 1; $("#LabelScreenName, #LabelPassword, #LabelSecurityQuestion, #LabelSecurityWord").text(''); }
    //                                else if (i == 0) { alert("There is no record in the table."); WebAccountInfoResult = 1; }
    //                                else { alert("Error while deleting, please contact IT support."); WebAccountInfoResult = 3; }
    //                            },
    //                            failure: function () {
    //                                alert("Error while Deleting");
    //                                return false;
    //                            }
    //                        });
    //                        return true;
    //                    },
    //                    No: function () {
    //                        $(this).dialog("close");
    //                        return false;
    //                    }
    //                }
    //            });
    //        }

    //    }
    //}
    ////YRS 5.0-1561: Add 'Print Letter' and 'Reset Account' buttons to Web info display page
    function PrintReport() {
        jQuery('.ui-dialog button:nth-child(1)').button('disable');
        if (CheckAccess("ButtonWebFrontEndPrint") == false)
        { return false; }
        else {
            $.ajax({
                type: "POST",
                url: "ParticipantsInformation.aspx/PrintForm",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (record) {
                    var i;
                    i = record.d;
                    if (i == 1) { alert("You are not authorized to view data"); }
                        // Anudeep:28.11.2012 - added for Bt-1026/YRS 5.0-1629 : Web frontend letters to IDM.
                    else {
                        window.open('FT\\ReportViewer.aspx', 'ReportPopUp', "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')");
                        //'Anudeep:2012.11.04 - Bt-1026/YRS 5.0-1629:Code changes to copy report into IDM folder 
                        window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', 'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');
                    }
                    if (i != 1 && i != "") { alert(i); }
                },
                failure: function () {
                    alert("Error while printing");
                    return false;
                }
            });

            return true;
        }

    }

    //2012.08.17 SR : BT-957/YRS 5.0-1484 : Termination Watcher

    function LoadTerminationWatcher() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ParticipantsInformation.aspx/Binddata",
            data: "{}",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.d.length; i++) {

                    //$("#gvPersonDetails").append("<tr><td>" + data.d[i].Type + "</td><td>" + data.d[i].PlanType + "</td><td>" + data.d[i].Notes + "</td></tr>" );
                    //$("#gvPersonDetails").html("");


                    //$("[id*=gvPersonDetails] tr").not($("[id*=gvPersonDetails] tr:first")).remove();



                    if (data.d[i].Important == true) {
                        $("#gvPersonDetails").append("<tr><td style='color: red;  width:50px;'>" + data.d[i].Type + "</td><td style='color: red;  width:50px;'>" + data.d[i].PlanType + "</td><td style='color: red;  width:60px;'><label id='lblNote' style='width:60px;' >" + data.d[i].Notes + "</label></td> " + "</td><td style='width:60px;'><label id='lblNote' style='width:60px;' >" + data.d[i].Status + "</label></td> </tr>");
                    }
                    else {
                        $("#gvPersonDetails").append("<tr><td style='width:50px;'>" + data.d[i].Type + "</td><td  style='width:50px;'>" + data.d[i].PlanType + "</td><td style='width:50px;'> <label id='lblNote' style='width:60px;' >" + data.d[i].Notes + "</label> </td> " + "</td><td style='width:60px;'><label id='lblNote' style='width:60px;' >" + data.d[i].Status + "</label></td> </tr>");
                    }
                    //$("#ddlPlan").val(data.d[i].Type);             
                    // $("#ddlPlanType").val(data.d[i].PlanType);
                    // $("#txtTWNotes").val(data.d[i].Notes);
                }


                if (data.d.length <= 0) {
                    // $("#gvPersonDetails").hide(); 
                    $("#divTerminationWatcher").height($("#divTerminationWatcher").height());
                    $("#divTerminationWatcher").width($("#divTerminationWatcher").width());
                }

            },
            error: function (result) {
                //alert("Error");
            }
        });

    }


    function InitializeTerminationWatcherDialogBox() {
        $("#divTerminationWatcher").dialog
					({
					    modal: true,
					    open: function (event, ui) { $(this).parent('div').find('button:contains("SAVE")').focus(); },
					    autoOpen: false,
					    title: "Termination Watcher",
					    width: 780,
					    buttons: [{ text: "OK", click: SaveTerminationWatcher }, { text: "Cancel", click: CloseTerminationWatcher }]
					});
    }





    function SaveTerminationWatcher() {
        if (CheckAccess("ButtonTerminationWatcher") == false)
        { return false; }
        else {
            var DropDownList1 = $("#ddlPlan");
            var Type = DropDownList1.val();
            var DropDownList2 = $("#ddlPlanType");
            var PlanType = DropDownList2.val();
            var isImportant = $('#chkImportant').is(':checked');
            var Notes = $("#txtTWNotes").val();
            //   if( $("#txtTWNotes").val() == "")
            //   {
            //  
            //    //$("#p_dialog_confirm").html('Notes cannot be blank.');

            //   $("#dialog-confirm").dialog({
            //					title:"Termination Watcher",
            //	                resizable: false,
            //	                height: 250,
            //	                modal: true,
            //					buttons: { Ok: function () { $(this).dialog("close"); return false; } }
            //	            });
            //	
            //	 $("#p_dialog_confirm").html('Notes cannot be blank.');
            //	//alert("Notes cannot be blank.");
            //	return false;
            //   }

            $.ajax({
                type: "POST",
                url: "ParticipantsInformation.aspx/SaveTerminationWatcher",
                data: "{'Type':'" + Type + "','PlanType':'" + PlanType + "','isImportant':'" + isImportant + "','Notes':'" + Notes + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (record) {
                    $("#dialog-confirm").dialog({
                        title: "Termination Watcher",
                        resizable: false,
                        height: 200,
                        modal: true,
                        buttons: { Ok: function () { $(this).dialog("close"); return false; } }
                    });

                    var i;
                    i = record.d;
                    if (i == 1) {



                        $("#p_dialog_confirm").html('Record saved successfully');
                        $("[id*=gvPersonDetails] tr").not($("[id*=gvPersonDetails] tr:first")).remove();
                        $("#txtTWNotes").val('');
                        //$('#chkImportant').is(':unchecked');
                        $('#chkImportant').attr('checked', false);
                        //	alert("Record inserted successfully!"); 
                        LoadTerminationWatcher();
                    }
                    else {
                        //alert("Record already exist"); 
                        $("#p_dialog_confirm").html('Record already exist for plan type.');
                    }

                },
                failure: function () {
                    $("#dialog-confirm").dialog({
                        title: "Termination Watcher",
                        resizable: false,
                        height: 200,
                        modal: true,
                        buttons: { Ok: function () { $(this).dialog("close"); return false; } }
                    });

                    $("#p_dialog_confirm").html('Error while Saving Termination Watcher.');
                    //alert("Error while Saving Termination Watcher");
                    return false;
                }
            });
            return true;
        }
    }


    function CloseTerminationWatcher() { $("#divTerminationWatcher").dialog('close'); }


    function ControlTerminationWatcherButton() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ParticipantsInformation.aspx/LoadTerminationWatchers",
            data: "{}",
            dataType: "json",
            success: function (record) {
                var i;
                i = record.d;
                if (i == 0) { $("#ButtonTerminationWatcher").hide(); }
                else { $("#ButtonTerminationWatcher").show(); }
            },

            error: function (result) {
                //alert("Error");
            }
        });

    }

    function TogglePrimaryDiv() {
        if ($('#imgPrimary').attr('src') == 'images/expand.GIF') {

            $('#imgPrimary').attr('src', 'images/collapse.GIF');
            $('#tdPrimary').attr('title', 'Collapse');
            $('#tdPrimaryclick')[0].innerHTML = '(Please click to hide details)'

        }
        else {
            $('#imgPrimary').attr('src', 'images/expand.GIF');
            $('#tdPrimary').attr('title', 'Expand');
            $('#tdPrimaryclick')[0].innerHTML = '(Please click to view details)'
        }
        $('#divPrimary').slideToggle(500);

    }

    function ToogleSecondaryDiv() {
        if ($('#imgSecondary').attr('src') == 'images/expand.GIF') {

            $('#imgSecondary').attr('src', 'images/collapse.GIF');
            $('#tdSecondary').attr('title', 'Collapse');
            $('#tdSecondaryClick')[0].innerHTML = '(Please click to hide details)'
        }
        else {
            $('#imgSecondary').attr('src', 'images/expand.GIF');
            $('#tdSecondary').attr('title', 'Expand');
            $('#tdSecondaryClick')[0].innerHTML = '(Please click to view details)'
        }
        $('#divSecondary').slideToggle(500);


    }
    //SR:2013.08.05 - YRS 5.0-2070 : Create tooltip to display web service message.   
    function showToolTip(message, Type) {
        var elementRef = document.getElementById("Tooltip");
        if (elementRef != null) {
            elementRef.style.position = 'absolute';
            elementRef.style.left = event.clientX + 5 + document.body.scrollLeft;
            elementRef.style.top = event.clientY + 5 + document.body.scrollTop;
            elementRef.style.width = '570';
            elementRef.style.visibility = 'visible';
            elementRef.style.display = 'block';
        }
        var lblNote = document.getElementById("lblComments");
        // checking whether tooltips exists or not 
        if (Type == 'Pers') {
            lblNote.innerText = 'Person\'s Marital Status can not be changed due to following reason(s).\n' + message;
        }
            //Start: Bala: 01/19/2019: YRS-AT-2398: Tooltip for officers details
        else if (Type == "OfficersDetails") {
            if (lblNote.innerText == '') {
                lblNote.innerText = $('#HiddenFieldOfficerDetails').val();
                elementRef.style.left = event.clientX + 20 + document.body.scrollTop;
                elementRef.style.width = '300';
            } else {
                lblNote.innerText = ''
                hideToolTip();
            }
        }
            //End: Bala: 01/19/2019: YRS-AT-2398: Tooltip for officers details
        else {
            lblNote.innerText = 'Beneficiary add,edit and delete operation can not be performed due to following reason(s). \n' + message;
        }
    }
    //to hide tool tip when mouse is removed
    function hideToolTip() {
        var elementRef = document.getElementById("Tooltip");
        if (elementRef != null) {
            elementRef.style.visibility = 'hidden';
        }
    }
    //End, SR:2013.08.05 - YRS 5.0-2070 : Create tooltip to display web service message.   

    function InitializePINDialogBox() {
        $("#divPIN").dialog
                    ({
                        modal: true,
                        open: function (event, ui) { $(this).parent('div').find('button:contains("Save")').focus(); $('a.ui-dialog-titlebar-close').remove(); },
                        autoOpen: false,
                        resizable: false,
                        draggable: false,
                        closeOnEscape: false,
                        title: "Add / Edit PIN No.",
                        width: 350, height: 260,
                        buttons: [{ text: "Save", click: UpdatePIN }, { text: "Delete", click: ConfirmDeletePIN }, { text: "Close", click: ClosePINno }]
                    });
    }
    function UpdatePIN() {
        var strPin;
        var strConfirmPin;
        strPin = $("#txtPINno").val();
        strConfirmPin = $("#txtConfirmPIN").val();
        if (strPin == '') {
            ShowMessage('New PIN should not be blank.', 'error');
            return false;
        }
        if (strPin.length != 4) {
            ShowMessage('Please enter 4 digits PIN no.', 'error');
        }
        else if (strPin == strConfirmPin) {
            ShowMessage('In Progress', 'info');
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ParticipantsInformation.aspx/UpdatePIN",
                data: "{'strPIN':'" + strPin + "'}",
                dataType: "json",
                success: function (record) {
                    var i;
                    i = record.d.replace(/\s+/g, '');
                    if (i == "0") {
                        alert('Invalid Person');
                    }
                    else if (i == "-1") {
                        ShowMessage('New PIN should not be similar to last three pins.', 'error');
                    }
                    else if (i.length == 4) {
                        ShowMessage('New PIN updated successfully.', 'success');
                        $("#trPIN").css('display', 'block');
                        $("#lblPIN")[0].innerText = i;
                        $("#txtPIN").val(i);
                    }
                    else {
                        ShowMessage(record.d, 'error');
                    }
                },

                error: function (result) {
                    alert(result.responseText);
                }
            });
        }
        else {
            ShowMessage('New PIN and Confirm New PIN must be same.', 'error');
        }
        ClearPIN('clear');
    }

    function DeletePIN() {
        $("#dvPINConfirm").dialog('close');
        ShowMessage('In Progress', 'info');
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ParticipantsInformation.aspx/DeletePIN",
            dataType: "json",
            success: function (record) {
                var i;
                i = record.d.replace(/\s+/g, '');
                if (i == "0") {
                    alert('Invalid Person');
                }
                else if (i == "-1") {
                    ShowMessage('PIN does not exist.', 'error');
                }
                else if (i == '1') {
                    ShowMessage('PIN deleted successfully.', 'success');
                    $("#trPIN").css('display', 'none');
                    $("#lblPIN")[0].innerText = '';
                    $("#txtPIN").val('');
                    $("#txtPINno").val('');
                    $("#txtConfirmPIN").val('');
                }
                else {
                    ShowMessage(record.d, 'error');
                }
            },

            error: function (result) {
                alert(result.responseText);
            }
        });
    }

    function ClosePINno() {
        $("#divPIN").dialog('close');
    }

    function ValidatePIN() {
        if (!(event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57))
        { event.returnValue = false; }


    }
    function GetPIN() {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ParticipantsInformation.aspx/GetPIN",
            data: "{}",
            dataType: "json",
            success: function (record) {
                var i;
                i = record.d.replace(/\s+/g, '');
                if (i.length == 4) {
                    $("#lblPIN")[0].innerText = i;
                }
                else if (i != "0" && i != '') {
                    alert(record.d);
                    return false;
                }
                if ($("#lblPIN")[0].innerText == '' || $("#lblPIN")[0].innerText == '0') {
                    $("#trPIN").css('display', 'none');
                }
                else {
                    $("#trPIN").css('display', 'block');
                }
                $("#divPIN").dialog('open');
                $("#divMessage").css('display', 'none');
            },

            error: function (result) {
                alert(result.responseText);
            }
        });
    }
    function ShowMessage(message, type) {

        $("#divMessage").html(message);
        $("#divMessage").css('display', 'block');
        if (type == "error") {
            $("#divMessage")[0].className = "error-msg";
        }
        else if (type == "success") {
            $("#divMessage")[0].className = "success-msg";
        }
        else if (type == "info") {
            $("#divMessage")[0].className = "info-msg";
        }
    }

    function ClearPIN(type) {
        if (type == 'focus') {
            if ($("#txtPINno").val() == '') {
                $("#txtConfirmPIN").val('');
            }
        }
        else {
            $("#txtPINno").val('');
            $("#txtConfirmPIN").val('');
        }
    }
    function InitializePINConfirmDialogBox() {
        $("#dvPINConfirm").dialog
                    ({
                        modal: true,
                        open: function (event, ui) { $(this).parent('div').find('button:contains("Save")').focus(); $('a.ui-dialog-titlebar-close').remove(); },
                        autoOpen: false,
                        resizable: false,
                        draggable: false,
                        closeOnEscape: false,
                        title: "Add / Edit PIN No.",
                        width: 350, height: 120,
                        buttons: [{ text: "Yes", click: DeletePIN }, { text: "No", click: ClosePINConfirm }]
                    });
    }
    function ConfirmDeletePIN() {
        if ($("#trPIN").css('display') == 'block') {
            $("#dvPINConfirm").dialog('open');
            $("#lblPINConfirmmessage").text('Are you sure you want to delete PIN ?');
        }
        else {
            ShowMessage('PIN does not exist.', 'error');
        }
    }
    function ClosePINConfirm() {
        $("#dvPINConfirm").dialog('close');
    }
    /*Start: Anudeep A 2014.09.26 BT:2440:YRS 5.0-2318 - Primary Active address record being created twice. */
    function OpenProgressDialog() {
        $('#divProgress').dialog({
            autoOpen: false,
            resizable: false,
            draggable: true,
            closeOnEscape: false,
            close: false,
            modal: true,
            width: 350, height: 150,
            title: "Processing Information",
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });
    }

    function ShowProcessingDialog(lblMessage) {<%--VC | 2018.11.14 | YRS-AT-4018 | Receiving message as parameters--%>
        $("#divProgress").dialog('open');
        <%--START: VC | 2018.11.14 | YRS-AT-4018 | Commented existing code and added new code--%>
        //$('#labelMessage').text('Please wait while information is being saved.');
        $('#labelMessage').text(lblMessage);
        return true;
        <%--END: VC | 2018.11.14 | YRS-AT-4018 | Commented existing code and added new code--%>
    }
    /*End: Anudeep A 2014.09.26 BT:2440:YRS 5.0-2318 - Primary Active address record being created twice. */

    //Start: Bala: 01/19/2019: YRS-AT-2398: Hide if special handling is not required
    if ($("#LabelSpecialHandling").text() == '') {
        $('#LinkButtonSpecialHandling').hide();
        $("#LabelSpecialHandling").closest("tr").hide();
    }
    //End: Bala: 01/19/2019: YRS-AT-2398: Hide if special handling is not required
</script>
</html>
