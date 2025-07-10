<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master"
    CodeBehind="RMDPrintLetters.aspx.vb" Inherits="YMCAUI.RMDPrintLetters" %>

<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register Src="~/UserControls/BatchProcessProgressControl.ascx" TagPrefix="uc1" TagName="BatchProcessProgressControl" %> <%--PPP | 04/21/2017 | YRS-AT-3356 | Registering new control "BatchProcessProgressControl" which will host 3 dialog 1. Confirm Box 2. Progress Box and 3. Error display Box--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="JS/jquery-1.5.1.min.js"></script>
    <script src="JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="JS/jquery-bPopup.js"></script>
    <link href="JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />
    <style type="text/css">
        .BG_ColourIsLocked {
            background-color: #90EE90; /*LightGreen*/
        }

        .BG_ColourIsBlocked {
            background-color: #FFB6C1; /*LightPink*/
        }

        .BG_ColourIsMultipleYearMRD {
            background-color: #00BFFF; /*DodgerBlue*/
        }
        /*END: SG: 2012.03.16: BT-1011*/

        /*
        CSS added by Dinesh Kanojia BT 2012 : YRS 5.0-2165:RMD enhancements
        */
        .BG_ColourIsNotEnrollAnnualMRDPayments {
            background-color: #FBC97A; /*Red*/
        }
         
        .BG_ColourFollowUpLetter {
            background-color: #F08080; 
        }   
        
        /*START : SB | 10/19/2016 | YRS-AT-3088 | Stylesheet for Cashout plans Code is commented css is applied thorugh custom stylesheet.css for cashout color highlighting*/
         /*.BG_ColourSubjectToCashoutLetter {
            background-color: #ade4f6; /*LightSkyBlue*/
        /*}*/
        /*END : SB | 10/19/2016 | YRS-AT-3088 | Stylesheet for Cashout plans Code is commented css is applied thorugh custom stylesheet.css for cashout color highlighting*/
        
        .LstFilter {
            width: 120px;
            height: 50px;
            font-family: Arial;
            font-size: 11px;
        }

        .txtsearch {
            width: 120px;
        }

        .tabSelected {
            font-family: verdana;
            text-align: center;
            font-weight: bold;
            color: #000000;
            background-color: #93BEEE;
            border-bottom: none;
            font-size: 10pt;
            width: 100%;
            height: 25px;
            text-decoration: none;
        }

        .tabNotSelected {
            background-color: #4172A9;
            font-family: verdana;
            font-weight: bold;
            font-size: 10pt;
            color: #ffffff;
            text-align: center;
            border: solid 1px White;
            border-bottom: none;
            width: 100%;
            height: 25px;
            text-decoration: none;
        }

        .tabNotSelectedLink {
            font-family: verdana;
            font-weight: bold;
            font-size: 10pt;
            color: #ffffff;
            width: 100%;
        }

        .tabSelectedLink {
            font-family: verdana;
            font-weight: bold;
            font-size: 10pt;
            color: #000000;
            width: 100%;
        }

        .hide {
            display: none;
        }

        .show {
            display: block;
        }

        .textAlign {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    args.set_errorHandled(true);
                }
            }

            <%--START: PPP | 04/21/2017 | YRS-AT-3356 | Moved to BatchProcessProgressControl--%>
            <%--
            $('#ConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 350, height: 350,
                title: "RMD Print Letter",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
            --%>
            <%--END: PPP | 04/21/2017 | YRS-AT-3356 | Moved to BatchProcessProgressControl--%>

            <%--START: PPP | 05/24/2017 | YRS-AT-3356 | Warning message is not required--%>
            <%--
            var checkBoxSelector = '#< %=gvLetters.ClientID% > input[id*="chkSelect"]:checkbox';

            $(checkBoxSelector).bind('click', function () {
                mark_dirty();
            });
            --%>
            <%--END: PPP | 05/24/2017 | YRS-AT-3356 | Warning message is not required--%>
        });


        <%--START: PPP | 04/21/2017 | YRS-AT-3356 | Moved to BatchProcessProgressControl--%>
        <%--
        function showConfirmdialog() {
            $('#ConfirmDialog').dialog("open");
        }

        function closeConfirmdialog() {
            $('#ConfirmDialog').dialog('close');
        }
        --%>
        <%--END: PPP | 04/21/2017 | YRS-AT-3356 | Moved to BatchProcessProgressControl--%>
        function ontabmousehover(id) {
            $('#' + id).attr('class', 'tabselected');
        }
        function ontabmousehout(id) {
            $('#' + id).attr('class', 'tabNotSelected');
        }

        function OpenPDF(url) {
            try {
                //START: MMR | 2016.12.15 | YRS-AT-3203 | Changed width and height of pop-up window
                //window.open(url, 'OpenCustomPDF', 'width=900, height=900, resizable=yes, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no');
                if (!CheckAccessRightsForReprint()) { <%-- MMR | 2017.05.16 | YRS-AT-3356 | Check read only access rights before opening PDF--%>
                    window.open(url, 'OpenCustomPDF', 'width=1024, height=768, resizable=yes, top=0, left=0, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no');
                }
                //END: MMR | 2016.12.15 | YRS-AT-3203 | Changed width and height of pop-up window
                return false;
            }
            catch (err) {
                alert(err.message);
            }
        }

        <%--START: PPP | 04/17/2017 | YRS-AT-3237 | Duplicate method, so cmmenting it.--%>
        ////Below function will close the modal popup
        //function ClosePrintDialog() {
        //    $("#modalRMDPrintLetter").dialog('close');
        //    window.location.href = "RMDPrintLetters.aspx?Form=call";
        //}
        <%--END: PPP | 04/17/2017 | YRS-AT-3237 | Duplicate method, so cmmenting it.--%>

        function CallLetter() {
            window.open('CallReport.aspx', 'ReportPopUp1', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
        }

        /* Check Box Selection code */

        function chkall(ival) {
            var f = document.getElementById("gvLetters");
            for (var i = 1; i < f.getElementsByTagName("input").length; i++) {
                if (f.getElementsByTagName("input").item(i).type == "checkbox") {
                    if (f.getElementsByTagName("input").item(i).disabled == false) {
                        f.getElementsByTagName("input").item(i).checked = ival;
                    }
                }
            }
        }

        function Unckeck(obj) {
            var f = document.getElementById("gvLetters");
            if (!obj.checked) {
                f.getElementsByTagName("input").item(0).checked = false;
            }
        }

        //Start: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.

        <%--START: PPP | 04/21/2017 | YRS-AT-3356 | Moved to BatchProcessProgressControl--%>
        <%--
        function CallProcess(strbatchId, strModule) {
            OpenProgressPopup();
            CallPrintLetterProcess(strbatchId, 0, "", 0, 0, strModule);
        }

        function OpenProgressPopup() {
            $('#modalRMDPrintLetter').dialog({
                autoOpen: false,
                draggable: true,
                show: "fade",
                hide: "fade",
                modal: true,
                close: false,
                title: "RMD Print Letters - Creation Status",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                },
                width: 900,
                height: 700,
                display: "block",
                closeOnEscape: false
            });
            buttons: [{ text: "Close", click: ClosePrintDialog }]
            $('#modalRMDPrintLetter').dialog("open");
        }

        < %--START : CS | 2016.10.24 |  YRS-AT-3088 | Opening the Cash out and intialOrFollowup letter--% >
        function OpenIntialLetter() {
            OpenPDF($('#hdnInitialLetter').val());
        }

        function OpenCashoutLetter() {
            OpenPDF($('#hdnCashoutLetter').val());
        }

        function OpenFollowupLetter() {
            OpenPDF($('#hdnFollowupLetter').val());
        }
        < %--END : CS | 2016.10.24 |  YRS-AT-3088 | Opening the Cash out and intialOrFollowup letter--% >
        function ClosePrintDialog() {
            $('#modalRMDPrintLetter').dialog('close');
        }
        < %--START : SB | 2016.11.22 |  YRS-AT-3203 | Opening the Cash out Followup letter--% >
        function OpenCashoutFollowupLetter() {
            OpenPDF($('#hdnCashoutFollowupLetter').val());
        }
        < %--END : SB | 2016.11.22 |  YRS-AT-3203 | Opening the Cash out Followup letter--% >

        function CallPrintLetterProcess(strbatchId, iCount, strProcessName, iIDXCreated, iPDFCreated, strModule) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "RMDPrintLetters.aspx/RMDProcess",
                data: "{'strBatchId': '" + strbatchId + "','iCount':" + iCount + ",'strProcessName':'" + strProcessName + "'," +
                    "'iIDXCreated':" + iIDXCreated + ",'iPDFCreated':" + iPDFCreated + ",'strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d.strretValue == "success") {
                        CopyFilestoFileServer(1, 1, strbatchId, strModule, strModule);
                        RMDBatchProcessCounter = data;

                        $('#dvPDF').text("Completed");
                        $('#dvIDX').text("Completed");
                        $('#dvReg').text("Completed");

                        $('#dvPDF')[0].className = "success-Progress";
                        $('#dvIDX')[0].className = "success-Progress";
                        $('#dvReg')[0].className = "success-Progress";
                        < %--START : CS | 2016.10.24 |  YRS-AT-3088 | To hide the Print Report Links--% >
                        $('#hdnInitialLetter').attr("value", data.d.initialLetterPath)
                        $('#hdnFollowupLetter').attr("value", data.d.followupLetterPath)
                        $('#hdnCashoutLetter').attr("value", data.d.cashOutLetterPath)
                        $('#hdnCashoutFollowupLetter').attr("value", data.d.cashOutFollowupLetterPath)        < %--SB | 2016.11.22 |  YRS-AT-32038 |  Assigning cashout followup letter path --% >
                        < %--END : CS | 2016.10.24 |  YRS-AT-3088 | To hide the Print Report Links--% >
                        $('#hdnProgress').attr("value", "1");
                       
                    }
                    else if (data.d.strretValue == "error") {
                        $('#dvMessage').text("Some error occured.");
                        $("#dvMessage")[0].className = "error-msg";
                        $("#trshowErrors")[0].className = 'show';
                        CallArrErrorDataList(strbatchId, strModule);
                        UpdateDisplay(data);
                        $('#dvPDF').text("Error");
                        $('#dvIDX').text("Error");
                        $('#dvReg').text("Error");

                        $('#dvPDF')[0].className = "error-Progress";
                        $('#dvIDX')[0].className = "error-Progress";
                        $('#dvReg')[0].className = "error-Progress";
                        $('#dvCopy')[0].className = "info-Progress";
                        < %--START : CS | 2016.10.24 |  YRS-AT-3088 | To hide the Print Report Links--% >
                        $("#trshowPrintCtls")[0].className = "hide";
                        $("#trshowPrintTitle")[0].className = "hide";
                        < %--START : CS | 2016.10.24 |  YRS-AT-3088 | To hide the Print Report Links--% >

                    }
                    else if (data.d.strretValue == "pending") {
                        CallPrintLetterProcess(data.d.strBatchId, data.d.iProcessCount, data.d.strProcessName, data.d.iIdxCreated, data.d.iPdfCreated, strModule);
                        UpdateDisplay(data);
                        //info-msg
                        //success - msg
                        $('#dvPDF')[0].className = "info-Progress";
                        $('#dvIDX')[0].className = "info-Progress";
                        $('#dvReg')[0].className = "info-Progress";
                        $('#dvCopy')[0].className = "info-Progress";
                        < %--START : CS | 2016.10.24 |  YRS-AT-3088 | To hide the Print Report Links--% >
                        $("#trshowPrintCtls")[0].className = "hide";
                        $("#trshowPrintTitle")[0].className = "hide";
                        < %--START : CS | 2016.10.24 |  YRS-AT-3088 | To hide the Print Report Links--% >

                    }
                    else {
                        $("#dvMessage")[0].className = "error-msg";
                        $("#trshowErrors")[0].className = 'show';
                        CallArrErrorDataList(strbatchId, strModule);
                        UpdateDisplay(data);
                        $('#dvPDF').text("Error");
                        $('#dvIDX').text("Error");
                        $('#dvReg').text("Error");
                        $('#dvPDF')[0].className = "error-Progress";
                        $('#dvIDX')[0].className = "error-Progress";
                        $('#dvReg')[0].className = "error-Progress";
                        $('#dvCopy')[0].className = "info-Progress";
                         < %--START : CS | 2016.10.24 |  YRS-AT-3088 | To hide the Print Report Links--% >
                        $("#trshowPrintCtls")[0].className = "hide";
                        $("#trshowPrintTitle")[0].className = "hide";
                         < %--START : CS | 2016.10.24 |  YRS-AT-3088 | To hide the Print Report Links--% >
                    }
                },
                error: function (result) {
                    $('#dvMessage').text("Some network error occured.");
                    $("#dvMessage")[0].className = "error-msg";
                    $("#trshowErrors")[0].className = 'show';
                    CallArrErrorDataList(strbatchId, strModule);
                    UpdateDisplay(data);
                    $('#dvPDF').text("Error");
                    $('#dvIDX').text("Error");
                    $('#dvReg').text("Error");

                    $('#dvPDF')[0].className = "error-Progress";
                    $('#dvIDX')[0].className = "error-Progress";
                    $('#dvReg')[0].className = "error-Progress";
                    $('#dvCopy')[0].className = "info-Progress";
                     < %-- START : CS | 2016.10.24 |  YRS-AT-3088 | To hide the Print Report Links--% >
                    $("#trshowPrintCtls")[0].className = "hide";
                    $("#trshowPrintTitle")[0].className = "hide";
                      < %-- START : CS | 2016.10.24 |  YRS-AT-3088 | To hide the Print Report Links--% >
                }
            });
        }

        var RMDBatchProcessCounter
        function CopyFilestoFileServer(strOR, strDEL, strBatchId, strModule) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "FT/CopyFilestoFileServer.aspx/CopytoFileServer",
                data: "{'strOR':'" + strOR + "','strDEL':'" + strDEL + "','strBatchId':'" + strBatchId + "','strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    RMDBatchProcessCounter.d.iPdfCopied = data.d.iIdxCopied;
                    RMDBatchProcessCounter.d.iIdxCopied = data.d.iPdfCopied;

                    //START : CS | 2016.10.24 |  YRS-AT-3088 | To displaying the Print Report Links
                    if ($('#hdnProgress').val() == "1") {
                        $('#hdnProgress').attr("value", "0");
                        $("#trshowPrintCtls")[0].className = "show";
                        $("#trshowPrintTitle")[0].className = "show";
                        $('#lnkFollowupLetter').css("display", "none");
                        $('#lnkInitialLetter').css("display", "none");
                        $('#lnkCashoutLetter').css("display", "none");
                        $('#lblLine').css("display", "none")
                        //START : SB | 2016.11.22 |  YRS-AT-3203 | If file path is available then showing Cashout Followup Letter printing link
                        $('#lnkCashoutFollowupLetter').css("display", "none");     
                        if ($('#hdnCashoutFollowupLetter').val() != '') {
                            $('#lnkCashoutFollowupLetter').css("display", "block");
                        }
                        if ($('#hdnFollowupLetter').val() != "" && $('#hdnCashoutFollowupLetter').val() != '') {
                            $('#lblLine').css("display", "block")
                        }
                        //END : SB | 2016.11.22 |  YRS-AT-3203 | If file path is available then showing Cashout Followup Letter printing link
                        if ($('#hdnFollowupLetter').val() != "") {
                            $('#lnkFollowupLetter').css("display", "block");
                       
                        }
                        if ($('#hdnInitialLetter').val() != "") {
                            $('#lnkInitialLetter').css("display", "block");
                        }
                        if ($('#hdnCashoutLetter').val() != '') {
                            $('#lnkCashoutLetter').css("display", "block");
                        }
                        if ($('#hdnInitialLetter').val() != "" && $('#hdnCashoutLetter').val() != '') { 
                            $('#lblLine').css("display", "block")
                        }
                    }
                    //END : CS | 2016.10.24 |  YRS-AT-3088 | To displaying the Print Report Links
                    if (data.d.strStatus == 'error') {
                        $('#dvStatusMsg').text("Some IDX or PDF files were not copied to IDM server.");
                        CallArrErrorDataList(strBatchId, strModule)
                        $("#trshowErrors")[0].className = "show";
                        UpdateDisplay(RMDBatchProcessCounter);
                        $('#dvCopy').text("Error");
                        $('#dvCopy')[0].className = "error-Progress";
                    }
                    else {
                        UpdateDisplay(RMDBatchProcessCounter);

                        if (RMDBatchProcessCounter.d.iTotalIDXPDFCount < RMDBatchProcessCounter.d.iTotalCount) {
                            $('#dvStatusMsg').text("Some of the records were not proccessed due to problem, please click on view details link for details.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(RMDBatchProcessCounter);
                            $('#dvCopy').text("Error");
                            $('#dvCopy')[0].className = "error-Progress";
                        }
                        else if (RMDBatchProcessCounter.d.iPdfCreated < RMDBatchProcessCounter.d.iTotalIDXPDFCount || RMDBatchProcessCounter.d.iIdxCreated < RMDBatchProcessCounter.d.iTotalIDXPDFCount) {
                            $('#dvStatusMsg').text("Some IDX or PDF files were not created.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(RMDBatchProcessCounter);
                            $('#dvCopy').text("Error");
                            $('#dvCopy')[0].className = "error-Progress";
                        }
                        else if (RMDBatchProcessCounter.d.iPdfCopied == RMDBatchProcessCounter.d.iTotalIDXPDFCount && RMDBatchProcessCounter.d.iIdxCopied == RMDBatchProcessCounter.d.iTotalIDXPDFCount) {
                            $('#dvStatusMsg').text("RMD letter process completed sucessfully.");

                            $('#dvCopy').text("Completed");
                            $('#dvPDF').text("Completed");
                            $('#dvIDX').text("Completed");
                            $('#dvReg').text("Completed");

                            $('#dvPDF')[0].className = "success-Progress";
                            $('#dvIDX')[0].className = "success-Progress";
                            $('#dvReg')[0].className = "success-Progress";
                            $('#dvCopy')[0].className = "success-Progress";
                        }
                        else if (RMDBatchProcessCounter.d.iTotalIDXPDFCount == RMDBatchProcessCounter.d.iTotalCount) {
                            $('#dvStatusMsg').text("RMD letter process completed sucessfully.");
                            $('#dvCopy').text("Completed");
                            $('#dvPDF').text("Completed");
                            $('#dvIDX').text("Completed");
                            $('#dvReg').text("Completed");

                            $('#dvPDF')[0].className = "success-Progress";
                            $('#dvIDX')[0].className = "success-Progress";
                            $('#dvReg')[0].className = "success-Progress";
                            $('#dvCopy')[0].className = "success-Progress";
                        }
                        else {
                            $('#dvStatusMsg').text("Internal exception.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(RMDBatchProcessCounter);
                            $('#dvCopy').text("Error");
                            $('#dvCopy')[0].className = "error-Progress";
                        }

                        //$('#dvCopy').text("");
                        //$('#dvPDF').text("");
                        //$('#dvIDX').text("");
                        //$('#dvReg').text("");

                        $('#imgCopiedComplete').css("display", "none");
                        $('#imgPDFComplete').css("display", "none");
                        $('#imgIDXComplete').css("display", "none");
                        $('#imgRegComplete').css("display", "none");

                    }
                },
                error: function (result) {
                }
            });
        }

        function CallArrErrorDataList(strBatchId, strModule) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "FT/CopyFilestoFileServer.aspx/ArrErrorDataList",
                data: "{'strBatchId': '" + strBatchId + "','strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d == "ArrErrorDataList table not found in database.") {
                        $('#divPopErrorMsg').html("Some internal error occured while creating this batch.");
                    }
                    else {
                        $('#divPopErrorMsg').html(data.d);
                    }
                },
                error: function (result) {
                    $('#divPopErrorMsg').text("Some network error occured while copying files to IDM server.");
                }
            });
        }

        var iPreviousValue = 0;
        function UpdateDisplay(data) {
            var strStatus = ''
            strStatus = "Total No. of records under process " + data.d.iTotalCount + ".";
            $('#dvShowStatus').html(strStatus);
            $('#dvReqProgress').text(data.d.iTotalIDXPDFCount + " out of " + data.d.iTotalCount + ".");
            $('#dvIDXProgress').text(data.d.iIdxCreated + " out of " + data.d.iTotalCount + ".");
            $('#dvPDFProgress').text(data.d.iPdfCreated + " out of " + data.d.iTotalCount + ".");
            $('#dvCopiedProgress').text(data.d.iPdfCopied + " out of " + data.d.iTotalCount + ".");
        }

        function ExceptionShowError() {
            $('#dvShowErrorsMsg').dialog({
                autoOpen: false,
                draggable: false,
                resizable: false,
                close: false,
                modal: true,
                width: 500, height: 400,
                title: "Error Message",
                open: function (type, data) {
                }
            });
            $('#dvShowErrorsMsg').css({ display: "block" });
            $('#dvShowErrorsMsg').dialog("open");
        }--%>
        <%--END: PPP | 04/21/2017 | YRS-AT-3356 | Moved to BatchProcessProgressControl--%>

        //End: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.
        
        <%-- START: MMR | 2017.05.16 | YRS-AT-3356 | Get read only access rights and warning message to be displayed--%>
        function CheckAccessRightsForReprint() {
            var readOnlyWarningMessage = '';
            var isReadOnly = false;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "RMDPrintLetters.aspx/CheckAccessRightsForReprint",
                data: "{}",
                async: false,
                dataType: "json",
                success: function (data) {
                    isReadOnly = data.d.Value;
                    if (isReadOnly && data.d.MessageList != undefined) {
                        var readOnlyWarningMessage = data.d.MessageList[0];
                        $('#<%=Me.Master.FindControl("DivMainMessage").ClientID%>').html('<DIV class="error-msg">' + readOnlyWarningMessage + '</DIV>');
                    }
                },
                error: function (result) {
                }
            });
            return isReadOnly;
        }
        <%-- END: MMR | 2017.05.16 | YRS-AT-3356 | Get read only access rights and warning message to be displayed--%>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="div_center">
        <asp:ScriptManagerProxy ID="RMDPRintScriptManager" runat="server">
        </asp:ScriptManagerProxy>
        <asp:UpdatePanel ID="uplGenerateRMD" runat="server">
            <ContentTemplate>
                <table class="Table_WithoutBorder" cellpadding="0" cellspacing="0" width="100%" align="center">
                    <tr>
                        <td align="left" valign="middle" style="width: auto">
                            <asp:Label ID="lblMRDMsg" runat="server" CssClass="Label_Small"></asp:Label>
                        </td>
                        <td align="right" cellspacing="0">
                            <table class="td_withoutborder" cellpadding="0" cellspacing="0" style="width: 100%; height: 25px;">
                                <tr>
                                    <td style="width: 33%;">  <%-- SB | 10/19/2016 | YRS-AT-2685 |Changing the width to add new tab for Reprint letter --%>
                                        <asp:LinkButton ID="lnkInitial" Text="Generate Initial Letter" runat="server" CssClass="tabNotSelected"
                                            onmouseover="javascript: ontabmousehover('lnkInitial');" onmouseout="javascript: ontabmousehout('lnkInitial');"></asp:LinkButton>
                                        <asp:Label ID="lbllnkInitial" CssClass="tabSelected" runat="server" Text="Generate Initial Letter"></asp:Label>
                                    </td>
                                    <td style="width: 33%;">  <%-- SB | 10/19/2016 | YRS-AT-2685 |Changing the width to add new tab for Reprint letter --%>
                                        <asp:LinkButton ID="lnkFollowup" CssClass="tabNotSelected" Text="Generate Follow-up Letter" runat="server"
                                            onmouseover="javascript: ontabmousehover('lnkFollowup');" onmouseout="javascript: ontabmousehout('lnkFollowup');"></asp:LinkButton>
                                        <asp:Label ID="lbllnkFollowup" CssClass="tabSelected" runat="server" Text="Generate Follow-up Letter"></asp:Label>
                                    </td>
                                     <%-- START : SB | 10/19/2016 | YRS-AT-2685 | Creating New tab for Reprint letter--%>
                                    <td style="width: 34%;">
                                        <asp:LinkButton ID="lnkReprintLetter" CssClass="tabNotSelected" Text="Re-Print Letter" runat="server"
                                          onmouseover="javascript: ontabmousehover('lnkReprintLetter');" onmouseout="javascript: ontabmousehout('lnkReprintLetter');"  ></asp:LinkButton>
                                       <asp:Label ID="lbllnkReprintLetter" CssClass="tabSelected" runat="server" Text="Re-Print Letter"></asp:Label>
                                    </td>
                                     <%-- END : SB | 10/19/2016 | YRS-AT-2685 | Creating new tab for Reprint letter--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table class="Table_WithoutBorder" cellpadding="0" cellspacing="0" width="100%" align="center">
                    <tr style="vertical-align: top;">
                        <td class="tabSelectedLink" style="width: 100%; height: 3px; text-align: left;">
                            <asp:Label ID="lblDescription" runat="server" Text="List of person(s) eligible for generating initial letters."></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="Table_WithBorder" cellpadding="0" cellspacing="0" align="center">
                    <tr>
                        <td style="vertical-align: top; width: 145px; text-align: center">
                            <div style="overflow: auto; width: 145px; border-top-style: none; border-right-style: none; border-left-style: none; height: 400px; border-bottom-style: none">
                                <table class="Table_WithBorder" id="Table2" cellspacing="1" cellpadding="1" border="0" style="width: 100%">
                                    <tr class="DataGrid_AlternateStyle">
                                        <td>
                                            <b>Filter Criteria</b>
                                        </td>
                                    </tr>
                                    
                                    <tr class="DataGrid_HeaderStyle" id="trFilterAccountLocked" runat="server">
                                        <td>Account Locked
                                        </td>
                                    </tr>
                                    <tr id="trFilterAccountDDL" runat="server" >
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlAcctLocked" CssClass="LstFilter" Width="120px">
                                                <asp:ListItem Selected="True" Text="Any" Value="All"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle" id="trFilterInsufficientBal" runat="server">
                                        <td>Insufficient Balance
                                        </td>
                                    </tr>
                                    <tr id="trFilterInsufficientBalDDL" runat="server">
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlInsufficientBal" CssClass="LstFilter" Width="120px">
                                                <asp:ListItem Selected="True" Text="Any" Value="All"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid_HeaderStyle">
                                        <td>
                                            <asp:Label ID="lblYears" Text="Year" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlRMDYear" CssClass="LstFilter" Width="120px" AutoPostBack="True"></asp:DropDownList>   <%-- SB | 11/10/2016 | YRS-AT-2685 | To fill batch id after RMD year selection on reprint tab --%>
                                        </td>
                                    </tr>
                                    <%-- START : SB | 10/19/2016 | YRS-AT-2685 | Creating new fields in  Filter section for Reprint letter--%>
                                        <tr id="trRMDBatchWiseDataControl" runat="server">
                                            <td style="text-align: left" class="Label_Small"><div id="divBatchTitle" runat="server" style="overflow: auto; width: 100%; height: 100%">
                                                 Batch(s)</div>
                                            </td></tr>
                                      <asp:Repeater ID="rptRMDBatchIDs" runat="server" Visible="true">
                                                <ItemTemplate>
                                                    <tr valign="top" id="trRptRMDReprintBatchIDs" runat="server">
                                                        <td id="liRMDRePrintLetterBatchIDs" runat="server" style="padding-bottom: 10px; list-style-type: none; text-align: left; width: 100%;">
                                                            <asp:LinkButton runat="server" ID="lnkRMDRePrintLetterBatchID" Text='<%# Eval("BatchIdDetail")%>' OnClick="lnkRMDRePrintLetterBatchId_Click" CssClass="Link_SmallBold"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                         </asp:Repeater> 
                                      <%-- END : SB | 10/19/2016 | YRS-AT-2685 | Creating new fields in  Filter section for Reprint letter--%>
                                    <tr class="DataGrid_HeaderStyle" id="trFilterFollowupGenerated" runat="server">
                                        <td>
                                            <asp:Label ID="txtFollowup" Text="Follow-up Generated" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trFilterFollowupGeneratedDDL" runat="server">
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlFollowUp" CssClass="LstFilter" Width="120px">
                                                <asp:ListItem Selected="True" Text="Any" Value="All"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                </table>
                                <table class="Table_WithBorder" id="Table3" cellspacing="1" cellpadding="1" border="0" style="width: 100%" runat="server">
                                    <tr>
                                        <td class="Label_Small">Fund No.:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFundId" CssClass="TexBox_Normal" runat="server" MaxLength="20" Width="130px" onkeypress="ValidateNumeric();"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button runat="server" ID="btnSearch" Text="View >>" CssClass="Button_Normal" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td style="text-align: left; width: 100%" id="tdBatchWiseInformation" runat="server">
                            <%-- START : SB | 10/19/2016 | YRS-AT-2685 | Displaying Message for Batch Wise Re-Print Letter --%>
                            <div id="divBatchwiseInformation" class="Td_ButtonContainer" style="width: 100%" runat="server"></div>
                             <div id="divToShowReprintLinks" style="width: 95% ; text-align:right" runat="server" visible="false">
                                  <asp:LinkButton runat="server" ID="lnkRMDRePrintInitialLetter" Text="Print RMD Initial Letter"  CssClass="Link_SmallBold"   Visible="false"></asp:LinkButton> 
                                <span id="spanPipeForCashout" runat="server" Visible="false">|</span>  <asp:LinkButton runat="server" ID="lnkRMDRePrintCashoutLetter" Text=" Print RMD Cashout Letter"  CssClass="Link_SmallBold"  Visible="false"></asp:LinkButton>
                             </div>
                             <div id="divToShowReprintFollowupLink" style="width: 95% ; text-align:right" runat="server" visible="false">
                                <asp:LinkButton runat="server" ID="lnkRMDRePrintFollowupLetter" Text=" Print RMD Follow-up Letter"  CssClass="Link_SmallBold"  Visible="false"></asp:LinkButton>
                                <%-- START : SB | 11/22/2016 | YRS-AT-3203 | Displaying Link for cashout followup letter --%>
                                  <span id="spanPipeForCashoutFollowup" runat="server" Visible="false">|</span> 
                                  <asp:LinkButton runat="server" ID="lnkRMDRePrintCashoutFollowupLetter" Text=" Print RMD Cashout Followup Letter"  CssClass="Link_SmallBold"  Visible="false"></asp:LinkButton>
                                <%-- END : SB | 11/22/2016 | YRS-AT-3203 | Displaying Link for cashout followup letter --%>
                             </div>
                            <%-- END : SB | 10/19/2016 | YRS-AT-3088 | Displaying Message for Batch Wise Re-Print Letter --%>
                            <table class="Table_WithOutBorder" id="Table1" spellcheck="true" cellspacing="1" cellpadding="1" width="100%">
                                <tr>
                                    <td>
                                        <div style="overflow: scroll; width: 100%; height: 400px">
                                            <asp:GridView runat="server" ID="gvLetters" CssClass="DataGrid_Grid"
                                                AllowSorting="true" AutoGenerateColumns="false" SelectedRowStyle-VerticalAlign="Top"
                                                Width="100%" EmptyDataText="No records found.">
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" VerticalAlign="Top" />
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="20px">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="chkall(this.checked)" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" onclick="Unckeck(this)" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20px" />
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="MRDUniqueID" InsertVisible="true" HeaderText="MRDUniqueID" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />--%>
                                                    <asp:BoundField DataField="PerssID" InsertVisible="true" HeaderText="PerssID" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="FundIdNo" SortExpression="FundIdNo" HeaderText="Fund No" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="PlanType" SortExpression="PlanType" HeaderText="Plan Type" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="CurrentBalance" SortExpression="CurrentBalance" HeaderText="Current Balance" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="MRDAmount" SortExpression="MRDAmount" HeaderText="RMD Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="PaidAmount" SortExpression="PaidAmount" HeaderText="Paid Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="StatusTypeDescription" SortExpression="StatusTypeDescription" HeaderText="Fund Status" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="MRDExpireDate" SortExpression="MRDExpireDate" HeaderText="Due Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="chrSSNo" SortExpression="SSNo" HeaderText="SSNo" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="chvFirstName" SortExpression="FirstName" HeaderText="FirstName" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="chvLastName" SortExpression="LastName" HeaderText="LastName" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="chvMiddleName" SortExpression="MiddleName" HeaderText="MiddleName" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                       <%-- START : CS | 2016.10.24 |  YRS-AT-3088 | YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)--%>
                                                    <asp:BoundField DataField="IsCashOutEligible" SortExpression="IsCashOutEligible" HeaderText="IsCashOutEligible" ControlStyle-CssClass="hide" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide"  />
                                                       <%-- END : CS | 2016.10.24 |  YRS-AT-3088 | YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)--%>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr runat="server" id="trColorCodingLegends">    <%-- SB | 10/19/2016 | YRS-AT-2685 | Setting Id for table row--%>
                                    <td style="text-align: left" colspan="2">
                                        <table style="text-align: left" border="0">
                                            <tr>
                                                <td style="text-align: center" class="Label_Small">  <%-- SB | 10/19/2016 | YRS-AT-3088 | For displaying in proper alignment --%>
                                                    <span class="BG_ColourIsLocked">&nbsp;&nbsp;</span> - Account Locked.
                                                </td>
                                                <td style="text-align: center" class="Label_Small">   <%-- SB | 10/19/2016 | YRS-AT-3088 | For displaying in proper alignment --%>
                                                    <span class="BG_ColourIsBlocked">&nbsp;&nbsp;</span> - Insufficient Balance in One or Both Plans.
                                                </td>
                                                <%-- START : CS | 2016.10.24 |  YRS-AT-3088 | YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)--%>
                                                 <%--<td style="text-align: left" class="Label_Small" id="tdCashoutLegends" runat="server">
                                                    <span class="BG_ColourCashOutLetter">&nbsp;&nbsp;</span> - Subject to Cashout
                                                </td>--%>
                                                   <%-- END : CS | 2016.10.24 |  YRS-AT-3088 | YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)--%>
                                                <%-- START : SB | 10/19/2016 | YRS-AT-3088 | Displaying new color code and removing processed letter legends --%>
                                                <td style="text-align: center" class="Label_Small" id="tdCashoutLegends" runat="server">
                                                    <span class="BG_ColourSubjectToCashoutLetter">&nbsp;&nbsp;</span> - Subject to CashOut.
                                                </td>
                                                 <%-- END : SB | 10/19/2016 | YRS-AT-3088 | Displaying new color code and removing processed letter legends--%>
                                                <td style="text-align: center" class="Label_Small" id="tdFollowUpLegends" runat="server">
                                                    <span id="tdFollow" runat="server" class="BG_ColourFollowUpLetter">&nbsp;&nbsp;</span> - Follow-up Letter Processed.
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <%-- START : SB | 10/19/2016 | YRS-AT-2685 | Displaying Letter type information for reprint tab batchwise--%>
                                <tr runat="server" id="trColorCodingLegendsForReprintLetters">
                                    <td style="text-align: left" colspan="2">
                                        <table style="text-align: left" border="0">
                                            <tr>
                                                <td style="text-align: left" class="Label_Small">I - RMD Initial Letter.
                                                </td>
                                                <td style="text-align: left" class="Label_Small">F - RMD Follow-up Letter.
                                                </td>

                                               <%-- START : SB | 05/12/2016 | YRS-AT-3203 | Not to display cashout legend in Re-print letter tab --%>
                                               <%-- <td style="text-align: center" class="Label_Small" >
                                                    <span class="BG_ColourSubjectToCashoutLetter">&nbsp;&nbsp;</span> - Subject to CashOut.
                                                </td>--%>
                                               <%-- END : SB | 05/12/2016 | YRS-AT-3203 | Not to display cashout legend in Re-print letter tab --%>

                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                           <%-- END : SB | 10/19/2016 | YRS-AT-2685 | Displaying Letter type information for reprint tab batchwise--%>
                            </table>
                        </td>
                    </tr>

                </table>
                <table class="Td_ButtonContainer" style="width: 100%">
                    <tr>
                        <td style="text-align: right; width: 720px;">
                            <asp:Button runat="server" ID="btnPrintList" Text="Print List" CssClass="Button_Normal" />
                        </td>
                        <td style="text-align: right; width: 80px;">
                            <asp:Button runat="server" ID="btnPrintLetters" Text="Print Initial Letter" CssClass="Button_Normal" />
                        </td>
                        <td style="text-align: right; width: 40px;">
                            <asp:Button runat="server" ID="btnClose" Text="Close" CssClass="Button_Normal" />
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnPrintLetters" EventName="Click" />
            </Triggers>

        </asp:UpdatePanel>
    </div>
    <uc1:BatchProcessProgressControl runat="server" id="BatchProcessProgressControl" />
    <%--START: PPP | 04/21/2017 | YRS-AT-3356 | Moved to BatchProcessProgressControl--%>
    <%--<div id="ConfirmDialog" title="RMD Print Letter" runat="server" style="overflow: visible;">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div>
                    <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" CssClass="Label_Small" runat="server"></asp:Label>
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

                                <asp:Button runat="server" ID="btnYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                    OnClientClick="javascript: closeConfirmdialog();" />&nbsp;
                                    <asp:Button runat="server" ID="btnNo" Text="No" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        OnClientClick="javascript: closeConfirmdialog();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>
    <%--END: PPP | 04/21/2017 | YRS-AT-3356 | Moved to BatchProcessProgressControl--%>

     <%--Start: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.--%>
    <%--START: PPP | 04/21/2017 | YRS-AT-3356 | Moved to BatchProcessProgressControl--%>
    <%--<asp:UpdatePanel runat="server" ID="updatepnl">
        <ContentTemplate>
            <div id="modalRMDPrintLetter" style="display: none;">
                <table style="width: 100%;">
                    <tr>
                        <td class="Label_Small">Please do not close this window until all activities are complete.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvMessage" runat="server"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvShowStatus" runat="server" class="Label_Small"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="dvStatusMsg" runat="server" class="Label_Small"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" align="center" width="99%" cssclass="DataGrid_Grid" id="tblIDMDetails">
                                <tr>
                                    <td colspan="3" align="center" class="Label_Small">
                                        <u>Activities Performed</u>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" width="55%" class="DataGrid_HeaderStyle" style="text-align: center">
                                        <u>Activities</u>
                                    </td>
                                    <td align="center" width="15%" class="DataGrid_HeaderStyle" style="text-align: left">
                                        <u>Status</u>
                                    </td>
                                    <td align="center" width="30%" class="DataGrid_HeaderStyle" style="text-align: center">
                                        <u>Progress</u>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="DataGrid_AlternateStyle" width="55%" valign="middle" style="vertical-align: middle; background-color: White">1. Documents Entry Creation
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="background-color: White; width: 15%;">
                                        <table>
                                            <tr>
                                                <td class="DataGrid_AlternateStyle" style="background-color: White; text-align: center">
                                                    <asp:Image ID="imgRegComplete" runat="server" ImageUrl="~/images/complete.jpg" AlternateText="Complete"
                                                        CssClass="hide" Width="20" Height="20" />
                                                    <div id="dvReg" runat="server" class="info-Progress">
                                                        In-Progress
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White; width: 30%">
                                        <div runat="server" id="dvReqProgress" align="center" style="vertical-align: middle">
                                            In-Process
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="DataGrid_AlternateStyle" valign="middle" style="vertical-align: middle; background-color: White">2. Generate IDX document(s)
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="background-color: White">
                                        <table>
                                            <tr>
                                                <td class="DataGrid_AlternateStyle" style="background-color: White; text-align: center">
                                                    <asp:Image ID="imgIDXComplete" runat="server" ImageUrl="~/images/complete.jpg" AlternateText="Complete"
                                                        CssClass="hide" Width="20" Height="20" />
                                                    <div id="dvIDX" runat="server" class="info-Progress">
                                                        In-Progress
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                        <div runat="server" id="dvIDXProgress" align="center" style="vertical-align: middle">
                                            In-Process
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="DataGrid_AlternateStyle" valign="middle" style="vertical-align: middle; background-color: White">3. Generate PDF document(s)
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="background-color: White">
                                        <table>
                                            <tr>
                                                <td class="DataGrid_AlternateStyle" style="background-color: White; text-align: center">
                                                    <asp:Image ID="imgPDFComplete" runat="server" ImageUrl="~/images/complete.jpg" AlternateText="Complete"
                                                        CssClass="hide" Width="20" Height="20" />
                                                    <div id="dvPDF" runat="server" class="info-Progress">
                                                        In-Progress
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                        <div runat="server" id="dvPDFProgress" align="center" style="vertical-align: middle">
                                            In-Process
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="DataGrid_AlternateStyle" valign="middle" style="vertical-align: middle; background-color: White">4. Copy Generated IDX and PDF document(s) to IDM
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="background-color: White">
                                        <table>
                                            <tr>
                                                <td class="DataGrid_AlternateStyle" style="background-color: White; text-align: center">
                                                    <asp:Image ID="imgCopiedComplete" runat="server" ImageUrl="~/images/complete.jpg"
                                                        AlternateText="Complete" CssClass="hide" Width="20" Height="20" />
                                                    <div id="dvCopy" runat="server" class="info-Progress">
                                                        Pending
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="DataGrid_AlternateStyle" style="vertical-align: middle; background-color: White">
                                        <div runat="server" id="dvCopiedProgress" align="center" style="vertical-align: middle">
                                            Pending
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trshowErrors" class="hide">
                        <td>
                            <div>
                                <asp:Label ID="lblException" runat="server" class="Label_Small" Text="One or more errors where encounter while processing this batch. Please click here ">
                                    <asp:LinkButton ID="lnkShowError" runat="server" ForeColor="#3399ff" OnClientClick="ExceptionShowError(); return false;">view details</asp:LinkButton></asp:Label>
                            </div>
                        </td>
                    </tr>
        
                      < %-- START : CS | 2016.10.24 |  YRS-AT-3088 | YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)--% >
                    <tr id="trshowPrintTitle" class="hide">
                        <td>
                             <asp:Label ID="Label1" runat="server" class="Label_Small" Text=" To print the batch letter(s) please click on below link"></asp:Label>
                           
                        </td>
                    </tr>
                    <tr id="trshowPrintCtls" class="hide">
                        <td>  
                            <a href="#" id="lnkInitialLetter" style="color:#3399ff;float:left;text-decoration: underline;"  class="Label_Small" onclick="OpenIntialLetter();">Print RMD Initial Letter</a> 
                            <a href="#" id="lnkFollowupLetter" style="color:#3399ff;float:left;text-decoration: underline;"  class="Label_Small" onclick="OpenFollowupLetter();">Print RMD Followup Letter</a>
                            <asp:Label ID="lblLine" style="float:left;" runat="server"  Text="|"></asp:Label> 
                            <a href="#" id="lnkCashoutLetter" style="color:#3399ff;text-decoration: underline;"  class="Label_Small"  onclick="OpenCashoutLetter();">Print RMD Cashout Letter</a>
                            <a href="#" id="lnkCashoutFollowupLetter" style="color:#3399ff;text-decoration: underline;"  class="Label_Small"  onclick="OpenCashoutFollowupLetter();">Print RMD Cashout Followup Letter</a>   < %-- SB | 11/22/2016 | YRS-AT-3203 | Cashout followup letter link --% >
                                <input type="hidden" name="hdnInitialLetter" id="hdnInitialLetter" value="">
                                <input type="hidden" name="hdnCashoutLetter" id="hdnCashoutLetter" value="">
                                <input type="hidden" name="hdnFollowupLetter" id="hdnFollowupLetter" value="">
                                <input type="hidden" name="hdnCashoutFollowupLetter" id="hdnCashoutFollowupLetter" value="">  < %-- SB | 11/22/2016 | YRS-AT-3203 | Hidden field variable to hold cashout followup letter pdf file path --% >

                             <input type="hidden" name="hdnProgress" id="hdnProgress" value=""">
                         </td>
                    </tr>
                        < %-- END : CS | 2016.10.24 |  YRS-AT-3088 | YRS enh: RMD Utility distinguish cashout candidates (TrackIT 26224)--% >
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnClosePop" OnClientClick="ClosePrintDialog();" Text="Close" CssClass="Button_Normal" Style="width: 60px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" /> < %--PPP | 04/17/2017 | YRS-AT-3237 | Adding property [OnClientClick="ClosePrintDialog();"] which will help to close the dialog box--% >
                        </td>
                    </tr>
                </table>
            </div>

            <div style="display: none; overflow: auto" id="dvShowErrorsMsg">
                <div id="divPopErrorMsg">
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPrintLetters" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnClosePop" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <%--END: PPP | 04/21/2017 | YRS-AT-3356 | Moved to BatchProcessProgressControl--%>
     <%--//Start: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.--%>
</asp:Content>
