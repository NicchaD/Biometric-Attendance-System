<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LACLoansProcessing.aspx.vb" MasterPageFile="~/MasterPages/YRSMain.Master" Inherits="YMCAUI.LACLoansProcessing" %>

<asp:Content ID="contentHeader" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CustomStyleSheet.css" type="text/css" rel="stylesheet">
    <style type="text/css">
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

        .ui-progressbar {
            position: relative;
            color: #362878;
            width: 400px;
            margin-left: auto;
            margin-right: auto;
        }

        .progress-label {
            position: absolute;
            left: 50%;
            top: 4px;
            font-weight: bold;
            text-shadow: 1px 1px 0 #fff;
            margin-left: -40px;
        }

        .ui-dialog-titlebar-close {
            display: none;
        }

        #divProgressBar {
            margin-top: 4px;
        }

        .SummaryTable {
            padding-left: 2px;
            border: 1px solid black;
        }

            .SummaryTable td {
                border: 1px solid black;
            }

        .ClassHide {
            display: none;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function BindEvents() {
            $('#<%=ConfirmDialog.ClientID%>').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                width: 520, maxHeight: 420,<%--VC | 2018.11.22 | YRS-AT-4017 | Changed dialog width from 450 to 520--%>
                height: 260,
                title: "Disburse",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            $('#ProgressDialogNew').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                width: 450, maxHeight: 420,
                height: 280,
                title: "Processing Loan",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            var progressbar = $("#<%=divProgressBar.ClientID%>");
            progressbar.progressbar({
                value: false,
                change: function () {
                    //progressLabel.text("Current Progress: " + progressbar.progressbar("value") + "%");
                    //alert('hi');
                },
                complete: function () {
                    ShowSummary();
                }
            });

            //Check/uncheck all checkboxes in list according to main checkbox 
            $("#<%=gvLoansForProcessing.ClientID%> input[id*='chkSelectAll']:checkbox").click(function () {
                //Header checkbox is checked or not
                var bool = $("#<%=gvLoansForProcessing.ClientID%> input[id*='chkSelectAll']:checkbox").is(':checked');
                //check and check the checkboxes on basis of Boolean value
                $("#<%=gvLoansForProcessing.ClientID%> input[id*='chkSelect']:checkbox").attr('checked', bool);

                //START :ML |2019.01.28 | YRS-AT-4244 | Ajaxcall to update checkbox changes in session datatable
                CheckUncheckLoanProcessing();
                //END :ML |2019.01.28 | YRS-AT-4244 | Ajaxcall to update checkbox changes in session datatable
            });


            $("#<%=gvLoansForProcessing.ClientID%> input[id*='chkSelect']:checkbox").click(function () {
                //Get number of checkboxes in list either checked or not checked
                var totalCheckboxes = $("#<%=gvLoansForProcessing.ClientID%> input[id*='chkSelect']:checkbox").size();
                //Get number of checked checkboxes in list
                var checkedCheckboxes = $("#<%=gvLoansForProcessing.ClientID%> input[id*='chkSelect']:checkbox:checked").size();
                //check and uncheck the header checkbox on the basis of difference of both values
                $("#<%=gvLoansForProcessing.ClientID%> input[id*='chkSelectAll']:checkbox").attr('checked', totalCheckboxes == checkedCheckboxes);
                //Enabled/disabled "Disburse" button on selection of Checkbox
                if (checkedCheckboxes > 0) {
                    $('#btnDisburse').removeAttr("disabled");
                }
                else {
                    $('#btnDisburse').attr('disabled', 'disabled');                 
                }
                //START :ML |2019.01.28 | YRS-AT-4244 | Ajaxcall to update checkbox changes in session datatable
                CheckUncheckLoanProcessing();
                //END :ML |2019.01.28 | YRS-AT-4244 | Ajaxcall to update checkbox changes in session datatable
            });


           
            //START :ML |2019.01.28 | YRS-AT-4244 | Ajaxcall to update checkbox changes in session datatable
            function CheckUncheckLoanProcessing()
            {
                var CheckedStatusTrue = '';
                var CheckedStatusFalse = '';

                var IsSelected = '';
                //ML |2019.01.28 | YRS-AT-4244 | Code to get LoanrequestID for all selected and non selected checkbox and generate different string.
                $("#gvLoansForProcessing tr").each(function () {
                    var $checkBox = $(this).find("[id$='chkSelect']");
                    if ($checkBox.length > 0) {
                        var checkBoxId = $checkBox[0].id;
                        var tds = $(this).find('td');
                        if (tds.length >= 1) {
                            if ($checkBox.is(':checked')) {                         
                                    CheckedStatusTrue = CheckedStatusTrue + ',' + tds[1].innerText;                                
                            }
                            else {
                                
                                    CheckedStatusFalse = CheckedStatusFalse + ',' + tds[1].innerText;                                
                            }
                        }
                    }
                });

                //ML |2019.01.28 | YRS-AT-4244 | Remove special char from string 
                CheckedStatusTrue = CheckedStatusTrue.replace(/,\s*$/, "");
                CheckedStatusFalse = CheckedStatusFalse.replace(/,\s*$/, "");

                //ML |2019.01.28 | YRS-AT-4244 | Ajax call to update checkbox check /uncheck status in session datatable (pagelevel).

                $.ajax({
                    type: "POST",
                    url: "LACLoansProcessing.aspx/CheckUncheckLoanProcessing",
                    data: "{'checkedStatusFalse':'" + CheckedStatusFalse + "','checkedStatusTrue':'" + CheckedStatusTrue + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (returndata) {
                        //CloseWebProcessingDialog();
                    },
                    failure: function () {
                        //CloseWebProcessingDialog();
                        return false;
                    }
                });
                return false;
            }

            //END :ML |2019.01.28 | YRS-AT-4244 | Ajaxcall to update checkbox changes in session datatable

            $('#LoanProcessingSummaryDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                width: 450, maxHeight: 420,
                height: 410,
                title: "Summary of processed Loan(s)",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
                // For both loans width is 450
                // For EFT loans width is 320
                // For CHECK loans width is 410
            });

            $("#<%=gvLoansForProcessing.ClientID%> input[id*='chkONDRequested']:checkbox").click(function () {
                var isChecked = $(this).is(':checked');
                var completeTableRow = $(this).parent().parent().parent();   //ML|2019.01.09|YRS-AT-4244 - .parent() Added to handle unsaved changes     

                var requestID = $(completeTableRow.find("td")[1]).text();
                if (!isChecked) {
                    $(this).attr('checked', true);
                }
                else {
                    $(this).attr('checked', false);
                }               
                OpenONDConfirmationDialogBox(requestID, isChecked);
            });


            $('#<%=UncheckONDDialogBox.ClientID%>').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                width: 450, maxHeight: 420,
                height: 260,
                title: "Confirm",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            $('#<%=CheckONDDialogBox.ClientID%>').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                modal: true,
                width: 450, maxHeight: 420,
                height: 260,
                title: "Confirm",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
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

      
            //         function SetProgress(increment) {
            //            var progressbar = $("#< %=divProgressBar.ClientID%>");
            //            var val = progressbar.progressbar("value") || 0;

            //            val = val + increment;
            //            if (val > 100) {
            //                val = 100;
            //            }
            //            progressbar.progressbar("value", val);
            //        }


            function SetProgress(processed, total) {
                var progressbar = $("#<%=divProgressBar.ClientID%>");
            var val = processed * 100.00 / total; //progressbar.progressbar("value") || 0;

            progressbar.progressbar("value", val);
        }


        function ShowDialog(id, confirmationMessage) {<%--VC | 2018.11.22 | YRS-AT-4017 | Accepting new parameter 'confirmationMessage'--%>
            if ('<%=ConfirmDialog.ClientID%>' == id) {
                var data = GetLoansSelectedForProcessing();
                <%--START : VC | 2018.11.22 | YRS-AT-4017 | Commented existing code and added code to show confirmation message--%>
				<%--$('#< %=lblTotalSelectedLoan.ClientID()% >').html(data.length);--%>
                confirmationMessage = confirmationMessage.replace("$$NumberOfloans$$", data.length);
                $("#divConfirmDialogMessage").html(confirmationMessage);
                <%--END : VC | 2018.11.22 | YRS-AT-4017 | Commented existing code and added code to show confirmation message--%>
            }
            $('#' + id).dialog("open");
        }

        function CloseDialog(id) {
            $('#' + id).dialog('close');
        }

        function ValidateDisbursement() {
            <%--START : VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button--%>
            <%--var securityRight = '<%=Me.AccessPermissionRight%>';
            var isSelected = false;
            var divMasterControl = $('#<%=(Master.FindControl("DivMainMessage")).ClientID%>');

            if (securityRight == 'False') {
                divMasterControl.addClass('error-msg');
                divMasterControl.html('<%=Me.AccessPermissionMessage%>');
                return true;
            }
            else {--%>
            <%--END : VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button--%>
            var isSelected = false;
            var divMasterControl = $('#<%=(Master.FindControl("DivMainMessage")).ClientID%>');
            $("#gvLoansForProcessing input[id*='chkSelect']:checkbox").each(function (index) {
                if ($(this).is(':checked')) {
                    isSelected = true;
                    //return false;
                }
            });
            if (!(isSelected)) {
                divMasterControl.addClass('error-msg');
                divMasterControl.html('');
            }
            else {
                divMasterControl.hide();
            }








            //}<%--VC | 2018.11.06 | YRS-AT-4017 |Commented for removing access checking on Disburse button--%>
            return true;
        }

        function navigateUrl(url) {
            $('#<%=(Master.FindControl("UpdateProgress1")).ClientID%>').show();
            window.location.href = url;
        }

        function CallLetter() {
            window.open('CallReport.aspx', 'ReportPopUp1', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
        }

        function OpenPDF(strFileName) {
            //alert("hi");
            try {
                window.open(strFileName, 'OpenCustomPDF', 'width=900, height=900, resizable=yes, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no');
            }
            catch (err) {
                alert(err.message);
            }
        }

        function GetLoansSelectedForProcessing() {
            var details = null;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "LACLoansProcessing.aspx/GetListOfLoans",
                data: "",
                async: false,
                dataType: "json",
                success: function (data) {
                    details = data.d;
                },
                error: function (result) {
                    details = null;
                }
            });

            return details;
        }

        function StartLoanProcessing() {
            var count = 0;
            var runningTotal = 0;
            var increment = 0;

            var data = GetLoansSelectedForProcessing();
            count = data.length;
            increment = 100 / count;

            $('#<%=spanProgressDialogStaticTotalCount.ClientID%>').html(count);
            $('#<%=spanProgressDialogTotalCount.ClientID%>').html(count);
            $('#<%=spanProgressDialogRunningTotalCount.ClientID%>').html(runningTotal);

            for (var i = 0; i < data.length; i++) {
                runningTotal = i + 1;
                ProcessSingleLoan(data[i]);
                $('#<%=spanProgressDialogRunningTotalCount.ClientID%>').html(runningTotal);
                // START | SR | 2018.12.17 | YRS-AT-4017 | Changed to handle display summary for Loan batches. Loan progress Counter was not accurate resulting in summary dialog box not visible.
                //SetProgress(increment);
                SetProgress(runningTotal, count); 
                // END | SR | 2018.12.17 | YRS-AT-4017 | Changed to handle display summary for Loan batches. Loan progress Counter was not accurate resulting in summary dialog box not visible.
            }

            $('#<%=divCompletionDisplay.ClientID%>').html('Finished processing loans. Please wait, summary is being prepared.');
        }

        function ProcessSingleLoan(requestID) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "LACLoansProcessing.aspx/ProcessLoan",
                data: "{'requestID': '" + requestID + "'}",
                dataType: "json",
                async: false,
                success: function (data) {
                },
                error: function (result) {
                }
            });
        }

        function ShowSummary() {
            var EFTProcessed, EFTFailed, EFTTotal, CHECKProcessed, CHECKFailed, CHECKTotal, ParticipantLetter, YmcaLetter, ErrorFilePath;
            var totalProcessed, totalFailed, total;
            var url;
            var isEFTLoansExists, isCheckLoanExists;

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "LACLoansProcessing.aspx/GetSummaryReport",
                data: "",
                dataType: "json",
                async: false,
                success: function (data) {
                    $('#tblSummaryTable').show();
                    var result = data.d;

                    if (typeof result["EFTProcessed"] != 'undefined') { EFTProcessed = result["EFTProcessed"]; }
                    if (typeof result["EFTFailed"] != 'undefined') { EFTFailed = result["EFTFailed"]; }
                    if (typeof result["EFTTotal"] != 'undefined') { EFTTotal = result["EFTTotal"]; }
                    if (typeof result["CHECKProcessed"] != 'undefined') { CHECKProcessed = result["CHECKProcessed"]; }
                    if (typeof result["CHECKFailed"] != 'undefined') { CHECKFailed = result["CHECKFailed"]; }
                    if (typeof result["CHECKTotal"] != 'undefined') { CHECKTotal = result["CHECKTotal"]; }
                    if (typeof result["ParticipantLetter"] != 'undefined') { ParticipantLetter = result["ParticipantLetter"]; }
                    if (typeof result["YmcaLetter"] != 'undefined') { YmcaLetter = result["YmcaLetter"]; }
                    if (typeof result["ErrorFilePath"] != 'undefined') { ErrorFilePath = result["ErrorFilePath"]; }

                    totalProcessed = 0;
                    totalFailed = 0;
                    total = 0;
                    isEFTLoansExists = false;
                    isCheckLoanExists = false;

                    if (typeof EFTProcessed != 'undefined') {
                        $('#trSummaryEFT').show();
                        $('#divSummaryEFTProcessed').html(EFTProcessed);
                        $('#divSummaryEFTFailed').html(EFTFailed);
                        $('#divSummaryEFTTotal').html(EFTTotal);

                        totalProcessed = parseInt(totalProcessed) + parseInt(EFTProcessed);
                        totalFailed = parseInt(totalFailed) + parseInt(EFTFailed);
                        isEFTLoansExists = true;
                    }

                    if (typeof CHECKProcessed != 'undefined') {
                        $('#trSummaryCheck').show();
                        $('#divSummaryCheckProcessed').html(CHECKProcessed);
                        $('#divSummaryCheckFailed').html(CHECKFailed);
                        $('#divSummaryCheckTotal').html(CHECKTotal);

                        totalProcessed = parseInt(totalProcessed) + parseInt(CHECKProcessed);
                        totalFailed = parseInt(totalFailed) + parseInt(CHECKFailed);
                        isCheckLoanExists = true;
                    }

                    total = totalProcessed + totalFailed;
                    $('#divSummaryTotalProcessed').html(totalProcessed);
                    $('#divSummaryTotalFailed').html(totalFailed);
                    $('#divSummaryTotal').html(total);
                    $('#spanLoanProcessingSummaryTotal').html(total);

                    if (totalFailed > 0 && typeof ErrorFilePath != 'undefined') {
                        url = '<a href="#" onclick="OpenPDF(\'' + ErrorFilePath + '\')" style="color: #3399ff; text-decoration: underline;" class="Label_Small">' + totalFailed + '</a>';
                        $('#divSummaryTotalFailed').html(url);
                    }

                    if (typeof ParticipantLetter != 'undefined' || typeof YmcaLetter != 'undefined') {
                        $('#trLoanProcessingSummaryPrintLetterRow').show();
                        $('#trLoanProcessingSummaryAfterPrintLetterEmptyRow').show();
                        $('#trLoanProcessingSummaryLetterDisclaimerRow').show();

                        var isParticipantLetterExists = false;
                        var isYmcaLetterExists = false;
                        if (typeof ParticipantLetter != 'undefined') {
                            url = '<a href="#" onclick="OpenPDF(\'' + ParticipantLetter + '\')" style="color: #3399ff; text-decoration: underline;" class="Label_Small">Participant</a>';
                            $('#spanLoanProcessingSummaryPrintLetterParticipant').html(url);
                            $('#spanLoanProcessingSummaryPrintLetterParticipant').show();
                            isParticipantLetterExists = true;
                        }
                          
                        if (typeof YmcaLetter != 'undefined') {
                            url = '<a href="#" onclick="OpenPDF(\'' + YmcaLetter + '\')" style="color: #3399ff; text-decoration: underline;" class="Label_Small">LPA</a>';
                            $('#spanLoanProcessingSummaryPrintLetterYmca').html(url);
                            $('#spanLoanProcessingSummaryPrintLetterYmca').show();
                            isYmcaLetterExists = true;
                        }

                        if (isParticipantLetterExists && isYmcaLetterExists) {
                            $('#spanLoanProcessingSummaryPrintLetterLinkDivider').show();
                        }
                    }

                    // For both loans width is 450
                    // For EFT loans width is 320
                    // For CHECK loans width is 410
                    if (isEFTLoansExists && isCheckLoanExists) {
                        $("#LoanProcessingSummaryDialog").dialog("option", "height", 480);
                    }
                    else if (isEFTLoansExists) {
                        $("#LoanProcessingSummaryDialog").dialog("option", "height", 330);
                    }
                    else if (isCheckLoanExists) {
                        $("#LoanProcessingSummaryDialog").dialog("option", "height", 450);
                    }
                },
                error: function (result) {
                    $('#tblSummaryTable').hide();
                    alert("error");
                }
            });
            //CloseDialog('ProgressDialogNew');
            CloseDialog('dvWebProcessing');
            ShowDialog('LoanProcessingSummaryDialog');
            window.open('FT\\CopyFilestoFileServer.aspx?OR=1&CO=1', 'FileCopyPopUp', 'width=1,height=1, menubar=no, resizable=yes,top=1300,left=1300, scrollbars=no, status=no');
        }

        function DialogController() {
            $('#<%=tblConfirmTable.ClientID%>').hide();
            $('#<%=tblProgressDialogNew.ClientID%>').show();

            $('#<%=spanProgressDialogStaticTotalCount.ClientID%>').html('0');
            $('#<%=spanProgressDialogTotalCount.ClientID%>').html('0');
            $('#<%=spanProgressDialogRunningTotalCount.ClientID%>').html('0');

            CloseDialog('<%=ConfirmDialog.ClientID%>');
            ShowWebProcessingDialog('Please wait process is in progress...', 'Processing Loan');

            setTimeout(function () { StartLoanProcessing(); }, 2000);
            //StartLoanProcessing();
        }

        function OpenONDConfirmationDialogBox(requestID, isChecked) {
            $('#<%=hdnSelectedLoanLoanRequestID.ClientID%>').val(requestID);
            if (!isChecked)
                ShowDialog('UncheckONDDialogBox');
            else
                ShowDialog('CheckONDDialogBox');           

        }

        function ShowWebProcessingDialog(Message, divTitle) {
            $('#dvWebProcessing').dialog({ title: divTitle });
            $('#dvWebProcessing').dialog("open");
            $('#lblProcessing').text(Message);

        }


    </script>
</asp:Content>
<asp:Content ID="contentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:PlaceHolder ID="PlaceHolderSecurityCheck" runat="server"></asp:PlaceHolder>    <%--VC | 2018.11.09 | YRS-AT-4017 -  Added placeholder--%>

    <%--<asp:ScriptManagerProxy ID="dbScriptManagerProxy" runat="server"></asp:ScriptManagerProxy>--%>
    <div class="Div_Center" style="width: 100%; height: 100%;">
        <asp:UpdatePanel ID="upLoan" runat="server" UpdateMode="Conditional" EnableViewState="true" ViewStateMode="Enabled">
            <ContentTemplate>
                <table style="width: 100%; vertical-align: top; border: 0px;">
                    <tr>
                         <%--START: ML|2019.03.06|YRS-AT-4244 - Added Warn_Dirty class to handle unsaved changes. --%>
                        <td id="Td1" runat="server" style="width: 20%;" class="tabNotSelected Warn_Dirty" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=1')">Loans Pending Approval</td>
                        <td id="Td2" runat="server" style="width: 17%;" class="tabSelected Warn_Dirty" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=2')">Loan Processing</td>
                        <td id="Td3" runat="server" style="width: 17%;" class="tabNotSelected Warn_Dirty" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=3')">Exceptions/On hold</td>
                        <td id="Td4" runat="server" style="width: 15%;" class="tabNotSelected Warn_Dirty" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=4')">Search</td>
                        <td id="Td5" runat="server" style="width: 17%;" class="tabNotSelected Warn_Dirty" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=5')">Statistics</td>
                        <td id="Td6" runat="server" style="width: 14%;" class="tabNotSelected Warn_Dirty" onclick="navigateUrl('SecurityCheck.aspx?Form=LACWebLoans.aspx&TabId=6')">Rate</td>
                         <%--END: ML|2019.03.06|YRS-AT-4244 - Added Warn_Dirty class to handle unsaved changes. --%>
                    </tr>
                </table>
                <table class="Table_WithBorder" style="width: 100%; height: 500px;" border="0">
                    <%-- Filter--%>
                    <tr>
                        <td style="vertical-align: top; height: 15%;" class="Table_WithBorder">
                            <table border="0" style="width: 100%;">
                                <tr>
                                    <td class="td_Text" style="width: 100%;">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td id="headerSectionFilter" class="td_Text" style="text-align: left; width: 100%;">Filter</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; height: 40%;" colspan="2">
                                        <div id="divFilter">
                                            <table border="0" style="width: 100%;">
                                                <tr>
                                                    <td style="width: 10%;">
                                                        <asp:Label ID="lblFundId" runat="server" CssClass="Label_Small">Fund No.:</asp:Label></td>
                                                    <td style="width: 16%;">
                                                        <asp:TextBox ID="txtFundNo" runat="server" CssClass="TextBox_Normal" Width="100px" MaxLength="20" onkeypress="ValidateNumeric();"></asp:TextBox></td>
                                                    <td style="width: 10%;" class="Label_Small">
                                                        <asp:CheckBox ID="chkCHECK" runat="server" CssClass="CheckBox_Normal" Text="CHECK" AutoPostBack="false" /></td>
                                                    <td style="width: 44%;" class="Label_Small">
                                                        <asp:CheckBox ID="chkEFT" runat="server" CssClass="CheckBox_Normal" Text="EFT" AutoPostBack="false" /></td>
                                                    <td style="width: 20%; text-align: left;">
                                                        <asp:Button ID="btnFind" runat="server" CssClass="Button_Normal" Text="Find"></asp:Button>
                                                        &nbsp;
                                                        <asp:Button ID="btnClear" runat="server" CssClass="Button_Normal" Text="Clear"></asp:Button>
                                                    </td>
                                                </tr> 
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%-- Pending Loans --%>
                    <tr>
                        <td class="Table_WithBorder" style="height: 85%;">
                            <table style="height: 100%; width: 100%">
                                <tr>
                                    <td style="width: 100%; height: 90%;">
                                        <div style="overflow: auto; width: 100%; height: 100%; border-top-style: none; border-right-style: none; border-left-style: none; position: static; border-bottom-style: none">
                                            <asp:GridView ID="gvLoansForProcessing" AllowSorting="true" runat="server" CssClass="DataGrid_Grid" Width="100%" AutoGenerateColumns="false" EmptyDataText="No records found.">
                                                <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                                <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                                <HeaderStyle CssClass="fixedHeader DataGrid_HeaderStyle" /><%--VC | 2018.11.26 | YRS-AT-4017 |Added class 'fixedHeader' to fix grid header while scrolling the grid--%>
                                                <RowStyle CssClass="DataGrid_NormalStyle" />
                                                <SortedAscendingHeaderStyle CssClass="sortasc" />
                                                <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("Select")%>' />   <%--ML|2019.01.28|YRS-AT-4244 - Bind checkbox To maintain checkbox value after page postback   --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="LoanRequestId" HeaderText="LoanRequestId" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="LoanRequestId" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="PersId" HeaderText="PersId" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%" SortExpression="PersId" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="FundEventId" HeaderText="FundEventId" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" SortExpression="FundEventId" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="FundNo" HeaderText="Fund No." ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="8%" SortExpression="FundNo" />
                                                    <asp:BoundField DataField="Name" HeaderText="Participant Name" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="16%" SortExpression="Name" />
                                                    <asp:BoundField DataField="YMCAName" HeaderText="YMCA Name" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20%" SortExpression="YMCAName" />
                                                    <asp:BoundField DataField="SavingsBalance" HeaderText="Savings Balance" ReadOnly="True" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%" SortExpression="SavingsBalance" />
                                                    <asp:BoundField DataField="LoanStatus" HeaderText="Status" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%" SortExpression="LoanStatus" />
                                                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" ItemStyle-Width="8%" SortExpression="RequestDate" /><%--VC | 2018.11.14 | YRS-AT-4017 | Added time format with date format--%>
                                                    <asp:BoundField DataField="PaymentMethod" HeaderText="Payment Method" ReadOnly="True" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="7%" SortExpression="PaymentMethod" />
                                                    <asp:BoundField DataField="RequestedAmount" HeaderText="Requested Amt." ReadOnly="True" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="9%" SortExpression="RequestedAmount" />
                                                    <asp:TemplateField HeaderText="OND Requested" HeaderStyle-Width="7%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center" SortExpression="IsONDRequested"><%--VC | 2018.11.06 | YRS-AT-4017 |Added sort expression--%>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkONDRequested" runat="server" CssClass="Warn" Checked='<%# Eval("IsONDRequested")%>' Enabled='<%# IIf(Eval("PaymentMethod").ToString().ToUpper() = "EFT", "false", "true")%>'  />
                                                            <%--ML|2019.01.09|YRS-AT-4244 - Added Warn class to handle unsaved changes. --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Link to Loan" HeaderStyle-Width="9%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgProcessing" runat="server" CssClass="Warn_Dirty" ImageUrl="images/process.gif" alt="Go to Loan Processing" Style="cursor: pointer;" CommandName="loanrequestandprocessing" CommandArgument='<%#Bind("FundNo")%>' />
                                                             <%--ML|2019.01.09|YRS-AT-4244 - Added Warn_Dirty class to handle unsaved changes. --%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="chvLoanNumber" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="chrSSNo" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="FirstName" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="MiddleName" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="LastName" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="YmcaID" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="YmcaNo" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                    <asp:BoundField DataField="ONDAmount" HeaderStyle-CssClass="ClassHide" ItemStyle-CssClass="ClassHide" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; width: 100%; height: 10%;">
                                        <label class="Label_Small">No of record(s) : </label>
                                        <span id="lblRecords" class="Label_Small" runat="server"></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 100%;" class="td_Text">
                            <asp:Button ID="btnPrintList" runat="server" Text="Print List" class="Button_Normal" />&nbsp;
                            <asp:Button ID="btnSaveOND" runat="server" Text="Save OND" class="Button_Normal" Enabled="true" />&nbsp;   <%--ML|2019.01.09|YRS-AT-4244 - New Introduce SAVE OND button to update OND status --%>
                            <asp:Button ID="btnDisburse" runat="server" Text="Disburse" class="Button_Normal" Enabled="true" OnClientClick="javascript: return ValidateDisbursement();" />&nbsp;
                            <asp:Button ID="btnClose" runat="server" Text="Close" class="Button_Normal Warn_Dirty" />
                            <%--ML|2019.01.09|YRS-AT-4244 - warn_Dirty Added handle unsaved changes.--%>
                        </td>
                    </tr>
                </table>

                <div id="ConfirmDialog" title="Disburse" style="display: none;" runat="server">
                    <div>
                        <table width="100%" height="250px" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;" id="tblConfirmTable" runat="server">
                            <tr>
                                <td rowspan="2" style="width: 10%;">
                                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="imgConfirmDialogInfo" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divConfirmDialogMessage">
                                        <%--START : VC | 2018.11.22 | YRS-AT-4017 | Commented code to remove hardcoded message--%>
                                        <%--Are you sure, you want to disburse selected
                                        <asp:Label ID="lblTotalSelectedLoan" runat="server" Text="0" Visible="false"></asp:Label>
                                        loan(s)?--%>
                                        <%--END : VC | 2018.11.22 | YRS-AT-4017 | Commented code to remove hardcoded message--%>
                                    </div>
                                    <%--START : VC | 2018.11.22 | YRS-AT-4017 | Created new div to show last business day processing warning message--%>
                                    <div id="divConfirmBusinessDayProcessing" runat="server" style="color: red; padding-top: 10px;">
                                    </div>
                                    <%--END : VC | 2018.11.22 | YRS-AT-4017 | Created new div to show last business day processing warning message--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr id="trConfirmDialogYesNo">
                                <td align="right" valign="bottom" colspan="2">
                                    <asp:Button ID="btnConfirmDialogYes" runat="server" Text="Yes" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" OnClientClick="javascript: DialogController(); return false;" />&nbsp;
                                    <%--<input type="button" id="btnConfirmDialogYes" value="Yes" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="DialogController(); " />--%>
                                    <%--onclick="CloseDialog('ConfirmDialog'); ShowDialog('ProgressDialogNew'); StartLoanProcessing();" />--%>
                                    <input type="button" id="btnConfirmDialogNo" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="CloseDialog('ConfirmDialog');" />
                                </td>
                            </tr>
                        </table>

                        <table class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left; width: 100%; display: none;" border="0" id="tblProgressDialogNew" runat="server">
                            <tbody>
                                <tr>
                                    <td style="text-align: center">
                                        <div>Total loans selected for processing: <span id="spanProgressDialogStaticTotalCount" runat="server"></span></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <div id="divCompletionDisplay" runat="server">Completed: <span id="spanProgressDialogRunningTotalCount" runat="server"></span>/ <span id="spanProgressDialogTotalCount" runat="server"></span></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="divProgressBar" runat="server"></div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div id="ProgressDialogNew" style="display: none;">
                    <div>
                    </div>
                </div>

                <div id="LoanProcessingSummaryDialog" style="display: none;">
                    <div>
                        <table class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left; width: 100%" border="0" cellspacing="0" cellpadding="0">
                            <tbody>
                                <tr>
                                    <td style="text-align: center">
                                        <div>Total loans selected for processing: <span id="spanLoanProcessingSummaryTotal" runat="server">10</span></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder SummaryTable" style="width: 100%; display: none;" border="1" cellspacing="0" cellpadding="0" id="tblSummaryTable">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 25%;">Payment Method</td>
                                                    <td style="width: 25%">Processed</td>
                                                    <td style="width: 25%">Failed</td>
                                                    <td style="width: 25%">Total</td>
                                                </tr>
                                                <tr id="trSummaryEFT" style="display: none;">
                                                    <td>EFT</td>
                                                    <td style="text-align: center;">
                                                        <div id="divSummaryEFTProcessed">5</div>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <div id="divSummaryEFTFailed">1</div>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <div id="divSummaryEFTTotal">6</div>
                                                    </td>
                                                </tr>
                                                <tr id="trSummaryCheck" style="display: none;">
                                                    <td>CHECK</td>
                                                    <td style="text-align: center;">
                                                        <div id="divSummaryCheckProcessed">2</div>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <div id="divSummaryCheckFailed">2</div>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <div id="divSummaryCheckTotal">4</div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Total</td>
                                                    <td style="text-align: center;">
                                                        <div id="divSummaryTotalProcessed">7</div>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <div id="divSummaryTotalFailed"><a id="aError" href="#" onclick="PUCOpenPDF('x')" style="color: #3399ff; text-decoration: underline;" class="Label_Small">3</a></div>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <div id="divSummaryTotal">10</div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr id="trLoanProcessingSummaryPrintLetterRow" style="display: none;">
                                    <td>To print letters click on link: <span id="spanLoanProcessingSummaryPrintLetterParticipant" style="display: none;"><a href="#" onclick="Sample('x')" style="color: #3399ff; text-decoration: underline;" class="Label_Small">Participant</a></span><span id="spanLoanProcessingSummaryPrintLetterLinkDivider" style="display: none;"> | </span><span id="spanLoanProcessingSummaryPrintLetterYmca" style="display: none;"><a href="#" onclick="Sample('x')" style="color: #3399ff; text-decoration: underline;" class="Label_Small">LPA</a></span>
                                    </td>
                                </tr>
                                <tr id="trLoanProcessingSummaryAfterPrintLetterEmptyRow" style="display: none;">
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr id="trLoanProcessingSummaryLetterDisclaimerRow" style="display: none;">
                                    <td><%--Letters for Participant and LPA will be generated for Loans with payment mode as CHECK.--%>
                                        <%--Letters for Participant(s) and LPA(s) will be generated for loans with payment method as CHECK. For payment method EFT participant(s) will receive an email.--%>
                                        Letters will be generated for Participant(s) only in case payment method is CHECK.
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <%--<input onclick="CloseDialog('LoanProcessingSummaryDialog');" id="Button1" class="Button_Normal" style="FONT-SIZE: 8pt; HEIGHT: 16pt; FONT-FAMILY: Verdana, Tahoma, Arial; WIDTH: 80px; FONT-WEIGHT: bold; COLOR: black" type="button" value="Close">&nbsp;--%>
                                        <asp:Button ID="btnCloseSummary" runat="server" OnClientClick="CloseDialog('LoanProcessingSummaryDialog');" CssClass="Button_Normal" Style="FONT-SIZE: 8pt; HEIGHT: 16pt; FONT-FAMILY: Verdana, Tahoma, Arial; WIDTH: 80px; FONT-WEIGHT: bold; COLOR: black" OnClick="btnCloseSummary_Click" Text="Close" />&nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div id="UncheckONDDialogBox" title="Confirm" style="display: none;" runat="server">
                    <div>
                        <table width="100%" height="250px" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            <tr>
                                <td rowspan="2" style="width: 10%;">
                                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="img1" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        Participant has chosen OND while applying for loan. Do you want to cancel the OND?
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr id="tr1">
                                <td align="right" valign="bottom" colspan="2">
                                    <asp:Button ID="btnUncheckONDDialogBoxYes" Text="Yes" runat="server" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" OnClick="btnUncheckONDDialogBoxYes_Click" />
                                   
                                    <input type="button" id="btnUncheckONDDialogBoxNo" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="CloseDialog('UncheckONDDialogBox');" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div id="CheckONDDialogBox" title="Confirm" style="display: none;" runat="server">
                    <div>
                        <table width="100%" height="250px" border="0" class="formlayout formlayout-bg margin-5px-bottom Table_WithoutBorder" style="text-align: left;">
                            <tr>
                                <td rowspan="2" style="width: 10%;">
                                    <img src="images/help48.JPG" style="border-width: 0px; display: block;" alt="information" id="img2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        Participant has not chosen OND while applying for loan. Do you want to select OND? 
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr id="tr2">
                                <td align="right" valign="bottom" colspan="2">
                                    <asp:Button ID="btnCheckONDDialogBoxYes" Text="Yes" runat="server" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" OnClick="btnCheckONDDialogBoxYes_Click" />
                                    <input type="button" id="btnCheckONDDialogBoxNo" value="No" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;"
                                        onclick="CloseDialog('CheckONDDialogBox');" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:HiddenField ID="hdnSelectedLoanLoanRequestID" runat="server" Value="0" />
                <div id="dvWebProcessing">
                    <label id="lblProcessing" class="Label_Small"></label>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnFind" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btnPrintList" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btnDisburse" EventName="click" />
                
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        //START:ML|2019.01.09| YRS-AT-4244-  Added to handle unsaved changes.
        function EnableDirty() {
            $('#HiddenFieldDirty').val('true');
        }

        function ClearDirty() {
            $('#HiddenFieldDirty').val('false');
        }
        //END:ML|2019.01.09| YRS-AT-4244-   Added to handle unsaved changes.
    </script>
</asp:Content>
