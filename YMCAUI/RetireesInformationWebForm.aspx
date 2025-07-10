<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%--BS:2012.05.10:yrs:1470:enhancement of user control for person maintenance--%>
<%--<%@ Register TagPrefix="uc1" TagName="AddressWebUserControl" Src="UserControls/AddressWebUserControl.ascx" %>--%>
<%@ Register TagPrefix="uc1" TagName="Enhance_AddressWebUserControl" Src="~/UserControls/Enhance_AddressWebUserControl.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="uc1" TagName="DateUserControl" Src="UserControls/DateUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Header_WebUserControl" Src="UserControls/YMCA_Header_WebUserControl.ascx" %>
<%@ Register TagPrefix="NewYRSControls" TagName="New_AddressWebUserControl" Src="~/UserControls/AddressUserControl_new.ascx" %>
<%@ Register TagPrefix="YRSTelephoneControls" TagName="Telephone_WebUserControl" Src="~/UserControls/Telephone_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSControls" TagName="YMCA_Footer_WebUserControl" Src="~/UserControls/YMCA_Footer_WebUserControl.ascx" %>
<%@ Register TagPrefix="YRSStateTaxControls" TagName="StateWithholdingListing_WebUserControl" Src="~/UserControls/StateWithholdingListingControl.ascx" %><%-- ML |20.09.2019| YRS-AT-4598 |Added refrence of Statewithholding User control --%>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RetireesInformationWebForm.aspx.vb"
    Inherits="YMCAUI.RetireesInformationWebForm" EnableEventValidation="false" %>

<!--#include virtual="top.html"-->
<!--Included by Shashi Shekhar:2009-12-23:To use the common js function -->
<script type="text/javascript" language="javascript" src="JS/YMCA_JScript.js"></script>
<!--Included by Prasad Jadhav:2011-08-26:For BT-895,YRS 5.0-1364 : prompt user to if changes not saved -->
<script language="javascript" src="JS/YMCA_JScript_Warn.js"></script>
<script src="JS/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="JS/jquery-ui/jquery-ui-1.8.20.custom.min.js" type="text/javascript"></script>
<!-- START : SC | 2019.05.13 | YRS-AT-2601 | Adding .js file for disabled controls-->
<script type="text/javascript" src="JS/YMCA_JScript_DisableTextBox.js"></script>
<!-- END : SC | 2019.05.13 | YRS-AT-2601 | Adding .js file for disabled controls-->
<link href="JS/jquery-ui/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
<script language="JavaScript">

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
        RegisterUnSuppressAnnuityDialog(); <%--PPP | 04/20/2016 | YRS-AT-2719 | Registering Unsuppress annuity dialog box--%>
    });

    function CloseDialogDeathNotify() {
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
    //START : Shilpa N | 2019.03.08 | YRS-AT-4248 | To check Readonly mode and disable the button and add tooltip 
    function CheckReadOnly() {
        var unsupress = document.getElementById('<%= hdnUnSuppress.ClientID%>').value;
        var btnunsuppress = document.getElementById('ButtonSuppressedJSAnnuities');
        if (btnunsuppress) {
            if (unsupress != '') {
                document.getElementById('<%= ButtonSuppressedJSAnnuities.ClientID%>').disabled = true;
                var unsuppressbtn = document.getElementById('<%= ButtonSuppressedJSAnnuities.ClientID%>');
                unsuppressbtn.title = unsupress;
            }
        }
    }
    //END : Shilpa N | 2019.03.08 | YRS-AT-4248 | To check Readonly mode and disable the button and add tooltip 
    $(document).ready(function () {
        CheckReadOnly(); //Shilpa N | 2019.03.08 | YRS-AT-4248 | To check Readonly mode and disable the button and add tooltip
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

    function CallFrame(PageID) {
        var frameelement = document.getElementsByTagName('Iframe');
        frameelement.src = PageID;
    }

    //NP:2011.02.28 - Removed fncKeyStop(), fncKeySecStop(), fncKeyCityStop(), fncKeyCitySecStop() as it is no longer being used

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

        if (!ValidateTelephoneNo(document.Form1.all.TextBoxFax, document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value)) {
            <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
            alert('Fax number must be 10 digits.');--%>
            alert('Please provide valid Fax number.');
            <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
            return false;
        }

        if (!ValidateTelephoneNo(document.Form1.all.TextBoxMobile, document.Form1.all.AddressWebUserControl1_DropDownListCountry_hid.value)) {
            <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
            alert('Mobile number must be 10 digits.');--%>
            alert('Please provide valid Mobile number.');
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

        if (!ValidateTelephoneNo(document.Form1.all.TextBoxSecFax, document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value)) {
            <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
            alert('Fax number must be 10 digits.');--%>
            alert('Please provide valid Fax number.');
            <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
            return false;
        }

        if (!ValidateTelephoneNo(document.Form1.all.TextBoxSecMobile, document.Form1.all.AddressWebUserControl2_DropDownListCountry_hid.value)) {
            <%--START: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text
            alert('Mobile number must be 10 digits.');--%>
            alert('Please provide valid Mobile number.');
            <%--END: PPP | 2015.10.13 | YRS-AT-2588 | Error message replaced with system message text--%>
            return false;
        }
        //Start:AA:2015.13.05 -  BT:2680 : YRS 5.0-2428:Enhancement-for deceased participants, do not require spousal consent or cannot locate spouse
        if ($('#HiddenFieldDeathDate').val() != '') {
            if (IsValidUpdatedDeathDate() == false) {
                $("#ButtonSaveRetireeParticipants").disableControl(false);
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
        //End:AA:2015.13.05 -  BT:2680 : YRS 5.0-2428:Enhancement-for deceased participants, do not require spousal consent or cannot locate spouse
        ShowProcessingDialog(); /*Anudeep A:2014.09.26 BT:2440:YRS 5.0-2318 - Primary Active address record being created twice. */
        return true;
    }
    function SetMaxLengthPhone(str, value) {
        if (value == "US" || value == "CA") {
            str.maxLength = 10;
        }
        else {
            str.maxLength = 25;
        }
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

    function _OnBlur_TextBoxSSNoAnnuitites() {

        var str = String(document.Form1.all.TextBoxAnnuitiesSSNo.value);
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
            return false;
        }

        if (isNaN(parseInt(document.Form1.all.TextBoxAnnuitiesSSNo.value))) {
            alert('SSNo. cannot have characters.');
            document.Form1.all.TextBoxAnnuitiesSSNo.value = 0;
            document.Form1.all.TextBoxAnnuitiesSSNo.focus();
            return false;
        }
        else {
            if (str.length != 9) {
                alert('SSNo. must be 9 digits.');
                document.Form1.all.TextBoxAnnuitiesSSNo.focus();
                return false;
            }
        }


    }

    /* Shashi Shekhar:2009-12-31: comment and shift ValidateNumeric() in common external js file(JS/YMCA_JScript.js). Please check older version from SVN if needed. */


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

    //email function
    function echeck(str) {
        //alert('echeck called')
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
        //alert('called')
        var emailID = document.Form1.all.TextBoxEmailId
        //alert(emailID)
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

    /* Shashi Shekhar:2009-12-31: Added below function to open new window on Retirees QDRO Edit button click(ButtonGeneralQDROPendingEdit) */
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
            NewWindow('AddEditQDRO.aspx', 'mywin', '750', '550', 'yes', 'center')
            return true;
            //document.Form1.all.HiddenText.value="";
            //document.Form1.all.HiddenText.value="1";
        }

    }



    /* Shashi Shekhar:2009-12-31: comment and shift  CheckAccess(controlname) in common external js file(JS/YMCA_JScript.js). Please check older version from SVN if needed. */

    //Name Anita and Shubhrata Date 11 Apr 06 Reason:Secured Check Control
    //--Added by imran on 30-July-2010 : YRS 5.0-1141 add Age format to the person maintanace screen.
    function getAge() {

        var datDate1 = new Date();
        var datDate2 = new Date();
        var numTotalNumberofDays;
        datDate1 = Date.parse(document.getElementById('TextBoxGeneralDOB').value);

        if (document.getElementById('TextBoxGeneralDateDeceased').value != "") {
            datDate2 = Date.parse(document.getElementById('TextBoxGeneralDateDeceased').value);
        }

        var numTotalNumberofDays = (datDate2 - datDate1) / (24 * 60 * 60 * 1000)
        var numReminder = (numTotalNumberofDays % 365.2425)

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
            // strnumAge = strnumAge.replace(".", "Y/") + "M"
            if (strnumAge.indexOf(".") > -1) {
                //.1 means 10 month.In javascript retuns 10 month as .1
                if (strnumAge.indexOf(".11") == -1) {
                    strnumAge = strnumAge.replace(".1", ".10")
                }

                strnumAge = strnumAge.replace(".", "Y/") + "M"
            }
            else { strnumAge = strnumAge + "Y" }
        }
        document.getElementById('LabelBoxAge').innerText = strnumAge;
    }

    //Start:AA:2015.13.05 -  BT:2680 : YRS 5.0-2428:commented below beacause it is handled in checkTelephoneLength() method
    //BS:2012.01.18:YRS 5.0-1497 :- here we have call IsValidUpdatedDeathDate() on ButtonSaveParticipant click  if it is false then enabled button
    //$(document).ready(function () {
    //    $("#ButtonSaveRetireeParticipants").click(function () {


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
    //End:AA:2015.13.05 -  BT:2680 : YRS 5.0-2428:commented below beacause it is handled in checkTelephoneLength() method
    function IsValidUpdatedDeathDate() {
        var NewDeathDate = new Date($('#TextBoxGeneralDateDeceased').val());
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
    function EnableControls() {
        document.Form1.all.ButtonSaveRetireeParticipants.disabled = false;
        document.Form1.all.ButtonRetireesInfoCancel.disabled = false;
        //document.Form1.all.ButtonAdd.disabled=true;
        document.Form1.all.ButtonRetireesInfoOK.disabled = true;
        $('#ButtonSaveRetireeParticipants').click(function () {
            return checkTelephoneLength();
        });

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
                str = 'Person\'s Marital Status can not be changed due to following reason(s). <br/>' + str;
            }
            $("#divWSMessage").html(str);
            $("#divWSMessage").dialog('open');
            return false;
        });
    }
    //End, Sanjay R:2013.08.05 - YRS 5.0-2070: Need web service to accept beneficiary updates (Implementing restriction in YRS)
    //SP 2014.02.05 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -Start
    function OpenViewSSNUpdatesWindow() {
        window.open('UpdateSSN.aspx?Name=retire&Mode=ViewSSN', 'CustomPopUp', 'width=580, height=500, menubar=no, Resizable=No,top=70,left=120, scrollbars=yes');
        return false;
    }
    //'SP 2014.02.05 BT-2189\YRS 5.0-2198:Need to capture 'reason' value when changing an SSN -End

    <%-- START : SB | 07/07/2016 | YRS-AT-2382 | Sending the unique beneficiary id while calling UpdateSSN Page --%>
    //function OpenViewSSNUpdatesWindow(beneficiaryID) {
    //    window.open('UpdateSSN.aspx?Name=retireAndBeneficiary&Mode=ViewSSN&BenefID=' + beneficiaryID, 'CustomPopUp', 'width=580, height=500 menubar=no, Resizable=No,top=70,left=120, scrollbars=yes');
    //    return false;
    //}

    function OpenViewSSNUpdatesWindowAnnuities(url) {
        window.open(url, 'CustomPopUp', 'width=580, height=500 menubar=no, Resizable=No,top=70,left=120, scrollbars=yes');
        return false;
    }
    <%-- END : SB | 07/07/2016 | YRS-AT-2382 | Sending the unique beneficiary id while calling UpdateSSN Page --%>

    <%-- START : SB | 07/07/2016 | YRS-AT-2382 | Calling the UpdateSSN Page --%>
    function OpenViewAnnuityBenifSSNUpdatesWindow() {
        window.open('UpdateSSN.aspx?Name=retireAndAnnuityBeneficiary&Mode=ViewSSN', 'CustomPopUp', 'width=580, height=500, menubar=no, Resizable=No,top=70,left=120, scrollbars=yes');
        return false;
    }
    <%-- END : SB | 07/07/2016 | YRS-AT-2382 | Calling the UpdateSSN Page --%>
    <%-- START: MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
    function ShowPIIErrorMessage() {
        var messageText;
        var result = true;
        messageText = '<%= Me.PIIInformationRestrictionMessageText%>';
        if (messageText != null && messageText != '') {
            $("#DivErrorMessage").html(messageText);
                $("#DivErrorMessage").show();
                $("#DivErrorMessage").addClass("error-msg");
                result = false;
            }
            return result;
        }
    <%-- END: MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
</script>
<!--Added two SortExpressions/Dataformatstring for DataGridNotes by Swopna in response to YREN-4126-->
<body oncontextmenu="return false">
    <form id="Form1" method="post" runat="server">
        <div class="Div_Center">
            <table class="Table_WithoutBorder" cellspacing="0" width="980">
                <tr>
                    <td class="Td_BackGroundColorMenu" align="left">
                        <cc1:Menu ID="Menu1" runat="server" Layout="Horizontal" HighlightTopMenu="False"
                            Cursor="Pointer" CssClass="menustyle" DefaultCssClass="menuitem" DefaultMouseDownCssClass="mousedown"
                            DefaultMouseOverCssClass="mouseover" DefaultMouseUpCssClass="mouseup" MenuFadeDelay="2"
                            mouseovercssclass="MouseOver">
                            <SelectedMenuItemStyle ForeColor="#3B5386" BackColor="#FBC97A"></SelectedMenuItemStyle>
                        </cc1:Menu>
                    </td>
                    <td class="menuitem" width="10%" nowrap align="left">
                        <asp:HyperLink ID="HyperLinkViewParticipantsInfo" Font-Underline="False" BackColor="#000099"
                            ForeColor="#ffffff" NavigateUrl="ParticipantsInformation.aspx" runat="server"
                            CssClass="Warn_Dirty">Participant Information</asp:HyperLink>
                    </td>
                </tr>
                <!--<tr>
					<td class="Td_HeadingFormContainer" align="left" colSpan="3"><IMG title="image" height="10" alt="image" src="images/spacer.gif" width="10">
						Retirees Information --
						<asp:label id="LabelHdr" Height="70%" runat="server" CssClass="Td_HeadingFormContainer"></asp:label></td>
				</tr>-->
                <tr align="left">
                    <td class="Td_HeadingFormContainer" align="left" colspan="2">
                        <YRSControls:YMCA_Header_WebUserControl ID="HeaderControl" runat="server"></YRSControls:YMCA_Header_WebUserControl>
                    </td>
                </tr>
                <tr>
                    <%-- Start, Changed to visble false by Anudeep on 2012.11.02 for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen  --%>
                    <%--Reverted changes by Anudeep on 2012.11.14 for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen Changed from 'Proirity Handling' to 'Exhausted DB settlement efforts' --%>
                    <td colspan="2" bgcolor="red">
                    <%-- Start: Bala: 01/19/2019: YRS-AT-2398: Priority is replaced with ExhaustedDBSettle--%>                    
                    <%--<asp:Label ID="LabelPriorityHdr" ForeColor="White" runat="server" Font-Size="12px"
                        Font-Bold="True">Exhausted DB / RMD Settlement Efforts </asp:Label>--%>
                        <asp:Label ID="LabelExhaustedDBSettleHdr" runat="server" Font-Bold="True" Font-Size="12px"
                            ForeColor="White">Exhausted DB / RMD Settlement Efforts</asp:Label>
                    <%-- End: Bala: 01/19/2019: YRS-AT-2398: Priority is replaced with ExhaustedDBSettle--%>
                    </td>
                    <%--End,  Changed to visble false by Anudeep on 2012.11.02 for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen  --%>
                </tr>
                <%-- Start: Bala: 01/19/2019: YRS-AT-2398: Alert message--%>
                <tr>
                    <td bgcolor="red" colspan="2" style="text-align: left;">
                        <asp:Label ID="LabelSpecialHandling" ForeColor="White" runat="server" Font-Size="12px" Font-Bold="True"></asp:Label>
                        <a id="LinkButtonSpecialHandling" style="color: white; font-size: 9px; cursor: pointer;" onclick="showToolTip('', 'OfficersDetails')" onmouseout="javascript: document.getElementById('lblComments').innerHTML = ''; hideToolTip();">[view details]</a>
                        <asp:HiddenField ID="HiddenFieldOfficerDetails" runat="server" />
                    </td>
                </tr>
                <%-- End: Bala: 01/19/2019: YRS-AT-2398: Alert message--%>
                <tr>
                    <td align="left">
                        <asp:ValidationSummary ID="ValidationSummaryRetirees" runat="server" EnableClientScript="True"
                            CssClass="Error_Message"></asp:ValidationSummary>
                    </td>
                </tr>
            </table>
        </div>
        <div id="DivMainMessage" class="warning-msg" runat="server" style="text-align: left;" enableviewstate="false">
        </div>
        <%-- START: MMR | 2018.11.23 | YRS-AT-3101 | Added div control to display error message --%>
        <div id="DivErrorMessage" class="error-msg" runat="server" style="text-align: left;display:none;">
        </div>
        <%-- END: MMR | 2018.11.23 | YRS-AT-3101 | Added div control to display error message --%>
        <div id="divErrorMsg" class="error-msg" runat="server" style="text-align: left;" enableviewstate="false" visible="false"></div><%--Dharmesh : 11/20/2018 : YRS-AT-4136 : Added new error div to display error for participant who enrolled on or after 2019 --%>
        <div class="Div_Left">
            <table id="TableContainer" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <!-- Shashi:24-01-2011: For YRS 5.0-1247, BT-713 : Change two tab's heading -->
                        <td>
                            <iewc:TabStrip ID="TabStripRetireesInformation" runat="server" BorderStyle="None"
                                AutoPostBack="True" TabDefaultStyle="background-color:#4172A9;font-family:verdana;font-weight:bold;font-size:8pt;color:#ffffff;width:55;text-align:center;border:solid 1px White;border-bottom:none"
                                TabHoverStyle="background-color:#93BEEE;color:#4172A9;" TabSelectedStyle="background-color:#93BEEE;color:#000000;"
                                Width="100%" Height="30px">
                                <iewc:Tab Text="General" ID="TabStripTabGeneral"></iewc:Tab>
                                <iewc:Tab Text="Annuities" ID="TabStripTabAnnuities"></iewc:Tab>
                                <iewc:Tab Text="Beneficiaries" ID="TabStripTabBeneficiaries"></iewc:Tab>
                                 <%--START : SN | 2020.02.03 | YRS-AT-4604 | Added space between Annuities and Paid --%>
                                <iewc:Tab Text="Annuities Paid" ID="TabStripTabAnnuitiesPaid"></iewc:Tab>
                                 <%--END : SN | 2020.02.03 | YRS-AT-4604 | Added space between Annuities and Paid --%>
                                <iewc:Tab Text="Banking" ID="TabStripTabBanking"></iewc:Tab>
                                <%--START : ML | 2019.09.20 | YRA-At-4598 | Rename Federal withholding to Tax withholding--%>
                                <%--<iewc:Tab Text="Federal W/h" ID="Tab1"></iewc:Tab>--%>
                                <iewc:Tab Text="Tax Withholding" ID="TabStripTabTaxWithholding"></iewc:Tab>
                                <%--END : ML | 2019.09.20 | YRA-At-4598 | Rename Federal W/h to Tax W/h"--%>
                                <iewc:Tab Text="Other W/h" ID="TabStripTabGeneralWithholding"></iewc:Tab>
                                <iewc:Tab Text="Notes"></iewc:Tab>
                                <iewc:Tab Text="Documents"></iewc:Tab>
                            </iewc:TabStrip>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <iewc:MultiPage ID="MultiPageRetireesInformation" runat="server" Width="100%">
                                <iewc:PageView>
                                    <!-- General PageView Start -->
                                    <table class="Table_WithBorder" width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td class="td_Text">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="left" width="75%" class="td_Text">General
                                                        <asp:Label ID="LabelGenHdr" runat="server" CssClass="td_Text"></asp:Label>
                                                        </td>
                                                        <td align="right" class="td_Text">
                                                            <input type="button" id="ButtonWebFrontEndRetiree" style="width: 100px;" value="Web Acct Info"
                                                                class="Button_Normal" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td colspan="4" align="left">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="LabelGeneralSalute" runat="server" CssClass="Label_Small">Salute</asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="cboSalute" runat="server" CssClass="DropDown_Normal Warn" Width="56px">
                                                                            <asp:ListItem Value=""></asp:ListItem>
                                                                            <asp:ListItem Value="Mr.">Mr</asp:ListItem>
                                                                            <asp:ListItem Value="Mrs.">Mrs</asp:ListItem>
                                                                            <asp:ListItem Value="Ms.">Ms</asp:ListItem>
                                                                            <asp:ListItem Value="Dr.">Dr</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="LabelGeneralFirstName" runat="server" Width="32px" CssClass="Label_Small">First</asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox Width="100px" runat="server" ID="TextBoxGeneralFirstName" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator CssClass="Label_Small" ID="RequiredFieldValidator1" runat="server"
                                                                            ErrorMessage="First name required (General)" Display="Dynamic" ControlToValidate="TextBoxGeneralFirstName">*</asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="LabelGeneralMiddleName" runat="server" CssClass="Label_Small">Middle</asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox Width="100px" runat="server" ID="TextBoxGeneralMiddleName" CssClass="TextBox_Normal Warn"
                                                                            MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="LabelGeneralLastName" runat="server" CssClass="Label_Small">Last</asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox Width="100px" runat="server" ID="TextBoxGeneralLastName" CssClass="TextBox_Normal Warn"
                                                                            MaxLength="50"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator CssClass="Label_Small" ID="Requiredfieldvalidator2" runat="server"
                                                                            ErrorMessage="Last name required (General)" Display="Dynamic" ControlToValidate="TextBoxGeneralLastName">*</asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="LabelGeneralSuffixName" runat="server" CssClass="Label_Small" maxlength="20">Suffix</asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox Width="100px" runat="server" ID="TextBoxGeneralSuffix" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Button ID="ButtonGeneralEdit" runat="server" Width="73px" CssClass="Button_Normal"
                                                                            Text="Edit" controltovalidate="False" OnClientClick="javascript: return ShowPIIErrorMessage();"></asp:Button> <%--MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" valign="top">
                                                            <table cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td align="left" width="17%">
                                                                        <asp:Label ID="FundNo" runat="server" CssClass="Label_Small">Fund No</asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 40%">
                                                                        <asp:TextBox ID="TextboxFundNo" Width="90" runat="server" CssClass="TextBox_Normal"></asp:TextBox>
                                                                    </td>
                                                                    <td rowspan="4" valign="top" align="right" colspan="3" style="width: 48%">
                                                                        <table border="0" width="100%" cellspacing="0" class="Table_WithBorder" align="right">
                                                                            <tr>
                                                                                <td align="left" class="td_Text">QDRO
                                                                                </td>
                                                                                <td align="right" class="td_Text">
                                                                                    <asp:Button ID="ButtonGeneralQDROPendingEdit" Width="73px" runat="server" Text="Edit"
                                                                                        CssClass="Button_Normal" CausesValidation="false"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="LabelGeneralQDROPending" runat="server" CssClass="Label_Small">Pending</asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkGeneralQDROPending" runat="server" CssClass="CheckBox_Normal Warn"></asp:CheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="LabelGeneralQDROStatudDate" runat="server" CssClass="Label_Small">Status Date</asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="TextBoxGeneralQDROStatudDate" runat="server" CssClass="TextBox_Normal Warn"
                                                                                        Width="70"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="LabelGeneralQDROStatus" runat="server" CssClass="Label_Small">Status</asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="TextBoxGeneralQDROStatus" runat="server" CssClass="TextBox_Normal Warn"
                                                                                        Width="70"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="120">
                                                                        <asp:Label ID="LabelGeneralSSNo" runat="server" CssClass="Label_Small">SS No</asp:Label>
                                                                    </td>
                                                                    <td align="left" colspan="2">
                                                                        <asp:TextBox ID="TextBoxGeneralSSNo" runat="server" CssClass="TextBox_Normal Warn"
                                                                            MaxLength="9" Width="90"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator CssClass="Label_Small" ID="Requiredfieldvalidator3" runat="server"
                                                                            ErrorMessage="SS No required (General)" Display="Dynamic" ControlToValidate="TextBoxGeneralSSNo">*</asp:RequiredFieldValidator>
                                                                        &nbsp;&nbsp;
                                                                    <asp:Button ID="ButtonGeneralSSNoEdit" runat="server" Text="Edit" CssClass="Button_Normal"
                                                                        Width="73px" CausesValidation="False" OnClientClick="javascript: return ShowPIIErrorMessage();"></asp:Button> <%--MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                        &nbsp;<asp:LinkButton ID="lnkbtnViewSSNUpdate" Text="View SSN Updates" OnClientClick="javascript:OpenViewSSNUpdatesWindow(); return false;" runat="server"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="17%">
                                                                        <asp:Label ID="LabelGeneralGender" runat="server" Width="150px" CssClass="Label_Small">Gender</asp:Label>
                                                                    </td>
                                                                    <td colspan="2" width="40%">
                                                                        <asp:DropDownList ID="DropDownListGeneralGender" runat="server" Width="90" CssClass="DropDown_Normal Warn"
                                                                            AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator CssClass="Label_Small" EnableClientScript="True" ID="rfvGender"
                                                                            runat="server" ErrorMessage="Please select valid gender." Display="Dynamic" Enabled="true"
                                                                            ControlToValidate="DropDownListGeneralGender" InitialValue="U">*</asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" width="17%">
                                                                        <asp:Label ID="LabelGeneralMaritalStatus" runat="server" Width="127px" CssClass="Label_Small">Marital Status</asp:Label>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td colspan="2" width="40%">
                                                                        <%-- Changed by Anudeep:02-11-2012 For BT-1321-YRS 5.0-1712:lengthen marital status field  --%>
                                                                        <asp:DropDownList ID="cboGeneralMaritalStatus" runat="server" CssClass="DropDown_Normal Warn"
                                                                            Width="150" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                        <asp:Image ID="imgLock" runat="server" ImageUrl="Images/lock.png" Visible="false"
                                                                            Width="15px" Height="15px" />
                                                                    </td>
                                                                </tr>
                                                                <tr valign="top">
                                                                    <td align="left" width="18%">
                                                                        <asp:Label ID="LabelGeneralDOB" runat="server" CssClass="Label_Small">DOB</asp:Label>
                                                                        <span style="color: Gray">
                                                                            <asp:Label ID="Label1" runat="server">(Age:</asp:Label>
                                                                            <asp:Label ID="LabelBoxAge" runat="server" Width="55px"></asp:Label>)
                                                                        <%--<asp:TextBox ID="TextBoxAge" runat="server" Width="60px" CssClass="TextBox_Normal Warn"
                                                                            ReadOnly="true" Visible="false"></asp:TextBox>--%>
                                                                        </span>
                                                                    </td>
                                                                    <td align="left" width="35%">
                                                                        <asp:TextBox ID="TextBoxGeneralDOB" runat="server" Width="70" CssClass="TextBox_Normal DateControl"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="PopcalendarDate" runat="server" Separator="/" Format="mm dd yyyy"
                                                                            ScriptsValidators="IsValidDate" Enabled="false" Control="TextBoxGeneralDOB"></rjs:PopCalendar>
                                                                        <asp:CustomValidator ID="valCustomDOB" runat="server" ClientValidationFunction="IsValidDate"
                                                                            ControlToValidate="TextBoxGeneralDOB" Display="Dynamic">*</asp:CustomValidator>
                                                                        <asp:RequiredFieldValidator CssClass="Label_Small" ID="ReqGeneralDOB" runat="server"
                                                                            Enabled="False" ControlToValidate="TextBoxGeneralDOB" ErrorMessage="DOB required (General)"
                                                                            Display="Dynamic">*</asp:RequiredFieldValidator>&nbsp;&nbsp;
                                                                    <asp:Button ID="ButtonGeneralDOBEdit" runat="server" Width="73px" Text="Edit" CssClass="Button_Normal"
                                                                        CausesValidation="False" OnClientClick="javascript: return ShowPIIErrorMessage();"></asp:Button> <%--MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                    </td>
                                                                    <!--Code inserted by shashi on Feb 01 2011 For YRS 5.0-1236, BT-698 -->
                                                                    <td valign="top" id="td1" runat="server" align="right" rowspan="2" style="width: 48%">
                                                                        <table id="TABLE3" runat="server" class="Table_WithBorder" cellspacing="0" width="100%"
                                                                            align="right">
                                                                            <tbody>
                                                                                <tr>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="td_Text" align="left" colspan="1">Account Lock/Unlock
                                                                                    </td>
                                                                                    <td align="right" class="td_Text">
                                                                                        <asp:Button ID="btnAcctLockEditRet" runat="server" Text="Edit" CssClass="Button_Normal"
                                                                                            Width="73px" CausesValidation="false"></asp:Button>
                                                                                    </td>
                                                                                </tr>
                                                                                <!--Start: Lock reason Display Section row -->
                                                                                <tr id="trLockResDisplay" runat="server">
                                                                                    <td colspan="2" align="left">
                                                                                        <table cellspacing="0">
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
                                                                                    <td colspan="2" align="left">
                                                                                        <table cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td align="left" width="40%" style="color: Gray">Reason Code:
                                                                                                <asp:Label ID="lblResCode" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td width="60%">
                                                                                                    <asp:DropDownList ID="ddlReasonCode" runat="server" CssClass="DropDown_Normal" Width="100%"
                                                                                                        Enabled="true">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <asp:Button ID="btnLockUnlock" runat="server" Text="Lock Account" CssClass="Button_Normal"
                                                                                                        Width="60%" CausesValidation="False"></asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <!--End: Lock reason Edit Section row -->
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                    <!-----------------End-------------------------->
                                                                </tr>
                                                                <tr valign="top">
                                                                    <td align="left" width="120">
                                                                        <asp:Label ID="LabelGeneralRetireDate" runat="server" CssClass="Label_Small">Retire Date</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="TextBoxGeneralRetireDate" runat="server" CssClass="TextBox_Normal"
                                                                            ReadOnly="true" Width="70"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr valign="top">
                                                                    <td align="left" width="120">
                                                                        <asp:Label ID="LabelGeneralDateDeceased" runat="server" CssClass="Label_Small">Date Deceased</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <%-- 'BS:2012.03.13:BT:993:- allow user to enter Deceased Date through keyboard so remove readonly property 
                                                                  <asp:TextBox ID="TextBoxGeneralDateDeceased" runat="server" CssClass="TextBox_Normal Warn" Width="105"
                                                                        ReadOnly="true" Enabled="False"></asp:TextBox>--%>
                                                                        <asp:TextBox ID="TextBoxGeneralDateDeceased" runat="server" CssClass="TextBox_Normal Warn"
                                                                            Width="70" Enabled="False"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="PopCalTextBoxGeneralDateDeceased" runat="server" ScriptsValidators="IsValidDate"
                                                                            Format="mm dd yyyy" Control="TextBoxGeneralDateDeceased" Separator="/"></rjs:PopCalendar>
                                                                        <asp:CustomValidator ID="CustomValidatorTextBoxGeneralDateDeceased" runat="server"
                                                                            ClientValidationFunction="IsValidDate" ControlToValidate="TextBoxGeneralDateDeceased"
                                                                            Display="Dynamic">*</asp:CustomValidator>&nbsp;&nbsp;&nbsp;
                                                                    <asp:Button ID="ButtonDeathNotification" runat="server" Width="150" Text="Death Notification"
                                                                        CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                                        <asp:Button ID="btnEditDeathDateRet" runat="server" Text="Edit Death Date" CssClass="Button_Normal"
                                                                            Enabled="True" CausesValidation="false"></asp:Button>
                                                                    </td>
                                                                    <td id="Td2" valign="top" runat="server" align="right" style="width: 48%" rowspan="1">
                                                                        <table border="0" width="100%" cellspacing="0" class="Table_WithBorder" runat="server"
                                                                            id="tblSuppress" align="right">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="LabelSuppressedJSAnnuities" runat="server" CssClass="Label_Small"
                                                                                        Text="Suppressed JS Annuities"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="ButtonSuppressedJSAnnuities" runat="server" Width="83px" Text="Unsuppress" ClientIDMode="Static"
                                                                                        CssClass="Button_Normal" CausesValidation="False" OnClientClick="javascript:return ShowUnSuppressAnnuityDialog();"></asp:Button> <%--PPP | 04/20/2016 | YRS-AT-2719 | Added OnClientClick="javascript:return ShowUnSuppressAnnuityDialog();" to open Unsuppress annuity dialog box--%>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="TextboxSuppressAnnuityCount" runat="server" CssClass="TextBox_Normal Warn"
                                                                                        ReadOnly="true" Width="35"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr valign="top">
                                                                    <td align="left" width="120">
                                                                        <asp:Label runat="server" CssClass="Label_Small" ID="LabelPOA">POA/Other Rep.</asp:Label>
                                                                    </td>
                                                                    <td align="left" width="252">
                                                                        <asp:Button ID="ButtonGeneralPOA" runat="server" Width="150" Text="Show/Edit all"
                                                                            CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                                        <asp:TextBox ID="TextBoxGeneralPOA" runat="server" CssClass="TextBox_Normal Warn"
                                                                            Visible="false" Width="90"></asp:TextBox>
                                                                        <asp:CheckBox runat="server" Visible="false" ID="chkOldGuardNews" Text="Old Guard News"
                                                                            CssClass="CheckBox_Normal" AutoPostBack="true"></asp:CheckBox>
                                                                    </td>
                                                                    <!--Code inserted by shashi on 28th Aug 2009 For PS: YMCA PS Data Archive.Doc-->
                                                                    <td valign="top" id="tdRetrieveData" runat="server" align="left" rowspan="2" style="width: 48%">
                                                                        <table id="tblRetrieveData" runat="server" class="Table_WithBorder" cellspacing="0"
                                                                            border="0" width="100%" align="right">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td class="td_Text" align="left">Data Archived
                                                                                    </td>
                                                                                    <td class="td_Text" align="right"></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Button ID="ButtonGetArchiveDataBack" runat="server" Text="Retrieve Data" CssClass="Button_Normal"
                                                                                            CausesValidation="False"></asp:Button>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                    <!----- ------------End-------------------------->
                                                                    <%--<td colspan="3">
                                                                    <asp:TextBox ID="TextBoxGeneralPOA" runat="server" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                    <asp:Button ID="ButtonGeneralPOA" runat="server" Width="150" Text="Show/Edit all POA" CssClass="Button_Normal"
                                                                        CausesValidation="False"></asp:Button>
                                                                    &nbsp; &nbsp;
                                                                    <asp:CheckBox runat="server" Visible="false" ID="chkOldGuardNews" Text="Old Guard News"
                                                                        CssClass="CheckBox_Normal" AutoPostBack="true"></asp:CheckBox>
                                                                </td>--%>
                                                                </tr>
                                                                <tr>
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

                                                                </tr>
                                                                <tr>
                                                                    <table border="0" width="85%" cellpadding="1" cellspacing="1">
                                                                        <tr>
                                                                            <td align="right" nowrap>
                                                                                <%--Reverted changes by Anudeep on 2012.11.14 for BT-1237 YRS 5.0-1677:Remove Priorty Handling checkbox from this screen Changed from 'Proirity Handling' to 'Exhausted DB settlement efforts' --%>
                                                                                <%--Start: Bala: 01/09/2019: YRS-AT-2398: Control name chenged form CheckboxPriority to CheckboxExhaustedDBSettle --%>
                                                                                <%--<asp:CheckBox ID="CheckboxPriority" runat="server" AutoPostBack="True" Text="Exhausted DB / RMD Settlement Efforts"
                                                                                    CssClass="Warn"></asp:CheckBox>--%>
                                                                                <asp:CheckBox ID="CheckboxExhaustedDBSettle" runat="server" AutoPostBack="True" Text="Exhausted DB / RMD Settlement Efforts"
                                                                                    CssClass="Warn"></asp:CheckBox> 
                                                                                <%--End: Bala: 01/09/2019: YRS-AT-2398: Control name chenged form CheckboxPriority to CheckboxExhaustedDBSettle --%>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:CheckBox runat="server" ID="chkPersonalInfoSharingOptOut" AutoPostBack="true"
                                                                                    Text="Personal Info Sharing Opt-out" CssClass="Warn"></asp:CheckBox>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:CheckBox runat="server" ID="chkGoPaperless" AutoPostBack="true" CssClass="Warn"
                                                                                    Text="Go Paperless"></asp:CheckBox>
                                                                            </td>
                                                                            <td>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <%--Start: Bala: 01/05/2016: YRS-AT-1972: Added special death processing required check box--%>
                                                                            <td align="left">
                                                                                <asp:CheckBox runat="server" ID="chkSpecialDeathProcess" CssClass=" CheckBox_Normal Warn" AutoPostBack="true" Text="Special Death Processing Required"></asp:CheckBox>
                                                                            </td>
                                                                            <%--End: Bala: 01/05/2016: YRS-AT-1972: Added special death processing required check box--%>
                                                                            <%--START | Sanjay S. | 2016.02.03 - YRS-AT-2247 - Need bitflag that will not allow a participant to create a web account. This feature is turned on in 17.0.0--%>
                                                                            <td align="left">
                                                                                <asp:CheckBox runat="server" ID="chkNoWebAcctCreate" CssClass=" CheckBox_Normal Warn" AutoPostBack="true" Text="Restrict Web Acct. Creation"></asp:CheckBox>
                                                                            </td>
                                                                            <%--END | Sanjay S. | 2016.02.03 - YRS-AT-2247 - Need bitflag that will not allow a participant to create a web account. This feature is turned on in 17.0.0--%>
                                                                        </tr>
                                                                        <%--  <td align="left" width="32%">
                                                                    <asp:Label ID="LabelPriority" runat="server" CssClass="Label_Small">Priority Handling</asp:Label>
                                                                    <asp:CheckBox ID="CheckboxPriority" runat="server" AutoPostBack="True" Style="margin: 0px 0px -4px -4px;">
                                                                    </asp:CheckBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </td>
                                                                <td align="left" width="45%">
                                                                    <asp:Label ID="lblPersonalInfoSharingOptOut" runat="server" CssClass="Label_Small">Personal Info Sharing Opt-out</asp:Label>
                                                                    <asp:CheckBox runat="server" ID="chkPersonalInfoSharingOptOut" AutoPostBack="true"
                                                                        Style="margin: 0px 0px -4px -4px;"></asp:CheckBox>
                                                                </td>
                                                                <td align="right" width="23%">
                                                                    <asp:Label ID="lblGoPaperless" runat="server" CssClass="Label_Small">Go Paperless</asp:Label>
                                                                    <asp:CheckBox runat="server" ID="chkGoPaperless" AutoPostBack="true" Style="margin: 0px 0px -4px -4px;">
                                                                    </asp:CheckBox>
                                                                </td>--%>
                                                                </tr>
                                                                <tr>
                                                                    <td>
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


                                                                                <asp:Label ID="Label6" CssClass="Label_Small" runat="server" Text=""></asp:Label>
                                                                            </div>

                                                                            <asp:Label ID="lblUserid" CssClass="Label_Small" runat="server" Text=""></asp:Label>
                                                                            <asp:Label ID="lblName" CssClass="Label_Small" runat="server" Text=""></asp:Label>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" colspan="6">
                                                                        <div id="divPIN" runat="server" style="display: none;">
                                                                            <div id="divMessage" style="width: 88%;"></div>
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
                                                                <%--   <tr>
                                                            <td align="left">
                                                                	
                                                                </td>
                                                           <td colspan="3"></td>

                                                            </tr>--%>
                                                                <tr>
                                                                    <td align="left" width="120">
                                                                        <asp:Label Visible="False" ID="lblShareInfoAllowed" runat="server" CssClass="Label_Small">Sharing of Contact Info Allowed </asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:CheckBox ID="chkShareInfoAllowed" Visible="False" runat="server" CssClass="Warn"
                                                                            AutoPostBack="True"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4">
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
                                                                                                <asp:Button ID="ButtonEditAddress" runat="server" CssClass="Button_Normal" Text="Edit"
                                                                                                    Width="73px" CausesValidation="False" Visible="false"></asp:Button>
                                                                                            </td>
                                                                                            <td class="td_Text" align="right" style="cursor: pointer;" width="0%" onclick="TogglePrimaryDiv()">
                                                                                                <img src="~/images/collapse.GIF" id="imgPrimary" runat="server" />
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
                                                                                                <td align="left" valign="top" width="50%" class="Table_WithBorder">
                                                                                                    <%--BS:2012.05.10:yrs:1470:enhancement of user control for person maintenance--%>
                                                                                                    <%--  <YRSControls:AddressWebUserControl ID="AddressWebUserControl1" runat="server" AllowNote="true" AllowEffDate="true"></YRSControls:AddressWebUserControl>--%>
                                                                                                    <%--<YRSControls:Enhance_AddressWebUserControl ID="AddressWebUserControl1" runat="server"
                                                                        AllowNote="true" AllowEffDate="true" />--%>
                                                                                                    <NewYRSControls:New_AddressWebUserControl ID="AddressWebUserControl1" runat="server"
                                                                                                        PopupHeight="530" AllowNote="true" AllowEffDate="true" />
                                                                                                </td>
                                                                                                <td colspan="2" width="50%" valign="top" align="left" class="Table_WithBorder" style="font-weight: normal;">
                                                                                                    <table width="100%">
                                                                                                        <tr valign="top">
                                                                                                            <td align="left">
                                                                                                                <YRSTelephoneControls:Telephone_WebUserControl ID="TelephoneWebUserControl1" runat="server" />
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <%--<input id="btnEditPrimaryContact" type="button" runat="server" tabindex="50" onclick="showDialog('DivEditContacts');"
                                                                                        value="Edit Contacts" style="width: 100px" />--%>
                                                                                                                <asp:ImageButton ID="btnEditPrimaryContact" ImageUrl="images/Contacts_Brown.bmp"
                                                                                                                    AlternateText="Add/Edit Phone/Email" Width="18" Height="18" runat="server" OnClientClick="javascript: return ShowPIIErrorMessage();" /> <%--MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="right">
                                                                                                                <table border="0" width="100%" id="tbPricontacts" runat="server">
                                                                                                                    <tr style="height: 120">
                                                                                                                        <td align="left" width="5%">
                                                                                                                            <asp:Label ID="LabelTelephone" runat="server" CssClass="Label_Small">Office</asp:Label>
                                                                                                                        </td>
                                                                                                                        <td align="left" width="10%">
                                                                                                                            <asp:Label runat="server" ID="lblOfficeContact"></asp:Label>
                                                                                                                            <asp:TextBox ID="TextBoxTelephone" runat="server" CssClass="TextBox_Normal Warn"
                                                                                                                                Width="95%" MaxLength="10"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="left" width="5%">
                                                                                                                            <asp:Label ID="LabelHomeTelephone" runat="server" CssClass="Label_Small">Home</asp:Label>
                                                                                                                        </td>
                                                                                                                        <td align="left" width="10%">
                                                                                                                            <asp:Label runat="server" ID="lblHomeContact"></asp:Label>
                                                                                                                            <asp:TextBox ID="TextBoxHome" runat="server" CssClass="TextBox_Normal Warn" Width="95%"
                                                                                                                                MaxLength="10"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr style="height: 120">
                                                                                                                        <td align="left" width="5%">
                                                                                                                            <asp:Label ID="LabelMobile" runat="server" CssClass="Label_Small">Mobile</asp:Label>
                                                                                                                        </td>
                                                                                                                        <td align="left" width="10%">
                                                                                                                            <asp:Label runat="server" ID="lblMobile"></asp:Label>
                                                                                                                            <asp:TextBox ID="TextBoxMobile" runat="server" CssClass="TextBox_Normal Warn" Width="95%"
                                                                                                                                MaxLength="10"> </asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="left" width="5%">
                                                                                                                            <asp:Label ID="LabelFax" runat="server" CssClass="Label_Small">Fax</asp:Label>
                                                                                                                        </td>
                                                                                                                        <td align="left" width="10%">
                                                                                                                            <asp:Label runat="server" ID="lblFax"></asp:Label>
                                                                                                                            <asp:TextBox ID="TextBoxFax" runat="server" CssClass="TextBox_Normal Warn" Width="95%"
                                                                                                                                MaxLength="10"></asp:TextBox>
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
                                                                                        Text="Deactivate" Width="80px" CausesValidation="true" align="right" OnClientClick="javascript: return ShowPIIErrorMessage();"></asp:Button> <%--MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                                </td>
                                                                                 <%--End:Manthan Rajguru | 2016.02.24 | YRS-AT-2328 | Added Deactivate button control for Secondary address--%>
                                                                                            <td align="right" width="15%" class="td_Text">
                                                                                                <asp:Button ID="ButtonActivateAsPrimary" runat="server" CssClass="Button_Normal"
                                                                                                    Text="Activate as Primary" Width="140px" CausesValidation="true" align="right" OnClientClick="javascript: return ShowPIIErrorMessage();"></asp:Button> <%--MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
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
                                                                                                <td align="left" valign="top" width="50%" class="Table_WithBorder">
                                                                                                    <%--BS:2012.05.10:yrs:1470:enhancement of user control for person maintenance--%>
                                                                                                    <%-- <YRSControls:AddressWebUserControl ID="AddressWebUserControl2" runat="server" AllowNote="true" AllowEffDate="true"></YRSControls:AddressWebUserControl>--%>
                                                                                                    <%--<YRSControls:Enhance_AddressWebUserControl ID="AddressWebUserControl2" runat="server"
                                                                        AllowNote="true" AllowEffDate="true" />--%>
                                                                                                    <NewYRSControls:New_AddressWebUserControl ID="AddressWebUserControl2" runat="server"
                                                                                                        PopupHeight="530" AllowNote="true" AllowEffDate="true" />
                                                                                                </td>
                                                                                                <td colspan="2" width="50%" valign="top" align="left" class="Table_WithBorder" style="font-weight: normal;">
                                                                                                    <table width="100%">
                                                                                                        <tr valign="top">
                                                                                                            <td align="left">
                                                                                                                <YRSTelephoneControls:Telephone_WebUserControl ID="TelephoneWebUserControl2" runat="server" />
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <%--<input id="btnEditSecondaryContact" type="button" runat="server" tabindex="50" onclick="showDialog('DivEditContacts');"
                                                                                    value="Edit Contacts" style="width: 100px" />--%>
                                                                                                                <asp:ImageButton ID="btnEditSecondaryContact" ImageUrl="images/Contacts_Brown.bmp"
                                                                                                                    AlternateText="Add/Edit Phone/Email" Width="18" Height="18" runat="server" OnClientClick="javascript: return ShowPIIErrorMessage();" /> <%--MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="right">
                                                                                                                <table border="0" width="100%" id="tbSeccontacts" runat="server">
                                                                                                                    <tr>
                                                                                                                        <td align="left" width="5%">
                                                                                                                            <asp:Label ID="LabelSecTelephone" runat="server" CssClass="Label_Small">Office</asp:Label>
                                                                                                                        </td>
                                                                                                                        <td align="left" width="10%">
                                                                                                                            <asp:Label runat="server" ID="lblSecoffice"></asp:Label>
                                                                                                                            <asp:TextBox ID="TextBoxSecTelephone" runat="server" CssClass="TextBox_Normal Warn"
                                                                                                                                MaxLength="10" Width="95%"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="left" width="5%">
                                                                                                                            <asp:Label ID="LabelSecHome" runat="server" CssClass="Label_Small">Home</asp:Label>
                                                                                                                        </td>
                                                                                                                        <td align="left" width="10%">
                                                                                                                            <asp:Label runat="server" ID="lblSecHome"></asp:Label>
                                                                                                                            <asp:TextBox ID="TextBoxSecHome" runat="server" CssClass="TextBox_Normal Warn" MaxLength="10"
                                                                                                                                Width="95%"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="left" width="5%">
                                                                                                                            <asp:Label ID="LabelSecMobile" runat="server" CssClass="Label_Small">Mobile</asp:Label>
                                                                                                                        </td>
                                                                                                                        <td align="left" width="10%">
                                                                                                                            <asp:Label runat="server" ID="lblSecMobile"></asp:Label>
                                                                                                                            <asp:TextBox ID="TextBoxSecMobile" runat="server" CssClass="TextBox_Normal Warn"
                                                                                                                                MaxLength="10" Width="95%"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="left" width="5%">
                                                                                                                            <asp:Label ID="LabelSecFax" runat="server" CssClass="Label_Small">Fax</asp:Label>
                                                                                                                        </td>
                                                                                                                        <td align="left" width="10%">
                                                                                                                            <asp:Label runat="server" ID="lblSecFax"></asp:Label>
                                                                                                                            <asp:TextBox ID="TextBoxSecFax" runat="server" CssClass="TextBox_Normal Warn" MaxLength="10"
                                                                                                                                Width="95%"></asp:TextBox>
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
                                                                                <td colspan="3" align="left">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td align="left">
                                                                                                <asp:Label ID="LabelEmailId" runat="server" CssClass="Label_Small">Email Id</asp:Label>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <asp:TextBox ID="TextBoxEmailId" runat="server" CssClass="TextBox_Normal Warn" Width="230"
                                                                                                    ReadOnly="true" MaxLength="70" Enabled="false"></asp:TextBox>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:CheckBox ID="CheckboxBadEmail" runat="server" Width="100" Text="Bad Email" TextAlign="Left"
                                                                                                    CssClass="CheckBox_Normal Warn" Enabled="false"></asp:CheckBox>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:CheckBox ID="CheckboxUnsubscribe" runat="server" Width="100" Text="Unsubscribe"
                                                                                                    TextAlign="Left" CssClass="CheckBox_Normal Warn" Enabled="false"></asp:CheckBox>
                                                                                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="CheckboxTextOnly" runat="server" Width="90"
                                                                                                    Text="Text Only" TextAlign="Left" CssClass="CheckBox_Normal Warn" Enabled="false"></asp:CheckBox>
                                                                                            </td>
                                                                                            <td align="right" width="141">
                                                                                                <asp:ImageButton ID="imgBtnEmail" ImageUrl="images/Contacts_Brown.bmp" AlternateText="Add/Edit Phone/Email"
                                                                                                    Width="18" Height="18" runat="server" OnClientClick="javascript: return ShowPIIErrorMessage();"/> <%--MMR | 2018.11.23 | YRS-AT-4018 | Added to show error message if not allowed to update PII Information--%>
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
                                                                <tr>
                                                                    <td colspan="2" align="left"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="left"></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                        <td class="Table_WithBorder" colspan="2" align="left">
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="LabelEmailId" runat="server" CssClass="Label_Small">Email Id</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="TextboxEmailId" runat="server" CssClass="TextBox_Normal Warn" Width="230"
                                                            ReadOnly="true" MaxLength="70"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td align="right">
                                                        <asp:CheckBox ID="CheckboxBadEmail" runat="server" Width="70" Text="Bad Email" TextAlign="Left"
                                                            CssClass="CheckBox_Normal Warn" Enabled="false"></asp:CheckBox>
                                                    </td>
                                                    <td align="right">
                                                        <asp:CheckBox ID="CheckboxUnsubscribe" runat="server" Width="70" Text="Unsubscribe"
                                                            TextAlign="Left" CssClass="CheckBox_Normal Warn" Enabled="false"></asp:CheckBox>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="CheckboxTextOnly" runat="server" Width="70"
                                                            Text="Text Only" TextAlign="Left" CssClass="CheckBox_Normal Warn" Enabled="false">
                                                        </asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>--%>
                                    </table>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <div class="Div_Left">
                                        <table width="100%" height="370" class="Table_WithBorder" cellspacing="0">
                                            <tr>
                                                <td align="left" class="td_Text">Annuities
                                                </td>
                                                <td align="right" class="td_Text">
                                                    <asp:Button runat="server" ID="ButtonAnnuitiesViewDetails" Width="96px" Text="View Details"
                                                        CssClass="Button_Normal" Enabled="true" CausesValidation="False" Visible="false"></asp:Button>&nbsp;<!--YRPS-4659-->
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" valign="top">
                                                    <div style="overflow: auto; width: 98%; height: 140px; text-align: left">
                                                        <asp:DataGrid ID="DataGridAnnuities" runat="server" Width="98%" CssClass="DataGrid_Grid"
                                                            AutoGenerateColumns="false">
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                            <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImagebuttonView" runat="server" ImageUrl="~/images/view.gif"
                                                                            CausesValidation="False" CommandName="View" ToolTip="View"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn HeaderText="Annuity Source" DataField="Annuity Source" />
                                                                <asp:BoundColumn HeaderText="Plan Type" DataField="PlanType" />
                                                                <asp:BoundColumn HeaderText="Annuity Type" DataField="Annuity Type" />
                                                                <asp:BoundColumn HeaderText="Purchase Date" DataField="Purchase Date" />
                                                                <asp:BoundColumn HeaderText="Current Payment" DataField="Current Payment" />
                                                                <asp:BoundColumn HeaderText="Social Security Adj" DataField="Social Security Adj" />
                                                                <asp:BoundColumn HeaderText="Death Benefit" DataField="Death Benefit" />
                                                                <asp:BoundColumn HeaderText="ID" DataField="ID" Visible="false" />
                                                                <asp:BoundColumn HeaderText="AnnuityJointSurvivorsID" DataField="AnnuityJointSurvivorsID"
                                                                    Visible="False" />
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>--%>
                                            <tr>
                                                <td align="left" class="td_Text">Annuity Beneficiary
                                                </td>
                                                <td class="td_Text" align="right">
                                                    <asp:Button ID="ButtonEditJSBeneficiary" runat="server" CssClass="Button_Normal"
                                                        Width="73" Text="Edit" CausesValidation="True"></asp:Button>
                                                    <asp:Button ID="ButtonUpdateJSBeneficiary" runat="server" CssClass="Button_Normal"
                                                        Enabled="False" Text="Update" Width="73" CausesValidation="True"></asp:Button>
                                                    <asp:Button ID="ButtonCancelJSBeneficiary" runat="server" CssClass="Button_Normal"
                                                        Enabled="False" Text="Cancel" Width="73" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <table class="Table_WithOutBorder" border="0" width="100%">
                                                        <tr valign="top">
                                                            <td align="left" width="45%"><%-- // SB | 07/07/2016 | YRS-AT-2382 | Width increased as new Edit button is added --%>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td valign="top" width="55" align="left">
                                                                            <asp:Label ID="LabelAnnuitiesSSNo" runat="server" CssClass="Label_Small">SS No.</asp:Label>
                                                                        </td>
                                                                        <td valign="top" align="left">
                                                                            <asp:TextBox ID="TextBoxAnnuitiesSSNo" runat="server" CssClass="TextBox_Normal Warn"
                                                                                MaxLength="9" Enabled="false"></asp:TextBox>
                                                                            <%-- // START : SB | 07/07/2016 | YRS-AT-2382 | Edit SSN button is added with link to view SSN Updatss --%>
                                                                            <asp:Button ID="ButtonAnnuitiesSSNoEdit" runat="server" Text="Edit" CssClass="Button_Normal"
                                                                            Width="73px" CausesValidation="False" Enabled="true"></asp:Button>
                                                                            &nbsp;<asp:LinkButton ID="lnkbtnViewAnnuitiesSSNUpdate" Text="View SSN Updates" OnClientClick="javascript:OpenViewAnnuityBenifSSNUpdatesWindow(); return false;" runat="server"></asp:LinkButton>
                                                                            <%-- // END : SB | 07/07/2016 | YRS-AT-2382 | Edit SSN button is added with link to view SSN Updatss --%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" align="left">
                                                                            <asp:Label ID="LabelAnnuitiesFirstName" runat="server" CssClass="Label_Small">First</asp:Label>
                                                                        </td>
                                                                        <td colspan="2" valign="top" align="left">
                                                                            <asp:TextBox runat="server" ID="TextBoxAnnuitiesFirstName" CssClass="TextBox_Normal Warn"
                                                                                MaxLength="30" Enabled="false"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" align="left">
                                                                            <asp:Label ID="LabelAnnuitiesMiddleName" runat="server" CssClass="Label_Small">Middle</asp:Label>
                                                                        </td>
                                                                        <td colspan="2" valign="top" align="left">
                                                                            <asp:TextBox runat="server" ID="TextBoxAnnuitiesMiddleName" CssClass="TextBox_Normal Warn"
                                                                                Width="90" MaxLength="20" Enabled="false"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" align="left">
                                                                            <asp:Label ID="LabelAnnuitiesLastName" runat="server" CssClass="Label_Small Warn">Last</asp:Label>
                                                                        </td>
                                                                        <td colspan="2" valign="top" align="left">
                                                                            <asp:TextBox runat="server" ID="TextBoxAnnuitiesLastName" CssClass="TextBox_Normal Warn"
                                                                                MaxLength="30" Enabled="false"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" align="left">
                                                                            <asp:Label ID="LabelAnnuitiesDOB" runat="server" CssClass="Label_Small">DOB</asp:Label>
                                                                        </td>
                                                                        <td colspan="2" valign="top" align="left">
                                                                            <asp:TextBox runat="server" ID="TextBoxAnnuitiesDOB" CssClass="TextBox_Normal DateControl"
                                                                                Width="90" Enabled="false"></asp:TextBox>
                                                                            <rjs:PopCalendar ID="PopcalendarDOB" runat="server" Separator="/" Format="mm dd yyyy"
                                                                                ScriptsValidators="IsValidDate" Control="TextBoxAnnuitiesDOB" Enabled="false"></rjs:PopCalendar>
                                                                            <asp:CustomValidator ID="Customvalidator1" runat="server" ClientValidationFunction="IsValidDate"
                                                                                ControlToValidate="TextBoxAnnuitiesDOB" Display="Dynamic">*</asp:CustomValidator>
                                                                            <asp:RequiredFieldValidator CssClass="Label_Small" ID="ReqValidatorAnnuityDOB" runat="server"
                                                                                Enabled="False" ControlToValidate="TextBoxAnnuitiesDOB" ErrorMessage="DOB required (Annuity Beneficiary)"
                                                                                Display="Dynamic">*</asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="left" width="55%">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td width="100%">
                                                                            <table width="100%" align="left">
                                                                                <tr>
                                                                                    <td align="left" width="135">
                                                                                        <asp:Label ID="LabelDateDeceased" runat="server" CssClass="Label_Small">Date Deceased</asp:Label>
                                                                                    </td>
                                                                                    <td valign="top" align="left">
                                                                                        <asp:TextBox runat="server" ID="TextBoxDateDeceased" Width="90" CssClass="TextBox_Normal DateControl"
                                                                                            Enabled="false"></asp:TextBox>
                                                                                        <rjs:PopCalendar ID="PopcalendarDeceased" Enabled="false" runat="server" Separator="/"
                                                                                            Format="mm dd yyyy" ScriptsValidators="IsValidDate" Control="TextBoxDateDeceased"></rjs:PopCalendar>
                                                                                        <asp:CustomValidator ID="Customvalidator2" runat="server" ClientValidationFunction="IsValidDate"
                                                                                            ControlToValidate="TextBoxDateDeceased" Display="Dynamic">*</asp:CustomValidator>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td valign="top" align="left">
                                                                                        <asp:Label ID="Label4" runat="server" CssClass="Label_Small">Spouse</asp:Label>
                                                                                    </td>
                                                                                    <td valign="top" align="left">
                                                                                        <asp:CheckBox ID="CheckboxSpouse" AutoPostBack="False" runat="server" CssClass="Warn"
                                                                                            Enabled="False"></asp:CheckBox>&nbsp;&nbsp;&nbsp;
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="100%">
                                                                            <table width="100%" class="Table_WithBorder">
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <asp:LinkButton ID="lnkParticipantAddress" runat="server" CssClass="Link_Small" Text="Use Participant Address"></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <NewYRSControls:New_AddressWebUserControl ID="AddressWebUserControlAnn" AddressFor="BENE" runat="server"
                                                                                            PopupHeight="530" AllowNote="true" AllowEffDate="true" ViewStateMode="Enabled" />
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
                                    </div>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <div class="Div_Left">
                                        <table width="100%" height="370" class="Table_WithBorder" border="0" cellspacing="0">
                                            <tr>
                                                <td align="left" class="td_Text" height="5%">Beneficiaries
                                                <asp:Image ID="imgLockBeneficiary" runat="server" ImageUrl="Images/lock-yellow.png"
                                                    Visible="false" Width="20px" Height="20px" />
                                                </td>
                                                <td align="right" class="td_Text" nowrap height="5%">
                                                    <asp:Button ID="ButtonAddRetired" runat="server" Text="Add..." Width="73px" CssClass="Button_Normal"
                                                        CausesValidation="False"></asp:Button>
                                                    <asp:Button ID="ButtonEditRetired" runat="server" Text="Edit" Width="73px" Visible="false"
                                                        CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                    <asp:Button ID="ButtonDeleteRetired" runat="server" Text="Delete" Width="73px" CssClass="Button_Normal"
                                                        Visible="false" CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top" colspan="2">
                                                    <table id="Table1" width="100%" border="0" cellspacing="0">
                                                        <tr>
                                                            <td valign="top">
                                                                <div style="overflow: auto; width: 100%; height: 170px;">
                                                                    <asp:DataGrid ID="DataGridRetiredBeneficiaries" runat="server" Width="98%" CssClass="DataGrid_Grid" AutoGenerateColumns="false">
                                                                        <%-- // SB | 07/07/2016 | YRS-AT-2382 | Turned off auto generate columns--%>
                                                                        <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                                        <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                                        <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                                        <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                                        <Columns>
                                                                            <%--<asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="Imagebutton7" runat="server" ToolTip="Select" CommandName="Select"
                                                                CausesValidation="False" ImageUrl="images\select.gif" AlternateText="Select">
                                                            </asp:ImageButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>--%>
                                                                            <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImagebuttonEdit" runat="server" ToolTip="Edit" CommandName="Edit"
                                                                                        CausesValidation="False" ImageUrl="~/images/edits.gif" AlternateText="Edit"></asp:ImageButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="ImagebuttonDelete" runat="server" ToolTip="Delete" CommandName="Delete"
                                                                                        CausesValidation="False" ImageUrl="~/images/delete.gif" AlternateText="Delete"></asp:ImageButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>

                                                                            <%--  // START : SB | 07/07/2016 | YRS-AT-2382 | Columns are now binded here manually--%>
                                                                            <asp:BoundColumn HeaderText="UniqueId" DataField="UniqueId" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="PersId" DataField="PersId" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="BenePersId" DataField="BenePersId" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="BeneFundEventId" DataField="BeneFundEventId" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="Name" DataField="Name" SortExpression="Name" ItemStyle-Width ="14%"  /> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Name2" DataField="Name2" SortExpression="Name2" ItemStyle-Width ="14%"/> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                         
                                                                            <asp:TemplateColumn HeaderText="TaxID" ItemStyle-Width ="10%"> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                                <ItemTemplate>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblSSNo" runat="server" Text='<%# Bind("TaxID")%>'></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:HyperLink runat="server" NavigateUrl='<%# Eval("UniqueId", "~/UpdateSSN.aspx?Name=retireAndBeneficiary&Mode=ViewSSN&BenefID={0}")%>' Target="_blank" ID="hypViewSSN"
                                                                                                    onclick="javascript: OpenViewSSNUpdatesWindowAnnuities(this.href); return false;" >
                                                                                                    <span class="ui-button-icon-primary ui-icon ui-icon-clock" style="float: left; cursor: pointer" title="View SSN Updates"/>
                                                                                                </asp:HyperLink>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                        
                                                                            <asp:BoundColumn HeaderText="Rel" DataField="Rel" Visible="true" ItemStyle-Width ="4%"/>
                                                                            <%--Start - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                                            <%--<asp:BoundColumn HeaderText="Birthdate" DataField="Birthdate" />--%>                                                                           
                                                                            <asp:BoundColumn HeaderText="Birth/Estd. Date" DataField="Birthdate" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width ="11%"/>
                                                                            <%--End - Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Commented Existing code and changed header text for birth date--%>
                                                                            <asp:BoundColumn HeaderText="Groups" DataField="Groups" ItemStyle-Width = "5%" /> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Lvl" DataField="Lvl" ItemStyle-Width = "4%"/>
                                                                            <asp:BoundColumn HeaderText="DeathFundEventStatus" DataField="DeathFundEventStatus" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="BeneficiaryStatusCode" DataField="BeneficiaryStatusCode" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="Pct" DataField="Pct" ItemStyle-HorizontalAlign="Right" ItemStyle-Width = "6%" /> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Type" DataField="BeneficiaryTypeCode" ItemStyle-Width ="6%" /> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Updated On" DataField="Updated On" ItemStyle-Width ="14%" /> <%--Manthan Rajguru | 2016.07.18 | YRS-AT-2919 | Changed the width of grid column --%>
                                                                            <asp:BoundColumn HeaderText="Updated By " DataField="Updated By" ItemStyle-Width ="12%"/>
                                                                            <asp:BoundColumn HeaderText="NewId" DataField="NewId" Visible="false" />
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
                                                            <td valign="top">
                                                                <table id="Table2" width="35%" border="0">
                                                                    <tr>
                                                                        <td colspan="7" align="center">
                                                                            <asp:Label ID="LabelPercentage2" runat="server" CssClass="Label_Small">Percentage</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center">
                                                                            <asp:Label ID="LabelRetiredDBR" runat="server" CssClass="Label_Small">Retired DB %</asp:Label>
                                                                        </td>
                                                                        <td colspan="3" align="center">
                                                                            <asp:Label ID="LabelInsResR" runat="server" CssClass="Label_Small">Ins Res %</asp:Label>
                                                                        </td>
                                                                        <td colspan="2" align="center">
                                                                            <asp:Label ID="LabelInsSavR" runat="server" CssClass="Label_Small">Ins Sav %</asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <%--<tr>
                                                <td colspan="2" align="center">
                                                    <asp:Label ID="Label6" runat="server" CssClass="Label_Small">%</asp:Label>
                                                </td>
                                                <td colspan="3" align="center">
                                                    <asp:Label ID="Label10" runat="server" CssClass="Label_Small">%</asp:Label>
                                                </td>
                                                <td colspan="2" align="center">
                                                    <asp:Label ID="Label11" runat="server" CssClass="Label_Small">%</asp:Label>
                                                </td>
                                            </tr>--%>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="LabelPrimaryR" runat="server" CssClass="Label_Small">Primary</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxPrimaryR" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonPriR" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxPrimaryInsR" runat="server" Width="50px" Enabled="false"
                                                                                CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonPriInsR" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextboxPrimaryInssavR" runat="server" Width="50px" Enabled="false"
                                                                                CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonPriInssavR" runat="server" Text="E" CssClass="Button_Normal"
                                                                                CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="LabelCont1R" runat="server" CssClass="Label_Small">Cont1</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxCont1R" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonCont1R" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxCont1InsR" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonCont1InsR" runat="server" Text="E" CssClass="Button_Normal"
                                                                                CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxCont1InssavR" runat="server" Width="50px" Enabled="false"
                                                                                CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonCont1InssavR" runat="server" Text="E" CssClass="Button_Normal"
                                                                                CausesValidation="False"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="LabelCont2R" runat="server" CssClass="Label_Small">Cont2</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxCont2R" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonCont2R" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxCont2InsR" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonCont2InsR" runat="server" Text="E" CssClass="Button_Normal"
                                                                                CausesValidation="False" Visible="false"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxCont2InssavR" runat="server" Width="50px" Enabled="false"
                                                                                CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonCont2InssavR" runat="server" Text="E" CssClass="Button_Normal"
                                                                                CausesValidation="False" Visible="false"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="LabelCont3R" runat="server" CssClass="Label_Small">Cont3</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxCont3R" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonCont3R" runat="server" Text="E" CssClass="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxCont3InsR" runat="server" Width="50px" Enabled="false" CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonCont3InsR" runat="server" Text="E" CssClass="Button_Normal"
                                                                                CausesValidation="False" Visible="false"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxCont3InssavR" runat="server" Width="50px" Enabled="false"
                                                                                CssClass="TextBox_Normal Warn"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ButtonCont3InssavR" runat="server" Text="E" CssClass="Button_Normal"
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
                                    </div>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <div class="Div_Center">
                                        <table class="Table_WithBorder" width="100%" cellspacing="0" border="0" height="340">
                                            <tr>
                                                <td align="left" class="td_Text">Annuities Paid
                                                <asp:Label Visible="false" ID="LabelAnnuitiesHdr" runat="server" CssClass="td_Text"></asp:Label>
                                                </td>
                                            </tr>
                                            <!--SR:2011.03.15-Height changed to 100% from 330px so that Datagrid should not go down.-->
                                            <tr valign="top">
                                                <td align="left" valign="top">
                                                    <div style="overflow: scroll; width: 100%; height: 345px; text-align: left; vertical-align: top">
                                                        <asp:DataGrid ID="DataGridAnnuitiesPaid" runat="server" align="center" CssClass="DataGrid_Grid"
                                                            AllowSorting="True" Width="95%">
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImageButtonView" runat="server" ImageUrl="images\view.gif" CausesValidation="False"
                                                                            CommandName="View" ToolTip="View" OnClientClick="javascript: ShowTaxWithHoldingDetailsDialog(this); return false;" ></asp:ImageButton>
                                                                        <%--START: SN | 11/14/2019 | YRS-AT-4604 | YRS enh: State Withholding Project - Annuities Paid--%>
                                                                         <asp:HiddenField ID ="hdndisbursementId" runat="server" Value='<%#Eval("disbursementId")%>'></asp:HiddenField>
                                                                        <%--END: SN | 11/14/2019 | YRS-AT-4604 | YRS enh: State Withholding Project - Annuities Paid--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <div class="Div_Center">
                                        <table class="Table_WithBorder" width="100%" cellspacing="0" border="0" height="370">
                                            <tr>
                                                <td align="left" class="td_Text" valign="top">Banking Information
                                                </td>
                                                <td align="right" class="Td_ButtonContainer">
                                                    <asp:Button ID="ButtonBankingInfoCheckPayment" runat="server" Width="180px" Text="Change To Check Payment"
                                                        CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                    <asp:Button ID="ButtonBankingInfoUpdate" runat="server" Text="View Details" CssClass="Button_Normal"
                                                        Enabled="False" CausesValidation="False" Visible="False"></asp:Button>
                                                    <asp:Button ID="ButtonBankingInfoAdd" runat="server" Width="90px" Text="Add..." CssClass="Button_Normal"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>&nbsp;&nbsp;<asp:Label ID="LabelCurrentEFT" runat="server" Width="180px" CssClass="Label_Small"></asp:Label>&nbsp;
                                                            <asp:Label ID="LabelCurrency" runat="server" Width="180px" CssClass="Label_Small"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top" colspan="2">
                                                    <div style="overflow: auto; width: 100%; height: 310px; text-align: left">
                                                        <asp:DataGrid ID="DataGridBankInfoList" runat="server" Width="98%" align="center"
                                                            CssClass="DataGrid_Grid" AutoGenerateColumns="false">
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                            <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImagebuttonEdit" runat="server" ImageUrl="images\View.gif" CausesValidation="False"
                                                                            CommandName="View" ToolTip="View"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn HeaderText="BankName" DataField="BankName" />
                                                                <asp:BoundColumn HeaderText="BankABA#" DataField="BankABA#" />
                                                                <asp:BoundColumn HeaderText="Account#" DataField="AccountNo" />
                                                                <asp:BoundColumn HeaderText="Effec. Date" DataField="EffecDate" DataFormatString="{0:MM/dd/yyyy}" />
                                                                <asp:BoundColumn HeaderText="PaymentDesc" DataField="PaymentDesc" Visible="false" />
                                                                <asp:BoundColumn HeaderText="EFTDesc" DataField="EFTDesc" Visible="false" />
                                                                <asp:BoundColumn HeaderText="BankID" DataField="BankID" Visible="false" />
                                                                <asp:BoundColumn HeaderText="guiUniqueId" DataField="UniqueID" Visible="false" />
                                                                <asp:BoundColumn HeaderText="chrEFTStatus" DataField="chrEFTStatus" Visible="false" />
                                                                <asp:BoundColumn HeaderText="dtmEffDate" DataField="dtmEffDate" Visible="false" />
                                                                <asp:BoundColumn HeaderText="AccountType" DataField="AccountType" />
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <div class="Div_Center">
                                        <table class="Table_WithBorder" width="100%" height="370" cellspacing="0">
                                            <tr>
                                                <td align="left" class="td_Text">Federal Withholding
                                                </td>
                                                <td align="right" class="Td_ButtonContainer">
                                                    <asp:Button ID="ButtonFederalWithholdAdd" runat="server" Width="90px" Text="Add..."
                                                        CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                    <asp:Button ID="ButtonFederalWithholdUpdate" runat="server" Width="90px" Text="Update Item"
                                                        CssClass="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td align="center" colspan="2">
                                                     <%--<div style="overflow: auto; width: 100%; height: 320px; text-align: left">--%>  
                                                    <div style="overflow: auto; width:100%; height: 140px; text-align: left"><%-- ML |20.09.2019| YRS-AT-4598 |change div height   --%>
                                                        <asp:DataGrid ID="DataGridFederalWithholding" runat="server" Width="98%" CssClass="DataGrid_Grid"
                                                            AutoGenerateColumns="false">
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImageDataGridFederalWithholdingEdit" runat="server" ImageUrl="images\edits.gif"
                                                                            CausesValidation="False" CommandName="Edit" ToolTip="Edit"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn HeaderText="Exemptions" DataField="Exemptions" />
                                                                <asp:BoundColumn HeaderText="Add'l Amount" DataField="Add'l Amount" />
                                                                <asp:BoundColumn HeaderText="Type" DataField="Type" />
                                                                <asp:BoundColumn HeaderText="Tax Entity" DataField="Tax Entity" />
                                                                <asp:BoundColumn HeaderText="Marital Status" DataField="Marital Status" />
                                                                <asp:BoundColumn HeaderText="ID" DataField="FedWithdrawalID" Visible="false" />
                                                            </Columns>
                                                            <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                             <%--START : ML |20.09.2019| YRS-AT-4598 |State withholding user control added  --%>
                                           <tr>    
                                                <%--DO NOT CHANGE PreFixID value as it is used to Manage UserPermission. Page Wise PrefixID will be different--%>                                          
                                                <YRSStateTaxControls:StateWithholdingListing_WebUserControl  ID ="stwListUserControl"   PreFixID="retireeInfo" ClientIDMode ="Static" runat="server" />
                                            </tr>
                                             <%--END : ML |20.09.2019| YRS-AT-4598 |State withholding user control added  --%>
                                        </table>
                                    </div>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <div class="Div_Center">
                                        <table class="Table_WithBorder" width="100%" cellspacing="0">
                                            <tr>
                                                <td align="left" class="td_Text">General Withholding
                                                </td>
                                                <td align="right" class="Td_ButtonContainer">
                                                    <asp:Button ID="ButtonGeneralWithholdAdd" runat="server" Width="90px" Text="Add..."
                                                        CssClass="Button_Normal" CausesValidation="False"></asp:Button>
                                                    <asp:Button ID="ButtonGeneralWithholdUpdate" runat="server" Width="90px" Text="Update Item"
                                                        CssClass="Button_Normal" CausesValidation="False" Visible="false"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr valign="top" height="325">
                                                <td align="center" class="Table_WithOutBorder" colspan="2">
                                                    <div style="overflow: auto; width: 100%; height: 320px; text-align: left">
                                                        <asp:DataGrid ID="DataGridGeneralWithhold" runat="server" Width="100%" CssClass="DataGrid_Grid"
                                                            AutoGenerateColumns="false">
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="EditImageGeneralWithHold" runat="server" ImageUrl="images\edits.gif"
                                                                            CausesValidation="False" CommandName="Edit" ToolTip="Edit"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn HeaderText="Type" DataField="Type" />
                                                                <asp:BoundColumn HeaderText="Add'l Amount" DataField="Add'l Amount" />
                                                                <asp:BoundColumn HeaderText="Start Date" DataField="Start Date" />
                                                                <asp:BoundColumn HeaderText="End Date" DataField="End Date" />
                                                                <asp:BoundColumn HeaderText="ID" DataField="GenWithdrawalID" Visible="false" />
                                                            </Columns>
                                                            <SelectedItemStyle CssClass="DataGrid_SelectedStyle"></SelectedItemStyle>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <div class="Div_Center">
                                        <table class="Table_WithBorder" width="100%" cellspacing="0" border="0">
                                            <tr>
                                                <td align="left" class="td_Text">Notes
                                                </td>
                                                <td align="right" class="Td_ButtonContainer">
                                                    <asp:Button ID="ButtonNotesAdd" runat="server" Width="90px" Text="Add..." CssClass="Button_Normal"
                                                        CausesValidation="False"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr valign="top" height="325">
                                                <td align="center" class="Table_WithOutBorder" colspan="2">
                                                    <div style="overflow: auto; width: 100%; height: 320px; text-align: left">
                                                        <asp:DataGrid ID="DataGridNotes" runat="server" CssClass="DataGrid_Grid" Width="98%"
                                                            AutoGenerateColumns="false" AllowSorting="true">
                                                            <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                                            <AlternatingItemStyle CssClass="DataGrid_AlternateStyle"></AlternatingItemStyle>
                                                            <ItemStyle CssClass="DataGrid_NormalStyle"></ItemStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderStyle-Width="2%">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImageViewNotes" runat="server" ImageUrl="images\view.gif" CausesValidation="False"
                                                                            CommandName="View" ToolTip="View"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn HeaderText="Date" DataField="Date" SortExpression="Date" DataFormatString="{0:MM/dd/yyyy}"
                                                                    ItemStyle-Width="8" />
                                                                <asp:BoundColumn HeaderText="Creator" DataField="Creator" SortExpression="Creator"
                                                                    ItemStyle-Width="12%" />
                                                                <asp:BoundColumn HeaderText="First Line Of Notes" DataField="Note" />
                                                                <asp:BoundColumn HeaderText="UniqueId" DataField="UniqueID" Visible="false" />
                                                                <asp:TemplateColumn HeaderText="Mark As Important" HeaderStyle-Width="12%">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="CheckBoxImportant" runat="server" AutoPostBack="True" OnCheckedChanged="Check_Clicked"
                                                                            Enabled="False" CssClass="Warn" Checked='<%# Databinder.Eval(Container.DataItem, "bitImportant") %>'></asp:CheckBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <%--Start: Bala: 12/01/2016: YRS-AT-1718: Added Delete button--%>
                                                                <asp:TemplateColumn HeaderText="Delete" ItemStyle-Width="10%" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="DeleteNotes" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <%--End: Bala: 12/01/2016: YRS-AT-1718: Added Delete button --%>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </iewc:PageView>
                                <iewc:PageView>
                                    <table class="Table_WithBorder" width="100%" height="370" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <table width="100%" class="Table_WithOutBorder">
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
                                                    <tr height="325">
                                                        <td align="left">
                                                            <%--<asp:LinkButton ID="LinkButtonIDM" runat="server">View IDM</asp:LinkButton>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </iewc:PageView>
                            </iewc:MultiPage>
                        </td>
                    </tr>
            </table>
            <%--</iewc:multipage></TD></TR></TBODY></TABLE>--%>
            <table class="Table_WithBorder" id="Table6" cellspacing="0" cellpadding="1" width="980px">
                <tr>
                    <td class="Td_ButtonContainer" align="center" width="25%" height="10">&nbsp;
                                <asp:Button ID="ButtonSaveRetireeParticipants" runat="server" CssClass="Button_Normal"
                                    Width="72px" Enabled="False" Visible="True" name="ButtonRetireesInfoSave" Text="Save"
                                    OnClientClick="return checkTelephoneLength();" CausesValidation="True"></asp:Button>
                    </td>
                    <td class="Td_ButtonContainer" align="center" width="25%" height="10">&nbsp;<asp:Button ID="ButtonRetireesInfoCancel" runat="server" CssClass="Button_Normal"
                        Enabled="False" name="ButtonRetireesInfoCancel" Text="Cancel" CausesValidation="False"
                        Width="74px"></asp:Button>
                    </td>
                    <td class="Td_ButtonContainer" align="center" width="25%" height="10">
                        <asp:Button ID="ButtonRetireesInfoPHR" runat="server" CssClass="Button_Normal" Text="PHR"
                            CausesValidation="False" Width="73px"></asp:Button>
                    </td>
                    <td class="Td_ButtonContainer">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="ButtonRetireesInfoOK" runat="server" CssClass="Button_Normal Warn_Dirty"
                                    Width="80px" Visible="true" Text="Close" CausesValidation="False"></asp:Button>&nbsp;&nbsp;
                    </td>
                </tr>
            </table>

            <input id="NotesFlag" type="hidden" name="NotesFlag" runat="server" cssclass="Warn">
            <input id="EmailId" type="hidden" name="EmailId" runat="server" cssclass="Warn">
            <input id="Unsubscribe" type="hidden" name="Unsubscribe" runat="server" cssclass="Warn">
            <input id="TextOnly" type="hidden" name="TextOnly" runat="server" cssclass="Warn">
            <input id="BadEmail" type="hidden" name="BadEmail" runat="server" cssclass="Warn">
            <!--Name Anita and Shubhrata Date 11 Apr 06 Reason:Secured Check Control-->
            <input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server"
                cssclass="Warn">
            <!--Name Anita and Shubhrata Date 11 Apr 06 Reason:Secured Check Control-->
            <%--    Added by prasad--%>
            <asp:HiddenField ID="HiddenFieldDirty" Value="false" runat="server" />
            <asp:HiddenField ID="HiddenFieldDeathDate" Value="" runat="server" />
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            <div id="dialog-confirm" title="Delete Web Account Info" style="display: none;">
                <p id="p_dialog_confirm" style="height: 55px">
                    Are you sure you want to proceed?
                </p>
            </div>
            <div id="ConfirmDialog" runat="server" style="display: none;">
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
                                                OnClientClick="CloseDialogDeathNotify()" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Tooltip" runat="server" style="z-index: 1000; width: auto; border-left: 1px solid silver; border-top: 1px solid silver; border-right: 2px solid black; border-bottom: 1px solid #cccccc; padding: 3px; position: absolute; top: 0; left: 0; background: LightYellow; color: black; display: none; /* does not work in ie6 */	font-size: 7pt; font-family: verdana; margin: 0; overflow: visible; text-align: left;">
                <asp:Label runat="server" ID="lblComments" Style="display: block; width: auto; overflow: visible; font-size: x-small;"></asp:Label>
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

             <%--START: PPP | 04/20/2016 | YRS-AT-2719 | Unsuppress annuity dialog box--%>
            <div id="divUnSuppressAnnuity" style="overflow: visible; display: none;">
                <table class="formlayout formlayout-bg margin-5px-bottom" width="100%" cellspacing="0">
                    <tr>
                        <td colspan="2" class="UnSuppressAnnuityMessage">
                            <div id="divUnsppressErrorMessage" runat="server"></div>
                        </td>
                    </tr>
                    <%--START: PK | 10/10/2019 | YRS-AT-4598 | Added div to display error message of state withholding--%>
                   <tr>
                       <td colspan="2" align="left">
                           <div id="divUnsupressStateWithholdingWarning" runat="server"></div>
                       </td>
                   </tr>
                   <%--END:  PK | 10/10/2019 | YRS-AT-4598 | Added div to display error message of state withholding--%>
                    <tr id="tdUnSuppressAnnuitySummary" runat="server">
                        <td style="vertical-align: top; text-align: left; width: 45%; text-align: center;">
                            <table class="DataGrid_Grid">
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblUnSuppressAnnuityTaxable" runat="server" CssClass="Label_Small">Taxable:</asp:Label></td>
                                    <td style="width: 50%">
                                        <asp:TextBox ID="txtUnSuppressAnnuityTaxable" runat="server" CssClass="TextBox_Small_UnSuppressAnnuity" Text="0.00" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblUnSuppressAnnuityNonTaxable" runat="server" CssClass="Label_Small">Non-Taxable:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtUnSuppressAnnuityNonTaxable" runat="server" CssClass="TextBox_Small_UnSuppressAnnuity" Text="0.00" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblUnSuppressAnnuityTotal" runat="server" CssClass="Label_Small">Total:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtUnSuppressAnnuityTotal" runat="server" CssClass="TextBox_Small_UnSuppressAnnuity" Text="0.00" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblUnSuppressAnnuityMonths" runat="server" CssClass="Label_Small">No. of Months:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtUnSuppressAnnuityMonths" runat="server" CssClass="TextBox_Small_UnSuppressAnnuity" Text="0" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblGrossAmount" runat="server" CssClass="Label_Small">Gross Amount:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtUnSuppressAnnuityGrossAmount" runat="server" CssClass="TextBox_Small_UnSuppressAnnuity" Text="0.00" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblUnSuppressAnnuityDeductions" runat="server" CssClass="Label_Small">Less Deductions*:</asp:Label></td> <%-- ML |20.09.2019| YRS-AT-4598 |Asteric added   --%>
                                    <td>
                                        <asp:TextBox ID="txtUnSuppressAnnuityDeductions" runat="server" CssClass="TextBox_Small_UnSuppressAnnuity" Text="0.00" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblUnSuppressAnnuityNetAmount" runat="server" CssClass="Label_Small">Net Amount*:</asp:Label></td>  <%-- ML |20.09.2019| YRS-AT-4598 |Asteric added   --%>
                                    <td>
                                        <asp:TextBox ID="txtUnSuppressAnnuityNetAmount" runat="server" CssClass="TextBox_Small_UnSuppressAnnuity" Text="0.00" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: center;">
                            <div style="overflow: auto; width: 90%; height: 100%; text-align: left">
                                <asp:GridView ID="dgUnSuppressAnnuity" runat="server" Width="98%" CssClass="DataGrid_Grid" AutoGenerateColumns="false"
                                    OnRowDataBound="dgUnSuppressAnnuity_RowDataBound">
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                    <RowStyle CssClass="DataGrid_NormalStyle"></RowStyle>
                                    <Columns>
							            <asp:TemplateField HeaderStyle-Width="10%">
								            <ItemTemplate>
                                                <asp:CheckBox ID="chkUnSuppressAnnuityDeduction" runat="server" Checked='<%# Eval("IsChecked")%>' Enabled='<%#If(Eval("IsReadOnly").ToString().ToLower() = "true", "false", "true")%>'></asp:CheckBox>
                                                <asp:HiddenField ID="hdnCodeValue" runat="server" Value='<%# Eval("CodeValue").ToString().Trim()%>' />
								            </ItemTemplate>
							            </asp:TemplateField>
                                        <asp:BoundField DataField="CodeValue" HeaderText="Deductions" Visible="False" HeaderStyle-Width="0%" />
                                        <asp:BoundField DataField="ShortDescription" HeaderText="Withholdings / Deductions" HeaderStyle-Width="55%" />
                                        <asp:TemplateField HeaderStyle-Width="35%" HeaderText="Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnSuppressAnnuityDeductionAmount" runat="server" CausesValidation="False" Text='<%# Eval("Amount")%>' CssClass="TextBox_Small_UnSuppressAnnuity" 
                                                    Visible='<%#If(Eval("SubGroup1").ToString().ToUpper() = "DEDUCTIONS" Or Eval("IsReadOnly").ToString().ToLower() = "true", "true", "false")%>'></asp:Label>
                                                <!--2019.22.05 : SC : YRS-AT-2601 Starts - added check for exclude textbox which have to be excluded from application of new style-->
                                                <asp:TextBox ID="txtUnSuppressAnnuityDeductionAmount" runat="server" CausesValidation="False" ReadOnly='<%# Eval("IsReadOnly")%>' Text='<%# Eval("Amount").ToString().Trim()%>' 
                                                    CssClass="TextBox_Small_UnSuppressAnnuity Reject_TextBox_Disabled" Enabled='<%#If(Eval("IsChecked").ToString().ToLower() = "true", "true", "false")%>'
                                                    Visible='<%#If(Eval("SubGroup1").ToString().ToUpper() = "DEDUCTIONS" Or Eval("IsReadOnly").ToString().ToLower() = "true", "false", "true")%>'></asp:TextBox>
                                                <!--2019.22.05 : SC : YRS-AT-2601 Ends-->
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="DataGrid_SelectedStyle"></SelectedRowStyle>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                     <%--START : ML |20.09.2019| YRS-AT-4598 |Alert Message for State Withholding Deducation --%>
                    <tr id="trNoteUnsuppress" runat="server">
                        <td colspan="2" style="text-align:left;">                                                   
                                <asp:Label ID="lblNoteUnsuppress" runat="server" CssClass="Label_Small"> </asp:Label> 
                        </td>
                    </tr>
                    <%--END : ML |20.09.2019| YRS-AT-4598 |Alert Message for State Withholding Deducation --%>
                   
                    <tr>
                        <td align="right" colspan="2">
                            <div style="border: 1px solid #aaaaaa/*{borderColorContent}*/; background: #ffffff/*{bgColorContent}*/ url(images/ui-bg_flat_75_ffffff_40x100.png)/*{bgImgUrlContent}*/ 50%/*{bgContentXPos}*/ 50%/*{bgContentYPos}*/ repeat-x/*{bgContentRepeat}*/; color: #222222/*{fcContent}*/;">
                            </div>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button runat="server" ID="btnUnSuppressAnnuitySave" Text="  Save  " CssClass="Button_Normal" OnClick="btnUnSuppressAnnuitySave_Click" OnClientClick="javascript:return ValidateUnSuppressAnnuityDialog();" />
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnUnSuppressAnnuityCancel" Text="  Cancel  " CssClass="Button_Normal" OnClientClick="javascript:return CloseUnSuppressAnnuityDialog()" />
                                        </td>
                                    </tr>
                                    <%--START: PK | 10/10/2019 | YRS-AT-4598 | Added tr to show Yes/No button for  state withholding error message--%>
                                      <tr id="trConfirmYesNo" runat="server">
                                       <td>
                                           <asp:Button  ID="btnConfirmYes" runat="server" Text="Yes" Width="80px" CssClass="Button_Normal" OnClientClick="javascript:return ShowAnnuitySummaryDialog();" />
                                       </td>
                                       <td>
                                           <asp:Button   ID="btnConfirmNo" runat="server" Text="No" Width="80px" CssClass="Button_Normal" OnClientClick="javascript:return CloseUnSuppressAnnuityDialog()" />
                                       </td>
                                   </tr>
                                    <%--END: PK | 10/10/2019 | YRS-AT-4598 | Added tr to show Yes/No button for  state withholding error message--%>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <input id="hdnOldFundCostAmount" type="hidden" value="0.00" />
             <%--END: PPP | 04/20/2016 | YRS-AT-2719 | Unsuppress annuity dialog box--%>

            <table width="980">
                <tr>
                    <td width="100%">
                        <YRSControls:YMCA_Footer_WebUserControl ID="YMCA_Footer_WebUserControl1" runat="server"></YRSControls:YMCA_Footer_WebUserControl>
                    </td>
                </tr>

            </table>
            <asp:HiddenField ID="hdnUnSuppress" runat="server"  />
            <%--START: SN | 11/14/2019 | YRS-AT-4604 | YRS enh: State Withholding Project - Annuities Paid--%>
             <div id="divTaxWithHoldingDetails" runat="server" style="display: block; overflow: auto;">
            <table class="Table_WithoutBorder" cellspacing="0" cellpadding="0" width="95%">
                <tr>
                    <td>
                        <div style="overflow: auto; height: 100px">                       
                            <table id="tblWithHoldingdetails" style="width:66%; BORDER-COLLAPSE: collapse" cellSpacing=0  rules=all border=1   class ="DataGrid_Grid">
                                <thead class="DataGrid_HeaderStyle" >
                                    <th>Withholding Type</th>                                    
                                    <th>Amount</th>                                    
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
                        <asp:Button class="Button_Normal" ID="Button2" runat="server" Width="80" Text="OK" OnClientClick="javascript:return CloseTaxWithHoldingDetailsDialog();"></asp:Button>
                    </td>
                </tr>
            </table>
        </div> 
            <%--END: SN | 11/14/2019 | YRS-AT-4604 | YRS enh: State Withholding Project - Annuities Paid--%>
        </div>
    </form>
    <script type="text/javascript" language="javascript">

        //var text = document.getElementById('TextBoxDOB_TextBoxUCDate');
        var text = document.getElementById('TextBoxGeneralDOB');
        //text.onchange = function () { getAge(); text.onchange(); };   //function () { alert('This is onclick') };
        text.attachEvent("onchange", function () { getAge() });
        //alert('Setting up onChange event for ' + text.value);
        //Added by prasad for YRS 5.0-1469: Add link to Web Front End
        $(document).ready(function () {
            InitializeYRelationDialogBox();
            $("#ButtonWebFrontEndRetiree").click(function () { CheckAccessButtonWebFrontEnd('ButtonWebFrontEndRetiree'); return false; });
            InitializePINDialogBox();
            InitializePINConfirmDialogBox();
            $("#ButtonPINno").click(function () {
                GetPIN();
                ClearPIN('clear');
                return false;
            });
            <%--START: SN | 11/14/2019 | YRS-AT-4604 | YRS enh: State Withholding Project - Annuities Paid--%>
            $('#divTaxWithHoldingDetails').dialog({
                autoOpen: false,
                resizable: false,
                dialogClass: 'no-close',
                draggable: true,
                width: 347, minheight: 150,
                height: 185,
                closeOnEscape: false,
                title: "Show Disbursement Information",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });
            <%--END: SN | 11/14/2019 | YRS-AT-4604 | YRS enh: State Withholding Project - Annuities Paid--%>
        });
        
        <%--START: SN | 11/14/2019 | YRS-AT-4604 | YRS enh: State Withholding Project - Annuities Paid--%>
        function GetTaxWithHoldingDetails(disbursementId) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "RetireesInformationWebForm.aspx/GetTaxWithHoldingDetails",
                data: "{ 'requestedDisbursementId': '" + disbursementId + "'}",
                datatype: "json",
                success: function (data) {
                    $("#tblWithHoldingdetails tbody").html("");
                if (data.d == null) {
                    $("#tblWithHoldingdetails").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>No records found.</td></tr>");
                }
                else if (data.d.length <= 0) {
                    $("#tblWithHoldingdetails").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>No records found.</td></tr>");
                }
                else {
                    for (var i = 0; i < data.d.length; i++) {
                        if (i % 2 === 0) {
                            $("#tblWithHoldingdetails").append("<tr class='DataGrid_NormalStyle'><td style='width:15%;'>" + data.d[i].WithHoldingType + "</td><td style='width:15%;text-align:right'>" + data.d[i].Amount + "</td></tr>"); <%-- MMR | 2019.01.30 | Added to align text right--%>
                        } else {
                            $("#tblWithHoldingdetails").append("<tr class='DataGrid_AlternateStyle'><td style='width:15%;'>" + data.d[i].WithHoldingType + "</td><td style='width:15%;text-align:right'>" + data.d[i].Amount + "</td></tr>"); <%-- MMR | 2019.01.30 | Added to align text right--%>

                        }
                    }
                }
            },
            error: function (result) {
                $("#tblWithHoldingdetails tbody").html("");
                $("#tblWithHoldingdetails").append("<tr class='DataGrid_NormalStyle'><td colspan='5'>" + result.responseText + "</td></tr>");
            }
        });

}
        function CloseTaxWithHoldingDetailsDialog() {
            $('#divTaxWithHoldingDetails').dialog("close");
            return false;
        }

        function ShowTaxWithHoldingDetailsDialog(fosender) {
            var disbursementId = $(fosender).parent().find("[id$=hdndisbursementId]").val();
            $('#divTaxWithHoldingDetails').dialog("open");
            GetTaxWithHoldingDetails(disbursementId);
            return false;
        }
        <%--END: SN | 11/14/2019 | YRS-AT-4604 | YRS enh: State Withholding Project - Annuities Paid--%>
        function InitializeYRelationDialogBox() {
            <%--START: JT | 2018.08.28 | YRS-AT-4031 | Based on key HIDE_WEB_ACCOUNT_PRINT value controlling the appearance of Print button--%>
            var isPrintButtonToBeHidden = '<%=Me.IsPrintButtonToBeHidden%>';
            var dynamicButton = [{ text: "Un-Lock Account", click: UnlockWebAccount }, { text: "Send email with temp pass", click: SendMailTempPass }, { text: "Lock Account", click: LockWebAccount }, { text: "Close", click: OkWebFront }];
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
                             <%--buttons: [{ text: "Print", click: PrintReport }, { text: "Un-Lock Account", click: UnlockWebAccount }, { text: "Send email with temp pass", click: SendMailTempPass }, { text: "Lock Account", click: LockWebAccount }, { text: "Close", click: OkWebFront }]--%>
                             buttons: dynamicButton
                             <%-- END: JT | 2018.08.28 | YRS-AT-4031 | Assigning button properties based on key HIDE_WEB_ACCOUNT_PRINT value --%>
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


        function CloseWebProcessingDialog() {
            $('#dvWebProcessing').dialog('close');
        }

        function ShowWebProcessingDialog(Message, divTitle) {
            $('#dvWebProcessing').dialog({ title: divTitle });
            $('#dvWebProcessing').dialog("open");
            $('#lblProcessing').text(Message);

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
                        if (returndata.d.UserName != null) {
                            //$("#divWebFront").parent('div').find('button:contains("Print")').attr("disabled", "disabled"); <%-- HT | 2019.04.05 | YRS-AT-4371 | Commented the code to enable the print button--%>
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

        function GetAdminActivity() {
            $.ajax({
                type: "POST",
                url: "RetireesInformationWebForm.aspx/GetAdminActivity",
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
            if (CheckAccess('btnRetWebLockUnLock') == false)
            { return false; }
            else {
                ShowWebProcessingDialog('Please wait process is in progress...', '');
                $.ajax({
                    type: "POST",
                    url: "RetireesInformationWebForm.aspx/UnlockWebAccount",
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
            if (CheckAccess('btnRetWebLockUnLock') == false)
            { return false; }
            else {
                ShowWebProcessingDialog('Please wait process is in progress...', '');
                $.ajax({
                    type: "POST",
                    url: "RetireesInformationWebForm.aspx/LockWebAccount",
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
            if (CheckAccess('btnRetWebSendTempPass') == false)
            { return false; }
            else {
                ShowWebProcessingDialog('Please wait process is in progress...', '');
                $.ajax({
                    type: "POST",
                    url: "RetireesInformationWebForm.aspx/SendMailTempPass",
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
                       <%-- $("#divWebAcctMessage").html('Email with temporary password send to: ' + strMessage);--%>
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
        //                            url: "RetireesInformationWebForm.aspx/DeleteRecord",
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
            if (CheckAccess("ButtonWebFrontEndPrintRetire") == false)
            { return false; }
            else {
                $.ajax({
                    type: "POST",
                    url: "RetireesInformationWebForm.aspx/PrintForm",
                    data: "{}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (record) {
                        var i;
                        i = record.d;
                        if (i == 1) { alert("You are not authorized to view data"); }
                        else {
                            window.open('FT\\ReportViewer.aspx', 'ReportPopUp', "'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes')");
                            //'Anudeep:2015.03.06 - BT:2877 : YRS 5.0-2517: Web account inforamtion letter not going to IDM for Retired  
                            window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', 'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');
                        }
                    },
                    failure: function () {
                        alert("Error while printing");
                        return false;
                    }
                });
                return true;
            }
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
                    url: "RetireesInformationWebForm.aspx/UpdatePIN",
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
        function ClosePINno() { $("#divPIN").dialog('close'); }

        function ValidatePIN() {
            if (!(event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57))
            { event.returnValue = false; }


        }
        function GetPIN() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "RetireesInformationWebForm.aspx/GetPIN",
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
        /*End: Anudeep A 2014.09.26 BT:2440:YRS 5.0-2318 - Primary Active address record being created twice. */

        //Start: Bala: 01/19/2019: YRS-AT-2398: Hide if special handling is not required
        if ($("#LabelSpecialHandling").text() == '') {
            $('#LinkButtonSpecialHandling').hide();
            $('#LabelSpecialHandling').closest("tr").hide();
        }
        //End: Bala: 01/19/2019: YRS-AT-2398: Hide if special handling is not required

        <%--START: PPP | 04/20/2016 | YRS-AT-2719 - Applying Fees and deductions to death payments - Part A.2 --%>
        function RegisterUnSuppressAnnuityDialog() {
            $('#divUnSuppressAnnuity').dialog({
                autoOpen: false,
                resizable: false,
                draggable: true,
                closeOnEscape: true,
                modal: true,
                width: 600, minheight: 250,
                title: "Joint and Survivor Beneficiaries",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });
        }

        function ShowUnSuppressAnnuityDialog() {
            $("#divUnSuppressAnnuity").dialog('open');
            return false;
        }

        function CloseUnSuppressAnnuityDialog() {
            $("#divUnSuppressAnnuity").dialog('close');
            <%--START: Manthan Rajguru | 05/11/2016 | YRS-AT-2719 | Clearing error message on click of cancel button --%>
            $('#<%=divUnsppressErrorMessage.ClientID%>').html('');
            $('#<%=divUnsppressErrorMessage.ClientID%>')[0].className = "";
            <%--END: Manthan Rajguru | 05/11/2016 | YRS-AT-2719 | Clearing error message on click of cancel button --%>
            //$('[id$=chkUnSuppressAnnuityDeduction]').attr('checked', false);
            $('[id$=chkUnSuppressAnnuityDeduction]').each(function () {
                if ($(this).prop('disabled') == false) {
                    $(this).attr('checked', false);
                }
            });

            $('[id$=txtUnSuppressAnnuityDeductionAmount]').each(function () {
                $(this).prop('disabled', true);
                $(this).val("0.00");
            });

            $.ajax({
                type: "POST",
                url: "RetireesInformationWebForm.aspx/CancelUnSuppressJSAnnuity",
                data: '{"strWithholdings":"' + $('#<%=txtUnSuppressAnnuityDeductions.ClientID%>').val() + '", "strNetAmount":"' + $('#<%=txtUnSuppressAnnuityNetAmount.ClientID%>').val() + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var result = response.d;
                    var amounts = result.split('|');

                    $('#<%=txtUnSuppressAnnuityDeductions.ClientID%>').val(amounts[0]);
                    $('#<%=txtUnSuppressAnnuityNetAmount.ClientID%>').val(amounts[1]);
                }
            });

            return false;
        }

        <%--START: PK | 10/10/2019 | YRS-AT-4598 | function to show/hide content based on error message--%>
        function ShowAnnuitySummaryDialog() {
            $("#divUnsupressStateWithholdingWarning").hide();
            $("#trConfirmYesNo").hide();
            $("#tdUnSuppressAnnuitySummary").show();
            $("#btnUnSuppressAnnuitySave").show();
            $("#btnUnSuppressAnnuityCancel").show();
            return false;
        }
       <%--END: PK | 10/10/2019 | YRS-AT-4598 | function to show/hide content based on error message--%>
        function DeductAmount(chk, codeValue, amount) {
            $.ajax({
                type: "POST",
                url: "RetireesInformationWebForm.aspx/UpdateJSAnnuityDeduction",
                data: '{"bIsDeductionSelected":"' + chk.checked.toString() + '", "strWithholdings":"' + $('#<%=txtUnSuppressAnnuityDeductions.ClientID%>').val() + '", "strNetAmount":"' + $('#<%=txtUnSuppressAnnuityNetAmount.ClientID%>').val() + '", "strDeductionAmount":"' + amount + '", "strCode":"' + codeValue + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var result = response.d;
                    var amounts = result.split('|');

                    $('#<%=txtUnSuppressAnnuityDeductions.ClientID%>').val(amounts[0]);
                    $('#<%=txtUnSuppressAnnuityNetAmount.ClientID%>').val(amounts[1]);
                }
            });
            $('#<%=divUnsppressErrorMessage.ClientID%>').html('');
        }

        function EnableFundCost(chk, textboxControl) {
            if (chk.checked) {
                $("#" + textboxControl).prop('disabled', false);
            }
            else {
                $("#" + textboxControl).prop('disabled', true);
                DeductAmount(chk, 'PRCSTS', $("#" + textboxControl).val());
                $("#" + textboxControl).val("0.00");
                $('#hdnOldFundCostAmount').val("0.00");
            }
        }

        function DeductFundCostAmount(codeValue, amount) {
            if ($('#' + amount.id).val() != '' && !isNaN(parseFloat($('#' + amount.id).val()))) {
                $.ajax({
                    type: "POST",
                    url: "RetireesInformationWebForm.aspx/UpdateJSAnnuityDeduction",
                    data: '{"bIsDeductionSelected":"true", "strWithholdings":"' + $('#<%=txtUnSuppressAnnuityDeductions.ClientID%>').val() + '", "strNetAmount":"' + $('#<%=txtUnSuppressAnnuityNetAmount.ClientID%>').val() + '", "strDeductionAmount":"' + $('#' + amount.id).val() + '", "strCode":"' + codeValue + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var result = response.d;
                        var amounts = result.split('|');

                        $('#<%=txtUnSuppressAnnuityDeductions.ClientID%>').val(amounts[0]);
                        $('#<%=txtUnSuppressAnnuityNetAmount.ClientID%>').val(amounts[1]);
                    }
                });
                $('#<%=divUnsppressErrorMessage.ClientID%>').html('');
                $('#<%=divUnsppressErrorMessage.ClientID%>')[0].className = "";
                $('#hdnOldFundCostAmount').val($('#' + amount.id).val());
            }
            else {
                $('#' + amount.id).val($('#hdnOldFundCostAmount').val());
            }
        }

        function ValidateUnSuppressAnnuityDialog() {
            var result;
            <%--START: Manthan Rajguru | 05/11/2016 | YRS-AT-2719 | storing control ID in variable and checking condition to show error message on save --%>
            var txtFundCosts = $("#<%=dgUnSuppressAnnuity.ClientID%> input[id*='txtUnSuppressAnnuityDeductionAmount']:text")
            if (txtFundCosts.is(':disabled') || (!(txtFundCosts.is(':disabled')) && txtFundCosts.val() != "0.00")) {
            <%--END: Manthan Rajguru | 05/11/2016 | YRS-AT-2719 | storing control ID in variable and checking condition to show error message on save --%>
                if (parseFloat($('#<%=txtUnSuppressAnnuityNetAmount.ClientID%>').val()) >= 0) {
                    result = true;
                    $('#<%=divUnsppressErrorMessage.ClientID%>').html('');
                    $('#<%=divUnsppressErrorMessage.ClientID%>')[0].className = "";
                }
                else {
                    result = false;
                    $('#<%=divUnsppressErrorMessage.ClientID%>').html('Insufficient annuity amount to apply withholdings.');
                    $('#<%=divUnsppressErrorMessage.ClientID%>')[0].className = "error-msg";
                }            
            }
            <%--START: Manthan Rajguru | 05/11/2016 | YRS-AT-2719 | Showing error message if above condition fails --%>
            else {
                result = false;
                $('#<%=divUnsppressErrorMessage.ClientID%>').html('Please provide Fund Costs.');
                $('#<%=divUnsppressErrorMessage.ClientID%>')[0].className = "error-msg";
            }
            <%--END: Manthan Rajguru | 05/11/2016 | YRS-AT-2719 | Showing error message if above condition fails --%>
            return result;
        }
        <%--END: PPP | 04/20/2016 | YRS-AT-2719 - Applying Fees and deductions to death payments - Part A.2 --%>
    </script>
</body>
