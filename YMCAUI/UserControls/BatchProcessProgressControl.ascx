<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BatchProcessProgressControl.ascx.vb" Inherits="YMCAUI.BatchProcessProgressControl" %>
<script type="text/javascript" src="../JS/jquery-1.5.1.min.js"></script>
<script src="../JS/jquery-ui/jquery-ui-1.8.13.custom.min.js" type="text/javascript"></script>
<link href="../JS/jquery-ui/base/jquery.ui.all.css" type="text/css" media="all" rel="stylesheet" />

<asp:UpdatePanel runat="server" ID="updBatchProcessProgressControl" UpdateMode="Conditional" EnableViewState="true"
    ViewStateMode="Enabled">
    <ContentTemplate>
        <div id="divConfirmDialog" title="RMD Print Letter" runat="server" style="overflow: visible; display: none;">
            <div>
                <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                    <tr>
                        <td>
                            <asp:Label ID="lblConfirmDialogMessage" CssClass="Label_Small" runat="server"></asp:Label>
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
                            <asp:Button runat="server" ID="btnYes" Text="Yes" CssClass="Button_Normal" Style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt; display:none;"
                                OnClientClick="javascript: CloseConfirmDialog();" />
                            <input type="button" name="BatchProcessProgressControl_btnYes" value="Yes" onclick="javascript: CloseConfirmDialog(); CallProcess();" id="Button1" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />&nbsp;
                            <input type="button" name="BatchProcessProgressControl_btnNo" value="No" onclick="javascript: CloseConfirmDialog();" id="BatchProcessProgressControl_btnNo" class="Button_Normal" style="width: 80px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div id="divRMDPrintLetter" runat="server" style="display: none;">
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
                            <asp:Label ID="lblException" runat="server" class="Label_Small" Text="One or more errors were encounterd while processing this batch. Please click here ">
                                <asp:LinkButton ID="lnkShowError" runat="server" ForeColor="#3399ff" OnClientClick="ExceptionShowError(); return false;">view details</asp:LinkButton></asp:Label>
                        </div>
                    </td>
                </tr>

                <tr id="trshowPrintTitle" class="hide">
                    <td>
                        <asp:Label ID="Label1" runat="server" class="Label_Small" Text=" To print the batch letter(s) please click on below link"></asp:Label>

                    </td>
                </tr>
                <tr id="trshowPrintCtls" class="hide">
                    <td>
                        <span id="spanPUCLinkHolder"></span>
                        <input type="hidden" name="hdnProgress" id="hdnProgress" value="">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnClose" OnClientClick="ClosePrintDialog();" Text="Close" CssClass="Button_Normal" Style="width: 60px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />
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
        <asp:AsyncPostBackTrigger ControlID="btnYes" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

<script type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        function EndRequest(sender, args) {
            if (args.get_error() == undefined) {
                BindEventsRMDUserControls();
            }
        }

        BindEventsRMDUserControls();
    });

    function BindEventsRMDUserControls() {
        $('#<%=divConfirmDialog.ClientID%>').dialog({
            autoOpen: false,
            draggable: true,
            close: false,
            width: 350, height: 350,
            title: "<%=DialogTitle%>",
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
                $('a.ui-dialog-titlebar-close').remove();
            }
        });

        $('#<%=divRMDPrintLetter.ClientID%>').dialog({
            autoOpen: false,
            draggable: true,
            show: "fade",
            <%--hide: "fade",--%> <%--MMR | 2017.05.15 | YRS-AT-3356 | Remove progress animation on click of close button in progress window--%>
            modal: true,
            close: false,
            title: "<%=DialogTitle%> - Creation Status",
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
    }

    function OpenConfirmDialog(message) {
        $('#<%=lblConfirmDialogMessage.ClientID%>').html(message);
        $('#<%=divConfirmDialog.ClientID%>').dialog("open");
    }

    function CloseConfirmDialog() {
        $('#<%=divConfirmDialog.ClientID%>').dialog('close');
    }

    function OpenProgressPopup() {
        $('#<%=divRMDPrintLetter.ClientID%>').dialog("open");
    }

    function ClosePrintDialog() {
        $('#<%=divRMDPrintLetter.ClientID%>').dialog('close');
        SetPUCDefaultState();
    }

    function CallProcess() {
        var batchID, moduleName;
        batchID = GetBatchID();
        moduleName = GetModuleName();
        var allowReprint = GetReprintStatus(); <%-- MMR | 2017.05.04 | YRS-AT-3205 | Get reprint status--%>
        OpenProgressPopup();
        if (batchID != '' && moduleName != '') {
            if (CreateBatch(batchID, moduleName)) {
                CallPrintLetterProcess(batchID, 0, "", 0, 0, moduleName, allowReprint); <%-- MMR | 2017.05.04 | YRS-AT-3205 | Passing reprint status to method which will be used for reprinting of letters --%>
            }
            else {
                $('#<%=dvMessage.ClientID%>').text("Some network error occured.");
                $("#<%=dvMessage.ClientID%>")[0].className = "error-msg";
                $("#trshowErrors")[0].className = 'show';
                MarkFirstThreeStagesStatus("Error");
                MarkFirstThreeStagesClass("error-Progress");
                $('#<%=dvCopy.ClientID%>')[0].className = "info-Progress";
            }
        }
    }

    function CreateBatch(batchID, moduleName) {
        var result = false;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "UserControls/BatchProcessProgressUCWebMethods.aspx/CreateRMDBatch",
            data: "{'batchID': '" + batchID + "','moduleName':'" + moduleName + "'}",
            async: false,
            dataType: "json",
            success: function (data) {
                result = true;
            },
            error: function (result) {
                $('#<%=dvMessage.ClientID%>').text("Some network error occured.");
                $("#<%=dvMessage.ClientID%>")[0].className = "error-msg";
                $("#trshowErrors")[0].className = 'show';
                CallArrErrorDataList(batchID, moduleName);
                UpdateDisplay(data);
                MarkFirstThreeStagesStatus("Error");
                MarkFirstThreeStagesClass("error-Progress");
                $('#<%=dvCopy.ClientID%>')[0].className = "info-Progress";

                $("#trshowPrintCtls")[0].className = "hide";
                $("#trshowPrintTitle")[0].className = "hide";
            }
        });

        return result;
    }

    function GetBatchID() {
        var batchID = '';
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "UserControls/BatchProcessProgressUCWebMethods.aspx/GetRMDBatchID",
            async: false,
            dataType: "json",
            success: function (data) {
                batchID = data.d;
            }
        });
        return batchID;
    }

    function GetModuleName() {
        var moduleName = '';
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "UserControls/BatchProcessProgressUCWebMethods.aspx/GetRMDModuleName",
            async: false,
            dataType: "json",
            success: function (data) {
                moduleName = data.d;
            }
        });
        return moduleName;
    }
    <%--START : MMR | 2017.05.04 | YRS-AT-3205 | Function will get reprint status based on which reprinting of letter will be decided--%>
    function GetReprintStatus() {
        var reprintStatus = '';
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "UserControls/BatchProcessProgressUCWebMethods.aspx/GetReprintStatus",
            async: false,
            dataType: "json",
            success: function (data) {
                reprintStatus = data.d;
            }
        });
        return reprintStatus;
    }
    <%--END : MMR | 2017.05.04 | YRS-AT-3205 | Function will get reprint status based on which reprinting of letter will be decided--%>

    function CallPrintLetterProcess(batchID, count, processName, idxCreated, pdfCreated, moduleName, allowReprint) { <%-- MMR | 2017.05.04 | YRS-AT-3205 | Parameter passed for reprinting to web method--%>
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "UserControls/BatchProcessProgressUCWebMethods.aspx/ProcessRMDLetters",
            data: "{'batchID': '" + batchID + "','count':" + count + ",'processName':'" + processName + "'," +
                "'idxCreated':" + idxCreated + ",'pdfCreated':" + pdfCreated + ",'moduleName':'" + moduleName + "','allowReprint':'" + allowReprint + "'}", <%-- MMR | 2017.05.04 | YRS-AT-3205 | Parameter passed for reprinting to web method--%>
            dataType: "json",
            success: function (data) {
                if (data.d.strretValue == "success") {
                    RMDBatchProcessCounter = data;
                    <%-- START: MMR | 2017.05.04 | YRS-AT-3205 | If reprint status is true, below function will be called which will perform operation required for reprinting--%>
                    if (data.d.AllowReprint)
                    {
                            MakeCopiesOfLetterForReprinting(batchID);
                    }
                    <%-- END: MMR | 2017.05.04 | YRS-AT-3205 | If reprint status is true, below function will be called which will perform operation required for reprinting--%>
                    CopyFilestoFileServer(1, 1, batchID, moduleName, moduleName);

                    MarkFirstThreeStagesStatus("Completed");
                    MarkFirstThreeStagesClass("success-Progress");
               
                    $('#hdnProgress').attr("value", "1");
                }
                else if (data.d.strretValue == "error") {
                    $('#<%=dvMessage.ClientID%>').text("Some error occured.");
                    $("#<%=dvMessage.ClientID%>")[0].className = "error-msg";
                    $("#trshowErrors")[0].className = 'show';
                    CallArrErrorDataList(batchID, moduleName);
                    UpdateDisplay(data);

                    MarkFirstThreeStagesStatus("Error");
                    MarkFirstThreeStagesClass("error-Progress");
                    $('#<%=dvCopy.ClientID%>')[0].className = "info-Progress";

                    $("#trshowPrintCtls")[0].className = "hide";
                    $("#trshowPrintTitle")[0].className = "hide";
                }
                else if (data.d.strretValue == "pending") {
                    <%--Display Links: At this stage only last step is pending and rest 1-3 steps are complete, so display links.--%>
                    if (data.d.hasOwnProperty('Links')) {
                        if (data.d.Links != undefined) {
                            var linkMatter = '';
                            for (var i = 0; i < data.d.Links.length; i++) {
                                if (linkMatter == '')
                                    linkMatter = '<a href="#" onClick="PUCOpenPDF(' + "'" + data.d.Links[i].URL + "'" + ')" style="color:#3399ff; text-decoration: underline;" class="Label_Small">' + data.d.Links[i].DisplayText + '</a>';
                                else
                                    linkMatter = linkMatter + '&nbsp;|&nbsp;' + '<a href="#" onClick="PUCOpenPDF(' + "'" + data.d.Links[i].URL + "'" + ')" style="color:#3399ff; text-decoration: underline;" class="Label_Small">' + data.d.Links[i].DisplayText + '</a>';
                            }
                            $('#spanPUCLinkHolder').html(linkMatter);
                        }
                    }

                    CallPrintLetterProcess(data.d.strBatchId, data.d.iProcessCount, data.d.strProcessName, data.d.iIdxCreated, data.d.iPdfCreated, moduleName, data.d.AllowReprint); <%-- MMR | 2017.05.04 | YRS-AT-3205 | Passing reprint status to method which will be used for reprinting of letters --%>
                    UpdateDisplay(data);

                    MarkFirstThreeStagesClass("info-Progress");
                    $('#<%=dvCopy.ClientID%>')[0].className = "info-Progress";
                 
                    $("#trshowPrintCtls")[0].className = "hide";
                    $("#trshowPrintTitle")[0].className = "hide";
                }
                else {
                    $("#<%=dvMessage.ClientID%>")[0].className = "error-msg";
                    $("#trshowErrors")[0].className = 'show';
                    CallArrErrorDataList(batchID, moduleName);
                    UpdateDisplay(data);
                    MarkFirstThreeStagesStatus("Error");
                    MarkFirstThreeStagesClass("error-Progress");
                    $('#<%=dvCopy.ClientID%>')[0].className = "info-Progress";
                 
                    $("#trshowPrintCtls")[0].className = "hide";
                    $("#trshowPrintTitle")[0].className = "hide";
                }
            },
            error: function (result) {
                $('#<%=dvMessage.ClientID%>').text("Some network error occured.");
                $("#<%=dvMessage.ClientID%>")[0].className = "error-msg";
                $("#trshowErrors")[0].className = 'show';
                CallArrErrorDataList(batchID, moduleName);
                UpdateDisplay(data);
                MarkFirstThreeStagesStatus("Error");
                MarkFirstThreeStagesClass("error-Progress");
                $('#<%=dvCopy.ClientID%>')[0].className = "info-Progress";

                $("#trshowPrintCtls")[0].className = "hide";
                $("#trshowPrintTitle")[0].className = "hide";
            }
        });
    }

    var RMDBatchProcessCounter;
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
               
                if ($('#hdnProgress').val() == "1") {
                    $('#hdnProgress').attr("value", "0");
                    $("#trshowPrintCtls")[0].className = "show";
                    $("#trshowPrintTitle")[0].className = "show";
                }

                if (data.d.strStatus == 'error') {
                    $('#<%=dvStatusMsg.ClientID%>').text("Some IDX or PDF files were not copied to IDM server.");
                    CallArrErrorDataList(strBatchId, strModule)
                    $("#trshowErrors")[0].className = "show";
                    UpdateDisplay(RMDBatchProcessCounter);
                    $('#<%=dvCopy.ClientID%>').text("Error");
                    $('#<%=dvCopy.ClientID%>')[0].className = "error-Progress";
                }
                else {
                    UpdateDisplay(RMDBatchProcessCounter);

                    if (RMDBatchProcessCounter.d.iTotalIDXPDFCount < RMDBatchProcessCounter.d.iTotalCount) {
                        $('#<%=dvStatusMsg.ClientID%>').text("Some of the records were not proccessed due to problem, please click on view details link for details.");
                        CallArrErrorDataList(strBatchId, strModule)
                        $("#trshowErrors")[0].className = "show";
                        UpdateDisplay(RMDBatchProcessCounter);
                        $('#<%=dvCopy.ClientID%>').text("Error");
                        $('#<%=dvCopy.ClientID%>')[0].className = "error-Progress";
                    }
                    else if (RMDBatchProcessCounter.d.iPdfCreated < RMDBatchProcessCounter.d.iTotalIDXPDFCount || RMDBatchProcessCounter.d.iIdxCreated < RMDBatchProcessCounter.d.iTotalIDXPDFCount) {
                        $('#<%=dvStatusMsg.ClientID%>').text("Some IDX or PDF files were not created.");
                        CallArrErrorDataList(strBatchId, strModule)
                        $("#trshowErrors")[0].className = "show";
                        UpdateDisplay(RMDBatchProcessCounter);
                        $('#<%=dvCopy.ClientID%>').text("Error");
                        $('#<%=dvCopy.ClientID%>')[0].className = "error-Progress";
                    }
                    else if (RMDBatchProcessCounter.d.iPdfCopied == RMDBatchProcessCounter.d.iTotalIDXPDFCount && RMDBatchProcessCounter.d.iIdxCopied == RMDBatchProcessCounter.d.iTotalIDXPDFCount) {
                        $('#<%=dvStatusMsg.ClientID%>').text("RMD letter process completed sucessfully.");
                        MarkAllStagesCompleted();
                    }
                    else if (RMDBatchProcessCounter.d.iTotalIDXPDFCount == RMDBatchProcessCounter.d.iTotalCount) {
                        $('#<%=dvStatusMsg.ClientID%>').text("RMD letter process completed sucessfully.");
                        MarkAllStagesCompleted();
                    }
                    else {
                        $('#<%=dvStatusMsg.ClientID%>').text("Internal exception.");
                        CallArrErrorDataList(strBatchId, strModule)
                        $("#trshowErrors")[0].className = "show";
                        UpdateDisplay(RMDBatchProcessCounter);
                        $('#<%=dvCopy.ClientID%>').text("Error");
                        $('#<%=dvCopy.ClientID%>')[0].className = "error-Progress";
                    }

                    $('#<%=imgCopiedComplete.ClientID%>').css("display", "none");
                    $('#<%=imgPDFComplete.ClientID%>').css("display", "none");
                    $('#<%=imgIDXComplete.ClientID%>').css("display", "none");
                    $('#<%=imgRegComplete.ClientID%>').css("display", "none");
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
        $('#<%=dvShowStatus.ClientID%>').html(strStatus);
        $('#<%=dvReqProgress.ClientID%>').text(data.d.iTotalIDXPDFCount + " out of " + data.d.iTotalCount + ".");
        $('#<%=dvIDXProgress.ClientID%>').text(data.d.iIdxCreated + " out of " + data.d.iTotalCount + ".");
        $('#<%=dvPDFProgress.ClientID%>').text(data.d.iPdfCreated + " out of " + data.d.iTotalCount + ".");
        $('#<%=dvCopiedProgress.ClientID%>').text(data.d.iPdfCopied + " out of " + data.d.iTotalCount + ".");
    }

    function ExceptionShowError() {
        $('#dvShowErrorsMsg').dialog("open");
    }

    function MarkFirstThreeStagesStatus(status) {
        $('#<%=dvPDF.ClientID%>').text(status);
        $('#<%=dvIDX.ClientID%>').text(status);
        $('#<%=dvReg.ClientID%>').text(status);
    }

    function MarkFirstThreeStagesClass(className) {
        $('#<%=dvPDF.ClientID%>')[0].className = className;
        $('#<%=dvIDX.ClientID%>')[0].className = className;
        $('#<%=dvReg.ClientID%>')[0].className = className;
    }

    function MarkAllStagesCompleted() {
        $('#<%=dvCopy.ClientID%>').text("Completed");
        $('#<%=dvPDF.ClientID%>').text("Completed");
        $('#<%=dvIDX.ClientID%>').text("Completed");
        $('#<%=dvReg.ClientID%>').text("Completed");

        $('#<%=dvPDF.ClientID%>')[0].className = "success-Progress";
        $('#<%=dvIDX.ClientID%>')[0].className = "success-Progress";
        $('#<%=dvReg.ClientID%>')[0].className = "success-Progress";
        $('#<%=dvCopy.ClientID%>')[0].className = "success-Progress";
    }

    function SetPUCDefaultState() {
        $('#<%=dvPDF.ClientID%>').text("In-Progress");
        $('#<%=dvIDX.ClientID%>').text("In-Progress");
        $('#<%=dvReg.ClientID%>').text("In-Progress");
        $('#<%=dvCopy.ClientID%>').text("Pending");

        $('#<%=dvPDF.ClientID%>')[0].className = "info-Progress";
        $('#<%=dvIDX.ClientID%>')[0].className = "info-Progress";
        $('#<%=dvReg.ClientID%>')[0].className = "info-Progress";
        $('#<%=dvCopy.ClientID%>')[0].className = "info-Progress";

        $('#<%=dvReqProgress.ClientID%>').text("In - Process");
        $('#<%=dvIDXProgress.ClientID%>').text("In - Process");
        $('#<%=dvPDFProgress.ClientID%>').text("In - Process");
        $('#<%=dvCopiedProgress.ClientID%>').text("Pending");

        $("#trshowPrintCtls")[0].className = "hide";
        $("#trshowPrintTitle")[0].className = "hide";
    }

    function PUCOpenPDF(url) {
        try {
            window.open(url, 'OpenCustomPDF', 'width=1024, height=768, resizable=yes, top=0, left=0, scrollbars=yes ,directories=no ,toolbar=no ,menubar=no, location=no');
        }
        catch (err) {
            alert(err.message);
        }
    }
    <%--START: MMR | 2017.05.04  | YRS-AT-3205 | Function will call web method to perform operation to copy letters for reprinting --%>
    function MakeCopiesOfLetterForReprinting(batchID) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "UserControls/BatchProcessProgressUCWebMethods.aspx/MakeCopiesOfLetterForReprinting",
            data: "{'batchID':'" + batchID + "'}",
            dataType: "json",
            success: function (data) {
            },
            error: function (result) {
            }
        });
    }
    <%--END: MMR | 2017.05.04  | YRS-AT-3205 | Function will call web method to perform operation to copy letters for reprinting --%>
</script>
