<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SpecialCashout.aspx.vb" Inherits="YMCAUI.SpecialCashout"
    MasterPageFile="~/MasterPages/YRSMain.Master" EnableEventValidation="false" %>

<%@ Register TagPrefix="DataPagerFindInfo" TagName="DataGridPager" Src="UserControls/DataGridPager.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <title>Special Cashout</title>
    <style  type="text/css">
         .hide {
            display: none;
        }

        .show {
            display: block;
        }
    </style>
    <script type="text/javascript" language="javascript">

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
            $('#dvConfirmDialog').dialog({
                autoOpen: false,
                draggable: true,
                close: false,
                width: 350, height: 300,
                title: "Special Cashout Batch Request Creation",
                modal: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                },
                buttons: [{ text: "Yes", click: StartCashoutProcess }, { text: "No", click: closeConfirmdialog }]
            });
        }

        function OpenProgressPopup() {
            $('#modalCashOut').dialog({
                autoOpen: false,
                draggable: true,
                show: "fade",
                hide: "fade",
                modal: true,
                close: false,
                title: "Special CashOut - Batch Creation Status",
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                    $('a.ui-dialog-titlebar-close').remove();
                },
                width: 900,
                height: 700,
                display: "block",
                closeOnEscape: false
            });
            // buttons: [{ text: "Close", click: closeConfirmdialog }]
            $('#modalCashOut').dialog("open");

        }


        function ClosePrintDialog() {
            $('#modalCashOut').dialog('close');
        }

        function StartCashoutProcess() {
            closeConfirmdialog();
            var strSplitData;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SpecialCashout.aspx/StartCashoutProcess",
                data: "{}",
                dataType: "json",
                success: function (data) {
                    strSplitData = data.d.split(',');
                    CallProcess(strSplitData[0], strSplitData[1], strSplitData[2], strSplitData[3]);
                },
                error: function (result) {
                    alert('Records not selected for batch creation.');
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
        var CashOutBatchProcessCounter
        function CopyFilestoFileServer(strOR, strDEL, strBatchId, strModule) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "FT/CopyFilestoFileServer.aspx/CopytoFileServer",
                data: "{'strOR':'" + strOR + "','strDEL':'" + strDEL + "','strBatchId':'" + strBatchId + "','strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    //$('#dvDsiplay').html(data.d);
                    CashOutBatchProcessCounter.d.iPdfCopied = data.d.iIdxCopied;
                    CashOutBatchProcessCounter.d.iIdxCopied = data.d.iPdfCopied;
                    if (data.d.strStatus == 'error') {
                        //$('#dvDsiplay').html(data.d.strError);
                        $('#dvStatusMsg').text("Some IDX or PDF files were not copied to IDM server.");
                        CallArrErrorDataList(strBatchId, strModule)
                        $("#trshowErrors")[0].className = "show";
                        UpdateDisplay(CashOutBatchProcessCounter);
                        $('#dvCopy').text("Error");
                        $('#dvCopy')[0].className = "error-Progress";
                    }
                    else {
                        UpdateDisplay(CashOutBatchProcessCounter);

                        if (CashOutBatchProcessCounter.d.iTotalIDXPDFCount < CashOutBatchProcessCounter.d.iTotalCount) {
                            //$('#dvStatusMsg').text("Some of the records were not proccessed due to problem, please click on View Errors link for more information.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(CashOutBatchProcessCounter);
                            $('#dvCopy').text("Error");
                            $('#dvCopy')[0].className = "error-Progress";
                        }
                        else if (CashOutBatchProcessCounter.d.iPdfCreated < CashOutBatchProcessCounter.d.iTotalIDXPDFCount || CashOutBatchProcessCounter.d.iIdxCreated < CashOutBatchProcessCounter.d.iTotalIDXPDFCount) {
                            //$('#dvStatusMsg').text("Some IDX or PDF files were not get created.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(CashOutBatchProcessCounter);
                            $('#dvCopy').text("Error");
                            $('#dvCopy')[0].className = "error-Progress";
                        }
                        else if (CashOutBatchProcessCounter.d.iPdfCopied == CashOutBatchProcessCounter.d.iTotalIDXPDFCount && CashOutBatchProcessCounter.d.iIdxCopied == CashOutBatchProcessCounter.d.iTotalIDXPDFCount) {
                            $('#dvStatusMsg').text("CashOut Batch Request Creation Process completed sucessfully.");

                            $('#dvCopy').text("Completed");
                            $('#dvPDF').text("Completed");
                            $('#dvIDX').text("Completed");
                            $('#dvReg').text("Completed");

                            $('#dvPDF')[0].className = "success-Progress";
                            $('#dvIDX')[0].className = "success-Progress";
                            $('#dvReg')[0].className = "success-Progress";
                            $('#dvCopy')[0].className = "success-Progress";

                        }
                        else if (CashOutBatchProcessCounter.d.iTotalIDXPDFCount == CashOutBatchProcessCounter.d.iTotalCount) {
                            $('#dvStatusMsg').text("Special CashOut Batch Request Creation Process completed sucessfully.");

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
                            //$('#dvStatusMsg').text("Internal exception.");
                            CallArrErrorDataList(strBatchId, strModule)
                            $("#trshowErrors")[0].className = "show";
                            UpdateDisplay(CashOutBatchProcessCounter);
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
                    UpdateDisplay(CashOutBatchProcessCounter);
                    $("#trshowErrors")[0].className = 'show';
                    CallArrErrorDataList(strBatchId, strModule);
                }
            });
        }
        function CallProcess(strbatchId, strReportType, strModule, strCashOutType) {
            OpenProgressPopup();
            CallCashoutProcess(strbatchId, strReportType, strCashOutType, 0, "", 0, 0, strModule);
        }

        function openReportPrinter() {
            window.open('FT\\ReportPrinter.aspx', '', 'width=1024,height=768, menubar=no, resizable=yes,top=200,left=150, scrollbars=yes, status=yes');
        }

        function CallCashoutProcess(strbatchId, strReportType, strCashOutType, iCount, strProcessName, iIDXCreated, iPDFCreated, strModule) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "SpecialCashout.aspx/CashOutBatchCreationProcess",
                data: "{'strBatchId':'" + strbatchId + "','strReportType':'" + strReportType + "','strCashOutType':'" + strCashOutType + "','iCount':" + iCount + ",'strProcessName':'" + strProcessName + "','iIDXCreated':" + iIDXCreated + ",'iPDFCreated':" + iPDFCreated + ",'strModule':'" + strModule + "'}",
                dataType: "json",
                success: function (data) {
                    if (data.d.strretValue == "success") {
                        CopyFilestoFileServer(1, 1, strbatchId, strModule);
                        CashOutBatchProcessCounter = data;
                        $('#dvPDF').text("Completed");
                        $('#dvIDX').text("Completed");
                        $('#dvReg').text("Completed");

                        $('#dvPDF')[0].className = "success-Progress";
                        $('#dvIDX')[0].className = "success-Progress";
                        $('#dvReg')[0].className = "success-Progress";

                    }
                    else if (data.d.strretValue == "error") {
                        UpdateDisplay(data);
                        $('#dvMessage').text("Some error occured.");
                        $("#dvMessage")[0].className = "error-msg";
                        $("#trshowErrors")[0].className = 'show';
                        CallArrErrorDataList(strbatchId, strModule);
                        $('#dvPDF').text("Error");
                        $('#dvIDX').text("Error");
                        $('#dvReg').text("Error");

                        $('#dvPDF')[0].className = "error-Progress";
                        $('#dvIDX')[0].className = "error-Progress";
                        $('#dvReg')[0].className = "error-Progress";
                        $('#dvCopy')[0].className = "info-Progress";

                    }
                    else if (data.d.strretValue == "pending") {
                        CallCashoutProcess(data.d.strBatchId, data.d.strReportType, strCashOutType, data.d.iProcessCount, data.d.strProcessName, data.d.iIdxCreated, data.d.iPdfCreated, strModule);
                        UpdateDisplay(data);
                        $('#dvPDF')[0].className = "info-Progress";
                        $('#dvIDX')[0].className = "info-Progress";
                        $('#dvReg')[0].className = "info-Progress";
                        $('#dvCopy')[0].className = "info-Progress";

                    }
                    else {
                        UpdateDisplay(data);
                        $('#dvMessage').text("Some error occured.");
                        $("#dvMessage")[0].className = "error-msg";
                        $("#trshowErrors")[0].className = 'show';
                        CallArrErrorDataList(strbatchId, strModule);
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
                    UpdateDisplay(result);
                    $('#dvMessage').text("Some network error occured.");
                    $("#dvMessage")[0].className = "error-msg";
                    CallArrErrorDataList(strbatchId, strModule);
                    $("#trshowErrors")[0].className = 'show';

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

        var iPreviousValue = 0;
        function UpdateDisplay(data) {
            var strStatus = ''
            if (data.d == undefined) {
                return;
            }
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

        function showConfirmdialog() {
            $('#dvConfirmDialog').dialog("open");
        }

        function closeConfirmdialog() {
            $('#dvConfirmDialog').dialog('close');
        }

        function openReportViewer() {
            window.open('FT\\ReportViewer.aspx', '', 'width=785,height=300, menubar=no, resizable=yes,top=200,left=150, scrollbars=yes, status=yes');
        }

    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="div_center">
        <asp:ScriptManagerProxy ID="SpecialCashoutScriptManager" runat="server">
        </asp:ScriptManagerProxy>
        <asp:UpdatePanel ID="uplGenerateCashout" runat="server">
            <ContentTemplate>
                <table style="width: 100%; height: 350px" class="Table_WithBorder" valign="top">
                    <tr>
                        <td style="vertical-align: top; width: 15%; text-align: left" class="Table_WithBorder" id="tdCashoutBatchId" runat="server">
                            <div style="overflow: auto; width: 100%; border-top-style: none; border-right-style: none; border-left-style: none; height: 420px; border-bottom-style: none">
                                <table class="Table_WithBorder" id="Table4" cellspacing="1" cellpadding="1" border="0" style="width: 100%">
                                    <tr class="DataGrid_AlternateStyle">
                                        <td>
                                            <b>View Reports</b>
                                        </td>
                                    </tr>
                                    <asp:Repeater ID="rptCashoutBatchId" runat="server">
                                        <ItemTemplate>
                                            <tr valign="top">
                                                <td id="liCashoutBatchId" runat="server" style="padding-bottom: 10px; list-style-type: none; text-align: left; width: 100%;">
                                                    <asp:LinkButton runat="server" ID="lnkCashoutBatchId" Text='<%# Eval("chvBatchId")%>' OnClick="lnkCashoutBatchId_Click" CssClass="Link_SmallBold" CommandName="CashoutBatchId" CommandArgument='<%# Eval("chvBatchId")%>'></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </td>
                        <td valign="top">
                            <table style="width: 100%; text-align: center;" class="Table_WithoutBorder">
                                <tr>
                                    <td class="Label_Small" width="15%">Upload File: 
                                    </td>
                                    <td width="20%">
                                        <asp:FileUpload ID="FileUpld" runat="server" />
                                    </td>
                                    <td width="30%" style="text-align: left">
                                        <asp:Button runat="server" ID="btnUpload" Text="Upload File" CssClass="Button_Normal" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblCount" CssClass="Label_Small"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table class="Table_WithoutBorder" cellpadding="0" cellspacing="0" width="100%" align="center">
                                            <tr style="vertical-align: top;">
                                                <td class="tabSelectedLink" style="width: 100%; height: 3px; text-align: left;">
                                                    <asp:Label ID="lblDescription" runat="server" Text="List of Uploaded Participants."></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" height="350px" valign="top">
                                        <asp:GridView ID="gvCashoutList" runat="server" CssClass="DataGrid_Grid" AutoGenerateColumns="false"
                                            AllowSorting="true" Width="100%"
                                            PageSize="20" EmptyDataText="No records found.">
                                            <SelectedRowStyle CssClass="DataGrid_SelectedStyle" />
                                            <RowStyle CssClass="DataGrid_NormalStyle" />
                                            <HeaderStyle CssClass="DataGrid_HeaderStyle" />
                                            <Columns>
                                                <asp:BoundField DataField="PersonId" SortExpression="PersonId" HeaderText="PersonId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="SSNO" SortExpression="SSNO" HeaderText="SSNO" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="FUNDNo" SortExpression="FUNDNo" HeaderText="Fund Id" HeaderStyle-Width="10%" />
                                                <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="LastName" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="MiddleName" SortExpression="MiddleName" HeaderText="MiddleName" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="FirstName" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                                                <asp:BoundField DataField="dtmVestingDate" SortExpression="dtmVestingDate" HeaderText="dtmVestingDate" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="PersonAgeDOB" SortExpression="PersonAgeDOB" HeaderText="PersonAgeDOB" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="PersonAgeDOD" SortExpression="PersonAgeDOD" HeaderText="PersonAgeDOD" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="MaxTermDate" SortExpression="MaxTermDate" HeaderText="Latest Termn." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="FundEventId" SortExpression="FundEventId" HeaderText="FundEventId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="IsTerminated" SortExpression="IsTerminated" HeaderText="IsTerminated" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="IsVested" SortExpression="IsVested" HeaderText="Vested" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px" Visible="false" />
                                                <asp:BoundField DataField="PersonAge" SortExpression="PersonAge" HeaderText="Person Age" ItemStyle-Width="60px" />
                                                <asp:BoundField DataField="IntAddressId" SortExpression="IntAddressId" HeaderText="IntAddressId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="EligibleBalance" SortExpression="EligibleBalance" HeaderText="Plan Amt." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px" />
                                                <asp:BoundField DataField="TaxableAmount" SortExpression="TaxableAmount" HeaderText="TaxableAmount" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="PlansType" SortExpression="PlansType" HeaderText="Plan Type" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="StatusType" SortExpression="StatusType" HeaderText="Fund Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="BatchId" SortExpression="BatchId" HeaderText="BatchId" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="Selected" SortExpression="Selected" HeaderText="Selected" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="LastContributionDate" SortExpression="LastContributionDate" HeaderText="Latest Rcvd. Cont." HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="IsHighlighted" SortExpression="IsHighlighted" HeaderText="IsHighlighted" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="RefRequestID" SortExpression="RefRequestID" HeaderText="RefRequestID" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="IsRMDEligible" SortExpression="IsRMDEligible" HeaderText="IsRMDEligible" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="chvShortDescription" SortExpression="chvShortDescription" HeaderText="chvShortDescription" ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide" />
                                                <asp:BoundField DataField="Remarks" SortExpression="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                                                <asp:BoundField DataField="mnyEstimatedBalance" SortExpression="mnyEstimatedBalance" HeaderText="Est. Bal." ControlStyle-CssClass="hide" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" FooterStyle-CssClass="hide"/>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Td_ButtonContainer" colspan="4">
                                        <asp:Button runat="server" ID="btnRequest" Text="Start Process" CssClass="Button_Normal" />
                                        <asp:Button runat="server" ID="btnReset" Text="Reset" CssClass="Button_Normal" />
                                    </td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
                <asp:PostBackTrigger ControlID="btnReset" />
            </Triggers>
        </asp:UpdatePanel>
        <div id="dvConfirmDialog">
            <table width="100%" border="0" class="formlayout formlayout-bg margin-5px-bottom">
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmMessage" CssClass="Label_Small" runat="server" Text="Are you sure you want to create a batch for listed person(s)"></asp:Label>
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

    <div id="modalCashOut" style="display: none;">
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
                            <td class="DataGrid_AlternateStyle" width="55%" valign="middle" style="vertical-align: middle; background-color: White">1. Request Creation
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
            <tr>
                <td>
                    <div id="dvStatusMsg" runat="server" class="Label_Small"></div>
                </td>
            </tr>
            <tr id="trshowErrors" class="hide">
                <td>
                    <div>
                        <asp:Label Text="One or more errors were encountered while creating this batch request. Please click here to " runat="server" ID="as" class="Label_Small">
                            <asp:LinkButton ID="lnkShowError" runat="server" ForeColor="#3399ff" OnClientClick="ExceptionShowError(); return false;">View details</asp:LinkButton></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnClose" Text="Close" CssClass="Button_Normal" Style="width: 60px; color: Black; font-family: Verdana, Tahoma, Arial; font-size: 8pt; font-weight: bold; height: 16pt;" />
                </td>
            </tr>
        </table>
    </div>
    <div style="overflow: auto; width: 100%" id="dvShowErrorsMsg">
        <div id="divPopErrorMsg" style="width: 100%">
        </div>
    </div>
</asp:Content>
