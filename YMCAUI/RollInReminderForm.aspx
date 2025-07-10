<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPages/YRSMain.Master" CodeBehind="RollInReminderForm.aspx.vb" Inherits="YMCAUI.RollInReminderForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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

    <script language="javascript" type="text/javascript">
        var CloseDisable = false;
        function ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector) {
            var totalCheckboxes = $(checkBoxSelector),
                        checkedCheckboxes = totalCheckboxes.filter(":checked"),
                        noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
                        allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);
            if (totalCheckboxes.length == 0) allCheckboxesAreChecked = false;
            $(allCheckBoxSelector).attr('checked', allCheckboxesAreChecked);
        }

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
            function EndRequest(sender, args) {
                if (args.get_error() == undefined) {
                    BindEvents();
                }
            }
            BindEvents();
        });

        function BindEvents() {
            var checkBoxSelector = '#<%=gvRolloverRoll.ClientID%> input[id*="chkSelect"]:checkbox';
            var allCheckBoxSelector = '#<%=gvRolloverRoll.ClientID%> input[id*="chkSelectAll"]:checkbox';

            $(allCheckBoxSelector).bind('click', function () {
                $(checkBoxSelector).attr('checked', $(this).is(':checked'));
                ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);
            });
            ToggleCheckUncheckAllOptionAsNeeded(allCheckBoxSelector, checkBoxSelector);

            $('#dvRollHist').dialog({
                autoOpen: false,
                resizable: false,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 550, height: 150,
                title: "Follow Up History",
                modal: true,
                buttons: [{ text: "Close", click: CloseFollowUpHistDialog }],
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });

            //OpenProcessScreen();

            $('#divConfirmDialog').dialog({
                autoOpen: false,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                close: false,
                modal: true,
                width: 350, height: 250,
                title: "Roll In Print Letter(s)",
                buttons: [{ text: "Yes", click: PrintRollinLetters }, { text: "No", click: CloseConfirmDialog }],
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                }
            });
        }

        function ShowFollowUpHistDialog(strName, strInstName, strPAcno) {
            $("#lblName").text(strName);
            $("#lblInstName").text(strInstName);
            $("#lblPAccno").text(strPAcno);
            $("#dvRollHist").dialog('open');
        }
        function CloseFollowUpHistDialog() {
            $("#dvRollHist").dialog('close');
        }

        function OpenProcessScreen() {
            $('#divFollowUpRollIn').dialog({
                autoOpen: false,
                draggable: true,
                resizable: false,
                show: "fade",
                hide: "fade",
                modal: true,
                close: false,
                title: "Roll In Follow-up Letters - Creation status",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                },
                width: 900,
                height: 700,
                closeOnEscape: false,
                beforeclose: function () {
                    return CloseDisable;
                },
                buttons: [{ text: "Close", click: ClosePrintDialog }]
            });
            $('#divFollowUpRollIn').dialog("open");
        }

        function CallProcess(strbatchId, strModule) {
            OpenProcessScreen();
            CallPrintLetterProcess(strbatchId, 0, "", 0, 0, strModule);
        }

        function CallPrintLetterProcess(strbatchId, iCount, strProcessName, iIDXCreated, iPDFCreated, strModule) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "RollInReminderForm.aspx/RollInProcess",
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
                    }
                    else if (data.d.strretValue == "pending") {
                        CallPrintLetterProcess(data.d.strBatchId, data.d.iProcessCount, data.d.strProcessName, data.d.iIdxCreated, data.d.iPdfCreated, strModule);
                        UpdateDisplay(data);

                        $('#dvPDF')[0].className = "info-Progress";
                        $('#dvIDX')[0].className = "info-Progress";
                        $('#dvReg')[0].className = "info-Progress";
                        $('#dvCopy')[0].className = "info-Progress";

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
                            $('#dvStatusMsg').text("RollIn letters process completed sucessfully.");
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
                            $('#dvStatusMsg').text("RollIn letters process completed sucessfully.");
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
        }


        function ShowPanel(strBatchId, strModuleType) {
            //OpenProcessScreen();
            //$('#divFollowUpRollIn').dialog("open");
            //$('#frmRMDPrtLtr').attr("visibility", "visible");
            //$('#frmRMDPrtLtr').attr("height", "300");
            //$('#frmRMDPrtLtr').attr("width", "750");
            //$('#frmRMDPrtLtr').attr("src", "BatchRequestCreation.aspx?Form=ROLLIN&strBatchId=" + strBatchId + "&strModuleType=" + strModuleType);
            CallProcess(strBatchId, strModuleType);
        }
        //End: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.
        function ClosePrintDialog() {
            $("#divFollowUpRollIn").dialog('close');
            document.forms(0).submit();
        }

        function ShowConfirmDialog(str) {
            $('#divConfirmDialog').dialog('open');
            $('#lblMessage').text(str);
        }

        function CloseConfirmDialog() {
            $.ajax({
                type: "POST",
                url: "RollInReminderForm.aspx/ClearRollinPrintSession",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $('#divConfirmDialog').dialog('close');
                }
            });
        }

        function PrintRollinLetters() {
            $.ajax({
                type: "POST",
                url: "RollInReminderForm.aspx/PrintRollinLetters",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CloseConfirmDialog();
                    var strData = String();
                    var str;
                    var strBatchID;
                    var strModule;
                    strData = msg.d;
                    str = strData.split('$$$');
                    strBatchID = str[0];
                    strModule = str[1];
                    ShowPanel(strBatchID, strModule);
                }
            });
        }

        function CallLetter() {
            window.open('CallReport.aspx', 'ReportPopUp1', 'width=1024,height=768, menubar=no, resizable=yes,top=0,left=0, scrollbars=yes, status=yes');
        }


        function OpenPDF(strFileName) {
            try {
                window.open(strFileName, 'OpenCustomPDF', 'width=900, height=900, resizable=yes, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no');
            }
            catch (err) {
                alert(err.message);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="Div_Center" style="width: 100%; height: 450px;">
        <asp:ScriptManagerProxy ID="DBScriptManager" runat="server">
        </asp:ScriptManagerProxy>
        <asp:UpdatePanel ID="uplRollinReminder" runat="server">
            <ContentTemplate>
                <table class="Table_WithBorder" style="width: 100%; text-align: left;">
                    <tr style="vertical-align: top;">
                        <td style="width: 100%; text-align: left;">

                            <asp:Label ID="lblheadtext" CssClass="Label_Small" runat="server"></asp:Label>
                        </td>
                    </tr>

                </table>
                <table class="Table_withBorder" style="width: 100%;">
                    <tr style="height: 400px; vertical-align: top;">

                        <td style="width: 100%; text-align: left;">
                            <div style="height: 400px; text-align: left; overflow: scroll">
                                <asp:GridView ID="gvRolloverRoll" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="Label_Small" EmptyDataText="No Roll In(s) exists."
                                    AllowSorting="true" DataKeyNames="RolloverID">
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <SortedAscendingHeaderStyle CssClass="sortasc" />
                                    <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="20px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" Checked="true" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked="true" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="20px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PersID" SortExpression="PersID" HeaderText="PersID" />
                                        <asp:BoundField DataField="FundNo" SortExpression="FundNo" HeaderText="Fund No." />
                                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" />
                                        <asp:BoundField DataField="InstitutionName" SortExpression="InstitutionName" HeaderText="Institution Name" />
                                        <asp:BoundField DataField="PartAccno" SortExpression="PartAccno" HeaderText="Participant Account Number" />
                                        <asp:BoundField DataField="DocRcvdDate" SortExpression="DocRcvdDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Doc. Rcvd. Date" />
                                        <asp:BoundField DataField="InstitutionId" SortExpression="InstitutionId" HeaderText="InstitutionId" />
                                        <asp:BoundField DataField="RequestDate" SortExpression="RequestDate" HeaderText="Request Date" DataFormatString="{0:MM/dd/yyyy}" />
                                        <asp:TemplateField HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkHist" Text="History" runat="server" CommandName="Select"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="Td_ButtonContainer" style="width: 100%; text-align: right">
                            <table cellspacing="0" style="height: 23px">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnPrintList" runat="server" Text="Print List"
                                            CssClass="Button_Normal" Style="width: 100px;" />&nbsp;&nbsp;
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnPrint" runat="server" Text="Print"
                                            CssClass="Button_Normal" Style="width: 100px;" />&nbsp;&nbsp;
                                    </td>

                                    <td align="center">
                                        <asp:Button ID="btnClose" runat="server" UseSubmitBehavior="false" Text="Close" CssClass="Button_Normal"
                                            Style="width: 80px;" />
                                        <input id="HiddenSecControlName" type="hidden" name="HiddenSecControlName" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="dvRollHist">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="overflow: auto; height: 150px; width: 98%">
                    <table style="width: 100%">
                        <tr>
                            <td class="Label_Small">Name: &nbsp;&nbsp;&nbsp;&nbsp;
                                <label class="Label_Small" id="lblName"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label_Small">Institution Name: &nbsp;&nbsp;&nbsp;&nbsp;
                                <label class="Label_Small" id="lblInstName"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label_Small">Participant Acct No. : &nbsp;&nbsp;&nbsp;&nbsp;
                                <label class="Label_Small" id="lblPAccno"></label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:GridView ID="gvRollFollowupHist" runat="server" AutoGenerateColumns="false" Width="40%" CssClass="Label_Small"
                                    AllowSorting="true" DataKeyNames="RefId">
                                    <AlternatingRowStyle CssClass="DataGrid_AlternateStyle" />
                                    <RowStyle CssClass="DataGrid_NormalStyle" />
                                    <SortedAscendingHeaderStyle CssClass="sortasc" />
                                    <SortedDescendingHeaderStyle CssClass="sortdesc" Font-Bold="true" />
                                    <HeaderStyle CssClass="DataGrid_HeaderStyle"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundField DataField="LettersCode" HeaderText="Letter" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created On" DataFormatString="{0:MM/dd/yyyy}" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <%-- <div id="divFollowUpRollIn" style="display: none;">
        <table style="width: 95%; height: auto">
            <tr>
                <td class="Label_Small">
                    Please do not close this window until all activities are complete. 
                </td>
            </tr>
            <tr>
                <td>
                    <iframe id="frmRMDPrtLtr" visible="false" frameborder="0"></iframe>
                </td>
            </tr>            
        </table>
    </div>--%>
     <%--Start: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.--%>
    <asp:UpdatePanel runat="server" ID="updatepnl">
        <ContentTemplate>
            <div id="divFollowUpRollIn" style="display: none;">
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
                                    <td class="DataGrid_AlternateStyle" width="55%" valign="middle" style="vertical-align: middle; background-color: White">1. Generating RollIn Followup Letter Entries
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
                    <%--<tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnClosePop" Text="Close" CssClass="Button_Normal" Style="width: 60px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />
                        </td>
                    </tr>--%>
                </table>
            </div>

            <div style="display: none; overflow: auto" id="dvShowErrorsMsg">
                <div id="divPopErrorMsg">
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPrint" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="btnClosePop" EventName="Click" />--%>
        </Triggers>
    </asp:UpdatePanel>
     <%--End: Dinesh.k           25/08/2014      BT:2630: Initial & Follow-up Letters:Storing the batch details in the newly created log table for handling application pool re-cycling.--%>

    <div id="divConfirmDialog" title="Cancel rollin" style="overflow: visible;">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                    <tr>
                        <td>
                            <label id="lblMessage" class="Label_Small"></label>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img title="image" height="50" alt="image" src="images/spacer.gif" width="10" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
